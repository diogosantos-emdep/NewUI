using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.ServiceProcess
{
    public enum ServiceType
    {
        Online, Offline
    }

    public static class ServiceFactory
    {
        public static IWorkbenchStartUp WorkbenchService(ServiceType type, params object[] param)
        {
            IWorkbenchStartUp workbenchStartUp=null;
            return workbenchStartUp;
        }

        public static IEpcService EpcService(ServiceType type, params object[] param)
        {
            IEpcService epcService = null;
            return epcService;
        }

        public static IGlpiService GlpiService(ServiceType type, params object[] param)
        {
            IGlpiService glpiService = null;
            return glpiService;
        }

        public static IGeosRepositoryService GeosRepositoryService(ServiceType type, params object[] param)
        {
            IGeosRepositoryService geosRepositoryService = null;
            return geosRepositoryService;
        }

        public static IHarnessPartService HarnessPartService(ServiceType type, params object[] param)
        {
            IHarnessPartService harnessPartService = null;
            return harnessPartService;
        }
    }
}
