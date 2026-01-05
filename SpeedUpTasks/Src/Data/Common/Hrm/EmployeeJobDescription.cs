using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_job_descriptions")]
    [DataContract]
    public class EmployeeJobDescription : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeJobDescription;
        Int32 idEmployee;
        Int32 idCompany;
        UInt32 idJobDescription;
        UInt16 jobDescriptionUsage;
        DateTime? jobDescriptionStartDate;
        DateTime? jobDescriptionEndDate;
        string jobDescriptionRemarks;

        Company company;
        JobDescription jobDescription;
        Employee employee;
        Int64? idEmployeeExitEvent;
        EmployeeExitEvent employeeExitEvent;
        byte isMainJobDescription;
        Visibility isMainVisible;
        Visibility isAliasDiffVisible;
        bool isAliasDiffEnable;
        Int32? idRegion;
        byte? idCountry;
        Country country;
        LookupValue region;
        LookupValue jdScope;
        string strjdScope;
        #endregion

        #region Constructor

        public EmployeeJobDescription()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeJobDescription")]
        [DataMember]
        public ulong IdEmployeeJobDescription
        {
            get { return idEmployeeJobDescription; }
            set
            {
                idEmployeeJobDescription = value;
                OnPropertyChanged("IdEmployeeJobDescription");
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

        [Column("IdCompany")]
        [DataMember]
        public int IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [Column("IdJobDescription")]
        [DataMember]
        public uint IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [Column("JobDescriptionUsage")]
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

        [Column("JobDescriptionStartDate")]
        [DataMember]
        public DateTime? JobDescriptionStartDate
        {
            get { return jobDescriptionStartDate; }
            set
            {
                jobDescriptionStartDate = value;
                OnPropertyChanged("JobDescriptionStartDate");
            }
        }

        [Column("JobDescriptionEndDate")]
        [DataMember]
        public DateTime? JobDescriptionEndDate
        {
            get { return jobDescriptionEndDate; }
            set
            {
                jobDescriptionEndDate = value;
                OnPropertyChanged("JobDescriptionEndDate");
            }
        }

        [Column("JobDescriptionRemarks")]
        [DataMember]
        public string JobDescriptionRemarks
        {
            get { return jobDescriptionRemarks; }
            set
            {
                jobDescriptionRemarks = value;
                OnPropertyChanged("JobDescriptionRemarks");
            }
        }

        [NotMapped]
        [DataMember]
        public JobDescription JobDescription
        {
            get { return jobDescription; }
            set
            {
                jobDescription = value;
                OnPropertyChanged("JobDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
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
        public Employee Employee
        {
            get
            {
                return employee;
            }

            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [Column("IdEmployeeExitEvent")]
        [DataMember]
        public Int64? IdEmployeeExitEvent
        {
            get { return idEmployeeExitEvent; }
            set
            {
                idEmployeeExitEvent = value;
                OnPropertyChanged("IdEmployeeExitEvent");
            }
        }


        [NotMapped]
        [DataMember]
        public EmployeeExitEvent EmployeeExitEvent
        {
            get { return employeeExitEvent; }
            set
            {
                employeeExitEvent = value;
                OnPropertyChanged("EmployeeExitEvent");
            }
        }

        [Column("IsMainJobDescription")]
        [DataMember]
        public byte IsMainJobDescription
        {
            get { return isMainJobDescription; }
            set
            {
                isMainJobDescription = value;
                OnPropertyChanged("IsMainJobDescription");
            }
        }

        [Column("IsMainVisible")]
        [DataMember]
        public Visibility IsMainVisible
        {
            get
            {
                return isMainVisible;
            }

            set
            {
                isMainVisible = value;
                OnPropertyChanged("IsMainVisible");
            }
        }

        [Column("IsAliasDiffVisible")]
        [DataMember]
        public Visibility IsAliasDiffVisible
        {
            get
            {
                return isAliasDiffVisible;
            }

            set
            {
                isAliasDiffVisible = value;
                OnPropertyChanged("IsAliasDiffVisible");
            }
        }

        [Column("IsAliasDiffEnable")]
        [DataMember]
        public bool IsAliasDiffEnable
        {
            get
            {
                return isAliasDiffEnable;
            }

            set
            {
                isAliasDiffEnable = value;
                OnPropertyChanged("IsAliasDiffEnable");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IdCountry
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


        [NotMapped]
        [DataMember]
        public LookupValue Region
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

        [NotMapped]
        [DataMember]
        public Country Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
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

        [NotMapped]
        [DataMember]
        public string StrJDScope
        {
            get { return strjdScope; }
            set
            {
                strjdScope = value;
                OnPropertyChanged("StrJDScope");
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
            EmployeeJobDescription employeeJobDescription = (EmployeeJobDescription)this.MemberwiseClone();

            if (employeeJobDescription.Company != null)
                employeeJobDescription.Company = (Company)this.Company.Clone();

            if (employeeJobDescription.JobDescription != null)
                employeeJobDescription.JobDescription = (JobDescription)this.JobDescription.Clone();

            return employeeJobDescription;
        }

        #endregion
    }
}
