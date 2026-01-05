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
        TimeSpan enjoyedHours;
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
    }
}
