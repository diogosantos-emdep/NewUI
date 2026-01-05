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
    [Table("company_leaves")]
    [DataContract]
    public class EmployeeLeave : ModelBase, IDisposable
    {
        #region Fields


        UInt64 idEmployeeLeave;
        Int32 idEmployee;
        Int32 idLeave;
        DateTime? startDate;
        DateTime? endDate;

        TimeSpan? startTime;
        TimeSpan? endTime;

        string fileName;
        string remark;

        CompanyLeave companyLeave;
        Employee employee;
        sbyte isAllDayEvent;

        //byte[] fileInBytes;
        List<EmployeeChangelog> employeeChangelogs;
        Int32 weekNumber;
        string leaveDate;
        DateTime leaveDateTime;
        Int32 idCompanyShift;
        CompanyShift companyShift;
        DateTime? startLeaveDateForExcel;
        DateTime? endLeaveDateForExcel;

        TimeSpan? startLeaveTimeForExcel;
        TimeSpan? endLeaveTimeForExcel;
        Int32 idEmpContractCompany;
         Int32 creator;
         DateTime? modificationDate;
         DateTime? creationDate;
         Int32? modifier;
        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeLeave")]
        [DataMember]
        public UInt64 IdEmployeeLeave
        {
            get { return idEmployeeLeave; }
            set
            {
                idEmployeeLeave = value;
                OnPropertyChanged("IdEmployeeLeave");
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

        [Column("IdLeave")]
        [DataMember]
        public int IdLeave
        {
            get { return idLeave; }
            set
            {
                idLeave = value;
                OnPropertyChanged("IdLeave");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
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
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
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

        [DataMember]
        [NotMapped]
        public sbyte IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged("IsAllDayEvent");
            }
        }

        [DataMember]
        [NotMapped]
        public CompanyLeave CompanyLeave
        {
            get { return companyLeave; }
            set
            {
                companyLeave = value;
                OnPropertyChanged("CompanyLeave");
            }
        }

        [DataMember]
        [NotMapped]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [DataMember]
        [NotMapped]
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [DataMember]
        [NotMapped]
        public TimeSpan? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [DataMember]
        [NotMapped]
        public List<EmployeeChangelog> EmployeeChangelogs
        {
            get { return employeeChangelogs; }
            set
            {
                employeeChangelogs = value;
                OnPropertyChanged("EmployeeChangelogs");
            }
        }

        [DataMember]
        [NotMapped]
        public Int32 WeekNumber
        {
            get { return weekNumber; }
            set
            {
                weekNumber = value;
                OnPropertyChanged("WeekNumber");
            }
        }

        [DataMember]
        [NotMapped]
        public string LeaveDate
        {
            get { return leaveDate; }
            set
            {
                leaveDate = value;
                OnPropertyChanged("LeaveDate");
            }
        }


        [DataMember]
        [NotMapped]
        public Int32 IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }

        [DataMember]
        [NotMapped]
        public CompanyShift CompanyShift
        {
            get { return companyShift; }
            set
            {
                companyShift = value;
                OnPropertyChanged("CompanyShift");
            }
        }

        public DateTime LeaveDateTime
        {
            get { return leaveDateTime; }
            set
            {
                leaveDateTime = value;
                OnPropertyChanged("LeaveDateTime");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? StartLeaveDateForExcel
        {
            get { return startLeaveDateForExcel; }
            set
            {
                startLeaveDateForExcel = value;
                OnPropertyChanged("StartLeaveDateForExcel");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? EndLeaveDateForExcel
        {
            get { return endLeaveDateForExcel; }
            set
            {
                endLeaveDateForExcel = value;
                OnPropertyChanged("EndLeaveDateForExcel");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? StartLeaveTimeForExcel
        {
            get { return startLeaveTimeForExcel; }
            set
            {
                startLeaveTimeForExcel = value;
                OnPropertyChanged("StartLeaveTimeForExcel");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? EndLeaveTimeForExcel
        {
            get { return endLeaveTimeForExcel; }
            set
            {
                endLeaveTimeForExcel = value;
                OnPropertyChanged("EndLeaveTimeForExcel");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 IdEmpContractCompany
        {
            get { return idEmpContractCompany; }
            set
            {
                idEmpContractCompany = value;
                OnPropertyChanged("IdEmpContractCompany");
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


        Int32? idEmployeeStatus;
        [Column("IdEmployeeStatus")]
        [DataMember]
        public Int32? IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }

        Epc.LookupValue employeeStatus;
        [NotMapped]
        [DataMember]
        public Epc.LookupValue EmployeeStatus
        {
            get { return employeeStatus; }
            set
            {
                employeeStatus = value;
                OnPropertyChanged("EmployeeStatus");
            }
        }
        bool isEmployeeStatus;
        [NotMapped]
        [DataMember]
        public bool IsEmployeeStatus
        {
            get { return isEmployeeStatus; }
            set
            {
                isEmployeeStatus = value;
                OnPropertyChanged("IsEmployeeStatus");
            }
        }

        DateTime employeeContractSituationStartDate;
        [DataMember]
        public DateTime EmployeeContractSituationStartDate
        {
            get { return employeeContractSituationStartDate; }
            set
            {
                employeeContractSituationStartDate = value;
                OnPropertyChanged("EmployeeContractSituationStartDate");
            }
        }

        string employeeContractStartDate;
        [NotMapped]
        [DataMember]
        public string EmployeeContractStartDate
        {
            get { return employeeContractStartDate; }
            set
            {
                employeeContractStartDate = value;
                OnPropertyChanged("EmployeeContractStartDate");
            }
        }
        //[DataMember]
        //[NotMapped]
        //public byte[] FileInBytes
        //{
        //    get { return fileInBytes; }
        //    set
        //    {
        //        fileInBytes = value;
        //        OnPropertyChanged("FileInBytes");
        //    }
        //}

        #endregion

        #region Constructor

        public EmployeeLeave()
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

        #endregion

    }
}
