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
    public class PlantsController : ApiController
    {
        string MainConn = string.Empty;

        // GET api/<controller>

        [HttpGet]
        public HttpResponseMessage Targets([FromUri]string Currency = "EUR", [FromUri]string Year = "")
        {
            string FromDate = Year + "-" + "1" + "-" + "1";
            string ToDate = Year + "-" + "12" + "-" + "31";
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            PlantsModel plantsModel = new PlantsModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(null, null, null, Currency, MainConn, "PlantQuota", Year);

                if (!string.IsNullOrEmpty(error.code))
                {

                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                QuotaManager quotaManager = new QuotaManager(MainConn);

                plantsModel.success = true;
                plantsModel.PlantTargets = quotaManager.GetPlantTarget(string.IsNullOrEmpty(Currency) ? "EUR" : Currency,FromDate,ToDate);

                return Request.CreateResponse(HttpStatusCode.OK, plantsModel);

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