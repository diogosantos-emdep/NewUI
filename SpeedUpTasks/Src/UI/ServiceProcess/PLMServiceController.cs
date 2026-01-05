using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.ServiceProcess
{
    public class PLMServiceController : IPLMService
    {
        string serviceAddress = "";
        ServiceBehaviorAttribute serviceb;
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<IPLMService> factory = null;

        public PLMServiceController(string serviceUrl)
        {
            serviceAddress = "http://" + serviceUrl + "/PLMService.svc";

            BasicHttpBinding b = new BasicHttpBinding(BasicHttpSecurityMode.None);
            b.AllowCookies = false;
            b.OpenTimeout = new TimeSpan(4, 1, 1);
            b.CloseTimeout = new TimeSpan(4, 1, 1);

            b.MaxBufferSize = int.MaxValue;
            b.MaxReceivedMessageSize = int.MaxValue;
            b.MessageEncoding = WSMessageEncoding.Text;
            b.MaxBufferPoolSize = int.MaxValue;
            b.BypassProxyOnLocal = false;
            b.ReceiveTimeout = new TimeSpan(0, 20, 0);
            b.SendTimeout = new TimeSpan(4, 1, 1);
            b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
            b.ReaderQuotas.MaxDepth = int.MaxValue;
            b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            b.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            b.ReaderQuotas.MaxArrayLength = int.MaxValue;

            binding = b;
            ServiceBehaviorAttribute sb = new ServiceBehaviorAttribute();
            sb.MaxItemsInObjectGraph = 2147483647;
            serviceb = sb;

            try
            {
                endPointAddress = new EndpointAddress(serviceAddress);
            }
            catch (Exception)
            {
            }
        }

        public List<BasePrice> GetBasePricesByYear()
        {
            try
            {
                factory = new ChannelFactory<IPLMService>(binding, endPointAddress);
                IPLMService channel = factory.CreateChannel();
                return channel.GetBasePricesByYear();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }


        public bool IsDeletedBasePriceList(UInt64 idBasePriceList)
        {
            try
            {
                factory = new ChannelFactory<IPLMService>(binding, endPointAddress);
                IPLMService channel = factory.CreateChannel();
                return channel.IsDeletedBasePriceList(idBasePriceList);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }
    }
}
