using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class HolidayDaywiseDetail
    {
        DateTime holidayDate;
        TimeSpan holidayHours;
        TimeSpan startTime;
        TimeSpan endTime;
        sbyte isAllDayEvent;
        Int32 idCompany;
        public DateTime HolidayDate
        {
            get
            {
                return holidayDate;
            }

            set
            {
                holidayDate = value;

            }
        }

        public Int32 IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;

            }
        }

        public TimeSpan HolidayHours
        {
            get
            {
                return holidayHours;
            }

            set
            {
                holidayHours = value;

            }
        }

        public sbyte IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
              
            }
        }




        public TimeSpan StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;

            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;

            }
        }
    }
}
