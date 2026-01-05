using log4net;
using log4net.Config;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.FileReplicator.Utility
{
    public class CustomTextLoggerAdapter : ILoggerFacade
    {
        private readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CustomTextLoggerAdapter(FileInfo fileinfo)
        {
            log4net.Config.XmlConfigurator.Configure(fileinfo);
        }

        void ILoggerFacade.Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    Logger.Debug(message);
                    break;
                case Category.Warn:
                    Logger.Warn(message);
                    break;
                case Category.Info:
                    Logger.Info(message);
                    break;
                case Category.Exception:
                    Logger.Fatal(message);
                    break;
            }
        }

    }
}
