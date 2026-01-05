using log4net;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emdep.Geos.FileReplicator.Utility
{
    public class Log4NetLogger // : ILoggerFacade
    {
        #region Fields

        public static Prism.Logging.ILoggerFacade Logger { get; set; }

        #endregion
    }
}