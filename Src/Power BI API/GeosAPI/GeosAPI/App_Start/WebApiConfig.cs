using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GeosAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ListApi",
                routeTemplate: "api/Sales/{controller}/List"
            );
            config.Routes.MapHttpRoute(
                name: "TargetsApi",
                routeTemplate: "api/Sales/{controller}/Targets"
            );
            config.Routes.MapHttpRoute(
                name: "ListActionApi",
                routeTemplate: "api/{controller}/{action}/List"
            );

           // config.Routes.MapHttpRoute(
           //    name: "TrackingApi",
           //    routeTemplate: "api/{controller}/{action}/Trackings"
           //);

            config.Filters.Add(new BasicAuthenticationAttribute());

            config.Filters.Add(new RequireHttpsAttribute());

           
        }
    }
}
