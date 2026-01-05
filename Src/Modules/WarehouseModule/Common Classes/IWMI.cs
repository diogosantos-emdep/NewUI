using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseCommon.Wmi
{
    interface IWMI
    {
        //IList<string> GetPropertyValues();
        IList<ParallelPort> GetPropertyValues();
    }
}
