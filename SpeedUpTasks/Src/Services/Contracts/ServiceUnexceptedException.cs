using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Contracts
{

    public enum ServiceExceptionType
    {
        FaultException,         // Service and bussiness logic method exception will handle
        CommunicationException, //localhost in address and port empty
        TimeoutException,       //service time out exception handle
        SecurityNegotiationException,//10.0.9.3 in address
        ProtocolException,      //5599 in address
        UriFormatException,     //in port localhost and address,port empty 
        MessageSecurityException,////10.0.9
        EndpointNotFoundException,//LAN Disconnect and 10.0.9.4
        UnknownException,        //other exception
        ObjectDisposedException //object disposed exception
    }


    public class ServiceUnexceptedException : Exception
    {

        public ServiceExceptionType ExceptionType
        {
            get;
            set;
        }

        public ServiceUnexceptedException()
        {
        }

        public ServiceUnexceptedException(string message)
            : base(message)
        {
        }

        public ServiceUnexceptedException(ServiceExceptionType exceptiontype, string message, Exception inner, string source)
            : base(message, inner)
        {
            this.ExceptionType = exceptiontype;
        }

    }
}
