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
   public class EmployeeLeavesImportField : ModelBase, IDisposable
    {
        #region Fields

        string name;
        DateTime startDate;
        DateTime endDate;
        Int32 idCompanyLeave;
        EmployeeLeavesImportField employeeLeavesImportFieldChild;
        Int32 selectedIndex;
        Int32 id;
        CompanyShift companyShift;
        DateTime? accountingDate;

        #endregion

        #region Constructor
        public EmployeeLeavesImportField()
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
        public int IdCompanyLeave
        {
            get { return idCompanyLeave; }
            set
            {
                idCompanyLeave = value;
                OnPropertyChanged("IdCompanyLeave");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeLeavesImportField EmployeeLeavesImportFieldChild
        {
            get { return employeeLeavesImportFieldChild; }
            set
            {
                employeeLeavesImportFieldChild = value;
                OnPropertyChanged("EmployeeLeavesImportFieldChild");
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
