using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class WorkbenchManager
    {
        /// <summary>
        /// Function to get current version from table GeosWorkbenchVersions
        /// </summary>
        public GeosWorkbenchVersions GetCurrentVersion()
        {
            GeosWorkbenchVersions GeosWorkbenchVersion;
            using (var db = new WorkbenchContext())
            {
                GeosWorkbenchVersion = db.GeosWorkbenchVersions.OrderByDescending(p => p.IdGeosWorkbenchVersion).First(); // p.VersionNumber == db.GeosWorkbenchVersions.Max(r => r.VersionNumber)).SingleOrDefault();
            }
            return GeosWorkbenchVersion;
        }
    }
}
