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
    public class CurrenciesController:ApiController
    {
        string MainConn = string.Empty;
        string LoginConn = string.Empty;
        [HttpGet]
        public HttpResponseMessage List([FromUri]string Plants = "0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            CurrencyModel currencyModel = new CurrencyModel();
            try
            {
                CurrencyManager currencyManager = new CurrencyManager(MainConn);

                currencyModel.success = true;
                currencyModel.Currencies = currencyManager.GetCurrencies();

                return Request.CreateResponse(HttpStatusCode.OK, currencyModel);

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

        [HttpGet]
        [Route("api/Sales/Currencies/Rates")]
        public HttpResponseMessage Rates([FromUri]string FromDate = "", [FromUri]string ToDate = "", [FromUri]string Source = "")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            LoginConn = ConfigurationManager.ConnectionStrings["LoginContext"].ConnectionString;
            CurrencyRateModel currencyRateModel = new CurrencyRateModel();

            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            
            try
            {
                Vallidate validate = new Vallidate();
                error = validate.Validate(FromDate, ToDate, null, string.IsNullOrEmpty(Source) ? "" : Source.ToUpper(), MainConn, "CurrenciesRates");

                if (!string.IsNullOrEmpty(error.code))
                {

                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }

                CurrencyManager currencyManager = new CurrencyManager(LoginConn);
                currencyRateModel.success = true;
                currencyRateModel.CurrenciesRate = currencyManager.GetCurrencyRates(Convert.ToDateTime(FromDate),
                    Convert.ToDateTime(ToDate), string.IsNullOrEmpty(Source) ? "0" : Source);

                return Request.CreateResponse(HttpStatusCode.OK, currencyRateModel);

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