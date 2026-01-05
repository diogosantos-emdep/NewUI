using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Entities;
using BusinessLogic;
using GeosAPI.Models;
namespace GeosAPI.Controllers
{
    public class OpportunitiesController:ApiController
    {
        string MainConn = string.Empty;

        public HttpResponseMessage Get([FromUri] string FromDate = "", [FromUri] string ToDate = "", [FromUri]string Plants = "0", [FromUri]string Currency = "EUR")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            OfferModel offerModel = new OfferModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {
                
                Vallidate validate = new Vallidate();
                error = validate.Validate(FromDate, ToDate, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper(), string.IsNullOrEmpty(Currency) ? "" : Currency.ToUpper(), MainConn, "Offer");
                
                if (!string.IsNullOrEmpty(error.code))
                {
                    
                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }

                
                OpportunityManager offerManager = new OpportunityManager(MainConn);

                offerModel.success = true;
                offerModel.Opportunities = offerManager.GetOffers(Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate),
                    string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper(), string.IsNullOrEmpty(Currency) ? "EUR" : Currency.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, offerModel);

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