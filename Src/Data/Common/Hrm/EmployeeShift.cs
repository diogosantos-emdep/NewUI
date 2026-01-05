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
    [Table("employee_shifts")]
    [DataContract]
    public class EmployeeShift : ModelBase, IDisposable
    {

        #region Fields

        Int64 idEmployeeShift;
        Int32 idEmployee;
        Int32 idCompanyShift;
        DateTime? startDate;
        DateTime? endDate;
        Employee employee;
        CompanyShift companyShift;
        bool isEnabled = true;
        bool isEmployeeShiftNOTInUse;
        bool isNightShift;
        DateTime accountingDate;//[Sudhir.Jangra][GEOS2-4037][29/06/2023]
        Visibility isMaxVisible;//[Sudhir.Jangra][GEOS2-4037][29/06/2023]
        #endregion

        #region Constructor
        public EmployeeShift()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeShift")]
        [DataMember]
        public Int64 IdEmployeeShift
        {
            get { return idEmployeeShift; }
            set
            {
                idEmployeeShift = value;
                OnPropertyChanged("IdEmployeeShift");
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

        [Column("IdCompanyShift")]
        [DataMember]
        public Int32 IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
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
        //[sudhir.jangra][GEOS2-2716][21/10/2022][added field EmployeeshiftInUse]
        [NotMapped]
        [DataMember]
        public bool IsEmployeeShiftNOTInUse
        {
            get { return isEmployeeShiftNOTInUse; }
            set
            {
                isEmployeeShiftNOTInUse = value;
                OnPropertyChanged("IsEmployeeShiftNOTInUse");
            }
        }
        //[sudhir.jangra][GEOS2-2716][31/10/2022][added field NightShift]
        [NotMapped]
        [DataMember]
        public bool IsNightShift
        {
            get { return isNightShift; }
            set
            {
                isNightShift = value;
                OnPropertyChanged("IsNightShift");
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
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        //[Sudhir.Jangra][GEOS2-4037]
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

        //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
        public Visibility IsMaxVisible
        {
            get { return isMaxVisible; }
            set
            {
                isMaxVisible = value;
                OnPropertyChanged("IsMaxVisible");
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
