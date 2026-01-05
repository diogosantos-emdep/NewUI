using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPLMService" in both code and config file together.
    [ServiceContract]
    public interface IPLMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePrice> GetBasePricesByYear();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedBasePriceList(UInt64 idBasePriceList);
    }
}
