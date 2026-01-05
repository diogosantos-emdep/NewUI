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
    [Table("job_descriptions")]
    [DataContract]
    public class JobDescription : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idJobDescription;
        string jobDescriptionCode;
        string jobDescriptionTitle;
        UInt32 idDepartment;
        string jobDescriptionFileName;
        byte jobDescriptionInUse;
        byte jobDescriptionIsRemote;
        Department department;
        List<Employee> employees;
        string jobDescriptionTitleAndCode;
        UInt64 employeeCount;
        decimal yearsCount;

        bool isJobDescriptionFileUpdated;
        byte[] jobDescriptionFileInBytes;

        string oldJobDescriptionCode;
        UInt32 idParent;
        string abbreviation;

        //ChildOrientations childOrientation1;
        JobDescription parentJobDescription;

        string childOrientation;
        List<JobDescription> childJobDescriptions;
        List<EmployeeJobDescription> employeeJobDescriptions;
        bool isEmployeeExistInChildJD;
        decimal employeeRecordCount;
        Int32? idJDLevel;
        LookupValue jdLevel;
        byte jobDescriptionIsMandatory;
        Int32 idJdScope;
        LookupValue jdScope;
        List<LogNewJobDescription> logEntries;
        //[pramod.misal][17.06.2024]
        List<HealthAndSafetyAttachedDoc> healthAndSafetyAttachedDocList;
        List<EquipmentAndToolsAttachedDoc> equipmentAndToolsList;
        #endregion

        //public enum ChildOrientations
        //{
        //    H, V, NULL
        //}

        #region Constructor
        public JobDescription()
        {
        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public List<LogNewJobDescription> LogEntries
        {
            get
            {
                return logEntries;
            }

            set
            {
                logEntries = value;
                OnPropertyChanged("LogEntries");
            }
        }
        [Key]
        [Column("IdJobDescription")]
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

        [Column("JobDescriptionCode")]
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

        [Column("JobDescriptionTitle")]
        [DataMember]
        public string JobDescriptionTitle
        {
            get { return jobDescriptionTitle; }
            set
            {
                jobDescriptionTitle = value;
                OnPropertyChanged("JobDescriptionTitle");
            }
        }

        [Column("IdDepartment")]
        [DataMember]
        public uint IdDepartment
        {
            get { return idDepartment; }
            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }

        [Column("JobDescriptionFileName")]
        [DataMember]
        public string JobDescriptionFileName
        {
            get { return jobDescriptionFileName; }
            set
            {
                jobDescriptionFileName = value;
                OnPropertyChanged("JobDescriptionFileName");
            }
        }

        [Column("JobDescriptionInUse")]
        [DataMember]
        public byte JobDescriptionInUse
        {
            get { return jobDescriptionInUse; }
            set
            {
                jobDescriptionInUse = value;
                OnPropertyChanged("JobDescriptionInUse");
            }
        }

        [Column("JobDescriptionIsRemote")]
        [DataMember]
        public byte JobDescriptionIsRemote
        {
            get { return jobDescriptionIsRemote; }
            set
            {
                jobDescriptionIsRemote = value;
                OnPropertyChanged("JobDescriptionIsRemote");
            }
        }

        [NotMapped]
        [DataMember]
        public Department Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }

        [NotMapped]
        [DataMember]
        public string JobDescriptionTitleAndCode
        {
            get { return jobDescriptionTitleAndCode; }
            set
            {
                jobDescriptionTitleAndCode = value;
                OnPropertyChanged("JobDescriptionTitleAndCode");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong EmployeeCount
        {
            get { return employeeCount; }
            set
            {
                employeeCount = value;
                OnPropertyChanged("EmployeeCount");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal YearsCount
        {
            get { return yearsCount; }
            set
            {
                yearsCount = value;
                OnPropertyChanged("YearsCount");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsJobDescriptionFileUpdated
        {
            get { return isJobDescriptionFileUpdated; }
            set
            {
                isJobDescriptionFileUpdated = value;
                OnPropertyChanged("IsJobDescriptionFileUpdated");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] JobDescriptionFileInBytes
        {
            get { return jobDescriptionFileInBytes; }
            set
            {
                jobDescriptionFileInBytes = value;
                OnPropertyChanged("JobDescriptionFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public string OldJobDescriptionCode
        {
            get { return oldJobDescriptionCode; }
            set
            {
                oldJobDescriptionCode = value;
                OnPropertyChanged("OldJobDescriptionCode");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        [NotMapped]
        [DataMember]
        public string Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }

        [NotMapped]
        [DataMember]
        public string ChildOrientation
        {
            get { return childOrientation; }
            set
            {
                childOrientation = value;
                OnPropertyChanged("ChildOrientation");
            }
        }

        [NotMapped]
        [DataMember]
        public List<JobDescription> ChildJobDescriptions
        {
            get { return childJobDescriptions; }
            set
            {
                childJobDescriptions = value;
                OnPropertyChanged("ChildJobDescriptions");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeJobDescription> EmployeeJobDescriptions
        {
            get { return employeeJobDescriptions; }
            set
            {
                employeeJobDescriptions = value;
                OnPropertyChanged("EmployeeJobDescriptions");
            }
        }

        [NotMapped]
        [DataMember]
        public JobDescription ParentJobDescription
        {
            get { return parentJobDescription; }
            set
            {
                parentJobDescription = value;
                OnPropertyChanged("ParentJobDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEmployeeExistInChildJD
        {
            get { return isEmployeeExistInChildJD; }
            set
            {
                isEmployeeExistInChildJD = value;
                OnPropertyChanged("IsEmployeeExistInChildJD");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal EmployeeRecordCount
        {
            get { return employeeRecordCount; }
            set
            {
                employeeRecordCount = value;
                OnPropertyChanged("EmployeeRecordCount");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdJDLevel
        {
            get { return idJDLevel; }
            set
            {
                idJDLevel = value;
                OnPropertyChanged("IdJDLevel");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue JDLevel
        {
            get { return jdLevel; }
            set
            {
                jdLevel = value;
                OnPropertyChanged("JDLevel");
            }
        }


        [Column("JobDescriptionIsMandatory")]
        [DataMember]
        public byte JobDescriptionIsMandatory
        {
            get { return jobDescriptionIsMandatory; }
            set
            {
                jobDescriptionIsMandatory = value;
                OnPropertyChanged("JobDescriptionIsMandatory");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdJdScope
        {
            get { return idJdScope; }
            set
            {
                idJdScope = value;
                OnPropertyChanged("IdJdScope");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue JDScope
        {
            get { return jdScope; }
            set
            {
                jdScope = value;
                OnPropertyChanged("JDScope");
            }
        }

        //pramod.misal 17.06.2024
        [NotMapped]
        [DataMember]
        public List<HealthAndSafetyAttachedDoc> HealthAndSafetyAttachedDocList
        {
            get
            {
                return healthAndSafetyAttachedDocList;
            }

            set
            {
                healthAndSafetyAttachedDocList = value;
                OnPropertyChanged("HealthAndSafetyAttachedDocList");
            }
        }


        //[Sudhir.jangra][GEOS2-5549]
        [NotMapped]
        [DataMember]
        public List<EquipmentAndToolsAttachedDoc> EquipmentAndToolsList
        {
            get { return equipmentAndToolsList; }
            set
            {
                equipmentAndToolsList = value;
                OnPropertyChanged("EquipmentAndToolsList");
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
            JobDescription jobDescription = (JobDescription)this.MemberwiseClone();

            if (jobDescription.Department != null)
                jobDescription.Department = (Department)this.Department.Clone();

            if (jobDescription.ParentJobDescription != null)
                jobDescription.ParentJobDescription = (JobDescription)this.ParentJobDescription.Clone();

            if (jobDescription.Employees != null)
                jobDescription.Employees = Employees.Select(x => (Employee)x.Clone()).ToList();

            if (jobDescription.ChildJobDescriptions != null)
                jobDescription.ChildJobDescriptions = ChildJobDescriptions.Select(x => (JobDescription)x.Clone()).ToList();

            if (jobDescription.EmployeeJobDescriptions != null)
                jobDescription.EmployeeJobDescriptions = EmployeeJobDescriptions.Select(x => (EmployeeJobDescription)x.Clone()).ToList();

            return jobDescription;
        }

        #endregion
    }
}
