using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Data.Common.SynchronizationClass;
//using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.ServiceProcess
{
    public class SCMServiceController : ISCMService
    {
        string serviceAddress = "";
        ServiceBehaviorAttribute serviceb;
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<ISCMService> factory = null;
        public SCMServiceController(string serviceUrl)
        {
            if (serviceUrl.Contains("https://") || serviceUrl.Contains("http://"))
            {
                serviceAddress = serviceUrl + "/SCMService.svc";
            }
            else
            {
                serviceAddress = "http://" + serviceUrl + "/SCMService.svc";
            }

            // serviceAddress = serviceUrl + "/CrmService.svc";
            if (serviceAddress.Contains("https://"))
            {

                BasicHttpBinding b = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
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
                b.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding = b;
                ServiceBehaviorAttribute sb = new ServiceBehaviorAttribute();
                sb.MaxItemsInObjectGraph = 2147483647;
                serviceb = sb;

                try
                {
                    endPointAddress = new EndpointAddress(serviceAddress);
                    //if (serviceAddress.Contains("9.7") || serviceAddress.Contains("9.202"))
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                }
                catch (Exception)
                {
                }
            }
            else if (serviceAddress.Contains("http://"))
            {
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
        }
        public List<Color> GetAllColors(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllColors(language);
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

        public List<Data.Common.SCM.Company> GetAllCompany()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllCompany();
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

        public List<Family> GetAllFamilies(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllFamilies(language);
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

        public List<Subfamily> GetAllSubfamilies(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllSubfamilies(language);
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

        //[rdixit][GEOS2-4399][23.06.2023]
        public List<Gender> GetGender(String language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetGender(language);
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

        //[nsatpute][10-07-2023] SCM - Properties Manager (2/4) GEOS2-4501
        public List<ConnectorProperties> GetConnectorProperties(String language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorProperties(language);
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

        //[Sudhir.Jangra][GEOS2-4502][11/07/2023]
        public bool AddNewValueKey(ValueKey valueKey, string GeosSettingsKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddNewValueKey(valueKey, GeosSettingsKey);
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



        //[Sudhir.Jangra][GEOS2-4502][18/07/2023]
        public List<Language> GetAllLanguages()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLanguages();
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


        //[Sudhir.Jangra][GEOS2-4501][19/07/2023]
        //[nsatpute][GEOS2-4501][21/07/2023]
        public void AddPropertyManager(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                channel.AddPropertyManager(connectorProperty);
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
        //[nsatpute][GEOS2-4501][26/07/2023]
        public void DeletePropertyManager(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                channel.DeletePropertyManager(connectorProperty);
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
        //[nsatpute][GEOS2-4501][21/07/2023]
        public List<ConnectorProperties> GetPropertyManager()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManager();
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
        //[Sudhir.Jangra][GEOS2-4502][19/07/2023]
        public bool AddCustomProperty(CustomProperty customProperty)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddCustomProperty(customProperty);
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

        //[Sudhir.Jangra][GEOS2-4502]
        public List<Data.Common.SCM.ValueType> GetValueType()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetValueType();
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

        //[Sudhir.Jangra][GEOS2-4504]
        public List<ValueKey> GetValueKey(string LookupValueKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetValueKey(LookupValueKey);
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

        //[Sudhir.Jangra][GEOS2-4504]
        public bool UpdateValueKey(ValueKey valueKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateValueKey(valueKey);
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

        //[Sudhir.Jangra][GEOS2-4503]
        public CustomProperty GetEditCustomProperty(Int32 Id)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetEditCustomProperty(Id);
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

        //[Sudhir.Jangra][GEOS2-4505]
        public bool UpdateEditCustomProperty(CustomProperty customProperty)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateEditCustomProperty(customProperty);
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

        //[pallavi.jadhav][GEOS2-4562][27/07/2023]
        public List<Data.Common.SCM.ConnectorFamilies> GetConnectorFamilies()
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorFamilies();
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



        //[GEOS2-4563][rupali sarode][31-07-2023]
        public Emdep.Geos.Data.Common.SCM.ConnectorFamilies AddConnectorFamilies_V2420(Emdep.Geos.Data.Common.SCM.ConnectorFamilies ConnectorFamily, List<Subfamily> SubFamiliesList)
        {//Emdep.Geos.Data.Common.SCM.ConnectorFamilies ConnectorFamily
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnectorFamilies_V2420(ConnectorFamily, SubFamiliesList);
                //ConnectorFamily
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
        //[gulab lakade][GEOS2-4506][16 08 2023]
        public List<ValueKey> GetAllLookupKey()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLookupKey();
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

        //[gulab lakade][GEOS2-4506][16 08 2023]
        public List<LookUpValues> GetAllLookUpValuesRecordByIDLookupkey(int IdLookupKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLookUpValuesRecordByIDLookupkey(IdLookupKey);
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
        //[gulab lakade][GEOS2-4506][16 08 2023]
        public bool InsertLookupKey_V2420(LookUpValues valueKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.InsertLookupKey_V2420(valueKey);
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
        //[gulab lakade][GEOS2-4506][16 08 2023]
        public bool UpdateLookupKey_V2420(LookUpValues valueKey)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateLookupKey_V2420(valueKey);
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

        //[gulab lakade][GEOS2-4506][16 08 2023]
        public bool IsDeletedValueList_V2420(int IdLookupValue)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.IsDeletedValueList_V2420(IdLookupValue);
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

        public Emdep.Geos.Data.Common.SCM.ConnectorFamilies UpdateConnectorFamilies_V2420(Emdep.Geos.Data.Common.SCM.ConnectorFamilies ConnectorFamily, Int32 IdFamily, List<Subfamily> SubFamiliesList)
        {//Emdep.Geos.Data.Common.SCM.ConnectorFamilies ConnectorFamily
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorFamilies_V2420(ConnectorFamily, IdFamily, SubFamiliesList);
                //ConnectorFamily
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


        //[Aishwarya Ingale]]
        public List<Subfamily> GetSubFamily(int IdFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamily(IdFamily);
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

        #region V2430    
        public List<ConnectorSubFamily> GetAllSubfamilies_V2430(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllSubfamilies_V2430(language);
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
        public List<ConnectorFamily> GetConnectorFamilies_V2430()
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorFamilies_V2430();
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
        public ConnectorFamily AddConnectorFamilies_V2430(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnectorFamilies_V2430(ConnectorFamily);
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
        public ConnectorFamily UpdateConnectorFamilies_V2430(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorFamilies_V2430(ConnectorFamily);
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
        public List<FamilyImage> GetFamilyImageImagesByIdFamily_V2430(int IdFamily, string FamilyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetFamilyImageImagesByIdFamily_V2430(IdFamily, FamilyName);
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
        public List<ConnectorSubFamily> GetSubFamily_V2430(int IdFamily, string FamilyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamily_V2430(IdFamily, FamilyName);
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

        //[GEOS2-4564][rdixit][05.09.2023]
        public bool UpdateSubFamily_V2430(ConnectorSubFamily subFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateSubFamily_V2430(subFamily);
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

        public ConnectorSubFamily GetSubFamilyDetails(int idSubFamily, string familyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamilyDetails(idSubFamily, familyName);
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
        #endregion
        //Shubham[skadam] GEOS2-4595 SCM - Search results (1/4) 08 09 2023 
        //public List<Connectors> GetAllConnectors(uint? IdCompany, uint? IdSubFamily, uint? IdFamily, uint? IdColor, uint? IdGender, Int32? IdShape)
        public List<Connectors> GetAllConnectors(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors(Connectors);
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
        //Shubham[skadam] GEOS2-4596 SCM - Search results (2/4) 12 09 2023 
        public List<SCMConnectorImage> GetAllConnectorImages(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorImages(Connectors);
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

        //[rdixit][14.09.2023][GEOS2-4602]
        public List<ComponentType> GetAllComponentTypes()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllComponentTypes();
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

        public List<CustomProperty> GetAllCustomeData()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllCustomeData();
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

        public List<Connectors> GetAllConnectors_V2440(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2440(Connectors);
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
        #region [rdixit][GEOS2-4958][20.10.2023]
        public List<ConnectorFamily> GetConnectorFamilies_V2450()
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorFamilies_V2450();
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

        public List<ConnectorSubFamily> GetSubFamily_V2450(int IdFamily, string FamilyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamily_V2450(IdFamily, FamilyName);
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

        public ConnectorSubFamily GetSubFamilyDetails_V2450(int idSubFamily, string familyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamilyDetails_V2450(idSubFamily, familyName);
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

        public List<FamilyImage> GetFamilyImageImagesByIdFamily_V2450(int IdFamily, string FamilyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetFamilyImageImagesByIdFamily_V2450(IdFamily, FamilyName);
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

        public List<Family> GetAllFamilies_V2450(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllFamilies_V2450(language);
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

        public ConnectorFamily AddConnectorFamilies_V2450(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnectorFamilies_V2450(ConnectorFamily);
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

        public bool UpdateSubFamily_V2450(ConnectorSubFamily subFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateSubFamily_V2450(subFamily);
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

        public ConnectorFamily UpdateConnectorFamilies_V2450(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorFamilies_V2450(ConnectorFamily);
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
        #endregion
        //[rdixit][29.11.2023][GEOS2-4955]
        public List<ConnectorProperties> GetPropertyManager_V2460()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManager_V2460();
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

        //[rdixit][29.11.2023][GEOS2-4955]
        public void AddPropertyManager_V2460(List<ConnectorProperties> connectorProperty)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                channel.AddPropertyManager_V2460(connectorProperty);
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

        //[Sudhir.Jangra][GEOS2-4973]
        public List<Family> GetFamiliesForSearchFamilyConfiguration_V2460(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetFamiliesForSearchFamilyConfiguration_V2460(language);
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

        //[rdixit][GEOS2-4951][08.12.2023]
        public List<Connectors> GetAllConnectors_V2460(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2460(Connectors);
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

        //[rdixit][GEOS2-4951][08.12.2023]
        public List<SCMConnectorImage> GetAllConnectorImages_V2460(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorImages_V2460(Connectors);
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

        public List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2460()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConfigurationsForSearchFilters_V2460();
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

        public bool SaveConfigurationsForSearchFilters(List<ConfigurationFamily> ConfigurationList)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.SaveConfigurationsForSearchFilters(ConfigurationList);
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

        //rajashri GEOS2-4956
        public ConnectorFamily GetFamilyDetails_V2470(Int32 idFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetFamilyDetails_V2470(idFamily);
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

        #region [rdixit][GEOS2-5148,5149,5150][29.01.2024]
        public List<ConnectorFamily> GetConnectorFamilies_V2480()
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorFamilies_V2480();
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
        public ConnectorFamily AddConnectorFamilies_V2480(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnectorFamilies_V2480(ConnectorFamily);
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
        public ConnectorFamily UpdateConnectorFamilies_V2480(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorFamilies_V2480(ConnectorFamily);
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
        public List<Family> GetAllFamilies_V2480(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllFamilies_V2480(language);
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

        public List<ConnectorSubFamily> GetAllSubfamilies_V2480(string language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllSubfamilies_V2480(language);
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

        public List<Connectors> GetAllConnectors_V2480(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2480(Connectors);
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
        #endregion
        //rajashri GEOS2-5227
        public List<ConnectorProperties> GetConnectorProperties_V2480(String language)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorProperties_V2480(language);
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
        public List<ValueKey> GetValueKeyOfCustomProperties_V2480(Int32 IdCustomConnectorProperty)
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetValueKeyOfCustomProperties_V2480(IdCustomConnectorProperty);
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
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2480(UInt32 familyId)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManagerByFamily_V2480(familyId);
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
        public List<Connectors> GetAllConnectors_V2490(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2490(Connectors);
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


        //[Sudhir.Jangra][GEOS2-5203]

        public List<Data.Common.Company> GetAuthorizedPlantsByIdUser_V2490(Int32 idUser)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAuthorizedPlantsByIdUser_V2490(idUser);
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


        //[Sudhir.Jangra][GEOS2-5204]

        public List<Samples> GetModifiedSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetModifiedSamplesByIdSite_V2490(startDate, endDate, idSite);
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




        //[Sudhir.Jangra][GEOS2-5205]

        public List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.Get3dConnectorByIdSite_V2490(startDate, endDate, idSite);
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


        //[Sudhir.Jangra][GEOS2-5203]

        public List<Samples> GetNewSamplesByIdSite_V2490(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetNewSamplesByIdSite_V2490(startDate, endDate, idSite);
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


        //[Sudhir.Jangra][GEOS2-5203]

        public List<WorkflowStatus> GetStatusForNewSamples_V2490()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetStatusForNewSamples_V2490();
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
        //[GEOS2-5296][rdixit][29.02.2024]
        public List<ConnectorProperties> GetPropertyManager_V2490()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManager_V2490();
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

        //[GEOS2-5297][rdixit][29.02.2024]
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2490(UInt32 familyId)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManagerByFamily_V2490(familyId);
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

        public List<SimilarColorsByConfiguration> GetSimilarColorsByConfiguration()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSimilarColorsByConfiguration();
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

        public List<ConfigurationFamily> GetConfigurationsForSearchFilters_V2490()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConfigurationsForSearchFilters_V2490();
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

        public bool SaveConfigurationsForSearchFilters_V2490(List<ConfigurationFamily> configurationList)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.SaveConfigurationsForSearchFilters_V2490(configurationList);
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


        public bool AddColorsByConfiguration_V2490(List<Tuple<int, int>> colorsList, int idUser)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddColorsByConfiguration_V2490(colorsList, idUser);
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

        //[GEOS2-5296][rdixit][11.03.2024]
        public List<Data.Common.SCM.Company> GetAllCompany_V2490()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllCompany_V2490();
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

        // [rdixit][GEOS2-5485][13.03.2024]
        public bool SaveConfigurationsForSearchFilters_V2500(List<ConfigurationFamily> configurationList)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.SaveConfigurationsForSearchFilters_V2500(configurationList);
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

        //[pramod.misal][GEOS2-5378][27-03-2024]
        public Connectors GetConnectorProperties_V2500(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorProperties_V2500(Connectors);
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

        //[pramod.misal][GEOS2-5378][28-03-2024]
        public List<ConnectorProperties> GetConnectorCustomPropertiesByFamily_V2500(UInt32 familyId)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorCustomPropertiesByFamily_V2500(familyId);
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

        //[rdixit][29.03.2024][GEOS2-5380]
        public List<SCMConnectorImage> GetAllConnectorImages_V2500(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorImages_V2500(Connectors);
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

        //[pramod.misal][GEOS2-5379][29-03-2024]
        public List<ConnectorReference> GetConnectorReferencesByRef_V2500(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorReferencesByRef_V2500(ConnectorRef);
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


        public List<ConnectorWorkflowStatus> GetAllConnectorStatus()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorStatus();
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

        public List<ConnectorWorkflowTransitions> GetAllWorkflowTransitions()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllWorkflowTransitions();
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

        //[pramod.misal] [GEOS2-5381] [02.04.2024]
        public List<ConnectorComponents> GetConnectorComponentsByRef_V2500(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorComponentsByRef_V2500(ConnectorRef);
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

        //[GEOS2-5383][rdixit][11.04.2024]
        public List<Connectors> GetAllLinkedConnectorByRef(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLinkedConnectorByRef(idConnector);
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

        //[pramod.misal] [GEOS2-5387] [09.04.2024]
        public List<ConnectorAttachements> GetConnectorAttachementsByRef_V2510(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorAttachementsByRef_V2510(ConnectorRef);
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


        //[Sudhir.Jangra][GEOS2-5384]
        public List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2510(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorImagesForImageSection_V2510(Connectors);
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

        //[GEOS2-5382][rdixit][19.04.2024]
        public List<ConnectorLocation> GetConnectorLocationsByRef(string connref)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorLocationsByRef(connref);
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


        //[rdixit][GEOS2-5390][22.04.2024]
        public List<ConnectorLogEntry> GetConnectorLogEntries(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorLogEntries(idConnector);
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

        //[pramod.misal] [GEOS2-5391] [23.04.2024]
        public List<ConnectorLogEntry> GetConnectorCommentsByRef_V2510(Int64 IdConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorCommentsByRef_V2510(IdConnector);
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

        public List<ScmDrawing> GetDrawingsByConnectorRef(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetDrawingsByConnectorRef(idConnector);
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

        public List<ArticlesbyDrawing> GetArticlesByDrawing(uint idDrawing)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetArticlesByDrawing(idDrawing);
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

        //[rdixit][02.05.2024][GEOS2-5476]
        public List<Data.Common.Company> GetAuthorizedPlants(Int32 idUser)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAuthorizedPlants(idUser);
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

        //[pramod.misal][GEOS2-5479][30-04-2024]
        public bool UpdateConnectorStatus_V2510(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorStatus_V2510(connectorSearch);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public bool UpdateConnectorStatus_V2520(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorStatus_V2520(connectorSearch);
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

        //[pramod.misal][15.05.2024][GEOS2-]
        public List<ConnectorAttachements> GetAllConnectorAttachmentTypes()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorAttachmentTypes();
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

        //[pramod.misal] [GEOS2-5477] [16.05.2024]
        public List<ConnectorAttachements> GetConnectorAttachementsByRef_V2520(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorAttachementsByRef_V2520(ConnectorRef);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public List<ConnectorReference> GetConnectorReferencesByRef_V2520(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorReferencesByRef_V2520(ConnectorRef);
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

        //[rdixit][14.05.2024][GEOS2-5477]
        public List<Data.Common.SCM.Company> GetAllCompanyList_V2520()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllCompanyList_V2520();
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

        //[pramod.misal] [GEOS2-5391] [23.04.2024]
        public List<ConnectorLogEntry> GetConnectorCommentsByRef_V2520(Int64 IdConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorCommentsByRef_V2520(IdConnector);
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

        //[rdiixt][22.05.2024][GEOS2-5751]
        public List<ConnectorComponents> GetConnectorComponentsByRef_V2520(String ConnectorRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorComponentsByRef_V2520(ConnectorRef);
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

        public string CheckOtherRefIsValid(string _ref)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.CheckOtherRefIsValid(_ref);
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

        //[rdiixt][27.05.2024][GEOS2-5753]
        public List<Connectors> GetAllLinkedConnectorByRef_V2520(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLinkedConnectorByRef_V2520(idConnector);
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

        //[rdiixt][27.05.2024][GEOS2-5753]
        public List<LinkType> GetAllLinkTypes()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllLinkTypes();
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

        //[rdiixt][27.05.2024][GEOS2-5753]
        public Connectors CheckLinkedRefIsValid(string _ref)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.CheckLinkedRefIsValid(_ref);
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

        //[rushikesh.gaikwad][18.06.2024]
        public List<ScmDrawing> GetDrawingsByConnectorRef_V2530(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetDrawingsByConnectorRef_V2530(idConnector);
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

        //[pramod.misal][GEOS2-5524][06.08.2024]
        public List<SCMLocationsManager> GetSCMLocationsManagerByIdSCM_V2550(Data.Common.Company selectedPlant)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSCMLocationsManagerByIdSCM_V2550(selectedPlant);
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
        public Int64 GetMaxPosition(Int64 idParent, Int64 idCompany)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetMaxPosition(idParent, idCompany);
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

        //[rushikesh.gaikwad] [GEOS2-5524] [08.07.2024]
        public SCMLocationsManager AddSCMLocation(SCMLocationsManager scmLocationsManager)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddSCMLocation(scmLocationsManager);
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

        //[pramod.misal] [GEOS2-5525] [09.08.2024]
        public SCMLocationsManager UpdateSCMLocation(SCMLocationsManager scmLocationsManager)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateSCMLocation(scmLocationsManager);
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

        //[pramod.misal][GEOS2-5525][08.08.2024]
        public bool IsInUSESCMLocation(Int64 idSampleLocation, Int64 idCompany)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.IsInUSESCMLocation(idSampleLocation, idCompany);
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

        //[pramod.misal][GEOS2-5524][05.08.2024]
        public List<SCMLocationsManager> GetIsLeafSCMLocationsManager_V2550(Data.Common.Company selectedPlant)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetIsLeafSCMLocationsManager_V2550(selectedPlant);
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


        public List<ConnectorLocation> GetConnectorLocationsByRef_V2550(string connref)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorLocationsByRef_V2550(connref);
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

        public bool UpdateConnectorStatus_V2550(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorStatus_V2550(connectorSearch);
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

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        public Int64 GetMaxPosition_V2550(Int64 idParent, Int64 idCompany, string fullName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetMaxPosition_V2550(idParent, idCompany, fullName);
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

        //[pramod.misal] [GEOS2-5525] [08.08.2024]
        public bool IsExistSCMLocationsManagerName(string name, Int64 parent, Int64 idCompany)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.IsExistSCMLocationsManagerName(name, parent, idCompany);
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

        //[rdixit][12.08.2024][GEOS2-5752]
        public List<Data.Common.Company> GetAuthorizedPlants_V2550(Int32 idUser)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAuthorizedPlants_V2550(idUser);
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
        //[Rahul.Gadhave][GEOS2-5779][Date-09/08/2024]
        public List<SCMConnectorImage> GetAllConnectorImagesForImageSection_V2550(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorImagesForImageSection_V2550(Connectors);
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

        //[pramod.misal][GEOS2-5804][Date-5/08/2024]
        public ConnectorSearch UpdateConnectorStatus_V2560(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorStatus_V2560(connectorSearch);
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

        #region [rdixit][GEOS2-5802][05.09.2024]
        public List<Connectors> GetSampleRegistrationAllConnectors_V2560(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSampleRegistrationAllConnectors_V2560(Connectors);
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
        public List<Connectors> GetConnectorsBySearchConfiguration_V2560(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorsBySearchConfiguration_V2560(Connectors);
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
        public List<ConnectorProperties> GetAllConnectorCustomProperties_V2560()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectorCustomProperties_V2560();
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

        //[rushikesh.gaikwad][GEOS2-5801][12.09.2024]

        public List<UserRoles> GetUserRoles_V2560()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetUserRoles_V2560();
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

        #endregion

        //[GEOS2-5803][rdixit][13.09.2024]
        public string GetLatestConnectorReference(string year)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetLatestConnectorReference(year);
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

        //[GEOS2-5803][rdixit][13.09.2024]
        public bool AddConnector_V2560(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnector_V2560(connectorSearch);
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

        //[GEOS2-6601][14.11.2024][rdixit]
        public List<Connectors> GetAllConnectors_V2580(Connectors Connectors, string componentQuery)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2580(Connectors, componentQuery);
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

        //[GEOS2-6080][05.12.2024][rdixit]
        public List<ScmDrawing> GetDrawingsByConnectorRef_V2590(long idConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetDrawingsByConnectorRef_V2590(idConnector);
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

        //[rdixit][GEOS2-6654][03.01.2025]
        public List<Samples> GetReferenceByCustomer_V2600(DateTime startDate, DateTime endDate, string idCustomerList)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetReferenceByCustomer_V2600(startDate, endDate, idCustomerList);
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

        //[rdixit][14.01.2025][GEOS2-6857]
        public string GetLatestConnectorReference_V2600(string year)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetLatestConnectorReference_V2600(year);
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

        //[rdixit][GEOS2-6987][03.03.2025]
        public List<Samples> GetModifiedSamplesByIdSite_V2620(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetModifiedSamplesByIdSite_V2620(startDate, endDate, idSite);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public bool UpdateSubFamily_V2620(ConnectorSubFamily subFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateSubFamily_V2620(subFamily);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorFamily AddConnectorFamilies_V2620(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnectorFamilies_V2620(ConnectorFamily);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorFamily UpdateConnectorFamilies_V2620(ConnectorFamily ConnectorFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorFamilies_V2620(ConnectorFamily);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<ConnectorFamily> GetConnectorFamilies_V2620()
        {

            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorFamilies_V2620();
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public ConnectorSubFamily GetSubFamilyDetails_V2620(int idSubFamily, string familyName)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamilyDetails_V2620(idSubFamily, familyName);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<ConnectorSubFamily> GetSubFamiliesListByIdFamily_V2620(int idFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSubFamiliesListByIdFamily_V2620(idFamily);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public List<FamilyImage> GetFamilyImagesByIdFamily_V2620(int IdFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetFamilyImagesByIdFamily_V2620(IdFamily);
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

        //[rdixit][05.03.2025][GEOS2-7026] 
        public bool InsertSubFamily_V2620(ConnectorSubFamily subFamily)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.InsertSubFamily_V2620(subFamily);
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

        //[rdixit][GEOS2-6984][27.03.2025]
        public List<Connectors> GetAllConnectors_V2630(Connectors Connectors, string componentQuery)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2630(Connectors, componentQuery);
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

        //[rdixit][GEOS2-7859][14.05.2025]
        public List<Samples> GetNewSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetNewSamplesByIdSite_V2640(startDate, endDate, idSite);
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

        #region [GEOS2-7863][rani dhamankar][12-05-2025]
        public List<ThreeDConnectorItems> Get3dConnectorByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.Get3dConnectorByIdSite_V2640(startDate, endDate, idSite);
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
        #endregion

        //[rdixit][GEOS2-7861][14.05.2025]
        public List<Samples> GetModifiedSamplesByIdSite_V2640(DateTime startDate, DateTime endDate, Int32 idSite)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetModifiedSamplesByIdSite_V2640(startDate, endDate, idSite);
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

        #region [GEOS2-7855][rani dhamankar][15-05-2025]
        public List<ConnectorProperties> GetPropertyManagerByFamily_V2640(string familyId)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetPropertyManagerByFamily_V2640(familyId);
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
		//[nsatpute][23-05-2025][GEOS2-7996]
        public List<Connectors> GetAllConnectors_V2640(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2640(Connectors, componentQuery, getAllRecords);
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

        #endregion

        //[rdixit][GEOS2-8133][26.05.2025]
        public List<Connectors> GetSampleRegistrationAllConnectors_V2640(Connectors Connectors, bool isLoadAll)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSampleRegistrationAllConnectors_V2640(Connectors, isLoadAll);
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

        //[rdixit][GEOS2-8133][26.05.2025]
        public string AddConnector_V2650(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnector_V2650(connectorSearch);
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

        //[rdixit][11.06.2025][GEOS2-6644]
        public bool SaveConfigurationsForSearchFilters_V2650(List<ConfigurationFamily> configurationList)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.SaveConfigurationsForSearchFilters_V2650(configurationList);
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

        //[rdixit][11.06.2025][GEOS2-6644]
        public List<SimilarCharactersByConfiguration> GetSimilarCharachtersByConfiguration()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetSimilarCharachtersByConfiguration();
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

        //[rdixit][19.06.2025][GEOS2-6645]
        public List<Connectors> GetAllConnectors_V2650(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2650(Connectors, componentQuery, getAllRecords);
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
        //[nsatpute][16.07.2025][GEOS2-8090]
        public bool IsConnectorExists(string connectorRerence)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.IsConnectorExists(connectorRerence);
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
        //[nsatpute][22.07.2025][GEOS2-8090]
        public List<ScmDrawing> GetDrawingsByCustomerRef_V2660(string customerRef)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetDrawingsByCustomerRef_V2660(customerRef);
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
        public List<Emdep.Geos.Data.Common.Company> GetAllCompaniesWithServiceProvider_V2660()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllCompaniesWithServiceProvider_V2660();
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

        //[rdixit][GEOS2-9199][11.09.2025]
        public ConnectorSearch UpdateConnectorStatus_V2670(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.UpdateConnectorStatus_V2670(connectorSearch);
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

        //[rdixit][18.09.2025][GEOS2-]
        public Connectors GetConnectorProperties_V2670(Connectors Connectors)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorProperties_V2670(Connectors);
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

        //[rdixit][18.09.2025][GEOS2-]
        public List<ConnectorProperties> GetCustomeProperties_V2670()
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetCustomeProperties_V2670();
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


        //[rdixit][19.06.2025][GEOS2-6645]
        public List<Connectors> GetAllConnectors_V2670(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2670(Connectors, componentQuery, getAllRecords);
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

        //[rdixit][14.10.2025][GEOS2-8895]
        public string AddConnector_V2680(ConnectorSearch connectorSearch)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.AddConnector_V2680(connectorSearch);
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
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public bool DeleteReference_V2680(string Reference, Int64 IdConnector, Int32 IdUser)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.DeleteReference_V2680(Reference, IdConnector, IdUser);
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
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public bool GetConnectorCreatorEmailAndSendMail_V2680(Int64 IdConnector)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetConnectorCreatorEmailAndSendMail_V2680(IdConnector);
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
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public List<Connectors> GetAllConnectors_V2680(Connectors Connectors, string componentQuery, bool getAllRecords)
        {
            try
            {
                factory = new ChannelFactory<ISCMService>(binding, endPointAddress);
                ISCMService channel = factory.CreateChannel();
                return channel.GetAllConnectors_V2680(Connectors, componentQuery, getAllRecords);
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
