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
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetOrderGridDetails_V2120 instead.")]
        List<OrderGrid> GetOrderGridDetails_V2037(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        Tuple<string, byte[]> GetOfferEngAnalysisAttachments(OfferParams offerParams);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        List<GeosProviderDetail> GetGeosProviderDetail();


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetTimelineGridDetails_V2120 instead.")]
        List<TimelineGrid> GetTimelineGridDetails_V2040(TimelineParams timelineParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<OrderGrid> GetOrderGridDetails_V2110(OrderParams orderParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetOrderGridDetails_V2200 instead.")]
        List<OrderGrid> GetOrderGridDetails_V2120(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetTimelineGridDetails_V2200 instead.")]
        List<TimelineGrid> GetTimelineGridDetails_V2120(TimelineParams timelineParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<OrderGrid> GetOrderGridDetails_V2200(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetTimelineGridDetails_V2380 instead.")]
        List<TimelineGrid> GetTimelineGridDetails_V2200(TimelineParams timelineParams);

        //[pmisal][GEOS2-4323][10.04.2023]
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetTimelineGridDetails_V2390 instead.")]
        List<TimelineGrid> GetTimelineGridDetails_V2380(TimelineParams timelineParams);

        //[pmisal][GEOS2-4323][10.04.2023]
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]        
        List<OrderGrid> GetOrderGridDetails_V2380(OrderParams orderParams);

        //[GEOS2-4284][rdixit][09.05.2023]
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2390(TimelineParams timelineParams);

        //[GEOS2-][rdixit][31.05.2023]
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOrderGridDetails_V2420 instead.")]
        List<OrderGrid> GetOrderGridDetails_V2390(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TimelineGrid> GetTimelineGridDetails_V2420(TimelineParams timelineParams);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetOrderGridDetails_V2670 instead.")]
        List<OrderGrid> GetOrderGridDetails_V2420(OrderParams orderParams);


        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<OrderGrid> GetOrderGridDetails_V2670(OrderParams orderParams);
    }
}
