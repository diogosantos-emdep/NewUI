using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common;
using System.Configuration;
using System.ServiceModel.Web;
using System.Data;
using Emdep.Geos.Data.Common.File;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICrmRestService" in both code and config file together.
    [ServiceContract]
    public interface ICrmRestService
    {
        /// <summary>
        /// Get Activity Details
        /// </summary>
        /// <param name="objActivityParams"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<ActivityGrid> GetActivity(ActivityParams objActivityParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<CompanyGrid> GetCustomersBySalesOwnerId(CompanyParams companyParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails(TimelineParams timelineParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<PeopleDetails> GetPeoples();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<OrderGrid> GetOrderGridDetails(OrderParams orderParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2031(TimelineParams timelineParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2033(TimelineParams timelineParams);

        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetOrderGridDetails_V2037 instead.")]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<OrderGrid> GetOrderGridDetails_V2035(OrderParams orderParams);


        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetTimelineGridDetails_V2037 instead.")]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2036(TimelineParams timelineParams);


        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetTimelineGridDetails_V2040 instead.")]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2037(TimelineParams timelineParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        
        List<OrderGrid> GetOrderGridDetails_V2037(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        Tuple<string, byte[]> GetOfferEngAnalysisAttachments(OfferParams offerParams);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare,ResponseFormat = WebMessageFormat.Json)]
        List<GeosProviderDetail> GetGeosProviderDetail();


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2040(TimelineParams timelineParams);


       

    }
}
