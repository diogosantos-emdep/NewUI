using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PLMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PLMService.svc or PLMService.svc.cs at the Solution Explorer and start debugging.
    public class PLMService : IPLMService
    {
        PLMManager mgr = new PLMManager();

        public List<BasePrice> GetBasePricesByYear()
        {
            try
            {
                string PLMConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetBasePricesByYear(PLMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000100";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeletedBasePriceList(UInt64 idBasePriceList)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedBasePriceList(idBasePriceList, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    }
}
