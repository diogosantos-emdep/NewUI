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
    [Table("employee_polyvalence")]
    [DataContract]
    public class EmployeePolyvalence : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeePolyvalence;
        Int32 idEmployee;
        Int32 idCompany;
        UInt32 idJobDescription;
        UInt16 polyvalenceUsage;
        string polyvalenceUsages;
        string emplyeePolyvalenceAndJD;
        string polyvalenceRemarks;
        string alias;
        Company company;
        JobDescription jobDescription;
        Employee employee;
        DateTime? jobDescriptionStartDate;
        DateTime? jobDescriptionEndDate;
        string jobDescriptionTitle;
        #endregion

        #region Constructor

        public EmployeePolyvalence()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeePolyvalence")]
        [DataMember]
        public ulong IdEmployeePolyvalence
        {
            get { return idEmployeePolyvalence; }
            set
            {
                idEmployeePolyvalence = value;
                OnPropertyChanged("IdEmployeePolyvalence");
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

        [Column("PolyvalenceUsage")]
        [DataMember]
        public UInt16 PolyvalenceUsage
        {
            get { return polyvalenceUsage; }
            set
            {
                polyvalenceUsage = value;
                OnPropertyChanged("PolyvalenceUsage");
            }
        }


        [Column("PolyvalenceUsage")]
        [DataMember]
        public string PolyvalenceUsages
        {
            get { return polyvalenceUsages; }
            set
            {
                polyvalenceUsages = value;
                OnPropertyChanged("PolyvalenceUsages");
            }
        }
       

        [Column("PolyvalenceRemarks")]
        [DataMember]
        public string PolyvalenceRemarks
        {
            get { return polyvalenceRemarks; }
            set
            {
                polyvalenceRemarks = value;
                OnPropertyChanged("PolyvalenceRemarks");
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


        [Column("Alias")]
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

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            EmployeePolyvalence employeePolyvalence = (EmployeePolyvalence)this.MemberwiseClone();

            if (employeePolyvalence.Company != null)
                employeePolyvalence.Company = (Company)this.Company.Clone();

            if (employeePolyvalence.JobDescription != null)
                employeePolyvalence.JobDescription = (JobDescription)this.JobDescription.Clone();

            return employeePolyvalence;
        }

        #endregion
    }
}
