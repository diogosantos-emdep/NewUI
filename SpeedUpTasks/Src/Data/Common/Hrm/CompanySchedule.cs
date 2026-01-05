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
    [Table("company_schedules")]
    [DataContract]
    public class CompanySchedule : ModelBase, IDisposable
    {
        #region Fields

        Int32 idCompanySchedule;
        Int32 idCompany;
        string name;

        Company company;
        CompanyAnnualSchedule companyAnnualSchedule;

        List<CompanyShift> companyShifts;

        #endregion

        #region Properties

        [Key]
        [Column("IdCompanySchedule")]
        [DataMember]
        public Int32 IdCompanySchedule
        {
            get { return idCompanySchedule; }
            set
            {
                idCompanySchedule = value;
                OnPropertyChanged("IdCompanySchedule");
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

        [Column("Name")]
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
        public CompanyAnnualSchedule CompanyAnnualSchedule
        {
            get { return companyAnnualSchedule; }
            set
            {
                companyAnnualSchedule = value;
                OnPropertyChanged("CompanyAnnualSchedule");
            }
        }

        [NotMapped]
        [DataMember]
        public List<CompanyShift> CompanyShifts
        {
            get { return companyShifts; }
            set
            {
                companyShifts = value;
                OnPropertyChanged("CompanyShifts");
            }
        }

        #endregion

        #region Constructor

        public CompanySchedule()
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
            CompanySchedule companySchedule = (CompanySchedule)this.MemberwiseClone();

            if (companySchedule.Company != null)
                companySchedule.Company = (Company)this.Company.Clone();

            if (companySchedule.CompanyAnnualSchedule != null)
                companySchedule.CompanyAnnualSchedule = (CompanyAnnualSchedule)this.CompanyAnnualSchedule.Clone();

            if (companySchedule.CompanyShifts != null)
                companySchedule.CompanyShifts = CompanyShifts.Select(x => (CompanyShift)x.Clone()).ToList();

            return companySchedule;
        }

        #endregion
    }
}
