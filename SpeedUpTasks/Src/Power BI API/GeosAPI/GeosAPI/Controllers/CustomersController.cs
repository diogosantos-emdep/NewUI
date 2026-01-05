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
    public class CustomersController:ApiController
    {
        string MainConn = string.Empty;

        [HttpGet]
        [Route("api/Sales/Customers/List")]
        public HttpResponseMessage List([FromUri]string Plants = "0")
        {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            CustomerModel customerModel = new CustomerModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(null, null, string.IsNullOrEmpty(Plants) ? "" : Plants.ToUpper(), null, MainConn, "Customer");

                if (!string.IsNullOrEmpty(error.code))
                {

                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                CustomerManager customerManager = new CustomerManager(MainConn);

                customerModel.success = true;
                customerModel.Customers = customerManager.GetCustomers(string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, customerModel);

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
        [Route("api/Sales/Customers/Targets")]
        public HttpResponseMessage Targets([FromUri]string Currency = "EUR", [FromUri]string Year="")
        {
            string FromDate = Year + "-" + "1" + "-" + "1";
            string ToDate = Year + "-" + "12" + "-" + "31";
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            CustomerTargetModel customerTargetModel = new CustomerTargetModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(null, null, null, Currency, MainConn, "CustomerTargets", Year);

                if (!string.IsNullOrEmpty(error.code))
                {

                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                QuotaManager quotaManager = new QuotaManager(MainConn);

                customerTargetModel.success = true;
                customerTargetModel.CustomerTargets = quotaManager.GetCustomerTarget(string.IsNullOrEmpty(Currency) ? "EUR" : Currency, FromDate, ToDate);

                return Request.CreateResponse(HttpStatusCode.OK, customerTargetModel);

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