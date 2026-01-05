using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.TechnicalRestService;
using log4net.Core;
using Prism.Logging;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using Emdep.Geos.Data.BusinessLogic.Logging;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TechnicalRestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TechnicalRestService.svc or TechnicalRestService.svc.cs at the Solution Explorer and start debugging.
    public class TechnicalRestService : ITechnicalRestService
    {
        string MainConn = string.Empty;
        string LoginContext = string.Empty;
        string Plantwiseconnectionstring = string.Empty;
        string PlantwiseconnectionstringSlave = string.Empty;
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string ExportTechnicalAssistanceReport(string IdTechnicalAssistanceReport, string PlantOwner, string Lang)
        {
            bool success = false;
            try
            {
                MainConn = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                LoginContext = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                //Plantwiseconnectionstring = ConfigurationManager.ConnectionStrings["Plantwiseconnectionstring"].ConnectionString;
                Plantwiseconnectionstring = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                //string TechnicalAssistanceReportTemplatePath = ConfigurationManager.AppSettings["TechnicalAssistanceReportTemplatePath"];
                //string TechnicalAssistanceReportPath = ConfigurationManager.AppSettings["TechnicalAssistanceReportPath"];
                //string WorkingOrdersPath = ConfigurationManager.AppSettings["WorkingOrdersPath"];
                string TechnicalAssistanceReportTemplatePath = Properties.Settings.Default.TechnicalAssistanceReportTemplatePath;
                string TechnicalAssistanceReportPath = Properties.Settings.Default.TechnicalAssistanceReportPath;
                string WorkingOrdersPath = Properties.Settings.Default.WorkingOrdersPath + @"\";

                TechnicalServiceManager technicalServiceManager = new TechnicalServiceManager(MainConn);
                technicalServiceManager = new TechnicalServiceManager(MainConn, LoginContext);
                success = technicalServiceManager.ExportTechnicalAssistanceReport(IdTechnicalAssistanceReport, PlantOwner, string.IsNullOrEmpty(Lang) ? "en" : Lang.ToLower(),
                    TechnicalAssistanceReportTemplatePath, TechnicalAssistanceReportPath, WorkingOrdersPath, Plantwiseconnectionstring);
                
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            // Example response (you can replace this with your actual logic)
            return Convert.ToString(success);
        }

        //public string ExportTechnicalAssistanceReportNew(InputData input)
        //{

        //}

        public async Task<OpportunityCreateModel> SalesOpportunitiesCreate(CreateOpportunityModel request)
        {
            bool success = false;
            OpportunityCreateModel opportunityCreateModel = new OpportunityCreateModel();
            ErrorModel errorModel = new ErrorModel();
            opportunityCreateModel.Offer = new OfferCreatedModel();
            try
            {
                string PlantOwner = request.ParameterPlantOwner;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string CommercialQuotationsPath = request.ParameterCommercialQuotationsPath;
                string WorkingOrdersPath = request.ParameterWorkingOrdersPath;
                OpportunityManager opportunityManager = new OpportunityManager(MainConn);

                // Call AddOffer
                CreateOfferReturn createOfferReturns = await opportunityManager.AddOffer(
                    PlantOwner,
                    request,
                    MainConn,
                    LoginContext,
                    CommercialQuotationsPath,
                    WorkingOrdersPath,
                    Plantwiseconnectionstring
                );
                if (createOfferReturns != null)
                {
                    opportunityCreateModel.Offer.id = createOfferReturns.id;
                    opportunityCreateModel.Offer.code = createOfferReturns.code;
                    string Email;
                    if (opportunityManager.GetErrorcode(MainConn, request.offer_owner, out Email))
                    {
                        opportunityCreateModel.warning = new ErrorMessage
                        {
                            code = "878",
                            info = $"The user {Email} has been assigned as offer owner although is disabled"
                        };
                    }
                }
                else
                {
                    opportunityCreateModel.Offer = null;
                }
            }
            catch (Exception ex)
            {
                throw;  //chitra.girigosavi APIGEOS-1549 09/09/2025
            }
            return opportunityCreateModel;
        }

        public bool SalesOpportunitiesExport(CreateOpportunityModel request)
        {
            OpportunityCreateModel salesCreateOpportunityModel = new OpportunityCreateModel();
            OpportunityManager offerManager = new OpportunityManager(MainConn);
            bool result = false;
            try
            {
                string PlantOwner = request.ParameterPlantOwner;
                string OfferId = request.OfferId;
                string Lang = request.Lang;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string CommercialPath = request.ParameterCommercialPath;
                string CommercialOfferTemplatePath = request.ParameterCommercialOfferTemplatePath;
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                    &&!string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(OfferId) && !string.IsNullOrEmpty(Lang)
                    )
                {
                    result= offerManager.ExportOffer(PlantOwner, OfferId, MainConn, LoginContext, CommercialOfferTemplatePath, CommercialPath, Lang.ToLower(), Plantwiseconnectionstring);
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
           // return salesCreateOpportunityModel;
            return result;
        }


        public SendEmail SalesOpportunitiesSend(SendMailOffer request)
        {
            OpportunityCreateModel salesCreateOpportunityModel = new OpportunityCreateModel();
            OpportunityManager offerManager = new OpportunityManager(MainConn);
            SendMailOffer SendMailModel = new SendMailOffer();
            SendEmailModel SendEmailModel = new SendEmailModel();
            ErrorModel errorModel = new ErrorModel();
            SendEmail sendemail = new SendEmail();
            try
            {
                string PlantOwner = request.ParameterPlantOwner;
                string OfferId = request.OfferId;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string CommercialPath = request.ParameterCommercialPath;
                string MailServerName = request.ParameterMailServerName;
                string MailServerPort = request.ParameterMailServerPort;
                string MailFrom = request.ParameterMailFrom;
                string Mailpassword = request.ParameterMailpassword;
                string MailUser = request.ParameterMailUser;
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                    )
                {
                    sendemail = offerManager.SendOffer(request,PlantOwner,OfferId,MainConn,LoginContext,
                        CommercialPath,MailServerName,MailServerPort,MailFrom,Mailpassword,MailUser,Plantwiseconnectionstring);
                    if (sendemail.error != null)
                    {
                        SendEmailModel.error = sendemail.error;
                    }
                    if (sendemail.warning != null)
                    {
                        SendEmailModel.warning = sendemail.warning;
                    }
                    SendEmailModel.success = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return sendemail;
        }

        public bool PurchasingOrdersExport(PurchasingOrderExport request)
        {
            bool result = false;
            
            try
            {
                string IdPO = request.IdPO;
                string Lang = request.Lang;
                string PlantOwner = request.ParameterPlantOwner;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string ExportPurchaseOrderTemplatePath = request.ParameterExportPurchaseOrderTemplatePath;
                string PurchasingOrdersFilePath = request.ParameterPurchasingOrdersFilePath;
                OrderManager orderManager = new OrderManager(MainConn, LoginContext);
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                    && !string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(IdPO) && !string.IsNullOrEmpty(Lang))
                {
                    result = orderManager.ExportPurchaseOrder(IdPO,
                        PlantOwner, 
                        string.IsNullOrEmpty(Lang) ? "en" : Lang.ToLower(),
                        ExportPurchaseOrderTemplatePath,
                        PurchasingOrdersFilePath,
                        Plantwiseconnectionstring);
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }


        public async Task<bool> PurchasingOrdersSign(PurchasingOrderExport request)
        {
            bool result = false;
            try
            {
                string IdPO = request.IdPO;
                string PlantOwner = request.ParameterPlantOwner;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string ExportPurchaseOrderTemplatePath = request.ParameterExportPurchaseOrderTemplatePath;
                string PurchasingOrdersFilePath = request.ParameterPurchasingOrdersFilePath;
                CreateExpenseReportsSign createPurchasingOrdersReportsSign = request.ParameterCreateExpenseReportsSign;
                OrderManager orderManager = new OrderManager(MainConn, LoginContext);
                //result = orderManager.PurchasingOrdersReportsSign(createPurchasingOrdersReportsSign, MainConn, LoginContext, IdPO, PurchasingOrdersFilePath, PlantOwner, Plantwiseconnectionstring);
                //chitra.girigosavi APIGEOS-1646 10/03/2025
                result = await orderManager.PurchasingOrdersReportsSignAsync(createPurchasingOrdersReportsSign, MainConn, LoginContext, IdPO, PurchasingOrdersFilePath, PlantOwner, Plantwiseconnectionstring);//chitra.girigosavi APIGEOS-1646 10/03/2025
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> PurchasingOrdersSign(Stream requestStream)
        {
            bool result = false;
            try
            {
                using (GZipStream gzipStream = new GZipStream(requestStream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(requestBody))
                    {
                        result = false;
                        throw new Exception("Request body is empty.");
                    }
                    PurchasingOrderExport request = JsonConvert.DeserializeObject<PurchasingOrderExport>(requestBody);
                    // Extract parameters
                    string IdPO = request.IdPO;
                    string PlantOwner = request.ParameterPlantOwner;
                    string MainConn = request.ParameterMainConn;
                    string LoginContext = request.ParameterLoginContext;
                    string Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                    string ExportPurchaseOrderTemplatePath = request.ParameterExportPurchaseOrderTemplatePath;
                    string PurchasingOrdersFilePath = request.ParameterPurchasingOrdersFilePath;
                    CreateExpenseReportsSign createPurchasingOrdersReportsSign = request.ParameterCreateExpenseReportsSign;
                    OrderManager orderManager = new OrderManager(MainConn, LoginContext);
                    //result = orderManager.PurchasingOrdersReportsSign(createPurchasingOrdersReportsSign, MainConn, LoginContext, IdPO, PurchasingOrdersFilePath, PlantOwner, Plantwiseconnectionstring);
                    //chitra.girigosavi APIGEOS-1646 10/03/2025
                    result = await orderManager.PurchasingOrdersReportsSignAsync(createPurchasingOrdersReportsSign, MainConn, LoginContext, IdPO, PurchasingOrdersFilePath, PlantOwner, Plantwiseconnectionstring);
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }

        public async Task<bool> TechnicalServiceSign(Stream requestStream)
        {
            bool result = false;
            try
            {
                using (GZipStream gzipStream = new GZipStream(requestStream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(requestBody))
                    {
                        result = false;
                        throw new Exception("Request body is empty.");
                    }
                    TechnicalServiceExport request = JsonConvert.DeserializeObject<TechnicalServiceExport>(requestBody);
                    // Extract parameters
                    string IdTechnicalAssistanceReport = request.IdTechnicalAssistanceReport;
                    string Signatory = request.Signatory;
                    string PlantOwner = request.ParameterPlantOwner;
                    string MainConn = request.ParameterMainConn;
                    string LoginContext = request.ParameterLoginContext;
                    string Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                    string TechnicalAssistanceReportPath = request.TechnicalAssistanceReportPath;
                    CreateExpenseReportsSign createExpenseReportsSign = request.ParameterSign;
                    TechnicalServiceManager technicalServiceManager = new TechnicalServiceManager(MainConn);
                    technicalServiceManager = new TechnicalServiceManager(MainConn, LoginContext);
                    if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                        && !string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(Signatory) && !string.IsNullOrEmpty(IdTechnicalAssistanceReport))
                    {
                        result = technicalServiceManager.TechnicalServiceOrderReportsSign(createExpenseReportsSign, MainConn, LoginContext, Plantwiseconnectionstring, IdTechnicalAssistanceReport, TechnicalAssistanceReportPath, Signatory, PlantOwner); ;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }

        public SendEmail TechnicalServiceSend(SendMailOffer request)
        {
            SendMailOffer SendMailModel = new SendMailOffer();
            SendEmailModel SendEmailModel = new SendEmailModel();
            Error error = new Error();
            ErrorModel errorModel = new ErrorModel();
            SendEmail sendemail = new SendEmail();
            try
            {
                string PlantOwner = request.ParameterPlantOwner;
                string IdTechnicalAssistanceReport = request.IdTechnicalAssistanceReport;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                string TechnicalAssistanceReportPath = request.TechnicalAssistanceReportPath;
                string MailServerName = request.ParameterMailServerName;
                string MailServerPort = request.ParameterMailServerPort;
                string MailFrom = request.ParameterMailFrom;
                string Mailpassword = request.ParameterMailpassword;
                string MailUser = request.ParameterMailUser;
                TechnicalServiceManager technicalServiceManager = new TechnicalServiceManager(MainConn, LoginContext);
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring) && !string.IsNullOrEmpty(PlantOwner)
                    && !string.IsNullOrEmpty(IdTechnicalAssistanceReport) && !string.IsNullOrEmpty(TechnicalAssistanceReportPath) )
                {
                    sendemail = technicalServiceManager.SendTechnicalAssistanceReport(request, PlantOwner, IdTechnicalAssistanceReport, MainConn, LoginContext,
                        TechnicalAssistanceReportPath, MailServerName, MailServerPort, MailFrom,Mailpassword, MailUser, Plantwiseconnectionstring);
                    if (sendemail.error != null)
                    {
                        SendEmailModel.error = sendemail.error;
                    }
                    if (sendemail.warning != null)
                    {
                        SendEmailModel.warning = sendemail.warning;
                    }
                    SendEmailModel.success = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           // return SendEmailModel;
            return sendemail;
        }
        public string TechnicalAssistanceReportExport(TechnicalServiceExport request)
        {
            bool success = false;
            try
            {
                string IdTechnicalAssistanceReport = request.IdTechnicalAssistanceReport;
                string PlantOwner = request.ParameterPlantOwner;
                string Lang = request.Lang;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;

                string TechnicalAssistanceReportTemplatePath = request.TechnicalAssistanceReportTemplatePath;
                string TechnicalAssistanceReportPath = request.TechnicalAssistanceReportPath;
                string WorkingOrdersPath = request.WorkingOrdersPath;

                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring) &&
                    !string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(IdTechnicalAssistanceReport) && !string.IsNullOrEmpty(TechnicalAssistanceReportPath))
                {
                    TechnicalServiceManager technicalServiceManager = new TechnicalServiceManager(MainConn);
                    technicalServiceManager = new TechnicalServiceManager(MainConn, LoginContext);
                    success = technicalServiceManager.ExportTechnicalAssistanceReport_V800(IdTechnicalAssistanceReport, PlantOwner, string.IsNullOrEmpty(Lang) ? "en" : Lang.ToLower(),
                        TechnicalAssistanceReportTemplatePath, TechnicalAssistanceReportPath, WorkingOrdersPath, Plantwiseconnectionstring);
                }
            }
            catch (Exception ex)
            {
                success = false;
                return ex.ToString();
            }
            return Convert.ToString(success);
        }
        //Shubham[skadam] APIGEOS-1392 Use plant service for Accounting->ExpenseReports->Export  28 04 2025
        public string ExpenseReportsExport(EmployeeExpenseReportExport request)
        {
            bool success = false;
            try
            {
                string IdExpenseReport = request.IdExpenseReport;
                string PlantOwner = request.ParameterPlantOwner;
                string Lang = request.Lang;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;

                string ExpensesFilePath = request.ExpensesFilePath;
                string ExpensesTemplatePath = request.ExpensesTemplatePath;
                string EmployeesExpensesAttachmentPath = request.EmployeesExpensesAttachmentPath;
                string CurrencyLayerAPI = request.CurrencyLayerAPI;

                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring) && !string.IsNullOrEmpty(ExpensesTemplatePath) 
                    && !string.IsNullOrEmpty(EmployeesExpensesAttachmentPath) &&  !string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(IdExpenseReport) && !string.IsNullOrEmpty(ExpensesFilePath))
                {
                    AccountingExpensesManager accountingExpensesManager = new AccountingExpensesManager(MainConn, LoginContext);
                    accountingExpensesManager.CurrencyLayerAPI = CurrencyLayerAPI;
                    success = accountingExpensesManager.ExportExpenses(IdExpenseReport, string.IsNullOrEmpty(Lang) ? "0" : Lang.ToLower(), ExpensesTemplatePath, ExpensesFilePath,//[Plahange][GEOSAPI-614] Change for multiple languages
                   Plantwiseconnectionstring, MainConn, LoginContext, EmployeesExpensesAttachmentPath);
                }
            }
            catch (Exception ex)
            {
                success = false;
                return ex.ToString();
            }
            return Convert.ToString(success);
        }
        //Shubham[skadam] APIGEOS-1394 Use plant service for Accounting->ExpenseReports->Send  28 04 2025
        public SendEmail ExpenseReportsSend(EmployeeExpenseReportExport request)
        {
            bool success = false;
            SendEmailModel SendEmailModel = new SendEmailModel();
            SendEmail sendemail = new SendEmail();
            try
            {
                string IdExpenseReport = request.IdExpenseReport;
                string PlantOwner = request.ParameterPlantOwner;
                string Lang = request.Lang;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;

                string ExpensesFilePath = request.ExpensesFilePath;
                string CurrencyLayerAPI = request.CurrencyLayerAPI;
                string MailServerName = request.ParameterMailServerName;
                string MailServerPort = request.ParameterMailServerPort;
                string MailFrom = request.ParameterMailFrom;
                string Mailpassword = request.ParameterMailpassword;
                string MailUser = request.ParameterMailUser;
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(IdExpenseReport) && !string.IsNullOrEmpty(ExpensesFilePath)
                    && !string.IsNullOrEmpty(Plantwiseconnectionstring))
                {
                    AccountingExpensesManager accountingExpensesManager = new AccountingExpensesManager(MainConn, LoginContext);
                    sendemail = accountingExpensesManager.SendExpense(request.SendMailModel, IdExpenseReport, ExpensesFilePath, MailServerName, MailServerPort,
                        MailFrom, Mailpassword, MailUser, Plantwiseconnectionstring);
                    if (sendemail.error != null)
                    {
                        SendEmailModel.error = sendemail.error;
                    }
                    if (sendemail.warning != null)
                    {
                        SendEmailModel.warning = sendemail.warning;
                    }
                    SendEmailModel.success = true;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return sendemail;
        }
        //Shubham[skadam] APIGEOS-1393 Use plant service for Accounting->ExpenseReports->Sign  28 04 2025
        public async Task<string> ExpenseReportsSign(Stream requestStream)
        {
            bool success = false;
            try
            {
                using (GZipStream gzipStream = new GZipStream(requestStream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(requestBody))
                    {
                        success = false;
                        throw new Exception("Request body is empty.");
                    }
                    EmployeeExpenseReportExport request = JsonConvert.DeserializeObject<EmployeeExpenseReportExport>(requestBody);
                    // Extract parameters
                    string IdExpenseReport = request.IdExpenseReport;
                    string Signatory = request.Signatory;
                    string PlantOwner = request.ParameterPlantOwner;
                    string MainConn = request.ParameterMainConn;
                    string LoginContext = request.ParameterLoginContext;
                    Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;

                    string ExpensesFilePath = request.ExpensesFilePath;
                    string ExpensesTemplatePath = request.ExpensesTemplatePath;
                    string EmployeesExpensesAttachmentPath = request.EmployeesExpensesAttachmentPath;
                    string CurrencyLayerAPI = request.CurrencyLayerAPI;
                    CreateExpenseReportsSign createExpenseReportsSign = request.ParameterSign;
                    TechnicalServiceManager technicalServiceManager = new TechnicalServiceManager(MainConn);
                    if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                        && !string.IsNullOrEmpty(PlantOwner) && !string.IsNullOrEmpty(Signatory) && !string.IsNullOrEmpty(IdExpenseReport))
                    {
                        AccountingExpensesManager accountingExpensesManager = new AccountingExpensesManager(MainConn, LoginContext);
                        success = accountingExpensesManager.ExpenseReportsSign(createExpenseReportsSign, MainConn, LoginContext, IdExpenseReport, ExpensesFilePath, Signatory);
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                return ex.ToString(); 
            }
            return Convert.ToString(success);
        }
        //Shubham[skadam] APIGEOS-1394 Use plant service for Accounting->ExpenseReports->Send  07 05 2025
        public byte[] ExpenseReportsFileBytes(EmployeeExpenseReportExport request)
        {
            if (!string.IsNullOrEmpty(request.FilePath))
            {
                byte[] bytes = null;
                string fileUploadPath = request.FilePath;
                try
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
                    return bytes;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error ExpenseReportsFileBytes(). ErrorMessage- {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return null;
        }

        //[pallavi.jadhav][03 09 2025][APIGEOS-1628]
        public ProductionTimetracking TimetrackingList(TimetrackingList request)
        {
            //  bool result = false;
            ProductionTimetracking TimetrackingList = new ProductionTimetracking();
            TimeTrackingResult timeTrackingResult = new TimeTrackingResult();
            try
            {
                string Plants = request.Plants;
                string From = request.From;
                string To = request.To;
                string Workstage = request.Workstage;
                string Currency = request.Currency;
                MainConn = request.ParameterMainConn;
                LoginContext = request.ParameterLoginContext;
                Plantwiseconnectionstring = request.ParameterPlantwiseconnectionstring;
                PlantwiseconnectionstringSlave = request.Plantwiseconnectionstringslave;

                ProductionManager productionTimetrackingManager = new ProductionManager(MainConn, LoginContext);
                if (!string.IsNullOrEmpty(MainConn) && !string.IsNullOrEmpty(LoginContext) && !string.IsNullOrEmpty(Plantwiseconnectionstring)
                     && !string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To) && !string.IsNullOrEmpty(PlantwiseconnectionstringSlave))
                {
                    timeTrackingResult = productionTimetrackingManager.GetPlantwiseTimetracking(string.IsNullOrEmpty(Plants.ToUpper()) ? "0" : Plants.ToUpper(),
                  string.IsNullOrEmpty(From) ? null : From.ToUpper(), string.IsNullOrEmpty(To) ? null : To.ToUpper(), string.IsNullOrEmpty(Workstage) ? null : Workstage, string.IsNullOrEmpty(Currency.ToUpper()) ? null : Currency.ToUpper(),
                  MainConn, PlantwiseconnectionstringSlave);
                }
                TimetrackingList.TimeTracking = timeTrackingResult.TimeTracking;
            }
            catch (Exception ex)
            {
                // result = false;
                throw;
            }
            return TimetrackingList;
        }


    }
}
