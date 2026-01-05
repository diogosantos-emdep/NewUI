using BusinessLogic;
using GeosAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GeosAPI.Controllers
{
    public class QualityController : ApiController
    {
        string MainConn = string.Empty;
        [ActionName("ServiceRequests")]
        [HttpGet]
        //public HttpResponseMessage List([FromUri] string FromDate = "", [FromUri] string ToDate = "", [FromUri]string Plants = "0")
            public HttpResponseMessage List()
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            QualityModel qualityModel = new QualityModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                   QualityManager qualityManager = new QualityManager(MainConn);

                qualityModel.success = true;
                qualityModel.ServiceRequests = qualityManager.GetServiceRequestData();

                return Request.CreateResponse(HttpStatusCode.OK, qualityModel);

            }
            catch (Exception ex)
            {
                errorModel.success = false;

                error.code = "500";
                error.info = ex.HelpLink + " - " + ex.Message + " " + ex.InnerException +
                       " [Technical Support:" + ConfigurationManager.AppSettings["Email"] + "]";
                errorModel.error = error;
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
            }

        }
    }
}