using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;
using GeosAPI.Models;
using BusinessLogic;
using System.Configuration;

namespace GeosAPI
{
    public class BasicAuthenticationAttribute:AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null)
            {
                ErrorModel errorModel = new ErrorModel();
                errorModel.success = false;
                Error err = new Error();
                err.code = "401";
                err.info = "Authentication Fail";
                errorModel.error = err;
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, errorModel);
               
            }
            else
            {
                string AuthenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(AuthenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];
                
                if (Authentication.CheckAuthentication(username, password, ConfigurationManager.ConnectionStrings["LoginContext"].ConnectionString))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username),null);

                }
                else
                {
                    ErrorModel errorModel = new ErrorModel();
                    errorModel.success = false;
                    Error err = new Error();
                    err.code = "401";
                    err.info = "Authentication Fail";
                    errorModel.error = err;
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, errorModel);
                }
            }            
        }
    }
}