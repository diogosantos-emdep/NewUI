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
namespace GeosAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        string MainConn = string.Empty;

        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Targets([FromUri]string Currency = "EUR", [FromUri]string Year = "")
        {
           MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            EmployeesModel employeesModel = new EmployeesModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            try
            {

                Vallidate validate = new Vallidate();
                error = validate.Validate(null, null, null, Currency, MainConn, "SalesQuota", Year);

                if (!string.IsNullOrEmpty(error.code))
                {

                    errorModel.success = false;
                    errorModel.error = error;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
                }


                QuotaManager quotaManager = new QuotaManager(MainConn);

                employeesModel.success = true;
                employeesModel.EmployeeTargets = quotaManager.GetSalesQuotas(string.IsNullOrEmpty(Currency) ? "EUR" : Currency,Year);

                return Request.CreateResponse(HttpStatusCode.OK, employeesModel);

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