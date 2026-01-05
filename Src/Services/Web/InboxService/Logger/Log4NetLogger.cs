using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InboxService.Logger
{
    public class Log4NetLogger
    {
        public static Prism.Logging.ILoggerFacade Logger { get; set; }
    }
}