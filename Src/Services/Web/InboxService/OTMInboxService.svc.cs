using InboxService.Data;
using InboxService.Logger;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace InboxService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OTMInboxService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OTMInboxService.svc or OTMInboxService.svc.cs at the Solution Explorer and start debugging.
    public class OTMInboxService : IOTMInboxService
    {
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public OTMInboxService()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }             
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  OTMInboxService() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            Log4NetLogger.Logger.Log(string.Format("OTMInboxService Constructor Executed.... "), category: Category.Info, priority: Priority.Low);

        }

        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public bool AddEmails(email emailDetails)
        {
            #region AddEmails
            try
            {
                Log4NetLogger.Logger.Log(string.Format("AddEmails Constructor Started.... "), category: Category.Info, priority: Priority.Low);
                using (var context = new geosEntities())
                {
                    try
                    {         
                        context.emails.Add(emailDetails);
                        context.SaveChanges();
                        var emailId = emailDetails.IdEmail;

                        using (var context1 = new geosEntities1())
                        {
                            if (emailDetails.Attachments?.Count > 0)
                            {
                                foreach (var attachment in emailDetails.Attachments)
                                {
                                    attachment.AttachmentPath = Path.Combine(Properties.Settings.Default.AttachmentSavePath,emailId.ToString(), attachment.AttachmentName);
                                    attachment.IdEmail = emailId;
                                    context1.emailattachments.Add(attachment);
                                    context1.SaveChanges();

                                    if (attachment.FileContent != null)
                                        InsertAttachment(attachment, Path.Combine(Properties.Settings.Default.AttachmentSavePath,emailId.ToString()));
                                }
                            }                         
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error AddEmails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }

                    Log4NetLogger.Logger.Log(string.Format("AddEmails Constructor Executed.... "), category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddEmails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            #endregion

            return true;
        }
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public bool InsertAttachment(emailattachment attachment, string ConnectorAttachedDocPath)
        {
            string filePath = ConnectorAttachedDocPath + "\\" + attachment.AttachmentName;
            try
            {
                if (!Directory.Exists(ConnectorAttachedDocPath))
                {
                    Directory.CreateDirectory(ConnectorAttachedDocPath);
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                File.WriteAllBytes(filePath, attachment.FileContent);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
    }
}
