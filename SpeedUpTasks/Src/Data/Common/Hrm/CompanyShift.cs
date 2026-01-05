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
    [Table("company_shifts")]
    [DataContract]
    public class CompanyShift : ModelBase, IDisposable
    {
        #region Fields

        Int32 idCompanyShift;
        Int32 idCompanySchedule;
        string name;
        TimeSpan startTime1;
        TimeSpan endTime1;
        TimeSpan startTime2;
        TimeSpan endTime2;
        TimeSpan breakTime;

        CompanySchedule companySchedule;
        CompanyAnnualSchedule companyAnnualSchedule;

        sbyte mon;
        sbyte tue;
        sbyte wed;
        sbyte thu;
        sbyte fri;
        sbyte sat;
        sbyte sun;

        TimeSpan monStartTime;
        TimeSpan monEndTime;
        TimeSpan monBreakTime;

        TimeSpan tueStartTime;
        TimeSpan tueEndTime;
        TimeSpan tueBreakTime;

        TimeSpan wedStartTime;
        TimeSpan wedEndTime;
        TimeSpan wedBreakTime;

        TimeSpan thuStartTime;
        TimeSpan thuEndTime;
        TimeSpan thuBreakTime;

        TimeSpan friStartTime;
        TimeSpan friEndTime;
        TimeSpan friBreakTime;

        TimeSpan satStartTime;
        TimeSpan satEndTime;
        TimeSpan satBreakTime;

        TimeSpan sunStartTime;
        TimeSpan sunEndTime;
        TimeSpan sunBreakTime;

        bool isExistShift;
        sbyte isNightShift;
        float totalTimeInHours;
        #endregion

        #region Properties

        [Key]
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

        [Column("StartTime1")]
        [DataMember]
        public TimeSpan StartTime1
        {
            get { return startTime1; }
            set
            {
                startTime1 = value;
                OnPropertyChanged("StartTime1");
            }
        }

        [Column("EndTime1")]
        [DataMember]
        public TimeSpan EndTime1
        {
            get { return endTime1; }
            set
            {
                endTime1 = value;
                OnPropertyChanged("EndTime1");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShiftTime
        {
            get
            {
                if (IdCompanyShift == 0 && TimeSpan.Compare(StartTime1, TimeSpan.Zero) == 0 && TimeSpan.Compare(EndTime1, TimeSpan.Zero) == 0)
                    return "---";
                else
                    return StartTime1.ToString(@"hh\:mm") + " - " + EndTime1.ToString(@"hh\:mm");
            }
            set { }
        }

        [Column("StartTime2")]
        [DataMember]
        public TimeSpan StartTime2
        {
            get { return startTime2; }
            set
            {
                startTime2 = value;
                OnPropertyChanged("StartTime2");
            }
        }

        [Column("EndTime2")]
        [DataMember]
        public TimeSpan EndTime2
        {
            get { return endTime2; }
            set
            {
                endTime2 = value;
                OnPropertyChanged("EndTime2");
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
        public CompanyAnnualSchedule CompanyAnnualSchedule
        {
            get { return companyAnnualSchedule; }
            set
            {
                companyAnnualSchedule = value;
                OnPropertyChanged("CompanyAnnualSchedule");
            }
        }

        [Column("BreakTime")]
        [DataMember]
        public TimeSpan BreakTime
        {
            get { return breakTime; }
            set
            {
                breakTime = value;
                OnPropertyChanged("BreakTime");
            }
        }

        [Column("Mon")]
        [DataMember]
        public sbyte Mon
        {
            get { return mon; }
            set
            {
                mon = value;
                OnPropertyChanged("Mon");
            }
        }

        [Column("Tue")]
        [DataMember]
        public sbyte Tue
        {
            get { return tue; }
            set
            {
                tue = value;
                OnPropertyChanged("Tue");
            }
        }

        [Column("Wed")]
        [DataMember]
        public sbyte Wed
        {
            get { return wed; }
            set
            {
                wed = value;
                OnPropertyChanged("Wed");
            }
        }

        [Column("Thu")]
        [DataMember]
        public sbyte Thu
        {
            get { return thu; }
            set
            {
                thu = value;
                OnPropertyChanged("Thu");
            }
        }

        [Column("Fri")]
        [DataMember]
        public sbyte Fri
        {
            get { return fri; }
            set
            {
                fri = value;
                OnPropertyChanged("Fri");
            }
        }

        [Column("Sat")]
        [DataMember]
        public sbyte Sat
        {
            get { return sat; }
            set
            {
                sat = value;
                OnPropertyChanged("Sat");
            }
        }

        [Column("Sun")]
        [DataMember]
        public sbyte Sun
        {
            get { return sun; }
            set
            {
                sun = value;
                OnPropertyChanged("Sun");
            }
        }

        [Column("MonStartTime")]
        [DataMember]
        public TimeSpan MonStartTime
        {
            get { return monStartTime; }
            set
            {
                monStartTime = value ;
                OnPropertyChanged("MonStartTime");
            }
        }
        [Column("MonEndTime")]
        [DataMember]
        public TimeSpan MonEndTime
        {
            get { return monEndTime; }
            set
            {
                monEndTime = value;
                OnPropertyChanged("MonEndTime");
            }
        }
        [Column("MonBreakTime")]
        [DataMember]
        public TimeSpan MonBreakTime
        {
            get { return monBreakTime; }
            set
            {
                monBreakTime = value;
                OnPropertyChanged("MonBreakTime");
            }
        }

        [Column("TueStartTime")]
        [DataMember]
        public TimeSpan TueStartTime
        {
            get { return tueStartTime; }
            set
            {
                tueStartTime = value;
                OnPropertyChanged("TueStartTime");
            }
        }

        [Column("TueEndTime")]
        [DataMember]
        public TimeSpan TueEndTime
        {
            get { return tueEndTime; }
            set
            {
                tueEndTime = value;
                OnPropertyChanged("TueEndTime");
            }
        }

        [Column("TueBreakTime")]
        [DataMember]
        public TimeSpan TueBreakTime
        {
            get { return tueBreakTime; }
            set
            {
                tueBreakTime = value;
                OnPropertyChanged("TueBreakTime");
            }
        }

        [Column("WedStartTime")]
        [DataMember]
        public TimeSpan WedStartTime
        {
            get { return wedStartTime; }
            set
            {
                wedStartTime = value;
                OnPropertyChanged("WedStartTime");
            }
        }

        [Column("WedEndTime")]
        [DataMember]
        public TimeSpan WedEndTime
        {
            get { return wedEndTime; }
            set
            {
                wedEndTime = value;
                OnPropertyChanged("WedEndTime");
            }
        }

        [Column("WedBreakTime")]
        [DataMember]
        public TimeSpan WedBreakTime
        {
            get { return wedBreakTime; }
            set
            {
                wedBreakTime = value;
                OnPropertyChanged("WedBreakTime");
            }
        }

        [Column("ThuStartTime")]
        [DataMember]
        public TimeSpan ThuStartTime
        {
            get { return thuStartTime; }
            set
            {
                thuStartTime = value;
                OnPropertyChanged("ThuStartTime");
            }
        }

        [Column("ThuEndTime")]
        [DataMember]
        public TimeSpan ThuEndTime
        {
            get { return thuEndTime; }
            set
            {
                thuEndTime = value;
                OnPropertyChanged("ThuEndTime");
            }
        }

        [Column("ThuBreakTime")]
        [DataMember]
        public TimeSpan ThuBreakTime
        {
            get { return thuBreakTime; }
            set
            {
               thuBreakTime = value;
                OnPropertyChanged("ThuBreakTime");
            }
        }

        [Column("FriStartTime")]
        [DataMember]
        public TimeSpan FriStartTime
        {
            get { return friStartTime; }
            set
            {
                friStartTime = value;
                OnPropertyChanged("FriStartTime");
            }
        }

        [Column("FriEndTime")]
        [DataMember]
        public TimeSpan FriEndTime
        {
            get { return friEndTime; }
            set
            {
                friEndTime = value;
                OnPropertyChanged("FriEndTime");
            }
        }

        [Column("FriBreakTime")]
        [DataMember]
        public TimeSpan FriBreakTime
        {
            get { return friBreakTime; }
            set
            {
                friBreakTime = value;
                OnPropertyChanged("FriBreakTime");
            }
        }

        [Column("SatStartTime")]
        [DataMember]
        public TimeSpan SatStartTime
        {
            get { return satStartTime; }
            set
            {
                satStartTime = value;
                OnPropertyChanged("SatStartTime");
            }
        }

        [Column("SatEndTime")]
        [DataMember]
        public TimeSpan SatEndTime
        {
            get { return satEndTime; }
            set
            {
                satEndTime = value;
                OnPropertyChanged("SatEndTime");
            }
        }

        [Column("SatBreakTime")]
        [DataMember]
        public TimeSpan SatBreakTime
        {
            get { return satBreakTime; }
            set
            {
                satBreakTime = value;
                OnPropertyChanged("SatBreakTime");
            }
        }

        [Column("SunStartTime")]
        [DataMember]
        public TimeSpan SunStartTime
        {
            get { return sunStartTime; }
            set
            {
                sunStartTime = value;
                OnPropertyChanged("SunStartTime");
            }
        }

        [Column("SunEndTime")]
        [DataMember]
        public TimeSpan SunEndTime
        {
            get { return sunEndTime; }
            set
            {
                sunEndTime = value;
                OnPropertyChanged("SunEndTime");
            }
        }

        [Column("SunBreakTime")]
        [DataMember]
        public TimeSpan SunBreakTime
        {
            get { return sunBreakTime; }
            set
            {
                sunBreakTime = value;
                OnPropertyChanged("SunBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsExistShift
        {
            get { return isExistShift; }
            set
            {
                isExistShift = value;
                OnPropertyChanged("IsExistShift");
            }
        }


        [NotMapped]
        [DataMember]
        public sbyte IsNightShift
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
        public float TotalTimeInHours
        {
            get { return totalTimeInHours; }
            set
            {
                totalTimeInHours = value;
                OnPropertyChanged("TotalTimeInHours");
            }
        }

        #endregion

        #region Constructor

        public CompanyShift()
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
            CompanyShift companyShift = (CompanyShift)this.MemberwiseClone();

            if (companyShift.CompanySchedule != null)
                companyShift.CompanySchedule = (CompanySchedule)this.CompanySchedule.Clone();

            if (companyShift.CompanyAnnualSchedule != null)
                companyShift.CompanyAnnualSchedule = (CompanyAnnualSchedule)this.CompanyAnnualSchedule.Clone();

            return companyShift;
        }

        #endregion
    }
}
