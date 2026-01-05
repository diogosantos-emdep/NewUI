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
    [Table("departments")]
    [DataContract]
    public class Department : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idDepartment;
        string departmentName;
        string abbreviation;
        UInt32 idDepartmentParent;
        UInt32 idDepartmentArea;
        string departmentPosition;
        byte departmentInUse;
        string departmentHtmlColor;
        byte departmentIsIsolated;
        List<Employee> employees;
        List<JobDescription> jobDescriptions;
        List<Department> childDepartments;
        LookupValue departmentArea;
        Department parentDepartment;
        UInt32 employeesCount;
        decimal yearsCount;
        decimal employeesRecordCount;

        #endregion

        #region Constructor
        public Department()
        {
        }

        #endregion

        #region Properties

        [Key]
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

        [Column("DepartmentName")]
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

        [Column("IdDepartmentParent")]
        [DataMember]
        public uint IdDepartmentParent
        {
            get { return idDepartmentParent; }
            set
            {
                idDepartmentParent = value;
                OnPropertyChanged("IdDepartmentParent");
            }
        }

        [Column("DepartmentPosition")]
        [DataMember]
        public string DepartmentPosition
        {
            get { return departmentPosition; }
            set
            {
                departmentPosition = value;
                OnPropertyChanged("DepartmentPosition");
            }
        }

        [Column("DepartmentInUse")]
        [DataMember]
        public byte DepartmentInUse
        {
            get { return departmentInUse; }
            set
            {
                departmentInUse = value;
                OnPropertyChanged("DepartmentInUse");
            }
        }

        [Column("DepartmentHtmlColor")]
        [DataMember]
        public string DepartmentHtmlColor
        {
            get { return departmentHtmlColor; }
            set
            {
                departmentHtmlColor = value;
                OnPropertyChanged("DepartmentHtmlColor");
            }
        }

        [Column("DepartmentIsIsolated")]
        [DataMember]
        public byte DepartmentIsIsolated
        {
            get { return departmentIsIsolated; }
            set
            {
                departmentIsIsolated = value;
                OnPropertyChanged("DepartmentIsIsolated");
            }
        }

        [DataMember]
        [NotMapped]
        public List<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }

        [DataMember]
        [NotMapped]
        public List<Department> ChildDepartments
        {
            get { return childDepartments; }
            set
            {
                childDepartments = value;
                OnPropertyChanged("ChildDepartments");
            }
        }

        [DataMember]
        [NotMapped]
        public List<JobDescription> JobDescriptions
        {
            get { return jobDescriptions; }
            set
            {
                jobDescriptions = value;
                OnPropertyChanged("JobDescriptions");
            }
        }

        [DataMember]
        [NotMapped]
        public LookupValue DepartmentArea
        {
            get { return departmentArea; }
            set
            {
                departmentArea = value;
                OnPropertyChanged("DepartmentArea");
            }
        }

        [DataMember]
        [NotMapped]
        public Department ParentDepartment
        {
            get { return parentDepartment; }
            set
            {
                parentDepartment = value;
                OnPropertyChanged("ParentDepartment");
            }
        }

        /// <summary>
        /// Lookup key 27
        /// </summary>
        [Column("IdDepartmentArea")]
        [DataMember]
        public UInt32 IdDepartmentArea
        {
            get { return idDepartmentArea; }
            set
            {
                idDepartmentArea = value;
                OnPropertyChanged("IdDepartmentArea");
            }
        }

        [DataMember]
        [NotMapped]
        public uint EmployeesCount
        {
            get { return employeesCount; }
            set
            {
                employeesCount = value;
                OnPropertyChanged("EmployeesCount");
            }
        }

        [DataMember]
        [NotMapped]
        public decimal YearsCount
        {
            get { return yearsCount; }
            set
            {
                yearsCount = value;
                OnPropertyChanged("YearsCount");
            }
        }

        [Column("Abbreviation")]
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
        public decimal EmployeesRecordCount
        {
            get { return employeesRecordCount; }
            set
            {
                employeesRecordCount = value;
                OnPropertyChanged("EmployeesRecordCount");
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return DepartmentName;
        }

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
