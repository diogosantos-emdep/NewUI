using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;
using Emdep.Geos.UI.Adapters.Logging;
using System.Windows;
using System.Reflection;
using Emdep.Geos.UI.Common;
using System.IO;

namespace Workbench
{
    public class Bootstrapper : Prism.Unity.UnityBootstrapper
    {
        protected override ILoggerFacade CreateLogger()
        {

            try
            {
                string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;
                if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
                {
                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\"+ GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                }
                FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);

                ILoggerFacade logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);


                logger.Log("Create Logger is created", category: Category.Info, priority: Priority.Low);

                return logger;
            }
            catch (Exception)
            {


            }



            return base.CreateLogger();
        }
    }
}
