using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("company_departments")]
    [DataContract]
    public class CompanyDepartment : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idDepartment;
        Int64 idCompanyDepartment;
        Int32 idCompany;
        Int64 numberOfWorkstations;
        double size;
        Department department;
        List<JobDescription> jobDescriptions;
        decimal employeesRecordCount;
        Int32 idDepartmentArea;
        #endregion


        #region Constructor
        public CompanyDepartment()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdCompanyDepartment")]
        [DataMember]
        public Int64 IdCompanyDepartment
        {
            get { return idCompanyDepartment; }
            set
            {
                idCompanyDepartment = value;
                OnPropertyChanged("IdCompanyDepartment");
            }
        }

        [Column("IdDepartment")]
        [DataMember]
        public UInt32 IdDepartment
        {
            get { return idDepartment; }
            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }

        [Column("IdCompany")]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [Column("NumberOfWorkstations")]
        [DataMember]
        public Int64 NumberOfWorkstations
        {
            get { return numberOfWorkstations; }
            set
            {
                numberOfWorkstations = value;
                OnPropertyChanged("NumberOfWorkstations");
            }
        }

        [Column("Size")]
        [DataMember]
        public double Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
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
        public List<JobDescription> JobDescriptions
        {
            get { return jobDescriptions; }
            set
            {
                jobDescriptions = value;
                OnPropertyChanged("JobDescriptions");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal EmployeesRecordCount
        {
            get { return employeesRecordCount; }
            set
            {
                employeesRecordCount = value;
                OnPropertyChanged("EmployeesRecordCount");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdDepartmentArea
        {
            get { return idDepartmentArea; }
            set
            {
                idDepartmentArea = value;
                OnPropertyChanged("IdDepartmentArea");
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
            return this.MemberwiseClone();

        }

        #endregion
    }
}
