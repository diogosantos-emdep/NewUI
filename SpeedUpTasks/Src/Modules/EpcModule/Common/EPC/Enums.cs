using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public enum ProjectStatus
    {
        Analysis,
        Estimation,
        KickOff,
        Delivery,
        Validation,
        Close,
    }

    public enum TaskPriority
    {
        Low = 1,
        Normal = 2,
        Medium = 3,
        High = 4,

    }

    public enum ProjectType
    {
        Strategical = 1,
        Short = 2,
        Long = 3,
    }

    public enum MilestoneMailType
    {
        Achieved = 1,
        Warning = 2,
        Failed = 3,
    }

    public enum ProjectStatusType
    {
        UnderStudy = 0,
        AnalysisReady = 1,
        NewRequest = 2,
        Validated = 3,
        Delivered = 4,
        Closed = 5,
    }

    public enum RoadMapStatus
    {
        Accept = 1,
        Pending = 2,
        Reject = 3,
    }
}
