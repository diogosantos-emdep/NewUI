using ActivityReminder.Classes;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ActivityReminderServices
{
    static class Program
    {
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
                    new ActivityReminderServices()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Program Main() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Program Main() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Program Main() - Exception - {0}", ex.Message));
            }
        }
    }
}
