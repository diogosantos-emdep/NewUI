using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeosRestService.Response;
namespace GeosRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGeosService" in both code and config file together.
    [ServiceContract]
    public interface IAPI
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Activities/List?Plants={Plants}&FromDate={FromDate}&ToDate={ToDate}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ActivityResponse GetActivities(string FromDate, string ToDate,string Plants);

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Opportunities/List?Plants={Plants}&Currency={Currency}&FromDate={FromDate}&ToDate={ToDate}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        OfferResponse GetOffers(string FromDate, string ToDate, string Plants,string Currency);

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Orders/List?Plants={Plants}&Currency={Currency}&FromDate={FromDate}&ToDate={ToDate}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        OrderResponse GetOrders(string FromDate, string ToDate, string Plants, string Currency);

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Customers/List?Plants={Plants}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CustomerResponse GetCustomers(string Plants);

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Contacts/List?Plants={Plants}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ContactResponse GetContacts(string Plants);

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Currencies/List", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CurrencyResponse GetCurrencies();

        [OperationContract]
        [WebInvoke(UriTemplate = "Sales/Currencies/Rates?Source={Source}&FromDate={FromDate}&ToDate={ToDate}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CurrencyRateResponse GetCurrenciesRates(string FromDate,string ToDate,string Source);


        [OperationContract]
        [WebInvoke(UriTemplate = "GetData?value1={value1}&value2={value2}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Test GetData(string value1, string value2);
    }
}
