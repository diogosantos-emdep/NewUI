using Emdep.Geos.Data.Common.ERM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TimeTrackingResult
    {
        ErrorMessage error;
        List<TimetrackingAPI> timeTracking;

        public ErrorMessage Error
        {
            get
            {
                return error;
            }

            set
            {
                error = value;
            }
        }

        public List<TimetrackingAPI> TimeTracking
        {
            get
            {
                return timeTracking;
            }

            set
            {
                timeTracking = value;
            }
        }
    }
}
