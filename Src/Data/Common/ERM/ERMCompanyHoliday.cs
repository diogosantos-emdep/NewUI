using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMCompanyHoliday : ModelBase, IDisposable
    {
        #region Field
        private Int32 idCompanyHoliday;
        private Int32 idCompany;
        private string name;
        private DateTime? startDate;
        private DateTime? endDate;
        private Int32 isRecursive;
        private Int32 isAllDayEvent;
        private string holidayType;//[gulab lakade][08 08 2025] 
        private string calenderWeek; //[GEOS2-9220][gulab lakade][12 08 2025]
        #endregion
        #region Property
        [DataMember]
        public Int32 IdCompanyHoliday
        {
            get { return idCompanyHoliday; }
            set
            {
                idCompanyHoliday = value;
                OnPropertyChanged("IdCompanyHoliday");
            }
        }
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
        // [NotMapped]
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

        [DataMember]
        public Int32 IsRecursive
        {
            get { return isRecursive; }
            set
            {
                isRecursive = value;
                OnPropertyChanged("IsRecursive");
            }
        }
        [DataMember]
        public Int32 IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged("IsAllDayEvent");
            }
        }
        //[GEOS2-9220][gulab lakade][12 08 2025]
        [DataMember]
        public string HolidayType
        {
            get
            {
                return holidayType;
            }

            set
            {
                holidayType = value;
                OnPropertyChanged("HolidayType");
            }
        }
        [DataMember]
        public string CalenderWeek
        {
            get { return calenderWeek; }
            set
            {
                calenderWeek = value;
                OnPropertyChanged("CalenderWeek");
            }
        }
        //end[GEOS2-9220][gulab lakade][12 08 2025]
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
        #region Constructor
        public ERMCompanyHoliday()
        {

        }
        #endregion
    }
}
