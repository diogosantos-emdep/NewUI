using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Entities;
using BusinessLogic;
using GeosAPI.Models;
using System.Web.Mvc;
using System.Threading;
namespace GeosAPI.Controllers
{
    public class ActivitiesController : ApiController
    {
        string MainConn = string.Empty;
        // GET api/<controller>
        //[RequireHttps]
        //[BasicAuthentication]
        public HttpResponseMessage Get([FromUri] string FromDate="", [FromUri] string ToDate="", [FromUri]string Plants="0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            
            Error error = new Error();
            Vallidate validate = new Vallidate();
            error = validate.Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper(), null, MainConn, "Activity");
            if (!string.IsNullOrEmpty(error.code))
            {
                ErrorModel errorModel = new ErrorModel();
                errorModel.success = false;
                errorModel.error = error;
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
            }
                                            
            
            ActivityModel activityModel = new ActivityModel();
            
            ActivityManager activityManager = new ActivityManager(MainConn);
            activityModel.Activities = activityManager.GetActivities(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Plants);
            activityModel.success = true;
            return Request.CreateResponse(HttpStatusCode.OK, activityModel);
            
        }

       
    }
}