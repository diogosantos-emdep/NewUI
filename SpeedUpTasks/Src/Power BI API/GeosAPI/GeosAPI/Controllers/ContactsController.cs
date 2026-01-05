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
    public class ContactsController:ApiController
    {
        string MainConn = string.Empty;

       [HttpGet]
       public HttpResponseMessage List([FromUri]string Plants = "0")
       {
            MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            ContactModel contactModel = new ContactModel();
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


                ContactManager contactManager = new ContactManager(MainConn);

                contactModel.success = true;
                contactModel.Contacts = contactManager.GetContacts(string.IsNullOrEmpty(Plants) ? "0" : Plants.ToUpper());

                return Request.CreateResponse(HttpStatusCode.OK, contactModel);

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