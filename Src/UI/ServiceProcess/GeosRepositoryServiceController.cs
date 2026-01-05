using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class GeosRepositoryServiceController : IGeosRepositoryService
    {
        string serviceIP;
        string servicePort = "";
        string serviceAddress = "";
        Binding binding;
        EndpointAddress endPointAddress;
        IGeosRepositoryService service = null;
        ChannelFactory<IGeosRepositoryService> factory = null;
        Action<int> ProgressEvent;
        long filesize = 0;

        public GeosRepositoryServiceController(string serviceIP, string servicePort, string serviceFolderPath)
        {

            this.serviceIP = serviceIP;
            this.servicePort = servicePort;
            serviceAddress = "http://" + serviceIP + ((!string.IsNullOrEmpty(servicePort)) ? ":" : "") + servicePort + serviceFolderPath + "/GeosRepositoryService.svc";
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
            b.TransferMode = TransferMode.Streamed;
            b.MessageEncoding = WSMessageEncoding.Mtom;
            b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
            b.ReaderQuotas.MaxDepth = 32;
            b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            b.ReaderQuotas.MaxNameTableCharCount = 16384;
            b.ReaderQuotas.MaxArrayLength = int.MaxValue;

            binding = b;

            try
            {
                endPointAddress = new EndpointAddress(serviceAddress);
            }
            catch (Exception)
            {

            }
        }

        public GeosRepositoryServiceController(string serviceUrl)
        {
            if (serviceUrl.Contains("https://") || serviceUrl.Contains("http://"))
            {
                serviceAddress = serviceUrl + "/GeosRepositoryService.svc";
            }
            else
            {
                serviceAddress = "http://" + serviceUrl + "/GeosRepositoryService.svc";
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
                b.SendTimeout = new TimeSpan(0, 20, 0);
                b.TransferMode = TransferMode.Streamed;
                b.MessageEncoding = WSMessageEncoding.Mtom;
                b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                b.ReaderQuotas.MaxDepth = 32;
                b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                b.ReaderQuotas.MaxNameTableCharCount = 16384;
                b.ReaderQuotas.MaxArrayLength = int.MaxValue;
                b.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding = b;

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
                //serviceAddress =  serviceUrl + "/GeosRepositoryService.svc";
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
                b.TransferMode = TransferMode.Streamed;
                b.MessageEncoding = WSMessageEncoding.Mtom;
                b.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                b.ReaderQuotas.MaxDepth = 32;
                b.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                b.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                b.ReaderQuotas.MaxNameTableCharCount = 16384;
                b.ReaderQuotas.MaxArrayLength = int.MaxValue;

                binding = b;

                try
                {
                    endPointAddress = new EndpointAddress(serviceAddress);
                }
                catch (Exception)
                {

                }
            }
        }

        public void IssueDownloadRequest(GeosWorkbenchVersion geosWorkbenchVersion, string localFile, Action<int> action, int progressDownloadValue)
        {
            Stream byteStream = null;
            try
            {
                FileMetaData response = DownloadFile(geosWorkbenchVersion, out byteStream);
                filesize = response.FileSize;
                ProgressEvent = action;
                if (response != null && !string.IsNullOrEmpty(response.RemoteFileName) && byteStream != null)
                {
                    SaveFile(byteStream, localFile, progressDownloadValue);
                }
                if (byteStream != null)
                {
                    byteStream.Close();
                }
            }
            catch (Exception unknownProblem)
            {
                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (byteStream != null)
                {
                    byteStream.Close();
                }

            }

        }

        private void SaveFile(Stream saveFile, string localFilePath, int progressDownloadValue)
        {
            const int bufferSize = 65536; // 64K
            FileInfo finfo = new FileInfo(localFilePath);

            FileStream outfile = new FileStream(localFilePath, FileMode.Create);
            try
            {
                byte[] buffer = new byte[bufferSize];
                int bytesRead = saveFile.Read(buffer, 0, bufferSize);

                while (bytesRead > 0)
                {
                    outfile.Write(buffer, 0, bytesRead);
                    bytesRead = saveFile.Read(buffer, 0, bufferSize);

                    ProgressEvent.Invoke((int)(outfile.Position * progressDownloadValue / filesize));
                }
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }

            finally
            {
                if (outfile != null)
                {
                    outfile.Close();
                }

                if (factory != null) factory.Abort();

            }
        }

        public FileMetaData DownloadFile(GeosWorkbenchVersion WorkbenchVersion, out System.IO.Stream FileByteStream)
        {
            try
            {

                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                FileDownloadMessage msg = new FileDownloadMessage();
                msg.Version = WorkbenchVersion;
                IGeosRepositoryService channel = factory.CreateChannel();
                FileDownloadReturnMessage retVal = channel.GetWorkbenchDownloadVersion(msg);
                FileByteStream = retVal.FileByteStream;
                service = null;
                factory.Close();
                factory = null;
                return retVal.DownloadedFileMetadata;
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public FileDownloadReturnMessage GetWorkbenchDownloadVersion(FileDownloadMessage mgr)
        {

            try
            {

                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                service = null;
                factory.Close();
                factory = null;
                return channel.GetWorkbenchDownloadVersion(mgr);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }

            finally
            {
                if (service != null) ((IClientChannel)service).Close();
                if (factory != null) factory.Abort();
            }

        }

        public byte[] GetCompanyImage(Int32 idCompany)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetCompanyImage(idCompany);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public void Uploader(FileUploader userProfileFileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                channel.Uploader(userProfileFileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public FileUploadReturnMessage TaskAttachmentUploader(FileUploader userProfileFileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.TaskAttachmentUploader(userProfileFileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public FileUploadReturnMessage UploaderProjectScopeFile(FileUploader fileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderProjectScopeFile(fileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public byte[] GetUserProfileImage(string userName, byte isValidate)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetUserProfileImage(userName, isValidate);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();


                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public byte[] GetEmployeesImage(string employeeCode)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetEmployeesImage(employeeCode);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public byte[] GetUserProfileImageWithoutException(string userName, byte isValidate = 1)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetUserProfileImageWithoutException(userName, isValidate);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public FileUploadReturnMessage UploaderGLPIZipFile(FileUploader fileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                //FileUploader fileUploadermsg = new FileUploader();
                //fileUploadermsg = fileUploader;
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderGLPIZipFile(fileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }

        public void UploadIsValidateFalseUserImage(FileUploader userProfileFileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                channel.UploadIsValidateFalseUserImage(userProfileFileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }



        public byte[] GetModuleImage(Int32 idModule)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetModuleImage(idModule);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }


        public FileUploadReturnMessage UploaderActivityAttachmentZipFile(FileUploader fileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderActivityAttachmentZipFile(fileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }

        public byte[] GetCaroemIconFileInBytes(string caroemName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetCaroemIconFileInBytes(caroemName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }


        public byte[] GetCustomerIconFileInBytes(string customerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetCustomerIconFileInBytes(customerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        public FileUploadReturnMessage UploaderEngineeringAnalysisZipFile(FileUploader fileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderEngineeringAnalysisZipFile(fileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }

        public bool IsEmployeeDetailFileDeleted(string fileName, string folderName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.IsEmployeeDetailFileDeleted(fileName, folderName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }

        public byte[] GetArticleAttachmentFile(string atricleReference, string savedFileName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetArticleAttachmentFile(atricleReference, savedFileName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetEmployeesExitDocument(string employeeCode, string fileName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetEmployeesExitDocument(employeeCode, fileName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetPrintLabelFile(string printerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetPrintLabelFile(printerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public byte[] GetCompanyLayoutFile(string fileName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetCompanyLayoutFile(fileName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public List<FileDetail> GetAllFileDetailsFromCompanyLayout()
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetAllFileDetailsFromCompanyLayout();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetArticleImageInBytes(string ImagePath)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetArticleImageInBytes(ImagePath);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetPrintDNItemLabelFile(string printerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetPrintDNItemLabelFile(printerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetBoxLabelFile(string printerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetBoxLabelFile(printerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetOrganizationChart(string companyAlias)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetOrganizationChart(companyAlias);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetActionPlanExcel()
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetActionPlanExcel();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public byte[] GetPrintSmallDNItemLabelFile(string printerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetPrintSmallDNItemLabelFile(printerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetPrintDeliveryNoteItemLabelFile(string printerName, string labelSize)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetPrintDeliveryNoteItemLabelFile(printerName, labelSize);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }


        public byte[] GetBPLDetailsExcel(string fileName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetBPLDetailsExcel(fileName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }



        public byte[] GetLookupImages(string fileName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetLookupImages(fileName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        public FileUploadReturnMessage UploaderOTAttachmentZipFile(FileOTAttachmentUploader fileOTAttachmentUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderOTAttachmentZipFile(fileOTAttachmentUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }


        public byte[] GetQCTemplate()
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetQCTemplate();
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }


        }

        //[rdixit][24.05.2023]
        public byte[] GetImagesByUrl(string imageUrl)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetImagesByUrl(imageUrl);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        //[rdixit][GEOS2-3741][24.05.2023]
        public void UploadIsValidateFalseUserImage_V2400(FileUploader userProfileFileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                channel.UploadIsValidateFalseUserImage_V2400(userProfileFileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }

        }

        //[rdixit][GEOS2-3741][24.05.2023]
        public byte[] GetUserProfileImage_V2400(string userName, byte isValidate)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetUserProfileImage_V2400(userName, isValidate);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();


                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {

                if (factory != null)
                    factory.Close();
            }
        }

        // [nsatpute][04-09-2024][GEOS2-5415]
        public byte[] GetPrintQcPassLabelFile(string printerName)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.GetPrintQcPassLabelFile(printerName);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }
            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();
                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }
            catch (EndpointNotFoundException endpointProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);
            }
            catch (MessageSecurityException messagesecurityProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);
            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);
            }
            catch (ProtocolException protocolProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);
            }
            catch (CommunicationException commProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);
            }
            catch (UriFormatException uriFormatProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.UnknownException, unknownProblem.Message, unknownProblem.InnerException, unknownProblem.Source);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }

        //[Sudhir.Jangra][GEOS2-6016]
        public FileUploadReturnMessage UploaderActionPlanTaskAttachmentZipFile_V2580(FileUploader fileUploader)
        {
            try
            {
                factory = new ChannelFactory<IGeosRepositoryService>(binding, endPointAddress);
                IGeosRepositoryService channel = factory.CreateChannel();
                return channel.UploaderActionPlanTaskAttachmentZipFile_V2580(fileUploader);
            }
            catch (TimeoutException timeProblem)
            {
                if (factory != null)
                    factory.Abort();
                throw new ServiceUnexceptedException(ServiceExceptionType.TimeoutException, timeProblem.Message, timeProblem.InnerException, timeProblem.Source);
            }

            catch (FaultException<ServiceException> unknownFault)
            {
                if (factory != null)
                    factory.Abort();

                throw new FaultException<ServiceException>(unknownFault.Detail, unknownFault.ToString());
            }

            catch (EndpointNotFoundException endpointProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.EndpointNotFoundException, endpointProblem.Message, endpointProblem.InnerException, endpointProblem.Source);

            }
            catch (MessageSecurityException messagesecurityProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.MessageSecurityException, messagesecurityProblem.Message, messagesecurityProblem.InnerException, messagesecurityProblem.Source);

            }
            catch (SecurityNegotiationException securitynegotiationProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.SecurityNegotiationException, securitynegotiationProblem.Message, securitynegotiationProblem.InnerException, securitynegotiationProblem.Source);

            }
            catch (ProtocolException protocolProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.ProtocolException, protocolProblem.Message, protocolProblem.InnerException, protocolProblem.Source);

            }
            catch (CommunicationException commProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.CommunicationException, commProblem.Message, commProblem.InnerException, commProblem.Source);

            }
            catch (UriFormatException uriFormatProblem)
            {

                if (factory != null)
                    factory.Abort();

                throw new ServiceUnexceptedException(ServiceExceptionType.UriFormatException, uriFormatProblem.Message, uriFormatProblem.InnerException, uriFormatProblem.Source);
            }
            catch (Exception unknownProblem)
            {

                if (factory != null)
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
