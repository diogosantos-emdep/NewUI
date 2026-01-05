using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    public class LeavedHoursDetail
    {
        TimeSpan ignoredHours;
        TimeSpan ignoredHoursTillYesterday;
        TimeSpan ignoredHoursFromToday;
        TimeSpan enjoyedHours;
        TimeSpan enjoyedHoursTillYesterday;
        TimeSpan enjoyedHoursFromToday;
        Int32 idLeave;


        public Int32 IdLeave
        {
            get
            {
                return idLeave;
            }

            set
            {
                idLeave = value;

            }
        }

        [Obsolete("Use new Properties IgnoredHoursTillYesterday,IgnoredHoursFromToday")]
        public TimeSpan IgnoredHours
        {
            get
            {
                return ignoredHours;
            }

            set
            {
                ignoredHours = value;

            }
        }
        public TimeSpan IgnoredHoursTillYesterday
        {
            get
            {
                return ignoredHoursTillYesterday;
            }

            set
            {
                ignoredHoursTillYesterday = value;

            }
        }
        public TimeSpan IgnoredHoursFromToday
        {
            get
            {
                return ignoredHoursFromToday;
            }

            set
            {
                ignoredHoursFromToday = value;

            }
        }

        [Obsolete("Use new Properties EnjoyedHoursTillYesterday,EnjoyedHoursFromToday")]
        public TimeSpan EnjoyedHours
        {
            get
            {
                return enjoyedHours;
            }

            set
            {
                enjoyedHours = value;

            }
        }

        public TimeSpan EnjoyedHoursTillYesterday
        {
            get
            {
                return enjoyedHoursTillYesterday;
            }

            set
            {
                enjoyedHoursTillYesterday = value;

            }
        }

        public TimeSpan EnjoyedHoursFromToday
        {
            get
            {
                return enjoyedHoursFromToday;
            }

            set
            {
                enjoyedHoursFromToday = value;

            }
        }
    }
}
