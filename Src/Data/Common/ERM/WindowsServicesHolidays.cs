using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [Table("company_holidays")]
    [DataContract]
    public class WindowsServicesHolidays : ModelBase, IDisposable
    {

        #region Fields
        private string startdayName;
        private string enddayName;
        UInt64 idCompanyHoliday;
        Int32 idCompany;
        string name;
        Int32 idHoliday;
        DateTime? startDate;
        TimeSpan? startTime = TimeSpan.Zero;

        DateTime? endDate;
        TimeSpan? endTime = TimeSpan.Zero;

        byte isRecursive;
        sbyte isAllDayEvent = 1;

        LookupValue holiday;
        Company company;
        string startDateWeek;
        string endDateWeek;
        #endregion

        #region Properties

        [Key]
        [Column("IdCompanyHoliday")]
        [DataMember]
        public ulong IdCompanyHoliday
        {
            get { return idCompanyHoliday; }
            set
            {
                idCompanyHoliday = value;
                OnPropertyChanged("IdCompanyHoliday");
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

        [Column("IdHoliday")]
        [DataMember]
        public Int32 IdHoliday
        {
            get { return idHoliday; }
            set
            {
                idHoliday = value;
                OnPropertyChanged("IdHoliday");
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

        [Column("IsRecursive")]
        [DataMember]
        public byte IsRecursive
        {
            get { return isRecursive; }
            set
            {
                isRecursive = value;
                OnPropertyChanged("IsRecursive");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Holiday
        {
            get { return holiday; }
            set
            {
                holiday = value;
                OnPropertyChanged("Holiday");
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

        [Column("IsAllDayEvent")]
        [DataMember]
        public sbyte IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged("IsAllDayEvent");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
        [DataMember]
        public string StartDayName
        {
            get
            {
                return startdayName;
            }

            set
            {
                startdayName = value;
                OnPropertyChanged("StartDayName");
            }
        }
        [DataMember]
        public string EndDayName
        {
            get
            {
                return enddayName;
            }

            set
            {
                enddayName = value;
                OnPropertyChanged("EndDayName");
            }
        }
        [DataMember]
        public string StartDateWeek
        {
            get
            {
                return startDateWeek;
            }

            set
            {
                startDateWeek = value;
                OnPropertyChanged("StartDateWeek");
            }
        }
        [DataMember]
        public string EndDateWeek
        {
            get
            {
                return endDateWeek;
            }

            set
            {
                endDateWeek = value;
                OnPropertyChanged("EndDateWeek");
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
