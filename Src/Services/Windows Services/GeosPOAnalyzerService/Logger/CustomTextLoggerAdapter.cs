using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using Prism.Logging;

namespace GeosPOAnalyzerService.Logger
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
