using Emdep.Geos.Services.Contracts;
using GeosPOAnalyzerService.Logger;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GeosPOAnalyzerService
{
    static class Program
    {
        //[rdixit][GEOS2-5867][03.07.2024]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new GeosPOAnalyzer()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log(string.Format("[ERROR] Program Main() - FaultException - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("[ERROR] Program Main() - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("[ERROR] Program Main() - Exception - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
    }
}
