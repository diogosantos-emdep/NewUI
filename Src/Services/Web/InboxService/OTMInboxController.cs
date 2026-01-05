using InboxService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace InboxService
{
    public class OTMInboxController : IOTMInboxService
    {
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        string serviceIP = "";
        string servicePort = "";
        string serviceAddress = "";
        ServiceBehaviorAttribute serviceb;
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<IOTMInboxService> factory = null;


        public OTMInboxController(string serviceUrl)
        {
            if (serviceUrl.Contains("https://") || serviceUrl.Contains("http://"))
            {
                serviceAddress = serviceUrl + "/OTMInboxService.svc";
            }
            else
            {
                serviceAddress = "http://" + serviceUrl + "/OTMInboxService.svc";
            }

            if (serviceAddress.Contains("http://"))
            {
                BasicHttpBinding b = new BasicHttpBinding(BasicHttpSecurityMode.None);
                b.AllowCookies = false;
                b.OpenTimeout = new TimeSpan(4, 1, 1);
                b.CloseTimeout = new TimeSpan(4, 1, 1);
                b.MaxBufferSize = int.MaxValue;
                b.MaxReceivedMessageSize = long.MaxValue;
                b.MessageEncoding = WSMessageEncoding.Text;
                b.MaxBufferPoolSize = long.MaxValue;
                b.BypassProxyOnLocal = false;
                b.ReceiveTimeout = new TimeSpan(0, 20, 0);
                b.SendTimeout = new TimeSpan(4, 1, 1);
                b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                b.ReaderQuotas.MaxDepth = int.MaxValue;
                b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                b.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
                b.ReaderQuotas.MaxArrayLength = int.MaxValue;
                b.TransferMode = TransferMode.Streamed;
                binding = b;
                ServiceBehaviorAttribute sb = new ServiceBehaviorAttribute();
                sb.IncludeExceptionDetailInFaults = true;
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

        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public bool AddEmails(email emailDetails)
        {
            try
            {
                factory = new ChannelFactory<IOTMInboxService>(binding, endPointAddress);
                IOTMInboxService channel = factory.CreateChannel();
                return channel.AddEmails(emailDetails);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new Exception();
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }
    }
}