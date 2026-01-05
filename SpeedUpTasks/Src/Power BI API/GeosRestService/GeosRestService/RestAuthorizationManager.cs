using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using BusinessLogic;
using System.Configuration;
using System.ServiceModel.Channels;
using Entities;
using System.Runtime.Serialization.Json;
using GeosRestService.Response;
using System.Web.ModelBinding;
using System.Runtime.Serialization;
using System.ServiceModel.Dispatcher;

namespace GeosRestService
{
    
    public class RestAuthorizationManager:ServiceAuthorizationManager
    {
        string LoginConn;
        /// <summary>  
        /// Method source sample taken from here: http://bit.ly/1hUa1LR  
        /// </summary>  
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                LoginConn = ConfigurationManager.ConnectionStrings["LoginContext"].ConnectionString;
                //Extract the Authorization header, and parse out the credentials converting the Base64 string:  
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if ((authHeader != null) && (authHeader != string.Empty))
                {
                    var svcCredentials = System.Text.ASCIIEncoding.ASCII
                        .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                        .Split(':');
                    var user = new
                    {
                        Name = svcCredentials[0],
                        Password = svcCredentials[1]
                    };
                    if (Authentication.CheckAuthentication(user.Name, user.Password, LoginConn))
                    {
                        //User is authrized and originating call will proceed  
                        return true;
                    }
                    else
                    {
                        //throw new FaultException("Authentication Fail", new FaultCode("Authorization"));
                        throw new FaultException(new FaultReason(new FaultReasonText("Authentication fail", "en-US")), new FaultCode("Authorization"));
                        
                        
                    }
                }
                else
                {
                    //No authorization header was provided, so challenge the client to provide before proceeding:  
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"GeosRestService\"");

                    throw new FaultException(new FaultReason(new FaultReasonText("Authentication fail", "en-US")), new FaultCode("Authorization"));
                    
                }
            }
            catch (Exception ex)
            {
                //throw new FaultException(new FaultReason(new FaultReasonText("Authentication fail", "en-US")), new FaultCode("Authorization"));
                throw new FaultException("Authentication Fail", new FaultCode("Authorization"));
                
            }
            
        }
 
       
    }

    
    public class CustomFault:FaultException
    {
        
        FaultReason reason = new FaultReason(new FaultReasonText("Authentication fail", "en-US"));
        
    }
    [DataContract]
    public class ErrorMessage
    {
        public ErrorMessage(Exception error)
        {
            Message = error.Message;
            StackTrace = error.StackTrace;
            Exception = error.GetType().Name;
        }

        [DataMember(Name = "stacktrace")]
        public string StackTrace { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "exception-name")]
        public string Exception { get; set; }
    }
}