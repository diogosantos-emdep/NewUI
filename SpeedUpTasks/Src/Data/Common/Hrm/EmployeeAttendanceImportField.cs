using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeAttendanceImportField : ModelBase, IDisposable
    {
        #region Fields

        string name;
        DateTime startDate;
        DateTime endDate;
        Int32 idCompanyWork;
        EmployeeAttendanceImportField employeeAttendanceImportFieldChild;
        Int32 selectedIndex;
        Int32 id;
        CompanyShift companyShift;
        DateTime? accountingDate;
        //string selectedFieldName;

        #endregion

        #region Constructor
        public EmployeeAttendanceImportField()
        {
        }
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [NotMapped]
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

        [NotMapped]
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
        public int IdCompanyWork
        {
            get { return idCompanyWork; }
            set
            {
                idCompanyWork = value;
                OnPropertyChanged("IdCompanyWork");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeAttendanceImportField EmployeeAttendanceImportFieldChild
        {
            get { return employeeAttendanceImportFieldChild; }
            set
            {
                employeeAttendanceImportFieldChild = value;
                OnPropertyChanged("EmployeeAttendanceImportFieldChild");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
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
        public DateTime? AccountingDate
        {
            get { return accountingDate; }
            set
            {
                accountingDate = value;
                OnPropertyChanged("AccountingDate");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public string SelectedFieldName
        //{
        //    get { return selectedFieldName; }
        //    set
        //    {
        //        selectedFieldName = value;
        //        OnPropertyChanged("SelectedFieldName");
        //    }
        //}

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
