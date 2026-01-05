using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using BusinessLogic;
using GeosAPI.Models;
namespace GeosAPI.Controllers
{
    public class EngineeringController : ApiController
    {
        string MainConn = string.Empty;

        [HttpGet]
        [ActionName("Analysis")]
        public HttpResponseMessage List([FromUri] string FromDate = "", [FromUri] string ToDate = "", [FromUri]string Plants = "0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            EngineeringModel engineeringModel = new EngineeringModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "EngineeringAnalysis");

                if (!string.IsNullOrEmpty(error.code))
                {
                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                EngineeringAnalysisManager engineeringAnalysisManager = new EngineeringAnalysisManager(MainConn);

                engineeringModel.success = true;
                engineeringModel.EngineeringAnalysis = engineeringAnalysisManager.GetEngineeringAnalysisDetails(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate)
                    , string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, engineeringModel);

            }
            catch (Exception ex)
            {
                errorModel.success = false;

                error.code = "500";
                error.info = ex.HelpLink + " - " + ex.Message + " " + ex.InnerException +
                       " [Technical Supoort:" + ConfigurationManager.AppSettings["Email"] + "]";
                errorModel.error = error;
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
            }

        }
    }
}