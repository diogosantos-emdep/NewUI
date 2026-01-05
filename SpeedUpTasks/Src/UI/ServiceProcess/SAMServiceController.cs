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
            serviceAddress = "http://" + serviceUrl + "/SAMService.svc";

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


        public bool UpdateTestBoardPartNumberTracking(Company company, Int64 idPartNumberTracking, Int32 status , Int32 idOperator)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.UpdateTestBoardPartNumberTracking(company, idPartNumberTracking,status, idOperator);
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
                return channel.UpdateOTFromGrid(company,ot);
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

        public List<TempWorklog> GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite,Company Plant)
        {
            try
            {
                factory = new ChannelFactory<ISAMService>(binding, endPointAddress);
                ISAMService channel = factory.CreateChannel();
                return channel.GetWorkLogOTWithHoursAndUserByPeriodAndSite(FromDate, ToDate, IdSite,Plant);
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

    }
}


