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
    [Table("company_annual_schedules")]
    [DataContract]
    public class CompanyAnnualSchedule : ModelBase, IDisposable
    {
        #region Fields

        Int64 idCompanyAnnualSchedule;
        Int32 idCompany;
        Int32 idCompanySchedule;
        Int32 year;
        decimal dailyHoursCount;
        decimal weeklyHoursCount;

        Company company;
        CompanySchedule companySchedule;
        string workingDays;
        CompanySetting companySetting;
        #endregion

        #region Properties

        [Key]
        [Column("IdCompanyAnnualSchedule")]
        [DataMember]
        public Int64 IdCompanyAnnualSchedule
        {
            get { return idCompanyAnnualSchedule; }
            set
            {
                idCompanyAnnualSchedule = value;
                OnPropertyChanged("IdCompanyAnnualSchedule");
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


        [Column("Year")]
        [DataMember]
        public Int32 Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }


        [Column("DailyHoursCount")]
        [DataMember]
        public decimal DailyHoursCount
        {
            get { return dailyHoursCount; }
            set
            {
                dailyHoursCount = value;
                OnPropertyChanged("DailyHoursCount");
            }
        }


        [Column("WeeklyHoursCount")]
        [DataMember]
        public decimal WeeklyHoursCount
        {
            get { return weeklyHoursCount; }
            set
            {
                weeklyHoursCount = value;
                OnPropertyChanged("WeeklyHoursCount");
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
        public CompanySchedule CompanySchedule
        {
            get { return companySchedule; }
            set
            {
                companySchedule = value;
                OnPropertyChanged("CompanySchedule");
            }
        }


        [NotMapped]
        [DataMember]
        public string WorkingDays
        {
            get { return workingDays; }
            set
            {
                workingDays = value;
                OnPropertyChanged("WorkingDays");
            }
        }



        [NotMapped]
        [DataMember]
        public CompanySetting CompanySetting
        {
            get { return companySetting; }
            set
            {
                companySetting = value;
                OnPropertyChanged("CompanySetting");
            }
        }
        #endregion

        #region Constructor

        public CompanyAnnualSchedule()
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
            CompanyAnnualSchedule companyAnnualSchedule = (CompanyAnnualSchedule)this.MemberwiseClone();

            if (companyAnnualSchedule.Company != null)
                companyAnnualSchedule.Company = (Company)this.Company.Clone();

            if (companyAnnualSchedule.CompanySchedule != null)
                companyAnnualSchedule.CompanySchedule = (CompanySchedule)this.CompanySchedule.Clone();

            return this.MemberwiseClone();
        }

        #endregion
    }
}
