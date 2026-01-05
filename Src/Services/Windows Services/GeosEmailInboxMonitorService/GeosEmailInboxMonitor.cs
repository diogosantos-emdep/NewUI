using Azure.Identity;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.OTMDataModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using GeosEmailInboxMonitorService.Logger;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MySqlX.XDevAPI;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace GeosEmailInboxMonitorService
{
    //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
    public partial class GeosEmailInboxMonitor : ServiceBase
    {
        #region Services     
       //IOTMService OtmStartUp = new OTMServiceController("localhost:6699");
       IOTMService OtmStartUp = new OTMServiceController(Properties.Settings.Default.SERVICE_PROVIDER_URL);
        #endregion

        #region Declaration
        private Timer EmailInboxMonitortimer = null;
        static readonly object _object = new object();
        string TimeZoneSetting = string.Empty;
        DateTime timeInTz;
        #endregion

        #region Constructor
        public GeosEmailInboxMonitor()
        {
            InitializeComponent();
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }
                //CheckUnseenEmails();  commented for //[pramod.misal][10-111-2025][GEOS2-9023] https://helpdesk.emdep.com/browse/GEOS2-9023 now we fetch the mail from cloud
                //CheckCloudEmails();//[pramod.misal][10-111-2025][GEOS2-9023]https://helpdesk.emdep.com/browse/GEOS2-9023
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  GeosEmailInboxMonitor() - OnStart(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            Log4NetLogger.Logger.Log(string.Format("GeosEmailInboxMonitor Constructor Executed.... "), category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods
        void CreateIfNotExists(string config_path)
        {
            string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\Emdep\Geos\OTMInboxMonitorServiceLogs.txt""/>
                                              <appendToFile value=""true"" />
                                              <rollingStyle value=""Size"" />
                                              <maxSizeRollBackups value=""10"" />
                                              <maximumFileSize value=""10MB"" />
                                              <staticLogFileName value=""true"" />
                                              <layout type=""log4net.Layout.PatternLayout"">
                                                <conversionPattern value=""%-5p %d %5rms - %m%n"" />
                                              </layout>
                                            </appender>
                                            <root>
                                              <level value=""Info"" />
                                              <appender-ref ref=""RollingLogFileAppender"" />
                                            </root>
                                          </log4net>
                                        </configuration>";

            if (!File.Exists(config_path))
            {
                File.WriteAllText(config_path, log4netConfig);
            }
        }

        void CheckUnseenEmails()
        {
            Log4NetLogger.Logger.Log(string.Format("CheckUnseenEmails() Service Starting"), category: Category.Info, priority: Priority.Low);
            try
            {
                //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
                if (string.IsNullOrEmpty(TimeZoneSetting))
                    TimeZoneSetting = OtmStartUp.GetTimezoneByServiceUrl_V2670(Properties.Settings.Default.SERVICE_PROVIDER_URL);
                using (var client = new ImapClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    client.Connect(Properties.Settings.Default.ImapServerHost, Convert.ToInt32(Properties.Settings.Default.ImapServerPort), SecureSocketOptions.Auto);
                    client.Authenticate(Properties.Settings.Default.ImapUsername, Properties.Settings.Default.ImapPassword);
                    client.Inbox.Open(FolderAccess.ReadWrite);
                    var query = MailKit.Search.SearchQuery.DeliveredAfter(new DateTime(2025, 09, 04));//[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
                    var results = client.Inbox.Search(query)?.OrderByDescending(i => i.Id);



                    List<Email> AllEmailsCreationDateList = OtmStartUp.GetEmailCreatedIn_V2670();
                    foreach (var uniqueId in results)
                    {
                        try
                        {
                            var message = client.Inbox.GetMessage(uniqueId);
                            //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
                            TimeZoneInfo Tz = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneSetting);
                            DateTime timeInTz = TimeZoneInfo.ConvertTimeFromUtc(message.Date.UtcDateTime, Tz);
                            //[rdixit][GEOS2-9624][03.10.2025] //V.2.6.8.0 Date validation ignorng the miliseconds
                            bool alreadyExists = AllEmailsCreationDateList.Any(i =>
                                i.EmailSentAt.HasValue &&
                                i.EmailSentAt.Value.Year == timeInTz.Year &&
                                i.EmailSentAt.Value.Month == timeInTz.Month &&
                                i.EmailSentAt.Value.Day == timeInTz.Day &&
                                i.EmailSentAt.Value.Hour == timeInTz.Hour &&
                                i.EmailSentAt.Value.Minute == timeInTz.Minute &&
                                i.EmailSentAt.Value.Second == timeInTz.Second &&
                                string.Equals(i.Subject?.Trim(), message.Subject?.Trim(), StringComparison.OrdinalIgnoreCase)
                            );

                            if (!alreadyExists)
                            {
                                ProcessEmail(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("An error occurred while checking the email inbox..... - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    client.Disconnect(true);
                }
                Log4NetLogger.Logger.Log(string.Format("CheckUnseenEmails() Service Executed"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("An error occurred while checking the email inbox..... - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  //[pramod.misal][10-111-2025][GEOS2-9023]https://helpdesk.emdep.com/browse/GEOS2-9023
        /// </summary>
        public void CheckCloudEmails()
        {
            Log4NetLogger.Logger.Log("CheckCloudEmails() Service Starting", category: Category.Info, priority: Priority.Low);
            try
            {
                if (string.IsNullOrEmpty(TimeZoneSetting))
                    TimeZoneSetting = OtmStartUp.GetTimezoneByServiceUrl_V2670(Properties.Settings.Default.SERVICE_PROVIDER_URL);

                 var credential = new ClientSecretCredential
                (
                  Properties.Settings.Default.TenantID,  // tenantId
                  Properties.Settings.Default.ClientID,  // clientId
                  Properties.Settings.Default.ClientSecret // clientSecret
                );


                var UserMail = Properties.Settings.Default.ImapUsername;


                var graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

                var allMessages = new List<Microsoft.Graph.Models.Message>();
                string nextLink = null;


                Log4NetLogger.Logger.Log($"Connecting to cloud mailbox: {UserMail}", category: Category.Info, priority: Priority.Low);

                // Step 3: Fetch all pages of messages synchronously
                do
                {
                    var page = nextLink == null
                        ? graphClient.Users[UserMail].MailFolders["Inbox"].Messages
                            .GetAsync(req =>
                            {
                                req.QueryParameters.Select = new[]
                                {
                            "id", "subject", "from", "toRecipients", "ccRecipients",
                            "receivedDateTime", "isRead", "bodyPreview"
                                };
                                req.QueryParameters.Orderby = new[] { "receivedDateTime desc" };
                                
                            }).GetAwaiter().GetResult()
                        : graphClient.Users[UserMail].MailFolders["Inbox"].Messages
                            .WithUrl(nextLink)
                            .GetAsync().GetAwaiter().GetResult();

                    if (page?.Value != null && page.Value.Count > 0)
                    {
                        allMessages.AddRange(page.Value);
                        //Log4NetLogger.Logger.Log($"Loaded {allMessages.Count} messages so far...", category: Category.Info, priority: Priority.Low);
                    }

                    nextLink = page?.OdataNextLink;


                } while (!string.IsNullOrEmpty(nextLink));


                Log4NetLogger.Logger.Log("Total messages fetched: {allMessages.Count}", category: Category.Info, priority: Priority.Low);

                // Step 4: Get list of already saved email IDs from DB
                //List<Email> existingEmails = OtmStartUp.GetEmailCreatedIn_V2670();

               
                // Step 4: Process each email
                foreach (var msg in allMessages)
                {
                    try
                    {
                        // Step 4.1: Get full message details (with attachments)
                        var fullMessage = graphClient.Users[UserMail].Messages[msg.Id]
                            .GetAsync(req =>
                            {
                                req.QueryParameters.Expand = new[] { "attachments" };
                                req.QueryParameters.Select = new[]
                                {
                                     "subject", "from", "toRecipients", "ccRecipients",
                                     "receivedDateTime", "body", "attachments"
                                };
                            }).GetAwaiter().GetResult();
                      

                        if (fullMessage == null)
                            continue;

                        

                        // Step 4.2: Convert Graph message to MimeMessage
                        var mimeMessage = new MimeMessage();

                        // From
                        if (fullMessage.From?.EmailAddress != null)
                        {
                            mimeMessage.From.Add(new MailboxAddress(
                                fullMessage.From.EmailAddress.Name,
                                fullMessage.From.EmailAddress.Address));
                        }

                        // To
                        if (fullMessage.ToRecipients != null)
                        {
                            foreach (var to in fullMessage.ToRecipients)
                            {
                                mimeMessage.To.Add(new MailboxAddress(to.EmailAddress?.Name, to.EmailAddress?.Address));
                            }
                        }

                        // CC
                        if (fullMessage.CcRecipients != null)
                        {
                            foreach (var cc in fullMessage.CcRecipients)
                            {
                                mimeMessage.Cc.Add(new MailboxAddress(cc.EmailAddress?.Name, cc.EmailAddress?.Address));
                            }
                        }

                        mimeMessage.Subject = fullMessage.Subject ?? "(No Subject)";



                        // Body (HTML preferred)
                        var builder = new BodyBuilder
                        {
                            HtmlBody = fullMessage.Body?.Content ?? string.Empty
                        };

                        // Find inline CID references inside body
                        var htmlBody = fullMessage.Body?.Content ?? string.Empty;
                        var cidRegex = new Regex(@"cid:(?<cid>[^\\""'>]+)", RegexOptions.IgnoreCase);

                        var inlineCids = cidRegex.Matches(htmlBody).Cast<Match>().Select(m => m.Groups["cid"].Value.Trim('<', '>')).ToHashSet();

                        // Filter ONLY real attachments
                        var realAttachments = fullMessage.Attachments?
                            .OfType<FileAttachment>()
                            .Where(a =>
                                (string.IsNullOrEmpty(a.ContentId) || !inlineCids.Contains(a.ContentId.Trim('<', '>'))) &&
                                !a.Name.EndsWith(".p7s", StringComparison.OrdinalIgnoreCase) &&
                                (a.ContentBytes?.Length ?? 0) > 200
                            )
                            .ToList() ?? new List<FileAttachment>();


                        // Add valid attachments
                        foreach (var file in realAttachments)
                        {
                            var fileName = file.Name ?? Guid.NewGuid().ToString();
                            builder.Attachments.Add(fileName, file.ContentBytes);
                        }



                        // Step 4.3: Add attachments
                        //if (fullMessage.Attachments != null && fullMessage.Attachments.Any())
                        //{
                        //    foreach (var attachment in fullMessage.Attachments)
                        //    {
                        //        if (attachment is FileAttachment fileAttachment)
                        //        {
                        //            var fileName = fileAttachment.Name ?? Guid.NewGuid().ToString();
                        //            builder.Attachments.Add(fileName, fileAttachment.ContentBytes);
                        //        }
                        //    }
                        //}




                        mimeMessage.Body = builder.ToMessageBody();
                        


                        //[pramod.misal][10-111-2025][GEOS2-9023] 
                        if (fullMessage.ReceivedDateTime.HasValue)
                        {
                                // ORIGINAL date from server with offset
                                DateTimeOffset receivedDto = fullMessage.ReceivedDateTime.Value;

                                // Convert to your plant timezone
                                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneSetting);

                                DateTime receivedInTargetTz = TimeZoneInfo.ConvertTime(receivedDto, timeZone).DateTime;

                                mimeMessage.Date = new DateTimeOffset(receivedInTargetTz);

                                // Remove milliseconds to standardize
                                receivedInTargetTz = new DateTime(
                                    receivedInTargetTz.Year,
                                    receivedInTargetTz.Month,
                                    receivedInTargetTz.Day,
                                    receivedInTargetTz.Hour,
                                    receivedInTargetTz.Minute,
                                    receivedInTargetTz.Second
                                );

                                // Load email list again for duplication check
                                List<Email> allEmailsCreationDateList = OtmStartUp.GetEmailCreatedIn_V2670();

                                // Date comparison (ignore milliseconds)
                                bool alreadyExists = allEmailsCreationDateList.Any(i =>
                                    i.EmailSentAt.HasValue &&
                                    i.EmailSentAt.Value.Year == receivedInTargetTz.Year &&
                                    i.EmailSentAt.Value.Month == receivedInTargetTz.Month &&
                                    i.EmailSentAt.Value.Day == receivedInTargetTz.Day &&
                                    i.EmailSentAt.Value.Hour == receivedInTargetTz.Hour &&
                                    i.EmailSentAt.Value.Minute == receivedInTargetTz.Minute &&
                                    i.EmailSentAt.Value.Second == receivedInTargetTz.Second &&
                                    string.Equals(i.Subject?.Trim(), mimeMessage.Subject?.Trim(), StringComparison.OrdinalIgnoreCase)
                                );

                                if (!alreadyExists)
                                {
                                    ProcessEmail(mimeMessage);
                                }


                        }
                        else
                        {
                                Log4NetLogger.Logger.Log("Processing cloud email - ReceivedDateTime.HasValue = " + fullMessage.ReceivedDateTime.HasValue, category: Category.Info, priority: Priority.Low);

                        }
                                                                       

                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log("Error processing cloud email - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                    }
                }


                Log4NetLogger.Logger.Log("CheckCloudEmails() Service Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in CheckCloudEmails(): {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][03-11-2025]https://helpdesk.emdep.com/browse/GEOS2-10052
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ProcessEmail(MimeMessage message)
        {
            bool isSave = false;

            Email emaildetails = new Email();
            try
            {
                Log4NetLogger.Logger.Log(string.Format("ProcessEmail() Service Starting"), category: Category.Info, priority: Priority.Low);

                #region Commented for //[pramod.misal][10-111-2025][GEOS2-9023]https://helpdesk.emdep.com/browse/GEOS2-9023
                //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
                //DateTime utcTime = message.Date.UtcDateTime; 
                //TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneSetting);
                //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                #endregion

                //[pramod.misal][10-111-2025][GEOS2-9023]https://helpdesk.emdep.com/browse/GEOS2-9023
                DateTimeOffset dto = message.Date;
                DateTime localTime = dto.DateTime;

                emaildetails.SenderEmail = message.From?.Mailboxes.FirstOrDefault()?.Address;
                //emaildetails.SenderName = message.From?.Mailboxes.FirstOrDefault()?.Name;  old code
                emaildetails.SenderName = message.From?.Mailboxes?.FirstOrDefault()?.Name?? "";  // DB we have NOT NULL for  SenderName
                emaildetails.CCEmail = string.Join(";", message.Cc?.Mailboxes?.Select(m => m.Address).ToList());
                emaildetails.CCName = string.Join(",", message.Cc?.Mailboxes?.Select(m => m.Name?.Replace(",", "")).Where(name => !string.IsNullOrWhiteSpace(name)).ToList());
                emaildetails.BCCEmail = string.Join(";", message.Bcc?.Mailboxes?.Select(m => m.Address).ToList());
                emaildetails.Subject = message.Subject?.ToString();
                emaildetails.Body = message.HtmlBody;
                //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                emaildetails.CreatedIn = DateTime.Now;
                //V.2.6.8.0 Date validation ignorng the miliseconds
                emaildetails.EmailSentAt = new DateTime(localTime.Year, localTime.Month, localTime.Day, localTime.Hour, localTime.Minute, localTime.Second); //{25/11/2025 18:17:29}
                //End
                emaildetails.RecipientEmail = string.Join(";", message.To?.Mailboxes?.Select(m => m.Address).ToList());
                emaildetails.RecipientName = string.Join(",", message.To?.Mailboxes?
                .Select(m => m.Name?.Replace(",", ""))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList());//[rdixit][GEOS2-9145][08.08.2025]

                emaildetails.SourceInboxId = Properties.Settings.Default.ImapUsername;
                foreach (var attachment in message.Attachments)
                {
                    try
                    {
                        Emailattachment Attachment = new Emailattachment();
                        string originalFileName = attachment.ContentDisposition?.FileName ?? Guid.NewGuid().ToString();
                        List<string> existingFileNames = emaildetails.EmailattachmentList?.Select(a => a.AttachmentName).ToList();
                        string uniqueFileName = GetUniqueFileName(originalFileName, existingFileNames);
                        Attachment.AttachmentName = RemoveInvalidPathChars(uniqueFileName);//[001]
                        Attachment.AttachmentExtension = Path.GetExtension(uniqueFileName);
                        Attachment.CreatedBy = 1;
                        Attachment.CreatedIn = DateTime.Now;
                        using (var memoryStream = new MemoryStream())
                        {
                            if (attachment is MessagePart)
                            {
                                var part = (MessagePart)attachment;
                                part.Message.WriteTo(memoryStream);
                            }
                            else
                            {
                                var part = (MimePart)attachment;
                                part.Content.DecodeTo(memoryStream);
                            }
                            Attachment.FileContent = memoryStream.ToArray();
                        }
                        if (emaildetails.EmailattachmentList == null)
                            emaildetails.EmailattachmentList = new List<Emailattachment>();

                        //[rdixit][GEOS2-9020][23.07.2025]
                        if (Attachment.AttachmentExtension?.ToLower() == ".xml")
                        {
                            string directoryPath = @"C:\Temp\POTemp";
                            string filePath = Path.Combine(directoryPath, "Po.xml");
                            try
                            {
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                File.WriteAllBytes(filePath, Attachment.FileContent);
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.Load(filePath);
                                XmlNode quotationNode = xmlDoc.SelectSingleNode("//Quotation");

                                if (quotationNode != null)
                                {
                                    emaildetails.IdCustomer = Convert.ToInt32(quotationNode.SelectSingleNode("customer")?.InnerText);
                                    emaildetails.IdPlant = Convert.ToInt32(quotationNode.SelectSingleNode("site")?.InnerText);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("An error occurred while checking the email inbox .xml.....{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            finally
                            {
                                if (Directory.Exists(directoryPath) && !Directory.EnumerateFiles(directoryPath).Any())
                                {
                                    Directory.Delete(directoryPath);
                                }
                            }
                        }

                        emaildetails.EmailattachmentList.Add(Attachment);
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("An error occurred while checking the email attachment inbox.....- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                //[rdixit][30.07.2024][GEOS2-6005]
                //isSave = OtmStartUp.AddEmails_V2570(emaildetails); //[GEOS2-5868][rdixit][14.10.2024]
                //isSave = OtmStartUp.AddEmails_V2610(emaildetails);   //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                //[pramod.misal][GEOS2-7724][07/04/2025]
                //[rdixit][GEOS2-9020][23.07.2025]
                //OtmStartUp = new OTMServiceController("localhost:6699");
                isSave = OtmStartUp.AddEmails_V2660(emaildetails);

                Log4NetLogger.Logger.Log(string.Format("ProcessEmail() Service Executed for email " + emaildetails.Subject), category: Category.Info, priority: Priority.Low);
            }
            catch (SmtpException smtpEx)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in method ProcessEmail() - {0}", smtpEx.Message), category: Category.Exception, priority: Priority.Low);
                if (smtpEx.InnerException != null)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in method ProcessEmail() - {0}", smtpEx.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in method ProcessEmail() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            return isSave;
        }
        public static string GetUniqueFileName(string fileName, List<string> existingFileNames)
        {
            Log4NetLogger.Logger.Log(string.Format("GetUniqueFileName() Service Starting"), category: Category.Info, priority: Priority.Low);
            if (existingFileNames == null || !existingFileNames.Contains(fileName))
            {
                return fileName;
            }

            string baseFileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);
            int copyNumber = 1;

            string newFileName;
            do
            {
                newFileName = $"{baseFileName}-Copy{(copyNumber > 1 ? $"({copyNumber})" : string.Empty)}{extension}";
                copyNumber++;
            } while (existingFileNames.Contains(newFileName));
            Log4NetLogger.Logger.Log(string.Format("GetUniqueFileName() Service Executed"), category: Category.Info, priority: Priority.Low);
            return newFileName;
        }
        protected override void OnStart(string[] args)
        {
            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Starting"), category: Category.Info, priority: Priority.Low);

            InitiaizeOnStartService();

            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Executed"), category: Category.Info, priority: Priority.Low);
        }
        protected override void OnStop()
        {
            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stopping"), category: Category.Info, priority: Priority.Low);

            if (EmailInboxMonitortimer != null)
                EmailInboxMonitortimer.Enabled = false;

            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stoped"), category: Category.Info, priority: Priority.Low);
        }
        internal void InitiaizeOnStartService()
        {
            try
            {
                EmailInboxMonitortimer = new Timer();
                this.EmailInboxMonitortimer.Interval = Convert.ToInt32(Properties.Settings.Default.INTERVAL_REFRESH);
                this.EmailInboxMonitortimer.Elapsed += new ElapsedEventHandler(this.EmailInboxMonitor_Tick);
                EmailInboxMonitortimer.Enabled = true;
            }
            catch (FaultException<Emdep.Geos.Services.Contracts.ServiceException> ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-9189][rdixit][12.08.2025]
        private void EmailInboxMonitor_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log4NetLogger.Logger.Log("EmailInboxMonitor_Tick() started.", Category.Info, Priority.Low);
                if (EmailInboxMonitortimer != null)
                    EmailInboxMonitortimer.Enabled = false;

                lock (_object)
                {
                    //CheckUnseenEmails(); commented for  //[pramod.misal][10-111-2025][GEOS2-9023] https://helpdesk.emdep.com/browse/GEOS2-9023 now fetch the mails from cloud
                    CheckCloudEmails();//[pramod.misal][10-11-2025][GEOS2-9023] https://helpdesk.emdep.com/browse/GEOS2-9023
                }
                Log4NetLogger.Logger.Log("EmailInboxMonitor_Tick() executed.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"EmailInboxMonitor_Tick() - Error: {ex}", Category.Exception, Priority.Low);
            }
            finally
            {
                if (EmailInboxMonitortimer != null)
                    EmailInboxMonitortimer.Enabled = true;
            }
         
        }
        /// <summary>
        /// [ashish.malkhede][03-11-2025]https://helpdesk.emdep.com/browse/GEOS2-10052
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string RemoveInvalidPathChars(string name)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c.ToString(), string.Empty);
            }
            return name.Trim();
        }

        #endregion
    }
}
