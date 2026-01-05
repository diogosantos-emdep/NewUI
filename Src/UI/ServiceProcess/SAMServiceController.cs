using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using System.ServiceModel.Security;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.File;

namespace Emdep.Geos.UI.ServiceProcess
{
    public class SAMServiceController : ISAMService
    {
        string serviceIP = "";
        string servicePort = "";
        string serviceAddress = "";
        ServiceBehaviorAttribute serviceb;
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<ISAMService> factory = null;

        public SAMServiceController(string serviceIP, string servicePort, string serviceFolderPath)
        {
            this.serviceIP = serviceIP;
            this.servicePort = servicePort;
            serviceAddress = "http://" + serviceIP + ((!string.IsNullOrEmpty(servicePort)) ? ":" : "") + servicePort + serviceFolderPath + "/SAMService.svc";

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

        public SAMServiceController(string serviceUrl)
        {
            if (serviceUrl.Contains("https://") || serviceUrl.Contains("http://"))
            {
                serviceAddress = serviceUrl + "/SAMService.svc";
            }
            else
            {
                serviceAddress = "http://" + serviceUrl + "/SAMService.svc";
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


        public List<Ots> GetPendingWorkorders(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders(company);
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

        public List<OTWorkingTime> GetOTWorkingTimeByIdOT(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTWorkingTimeByIdOT(company, idOT);
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


        public OTWorkingTime AddOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.AddOTWorkingTime(company, otWorkingTime);
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


        public bool UpdateOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTWorkingTime(company, otWorkingTime);
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

        public List<Ots> GetPendingWorkorders_V2038(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2038(company);
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


        public List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTWorkingTimeDetails(idOT, company);
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


        public Article GetArticleDetails(Int32 idArticle)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetArticleDetails(idArticle);
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

        public Ots GetWorkOrderByIdOt(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderByIdOt(idOt, company);
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


        public List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTAssignedUsers(company, idOT);
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


        public List<OTAssignedUser> GetUsersToAssignedOT(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetUsersToAssignedOT(company);
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


        public bool UpdateOTAssignedUser(Company company, List<OTAssignedUser> otAssignedUsers)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTAssignedUser(company, otAssignedUsers);
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

        public List<OTWorkingTime> GetOTWorkingTimeByIdOT_V2040(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTWorkingTimeByIdOT_V2040(company, idOT);
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

        public List<ValidateItem> GetWorkOrderItemsToValidate(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderItemsToValidate(company, idOT);
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


        public bool UpdateTestBoardPartNumberTracking(Company company, Int64 idPartNumberTracking, Int32 status, Int32 idOperator)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateTestBoardPartNumberTracking(company, idPartNumberTracking, status, idOperator);
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

        public List<Ots> GetPendingWorkorders_V2043(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2043(company);
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

        public List<OTAttachment> GetOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTAttachment(company, idOT);
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


        public byte[] GetOTAttachmentInBytes(string fileName, string quotationYear, string quotationCode)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTAttachmentInBytes(fileName, quotationYear, quotationCode);
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

        public List<OTAssignedUser> GetUsersToAssignedOT_V2044(Company company, Int32 idCompany)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetUsersToAssignedOT_V2044(company, idCompany);
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


        public List<Ots> GetPendingWorkorders_V2044(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2044(company);
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

        public bool UpdateOTFromGrid(Company company, Ots ot)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTFromGrid(company, ot);
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
        public List<WorklogUser> GetWorkLogUserListByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkLogUserListByPeriodAndSite(FromDate, ToDate, IdSite, Plant);
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

        public List<TempWorklog> GetWorkLogHoursByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkLogHoursByPeriodAndSite(FromDate, ToDate, IdSite);
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

        public List<TempWorklog> GetWorkLogOTWithHoursByPeriodAndSiteAndUser(DateTime FromDate, DateTime ToDate, int IdSite, int IdUser)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkLogOTWithHoursByPeriodAndSiteAndUser(FromDate, ToDate, IdSite, IdUser);
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

        public List<TempWorklog> GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkLogOTWithHoursAndUserByPeriodAndSite(FromDate, ToDate, IdSite, Plant);
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

        public List<TempWorklog> GetOTWorkLogTimesByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTWorkLogTimesByPeriodAndSite(FromDate, ToDate, IdSite, Plant);
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


        public bool DeleteWorkLog(Company company, Int64 idOTWorkingTime)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.DeleteWorkLog(company, idOTWorkingTime);
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

        public bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateWorkLog(company, otWorkingTime);
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

        public List<Ots> GetPendingWorkorders_V2090(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2090(company);
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

        //[GEOS2-2906]
        public List<Ots> GetPendingWorkorders_V2170(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2170(company);
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

        public bool UpdateWorkflowStatusInOT(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateWorkflowStatusInOT(IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
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

        public List<WorkflowStatus> GetAllWorkflowStatus()
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllWorkflowStatus();
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

        public List<WorkflowTransition> GetAllWorkflowTransitions()
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
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



        public Ots GetWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderByIdOt_V2170(idOt, company);
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


        public Ots GetStructureWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetStructureWorkOrderByIdOt_V2170(idOt, company);
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

        public Ots GetWorkOrderByIdOt_V2180(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderByIdOt_V2180(idOt, company);
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


        public bool UpdateOTAssignedUser_V2180(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTAssignedUser_V2180(company, otAssignedUsers, IdOT, OldRemark, Remark, IdUser);
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


        //[GEOS2-2961]
        public List<Ots> GetPendingWorkorders_V2180(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2180(company);
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

        public bool UpdateOTFromGrid_V2180(Company company, Ots ot)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTFromGrid_V2180(company, ot);
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

        //[GEOS2-2959]
        public List<Ots> GetPendingWorkordersForDashboard_V2180(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkordersForDashboard_V2180(company);
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


        public List<Ots> GetAllAssignedWorkordersForPlanning_V2250(Company company,
            out List<OTWorkingTime> listLoggedHoursForOT_User_Date, out List<PlannedHoursForOT_User_Date> listPlannedHoursForOT_User_Date)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllAssignedWorkordersForPlanning_V2250(company,
                out listLoggedHoursForOT_User_Date, out listPlannedHoursForOT_User_Date);
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



        public bool UpdateOTUserPlanningsFromGrid_V2250(Company company, List<PlannedHoursForOT_User_Date> listLoggedHoursForOT_User_Date)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTUserPlanningsFromGrid_V2250(company, listLoggedHoursForOT_User_Date);
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


        public List<Ots> GetSAMWorkOrdersReport_V2260(Company company, DateTime fromDate, DateTime toDate)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetSAMWorkOrdersReport_V2260(company, fromDate, toDate);

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

        //[GEOS2-3585]
        public List<Ots> GetPendingWorkorders_V2280(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2280(company);
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


        //[GEOS2-3417]
        public List<Ots> GetPendingWorkorders_V2270(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2270(company);
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

        public List<WorkflowStatus> GetAllWorkflowStatusForQuality()
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllWorkflowStatusForQuality();
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

        public List<WorkflowTransition> GetAllWorkflowTransitionsForQuality()
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllWorkflowTransitionsForQuality();
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

        public bool UpdateOTQuality(Company company, Int64 IdOT, int IdUser, string otAttachment, string GUIDString, string year, string quotationCode, List<OTAttachment> lstAttachmentDeleted, string OldRemark, string Remark)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateOTQuality(company, IdOT, IdUser, otAttachment, GUIDString, year, quotationCode, lstAttachmentDeleted, OldRemark, Remark);
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

        public List<OTAttachment> GetQualityOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetQualityOTAttachment(company, idOT);
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


        public bool UpdateWorkflowStatusInOTQC(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateWorkflowStatusInOTQC(IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
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

        public Ots GetWorkOrderByIdOtForQC(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderByIdOtForQC(idOt, company);
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

        //[GEOS2-3915]
        public List<Ots> GetPendingWorkorders_V2301(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2301(company);
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

        //[pjadhav][GEOS2-3681][12/09/2022]
        public List<OTs> GetAllOrderItemsList(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllOrderItemsList(company);
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
        //Shubham[skadam] GEOS2-3682 Implement in SAM the items in the WO that must be passed in Bancos Stage (3/9) 09 12 2022
        public OTs GetSAMOrderItemsInformationByIdOt_V2340(Int64 idOt, Company company, UInt32 IdArticle)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetSAMOrderItemsInformationByIdOt_V2340(idOt, company, IdArticle);
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
        public List<Data.Common.PCM.PCMArticleImage> GetPCMArticleImagesByIdPCMArticle(Int32 IdArticle, string ArticleReference)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPCMArticleImagesByIdPCMArticle(IdArticle, ArticleReference);
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

        public OtItemsComment AddObservationCommentItem(OtItemsComment ItemsComment, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.AddObservationCommentItem(ItemsComment, company);
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

        public bool DeleteComment_V2340(Int64 idComment, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.DeleteComment_V2340(idComment, company);
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

        //[pjadhav][GEOS2-3686][12/26/2022]
        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2350(Int64 otItems, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetOTWorkingTimeDetails_V2350(otItems, company);
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


        public OTWorkingTime AddOtWorkingTimeWorkLogItem(Int64 IdOT, Int64 idOTItems, OTWorkingTime Items, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.AddOtWorkingTimeWorkLogItem(IdOT, idOTItems, Items, company);
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

        public List<OTs> GetAllOrderItemsList_V2350(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllOrderItemsList_V2350(company);
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

        //[rdixit][28.08.2023][GEOS2-4754]
        public List<Ots> GetPendingWorkorders_V2430(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2430(company);
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

        //[rdixit][28.08.2023][GEOS2-4754]
        public List<OTs> GetAllOrderItemsList_V2430(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllOrderItemsList_V2430(company);
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

        //[rdixit][12.03.2024][GEOS2-5361]
        public List<Ots> GetPendingWorkorders_V2500(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2500(company);
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
        /// [rgadhave][18-06-2024][GEOS2-5583]
        public Ots GetStructureWorkOrderByIdOt_V2530(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetStructureWorkOrderByIdOt_V2530(idOt, company);
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

        // [GEOS2-5472][nsatpute][04-07-2024]
        public Ots GetStructureWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetStructureWorkOrderByIdOt_V2540(idOt, company);
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

        //[GEOS2-5472][nsatpute][05-07-2024]
        public byte[] GetSolidworksDrawingFileImageInBytes(string darwingPath, string SolidworksDrawingFileName)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetSolidworksDrawingFileImageInBytes(darwingPath, SolidworksDrawingFileName);
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
        //[nsatpute][04-07-2024][GEOS2-5408]
        public IList<LookupValue> GetLookupValues(byte key)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetLookupValues(key);
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

        //[pramod.misal][GEOS2-5473][09.07.2024]
        public List<Article> GetAllReference(bool seeAllArticles)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllReference(seeAllArticles);
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

        //[pramod.misal][GEOS2-5473][15.07.2024]
        public Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkOrderByIdOt_V2540(idOt, company);
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

        //[pramod.misal][GEOS2-5473][17.07.2024]
        public bool UpdateOTAssignedUser_V2540(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser, List<OtItem> DeletedOtItemList, List<OtItem> AddedOtItemList, List<OtItem> UpdatedOtItemList,List<LogEntriesByOT> LogEntriesByOTList)        {            try            {                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);                ISAMService channel = factory.CreateChannel();                return channel.UpdateOTAssignedUser_V2540(company, otAssignedUsers, IdOT, OldRemark, Remark, IdUser, DeletedOtItemList, AddedOtItemList, UpdatedOtItemList, LogEntriesByOTList);            }            catch (TimeoutException timeProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);            }            catch (FaultException<ServiceException> unknownFault)            {                if (factory != null)                    factory.Abort();                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());            }            catch (EndpointNotFoundException endpointProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);            }            catch (MessageSecurityException messagesecurityProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);            }            catch (SecurityNegotiationException securitynegotiationProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);            }            catch (ProtocolException protocolProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);            }            catch (CommunicationException commProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);            }            catch (UriFormatException uriFormatProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);            }            catch (Exception unknownProblem)            {                if (factory != null)                    factory.Abort();                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);            }            finally            {                if (factory != null)                    factory.Close();            }        }

        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public void StartSavingFile(string saveDirectorPath, string fileName)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                channel.StartSavingFile(saveDirectorPath, fileName);
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
		// [nsatpute][18-07-2024] [GEOS2-5409] 
        public void SavePartData(string saveDirectorPath, string fileName, byte[] partData)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                channel.SavePartData(saveDirectorPath, fileName, partData);
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
		// [nsatpute][19-07-2024] [GEOS2-5409] 
        public List<OTAttachment> GetQualityOTAndItemAttachments(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
               return channel.GetQualityOTAndItemAttachments(company, idOT);
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
		// [nsatpute][26-07-2024] [GEOS2-5473] 
        public List<Article> GetComponentArticlesByIdArticle(int idArticle)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetComponentArticlesByIdArticle(idArticle);
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

        //[nsatpute][02-08-2024][GEOS2-5410]
        public TechnicalSpecifications GetTechnicalSpecificationForReport(Company company, long idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetTechnicalSpecificationForReport(company, idOt);
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
        // [GEOS2-5410][06-08-2024][nsatpute]
        public List<ModuleReference> GetModuleReferencesForReport(Company company, long idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetModuleReferencesForReport(company, idOt);
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
        // [GEOS2-5410][06-08-2024][nsatpute]
        public List<OTAttachment> GetStructureOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetStructureOTAttachment(company, idOT);
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

        //[nsatpute][GEOS2-5410][08-08-2024]
        public List<Article> GetTestboardElectrificationOtArticles(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetTestboardElectrificationOtArticles(company, idOT);
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

        // [nsatpute][12-08-2024][GEOS2-5412]
        public List<OTAttachment> GetItemAttachments(Company company, Int64 idOT)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetItemAttachments(company, idOT);
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

        // [nsatpute][13-08-2024][GEOS2-5411]
        public byte[] GetstructureOtPlanolInBytes(Company company, Int64 idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetstructureOtPlanolInBytes(company, idOt);
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

        // [nsatpute][14-08-2024][GEOS2-5411]
        public List<byte[]> GetstructureOtElectricalDiagramsInBytes(Company company, Int64 idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetstructureOtElectricalDiagramsInBytes(company, idOt);
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
		// [nsatpute][19-08-2024][GEOS2-5412]
        public ProductTypes GetstructureOtProductDetails(Company company, Int64 idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetstructureOtProductDetails(company, idOt);
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
        // [nsatpute][21-08-2024][GEOS2-5412]
        public byte[] GenerateAndGetDeclarationOfConformity(Company company, Int64 idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GenerateAndGetDeclarationOfConformity(company, idOt);
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
        // [nsatpute][22-08-2024][GEOS2-5412]
        public byte[] GetCustomerLogoById(int idCustomer)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetCustomerLogoById(idCustomer);
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
		//[nsatpute][04-09-2024][GEOS2-5415]
        public QcPassLabelDetails GetQCPassLabelDetails(Company company, long idOt)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetQCPassLabelDetails(company, idOt);
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

        // [nsatpute][12-11-2024][GEOS2-5890]
        public bool UpdateWorkflowStatusInOTQC_V2580(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateWorkflowStatusInOTQC_V2580(IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
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
        // [nsatpute][13-11-2024][GEOS2-5889]
        public List<Ots> GetPendingWorkorders_V2580(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2580(company);
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

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public Ots GetStructureWorkOrderByIdOt_V2640(Int64 idOt, Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetStructureWorkOrderByIdOt_V2640(idOt, company);
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
       
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public List<ElectricalDiagram> GetElectricalDiagram()
        {

            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetElectricalDiagram();
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

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public byte[] GetElectricalDiagramsFileImageInBytes(string ElectricalDiagramFileName)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetElectricalDiagramsFileImageInBytes(ElectricalDiagramFileName);
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

        // [GEOS2-6728][pallavi.kale][14-04-2025]
		// [nsatpute][13-05-2025][GEOS2-6728]
        public bool AddEditDeleteElectricalDiagramForIdDrawing(List<OtItem> otItems)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.AddEditDeleteElectricalDiagramForIdDrawing(otItems);
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
        //[nsatpute][12.08.2025][GEOS2-9163]
        public List<Ots> GetPendingWorkorders_V2660(Company company)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetPendingWorkorders_V2660(company);
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
        // [Rahul.Gadhave][GEOS2-8713][Date:03/11/2025]
        public List<Company> GetAllCompaniesDetails_V2680(Int32 idUser)
        {
            try
            {

                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetAllCompaniesDetails_V2680(idUser);

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


