using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Runtime.Serialization.Json;

namespace GeosRestService.Response
{
    [DataContract]
    public class CustomException:FaultException
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public error error { get; set; }
        //public CustomException(bool success, string code, string info)
        //{
        //    this.Success = success;
        //    this.error.Code = code;
        //    this.error.Info = info;
        //}
    }
    public class error
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Info { get; set; }
    }
    public class MyFactory : WebServiceHostFactory
    {
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var sh = new ServiceHost(typeof(RestAuthorizationManager), baseAddresses);
            sh.Description.Endpoints[0].Behaviors.Add(new WebHttpBehaviorEx());

            return sh;
        }
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return base.CreateServiceHost(serviceType, baseAddresses);
        }
    }
    public class WebHttpBehaviorEx : WebHttpBehavior
    {
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // clear default erro handlers.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            // add our own error handler.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new ErrorHandlerEx());
        }
    }
    public class ErrorHandlerEx : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version,ref Message fault)
        {
            //, ref Message fault
            
            if (error is FaultException)
            {
                // extract the our FaultContract object from the exception object.
                var detail = error.GetType().GetProperty("Detail").GetGetMethod().Invoke(error, null);
                // create a fault message containing our FaultContract object
                fault = Message.CreateMessage(version, "", detail, new DataContractJsonSerializer(detail.GetType()));
                // tell WCF to use JSON encoding rather than default XML
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

                // return custom error code.
                var rmp = new HttpResponseMessageProperty();
                rmp.StatusCode = System.Net.HttpStatusCode.BadRequest;
                // put appropraite description here..
                rmp.StatusDescription = "See fault object for more information.";
                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
            else
            {
                fault = Message.CreateMessage(version, "", "An non-fault exception is occured.", new DataContractJsonSerializer(typeof(string)));
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);
                // return custom error code.
                var rmp = new HttpResponseMessageProperty();
                rmp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                // put appropraite description here..
                rmp.StatusDescription = "Uknown exception...";
                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
        }
    }
}