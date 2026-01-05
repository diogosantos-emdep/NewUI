using InboxService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace InboxService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOTMInboxService" in both code and config file together.
    [ServiceContract]
    public interface IOTMInboxService
    { 
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        [OperationContract]
        bool AddEmails(email emailDetails);

    }
}
