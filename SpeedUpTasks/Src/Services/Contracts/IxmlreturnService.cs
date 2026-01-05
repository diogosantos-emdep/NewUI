using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IxmlreturnService" in both code and config file together.
    [ServiceContract]
    public interface IxmlreturnService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetOffersWithoutPurchaseOrderReturnListDatatable1")]
        DataSet GetOffersWithoutPurchaseOrderReturnListDatatable1();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
         RequestFormat = WebMessageFormat.Json, UriTemplate = "GetOffersWithoutPurchaseOrder")]
        string GetOffersWithoutPurchaseOrder();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
       RequestFormat = WebMessageFormat.Json, UriTemplate = "GetOffersWithoutPurchaseOrderList")]
        string GetOffersWithoutPurchaseOrderList();


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
     RequestFormat = WebMessageFormat.Json, UriTemplate = "GetoptionsByOfferList")]
        string GetoptionsByOfferList();
    }
}
