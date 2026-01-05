using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_attendance")]
    [DataContract]
    public class EmployeeAttendance : ModelBase, IDisposable
    {
        #region Fields

        Int64 idEmployeeAttendance;
        Int32 idEmployee;
        Int32 idCompanyWork;
        Int32 idCompanyShift;
        DateTime startDate;
        TimeSpan? startTime;
        DateTime endDate;
        TimeSpan? endTime;
        Employee employee;
        CompanyWork companyWork;
        CompanyShift companyShift;
        string employeeCode;
        TimeSpan? totalTime;
        Int32 weekNumber;
        string clockID;
        Company companyContract;
        DateTime? accountingDate;
        string currentContractForAttendance;
        byte isManual;
        Int32 creator;
        DateTime? creationDate;
        Int32? modifier;
        DateTime? modificationDate;
        //Rupali Sarode
        TimeSpan? dailyHours;
        TimeSpan? workedHours;
        LookupValue status;
        //

        string fileName;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        string remark;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        bool isNotRegularShift;//[Sudhir.Jangra][GEOS2-5275]
        LookupValue extraHours;//[Sudhir.Jangra][GEOS2-5275]
        bool isAttendanceChecked;//[Sudhir.Jangra][GEOS2-5275]
        bool isNotWorkingDay;
        int idStatus;
        string htmlcolor;
        string location;
        bool isLocationAvailable;
        double? latitude;
        double? langitude;
        LookupValue attendanceStatus;
        int registerNumber;
        int totalRegistersInTheDay;
        TimeSpan registerTime;
        TimeSpan totalDayTime;
        bool isMobileApiAttendance;
        List<EmployeeChangelog> changeLogList; //[shweta.thube][GEOS2-5511]
        #endregion

        #region Properties
        [DataMember]
        public LookupValue AttendanceStatus
        {
            get { return attendanceStatus; }
            set
            {
                attendanceStatus = value;
                OnPropertyChanged("AttendanceStatus");
            }
        }
        [Key]
        [Column("IdEmployeeAttendance")]
        [DataMember]
        public Int64 IdEmployeeAttendance
        {
            get { return idEmployeeAttendance; }
            set
            {
                idEmployeeAttendance = value;
                OnPropertyChanged("IdEmployeeAttendance");
            }
        }
        [Column("Latitude")]
        [DataMember]
        public double?  Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }
        [Column("Langitude")]
        [DataMember]
        public double? Langitude
        {
            get { return langitude; }
            set
            {
                langitude = value;
                OnPropertyChanged("Langitude");
            }
        }
        public bool IsLocationAvailable
        {
            get { return isLocationAvailable; }
            set
            {
                isLocationAvailable = value;
                OnPropertyChanged("IsLocationAvailable");
            }
        }

        [Column("Status")]
        [DataMember]
        public LookupValue Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [Column("Location")]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }
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
        [Column("IdStatus")]
        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }
        [NotMapped]
        [DataMember]
        public string HTMLColor
        {
            get { return htmlcolor; }
            set
            {
                htmlcolor = value;
                OnPropertyChanged("HTMLColor");
            }
        }
        [Column("IdCompanyWork")]
        [DataMember]
        public Int32 IdCompanyWork
        {
            get { return idCompanyWork; }
            set
            {
                idCompanyWork = value;
                OnPropertyChanged("IdCompanyWork");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }


        [Column("EndDate")]
        [DataMember]
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;

                OnPropertyChanged("EndDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyWork CompanyWork
        {
            get { return companyWork; }
            set
            {
                companyWork = value;
                OnPropertyChanged("CompanyWork");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdCompanyShift
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
        public CompanyShift CompanyShift
        {
            get { return companyShift; }
            set
            {
                companyShift = value;
                OnPropertyChanged("CompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }


        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public int WeekNumber
        {
            get { return weekNumber; }
            set
            {
                weekNumber = value;
                OnPropertyChanged("WeekNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string ClockID
        {
            get { return clockID; }
            set
            {
                clockID = value;
                OnPropertyChanged("ClockID");
            }
        }


        [NotMapped]
        [DataMember]
        public Company CompanyContract
        {
            get { return companyContract; }
            set
            {
                companyContract = value;
                OnPropertyChanged("CompanyContract");
            }
        }


        [Column("AccountingDate")]
        [DataMember]
        public DateTime? AccountingDate
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
        public string CurrentContractForAttendance
        {
            get { return currentContractForAttendance; }
            set
            {
                currentContractForAttendance = value;

                OnPropertyChanged("CurrentContractForAttendance");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsManual
        {
            get { return isManual; }
            set
            {
                isManual = value;
                OnPropertyChanged("IsManual");
            }
        }


        [Column("Creator")]
        [DataMember]
        public Int32 Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }


        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }


        [Column("Modifier")]
        [DataMember]
        public Int32? Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }


        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }
        //Rupali Sarode
        [NotMapped]
        [DataMember]
        public TimeSpan? WorkedHours
        {
            get { return workedHours; }
            set
            {
                workedHours = value;
                OnPropertyChanged("WorkedHours");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? DailyHours
        {
            get { return dailyHours; }
            set
            {
                dailyHours = value;
                OnPropertyChanged("DailyHours");
            }
        }

        [Column("Remark")]
        [DataMember]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }
        [Column("FileName")]
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        //
        //[Sudhir.Jangra][GEOS2-5275]
        [NotMapped]
        [DataMember]
        public bool IsNotRegularShift
        {
            get { return isNotRegularShift; }
            set
            {
                isNotRegularShift = value;
                OnPropertyChanged("IsNotRegularShift");
            }
        }

        //[Sudhir.Jangra][GEOS2-5275]
        [NotMapped]
        [DataMember]
        public LookupValue ExtraHours
        {
            get { return extraHours; }
            set
            {
                extraHours = value;
                OnPropertyChanged("ExtraHours");
            }
        }

        //[Sudhir.Jangra][GEOS2-5275]
        [NotMapped]
        [DataMember]
        public bool IsAttendanceChecked
        {
            get { return isAttendanceChecked; }
            set
            {
                isAttendanceChecked = value;
                OnPropertyChanged("IsAttendanceChecked");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsNotWorkingDay
        {
            get { return isNotWorkingDay; }
            set
            {
                isNotWorkingDay = value;
                OnPropertyChanged("IsNotWorkingDay");
            }
        }
     
        [DataMember]
        public TimeSpan TotalDayTime
        {
            get { return totalDayTime; }
            set
            {
                totalDayTime = value;
                OnPropertyChanged("TotalDayTime");
            }
        }

        [DataMember]
        public TimeSpan RegisterTime
        {
            get { return registerTime; }
            set
            {
                registerTime = value;
                OnPropertyChanged("RegisterTime");
            }
        }

        [DataMember]
        public int RegisterNumber
        {
            get { return registerNumber; }
            set
            {
                registerNumber = value;
                OnPropertyChanged("RegisterNumber");
            }
        }

        [DataMember]
        public int TotalRegistersInTheDay
        {
            get { return totalRegistersInTheDay; }
            set
            {
                totalRegistersInTheDay = value;
                OnPropertyChanged("TotalRegistersInTheDay");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsMobileApiAttendance
        {
            get { return isMobileApiAttendance; }
            set
            {
                isMobileApiAttendance = value;
                OnPropertyChanged("IsMobileApiAttendance");
            }
        }

        Int32? idCreator;
        [NotMapped]
        [DataMember]
        public Int32? IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }
        string creatorName;
        [NotMapped]
        [DataMember]
        public string CreatorName
        {
            get { return creatorName; }
            set
            {
                creatorName = value;
                OnPropertyChanged("CreatorName");
            }
        }

        string creatorCode;
        [NotMapped]
        [DataMember]
        public string CreatorCode
        {
            get { return creatorCode; }
            set
            {
                creatorCode = value;
                OnPropertyChanged("CreatorCode");
            }
        }

        string modifierName;
        [NotMapped]
        [DataMember]
        public string ModifierName
        {
            get { return modifierName; }
            set
            {
                modifierName = value;
                OnPropertyChanged("ModifierName");
            }
        }
        string modifierCode;
        [NotMapped]
        [DataMember]
        public string ModifierCode
        {
            get { return modifierCode; }
            set
            {
                modifierCode = value;
                OnPropertyChanged("ModifierCode");
            }
        }

        Int32 idReporterSource;
        [NotMapped]
        [DataMember]
        public Int32 IdReporterSource
        {
            get { return idReporterSource; }
            set
            {
                idReporterSource = value;
                OnPropertyChanged("IdReporterSource");
            }
        }
        //[shweta.thube][GEOS2-5511]
        [NotMapped]
        [DataMember]
        public List<EmployeeChangelog> ChangeLogList
        {
            get
            {
                return changeLogList;
            }

            set
            {
                changeLogList = value;
                OnPropertyChanged("ChangeLogList");
            }
        }
        #endregion

        #region Constructor

        public EmployeeAttendance()
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
            // return this.MemberwiseClone();
            //[GEOS2-5275][09.02.2024][rdixit]
            EmployeeAttendance employeeAttendance = (EmployeeAttendance)this.MemberwiseClone();

            if (CompanyWork != null)
                employeeAttendance.CompanyWork = (CompanyWork)this.CompanyWork.Clone();

            return employeeAttendance;
        }

        #endregion
    }
}
