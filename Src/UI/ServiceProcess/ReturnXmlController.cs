using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using System.ServiceModel.Security;
using Emdep.Geos.Data.Common;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Emdep.Geos.UI.ServiceProcess
{
    public class ReturnXmlController:IxmlreturnService
    {
        string serviceIP = "";
        string servicePort = "";
        string serviceAddress = "";
        ServiceBehaviorAttribute serviceb;
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<IxmlreturnService> factory = null;
        
        public ReturnXmlController(string serviceIP, string servicePort, string serviceFolderPath)
        {
            this.serviceIP = serviceIP;
            this.servicePort = servicePort;
            serviceAddress = "http://" + serviceIP + ((!string.IsNullOrEmpty(servicePort)) ? ":" : "") + servicePort + serviceFolderPath + "/xmlreturnService.svc";

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
            b.SendTimeout = new TimeSpan(0, 20, 0);
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

        public ReturnXmlController(string serviceUrl)
        {
            serviceAddress = "http://" + serviceUrl + "/xmlreturnService.svc";
          //  serviceAddress = "http://" + serviceIP + ((!string.IsNullOrEmpty(servicePort)) ? ":" : "") + servicePort + serviceFolderPath + "/xmlreturnService.svc";

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
            b.SendTimeout = new TimeSpan(0, 20, 0);
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
        public DataSet GetOffersWithoutPurchaseOrderReturnListDatatable1()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://10.0.5.19:85/xmlreturnService.svc/GetOffersWithoutPurchaseOrderReturnListDatatable1");
                request.Method = "GET";
                request.ContentType = "application/json";



                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(DataSet));
                DataSet ds = new DataSet();
                ds = (DataSet)jsonResp.ReadObject(stream);
                return ds;
                //return channel.GetOffersWithoutPurchaseOrder(offerPlantId, idCurrency, idUser, idZone, accountingYear, companydetails);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);


            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (WebException we)
            {
                //string pageContent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
                //return pageContent;
                throw;
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public string GetOffersWithoutPurchaseOrder()
        {
            try
            {
                factory = new ChannelFactory<IxmlreturnService>(binding, endPointAddress);
                IxmlreturnService channel = factory.CreateChannel();
                return channel.GetOffersWithoutPurchaseOrder();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);


            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }
        public string GetOffersWithoutPurchaseOrderList()
        {
            try
            {
                //factory = new ChannelFactory<IxmlreturnService>(binding, endPointAddress);
                //IxmlreturnService channel = factory.CreateChannel();
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://10.0.9.7/xmlreturnService.svc/GetOffersWithoutPurchaseOrderList");
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(string));
                return (string)jsonResp.ReadObject(stream);
                //return channel.GetOffersWithoutPurchaseOrder(offerPlantId, idCurrency, idUser, idZone, accountingYear, companydetails);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);


            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public string GetoptionsByOfferList()
        {
            try
            {
                //factory = new ChannelFactory<IxmlreturnService>(binding, endPointAddress);
                //IxmlreturnService channel = factory.CreateChannel();
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://10.0.9.7/xmlreturnService.svc/GetoptionsByOfferList");
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(string));
                return (string)jsonResp.ReadObject(stream);
                //return channel.GetOffersWithoutPurchaseOrder(offerPlantId, idCurrency, idUser, idZone, accountingYear, companydetails);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);


            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
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
