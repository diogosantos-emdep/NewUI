using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Glpi;
using Emdep.Geos.Data.Common.HarnessPart;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.ServiceProcess
{
    public class HarnessPartController : IHarnessPartService
    {
        string serviceIP="";
        string servicePort="";
        string serviceAddress = "";
        Binding binding;
        EndpointAddress endPointAddress;
        ChannelFactory<IHarnessPartService> factory = null;

        public HarnessPartController(string serviceIP, string servicePort,string serviceFolderPath)
        {
            this.serviceIP = serviceIP;
            this.servicePort = servicePort;
            serviceAddress = "http://" + serviceIP + ((!string.IsNullOrEmpty(servicePort)) ? ":" : "") + servicePort + serviceFolderPath+ "/HarnessPartService.svc";
            BasicHttpBinding b = new BasicHttpBinding(BasicHttpSecurityMode.None);
            b.AllowCookies = false;
            b.OpenTimeout = new TimeSpan(4, 1, 1);
            b.CloseTimeout = new TimeSpan(4, 1, 1);
            b.MaxBufferSize = int.MaxValue;
            b.MaxReceivedMessageSize = int.MaxValue;
            b.MessageEncoding = WSMessageEncoding.Text;
            b.MaxBufferPoolSize = int.MaxValue;
            b.BypassProxyOnLocal = false;

            b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
            b.ReaderQuotas.MaxDepth = int.MaxValue;
            b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            b.ReaderQuotas.MaxNameTableCharCount = 16384;
            b.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding = b;

            endPointAddress = new EndpointAddress(serviceAddress);
        }

        public HarnessPartController(string serviceUrl)
        {
            if (serviceUrl.Contains("https://") || serviceUrl.Contains("http://"))
            {
                serviceAddress = serviceUrl + "/HarnessPartService.svc";
            }
            else
            {
                serviceAddress = "http://" + serviceUrl + "/HarnessPartService.svc";
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

                b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                b.ReaderQuotas.MaxDepth = int.MaxValue;
                b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                b.ReaderQuotas.MaxNameTableCharCount = 16384;
                b.ReaderQuotas.MaxArrayLength = int.MaxValue;
                binding = b;
                b.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                endPointAddress = new EndpointAddress(serviceAddress);
                //if (serviceAddress.Contains("9.7") || serviceAddress.Contains("9.202"))
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
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

                b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                b.ReaderQuotas.MaxDepth = int.MaxValue;
                b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                b.ReaderQuotas.MaxNameTableCharCount = 16384;
                b.ReaderQuotas.MaxArrayLength = int.MaxValue;
                binding = b;

                endPointAddress = new EndpointAddress(serviceAddress);
            }
        }
        public List<Language> GetAllLanguage()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllLanguage();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
       
        }

        public List<Color> GetAllColor()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllColor();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
         

        }

        public List<Company> GetAllCompanies()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllCompanies();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
            

        }

        public List<HarnessPartAccessoryType> GetAllHarnessPartAccessoryType()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllHarnessPartAccessoryType();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
           

        }

        public List<HarnessPart> GetEnumTest()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetEnumTest();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }
        public List<EnterpriseGroup> GetAllEnterpriseGroup()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllEnterpriseGroup();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
          

        }

        public List<HarnessPartType> GetAllHarnessPartType()
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllHarnessPartType();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
            
        }

        public List<HarnessPart> GetAllHarnessPartSearchResult(HarnessPartSearch harnessPartSearch)
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllHarnessPartSearchResult(harnessPartSearch);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
           
        }

        public List<HarnessPart> GetAllHarnessPartSearchResultPossibleSearch(HarnessPartSearch harnessPartSearch)
        {
            try
            {
                factory = new ChannelFactory<IHarnessPartService>(binding, endPointAddress);
                IHarnessPartService channel = factory.CreateChannel();
                return channel.GetAllHarnessPartSearchResultPossibleSearch(harnessPartSearch);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw timeProblem;
            }

            catch (FaultException unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw unknownFault;

            }

            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw commProblem;

            }

            finally
            {

                if (factory != null)
                    factory.Close();
            }
            

        }
   }
}
