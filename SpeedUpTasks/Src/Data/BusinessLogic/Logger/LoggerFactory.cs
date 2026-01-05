using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.BusinessLogic.Logging
{
    public class LoggerFactory
    {
        public static ILoggerFacade CreateLogger(LogType logtype, params object[] parameter)
        {
            if (parameter.Length > 0)
            {
                if (logtype == LogType.Log4Net)
                {
                    if (parameter[0].GetType().Name == "FileInfo")
                    {
                        return new CustomTextLoggerAdapter((FileInfo)parameter[0]);
                    }
                }
                else if (logtype == LogType.Window)
                {
                }
            }

            return null;
        }
    }
}
