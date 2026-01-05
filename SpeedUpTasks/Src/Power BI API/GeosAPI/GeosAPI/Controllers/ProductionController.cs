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
    public class ProductionController : ApiController
    {
        string MainConn = string.Empty;
        [ActionName("CI")]
        [HttpGet]
        public HttpResponseMessage List([FromUri] string FromDate = "", [FromUri] string ToDate = "", [FromUri]string Plants = "0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            ProductionModel productionModel = new ProductionModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "ProductionOffer");

                if (!string.IsNullOrEmpty(error.code))
                {
                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                OpportunityManager offerManager = new OpportunityManager(MainConn);

                productionModel.success = true;
                productionModel.CI = offerManager.GetOfferProductionData(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, productionModel);

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

        [Route("api/Production/Modules/Trackings")]
        [ActionName("MODULES")]
        [HttpGet]
        //[Route("api/Production/Modules/Trackings")]
        public HttpResponseMessage Trackings([FromUri] string FromDate = "", [FromUri] string ToDate = "", [FromUri]string Plants = "0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            ModuleModel moduleModel = new ModuleModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper()
                    , null, MainConn, "ProductionOffer");

                if (!string.IsNullOrEmpty(error.code))
                {
                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                OpportunityManager offerManager = new OpportunityManager(MainConn);

                moduleModel.success = true;
              
                moduleModel.Modules = offerManager.GetCounterPartTrackingData(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, moduleModel);

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