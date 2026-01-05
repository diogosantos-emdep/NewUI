using DevExpress.XtraSpreadsheet.Import.Xls;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.OTMDataModel;
using Emdep.Geos.Data.Common.TechnicalRestService;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using Org.BouncyCastle.Ocsp;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Media.Imaging;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using ShippingAddress = Emdep.Geos.Data.Common.OTM.ShippingAddress;
namespace Emdep.Geos.Data.BusinessLogic
{
    public class OTMManager
    {

        public OTMManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);//[GEOS2-5404][rdixit][13.08.2024]
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("OTMManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch
            {
                throw;
            }
        }
        //[GEOS2-5404][rdixit][13.08.2024]
        void CreateIfNotExists(string config_path)
        {
            string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\LogsService.txt""/>
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
        public bool IsConnectionStringNameExist(string Name)
        {
            bool isExist = false;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == Name)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
            }
            return isExist;
        }
        public bool AddEmails(email emailDetails, string path)
        {
            #region AddEmails
            try
            {
                Log4NetLogger.Logger.Log(string.Format("AddEmails Constructor Started.... "), category: Category.Info, priority: Priority.Low);
                List<emailattachment> attach = emailDetails.emailattachments.ToList();
                emailDetails.emailattachments = null;
                using (var context = new OtmEntities())
                {
                    try
                    {
                        context.emails.Add(emailDetails);
                        context.SaveChanges();
                        var emailId = emailDetails.IdEmail;
                        if (attach?.Count > 0)
                        {
                            foreach (var attachment in attach)
                            {
                                attachment.AttachmentPath = Path.Combine(path, emailId.ToString(), attachment.AttachmentName);
                                attachment.IdEmail = emailId;
                                context.emailattachments.Add(attachment);
                                context.SaveChanges();
                                if (attachment.FileContent != null)
                                    InsertAttachment(attachment, Path.Combine(path, emailId.ToString()));
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
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public bool AddEmails(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_RecipientName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_RecipientEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CreatedBy", emailDetails.CreatedBy);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public void AddEmailAttachedDoc(string mainServerConnectionString, List<Emailattachment> emailattachmentList, string attachedDocPath)
        {
            try
            {
                if (emailattachmentList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (Emailattachment attachdoc in emailattachmentList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailAttachmentDetails", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdEmail", attachdoc.IdEmail);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentName", attachdoc.AttachmentName);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentPath", Path.Combine(attachedDocPath, attachdoc.IdEmail.ToString(), attachdoc.AttachmentName));
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", attachdoc.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentExtension", attachdoc.AttachmentExtension);
                            attachdoc.IdAttachment = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                            if (attachdoc.IdAttachment > 0)
                            {
                                InsertAttachment(attachdoc, Path.Combine(attachedDocPath, attachdoc.IdEmail.ToString()));
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddEmailAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[GEOS2-5531][GEOS2-5533][GEOS2-5535][GEOS2-5536][rdixit][25.06.2024]
        public bool InsertAttachment(Emailattachment attachment, string ConnectorAttachedDocPath)
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
        //[rdixit][30.07.2024][GEOS2-6005]
        public bool AddEmails_V2550(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails_V2550", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_CCEmail", emailDetails.CCEmail);
                        mySqlCommand.Parameters.AddWithValue("_ToName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_ToEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CCName", emailDetails.CCName);
                        mySqlCommand.Parameters.AddWithValue("_SourceInboxId", emailDetails.SourceInboxId);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc_V2550(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }
        #region [GEOS2-5867][rdixit][25.07.2024]
        public List<Email> GetAllUnprocessedEmails(string ConnectionStringgeos, string ConnectionStringemdep_geos, string poAnalyzerEmailToCheck, string attachedDocPath)
        {
            List<Email> EmailList = new List<Email>();
            List<LookupValue> UntructedExtensionList = GetLookupValues(ConnectionStringemdep_geos, 157);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetUnProcessedEmails_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["IsDeleted"] != DBNull.Value)
                                    email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                try
                                {
                                    if (rdr["IdEmail"] != DBNull.Value)
                                    {
                                        Emailattachment attach = new Emailattachment();
                                        Email email = EmailList.FirstOrDefault(i => i.IdEmail == Convert.ToInt32(rdr["IdEmail"]));
                                        if (email != null)
                                        {
                                            if (email.EmailattachmentList == null)
                                            {
                                                email.EmailattachmentList = new List<Emailattachment>();
                                            }
                                            if (rdr["IdEmail"] != DBNull.Value)
                                                attach.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                            if (rdr["IdAttachment"] != DBNull.Value)
                                                attach.IdAttachment = Convert.ToInt32(rdr["IdAttachment"]);
                                            if (rdr["AttachmentName"] != DBNull.Value)
                                                attach.AttachmentName = Convert.ToString(rdr["AttachmentName"]);
                                            if (rdr["AttachmentPath"] != DBNull.Value)
                                                attach.AttachmentPath = Convert.ToString(rdr["AttachmentPath"]);
                                            if (rdr["CreatedIn"] != DBNull.Value)
                                                email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                            if (rdr["ModifiedIn"] != DBNull.Value)
                                                email.ModifiedIn = rdr["ModifiedIn"] as DateTime?;
                                            if (rdr["IsDeleted"] != DBNull.Value)
                                                email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                            if (rdr["CreatedBy"] != DBNull.Value)
                                                email.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);
                                            if (rdr["ModifiedBy"] != DBNull.Value)
                                                email.ModifiedBy = rdr["ModifiedBy"] as int?;
                                            if (rdr["AttachmentExtension"] != DBNull.Value)
                                                attach.AttachmentExtension = Convert.ToString(rdr["AttachmentExtension"]);
                                            //attach.FileContent = GetEmailAttachedDoc(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                            if (!UntructedExtensionList.Any(j => j.Value?.Trim() == attach.AttachmentExtension?.Trim()))
                                            {
                                                if (attach.AttachmentExtension == ".pdf")
                                                    attach.FileText = GetPdfAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                            }
                                        }
                                        email.EmailattachmentList.Add(attach);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        public List<BlackListEmail> GetAllBlackListEmails(string ConnectionString)
        {
            List<BlackListEmail> EmailList = new List<BlackListEmail>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetBlacklistEmails_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            BlackListEmail email = new BlackListEmail();
                            try
                            {
                                if (empReader["IdBlacklist"] != DBNull.Value)
                                    email.IdBlackListEmail = Convert.ToInt32(empReader["IdBlacklist"]);
                                if (empReader["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = empReader["SenderEmail"].ToString();
                                if (empReader["Domain"] != DBNull.Value)
                                    email.Domain = empReader["Domain"].ToString();
                                if (empReader["IsDeleted"] != DBNull.Value)
                                    email.IsDeleted = Convert.ToInt32(empReader["IsDeleted"]);
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllBlackListEmails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        public List<LookupValue> GetLookupValues(string ConnectionString, byte key)
        {
            List<LookupValue> UnTrustedExtentionList = new List<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("lookupvalues_GetlookupvaluesByKey", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdLookupKey", key);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);
                                if (empReader["Abbreviation"] != DBNull.Value)
                                    lookupval.Abbreviation = empReader["Abbreviation"].ToString();
                                if (empReader["Value"] != DBNull.Value)
                                    lookupval.Value = empReader["Value"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            UnTrustedExtentionList.Add(lookupval);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLookupValues(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return UnTrustedExtentionList;
        }
        public string GetPdfAttachedDocText(long idEmail, string attachedDocPath, string SavedFileName)
        {
            var pages = "";
            try
            {
                string fileUploadPath = Path.Combine(attachedDocPath, idEmail.ToString(), SavedFileName);
                if (File.Exists(fileUploadPath))
                {
                    using (PdfReader pdfReader = new PdfReader(fileUploadPath))
                    using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
                    {
                        for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                        {
                            try
                            {
                                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                                string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                                pages += pageContent;
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPdfAttachedDocText(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return pages;
        }
        public string GetPdfAttachedDocTextByLocation(long idEmail, string attachedDocPath, string SavedFileName)
        {
            var pages = "";
            try
            {
                string fileUploadPath = Path.Combine(attachedDocPath, idEmail.ToString(), SavedFileName);
                if (File.Exists(fileUploadPath))
                {
                    using (PdfReader pdfReader = new PdfReader(fileUploadPath))
                    using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
                    {
                        for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                        {
                            try
                            {
                                ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                                string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                                pages += pageContent;
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPdfAttachedDocText(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return pages;
        }
        public bool AddPORequest(PORequest poRequest, string mainServerConnectionString)
        {
            try
            {
                if (poRequest != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddPORequest_V2550", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdEmail", poRequest.IdEmail);
                        mySqlCommand.Parameters.AddWithValue("_PORequestStatus", poRequest.PORequestStatus?.IdLookupValue);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPORequest(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        public bool AddPODetails(List<CustomerDetail> poDetailList, string mainServerConnectionString)
        {
            try
            {
                if (poDetailList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (CustomerDetail poReq in poDetailList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddPODetails_V2550", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_PONumber", poReq.PONumber);
                            mySqlCommand.Parameters.AddWithValue("_PODate", poReq.DateIssued.Date);
                            mySqlCommand.Parameters.AddWithValue("_TransferAmount", poReq.TotalNetValue);
                            mySqlCommand.Parameters.AddWithValue("_Currency", poReq.Currency);
                            mySqlCommand.Parameters.AddWithValue("_Incoterm", poReq.Incoterm);
                            mySqlCommand.Parameters.AddWithValue("_Email", poReq.Email);
                            mySqlCommand.Parameters.AddWithValue("_Customer", poReq.Customer);
                            mySqlCommand.Parameters.AddWithValue("_Offer", poReq.Offer);
                            mySqlCommand.Parameters.AddWithValue("_IntegrationDate", DateTime.Now);
                            mySqlCommand.Parameters.AddWithValue("_IdIntegrationUser", 1);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPODetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        public List<TemplateSetting> GetAllTags(string ConnectionString)
        {
            List<TemplateSetting> TemplateList = new List<TemplateSetting>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllTemplateTags_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            TemplateSetting template = new TemplateSetting();
                            try
                            {
                                if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    template.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                if (empReader["TemplateName"] != DBNull.Value)
                                    template.TemplateName = empReader["TemplateName"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            TemplateList.Add(template);
                        }
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                LookupValue tag = new LookupValue();
                                try
                                {
                                    if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    {
                                        TemplateSetting exp = TemplateList.Where(i => i.IdTemplateSettings == Convert.ToInt32(empReader["IdTemplate_settings"])).FirstOrDefault();
                                        if (exp.TagList == null)
                                        {
                                            exp.TagList = new List<LookupValue>();
                                        }
                                        if (empReader["idTag"] != DBNull.Value)
                                            tag.IdLookupValue = Convert.ToInt32(empReader["idTag"]);
                                        if (empReader["Tag"] != DBNull.Value)
                                            tag.Value = Convert.ToString(empReader["Tag"]);
                                        if (empReader["IdTemplate_settings"] != DBNull.Value)
                                            tag.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                        exp.TagList.Add(tag);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                TemplateTag tagvalue = new TemplateTag();
                                LookupValue Tag = null;
                                try
                                {
                                    if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    {
                                        TemplateSetting exp = TemplateList.Where(i => i.IdTemplateSettings == Convert.ToInt32(empReader["IdTemplate_settings"])).FirstOrDefault();
                                        if (exp != null)
                                        {
                                            if (exp.TagList != null)
                                            {
                                                Tag = exp.TagList.FirstOrDefault(i => i.IdLookupValue == Convert.ToInt32(empReader["idTag"]));
                                            }
                                            if (Tag != null)
                                            {
                                                if (Tag.TagValueList == null)
                                                    Tag.TagValueList = new List<TemplateTag>();
                                                if (empReader["IdTemplate_settings"] != DBNull.Value)
                                                    tagvalue.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                                if (empReader["TagValue"] != DBNull.Value)
                                                    tagvalue.TagValue = Convert.ToString(empReader["TagValue"]);
                                                if (empReader["IdTemplateTags"] != DBNull.Value)
                                                    tagvalue.IdTemplateTags = Convert.ToInt32(empReader["IdTemplateTags"]);
                                                if (empReader["idTag"] != DBNull.Value)
                                                    tagvalue.IdTag = Convert.ToInt32(empReader["idTag"]);
                                                if (empReader["Skip"] != DBNull.Value)
                                                    tagvalue.SkipValue = Convert.ToInt32(empReader["Skip"]);
                                                Tag.TagValueList.Add(tagvalue);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllTags(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return TemplateList;
        }
        public List<Currency> GetAllCurrencies(string ConnectionStringGeos)
        {
            List<Currency> CurrencyList = new List<Currency>();
            Currency defaultCurrency = new Currency();
            defaultCurrency.IdCurrency = 0;
            defaultCurrency.Name = "---";
            CurrencyList.Add(defaultCurrency);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCurrency_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Currency cur = new Currency();
                            try
                            {
                                if (rdr["IdCurrency"] != DBNull.Value)
                                    cur.IdCurrency = Convert.ToByte(rdr["IdCurrency"]);
                                if (rdr["Name"] != DBNull.Value)
                                    cur.Name = rdr["Name"].ToString();
                                if (rdr["Symbol"] != DBNull.Value)
                                    cur.Symbol = rdr["Symbol"].ToString();
                                if (rdr["Description"] != DBNull.Value)
                                    cur.Description = rdr["Description"].ToString();
                                if (rdr["CodeN"] != DBNull.Value)
                                    cur.CodeN = Convert.ToInt32(rdr["CodeN"]);
                            }
                            catch (Exception ex)
                            { }
                            CurrencyList.Add(cur);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCurrencies(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CurrencyList;
        }
        public void AddEmailAttachedDoc_V2550(string mainServerConnectionString, List<Emailattachment> emailattachmentList, string attachedDocPath)
        {
            try
            {
                if (emailattachmentList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (Emailattachment attachdoc in emailattachmentList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailAttachmentDetails_V2550", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdEmail", attachdoc.IdEmail);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentName", attachdoc.AttachmentName);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentPath", Path.Combine(attachedDocPath, attachdoc.IdEmail.ToString(), attachdoc.AttachmentName));
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", attachdoc.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_AttachmentExtension", attachdoc.AttachmentExtension);
                            attachdoc.IdAttachment = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                            if (attachdoc.IdAttachment > 0)
                            {
                                InsertAttachment(attachdoc, Path.Combine(attachedDocPath, attachdoc.IdEmail.ToString()));
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddEmailAttachedDoc_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001] [ashish.malkhede][08-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <param name="correnciesIconFilePath"></param>
        /// <returns></returns>
        public List<PORegisteredDetails> GetPORegisteredDetails(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, string correnciesIconFilePath, string countryIconFilePath, PORegisteredDetailFilter filter)
        {
            List<PORegisteredDetails> poRegiList = new List<PORegisteredDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                List<string> countryISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        if (filter == null)
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetails_V2580";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                        }
                        else
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsFilter_V2580";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandTimeout = 6000;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_Number", filter.Number);
                            mySqlCommand.Parameters.AddWithValue("_Type", filter.IdType);
                            mySqlCommand.Parameters.AddWithValue("_Group", filter.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_PlantC", filter.IdCustomerPlant);
                            mySqlCommand.Parameters.AddWithValue("_Sender", filter.Sender);
                            mySqlCommand.Parameters.AddWithValue("_Currency", filter.IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeFrom", filter.PoValueRangeFrom);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeTo", filter.PoValueRangeTo);
                            mySqlCommand.Parameters.AddWithValue("_Offer", filter.Offer);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateFrom", filter.ReceptionDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateTo", filter.ReceptionDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateFrom", filter.CreationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateTo", filter.CreationDateTo);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateFrom", filter.UpdateDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateTO", filter.UpdateDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateFrom", filter.CancellationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateTo", filter.CancellationDateTo);
                        }
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORegisteredDetails po = new PORegisteredDetails();
                                try
                                {
                                    if (rdr["IdPO"] != DBNull.Value)
                                        po.IdPO = Convert.ToInt64(rdr["IdPO"]);
                                    if (rdr["Code"] != DBNull.Value)
                                        po.Code = rdr["Code"].ToString();
                                    if (rdr["IdPOType"] != DBNull.Value)
                                        po.IdPOType = Convert.ToInt32(rdr["IdPOType"]);
                                    if (rdr["Type"] != DBNull.Value)
                                        po.Type = rdr["Type"].ToString();
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["Plant"] != DBNull.Value)
                                        po.Plant = rdr["Plant"].ToString();
                                    if (rdr["Country"] != DBNull.Value)
                                    {
                                        po.Country = rdr["Country"].ToString();
                                        po.CountryISO = rdr["CountryISO"].ToString();
                                    }
                                    if (rdr["Region"] != DBNull.Value)
                                        po.Region = rdr["Region"].ToString();
                                    if (rdr["ReceptionDate"] != DBNull.Value)
                                        po.ReceptionDate = Convert.ToDateTime(rdr["ReceptionDate"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["POValue"] != DBNull.Value)
                                        po.POValue = Convert.ToDouble(rdr["POValue"].ToString());
                                    if (rdr["Amount"] != DBNull.Value)
                                        po.Amount = Convert.ToDouble(rdr["Amount"].ToString());
                                    if (rdr["Remarks"] != DBNull.Value)
                                        po.Remarks = rdr["Remarks"].ToString();
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["LinkedOffer"] != DBNull.Value)
                                        po.LinkedOffer = rdr["LinkedOffer"].ToString();
                                    if (rdr["ShippingAddress"] != DBNull.Value)
                                        po.ShippingAddress = rdr["ShippingAddress"].ToString();
                                    if (rdr["IsOK"] != DBNull.Value)
                                        po.IsOK = rdr["IsOK"].ToString();
                                    if (rdr["Confirmation"] != DBNull.Value)
                                        po.Confirmation = rdr["Confirmation"].ToString();
                                    if (rdr["CreationDate"] != DBNull.Value)
                                        po.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);
                                    if (rdr["Creator"] != DBNull.Value)
                                        po.Creator = rdr["Creator"].ToString();
                                    if (rdr["UpdaterDate"] != DBNull.Value)
                                        po.UpdaterDate = Convert.ToDateTime(rdr["UpdaterDate"].ToString());
                                    if (rdr["Updater"] != DBNull.Value)
                                        po.Updater = rdr["Updater"].ToString();
                                    if (rdr["IsCancelled"] != DBNull.Value)
                                        po.IsCancelled = rdr["IsCancelled"].ToString();
                                    if (rdr["Canceler"] != DBNull.Value)
                                        po.Canceler = rdr["Canceler"].ToString();
                                    if (rdr["CancellationDate"] != DBNull.Value)
                                        po.CancellationDate = Convert.ToDateTime(rdr["CancellationDate"].ToString());
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (!countryISOs.Any(co => co.ToString() == po.CountryISO))
                                    {
                                        countryISOs.Add(po.CountryISO);
                                    }
                                    poRegiList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poRegiList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                            }
                            // for country
                            foreach (string item in countryISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                                poRegiList.Where(ot => ot.CountryISO == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                return poRegiList;
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <returns></returns>
        public List<PORequestDetails> GetPORequestDetails(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2570";
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["Offer"] != DBNull.Value)
                                        po.Offer = rdr["Offer"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                        po.AttachmentCount = Convert.ToInt16(rdr["attachCount"]);
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";
                                    if (rdr["IDPOfinalresult"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IDPOfinalresult"].ToString());
                                    if (rdr["PONumber"] != DBNull.Value)
                                        po.PONumber = rdr["PONumber"].ToString();
                                    else
                                        po.PONumber = "";
                                    if (rdr["PODate"] != DBNull.Value)
                                        po.DateIssued = Convert.ToDateTime(rdr["PODate"]);
                                    if (rdr["TransferAmount"] != DBNull.Value)
                                        po.TransferAmount = Convert.ToDouble(rdr["TransferAmount"]);
                                    else
                                        po.TransferAmount = 0;
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Email = rdr["Email"].ToString();
                                    if (rdr["Customer"] != DBNull.Value)
                                        po.Customer = rdr["Customer"].ToString();
                                    else
                                        po.Customer = "";
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Contact = rdr["Email"].ToString();
                                    else
                                        po.Contact = "";
                                    if (rdr["Incoterm"] != DBNull.Value)
                                        po.POIncoterms = rdr["Incoterm"].ToString();
                                    else
                                        po.POIncoterms = "";
                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["ShipTo"] != DBNull.Value)
                                        po.ShipTo = rdr["ShipTo"].ToString();
                                    else
                                        po.ShipTo = "";
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    poList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        /// <summary>
        /// [001][ashish.malkhede][04102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <param name="PLMConnectionString"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public List<Common.Company> GetAllSitesWithImagesByIdUser(string PLMConnectionString, Int32 idUser)
        {
            List<Common.Company> Companies = new List<Common.Company>();
            List<string> CompanyIcon = new List<string>();
            Dictionary<string, byte[]> DictCompanies = new Dictionary<string, byte[]>();
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(PLMConnectionString);
                // Extract components
                string ConnecteddataSource = builder.Server;
                string database = builder.Database;
                string userId = builder.UserID;
                string password = builder.Password;
                string connectedPlantName = GetConnectedPlantNameFromDataSource(ConnecteddataSource.ToUpper(), PLMConnectionString);
                string dataSource = "";
                using (MySqlConnection mySqlConnection = new MySqlConnection(PLMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAuthorizedPlantsByIdUser_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idUser", idUser);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Common.Company Company = new Common.Company();
                            Company.IdCompany = Convert.ToInt32(reader["IdSite"]);
                            if (reader["CountryISO2"] != DBNull.Value)
                            {
                                Company.Iso = Convert.ToString(reader["CountryISO2"]);
                                Company.Country = new Country();
                                if (Companies.Any(k => k.Iso == Company.Iso))
                                {
                                    Company.ImageInBytes = Companies.FirstOrDefault(j => j.Iso == Company.Iso).ImageInBytes;
                                }
                                else
                                {
                                    //using (WebClient webClient = new WebClient())
                                    //{
                                    //    Company.ImageInBytes = webClient.DownloadData("https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Company.Iso + ".png");
                                    //}
                                    Company.ImageInBytes = Utility.ImageUtil.GetImageByWebClient("https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Company.Iso + ".png");
                                }
                            }
                            if (reader["Alias"] != DBNull.Value)
                            {
                                Company.Alias = Convert.ToString(reader["Alias"]);
                            }
                            if (reader["idCountry"] != DBNull.Value)
                            {
                                Company.Country.IdCountry = Convert.ToByte(reader["idCountry"]);
                            }
                            if (reader["IdSite"] != DBNull.Value)
                            {
                                if (string.IsNullOrEmpty(connectedPlantName))
                                {
                                    dataSource = ConnecteddataSource.Replace(ConnecteddataSource, reader["DatabaseIP"].ToString());
                                }
                                else
                                {
                                    dataSource = ConnecteddataSource.Replace(connectedPlantName, reader["Alias"].ToString());
                                }
                                string connstr = PLMConnectionString.Replace(ConnecteddataSource, dataSource);
                                Company.ConnectPlantConstr = connstr;
                            }
                            Companies.Add(Company);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCompanies_V2420(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Companies;
        }
        #endregion
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<POStatus> OTM_GetAllPOWorkflowStatus(string connectionString)
        {
            List<POStatus> POStatusList = new List<POStatus>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPOWorkflowStatus_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            POStatus poStatus = new POStatus();
                            if (mySqlDataReader["Value"] != DBNull.Value)
                                poStatus.Name = mySqlDataReader["Value"].ToString();
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                                poStatus.HtmlColor = mySqlDataReader["HtmlColor"].ToString();
                            if (mySqlDataReader["IdLookupValue"] != DBNull.Value)
                                poStatus.IdLookupValue = Convert.ToInt32(mySqlDataReader["IdLookupValue"].ToString());
                            POStatusList.Add(poStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetAllPOWorkflowStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POStatusList;
        }
        // [001] [ashish.malkhede][13-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        public List<POType> OTM_GetAllPOTypeStatus(string connectionString)
        {
            List<POType> POTypeList = new List<POType>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPOType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            POType potype = new POType();
                            if (mySqlDataReader["IdPOType"] != DBNull.Value)
                                potype.IdPoType = Convert.ToInt32(mySqlDataReader["IdPOType"].ToString());
                            if (mySqlDataReader["POType"] != DBNull.Value)
                                potype.Type = mySqlDataReader["POType"].ToString();
                            POTypeList.Add(potype);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetAllPOTypeStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POTypeList;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Customer> GetAllCustomers(string connectionString)
        {
            List<Customer> CustomerList = new List<Customer>();
            Customer defaultCustomer = new Customer();
            defaultCustomer.IdCustomer = 0;
            defaultCustomer.CustomerName = "---";
            CustomerList.Add(defaultCustomer);
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCustomers", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Name"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Name"]);
                        CustomerList.Add(cust);
                    }
                }
            }
            return CustomerList;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public List<TemplateSetting> GetTemplateByCustomer(string ConnectionString, int idCustomer)
        {
            List<TemplateSetting> TemplateList = new List<TemplateSetting>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetTemplateSettingsByCustomer_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", idCustomer);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            TemplateSetting template = new TemplateSetting();
                            try
                            {
                                if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    template.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                if (empReader["TemplateName"] != DBNull.Value)
                                    template.TemplateName = empReader["TemplateName"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            TemplateList.Add(template);
                        }
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                LookupValue tag = new LookupValue();
                                try
                                {
                                    if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    {
                                        TemplateSetting exp = TemplateList.Where(i => i.IdTemplateSettings == Convert.ToInt32(empReader["IdTemplate_settings"])).FirstOrDefault();
                                        if (exp.TagList == null)
                                        {
                                            exp.TagList = new List<LookupValue>();
                                        }
                                        if (empReader["idTag"] != DBNull.Value)
                                            tag.IdLookupValue = Convert.ToInt32(empReader["idTag"]);
                                        if (empReader["Tag"] != DBNull.Value)
                                            tag.Value = Convert.ToString(empReader["Tag"]);
                                        if (empReader["IdTemplate_settings"] != DBNull.Value)
                                            tag.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                        exp.TagList.Add(tag);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                TemplateTag tagvalue = new TemplateTag();
                                LookupValue Tag = null;
                                try
                                {
                                    if (empReader["IdTemplate_settings"] != DBNull.Value)
                                    {
                                        TemplateSetting exp = TemplateList.Where(i => i.IdTemplateSettings == Convert.ToInt32(empReader["IdTemplate_settings"])).FirstOrDefault();
                                        if (exp != null)
                                        {
                                            if (exp.TagList != null)
                                            {
                                                Tag = exp.TagList.FirstOrDefault(i => i.IdLookupValue == Convert.ToInt32(empReader["idTag"]));
                                            }
                                            if (Tag != null)
                                            {
                                                if (Tag.TagValueList == null)
                                                    Tag.TagValueList = new List<TemplateTag>();
                                                if (empReader["IdTemplate_settings"] != DBNull.Value)
                                                    tagvalue.IdTemplateSettings = Convert.ToInt32(empReader["IdTemplate_settings"]);
                                                if (empReader["TagValue"] != DBNull.Value)
                                                    tagvalue.TagValue = Convert.ToString(empReader["TagValue"]);
                                                if (empReader["IdTemplateTags"] != DBNull.Value)
                                                    tagvalue.IdTemplateTags = Convert.ToInt32(empReader["IdTemplateTags"]);
                                                if (empReader["idTag"] != DBNull.Value)
                                                    tagvalue.IdTag = Convert.ToInt32(empReader["idTag"]);
                                                if (empReader["Skip"] != DBNull.Value)
                                                    tagvalue.SkipValue = Convert.ToInt32(empReader["Skip"]);
                                                if (empReader["NextValue"] != DBNull.Value)
                                                    tagvalue.NextValue = Convert.ToString(empReader["NextValue"]);
                                                Tag.TagValueList.Add(tagvalue);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTemplateByCustomer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return TemplateList;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public bool AddPODetails_V2570(List<CustomerDetail> poDetailList, string mainServerConnectionString)
        {
            try
            {
                if (poDetailList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (CustomerDetail poReq in poDetailList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddPODetails_V2570", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPORequest", poReq.IdPORequest);
                            mySqlCommand.Parameters.AddWithValue("_PONumber", poReq.PONumber);
                            mySqlCommand.Parameters.AddWithValue("_PODate", poReq.DateIssued.Date);
                            mySqlCommand.Parameters.AddWithValue("_TransferAmount", poReq.TotalNetValue);
                            mySqlCommand.Parameters.AddWithValue("_Currency", poReq.Currency);
                            mySqlCommand.Parameters.AddWithValue("_Incoterm", poReq.Incoterm);
                            mySqlCommand.Parameters.AddWithValue("_Email", poReq.Email);
                            mySqlCommand.Parameters.AddWithValue("_Customer", poReq.Customer);
                            mySqlCommand.Parameters.AddWithValue("_Offer", poReq.Offer);
                            mySqlCommand.Parameters.AddWithValue("_InvoiceAddress", poReq.InvoiceAddress);
                            mySqlCommand.Parameters.AddWithValue("_InvoiceTo", poReq.InvoiceTo);
                            mySqlCommand.Parameters.AddWithValue("_ShipAddress", poReq.ShipTo);
                            mySqlCommand.Parameters.AddWithValue("_IntegrationDate", DateTime.Now);
                            mySqlCommand.Parameters.AddWithValue("_IdIntegrationUser", 1);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPODetails_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Email> GetAllUnprocessedEmails_V2570(string ConnectionStringgeos, string ConnectionStringemdep_geos, string poAnalyzerEmailToCheck, string attachedDocPath)
        {
            List<Email> EmailList = new List<Email>();
            List<LookupValue> UntructedExtensionList = GetLookupValues(ConnectionStringemdep_geos, 157);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetUnProcessedEmails_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["IsDeleted"] != DBNull.Value)
                                    email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                if (rdr["IdPORequest"] != DBNull.Value)
                                    email.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                try
                                {
                                    if (rdr["IdEmail"] != DBNull.Value)
                                    {
                                        Emailattachment attach = new Emailattachment();
                                        Email email = EmailList.FirstOrDefault(i => i.IdEmail == Convert.ToInt32(rdr["IdEmail"]));
                                        if (email != null)
                                        {
                                            if (email.EmailattachmentList == null)
                                            {
                                                email.EmailattachmentList = new List<Emailattachment>();
                                            }
                                            if (rdr["IdEmail"] != DBNull.Value)
                                                attach.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                            if (rdr["IdAttachment"] != DBNull.Value)
                                                attach.IdAttachment = Convert.ToInt32(rdr["IdAttachment"]);
                                            if (rdr["AttachmentName"] != DBNull.Value)
                                                attach.AttachmentName = Convert.ToString(rdr["AttachmentName"]);
                                            if (rdr["AttachmentPath"] != DBNull.Value)
                                                attach.AttachmentPath = Convert.ToString(rdr["AttachmentPath"]);
                                            if (rdr["CreatedIn"] != DBNull.Value)
                                                email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                            if (rdr["ModifiedIn"] != DBNull.Value)
                                                email.ModifiedIn = rdr["ModifiedIn"] as DateTime?;
                                            if (rdr["IsDeleted"] != DBNull.Value)
                                                email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                            if (rdr["CreatedBy"] != DBNull.Value)
                                                email.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);
                                            if (rdr["ModifiedBy"] != DBNull.Value)
                                                email.ModifiedBy = rdr["ModifiedBy"] as int?;
                                            if (rdr["AttachmentExtension"] != DBNull.Value)
                                                attach.AttachmentExtension = Convert.ToString(rdr["AttachmentExtension"]);
                                            //attach.FileContent = GetEmailAttachedDoc(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                            if (!UntructedExtensionList.Any(j => j.Value?.Trim() == attach.AttachmentExtension?.Trim()))
                                            {
                                                try
                                                {
                                                    if (attach.AttachmentExtension?.ToLower() == ".pdf")
                                                    {
                                                        attach.FileText = GetPdfAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.LocationFileText = GetPdfAttachedDocTextByLocation(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                        }
                                        email.EmailattachmentList.Add(attach);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public bool UpdatePORequestStatus(PORequest poRequest, string mainServerConnectionString)
        {
            try
            {
                if (poRequest != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdatePORequestStatus_V2570", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdEmail", poRequest.IdEmail);
                        mySqlCommand.Parameters.AddWithValue("_PORequestStatus", poRequest.PORequestStatus?.IdLookupValue);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePORequestStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Email> GetEmailCreatedIn_V2570(string ConnectionStringgeos, string poAnalyzerEmailToCheck)
        {
            List<Email> EmailList = new List<Email>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailCreatedIn_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        //[rdixit][GEOS2-5868][14.10.2024]
        public bool AddEmails_V2570(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails_V2570", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_CCEmail", emailDetails.CCEmail);
                        mySqlCommand.Parameters.AddWithValue("_ToName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_ToEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CCName", emailDetails.CCName);
                        mySqlCommand.Parameters.AddWithValue("_SourceInboxId", emailDetails.SourceInboxId);
                        mySqlCommand.Parameters.AddWithValue("_CreatedIn", emailDetails.CreatedIn);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc_V2550(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }
        private string GetConnectedPlantNameFromDataSource(string datasource, string connectionString)
        {
            string connectedPlantName = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand("OTM_GetAllPlantNameToCheckDataSource_V2570", conn);
                command.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader itemRow = command.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            string plantShortName = itemRow["ShortName"].ToString();
                            if (datasource.Contains(plantShortName.ToUpper()))
                            {
                                connectedPlantName = plantShortName.ToUpper();
                            }
                        }
                    }
                }
            }
            return connectedPlantName;
        }
        //[ashish.malkhede][GEOS2-6520]
        public byte[] GetCountryIconFileInBytes(string countryiso, string filepath)
        {
            byte[] bytes = null;
            try
            {
                string filenamepng = countryiso + ".png";
                string filenamejpg = countryiso + ".jpg";
                string filepathjpg = Path.Combine(filepath, filenamejpg);
                string filepathpng = Path.Combine(filepath, filenamepng);
                if (File.Exists(filepathjpg))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.	
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.	
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.	
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.	
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (DirectoryNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[rdixit][07.11.2024][GEOS2-6600]
        //public List<PORequestDetails> GetPORequestDetails_V2580(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, long plantId, string plantConnection, string correnciesIconFilePath)
        //{
        //    List<PORequestDetails> poList = new List<PORequestDetails>();
        //    List<PORequestDetails> finalpoList = new List<PORequestDetails>();
        //    try
        //    {
        //        using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
        //        {
        //            List<string> currencyISOs = new List<string>();
        //            mySqlconn.Open();
        //            using (MySqlCommand mySqlCommand = new MySqlCommand())
        //            {
        //                mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2580";
        //                mySqlCommand.Connection = mySqlconn;
        //                mySqlCommand.CommandType = CommandType.StoredProcedure;
        //                mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
        //                mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
        //                using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        PORequestDetails po = new PORequestDetails();
        //                        try
        //                        {
        //                            if (rdr["DateTime"] != DBNull.Value)
        //                                po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
        //                            if (rdr["IdPORequest"] != DBNull.Value)
        //                                po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"].ToString());
        //                            if (rdr["Sender"] != DBNull.Value)
        //                                po.Sender = rdr["Sender"].ToString();
        //                            if (rdr["Recipient"] != DBNull.Value)
        //                                po.Recipient = rdr["Recipient"].ToString();
        //                            if (rdr["Subject"] != DBNull.Value)
        //                                po.Subject = rdr["Subject"].ToString();                                    
        //                            if (rdr["POfound"] != DBNull.Value)
        //                                po.POFound = rdr["POfound"].ToString();
        //                            if (rdr["attachCount"] != DBNull.Value)
        //                                po.AttachmentCount = Convert.ToInt16(rdr["attachCount"]);
        //                            if (rdr["Attachments"] != DBNull.Value)
        //                                po.Attachments = rdr["Attachments"].ToString();
        //                            else
        //                                po.Attachments = "";
        //                            if (rdr["AttachmentPath"] != DBNull.Value)
        //                                po.Path = rdr["AttachmentPath"].ToString();
        //                            else
        //                                po.Path = "";                                   
        //                            if (rdr["Requester"] != DBNull.Value)
        //                                po.Requester = rdr["Requester"].ToString();
        //                            else
        //                                po.Requester = "";
        //                            if (rdr["Status"] != DBNull.Value)
        //                            {
        //                                LookupValue lv = new LookupValue();
        //                                lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
        //                                lv.Value = rdr["Status"].ToString();
        //                                lv.HtmlColor = rdr["HtmlColor"].ToString();
        //                                po.PORequestStatus = lv;
        //                            }                                  
        //                            if (rdr["IdPORequestStatus"] != DBNull.Value)
        //                                po.IdStatus = Convert.ToUInt32(rdr["IdPORequestStatus"]);
        //                            else
        //                                po.IdStatus = 0;
        //                            if (po.Currency != null)
        //                            {
        //                                if (!currencyISOs.Any(co => co.ToString() == po.Currency))
        //                                {
        //                                    currencyISOs.Add(po.Currency);
        //                                }
        //                            }
        //                            poList.Add(po);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2580(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
        //                            throw;
        //                        }
        //                    }
        //                    if (rdr.NextResult())
        //                    {
        //                        while (rdr.Read())
        //                        {
        //                            double idpo;
        //                            if (rdr["IdPORequest"] != DBNull.Value)
        //                            {
        //                                idpo = Convert.ToInt32(rdr["IdPORequest"].ToString());
        //                                if (poList.Any(i => i.IdPORequest == idpo))
        //                                {
        //                                    var po = poList.FirstOrDefault(i => i.IdPORequest == idpo);
        //                                    if (rdr["Offer"] != DBNull.Value)
        //                                        po.Offer = rdr["Offer"].ToString();
        //                                    if (rdr["IDPOfinalresult"] != DBNull.Value)
        //                                        po.IdPORequest = Convert.ToInt32(rdr["IDPOfinalresult"].ToString());
        //                                    if (rdr["PONumber"] != DBNull.Value)
        //                                        po.PONumber = rdr["PONumber"].ToString();
        //                                    else
        //                                        po.PONumber = "";
        //                                    if (rdr["PODate"] != DBNull.Value)
        //                                    {
        //                                        po.DateIssued = Convert.ToDateTime(rdr["PODate"]);
        //                                        if (po.DateIssued == DateTime.MinValue)
        //                                            po.DateIssued = null;
        //                                    }
        //                                    if (rdr["TransferAmount"] != DBNull.Value)
        //                                    {
        //                                        po.TransferAmount = Convert.ToDouble(rdr["TransferAmount"]);
        //                                        if (po.TransferAmount == 0)
        //                                            po.TransferAmount = null;
        //                                    }
        //                                    else
        //                                        po.TransferAmount = null;
        //                                    if (rdr["Currency"] != DBNull.Value)
        //                                        po.Currency = rdr["Currency"].ToString();
        //                                    if (rdr["Email"] != DBNull.Value)
        //                                        po.Email = rdr["Email"].ToString();
        //                                    if (rdr["Customer"] != DBNull.Value)
        //                                        po.Customer = rdr["Customer"].ToString();
        //                                    else
        //                                        po.Customer = "";
        //                                    if (rdr["Email"] != DBNull.Value)
        //                                        po.Contact = rdr["Email"].ToString();
        //                                    else
        //                                        po.Contact = "";
        //                                    if (rdr["Incoterm"] != DBNull.Value)
        //                                        po.POIncoterms = rdr["Incoterm"].ToString();
        //                                    else
        //                                        po.POIncoterms = "";
        //                                    if (rdr["ShipTo"] != DBNull.Value)
        //                                        po.ShipTo = rdr["ShipTo"].ToString();
        //                                    else
        //                                        po.ShipTo = "";
        //                                    finalpoList.Add(po);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    var filteredPOList = poList.Where(po => !finalpoList.Any(final => final.IdPORequest == po.IdPORequest)).ToList();
        //                    finalpoList.AddRange(filteredPOList);
        //                    finalpoList = new List<PORequestDetails>(finalpoList.OrderByDescending(j=>j.DateTime));
        //                    foreach (string item in currencyISOs)
        //                    {
        //                        byte[] bytes = null;
        //                        bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
        //                        finalpoList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
        //                    }
        //                }
        //            }
        //            mySqlconn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2580(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        throw;
        //    }
        //    return finalpoList;
        //}
        //[RGadhave][12.11.2024][GEOS2-6461]
        public List<PORequestDetails> GetPORequestDetails_V2580(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2580";
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["Offer"] != DBNull.Value)
                                        po.Offer = rdr["Offer"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                        po.AttachmentCount = Convert.ToInt16(rdr["attachCount"]);
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";
                                    if (rdr["IDPOfinalresult"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IDPOfinalresult"].ToString());
                                    if (rdr["PONumber"] != DBNull.Value)
                                        po.PONumber = rdr["PONumber"].ToString();
                                    else
                                        po.PONumber = "";
                                    if (rdr["PODate"] != DBNull.Value)
                                        po.DateIssued = Convert.ToDateTime(rdr["PODate"]);
                                    if (rdr["TransferAmount"] != DBNull.Value)
                                        po.TransferAmount = Convert.ToDouble(rdr["TransferAmount"]);
                                    else
                                        po.TransferAmount = 0;
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Email = rdr["Email"].ToString();
                                    if (rdr["Customer"] != DBNull.Value)
                                        po.Customer = rdr["Customer"].ToString();
                                    else
                                        po.Customer = "";
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Contact = rdr["Email"].ToString();
                                    else
                                        po.Contact = "";
                                    if (rdr["Incoterm"] != DBNull.Value)
                                        po.POIncoterms = rdr["Incoterm"].ToString();
                                    else
                                        po.POIncoterms = "";
                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["ShipTo"] != DBNull.Value)
                                        po.ShipTo = rdr["ShipTo"].ToString();
                                    else
                                        po.ShipTo = "";
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    poList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<Company> GetAllCompaniesDetails_V2580(Int32 idUser, string connectionString)
        {
            List<Company> companies = new List<Company>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        command.CommandText = "OTM_GetAllPlantsDetails_V2580";
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_idUser", idUser);
                        List<string> strDatabaseIP = new List<string>();
                        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
                        string ConnecteddataSource = builder.Server.ToUpper();
                        string database = builder.Database;
                        string userId = builder.UserID;
                        string password = builder.Password;
                        string connectedPlantName = GetConnectedPlantNameFromDataSource(ConnecteddataSource.ToUpper(), connectionString);
                        string dataSource = "";
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Company company = new Company();
                                if (string.IsNullOrEmpty(connectedPlantName))
                                {
                                    dataSource = ConnecteddataSource.Replace(ConnecteddataSource, reader["DatabaseIP"].ToString());
                                }
                                else
                                {
                                    dataSource = ConnecteddataSource.Replace(connectedPlantName, reader["ShortName"].ToString());
                                }
                                string connstr = connectionString.Replace(builder.Server, dataSource);
                                company.Alias = reader["ShortName"].ToString();
                                company.ShortName = reader["ShortName"].ToString();
                                company.Name = reader["Name"].ToString();
                                company.IdCompany = Convert.ToInt32(reader["idSite"].ToString());
                                company.Country = new Country();
                                company.Country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                                company.Country.Name = reader["name"].ToString();
                                company.ConnectPlantId = reader["idSite"].ToString();
                                company.ConnectPlantConstr = connstr;
                                company.ServiceProviderUrl = reader["ServiceProviderUrl"].ToString();
                                companies.Add(company);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("GetAllCompaniesDetails_V2580(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return companies;
        }
        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<POType> OTM_GetPOTypes_V2580(string connectionString)
        {
            List<POType> POTypeList = new List<POType>();
            POType defaultPOType = new POType();
            defaultPOType.IdPoType = 0;
            defaultPOType.Type = "---";
            POTypeList.Add(defaultPOType);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOTypes_V2580", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            POType potype = new POType();
                            if (mySqlDataReader["IdPOType"] != DBNull.Value)
                                potype.IdPoType = Convert.ToInt32(mySqlDataReader["IdPOType"].ToString());
                            if (mySqlDataReader["POType"] != DBNull.Value)
                                potype.Type = mySqlDataReader["POType"].ToString();
                            POTypeList.Add(potype);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POTypeList;
        }
        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<CustomerPlant> OTM_GetCustomerPlant_V2580(string connectionString)
        {
            List<CustomerPlant> CustomerPlantList = new List<CustomerPlant>();
            CustomerPlant defaultCustomerPlant = new CustomerPlant();
            //defaultCustomerPlant.IdCustomerPlant = 0;
            //defaultCustomerPlant.CustomerPlantName = "---";
            CustomerPlantList.Add(defaultCustomerPlant);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCustomerPlant_V2580", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            CustomerPlant customerPlant = new CustomerPlant();
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                                customerPlant.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdSite"].ToString());
                            if (mySqlDataReader["PlantName"] != DBNull.Value)
                                customerPlant.CustomerPlantName = mySqlDataReader["PlantName"].ToString();
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                                customerPlant.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"].ToString());
                            if (mySqlDataReader["idCountry"] != DBNull.Value)
                                customerPlant.IdCountry = Convert.ToInt32(mySqlDataReader["idCountry"].ToString());
                            CustomerPlantList.Add(customerPlant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CustomerPlantList;
        }
        //[pooja.jadhav][18-11-2024][GEOS2-6460]
        public List<string> OTM_GetPOSender_V2580(string connectionString)
        {
            List<string> POSenderList = new List<string>();
            string defaultPOSender = "---";
            POSenderList.Add(defaultPOSender);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOSender_V2580", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["ReceivedBy"] != DBNull.Value)
                                POSenderList.Add(mySqlDataReader["ReceivedBy"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOSender_V2580. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POSenderList;
        }
        //[pramod.misal][GEOS2-6460][28-11-2024]
        public List<Currency> GetAllCurrencies_V2590(string ConnectionStringGeos, string currencyIconFilePath)
        {
            List<Currency> CurrencyList = new List<Currency>();
            List<string> CurrencyIcon = new List<string>();
            Dictionary<string, byte[]> DictCurrencies = new Dictionary<string, byte[]>();
            Currency defaultCurrency = new Currency();
            defaultCurrency.IdCurrency = 0;
            defaultCurrency.Name = "---";
            CurrencyList.Add(defaultCurrency);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCurrency_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Currency cur = new Currency();
                            try
                            {
                                if (rdr["IdCurrency"] != DBNull.Value)
                                    cur.IdCurrency = Convert.ToByte(rdr["IdCurrency"]);
                                if (rdr["Name"] != DBNull.Value)
                                    cur.Name = rdr["Name"].ToString();
                                if (rdr["Symbol"] != DBNull.Value)
                                    cur.Symbol = rdr["Symbol"].ToString();
                                if (rdr["Description"] != DBNull.Value)
                                    cur.Description = rdr["Description"].ToString();
                                if (rdr["CodeN"] != DBNull.Value)
                                    cur.CodeN = Convert.ToInt32(rdr["CodeN"]);
                                if (rdr["Name"] != DBNull.Value)
                                {
                                    if (!CurrencyIcon.Any(co => co.ToString() == cur.Name))
                                    {
                                        CurrencyIcon.Add(cur.Name);
                                    }
                                }
                            }
                            catch (Exception ex)
                            { }
                            CurrencyList.Add(cur);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCurrencies_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            foreach (string item in CurrencyIcon)
            {
                byte[] bytes = null;
                if (DictCurrencies.Any(i => i.Key == item))
                {
                    bytes = DictCurrencies.Where(i => i.Key == item).FirstOrDefault().Value;
                }
                else
                {
                    bytes = GetCurrencyIconFileInBytes(item, currencyIconFilePath);
                    DictCurrencies.Add(item, bytes);
                }
                CurrencyList.Where(ma => ma.Name == item).ToList().ForEach(ma => ma.CurrencyIconbytes = bytes);
            }
            return CurrencyList;
        }
        public byte[] GetCurrencyIconFileInBytes(string currencyName, string filepath)
        {
            byte[] bytes = null;
            try
            {
                string filenamepng = currencyName + ".png";
                string filenamejpg = currencyName + ".jpg";
                string filepathjpg = Path.Combine(filepath, filenamejpg);
                string filepathpng = Path.Combine(filepath, filenamepng);
                if (File.Exists(filepathjpg))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCurrencyIconFileInBytes() ISO-{0}. ErrorMessage- {1}", currencyName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (DirectoryNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCurrencyIconFileInBytes() ISO-{0}. ErrorMessage- {1}", currencyName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCurrencyIconFileInBytes() ISO-{0}. ErrorMessage- {1}", currencyName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<Common.Company> GetAllSitesWithImagesByIdUser_V2590(string PLMConnectionString, Int32 idUser)
        {
            List<Common.Company> Companies = new List<Common.Company>();
            List<string> CompanyIcon = new List<string>();
            Dictionary<string, byte[]> DictCompanies = new Dictionary<string, byte[]>();
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(PLMConnectionString);
                // Extract components
                string ConnecteddataSource = builder.Server;
                string database = builder.Database;
                string userId = builder.UserID;
                string password = builder.Password;
                string connectedPlantName = GetConnectedPlantNameFromDataSource(ConnecteddataSource.ToUpper(), PLMConnectionString);
                string dataSource = "";
                using (MySqlConnection mySqlConnection = new MySqlConnection(PLMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAuthorizedPlantsByIdUser_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idUser", idUser);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Common.Company Company = new Common.Company();
                            Company.IdCompany = Convert.ToInt32(reader["IdSite"]);
                            if (reader["CountryISO2"] != DBNull.Value)
                            {
                                Company.Iso = Convert.ToString(reader["CountryISO2"]);
                                Company.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Company.Iso + ".png";
                                Company.Country = new Country();
                                //if (Companies.Any(k => k.Iso == Company.Iso))
                                //{
                                //    Company.ImageInBytes = Companies.FirstOrDefault(j => j.Iso == Company.Iso).ImageInBytes;
                                //}
                                //else
                                //{
                                //    using (WebClient webClient = new WebClient())
                                //    {
                                //        Company.ImageInBytes = webClient.DownloadData("https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + Company.Iso + ".png");
                                //    }
                                //}
                            }
                            if (reader["Alias"] != DBNull.Value)
                            {
                                Company.Alias = Convert.ToString(reader["Alias"]);
                            }
                            if (reader["idCountry"] != DBNull.Value)
                            {
                                Company.Country.IdCountry = Convert.ToByte(reader["idCountry"]);
                            }
                            if (reader["ServiceProviderUrl"] != DBNull.Value)
                            {
                                Company.ServiceProviderUrl = Convert.ToString(reader["ServiceProviderUrl"]);
                            }
                            if (reader["IdSite"] != DBNull.Value)
                            {
                                if (string.IsNullOrEmpty(connectedPlantName))
                                {
                                    dataSource = ConnecteddataSource.Replace(ConnecteddataSource, reader["DatabaseIP"].ToString());
                                }
                                else
                                {
                                    dataSource = ConnecteddataSource.Replace(connectedPlantName, reader["Alias"].ToString());
                                }
                                string connstr = PLMConnectionString.Replace(ConnecteddataSource, dataSource);
                                Company.ConnectPlantConstr = connstr;
                            }
                            Companies.Add(Company);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCompanies_V2420(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Companies;
        }
        ///[Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<ShippingAddress> OTM_GetShippingAddress_V2590(string connectionString, int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = new List<ShippingAddress>();
            ShippingAddress defaultPOType = new ShippingAddress();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetShippingAddress_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomerPlant);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            ShippingAddress shippingAddress = new ShippingAddress();
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                shippingAddress.IdShippingAddress = mySqlDataReader.GetInt64("IdShippingAddress");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                shippingAddress.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = mySqlDataReader.GetString("iso");
                            }
                            if (mySqlDataReader["FullAddress"] != DBNull.Value)
                            {
                                shippingAddress.FullAddress = mySqlDataReader.GetString("FullAddress");
                            }
                            if (mySqlDataReader["Address"] != DBNull.Value)
                            {
                                shippingAddress.Address = mySqlDataReader.GetString("Address");
                            }
                            if (mySqlDataReader["ZipCode"] != DBNull.Value)
                            {
                                shippingAddress.ZipCode = mySqlDataReader.GetString("ZipCode");
                            }
                            if (mySqlDataReader["City"] != DBNull.Value)
                            {
                                shippingAddress.City = mySqlDataReader.GetString("City");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = Convert.ToString(mySqlDataReader["iso"]);
                                shippingAddress.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + shippingAddress.IsoCode + ".png";
                            }
                            if (mySqlDataReader["CountriesName"] != DBNull.Value)
                            {
                                shippingAddress.CountriesName = mySqlDataReader.GetString("CountriesName");
                            }
                            ShippingAddressList.Add(shippingAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ShippingAddressList;
        }
        /// <summary>
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <param name="correnciesIconFilePath"></param>
        /// <returns></returns>
        public List<PORegisteredDetails> GetPORegisteredDetails_V2590(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, string correnciesIconFilePath, string countryIconFilePath, PORegisteredDetailFilter filter)
        {
            List<PORegisteredDetails> poRegiList = new List<PORegisteredDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                List<string> countryISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        if (filter == null)
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetails_V2590";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                        }
                        else
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsFilter_V2590";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandTimeout = 6000;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_Number", filter.Number);
                            mySqlCommand.Parameters.AddWithValue("_Type", filter.IdType);
                            mySqlCommand.Parameters.AddWithValue("_Group", filter.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_PlantC", filter.IdCustomerPlant);
                            mySqlCommand.Parameters.AddWithValue("_Sender", filter.Sender);
                            mySqlCommand.Parameters.AddWithValue("_Currency", filter.IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeFrom", filter.PoValueRangeFrom);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeTo", filter.PoValueRangeTo);
                            mySqlCommand.Parameters.AddWithValue("_Offer", filter.Offer);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateFrom", filter.ReceptionDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateTo", filter.ReceptionDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateFrom", filter.CreationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateTo", filter.CreationDateTo);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateFrom", filter.UpdateDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateTO", filter.UpdateDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateFrom", filter.CancellationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateTo", filter.CancellationDateTo);
                        }
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORegisteredDetails po = new PORegisteredDetails();
                                try
                                {
                                    if (rdr["IdPO"] != DBNull.Value)
                                        po.IdPO = Convert.ToInt64(rdr["IdPO"]);
                                    if (rdr["Code"] != DBNull.Value)
                                        po.Code = rdr["Code"].ToString();
                                    if (rdr["IdPOType"] != DBNull.Value)
                                        po.IdPOType = Convert.ToInt32(rdr["IdPOType"]);
                                    if (rdr["Type"] != DBNull.Value)
                                        po.Type = rdr["Type"].ToString();
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["Plant"] != DBNull.Value)
                                        po.Plant = rdr["Plant"].ToString();
                                    if (rdr["Country"] != DBNull.Value)
                                    {
                                        po.Country = rdr["Country"].ToString();
                                        po.CountryISO = rdr["CountryISO"].ToString();
                                    }
                                    if (rdr["Region"] != DBNull.Value)
                                        po.Region = rdr["Region"].ToString();
                                    if (rdr["ReceptionDate"] != DBNull.Value)
                                        po.ReceptionDate = Convert.ToDateTime(rdr["ReceptionDate"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["POValue"] != DBNull.Value)
                                        po.POValue = Convert.ToDouble(rdr["POValue"].ToString());
                                    if (rdr["Amount"] != DBNull.Value)
                                        po.Amount = Convert.ToDouble(rdr["Amount"].ToString());
                                    if (rdr["Remarks"] != DBNull.Value)
                                        po.Remarks = rdr["Remarks"].ToString();
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["LinkedOffer"] != DBNull.Value)
                                        po.LinkedOffer = rdr["LinkedOffer"].ToString();
                                    if (rdr["ShippingAddress"] != DBNull.Value)
                                        po.ShippingAddress = rdr["ShippingAddress"].ToString();
                                    if (rdr["IsOK"] != DBNull.Value)
                                        po.IsOK = rdr["IsOK"].ToString();
                                    if (rdr["Confirmation"] != DBNull.Value)
                                        po.Confirmation = rdr["Confirmation"].ToString();
                                    if (rdr["CreationDate"] != DBNull.Value)
                                        po.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);
                                    if (rdr["Creator"] != DBNull.Value)
                                        po.Creator = rdr["Creator"].ToString();
                                    if (rdr["UpdaterDate"] != DBNull.Value)
                                        po.UpdaterDate = Convert.ToDateTime(rdr["UpdaterDate"].ToString());
                                    if (rdr["Updater"] != DBNull.Value)
                                        po.Updater = rdr["Updater"].ToString();
                                    if (rdr["IsCancelled"] != DBNull.Value)
                                        po.IsCancelled = rdr["IsCancelled"].ToString(); // PO Not Cancelled
                                    if (rdr["IsCancelled"] != DBNull.Value && rdr["IsCancelled"].ToString() != "PO Not Cancelled")
                                    {
                                        if (rdr["Canceler"] != DBNull.Value)
                                            po.Canceler = rdr["Canceler"].ToString();
                                        if (rdr["CancellationDate"] != DBNull.Value)
                                            po.CancellationDate = Convert.ToDateTime(rdr["CancellationDate"].ToString());
                                    }
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (!countryISOs.Any(co => co.ToString() == po.CountryISO))
                                    {
                                        countryISOs.Add(po.CountryISO);
                                    }
                                    /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["IdSite"] != DBNull.Value)
                                        po.IdSite = Convert.ToInt32(rdr["IdSite"]);
                                    if (rdr["IdShippingAddress"] != DBNull.Value)
                                        po.IdShippingAddress = Convert.ToInt64(rdr["IdShippingAddress"]);
                                    if (rdr["IdCurrency"] != DBNull.Value)
                                        po.IdCurrency = Convert.ToInt32(rdr["IdCurrency"]);
                                    if (rdr["CreatorCode"] != DBNull.Value)
                                        po.CreatorCode = rdr["CreatorCode"].ToString();
                                    if (rdr["UpdaterCode"] != DBNull.Value)
                                        po.UpdaterCode = rdr["UpdaterCode"].ToString();
                                    if (rdr["CancelerCode"] != DBNull.Value)
                                        po.CancelerCode = rdr["CancelerCode"].ToString();
                                    if (rdr["AttachmentFileName"] != DBNull.Value)
                                        po.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    if (rdr["IdSender"] != DBNull.Value)
                                        po.IdSender = Convert.ToInt32(rdr["IdSender"]);
                                    //
                                    poRegiList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poRegiList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                            }
                            // for country
                            foreach (string item in countryISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                                poRegiList.Where(ot => ot.CountryISO == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                return poRegiList;
            }
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public bool IsProfileUpdate(string EmployeeCode, string workbenchConnectionString)
        {
            bool isProfileUpdate = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("Hrm_GetEmployeeByEmployeeCode_V2520", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_EmployeeCode", EmployeeCode);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            try
                            {
                                if (empReader["IsProfileUpdate"] != DBNull.Value)
                                {
                                    isProfileUpdate = Convert.ToBoolean(empReader["IsProfileUpdate"]);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error IsProfileUpdate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsProfileUpdate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isProfileUpdate;
        }
        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant(string ConnectionString, Int32 SelectedIdCustomerPlant, Int64 SelectedIdPO, string correnciesIconFilePath, GeosAppSetting geosAppSetting, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            string AppSetting = "";
            if (geosAppSetting != null)
            {
                AppSetting = geosAppSetting.DefaultValue;
            }
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllLinkedOffers_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", SelectedIdCustomerPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdPO", SelectedIdPO);
                    mySqlCommand.Parameters.AddWithValue("_geosAppSetting", AppSetting);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {//PRSA
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = mySqlDataReader.GetString("Code");
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = mySqlDataReader.GetString("Year");
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = mySqlDataReader.GetString("CustomerGroup");
                            }
                            if (mySqlDataReader["CustomerName"] != DBNull.Value)
                            {
                                linkedOffers.CutomerName = mySqlDataReader.GetString("CustomerName");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = mySqlDataReader.GetString("Description");
                            }
                            if (mySqlDataReader["Contact"] != DBNull.Value)
                            {
                                linkedOffers.Contact = mySqlDataReader.GetString("Contact");
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = mySqlDataReader.GetString("Rfq");
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = mySqlDataReader.GetDouble("discount");
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = mySqlDataReader.GetInt32("IdCurrency");
                            }
                            if (mySqlDataReader["OfferCurrency"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = mySqlDataReader.GetString("OfferCurrency");
                            }
                            //if (mySqlDataReader["Name"] != DBNull.Value)
                            //{
                            //    linkedOffers.Name = mySqlDataReader.GetString("Name");
                            //}
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = mySqlDataReader.GetInt64("IdOffer");
                            }
                            if (mySqlDataReader["IdPO"] != DBNull.Value)
                            {
                                linkedOffers.IdPO = mySqlDataReader.GetInt32("IdPO");
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = mySqlDataReader.GetInt32("IdCustomer");
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = mySqlDataReader.GetString("LinkedPO");
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = mySqlDataReader.GetInt32("IdStatus");
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = mySqlDataReader.GetString("HtmlColor");
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = mySqlDataReader.GetDouble("Amount");
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = mySqlDataReader.GetInt32("idOfferType");
                            }
                            if (mySqlDataReader["offersType"] != DBNull.Value)
                            {
                                linkedOffers.OffersType = mySqlDataReader.GetString("offersType");
                            }
                            if (mySqlDataReader["OfferStatus"] != DBNull.Value)
                            {
                                linkedOffers.Status = mySqlDataReader.GetString("OfferStatus");
                            }
                            if (mySqlDataReader["IdProductCategory"] != DBNull.Value)
                            {
                                linkedOffers.IdProductCategory = mySqlDataReader.GetInt32("IdProductCategory");
                            }
                            if (mySqlDataReader["category"] != DBNull.Value)
                            {
                                linkedOffers.Category = mySqlDataReader.GetString("category");
                            }
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                            {
                                linkedOffers.Confirmation = mySqlDataReader.GetString("Conformation");
                            }
                            if (mySqlDataReader["Currency"] != DBNull.Value)
                            {
                                linkedOffers.Currency = mySqlDataReader.GetString("Currency");
                            }
                            if (linkedOffers.Currency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.Currency))
                                {
                                    currencyISOs.Add(linkedOffers.Currency);
                                }
                            }
                            if (mySqlDataReader["AttachmentFileName"] != DBNull.Value)
                            {
                                linkedOffers.AttachmentFileName = mySqlDataReader.GetString("AttachmentFileName");
                                linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, linkedOffers);
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetLinkedofferByIdCustomerPlant(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<Customer> GetAllCustomers_V2590(string connectionString)
        {
            List<Customer> CustomerList = new List<Customer>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCustomers", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Name"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Name"]);
                        CustomerList.Add(cust);
                    }
                }
            }
            return CustomerList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<CustomerPlant> OTM_GetCustomerPlant_V2590(string connectionString)
        {
            List<CustomerPlant> CustomerPlantList = new List<CustomerPlant>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCustomerPlant_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            CustomerPlant customerPlant = new CustomerPlant();
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                                customerPlant.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdSite"].ToString());
                            if (mySqlDataReader["PlantName"] != DBNull.Value)
                                customerPlant.CustomerPlantName = mySqlDataReader["PlantName"].ToString();
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                                customerPlant.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"].ToString());
                            if (mySqlDataReader["idCountry"] != DBNull.Value)
                                customerPlant.IdCountry = Convert.ToInt32(mySqlDataReader["idCountry"].ToString());
                            if (mySqlDataReader["CountryName"] != DBNull.Value)
                                customerPlant.Country = Convert.ToString(mySqlDataReader["CountryName"].ToString());
                            if (mySqlDataReader["CityName"] != DBNull.Value)
                                customerPlant.City = Convert.ToString(mySqlDataReader["CityName"].ToString());
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                customerPlant.Iso = Convert.ToString(mySqlDataReader["iso"]);
                                customerPlant.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + customerPlant.Iso + ".png";
                            }
                            CustomerPlantList.Add(customerPlant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CustomerPlantList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<Currency> GetAllPOCurrencies_V2590(string ConnectionStringGeos, string currencyIconFilePath)
        {
            List<Currency> CurrencyList = new List<Currency>();
            List<string> CurrencyIcon = new List<string>();
            Dictionary<string, byte[]> DictCurrencies = new Dictionary<string, byte[]>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCurrency_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Currency cur = new Currency();
                            try
                            {
                                if (rdr["IdCurrency"] != DBNull.Value)
                                    cur.IdCurrency = Convert.ToByte(rdr["IdCurrency"]);
                                if (rdr["Name"] != DBNull.Value)
                                    cur.Name = rdr["Name"].ToString();
                                if (rdr["Symbol"] != DBNull.Value)
                                    cur.Symbol = rdr["Symbol"].ToString();
                                if (rdr["Description"] != DBNull.Value)
                                    cur.Description = rdr["Description"].ToString();
                                if (rdr["CodeN"] != DBNull.Value)
                                    cur.CodeN = Convert.ToInt32(rdr["CodeN"]);
                                if (rdr["Name"] != DBNull.Value)
                                {
                                    if (!CurrencyIcon.Any(co => co.ToString() == cur.Name))
                                    {
                                        CurrencyIcon.Add(cur.Name);
                                    }
                                }
                            }
                            catch (Exception ex)
                            { }
                            CurrencyList.Add(cur);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCurrencies_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            foreach (string item in CurrencyIcon)
            {
                byte[] bytes = null;
                if (DictCurrencies.Any(i => i.Key == item))
                {
                    bytes = DictCurrencies.Where(i => i.Key == item).FirstOrDefault().Value;
                }
                else
                {
                    bytes = GetCurrencyIconFileInBytes(item, currencyIconFilePath);
                    DictCurrencies.Add(item, bytes);
                }
                CurrencyList.Where(ma => ma.Name == item).ToList().ForEach(ma => ma.CurrencyIconbytes = bytes);
            }
            return CurrencyList;
        }
        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<PORegisteredDetails> OTM_GetPOSender_V2590(string connectionString)
        {
            List<PORegisteredDetails> POSenderList = new List<PORegisteredDetails>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOSender_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            PORegisteredDetails Sender = new PORegisteredDetails();
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                Sender.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["FullName"] != DBNull.Value)
                            {
                                Sender.FullName = Convert.ToString(mySqlDataReader["FullName"]);
                            }
                            if (mySqlDataReader["Surname"] != DBNull.Value)
                            {
                                Sender.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                            }
                            if (mySqlDataReader["IdPersonGender"] != DBNull.Value)
                            {
                                Sender.IdGender = Convert.ToInt16(mySqlDataReader["IdPersonGender"]);
                            }
                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                Sender.IdPerson = Convert.ToInt16(mySqlDataReader["IdPerson"]);
                            }
                            POSenderList.Add(Sender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOSender_V2580. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POSenderList;
        }
        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2590(string ConnectionStringGeos, PORegisteredDetails poRegisteredDetails, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedOffers_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrders", poRegisteredDetails.IdPO);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", poRegisteredDetails.IdCurrency);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LinkedOffers link = new LinkedOffers();
                            try
                            {
                                if (rdr["Code"] != DBNull.Value)
                                    link.Code = Convert.ToString(rdr["Code"]);
                                if (rdr["CutomerName"] != DBNull.Value)
                                    link.CutomerName = Convert.ToString(rdr["CutomerName"]);
                                if (rdr["Name"] != DBNull.Value)
                                    link.Name = Convert.ToString(rdr["Name"]);
                                if (rdr["OfferStatus"] != DBNull.Value)
                                    link.Status = Convert.ToString(rdr["OfferStatus"]);
                                if (rdr["CustomerGroup"] != DBNull.Value)
                                    link.CustomerGroup = Convert.ToString(rdr["CustomerGroup"]);
                                if (rdr["HtmlColor"] != DBNull.Value)
                                    link.HtmlColor = Convert.ToString(rdr["HtmlColor"]);
                                if (rdr["Amount"] != DBNull.Value)
                                    link.Amount = Convert.ToInt64(rdr["Amount"]);
                                if (rdr["Conformation"] != DBNull.Value)
                                    link.Confirmation = Convert.ToString(rdr["Conformation"]);
                                if (rdr["OfferCurrency"] != DBNull.Value)
                                    link.OfferCurrency = Convert.ToString(rdr["OfferCurrency"]);
                                if (rdr["category"] != DBNull.Value)
                                    link.Category = Convert.ToString(rdr["category"]);
                                if (rdr["IdProductCategory"] != DBNull.Value)
                                    link.IdProductCategory = Convert.ToInt32(rdr["IdProductCategory"]);
                                if (rdr["IdOffer"] != DBNull.Value)
                                    link.IdOffer = Convert.ToInt64(rdr["IdOffer"]);
                                if (rdr["Year"] != DBNull.Value)
                                    link.Year = Convert.ToString(rdr["Year"]);
                                if (rdr["AttachmentFileName"] != DBNull.Value)
                                {
                                    link.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    link.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, link);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            LinkedOffersList.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[pramod.misal][GEOS2-5379][29-03-2024]
        public byte[] CommericalAttachedDoc(string CommericalPath, LinkedOffers link)
        {
            byte[] bytes = null;
            string fileUploadPath = Path.Combine(
                 $"{CommericalPath} {link.Year}",
                 $"{link.CustomerGroup} - {link.Name}",
                 link.Code,
                 "03 - PO",
                 link.AttachmentFileName);
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public byte[] CommericalAttachedDoc_V2670(string CommericalPath, LinkedOffers link)
        {
            byte[] bytes = null;
            string oldfileUploadPath = Path.Combine($"{CommericalPath} {link.Year}", $"{link.CustomerGroup} - {link.Name}", link.Code, "03 - PO", link.AttachmentFileName);
            string newfileUploadPath = Path.Combine(CommericalPath, link.Year.ToString(), $"{link.CustomerGroup} - {link.Name}", link.Code, "03 - PO", link.AttachmentFileName);

            try
            {
                if (File.Exists(newfileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(newfileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                else
                {
                    if ((File.Exists(oldfileUploadPath)))
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(oldfileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            bytes = new byte[stream.Length];
                            int numBytesToRead = (int)stream.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;
                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                        }

                    }

                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="idPO"></param>
        /// <returns></returns>
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<LogEntryByPOOffer> GetAllPOChangeLog_V2590(string ConnectionStringGeos, long idPO)
        {
            List<LogEntryByPOOffer> logList = new List<LogEntryByPOOffer>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPOChangeLog_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idPO", idPO);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogEntryByPOOffer poLog = new LogEntryByPOOffer();
                            try
                            {
                                poLog.IdOffer = Convert.ToInt64(reader["idOffer"].ToString());
                                poLog.IdLogEntry = Convert.ToInt64(reader["idLogEntry"].ToString());
                                poLog.IdLogEntryType = Convert.ToByte(reader["idLogEntryType"].ToString());
                                poLog.IdUser = Convert.ToInt32(reader["idUser"].ToString());
                                poLog.People = new People { IdPerson = Convert.ToInt32(reader["idUser"].ToString()), Name = reader["Name"].ToString(), Surname = reader["Surname"].ToString() };
                                poLog.DateTime = Convert.ToDateTime(reader["DateTime"].ToString());
                                poLog.Comments = reader["comments"].ToString();
                                poLog.IsDeleted = false;
                                logList.Add(poLog);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetAllPOChangeLog_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllPOChangeLog_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return logList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// <summary>
        /// [001][ashish.malkhede][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="PO"></param>
        /// <param name="localServerConnectionString"></param>
        /// <returns></returns>
        public bool UpdatePurchaseOrder_V2590(PORegisteredDetails PO, string mainServerConnectionString, string CommericalPath)
        {
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateCustomerPurchaseOrders_V2590", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrder", PO.IdPO);
                        MyCommand.Parameters.AddWithValue("_IdPOType", PO.IdPOType);
                        MyCommand.Parameters.AddWithValue("_IdSite", PO.IdSite);
                        MyCommand.Parameters.AddWithValue("_Code", PO.Code);
                        MyCommand.Parameters.AddWithValue("_ReceivedIn", PO.ReceptionDate);
                        MyCommand.Parameters.AddWithValue("_IdShippingAddress", PO.IdShippingAddress);
                        MyCommand.Parameters.AddWithValue("_Value", PO.POValue);
                        MyCommand.Parameters.AddWithValue("_IdCurrency", PO.IdCurrency);
                        MyCommand.Parameters.AddWithValue("_AttachmentFileName", PO.AttachmentFileName);
                        MyCommand.Parameters.AddWithValue("_IdSender", PO.IdSender);
                        MyCommand.Parameters.AddWithValue("_Sender", PO.Sender);
                        MyCommand.Parameters.AddWithValue("_NOK", PO.IsOK);
                        MyCommand.Parameters.AddWithValue("_IsCancelledStatus", PO.IsCancelled);
                        MyCommand.Parameters.AddWithValue("_UpdatedBy", PO.UpdatedBy);
                        MyCommand.Parameters.AddWithValue("_UpdatedIN", DateTime.Now);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
                    // Insert and Delete Linked Offers
                    InsertUpdateLinkedOffersByPO(PO, mainServerConnectionString, CommericalPath);
                    // Inset Log
                    if (PO.LogEntriesByPO != null && PO.LogEntriesByPO.Count > 0)
                    {
                        // Insert Log entries Po logentriesbypo
                        AddLogEntriesByPO(PO.LogEntriesByPO, PO.IdPO, mainServerConnectionString);
                        // Insert Log All linked offers by PO
                        if (PO.OffersLinked.Count > 0)
                        {
                            foreach (LinkedOffers lo in PO.OffersLinked)
                            {
                                AddLogEntriesLinkedOfferByPO(lo.IdOffer, PO.LogEntriesByPO, mainServerConnectionString);
                            }
                        }
                    }
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return true;
        }

        public bool UpdatePurchaseOrder_V2660(PORegisteredDetails PO, string mainServerConnectionString, string CommericalPath)
        {
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateCustomerPurchaseOrders_V2590", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrder", PO.IdPO);
                        MyCommand.Parameters.AddWithValue("_IdPOType", PO.IdPOType);
                        MyCommand.Parameters.AddWithValue("_IdSite", PO.IdSite);
                        MyCommand.Parameters.AddWithValue("_Code", PO.Code);
                        MyCommand.Parameters.AddWithValue("_ReceivedIn", PO.ReceptionDateNew);
                        MyCommand.Parameters.AddWithValue("_IdShippingAddress", PO.IdShippingAddress);
                        MyCommand.Parameters.AddWithValue("_Value", PO.POValue);
                        MyCommand.Parameters.AddWithValue("_IdCurrency", PO.IdCurrency);
                        MyCommand.Parameters.AddWithValue("_AttachmentFileName", PO.AttachmentFileName);
                        MyCommand.Parameters.AddWithValue("_IdSender", PO.IdSender);
                        MyCommand.Parameters.AddWithValue("_Sender", PO.Sender);
                        MyCommand.Parameters.AddWithValue("_NOK", PO.IsOK);
                        MyCommand.Parameters.AddWithValue("_IsCancelledStatus", PO.IsCancelled);
                        MyCommand.Parameters.AddWithValue("_UpdatedBy", PO.UpdatedBy);
                        MyCommand.Parameters.AddWithValue("_UpdatedIN", DateTime.Now);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
                    // Insert and Delete Linked Offers
                    InsertUpdateLinkedOffersByPO(PO, mainServerConnectionString, CommericalPath);
                    // Inset Log
                    if (PO.LogEntriesByPO != null && PO.LogEntriesByPO.Count > 0)
                    {
                        // Insert Log entries Po logentriesbypo
                        AddLogEntriesByPO(PO.LogEntriesByPO, PO.IdPO, mainServerConnectionString);
                        // Insert Log All linked offers by PO
                        if (PO.OffersLinked.Count > 0)
                        {
                            foreach (LinkedOffers lo in PO.OffersLinked)
                            {
                                AddLogEntriesLinkedOfferByPO(lo.IdOffer, PO.LogEntriesByPO, mainServerConnectionString);
                            }
                        }
                    }
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return true;
        }
        /// <summary>
        /// [001][ashish.malkhede][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        /// <param name="logPO"></param>
        /// <param name="localServerConnectionString"></param>
        public void AddLogEntriesByPO(List<LogEntryByPOOffer> logPO, Int64 idPO, string mainServerConnectionString)
        {
            if (logPO != null)
            {
                foreach (LogEntryByPOOffer log in logPO)
                {
                    using (MySqlConnection connLogEntries = new MySqlConnection(mainServerConnectionString))
                    {
                        connLogEntries.Open();
                        MySqlCommand logEntriesCommand = new MySqlCommand("OTM_logentriesbypo_V2590", connLogEntries);
                        logEntriesCommand.CommandType = CommandType.StoredProcedure;
                        logEntriesCommand.Parameters.AddWithValue("_idPO", idPO);
                        logEntriesCommand.Parameters.AddWithValue("_idUser", log.IdUser);
                        logEntriesCommand.Parameters.AddWithValue("_dateTime", DateTime.Now);
                        logEntriesCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                        logEntriesCommand.Parameters.AddWithValue("_comments", log.Comments);
                        // logEntriesCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                        logEntriesCommand.ExecuteScalar();
                        connLogEntries.Close();
                    }
                }
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][11-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="IdAppSetting"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting, string connectionString)
        {
            GeosAppSetting geosAppSetting = new GeosAppSetting();
            using (MySqlConnection MyConnection = new MySqlConnection(connectionString))
            {
                MyConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("geosappsettings_GetGeosAppSettings", MyConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_IdAppSetting", IdAppSetting);
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        geosAppSetting.IdAppSetting = IdAppSetting;
                        geosAppSetting.DefaultValue = reader["DefaultValue"].ToString();
                        geosAppSetting.AppSettingName = reader["AppSettingName"].ToString();
                    }
                    reader.Close();
                }
                myCommand.Dispose();
                MyConnection.Close();
            }
            return geosAppSetting;
        }
        public void InsertUpdateLinkedOffersByPO(PORegisteredDetails offerslink, string mainServerConnectionString, string CommericalPath)
        {
            foreach (LinkedOffers lo in offerslink.offersLinked)
            {
                if (lo.IsNew)
                {
                    using (MySqlConnection connmainPO = new MySqlConnection(mainServerConnectionString))
                    {
                        connmainPO.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_InsertCustomerPurchaseOrdersByOffer_V2590", connmainPO);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idCustomerPurchaseOrder", offerslink.IdPO);
                        MyCommand.Parameters.AddWithValue("_idOffer", lo.IdOffer);
                        MyCommand.Parameters.AddWithValue("_Comments", offerslink.Remarks);
                        MyCommand.ExecuteNonQuery();
                        connmainPO.Close();
                        AddPOAttachedDoc(lo, CommericalPath, offerslink);
                        OTM_UpdateLinkedOfferStatus_V2660(lo, mainServerConnectionString);
                    }
                }
                else if (lo.IsDelete)
                {
                    using (MySqlConnection connPODelete = new MySqlConnection(mainServerConnectionString))
                    {
                        connPODelete.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_DeleteCustomerPurchaseOrdersByOffer_V2590", connPODelete);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idCustomerPurchaseOrder", offerslink.IdPO);
                        MyCommand.Parameters.AddWithValue("_idOffer", lo.IdOffer);
                        MyCommand.ExecuteNonQuery();
                        connPODelete.Close();
                        DeletePOAttachedDoc(lo, CommericalPath, offerslink);
                    }
                }
                else if (lo.IsUpdate)
                {
                    using (MySqlConnection connmainPOUpdate = new MySqlConnection(mainServerConnectionString))
                    {
                        connmainPOUpdate.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateCustomerPurchaseOrdersByOffer_V2590", connmainPOUpdate);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idCustomerPurchaseOrder", offerslink.IdPO);
                        MyCommand.Parameters.AddWithValue("_idOffer", lo.IdOffer);
                        MyCommand.Parameters.AddWithValue("_Comments", offerslink.Remarks);
                        MyCommand.ExecuteNonQuery();
                        connmainPOUpdate.Close();
                        //[pramod.misal][14-12-2024][GEOS2-6463] 
                        UpdatePOAttachedDoc(lo, CommericalPath, offerslink);
                        OTM_UpdateLinkedOfferStatus_V2660(lo, mainServerConnectionString);
                    }
                }
            }
        }
        //[pramod.misal][14-12-2024][GEOS2-6463] 
        public bool AddPOAttachedDoc(LinkedOffers Doc, string CommericalPath, PORegisteredDetails offerslink)
        {
            try
            {
                if (Doc.CommericalAttachementsDocInBytes != null)
                {
                    //string completePath = string.Format(@"{0}\{1}", ConnectorAttachedDocPath, attachment.Reference);
                    //string completePath = Path.Combine($"{CommericalPath} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code, "03 - PO");
                    string completePath = Path.Combine(CommericalPath, Doc.Year.ToString(), $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code, "03 - PO");
                    string filePath = completePath + "\\" + Doc.AttachmentFileName;
                    try
                    {
                        if (!Directory.Exists(completePath))
                        {
                            Directory.CreateDirectory(completePath);
                        }
                        else
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            // Delete all files in the directory except the target file
                            foreach (string file in Directory.GetFiles(completePath))
                            {
                                if (!file.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        File.WriteAllBytes(filePath, Doc.CommericalAttachementsDocInBytes);
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertConnectorAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteConnectorAttachedDoc()- Filename - {0}. ErrorMessage- {1}", Doc.AttachmentFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        //[pramod.misal][14-12-2024][GEOS2-6463] 
        public bool UpdatePOAttachedDoc(LinkedOffers Doc, string CommericalPath, PORegisteredDetails offerslink)
        {
            if (Doc.CommericalAttachementsDocInBytes != null)
            {
                try
                {
                    //string completePath = string.Format(@"{0}\{1}", ConnectorAttachedDocPath, attachment.Reference);
                    string completePath = Path.Combine($"{CommericalPath} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code, "03 - PO");
                    string filePath = completePath + "\\" + offerslink.AttachmentFileName;
                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        //DirectoryInfo di = new DirectoryInfo(completePath);
                        //foreach (FileInfo file in di.GetFiles())
                        //{
                        //    file.Delete();
                        //}
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        // Delete all files in the directory except the target file
                        foreach (string file in Directory.GetFiles(completePath))
                        {
                            if (!file.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    File.WriteAllBytes(filePath, Doc.CommericalAttachementsDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdatePOAttachedDoc()- Filename - {0}. ErrorMessage- {1}", Doc.AttachmentFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return true;
        }
        //[pramod.misal][14-12-2024][GEOS2-6463] 
        public bool DeletePOAttachedDoc(LinkedOffers Doc, string CommericalPath, PORegisteredDetails offerslink)
        {
            try
            {
                if (CommericalPath != null)
                {
                    //string completePath = string.Format(@"{0}\{1}", ConnectorAttachedDocPath, attachment.Reference);
                    //string completePath = Path.Combine($"{CommericalPath} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code, "03 - PO");
                    string completePath = Path.Combine(CommericalPath, Doc.Year.ToString(), $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code, "03 - PO");
                    string filePath = completePath + "\\" + Doc.AttachmentFileName;
                    if (!File.Exists(filePath))
                    {
                        return false;
                    }
                    else
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteConnectorAttachedDoc()- Filename - {0}. ErrorMessage- {1}", Doc.AttachmentFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        public void AddLogEntriesLinkedOfferByPO(Int64 idOffer, List<LogEntryByPOOffer> logPO, string mainServerConnectionString)
        {
            if (logPO != null)
            {
                foreach (LogEntryByPOOffer log in logPO)
                {
                    using (MySqlConnection connLogEntries = new MySqlConnection(mainServerConnectionString))
                    {
                        connLogEntries.Open();
                        MySqlCommand logEntriesCommand = new MySqlCommand("logEntriesByPO_Insert_V2590", connLogEntries);
                        logEntriesCommand.CommandType = CommandType.StoredProcedure;
                        logEntriesCommand.Parameters.AddWithValue("_IdOffer", idOffer);
                        logEntriesCommand.Parameters.AddWithValue("_IdUser", log.IdUser);
                        logEntriesCommand.Parameters.AddWithValue("_Datetime", DateTime.Now);
                        logEntriesCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                        logEntriesCommand.Parameters.AddWithValue("_Comments", log.Comments);
                        logEntriesCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                        logEntriesCommand.ExecuteScalar();
                        connLogEntries.Close();
                    }
                }
            }
        }
        //[pramod.misal][GEOS2-6462][18-11-2024]
        public Company GetCompanyDetailsById_V2580(string connectionstring, Int32 idSite)
        {
            Company company = new Company();
            DataTable dt = new DataTable();
            using (MySqlConnection concompany = new MySqlConnection(connectionstring))
            {
                concompany.Open();
                MySqlCommand concompanycommand = new MySqlCommand("sites_GetCompanyDetailsById_V2580", concompany);
                concompanycommand.CommandType = CommandType.StoredProcedure;
                concompanycommand.Parameters.AddWithValue("_IdSite", idSite);
                MySqlDataAdapter da = new MySqlDataAdapter(concompanycommand);
                //DataSet ds = new DataSet();
                da.Fill(dt);
            }
            foreach (DataRow companyreader in dt.Rows)
            {
                //pramod
                if (companyreader["IdIncomTerms"] != DBNull.Value)
                {
                    company.Idincoterm = Convert.ToInt32(companyreader["IdIncomTerms"].ToString());
                }
                if (companyreader["IdPaymentType"] != DBNull.Value)
                {
                    company.IdPaymentType = Convert.ToInt32(companyreader["IdPaymentType"].ToString());
                }
                company.IdCompany = Convert.ToInt32(companyreader["IdSite"].ToString());
                company.Name = companyreader["CustomerPlant"].ToString();
                company.RegisteredName = companyreader["RegisteredName"].ToString();
                company.CIF = companyreader["RegistrationNumber"].ToString();
                company.IdCountry = Convert.ToByte(companyreader["idCountry"].ToString());
                company.Country = new Country { IdCountry = Convert.ToByte(companyreader["idCountry"].ToString()), Name = companyreader["Country"].ToString() };
                company.City = companyreader["City"].ToString();
                company.Address = companyreader["Address"].ToString();
                company.Telephone = companyreader["Phone"].ToString();
                company.ZipCode = companyreader["PostCode"].ToString();
                company.Fax = companyreader["Fax"].ToString();
                company.Website = companyreader["WebSite"].ToString();
                company.Email = companyreader["Email"].ToString();
                company.Region = companyreader["Region"].ToString();
                company.ShortName = companyreader["ShortName"].ToString();
                if (companyreader["IdSalesResponsible"] != DBNull.Value)
                {
                    company.IdSalesResponsible = Convert.ToInt32(companyreader["IdSalesResponsible"].ToString());
                    company.People = new People { IdPerson = Convert.ToInt32(companyreader["IdSalesResponsible"].ToString()), Name = companyreader["SalesResponsibleName"].ToString(), Surname = companyreader["SalesResponsibleSurname"].ToString(), IsSalesResponsible = true };
                }
                if (companyreader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                {
                    company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(companyreader["IdSalesResponsibleAssemblyBU"].ToString());
                    company.PeopleSalesResponsibleAssemblyBU = new People { IdPerson = Convert.ToInt32(companyreader["IdSalesResponsibleAssemblyBU"].ToString()), Name = companyreader["SalesResponsiblebuName"].ToString(), Surname = companyreader["SalesResponsiblebuSurname"].ToString(), IsSalesResponsible = false };
                }
                company.Country.Zone = new Zone { Name = companyreader["SalesZone"].ToString() };
                company.IdCustomer = Convert.ToInt32(companyreader["IdCustomer"].ToString());
                company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(companyreader["IdCustomer"].ToString()), CustomerName = companyreader["CustomerGroup"].ToString() } };
                if (companyreader["Line"] != DBNull.Value)
                    company.Line = Convert.ToInt32(companyreader["Line"].ToString());
                if (companyreader["ModifiedIn"] != DBNull.Value)
                    company.ModifiedIn = Convert.ToDateTime(companyreader["ModifiedIn"].ToString());
                else
                    company.ModifiedIn = null;
                if (companyreader["CreatedIn"] != DBNull.Value)
                    company.CreatedIn = Convert.ToDateTime(companyreader["CreatedIn"].ToString());
                if (companyreader["CuttingMachines"] != DBNull.Value)
                    company.CuttingMachines = Convert.ToInt32(companyreader["CuttingMachines"].ToString());
                //if (companyreader["Size"] != DBNull.Value)
                //    company.Size = Convert.ToDouble(companyreader["Size"].ToString());
                //if (companyreader["NumberOfEmployees"] != DBNull.Value)
                //    company.NumberOfEmployees = Convert.ToInt32(companyreader["NumberOfEmployees"].ToString());
                if (companyreader["IdSource"] != DBNull.Value)
                {
                    company.IdSource = Convert.ToInt32(companyreader["IdSource"]);
                }
                if (companyreader["IdBusinessField"] != DBNull.Value)
                {
                    company.IdBusinessField = Convert.ToByte(companyreader["IdBusinessField"].ToString());
                    company.BusinessField = new LookupValue { IdLookupValue = Convert.ToByte(companyreader["IdBusinessField"].ToString()), Value = companyreader["BusinessField"].ToString() };
                }
                if (companyreader["IdBusinessCenter"] != DBNull.Value)
                {
                    company.IdBusinessCenter = Convert.ToByte(companyreader["IdBusinessCenter"].ToString());
                    company.BusinessCenter = new LookupValue { IdLookupValue = Convert.ToByte(companyreader["IdBusinessCenter"].ToString()), Value = companyreader["BusinessCenter"].ToString() };
                }
                if (companyreader["IdBusinessType"] != DBNull.Value)
                {
                    company.IdBusinessType = Convert.ToByte(companyreader["IdBusinessType"].ToString());
                    company.BusinessType = new LookupValue { IdLookupValue = Convert.ToByte(companyreader["IdBusinessType"].ToString()), Value = companyreader["BusinessType"].ToString() };
                }
                if (companyreader["Latitude"] != DBNull.Value)
                {
                    company.Latitude = Convert.ToDouble(companyreader["Latitude"].ToString());
                }
                if (companyreader["Longitude"] != DBNull.Value)
                {
                    company.Longitude = Convert.ToDouble(companyreader["Longitude"].ToString());
                }
                company.BusinessProductList = new List<SitesByBusinessProduct>();
                using (MySqlConnection concompanybusinessproduct = new MySqlConnection(connectionstring))
                {
                    concompanybusinessproduct.Open();
                    MySqlCommand concompanycommandbusinessproduct = new MySqlCommand("sitesbybusinessproduct_GetBusinessproductByIdSite", concompanybusinessproduct);
                    concompanycommandbusinessproduct.CommandType = CommandType.StoredProcedure;
                    concompanycommandbusinessproduct.Parameters.AddWithValue("_idSite", company.IdCompany);
                    using (MySqlDataReader companybusinessproductreader = concompanycommandbusinessproduct.ExecuteReader())
                    {
                        while (companybusinessproductreader.Read())
                        {
                            SitesByBusinessProduct sitesByBusinessProduct = new SitesByBusinessProduct();
                            sitesByBusinessProduct.IdSite = Convert.ToInt32(companybusinessproductreader["IdSite"].ToString());
                            sitesByBusinessProduct.IdBusinessProduct = Convert.ToInt32(companybusinessproductreader["IdBusinessProduct"].ToString());
                            sitesByBusinessProduct.IsAdded = true;
                            company.BusinessProductList.Add(sitesByBusinessProduct);
                        }
                    }
                }
            }
            return company;
        }
        public List<People> GetContactsByIdPermission_V2590(string connectionstring, Int32 Idperson)
        {
            List<People> peoples = new List<People>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetContactDetailsbyIdPerson_V2590", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdPerson", Idperson);
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        People people = new People();
                        people.IdPerson = Convert.ToInt32(reader["IdPerson"]);
                        people.Name = Convert.ToString(reader["FirstName"]);
                        people.Surname = Convert.ToString(reader["LastName"]);
                        people.Phone = Convert.ToString(reader["Telephone"]);
                        people.Email = Convert.ToString(reader["Email"]);
                        if (reader["IdPersonGender"] != DBNull.Value)
                        {
                            people.IdPersonGender = Convert.ToByte(reader["IdPersonGender"]);
                            if (people.IdPersonGender == 1)
                                people.UserGender = "Female";
                            else if (people.IdPersonGender == 2)
                                people.UserGender = "Male";
                        }
                        people.IdPersonType = Convert.ToByte(reader["IdPersonType"].ToString());
                        people.PeopleType = new PeopleType();
                        people.PeopleType.IdPersonType = Convert.ToByte(reader["IdPersonType"].ToString());
                        people.PeopleType.Name = Convert.ToString(reader["PersonTypeName"]);
                        if (reader["CreatedIn"] != DBNull.Value)
                            people.CreatedIn = Convert.ToDateTime(reader["CreatedIn"].ToString());
                        people.Company = new Company() { IdCompany = Convert.ToInt32(reader["IdCompany"].ToString()), Name = reader["CustomerPlant"].ToString().Trim(), Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["Country"].ToString(), CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + reader["Iso"].ToString() + ".png", Zone = new Zone { Name = reader["Zone"].ToString() } } };
                        people.Company.Address = Convert.ToString(reader["SiteAddress"]);
                        people.Observations = Convert.ToString(reader["Observations"]);
                        people.Company.City = Convert.ToString(reader["SiteCity"]);
                        people.Company.CIF = Convert.ToString(reader["SiteCIF"]);
                        people.Company.Telephone = Convert.ToString(reader["SiteTelephone"]);
                        people.Company.Fax = Convert.ToString(reader["SiteFax"]);
                        people.Company.PostCode = Convert.ToString(reader["SitePostCode"]);
                        people.Company.ZipCode = Convert.ToString(reader["SitePostCode"]);
                        people.Company.Region = Convert.ToString(reader["SiteRegion"]);
                        people.Company.RegisteredName = Convert.ToString(reader["SiteRegisteredName"]);
                        people.Company.Website = Convert.ToString(reader["SiteWebsite"]);
                        people.Company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString()), CustomerName = reader["CustomerGroup"].ToString() } };
                        if (reader["IdCompanyDepartment"] != DBNull.Value)
                        {
                            people.IdCompanyDepartment = Convert.ToInt32(reader["IdCompanyDepartment"].ToString());
                            people.CompanyDepartment = new LookupValue();
                            people.CompanyDepartment.IdLookupValue = Convert.ToInt32(reader["IdCompanyDepartment"].ToString());
                            people.CompanyDepartment.Value = reader["Value"].ToString();
                            if (reader["IdImage"] != DBNull.Value)
                                people.CompanyDepartment.IdImage = Convert.ToInt64(reader["IdImage"].ToString());
                        }
                        people.JobTitle = reader["JobTitle"].ToString();
                        people.ImageText = reader["Image"].ToString();
                        if (reader["IdContactInfluenceLevel"] != DBNull.Value)
                        {
                            people.IdContactInfluenceLevel = Convert.ToInt32(reader["IdContactInfluenceLevel"].ToString());
                            people.InfluenceLevel = new LookupValue();
                            people.InfluenceLevel.IdLookupValue = Convert.ToInt32(reader["IdContactInfluenceLevel"].ToString());
                            people.InfluenceLevel.Value = reader["InfluenceLevel"].ToString();
                            people.InfluenceLevel.HtmlColor = reader["InfluenceLevelHtmlcolor"].ToString();
                            if (reader["InfluenceLevelIdImage"] != DBNull.Value)
                                people.InfluenceLevel.IdImage = Convert.ToInt64(reader["InfluenceLevelIdImage"].ToString());
                        }
                        if (reader["IdContactEmdepAffinity"] != DBNull.Value)
                        {
                            people.IdContactEmdepAffinity = Convert.ToInt32(reader["IdContactEmdepAffinity"].ToString());
                            people.EmdepAffinity = new LookupValue();
                            people.EmdepAffinity.IdLookupValue = Convert.ToInt32(reader["IdContactEmdepAffinity"].ToString());
                            people.EmdepAffinity.Value = reader["EmdepAffinity"].ToString();
                            people.EmdepAffinity.HtmlColor = reader["EmdepAffinityHtmlcolor"].ToString();
                            if (reader["EmdepAffinityIdImage"] != DBNull.Value)
                                people.EmdepAffinity.IdImage = Convert.ToInt64(reader["EmdepAffinityIdImage"].ToString());
                        }
                        if (reader["IdContactProductInvolved"] != DBNull.Value)
                        {
                            people.IdContactProductInvolved = Convert.ToInt32(reader["IdContactProductInvolved"].ToString());
                            people.ProductInvolved = new LookupValue();
                            people.ProductInvolved.IdLookupValue = Convert.ToInt32(reader["IdContactProductInvolved"].ToString());
                            people.ProductInvolved.Value = reader["ProductInvolved"].ToString();
                            people.ProductInvolved.HtmlColor = reader["ProductInvolvedLevelHtmlcolor"].ToString();
                            if (reader["ProductInvolvedLevelIdImage"] != DBNull.Value)
                                people.ProductInvolved.IdImage = Convert.ToInt64(reader["ProductInvolvedLevelIdImage"].ToString());
                        }
                        if (reader["IdCompetitor"] != DBNull.Value)
                        {
                            people.IdCompetitor = Convert.ToInt32(reader["IdCompetitor"].ToString());
                            people.Competitor = new Competitor();
                            people.Competitor.IdCompetitor = Convert.ToInt32(reader["IdCompetitor"].ToString());
                            people.Competitor.Name = reader["Competitor"].ToString();
                        }
                        if (reader["IdCreator"] != DBNull.Value)
                        {
                            people.IdCreator = Convert.ToInt32(reader["IdCreator"].ToString());
                            people.Creator = new People();
                            people.Creator.IdPerson = Convert.ToInt32(reader["IdCreator"].ToString());
                            people.Creator.Name = Convert.ToString(reader["CreatedByFirstName"].ToString());
                            people.Creator.Surname = Convert.ToString(reader["CreatedByLastName"].ToString());
                        }
                        peoples.Add(people);
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            People saleowner = new People();
                            if (reader["IdPerson"] != DBNull.Value)
                            {
                                People people = peoples.Where(i => i.IdPerson == Convert.ToInt64(reader["IdPerson"].ToString())).FirstOrDefault();
                                if (people.Company.SalesOwnerList == null)
                                    people.Company.SalesOwnerList = new List<People>();
                                //if (idPermission == 21 || idPermission == 22)
                                //{
                                if (reader["IdSiteSalesOwner"] != DBNull.Value)
                                {
                                    //people.Company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"]);
                                    saleowner.IdPerson = Convert.ToInt32(reader["IdSalesOwner"]);
                                    if (reader["IdSite"] != DBNull.Value)
                                        saleowner.IdSite = Convert.ToInt32(reader["IdSite"]);
                                    if (reader["IsMainSalesOwner"] != DBNull.Value)
                                    {
                                        int i = Convert.ToInt32(reader["IsMainSalesOwner"]);
                                        if (i == 1)
                                            saleowner.IsSalesResponsible = true;
                                    }
                                    if (reader["salesresponsiblename"] != DBNull.Value)
                                    {
                                        saleowner.Name = Convert.ToString(reader["salesresponsiblename"]);
                                        if (people.SalesOwner == null)
                                            people.SalesOwner = saleowner.Name;
                                        else
                                            people.SalesOwner = people.SalesOwner + "\n" + saleowner.Name;
                                    }
                                }
                                // }
                                people.Company.SalesOwnerList.Add(saleowner);
                            }
                        }
                    }
                }
            }
            return peoples;
        }
        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPOReceptionEmailToFeilds_V2590(string connectionstring, Int64 IdPO)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOReceptionEmailToFeilds_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPO", IdPO);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdSender"] != DBNull.Value)
                            {
                                People PORequester = new People();
                                PORequester.IdPerson = Convert.ToInt32(reader["IdSender"]);
                                PORequester.Name = Convert.ToString(reader["PORequesterName"]);
                                PORequester.Surname = Convert.ToString(reader["PORequesterSurName"]);
                                PORequester.Email = Convert.ToString(reader["PORequesterEmail"]);
                                peoples.Add(PORequester);
                            }
                            if (reader["Idperson"] != DBNull.Value)
                            {
                                People OfferContacts = new People();
                                OfferContacts.IdPerson = Convert.ToInt32(reader["Idperson"]);
                                OfferContacts.Name = Convert.ToString(reader["OfferContactsName"]);
                                OfferContacts.Surname = Convert.ToString(reader["OfferContactsSurname"]);
                                OfferContacts.Email = Convert.ToString(reader["OfferContactsEmail"]);
                                peoples.Add(OfferContacts);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("GetPOReceptionEmailToFeilds_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }
        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPOReceptionEmailCCFeilds_V2590(string connectionstring, Int64 IdPO)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOReceptionEmailCCFeilds_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPO", IdPO);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdSalesOwner"] != DBNull.Value)
                            {
                                People SalesOwner = new People();
                                SalesOwner.IdPerson = Convert.ToInt32(reader["IdSalesOwner"]);
                                SalesOwner.Name = Convert.ToString(reader["SalesOwnerName"]);
                                SalesOwner.Surname = Convert.ToString(reader["SalesOwnerSurname"]);
                                SalesOwner.Email = Convert.ToString(reader["SalesOwnerEmail"]);
                                peoples.Add(SalesOwner);
                            }
                            if (reader["OfferedBy"] != DBNull.Value)
                            {
                                People OfferOwner = new People();
                                OfferOwner.IdPerson = Convert.ToInt32(reader["OfferedBy"]);
                                OfferOwner.Name = Convert.ToString(reader["OfferOwnerName"]);
                                OfferOwner.Surname = Convert.ToString(reader["OfferOwnerSurname"]);
                                OfferOwner.Email = Convert.ToString(reader["OfferOwnerEmail"]);
                                peoples.Add(OfferOwner);
                            }
                            if (reader["AssignedTo"] != DBNull.Value)
                            {
                                People OfferAssignee = new People();
                                OfferAssignee.IdPerson = Convert.ToInt32(reader["AssignedTo"]);
                                OfferAssignee.Name = Convert.ToString(reader["OfferAssigneeName"]);
                                OfferAssignee.Surname = Convert.ToString(reader["OfferAssigneeSurname"]);
                                OfferAssignee.Email = Convert.ToString(reader["OfferAssigneeEmail"]);
                                peoples.Add(OfferAssignee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPOReceptionEmailCCFeilds_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }
        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPeopleByEMDEPcustomer_V2590(string connectionstring)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPeopleByEMDEPcustomer_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            People people = new People();
                            people.IdPerson = Convert.ToInt32(reader["IdPerson"]);
                            people.Name = Convert.ToString(reader["Name"]);
                            people.Surname = Convert.ToString(reader["Surname"]);
                            people.Email = Convert.ToString(reader["Email"]);
                            peoples.Add(people);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPeopleByEMDEPcustomer_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }
        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<Language> GetAllLanguages_V2590(string connectionstring)
        {
            List<Language> Languages = new List<Language>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllLanguages_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Language Language = new Language();
                            Language.IdLanguage = Convert.ToInt32(reader["IdLanguage"]);
                            if (reader["Name"] != DBNull.Value)
                            {
                                Language.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["TwoLetterIsoLanguage"] != DBNull.Value)
                            {
                                Language.TwoLetterISOLanguage = Convert.ToString(reader["TwoLetterIsoLanguage"]);
                            }
                            Languages.Add(Language);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllLanguages_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Languages;
        }
        public string GetEmployeeCodeByUserID(Int64 IdUser, string workbenchConnectionString)
        {
            string EmployeeCode = "";
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmployeeByEmployeeIdUser_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idUser", IdUser);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            try
                            {
                                if (empReader["EmployeeCode"] != DBNull.Value)
                                {
                                    EmployeeCode = empReader["EmployeeCode"].ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetEmployeeCodeByUserID(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsProfileUpdate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmployeeCode;
        }
        public double GetOfferAmountByCurrencyConversion_V2590(string connectionstring, int PreIdCurrency, int idCurrency, long IdPO)
        {
            double convertedOfferAmount = 0;
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAmountByCurrencyConversion_V2590", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdPO", IdPO);
                mySqlCommand.Parameters.AddWithValue("_FromIdcurrency", PreIdCurrency);
                mySqlCommand.Parameters.AddWithValue("_ToIdcurrency", idCurrency);
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["OfferAmount"] != DBNull.Value)
                            convertedOfferAmount = Convert.ToDouble(reader["OfferAmount"].ToString());
                    }
                }
            }
            return convertedOfferAmount;
        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        public bool POEmailSend_V2590(string EmailSubject, string htmlEmailtemplate, ObservableCollection<People> toContactList, ObservableCollection<People> CcContactList, string fromMail, string workbenchConnectionString, string MailServerName, string MailServerPort, List<LinkedResource> imageList)
        {
            bool isSend = false;
            try
            {
                List<string> toAddresses = toContactList.Select(person => person.Email).ToList();
                List<string> ccAddresses = CcContactList.Select(person => person.Email).ToList();
                #region  Old Take Images From URL
                List<System.Net.Mail.LinkedResource> ImgResourceList = new List<System.Net.Mail.LinkedResource>();
                List<string> Images = new List<string>();
                List<string> SetImages = new List<string>();
                Regex regexImg = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                MatchCollection matchesImg = regexImg.Matches(htmlEmailtemplate.ToString());
                //Images = new List<string>();
                foreach (Match matchImg in matchesImg)
                {
                    Images.Add(matchImg.Groups[1].Value);
                }
                Log4NetLogger.Logger.Log(string.Format("OTM SendMail Method Start Imgae download - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                int i = 0;
                foreach (var item in Images)
                {
                    if (i == 0)
                    {
                        htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_1");
                        //using (WebClient webClient = new WebClient())
                        //{
                        //    byte[] imageBytes1 = webClient.DownloadData("https://api.emdep.com/images/logo-geos.png");
                        //    MemoryStream imgGeosLogo = new MemoryStream(imageBytes1);
                        //    System.Net.Mail.LinkedResource LRGeosLogo = new System.Net.Mail.LinkedResource(imgGeosLogo);
                        //    LRGeosLogo.ContentId = "EmbeddedContent_1";
                        //    LRGeosLogo.ContentLink = new Uri("cid:" + LRGeosLogo.ContentId);
                        //    ImgResourceList.Add(LRGeosLogo);
                        //}
                        try
                        {
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://api.emdep.com/images/logo-geos.png");
                            MemoryStream imgGeosLogo = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LRGeosLogo = new System.Net.Mail.LinkedResource(imgGeosLogo);
                            LRGeosLogo.ContentId = "EmbeddedContent_1";
                            LRGeosLogo.ContentLink = new Uri("cid:" + LRGeosLogo.ContentId);
                            ImgResourceList.Add(LRGeosLogo);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    if (i == 1)
                    {
                        htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_2");
                        //using (WebClient webClient = new WebClient())
                        //{
                        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        //    byte[] imageBytes1 = webClient.DownloadData("https://ecos.emdep.com/images/logo-emdep.png");
                        //    MemoryStream imgEmdepLogo = new MemoryStream(imageBytes1);
                        //    System.Net.Mail.LinkedResource LREmdepLogo = new System.Net.Mail.LinkedResource(imgEmdepLogo);
                        //    LREmdepLogo.ContentId = "EmbeddedContent_2";
                        //    LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                        //    ImgResourceList.Add(LREmdepLogo);
                        //}
                        try
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://ecos.emdep.com/images/logo-emdep.png");
                            MemoryStream imgEmdepLogo = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LREmdepLogo = new System.Net.Mail.LinkedResource(imgEmdepLogo);
                            LREmdepLogo.ContentId = "EmbeddedContent_2";
                            LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                            ImgResourceList.Add(LREmdepLogo);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (i == 2)
                    {
                        htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_3");
                        //using (WebClient webClient = new WebClient())
                        //{
                        //    byte[] imageBytes1 = webClient.DownloadData("https://ecos.emdep.com/images/social/linkedin_sm.png");
                        //    MemoryStream imglinkedin = new MemoryStream(imageBytes1);
                        //    System.Net.Mail.LinkedResource LRlinkedin = new System.Net.Mail.LinkedResource(imglinkedin);
                        //    LRlinkedin.ContentId = "EmbeddedContent_3";
                        //    LRlinkedin.ContentLink = new Uri("cid:" + LRlinkedin.ContentId);
                        //    ImgResourceList.Add(LRlinkedin);
                        //}
                        try
                        {
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://ecos.emdep.com/images/social/linkedin_sm.png");
                            MemoryStream imglinkedin = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LRlinkedin = new System.Net.Mail.LinkedResource(imglinkedin);
                            LRlinkedin.ContentId = "EmbeddedContent_3";
                            LRlinkedin.ContentLink = new Uri("cid:" + LRlinkedin.ContentId);
                            ImgResourceList.Add(LRlinkedin);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    i++;
                }
                #endregion;
                Log4NetLogger.Logger.Log(string.Format("OTM SendMail Method End Imgae download - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("OTM Call Send Mail Method - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                Utility.MailControl.SendHtmlMail(EmailSubject, htmlEmailtemplate, string.Join(";", toAddresses), ccAddresses, "noreply@emdep.com", MailServerName, MailServerPort, ImgResourceList);
                Log4NetLogger.Logger.Log(string.Format("OTM End Send Mail Method - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                isSend = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return isSend;
        }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public List<OtRequestTemplates> GetAllOtImportTemplate_V2600(string workbenchConnectionString, string OtAttachmentPath)
        {
            List<OtRequestTemplates> Otrequesttemplateslist = new List<OtRequestTemplates>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllOtImportTemplate_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OtRequestTemplates Otrequesttemplates = new OtRequestTemplates();
                            if (reader["Code"] != DBNull.Value)
                            {
                                Otrequesttemplates.Code = Convert.ToString(reader["Code"]);
                            }
                            if (reader["TemplateName"] != DBNull.Value)
                            {
                                Otrequesttemplates.TemplateName = Convert.ToString(reader["TemplateName"]);
                            }
                            if (reader["Group"] != DBNull.Value)
                            {
                                Otrequesttemplates.Group = Convert.ToString(reader["Group"]);
                            }
                            if (reader["Region"] != DBNull.Value)
                            {
                                Otrequesttemplates.Region = Convert.ToString(reader["Region"]);
                            }
                            if (reader["Country"] != DBNull.Value)
                            {
                                Otrequesttemplates.Country = Convert.ToString(reader["Country"]);
                            }
                            if (reader["Plant"] != DBNull.Value)
                            {
                                Otrequesttemplates.Plant = Convert.ToString(reader["Plant"]);
                            }
                            if (reader["CreatedBy"] != DBNull.Value)
                            {
                                Otrequesttemplates.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                            }
                            if (reader["CreatedIn"] != DBNull.Value)
                            {
                                Otrequesttemplates.CreatedAt = Convert.ToDateTime(reader["CreatedIn"]);
                            }
                            if (reader["UpdatedBy"] != DBNull.Value)
                            {
                                Otrequesttemplates.UpdatedBy = Convert.ToString(reader["UpdatedBy"]);
                            }
                            if (reader["UpdatedIn"] != DBNull.Value)
                            {
                                Otrequesttemplates.UpdatedAt = Convert.ToDateTime(reader["UpdatedIn"]);
                            }
                            if (reader["File"] != DBNull.Value)
                            {
                                Otrequesttemplates.File = Convert.ToString(reader["File"]);
                                Otrequesttemplates.fileExtension = Path.GetExtension(Otrequesttemplates.File).ToLower();
                                Otrequesttemplates.FileDocInBytes = FileAttachedDoc(OtAttachmentPath, Otrequesttemplates);
                            }
                            if (reader["InUse"] != DBNull.Value)
                            {
                                Otrequesttemplates.Action = Convert.ToString(reader["InUse"]);
                                if (Otrequesttemplates.Action == "True")
                                {
                                    Otrequesttemplates.Action = "Yes";
                                }
                                else
                                {
                                    Otrequesttemplates.Action = "No";
                                }
                            }
                            if (reader["InUse"] != DBNull.Value)
                            {
                                Otrequesttemplates.InUse = Convert.ToInt32(reader["InUse"]);
                            }
                            if (reader["idOTRequestTemplate"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdOTRequestTemplate = Convert.ToInt32(reader["idOTRequestTemplate"]);
                            }
                            Otrequesttemplateslist.Add(Otrequesttemplates);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOtImportTemplate_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Otrequesttemplateslist;
        }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public bool DeletetImportTemplate_V2600(string connectionString, Int32 IdOTRequestTemplate)
        {
            bool isDeleted = false;
            if (IdOTRequestTemplate > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeletetImportTemplate_V2600", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_idOTRequestTemplate", IdOTRequestTemplate);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    isDeleted = true;
                    //  AddLogEntriesByActionPlan_V2580(connectionString, ActionPlanLogEntries);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error DeletetImportTemplate_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<Customer> GetCustomerDetails_V2600(string ConnectionString)
        {
            List<Customer> Customers = new List<Customer>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCustomerDetails_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer();
                            if (reader["IdCustomer"] != DBNull.Value)
                                customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                            if (reader["Name"] != DBNull.Value)
                                customer.CustomerName = Convert.ToString(reader["Name"].ToString());
                            if (reader["IdCustomerType"] != DBNull.Value)
                                customer.IdCustomerType = Convert.ToByte(reader["IdCustomerType"].ToString());
                            if (reader["Logo"] != DBNull.Value)
                                customer.Logo = Convert.ToString(reader["Logo"].ToString());
                            if (reader["PatternForConnectorReferences"] != DBNull.Value)
                                customer.PatternForConnectorReferences = Convert.ToString(reader["PatternForConnectorReferences"].ToString());
                            if (reader["Web"] != DBNull.Value)
                                customer.Web = Convert.ToString(reader["Web"].ToString());
                            if (reader["IsStillActive"] != DBNull.Value)
                                customer.IsStillActive = Convert.ToSByte(reader["IsStillActive"]);
                            Customers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCustomerDetails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Customers;
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<Regions> GetRegions_V2600(string ConnectionString)
        {
            List<Regions> RegionsList = new List<Regions>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetRegions_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Regions = new Regions();
                            if (reader["IdLookupValue"] != DBNull.Value)
                                Regions.IdRegion = Convert.ToInt32(reader["IdLookupValue"].ToString());
                            if (reader["Value"] != DBNull.Value)
                                Regions.RegionName = Convert.ToString(reader["Value"].ToString());
                            RegionsList.Add(Regions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetRegion_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return RegionsList;
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<Country> GetCountriesDetails_V2600(string ConnectionString)
        {
            List<Country> Countries = new List<Country>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCountriesDetails_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var country = new Country();
                            if (reader["idCountry"] != DBNull.Value)
                                country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                            if (reader["CountryName"] != DBNull.Value)
                                country.Name = Convert.ToString(reader["CountryName"].ToString());
                            Countries.Add(country);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountriesDetails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Countries;
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mainServerConnectionString"></param>
        /// <returns></returns>
        public bool AddUpdateOTRequestTemplates(OtRequestTemplates template, string mainServerConnectionString, string OtAttachmentPath)
        {
            template.FileLocation = OtAttachmentPath;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    if (template.TransactionOperation == ModelBase.TransactionOperations.Add)
                    {
                        using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                        {
                            con.Open();
                            MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplate_V2600", con);
                            MyCommand.CommandType = CommandType.StoredProcedure;
                            MyCommand.Parameters.AddWithValue("_Code", template.Code);
                            MyCommand.Parameters.AddWithValue("_TemplateName", template.TemplateName);
                            MyCommand.Parameters.AddWithValue("_Group", template.IdGroup);
                            MyCommand.Parameters.AddWithValue("_Region", template.IdRegion);
                            MyCommand.Parameters.AddWithValue("_Country", template.IdCountry);
                            MyCommand.Parameters.AddWithValue("_Plant", template.IdPlant);
                            MyCommand.Parameters.AddWithValue("_FileName", template.File);
                            MyCommand.Parameters.AddWithValue("_FileLocation", template.FileLocation);
                            MyCommand.Parameters.AddWithValue("_InUse", template.InUse);
                            MyCommand.Parameters.AddWithValue("_CreatedBy", template.IdCreatedBy);
                            MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                            template.IdOTRequestTemplate = Convert.ToInt32(MyCommand.ExecuteScalar());
                            if (template.IdOTRequestTemplate > 0)
                            {
                                InsertOtRequestTemplatesAttachment(template);
                            }
                            con.Close();
                        }
                    }
                    if (template.TransactionOperation == ModelBase.TransactionOperations.Update)
                    {
                        using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                        {
                            con.Open();
                            MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplate_V2600", con);
                            MyCommand.CommandType = CommandType.StoredProcedure;
                            MyCommand.Parameters.AddWithValue("_idOTRequestTemplate", template.IdOtRequestTemplate);
                            MyCommand.Parameters.AddWithValue("_Code", template.Code);
                            MyCommand.Parameters.AddWithValue("_TemplateName", template.TemplateName);
                            MyCommand.Parameters.AddWithValue("_Group", template.Group);
                            MyCommand.Parameters.AddWithValue("_Region", template.Region);
                            MyCommand.Parameters.AddWithValue("_Country", template.Country);
                            MyCommand.Parameters.AddWithValue("_Plant", template.Plant);
                            MyCommand.Parameters.AddWithValue("_FileName", template.File);
                            MyCommand.Parameters.AddWithValue("_FileLocation", template.FileLocation);
                            MyCommand.Parameters.AddWithValue("_InUse", template.InUse);
                            MyCommand.Parameters.AddWithValue("_UpdatedBy", template.UpdatedBy);
                            MyCommand.Parameters.AddWithValue("_UpdatedIn", DateTime.Now);
                            MyCommand.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    foreach (OtRequestTemplateFeildOptions item in template.OtRequestTemplateFeildOptions)
                    {
                        item.IdOTRequestTemplate = template.IdOTRequestTemplate;
                        AddUpdateOtRequestTemplateFeildOptions(item, mainServerConnectionString);
                    }
                    //AddUpdateOtRequestTemplateFeildOptions(template, mainServerConnectionString);
                    //AddUpdateOtRequestTemplateTextFields(template, mainServerConnectionString);
                    //AddUpdateOtRequestTemplateLocationFields(template, mainServerConnectionString);
                    //AddUpdateOtRequestTemplateCellFields(template, mainServerConnectionString);
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return true;
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mainServerConnectionString"></param>
        public void AddUpdateOtRequestTemplateFeildOptions(OtRequestTemplateFeildOptions OtRequestTemplateFeildOption, string mainServerConnectionString)
        {
            int IdOTRequestFieldOption;
            if (OtRequestTemplateFeildOption.TransactionOperation == ModelBase.TransactionOperations.Add)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateFieldOptions_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplate", OtRequestTemplateFeildOption.IdOTRequestTemplate);
                    MyCommand.Parameters.AddWithValue("_IdField", OtRequestTemplateFeildOption.IdField);
                    MyCommand.Parameters.AddWithValue("_idFieldType", OtRequestTemplateFeildOption.IdFieldType);
                    MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateFeildOption.CreatedBy);
                    MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                    IdOTRequestFieldOption = Convert.ToInt32(MyCommand.ExecuteScalar());
                    con.Close();
                }
                if (OtRequestTemplateFeildOption.OtRequestTemplateCellField != null)
                {
                    OtRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestFieldOption = IdOTRequestFieldOption;
                    AddUpdateOtRequestTemplateCellFields(OtRequestTemplateFeildOption.OtRequestTemplateCellField, mainServerConnectionString);
                }
                else if (OtRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                {
                    OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestFieldOption = IdOTRequestFieldOption;
                    AddUpdateOtRequestTemplateTextFields(OtRequestTemplateFeildOption.OtRequestTemplateTextField, mainServerConnectionString);
                }
                else if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                {
                    OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestFieldOption = IdOTRequestFieldOption;
                    AddUpdateOtRequestTemplateLocationFields(OtRequestTemplateFeildOption.OtRequestTemplateLocationField, mainServerConnectionString);
                }
            }
            if (OtRequestTemplateFeildOption.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplateFieldOptions_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplateFieldOption", OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption);
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplate", OtRequestTemplateFeildOption.IdOTRequestTemplate);
                    MyCommand.Parameters.AddWithValue("_IdField", OtRequestTemplateFeildOption.IdField);
                    MyCommand.Parameters.AddWithValue("_idFieldType", OtRequestTemplateFeildOption.IdFieldType);
                    MyCommand.Parameters.AddWithValue("_UpdatedBy", OtRequestTemplateFeildOption.UpdatedBy);
                    MyCommand.Parameters.AddWithValue("_UpdatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    //IdOTRequestFieldOption = Convert.ToInt32(MyCommand.ExecuteScalar());
                    con.Close();
                }
                if (OtRequestTemplateFeildOption.ChangingField == true)
                {
                    if (OtRequestTemplateFeildOption.OtRequestTemplateCellField != null)
                    {
                        OtRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                        OtRequestTemplateFeildOption.OtRequestTemplateCellField.TransactionOperation = ModelBase.TransactionOperations.Add;
                        AddUpdateOtRequestTemplateCellFields(OtRequestTemplateFeildOption.OtRequestTemplateCellField, mainServerConnectionString);
                    }
                    else if (OtRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                    {
                        OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                        OtRequestTemplateFeildOption.OtRequestTemplateTextField.TransactionOperation = ModelBase.TransactionOperations.Add;
                        AddUpdateOtRequestTemplateTextFields(OtRequestTemplateFeildOption.OtRequestTemplateTextField, mainServerConnectionString);
                    }
                    else if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                    {
                        OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                        OtRequestTemplateFeildOption.OtRequestTemplateLocationField.TransactionOperation = ModelBase.TransactionOperations.Add;
                        AddUpdateOtRequestTemplateLocationFields(OtRequestTemplateFeildOption.OtRequestTemplateLocationField, mainServerConnectionString);
                    }
                }
                else
                {
                    if (OtRequestTemplateFeildOption.OtRequestTemplateCellField != null)
                    {
                        OtRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                        AddUpdateOtRequestTemplateCellFields(OtRequestTemplateFeildOption.OtRequestTemplateCellField, mainServerConnectionString);
                    }
                    if (OtRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                    {
                        //Added 
                        if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                        {
                            if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField == 0)
                            {
                                //DeleteLocationFields_V2610(OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField, mainServerConnectionString);
                                DeleteTextFields_V2610(OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField, mainServerConnectionString);
                                OtRequestTemplateFeildOption.OtRequestTemplateTextField = null;
                                return;
                            }
                        }
                        else
                        {
                            if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField == null)
                            {
                                OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                                AddUpdateOtRequestTemplateTextFields(OtRequestTemplateFeildOption.OtRequestTemplateTextField, mainServerConnectionString);
                            }
                        }
                    }
                    if (OtRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                    {
                        if (OtRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                        {
                            if (OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField == 0)
                            {
                                //DeleteTextFields_V2610(OtRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField, mainServerConnectionString);
                                DeleteLocationFields_V2610(OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField, mainServerConnectionString);
                                OtRequestTemplateFeildOption.OtRequestTemplateLocationField = null;
                            }
                        }
                        else
                        {
                            if (OtRequestTemplateFeildOption.OtRequestTemplateTextField == null)
                            {
                                OtRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestFieldOption = OtRequestTemplateFeildOption.IdOTRequestTemplateFieldOption;
                                AddUpdateOtRequestTemplateLocationFields(OtRequestTemplateFeildOption.OtRequestTemplateLocationField, mainServerConnectionString);
                            }
                        }
                    }
                }
            }
        }
        //Added
        public void DeleteTextFields_V2610(int IdOTRequestTemplateTextField, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeletetTextFields_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOTRequestTemplateTextField", IdOTRequestTemplateTextField);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteTextFields_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //Added
        public void DeleteLocationFields_V2610(int IdOTRequestTemplateLocationField, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeletetLocationFields_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOTRequestTemplateLocationField", IdOTRequestTemplateLocationField);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteLocationFields_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mainServerConnectionString"></param>
        public void AddUpdateOtRequestTemplateTextFields(OtRequestTemplateTextFields OtRequestTemplateTextField, string mainServerConnectionString)
        {
            if (OtRequestTemplateTextField.TransactionOperation == ModelBase.TransactionOperations.Add)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateTextFields_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateTextField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateTextField.Keyword);
                    MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateTextField.Delimiter);
                    MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateTextField.CreatedBy);
                    MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
            if (OtRequestTemplateTextField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                if (OtRequestTemplateTextField.IdOTRequestTemplateTextField == 0)
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateTextFields_V2600", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateTextField.IdOTRequestFieldOption);
                        MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateTextField.Keyword);
                        MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateTextField.Delimiter);
                        MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateTextField.CreatedBy);
                        MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            if (OtRequestTemplateTextField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplateTextFields_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplateTextField", OtRequestTemplateTextField.IdOTRequestTemplateTextField);
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateTextField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateTextField.Keyword);
                    MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateTextField.Delimiter);
                    MyCommand.Parameters.AddWithValue("_UpdatedBy", OtRequestTemplateTextField.UpdatedBy);
                    MyCommand.Parameters.AddWithValue("_UpdatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mainServerConnectionString"></param>
        public void AddUpdateOtRequestTemplateLocationFields(OtRequestTemplateLocationFields OtRequestTemplateLocationField, string mainServerConnectionString)
        {
            if (OtRequestTemplateLocationField.TransactionOperation == ModelBase.TransactionOperations.Add)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateLocationField_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateLocationField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Coordinates", OtRequestTemplateLocationField.Coordinates);
                    MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateLocationField.CreatedBy);
                    MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
            if (OtRequestTemplateLocationField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                if (OtRequestTemplateLocationField.IdOTRequestTemplateLocationField == 0)
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateLocationField_V2600", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateLocationField.IdOTRequestFieldOption);
                        MyCommand.Parameters.AddWithValue("_Coordinates", OtRequestTemplateLocationField.Coordinates);
                        MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateLocationField.CreatedBy);
                        MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            if (OtRequestTemplateLocationField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplateLocationField_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplateLocationField", OtRequestTemplateLocationField.IdOTRequestTemplateLocationField);
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateLocationField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Coordinates", OtRequestTemplateLocationField.Coordinates);
                    MyCommand.Parameters.AddWithValue("_UpdatedBy", OtRequestTemplateLocationField.UpdatedBy);
                    MyCommand.Parameters.AddWithValue("_UpdatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mainServerConnectionString"></param>
        public void AddUpdateOtRequestTemplateCellFields(OtRequestTemplateCellFields OtRequestTemplateCellField, string mainServerConnectionString)
        {
            if (OtRequestTemplateCellField.TransactionOperation == ModelBase.TransactionOperations.Add)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateFieldCellField_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateCellField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Cells", OtRequestTemplateCellField.Cells);
                    MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateCellField.Keyword);
                    MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateCellField.Delimiter);
                    MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateCellField.CreatedBy);
                    MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
            if (OtRequestTemplateCellField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                if (OtRequestTemplateCellField.IdOTRequestTemplateCellField == 0)
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplateFieldCellField_V2600", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateCellField.IdOTRequestFieldOption);
                        MyCommand.Parameters.AddWithValue("_Cells", OtRequestTemplateCellField.Cells);
                        MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateCellField.Keyword);
                        MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateCellField.Delimiter);
                        MyCommand.Parameters.AddWithValue("_CreatedBy", OtRequestTemplateCellField.CreatedBy);
                        MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            if (OtRequestTemplateCellField.TransactionOperation == ModelBase.TransactionOperations.Update)
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplateFieldCellField_V2600", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idOTRequestTemplateTextField", OtRequestTemplateCellField.IdOTRequestTemplateCellField);
                    MyCommand.Parameters.AddWithValue("_idOTRequestFieldOption", OtRequestTemplateCellField.IdOTRequestFieldOption);
                    MyCommand.Parameters.AddWithValue("_Cells", OtRequestTemplateCellField.Cells);
                    MyCommand.Parameters.AddWithValue("_Keyword", OtRequestTemplateCellField.Keyword);
                    MyCommand.Parameters.AddWithValue("_Delimiter", OtRequestTemplateCellField.Delimiter);
                    MyCommand.Parameters.AddWithValue("_UpdatedBy", OtRequestTemplateCellField.UpdatedBy);
                    MyCommand.Parameters.AddWithValue("_UpdatedIn", DateTime.Now);
                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public string GetCode(string connectionString)
        {
            string code;
            int idOTRequestTemplate = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_GetMaxOTRequestTemplate", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = MyCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idOTRequestTemplate"] != DBNull.Value)
                                idOTRequestTemplate = Convert.ToInt32(reader["idOTRequestTemplate"]);
                        }
                    }
                }
                idOTRequestTemplate++;
                int currentYear = DateTime.Now.Year;
                string yearSuffix = currentYear.ToString().Substring(2);
                string incrementalNumber = idOTRequestTemplate.ToString("D4");
                code = $"POT{yearSuffix}.{incrementalNumber}";
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error getCode(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return code;
        }
        public byte[] ExcelFileAttachedDoc(string OtAttachmentPath, Emailattachment file)
        {
            byte[] bytes = null;
            string fileUploadPath = Path.Combine(OtAttachmentPath, file.AttachmentName);
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public byte[] FileAttachedDoc(string OtAttachmentPath, OtRequestTemplates link)
        {
            byte[] bytes = null;
            string fileUploadPath = Path.Combine(OtAttachmentPath, link.File);
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// //[pooja.jadhav][20-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetPoAnalizerTag_V2600(string ConnectionString)
        {
            Dictionary<int, string> PoAnalizerTag = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoAnalizerTag_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["IdLookupValue"].ToString());
                            string value = Convert.ToString(reader["Value"].ToString());
                            PoAnalizerTag[id] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPoAnalizerTag_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PoAnalizerTag;
        }
        /// <summary>
        /// //[pooja.jadhav][20-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetPORequestTemplateFieldType_V2600(string ConnectionString)
        {
            Dictionary<int, string> PORequestTemplateFieldType = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPORequestTemplateFieldType_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["IdLookupValue"].ToString());
                            string value = Convert.ToString(reader["Value"].ToString());
                            PORequestTemplateFieldType[id] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPORequestTemplateFieldType_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PORequestTemplateFieldType;
        }
        /// <summary>
        /// //[pooja.jadhav][20-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public bool InsertOtRequestTemplatesAttachment(OtRequestTemplates template)
        {
            string filePath = template.FileLocation + "\\" + template.File;
            try
            {
                if (template.FileDocInBytes != null)
                {
                    if (!Directory.Exists(template.FileLocation))
                    {
                        Directory.CreateDirectory(template.FileLocation);
                    }
                    else
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    //template.FileDocInBytes = FileAttachedDoc(template.FileLocation, template);
                    File.WriteAllBytes(filePath, template.FileDocInBytes);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOtRequestTemplatesAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        public bool InsertOtRequestTemplatesAttachment_V2610(OtRequestTemplates template, string mainServerConnectionString)
        {
            string filePath = template.FileLocation + "\\" + template.IdOTRequestTemplate + "\\" + template.File;
            string filePath1 = template.FileLocation + "\\" + template.IdOTRequestTemplate;
            string filelocation = template.IdOTRequestTemplate + "\\\\" + template.File;
            using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
            {
                con.Open();
                MySqlCommand MyCommand = new MySqlCommand("OTM_Update_FileLocation_V2600", con);
                MyCommand.CommandType = CommandType.StoredProcedure;
                MyCommand.Parameters.AddWithValue("_IdOTRequestTemplate", template.IdOTRequestTemplate);
                MyCommand.Parameters.AddWithValue("_FileLocation", filelocation);
                MyCommand.ExecuteScalar();
                con.Close();
            }
            try
            {
                if (template.FileDocInBytes != null)
                {
                    if (!Directory.Exists(filePath1))
                    {
                        Directory.CreateDirectory(filePath1);
                    }
                    else
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    //template.FileDocInBytes = FileAttachedDoc(template.FileLocation, template);
                    File.WriteAllBytes(filePath, template.FileDocInBytes);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOtRequestTemplatesAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        /// <summary>
        /// [pooja.jadhav][21-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetPORequestTemplateFieldTypeForPDF_V2600(string ConnectionString)
        {
            Dictionary<int, string> PORequestTemplateFieldType = new Dictionary<int, string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPORequestTemplateFieldTypeForPDF_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["IdLookupValue"].ToString());
                            string value = Convert.ToString(reader["Value"].ToString());
                            PORequestTemplateFieldType[id] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPORequestTemplateFieldTypeForPDF_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PORequestTemplateFieldType;
        }
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public List<CustomerCountriesDetails> GetAllCustomersAndCountries_V2600(string connectionString)
        {
            List<CustomerCountriesDetails> CustomerList = new List<CustomerCountriesDetails>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("GetAllCustomersAndCountris_V2600", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerCountriesDetails cust = new CustomerCountriesDetails();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Customer"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Customer"]);
                        if (reader["IdSite"] != DBNull.Value)
                            cust.Idsite = Convert.ToInt32(reader["IdSite"]);
                        if (reader["Site"] != DBNull.Value)
                            cust.Site = Convert.ToString(reader["Site"]);
                        if (reader["IdCountry"] != DBNull.Value)
                            cust.IdCountries = Convert.ToInt32(reader["IdCountry"]);
                        if (reader["Countries"] != DBNull.Value)
                            cust.Countries = Convert.ToString(reader["Countries"]);
                        if (reader["Region"] != DBNull.Value)
                            cust.Region = Convert.ToString(reader["Region"]);
                        CustomerList.Add(cust);
                    }
                }
            }
            return CustomerList;
        }
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public List<POAnalyzerOTTemplate> GetOTRequestTemplateByCustomer_V2600(string ConnectionString, int idCustomer)
        {
            List<POAnalyzerOTTemplate> TemplateList = new List<POAnalyzerOTTemplate>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPDFOTRequestTemplateByCustomer_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", idCustomer);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            POAnalyzerOTTemplate template = new POAnalyzerOTTemplate();
                            try
                            {
                                if (empReader["idOTRequestTemplate"] != DBNull.Value)
                                    template.IdOTRequestTemplate = Convert.ToInt32(empReader["idOTRequestTemplate"]);
                                if (empReader["TemplateName"] != DBNull.Value)
                                    template.TemplateName = empReader["TemplateName"].ToString();
                                if (empReader["idCustomer"] != DBNull.Value)
                                    template.IdCustomer = Convert.ToInt32(empReader["idCustomer"]);
                                if (empReader["Customer"] != DBNull.Value)
                                    template.Customer = empReader["Customer"].ToString();
                                if (empReader["idCountry"] != DBNull.Value)
                                    template.IdCountries = Convert.ToInt32(empReader["idCountry"]);
                                if (empReader["CountriesName"] != DBNull.Value)
                                    template.Countries = empReader["CountriesName"].ToString();
                                if (empReader["idRegion"] != DBNull.Value)
                                    template.IdRegion = Convert.ToInt32(empReader["idRegion"]);
                                if (empReader["RegionName"] != DBNull.Value)
                                    template.Region = empReader["RegionName"].ToString();
                                if (empReader["idPlant"] != DBNull.Value)
                                    template.IdPlant = Convert.ToInt32(empReader["idPlant"]);
                                if (empReader["PlantName"] != DBNull.Value)
                                    template.Plant = empReader["PlantName"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            TemplateList.Add(template);
                        }
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                TextFileTemplateValue tag = new TextFileTemplateValue();
                                try
                                {
                                    if (empReader["idOTRequestTemplate"] != DBNull.Value)
                                    {
                                        POAnalyzerOTTemplate exp = TemplateList.Where(i => i.IdOTRequestTemplate == Convert.ToInt32(empReader["idOTRequestTemplate"])).FirstOrDefault();
                                        if (exp.TextValue == null)
                                        {
                                            exp.TextValue = new List<TextFileTemplateValue>();
                                        }
                                        if (empReader["idOTRequestTemplateFieldOption"] != DBNull.Value)
                                            tag.IdOTRequestTemplateField = Convert.ToInt32(empReader["idOTRequestTemplateFieldOption"]);
                                        if (empReader["IdField"] != DBNull.Value)
                                            tag.Idfield = Convert.ToInt32(empReader["IdField"]);
                                        if (empReader["Fieldvalue"] != DBNull.Value)
                                            tag.FieldValue = Convert.ToString(empReader["Fieldvalue"]);
                                        if (empReader["idFieldType"] != DBNull.Value)
                                            tag.IdfieldType = Convert.ToInt32(empReader["idFieldType"]);
                                        if (empReader["FieldTypevalue"] != DBNull.Value)
                                            tag.FieldTypeValue = Convert.ToString(empReader["FieldTypevalue"]);
                                        if (empReader["KeywordAndCoordinatesValues"] != DBNull.Value)
                                            tag.KeywordAndCoordinatesValues = Convert.ToString(empReader["KeywordAndCoordinatesValues"]);
                                        if (empReader["Delimiter"] != DBNull.Value)
                                            tag.Delimiter = Convert.ToString(empReader["Delimiter"]);
                                        exp.TextValue.Add(tag);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        //if (empReader.NextResult())
                        //{
                        //    while (empReader.Read())
                        //    {
                        //        LocationFileTemplateValue tag = new LocationFileTemplateValue();
                        //        try
                        //        {
                        //            if (empReader["idOTRequestTemplate"] != DBNull.Value)
                        //            {
                        //                POAnalyzerOTTemplate exp = TemplateList.Where(i => i.IdOTRequestTemplate == Convert.ToInt32(empReader["idOTRequestTemplate"])).FirstOrDefault();
                        //                if (exp.LocationValue == null)
                        //                {
                        //                    exp.LocationValue = new List<LocationFileTemplateValue>();
                        //                }
                        //                if (empReader["idOTRequestTemplateFieldOption"] != DBNull.Value)
                        //                    tag.IdOTRequestTemplateField = Convert.ToInt32(empReader["idOTRequestTemplateFieldOption"]);
                        //                if (empReader["IdField"] != DBNull.Value)
                        //                    tag.Idfield = Convert.ToInt32(empReader["IdField"]);
                        //                if (empReader["Fieldvalue"] != DBNull.Value)
                        //                    tag.FieldValue = Convert.ToString(empReader["Fieldvalue"]);
                        //                if (empReader["idFieldType"] != DBNull.Value)
                        //                    tag.IdfieldType = Convert.ToInt32(empReader["idFieldType"]);
                        //                if (empReader["FieldTypevalue"] != DBNull.Value)
                        //                    tag.FieldTypeValue = Convert.ToString(empReader["FieldTypevalue"]);
                        //                if (empReader["Coordinates"] != DBNull.Value)
                        //                    tag.Coordinates = Convert.ToString(empReader["Coordinates"]);
                        //                exp.LocationValue.Add(tag);
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }
                        //    }
                        //}
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTemplateByCustomer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return TemplateList;
        }
        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        public List<Country> GetCountriesByCustomerAndRegion(string ConnectionString, int IdCustomer, int IdRegion)
        {
            List<Country> Countries = new List<Country>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCountriesByCustomerAndRegion", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomer);
                    mySqlCommand.Parameters.AddWithValue("_IdRegion", IdRegion);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var country = new Country();
                            if (reader["idCountry"] != DBNull.Value)
                                country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                            if (reader["CountryName"] != DBNull.Value)
                                country.Name = Convert.ToString(reader["CountryName"].ToString());
                            Countries.Add(country);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountriesByCustomerAndRegion(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Countries;
        }
        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        public List<CustomerPlant> GetPlantByCustomerAndCountry(string connectionString, int IdCustomer, int IdCountry)
        {
            List<CustomerPlant> CustomerPlantList = new List<CustomerPlant>();
            CustomerPlant defaultCustomerPlant = new CustomerPlant();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPlantByCustomerAndCountry", mySqlConnection);
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomer);
                    mySqlCommand.Parameters.AddWithValue("_IdCountry", IdCountry);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            CustomerPlant customerPlant = new CustomerPlant();
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                                customerPlant.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdSite"].ToString());
                            if (mySqlDataReader["PlantName"] != DBNull.Value)
                                customerPlant.CustomerPlantName = mySqlDataReader["PlantName"].ToString();
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                                customerPlant.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"].ToString());
                            if (mySqlDataReader["idCountry"] != DBNull.Value)
                                customerPlant.IdCountry = Convert.ToInt32(mySqlDataReader["idCountry"].ToString());
                            CustomerPlantList.Add(customerPlant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPlantByCustomerAndCountry(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CustomerPlantList;
        }
        //[pramod.misal][GEOS2-6735][27-01-2025]
        public List<Email> GetAllUnprocessedEmails_V2600(string ConnectionStringgeos, string ConnectionStringemdep_geos, string poAnalyzerEmailToCheck, string attachedDocPath)
        {
            List<Email> EmailList = new List<Email>();
            List<LookupValue> UntructedExtensionList = GetLookupValues(ConnectionStringemdep_geos, 157);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetUnProcessedEmails_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["IsDeleted"] != DBNull.Value)
                                    email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                if (rdr["IdPORequest"] != DBNull.Value)
                                    email.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                try
                                {
                                    if (rdr["IdEmail"] != DBNull.Value)
                                    {
                                        Emailattachment attach = new Emailattachment();
                                        Email email = EmailList.FirstOrDefault(i => i.IdEmail == Convert.ToInt32(rdr["IdEmail"]));
                                        if (email != null)
                                        {
                                            if (email.EmailattachmentList == null)
                                            {
                                                email.EmailattachmentList = new List<Emailattachment>();
                                            }
                                            if (rdr["IdEmail"] != DBNull.Value)
                                                attach.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                            if (rdr["IdAttachment"] != DBNull.Value)
                                                attach.IdAttachment = Convert.ToInt32(rdr["IdAttachment"]);
                                            if (rdr["AttachmentName"] != DBNull.Value)
                                                attach.AttachmentName = Convert.ToString(rdr["AttachmentName"]);
                                            if (rdr["AttachmentPath"] != DBNull.Value)
                                                attach.AttachmentPath = Convert.ToString(rdr["AttachmentPath"]);
                                            if (rdr["CreatedIn"] != DBNull.Value)
                                                email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                            if (rdr["ModifiedIn"] != DBNull.Value)
                                                email.ModifiedIn = rdr["ModifiedIn"] as DateTime?;
                                            if (rdr["IsDeleted"] != DBNull.Value)
                                                email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                            if (rdr["CreatedBy"] != DBNull.Value)
                                                email.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);
                                            if (rdr["ModifiedBy"] != DBNull.Value)
                                                email.ModifiedBy = rdr["ModifiedBy"] as int?;
                                            if (rdr["AttachmentExtension"] != DBNull.Value)
                                                attach.AttachmentExtension = Convert.ToString(rdr["AttachmentExtension"]);
                                            //attach.FileContent = GetEmailAttachedDoc(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                            if (!UntructedExtensionList.Any(j => j.Value?.Trim() == attach.AttachmentExtension?.Trim()))
                                            {
                                                try
                                                {
                                                    if (attach.AttachmentExtension?.ToLower() == ".pdf")
                                                    {
                                                        attach.FileText = GetPdfAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.LocationFileText = GetPdfAttachedDocTextByLocation(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                    }
                                                    //[pramod.misal][GEOS2-6735][27-01-2025]
                                                    if (attach.AttachmentExtension?.ToLower() == ".xls" || attach.AttachmentExtension?.ToLower() == ".xlsx")
                                                    {
                                                        attach.ExcelFileText = GetExcelAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.FileDocInBytes = ExcelFileAttachedDoc(attachedDocPath, attach);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                        }
                                        email.EmailattachmentList.Add(attach);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        //[pramod.misal][GEOS2-6735][27-01-2025]
        public string GetExcelAttachedDocText(long idEmail, string attachedDocPath, string savedFileName)
        {
            var content = "";
            try
            {
                string fileUploadPath = Path.Combine(attachedDocPath, idEmail.ToString(), savedFileName);
                if (File.Exists(fileUploadPath))
                {
                    using (var package = new ExcelPackage(new FileInfo(fileUploadPath)))
                    {
                        foreach (var worksheet in package.Workbook.Worksheets)
                        {
                            try
                            {
                                for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                                    {
                                        var cellValue = worksheet.Cells[row, col].Text;
                                        if (!string.IsNullOrWhiteSpace(cellValue))
                                        {
                                            content += cellValue + " ";
                                        }
                                    }
                                    content += Environment.NewLine;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetExcelAttachedDocText(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetExcelAttachedDocText(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return content;
        }
        //[pramod.misal][GEOS2-6735][23.01.2025]
        public List<POAnalyzerOTTemplate> GetOTRequestExcelTemplateByCustomer_V2600(string ConnectionString, int idCustomer)
        {
            List<POAnalyzerOTTemplate> TemplateList = new List<POAnalyzerOTTemplate>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetExcelOTRequestTemplateByCustomer_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", idCustomer);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        Dictionary<long, POAnalyzerOTTemplate> templateDictionary = new Dictionary<long, POAnalyzerOTTemplate>();
                        // First result set: Template data
                        POAnalyzerOTTemplate template = new POAnalyzerOTTemplate();
                        while (empReader.Read())
                        {
                            try
                            {
                                if (empReader["idOTRequestTemplate"] != DBNull.Value)
                                    template.IdOTRequestTemplate = Convert.ToInt32(empReader["idOTRequestTemplate"]);
                                if (empReader["TemplateName"] != DBNull.Value)
                                    template.TemplateName = empReader["TemplateName"].ToString();
                                if (empReader["idCustomer"] != DBNull.Value)
                                    template.IdCustomer = Convert.ToInt32(empReader["idCustomer"].ToString());
                                if (empReader["Customer"] != DBNull.Value)
                                    template.Customer = empReader["Customer"].ToString();
                                if (empReader["idCountry"] != DBNull.Value)
                                    template.IdCountries = Convert.ToInt32(empReader["idCountry"].ToString());
                                if (empReader["CountriesName"] != DBNull.Value)
                                    template.Countries = empReader["CountriesName"].ToString();
                                if (empReader["idRegion"] != DBNull.Value)
                                    template.IdRegion = Convert.ToInt32(empReader["idRegion"].ToString());
                                if (empReader["RegionName"] != DBNull.Value)
                                    template.Region = empReader["RegionName"].ToString();
                                if (empReader["idPlant"] != DBNull.Value)
                                    template.IdPlant = Convert.ToInt32(empReader["idPlant"].ToString());
                                if (empReader["PlantName"] != DBNull.Value)
                                    template.Plant = empReader["PlantName"].ToString();
                                // Initialize the ExcelRangeList property
                                template.ExcelRangeValue = new List<ExcelFileTemplateValue>();
                                templateDictionary[template.IdOTRequestTemplate] = template;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTRequestExcelTemplateByCustomer_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                        // Second result set: Excel range data
                        if (empReader.NextResult())
                        {
                            while (empReader.Read())
                            {
                                ExcelFileTemplateValue excelrange = new ExcelFileTemplateValue();
                                try
                                {
                                    if (empReader["idOTRequestTemplate"] != DBNull.Value)
                                        excelrange.IdOTRequestTemplateTextField = Convert.ToInt32(empReader["idOTRequestTemplate"]);
                                    if (empReader["idOTRequestTemplateFieldOption"] != DBNull.Value)
                                        excelrange.IdOTRequestTemplateFieldOption = Convert.ToInt32(empReader["idOTRequestTemplateFieldOption"]);
                                    if (empReader["Fieldvalue"] != DBNull.Value)
                                        excelrange.FieldValue = empReader["Fieldvalue"].ToString();
                                    if (empReader["cells"] != DBNull.Value)
                                        excelrange.Range = empReader["cells"].ToString();
                                    if (empReader["Keyword"] != DBNull.Value)
                                        excelrange.Keyword = empReader["Keyword"].ToString();
                                    if (empReader["Delimiter"] != DBNull.Value)
                                        excelrange.Delimiter = empReader["Delimiter"].ToString();
                                    if (empReader["idFieldType"] != DBNull.Value)
                                        excelrange.IdFieldType = Convert.ToInt32(empReader["idFieldType"].ToString());
                                    if (empReader["FieldTypevalue"] != DBNull.Value)
                                        excelrange.FieldTypevalue = empReader["FieldTypevalue"].ToString();
                                    if (template.IdOTRequestTemplate.ToString() != null)
                                    {
                                        int templateId = Convert.ToInt32(template.IdOTRequestTemplate);
                                        if (templateDictionary.ContainsKey(templateId))
                                        {
                                            templateDictionary[templateId].ExcelRangeValue.Add(excelrange);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetOTRequestExcelTemplateByCustomer_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                        }
                        // Add all templates from the dictionary to the final list
                        TemplateList.AddRange(templateDictionary.Values);
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTRequestExcelTemplateByCustomer_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return TemplateList;
        }
        //[rahul.gadhave][GEOS2-6720][29.01.2025]
        //[nsatpute][GEOS2-6720][17.02.2025]
        public List<PORequestDetails> GetPORequestDetails_V2610_V1(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2610_V1";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["SenderEmployeeCode"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderEmployeeCode = rdr["SenderEmployeeCode"].ToString();
                                    if (rdr["SenderJobDescriptionTitle"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderJobDescriptionTitle = rdr["SenderJobDescriptionTitle"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["ToRecipientNameEmployeeCodes"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientNameEmployeeCodes = rdr["ToRecipientNameEmployeeCodes"].ToString();
                                    //if (rdr["ToRecipientNameJobDescriptionTitles"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                    //    po.ToRecipientNameJobDescriptionTitles = rdr["ToRecipientNameJobDescriptionTitles"].ToString();
                                    //[pramod.misal][17.02.2025][GEOS2-6719]
                                    #region [pramod.misal][17.02.2025][GEOS2-6719]
                                    //po.Group = Convert.ToString(rdr["Group"]);
                                    //po.Plant = Convert.ToString(rdr["Plant"]);
                                    if (rdr["EmailBody"] != DBNull.Value)
                                        po.Emailbody = rdr["EmailBody"].ToString();
                                    #endregion
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offer = rdr["Offers"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";
                                    if (rdr["IDPOfinalresult"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IDPOfinalresult"].ToString());
                                    if (rdr["PONumber"] != DBNull.Value)
                                        po.PONumber = rdr["PONumber"].ToString();
                                    else
                                        po.PONumber = "";
                                    if (rdr["PODate"] != DBNull.Value)
                                        po.DateIssued = Convert.ToDateTime(rdr["PODate"]);
                                    if (rdr["TransferAmount"] != DBNull.Value)
                                        po.TransferAmount = Convert.ToDouble(rdr["TransferAmount"]);
                                    else
                                        po.TransferAmount = 0;
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Email = rdr["Email"].ToString();
                                    if (rdr["Customer"] != DBNull.Value)
                                        po.Customer = rdr["Customer"].ToString();
                                    else
                                        po.Customer = "";
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Contact = rdr["Email"].ToString();
                                    else
                                        po.Contact = "";
                                    if (rdr["Incoterm"] != DBNull.Value)
                                        po.POIncoterms = rdr["Incoterm"].ToString();
                                    else
                                        po.POIncoterms = "";
                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["ShipTo"] != DBNull.Value)
                                        po.ShipTo = rdr["ShipTo"].ToString();
                                    else
                                        po.ShipTo = "";
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["CCNameEmployeeCodes"] != DBNull.Value)
                                        po.CCNameEmployeeCodes = rdr["CCNameEmployeeCodes"].ToString();
                                    //if (rdr["CCNameJobDescriptionTitles"] != DBNull.Value)
                                    //    po.CCNameJobDescriptionTitles = rdr["CCNameJobDescriptionTitles"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["InvoiceTo"] != DBNull.Value)
                                        po.InvoioceTO = rdr["InvoiceTo"].ToString();
                                    if (rdr["offer"] != DBNull.Value)
                                        po.Offers = rdr["offer"].ToString();
                                    if (rdr["PaymentTerms"] != DBNull.Value)
                                        po.POPaymentTerm = rdr["PaymentTerms"].ToString();
                                    poList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        public List<OtRequestTemplates> GetAllOtImportTemplate_V2610(string workbenchConnectionString, string OtAttachmentPath)
        {
            List<OtRequestTemplates> Otrequesttemplateslist = new List<OtRequestTemplates>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllOtImportTemplate_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OtRequestTemplates Otrequesttemplates = new OtRequestTemplates();
                            if (reader["idOTRequestTemplate"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdOTRequestTemplate = Convert.ToInt32(reader["idOTRequestTemplate"]);
                            }
                            if (reader["Code"] != DBNull.Value)
                            {
                                Otrequesttemplates.Code = Convert.ToString(reader["Code"]);
                            }
                            if (reader["TemplateName"] != DBNull.Value)
                            {
                                Otrequesttemplates.TemplateName = Convert.ToString(reader["TemplateName"]);
                            }
                            if (reader["Group"] != DBNull.Value)
                            {
                                Otrequesttemplates.Group = Convert.ToString(reader["Group"]);
                            }
                            if (reader["Region"] != DBNull.Value)
                            {
                                Otrequesttemplates.Region = Convert.ToString(reader["Region"]);
                            }
                            if (reader["Country"] != DBNull.Value)
                            {
                                Otrequesttemplates.Country = Convert.ToString(reader["Country"]);
                            }
                            if (reader["Plant"] != DBNull.Value)
                            {
                                Otrequesttemplates.Plant = Convert.ToString(reader["Plant"]);
                            }
                            if (reader["CreatedBy"] != DBNull.Value)
                            {
                                Otrequesttemplates.CreatedBy = Convert.ToString(reader["CreatedBy"]);
                            }
                            if (reader["CreatedIn"] != DBNull.Value)
                            {
                                Otrequesttemplates.CreatedAt = Convert.ToDateTime(reader["CreatedIn"]);
                            }
                            if (reader["UpdatedBy"] != DBNull.Value)
                            {
                                Otrequesttemplates.UpdatedBy = Convert.ToString(reader["UpdatedBy"]);
                            }
                            if (reader["UpdatedIn"] != DBNull.Value)
                            {
                                Otrequesttemplates.UpdatedAt = Convert.ToDateTime(reader["UpdatedIn"]);
                            }
                            if (reader["File"] != DBNull.Value)
                            {
                                Otrequesttemplates.File = Convert.ToString(reader["File"]);
                                Otrequesttemplates.fileExtension = Path.GetExtension(Otrequesttemplates.File).ToLower();
                                //Otrequesttemplates.FileDocInBytes = FileAttachedDoc(OtAttachmentPath, Otrequesttemplates);
                                Otrequesttemplates.FileDocInBytes = FileAttachedDoc_V2610(OtAttachmentPath, Otrequesttemplates);
                            }
                            if (reader["InUse"] != DBNull.Value)
                            {
                                Otrequesttemplates.Action = Convert.ToString(reader["InUse"]);
                                if (Otrequesttemplates.Action == "True")
                                {
                                    Otrequesttemplates.Action = "Yes";
                                }
                                else
                                {
                                    Otrequesttemplates.Action = "No";
                                }
                            }
                            if (reader["InUse"] != DBNull.Value)
                            {
                                Otrequesttemplates.InUse = Convert.ToInt32(reader["InUse"]);
                            }
                            if (reader["idOTRequestTemplate"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdOTRequestTemplate = Convert.ToInt32(reader["idOTRequestTemplate"]);
                            }
                            //Edit 
                            if (reader["IdCustomer"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                            }
                            if (reader["IdRegion"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdRegion = Convert.ToInt32(reader["IdRegion"]);
                            }
                            if (reader["IdCountry"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdCountry = Convert.ToInt32(reader["IdCountry"]);
                            }
                            if (reader["IdCustomerPlant"] != DBNull.Value)
                            {
                                Otrequesttemplates.IdCustomerPlant = Convert.ToInt32(reader["IdCustomerPlant"]);
                            }
                            Otrequesttemplateslist.Add(Otrequesttemplates);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOtImportTemplate_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Otrequesttemplateslist;
        }
        public List<OtRequestTemplates> OTM_GetAllMappingFieldsData_V2610(string workbenchConnectionString, OtRequestTemplates ObjOtRequestTemplates)
        {
            List<OtRequestTemplates> Otrequesttemplateslist = new List<OtRequestTemplates>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllMappingFieldsData_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_Code", ObjOtRequestTemplates.Code);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OtRequestTemplates Otrequesttemplate = new OtRequestTemplates();
                            Otrequesttemplate.otRequestTemplateFeildOption = new OtRequestTemplateFeildOptions();
                            if (reader["idFieldType"] != DBNull.Value)
                            {
                                Otrequesttemplate.otRequestTemplateFeildOption.IdFieldType = Convert.ToInt32(reader["idFieldType"]);
                            }
                            if (reader["idOTRequestTemplateFieldOption"] != DBNull.Value)
                            {
                                Otrequesttemplate.otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = Convert.ToInt32(reader["idOTRequestTemplateFieldOption"]);
                            }
                            if (reader["IdField"] != DBNull.Value)
                            {
                                Otrequesttemplate.otRequestTemplateFeildOption.IdField = Convert.ToInt32(reader["IdField"]);
                            }
                            if (reader["Value"] != DBNull.Value)
                            {
                                string Value = Convert.ToString(reader["Value"]);
                            }
                            if (ObjOtRequestTemplates.fileExtension == ".pdf")
                            {
                                Otrequesttemplate.otRequestTemplateTextField = new OtRequestTemplateTextFields();
                                if (reader["Keyword"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateTextField.Keyword = reader["Keyword"].ToString();
                                }
                                if (reader["Delimiter"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateTextField.Delimiter = reader["Delimiter"].ToString();
                                }
                                if (reader["idOTRequestTemplateTextField"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateTextField.IdOTRequestTemplateTextField = Convert.ToInt32(reader["idOTRequestTemplateTextField"]);
                                }
                                Otrequesttemplate.otRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                if (reader["Coordinates"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateLocationField.Coordinates = reader["Coordinates"].ToString();
                                }
                                if (reader["idOTRequestTemplateLocationField"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateLocationField.IdOTRequestTemplateLocationField = Convert.ToInt32(reader["idOTRequestTemplateLocationField"]);
                                }
                            }
                            else
                            {
                                Otrequesttemplate.otRequestTemplateCellField = new OtRequestTemplateCellFields();
                                if (reader["Cells"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateCellField.Cells = reader["Cells"].ToString();
                                }
                                if (reader["ExcelKeyword"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateCellField.Keyword = reader["ExcelKeyword"].ToString();
                                }
                                if (reader["ExcelDelimiter"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateCellField.Delimiter = reader["ExcelDelimiter"].ToString();
                                }
                                if (reader["idOTRequestTemplateCellField"] != DBNull.Value)
                                {
                                    Otrequesttemplate.otRequestTemplateCellField.IdOTRequestTemplateCellField = Convert.ToInt32(reader["idOTRequestTemplateCellField"]);
                                }
                            }
                            Otrequesttemplateslist.Add(Otrequesttemplate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOtImportTemplate_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Otrequesttemplateslist;
        }
        public OtRequestTemplates AddUpdateOTRequestTemplates_V2610(OtRequestTemplates template, string mainServerConnectionString, string OtAttachmentPath)
        {
            template.FileLocation = OtAttachmentPath;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    if (template.TransactionOperation == ModelBase.TransactionOperations.Add)
                    {
                        using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                        {
                            con.Open();
                            MySqlCommand MyCommand = new MySqlCommand("OTM_Insert_OtRequestTemplate_V2600", con);
                            MyCommand.CommandType = CommandType.StoredProcedure;
                            MyCommand.Parameters.AddWithValue("_Code", template.Code);
                            MyCommand.Parameters.AddWithValue("_TemplateName", template.TemplateName);
                            MyCommand.Parameters.AddWithValue("_Group", template.IdGroup);
                            MyCommand.Parameters.AddWithValue("_Region", template.IdRegion);
                            MyCommand.Parameters.AddWithValue("_Country", template.IdCountry);
                            MyCommand.Parameters.AddWithValue("_Plant", template.IdPlant);
                            MyCommand.Parameters.AddWithValue("_FileName", template.File);
                            MyCommand.Parameters.AddWithValue("_FileLocation", template.FileLocation);
                            MyCommand.Parameters.AddWithValue("_InUse", template.InUse);
                            MyCommand.Parameters.AddWithValue("_CreatedBy", template.IdCreatedBy);
                            MyCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now);
                            template.IdOTRequestTemplate = Convert.ToInt32(MyCommand.ExecuteScalar());
                            con.Close();
                            if (template.IdOTRequestTemplate > 0)
                            {
                                InsertOtRequestTemplatesAttachment_V2610(template, mainServerConnectionString);
                            }
                        }
                    }
                    if (template.TransactionOperation == ModelBase.TransactionOperations.Update)
                    {
                        template.IdOTRequestTemplate = template.IdOtRequestTemplate;
                        using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                        {
                            con.Open();
                            MySqlCommand MyCommand = new MySqlCommand("OTM_Update_OtRequestTemplate_V2610", con);
                            MyCommand.CommandType = CommandType.StoredProcedure;
                            MyCommand.Parameters.AddWithValue("_idOTRequestTemplate", template.IdOtRequestTemplate);
                            MyCommand.Parameters.AddWithValue("_Code", template.Code);
                            MyCommand.Parameters.AddWithValue("_TemplateName", template.TemplateName);
                            MyCommand.Parameters.AddWithValue("_Group", template.IdGroup);
                            MyCommand.Parameters.AddWithValue("_Region", template.IdRegion);
                            MyCommand.Parameters.AddWithValue("_Country", template.IdCountry);
                            MyCommand.Parameters.AddWithValue("_Plant", template.IdPlant);
                            MyCommand.Parameters.AddWithValue("_FileName", template.File);
                            //MyCommand.Parameters.AddWithValue("_FileLocation", template.FileLocation);
                            MyCommand.Parameters.AddWithValue("_InUse", template.InUse);
                            MyCommand.Parameters.AddWithValue("_UpdatedBy", template.IdUpdatedBy);
                            MyCommand.Parameters.AddWithValue("_UpdatedIn", template.UpdatedAt);
                            MyCommand.ExecuteNonQuery();
                            con.Close();
                            if (template.IdOTRequestTemplate > 0)
                            {
                                UpdateOtRequestTemplatesAttachment_V2610(template, mainServerConnectionString);
                            }
                        }
                    }
                    foreach (OtRequestTemplateFeildOptions item in template.OtRequestTemplateFeildOptions)
                    {
                        if (template.ChangingField == true)
                        {
                            item.ChangingField = template.ChangingField;
                        }
                        item.IdOTRequestTemplate = template.IdOTRequestTemplate;
                        AddUpdateOtRequestTemplateFeildOptions(item, mainServerConnectionString);
                    }
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return template;
        }
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public byte[] FileAttachedDoc_V2610(string OtAttachmentPath, OtRequestTemplates link)
        {
            byte[] bytes = null;
            string fileUploadPath = Path.Combine(OtAttachmentPath, link.IdOTRequestTemplate.ToString(), link.File);
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error FileAttachedDoc_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<Regions> GetRegions_V2610(string ConnectionString, int idCustomer)
        {
            List<Regions> RegionsList = new List<Regions>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetRegions_V2610", mySqlConnection);
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", idCustomer);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Regions = new Regions();
                            if (reader["IdLookupValue"] != DBNull.Value)
                                Regions.IdRegion = Convert.ToInt32(reader["IdLookupValue"].ToString());
                            if (reader["Value"] != DBNull.Value)
                                Regions.RegionName = Convert.ToString(reader["Value"].ToString());
                            RegionsList.Add(Regions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetRegions_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return RegionsList;
        }
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public bool UpdateOtRequestTemplatesAttachment_V2610(OtRequestTemplates template, string mainServerConnectionString)
        {
            string filePath = template.FileLocation + "\\" + template.IdOTRequestTemplate + "\\" + template.File;
            string filePath1 = template.FileLocation + "\\" + template.IdOTRequestTemplate;
            string filelocation = template.IdOTRequestTemplate + "\\\\" + template.File;
            using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
            {
                con.Open();
                MySqlCommand MyCommand = new MySqlCommand("OTM_Update_FileLocation_V2600", con);
                MyCommand.CommandType = CommandType.StoredProcedure;
                MyCommand.Parameters.AddWithValue("_IdOTRequestTemplate", template.IdOTRequestTemplate);
                MyCommand.Parameters.AddWithValue("_FileLocation", filelocation);
                MyCommand.ExecuteScalar();
                con.Close();
            }
            try
            {
                if (template.FileDocInBytes != null)
                {
                    if (!Directory.Exists(filePath1))
                    {
                        Directory.CreateDirectory(filePath1);
                    }
                    else
                    {
                        // Delete all files in the directory
                        foreach (var file in Directory.GetFiles(filePath1))
                        {
                            File.Delete(file);
                        }
                    }
                    //template.FileDocInBytes = FileAttachedDoc(template.FileLocation, template);
                    File.WriteAllBytes(filePath, template.FileDocInBytes);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOtRequestTemplatesAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public bool DeletetTextAndLocation_V2610(string connectionString, long IdOTRequestTemplate)
        {
            bool isDeleted = false;
            if (IdOTRequestTemplate > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeletetTextAndLocation_V2610", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_idOTRequestTemplate", IdOTRequestTemplate);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    isDeleted = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error DeletetTextAndLocation_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public bool DeletetCellFields_V2610(string connectionString, long IdOTRequestTemplate)
        {
            bool isDeleted = false;
            if (IdOTRequestTemplate > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeletetCellFields_V2610", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_idOTRequestTemplate", IdOTRequestTemplate);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    isDeleted = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error DeletetCellFields_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        public bool AddEmails_V2610(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails_V2610", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_CCEmail", emailDetails.CCEmail);
                        mySqlCommand.Parameters.AddWithValue("_ToName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_ToEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CCName", emailDetails.CCName);
                        mySqlCommand.Parameters.AddWithValue("_SourceInboxId", emailDetails.SourceInboxId);
                        mySqlCommand.Parameters.AddWithValue("_CreatedIn", emailDetails.CreatedIn);
                        mySqlCommand.Parameters.AddWithValue("_EmailSentAt", emailDetails.EmailSentAt);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc_V2550(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        public bool AddPODetails_V2610(List<CustomerDetail> poDetailList, string mainServerConnectionString)
        {
            try
            {
                if (poDetailList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (CustomerDetail poReq in poDetailList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddPODetails_V2610", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPORequest", poReq.IdPORequest);
                            mySqlCommand.Parameters.AddWithValue("_PONumber", poReq.PONumber);
                            mySqlCommand.Parameters.AddWithValue("_PODate", poReq.DateIssued.Date);
                            mySqlCommand.Parameters.AddWithValue("_TransferAmount", poReq.TotalNetValue);
                            mySqlCommand.Parameters.AddWithValue("_Currency", poReq.Currency);
                            mySqlCommand.Parameters.AddWithValue("_Incoterm", poReq.Incoterm);
                            mySqlCommand.Parameters.AddWithValue("_Email", poReq.Email);
                            mySqlCommand.Parameters.AddWithValue("_Customer", poReq.Customer);
                            mySqlCommand.Parameters.AddWithValue("_Offer", poReq.Offer);
                            mySqlCommand.Parameters.AddWithValue("_InvoiceAddress", poReq.InvoiceAddress);
                            mySqlCommand.Parameters.AddWithValue("_InvoiceTo", poReq.InvoiceTo);
                            mySqlCommand.Parameters.AddWithValue("_ShipAddress", poReq.ShipTo);
                            mySqlCommand.Parameters.AddWithValue("_IntegrationDate", DateTime.Now);
                            mySqlCommand.Parameters.AddWithValue("_IdIntegrationUser", 1);
                            mySqlCommand.Parameters.AddWithValue("_RawText", poReq.FileText);
                            mySqlCommand.Parameters.AddWithValue("_IdAttachment", poReq.IdAttachment);
                            mySqlCommand.Parameters.AddWithValue("_PaymentTerms", poReq.PaymentTerms);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPODetails_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        /// <summary>
        /// //[pramod.misal][04.02.2025][GEOS2 - 6726]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<POEmployeeInfo> GetPOEmployeeInfoList_V2610(string ConnectionString)
        {
            List<POEmployeeInfo> Employees = new List<POEmployeeInfo>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOEmployeeInfoList_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            POEmployeeInfo employee = new POEmployeeInfo();
                            if (reader["IdEmployee"] != DBNull.Value)
                                employee.IdEmployee = Convert.ToInt64(reader["IdEmployee"].ToString());
                            if (reader["FullName"] != DBNull.Value)
                                employee.FullName = Convert.ToString(reader["FullName"].ToString());
                            if (reader["EmployeeCode"] != DBNull.Value)
                                employee.EmployeeCode = Convert.ToString(reader["EmployeeCode"].ToString());
                            if (reader["IdJobDescription"] != DBNull.Value)
                                employee.IdJobDescription = Convert.ToInt64(reader["IdJobDescription"].ToString());
                            if (reader["JobDescriptionTitle"] != DBNull.Value)
                                employee.JobDescriptionTitle = Convert.ToString(reader["JobDescriptionTitle"].ToString());
                            if (reader["Email"] != DBNull.Value)
                                employee.Email = Convert.ToString(reader["Email"].ToString());
                            Employees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPOEmployeeInfoList_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Employees;
        }
        //[rahul.gadhave][GEOS2-6720][29.01.2025]
        public List<PORequestDetails> GetPORequestDetails_V2610(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandTimeout = 6000;
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2610";
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offer = rdr["Offers"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                        po.AttachmentCount = Convert.ToInt16(rdr["attachCount"]);
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";
                                    if (rdr["IDPOfinalresult"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IDPOfinalresult"].ToString());
                                    if (rdr["PONumber"] != DBNull.Value)
                                        po.PONumber = rdr["PONumber"].ToString();
                                    else
                                        po.PONumber = "";
                                    if (rdr["PODate"] != DBNull.Value)
                                        po.DateIssued = Convert.ToDateTime(rdr["PODate"]);
                                    if (rdr["TransferAmount"] != DBNull.Value)
                                        po.TransferAmount = Convert.ToDouble(rdr["TransferAmount"]);
                                    else
                                        po.TransferAmount = 0;
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Email = rdr["Email"].ToString();
                                    if (rdr["Customer"] != DBNull.Value)
                                        po.Customer = rdr["Customer"].ToString();
                                    else
                                        po.Customer = "";
                                    if (rdr["Email"] != DBNull.Value)
                                        po.Contact = rdr["Email"].ToString();
                                    else
                                        po.Contact = "";
                                    if (rdr["Incoterm"] != DBNull.Value)
                                        po.POIncoterms = rdr["Incoterm"].ToString();
                                    else
                                        po.POIncoterms = "";
                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["ShipTo"] != DBNull.Value)
                                        po.ShipTo = rdr["ShipTo"].ToString();
                                    else
                                        po.ShipTo = "";
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["InvoiceTo"] != DBNull.Value)
                                        po.InvoioceTO = rdr["InvoiceTo"].ToString();
                                    if (rdr["offer"] != DBNull.Value)
                                        po.Offers = rdr["offer"].ToString();
                                    poList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        //[rahul.gadhave][GEOS2-6799][Date:19/02/2025]
        public List<Email> GetEmailCreatedIn_V2610(string ConnectionStringgeos, string poAnalyzerEmailToCheck)
        {
            List<Email> EmailList = new List<Email>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailCreatedIn_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                //[rahul.gadhave][GEOS2-6799][Date:19/02/2025]
                                if (rdr["EmailSentAt"] != DBNull.Value)
                                    email.EmailSentAt = rdr["EmailSentAt"] as DateTime?;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2570(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        /// <summary>
        ///  //[pramod.misal][12.03.2025][GEOS2 - 6719]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<CustomerContacts> GetAllCustomerContactsList_V2620(string ConnectionString)
        {
            List<CustomerContacts> CustomerContacts = new List<CustomerContacts>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCustomerContactsList_V2620", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerContacts Contacts = new CustomerContacts();
                            if (reader["GroupName"] != DBNull.Value)
                                Contacts.GroupName = Convert.ToString(reader["GroupName"].ToString());
                            if (reader["Plant"] != DBNull.Value)
                                Contacts.Plant = Convert.ToString(reader["Plant"].ToString());
                            if (reader["Email"] != DBNull.Value)
                                Contacts.Email = Convert.ToString(reader["Email"].ToString());

                            if (reader["IdCustomer"] != DBNull.Value)
                                Contacts.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());

                            if (reader["IdPlant"] != DBNull.Value)
                                Contacts.IdPlant = Convert.ToInt32(reader["IdPlant"].ToString());


                            CustomerContacts.Add(Contacts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCustomerContactsList_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CustomerContacts;
        }
        //[ashish.malkhede][GEOS2-7042][12-03-2025]
        private void AssignValue(PORegisteredDetails po, string columnName, MySqlDataReader rdr)
        {
            if (rdr[columnName] != DBNull.Value)
            {
                var property = po.GetType().GetProperty(columnName);
                if (property != null && property.CanWrite)
                {
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(DateTime?))
                    {
                        // Handle Nullable<DateTime>
                        property.SetValue(po, rdr[columnName] != DBNull.Value ? (DateTime?)rdr[columnName] : null);
                    }
                    else if (propertyType == typeof(DateTime))
                    {
                        // Handle DateTime (non-nullable)
                        property.SetValue(po, Convert.ToDateTime(rdr[columnName]));
                    }
                    else
                    {
                        // For other types (e.g., string, int, etc.), just use Convert.ChangeType
                        property.SetValue(po, Convert.ChangeType(rdr[columnName], propertyType));
                    }
                }
            }
        }
        //[ashish.malkhede][GEOS2-7042][12-03-2025]
        public async Task<List<PORegisteredDetails>> GetPORegisteredDetails_V2620_V1(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, string correnciesIconFilePath, string countryIconFilePath, PORegisteredDetailFilter filter)
        {
            List<PORegisteredDetails> poRegiList = new List<PORegisteredDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                //mySqlconn.Open();
                await mySqlconn.OpenAsync();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        // Set common properties
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);

                        // Determine stored procedure and set parameters based on filter
                        if (filter == null)
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetails_V2620";

                        }
                        else
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsFilter_V2620";
                            mySqlCommand.CommandTimeout = 6000;

                            // Add filter parameters
                            mySqlCommand.Parameters.AddWithValue("_Number", filter.Number);
                            mySqlCommand.Parameters.AddWithValue("_Type", filter.IdType);
                            mySqlCommand.Parameters.AddWithValue("_Group", filter.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_PlantC", filter.IdCustomerPlant);
                            mySqlCommand.Parameters.AddWithValue("_Sender", filter.Sender);
                            mySqlCommand.Parameters.AddWithValue("_Currency", filter.IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeFrom", filter.PoValueRangeFrom);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeTo", filter.PoValueRangeTo);
                            mySqlCommand.Parameters.AddWithValue("_Offer", filter.Offer);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateFrom", filter.ReceptionDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateTo", filter.ReceptionDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateFrom", filter.CreationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateTo", filter.CreationDateTo);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateFrom", filter.UpdateDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateTO", filter.UpdateDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateFrom", filter.CancellationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateTo", filter.CancellationDateTo);
                        }

                        using (MySqlDataReader rdr = await mySqlCommand.ExecuteReaderAsync().ConfigureAwait(false) as MySqlDataReader)
                        {
                            // Initialize HashSets for unique currency and country ISOs
                            var currencyISOs = new HashSet<string>();
                            var countryISOs = new HashSet<string>();

                            // Read data
                            while (await rdr.ReadAsync())
                            {
                                PORegisteredDetails po = new PORegisteredDetails();
                                try
                                {
                                    // Use a helper function to check for DBNull values and assign values to PORegisteredDetails
                                    AssignValue(po, "IdPO", rdr);
                                    AssignValue(po, "Code", rdr);
                                    AssignValue(po, "IdPOType", rdr);
                                    AssignValue(po, "Type", rdr);
                                    AssignValue(po, "CustomerGroup", rdr);
                                    AssignValue(po, "Plant", rdr);
                                    AssignValue(po, "Country", rdr);
                                    AssignValue(po, "CountryISO", rdr);
                                    AssignValue(po, "Region", rdr);
                                    AssignValue(po, "ReceptionDate", rdr);
                                    AssignValue(po, "Sender", rdr);
                                    AssignValue(po, "POValue", rdr);
                                    AssignValue(po, "Amount", rdr);
                                    AssignValue(po, "Remarks", rdr);
                                    AssignValue(po, "Currency", rdr);
                                    AssignValue(po, "LinkedOffer", rdr);
                                    AssignValue(po, "ShippingAddress", rdr);
                                    AssignValue(po, "IsOK", rdr);
                                    AssignValue(po, "Confirmation", rdr);
                                    AssignValue(po, "CreationDate", rdr);
                                    AssignValue(po, "Creator", rdr);
                                    AssignValue(po, "UpdaterDate", rdr);
                                    AssignValue(po, "Updater", rdr);
                                    AssignValue(po, "IsCancelled", rdr);

                                    // Handle cancellation date
                                    if (rdr["IsCancelled"] != DBNull.Value && rdr["IsCancelled"].ToString() != "PO Not Cancelled")
                                    {
                                        AssignValue(po, "Canceler", rdr);
                                        AssignValue(po, "CancellationDate", rdr);
                                    }

                                    // Collect unique currency and country ISO codes
                                    if (po.Currency != null && !currencyISOs.Contains(po.Currency))
                                        currencyISOs.Add(po.Currency);
                                    if (po.CountryISO != null && !countryISOs.Contains(po.CountryISO))
                                        countryISOs.Add(po.CountryISO);

                                    // Add PO to list
                                    poRegiList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }

                            // Assign icons for currencies and countries in batches
                            foreach (string currencyISO in currencyISOs)
                            {
                                byte[] currencyIcon = GetCountryIconFileInBytes(currencyISO, correnciesIconFilePath);
                                poRegiList.Where(ot => ot.Currency == currencyISO).ToList().ForEach(ot => ot.CurrencyIconBytes = currencyIcon);
                            }

                            foreach (string countryISO in countryISOs)
                            {
                                byte[] countryIcon = GetCountryIconFileInBytes(countryISO, countryIconFilePath);
                                poRegiList.Where(ot => ot.CountryISO == countryISO).ToList().ForEach(ot => ot.CountryIconBytes = countryIcon);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poRegiList;
        }

        //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
        public List<PORegisteredDetails> GetPORegisteredDetails_V2620(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, string correnciesIconFilePath, string countryIconFilePath, PORegisteredDetailFilter filter)
        {
            List<PORegisteredDetails> poRegiList = new List<PORegisteredDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                List<string> countryISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        if (filter == null)
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetails_V2620";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                        }
                        else
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsFilter_V2620";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandTimeout = 6000;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_Number", filter.Number);
                            mySqlCommand.Parameters.AddWithValue("_Type", filter.IdType);
                            mySqlCommand.Parameters.AddWithValue("_Group", filter.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_PlantC", filter.IdCustomerPlant);
                            mySqlCommand.Parameters.AddWithValue("_Sender", filter.Sender);
                            mySqlCommand.Parameters.AddWithValue("_Currency", filter.IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeFrom", filter.PoValueRangeFrom);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeTo", filter.PoValueRangeTo);
                            mySqlCommand.Parameters.AddWithValue("_Offer", filter.Offer);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateFrom", filter.ReceptionDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateTo", filter.ReceptionDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateFrom", filter.CreationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateTo", filter.CreationDateTo);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateFrom", filter.UpdateDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateTO", filter.UpdateDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateFrom", filter.CancellationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateTo", filter.CancellationDateTo);
                        }
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORegisteredDetails po = new PORegisteredDetails();
                                try
                                {
                                    if (rdr["IdPO"] != DBNull.Value)
                                        po.IdPO = Convert.ToInt64(rdr["IdPO"]);
                                    if (rdr["Code"] != DBNull.Value)
                                        po.Code = rdr["Code"].ToString();
                                    if (rdr["IdPOType"] != DBNull.Value)
                                        po.IdPOType = Convert.ToInt32(rdr["IdPOType"]);
                                    if (rdr["Type"] != DBNull.Value)
                                        po.Type = rdr["Type"].ToString();
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["Plant"] != DBNull.Value)
                                        po.Plant = rdr["Plant"].ToString();
                                    if (rdr["Country"] != DBNull.Value)
                                    {
                                        po.Country = rdr["Country"].ToString();
                                        po.CountryISO = rdr["CountryISO"].ToString();
                                    }
                                    if (rdr["Region"] != DBNull.Value)
                                        po.Region = rdr["Region"].ToString();
                                    if (rdr["ReceptionDate"] != DBNull.Value)
                                        po.ReceptionDate = Convert.ToDateTime(rdr["ReceptionDate"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["POValue"] != DBNull.Value)
                                        po.POValue = Convert.ToDouble(rdr["POValue"].ToString());
                                    if (rdr["Amount"] != DBNull.Value)
                                        po.Amount = Convert.ToDouble(rdr["Amount"].ToString());
                                    if (rdr["Remarks"] != DBNull.Value)
                                        po.Remarks = rdr["Remarks"].ToString();
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["LinkedOffer"] != DBNull.Value)
                                        po.LinkedOffer = rdr["LinkedOffer"].ToString();
                                    if (rdr["ShippingAddress"] != DBNull.Value)
                                        po.ShippingAddress = rdr["ShippingAddress"].ToString();
                                    if (rdr["IsOK"] != DBNull.Value)
                                        po.IsOK = rdr["IsOK"].ToString();
                                    if (rdr["Confirmation"] != DBNull.Value)
                                        po.Confirmation = rdr["Confirmation"].ToString();
                                    if (rdr["CreationDate"] != DBNull.Value)
                                        po.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);
                                    if (rdr["Creator"] != DBNull.Value)
                                        po.Creator = rdr["Creator"].ToString();
                                    if (rdr["UpdaterDate"] != DBNull.Value)
                                        po.UpdaterDate = Convert.ToDateTime(rdr["UpdaterDate"].ToString());
                                    if (rdr["Updater"] != DBNull.Value)
                                        po.Updater = rdr["Updater"].ToString();
                                    if (rdr["IsCancelled"] != DBNull.Value)
                                        po.IsCancelled = rdr["IsCancelled"].ToString(); // PO Not Cancelled
                                    if (rdr["IsCancelled"] != DBNull.Value && rdr["IsCancelled"].ToString() != "PO Not Cancelled")
                                    {
                                        if (rdr["Canceler"] != DBNull.Value)
                                            po.Canceler = rdr["Canceler"].ToString();
                                        if (rdr["CancellationDate"] != DBNull.Value)
                                            po.CancellationDate = Convert.ToDateTime(rdr["CancellationDate"].ToString());
                                    }
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (!countryISOs.Any(co => co.ToString() == po.CountryISO))
                                    {
                                        countryISOs.Add(po.CountryISO);
                                    }
                                    /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["IdSite"] != DBNull.Value)
                                        po.IdSite = Convert.ToInt32(rdr["IdSite"]);
                                    if (rdr["IdShippingAddress"] != DBNull.Value)
                                        po.IdShippingAddress = Convert.ToInt64(rdr["IdShippingAddress"]);
                                    if (rdr["IdCurrency"] != DBNull.Value)
                                        po.IdCurrency = Convert.ToInt32(rdr["IdCurrency"]);
                                    if (rdr["CreatorCode"] != DBNull.Value)
                                        po.CreatorCode = rdr["CreatorCode"].ToString();
                                    if (rdr["UpdaterCode"] != DBNull.Value)
                                        po.UpdaterCode = rdr["UpdaterCode"].ToString();
                                    if (rdr["CancelerCode"] != DBNull.Value)
                                        po.CancelerCode = rdr["CancelerCode"].ToString();
                                    if (rdr["AttachmentFileName"] != DBNull.Value)
                                        po.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    if (rdr["IdSender"] != DBNull.Value)
                                        po.IdSender = Convert.ToInt32(rdr["IdSender"]);
                                    //
                                    poRegiList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poRegiList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                            }
                            // for country
                            foreach (string item in countryISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                                poRegiList.Where(ot => ot.CountryISO == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                return poRegiList;
            }
        }

        public List<CustomerPlant> OTM_GetCustomerPlant_V2620(string connectionString)
        {
            List<CustomerPlant> CustomerPlantList = new List<CustomerPlant>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCustomerPlant_V2620", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            CustomerPlant customerPlant = new CustomerPlant();
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                                customerPlant.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdSite"].ToString());
                            if (mySqlDataReader["PlantName"] != DBNull.Value)
                                customerPlant.CustomerPlantName = mySqlDataReader["PlantName"].ToString();
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                                customerPlant.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"].ToString());
                            if (mySqlDataReader["idCountry"] != DBNull.Value)
                                customerPlant.IdCountry = Convert.ToInt32(mySqlDataReader["idCountry"].ToString());
                            if (mySqlDataReader["CountryName"] != DBNull.Value)
                                customerPlant.Country = Convert.ToString(mySqlDataReader["CountryName"].ToString());
                            if (mySqlDataReader["CityName"] != DBNull.Value)
                                customerPlant.City = Convert.ToString(mySqlDataReader["CityName"].ToString());
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                customerPlant.Iso = Convert.ToString(mySqlDataReader["iso"]);
                                customerPlant.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + customerPlant.Iso + ".png";
                            }
                            CustomerPlantList.Add(customerPlant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetCustomerPlant_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return CustomerPlantList;
        }
        //[Rahul.Gadhave][GEOS-7040][Date:28-02-2025][https://helpdesk.emdep.com/browse/GEOS2-7040]
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2620(string ConnectionStringGeos, PORegisteredDetails poRegisteredDetails, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedOffers_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrders", poRegisteredDetails.IdPO);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", poRegisteredDetails.IdCurrency);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LinkedOffers link = new LinkedOffers();
                            try
                            {
                                if (rdr["Code"] != DBNull.Value)
                                    link.Code = Convert.ToString(rdr["Code"]);
                                if (rdr["CutomerName"] != DBNull.Value)
                                    link.CutomerName = Convert.ToString(rdr["CutomerName"]);
                                if (rdr["Name"] != DBNull.Value)
                                    link.Name = Convert.ToString(rdr["Name"]);
                                if (rdr["OfferStatus"] != DBNull.Value)
                                    link.Status = Convert.ToString(rdr["OfferStatus"]);
                                if (rdr["CustomerGroup"] != DBNull.Value)
                                    link.CustomerGroup = Convert.ToString(rdr["CustomerGroup"]);
                                if (rdr["HtmlColor"] != DBNull.Value)
                                    link.HtmlColor = Convert.ToString(rdr["HtmlColor"]);
                                if (rdr["Amount"] != DBNull.Value)
                                    link.Amount = Convert.ToDouble(rdr["Amount"]);
                                if (rdr["Conformation"] != DBNull.Value)
                                    link.Confirmation = Convert.ToString(rdr["Conformation"]);
                                if (rdr["OfferCurrency"] != DBNull.Value)
                                    link.OfferCurrency = Convert.ToString(rdr["OfferCurrency"]);
                                if (rdr["category"] != DBNull.Value)
                                    link.Category = Convert.ToString(rdr["category"]);
                                if (rdr["IdProductCategory"] != DBNull.Value)
                                    link.IdProductCategory = Convert.ToInt32(rdr["IdProductCategory"]);
                                if (rdr["IdOffer"] != DBNull.Value)
                                    link.IdOffer = Convert.ToInt64(rdr["IdOffer"]);
                                if (rdr["Year"] != DBNull.Value)
                                    link.Year = Convert.ToString(rdr["Year"]);
                                if (rdr["AttachmentFileName"] != DBNull.Value)
                                {
                                    link.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    link.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, link);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            LinkedOffersList.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        public int? GetOfferDetails_V2620(string ConnectionString, string OfferCode)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetIdOffer_V2620", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_OfferCode", OfferCode);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["IdOffer"] != DBNull.Value)
                                return Convert.ToInt32(reader["IdOffer"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in GetOfferDetails_V2620(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return null;
        }
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        public void OTM_InsertPORequestLinkedOffer_V2620(Int64 IdPORequest, int? IdOffer, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertPORequestLinkedOffer_V2620", conn);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOffer", IdOffer);
                    mySqlCommand.Parameters.AddWithValue("_IdPORequest", IdPORequest);
                    mySqlCommand.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in OTM_InsertPORequestLinkedOffer_V2620(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        // [nsatpute][10-03-2025][GEOS2-6722]
        public List<PORequestDetails> GetPORequestDetails_V2620(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2620";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["SenderEmployeeCode"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderEmployeeCode = rdr["SenderEmployeeCode"].ToString();
                                    if (rdr["SenderJobDescriptionTitle"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderJobDescriptionTitle = rdr["SenderJobDescriptionTitle"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["ToRecipientNameEmployeeCodes"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientNameEmployeeCodes = rdr["ToRecipientNameEmployeeCodes"].ToString();
                                    //if (rdr["ToRecipientNameJobDescriptionTitles"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                    //    po.ToRecipientNameJobDescriptionTitles = rdr["ToRecipientNameJobDescriptionTitles"].ToString();
                                    //[pramod.misal][17.02.2025][GEOS2-6719]
                                    #region [pramod.misal][17.02.2025][GEOS2-6719]
                                    //po.Group = Convert.ToString(rdr["Group"]);
                                    //po.Plant = Convert.ToString(rdr["Plant"]);
                                    if (rdr["EmailBody"] != DBNull.Value)
                                        po.Emailbody = rdr["EmailBody"].ToString();
                                    #endregion
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["CCNameEmployeeCodes"] != DBNull.Value)
                                        po.CCNameEmployeeCodes = rdr["CCNameEmployeeCodes"].ToString();
                                    //if (rdr["CCNameJobDescriptionTitles"] != DBNull.Value)
                                    //    po.CCNameJobDescriptionTitles = rdr["CCNameJobDescriptionTitles"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();

                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();


                                    //[nsatpute][06-03-2025][GEOS2-6722]

                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);

                                    poList.Add(po);


                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2620(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }


                            }

                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    //if (poList.Contains(rdr["IdPORequest"]))
                                    //{
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);
                                        p.POLinkedOffers = o;
                                    }
                                    // }
                                }
                            }
                            //foreach (string item in currencyISOs)
                            //{
                            //    byte[] bytes = null;
                            //    bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            //    poList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            //}
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        //[Rahul.Gadhave][GEOS2-7056][Date:07-03-2025]
        public List<ShippingAddress> OTM_GetShippingAddress_V2620(string connectionString, int IdSite)
        {
            List<ShippingAddress> ShippingAddressList = new List<ShippingAddress>();
            ShippingAddress defaultPOType = new ShippingAddress();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetShippingAddress_V2620", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            ShippingAddress shippingAddress = new ShippingAddress();
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                shippingAddress.IdShippingAddress = mySqlDataReader.GetInt64("IdShippingAddress");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                shippingAddress.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = mySqlDataReader.GetString("iso");
                            }
                            if (mySqlDataReader["FullAddress"] != DBNull.Value)
                            {
                                shippingAddress.FullAddress = mySqlDataReader.GetString("FullAddress");
                            }
                            if (mySqlDataReader["Address"] != DBNull.Value)
                            {
                                shippingAddress.Address = mySqlDataReader.GetString("Address");
                            }
                            if (mySqlDataReader["ZipCode"] != DBNull.Value)
                            {
                                shippingAddress.ZipCode = mySqlDataReader.GetString("ZipCode");
                            }
                            if (mySqlDataReader["City"] != DBNull.Value)
                            {
                                shippingAddress.City = mySqlDataReader.GetString("City");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = Convert.ToString(mySqlDataReader["iso"]);
                                shippingAddress.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + shippingAddress.IsoCode + ".png";
                            }
                            if (mySqlDataReader["CountriesName"] != DBNull.Value)
                            {
                                shippingAddress.CountriesName = mySqlDataReader.GetString("CountriesName");
                            }
                            //[pooja.jadhav][GEOS2-7057][12-03-2025]
                            if (mySqlDataReader["Region"] != DBNull.Value)
                            {
                                shippingAddress.Region = mySqlDataReader.GetString("Region");
                            }
                            ShippingAddressList.Add(shippingAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddress_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ShippingAddressList;
        }

        //[pooja.jadhav][GEOS2-7054][10-03-2025]
        public List<PORegisteredDetails> OTM_GetPOSender_V2620(string connectionString)
        {
            List<PORegisteredDetails> POSenderList = new List<PORegisteredDetails>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOSender_V2620", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            PORegisteredDetails Sender = new PORegisteredDetails();
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                Sender.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["FullName"] != DBNull.Value)
                            {
                                Sender.FullName = Convert.ToString(mySqlDataReader["FullName"]);
                            }
                            if (mySqlDataReader["Surname"] != DBNull.Value)
                            {
                                Sender.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                            }
                            if (mySqlDataReader["IdPersonGender"] != DBNull.Value)
                            {
                                Sender.IdGender = Convert.ToInt16(mySqlDataReader["IdPersonGender"]);
                            }
                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                Sender.IdPerson = Convert.ToInt16(mySqlDataReader["IdPerson"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                Sender.IdCustomer = Convert.ToInt16(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["SiteName"] != DBNull.Value)
                            {
                                Sender.SiteName = Convert.ToString(mySqlDataReader["SiteName"]);
                            }
                            POSenderList.Add(Sender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOSender_V2620. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POSenderList;
        }

        /// [001]  //[Rahul.Gadhave][GEOS2-7226][Date:24-03-2025] https://helpdesk.emdep.com/browse/GEOS2-7226
        public List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2630(string connectionString, int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = new List<ShippingAddress>();
            ShippingAddress defaultPOType = new ShippingAddress();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString)) 
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetShippingAddressForShowAll_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomerPlant);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            ShippingAddress shippingAddress = new ShippingAddress();
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                shippingAddress.IdShippingAddress = mySqlDataReader.GetInt64("IdShippingAddress");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                shippingAddress.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = mySqlDataReader.GetString("iso");
                            }
                            if (mySqlDataReader["FullAddress"] != DBNull.Value)
                            {
                                shippingAddress.FullAddress = mySqlDataReader.GetString("FullAddress");
                            }
                            if (mySqlDataReader["Address"] != DBNull.Value)
                            {
                                shippingAddress.Address = mySqlDataReader.GetString("Address");
                            }
                            if (mySqlDataReader["ZipCode"] != DBNull.Value)
                            {
                                shippingAddress.ZipCode = mySqlDataReader.GetString("ZipCode");
                            }
                            if (mySqlDataReader["City"] != DBNull.Value)
                            {
                                shippingAddress.City = mySqlDataReader.GetString("City");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = Convert.ToString(mySqlDataReader["iso"]);
                                shippingAddress.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + shippingAddress.IsoCode + ".png";
                            }
                            if (mySqlDataReader["CountriesName"] != DBNull.Value)
                            {
                                shippingAddress.CountriesName = mySqlDataReader.GetString("CountriesName");
                            }
                            if (mySqlDataReader["Region"] != DBNull.Value)
                            {
                                shippingAddress.Region = mySqlDataReader.GetString("Region");
                            }
                            ShippingAddressList.Add(shippingAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddressForShowAll_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ShippingAddressList;
        }
        // [001][ashish.malkhede][25/03/2025][GEOS2-7049] https://helpdesk.emdep.com/browse/GEOS2-7049
        public List<PORegisteredDetails> GetPORegisteredDetails_V2630(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, string correnciesIconFilePath, string countryIconFilePath, PORegisteredDetailFilter filter)
        {
            List<PORegisteredDetails> poRegiList = new List<PORegisteredDetails>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                List<string> currencyISOs = new List<string>();
                List<string> countryISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        if (filter == null)
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetails_V2630";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                        }
                        else
                        {
                            mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsFilter_V2630";
                            mySqlCommand.Connection = mySqlconn;
                            mySqlCommand.CommandTimeout = 6000;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                            mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                            mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_Number", filter.Number);
                            mySqlCommand.Parameters.AddWithValue("_Type", filter.IdType);
                            mySqlCommand.Parameters.AddWithValue("_Group", filter.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_PlantC", filter.IdCustomerPlant);
                            mySqlCommand.Parameters.AddWithValue("_Sender", filter.Sender);
                            mySqlCommand.Parameters.AddWithValue("_Currency", filter.IdCurrency);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeFrom", filter.PoValueRangeFrom);
                            mySqlCommand.Parameters.AddWithValue("_ValueRangeTo", filter.PoValueRangeTo);
                            mySqlCommand.Parameters.AddWithValue("_Offer", filter.Offer);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateFrom", filter.ReceptionDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_ReceptionDateTo", filter.ReceptionDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateFrom", filter.CreationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CreationDateTo", filter.CreationDateTo);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateFrom", filter.UpdateDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_UpdateDateTO", filter.UpdateDateTo);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateFrom", filter.CancellationDateFrom);
                            mySqlCommand.Parameters.AddWithValue("_CancellationDateTo", filter.CancellationDateTo);
                        }
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORegisteredDetails po = new PORegisteredDetails();
                                try
                                {
                                    if (rdr["IdPO"] != DBNull.Value)
                                        po.IdPO = Convert.ToInt64(rdr["IdPO"]);
                                    if (rdr["Code"] != DBNull.Value)
                                        po.Code = rdr["Code"].ToString();
                                    if (rdr["IdPOType"] != DBNull.Value)
                                        po.IdPOType = Convert.ToInt32(rdr["IdPOType"]);
                                    if (rdr["Type"] != DBNull.Value)
                                        po.Type = rdr["Type"].ToString();
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["Plant"] != DBNull.Value)
                                        po.Plant = rdr["Plant"].ToString();
                                    if (rdr["Country"] != DBNull.Value)
                                    {
                                        po.Country = rdr["Country"].ToString();
                                        po.CountryISO = rdr["CountryISO"].ToString();
                                    }
                                    if (rdr["Region"] != DBNull.Value)
                                        po.Region = rdr["Region"].ToString();
                                    if (rdr["ReceptionDate"] != DBNull.Value)
                                        po.ReceptionDate = Convert.ToDateTime(rdr["ReceptionDate"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["POValue"] != DBNull.Value)
                                        po.POValue = Convert.ToDouble(rdr["POValue"].ToString());
                                    if (rdr["Amount"] != DBNull.Value)
                                        po.Amount = Convert.ToDouble(rdr["Amount"].ToString());
                                    if (rdr["Remarks"] != DBNull.Value)
                                        po.Remarks = rdr["Remarks"].ToString();
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["LinkedOffer"] != DBNull.Value)
                                        po.LinkedOffer = rdr["LinkedOffer"].ToString();
                                    if (rdr["ShippingAddress"] != DBNull.Value)
                                        po.ShippingAddress = rdr["ShippingAddress"].ToString();
                                    if (rdr["IsOK"] != DBNull.Value)
                                        po.IsOK = rdr["IsOK"].ToString();
                                    if (rdr["Confirmation"] != DBNull.Value)
                                        po.Confirmation = rdr["Confirmation"].ToString();
                                    if (rdr["CreationDate"] != DBNull.Value)
                                        po.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);
                                    if (rdr["Creator"] != DBNull.Value)
                                        po.Creator = rdr["Creator"].ToString();
                                    if (rdr["UpdaterDate"] != DBNull.Value)
                                        po.UpdaterDate = Convert.ToDateTime(rdr["UpdaterDate"].ToString());
                                    if (rdr["Updater"] != DBNull.Value)
                                        po.Updater = rdr["Updater"].ToString();
                                    if (rdr["IsCancelled"] != DBNull.Value)
                                        po.IsCancelled = rdr["IsCancelled"].ToString(); // PO Not Cancelled
                                    if (rdr["IsCancelled"] != DBNull.Value && rdr["IsCancelled"].ToString() != "PO Not Cancelled")
                                    {
                                        if (rdr["Canceler"] != DBNull.Value)
                                            po.Canceler = rdr["Canceler"].ToString();
                                        if (rdr["CancellationDate"] != DBNull.Value)
                                            po.CancellationDate = Convert.ToDateTime(rdr["CancellationDate"].ToString());
                                    }
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (!countryISOs.Any(co => co.ToString() == po.CountryISO))
                                    {
                                        countryISOs.Add(po.CountryISO);
                                    }
                                    /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["IdSite"] != DBNull.Value)
                                        po.IdSite = Convert.ToInt32(rdr["IdSite"]);
                                    if (rdr["IdShippingAddress"] != DBNull.Value)
                                        po.IdShippingAddress = Convert.ToInt64(rdr["IdShippingAddress"]);
                                    if (rdr["IdCurrency"] != DBNull.Value)
                                        po.IdCurrency = Convert.ToInt32(rdr["IdCurrency"]);
                                    if (rdr["CreatorCode"] != DBNull.Value)
                                        po.CreatorCode = rdr["CreatorCode"].ToString();
                                    if (rdr["UpdaterCode"] != DBNull.Value)
                                        po.UpdaterCode = rdr["UpdaterCode"].ToString();
                                    if (rdr["CancelerCode"] != DBNull.Value)
                                        po.CancelerCode = rdr["CancelerCode"].ToString();
                                    if (rdr["AttachmentFileName"] != DBNull.Value)
                                        po.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    if (rdr["IdSender"] != DBNull.Value)
                                        po.IdSender = Convert.ToInt32(rdr["IdSender"]);

                                    //[Rahul.Gadhave][GEOS2-7850][Date:09-04-2025]
                                    if (rdr["Address"] != DBNull.Value)
                                    {
                                        po.Address = rdr.GetString("Address");
                                    }
                                    if (rdr["ZipCode"] != DBNull.Value)
                                    {
                                        po.ZipCode = rdr.GetString("ZipCode");
                                    }
                                    if (rdr["City"] != DBNull.Value)
                                    {
                                        po.City = rdr.GetString("City");
                                    }
                                    if (rdr["iso"] != DBNull.Value)
                                    {
                                        po.IsoCode = Convert.ToString(rdr["iso"]);
                                        po.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + po.IsoCode + ".png";
                                    }
                                    if (rdr["CountriesName"] != DBNull.Value)
                                    {
                                        po.CountriesName = rdr.GetString("CountriesName");
                                    }
                                    //if (rdr["Region"] != DBNull.Value)
                                    //{
                                    //    shippingAddress.Region = rdr.GetString("Region");
                                    //}
                                    //
                                    poRegiList.Add(po);
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            foreach (string item in currencyISOs)
                            {
                                byte[] bytes = null;
                                bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                                poRegiList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                            }
                            // for country
                            //foreach (string item in countryISOs)
                            //{
                            //    byte[] bytes = null;
                            //    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                            //    poRegiList.Where(ot => ot.CountryISO == item).ToList().ForEach(ot => ot.CountryIconBytes = bytes);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                return poRegiList;
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public bool POEmailSend_V2630(string EmailSubject, string htmlEmailtemplate, List<People> toContactList, List<People> CcContactList, string fromMail, string workbenchConnectionString, string MailServerName, string MailServerPort, List<LinkedResource> imageList)
        {
            bool isSend = false;
            try
            {
                List<string> toAddresses = toContactList.Select(person => person.Email).ToList();
                List<string> ccAddresses = CcContactList.Select(person => person.Email).ToList();
                #region  Old Take Images From URL
                List<System.Net.Mail.LinkedResource> ImgResourceList = new List<System.Net.Mail.LinkedResource>();
                List<string> Images = new List<string>();
                List<string> SetImages = new List<string>();
                Regex regexImg = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                MatchCollection matchesImg = regexImg.Matches(htmlEmailtemplate.ToString());
                //Images = new List<string>();
                foreach (Match matchImg in matchesImg)
                {
                    Images.Add(matchImg.Groups[1].Value);
                }
                Log4NetLogger.Logger.Log(string.Format("OTM SendMail Method Start Imgae download - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                int i = 0;
                foreach (var item in Images)
                {
                    try
                    {
                        if (i == 0)
                        {
                            htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_1");
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    byte[] imageBytes1 = webClient.DownloadData("https://api.emdep.com/images/logo-geos.png");
                            //    MemoryStream imgGeosLogo = new MemoryStream(imageBytes1);
                            //    System.Net.Mail.LinkedResource LRGeosLogo = new System.Net.Mail.LinkedResource(imgGeosLogo);
                            //    LRGeosLogo.ContentId = "EmbeddedContent_1";
                            //    LRGeosLogo.ContentLink = new Uri("cid:" + LRGeosLogo.ContentId);
                            //    ImgResourceList.Add(LRGeosLogo);
                            //}
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://api.emdep.com/images/logo-geos.png");
                            MemoryStream imgGeosLogo = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LRGeosLogo = new System.Net.Mail.LinkedResource(imgGeosLogo);
                            LRGeosLogo.ContentId = "EmbeddedContent_1";
                            LRGeosLogo.ContentLink = new Uri("cid:" + LRGeosLogo.ContentId);
                            ImgResourceList.Add(LRGeosLogo);
                        }

                    }
                    catch (Exception ex)
                    {

                        Log4NetLogger.Logger.Log(string.Format("Error POEmailSend_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    try
                    {
                        if (i == 1)
                        {
                            htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_2");
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            //    byte[] imageBytes1 = webClient.DownloadData("https://ecos.emdep.com/images/logo-emdep.png");
                            //    MemoryStream imgEmdepLogo = new MemoryStream(imageBytes1);
                            //    System.Net.Mail.LinkedResource LREmdepLogo = new System.Net.Mail.LinkedResource(imgEmdepLogo);
                            //    LREmdepLogo.ContentId = "EmbeddedContent_2";
                            //    LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                            //    ImgResourceList.Add(LREmdepLogo);
                            //}
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://ecos.emdep.com/images/logo-emdep.png");
                            MemoryStream imgEmdepLogo = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LREmdepLogo = new System.Net.Mail.LinkedResource(imgEmdepLogo);
                            LREmdepLogo.ContentId = "EmbeddedContent_2";
                            LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                            ImgResourceList.Add(LREmdepLogo);
                        }

                    }
                    catch (Exception ex)
                    {

                        Log4NetLogger.Logger.Log(string.Format("Error POEmailSend_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    }

                    try
                    {
                        if (i == 2)
                        {
                            htmlEmailtemplate = htmlEmailtemplate.Replace(item, "cid:EmbeddedContent_3");
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    byte[] imageBytes1 = webClient.DownloadData("https://ecos.emdep.com/images/social/linkedin_sm.png");
                            //    MemoryStream imglinkedin = new MemoryStream(imageBytes1);
                            //    System.Net.Mail.LinkedResource LRlinkedin = new System.Net.Mail.LinkedResource(imglinkedin);
                            //    LRlinkedin.ContentId = "EmbeddedContent_3";
                            //    LRlinkedin.ContentLink = new Uri("cid:" + LRlinkedin.ContentId);
                            //    ImgResourceList.Add(LRlinkedin);
                            //}
                            byte[] imageBytes1 = Utility.ImageUtil.GetImageByWebClient("https://ecos.emdep.com/images/social/linkedin_sm.png");
                            MemoryStream imglinkedin = new MemoryStream(imageBytes1);
                            System.Net.Mail.LinkedResource LRlinkedin = new System.Net.Mail.LinkedResource(imglinkedin);
                            LRlinkedin.ContentId = "EmbeddedContent_3";
                            LRlinkedin.ContentLink = new Uri("cid:" + LRlinkedin.ContentId);
                            ImgResourceList.Add(LRlinkedin);
                        }
                    }
                    catch (Exception ex)
                    {

                        Log4NetLogger.Logger.Log(string.Format("Error POEmailSend_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    }

                    i++;
                }
                #endregion;
                Log4NetLogger.Logger.Log(string.Format("OTM SendMail Method End Imgae download - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("OTM Call Send Mail Method - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                Utility.MailControl.SendHtmlMail(EmailSubject, htmlEmailtemplate, string.Join(";", toAddresses), ccAddresses, "noreply@emdep.com", MailServerName, MailServerPort, ImgResourceList);
                Log4NetLogger.Logger.Log(string.Format("OTM End Send Mail Method - {0}", DateTime.Now.ToString()), category: Category.Info, priority: Priority.Low);
                isSend = true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error POEmailSend_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                throw;
            }
            return isSend;
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public List<People> GetPeopleByJobDescriptions_V2630(string connectionstring, GeosAppSetting CustomerPOConfirmationJD, long plantIds)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPeopleByJobDescriptions_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdJobDescription", CustomerPOConfirmationJD.DefaultValue);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", plantIds);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            People people = new People();
                            people.IdPerson = Convert.ToInt32(reader["IdPerson"]);
                            people.Name = Convert.ToString(reader["Name"]);
                            people.Surname = Convert.ToString(reader["Surname"]);
                            people.Email = Convert.ToString(reader["Email"]);
                            peoples.Add(people);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPeopleByJobDescriptions_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public List<People> GetPeopleByEMDEPcustomer_V2630(string connectionstring, PORegisteredDetails poregistereddetailsforemail)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPeopleByEMDEPcustomer_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", poregistereddetailsforemail.IdCustomerPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", poregistereddetailsforemail.IdSite);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            People people = new People();
                            people.IdPerson = Convert.ToInt32(reader["IdPerson"]);
                            people.Name = Convert.ToString(reader["Name"]);
                            people.Surname = Convert.ToString(reader["Surname"]);
                            people.Email = Convert.ToString(reader["Email"]);
                            peoples.Add(people);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPeopleByEMDEPcustomer_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }
        public List<Email> GetAllEmailsBlankColumns(string ConnectionStringgeos)
        {
            List<Email> EmailList = new List<Email>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllEmailListBlankColumns_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["CCEmail"] != DBNull.Value)
                                    email.CCEmail = rdr["CCEmail"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["SenderIdPerson"] != DBNull.Value)
                                    email.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                if (rdr["ToIdPerson"] != DBNull.Value)
                                    email.ToIdPerson = rdr["ToIdPerson"].ToString();
                                if (rdr["CCIdPerson"] != DBNull.Value)
                                    email.CCIdPerson = rdr["CCIdPerson"].ToString();

                                if (rdr["IdCustomer"] != DBNull.Value)
                                    email.IdCustomer = Convert.ToInt32(rdr["IdCustomer"].ToString());

                                if (rdr["IdPlant"] != DBNull.Value)
                                    email.IdPlant = Convert.ToInt32(rdr["IdPlant"].ToString());
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllEmailsBlankColumns(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        public bool UpdatePORequestGroupPlant_V2630(Email poRequest, string mainServerConnectionString)
        {
            try
            {
                if (poRequest != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("UpdatePORequestGroupPlant_V2630", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdEmail", poRequest.IdEmail);
                        mySqlCommand.Parameters.AddWithValue("_IdCustomer", poRequest.IdCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IdPlant", poRequest.IdPlant);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePORequestGroupPlant_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        public void UpdateSenderIdPerson_V2630(Email email, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateSenderIdPerson_V2630", conn);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_SenderIdPerson", email.SenderIdPerson);
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", email.IdEmail);
                    mySqlCommand.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in UpdateSenderIdPerson_V2630(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void UpdateCCIdPerson_V2630(Email email, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateCCIdPerson_V2630", conn);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_CCIdPerson", email.CCIdPerson);
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", email.IdEmail);
                    mySqlCommand.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in UpdateCCIdPerson_V2630(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void UpdateToIdPerson_V2630(Email email, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateToIdPerson_V2630", conn);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_ToIdPerson", email.ToIdPerson);
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", email.IdEmail);
                    mySqlCommand.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in UpdateToIdPerson_V2630(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        /// <summary>
        /// //[pramod.misal][04.02.2025][GEOS2 - 6726]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<POEmployeeInfo> GetPOEmployeeInfoList_V2630(string ConnectionString)
        {
            List<POEmployeeInfo> Employees = new List<POEmployeeInfo>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOEmployeeInfoList_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            POEmployeeInfo employee = new POEmployeeInfo();
                            if (reader["IdEmployee"] != DBNull.Value)
                                employee.IdEmployee = Convert.ToInt64(reader["IdEmployee"].ToString());
                            if (reader["FullName"] != DBNull.Value)
                                employee.FullName = Convert.ToString(reader["FullName"].ToString());
                            if (reader["EmployeeCode"] != DBNull.Value)
                                employee.EmployeeCode = Convert.ToString(reader["EmployeeCode"].ToString());
                            if (reader["IdJobDescription"] != DBNull.Value)
                                employee.IdJobDescription = Convert.ToInt64(reader["IdJobDescription"].ToString());
                            if (reader["JobDescriptionTitle"] != DBNull.Value)
                                employee.JobDescriptionTitle = Convert.ToString(reader["JobDescriptionTitle"].ToString());
                            if (reader["Email"] != DBNull.Value)
                                employee.Email = Convert.ToString(reader["Email"].ToString());
                            Employees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPOEmployeeInfoList_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Employees;
        }

        public List<Email> GetAllEmailsBlankColumns_V2630(string ConnectionStringgeos)
        {
            List<Email> EmailList = new List<Email>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllEmailListBlankColumns_V2630_V1", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["CCEmail"] != DBNull.Value)
                                    email.CCEmail = rdr["CCEmail"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["SenderIdPerson"] != DBNull.Value)
                                    email.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                if (rdr["ToIdPerson"] != DBNull.Value)
                                    email.ToIdPerson = rdr["ToIdPerson"].ToString();
                                if (rdr["CCIdPerson"] != DBNull.Value)
                                    email.CCIdPerson = rdr["CCIdPerson"].ToString();

                                if (rdr["IdCustomer"] != DBNull.Value)
                                    email.IdCustomer = Convert.ToInt32(rdr["IdCustomer"].ToString());

                                if (rdr["IdPlant"] != DBNull.Value)
                                    email.IdPlant = Convert.ToInt32(rdr["IdPlant"].ToString());
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllEmailsBlankColumns(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        //[ashish.malkhede][GEOS2-7724][02 - 04 - 2025]
        public List<PORequestDetails> GetPORequestDetails_V2630(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2630";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();


                                    //[nsatpute][06-03-2025][GEOS2-6722]

                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);

                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();

                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();

                                    poList.Add(po);


                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2630(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }


                            }

                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);
                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        //[ashish.malkhede][GEOS-7049][03-04-2025][https://helpdesk.emdep.com/browse/GEOS2-7049]
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2630(string ConnectionStringGeos, PORegisteredDetails poRegisteredDetails, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedOffers_V2630", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrders", poRegisteredDetails.IdPO);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", poRegisteredDetails.IdCurrency);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LinkedOffers link = new LinkedOffers();
                            try
                            {
                                if (rdr["Code"] != DBNull.Value)
                                    link.Code = Convert.ToString(rdr["Code"]);
                                if (rdr["CutomerName"] != DBNull.Value)
                                    link.CutomerName = Convert.ToString(rdr["CutomerName"]);
                                if (rdr["Name"] != DBNull.Value)
                                    link.Name = Convert.ToString(rdr["Name"]);
                                if (rdr["OfferStatus"] != DBNull.Value)
                                    link.Status = Convert.ToString(rdr["OfferStatus"]);
                                if (rdr["CustomerGroup"] != DBNull.Value)
                                    link.CustomerGroup = Convert.ToString(rdr["CustomerGroup"]);
                                if (rdr["HtmlColor"] != DBNull.Value)
                                    link.HtmlColor = Convert.ToString(rdr["HtmlColor"]);
                                if (rdr["Amount"] != DBNull.Value)
                                    link.Amount = Convert.ToDouble(rdr["Amount"]);
                                if (rdr["Conformation"] != DBNull.Value)
                                    link.Confirmation = Convert.ToString(rdr["Conformation"]);
                                if (rdr["OfferCurrency"] != DBNull.Value)
                                    link.OfferCurrency = Convert.ToString(rdr["OfferCurrency"]);
                                if (rdr["category"] != DBNull.Value)
                                    link.Category = Convert.ToString(rdr["category"]);
                                if (rdr["IdProductCategory"] != DBNull.Value)
                                    link.IdProductCategory = Convert.ToInt32(rdr["IdProductCategory"]);
                                if (rdr["IdOffer"] != DBNull.Value)
                                    link.IdOffer = Convert.ToInt64(rdr["IdOffer"]);
                                if (rdr["Year"] != DBNull.Value)
                                    link.Year = Convert.ToString(rdr["Year"]);
                                if (rdr["AttachmentFileName"] != DBNull.Value)
                                {
                                    link.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    link.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, link);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            LinkedOffersList.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        /// <summary>
        /// [ashish.malkhede][GEOS-9194][08-12-2025]https://helpdesk.emdep.com/browse/GEOS2-9194
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="poRegisteredDetails"></param>
        /// <param name="CommericalPath"></param>
        /// <returns></returns>
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2660(string ConnectionStringGeos, PORegisteredDetails poRegisteredDetails, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedOffers_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPurchaseOrders", poRegisteredDetails.IdPO);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", poRegisteredDetails.IdCurrency);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LinkedOffers link = new LinkedOffers();
                            try
                            {
                                if (rdr["Code"] != DBNull.Value)
                                    link.Code = Convert.ToString(rdr["Code"]);
                                if (rdr["CutomerName"] != DBNull.Value)
                                    link.CutomerName = Convert.ToString(rdr["CutomerName"]);
                                if (rdr["Name"] != DBNull.Value)
                                    link.Name = Convert.ToString(rdr["Name"]);
                                if (rdr["OfferStatus"] != DBNull.Value)
                                    link.Status = Convert.ToString(rdr["OfferStatus"]);
                                if (rdr["CustomerGroup"] != DBNull.Value)
                                    link.CustomerGroup = Convert.ToString(rdr["CustomerGroup"]);
                                if (rdr["HtmlColor"] != DBNull.Value)
                                    link.HtmlColor = Convert.ToString(rdr["HtmlColor"]);
                                if (rdr["Amount"] != DBNull.Value)
                                    link.Amount = Convert.ToDouble(rdr["Amount"]);
                                if (rdr["Conformation"] != DBNull.Value)
                                    link.Confirmation = Convert.ToString(rdr["Conformation"]);
                                if (rdr["OfferCurrency"] != DBNull.Value)
                                    link.OfferCurrency = Convert.ToString(rdr["OfferCurrency"]);
                                if (rdr["category"] != DBNull.Value)
                                    link.Category = Convert.ToString(rdr["category"]);
                                if (rdr["IdProductCategory"] != DBNull.Value)
                                    link.IdProductCategory = Convert.ToInt32(rdr["IdProductCategory"]);
                                if (rdr["IdOffer"] != DBNull.Value)
                                    link.IdOffer = Convert.ToInt64(rdr["IdOffer"]);
                                if (rdr["Year"] != DBNull.Value)
                                    link.Year = Convert.ToString(rdr["Year"]);
                                if (rdr["AttachmentFileName"] != DBNull.Value)
                                {
                                    link.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    //link.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, link);
                                    link.CommericalAttachementsDocInBytes = CommericalAttachedDoc_V2670(CommericalPath, link);

                                }
                                if (rdr["IdStatus"] != DBNull.Value)
                                {
                                    link.IdStatus = Convert.ToInt32(rdr["IdStatus"]);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            LinkedOffersList.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLinkedOffers_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[pramod.misal][GEOS2-7724][07/04/2025]
        public bool AddEmails_V2630(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails_V2630", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_CCEmail", emailDetails.CCEmail);
                        mySqlCommand.Parameters.AddWithValue("_ToName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_ToEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CCName", emailDetails.CCName);
                        mySqlCommand.Parameters.AddWithValue("_SourceInboxId", emailDetails.SourceInboxId);
                        mySqlCommand.Parameters.AddWithValue("_CreatedIn", emailDetails.CreatedIn);
                        mySqlCommand.Parameters.AddWithValue("_EmailSentAt", emailDetails.EmailSentAt);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc_V2550(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails_V2610(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }

        //[pooja.jadhav][GEOS2-7052][11-04-2025]
        public List<Customer> GetAllCustomers_V2630(string connectionString)
        {
            List<Customer> CustomerList = new List<Customer>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCustomers_V2630", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Name"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Name"]);
                        cust.IsEnabled = true;
                        CustomerList.Add(cust);
                    }
                }
            }
            return CustomerList;
        }
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public ObservableCollection<LookupValue> GetLookupValues_V2640(string ConnectionString)
        {
            ObservableCollection<LookupValue> LookupValuesList = new ObservableCollection<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestStatus_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);

                                if (empReader["Value"] != DBNull.Value)
                                    lookupval.Value = empReader["Value"].ToString();

                                if (empReader["HtmlColor"] != DBNull.Value)
                                    lookupval.HtmlColor = empReader["HtmlColor"].ToString();

                            }
                            catch (Exception ex)
                            { }
                            LookupValuesList.Add(lookupval);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLookupValues(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LookupValuesList;
        }
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2640(string ConnectionString, string Offers, string correnciesIconFilePath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestLinkedOffers_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_OfferCode", Offers);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = mySqlDataReader["Code"].ToString();
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = mySqlDataReader["CustomerGroup"].ToString();
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);

                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                linkedOffers.Plant = mySqlDataReader["Plant"].ToString();
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = mySqlDataReader["Description"].ToString();
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = mySqlDataReader["Rfq"].ToString();
                            }
                            if (mySqlDataReader["OfferedByName"] != DBNull.Value)
                            {
                                linkedOffers.Contact = mySqlDataReader["OfferedByName"].ToString();
                            }
                            if (mySqlDataReader["IdContact"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferedBy = Convert.ToInt32(mySqlDataReader["IdContact"]);
                            }
                            if (mySqlDataReader["Status"] != DBNull.Value)
                            {
                                linkedOffers.Status = mySqlDataReader["Status"].ToString();
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = mySqlDataReader["HtmlColor"].ToString();
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = mySqlDataReader["LinkedPO"].ToString();
                            }
                            if (mySqlDataReader["CurrencyName"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = mySqlDataReader["CurrencyName"].ToString();
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["IdCarriageMethod"] != DBNull.Value)
                            {
                                linkedOffers.IdCarriageMethod = Convert.ToInt32(mySqlDataReader["IdCarriageMethod"]);
                            }
                            if (mySqlDataReader["CarriageMethod"] != DBNull.Value)
                            {
                                linkedOffers.CarriageMethod = mySqlDataReader["CarriageMethod"].ToString();
                            }
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                linkedOffers.IdShippingAddress = Convert.ToInt32(mySqlDataReader["IdShippingAddress"]);
                            }
                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                linkedOffers.IdPerson = Convert.ToString(mySqlDataReader["IdPerson"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["OfferTypeName"] != DBNull.Value)
                            {
                                linkedOffers.OfferTypeName = mySqlDataReader["OfferTypeName"].ToString();
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = mySqlDataReader["Year"].ToString();
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToUInt32(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["category"] != DBNull.Value)
                            {
                                linkedOffers.Category = Convert.ToString(mySqlDataReader["category"]);
                            }
                            //[Rahul.Gadhave][GEOS2-7253][Date:07/05/2025]
                            if (mySqlDataReader["PlantFullName"] != DBNull.Value)
                            {
                                linkedOffers.PlantFullName = Convert.ToString(mySqlDataReader["PlantFullName"]);
                            }
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                            {
                                linkedOffers.Confirmation = mySqlDataReader["Conformation"].ToString();
                            }
                            //IdPerson
                            if (linkedOffers.OfferCurrency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.OfferCurrency))
                                {
                                    currencyISOs.Add(linkedOffers.OfferCurrency);
                                }
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.OfferCurrency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPoRequestLinkedOffers_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2640(string connectionString, string OfferCode)
        {
            List<LinkedOffers> LinkedPoList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestLinkedPO_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_OfferCode", OfferCode);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkPO = new LinkedOffers();
                            if (mySqlDataReader["idCustomerPurchaseOrders"] != DBNull.Value)
                                linkPO.IdCustomerPurchaseOrder = Convert.ToInt32(mySqlDataReader["idCustomerPurchaseOrders"].ToString());
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                                linkPO.LinkedPO = mySqlDataReader["LinkedPO"].ToString();
                            if (mySqlDataReader["ReceivedIn"] != DBNull.Value)
                                linkPO.ReceivedIn = Convert.ToDateTime(mySqlDataReader["ReceivedIn"]);
                            if (mySqlDataReader["IdPOType"] != DBNull.Value)
                                linkPO.IdPOType = Convert.ToInt32(mySqlDataReader["IdPOType"]);
                            if (mySqlDataReader["POType"] != DBNull.Value)
                                linkPO.PoType = mySqlDataReader["POType"].ToString();
                            if (mySqlDataReader["OfferCode"] != DBNull.Value)
                                linkPO.Code = mySqlDataReader["OfferCode"].ToString();
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                                linkPO.Confirmation = mySqlDataReader["Conformation"].ToString();
                            LinkedPoList.Add(linkPO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPoRequestLinkedPO_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedPoList;
        }

        public List<LinkedOffers> GetPoRequestOfferTo_V2640(string connectionstring, LinkedOffers Obj)
        {
            List<LinkedOffers> peoples = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestOfferTo_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idCustomer", Obj.IdCustomer);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LinkedOffers people = new LinkedOffers();
                            people.Name = Convert.ToString(reader["FullName"]);
                            people.IdPerson = Convert.ToString(reader["IdPerson"]);
                            peoples.Add(people);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPoRequestOfferTo_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }

        public ObservableCollection<LookupValue> GetCarriageMethod_V2640(string ConnectionString)
        {
            ObservableCollection<LookupValue> LookupValuesList = new ObservableCollection<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestCarriageMethod_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);

                                if (empReader["Value"] != DBNull.Value)
                                    lookupval.Value = empReader["Value"].ToString();

                            }
                            catch (Exception ex)
                            { }
                            LookupValuesList.Add(lookupval);
                        }
                        LookupValuesList.Insert(0, new LookupValue
                        {
                            IdLookupValue = 0,
                            Value = "---"
                        });
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCarriageMethod_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LookupValuesList;
        }

        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public List<LinkedOffers> OTM_GetPoRequestOfferType_V2640(string ConnectionString)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestOfferType_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.OfferTypeName = mySqlDataReader["Name"].ToString();
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPoRequestOfferType_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }

        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant_V2630(string ConnectionString, Int32 SelectedIdCustomerPlant, Int64 SelectedIdPO, string correnciesIconFilePath, GeosAppSetting geosAppSetting, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            string AppSetting = "";
            if (geosAppSetting != null)
            {
                AppSetting = geosAppSetting.DefaultValue;
            }
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllLinkedOffers_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", SelectedIdCustomerPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdPO", SelectedIdPO);
                    mySqlCommand.Parameters.AddWithValue("_geosAppSetting", AppSetting);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {//PRSA
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = Convert.ToString(mySqlDataReader["Code"]);

                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);

                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            if (mySqlDataReader["CustomerName"] != DBNull.Value)
                            {
                                linkedOffers.CutomerName = Convert.ToString(mySqlDataReader["CustomerName"]);
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = Convert.ToString(mySqlDataReader["Description"]);
                            }
                            if (mySqlDataReader["Contact"] != DBNull.Value)
                            {
                                linkedOffers.Contact = Convert.ToString(mySqlDataReader["Contact"]);
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = Convert.ToString(mySqlDataReader["Rfq"]);
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["OfferCurrency"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = Convert.ToString(mySqlDataReader["OfferCurrency"]);
                            }
                            //if (mySqlDataReader["Name"] != DBNull.Value)
                            //{
                            //    linkedOffers.Name = mySqlDataReader.GetString("Name");
                            //}
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToInt64(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["IdPO"] != DBNull.Value)
                            {
                                linkedOffers.IdPO = Convert.ToInt32(mySqlDataReader["IdPO"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = Convert.ToString(mySqlDataReader["LinkedPO"]);
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["offersType"] != DBNull.Value)
                            {
                                linkedOffers.OffersType = Convert.ToString(mySqlDataReader["offersType"]);
                            }
                            if (mySqlDataReader["OfferStatus"] != DBNull.Value)
                            {
                                linkedOffers.Status = Convert.ToString(mySqlDataReader["OfferStatus"]);
                            }
                            if (mySqlDataReader["IdProductCategory"] != DBNull.Value)
                            {
                                linkedOffers.IdProductCategory = Convert.ToInt32(mySqlDataReader["IdProductCategory"]);
                            }
                            if (mySqlDataReader["category"] != DBNull.Value)
                            {
                                linkedOffers.Category = Convert.ToString(mySqlDataReader["category"]);
                            }
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                            {
                                linkedOffers.Confirmation = Convert.ToString(mySqlDataReader["Conformation"]);
                            }
                            if (mySqlDataReader["Currency"] != DBNull.Value)
                            {
                                linkedOffers.Currency = Convert.ToString(mySqlDataReader["Currency"]);
                            }
                            if (linkedOffers.Currency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.Currency))
                                {
                                    currencyISOs.Add(linkedOffers.Currency);
                                }
                            }
                            if (mySqlDataReader["AttachmentFileName"] != DBNull.Value)
                            {
                                linkedOffers.AttachmentFileName = Convert.ToString(mySqlDataReader["AttachmentFileName"]);
                                linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, linkedOffers);
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetLinkedofferByIdCustomerPlant(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }


        //[pramod.misal][GEOS2-7724][23 - 04 - 2025]
        public List<PORequestDetails> GetPORequestDetails_V2640(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string plantConnection, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(plantConnection))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2640";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();


                                    //[nsatpute][06-03-2025][GEOS2-6722]

                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);

                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();

                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();

                                    //[pramod.misal][GEOS2-7248][22.04.2025]
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    //if (rdr["IdAttachmentType"] != DBNull.Value)
                                    //    po.IdAttachementType = Convert.ToInt32(rdr["IdAttachmentType"]);
                                    //if (rdr["IdAttachment"] != DBNull.Value)
                                    //    po.IdAttachment = Convert.ToString(rdr["IdAttachment"]);



                                    poList.Add(po);


                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2630(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);
                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }


        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2640(string ConnectionString, string SelectedIdPlant, string SelectedIdGroup, string correnciesIconFilePath, GeosAppSetting geosAppSetting, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            string AppSetting = "";
            if (geosAppSetting != null)
            {
                AppSetting = geosAppSetting.DefaultValue;
            }
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedofferByIdPlantAndGroup_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPlant", SelectedIdPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdGroup", SelectedIdGroup);
                    mySqlCommand.Parameters.AddWithValue("_geosAppSetting", AppSetting);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = Convert.ToString(mySqlDataReader["Code"]);

                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = Convert.ToString(mySqlDataReader["Description"]);
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = Convert.ToString(mySqlDataReader["Rfq"]);
                            }
                            if (mySqlDataReader["Contact"] != DBNull.Value)
                            {
                                linkedOffers.Contact = Convert.ToString(mySqlDataReader["Contact"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            //if (mySqlDataReader["CustomerName"] != DBNull.Value)
                            //{
                            //    linkedOffers.CutomerName = Convert.ToString(mySqlDataReader["CustomerName"]);
                            //}
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["OfferStatus"] != DBNull.Value)
                            {
                                linkedOffers.Status = Convert.ToString(mySqlDataReader["OfferStatus"]);
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["OfferCurrency"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = Convert.ToString(mySqlDataReader["OfferCurrency"]);
                            }
                            if (mySqlDataReader["Currency"] != DBNull.Value)
                            {
                                linkedOffers.Currency = Convert.ToString(mySqlDataReader["Currency"]);
                            }
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToInt64(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                linkedOffers.Plant = Convert.ToString(mySqlDataReader["Plant"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdPlant = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["offersType"] != DBNull.Value)
                            {
                                linkedOffers.OffersType = Convert.ToString(mySqlDataReader["offersType"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = Convert.ToString(mySqlDataReader["LinkedPO"]);
                            }
                            if (linkedOffers.Currency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.Currency))
                                {
                                    currencyISOs.Add(linkedOffers.Currency);
                                }
                            }
                            if (mySqlDataReader["AttachmentFileName"] != DBNull.Value)
                            {
                                linkedOffers.AttachmentFileName = Convert.ToString(mySqlDataReader["AttachmentFileName"]);
                                linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, linkedOffers);
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetLinkedofferByIdPlantAndGroup_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }

        /// <summary>
        /// [001][pramod.misal] https://helpdesk.emdep.com/browse/GEOS2-7251
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool UpdateOffer(LinkedOffers offer, string connectionString, List<Emailattachment> POAttachementsList)
        {
            bool IsSave = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateOffer_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOffer", offer.IdOffer);
                    mySqlCommand.Parameters.AddWithValue("_RFQ", offer.RFQ);
                    mySqlCommand.Parameters.AddWithValue("_amount", offer.Amount);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", offer.IdCurrency);
                    mySqlCommand.Parameters.AddWithValue("_discount", offer.Discount);
                    mySqlCommand.Parameters.AddWithValue("_IdShipTO", offer.IdShippingAddress);
                    mySqlCommand.Parameters.AddWithValue("_IdcarriageMethod", offer.IdCarriageMethod);
                    mySqlCommand.ExecuteScalar();
                    IsSave = true;
                    UpdateOfferContact(offer, connectionString);
                    UpdatePoRequestStatus_V2640(offer, connectionString, POAttachementsList);
                    AddLinkedofferByIdPORequest(offer, connectionString);
                    if (offer.DeletedLinkedofferlist != null)
                    {
                        DeleteLinkedofferByIdPORequest(offer, connectionString);
                    }
                    AddChangeLogByOffer(offer.OfferChangeLog, connectionString);

                    //UpdatePoRequestStatus_V2640(offer, connectionString);

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOffer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IsSave;
        }
        /// <summary>
        /// [001][ashish.malkhede] https://helpdesk.emdep.com/browse/GEOS2-7251
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool UpdateOfferContact(LinkedOffers offer, string connectionString)
        {
            bool isUpdate = false;
            try
            {
                if (offer.OffersContact != null)
                {
                    foreach (LinkedOffers contacts in offer.OffersContact)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateOfferContact_V2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdOffer", offer.IdOffer);
                            mySqlCommand.Parameters.AddWithValue("_IdPerson", contacts.IdPerson);
                            mySqlCommand.Parameters.AddWithValue("_IsNew", contacts.IsNew);
                            mySqlCommand.Parameters.AddWithValue("_IsDelete", contacts.IsDelete);
                            mySqlCommand.ExecuteScalar();
                            isUpdate = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOffer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdate;
        }

        //[pramod.misal][28-04-2024][GEOS2-7247]
        public string OTM_GetEmailBodyByIdEmail_V2640(string connectionStringint, Int64 IdEmail)
        {
            string EmailBody = null;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionStringint))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailBodyByIdEmail_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", IdEmail);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["EmailBody"] != DBNull.Value)
                                EmailBody = Convert.ToString(mySqlDataReader["EmailBody"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error getCode(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailBody;
        }
        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        public bool UpdatePoRequestStatus_V2640(LinkedOffers offer, string connectionString, List<Emailattachment> POAttachementsList)
        {
            bool isUpdate = false;
            try
            {
                if (offer.SelectedIndexStatus != null)
                {
                    if (offer.SelectedIndexStatus.IdLookupValue != 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdatePoRequestStatus_V2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPORequest", offer.IdPORequest);
                            mySqlCommand.Parameters.AddWithValue("_PORequestStatus", offer.SelectedIndexStatus.IdLookupValue);
                            mySqlCommand.ExecuteScalar();
                            isUpdate = true;
                        }
                    }

                }

                if (POAttachementsList.Count > 0)
                {
                    foreach (Emailattachment attachemnet in POAttachementsList)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateIdAttachmentType_2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_POIdAttachment", attachemnet.IdAttachment);
                            mySqlCommand.Parameters.AddWithValue("_IdAttachmentType", attachemnet.IdAttachementType);
                            mySqlCommand.ExecuteScalar();
                            isUpdate = true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOffer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdate;
        }

        public void AddChangeLogByOffer(List<LogEntryByPOOffer> logEntry, string connectionString)
        {
            try
            {
                if (logEntry != null)
                {
                    foreach (LogEntryByPOOffer log in logEntry)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_logEntriesByPOOffer_V2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdOffer", log.IdOffer);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", log.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_IsDateTime", log.DateTime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", log.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                            mySqlCommand.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddChangeLogByOffer(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddLinkedofferByIdPORequest(LinkedOffers offer, string connectionString)
        {
            try
            {
                if (offer.Linkedofferlist != null)
                {
                    foreach (LinkedOffers log in offer.Linkedofferlist)
                    {
                        if (log.IsNewLinkedOffer == true)
                        {
                            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                            {
                                mySqlConnection.Open();
                                MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertLinkedofferByIdPORequest_V2640", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_Idoffer", log.IdOffer);
                                mySqlCommand.Parameters.AddWithValue("_IdPORequest", offer.IdPORequest);

                                mySqlCommand.ExecuteScalar();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddLinkedofferByIdPORequest(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void DeleteLinkedofferByIdPORequest(LinkedOffers offer, string connectionString)
        {
            try
            {

                foreach (LinkedOffers item in offer.DeletedLinkedofferlist)
                {

                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_DeleteLinkedofferByIdPORequest_V2640", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_Idoffer", item.IdOffer);
                        mySqlCommand.Parameters.AddWithValue("_IdPORequest", offer.IdPORequest);

                        mySqlCommand.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddLinkedofferByIdPORequest(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001][ashish.malkhede][07-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7254
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="idPORequest"></param>
        /// <param name="idOffer"></param>
        /// <returns></returns>
        public List<LogEntryByPORequest> GetAllPORequestChangeLog_V2640(string ConnectionStringGeos, long idPORequest, string idOffer)
        {
            List<LogEntryByPORequest> logList = new List<LogEntryByPORequest>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPORequestChangeLog_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idPORequest", idPORequest);
                    mySqlCommand.Parameters.AddWithValue("_idOffer", idOffer);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogEntryByPORequest poLog = new LogEntryByPORequest();
                            try
                            {
                                poLog.IdPORequest = Convert.ToInt64(reader["IdOffer"].ToString());
                                poLog.IdLogEntry = Convert.ToInt64(reader["idLogEntry"].ToString());
                                poLog.IdLogEntryType = Convert.ToByte(reader["idLogEntryType"].ToString());
                                poLog.IdUser = Convert.ToInt32(reader["idUser"].ToString());
                                poLog.People = new People { IdPerson = Convert.ToInt32(reader["idUser"].ToString()), Name = reader["Name"].ToString(), Surname = reader["Surname"].ToString() };
                                poLog.DateTime = Convert.ToDateTime(reader["DateTime"].ToString());
                                poLog.Comments = reader["comments"].ToString();
                                poLog.IsDeleted = false;
                                logList.Add(poLog);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetAllPORequestChangeLog_V2640. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllPOChangeLog_V2590. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return logList;
        }

        //[pramod.misal][07-05-2025][GEOS2-7248]
        public List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2640(string connectionString, Int64 IdEmail)
        {
            List<Emailattachment> EmailAttachementList = new List<Emailattachment>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailAttachementByIdEmail_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdEmail", IdEmail);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            Emailattachment emailattachment = new Emailattachment();
                            if (mySqlDataReader["IdAttachment"] != DBNull.Value)
                            {
                                emailattachment.IdAttachment = Convert.ToInt64(mySqlDataReader["IdAttachment"]);
                            }
                            if (mySqlDataReader["IdEmail"] != DBNull.Value)
                            {
                                emailattachment.IdEmail = Convert.ToInt64(mySqlDataReader["IdEmail"]);
                            }
                            if (mySqlDataReader["AttachmentName"] != DBNull.Value)
                            {
                                emailattachment.AttachmentName = Convert.ToString(mySqlDataReader["AttachmentName"]);
                            }
                            if (mySqlDataReader["AttachmentPath"] != DBNull.Value)
                            {
                                emailattachment.AttachmentPath = Convert.ToString(mySqlDataReader["AttachmentPath"]);
                            }
                            if (mySqlDataReader["AttachmentExtension"] != DBNull.Value)
                            {
                                emailattachment.AttachmentExtension = Convert.ToString(mySqlDataReader["AttachmentExtension"]);
                            }
                            if (mySqlDataReader["IdAttachmentType"] != DBNull.Value)
                            {
                                emailattachment.IdAttachementType = Convert.ToInt64(mySqlDataReader["IdAttachmentType"]);
                            }

                            EmailAttachementList.Add(emailattachment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddress_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailAttachementList;
        }
        //[Rahul.Gadhave][GEOS2-7253][Date:05/06/2025]
        public string GetCommercialOffersPath_V2640(string connectionString)
        {
            string path = string.Empty;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("CRM_GetCommercialOffersPath_V2620", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            path = reader["SettingValue"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return path;
        }

        /// <summary>
        /// [pooja.jadhav][GEOS2-7252][09-05-2025]
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="IdPo"></param>
        /// <param name="IdCurrency"></param>
        /// <returns></returns>
        public PORegisteredDetails GetPORegisteredDetailsByIdPo(string ConnectionStringGeos, int IdPo, int IdCurrency)
        {
            PORegisteredDetails po = new PORegisteredDetails();
            using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
            {
                List<string> currencyISOs = new List<string>();
                List<string> countryISOs = new List<string>();
                mySqlconn.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {

                        mySqlCommand.CommandText = "OTM_GetPORegisteredDetailsByIdPo";
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdPO", IdPo);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", IdCurrency);

                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                try
                                {
                                    if (rdr["IdPO"] != DBNull.Value)
                                        po.IdPO = Convert.ToInt64(rdr["IdPO"]);
                                    if (rdr["Code"] != DBNull.Value)
                                        po.Code = rdr["Code"].ToString();
                                    if (rdr["IdPOType"] != DBNull.Value)
                                        po.IdPOType = Convert.ToInt32(rdr["IdPOType"]);
                                    if (rdr["Type"] != DBNull.Value)
                                        po.Type = rdr["Type"].ToString();
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    if (rdr["Plant"] != DBNull.Value)
                                        po.Plant = rdr["Plant"].ToString();
                                    if (rdr["Country"] != DBNull.Value)
                                    {
                                        po.Country = rdr["Country"].ToString();
                                        po.CountryISO = rdr["CountryISO"].ToString();
                                    }
                                    if (rdr["Region"] != DBNull.Value)
                                        po.Region = rdr["Region"].ToString();
                                    if (rdr["ReceptionDate"] != DBNull.Value)
                                        po.ReceptionDate = Convert.ToDateTime(rdr["ReceptionDate"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["POValue"] != DBNull.Value)
                                        po.POValue = Convert.ToDouble(rdr["POValue"].ToString());
                                    if (rdr["Amount"] != DBNull.Value)
                                        po.Amount = Convert.ToDouble(rdr["Amount"].ToString());
                                    if (rdr["Remarks"] != DBNull.Value)
                                        po.Remarks = rdr["Remarks"].ToString();
                                    if (rdr["Currency"] != DBNull.Value)
                                        po.Currency = rdr["Currency"].ToString();
                                    if (rdr["LinkedOffer"] != DBNull.Value)
                                        po.LinkedOffer = rdr["LinkedOffer"].ToString();
                                    if (rdr["ShippingAddress"] != DBNull.Value)
                                        po.ShippingAddress = rdr["ShippingAddress"].ToString();
                                    if (rdr["IsOK"] != DBNull.Value)
                                        po.IsOK = rdr["IsOK"].ToString();
                                    if (rdr["Confirmation"] != DBNull.Value)
                                        po.Confirmation = rdr["Confirmation"].ToString();
                                    if (rdr["CreationDate"] != DBNull.Value)
                                        po.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);
                                    if (rdr["Creator"] != DBNull.Value)
                                        po.Creator = rdr["Creator"].ToString();
                                    if (rdr["UpdaterDate"] != DBNull.Value)
                                        po.UpdaterDate = Convert.ToDateTime(rdr["UpdaterDate"].ToString());
                                    if (rdr["Updater"] != DBNull.Value)
                                        po.Updater = rdr["Updater"].ToString();
                                    if (rdr["IsCancelled"] != DBNull.Value)
                                        po.IsCancelled = rdr["IsCancelled"].ToString(); // PO Not Cancelled
                                    if (rdr["IsCancelled"] != DBNull.Value && rdr["IsCancelled"].ToString() != "PO Not Cancelled")
                                    {
                                        if (rdr["Canceler"] != DBNull.Value)
                                            po.Canceler = rdr["Canceler"].ToString();
                                        if (rdr["CancellationDate"] != DBNull.Value)
                                            po.CancellationDate = Convert.ToDateTime(rdr["CancellationDate"].ToString());
                                    }
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (!countryISOs.Any(co => co.ToString() == po.CountryISO))
                                    {
                                        countryISOs.Add(po.CountryISO);
                                    }

                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["IdSite"] != DBNull.Value)
                                        po.IdSite = Convert.ToInt32(rdr["IdSite"]);
                                    if (rdr["IdShippingAddress"] != DBNull.Value)
                                        po.IdShippingAddress = Convert.ToInt64(rdr["IdShippingAddress"]);
                                    if (rdr["IdCurrency"] != DBNull.Value)
                                        po.IdCurrency = Convert.ToInt32(rdr["IdCurrency"]);
                                    if (rdr["CreatorCode"] != DBNull.Value)
                                        po.CreatorCode = rdr["CreatorCode"].ToString();
                                    if (rdr["UpdaterCode"] != DBNull.Value)
                                        po.UpdaterCode = rdr["UpdaterCode"].ToString();
                                    if (rdr["CancelerCode"] != DBNull.Value)
                                        po.CancelerCode = rdr["CancelerCode"].ToString();
                                    if (rdr["AttachmentFileName"] != DBNull.Value)
                                        po.AttachmentFileName = rdr["AttachmentFileName"].ToString();
                                    if (rdr["IdSender"] != DBNull.Value)
                                        po.IdSender = Convert.ToInt32(rdr["IdSender"]);


                                    if (rdr["Address"] != DBNull.Value)
                                    {
                                        po.Address = rdr.GetString("Address");
                                    }
                                    if (rdr["ZipCode"] != DBNull.Value)
                                    {
                                        po.ZipCode = rdr.GetString("ZipCode");
                                    }
                                    if (rdr["City"] != DBNull.Value)
                                    {
                                        po.City = rdr.GetString("City");
                                    }
                                    if (rdr["iso"] != DBNull.Value)
                                    {
                                        po.IsoCode = Convert.ToString(rdr["iso"]);
                                        po.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + po.IsoCode + ".png";
                                    }
                                    if (rdr["CountriesName"] != DBNull.Value)
                                    {
                                        po.CountriesName = rdr.GetString("CountriesName");
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORegisteredDetails_V2630(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                return po;
            }

        }

        /// <summary>
        /// [pooja.jadhav][GEOS2-7252][09-05-2025]
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsPoNumberExist(string connectionString, string code)
        {
            bool flag = false;
            int count = 0;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_IsPoNumberExist", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_Code", code);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["cnt"] != DBNull.Value)
                                count = Convert.ToInt32(mySqlDataReader["cnt"]);
                        }
                    }
                }

                if (count == 1)
                {
                    flag = true;
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsPoNumberExist(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return flag;
        }
        public PORequestDetails GetPODetailsbyAttachment(string connectionString, int IdAttachment)
        {
            PORequestDetails PODetails = new PORequestDetails();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPODetailsbyAttachment", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idAttachment", IdAttachment);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["PONumber"] != DBNull.Value)
                                PODetails.PONumber = mySqlDataReader["PONumber"].ToString();

                            if (mySqlDataReader["PODate"] != DBNull.Value)
                                PODetails.POdate = Convert.ToDateTime(mySqlDataReader["PODate"]);

                            if (mySqlDataReader["Sender"] != DBNull.Value)
                                PODetails.Sender = mySqlDataReader["Sender"].ToString();

                            if (mySqlDataReader["TransferAmount"] != DBNull.Value)
                                PODetails.TransferAmount = Convert.ToDouble(mySqlDataReader["TransferAmount"]);
                            else
                                PODetails.TransferAmount = 0;

                            if (mySqlDataReader["Currency"] != DBNull.Value)
                                PODetails.Currency = mySqlDataReader["Currency"].ToString();

                            if (mySqlDataReader["ShipAddress"] != DBNull.Value)
                                PODetails.ShipTo = mySqlDataReader["ShipAddress"].ToString();

                            if (mySqlDataReader["IdAttachment"] != DBNull.Value)
                                PODetails.IdAttachment = mySqlDataReader["IdAttachment"].ToString();

                            if (mySqlDataReader["AttachmentName"] != DBNull.Value)
                                PODetails.Attachments = mySqlDataReader["AttachmentName"].ToString();


                        }

                        if (mySqlDataReader.NextResult())
                        {
                            if (mySqlDataReader.HasRows)
                            {
                                PODetails.IsPOExist = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPODetailsbyAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PODetails;
        }

        /// <summary>
        /// [001][rahul.gadhave][13-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7252
        /// </summary>
        /// <param name="PO"></param>
        /// <param name="localServerConnectionString"></param>
        /// <returns></returns>
        public bool InsertPurchaseOrder_V2640(PORegisteredDetails PO, string mainServerConnectionString, string CommericalPath)
        {
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_InsertCustomerPurchaseOrders_V2640", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdPOType", PO.IdPOType);
                        MyCommand.Parameters.AddWithValue("_IdSite", PO.IdSite);
                        MyCommand.Parameters.AddWithValue("_Code", PO.Code);
                        MyCommand.Parameters.AddWithValue("_ReceivedIn", PO.ReceptionDate);
                        MyCommand.Parameters.AddWithValue("_IdShippingAddress", PO.IdShippingAddress);
                        MyCommand.Parameters.AddWithValue("_Value", PO.POValue);
                        MyCommand.Parameters.AddWithValue("_IdCurrency", PO.IdCurrency);
                        MyCommand.Parameters.AddWithValue("_AttachmentFileName", PO.AttachmentFileName);
                        MyCommand.Parameters.AddWithValue("_IdSender", PO.IdSender);
                        MyCommand.Parameters.AddWithValue("_Sender", PO.Sender);
                        MyCommand.Parameters.AddWithValue("_NOK", PO.IsOK);
                        MyCommand.Parameters.AddWithValue("_IsCancelledStatus", PO.IsCancelled);
                        MyCommand.Parameters.AddWithValue("_CreatedBy", PO.UpdatedBy);
                        MyCommand.Parameters.AddWithValue("_CreatedIN", DateTime.Now);
                        PO.IdPO = Convert.ToInt32(MyCommand.ExecuteScalar());
                        con.Close();
                    }
                    // Insert and Delete Linked Offers
                    InsertUpdateLinkedOffersByPO_V2640(PO, mainServerConnectionString, CommericalPath);
                    //// Inset Log
                    if (PO.LogEntriesByPO != null && PO.LogEntriesByPO.Count > 0)
                    {
                        // Insert Log entries Po logentriesbypo
                        //AddLogEntriesByPO(PO.LogEntriesByPO, PO.IdPO, mainServerConnectionString);
                        // Insert Log All linked offers by PO
                        if (PO.OffersLinked.Count > 0)
                        {
                            foreach (LinkedOffers lo in PO.OffersLinked)
                            {
                                AddLogEntriesLinkedOfferByPO(lo.IdOffer, PO.LogEntriesByPO, mainServerConnectionString);
                            }
                        }
                    }
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return true;
        }
        /// <summary>
        /// [001][ashish.malkhede] OTM - Offers have to update when a PO is Linked https://helpdesk.emdep.com/browse/GEOS2-9194
        /// </summary>
        /// <param name="offerslink"></param>
        /// <param name="mainServerConnectionString"></param>
        /// <param name="CommericalPath"></param>
        public void InsertUpdateLinkedOffersByPO_V2640(PORegisteredDetails offerslink, string mainServerConnectionString, string CommericalPath)
        {
            foreach (LinkedOffers lo in offerslink.offersLinked)
            {
                using (MySqlConnection connmainPO = new MySqlConnection(mainServerConnectionString))
                {
                    connmainPO.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_InsertCustomerPurchaseOrdersByOffer_V2590", connmainPO);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_idCustomerPurchaseOrder", offerslink.IdPO);
                    MyCommand.Parameters.AddWithValue("_idOffer", lo.IdOffer);
                    MyCommand.Parameters.AddWithValue("_Comments", offerslink.Remarks);
                    MyCommand.ExecuteNonQuery();
                    connmainPO.Close();
                    AddPOAttachedDoc(lo, CommericalPath, offerslink);
                    OTM_UpdateLinkedOfferStatus_V2660(lo, mainServerConnectionString); //[001]
                }
            }
        }
        //[Rahul.Gadhave][GEOS2-8339][Date:06-06-2025]
        public List<PORequestDetails> GetPORequestDetails_V2650(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2650";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();


                                    //[nsatpute][06-03-2025][GEOS2-6722]

                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);

                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();

                                    if (rdr["IdCustomerGroup"] != DBNull.Value)
                                        po.IdCustomerGroup = Convert.ToInt32(rdr["IdCustomerGroup"]);
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();

                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();

                                    //[pramod.misal][GEOS2-7248][22.04.2025]
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    //if (rdr["IdAttachmentType"] != DBNull.Value)
                                    //    po.IdAttachementType = Convert.ToInt32(rdr["IdAttachmentType"]);
                                    //if (rdr["IdAttachment"] != DBNull.Value)
                                    //    po.IdAttachment = Convert.ToString(rdr["IdAttachment"]);



                                    poList.Add(po);


                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2650(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);
                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2650(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }

        /// <summary>
        /// [001][pooja.jadhav][GEOS2-8342]
        /// </summary>
        /// <param name="logEntries"></param>
        /// <param name="connectionString"></param>
        public void AddChangeLogByPORequest(ObservableCollection<LogEntryByPORequest> logEntries, string connectionString)
        {
            try
            {
                if (logEntries != null)
                {
                    foreach (LogEntryByPORequest log in logEntries)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_logEntriesByPOOffer_V2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdOffer", log.IdPORequest);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", log.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_IsDateTime", log.DateTime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", log.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                            mySqlCommand.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddChangeLogByPORequest(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// //[pramod.misal][GEOS2-8772][Date:30-06-2025]https://helpdesk.emdep.com/browse/GEOS2-8772
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="Idcurrency"></param>
        /// <param name="plantId"></param>
        /// <param name="correnciesIconFilePath"></param>
        /// <returns></returns>

        public List<PORequestDetails> GetPORequestDetails_V2660(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2660";
                        //mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2650TEST_V2660";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();


                                    //[nsatpute][06-03-2025][GEOS2-6722]

                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);

                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                                    if (rdr["IdCustomerGroup"] != DBNull.Value)
                                        po.IdCustomerGroup = Convert.ToInt32(rdr["IdCustomerGroup"]);
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();

                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();

                                    //[pramod.misal][GEOS2-7248][22.04.2025]
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    //if (rdr["IdAttachmentType"] != DBNull.Value)
                                    //    po.IdAttachementType = Convert.ToInt32(rdr["IdAttachmentType"]);
                                    //if (rdr["IdAttachment"] != DBNull.Value)
                                    //    po.IdAttachment = Convert.ToString(rdr["IdAttachment"]);

                                    if (rdr["idcountry"] != DBNull.Value)
                                        po.IdCountry = Convert.ToInt64(rdr["idcountry"]);


                                    poList.Add(po);


                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2650(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);
                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2650(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }

        // [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
        public ObservableCollection<LookupValue> GetCarriageMethod_V2660(string ConnectionString)
        {
            ObservableCollection<LookupValue> LookupValuesList = new ObservableCollection<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestCarriageMethod_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                {
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);
                                }
                                if (empReader["Value"] != DBNull.Value)
                                {
                                    lookupval.Value = empReader["Value"].ToString();
                                }
                                //[Rahul.Gadhave][GEOS2-8307][24-06-2025]
                                if (empReader["ImageName"] != DBNull.Value)
                                {
                                    lookupval.ImageName = empReader["ImageName"].ToString();
                                    lookupval.ImageInBytes = Utility.ImageUtil.GetImageByWebClient("https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Enumerated/" + lookupval.ImageName);
                                }
                            }
                            catch (Exception ex)
                            { }
                            LookupValuesList.Add(lookupval);
                        }
                        LookupValuesList.Insert(0, new LookupValue
                        {
                            IdLookupValue = 0,
                            Value = "---"
                        });
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCarriageMethod_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LookupValuesList;
        }
        public List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2660(string ConnectionString, string Offers, string correnciesIconFilePath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestLinkedOffers_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_OfferCode", Offers);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = mySqlDataReader["Code"].ToString();
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = mySqlDataReader["CustomerGroup"].ToString();
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);

                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                linkedOffers.Plant = mySqlDataReader["Plant"].ToString();
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = mySqlDataReader["Description"].ToString();
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = mySqlDataReader["Rfq"].ToString();
                            }
                            if (mySqlDataReader["OfferedByName"] != DBNull.Value)
                            {
                                linkedOffers.Contact = mySqlDataReader["OfferedByName"].ToString();
                            }
                            if (mySqlDataReader["IdContact"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferedBy = Convert.ToInt32(mySqlDataReader["IdContact"]);
                            }
                            if (mySqlDataReader["Status"] != DBNull.Value)
                            {
                                linkedOffers.Status = mySqlDataReader["Status"].ToString();
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = mySqlDataReader["HtmlColor"].ToString();
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = mySqlDataReader["LinkedPO"].ToString();
                            }
                            if (mySqlDataReader["CurrencyName"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = mySqlDataReader["CurrencyName"].ToString();
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["IdCarriageMethod"] != DBNull.Value)
                            {
                                linkedOffers.IdCarriageMethod = Convert.ToInt32(mySqlDataReader["IdCarriageMethod"]);
                            }
                            if (mySqlDataReader["CarriageMethod"] != DBNull.Value)
                            {
                                linkedOffers.CarriageMethod = mySqlDataReader["CarriageMethod"].ToString();
                            }
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                linkedOffers.IdShippingAddress = Convert.ToInt32(mySqlDataReader["IdShippingAddress"]);
                            }
                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                linkedOffers.IdPerson = Convert.ToString(mySqlDataReader["IdPerson"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["OfferTypeName"] != DBNull.Value)
                            {
                                linkedOffers.OfferTypeName = mySqlDataReader["OfferTypeName"].ToString();
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = mySqlDataReader["Year"].ToString();
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToUInt32(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["category"] != DBNull.Value)
                            {
                                linkedOffers.Category = Convert.ToString(mySqlDataReader["category"]);
                            }
                            //[Rahul.Gadhave][GEOS2-7253][Date:07/05/2025]
                            if (mySqlDataReader["PlantFullName"] != DBNull.Value)
                            {
                                linkedOffers.PlantFullName = Convert.ToString(mySqlDataReader["PlantFullName"]);
                            }
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                            {
                                linkedOffers.Confirmation = mySqlDataReader["Conformation"].ToString();
                            }
                            //[rahul.gadhave]
                            if (mySqlDataReader["idCountry"] != DBNull.Value)
                            {
                                linkedOffers.IdCountry = Convert.ToUInt32(mySqlDataReader["idCountry"]);
                            }
                            if (mySqlDataReader["CompanyCountry"] != DBNull.Value)
                            {
                                linkedOffers.Country = Convert.ToString(mySqlDataReader["CompanyCountry"].ToString());
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                linkedOffers.Iso = Convert.ToString(mySqlDataReader["iso"]);
                                linkedOffers.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + linkedOffers.Iso + ".png";
                            }
                            //IdPerson
                            if (linkedOffers.OfferCurrency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.OfferCurrency))
                                {
                                    currencyISOs.Add(linkedOffers.OfferCurrency);
                                }
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.OfferCurrency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPoRequestLinkedOffers_V2640(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        public List<Email> GetAllUnprocessedEmails_V2660(string ConnectionStringgeos, string ConnectionStringemdep_geos, string poAnalyzerEmailToCheck, string attachedDocPath)
        {
            List<Email> EmailList = new List<Email>();
            List<LookupValue> UntructedExtensionList = GetLookupValues(ConnectionStringemdep_geos, 157);
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetUnProcessedEmails_V2570", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["SenderName"] != DBNull.Value)
                                    email.SenderName = rdr["SenderName"].ToString();
                                if (rdr["SenderEmail"] != DBNull.Value)
                                    email.SenderEmail = rdr["SenderEmail"].ToString();
                                if (rdr["ToName"] != DBNull.Value)
                                    email.RecipientName = rdr["ToName"].ToString();
                                if (rdr["ToEmail"] != DBNull.Value)
                                    email.RecipientEmail = rdr["ToEmail"].ToString();
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"].ToString();
                                if (rdr["Body"] != DBNull.Value)
                                    email.Body = rdr["Body"].ToString();
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                if (rdr["CCName"] != DBNull.Value)
                                    email.CCName = rdr["CCName"].ToString();
                                if (rdr["SourceInboxId"] != DBNull.Value)
                                    email.SourceInboxId = rdr["SourceInboxId"].ToString();
                                if (rdr["IsDeleted"] != DBNull.Value)
                                    email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                if (rdr["IdPORequest"] != DBNull.Value)
                                    email.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                            }
                            catch (Exception ex)
                            { }
                            EmailList.Add(email);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                try
                                {
                                    if (rdr["IdEmail"] != DBNull.Value)
                                    {
                                        Emailattachment attach = new Emailattachment();
                                        Email email = EmailList.FirstOrDefault(i => i.IdEmail == Convert.ToInt32(rdr["IdEmail"]));
                                        if (email != null)
                                        {
                                            if (email.EmailattachmentList == null)
                                            {
                                                email.EmailattachmentList = new List<Emailattachment>();
                                            }
                                            if (rdr["IdEmail"] != DBNull.Value)
                                                attach.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                            if (rdr["IdAttachment"] != DBNull.Value)
                                                attach.IdAttachment = Convert.ToInt32(rdr["IdAttachment"]);
                                            if (rdr["AttachmentName"] != DBNull.Value)
                                                attach.AttachmentName = Convert.ToString(rdr["AttachmentName"]);
                                            if (rdr["AttachmentPath"] != DBNull.Value)
                                                attach.AttachmentPath = Convert.ToString(rdr["AttachmentPath"]);
                                            if (rdr["CreatedIn"] != DBNull.Value)
                                                email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                            if (rdr["ModifiedIn"] != DBNull.Value)
                                                email.ModifiedIn = rdr["ModifiedIn"] as DateTime?;
                                            if (rdr["IsDeleted"] != DBNull.Value)
                                                email.IsDeleted = Convert.ToBoolean(rdr["IsDeleted"]);
                                            if (rdr["CreatedBy"] != DBNull.Value)
                                                email.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);
                                            if (rdr["ModifiedBy"] != DBNull.Value)
                                                email.ModifiedBy = rdr["ModifiedBy"] as int?;
                                            if (rdr["AttachmentExtension"] != DBNull.Value)
                                                attach.AttachmentExtension = Convert.ToString(rdr["AttachmentExtension"]);
                                            //attach.FileContent = GetEmailAttachedDoc(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                            if (!UntructedExtensionList.Any(j => j.Value?.Trim() == attach.AttachmentExtension?.Trim()))
                                            {
                                                try
                                                {
                                                    if (attach.AttachmentExtension?.ToLower() == ".pdf")
                                                    {
                                                        attach.FileText = GetPdfAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.LocationFileText = GetPdfAttachedDocTextByLocation(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                    }
                                                    //[pramod.misal][GEOS2-6735][27-01-2025]
                                                    if (attach.AttachmentExtension?.ToLower() == ".xls" || attach.AttachmentExtension?.ToLower() == ".xlsx")
                                                    {
                                                        attach.ExcelFileText = GetExcelAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.FileDocInBytes = ExcelFileAttachedDoc(attachedDocPath, attach);
                                                    }
                                                    //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                                                    if (attach.AttachmentExtension?.ToLower() == ".xml")
                                                    {
                                                        attach.XmlFileText = GetXmlAttachedDocText(attach.IdEmail, attachedDocPath, attach.AttachmentName);
                                                        attach.XmlFileDocInBytes = XmlFileAttachedDoc(attachedDocPath, attach);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                        }
                                        email.EmailattachmentList.Add(attach);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllUnprocessedEmails_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }
        public string GetXmlAttachedDocText(long idEmail, string attachedDocPath, string savedFileName)
        {
            var content = "";
            try
            {
                string fileUploadPath = Path.Combine(attachedDocPath, idEmail.ToString(), savedFileName);
                if (File.Exists(fileUploadPath))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileUploadPath);

                    // Traverse all nodes and extract their inner text
                    content = xmlDoc.InnerXml.ToString();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error GetXmlAttachedDocText(). ErrorMessage - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
            return content;
        }

        public byte[] XmlFileAttachedDoc(string attachmentPath, Emailattachment file)
        {
            byte[] bytes = null;
            string fileUploadPath = Path.Combine(attachmentPath, file.AttachmentName);
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (FileStream stream = new FileStream(fileUploadPath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error XmlFileAttachedDoc(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[rdixit][GEOS2-9020][23.07.2025]
        public bool AddEmails_V2660(Email emailDetails, string mainServerConnectionString, string attachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddEmailDetails_V2660", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_SenderName", emailDetails.SenderName);
                        mySqlCommand.Parameters.AddWithValue("_SenderEmail", emailDetails.SenderEmail);
                        mySqlCommand.Parameters.AddWithValue("_CCEmail", emailDetails.CCEmail);
                        mySqlCommand.Parameters.AddWithValue("_ToName", emailDetails.RecipientName);
                        mySqlCommand.Parameters.AddWithValue("_ToEmail", emailDetails.RecipientEmail);
                        mySqlCommand.Parameters.AddWithValue("_Subject", emailDetails.Subject);
                        mySqlCommand.Parameters.AddWithValue("_Body", emailDetails.Body);
                        mySqlCommand.Parameters.AddWithValue("_CCName", emailDetails.CCName);
                        mySqlCommand.Parameters.AddWithValue("_SourceInboxId", emailDetails.SourceInboxId);
                        mySqlCommand.Parameters.AddWithValue("_CreatedIn", emailDetails.CreatedIn);
                        mySqlCommand.Parameters.AddWithValue("_EmailSentAt", emailDetails.EmailSentAt);
                        mySqlCommand.Parameters.AddWithValue("_IdPlant", emailDetails.IdPlant);
                        mySqlCommand.Parameters.AddWithValue("_IdCustomer", emailDetails.IdCustomer);
                        emailDetails.IdEmail = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (emailDetails.IdEmail > 0 && emailDetails.EmailattachmentList?.Count > 0)
                    {
                        emailDetails.EmailattachmentList.ForEach(i => i.IdEmail = emailDetails.IdEmail);
                        AddEmailAttachedDoc_V2550(mainServerConnectionString, emailDetails.EmailattachmentList, attachedDocPath);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmails_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();
                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return true;
        }

        //[rdixit][GEOS2-9020][23.07.2025]
        public void AddUniqueOffersToPORequest_V2660(Int64 idPORequest, string quotationCode, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_AddUniqueOffersToPORequest_V2660", conn);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_QuotationCode", quotationCode);
                    mySqlCommand.Parameters.AddWithValue("_IdPORequest", idPORequest);
                    mySqlCommand.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in AddUniqueOffersToPORequest_V2660(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[rdixit][GEOS2-9020][23.07.2025]
        public bool UpdatePORequestGroupPlant_V2660(Email poRequest, string mainServerConnectionString)
        {
            try
            {
                if (poRequest != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("UpdatePORequestGroupPlant_V2660", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdEmail", poRequest.IdEmail);
                        mySqlCommand.Parameters.AddWithValue("_IdCustomer", poRequest.IdCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IdPlant", poRequest.IdPlant);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePORequestGroupPlant_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }
        //[Rahul.Gadhave][GEOS2-9080][Date:30-07-2025]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2660(string ConnectionString, string SelectedIdPlant, string SelectedIdGroup, string correnciesIconFilePath, GeosAppSetting geosAppSetting, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            string AppSetting = "";
            if (geosAppSetting != null)
            {
                AppSetting = geosAppSetting.DefaultValue;
            }
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedofferByIdPlantAndGroup_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPlant", SelectedIdPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdGroup", SelectedIdGroup);
                    mySqlCommand.Parameters.AddWithValue("_geosAppSetting", AppSetting);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = Convert.ToString(mySqlDataReader["Code"]);

                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = Convert.ToString(mySqlDataReader["Description"]);
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = Convert.ToString(mySqlDataReader["Rfq"]);
                            }
                            if (mySqlDataReader["Contact"] != DBNull.Value)
                            {
                                linkedOffers.Contact = Convert.ToString(mySqlDataReader["Contact"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            //if (mySqlDataReader["CustomerName"] != DBNull.Value)
                            //{
                            //    linkedOffers.CutomerName = Convert.ToString(mySqlDataReader["CustomerName"]);
                            //}
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["OfferStatus"] != DBNull.Value)
                            {
                                linkedOffers.Status = Convert.ToString(mySqlDataReader["OfferStatus"]);
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["OfferCurrency"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = Convert.ToString(mySqlDataReader["OfferCurrency"]);
                            }
                            if (mySqlDataReader["Currency"] != DBNull.Value)
                            {
                                linkedOffers.Currency = Convert.ToString(mySqlDataReader["Currency"]);
                            }
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToInt64(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                linkedOffers.Plant = Convert.ToString(mySqlDataReader["Plant"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdPlant = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["offersType"] != DBNull.Value)
                            {
                                linkedOffers.OffersType = Convert.ToString(mySqlDataReader["offersType"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = Convert.ToString(mySqlDataReader["LinkedPO"]);
                            }
                            if (linkedOffers.Currency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.Currency))
                                {
                                    currencyISOs.Add(linkedOffers.Currency);
                                }
                            }
                            if (mySqlDataReader["AttachmentFileName"] != DBNull.Value)
                            {
                                linkedOffers.AttachmentFileName = Convert.ToString(mySqlDataReader["AttachmentFileName"]);
                                //linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, linkedOffers);
                                linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc_V2670(CommericalPath, linkedOffers);

                            }
                            //[pramod.misal][04-08-2025]                           
                            if (mySqlDataReader["Country"] != DBNull.Value)
                            {
                                linkedOffers.Country = Convert.ToString(mySqlDataReader["Country"].ToString());
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                linkedOffers.Iso = Convert.ToString(mySqlDataReader["iso"]);
                                linkedOffers.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + linkedOffers.Iso + ".png";
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetLinkedofferByIdPlantAndGroup_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[ashish.malkhede][08-07-2025] https://helpdesk.emdep.com/browse/GEOS2-9105
        public Int64 InsertPurchaseOrder_V2660(PORegisteredDetails PO, string mainServerConnectionString, string CommericalPath)
        {
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_InsertCustomerPurchaseOrders_V2660", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdPOType", PO.IdPOType);
                        MyCommand.Parameters.AddWithValue("_IdSite", PO.IdSite);
                        MyCommand.Parameters.AddWithValue("_Code", PO.Code);
                        MyCommand.Parameters.AddWithValue("_ReceivedIn", PO.ReceptionDateNew);
                        MyCommand.Parameters.AddWithValue("_IdShippingAddress", PO.IdShippingAddress);
                        MyCommand.Parameters.AddWithValue("_Value", PO.POValue);
                        MyCommand.Parameters.AddWithValue("_IdCurrency", PO.IdCurrency);
                        MyCommand.Parameters.AddWithValue("_AttachmentFileName", PO.AttachmentFileName);
                        MyCommand.Parameters.AddWithValue("_IdSender", PO.IdSender);
                        MyCommand.Parameters.AddWithValue("_Sender", PO.Sender);
                        MyCommand.Parameters.AddWithValue("_NOK", PO.IsOK);
                        MyCommand.Parameters.AddWithValue("_IsCancelledStatus", PO.IsCancelled);
                        MyCommand.Parameters.AddWithValue("_CreatedBy", PO.UpdatedBy);
                        MyCommand.Parameters.AddWithValue("_CreatedIN", DateTime.Now);
                        PO.IdPO = Convert.ToInt32(MyCommand.ExecuteScalar());
                        con.Close();
                    }
                    // Insert and Delete Linked Offers
                    InsertUpdateLinkedOffersByPO_V2640(PO, mainServerConnectionString, CommericalPath);
                    if (PO.IdStatus != 0 && PO.IdOffer != 0)
                    {
                        if (PO.IdStatus == 3)
                        {
                            OTM_UpdateOfferStatus_V2660(PO, mainServerConnectionString)
;
                        }
                    }
                    //// Inset Log
                    if (PO.LogEntriesByPO != null && PO.LogEntriesByPO.Count > 0)
                    {
                        // Insert Log entries Po logentriesbypo
                        //AddLogEntriesByPO(PO.LogEntriesByPO, PO.IdPO, mainServerConnectionString);
                        // Insert Log All linked offers by PO
                        if (PO.OffersLinked.Count > 0)
                        {
                            foreach (LinkedOffers lo in PO.OffersLinked)
                            {
                                AddLogEntriesLinkedOfferByPO(lo.IdOffer, PO.LogEntriesByPO, mainServerConnectionString);
                            }
                        }
                    }
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
            return PO.IdPO;
        }
        //[Rahul.Gadhave][GEOS2-9113][Date:01-08-2025]
        public List<People> GetPOReceptionEmailCCFeilds_V2660(string connectionstring, Int64 IdPO)
        {
            List<People> peoples = new List<People>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOReceptionEmailCCFeilds_V2590", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPO", IdPO);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdSalesOwner"] != DBNull.Value)
                            {
                                People SalesOwner = new People();
                                SalesOwner.IdPerson = Convert.ToInt32(reader["IdSalesOwner"]);
                                SalesOwner.Name = Convert.ToString(reader["SalesOwnerName"]);
                                SalesOwner.Surname = Convert.ToString(reader["SalesOwnerSurname"]);
                                SalesOwner.Email = Convert.ToString(reader["SalesOwnerEmail"]);
                                peoples.Add(SalesOwner);
                            }
                            if (reader["OfferedBy"] != DBNull.Value)
                            {
                                People OfferOwner = new People();
                                OfferOwner.IdPerson = Convert.ToInt32(reader["OfferedBy"]);
                                OfferOwner.Name = Convert.ToString(reader["OfferOwnerName"]);
                                OfferOwner.Surname = Convert.ToString(reader["OfferOwnerSurname"]);
                                OfferOwner.Email = Convert.ToString(reader["OfferOwnerEmail"]);
                                peoples.Add(OfferOwner);
                            }
                            //if (reader["AssignedTo"] != DBNull.Value)
                            //{
                            //    People OfferAssignee = new People();
                            //    OfferAssignee.IdPerson = Convert.ToInt32(reader["AssignedTo"]);
                            //    OfferAssignee.Name = Convert.ToString(reader["OfferAssigneeName"]);
                            //    OfferAssignee.Surname = Convert.ToString(reader["OfferAssigneeSurname"]);
                            //    OfferAssignee.Email = Convert.ToString(reader["OfferAssigneeEmail"]);
                            //    peoples.Add(OfferAssignee);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPOReceptionEmailCCFeilds_V2590(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public int GetUserPlant_V2660(string connectionString, int idperson)
        {
            int idsite = 0;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetIdsiteByIdPerson", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_Idperson", idperson);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdSite"] != DBNull.Value)
                            {
                                idsite = Convert.ToInt32(reader["IdSite"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetUserPlant_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return idsite;
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public List<Company> GetAllCompanyDetailsList_V2660(string connectionString)
        {
            List<Company> connectionstrings = new List<Company>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand command = new MySqlCommand("OTM_GetAllPlantsDetails_V2660", con);
                    command.CommandType = CommandType.StoredProcedure;
                    List<string> strDatabaseIP = new List<string>();
                    // Parse the connection string
                    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
                    // Extract components
                    string ConnecteddataSource = builder.Server.ToUpper();
                    string database = builder.Database;
                    string userId = builder.UserID;
                    string password = builder.Password;
                    string connectedPlantName = GetConnectedPlantNameFromDataSource(ConnecteddataSource.ToUpper(), connectionString);
                    string dataSource = "";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Company company = new Company();
                            if (string.IsNullOrEmpty(connectedPlantName))
                            {
                                dataSource = ConnecteddataSource.Replace(ConnecteddataSource, reader["DatabaseIP"].ToString());
                            }
                            else
                            {
                                dataSource = ConnecteddataSource.Replace(connectedPlantName, reader["ShortName"].ToString());
                                //This will removed from code when we update everywhere logic to hit service through service provider url not from database connection string
                            }
                            string connstr = connectionString.Replace(builder.Server, dataSource);
                            if (connectionstrings.Count == 0)
                            {
                                strDatabaseIP.Add(reader["DatabaseIP"].ToString());
                                company.Alias = reader["ShortName"].ToString();
                                company.ShortName = reader["ShortName"].ToString();
                                company.Name = reader["Name"].ToString();
                                company.IdCompany = Convert.ToInt32(reader["idSite"].ToString());
                                company.Country = new Country();
                                company.Country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                                company.Country.Name = reader["name"].ToString();
                                company.ConnectPlantId = reader["idSite"].ToString();
                                company.ConnectPlantConstr = connstr;
                                company.ServiceProviderUrl = reader["ServiceProviderUrl"].ToString();
                                connectionstrings.Add(company);
                            }
                            if (strDatabaseIP.Exists(cs => cs.ToString() == reader["DatabaseIP"].ToString()))
                            {
                            }
                            else
                            {
                                strDatabaseIP.Add(reader["DatabaseIP"].ToString());
                                company.Alias = reader["ShortName"].ToString();
                                company.ShortName = reader["ShortName"].ToString();
                                company.Name = reader["Name"].ToString();
                                company.IdCompany = Convert.ToInt32(reader["idSite"].ToString());
                                company.Country = new Country();
                                company.Country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                                company.Country.Name = reader["name"].ToString();
                                company.ConnectPlantId = reader["idSite"].ToString();
                                company.ConnectPlantConstr = connstr;
                                company.ServiceProviderUrl = reader["ServiceProviderUrl"].ToString();
                                connectionstrings.Add(company);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCompanyDetailsList_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return connectionstrings;
        }

        public bool OTM_CheckOfferExistsInGeos_V2660(int year, int number, string workbenchConnectionString)
        {
            bool isOfferExist = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_CheckOfferExistsInGeos_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_Year", year);
                    mySqlCommand.Parameters.AddWithValue("_Number", number);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            try
                            {
                                isOfferExist = true;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error OTM_CheckOfferExistsInGeos_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_CheckOfferExistsInGeos_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isOfferExist;
        }

        //[rdixit][GEOS2-9141][02.08.2025] get ftom  EPIN
        public Offer GetOfferDetailsByYearAndNumber_V2660(int year, int number, string connectionString)
        {
            Offer offer = new Offer();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    conn.Open();
                    MySqlCommand conoffercommand = new MySqlCommand("OTM_GetOfferDetailsByYearAndNumber_V2660", conn);
                    conoffercommand.CommandType = CommandType.StoredProcedure;
                    conoffercommand.Parameters.AddWithValue("_Year", year);
                    conoffercommand.Parameters.AddWithValue("_Number", number);
                    using (MySqlDataReader offerreader = conoffercommand.ExecuteReader())
                    {
                        if (offerreader.Read())
                        {
                            offer.IdOffer = Convert.ToInt64(offerreader["idoffer"].ToString());
                            offer.Year = Convert.ToInt64(offerreader["Year"].ToString());
                            offer.Number = Convert.ToInt64(offerreader["number"].ToString());
                            offer.Code = offerreader["code"].ToString();
                            offer.IdCustomer = Convert.ToInt32(offerreader["idcustomer"].ToString());
                            offer.IdOfferType = Convert.ToByte(offerreader["idOfferType"].ToString());
                            offer.IdCurrency = Convert.ToByte(offerreader["IdCurrency"].ToString());
                            offer.IdStatus = Convert.ToByte(offerreader["IdStatus"].ToString());

                            if (offerreader["Description"] != DBNull.Value)
                                offer.Description = offerreader["Description"].ToString();

                            if (offerreader["contact"] != DBNull.Value)
                                offer.Contact = offerreader["contact"].ToString();

                            if (offerreader["probabilityOfSuccess"] != DBNull.Value)
                                offer.ProbabilityOfSuccess = Convert.ToSByte(offerreader["probabilityOfSuccess"].ToString());

                            if (offerreader["Rfq"] != DBNull.Value)
                                offer.Rfq = offerreader["Rfq"].ToString();

                            if (offerreader["Value"] != DBNull.Value)
                                offer.Value = Convert.ToDouble(offerreader["Value"].ToString());

                            if (offerreader["IdCustomerToDelivery"] != DBNull.Value)
                                offer.IdCustomerToDelivery = Convert.ToInt32(offerreader["IdCustomerToDelivery"].ToString());

                            if (offerreader["ModifiedIn"].ToString() == "01/01/1000 0:00:00")
                                offer.ModifiedIn = null;
                            else
                                offer.ModifiedIn = Convert.ToDateTime(offerreader["ModifiedIn"].ToString());

                            if (offerreader["modifiedBy"] != DBNull.Value)
                                offer.ModifiedBy = Convert.ToInt32(offerreader["modifiedBy"].ToString());

                            if (offerreader["CreatedIn"] != DBNull.Value)
                                offer.CreatedIn = Convert.ToDateTime(offerreader["CreatedIn"].ToString());

                            if (offerreader["createdBy"] != DBNull.Value)
                                offer.CreatedBy = Convert.ToInt32(offerreader["createdBy"].ToString());

                            if (offerreader["sendIn"] != DBNull.Value)
                                offer.SendIn = Convert.ToDateTime(offerreader["sendIn"].ToString());
                            else
                                offer.SendIn = null;

                            if (offerreader["Comments"] != DBNull.Value)
                                offer.Comments = offerreader["Comments"].ToString();

                            if (offerreader["Discount"] != DBNull.Value)
                                offer.Discount = float.Parse(offerreader["Discount"].ToString());

                            if (offerreader["Title"] != DBNull.Value)
                                offer.Title = Convert.ToString(offerreader["Title"].ToString());

                            if (offerreader["DeliveryWeeks"] != DBNull.Value)
                                offer.DeliveryWeeks = Convert.ToInt32(offerreader["DeliveryWeeks"].ToString());

                            if (offerreader["AssignedTo"] != DBNull.Value)
                                offer.AssignedTo = Convert.ToInt32(offerreader["AssignedTo"].ToString());

                            if (offerreader["productionFinish"] != DBNull.Value)
                                offer.ProductionFinish = Convert.ToDateTime(offerreader["productionFinish"].ToString());

                            if (offerreader["assignedIn"] != DBNull.Value)
                                offer.AssignedIn = Convert.ToDateTime(offerreader["assignedIn"].ToString());

                            if (offerreader["idOfferType"] != DBNull.Value)
                                offer.IdOfferType = Convert.ToByte(offerreader["idOfferType"].ToString());

                            if (offerreader["idcarproject"] != DBNull.Value)
                                offer.IdCarProject = Convert.ToInt64(offerreader["idcarproject"].ToString());

                            if (offerreader["Priority"] != DBNull.Value)
                                offer.Priority = Convert.ToInt32(offerreader["Priority"].ToString());

                            if (offerreader["IdShippingAddress"] != DBNull.Value)
                                offer.IdShippingAddress = Convert.ToInt64(offerreader["IdShippingAddress"].ToString());

                            if (offerreader["IsCritical"] != DBNull.Value)
                                offer.IsCritical = Convert.ToByte(offerreader["IsCritical"].ToString());

                            if (offerreader["offeredBy"] != DBNull.Value)
                                offer.OfferedBy = Convert.ToInt32(offerreader["offeredBy"].ToString());

                            if (offerreader["idServiceRequest"] != DBNull.Value)
                                offer.IdServiceRequest = Convert.ToInt64(offerreader["idServiceRequest"].ToString());

                            if (offerreader["rfqReception"] != DBNull.Value)
                                offer.RFQReception = Convert.ToDateTime(offerreader["rfqReception"].ToString());

                            if (offerreader["deliveryDate"] != DBNull.Value)
                                offer.DeliveryDate = Convert.ToDateTime(offerreader["deliveryDate"].ToString());

                            offer.Site = new Company();
                            if (offerreader["idsite"] != DBNull.Value)
                                offer.Site.IdCompany = Convert.ToInt32(offerreader["idsite"].ToString());

                            if (offerreader["FullName"] != DBNull.Value)
                                offer.Site.FullName = offerreader["FullName"].ToString();

                            if (offerreader["IdCarOEM"] != DBNull.Value)
                                offer.IdCarOEM = Convert.ToInt32(offerreader["IdCarOEM"].ToString());

                            if (offerreader["NewProduct"] != DBNull.Value)
                                offer.NewProduct = Convert.ToBoolean(offerreader["NewProduct"]);

                            if (offerreader["IdBusinessUnit"] != DBNull.Value)
                                offer.IdBusinessUnit = Convert.ToByte(offerreader["IdBusinessUnit"].ToString());

                            if (offerreader["IdSalesOwner"] != DBNull.Value)
                                offer.IdSalesOwner = Convert.ToInt32(offerreader["IdSalesOwner"].ToString());

                            if (offerreader["IdSource"] != DBNull.Value)
                                offer.IdSourceNew = Convert.ToInt32(offerreader["IdSource"].ToString());

                            if (offerreader["IdCarriageMethod"] != DBNull.Value)
                                offer.IdCarriageMethod = Convert.ToByte(offerreader["IdCarriageMethod"].ToString());

                            if (offerreader["IdPersonType"] != DBNull.Value)
                                offer.IdPersonType = Convert.ToInt32(offerreader["IdPersonType"].ToString());

                            if (offerreader["IdApprover"] != DBNull.Value)
                                offer.IdApprover = Convert.ToInt32(offerreader["IdApprover"].ToString());

                            if (offerreader["InitialOfferValue"] != DBNull.Value)
                                offer.InitialOfferValue = Convert.ToDouble(offerreader["InitialOfferValue"].ToString());

                            if (offerreader["IdJiraCode"] != DBNull.Value)
                                offer.IdJiraCode = Convert.ToInt32(offerreader["IdJiraCode"].ToString());

                            if (offerreader["IdProblem"] != DBNull.Value)
                                offer.IdProblem = Convert.ToInt32(offerreader["IdProblem"].ToString());

                        }
                        if (offerreader.NextResult())
                        {
                            while (offerreader.Read())
                            {
                                try
                                {
                                    if (offerreader["IdOffer"] != DBNull.Value)
                                    {
                                        if (offer.OfferContacts == null)
                                            offer.OfferContacts = new List<OfferContact>();

                                        OfferContact offerContact = new OfferContact();
                                        if (offerreader["IdOffer"] != DBNull.Value)
                                            offerContact.IdOffer = Convert.ToInt64(offerreader["IdOffer"].ToString());

                                        if (offerreader["IdContact"] != DBNull.Value)
                                            offerContact.IdContact = Convert.ToInt32(offerreader["IdContact"].ToString());

                                        if (offerreader["IdOfferContact"] != DBNull.Value)
                                            offerContact.IdOfferContact = Convert.ToInt64(offerreader["IdOfferContact"].ToString());

                                        if (offerreader["IsPrimaryOfferContact"] != DBNull.Value)
                                            offerContact.IsPrimaryOfferContact = Convert.ToByte(offerreader["IsPrimaryOfferContact"].ToString());
                                        // isPrimaryOfferContact
                                        offer.OfferContacts.Add(offerContact);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetOfferDetailsByYearAndNumber_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOfferDetailsByYearAndNumber_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return offer;
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertOffer_V2660(Quotation q, string connectionString, string WorkingOrdersPath, string workbenchConnectionString)
        {
            Int64 offerId = 0;
            TransactionScope transactionScope = null;
            Offer offer = q.Offer;
            try
            {
                using (transactionScope = new System.Transactions.TransactionScope())
                {
                    //Transaction.Current.
                    using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                    {
                        conoffer.Open();
                        try
                        {

                            MySqlCommand conoffercommand = new MySqlCommand("OTM_InsertOffer_V2660", conoffer);
                            conoffercommand.CommandType = CommandType.StoredProcedure;
                            conoffercommand.Parameters.AddWithValue("_Code", offer.Code);
                            conoffercommand.Parameters.AddWithValue("_IdOfferType", offer.IdOfferType);
                            conoffercommand.Parameters.AddWithValue("_IdCustomer", offer.IdCustomer);
                            conoffercommand.Parameters.AddWithValue("_IdProject", offer.IdProject);
                            conoffercommand.Parameters.AddWithValue("_Description", offer.Description);
                            conoffercommand.Parameters.AddWithValue("_IdStatus", offer.IdStatus);
                            conoffercommand.Parameters.AddWithValue("_Amount", offer.Value);
                            conoffercommand.Parameters.AddWithValue("_ProbabilityOfSuccess", offer.ProbabilityOfSuccess);
                            conoffercommand.Parameters.AddWithValue("_OfferExpectedDate", offer.OfferExpectedDate);
                            conoffercommand.Parameters.AddWithValue("_IdCurrency", offer.IdCurrency);
                            conoffercommand.Parameters.AddWithValue("_Year", DateTime.Now.Year);
                            conoffercommand.Parameters.AddWithValue("_Number", offer.Number);
                            conoffercommand.Parameters.AddWithValue("_CreatedBy", offer.CreatedBy);
                            conoffercommand.Parameters.AddWithValue("_CreatedIn", offer.CreatedIn);
                            conoffercommand.Parameters.AddWithValue("_ModifiedBy", offer.ModifiedBy);
                            conoffercommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now);
                            conoffercommand.Parameters.AddWithValue("_DeliveryDate", offer.DeliveryDate);
                            conoffercommand.Parameters.AddWithValue("_IdBusinessUnit", offer.IdBusinessUnit);
                            conoffercommand.Parameters.AddWithValue("_IdSalesOwner", offer.IdSalesOwner);
                            conoffercommand.Parameters.AddWithValue("_IdCarOEM", offer.IdCarOEM);
                            conoffercommand.Parameters.AddWithValue("_IdSource", offer.IdSourceNew);
                            conoffercommand.Parameters.AddWithValue("_RFQReception", offer.RFQReception);
                            conoffercommand.Parameters.AddWithValue("_Rfq", offer.Rfq);
                            conoffercommand.Parameters.AddWithValue("_SendIn", offer.SendIn);
                            conoffercommand.Parameters.AddWithValue("_IdCarProject", offer.IdCarProject);
                            conoffercommand.Parameters.AddWithValue("_IdSourceOffer", offer.IdSourceOffer);
                            conoffercommand.Parameters.AddWithValue("_IdReporterSource", offer.IdReporterSource);
                            conoffercommand.Parameters.AddWithValue("_IdProductCategory", offer.IdProductCategory);
                            conoffercommand.Parameters.AddWithValue("_IsManualCategory", offer.IsManualCategory);
                            conoffercommand.Parameters.AddWithValue("_Discount", offer.Discount);
                            conoffercommand.Parameters.AddWithValue("_contact", offer.Contact);
                            conoffercommand.Parameters.AddWithValue("_IdCustomerToDelivery", offer.IdCustomerToDelivery);
                            conoffercommand.Parameters.AddWithValue("_Comments", offer.Comments);
                            conoffercommand.Parameters.AddWithValue("_Title", offer.Title);
                            conoffercommand.Parameters.AddWithValue("_DeliveryWeeks", offer.DeliveryWeeks);
                            conoffercommand.Parameters.AddWithValue("_AssignedTo", offer.AssignedTo);
                            conoffercommand.Parameters.AddWithValue("_ProductionFinish", offer.ProductionFinish);
                            conoffercommand.Parameters.AddWithValue("_AssignedIn", offer.AssignedIn);
                            conoffercommand.Parameters.AddWithValue("_Priority", offer.Priority);
                            conoffercommand.Parameters.AddWithValue("_IdShippingAddress", offer.IdShippingAddress);
                            conoffercommand.Parameters.AddWithValue("_IsCritical", offer.IsCritical);
                            conoffercommand.Parameters.AddWithValue("_offeredBy", offer.OfferedBy);
                            conoffercommand.Parameters.AddWithValue("_IdServiceRequest", offer.IdServiceRequest);
                            conoffercommand.Parameters.AddWithValue("_IdSite", offer.Site.IdCompany);
                            conoffercommand.Parameters.AddWithValue("_NewProduct", offer.NewProduct);
                            conoffercommand.Parameters.AddWithValue("_IdCarriageMethod", offer.IdCarriageMethod);
                            conoffercommand.Parameters.AddWithValue("_IdPersonType", offer.IdPersonType);
                            conoffercommand.Parameters.AddWithValue("_IdApprover", offer.IdApprover);
                            conoffercommand.Parameters.AddWithValue("_InitialOfferValue", offer.InitialOfferValue);
                            conoffercommand.Parameters.AddWithValue("_IdJiraCode", offer.IdJiraCode);
                            conoffercommand.Parameters.AddWithValue("_IdProblem", offer.IdProblem);

                            offerId = Convert.ToInt64(conoffercommand.ExecuteScalar());
                            offer.IdOffer = offerId;
                        }
                        catch (Exception ex)
                        {
                            if (Transaction.Current != null)
                                Transaction.Current.Rollback();
                            if (transactionScope != null)
                                transactionScope.Dispose();
                            Log4NetLogger.Logger.Log(string.Format("Error InsertOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (offer.IdOffer > 0)
                    {
                        if (offer.OfferContacts != null)
                        {
                            InsertOfferContacts(offer, connectionString);
                        }
                        if (offer.OptionsByOffers != null)
                        {
                            offer.OptionsByOffers.ForEach(i => i.IdOffer = offer.IdOffer);
                            InsertOptionByOffer_V2660(offer.OptionsByOffers, connectionString);
                        }
                        if (q.IdDetectionsTemplate == 1)
                        {
                            InsertQuotations_V2660(q, connectionString);
                        }
                        else if (q.IdDetectionsTemplate == 5)
                        {
                            InsertMaterailQuotations_V2660(q, connectionString);
                        }

                        // Insert Log
                        LogEntryByOffer ltype = new LogEntryByOffer();
                        ltype.IdLogEntryType = Convert.ToByte(1);
                        ltype.Comments = string.Format("Offer {0} Imported", offer.Code);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    if (offer != null)
                    {
                        CreateFolderOffer_V2620(offer, connectionString, WorkingOrdersPath, workbenchConnectionString, true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                Log4NetLogger.Logger.Log(string.Format("Error InsertOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return offerId;
        }

        public bool OTM_CheckQuotationExistsInGeos_V2660(string Code, string workbenchConnectionString)
        {
            bool isquotationExist = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(workbenchConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_CheckQuotationExistsInGeos_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_Code", Code);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            try
                            {
                                if (empReader.Read())
                                {
                                    isquotationExist = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error OTM_CheckQuotationExistsInGeos_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_CheckQuotationExistsInGeos_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isquotationExist;
        }
        public Quotation OTM_GetQuotationByCode_V2660(string connectionstring, string code, int numOT, string otcode, Quotation q)
        {
            //Quotation q = new Quotation();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionstring))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("OTM_GetQuotationByCode_V2660", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_Code", code);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                q.IdQuotation = Convert.ToInt32(reader["IdQuotation"]);
                                q.Code = reader["code"].ToString();//
                                q.Year = Convert.ToInt32(reader["year"]);//
                                q.Number = Convert.ToInt32(reader["number"]);//
                                q.ProjectName = reader["projectname"].ToString();//
                                q.IsCO = Convert.ToByte(reader["isCO"]);//
                                q.Description = reader["Description"].ToString();//
                                q.IdTechnicalTemplate = Convert.ToUInt32(reader["IdTechnicalTemplate"]);//
                                q.IdDetectionsTemplate = Convert.ToByte(reader["IdDetectionsTemplate"]);//
                                q.CreatedIn = Convert.ToDateTime(reader["CreatedIn"]);//                              
                                q.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);  //
                            }
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    if (q.Offer == null)
                                    {
                                        q.Offer = new Offer();
                                        q.Offer.OptionsByOffers = new List<OptionsByOffer>();
                                    }
                                    try
                                    {
                                        if (reader["IdOffer"] != DBNull.Value)
                                        {
                                            if (q.Offer.OptionsByOffers == null)
                                                q.Offer.OptionsByOffers = new List<OptionsByOffer>();

                                            OptionsByOffer opbyoffer = new OptionsByOffer();
                                            if (reader["IdOption"] != DBNull.Value)
                                                opbyoffer.IdOption = Convert.ToInt64(reader["IdOption"].ToString());

                                            if (reader["Quantity"] != DBNull.Value)
                                                opbyoffer.Quantity = Convert.ToInt32(reader["Quantity"].ToString());

                                            if (reader["IdOffer"] != DBNull.Value)
                                                opbyoffer.IdOffer = Convert.ToInt64(reader["IdOffer"].ToString());

                                            q.Offer.OptionsByOffers.Add(opbyoffer);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log4NetLogger.Logger.Log(string.Format("Error OTM_GetQuotationByCode_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                            }
                        }
                        OTM_GetRevision_V2660(q, connectionstring);
                        if (q.IdDetectionsTemplate == 1)
                            OTM_GetOTInformation_V2660(q, numOT, otcode, connectionstring);
                        if (q.IdDetectionsTemplate == 5)
                            OTM_GetMaterialOTInformation_V2660(q, numOT, otcode, connectionstring);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(
                    string.Format("Error OTM_GetQuotationByCode_V2660(). ErrorMessage- {0}", ex.Message),
                    category: Category.Exception,
                    priority: Priority.Low);
                throw;
            }
            return q;
        }

        public void OTM_GetRevision_V2660(Quotation quo, string connectionstring)
        {
            quo.Revisions = new List<Revision>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionstring))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("OTM_GetRevision_V2660", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_idquotation", quo.IdQuotation);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                Revision rev = new Revision();
                                rev.Quotation = new Quotation();
                                rev.Quotation.IdQuotation = quo.IdQuotation;
                                if (reader["NumRevision"] != DBNull.Value)
                                    rev.NumRevision = Convert.ToInt32(reader["NumRevision"]);
                                if (reader["MadeBy"] != DBNull.Value)
                                    rev.CreatedBy = Convert.ToInt32(reader["MadeBy"]);
                                if (reader["ReviewedBy"] != DBNull.Value)
                                    rev.ReviewedBy = Convert.ToInt32(reader["ReviewedBy"]);
                                if (reader["CreationDate"] != DBNull.Value)
                                    rev.CreatedIn = Convert.ToDateTime(reader["CreationDate"]);
                                if (reader["Comments"] != DBNull.Value)
                                    rev.Comments = reader["Comments"]?.ToString();
                                if (reader["SentTo"] != DBNull.Value)
                                    rev.SentToComments = reader["SentTo"]?.ToString();
                                if (reader["Closed"] != DBNull.Value)
                                    rev.Closed = Convert.ToDateTime(reader["Closed"]);
                                if (reader["SentToClient"] != DBNull.Value)
                                    rev.SentToClient = Convert.ToBoolean(reader["SentToClient"]);
                                if (reader["Discount"] != DBNull.Value)
                                    rev.Discount = Convert.ToDecimal(reader["Discount"]);
                                if (reader["ExpiryDate"] != DBNull.Value)
                                    rev.ExpireDate = Convert.ToDateTime(reader["ExpiryDate"]);
                                if (reader["IdCurrency"] != DBNull.Value)
                                    rev.IdCurrency = Convert.ToInt32(reader["IdCurrency"]);
                                if (reader["idrevision"] != DBNull.Value)
                                    rev.Id = Convert.ToInt32(reader["idrevision"]);     // Make sure it's lowercase or use reader.GetOrdinal
                                quo.Revisions.Add(rev);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(
                    string.Format("Error OTM_GetRevision_V2660(). ErrorMessage- {0}", ex.Message),
                    category: Category.Exception,
                    priority: Priority.Low);
                throw;
            }
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public void OTM_GetOTInformation_V2660(Quotation quo, int numOT, string code, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("OTM_GetOTInformation_V2660", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_NumOT", numOT);
                        cmd.Parameters.AddWithValue("_Code", code);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (quo.Revisions == null)
                                    quo.Revisions = new List<Revision>();
                                RevisionItem item = new RevisionItem();

                                if (item.CPProduct == null)
                                    item.CPProduct = new CPProduct();
                                if (reader["IdRevision"] != DBNull.Value)
                                    item.IdRevision = Convert.ToInt64(reader["IdRevision"]);
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                    item.IdRevisionItemImported = Convert.ToInt64(reader["IdRevisionItem"]);
                                if (reader["IdRevisionItemStatus"] != DBNull.Value)
                                    item.Status = Convert.ToInt32(11);

                                if (reader["NumItem"] != DBNull.Value)
                                    item.NumItem = Convert.ToString(reader["NumItem"]);

                                if (reader["IdProduct"] != DBNull.Value)
                                    item.IdProduct = Convert.ToInt32(reader["IdProduct"]);

                                if (reader["Quantity"] != DBNull.Value)
                                    item.Quantity = Convert.ToInt32(reader["Quantity"]);

                                if (reader["UnitPrice"] != DBNull.Value)
                                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);

                                if (reader["ManualPrice"] != DBNull.Value)
                                    item.ManualPrice = Convert.ToBoolean(reader["ManualPrice"]);

                                if (reader["Validated"] != DBNull.Value)
                                    item.Validated = Convert.ToBoolean(reader["Validated"]);

                                item.CustomerComment = reader["CustomerComment"] != DBNull.Value ? reader["CustomerComment"].ToString() : null;

                                item.InternalComment = reader["InternalComment"] != DBNull.Value ? reader["InternalComment"].ToString() : null;

                                if (reader["Obsolete"] != DBNull.Value)
                                    item.Obsolete = Convert.ToInt16(reader["Obsolete"]);

                                if (reader["Marked"] != DBNull.Value)
                                    item.Marked = Convert.ToByte(reader["Marked"]);

                                if (reader["OiCreatedIn"] != DBNull.Value)
                                    item.CreatedIn = Convert.ToDateTime(reader["OiCreatedIn"]);

                                if (reader["OiCreatedBy"] != DBNull.Value)
                                    item.CreatedBy = Convert.ToInt32(reader["OiCreatedBy"]);

                                if (reader["OiModifiedBy"] != DBNull.Value)
                                    item.ModifiedBy = Convert.ToInt32(reader["OiModifiedBy"]);

                                if (reader["OiModifiedIn"] != DBNull.Value)
                                    item.ModifiedIn = Convert.ToDateTime(reader["OiModifiedIn"]);

                                item.AttachedFiles = reader["AttachedFiles"] != DBNull.Value ? reader["AttachedFiles"].ToString() : null;

                                if (reader["IsCritical"] != DBNull.Value)
                                    item.IsCritical = Convert.ToBoolean(reader["IsCritical"]);

                                if (reader["Reference"] != DBNull.Value)
                                    item.CPProduct.Reference = Convert.ToString(reader["Reference"].ToString());

                                if (reader["IdCP"] != DBNull.Value)
                                    item.CPProduct.IdCP = Convert.ToInt32(reader["IdCP"].ToString());

                                if (reader["OtherReference"] != DBNull.Value)
                                    item.CPProduct.OtherReference = Convert.ToString(reader["OtherReference"].ToString());

                                if (reader["Samples"] != DBNull.Value)
                                    item.CPProduct.Samples = Convert.ToString(reader["Samples"].ToString());

                                if (reader["Ways"] != DBNull.Value)
                                    item.CPProduct.Ways = Convert.ToInt32(reader["Ways"].ToString());

                                if (reader["IdDrawing"] != DBNull.Value)
                                    item.CPProduct.IdDrawing = Convert.ToInt32(reader["IdDrawing"].ToString());

                                if (reader["Zone"] != DBNull.Value)
                                    item.CPProduct.Zone = Convert.ToString(reader["Zone"].ToString());

                                if (reader["IdCPType"] != DBNull.Value)
                                    item.CPProduct.IdCPType = Convert.ToByte(reader["IdCPType"].ToString());

                                if (reader["FullMatchWithDrawing"] != DBNull.Value)
                                    item.CPProduct.FullMatchWithDrawing = Convert.ToBoolean(reader["FullMatchWithDrawing"].ToString());

                                if (reader["SampleReceivedIn"] != DBNull.Value)
                                    item.CPProduct.SampleReceivedIn = Convert.ToDateTime(reader["SampleReceivedIn"].ToString());

                                if (reader["IdDrawingAssignedBy"] != DBNull.Value)
                                    item.CPProduct.IdDrawingAssignedBy = Convert.ToInt32(reader["IdDrawingAssignedBy"].ToString());

                                if (reader["DrawingType"] != DBNull.Value)
                                    item.CPProduct.DrawingType = Convert.ToString(reader["DrawingType"].ToString());

                                if (reader["NotStandard"] != DBNull.Value)
                                    item.CPProduct.NotStandard = Convert.ToBoolean(reader["NotStandard"].ToString());

                                if (reader["cpCreatedBy"] != DBNull.Value)
                                    item.CPProduct.CreatedBy = Convert.ToInt32(reader["cpCreatedBy"].ToString());

                                if (reader["idmandatoryrevision"] != DBNull.Value)
                                    item.CPProduct.IdMandatoryRevision = Convert.ToInt32(reader["idmandatoryrevision"].ToString());
                                item.CPProduct.LstCPDetection = new List<CPDetection>();
                                item.CPProduct.LstCPDetection = OTM_GetDetectionRevisionItem_V2660(item, connectionString);

                                if (quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision) != null)
                                {
                                    if (quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items == null)
                                        quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items = new Dictionary<string, RevisionItem>();
                                    quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items.Add(item.IdRevisionItemImported.ToString(), item);
                                }

                                if (quo != null)
                                {
                                    if (quo.Ots == null)
                                        quo.Ots = new List<Ots>();

                                    if (!quo.Ots.Any(i => i.IdOT == Convert.ToInt64(reader["IdOT"])))
                                    {
                                        Ots ot = new Ots();
                                        ot.IdOT = Convert.ToInt64(reader["IdOT"].ToString());
                                        ot.Code = Convert.ToString(reader["Code"].ToString());
                                        ot.IdQuotation = quo.IdQuotation;
                                        if (reader["NumOT"] != DBNull.Value)
                                            ot.NumOT = Convert.ToByte(reader["NumOT"].ToString());
                                        if (reader["Comments"] != DBNull.Value)
                                            ot.Comments = Convert.ToString(reader["Comments"]);
                                        if (reader["Observations"] != DBNull.Value)
                                            ot.Observations = Convert.ToString(reader["Observations"]);
                                        if (reader["otsCreatedBy"] != DBNull.Value)
                                            ot.CreatedBy = Convert.ToInt32(reader["otsCreatedBy"]);
                                        if (reader["ReviewedBy"] != DBNull.Value)
                                            ot.ReviewedBy = Convert.ToInt32(reader["ReviewedBy"]);
                                        if (reader["otsCreationDate"] != DBNull.Value)
                                            ot.CreationDate = Convert.ToDateTime(reader["otsCreationDate"].ToString());
                                        if (reader["DeliveryDate"] != DBNull.Value)
                                            ot.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                        if (reader["DeliveryDate"] != DBNull.Value)
                                            ot.PrevDeliveryDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                        if (reader["ModifiedIn"] != DBNull.Value)
                                            ot.ModifiedIn = Convert.ToDateTime(reader["ModifiedIn"].ToString());
                                        if (reader["ModifiedBy"] != DBNull.Value)
                                            ot.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"].ToString());
                                        if (reader["IdShippingAddress"] != DBNull.Value)
                                            ot.IdShippingAddress = Convert.ToInt32(reader["IdShippingAddress"].ToString());
                                        if (reader["IdCarriageMethod"] != DBNull.Value)
                                            ot.IdCarriageMethod = Convert.ToByte(reader["IdCarriageMethod"].ToString());
                                        if (reader["IdSite"] != DBNull.Value)
                                            ot.IdSite = Convert.ToInt32(reader["IdSite"]);
                                        if (reader["AttachedFiles"] != DBNull.Value)
                                            ot.AttachedFiles = Convert.ToString(reader["AttachedFiles"]);
                                        if (reader["Code"] != DBNull.Value)
                                            ot.Code = Convert.ToString(reader["Code"]);
                                        if (reader["Year"] != DBNull.Value)
                                            ot.Year = Convert.ToInt32(reader["Year"]);
                                        if (reader["Number"] != DBNull.Value)
                                            ot.Number = Convert.ToInt32(reader["Number"]);

                                        quo.Ots.Add(ot);
                                    }

                                    var ot1 = quo.Ots.FirstOrDefault(i => i.IdOT == Convert.ToInt64(reader["IdOT"]));
                                    if (ot1 != null)
                                    {
                                        if (ot1.OtItems == null)
                                            ot1.OtItems = new List<OtItem>();
                                        OtItem otitem = new OtItem();
                                        otitem.IdOT = ot1.IdOT;
                                        if (reader["IdOTItem"] != DBNull.Value)
                                            otitem.IdOTItem = Convert.ToInt64(reader["IdOTItem"]);
                                        if (reader["IdRevisionItem"] != DBNull.Value)
                                            otitem.IdRevisionItemImported = Convert.ToInt64(reader["IdRevisionItem"]);
                                        if (reader["IdItemOtStatus"] != DBNull.Value)
                                            otitem.IdItemOtStatus = Convert.ToByte(11);
                                        if (reader["Rework"] != DBNull.Value)
                                            otitem.Rework = Convert.ToByte(reader["Rework"]);
                                        if (reader["CustomerComment"] != DBNull.Value)
                                            otitem.CustomerComment = Convert.ToString(reader["CustomerComment"]);
                                        if (reader["AttachedFiles"] != DBNull.Value)
                                            otitem.AttachedFiles = Convert.ToString(reader["AttachedFiles"]);
                                        if (reader["IsBatch"] != DBNull.Value)
                                            otitem.IsBatch = Convert.ToByte(reader["IsBatch"].ToString());
                                        if (reader["OiCreatedIn"] != DBNull.Value)
                                            otitem.CreatedIn = Convert.ToDateTime(reader["OiCreatedIn"]);
                                        if (reader["OiCreatedBy"] != DBNull.Value)
                                            otitem.CreatedBy = Convert.ToInt32(reader["OiCreatedBy"]);
                                        ot1.OtItems.Add(otitem);

                                        otitem.Counterparts = new List<Counterpart>();
                                        otitem.Counterparts = GetCounterpartByIdOtItem_V2660(connectionString, otitem.IdOTItem);

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error OTM_GetOTInformation_V2660(). ErrorMessage: {ex.Message}", category: Category.Exception, priority: Priority.High);
                throw;
            }
        }

        public void OTM_GetDetectionRevisionID_V2660(Quotation quo, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (Revision rev in quo.Revisions)
                    {
                        rev.Items = new Dictionary<string, RevisionItem>();

                        using (MySqlCommand cmd = new MySqlCommand("OTM_GetDetectionRevisionID_V2660", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("_idrevision", rev.Id);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    long idRevisionItem = Convert.ToInt64(reader["IdRevisionItem"]);
                                    string revisionItemKey = idRevisionItem.ToString();

                                    RevisionItem item;
                                    if (!rev.Items.TryGetValue(revisionItemKey, out item))
                                    {
                                        item = new RevisionItem
                                        {
                                            //IdRevisionItem = idRevisionItem,
                                            //CPProductID = Convert.ToInt64(reader["CPProductID"]),
                                            //IdProduct = Convert.ToInt64(reader["CPProductID"]),
                                            CPProduct = new CPProduct
                                            {
                                                LstCPDetection = new List<CPDetection>()
                                            }
                                        };

                                        rev.Items.Add(revisionItemKey, item);
                                    }

                                    // Populate CPDetection
                                    CPDetection detection = new CPDetection
                                    {
                                        CPProductID = Convert.ToInt64(reader["CPProductID"]),
                                        DetectionID = Convert.ToInt32(reader["DetectionID"]),
                                        NumDetections = Convert.ToInt32(reader["NumDetections"]),
                                        IdRevisionItem = Convert.ToInt64(reader["IdRevisionItem"]),
                                        EmdepComment = reader["EmdepComment"]?.ToString(),
                                        CustomerComment = reader["CustomerComment"]?.ToString(),
                                        AttachedFiles = reader["AttachedFiles"]?.ToString(),
                                        CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                                        HEC_DocumentID = Convert.ToUInt32(reader["HEC_DocumentID"])
                                    };
                                    item.CPProduct.LstCPDetection.Add(detection);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(
                    $"Error OTM_GetDetectionRevisionID_V2660(). ErrorMessage: {ex.Message}",
                    category: Category.Exception,
                    priority: Priority.High);
                throw;
            }
        }

        public Int64 InsertQuotations_V2660(Quotation quotation, string connectionString)
        {
            Int64 IdQuotation = 0;
            TransactionScope transactionScope = null;
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_quotations_Insert_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IdCustomer", quotation.IdCustomer);
                        conoffercommand.Parameters.AddWithValue("_Code", quotation.Code);
                        conoffercommand.Parameters.AddWithValue("_Year", quotation.Year);
                        conoffercommand.Parameters.AddWithValue("_Number", quotation.Number);
                        conoffercommand.Parameters.AddWithValue("_Description", quotation.Description);
                        conoffercommand.Parameters.AddWithValue("_IdDetectionsTemplate", quotation.IdDetectionsTemplate);
                        conoffercommand.Parameters.AddWithValue("_IdTechnicalTemplate", quotation.IdTechnicalTemplate);
                        conoffercommand.Parameters.AddWithValue("_ProjectName", quotation.ProjectName);
                        conoffercommand.Parameters.AddWithValue("_CreatedIn", quotation.CreatedIn);
                        conoffercommand.Parameters.AddWithValue("_ModifiedBy", quotation.ModifiedBy);
                        conoffercommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.Year);
                        conoffercommand.Parameters.AddWithValue("_IsCO", quotation.IsCO);
                        conoffercommand.Parameters.AddWithValue("_IdOffer", quotation.Offer.IdOffer);

                        IdQuotation = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        quotation.IdQuotation = IdQuotation;

                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                if (IdQuotation > 0)
                {
                    if (quotation.Revisions != null)
                    {
                        // foreach (var revision in quotation.Revisions)
                        // {
                        InsertRevision_0_ByApproved_V2660(quotation, connectionString);
                        InsertRevision_V2660(quotation, connectionString);
                        // }
                    }
                    if (quotation.Ots != null)
                    {
                        foreach (var ot in quotation.Ots)
                        {
                            ot.IdQuotation = quotation.IdQuotation;
                            InsertOT_V2660(ot, quotation, connectionString);
                            //if (ot.OtItems != null)
                            //{
                            //    foreach (var otItem in ot.OtItems)
                            //    {
                            //        try
                            //        {
                            //            if (otItem.Counterparts != null)
                            //            {
                            //                foreach (Counterpart cp in otItem.Counterparts)
                            //                {
                            //                    Int64 idCounterpart;
                            //                    if (quotation.IdDetectionsTemplate == 27)
                            //                        idCounterpart = InsertCounterparts_V2660(cp, otItem, connectionString);
                            //                    else
                            //                        idCounterpart = InsertCounterparts_V2660(cp, otItem, connectionString);
                            //                }
                            //            }
                            //        }
                            //        catch { }
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Error InsertQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IdQuotation;
        }
        public long CheckRevision0Exists_V2660(long idquotation, Revision rev, string connectionString)
        {
            long idRevision = 0;
            try
            {
                using (MySqlConnection c = new MySqlConnection(connectionString))
                {
                    c.Open();
                    try
                    {
                        MySqlCommand cmdtype = new MySqlCommand("OTM_CheckRevision0Exists", c);
                        cmdtype.CommandType = CommandType.StoredProcedure;
                        cmdtype.Parameters.AddWithValue("_IdQuotation", idquotation);
                        cmdtype.Parameters.AddWithValue("_NumRevision", 0);
                        using (MySqlDataReader reader = cmdtype.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["IdRevision"] != DBNull.Value)
                                {
                                    idRevision = Convert.ToUInt32(reader["IdRevision"]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error CheckRevision0Exists_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                    return idRevision;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error CheckRevision0Exists_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        // [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        public Int64 InsertMaterialRevision_0_ByApproved_V2660(Quotation q, string connectionString)
        {
            Int64 Idrevision = 0;
            foreach (Revision revision in q.Revisions)
            {
                try
                {
                    long isExists = CheckRevision0Exists_V2660(q.IdQuotation, revision, connectionString);
                    if (isExists == 0)
                    {
                        using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                        {
                            conoffer.Open();

                            try
                            {
                                MySqlCommand conoffercommand = new MySqlCommand("OTM_revisions_Insert_V2660", conoffer);
                                conoffercommand.CommandType = CommandType.StoredProcedure;
                                conoffercommand.Parameters.AddWithValue("_IdQuotation", q.IdQuotation);
                                conoffercommand.Parameters.AddWithValue("_NumRevision", 0);
                                conoffercommand.Parameters.AddWithValue("_MadeBy", revision.CreatedBy);
                                conoffercommand.Parameters.AddWithValue("_ReviewedBy", revision.ReviewedBy);
                                conoffercommand.Parameters.AddWithValue("_CreationDate", revision.CreatedIn);
                                conoffercommand.Parameters.AddWithValue("_Comments", revision.Comments);
                                conoffercommand.Parameters.AddWithValue("_SentTo", revision.SentToComments);
                                conoffercommand.Parameters.AddWithValue("_Closed", DateTime.Now);
                                conoffercommand.Parameters.AddWithValue("_Discount", revision.Discount);
                                conoffercommand.Parameters.AddWithValue("_ExpiryDate", revision.ExpireDate);
                                conoffercommand.Parameters.AddWithValue("_IdCurrency", revision.IdCurrency);
                                conoffercommand.Parameters.AddWithValue("_SentToClient", revision.SentToClient);
                                conoffercommand.Parameters.AddWithValue("_AttachedFiles", revision.AttachedFiles);

                                Idrevision = Convert.ToInt64(conoffercommand.ExecuteScalar());
                                revision.Id = Idrevision;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevision_0_ByApproved_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }

                            if (Idrevision > 0)
                            {
                                if (revision.Items != null)
                                {
                                    foreach (var revisionitem in revision.Items)
                                    {
                                        RevisionItem revitem = revisionitem.Value;
                                        CheckRevisionitemExists_V2660(revitem, connectionString);
                                        if (revitem.IdRevisionItem == 0)
                                        {
                                            revisionitem.Value.IdRevision = revision.Id;
                                            InsertMaterialRevisionItem_V2660(revisionitem.Value, connectionString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevision_0_ByApproved_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return Idrevision;
        }
        public Int64 InsertRevision_0_ByApproved_V2660(Quotation q, string connectionString)
        {
            Int64 Idrevision = 0;
            foreach (Revision revision in q.Revisions)
            {
                try
                {
                    long isExists = CheckRevision0Exists_V2660(q.IdQuotation, revision, connectionString);
                    if (isExists == 0)
                    {
                        using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                        {
                            conoffer.Open();

                            try
                            {
                                MySqlCommand conoffercommand = new MySqlCommand("OTM_revisions_Insert_V2660", conoffer);
                                conoffercommand.CommandType = CommandType.StoredProcedure;
                                conoffercommand.Parameters.AddWithValue("_IdQuotation", q.IdQuotation);
                                conoffercommand.Parameters.AddWithValue("_NumRevision", 0);
                                conoffercommand.Parameters.AddWithValue("_MadeBy", revision.CreatedBy);
                                conoffercommand.Parameters.AddWithValue("_ReviewedBy", revision.ReviewedBy);
                                conoffercommand.Parameters.AddWithValue("_CreationDate", revision.CreatedIn);
                                conoffercommand.Parameters.AddWithValue("_Comments", revision.Comments);
                                conoffercommand.Parameters.AddWithValue("_SentTo", revision.SentToComments);
                                conoffercommand.Parameters.AddWithValue("_Closed", DateTime.Now);
                                conoffercommand.Parameters.AddWithValue("_Discount", revision.Discount);
                                conoffercommand.Parameters.AddWithValue("_ExpiryDate", revision.ExpireDate);
                                conoffercommand.Parameters.AddWithValue("_IdCurrency", revision.IdCurrency);
                                conoffercommand.Parameters.AddWithValue("_SentToClient", revision.SentToClient);
                                conoffercommand.Parameters.AddWithValue("_AttachedFiles", revision.AttachedFiles);

                                Idrevision = Convert.ToInt64(conoffercommand.ExecuteScalar());
                                revision.Id = Idrevision;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error InsertRevision_0_ByApproved_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }

                            if (Idrevision > 0)
                            {
                                if (revision.Items != null)
                                {
                                    foreach (var revisionitem in revision.Items)
                                    {
                                        RevisionItem revitem = revisionitem.Value;
                                        revitem.IdRevision = revision.Id;
                                        CheckRevisionitemExists_V2660(revitem, connectionString);
                                        if (revitem.IdRevisionItem == 0)
                                        {
                                            InsertRevisionItem_V2660(revitem, connectionString);
                                            InsertCPDetections_V2660(revitem, connectionString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error InsertRevision_0_ByApproved_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return Idrevision;
        }
        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertRevision_V2660(Quotation q, string connectionString)
        {
            Int64 Idrevision = 0;
            foreach (Revision revision in q.Revisions)
            {
                try
                {
                    using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                    {
                        conoffer.Open();

                        try
                        {
                            MySqlCommand conoffercommand = new MySqlCommand("OTM_revisions_Insert_V2660", conoffer);
                            conoffercommand.CommandType = CommandType.StoredProcedure;
                            conoffercommand.Parameters.AddWithValue("_IdQuotation", q.IdQuotation);
                            conoffercommand.Parameters.AddWithValue("_NumRevision", revision.NumRevision);
                            conoffercommand.Parameters.AddWithValue("_MadeBy", revision.CreatedBy);
                            conoffercommand.Parameters.AddWithValue("_ReviewedBy", revision.ReviewedBy);
                            conoffercommand.Parameters.AddWithValue("_CreationDate", revision.CreatedIn);
                            conoffercommand.Parameters.AddWithValue("_Comments", revision.Comments);
                            conoffercommand.Parameters.AddWithValue("_SentTo", revision.SentToComments);
                            conoffercommand.Parameters.AddWithValue("_Closed", revision.Closed);
                            conoffercommand.Parameters.AddWithValue("_Discount", revision.Discount);
                            conoffercommand.Parameters.AddWithValue("_ExpiryDate", revision.ExpireDate);
                            conoffercommand.Parameters.AddWithValue("_IdCurrency", revision.IdCurrency);
                            conoffercommand.Parameters.AddWithValue("_SentToClient", revision.SentToClient);
                            conoffercommand.Parameters.AddWithValue("_AttachedFiles", revision.AttachedFiles);

                            Idrevision = Convert.ToInt64(conoffercommand.ExecuteScalar());
                            revision.Id = Idrevision;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error InsertQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }

                        if (Idrevision > 0)
                        {
                            if (revision.Items != null)
                            {
                                foreach (var revisionitem in revision.Items)
                                {
                                    RevisionItem revitem = revisionitem.Value;
                                    revitem.IdRevision = revision.Id;
                                    CheckRevisionitemExists_V2660(revitem, connectionString);
                                    if (revitem.IdRevisionItem == 0)
                                    {
                                        InsertRevisionItem_V2660(revitem, connectionString);
                                        InsertCPDetections_V2660(revitem, connectionString);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error InsertQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return Idrevision;
        }

        public void CheckRevisionitemExists_V2660(RevisionItem revisionitem, string connectionString)
        {
            try
            {
                using (MySqlConnection c = new MySqlConnection(connectionString))
                {
                    c.Open();
                    try
                    {
                        MySqlCommand cmdtype = new MySqlCommand("OTM_check_Revisionitem_exists_V2660", c);
                        cmdtype.CommandType = CommandType.StoredProcedure;
                        cmdtype.Parameters.AddWithValue("_IdRevision", revisionitem.IdRevision);
                        cmdtype.Parameters.AddWithValue("_NumItem", revisionitem.NumItem);
                        using (MySqlDataReader reader = cmdtype.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                {
                                    revisionitem.IdRevisionItem = Convert.ToInt32(reader["IdRevisionItem"]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertRevisionItem_V2660(RevisionItem revisionitem, string connectionString)
        {
            Int64 Idrevisionitem = 0;
            try
            {

                using (MySqlConnection c = new MySqlConnection(connectionString))
                {
                    c.Open();
                    try
                    {
                        MySqlCommand cmdtype = new MySqlCommand("OTM_InsertProductType_V2660", c);
                        cmdtype.CommandType = CommandType.StoredProcedure;
                        cmdtype.Parameters.AddWithValue("_typeId", 1);
                        revisionitem.IdProduct = Convert.ToInt64(cmdtype.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (revisionitem.CPProduct != null)
                {
                    InsertCpProduct_V2660(revisionitem, connectionString);
                }


                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_revisionitems_Insert_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IdRevision", revisionitem.IdRevision);
                        conoffercommand.Parameters.AddWithValue("_NumItem", revisionitem.NumItem);
                        conoffercommand.Parameters.AddWithValue("_IdProduct", revisionitem.IdProduct);
                        conoffercommand.Parameters.AddWithValue("_Quantity", revisionitem.Quantity);
                        conoffercommand.Parameters.AddWithValue("_UnitPrice", revisionitem.UnitPrice);
                        conoffercommand.Parameters.AddWithValue("_ManualPrice", revisionitem.ManualPrice);
                        conoffercommand.Parameters.AddWithValue("_Validated", revisionitem.Validated);
                        conoffercommand.Parameters.AddWithValue("_CustomerComment", revisionitem.CustomerComment);
                        conoffercommand.Parameters.AddWithValue("_InternalComment", revisionitem.InternalComment);
                        conoffercommand.Parameters.AddWithValue("_CreatedIn", revisionitem.CreatedIn);
                        conoffercommand.Parameters.AddWithValue("_CreatedBy", revisionitem.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_ModifiedIn", revisionitem.ModifiedIn);
                        conoffercommand.Parameters.AddWithValue("_ModifiedBy", revisionitem.ModifiedBy);
                        conoffercommand.Parameters.AddWithValue("_IdRevisionItemStatus", revisionitem.Status);
                        conoffercommand.Parameters.AddWithValue("_AttachedFiles", revisionitem.AttachedFiles);
                        conoffercommand.Parameters.AddWithValue("_IsCritical", revisionitem.IsCritical);

                        Idrevisionitem = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        revisionitem.IdRevisionItem = Idrevisionitem;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Idrevisionitem;
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public void InsertCpProduct_V2660(RevisionItem rev, string connectionString)
        {
            try
            {
                CPProduct CPProduct = rev.CPProduct;
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_AddCpProduct_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IdCP", rev.IdProduct);
                        conoffercommand.Parameters.AddWithValue("_Reference", CPProduct.Reference);
                        conoffercommand.Parameters.AddWithValue("_Samples", CPProduct.Samples);
                        conoffercommand.Parameters.AddWithValue("_OtherReference", CPProduct.OtherReference);
                        conoffercommand.Parameters.AddWithValue("_Ways", CPProduct.Ways);
                        conoffercommand.Parameters.AddWithValue("_IdDrawing", CPProduct.IdDrawing);
                        conoffercommand.Parameters.AddWithValue("_Zone", CPProduct.Zone);
                        conoffercommand.Parameters.AddWithValue("_IdCPType", CPProduct.IdCPType);
                        conoffercommand.Parameters.AddWithValue("_FullMatchWithDrawing", CPProduct.FullMatchWithDrawing);
                        conoffercommand.Parameters.AddWithValue("_SampleReceivedIn", CPProduct.SampleReceivedIn);
                        conoffercommand.Parameters.AddWithValue("_IdDrawingAssignedBy", CPProduct.IdDrawingAssignedBy);
                        conoffercommand.Parameters.AddWithValue("_DrawingType", CPProduct.DrawingType);
                        conoffercommand.Parameters.AddWithValue("_NotStandard", CPProduct.NotStandard);
                        conoffercommand.Parameters.AddWithValue("_CreatedBy", CPProduct.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_IdMandatoryRevision", CPProduct.IdMandatoryRevision);

                        conoffercommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertCpProduct_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertCpProduct_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertOT_V2660(Ots ot, Quotation q, string connectionString)
        {
            Int64 Idot = 0;
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_InsertOT_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_NumOT", ot.NumOT);
                        conoffercommand.Parameters.AddWithValue("_Comments", ot.Comments);
                        conoffercommand.Parameters.AddWithValue("_CreationDate", ot.CreationDate);
                        conoffercommand.Parameters.AddWithValue("_DeliveryDate", ot.DeliveryDate);
                        conoffercommand.Parameters.AddWithValue("_IdSite", ot.IdSite);
                        conoffercommand.Parameters.AddWithValue("_CreatedBy", ot.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_ReviewedBy", ot.ReviewedBy);
                        conoffercommand.Parameters.AddWithValue("_IdQuotation", ot.IdQuotation);
                        conoffercommand.Parameters.AddWithValue("_Code", ot.Code);
                        conoffercommand.Parameters.AddWithValue("_AttachedFiles", ot.AttachedFiles);
                        conoffercommand.Parameters.AddWithValue("_Year", ot.IdCurrency);
                        conoffercommand.Parameters.AddWithValue("_Number", ot.Number);
                        conoffercommand.Parameters.AddWithValue("_Observations", ot.Observations);
                        conoffercommand.Parameters.AddWithValue("_IdShippingAddress", ot.IdShippingAddress);
                        conoffercommand.Parameters.AddWithValue("_IdCarriageMethod", ot.IdCarriageMethod);

                        Idot = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        ot.IdOT = Idot;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertOT_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                var test = q.Revisions.SelectMany(i => i.Items);
                if (Idot > 0)
                {
                    if (ot.OtItems != null)
                    {
                        foreach (var otitem in ot.OtItems)
                        {
                            otitem.IdOT = Idot;
                            var revitem = test.FirstOrDefault(j => j.Value.IdRevisionItemImported == otitem.IdRevisionItemImported);
                            otitem.IdRevisionItem = revitem.Value.IdRevisionItem;
                            CheckOtItemExists_V2660(otitem, connectionString);
                            if (otitem.IdOTItem == 0)
                            {
                                InsertOtItem_V2660(otitem, connectionString);
                                if (otitem.Counterparts != null)
                                {
                                    foreach (Counterpart cp in otitem.Counterparts)
                                    {
                                        Int64 idCounterpart;
                                        if (q.IdDetectionsTemplate == 27)
                                            idCounterpart = InsertCounterparts_V2660(cp, otitem, connectionString);
                                        else
                                            idCounterpart = InsertCounterparts_V2660(cp, otitem, connectionString);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOT_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Idot;
        }
        public void CheckOtItemExists_V2660(OtItem otItem, string connectionString)
        {
            try
            {
                using (MySqlConnection c = new MySqlConnection(connectionString))
                {
                    c.Open();
                    try
                    {
                        MySqlCommand cmdtype = new MySqlCommand("OTM_check_OTItem_exists_V2660", c);
                        cmdtype.CommandType = CommandType.StoredProcedure;
                        cmdtype.Parameters.AddWithValue("_IDOT", otItem.IdOT);
                        cmdtype.Parameters.AddWithValue("_REVISIONITEM", otItem.IdRevisionItem);
                        using (MySqlDataReader reader = cmdtype.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["IdOTItem"] != DBNull.Value)
                                {
                                    otItem.IdOTItem = Convert.ToInt32(reader["IdOTItem"]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertOtItem_V2660(OtItem otitem, string connectionString)
        {
            Int64 idOTItem = 0;
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_Insert_OTItem_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IDOT", otitem.IdOT);
                        conoffercommand.Parameters.AddWithValue("_REVISIONITEM", otitem.IdRevisionItem);
                        conoffercommand.Parameters.AddWithValue("_STATUS", otitem.IdItemOtStatus);
                        conoffercommand.Parameters.AddWithValue("_REWORK", otitem.Rework);
                        conoffercommand.Parameters.AddWithValue("_CUSTOMERCOMMENT", otitem.CustomerComment);
                        conoffercommand.Parameters.AddWithValue("_ATTACHEDFILES", otitem.AttachedFiles);
                        conoffercommand.Parameters.AddWithValue("_ISBATCH", otitem.IsBatch);
                        conoffercommand.Parameters.AddWithValue("_CREATEDBY", otitem.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_CREATEDIN", otitem.CreatedIn);
                        conoffercommand.Parameters.AddWithValue("_MODIFIEDBY", otitem.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_MODIFIEDIN", otitem.ModifiedIn);

                        idOTItem = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        otitem.IdOTItem = idOTItem;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertOtItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOtItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return idOTItem;
        }

        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        public void InsertCPDetections_V2660(RevisionItem item, string connectionString)
        {
            TransactionScope transactionScope = null;
            List<CPDetection> detections = new List<CPDetection>();
            detections = item.CPProduct.LstCPDetection;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    try
                    {
                        foreach (var detection in detections)
                        {
                            using (MySqlCommand cmd = new MySqlCommand("OTM_AddCpDetection_V2660", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("_CPProductID", item.IdProduct);
                                cmd.Parameters.AddWithValue("_DetectionID", detection.DetectionID);
                                cmd.Parameters.AddWithValue("_NumDetections", detection.NumDetections);
                                cmd.Parameters.AddWithValue("_IdRevisionItem", item.IdRevisionItem);
                                cmd.Parameters.AddWithValue("_EmdepComment", detection.EmdepComment ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("_CustomerComment", detection.CustomerComment ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("_AttachedFiles", detection.AttachedFiles ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("_CreatedBy", detection.CreatedBy);
                                cmd.Parameters.AddWithValue("_HEC_DocumentID", detection.HEC_DocumentID);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Log4NetLogger.Logger.Log($"Error InsertCPDetections_V2660(). ErrorMessage: {ex.Message}", category: Category.Exception, priority: Priority.High);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertCPDetections_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Error InsertCPDetections_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public Int64 InsertCounterparts_V2660(Counterpart cp, OtItem otItem, string connectionString)
        {
            Int64 IdCounterpart = 0;
            try
            {
                using (MySqlConnection concounterpart = new MySqlConnection(connectionString))
                {
                    concounterpart.Open();
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("OTM_InsertCounterpart_V2660", concounterpart);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_IdOTItem", otItem.IdOTItem);
                        cmd.Parameters.AddWithValue("_Code", cp.Code);
                        cmd.Parameters.AddWithValue("_IdCounterpartType", cp.IdCounterpartType);
                        cmd.Parameters.AddWithValue("_Received", false);
                        cmd.Parameters.AddWithValue("_IsUrgent", cp.IsUrgent);
                        IdCounterpart = Convert.ToInt64(cmd.ExecuteScalar());
                        InsertCounterpartTracking_V2660(IdCounterpart, otItem, connectionString);
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertCounterparts_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertCounterparts_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IdCounterpart;
        }


        public void InsertCounterpartTracking_V2660(long IdCounterpart, OtItem otItem, string connectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("OTM_InsertCounterpartTracking_V2660", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_IdCounterpart", IdCounterpart);
                        cmd.Parameters.AddWithValue("_IdStage", 1);
                        cmd.Parameters.AddWithValue("_StartDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("_EndDate", DateTime.MinValue);
                        cmd.Parameters.AddWithValue("_IdOperator", otItem.CreatedBy);
                        cmd.Parameters.AddWithValue("_CurrentTime", 0);
                        cmd.Parameters.AddWithValue("_Rework", 0);
                        cmd.Parameters.AddWithValue("_Paused", 1);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error InsertCounterpartTracking_V2660(). ErrorMessage: {ex.Message}", category: Category.Exception, priority: Priority.High);
                throw;
            }
        }

        public List<Counterpart> GetCounterpartByIdOtItem_V2660(string connectionstring, Int64 idOtItem)
        {
            List<Counterpart> peoples = new List<Counterpart>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetCounterpartDetails_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idotitem", idOtItem);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Counterpart counter = new Counterpart();
                            if (reader["Code"] != DBNull.Value)
                                counter.Code = Convert.ToString(reader["Code"]);
                            if (reader["IdCounterpartType"] != DBNull.Value)
                                counter.IdCounterpartType = Convert.ToUInt64(reader["IdCounterpartType"]);
                            if (reader["Zone"] != DBNull.Value)
                                counter.Zone = Convert.ToString(reader["Zone"]);
                            if (reader["Received"] != DBNull.Value)
                                counter.Received = Convert.ToInt32(reader["Received"]);
                            if (reader["IsUrgent"] != DBNull.Value)
                                counter.IsUrgent = Convert.ToInt32(reader["IsUrgent"]);

                            peoples.Add(counter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCounterpartByIdOtItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return peoples;
        }

        public List<CPDetection> OTM_GetDetectionRevisionItem_V2660(RevisionItem revisionItem, string connectionString)
        {
            List<CPDetection> lstdet = new List<CPDetection>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand("OTM_GetDetectionRevisionItem_V2660", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_IdRevisionItem", revisionItem.IdRevisionItemImported);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long idRevisionItem = Convert.ToInt64(reader["IdRevisionItem"]);
                                string revisionItemKey = idRevisionItem.ToString();

                                // Populate CPDetection
                                CPDetection detection = new CPDetection();
                                if (reader["CPProductID"] != DBNull.Value)
                                    detection.CPProductID = Convert.ToInt64(reader["CPProductID"]);
                                if (reader["DetectionID"] != DBNull.Value)
                                    detection.DetectionID = Convert.ToInt32(reader["DetectionID"]);
                                if (reader["NumDetections"] != DBNull.Value)
                                    detection.NumDetections = Convert.ToInt32(reader["NumDetections"]);
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                    detection.IdRevisionItem = Convert.ToInt64(reader["IdRevisionItem"]);
                                if (reader["EmdepComment"] != DBNull.Value)
                                    detection.EmdepComment = reader["EmdepComment"]?.ToString();
                                if (reader["CustomerComment"] != DBNull.Value)
                                    detection.CustomerComment = reader["CustomerComment"]?.ToString();
                                if (reader["AttachedFiles"] != DBNull.Value)
                                    detection.AttachedFiles = reader["AttachedFiles"]?.ToString();
                                if (reader["CreatedBy"] != DBNull.Value)
                                    detection.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                if (reader["HEC_DocumentID"] != DBNull.Value)
                                    detection.HEC_DocumentID = Convert.ToUInt32(reader["HEC_DocumentID"]);

                                lstdet.Add(detection);


                            }
                        }
                    }
                }
                return lstdet;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetDetectionRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
                return null;
            }
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public void InsertOptionByOffer_V2660(List<OptionsByOffer> optionsByOfferList, string connectionString)
        {
            foreach (OptionsByOffer item in optionsByOfferList)
            {
                try
                {
                    using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                    {
                        conoffer.Open();

                        try
                        {
                            MySqlCommand conoffercommand = new MySqlCommand("OTM_optionsbyoffer_Insert_V2660", conoffer);
                            conoffercommand.CommandType = CommandType.StoredProcedure;
                            conoffercommand.Parameters.AddWithValue("_IdOption", item.IdOption);
                            conoffercommand.Parameters.AddWithValue("_IdOffer", item.IdOffer);
                            conoffercommand.Parameters.AddWithValue("_Quantity", item.Quantity);

                            conoffercommand.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error InsertOptionByOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public void Insertwarehouseproduct_V2660(RevisionItem RevItem, string connectionString)
        {
            //List<ArticleProduct> ArticlewarehouseProducts = new List<ArticleProduct>();
            ArticleProduct ArticlewarehouseProducts = new ArticleProduct();
            TransactionScope transactionScope = null;
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    //using (MySqlTransaction trans = conoffer.BeginTransaction())
                    //{
                    try
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_Insertwarehouseproduct_V2660", conoffer);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdWarehouseProduct", RevItem.IdProduct);
                        mySqlCommand.Parameters.AddWithValue("_IdArticle", RevItem.ArticlewarehouseProduct.Article.IdArticle);
                        mySqlCommand.Parameters.AddWithValue("_Description", RevItem.ArticlewarehouseProduct.Description);
                        mySqlCommand.Parameters.AddWithValue("_Parent", RevItem.ArticlewarehouseProduct.Parent);
                        mySqlCommand.Parameters.AddWithValue("_ParentMultiplier", RevItem.ArticlewarehouseProduct.ParentMultiplier);

                        //mySqlCommand.Transaction = trans;
                        RevItem.ArticlewarehouseProduct.IdProduct = Convert.ToUInt32(mySqlCommand.ExecuteScalar());
                        ArticlewarehouseProducts = RevItem.ArticlewarehouseProduct;
                        //trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        Log4NetLogger.Logger.Log(string.Format("Error Insertwarehouseproduct_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                    //}
                }
                if (ArticlewarehouseProducts.Components != null)
                {
                    foreach (WarehouseProductComponent component in ArticlewarehouseProducts.Components)
                    {
                        InsertWarehouseProductComponents(component, ArticlewarehouseProducts.IdProduct, 0, 1, connectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (Transaction.Current != null)
                //    Transaction.Current.Rollback();
                //if (transactionScope != null)
                //    transactionScope.Dispose();
                Log4NetLogger.Logger.Log(string.Format("Error Insertwarehouseproduct_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

        }
        //public void Insertwarehouseproduct_V2660(Revision revision, string connectionString)
        //{
        //    List<ArticleProduct> ArticlewarehouseProducts = new List<ArticleProduct>();
        //    TransactionScope transactionScope = null;
        //    try
        //    {
        //        foreach (KeyValuePair<string, RevisionItem> RevItem in revision.Items)
        //        {
        //            using (MySqlConnection conoffer = new MySqlConnection(connectionString))
        //            {
        //                conoffer.Open();
        //                using (MySqlTransaction trans = conoffer.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        UInt64 Id = InsertProduct(connectionString);
        //                        RevItem.Value.ArticlewarehouseProduct.IdProduct = Id;
        //                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_revisions_Insert_V2660", conoffer);
        //                        mySqlCommand.CommandType = CommandType.StoredProcedure;

        //                        mySqlCommand.Parameters.AddWithValue("_IdWarehouseProduct", RevItem.Value.ArticlewarehouseProduct.IdProduct);
        //                        mySqlCommand.Parameters.AddWithValue("_IdArticle", RevItem.Value.ArticlewarehouseProduct.Article.IdArticle);
        //                        mySqlCommand.Parameters.AddWithValue("_Description", RevItem.Value.ArticlewarehouseProduct.Description);
        //                        mySqlCommand.Parameters.AddWithValue("_Parent", RevItem.Value.ArticlewarehouseProduct.Parent);
        //                        mySqlCommand.Parameters.AddWithValue("_ParentMultiplier", RevItem.Value.ArticlewarehouseProduct.ParentMultiplier);


        //                        mySqlCommand.Transaction = trans;
        //                        RevItem.Value.ArticlewarehouseProduct.IdProduct = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

        //                        ArticlewarehouseProducts.Add(RevItem.Value.ArticlewarehouseProduct);
        //                        trans.Commit();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        trans.Rollback();
        //                        Log4NetLogger.Logger.Log(string.Format("Error Insertwarehouseproduct_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //                        throw;
        //                    }
        //                }
        //            }
        //        }

        //        foreach (ArticleProduct warehouseProduct in ArticlewarehouseProducts)
        //        {
        //            if (warehouseProduct.Components != null)
        //            {
        //                foreach (WarehouseProductComponent component in warehouseProduct.Components)
        //                {
        //                    InsertWarehouseProductComponents(component, warehouseProduct.IdProduct, 0, 1, connectionString);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Transaction.Current != null)
        //            Transaction.Current.Rollback();
        //        if (transactionScope != null)
        //            transactionScope.Dispose();
        //        throw;
        //    }

        //}

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public ulong InsertProduct(string connectionString)
        {
            UInt64 Id = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertProduct", con);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    Id = Convert.ToUInt64(mySqlCommand.ExecuteScalar());
                }
            }

            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertProduct(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Id;
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public void InsertWarehouseProductComponents(WarehouseProductComponent component, ulong idwarehouseProduct, ulong idWarehouseProductComponent, double quantity, string connectionString)
        {
            List<ArticleProduct> ArticlewarehouseProducts = new List<ArticleProduct>();
            TransactionScope transactionScope = null;
            try
            {

                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();

                    try
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertWarehouseProductComponents_V2660", conoffer);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdWarehouseProduct", idwarehouseProduct);
                        mySqlCommand.Parameters.AddWithValue("_IdArticle", component.Article.IdArticle);
                        mySqlCommand.Parameters.AddWithValue("_Quantity", component.Quantity);

                        if (idWarehouseProductComponent != 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_Parent", idwarehouseProduct);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_Parent", DBNull.Value);
                        }
                        mySqlCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertWarehouseProductComponents(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }

                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                throw;
            }
        }

        //public void GetWarehouseProductComponentsByRevisionItem(RevisionItem RevItem, string connectionString)
        //{
        //    try
        //    {
        //        RevItem.ArticlewarehouseProduct.Components = new List<WarehouseProductComponent>();
        //        List<Article> artList = GetArticlesByArticle(RevItem.ArticlewarehouseProduct.Article.IdArticle, connectionString);

        //        foreach (Article article in artList)
        //        {
        //            WarehouseProductComponent component = new WarehouseProductComponent();
        //            component.Article = new Article();
        //            component.ArticlewarehouseProduct = new ArticleProduct();
        //            component.ArticlewarehouseProduct.Components = new List<WarehouseProductComponent>();

        //            component.Article = article;
        //            component.Quantity = article.Quantity;

        //            component.Components = GetComponents(component, article, connectionString);

        //            RevItem.ArticlewarehouseProduct.Components.Add(component);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Log4NetLogger.Logger.Log(string.Format("Error GetWarehouseProductComponentsByRevisionItem(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        throw;
        //    }
        //}

        //public List<Article> GetArticlesByArticle(int idArticle, string connectionString)
        //{
        //    List<Article> list = new List<Article>();
        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(connectionString))
        //        {
        //            con.Open();
        //            MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetArticlesByIdArticle", con);
        //            mySqlCommand.CommandType = CommandType.StoredProcedure;

        //            mySqlCommand.Parameters.AddWithValue("_IdArticle", idArticle);
        //            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Article article = new Article();
        //                    article.Reference = reader["Reference"].ToString();
        //                    article.Description = reader["Description"].ToString();
        //                    article.Description_es = reader["Description_es"].ToString();
        //                    article.Description_fr = reader["Description_fr"].ToString();
        //                    article.Quantity = Convert.ToDouble(reader["Quantity"]);
        //                    article.IdArticle = Convert.ToInt32(reader["idcomponent"]);
        //                    list.Add(article);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4NetLogger.Logger.Log(string.Format("Error GetArticlesByArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        throw;
        //    }
        //    return list;
        //}

        //private List<WarehouseProductComponent> GetComponents(WarehouseProductComponent comp, Article art, string connectionString)
        //{
        //    List<Article> artList = GetArticlesByArticle(art.IdArticle, connectionString);
        //    List<WarehouseProductComponent> components = new List<WarehouseProductComponent>();

        //    foreach (Article article in artList)
        //    {
        //        WarehouseProductComponent component = new WarehouseProductComponent();
        //        component.Article = new Article();
        //        component.ArticlewarehouseProduct = new ArticleProduct();
        //        component.ArticlewarehouseProduct.Components = new List<WarehouseProductComponent>();

        //        component.Article = article;
        //        component.Quantity = article.Quantity;

        //        component.Components = GetComponents(component, article, connectionString);

        //        components.Add(component);
        //    }
        //    return components;

        //}

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public void InsertPartsNumber(Counterpart part, Revision rev, OtItem otitem, string connectionString, int IdSite)
        {
            int Id = 0;
            List<int> stages = new List<int>();
            try
            {
                var revItem = rev.Items.FirstOrDefault(x => x.Value.IdRevisionItemImported == otitem.IdRevisionItemImported);
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_InsertPartNumber_V2660", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_IdOTItem", otitem.IdOTItem);
                    MyCommand.Parameters.AddWithValue("_Code", part.Code);
                    MyCommand.Parameters.AddWithValue("_IdPartNumberType", 1);
                    Id = Convert.ToInt32(MyCommand.ExecuteScalar());
                }
                stages = GetStagesForPartsNumberTracking(revItem.Value.ArticlewarehouseProduct.Article.IdArticle, IdSite, connectionString);
                InserPartsNumberTracking(Id, stages, connectionString, otitem.CreatedBy);

            }

            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertPartsNumber(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public string GetBarCode(string quotationCode)
        {
            string barCode = String.Empty;

            if (quotationCode.Length == 8)
            {
                barCode = quotationCode.Substring(1, 1) + quotationCode.Substring(2, 2) + quotationCode.Substring(4, 4).PadLeft(5, '0');
            }
            if (quotationCode.Length == 9)
            {
                barCode = quotationCode.Substring(1, 1) + quotationCode.Substring(2, 2) + quotationCode.Substring(4, 5);
            }
            else if (quotationCode.Length == 10)
            {
                barCode = quotationCode.Substring(1, 1) + quotationCode.Substring(2, 2) + quotationCode.Substring(6, 4).PadLeft(5, '0');
            }
            else if (quotationCode.Length == 11)
            {
                barCode = quotationCode.Substring(1, 1) + quotationCode.Substring(2, 2) + quotationCode.Substring(6, 5);
            }
            return barCode;
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public List<int> GetStagesForPartsNumberTracking(int IdArticle, int site, string connectionString)
        {
            List<int> stages = new List<int>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetStagesByArticleAndSite", con);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", site);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stages.Add(Convert.ToInt32(reader["idStage"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetStagesForPartsNumberTracking(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return stages;
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public void InserPartsNumberTracking(int IdPartNumber, List<int> Stages, string connectionString, int CreatedBy)
        {
            try
            {
                foreach (int stage in Stages)
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertPartNumbersTracking", con);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdPartNumber", IdPartNumber);

                        if (stage == 1)
                        {
                            mySqlCommand.Parameters.AddWithValue("_EndDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Paused", false);
                            mySqlCommand.Parameters.AddWithValue("_IdOperator", CreatedBy);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_EndDate", null);
                            mySqlCommand.Parameters.AddWithValue("_Paused", true);
                            mySqlCommand.Parameters.AddWithValue("_IdOperator", null);
                        }

                        mySqlCommand.Parameters.AddWithValue("_StartDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IdStage", stage);
                        mySqlCommand.Parameters.AddWithValue("_CurrentTime", Convert.ToInt32(0));
                        mySqlCommand.Parameters.AddWithValue("_Rework", false);
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InserPartsNumberTracking(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[pooja.jadhav][GEOS2-9141][05.08.2025]
        public void OTM_GetMaterialOTInformation_V2660(Quotation quo, int numOT, string code, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("OTM_GetMaterialOTInformation_V2660", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_NumOT", numOT);
                        cmd.Parameters.AddWithValue("_Code", code);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (quo.Revisions == null)
                                    quo.Revisions = new List<Revision>();
                                RevisionItem item = new RevisionItem();

                                if (item.ArticlewarehouseProduct == null)
                                    item.ArticlewarehouseProduct = new ArticleProduct();
                                if (reader["IdRevision"] != DBNull.Value)
                                    item.IdRevision = Convert.ToInt64(reader["IdRevision"]);
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                    item.IdRevisionItemImported = Convert.ToInt64(reader["IdRevisionItem"]);
                                if (reader["IdRevisionItemStatus"] != DBNull.Value)
                                    item.Status = Convert.ToInt32(11);

                                if (reader["NumItem"] != DBNull.Value)
                                    item.NumItem = Convert.ToString(reader["NumItem"]);

                                if (reader["IdProduct"] != DBNull.Value)
                                    item.IdProduct = Convert.ToInt32(reader["IdProduct"]);

                                if (reader["Quantity"] != DBNull.Value)
                                    item.Quantity = Convert.ToInt32(reader["Quantity"]);

                                if (reader["UnitPrice"] != DBNull.Value)
                                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);

                                if (reader["ManualPrice"] != DBNull.Value)
                                    item.ManualPrice = Convert.ToBoolean(reader["ManualPrice"]);

                                if (reader["Validated"] != DBNull.Value)
                                    item.Validated = Convert.ToBoolean(reader["Validated"]);

                                item.CustomerComment = reader["CustomerComment"] != DBNull.Value ? reader["CustomerComment"].ToString() : null;

                                item.InternalComment = reader["InternalComment"] != DBNull.Value ? reader["InternalComment"].ToString() : null;

                                if (reader["Obsolete"] != DBNull.Value)
                                    item.Obsolete = Convert.ToInt16(reader["Obsolete"]);

                                if (reader["Marked"] != DBNull.Value)
                                    item.Marked = Convert.ToByte(reader["Marked"]);

                                if (reader["OiCreatedIn"] != DBNull.Value)
                                    item.CreatedIn = Convert.ToDateTime(reader["OiCreatedIn"]);

                                if (reader["OiCreatedBy"] != DBNull.Value)
                                    item.CreatedBy = Convert.ToInt32(reader["OiCreatedBy"]);

                                if (reader["OiModifiedBy"] != DBNull.Value)
                                    item.ModifiedBy = Convert.ToInt32(reader["OiModifiedBy"]);

                                if (reader["OiModifiedIn"] != DBNull.Value)
                                    item.ModifiedIn = Convert.ToDateTime(reader["OiModifiedIn"]);

                                item.AttachedFiles = reader["AttachedFiles"] != DBNull.Value ? reader["AttachedFiles"].ToString() : null;

                                if (reader["IsCritical"] != DBNull.Value)
                                    item.IsCritical = Convert.ToBoolean(reader["IsCritical"]);

                                if (reader["IdWarehouseProduct"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.IdProduct = Convert.ToUInt64(reader["IdWarehouseProduct"]);

                                if (reader["Description"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.Description = reader["Description"].ToString();

                                if (reader["Parent"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.Parent = Convert.ToUInt16(reader["Parent"]);

                                if (reader["ParentMultiplier"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.ParentMultiplier = Convert.ToUInt16(reader["ParentMultiplier"]);

                                if (item.ArticlewarehouseProduct.Article == null)
                                    item.ArticlewarehouseProduct.Article = new Article();

                                if (reader["IdArticle"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.Article.IdArticle = Convert.ToInt32(reader["IdArticle"]);

                                if (reader["Reference"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.Article.Reference = reader["Reference"].ToString();

                                if (reader["ArticleDescription"] != DBNull.Value)
                                    item.ArticlewarehouseProduct.Article.Description = reader["ArticleDescription"].ToString();

                                if (quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision) != null)
                                {
                                    if (quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items == null)
                                        quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items = new Dictionary<string, RevisionItem>();
                                    quo.Revisions.FirstOrDefault(p => p.Id == item.IdRevision).Items.Add(item.IdRevisionItemImported.ToString(), item);
                                }

                                if (quo != null)
                                {
                                    if (quo.Ots == null)
                                        quo.Ots = new List<Ots>();

                                    if (!quo.Ots.Any(i => i.IdOT == Convert.ToInt64(reader["IdOT"])))
                                    {
                                        Ots ot = new Ots();
                                        ot.IdOT = Convert.ToInt64(reader["IdOT"].ToString());
                                        ot.Code = Convert.ToString(reader["Code"].ToString());
                                        ot.IdQuotation = quo.IdQuotation;
                                        if (reader["NumOT"] != DBNull.Value)
                                            ot.NumOT = Convert.ToByte(reader["NumOT"].ToString());
                                        if (reader["Comments"] != DBNull.Value)
                                            ot.Comments = Convert.ToString(reader["Comments"]);
                                        if (reader["Observations"] != DBNull.Value)
                                            ot.Observations = Convert.ToString(reader["Observations"]);
                                        if (reader["otsCreatedBy"] != DBNull.Value)
                                            ot.CreatedBy = Convert.ToInt32(reader["otsCreatedBy"]);
                                        if (reader["ReviewedBy"] != DBNull.Value)
                                            ot.ReviewedBy = Convert.ToInt32(reader["ReviewedBy"]);
                                        if (reader["otsCreationDate"] != DBNull.Value)
                                            ot.CreationDate = Convert.ToDateTime(reader["otsCreationDate"].ToString());
                                        if (reader["DeliveryDate"] != DBNull.Value)
                                            ot.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                        if (reader["DeliveryDate"] != DBNull.Value)
                                            ot.PrevDeliveryDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                        if (reader["ModifiedIn"] != DBNull.Value)
                                            ot.ModifiedIn = Convert.ToDateTime(reader["ModifiedIn"].ToString());
                                        if (reader["ModifiedBy"] != DBNull.Value)
                                            ot.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"].ToString());
                                        if (reader["IdShippingAddress"] != DBNull.Value)
                                            ot.IdShippingAddress = Convert.ToInt32(reader["IdShippingAddress"].ToString());
                                        if (reader["IdCarriageMethod"] != DBNull.Value)
                                            ot.IdCarriageMethod = Convert.ToByte(reader["IdCarriageMethod"].ToString());
                                        if (reader["IdSite"] != DBNull.Value)
                                            ot.IdSite = Convert.ToInt32(reader["IdSite"]);
                                        if (reader["AttachedFiles"] != DBNull.Value)
                                            ot.AttachedFiles = Convert.ToString(reader["AttachedFiles"]);
                                        if (reader["Code"] != DBNull.Value)
                                            ot.Code = Convert.ToString(reader["Code"]);
                                        if (reader["Year"] != DBNull.Value)
                                            ot.Year = Convert.ToInt32(reader["Year"]);
                                        if (reader["Number"] != DBNull.Value)
                                            ot.Number = Convert.ToInt32(reader["Number"]);

                                        quo.Ots.Add(ot);
                                    }

                                    var ot1 = quo.Ots.FirstOrDefault(i => i.IdOT == Convert.ToInt64(reader["IdOT"]));
                                    if (ot1 != null)
                                    {
                                        if (ot1.OtItems == null)
                                            ot1.OtItems = new List<OtItem>();
                                        OtItem otitem = new OtItem();
                                        otitem.IdOT = ot1.IdOT;
                                        if (reader["IdOTItem"] != DBNull.Value)
                                            otitem.IdOTItem = Convert.ToInt64(reader["IdOTItem"]);
                                        if (reader["IdRevisionItem"] != DBNull.Value)
                                            otitem.IdRevisionItemImported = Convert.ToInt64(reader["IdRevisionItem"]);
                                        if (reader["IdItemOtStatus"] != DBNull.Value)
                                            otitem.IdItemOtStatus = Convert.ToByte(11);
                                        if (reader["Rework"] != DBNull.Value)
                                            otitem.Rework = Convert.ToByte(reader["Rework"]);
                                        if (reader["CustomerComment"] != DBNull.Value)
                                            otitem.CustomerComment = Convert.ToString(reader["CustomerComment"]);
                                        if (reader["AttachedFiles"] != DBNull.Value)
                                            otitem.AttachedFiles = Convert.ToString(reader["AttachedFiles"]);
                                        if (reader["IsBatch"] != DBNull.Value)
                                            otitem.IsBatch = Convert.ToByte(reader["IsBatch"].ToString());
                                        if (reader["OiCreatedIn"] != DBNull.Value)
                                            otitem.CreatedIn = Convert.ToDateTime(reader["OiCreatedIn"]);
                                        if (reader["OiCreatedBy"] != DBNull.Value)
                                            otitem.CreatedBy = Convert.ToInt32(reader["OiCreatedBy"]);
                                        ot1.OtItems.Add(otitem);
                                        otitem.Counterparts = new List<Counterpart>();
                                        otitem.Counterparts = GetPartnumbersByOtItem(otitem, connectionString);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error OTM_GetMaterialOTInformation_V2660(). ErrorMessage: {ex.Message}", category: Category.Exception, priority: Priority.High);
                throw;
            }
        }
        public List<Counterpart> GetPartnumbersByOtItem(OtItem otItem, string connectionString)
        {
            List<Counterpart> partNumbersList = new List<Counterpart>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPartNumbersByOtItem", con);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOtItem", otItem.IdOTItem);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Counterpart counterpart = new Counterpart();
                            if (reader["idPartNumber"] != DBNull.Value)
                                counterpart.IdCounterpart = Convert.ToUInt64(reader["idPartNumber"]);
                            if (reader["idOtItem"] != DBNull.Value)
                                counterpart.IdOTItem = Convert.ToInt64(reader["idOtItem"]);
                            if (reader["code"] != DBNull.Value)
                                counterpart.Code = reader["code"].ToString();
                            if (reader["idPartNumberType"] != DBNull.Value)
                                counterpart.IdCounterpartType = Convert.ToByte(reader["idPartNumberType"]);
                            partNumbersList.Add(counterpart);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPartnumbersByOtItem(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return partNumbersList;
        }

        public bool CreateFolderOffer_V2620(Offer offer, string connectionString, string workingOrdersPath, string workbenchConnectionString, bool? isNewFolderForOffer = null)
        {
            string basepath = "";
            string path = string.Empty;
            string year = string.Empty;
            //EmdepSite site = GetEmdepSiteById(Convert.ToInt32(559), connectionString);
            EmdepSite site = GetEmdepSiteById(Convert.ToInt32(offer.Site.ConnectPlantId), connectionString);

            if (offer.Year > 0)
            {
                year = Convert.ToString(offer.Year);
            }
            else
            {
                year = DateTime.Now.Year.ToString();
            }
            try
            {
                if (offer.IdOfferType == 1)
                {
                    basepath = GetCommercialOffersPath(connectionString);

                    basepath = basepath.Replace("{0}", site.FileServerIP);
                    basepath = basepath.Replace("{1}", site.ShortName);

                    if (!ExistsDirectory(basepath))
                    {
                        CreateDirectory(basepath);
                    }

                    basepath = System.IO.Path.Combine(basepath, year);
                    if (!ExistsDirectory(basepath))
                    {
                        CreateDirectory(basepath);
                    }

                    if (!ExistsDirectory(System.IO.Path.Combine(basepath, offer.Site.FullName)))
                    {
                        string newCustomerPath = System.IO.Path.Combine(basepath, offer.Site.FullName);
                        CreateDirectory(newCustomerPath);
                    }
                    basepath = System.IO.Path.Combine(basepath, offer.Site.FullName);
                    if (!ExistsDirectory(System.IO.Path.Combine(basepath, offer.Code)))
                    {
                        string newOfferPath = System.IO.Path.Combine(basepath, offer.Code);
                        CreateDirectory(newOfferPath);
                    }
                    basepath = basepath + "\\" + offer.Code;
                    if (isNewFolderForOffer.HasValue && isNewFolderForOffer.Value)
                    {
                        GeosAppSetting geosAppSetting = GetGeosAppSettings(6, workbenchConnectionString);
                        if (geosAppSetting != null && !string.IsNullOrEmpty(geosAppSetting.DefaultValue))
                        {
                            List<string> folders_offers = geosAppSetting.DefaultValue.Split(';').ToList();
                            foreach (string folder in folders_offers)
                            {
                                string docPath = System.IO.Path.Combine(basepath, folder.Trim());
                                if (!ExistsDirectory(docPath))
                                {
                                    CreateDirectory(docPath);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "DOC_TECNICA")))
                        {
                            string docPath = System.IO.Path.Combine(basepath, "DOC_TECNICA");
                            CreateDirectory(docPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "OFERTAS")))
                        {
                            string offerPath = System.IO.Path.Combine(basepath, "OFERTAS");
                            CreateDirectory(offerPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "OT")))
                        {
                            string otPath = System.IO.Path.Combine(basepath, "OT");
                            CreateDirectory(otPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "PLANOS")))
                        {
                            string drawingsPath = System.IO.Path.Combine(basepath, "PLANOS");
                            CreateDirectory(drawingsPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "TRACKINGS")))
                        {
                            string trackPath = System.IO.Path.Combine(basepath, "TRACKINGS");
                            CreateDirectory(trackPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "SHIPMENTS")))
                        {
                            string trackPath = System.IO.Path.Combine(basepath, "SHIPMENTS");
                            CreateDirectory(trackPath);
                        }
                        if (!ExistsDirectory(System.IO.Path.Combine(basepath, "PO")))
                        {
                            string trackPath = System.IO.Path.Combine(basepath, "PO");
                            CreateDirectory(trackPath);
                        }
                    }
                }
                basepath = System.IO.Path.Combine(workingOrdersPath, year);
                if (!ExistsDirectory(basepath))
                {
                    CreateDirectory(basepath);
                }
                basepath = System.IO.Path.Combine(basepath, offer.Code);
                if (!ExistsDirectory(basepath))
                {
                    CreateDirectory(basepath);
                }
                if (isNewFolderForOffer.HasValue && isNewFolderForOffer.Value)
                {
                    GeosAppSetting geosAppSetting1 = GetGeosAppSettings(45, workbenchConnectionString);
                    if (geosAppSetting1 != null && !string.IsNullOrEmpty(geosAppSetting1.DefaultValue))
                    {
                        List<string> folders_offers = geosAppSetting1.DefaultValue.Split(';').ToList();
                        foreach (string folder in folders_offers)
                        {
                            string Path = System.IO.Path.Combine(basepath, folder.Trim());
                            if (!ExistsDirectory(Path))
                            {
                                CreateDirectory(Path);
                            }
                        }
                    }
                }
                else
                {
                    path = System.IO.Path.Combine(basepath, "01 Project Specifications");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "02 Engineering Analysis");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "03 Production Documentation");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "04 Drawings");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "05 Backup");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "06 Pictures");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "07 QA Documents");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(basepath, "08 Official Documentation");
                    if (!ExistsDirectory(path))
                    {
                        CreateDirectory(path);
                    }
                }

                //  CopyAttachmentFolderFiles_V2140(offer, offerSiteId, workingOrdersPath);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error CreateFolderOffer_V2620(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private string GetCommercialOffersPath(string connectionString)
        {
            string path = string.Empty;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("CRM_GetCommercialOffersPath_V2620", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            path = reader["SettingValue"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error GetCommercialOffersPath(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return path;
        }

        public bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public EmdepSite GetEmdepSiteById(Int32 idSite, string connectionString)
        {
            //string query = @"SELECT em.IP,
            //                 em.idSite,
            //                 em.FileServerIP,
            //                 em.DatabaseIP,
            //                 si.ShortName
            //                 FROM emdepsites em INNER JOIN sites si ON em.idSite = si.IdSite
            //                 WHERE em.idsite = ?IDSITE";
            EmdepSite site = new EmdepSite();
            MySqlConnection myConnection;
            myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("emdepsites_GetemdepsitesDetailsById", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.AddWithValue("_IdSite", idSite);//Shubham[skadam] GEOS2-6488 Update MySQL database dll to support server version 8.0
            MySqlDataReader reader = myCommand.ExecuteReader();
            if (reader.Read())
            {
                site.IP = reader["IP"].ToString();
                site.IdSite = Convert.ToUInt32(reader["idSite"]);
                site.FileServerIP = Convert.ToString(reader["FileServerIP"]);
                site.DatabaseIP = Convert.ToString(reader["DatabaseIP"]);
                site.ShortName = reader["ShortName"].ToString();
            }
            reader.Close();
            myCommand.Dispose();
            myConnection.Close();
            return site;
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="quotation"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Int64 InsertMaterailQuotations_V2660(Quotation quotation, string connectionString)
        {
            Int64 IdQuotation = 0;
            int IdSite = Convert.ToInt32(quotation.Offer.Site.ConnectPlantId);
            TransactionScope transactionScope = null;
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_quotations_Insert_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IdCustomer", quotation.IdCustomer);
                        conoffercommand.Parameters.AddWithValue("_Code", quotation.Code);
                        conoffercommand.Parameters.AddWithValue("_Year", quotation.Year);
                        conoffercommand.Parameters.AddWithValue("_Number", quotation.Number);
                        conoffercommand.Parameters.AddWithValue("_Description", quotation.Description);
                        conoffercommand.Parameters.AddWithValue("_IdDetectionsTemplate", quotation.IdDetectionsTemplate);
                        conoffercommand.Parameters.AddWithValue("_IdTechnicalTemplate", quotation.IdTechnicalTemplate);
                        conoffercommand.Parameters.AddWithValue("_ProjectName", quotation.ProjectName);
                        conoffercommand.Parameters.AddWithValue("_CreatedIn", quotation.CreatedIn);
                        conoffercommand.Parameters.AddWithValue("_ModifiedBy", quotation.ModifiedBy);
                        conoffercommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.Year);
                        conoffercommand.Parameters.AddWithValue("_IsCO", quotation.IsCO);
                        conoffercommand.Parameters.AddWithValue("_IdOffer", quotation.Offer.IdOffer);

                        IdQuotation = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        quotation.IdQuotation = IdQuotation;
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        Log4NetLogger.Logger.Log(string.Format("Error InsertMaterailQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                if (IdQuotation > 0)
                {
                    if (quotation.Revisions != null)
                    {
                        //foreach (var revision in quotation.Revisions)
                        //{
                        InsertMaterialRevision_0_ByApproved_V2660(quotation, connectionString);
                        InsertMaterialRevision_V2660(quotation, connectionString);
                        //}
                    }
                    if (quotation.Ots != null)
                    {
                        foreach (var ot in quotation.Ots)
                        {
                            ot.IdQuotation = quotation.IdQuotation;
                            InsertMaterialOT_V2660(ot, quotation, connectionString);
                            //if (ot.OtItems != null)
                            //{
                            //    foreach (var otItem in ot.OtItems)
                            //    {
                            //        try
                            //        {
                            //            if (otItem.Counterparts != null)
                            //            {
                            //                foreach (Counterpart cp in otItem.Counterparts)
                            //                {
                            //                    InsertPartsNumber(cp, quotation.Revisions[0], otItem, connectionString, IdSite);
                            //                }
                            //            }
                            //        }
                            //        catch { }
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertMaterailQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                Log4NetLogger.Logger.Log(string.Format("Error InsertMaterailQuotations_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IdQuotation;
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="q"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Int64 InsertMaterialRevision_V2660(Quotation q, string connectionString)
        {
            Int64 Idrevision = 0;
            foreach (Revision revision in q.Revisions)
            {
                try
                {
                    using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                    {
                        conoffer.Open();

                        try
                        {
                            MySqlCommand conoffercommand = new MySqlCommand("OTM_revisions_Insert_V2660", conoffer);
                            conoffercommand.CommandType = CommandType.StoredProcedure;
                            conoffercommand.Parameters.AddWithValue("_IdQuotation", q.IdQuotation);
                            conoffercommand.Parameters.AddWithValue("_NumRevision", revision.NumRevision);
                            conoffercommand.Parameters.AddWithValue("_MadeBy", revision.CreatedBy);
                            conoffercommand.Parameters.AddWithValue("_ReviewedBy", revision.ReviewedBy);
                            conoffercommand.Parameters.AddWithValue("_CreationDate", revision.CreatedIn);
                            conoffercommand.Parameters.AddWithValue("_Comments", revision.Comments);
                            conoffercommand.Parameters.AddWithValue("_SentTo", revision.SentToComments);
                            conoffercommand.Parameters.AddWithValue("_Closed", revision.Closed);
                            conoffercommand.Parameters.AddWithValue("_Discount", revision.Discount);
                            conoffercommand.Parameters.AddWithValue("_ExpiryDate", revision.ExpireDate);
                            conoffercommand.Parameters.AddWithValue("_IdCurrency", revision.IdCurrency);
                            conoffercommand.Parameters.AddWithValue("_SentToClient", revision.SentToClient);
                            conoffercommand.Parameters.AddWithValue("_AttachedFiles", revision.AttachedFiles);

                            Idrevision = Convert.ToInt64(conoffercommand.ExecuteScalar());
                            revision.Id = Idrevision;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevision_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }

                        if (Idrevision > 0)
                        {
                            if (revision.Items != null)
                            {
                                foreach (var revisionitem in revision.Items)
                                {
                                    RevisionItem revitem = revisionitem.Value;
                                    revitem.IdRevision = revision.Id;
                                    CheckRevisionitemExists_V2660(revitem, connectionString);
                                    if (revitem.IdRevisionItem == 0)
                                    {
                                        revisionitem.Value.IdRevision = revision.Id;
                                        InsertMaterialRevisionItem_V2660(revisionitem.Value, connectionString);
                                    }
                                    //revisionitem.Value.IdRevision = revision.Id;
                                    //InsertMaterialRevisionItem_V2660(revisionitem.Value, connectionString);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevision_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return Idrevision;
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="revisionitem"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Int64 InsertMaterialRevisionItem_V2660(RevisionItem revisionitem, string connectionString)
        {
            Int64 Idrevisionitem = 0;
            try
            {
                revisionitem.IdProduct = (long)InsertProduct(connectionString);
                if (revisionitem.ArticlewarehouseProduct != null)
                {
                    Insertwarehouseproduct_V2660(revisionitem, connectionString);
                }
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_revisionitems_Insert_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_IdRevision", revisionitem.IdRevision);
                        conoffercommand.Parameters.AddWithValue("_NumItem", revisionitem.NumItem);
                        conoffercommand.Parameters.AddWithValue("_IdProduct", revisionitem.IdProduct);
                        conoffercommand.Parameters.AddWithValue("_Quantity", revisionitem.Quantity);
                        conoffercommand.Parameters.AddWithValue("_UnitPrice", revisionitem.UnitPrice);
                        conoffercommand.Parameters.AddWithValue("_ManualPrice", revisionitem.ManualPrice);
                        conoffercommand.Parameters.AddWithValue("_Validated", revisionitem.Validated);
                        conoffercommand.Parameters.AddWithValue("_CustomerComment", revisionitem.CustomerComment);
                        conoffercommand.Parameters.AddWithValue("_InternalComment", revisionitem.InternalComment);
                        conoffercommand.Parameters.AddWithValue("_CreatedIn", revisionitem.CreatedIn);
                        conoffercommand.Parameters.AddWithValue("_CreatedBy", revisionitem.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_ModifiedIn", revisionitem.ModifiedIn);
                        conoffercommand.Parameters.AddWithValue("_ModifiedBy", revisionitem.ModifiedBy);
                        conoffercommand.Parameters.AddWithValue("_IdRevisionItemStatus", revisionitem.Status);
                        conoffercommand.Parameters.AddWithValue("_AttachedFiles", revisionitem.AttachedFiles);
                        conoffercommand.Parameters.AddWithValue("_IsCritical", revisionitem.IsCritical);

                        Idrevisionitem = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        revisionitem.IdRevisionItem = Idrevisionitem;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertMaterialRevisionItem_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Idrevisionitem;
        }
        /// <summary>
        /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9167
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool UpdateOffer_V2660(LinkedOffers offer, string connectionString, List<Emailattachment> POAttachementsList)
        {
            bool IsSave = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    if (offer.IsNewPoForOffer == true && offer.IdStatus == 3)
                    {
                        MySqlCommand mySqlCommand1 = new MySqlCommand("OTM_UpdateOffer_V2660", mySqlConnection);
                        mySqlCommand1.CommandType = CommandType.StoredProcedure;
                        mySqlCommand1.Parameters.AddWithValue("_IdOffer", offer.IdOffer);
                        mySqlCommand1.Parameters.AddWithValue("_RFQ", offer.RFQ);

                        mySqlCommand1.Parameters.AddWithValue("_amount", offer.Amount);
                        mySqlCommand1.Parameters.AddWithValue("_idcurrency", offer.IdCurrency);
                        mySqlCommand1.Parameters.AddWithValue("_discount", offer.Discount);
                        mySqlCommand1.Parameters.AddWithValue("_IdShipTO", offer.IdShippingAddress);
                        mySqlCommand1.Parameters.AddWithValue("_IdcarriageMethod", offer.IdCarriageMethod);
                        mySqlCommand1.Parameters.AddWithValue("_IdStatus", offer.IdStatus);
                        mySqlCommand1.ExecuteScalar();
                    }
                    else
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateOffer_V2640", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOffer", offer.IdOffer);
                        mySqlCommand.Parameters.AddWithValue("_RFQ", offer.RFQ);
                        mySqlCommand.Parameters.AddWithValue("_amount", offer.Amount);
                        mySqlCommand.Parameters.AddWithValue("_idcurrency", offer.IdCurrency);
                        mySqlCommand.Parameters.AddWithValue("_discount", offer.Discount);
                        mySqlCommand.Parameters.AddWithValue("_IdShipTO", offer.IdShippingAddress);
                        mySqlCommand.Parameters.AddWithValue("_IdcarriageMethod", offer.IdCarriageMethod);
                        mySqlCommand.ExecuteScalar();

                    }

                    IsSave = true;
                    UpdateOfferContact(offer, connectionString);
                    UpdatePoRequestStatus_V2640(offer, connectionString, POAttachementsList);
                    AddLinkedofferByIdPORequest(offer, connectionString);
                    if (offer.DeletedLinkedofferlist != null)
                    {
                        DeleteLinkedofferByIdPORequest(offer, connectionString);
                    }
                    AddChangeLogByOffer(offer.OfferChangeLog, connectionString);

                    //UpdatePoRequestStatus_V2640(offer, connectionString);

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IsSave;
        }

        public void OTM_UpdateOfferStatus_V2660(PORegisteredDetails PO, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                {
                    con.Open();
                    MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateOfferStatus_V2660", con);
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.AddWithValue("_IdOffer", PO.IdOffer);
                    MyCommand.Parameters.AddWithValue("_IdStatus", PO.IdStatus);
                    MyCommand.ExecuteScalar();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_UpdateOfferStatus_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001][ashish.malkhede] OTM - Offers have to update when a PO is Linked https://helpdesk.emdep.com/browse/GEOS2-9194
        /// </summary>
        /// <param name="offers"></param>
        /// <param name="mainServerConnectionString"></param>
        public void OTM_UpdateLinkedOfferStatus_V2660(LinkedOffers offers, string mainServerConnectionString)
        {
            try
            {
                if (offers.IdStatus == 1 || offers.IdStatus == 2)
                {
                    using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateOfferStatus_V2660", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdOffer", offers.IdOffer);
                        MyCommand.Parameters.AddWithValue("_IdStatus", 3);
                        MyCommand.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_UpdateOfferStatus_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="ot"></param>
        /// <param name="q"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Int64 InsertMaterialOT_V2660(Ots ot, Quotation q, string connectionString)
        {
            Int64 Idot = 0;
            int IdSite = Convert.ToInt32(q.Offer.Site.ConnectPlantId);
            try
            {
                using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                {
                    conoffer.Open();
                    try
                    {
                        MySqlCommand conoffercommand = new MySqlCommand("OTM_InsertOT_V2660", conoffer);
                        conoffercommand.CommandType = CommandType.StoredProcedure;
                        conoffercommand.Parameters.AddWithValue("_NumOT", ot.NumOT);
                        conoffercommand.Parameters.AddWithValue("_Comments", ot.Comments);
                        conoffercommand.Parameters.AddWithValue("_CreationDate", ot.CreationDate);
                        conoffercommand.Parameters.AddWithValue("_DeliveryDate", ot.DeliveryDate);
                        conoffercommand.Parameters.AddWithValue("_IdSite", ot.IdSite);
                        conoffercommand.Parameters.AddWithValue("_CreatedBy", ot.CreatedBy);
                        conoffercommand.Parameters.AddWithValue("_ReviewedBy", ot.ReviewedBy);
                        conoffercommand.Parameters.AddWithValue("_IdQuotation", ot.IdQuotation);
                        conoffercommand.Parameters.AddWithValue("_Code", ot.Code);
                        conoffercommand.Parameters.AddWithValue("_AttachedFiles", ot.AttachedFiles);
                        conoffercommand.Parameters.AddWithValue("_Year", ot.IdCurrency);
                        conoffercommand.Parameters.AddWithValue("_Number", ot.Number);
                        conoffercommand.Parameters.AddWithValue("_Observations", ot.Observations);
                        conoffercommand.Parameters.AddWithValue("_IdShippingAddress", ot.IdShippingAddress);
                        conoffercommand.Parameters.AddWithValue("_IdCarriageMethod", ot.IdCarriageMethod);

                        Idot = Convert.ToInt64(conoffercommand.ExecuteScalar());
                        ot.IdOT = Idot;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error InsertOT_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
                var test = q.Revisions.SelectMany(i => i.Items);
                if (Idot > 0)
                {
                    if (ot.OtItems != null)
                    {
                        foreach (var otitem in ot.OtItems)
                        {
                            otitem.IdOT = Idot;
                            var revitem = test.FirstOrDefault(j => j.Value.IdRevisionItemImported == otitem.IdRevisionItemImported);
                            otitem.IdRevisionItem = revitem.Value.IdRevisionItem;
                            CheckOtItemExists_V2660(otitem, connectionString);
                            if (otitem.IdOTItem == 0)
                            {
                                InsertOtItem_V2660(otitem, connectionString);
                                if (otitem.Counterparts != null)
                                {
                                    foreach (Counterpart cp in otitem.Counterparts)
                                    {
                                        InsertPartsNumber(cp, q.Revisions[0], otitem, connectionString, IdSite);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertOT_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Idot;
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="connectionString"></param>
        public void InsertOfferContacts(Offer offer, string connectionString)
        {
            for (int i = 0; i < offer.OfferContacts.Count; i++)
            {
                InsertIntoOfferContacts(offer.IdOffer, offer.OfferContacts[i].IdContact, offer.OfferContacts[i].IsPrimaryOfferContact, connectionString);
            }
        }
        /// <summary>
        /// [001][ashish.malkhed][08/08/2025]  https://helpdesk.emdep.com/browse/GEOS2-9141
        /// </summary>
        /// <param name="idOffer"></param>
        /// <param name="idContact"></param>
        /// <param name="isPrimaryOfferContact"></param>
        /// <param name="connectionString"></param>
        public void InsertIntoOfferContacts(long idOffer, int idContact, byte? isPrimaryOfferContact, string connectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("OTM_InsertIntoOfferContacts_V2660", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_IdOffer", idOffer);
                        cmd.Parameters.AddWithValue("_IdContact", idContact);
                        cmd.Parameters.AddWithValue("_IsPrimaryOfferContact", isPrimaryOfferContact);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error InsertIntoOfferContacts. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9049
        /// </summary>
        /// <param name="ConnectionStringGeos"></param>
        /// <param name="idPORequest"></param>
        /// <param name="idOffer"></param>
        /// <returns></returns>
        public List<LogEntryByPORequest> GetAllPORequestChangeLog_V2660(string ConnectionStringGeos, long idPORequest, string idOffer)
        {
            List<LogEntryByPORequest> logList = new List<LogEntryByPORequest>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPORequestChangeLog_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idPORequest", idPORequest);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogEntryByPORequest poLog = new LogEntryByPORequest();
                            try
                            {
                                poLog.IdPORequest = Convert.ToInt64(reader["IdOffer"].ToString());
                                poLog.IdLogEntry = Convert.ToInt64(reader["idLogEntry"].ToString());
                                poLog.IdLogEntryType = Convert.ToByte(reader["idLogEntryType"].ToString());
                                poLog.IdUser = Convert.ToInt32(reader["idUser"].ToString());
                                poLog.People = new People { IdPerson = Convert.ToInt32(reader["idUser"].ToString()), Name = reader["Name"].ToString(), Surname = reader["Surname"].ToString() };
                                poLog.DateTime = Convert.ToDateTime(reader["DateTime"].ToString());
                                poLog.Comments = reader["comments"].ToString();
                                poLog.IsDeleted = false;
                                logList.Add(poLog);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetAllPORequestChangeLog_V2660. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllPORequestChangeLog_V2660. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return logList;
        }

        //[Rahul.gadhave][GEOS2-9191][Date:11-08-2025]
        public ObservableCollection<LookupValue> GetLookupValues_V2660(string ConnectionString)
        {
            ObservableCollection<LookupValue> LookupValuesList = new ObservableCollection<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestStatus_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);

                                if (empReader["Value"] != DBNull.Value)
                                    lookupval.Value = empReader["Value"].ToString();

                                if (empReader["HtmlColor"] != DBNull.Value)
                                    lookupval.HtmlColor = empReader["HtmlColor"].ToString();

                            }
                            catch (Exception ex)
                            { }
                            LookupValuesList.Add(lookupval);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLookupValues_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LookupValuesList;
        }

        //[rdixit][GEOS2-9137][12.08.2025]
        public void AddChangeLogByPORequest_V2660(ObservableCollection<LogEntryByPORequest> logEntries, string emailAttachmentPath, string connectionString)
        {
            try
            {
                if (logEntries != null)
                {
                    foreach (LogEntryByPORequest log in logEntries)
                    {
                        if (!string.IsNullOrEmpty(log.FileName))
                            log.Comments = log.Comments + "\nAttachment Path : " + Path.Combine(emailAttachmentPath, log.IdEmail.ToString(), log.FileName);

                        using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("OTM_logEntriesByPOOffer_V2640", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdOffer", log.IdPORequest);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", log.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_IsDateTime", log.DateTime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", log.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                            mySqlCommand.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddChangeLogByPORequest_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// [pooja.jadhav][GEOS2-9179][13-08-2025]
        /// </summary>
        /// <param name="IdCustomer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<Customer> GetAllCustomers_V2660(int IdCustomer, string connectionString)
        {
            List<Customer> RelatedCustomerList = GetRelatedCustomersByIdCustomer(IdCustomer, connectionString);
            string Customersids = string.Join(",", RelatedCustomerList.Select(c => c.IdCustomer));
            string Customers = IdCustomer.ToString() + "," + "10" + "," + Customersids;
            List<Customer> CustomerList = new List<Customer>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllCustomers_V2660", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdCustomers", Customers);
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Name"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Name"]);
                        cust.IsEnabled = true;
                        CustomerList.Add(cust);
                    }
                }
            }
            return CustomerList;
        }

        /// <summary>
        /// [pooja.jadhav][GEOS2-9179][13-08-2025]
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<Customer> GetRelatedCustomersByIdCustomer(int idCustomer, string connectionString)
        {

            List<Customer> RelatedCustomerList = new List<Customer>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetRelatedCustomersByCustomer_V2660", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdCustomer", idCustomer);
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer cust = new Customer();
                        if (reader["IdCustomer"] != DBNull.Value)
                            cust.IdCustomer = Convert.ToInt32(reader["IdCustomer"]);
                        if (reader["Name"] != DBNull.Value)
                            cust.CustomerName = Convert.ToString(reader["Name"]);

                        RelatedCustomerList.Add(cust);
                    }
                }
            }
            return RelatedCustomerList;
        }

        public List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2660(string connectionString, string OfferCode)
        {
            List<LinkedOffers> LinkedPoList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPoRequestLinkedPO_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_OfferCode", OfferCode);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkPO = new LinkedOffers();
                            if (mySqlDataReader["idCustomerPurchaseOrders"] != DBNull.Value)
                                linkPO.IdCustomerPurchaseOrder = Convert.ToInt32(mySqlDataReader["idCustomerPurchaseOrders"].ToString());
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                                linkPO.LinkedPO = mySqlDataReader["LinkedPO"].ToString();
                            if (mySqlDataReader["ReceivedIn"] != DBNull.Value)
                                linkPO.ReceivedIn = Convert.ToDateTime(mySqlDataReader["ReceivedIn"]);
                            if (mySqlDataReader["IdPOType"] != DBNull.Value)
                                linkPO.IdPOType = Convert.ToInt32(mySqlDataReader["IdPOType"]);
                            if (mySqlDataReader["POType"] != DBNull.Value)
                                linkPO.PoType = mySqlDataReader["POType"].ToString();
                            if (mySqlDataReader["OfferCode"] != DBNull.Value)
                                linkPO.Code = mySqlDataReader["OfferCode"].ToString();
                            if (mySqlDataReader["Conformation"] != DBNull.Value)
                                linkPO.Confirmation = mySqlDataReader["Conformation"].ToString();
                            if (linkPO.ReceivedIn == DateTime.MinValue)
                            {
                                linkPO.ReceivedIn = null;
                            }
                            LinkedPoList.Add(linkPO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPoRequestLinkedPO_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedPoList;
        }


        public List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2660(string connectionString, string emdep_geos_ConnectionString, Int64 IdEmail)
        {
            List<Emailattachment> EmailAttachementList = new List<Emailattachment>();
            List<LookupValue> AttachmentTypeList = GetLookupValues(emdep_geos_ConnectionString, 182).ToList();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailAttachementByIdEmail_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdEmail", IdEmail);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            Emailattachment emailattachment = new Emailattachment();
                            if (mySqlDataReader["IdAttachment"] != DBNull.Value)
                            {
                                emailattachment.IdAttachment = Convert.ToInt64(mySqlDataReader["IdAttachment"]);
                            }
                            if (mySqlDataReader["IdEmail"] != DBNull.Value)
                            {
                                emailattachment.IdEmail = Convert.ToInt64(mySqlDataReader["IdEmail"]);
                            }
                            if (mySqlDataReader["AttachmentName"] != DBNull.Value)
                            {
                                emailattachment.AttachmentName = Convert.ToString(mySqlDataReader["AttachmentName"]);
                            }
                            if (mySqlDataReader["AttachmentPath"] != DBNull.Value)
                            {
                                emailattachment.AttachmentPath = Convert.ToString(mySqlDataReader["AttachmentPath"]);
                            }
                            if (mySqlDataReader["AttachmentExtension"] != DBNull.Value)
                            {
                                emailattachment.AttachmentExtension = Convert.ToString(mySqlDataReader["AttachmentExtension"]);
                            }
                            if (mySqlDataReader["IdAttachmentType"] != DBNull.Value)
                            {
                                emailattachment.IdAttachementType = Convert.ToInt64(mySqlDataReader["IdAttachmentType"]);
                            }
                            emailattachment.AttachmentTypeList = new ObservableCollection<LookupValue>(AttachmentTypeList);
                            emailattachment.AttachmentTypeList.Insert(0, new LookupValue() { Value = "---" });
                            if (emailattachment.IdAttachementType > 0)
                            {
                                var matchedType = emailattachment.AttachmentTypeList.FirstOrDefault(i => i.IdLookupValue == emailattachment.IdAttachementType);
                                emailattachment.SelectedIndexAttachementType = emailattachment.AttachmentTypeList.IndexOf(matchedType);
                                emailattachment.Type = matchedType?.Value ?? "---"; // Set to "---" if not found
                            }
                            else
                            {
                                emailattachment.SelectedIndexAttachementType = 0;
                                emailattachment.Type = "---";
                            }
                            //if (emailattachment.AttachmentTypeList != null && emailattachment.SelectedIndexAttachementType >= 0 &&
                            //           emailattachment.SelectedIndexAttachementType < emailattachment.AttachmentTypeList.Count)
                            //{
                            //    emailattachment.IdAttachementType = emailattachment.AttachmentTypeList[emailattachment.SelectedIndexAttachementType].IdLookupValue;

                            //}
                            //else
                            //{
                            //    emailattachment.IdAttachementType = 0;
                            //}                           
                            emailattachment.FileDocInBytes = GetPoAttachmentByte(emailattachment.AttachmentPath);
                            EmailAttachementList.Add(emailattachment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddress_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailAttachementList;
        }
        public byte[] GetPoAttachmentByte(string attachmentPath)
        {
            byte[] bytes = null;
            try
            {
                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    if (File.Exists(attachmentPath))
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(attachmentPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            bytes = new byte[stream.Length];
                            int numBytesToRead = (int)stream.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;
                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPoAttachmentByte(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return bytes;
        }
        //[001][ashish.malkhed][19/08/2025] https://helpdesk.emdep.com/browse/GEOS2-9207
        public PORequestDetails GetPODetailsbyAttachment_V2660(string connectionString, int IdAttachment)
        {
            PORequestDetails PODetails = new PORequestDetails();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPODetailsbyAttachment_V2660", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idAttachment", IdAttachment);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["PONumber"] != DBNull.Value)
                                PODetails.PONumber = mySqlDataReader["PONumber"].ToString();

                            if (mySqlDataReader["PODate"] != DBNull.Value)
                                PODetails.POdate = Convert.ToDateTime(mySqlDataReader["PODate"]);

                            if (mySqlDataReader["Sender"] != DBNull.Value)
                                PODetails.Sender = mySqlDataReader["Sender"].ToString();

                            if (mySqlDataReader["TransferAmount"] != DBNull.Value)
                                PODetails.TransferAmount = Convert.ToDouble(mySqlDataReader["TransferAmount"]);
                            else
                                PODetails.TransferAmount = 0;

                            if (mySqlDataReader["Currency"] != DBNull.Value)
                                PODetails.Currency = mySqlDataReader["Currency"].ToString();

                            if (mySqlDataReader["ShipAddress"] != DBNull.Value)
                                PODetails.ShipTo = mySqlDataReader["ShipAddress"].ToString();

                            if (mySqlDataReader["IdAttachment"] != DBNull.Value)
                                PODetails.IdAttachment = mySqlDataReader["IdAttachment"].ToString();

                            if (mySqlDataReader["AttachmentName"] != DBNull.Value)
                                PODetails.Attachments = mySqlDataReader["AttachmentName"].ToString();


                        }

                        if (mySqlDataReader.NextResult())
                        {
                            if (mySqlDataReader.HasRows)
                            {
                                PODetails.IsPOExist = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPODetailsbyAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PODetails;
        }
        //[Rahul.Gadhave][GEOS2-9041][Date:02-09-2025] 
        public List<PORequestDetails> GetPORequestDetails_V2670(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2670";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {
                                    if (rdr["DateTime"] != DBNull.Value)
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();
                                    //[nsatpute][06-03-2025][GEOS2-6722]
                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                                    if (rdr["IdCustomerGroup"] != DBNull.Value)
                                        po.IdCustomerGroup = Convert.ToInt32(rdr["IdCustomerGroup"]);
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();
                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();
                                    //[pramod.misal][GEOS2-7248][22.04.2025]
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);
                                    //if (rdr["IdAttachmentType"] != DBNull.Value)
                                    //    po.IdAttachementType = Convert.ToInt32(rdr["IdAttachmentType"]);
                                    //if (rdr["IdAttachment"] != DBNull.Value)
                                    //    po.IdAttachment = Convert.ToString(rdr["IdAttachment"]);
                                    if (rdr["idcountry"] != DBNull.Value)
                                        po.IdCountry = Convert.ToInt64(rdr["idcountry"]);
                                    poList.Add(po);

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2670(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);

                                        if (rdr["offerIdcustomerPlant"] != DBNull.Value)
                                            o.IdCustomerPlant = Convert.ToInt32(rdr["offerIdcustomerPlant"]);

                                        if (rdr["offerIdcustomer"] != DBNull.Value)
                                            o.IdCustomerGroup = Convert.ToInt32(rdr["offerIdcustomer"]);

                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }
        public string InsertFileOnPath_V6(PORegisteredDetails offerslink, string mainServerConnectionString, string CommericalPath)
        {
            string CompleteFilePath = string.Empty;
            foreach (LinkedOffers lo in offerslink.offersLinked)
            {

                try
                {
                    if (lo.CommericalAttachementsDocInBytes != null)
                    {
                        //string completePath = string.Format(@"{0}\{1}", ConnectorAttachedDocPath, attachment.Reference);
                        string completePath = Path.Combine($"{CommericalPath} {lo.Year}", $"{lo.CustomerGroup} - {lo.Name}", lo.Code, "03 - PO");
                        string filePath = completePath + "\\" + lo.AttachmentFileName;
                        CompleteFilePath = string.Join(",", filePath);
                        Log4NetLogger.Logger.Log(string.Format($"File Path - {CompleteFilePath}"), category: Category.Exception, priority: Priority.Low);
                        try
                        {
                            if (!Directory.Exists(completePath))
                            {
                                Directory.CreateDirectory(completePath);
                            }
                            else
                            {
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                // Delete all files in the directory except the target file
                                foreach (string file in Directory.GetFiles(completePath))
                                {
                                    if (!file.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }
                            File.WriteAllBytes(filePath, lo.CommericalAttachementsDocInBytes);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error InsertConnectorAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                        //return true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error DeleteConnectorAttachedDoc()- Filename - {0}. ErrorMessage- {1}", lo.AttachmentFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
                // return CompletePath;
            }
            return CompleteFilePath;
        }

        /// <summary>
        /// //[pramod.misal][04-09-2025][GEOS2-9041] https://helpdesk.emdep.com/browse/GEOS2-9041
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool UpdateOffer_V2770(Int64 idEmail, LinkedOffers offer, string connectionString, List<Emailattachment> POAttachementsList, Int64 IdCustomerGroup, Int64 IdCustomerPlant)
        {
            bool IsSave = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_UpdateOffer_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOffer", offer.IdOffer);
                    mySqlCommand.Parameters.AddWithValue("_RFQ", offer.RFQ);
                    mySqlCommand.Parameters.AddWithValue("_amount", offer.Amount);
                    mySqlCommand.Parameters.AddWithValue("_idcurrency", offer.IdCurrency);
                    mySqlCommand.Parameters.AddWithValue("_discount", offer.Discount);
                    mySqlCommand.Parameters.AddWithValue("_IdShipTO", offer.IdShippingAddress);
                    mySqlCommand.Parameters.AddWithValue("_IdcarriageMethod", offer.IdCarriageMethod);
                    mySqlCommand.ExecuteScalar();
                    IsSave = true;
                    UpdateOfferContact(offer, connectionString);
                    UpdatePoRequestStatus_V2640(offer, connectionString, POAttachementsList);
                    AddLinkedofferByIdPORequest(offer, connectionString);
                    if (offer.DeletedLinkedofferlist != null)
                    {
                        DeleteLinkedofferByIdPORequest(offer, connectionString);
                    }
                    AddChangeLogByOffer(offer.OfferChangeLog, connectionString);
                    UpdatePORequestGroupPlant_V2670(idEmail, connectionString, IdCustomerGroup, IdCustomerPlant); //[pramod.misal][04-09-2025][GEOS2-9041] https://helpdesk.emdep.com/browse/GEOS2-9041
                                                                                                                  //UpdatePoRequestStatus_V2640(offer, connectionString);

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOffer_V2770(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IsSave;
        }

        //[pramod.misal][04-09-2025][GEOS2-9041] https://helpdesk.emdep.com/browse/GEOS2-9041
        public bool UpdatePORequestGroupPlant_V2670(Int64 idEmail, string connectionString, Int64 IdCustomerGroup, Int64 IdCustomerPlant)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("UpdatePORequestGroupPlant_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", idEmail);
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomerGroup);
                    mySqlCommand.Parameters.AddWithValue("_IdPlant", IdCustomerPlant);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePORequestGroupPlant_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return true;
        }

        //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
        public string GetTimezoneByServiceUrl_V2670(string emdep_geos_ConnectionString, string serviceProviderUrl)
        {
            string TimeZoneIdentifier = string.Empty;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(emdep_geos_ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetTimezoneByServiceUrl_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_ServiceProviderUrl", serviceProviderUrl);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["TimeZoneIdentifier"] != DBNull.Value)
                            {
                                TimeZoneIdentifier = Convert.ToString(mySqlDataReader["TimeZoneIdentifier"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTimezoneByServiceUrl_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return TimeZoneIdentifier;
        }
        public List<POType> OTM_GetAllPOTypeStatus_V2670(string connectionString)
        {
            List<POType> POTypeList = new List<POType>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllPOType_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            POType potype = new POType();
                            if (mySqlDataReader["IdPOType"] != DBNull.Value)
                                potype.IdPoType = Convert.ToInt32(mySqlDataReader["IdPOType"].ToString());
                            if (mySqlDataReader["POType"] != DBNull.Value)
                                potype.Type = mySqlDataReader["POType"].ToString();
                            if (mySqlDataReader["Abbreviation"] != DBNull.Value)
                                potype.Abbreviation = mySqlDataReader["Abbreviation"].ToString();
                            POTypeList.Add(potype);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetAllPOTypeStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POTypeList;
        }

        /// <summary>
        /// [pooja.jadhav][04-09-2025][GEOS2-9322] OTM - Limit the Sender list in Edit PO
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<PORegisteredDetails> OTM_GetPOSender_V2670(string connectionString)
        {
            List<PORegisteredDetails> POSenderList = new List<PORegisteredDetails>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPOSender_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            PORegisteredDetails Sender = new PORegisteredDetails();
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                Sender.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["FullName"] != DBNull.Value)
                            {
                                Sender.FullName = Convert.ToString(mySqlDataReader["FullName"]);
                            }
                            if (mySqlDataReader["Surname"] != DBNull.Value)
                            {
                                Sender.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                            }
                            if (mySqlDataReader["IdPersonGender"] != DBNull.Value)
                            {
                                Sender.IdGender = Convert.ToInt16(mySqlDataReader["IdPersonGender"]);
                            }
                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                Sender.IdPerson = Convert.ToInt16(mySqlDataReader["IdPerson"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                Sender.IdCustomer = Convert.ToInt16(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["SiteName"] != DBNull.Value)
                            {
                                Sender.SiteName = Convert.ToString(mySqlDataReader["SiteName"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                Sender.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            POSenderList.Add(Sender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetPOSender_V2670. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return POSenderList;
        }
        /// <summary>
        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public List<POEmployeeInfo> GetJob_DescriptionsList_V2670(string ConnectionString)
        {
            List<POEmployeeInfo> Employees = new List<POEmployeeInfo>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAlljob_descriptions_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            POEmployeeInfo employee = new POEmployeeInfo();
                            if (reader["IdJobDescription"] != DBNull.Value)
                                employee.IdJobDescription = Convert.ToInt64(reader["IdJobDescription"].ToString());
                            if (reader["JobDescriptionTitle"] != DBNull.Value)
                                employee.JobDescriptionTitle = Convert.ToString(reader["JobDescriptionTitle"].ToString());
                            Employees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetJob_DescriptionsList_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Employees;
        }
        //[Rahul.Gadhave][GEOS2-9437][10 - 09 - 2025]
        public bool ChectCustomerpurchaseOrderExist_V2670(PORegisteredDetails po, string connectionString)
        {
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("OTM_GetPOCodeExistsOrNot_V2670", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_PoCode", po.Code);
                    var result = cmd.ExecuteScalar();
                    bool exists = false;
                    if (result != null)
                    {
                        exists = true;
                    }
                    return exists;
                }
            }
        }

        public List<POEmployeeInfo> GetAddedJob_DescriptionsList_V2670(string ConnectionString)
        {
            List<POEmployeeInfo> Employees = new List<POEmployeeInfo>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetAllAddedjob_descriptions_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            POEmployeeInfo employee = new POEmployeeInfo();
                            if (reader["IdJobDescription"] != DBNull.Value)
                                employee.IdJobDescription = Convert.ToInt64(reader["IdJobDescription"].ToString());
                            if (reader["JobDescriptionTitle"] != DBNull.Value)
                                employee.JobDescriptionTitle = Convert.ToString(reader["JobDescriptionTitle"].ToString());
                            Employees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAddedJob_DescriptionsList_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return Employees;
        }
        public bool UpdateConfirmationJDSetting_V2670(List<POEmployeeInfo> PO, string mainServerConnectionString)
        {
            try
            {
                string jobDescriptionIds = string.Join(",", PO.Select(p => p.IdJobDescription));
                using (MySqlConnection con = new MySqlConnection(mainServerConnectionString))
                    {
                        con.Open();
                        MySqlCommand MyCommand = new MySqlCommand("OTM_UpdateJobDescriptions_V2670", con);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("_IdJobDescription", jobDescriptionIds);
                        MyCommand.ExecuteNonQuery();
                        con.Close();
                    }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        //[Rahul.Gadhave][GEOS2-9080][Date:30-07-2025]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2670(string ConnectionString, string SelectedIdPlant, string SelectedIdGroup, string correnciesIconFilePath, GeosAppSetting geosAppSetting, string CommericalPath)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            List<string> currencyISOs = new List<string>();
            string AppSetting = "";
            if (geosAppSetting != null)
            {
                AppSetting = geosAppSetting.DefaultValue;
            }
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetLinkedofferByIdPlantAndGroup_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPlant", SelectedIdPlant);
                    mySqlCommand.Parameters.AddWithValue("_IdGroup", SelectedIdGroup);
                    mySqlCommand.Parameters.AddWithValue("_geosAppSetting", AppSetting);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LinkedOffers linkedOffers = new LinkedOffers();
                            if (mySqlDataReader["Code"] != DBNull.Value)
                            {
                                linkedOffers.Code = Convert.ToString(mySqlDataReader["Code"]);
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                linkedOffers.Description = Convert.ToString(mySqlDataReader["Description"]);
                            }
                            if (mySqlDataReader["Rfq"] != DBNull.Value)
                            {
                                linkedOffers.RFQ = Convert.ToString(mySqlDataReader["Rfq"]);
                            }
                            if (mySqlDataReader["Contact"] != DBNull.Value)
                            {
                                linkedOffers.Contact = Convert.ToString(mySqlDataReader["Contact"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            //if (mySqlDataReader["CustomerName"] != DBNull.Value)
                            //{
                            //    linkedOffers.CutomerName = Convert.ToString(mySqlDataReader["CustomerName"]);
                            //}
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                linkedOffers.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            if (mySqlDataReader["OfferStatus"] != DBNull.Value)
                            {
                                linkedOffers.Status = Convert.ToString(mySqlDataReader["OfferStatus"]);
                            }
                            if (mySqlDataReader["IdStatus"] != DBNull.Value)
                            {
                                linkedOffers.IdStatus = Convert.ToInt32(mySqlDataReader["IdStatus"]);
                            }
                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                            {
                                linkedOffers.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                            }
                            if (mySqlDataReader["discount"] != DBNull.Value)
                            {
                                linkedOffers.Discount = Convert.ToDouble(mySqlDataReader["discount"]);
                            }
                            if (mySqlDataReader["Amount"] != DBNull.Value)
                            {
                                linkedOffers.Amount = Convert.ToDouble(mySqlDataReader["Amount"]);
                            }
                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                linkedOffers.IdCurrency = Convert.ToInt32(mySqlDataReader["IdCurrency"]);
                            }
                            if (mySqlDataReader["OfferCurrency"] != DBNull.Value)
                            {
                                linkedOffers.OfferCurrency = Convert.ToString(mySqlDataReader["OfferCurrency"]);
                            }
                            if (mySqlDataReader["Currency"] != DBNull.Value)
                            {
                                linkedOffers.Currency = Convert.ToString(mySqlDataReader["Currency"]);
                            }
                            if (mySqlDataReader["IdOffer"] != DBNull.Value)
                            {
                                linkedOffers.IdOffer = Convert.ToInt64(mySqlDataReader["IdOffer"]);
                            }
                            if (mySqlDataReader["Year"] != DBNull.Value)
                            {
                                linkedOffers.Year = Convert.ToString(mySqlDataReader["Year"]);
                            }
                            if (mySqlDataReader["CustomerGroup"] != DBNull.Value)
                            {
                                linkedOffers.CustomerGroup = Convert.ToString(mySqlDataReader["CustomerGroup"]);
                            }
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                linkedOffers.IdCustomer = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                linkedOffers.Plant = Convert.ToString(mySqlDataReader["Plant"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdPlant = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                linkedOffers.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["idOfferType"] != DBNull.Value)
                            {
                                linkedOffers.IdOfferType = Convert.ToInt32(mySqlDataReader["idOfferType"]);
                            }
                            if (mySqlDataReader["offersType"] != DBNull.Value)
                            {
                                linkedOffers.OffersType = Convert.ToString(mySqlDataReader["offersType"]);
                            }
                            if (mySqlDataReader["LinkedPO"] != DBNull.Value)
                            {
                                linkedOffers.LinkedPO = Convert.ToString(mySqlDataReader["LinkedPO"]);
                            }
                            if (linkedOffers.Currency != null)
                            {
                                if (!currencyISOs.Any(co => co.ToString() == linkedOffers.Currency))
                                {
                                    currencyISOs.Add(linkedOffers.Currency);
                                }
                            }
                            if (mySqlDataReader["AttachmentFileName"] != DBNull.Value)
                            {
                                linkedOffers.AttachmentFileName = Convert.ToString(mySqlDataReader["AttachmentFileName"]);
                                //linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc(CommericalPath, linkedOffers);
                                linkedOffers.CommericalAttachementsDocInBytes = CommericalAttachedDoc_V2670(CommericalPath, linkedOffers);

                            }
                            //[pramod.misal][04-08-2025]                           
                            if (mySqlDataReader["Country"] != DBNull.Value)
                            {
                                linkedOffers.Country = Convert.ToString(mySqlDataReader["Country"].ToString());
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                linkedOffers.Iso = Convert.ToString(mySqlDataReader["iso"]);
                                linkedOffers.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + linkedOffers.Iso + ".png";
                            }
                            LinkedOffersList.Add(linkedOffers);
                        }
                        foreach (string item in currencyISOs)
                        {
                            byte[] bytes = null;
                            bytes = GetCountryIconFileInBytes(item, correnciesIconFilePath);
                            LinkedOffersList.Where(ot => ot.Currency == item).ToList().ForEach(ot => ot.CurrencyIconBytes = bytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetLinkedofferByIdPlantAndGroup_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }

        //[pramod.misal][24.09.2025][GEOS2-9576]https://helpdesk.emdep.com/browse/GEOS2-9576
        public Int64 InsertOffer_V2670(Quotation q, string connectionString, string WorkingOrdersPath, string workbenchConnectionString)
        {
            Int64 offerId = 0;
            TransactionScope transactionScope = null;
            Offer offer = q.Offer;
            try
            {
                using (transactionScope = new System.Transactions.TransactionScope())
                {
                    //Transaction.Current.
                    using (MySqlConnection conoffer = new MySqlConnection(connectionString))
                    {
                        conoffer.Open();
                        try
                        {

                            MySqlCommand conoffercommand = new MySqlCommand("OTM_InsertOffer_V2670", conoffer);
                            conoffercommand.CommandType = CommandType.StoredProcedure;
                            conoffercommand.Parameters.AddWithValue("_Code", offer.Code);
                            conoffercommand.Parameters.AddWithValue("_IdOfferType", offer.IdOfferType);
                            conoffercommand.Parameters.AddWithValue("_IdCustomer", offer.IdCustomer);
                            conoffercommand.Parameters.AddWithValue("_IdProject", offer.IdProject);
                            conoffercommand.Parameters.AddWithValue("_Description", offer.Description);
                            conoffercommand.Parameters.AddWithValue("_IdStatus", offer.IdStatus);
                            conoffercommand.Parameters.AddWithValue("_Amount", offer.Value);
                            conoffercommand.Parameters.AddWithValue("_ProbabilityOfSuccess", offer.ProbabilityOfSuccess);
                            conoffercommand.Parameters.AddWithValue("_OfferExpectedDate", offer.OfferExpectedDate);
                            conoffercommand.Parameters.AddWithValue("_IdCurrency", offer.IdCurrency);
                            conoffercommand.Parameters.AddWithValue("_Year", DateTime.Now.Year);
                            conoffercommand.Parameters.AddWithValue("_Number", offer.Number);
                            conoffercommand.Parameters.AddWithValue("_CreatedBy", offer.CreatedBy);
                            conoffercommand.Parameters.AddWithValue("_CreatedIn", offer.CreatedIn);
                            conoffercommand.Parameters.AddWithValue("_ModifiedBy", offer.ModifiedBy);
                            conoffercommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now);
                            conoffercommand.Parameters.AddWithValue("_DeliveryDate", offer.DeliveryDate); 
                            conoffercommand.Parameters.AddWithValue("_IdBusinessUnit", offer.IdBusinessUnit);
                            conoffercommand.Parameters.AddWithValue("_IdSalesOwner", offer.IdSalesOwner);
                            conoffercommand.Parameters.AddWithValue("_IdCarOEM", offer.IdCarOEM);
                            conoffercommand.Parameters.AddWithValue("_IdSource", offer.IdSourceNew);
                            conoffercommand.Parameters.AddWithValue("_RFQReception", offer.RFQReception);
                            conoffercommand.Parameters.AddWithValue("_Rfq", offer.Rfq);
                            conoffercommand.Parameters.AddWithValue("_SendIn", offer.SendIn);
                            conoffercommand.Parameters.AddWithValue("_IdCarProject", offer.IdCarProject);
                            conoffercommand.Parameters.AddWithValue("_IdSourceOffer", offer.IdSourceOffer);
                            conoffercommand.Parameters.AddWithValue("_IdReporterSource", offer.IdReporterSource);
                            conoffercommand.Parameters.AddWithValue("_IdProductCategory", offer.IdProductCategory);
                            conoffercommand.Parameters.AddWithValue("_IsManualCategory", offer.IsManualCategory);
                            conoffercommand.Parameters.AddWithValue("_Discount", offer.Discount);
                            conoffercommand.Parameters.AddWithValue("_contact", offer.Contact);
                            conoffercommand.Parameters.AddWithValue("_IdCustomerToDelivery", offer.IdCustomerToDelivery);
                            conoffercommand.Parameters.AddWithValue("_Comments", offer.Comments);
                            conoffercommand.Parameters.AddWithValue("_Title", offer.Title);
                            conoffercommand.Parameters.AddWithValue("_DeliveryWeeks", offer.DeliveryWeeks);
                            conoffercommand.Parameters.AddWithValue("_AssignedTo", 0); 
                            conoffercommand.Parameters.AddWithValue("_ProductionFinish", offer.ProductionFinish);
                            conoffercommand.Parameters.AddWithValue("_AssignedIn", offer.AssignedIn);
                            conoffercommand.Parameters.AddWithValue("_Priority", offer.Priority);
                            conoffercommand.Parameters.AddWithValue("_IdShippingAddress", offer.IdShippingAddress);                     
                            conoffercommand.Parameters.AddWithValue("_IsCritical", offer.IsCritical);
                            conoffercommand.Parameters.AddWithValue("_offeredBy", offer.OfferedBy);
                            conoffercommand.Parameters.AddWithValue("_IdServiceRequest", offer.IdServiceRequest);
                            conoffercommand.Parameters.AddWithValue("_IdSite", offer.Site.IdCompany);
                            conoffercommand.Parameters.AddWithValue("_NewProduct", offer.NewProduct);
                            conoffercommand.Parameters.AddWithValue("_IdCarriageMethod", offer.IdCarriageMethod);
                            conoffercommand.Parameters.AddWithValue("_IdPersonType", offer.IdPersonType);
                            conoffercommand.Parameters.AddWithValue("_IdApprover", offer.IdApprover);
                            conoffercommand.Parameters.AddWithValue("_InitialOfferValue", offer.InitialOfferValue);
                            conoffercommand.Parameters.AddWithValue("_IdJiraCode", offer.IdJiraCode);
                            conoffercommand.Parameters.AddWithValue("_IdProblem", offer.IdProblem);

                            offerId = Convert.ToInt64(conoffercommand.ExecuteScalar());
                            offer.IdOffer = offerId;
                        }
                        catch (Exception ex)
                        {
                            if (Transaction.Current != null)
                                Transaction.Current.Rollback();
                            if (transactionScope != null)
                                transactionScope.Dispose();
                            Log4NetLogger.Logger.Log(string.Format("Error InsertOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (offer.IdOffer > 0)
                    {
                        if (offer.OfferContacts != null)
                        {
                            InsertOfferContacts(offer, connectionString);
                        }
                        if (offer.OptionsByOffers != null)
                        {
                            offer.OptionsByOffers.ForEach(i => i.IdOffer = offer.IdOffer);
                            InsertOptionByOffer_V2660(offer.OptionsByOffers, connectionString);
                        }
                        if (q.IdDetectionsTemplate == 1)
                        {
                            InsertQuotations_V2660(q, connectionString);
                        }
                        else if (q.IdDetectionsTemplate == 5)
                        {
                            InsertMaterailQuotations_V2660(q, connectionString);
                        }

                        // Insert Log
                        LogEntryByOffer ltype = new LogEntryByOffer();
                        ltype.IdLogEntryType = Convert.ToByte(1);
                        ltype.Comments = string.Format("The Offer Imported by System");
                        ltype.IdUser = 164;
                        AddLogEntriesByImportOffer(offer.IdOffer, ltype, connectionString);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    if (offer != null)
                    {
                        CreateFolderOffer_V2620(offer, connectionString, WorkingOrdersPath, workbenchConnectionString, true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();
                if (transactionScope != null)
                    transactionScope.Dispose();
                Log4NetLogger.Logger.Log(string.Format("Error InsertOffer_V2660(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return offerId;
        }

        public void AddLogEntriesByImportOffer(Int64 idOffer, LogEntryByOffer log, string mainServerConnectionString)
        {

            if (log != null)
            {

                using (MySqlConnection connLogEntries = new MySqlConnection(mainServerConnectionString))
                {

                    connLogEntries.Open();

                    MySqlCommand logEntriesCommand = new MySqlCommand("logEntriesByPO_Insert_V2590", connLogEntries);
                    logEntriesCommand.CommandType = CommandType.StoredProcedure;
                    logEntriesCommand.Parameters.AddWithValue("_IdOffer", idOffer);
                    logEntriesCommand.Parameters.AddWithValue("_IdUser", log.IdUser);
                    logEntriesCommand.Parameters.AddWithValue("_Datetime", DateTime.Now);
                    logEntriesCommand.Parameters.AddWithValue("_IdLogEntryType", log.IdLogEntryType);
                    logEntriesCommand.Parameters.AddWithValue("_Comments", log.Comments);
                    logEntriesCommand.Parameters.AddWithValue("_IsRtfText", log.IsRtfText);
                    logEntriesCommand.ExecuteScalar();
                    connLogEntries.Close();
                }

            }

        }


        //[rdixit][GEOS2-9624][03.10.2025]
        public List<Email> GetEmailCreatedIn_V2670(string ConnectionStringgeos, string poAnalyzerEmailToCheck)
        {
            List<Email> EmailList = new List<Email>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringgeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetEmailCreatedIn_V2670", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_poAnalyzerEmailToCheck", poAnalyzerEmailToCheck);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Email email = new Email();
                            try
                            {
                                if (rdr["IdEmail"] != DBNull.Value)
                                    email.IdEmail = Convert.ToInt32(rdr["IdEmail"]);
                                if (rdr["CreatedIn"] != DBNull.Value)
                                    email.CreatedIn = rdr["CreatedIn"] as DateTime?;
                                //[rahul.gadhave][GEOS2-6799][Date:19/02/2025]
                                if (rdr["EmailSentAt"] != DBNull.Value)
                                    email.EmailSentAt = rdr["EmailSentAt"] as DateTime?;
                                if (rdr["Subject"] != DBNull.Value)
                                    email.Subject = rdr["Subject"] as string;
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            EmailList.Add(email);
                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetEmailCreatedIn_V2670(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return EmailList;
        }

        /// [001]  //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9517
        public List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2680(string connectionString, int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = new List<ShippingAddress>();
            ShippingAddress defaultPOType = new ShippingAddress();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetShippingAddressForShowAll_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomerPlant);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            ShippingAddress shippingAddress = new ShippingAddress();
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                shippingAddress.IdShippingAddress = mySqlDataReader.GetInt64("IdShippingAddress");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                shippingAddress.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = mySqlDataReader.GetString("iso");
                            }
                            if (mySqlDataReader["FullAddress"] != DBNull.Value)
                            {
                                shippingAddress.FullAddress = mySqlDataReader.GetString("FullAddress");
                            }
                            if (mySqlDataReader["Address"] != DBNull.Value)
                            {
                                shippingAddress.Address = mySqlDataReader.GetString("Address");
                            }
                            if (mySqlDataReader["ZipCode"] != DBNull.Value)
                            {
                                shippingAddress.ZipCode = mySqlDataReader.GetString("ZipCode");
                            }
                            if (mySqlDataReader["City"] != DBNull.Value)
                            {
                                shippingAddress.City = mySqlDataReader.GetString("City");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = Convert.ToString(mySqlDataReader["iso"]);
                                shippingAddress.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + shippingAddress.IsoCode + ".png";
                            }
                            if (mySqlDataReader["CountriesName"] != DBNull.Value)
                            {
                                shippingAddress.CountriesName = mySqlDataReader.GetString("CountriesName");
                            }
                            if (mySqlDataReader["Region"] != DBNull.Value)
                            {
                                shippingAddress.Region = mySqlDataReader.GetString("Region");
                            }
                            //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025]
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                shippingAddress.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            ShippingAddressList.Add(shippingAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddressForShowAll_V2680(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ShippingAddressList;
        }

        //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025]
        public List<ShippingAddress> OTM_GetShippingAddress_V2680(string connectionString, int IdSite)
        {
            List<ShippingAddress> ShippingAddressList = new List<ShippingAddress>();
            ShippingAddress defaultPOType = new ShippingAddress();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetShippingAddress_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            ShippingAddress shippingAddress = new ShippingAddress();
                            if (mySqlDataReader["IdShippingAddress"] != DBNull.Value)
                            {
                                shippingAddress.IdShippingAddress = mySqlDataReader.GetInt64("IdShippingAddress");
                            }
                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                shippingAddress.Name = mySqlDataReader.GetString("Name");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = mySqlDataReader.GetString("iso");
                            }
                            if (mySqlDataReader["FullAddress"] != DBNull.Value)
                            {
                                shippingAddress.FullAddress = mySqlDataReader.GetString("FullAddress");
                            }
                            if (mySqlDataReader["Address"] != DBNull.Value)
                            {
                                shippingAddress.Address = mySqlDataReader.GetString("Address");
                            }
                            if (mySqlDataReader["ZipCode"] != DBNull.Value)
                            {
                                shippingAddress.ZipCode = mySqlDataReader.GetString("ZipCode");
                            }
                            if (mySqlDataReader["City"] != DBNull.Value)
                            {
                                shippingAddress.City = mySqlDataReader.GetString("City");
                            }
                            if (mySqlDataReader["iso"] != DBNull.Value)
                            {
                                shippingAddress.IsoCode = Convert.ToString(mySqlDataReader["iso"]);
                                shippingAddress.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + shippingAddress.IsoCode + ".png";
                            }
                            if (mySqlDataReader["CountriesName"] != DBNull.Value)
                            {
                                shippingAddress.CountriesName = mySqlDataReader.GetString("CountriesName");
                            }
                            //[pooja.jadhav][GEOS2-7057][12-03-2025]
                            if (mySqlDataReader["Region"] != DBNull.Value)
                            {
                                shippingAddress.Region = mySqlDataReader.GetString("Region");
                            }
                            //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025]
                            if (mySqlDataReader["IdCustomer"] != DBNull.Value)
                            {
                                shippingAddress.IdCustomerPlant = Convert.ToInt32(mySqlDataReader["IdCustomer"]);
                            }
                            ShippingAddressList.Add(shippingAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error OTM_GetShippingAddress_V2680(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ShippingAddressList;
        }

        public List<People> GetPeoples(string connectionstring)
        {
            List<People> peoples = new List<People>();
            try
            {
               
                using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(connectionstring))
                {
                    conofferwithoutpurchaseorder.Open();
                    MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("OTM_GetAllPeoples_V2680", conofferwithoutpurchaseorder);
                    conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                    {
                        while (offerreader.Read())
                        {
                            People people = new People();
                            if (offerreader["IdEmployee"] != DBNull.Value)
                            {
                                people.IdEmployee = Convert.ToInt32(offerreader["IdEmployee"].ToString()); //emdep_geos.employees
                            }
                            if (offerreader["IdUser"] != DBNull.Value)
                            {
                                people.IdUser = Convert.ToInt32(offerreader["IdUser"].ToString()); //emdep_geos.employees
                            }
                            if (offerreader["IdPerson"] != DBNull.Value)
                            {
                                people.IdPerson = Convert.ToInt32(offerreader["IdPerson"].ToString()); //people p
                            }                           
                            people.Name = offerreader["Name"].ToString();
                            people.Surname = offerreader["Surname"].ToString();
                            people.Email = offerreader["Email"].ToString();                                                   
                            peoples.Add(people);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPeoples(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return peoples;
        }

        public int GetPeopleDetailsbyEmpCode_V2680(string connectionString,string employeeCodes)
        {
            int IdPerson = 0;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_PeopleDetailsbyFullName_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_fullname", employeeCodes);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdPerson"] != DBNull.Value)
                            {
                                IdPerson = Convert.ToInt32(reader["IdPerson"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPeopleDetailsbyEmpCode_V2680(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IdPerson;
        }

        public People GetContactsByIdPermission_V2680(string connectionstring, Int32 idActiveUser, string idUser, string idSite, Int32 idPermission,int idperson)
        {
            
            People people = new People();
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("OTM_people_getcontactsbyidpermission_V2680", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_idActiveUser", idActiveUser);
                mySqlCommand.Parameters.AddWithValue("_idUser", idUser);
                mySqlCommand.Parameters.AddWithValue("_idSite", idSite);
                mySqlCommand.Parameters.AddWithValue("_idPermission", idPermission);
                mySqlCommand.Parameters.AddWithValue("_idperson", idperson);
                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       
                        people.IdPerson = Convert.ToInt32(reader["IdPerson"]);
                        people.Name = Convert.ToString(reader["FirstName"]);
                        people.Surname = Convert.ToString(reader["LastName"]);
                        people.Phone = Convert.ToString(reader["Telephone"]);
                        people.Email = Convert.ToString(reader["Email"]);
                        if (reader["IdPersonGender"] != DBNull.Value)
                        {
                            people.IdPersonGender = Convert.ToByte(reader["IdPersonGender"]);
                            if (people.IdPersonGender == 1)
                                people.UserGender = "Female";
                            else if (people.IdPersonGender == 2)
                                people.UserGender = "Male";
                        }
                        people.IdPersonType = Convert.ToByte(reader["IdPersonType"].ToString());
                        people.PeopleType = new PeopleType();
                        people.PeopleType.IdPersonType = Convert.ToByte(reader["IdPersonType"].ToString());
                        people.PeopleType.Name = Convert.ToString(reader["PersonTypeName"]);
                        if (reader["CreatedIn"] != DBNull.Value)
                            people.CreatedIn = Convert.ToDateTime(reader["CreatedIn"].ToString());
                        people.Company = new Company() { IdCompany = Convert.ToInt32(reader["IdCompany"].ToString()), Name = reader["CustomerPlant"].ToString().Trim(), Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["Country"].ToString(), CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + reader["Iso"].ToString() + ".png", Zone = new Zone { Name = reader["Zone"].ToString() } } };
                        //if (idPermission == 21 || idPermission == 22)
                        //{
                        //    if (reader["IdSalesResponsible"] != DBNull.Value)
                        //    {
                        //        people.Company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"]);
                        //        people.Company.People = new People();
                        //        people.Company.People.IdPerson = (Int32)people.Company.IdSalesResponsible;
                        //        if (reader["SalesResponsibleFirstName"] != DBNull.Value)
                        //            people.Company.People.Name = Convert.ToString(reader["SalesResponsibleFirstName"]);
                        //        if (reader["SalesResponsibleLastName"] != DBNull.Value)
                        //            people.Company.People.Surname = Convert.ToString(reader["SalesResponsibleLastName"]);
                        //    }
                        //    if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                        //    {
                        //        people.Company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"]);
                        //        people.Company.PeopleSalesResponsibleAssemblyBU = new People();
                        //        people.Company.PeopleSalesResponsibleAssemblyBU.IdPerson = (Int32)people.Company.IdSalesResponsibleAssemblyBU;
                        //        if (reader["SalesResponsibleBUFirstName"] != DBNull.Value)
                        //            people.Company.PeopleSalesResponsibleAssemblyBU.Name = Convert.ToString(reader["SalesResponsibleBUFirstName"]);
                        //        if (reader["SalesResponsibleBULastName"] != DBNull.Value)
                        //            people.Company.PeopleSalesResponsibleAssemblyBU.Surname = Convert.ToString(reader["SalesResponsibleBULastName"]);
                        //    }
                        //}
                        people.Company.Address = Convert.ToString(reader["SiteAddress"]);
                        people.Observations = Convert.ToString(reader["Observations"]);
                        people.Company.City = Convert.ToString(reader["SiteCity"]);
                        people.Company.CIF = Convert.ToString(reader["SiteCIF"]);
                        people.Company.Telephone = Convert.ToString(reader["SiteTelephone"]);
                        people.Company.Fax = Convert.ToString(reader["SiteFax"]);
                        people.Company.PostCode = Convert.ToString(reader["SitePostCode"]);
                        people.Company.ZipCode = Convert.ToString(reader["SitePostCode"]);
                        people.Company.Region = Convert.ToString(reader["SiteRegion"]);
                        people.Company.RegisteredName = Convert.ToString(reader["SiteRegisteredName"]);
                        people.Company.Website = Convert.ToString(reader["SiteWebsite"]);
                        people.Company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString()), CustomerName = reader["CustomerGroup"].ToString() } };
                        if (reader["IdCompanyDepartment"] != DBNull.Value)
                        {
                            people.IdCompanyDepartment = Convert.ToInt32(reader["IdCompanyDepartment"].ToString());
                            people.CompanyDepartment = new LookupValue();
                            people.CompanyDepartment.IdLookupValue = Convert.ToInt32(reader["IdCompanyDepartment"].ToString());
                            people.CompanyDepartment.Value = reader["Value"].ToString();
                            if (reader["IdImage"] != DBNull.Value)
                                people.CompanyDepartment.IdImage = Convert.ToInt64(reader["IdImage"].ToString());
                        }
                        people.JobTitle = reader["JobTitle"].ToString();
                        people.ImageText = reader["Image"].ToString();
                        if (reader["IdContactInfluenceLevel"] != DBNull.Value)
                        {
                            people.IdContactInfluenceLevel = Convert.ToInt32(reader["IdContactInfluenceLevel"].ToString());
                            people.InfluenceLevel = new LookupValue();
                            people.InfluenceLevel.IdLookupValue = Convert.ToInt32(reader["IdContactInfluenceLevel"].ToString());
                            people.InfluenceLevel.Value = reader["InfluenceLevel"].ToString();
                            people.InfluenceLevel.HtmlColor = reader["InfluenceLevelHtmlcolor"].ToString();
                            if (reader["InfluenceLevelIdImage"] != DBNull.Value)
                                people.InfluenceLevel.IdImage = Convert.ToInt64(reader["InfluenceLevelIdImage"].ToString());
                        }
                        if (reader["IdContactEmdepAffinity"] != DBNull.Value)
                        {
                            people.IdContactEmdepAffinity = Convert.ToInt32(reader["IdContactEmdepAffinity"].ToString());
                            people.EmdepAffinity = new LookupValue();
                            people.EmdepAffinity.IdLookupValue = Convert.ToInt32(reader["IdContactEmdepAffinity"].ToString());
                            people.EmdepAffinity.Value = reader["EmdepAffinity"].ToString();
                            people.EmdepAffinity.HtmlColor = reader["EmdepAffinityHtmlcolor"].ToString();
                            if (reader["EmdepAffinityIdImage"] != DBNull.Value)
                                people.EmdepAffinity.IdImage = Convert.ToInt64(reader["EmdepAffinityIdImage"].ToString());
                        }
                        if (reader["IdContactProductInvolved"] != DBNull.Value)
                        {
                            people.IdContactProductInvolved = Convert.ToInt32(reader["IdContactProductInvolved"].ToString());
                            people.ProductInvolved = new LookupValue();
                            people.ProductInvolved.IdLookupValue = Convert.ToInt32(reader["IdContactProductInvolved"].ToString());
                            people.ProductInvolved.Value = reader["ProductInvolved"].ToString();
                            people.ProductInvolved.HtmlColor = reader["ProductInvolvedLevelHtmlcolor"].ToString();
                            if (reader["ProductInvolvedLevelIdImage"] != DBNull.Value)
                                people.ProductInvolved.IdImage = Convert.ToInt64(reader["ProductInvolvedLevelIdImage"].ToString());
                        }
                        if (reader["IdCompetitor"] != DBNull.Value)
                        {
                            people.IdCompetitor = Convert.ToInt32(reader["IdCompetitor"].ToString());
                            people.Competitor = new Competitor();
                            people.Competitor.IdCompetitor = Convert.ToInt32(reader["IdCompetitor"].ToString());
                            people.Competitor.Name = reader["Competitor"].ToString();
                        }
                        if (reader["IdCreator"] != DBNull.Value)
                        {
                            people.IdCreator = Convert.ToInt32(reader["IdCreator"].ToString());
                            people.Creator = new People();
                            people.Creator.IdPerson = Convert.ToInt32(reader["IdCreator"].ToString());
                            people.Creator.Name = Convert.ToString(reader["CreatedByFirstName"].ToString());
                            people.Creator.Surname = Convert.ToString(reader["CreatedByLastName"].ToString());
                        }
                        
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            People saleowner = new People();
                            if (reader["IdPerson"] != DBNull.Value)
                            {
                                
                                if (people.Company.SalesOwnerList == null)
                                    people.Company.SalesOwnerList = new List<People>();
                                if (idPermission == 21 || idPermission == 22)
                                {
                                    if (reader["IdSiteSalesOwner"] != DBNull.Value)
                                    {
                                        //people.Company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"]);
                                        saleowner.IdPerson = Convert.ToInt32(reader["IdSalesOwner"]);
                                        if (reader["IdSite"] != DBNull.Value)
                                            saleowner.IdSite = Convert.ToInt32(reader["IdSite"]);
                                        if (reader["IsMainSalesOwner"] != DBNull.Value)
                                        {
                                            int i = Convert.ToInt32(reader["IsMainSalesOwner"]);
                                            if (i == 1)
                                                saleowner.IsSalesResponsible = true;
                                        }
                                        if (reader["salesresponsiblename"] != DBNull.Value)
                                        {
                                            saleowner.Name = Convert.ToString(reader["salesresponsiblename"]);
                                            if (people.SalesOwner == null)
                                                people.SalesOwner = saleowner.Name;
                                            else
                                                people.SalesOwner = people.SalesOwner + "\n" + saleowner.Name;
                                        }
                                    }
                                }
                                people.Company.SalesOwnerList.Add(saleowner);
                            }
                        }
                    }
                }
            }
            return people;
        }

        //[pramod.misal][GEOS2-9601][29-10-2025]
        public int GetPODetails_V2680(string ConnectionString, string PoCode)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetIdCustomerPurchaseOrders_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_POCode", PoCode);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["idCustomerPurchaseOrders"] != DBNull.Value)
                                return Convert.ToInt32(reader["idCustomerPurchaseOrders"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in GetPODetails_V2680(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return 0;
        }

        public List<int> GetIdOffersByCustomerPurchaseOrder_V2680( string connectionString, int idCustomerPurchaseOrder)
        {
            List<int> IdOffers = new List<int>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetIdOffersByCustomerPurchaseOrder", con);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idCustomerPurchaseOrder", idCustomerPurchaseOrder);
              
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IdOffers.Add(Convert.ToInt32(reader["idOffer"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetIdOffersByCustomerPurchaseOrder_V2680(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return IdOffers;
        }

        public void OTM_InsertPORequestLinkedOffer_V2680(Int64 IdPORequest, List<int> IdOfferList, string mainServerConnectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
                {
                    conn.Open();
                    if (IdOfferList.Count>0)
                    {
                        foreach (var IdOffer in IdOfferList)
                        {
                            using (MySqlCommand mySqlCommand = new MySqlCommand("OTM_InsertPORequestLinkedOffer_V2620", conn))
                            {
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdOffer", IdOffer);
                                mySqlCommand.Parameters.AddWithValue("_IdPORequest", IdPORequest);

                                mySqlCommand.ExecuteNonQuery(); 
                            }
                        }
                    }
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"Error in OTM_InsertPORequestLinkedOffer_V2680(). ErrorMessage- {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public List<LinkedOffers> GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(string ConnectionStringGeos,Int32 IdCustomer, int IdPlant, string amount)
        {
            List<LinkedOffers> LinkedOffersList = new List<LinkedOffers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionStringGeos))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetOffersDetaislByIdCustomerIdplant_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idCustomer", IdCustomer);
                    mySqlCommand.Parameters.AddWithValue("_idplant", IdPlant);
                    if (string.IsNullOrWhiteSpace(amount))
                        amount = "0";

                    double parsedAmount = 0;
                    if (!double.TryParse(amount, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedAmount))
                    {
                        // Try again with current culture (for commas like 43,080.81)
                        double.TryParse(amount, NumberStyles.Any, CultureInfo.CurrentCulture, out parsedAmount);
                    }

                    mySqlCommand.Parameters.AddWithValue("_amount", parsedAmount);

                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LinkedOffers link = new LinkedOffers();
                            try
                            {

                                if (rdr["IdOffer"] != DBNull.Value)
                                    link.IdOffer = Convert.ToInt64(rdr["IdOffer"]);
                                if (rdr["OfferCode"] != DBNull.Value)
                                    link.Code = Convert.ToString(rdr["OfferCode"]);
                                if (rdr["IdCustomer"] != DBNull.Value)
                                    link.IdCustomer = Convert.ToInt32(rdr["IdCustomer"]);
                                if (rdr["IdSite"] != DBNull.Value)
                                    link.IdSite = Convert.ToInt32(rdr["IdSite"]);

                                

                                if (rdr["Amount"] != DBNull.Value)
                                    link.Amount = Convert.ToDouble(rdr["Amount"]);
                                                      
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                            LinkedOffersList.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return LinkedOffersList;
        }
        //[001][ashish.malkhed][19/08/2025] https://helpdesk.emdep.com/browse/GEOS2-9207
        public PORequestDetails GetPODetailsbyAttachment_V2680(string connectionString, int IdAttachment)
        {
            PORequestDetails PODetails = new PORequestDetails();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("OTM_GetPODetailsbyAttachment_V2680", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idAttachment", IdAttachment);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["PONumber"] != DBNull.Value)
                                PODetails.PONumber = mySqlDataReader["PONumber"].ToString();

                            if (mySqlDataReader["PODate"] != DBNull.Value)
                                PODetails.POdate = Convert.ToDateTime(mySqlDataReader["PODate"]);

                            if (mySqlDataReader["Sender"] != DBNull.Value)
                                PODetails.Sender = mySqlDataReader["Sender"].ToString();

                            if (mySqlDataReader["TransferAmount"] != DBNull.Value)
                                PODetails.TransferAmount = Convert.ToDouble(mySqlDataReader["TransferAmount"]);
                            else
                                PODetails.TransferAmount = 0;

                            if (mySqlDataReader["Currency"] != DBNull.Value)
                                PODetails.Currency = mySqlDataReader["Currency"].ToString();

                            if (mySqlDataReader["ShipAddress"] != DBNull.Value)
                                PODetails.ShipTo = mySqlDataReader["ShipAddress"].ToString();

                            if (mySqlDataReader["IdAttachment"] != DBNull.Value)
                                PODetails.IdAttachment = mySqlDataReader["IdAttachment"].ToString();

                            if (mySqlDataReader["AttachmentName"] != DBNull.Value)
                                PODetails.Attachments = mySqlDataReader["AttachmentName"].ToString();


                        }

                        if (mySqlDataReader.NextResult())
                        {
                            if (mySqlDataReader.HasRows)
                            {
                                PODetails.IsPOExist = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPODetailsbyAttachment_V2680(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PODetails;
        }
        // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
        public PORequestDetails GetPODetailsbyIdEmail_V2690(string connectionString, Int64 IdEmail)
        {
            PORequestDetails PODetails = new PORequestDetails();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("GetPODetailsbyIdEmail_V2690", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdEmail", IdEmail);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader["PODate"] != DBNull.Value)
                                PODetails.POdate = Convert.ToDateTime(mySqlDataReader["PODate"]);

                            if (mySqlDataReader["Sender"] != DBNull.Value)
                                PODetails.Sender = mySqlDataReader["Sender"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPODetailsbyIdEmail_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PODetails;
        }
        // [Rahul.gadhave][GEOS2-9878][Date:19 - 11 - 2025]
        public PORequestDetails GetPODetailsbyEmail_V2690(string connectionString, List<string> allEmails)
        {
            PORequestDetails PODetails = new PORequestDetails();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    foreach (string email in allEmails)
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("GetPODetailsbyEmail_V2690", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_Email", email);
                        using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader["Sender"] != DBNull.Value)
                                        PODetails.Sender = reader["Sender"].ToString();
                                }
                            }
                            else
                            {
                                
                                continue;
                            }
                        }
                        if (PODetails.Sender != null || !string.IsNullOrEmpty(PODetails.Sender))
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log( $"Error GetPODetailsbyEmail_V2690(). ErrorMessage- {ex.Message}",category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return PODetails;
        }


        public List<PORequestDetails> GetPORequestDetails_V2690(string ConnectionStringGeos, DateTime FromDate, DateTime ToDate, int Idcurrency, long plantId, string correnciesIconFilePath)
        {
            List<PORequestDetails> poList = new List<PORequestDetails>();
            List<string> currencyISOs = new List<string>();
            using (MySqlConnection mySqlconn = new MySqlConnection(ConnectionStringGeos))
            {
                mySqlconn.Open();

                using (MySqlCommand mySqlCommand = new MySqlCommand())
                {
                    try
                    {
                        mySqlCommand.CommandText = "OTM_GetPORequestDetails_V2690";
                        mySqlCommand.CommandTimeout = 600;
                        mySqlCommand.Connection = mySqlconn;
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                        mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                        mySqlCommand.Parameters.AddWithValue("_Idcurrency", Idcurrency);
                        using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                PORequestDetails po = new PORequestDetails();
                                try
                                {

                                    if (rdr["DateTime"] != DBNull.Value)
                                    {
                                        po.DateTime = Convert.ToDateTime(rdr["DateTime"]);
                                        po.Date = po.DateTime.Date;//[pramod.misal][20.11.2025][PredefinedGeometryStock2DModel-9429] https://helpdesk.emdep.com/browse/GEOS2-9429

                                    }
                                    //[pramod.misal][20.11.2025][PredefinedGeometryStock2DModel-9429] https://helpdesk.emdep.com/browse/GEOS2-9429
                                    if (rdr["Time"] != DBNull.Value)
                                    {
                                        po.Time = Convert.ToString(rdr["Time"]);

                                    }
                                    if (rdr["Sender"] != DBNull.Value)
                                        po.Sender = rdr["Sender"].ToString();
                                    if (rdr["SenderName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.SenderName = rdr["SenderName"].ToString();
                                    if (rdr["Recipient"] != DBNull.Value)
                                        po.Recipient = rdr["Recipient"].ToString();
                                    if (rdr["ToRecipientName"] != DBNull.Value) //[pramod.misal][04.02.2025][GEOS2-6726]
                                        po.ToRecipientName = rdr["ToRecipientName"].ToString();
                                    if (rdr["Subject"] != DBNull.Value)
                                        po.Subject = rdr["Subject"].ToString();
                                    if (rdr["POfound"] != DBNull.Value)
                                        po.POFound = rdr["POfound"].ToString();
                                    if (rdr["attachCount"] != DBNull.Value)
                                    {
                                        if (rdr["attachCount"].ToString() == "0")
                                            po.AttachmentCnt = string.Empty;
                                        else
                                            po.AttachmentCnt = rdr["attachCount"].ToString();
                                    }
                                    if (rdr["Attachments"] != DBNull.Value)
                                        po.Attachments = rdr["Attachments"].ToString();
                                    else
                                        po.Attachments = "";
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);

                                    if (rdr["AttachmentPath"] != DBNull.Value)
                                        po.Path = rdr["AttachmentPath"].ToString();
                                    else
                                        po.Path = "";

                                    if (rdr["Requester"] != DBNull.Value)
                                        po.Requester = rdr["Requester"].ToString();
                                    else
                                        po.Requester = "";
                                    if (rdr["Status"] != DBNull.Value)
                                    {
                                        LookupValue lv = new LookupValue();
                                        lv.IdLookupValue = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                        lv.Value = rdr["Status"].ToString();
                                        lv.HtmlColor = rdr["HtmlColor"].ToString();
                                        po.PORequestStatus = lv;
                                    }
                                    if (rdr["IdPORequestStatus"] != DBNull.Value)
                                        po.IdPORequestStatus = Convert.ToInt32(rdr["IdPORequestStatus"]);
                                    else
                                        po.IdStatus = 0;
                                    if (po.Currency != null)
                                    {
                                        if (!currencyISOs.Any(co => co.ToString() == po.Currency))
                                        {
                                            currencyISOs.Add(po.Currency);
                                        }
                                    }
                                    if (rdr["IdPORequest"] != DBNull.Value)
                                        po.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);
                                    //[rahul.gadhave][GEOS2-6720][29-01-2025]
                                    if (rdr["CCRecipients"] != DBNull.Value)
                                        po.CCRecipient = rdr["CCRecipients"].ToString();
                                    //[pramod.misal][04.02.2025][GEOS2-6726]
                                    if (rdr["CCName"] != DBNull.Value)
                                        po.CCName = rdr["CCName"].ToString();
                                    if (rdr["Inbox"] != DBNull.Value)
                                        po.Inbox = rdr["Inbox"].ToString();
                                    //[rahul.gadhave][GEOS2-6718][29 -01-2025]
                                    if (rdr["Offers"] != DBNull.Value)
                                        po.Offers = rdr["Offers"].ToString();
                                    //[nsatpute][06-03-2025][GEOS2-6722]
                                    po.Customer = Convert.ToString(rdr["Customer"]);
                                    po.InvoioceTO = Convert.ToString(rdr["InvoiceTo"]);
                                    po.PONumber = Convert.ToString(rdr["PONumber"]);
                                    po.Offer = Convert.ToString(rdr["Offer"]);
                                    po.DateIssuedString = Convert.ToString(rdr["PODate"]);
                                    po.Contact = Convert.ToString(rdr["Email"]);
                                    po.TransferAmountString = Convert.ToString(rdr["TransferAmount"]);
                                    po.Currency = Convert.ToString(rdr["Currency"]);
                                    po.ShipTo = Convert.ToString(rdr["ShipTo"]);
                                    po.POIncoterms = Convert.ToString(rdr["Incoterm"]);
                                    po.POPaymentTerm = Convert.ToString(rdr["PaymentTerms"]);
                                    if (rdr["CustomerGroup"] != DBNull.Value)
                                        po.Group = rdr["CustomerGroup"].ToString();
                                    //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                                    if (rdr["IdCustomerGroup"] != DBNull.Value)
                                        po.IdCustomerGroup = Convert.ToInt32(rdr["IdCustomerGroup"]);
                                    //if (rdr["PlantName"] != DBNull.Value)
                                    //    po.Plant = rdr["PlantName"].ToString();
                                    if (rdr["CustomerPlant"] != DBNull.Value)
                                        po.Plant = rdr["CustomerPlant"].ToString();
                                    if (rdr["IdCustomerPlant"] != DBNull.Value)
                                        po.IdCustomerPlant = Convert.ToInt32(rdr["IdCustomerPlant"]);
                                    if (rdr["SenderIdPerson"] != DBNull.Value)
                                        po.SenderIdPerson = rdr["SenderIdPerson"].ToString();
                                    if (rdr["ToIdPerson"] != DBNull.Value)
                                        po.ToIdPerson = rdr["ToIdPerson"].ToString();
                                    if (rdr["CCIdPerson"] != DBNull.Value)
                                        po.CCIdPerson = rdr["CCIdPerson"].ToString();
                                    //[pramod.misal][GEOS2-7248][22.04.2025]
                                    if (rdr["IdEmail"] != DBNull.Value)
                                        po.IdEmail = Convert.ToInt64(rdr["IdEmail"]);
                                    //if (rdr["IdAttachmentType"] != DBNull.Value)
                                    //    po.IdAttachementType = Convert.ToInt32(rdr["IdAttachmentType"]);
                                    //if (rdr["IdAttachment"] != DBNull.Value)
                                    //    po.IdAttachment = Convert.ToString(rdr["IdAttachment"]);
                                    if (rdr["idcountry"] != DBNull.Value)
                                        po.IdCountry = Convert.ToInt64(rdr["idcountry"]);
                                    poList.Add(po);

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2690(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            //[pramod.misal][GEOS2-6719][13-13 -2025]
                            if (rdr.NextResult())
                            {
                                while (rdr.Read())
                                {
                                    POLinkedOffers o = new POLinkedOffers();
                                    var p = (PORequestDetails)poList.FirstOrDefault(x => x.IdPORequest == Convert.ToInt64(rdr["IdPORequest"]));
                                    if (p != null)
                                    {
                                        if (rdr["IdPORequest"] != DBNull.Value)
                                            o.IdPORequest = Convert.ToInt32(rdr["IdPORequest"]);

                                        if (rdr["Code"] != DBNull.Value)
                                            o.Code = Convert.ToString(rdr["Code"]);

                                        if (rdr["groupname"] != DBNull.Value)
                                            o.Groupname = Convert.ToString(rdr["groupname"]);

                                        if (rdr["plant"] != DBNull.Value)
                                            o.Plant = Convert.ToString(rdr["plant"]);

                                        if (rdr["offerIdcustomerPlant"] != DBNull.Value)
                                            o.IdCustomerPlant = Convert.ToInt32(rdr["offerIdcustomerPlant"]);

                                        if (rdr["offerIdcustomer"] != DBNull.Value)
                                            o.IdCustomerGroup = Convert.ToInt32(rdr["offerIdcustomer"]);

                                        p.POLinkedOffers = o;
                                    }
                                }
                            }
                        }
                        mySqlconn.Close();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPORequestDetails_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }
                }
            }
            return poList;
        }

    }
}