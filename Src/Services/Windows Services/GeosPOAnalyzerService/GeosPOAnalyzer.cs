using DevExpress.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using GeosPOAnalyzerService.Logger;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.SqlServer.Server;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Tls;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using static iTextSharp.text.pdf.AcroFields;
using static System.Net.Mime.MediaTypeNames;

namespace GeosPOAnalyzerService
{
    public partial class GeosPOAnalyzer : ServiceBase
    {
        //[rdixit][GEOS2-5867][03.07.2024]
        #region Services     
        //IOTMService OtmStartUp = new OTMServiceController("localhost:6699");
        IOTMService OtmStartUp = new OTMServiceController(Properties.Settings.Default.SERVICE_PROVIDER_URL);
        //IOTMService OtmStartUp = new OTMServiceController(Properties.Settings.Default.SERVICE_PROVIDER_URL);
        #endregion

        #region Declaration
        private string notFoundFields = string.Empty;
        List<string> missingFields = new List<string>();
        List <LookupValue> poStatusList;
        CustomerDetail poRequest;
        List<Currency> currencyList;
        List<TemplateSetting> templateSettingList;
        private string _PONumber = null;
        private string _Currency = null;
        private DateTime _DateIssued;
        private string _email = null;
        private string _offer = null;
        private string _customer = null;
        private string _pdfName = null;
        List<CustomerDetail> customerDetailList;
        List<Email> emailList;
        List<BlackListEmail> blackListEmailList;
        IList<LookupValue> untrustedExtensionList;
        bool isSavePoReq = false;
        bool isSavePoDetails = false;
        static readonly object PoAnalyzerObject = new object();
        private Timer PoAnalyzerTimer = null;
        List<Customer> customerList;
        List<Country> countriesList;
        List<CustomerDetail> detailList;
        List<CustomerCountriesDetails> customerCountriesDetails;
        List<PORequestDetails> extractedResults;
        CustomerDetail extractedResultsobj;
        List<Email> emailUpdatedList;
        private ObservableCollection<LogEntryByPORequest> poLog;

        //Group - plant
        private ObservableCollection<CustomerContacts> customerContactsList;
        private ObservableCollection<POEmployeeInfo> pOEmployeeInfoList;
        #endregion

        #region Properties
        public string NotFoundFields
        {
            get
            {
                return notFoundFields;
            }
            set
            {
                notFoundFields = value;
            }
        }
        public static string[] DateFormats { get; } = {    
            "dd-MMM-yyyy",    
            "dd/MM/yyyy",    
            "yyyy-MM-dd",    
            "dd-mm-yyyy",   
            "dd MMM yyyy",    
            "yyyy.MM.dd",       
            "dd.mm.yyyy",    
            "d 'de' MMMM 'de' yyyy"};

        public CustomerDetail PoRequest
        {
            get
            {
                return poRequest;
            }
            set
            {
                poRequest = value;
            }
        }

        public CustomerDetail ExtractedResultsobj
        {
            get
            {
                return extractedResultsobj;
            }
            set
            {
                extractedResultsobj = value;

            }
        }

        public List<PORequestDetails> ExtractedResults
        {
            get
            {
                return extractedResults;
            }
            set
            {
                extractedResults = value;

            }
        }
        List<Company> companyList;
        public List<Company> CompanyList
        {
            get
            {
                return companyList;
            }
            set
            {
                companyList = value;
            }
        }
        public List<LookupValue> PoStatusList
        {
            get
            {
                return poStatusList;
            }
            set
            {
                poStatusList = value;
            }
        }
        public List<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }
            set
            {
                currencyList = value;
            }
        }
        public bool isPOOk { get; set; }
        public List<CustomerDetail> CustomerDetailList
        {
            get
            {
                return customerDetailList;
            }
            set
            {
                customerDetailList = value;
            }
        }
        public string PONumber
        {
            get
            {
                return _PONumber;
            }
            set
            {
                _PONumber = value;
            }
        }
        public string Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                _Currency = value;
            }
        }
        public DateTime DateIssued
        {
            get
            {
                return _DateIssued;
            }
            set
            {
                _DateIssued = value;
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string Offer
        {
            get
            {
                return _offer;
            }
            set
            {
                _offer = value;
            }
        }
        public string Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
            }
        }
        public string PdfName
        {
            get
            {
                return _pdfName;
            }
            set
            {
                _pdfName = value;
            }
        }
        public List<Email> EmailList
        {
            get
            {
                return emailList;
            }
            set
            {
                emailList = value;
            }
        }
        public List<BlackListEmail> BlackListEmailList
        {
            get
            {
                return blackListEmailList;
            }
            set
            {
                blackListEmailList = value;
            }
        }
        public IList<LookupValue> UntrustedExtensionList
        {
            get
            {
                return untrustedExtensionList;
            }
            set
            {
                untrustedExtensionList = value;
            }
        }
        public List<TemplateSetting> TemplateSettingList
        {
            get
            {
                return templateSettingList;
            }
            set
            {
                templateSettingList = value;
            }
        }
        public List<Customer> CustomerList
        {
            get
            {
                return customerList;
            }
            set
            {
                customerList = value;
            }
        }
        public List<Country> CountriesList
        {
            get
            {
                return countriesList;
            }
            set
            {
                countriesList = value;
            }
        }
        public List<CustomerDetail> DetailList
        {
            get
            {
                return detailList;
            }
            set
            {
                detailList = value;
            }
        }
        public List<CustomerCountriesDetails> CustomerCountriesDetails
        {
            get
            {
                return customerCountriesDetails;
            }
            set
            {
                customerCountriesDetails = value;
            }
        }
        public List<Email> EmailUpdatedList
        {
            get
            {
                return emailUpdatedList;
            }
            set
            {
                emailUpdatedList = value;
            }
        }
        //Group - Plant
        public ObservableCollection<CustomerContacts> CustomerContactsList
        {
            get
            {
                return customerContactsList;
            }
            set
            {
                customerContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerContactsList"));
            }
        }

        public ObservableCollection<POEmployeeInfo> POEmployeeInfoList
        {
            get
            {
                return pOEmployeeInfoList;
            }
            set
            {
                pOEmployeeInfoList = value;

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Constructor
        public GeosPOAnalyzer()
        {
            InitializeComponent();
            try
            {
                //Log4NetLogger.Logger.Log("GeosPOAnalyzer Constructor Started...", category: Category.Info, priority: Priority.Low);
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.IO.Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }
                PoRequest = new CustomerDetail();
                //AnalyseEmails();
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  GeosPOAnalyzer() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            Log4NetLogger.Logger.Log(string.Format("GeosPOAnalyzer Constructor Executed.... "), category: Category.Info, priority: Priority.Low);

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
                                              <file value=""C:\Temp\Emdep\Geos\GeosPOAnalyzerServiceLogs.txt""/>
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

        internal void InitiaizeOnStartService()
        {
            try
            {
                PoAnalyzerTimer = new Timer();
                this.PoAnalyzerTimer.Interval = Convert.ToInt32(Properties.Settings.Default.INTERVAL_REFRESH);
                this.PoAnalyzerTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.PoAnalyzer_Tick);
                PoAnalyzerTimer.Enabled = true;
            }
            catch (FaultException<ServiceException> ex)
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

        public void AnalyseEmails()
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("AnalyseEmails Method Started.... "), category: Category.Info, priority: Priority.Low);
                FillCustomerContactsList();
                FillPOEmployeeInfo();
                FillAllCompaniesList();
                //FillBlankColumnsEmailList();
                FillUnprocessedEmails();
                FillBlackListEmails();
                FillCurrencyList();
                FillUntrustedExtensionList();
                FillPoStatusList();
                //FillTemplateSettingList();
                //CustomerList = OtmStartUp.GetAllCustomers();//[rdixit][GEOS2-5868][15.10.2024]
                FillCustomerList();

                //CustomerCountriesDetails = OtmStartUp.GetAllCustomersAndCountries_V2600();
                //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
                // Get Offer Code Pattern from settings
                //OtmStartUp = new OTMServiceController("localhost:6699");
                GeosAppSetting offerCodePatternSetting = OtmStartUp.GetGeosAppSettings(145);
                string offerCodePattern = offerCodePatternSetting?.DefaultValue ?? "";
                Regex offerRegex = new Regex(offerCodePattern, RegexOptions.IgnoreCase);

                //[pramod.misal][GEOS2-9601][29-10-2025]
                //OtmStartUp = new OTMServiceController("localhost:6699");
                // Po Code
                GeosAppSetting POCodePatternSetting = OtmStartUp.GetGeosAppSettings(164);
                string POCodePattern = POCodePatternSetting?.DefaultValue ?? "";
                Regex POCodeRegex = new Regex(POCodePattern, RegexOptions.IgnoreCase);

                // Amount
                GeosAppSetting POAmountPatternSetting = OtmStartUp.GetGeosAppSettings(165);
                string POAmountPattern = POAmountPatternSetting?.DefaultValue ?? "";
                Regex POAmountRegex = new Regex(POAmountPattern, RegexOptions.IgnoreCase);

                //
                //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
                //GeosAppSetting AutomaticOTImportForSharedOrders = OtmStartUp.GetGeosAppSettings(157);
                GeosAppSetting AutomaticOTImportForSharedOrders = OtmStartUp.GetGeosAppSettings(159);
                if (poLog==null)
                {
                    poLog = new ObservableCollection<LogEntryByPORequest>();
                }
                
                if (EmailList?.Count > 0)
                {
                    #region Emails
                    foreach (var item in EmailList)
                    {
                        poLog = new ObservableCollection<LogEntryByPORequest>();
                        isPOOk = false;
                        PORequest poReq = new PORequest();
                        try
                        {
                            try
                            {
                                //[rdixit][GEOS2-9020][23.07.2025]
                                if (item.EmailattachmentList != null)
                                {
                                    foreach (Emailattachment attachement in item.EmailattachmentList)
                                    {
                                        //[rdixit][GEOS2-9020][23.07.2025]
                                        //var xmlAttachment = item.EmailattachmentList.FirstOrDefault(i => System.IO.Path.GetExtension(i.AttachmentExtension).ToLower() == ".xml");
                                        if (attachement != null)
                                        {
                                            if (attachement.AttachmentExtension.ToLower() == ".xml")
                                            {
                                                XmlDocument xmlDoc = new XmlDocument();
                                                xmlDoc.LoadXml(attachement.XmlFileText); // Use LoadXml for string input

                                                XmlNode quotationNode = xmlDoc.SelectSingleNode("//Quotation");
                                                XmlNode revisionNode = xmlDoc.SelectSingleNode("//Revision");
                                                XmlNode otNode = xmlDoc.SelectSingleNode("//Ot");
                                                if (quotationNode != null)
                                                {
                                                    //[rdixit][GEOS2-9020][23.07.2025]
                                                    try
                                                    {
                                                        item.IdCustomer = Convert.ToInt32(quotationNode.SelectSingleNode("customer")?.InnerText);
                                                        item.IdPlant = Convert.ToInt32(quotationNode.SelectSingleNode("site")?.InnerText);
                                                        bool isgroupandplantsave = OtmStartUp.UpdatePORequestGroupPlant_V2660(item);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() - OnStart() in UpdatePORequestGroupPlant_V2660. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                    #region Offer
                                                    if (AutomaticOTImportForSharedOrders.DefaultValue == "1")
                                                    {
                                                        CheckInformationFromXMlBodyMailOffer(quotationNode, revisionNode, otNode);
                                                    }
                                                    #endregion

                                                    string QuotationCode = quotationNode.SelectSingleNode("code")?.InnerText;
                                                    if (!string.IsNullOrEmpty(QuotationCode))
                                                    {
                                                        OtmStartUp.AddUniqueOffersToPORequest_V2660(item.IdPORequest, QuotationCode);  //[rdixit][GEOS2-9020][23.07.2025]
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() - OnStart() checking XmlDocument. ErrorMessage- {0}", ex.StackTrace.ToString()), category: Category.Exception, priority: Priority.Low);
                            }

                            string domain = item.SenderEmail?.Split('@')[1].Trim();

                            //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
                            string offerCode = ExtractOfferCode_V2610(item, offerRegex);

                            #region//[pramod.misal][GEOS2-9601][29-10-2025]
                            string PoCode = ExtractPOCode_V2680(item, POCodeRegex);



                            #endregion//
                            //[Rahul.Gdhave][GEOS2-6725][Date:12/02/2025]
                            //if (IsEmailValid(item, domain))
                            //{
                            //BodyMail
                            CheckByBodyMail(item);
                            //Sender
                            CheckBySenderEmail(item);
                            //To Email
                            CheckByTOEmail(item);
                            //CC Email
                            CheckByCCEmail(item);
                            //Domain
                            CheckByDomain(item);

                            // [pramod.misal][GEOS2 -7718][28.03.2025]

                            string PoAmount = ExtractPOAmount_V2680(item, POAmountRegex);
                            // [pramod.misal][GEOS2 -7718][28.03.2025]
                            #region [pramod.misal][GEOS2-][28.03.2025]
                            MatchWithSendername(item);
                            MatchWithToName(item);
                            MatchWithCCName(item);
                            #endregion
                            #region comment By Ashish M task move to blocked
                            //GetTemplatesAndExtractData(item);
                            //if (CustomerDetailList.Count <= 0)
                            //{
                            //    ProcessAttachments(item);
                            //}
                            #endregion

                            ProcessAttachments(item);
                            ChangeLogEmail(item);
                            poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "to do");
                            poReq.IdEmail = item.IdEmail;
                            // Insert Change Log
                            OtmStartUp.AddChangeLogByPORequest_V2660(poLog);
                            isSavePoReq = OtmStartUp.UpdatePORequestStatus(poReq);

                            if (isSavePoReq)

                                Log4NetLogger.Logger.Log(string.Format("Po Attachment status updated in table.... "), category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() for Email id {1}. ErrorMessage- {0}", ex.ToString(), item.IdEmail), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    #endregion
                }
                Log4NetLogger.Logger.Log(string.Format("AnalyseEmails Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() - OnStart(). ErrorMessage- {0}", ex.StackTrace.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-9601][29-10-2025]
        private string ExtractPOAmount_V2680(Email item, Regex POAmountRegex)
        {
            Log4NetLogger.Logger.Log("Method ExtractPOAmount_V2680()...", category: Category.Info, priority: Priority.Low);
            bool POFound = false;

            // Check for Amount in subject
            Match match = POAmountRegex.Match(item.Subject ?? "");

            if (match.Success == true)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    //OtmStartUp = new OTMServiceController("localhost:6699");
                    string Amount = match.Value.Trim();

                    List<LinkedOffers> IdOfferList = null;
                    if (Amount != null)
                    {
                        IdOfferList = OtmStartUp.GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(item.IdCustomer, item.IdPlant, Amount);
                    }
                    List<int> IdOffersList = null;

                    if (IdOfferList != null && IdOfferList.Count > 0)
                    {
                        IdOffersList = IdOfferList.Select(o => (int)o.IdOffer).ToList() ?? new List<int>();
                        //OtmStartUp = new OTMServiceController("localhost:6699");
                        OtmStartUp.OTM_InsertPORequestLinkedOffer_V2680(item.IdPORequest, IdOffersList);
                        POFound = true;

                    }
                }
            }


            var bodyMatch = Regex.Match(item.Body, @"<body[^>]*>(.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string bodyContent = bodyMatch.Success ? bodyMatch.Groups[1].Value : item.Body;

            // ✅ Step 2: Remove all HTML tags
            string plainText = Regex.Replace(bodyContent, "<.*?>", string.Empty);
            plainText = WebUtility.HtmlDecode(plainText);

            List<string> allMatchedAmounts = new List<string>();

            MatchCollection matches = POAmountRegex.Matches(plainText ?? "");

            foreach (Match match1 in matches)
            {
                if (match1.Success && !string.IsNullOrEmpty(match1.Value))
                {
                    string rawAmount = match1.Value.Trim();

                    // Remove currency symbols and keep digits with comma/dot
                    string cleanAmount = Regex.Replace(rawAmount, @"[^\d.,]", "");

                    if (!string.IsNullOrEmpty(cleanAmount))
                    {
                        allMatchedAmounts.Add(cleanAmount);
                    }
                }
            }

            if (allMatchedAmounts.Count > 0)
            {
                foreach (string amount in allMatchedAmounts)
                {
                    if (!string.IsNullOrEmpty(amount))
                    {
                        //OtmStartUp = new OTMServiceController("localhost:6699");

                        // Fetch offers for this specific amount
                        List<LinkedOffers> IdOfferList = OtmStartUp.GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(
                            item.IdCustomer,
                            item.IdPlant,
                            amount
                        );

                        if (IdOfferList != null && IdOfferList.Count > 0)
                        {
                            List<int> IdOffersList = IdOfferList
                                .Select(o => (int)o.IdOffer)
                                .ToList();

                            //OtmStartUp = new OTMServiceController("localhost:6699");
                            OtmStartUp.OTM_InsertPORequestLinkedOffer_V2680(item.IdPORequest, IdOffersList);

                            POFound = true;
                        }
                    }
                }
            }



            if (POFound == false)
            {
                poLog.Add(new LogEntryByPORequest()
                {
                    IdPORequest = item.IdPORequest,
                    IdUser = 164,
                    DateTime = DateTime.Now,
                    Comments = "It is not possible to determine Amount in Subject And Body.",
                    IdLogEntryType = 25,
                    IdEmail = item.IdEmail
                });
            }

            Log4NetLogger.Logger.Log(string.Format("ExtractPOAmount_V2680() Method Executed.... "), category: Category.Info, priority: Priority.Low);
            return match.Success ? match.Value : match.Success ? match.Value : null;



        }

        //[pramod.misal][GEOS2-9601][29-10-2025]
        private string ExtractPOCode_V2680(Email item, Regex PoCodeRegex)
        {
            Log4NetLogger.Logger.Log("Method ExtractPOCode_V2680()...", category: Category.Info, priority: Priority.Low);
            bool POFound = false;
            string PoCodeRegexstring1 = PoCodeRegex.ToString();
            Regex regex1 = new Regex(PoCodeRegexstring1, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);


            // Check for offer code in subject
            Match match = regex1.Match(item.Subject ?? "");

            if (match.Success == true)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    //OtmStartUp = new OTMServiceController("localhost:6699");
                    int idCustomerPurchaseOrders = OtmStartUp.GetPODetails_V2680(match.Value.Trim());
                    List<int> IdOfferList = null;
                    if (idCustomerPurchaseOrders > 0)
                    {
                        IdOfferList = OtmStartUp.GetIdOffersByCustomerPurchaseOrder_V2680(idCustomerPurchaseOrders);
                    }

                    if (IdOfferList != null && IdOfferList.Count > 0)
                    {
                        //OtmStartUp = new OTMServiceController("localhost:6699");
                        OtmStartUp.OTM_InsertPORequestLinkedOffer_V2680(item.IdPORequest, IdOfferList);
                        POFound = true;

                    }
                }
            }

            // Step 2: Extract all <p> tags inside that section
            var paragraphMatches = Regex.Matches(item.Body, @"<p[^>]*>[\s\S]*?<\/p>", RegexOptions.IgnoreCase);
            var bodyMatch = Regex.Match(item.Body, @"<body[^>]*>(.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string bodyContent = bodyMatch.Success ? bodyMatch.Groups[1].Value : item.Body;

            // ✅ Step 2: Remove all HTML tags
            string plainText = Regex.Replace(bodyContent, "<.*?>", string.Empty);
            plainText = WebUtility.HtmlDecode(plainText);


            // Check in email body if not found in subject or attachments

            string PoCodeRegexstring = PoCodeRegex.ToString();
            Regex regex = new Regex(PoCodeRegexstring, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

            // Find all matches

            MatchCollection matches = regex.Matches(plainText);
            var matchedPocode = "";

            foreach (Match match1 in matches)
            {
                matchedPocode = match1.Value;

                if (match1.Success == true)
                {
                    if (!string.IsNullOrEmpty(match1.Value))
                    {
                        //OtmStartUp = new OTMServiceController("localhost:6699");
                        var idCustomerPurchaseOrders = OtmStartUp.GetPODetails_V2680(match1.Value.Trim());
                        List<int> IdOfferList = null;
                        if (idCustomerPurchaseOrders > 0)
                        {
                            IdOfferList = OtmStartUp.GetIdOffersByCustomerPurchaseOrder_V2680(idCustomerPurchaseOrders);
                        }


                        if (IdOfferList != null && IdOfferList.Count > 0)
                        {
                            //OtmStartUp = new OTMServiceController("localhost:6699");
                            OtmStartUp.OTM_InsertPORequestLinkedOffer_V2680(item.IdPORequest, IdOfferList);
                            POFound = true;

                        }
                    }
                }
                if (POFound == false)
                {
                    poLog.Add(new LogEntryByPORequest()
                    {
                        IdPORequest = item.IdPORequest,
                        IdUser = 164,
                        DateTime = DateTime.Now,
                        Comments = "It is not possible to determine PO Code in Subject And Body.",
                        IdLogEntryType = 25,
                        IdEmail = item.IdEmail
                    });
                }


            }




            Log4NetLogger.Logger.Log(string.Format("ExtractPOCode_V2680() Method Executed.... "), category: Category.Info, priority: Priority.Low);
            return matchedPocode;



        }


        void ProcessExtractedAttachments(List<CustomerDetail> CustomerDetailList)
        {
            try
            {
 
                Log4NetLogger.Logger.Log(string.Format("ProcessExtractedAttachments Method Started.... "), category: Category.Info, priority: Priority.Low);

                //foreach (var attach in item.EmailattachmentList)
                //{
                //    try
                //    {
                //        if (attach.AttachmentExtension?.ToLower() == ".pdf" && attach.FileText != null)
                //        {


                //        }

                //        else if ((attach.AttachmentExtension?.ToLower() == ".xls" || attach.AttachmentExtension?.ToLower() == ".xlsx") && attach.ExcelFileText != null)
                //        {



                //        }
                //    }
                //    catch (Exception ex) { }
                //}

                if (CustomerDetailList?.Count > 0 && isPOOk == true)
                {
                    //isSavePoDetails = OtmStartUp.AddPODetails_V2570(CustomerDetailList);
                    if (isSavePoDetails)
                        isPOOk = true;
                }
                Log4NetLogger.Logger.Log(string.Format("ProcessExtractedAttachments Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                isPOOk = false;
                Log4NetLogger.Logger.Log(string.Format("ERROR in  ProcessExtractedAttachments(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[rdixit][GEOS2-5868][15.10.2024]
        bool IsEmailValid(Email item, string domain)
        {
            try
            {
                Log4NetLogger.Logger.Log("IsEmailValid Method Started....", category: Category.Info, priority: Priority.Low);

                // Pre-trim values to avoid redundant trimming
                var senderEmail = item.SenderEmail?.Trim();
                var trimmedDomain = domain?.Trim();

                // Consolidate all conditions into a single if statement
                if (BlackListEmailList.Any(i => i.SenderEmail?.Trim() == senderEmail || i.Domain?.Trim() == trimmedDomain) ||
                    item.EmailattachmentList == null || item.EmailattachmentList.Count == 0 ||
                    item.EmailattachmentList.Any(attachment => UntrustedExtensionList.Any(untrusted => untrusted.Value?.Trim() == attachment.AttachmentExtension?.Trim())))
                {
                    return false;
                }

                Log4NetLogger.Logger.Log("IsEmailValid Method Executed....", category: Category.Info, priority: Priority.Low);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"ERROR in IsEmailValid(). ErrorMessage- {ex}", category: Category.Exception, priority: Priority.Low);
                return false;                
                throw;
            }
        }
        //[rdixit][GEOS2-5868][15.10.2024]
        void ProcessAttachments(Email item)
        {
            try
            {
                
                NotFoundFields = string.Empty;
                CustomerDetailList = new List<CustomerDetail>();
                Log4NetLogger.Logger.Log(string.Format("ProcessAttachments Method Started.... "), category: Category.Info, priority: Priority.Low);
                //OtmStartUp = new OTMServiceController("localhost:6699");
                //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
                GeosAppSetting offerCodePatternSetting = OtmStartUp.GetGeosAppSettings(145);
                string offerCodePattern = offerCodePatternSetting?.DefaultValue ?? "";
                Regex offerRegex = new Regex(offerCodePattern, RegexOptions.IgnoreCase);
                if (item.EmailattachmentList!=null)
                {
                    Log4NetLogger.Logger.Log(string.Format("Find email EmailattachmentList... count - {0}", item.EmailattachmentList.Count), category: Category.Info, priority: Priority.Low);
                    foreach (var attach in item.EmailattachmentList)
                    {
                        try
                        {
                            //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
                            bool offerCodeFound = false;
                            Match match = offerRegex.Match(attach.AttachmentName);
                            if (match.Success)
                            {
                                string offerCode = match.Value;
                                //OtmStartUp = new OTMServiceController("localhost:6699");
                                var offerDetails = OtmStartUp.GetOfferDetails_V2620(offerCode);
                                if (offerDetails != null)
                                {
                                    int? idOffer = offerDetails;
                                    if (idOffer > 0)
                                    {
                                        //OtmStartUp = new OTMServiceController("localhost:6699");
                                        OtmStartUp.OTM_InsertPORequestLinkedOffer_V2620(item.IdPORequest, idOffer);
                                        offerCodeFound = true;
                                        Log4NetLogger.Logger.Log(string.Format($"Inserted Linked Offer: PORequest {item.IdPORequest}, Offer {idOffer}"), category: Category.Info, priority: Priority.Low);
                                    }
                                }
                            }
                            //End
                            if (attach.AttachmentExtension?.ToLower() == ".pdf" && attach.FileText != null)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Find PDF File"), category: Category.Info, priority: Priority.Low);
                                //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
                                Match match1 = offerRegex.Match(attach.FileText);
                                if (match1.Success)
                                {
                                    string offerCode = match1.Value;
                                    //OtmStartUp = new OTMServiceController("localhost:6699");
                                    var offerDetails = OtmStartUp.GetOfferDetails_V2620(offerCode);
                                    if (offerDetails != null)
                                    {
                                        int? idOffer = offerDetails;
                                        if (idOffer > 0)
                                        {
                                            //OtmStartUp = new OTMServiceController("localhost:6699");
                                            OtmStartUp.OTM_InsertPORequestLinkedOffer_V2620(item.IdPORequest, idOffer);
                                            offerCodeFound = true;
                                            Log4NetLogger.Logger.Log(string.Format($"Inserted Linked Offer: PORequest {item.IdPORequest}, Offer {idOffer}"), category: Category.Info, priority: Priority.Low);
                                        }
                                    }
                                }
                                if (offerCodeFound == false)
                                {
                                    poLog.Add(new LogEntryByPORequest()
                                    {
                                        IdPORequest = item.IdPORequest,
                                        IdUser = 164,
                                        DateTime = DateTime.Now,
                                        Comments = "It is not possible to determine Offer Code in AttachmentName And FileText.",
                                        IdLogEntryType = 25,
                                        IdEmail = item.IdEmail
                                    });
                                }
                                if (attach.FileText == null || attach.FileText == "")
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Check FileText is not extracted..."), category: Category.Info, priority: Priority.Low);
                                }
                                Log4NetLogger.Logger.Log(string.Format("Check Matching customer..."), category: Category.Info, priority: Priority.Low);
                                Log4NetLogger.Logger.Log(string.Format("check customerList... count - {0}", customerList.Count), category: Category.Info, priority: Priority.Low);
                                //string pdfText = attach.FileText;
                                List<Customer> MatchingCustomerList = customerList.Where(i => Regex.IsMatch(attach.FileText, $@"\b{i.CustomerName}\b", RegexOptions.IgnoreCase)).ToList();
                                if (MatchingCustomerList == null)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("check MatchingCustomerList is null"), category: Category.Info, priority: Priority.Low);
                                }
                                if (MatchingCustomerList != null)
                                {
                                    Log4NetLogger.Logger.Log( string.Format("Matching customer found started checking Raw text"), category: Category.Info, priority: Priority.Low);

                                    DetailList = new List<CustomerDetail>();
                                    if (!string.IsNullOrEmpty(attach.FileText))
                                        CheckRawTExt(MatchingCustomerList, attach.FileText);
                                    if (!string.IsNullOrEmpty(attach.LocationFileText))
                                        CheckRawTExt(MatchingCustomerList, attach.LocationFileText);

                                    Log4NetLogger.Logger.Log(string.Format("Matching customer found  checking Completed"),category: Category.Info, priority: Priority.Low);
                                    //if (DetailList?.Count > 0)
                                    //    CustomerDetailList.Add(CalculatePercentage(item));
                                    if (DetailList?.Count > 0)
                                    {
                                        Log4NetLogger.Logger.Log(string.Format("Started  calculating Percentage"),category: Category.Info, priority: Priority.Low);
                                        var customerDetail = CalculatePercentage(item);
                                        ChangelogMethod(item, attach);
                                        customerDetail.FileText = attach.FileText;
                                        customerDetail.IdAttachment = attach.IdAttachment;
                                        CustomerDetailList.Add(customerDetail);
                                    }

                                    Log4NetLogger.Logger.Log(
                                     string.Format("CustomerDetailList count is = {0}", CustomerDetailList.Count),
                                     category: Category.Info,
                                     priority: Priority.Low);


                                }
                            }
                        }
                        catch (Exception ex) 
                        {
                            Log4NetLogger.Logger.Log(string.Format("ProcessAttachments failed due to {0}", ex.StackTrace),category: Category.Info, priority: Priority.Low);
                        }
                    } 
                }
                if (CustomerDetailList?.Count > 0 && isPOOk == true)
                {
                    Log4NetLogger.Logger.Log(string.Format("start of Po final result insertion... "), category: Category.Info, priority: Priority.Low);
                    //isSavePoDetails = OtmStartUp.AddPODetails_V2570(CustomerDetailList);//[rdixit][10.10.2024][GEOS2-5868]
                    isSavePoDetails = OtmStartUp.AddPODetails_V2610(CustomerDetailList);   //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                    if (isSavePoDetails)
                    {
                        try
                        {
                            Log4NetLogger.Logger.Log(string.Format("start of Po final result insertion completed... "), category: Category.Info, priority: Priority.Low);

                            isPOOk = true;
                            //[pooja.jadhav][GEOS2-8342][11-06-2025]
                            //[rdixit][GEOS2-9137][12.08.2025]
                            //OtmStartUp.AddChangeLogByPORequest_V2660(poLog);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in  ProcessAttachments() LogMethod. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                Log4NetLogger.Logger.Log(string.Format("ProcessAttachments Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                isPOOk = false;
                Log4NetLogger.Logger.Log(string.Format("ERROR in  ProcessAttachments(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[rdixit][GEOS2-9137][12.08.2025]
        void ChangelogMethod(Email item, Emailattachment attachment)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("ChangelogMethod Method Started.... "), category: Category.Info, priority: Priority.Low);
                var MaxPercentagePo = DetailList.OrderByDescending(i => i.Percentage).FirstOrDefault();
                MaxPercentagePo.IdPORequest = item.IdPORequest;
                var missingFields = new List<string>();

                if (string.IsNullOrEmpty(MaxPercentagePo.PONumber) || MaxPercentagePo.PONumber == "0")
                    missingFields.Add("PO Number");

                if (MaxPercentagePo.TotalNetValue == 0)
                    missingFields.Add("Amount");

                if (string.IsNullOrEmpty(MaxPercentagePo.Incoterm))
                    missingFields.Add("Incoterm");

                if (string.IsNullOrEmpty(MaxPercentagePo.Offer) || MaxPercentagePo.Offer == "0")
                    missingFields.Add("Offer");

                if (string.IsNullOrEmpty(MaxPercentagePo.Currency))
                    missingFields.Add("Currency");

                if (MaxPercentagePo.DateIssued == DateTime.MinValue)
                    missingFields.Add("Date Issued");

                if (string.IsNullOrEmpty(MaxPercentagePo.ShipTo))
                    missingFields.Add("Ship To");

                if (string.IsNullOrEmpty(MaxPercentagePo.InvoiceTo))
                    missingFields.Add("Invoice To");

                if (string.IsNullOrEmpty(MaxPercentagePo.PaymentTerms))
                    missingFields.Add("Payment Terms");

                // Join missing fields with commas (no leading/trailing commas)
                string notFoundFields = missingFields.Any() ? string.Join(", ", missingFields) : "";
                bool multipleFields = missingFields.Count > 1;

                // Log the error (if any fields are missing)
                if (!string.IsNullOrEmpty(notFoundFields))
                {
                    poLog.Add(new LogEntryByPORequest()
                    {
                        IdPORequest = item.IdPORequest,
                        IdUser = 164,
                        DateTime = DateTime.Now,
                        FileName = attachment.AttachmentName,
                        IdEmail = item.IdEmail,
                        Comments = $"It was not possible to find the data for the {(multipleFields ? "fields " : "field ")}{notFoundFields} in the document.",
                        //Comments = $"It was not possible to find the data for the {(multipleFields ? "fields " : "field ")}{notFoundFields} in the document {attachment.AttachmentName}",
                        IdLogEntryType = 25
                    });
                }
                Log4NetLogger.Logger.Log(string.Format("ChangelogMethod Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  ChangelogMethod(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void ChangeLogEmail(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("ChangeLogEmail Method Started.... "), category: Category.Info, priority: Priority.Low);
                if (string.IsNullOrEmpty(item.CCEmail) && string.IsNullOrEmpty(item.CCName))
                {
                    poLog.Add(new LogEntryByPORequest()
                    {
                        IdPORequest = item.IdPORequest,
                        IdUser = 164,
                        DateTime = DateTime.Now,
                        Comments = "CC Recipients data not available in email",
                        IdLogEntryType = 25,
                        IdEmail = item.IdEmail
                    });
                }
                if (string.IsNullOrEmpty(item.Subject) && string.IsNullOrEmpty(item.Subject))
                {
                    poLog.Add(new LogEntryByPORequest()
                    {
                        IdPORequest = item.IdPORequest,
                        IdUser = 164,
                        DateTime = DateTime.Now,
                        Comments = "Subject data not available in email",
                        IdLogEntryType = 25,
                        IdEmail = item.IdEmail
                    });
                }
                Log4NetLogger.Logger.Log(string.Format("ChangeLogEmail Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  ChangeLogEmail(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public bool GetTemplatesAndExtractData(Email email)
        {
            bool isTemplate = false;
            try
            {
                CustomerDetailList = new List<CustomerDetail>();
                Log4NetLogger.Logger.Log(string.Format("GetTemplatesAndExtractData Method Started.... "), category: Category.Info, priority: Priority.Low);
                foreach (var attach in email.EmailattachmentList)
                {
                    List<CustomerCountriesDetails> FinalMatchingCustomerList = new List<CustomerCountriesDetails>();
                    try
                    {
                        if ((attach.AttachmentExtension?.ToLower() == ".pdf" && attach.FileText != null) ||((attach.AttachmentExtension?.ToLower() == ".xls" || attach.AttachmentExtension?.ToLower() == ".xlsx") && attach.ExcelFileText!=null))
                        {
                            if (attach.AttachmentExtension?.ToLower() == ".pdf")
                            {
                                List<CustomerCountriesDetails> MatchingCustomerList = CustomerCountriesDetails
                                .Where(i => Regex.IsMatch(attach.FileText, $@"\b{i.CustomerName}\b.*\b{i.Countries}\b|\b{i.Countries}\b.*\b{i.CustomerName}\b", RegexOptions.IgnoreCase) &&
                                            i.Site.Any(site => Regex.IsMatch(attach.FileText, $@"\b{site}\b", RegexOptions.IgnoreCase)))
                                .ToList();

                                if (MatchingCustomerList != null)
                                {
                                    DetailList = new List<CustomerDetail>();
                                    CheckTemplatesRawData(MatchingCustomerList, attach);

                                    //if (DetailList?.Count > 0)
                                    //    CustomerDetailList.Add(CalculatePercentageByExtractingTemplate(email));
                                    //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                                    if (DetailList?.Count > 0)
                                    {
                                        var customerDetail = CalculatePercentageByExtractingTemplate(email);
                                        customerDetail.FileText = attach.FileText;
                                        customerDetail.IdAttachment = attach.IdAttachment;
                                        CustomerDetailList.Add(customerDetail);
                                    }
                                }
                            }
                            //[pramod.misal][GEOS2-6735][23.01.2025]
                            else if ((attach.AttachmentExtension?.ToLower() == ".xls" || attach.AttachmentExtension?.ToLower() == ".xlsx") && attach.ExcelFileText != null)
                            {

                                List<CustomerCountriesDetails> MatchingCustomerList = CustomerCountriesDetails
                                            .Where(i =>
                                                Regex.IsMatch(attach.ExcelFileText,
                                                    $@"\b{i.CustomerName}\b", RegexOptions.IgnoreCase) &&
                                                Regex.IsMatch(attach.ExcelFileText,
                                                    $@"\b{i.Countries}\b", RegexOptions.IgnoreCase) &&
                                                i.Site.Any(site =>
                                                    Regex.IsMatch(attach.ExcelFileText,
                                                        $@"\b{site}\b", RegexOptions.IgnoreCase))).ToList();



                                //[pramod.misal][GEOS2-6735][23.01.2025]
                                if (MatchingCustomerList != null)
                                {
                                    DetailList = new List<CustomerDetail>();
                                    CheckExcelTemplatesRawData(MatchingCustomerList, attach);

                                    //if (DetailList?.Count > 0)
                                    //    CustomerDetailList.Add(CalculatePercentageByExtractingTemplate(email));
                                    //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                                    if (DetailList?.Count > 0)
                                    {
                                        var customerDetail = CalculatePercentageByExtractingTemplate(email);
                                        customerDetail.FileText = attach.ExcelFileText;       
                                        customerDetail.IdAttachment = attach.IdAttachment;           
                                        CustomerDetailList.Add(customerDetail);                     
                                    }

                                }
                               
                            }
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
                if (CustomerDetailList?.Count > 0 && isPOOk == true)
                {
                    //isSavePoDetails = OtmStartUp.AddPODetails_V2570(CustomerDetailList);
                    //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
                    isSavePoDetails = OtmStartUp.AddPODetails_V2610(CustomerDetailList);
                    if (isSavePoDetails)
                        isPOOk = true;
                }
                Log4NetLogger.Logger.Log(string.Format("ProcessAttachments Method Executed.... "), category: Category.Info, priority: Priority.Low);
                return isTemplate;
            }
            catch(Exception ex)
            {
                return isTemplate;
                Log4NetLogger.Logger.Log(string.Format("ERROR in  GetTemplatesAndExtractData(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[pramod.misal][GEOS2-6735][23.01.2025]
        private void ExcelTemplateDetailsExtraction(CustomerCountriesDetails cust, Emailattachment attachemnt, POAnalyzerOTTemplate Temp)
        {
            CustomerDetail PoRequest = new CustomerDetail();
            CustomerCountriesDetails custDetails = new CustomerCountriesDetails();         
            ExtractedResults = new List<PORequestDetails>();            
            Workbook workbook = new Workbook();
            workbook.LoadDocument(attachemnt.AttachmentPath);       
            Worksheet worksheet = workbook.Worksheets[0];
            try
            {
               
                foreach (ExcelFileTemplateValue excelValue in Temp.ExcelRangeValue)
                {                  
                    string range = excelValue.Range; 
                    string keyword = excelValue.Keyword; 
                    string delimiter = excelValue.Delimiter; 

                    string[] individualRanges = range.Split(';');
                    foreach (string individualRange in individualRanges)
                    {
                        if (!string.IsNullOrEmpty(individualRange))
                        {
                            CellRange cellRange = worksheet.Range[individualRange.Trim()];

                            foreach (Cell cell in cellRange)
                            {
                                string cellValue = cell.DisplayText;

                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    string normalizedCellValue = cellValue.Replace("\n", "").Replace("\r", "").Trim();
                                    string normalizedKeyword = keyword.Trim();
                                    normalizedCellValue = normalizedCellValue.Replace(".", "").Replace(":", "");
                                    normalizedKeyword = normalizedKeyword.Replace(".", "").Replace(":", "");
                                    bool IskeywordPresnt = normalizedCellValue.Contains(normalizedKeyword);
                                    bool IsdelimiterPresnt = cellValue.Contains(delimiter);

                                    if (IskeywordPresnt && IsdelimiterPresnt)
                                    {
                                        int keywordIndex = normalizedCellValue.IndexOf(normalizedKeyword, StringComparison.OrdinalIgnoreCase);
                                        int delimiterIndex = cellValue.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);

                                        // Ensure that the keyword comes before the delimiter
                                        if (keywordIndex < delimiterIndex)
                                        {
                                            //"EMDEP 2, S.L."
                                            string extractedPart = cellValue.Substring(keywordIndex, delimiterIndex - keywordIndex + delimiter.Length);

                                            if (excelValue.FieldValue == "PO_Number")
                                            {
                                                poRequest.PONumber = extractedPart.Replace("\n", "").Replace(":", "").Replace(".", "");

                                            }
                                            if (excelValue.FieldValue == "Customer")
                                            {
                                                poRequest.Customer = extractedPart;

                                            }
                                            if (excelValue.FieldValue == "Email")
                                            {
                                                poRequest.Email = extractedPart;

                                            }
                                            if (excelValue.FieldValue == "Total")
                                            {
                                                poRequest.TotalNetValue = Convert.ToDouble(extractedPart);

                                            }
                                            if (excelValue.FieldValue == "Incoterm")
                                            {
                                                poRequest.IncotermName = extractedPart;

                                            }
                                            if (excelValue.FieldValue == "Date")
                                            {
                                                poRequest.DateIssued = DateTime.ParseExact(extractedPart, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                                            }
                                            if (excelValue.FieldValue == "Currency")
                                            {
                                                poRequest.Currency = extractedPart;

                                            }
                                            if (excelValue.FieldValue == "Ship To")
                                            {
                                                poRequest.ShipTo = extractedPart;

                                            }
                                            if (excelValue.FieldValue == "Payment Terms")
                                            {
                                                poRequest.PaymentTypeName = extractedPart;

                                            }

                                        }
                                    }
                                }
                            }

                        }
                        

                    }

                   // ExtractedResultsobj = poRequest;
                    
                   
                }
                DetailList.Add(poRequest);

            }
            catch (Exception ex)
            {
                isPOOk = false;
                Log4NetLogger.Logger.Log(string.Format("ERROR in ExcelTemplateDetailsExtraction(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }


        }

        //[pramod.misal][GEOS2-6735][23.01.2025]
        public void CheckExcelTemplatesRawData(List<CustomerCountriesDetails> MatchingCustomerList, Emailattachment excelText)
        {
            try
            {
                List<CustomerCountriesDetails> DistinctMatchingCustomerList = MatchingCustomerList
                                            .GroupBy(i => i.IdCustomer)
                                            .Select(g => g.First())  // Select the first element of each group
                                            .ToList();
                List<POAnalyzerOTTemplate> otTemplateList = new List<POAnalyzerOTTemplate>();

                foreach (var customer in DistinctMatchingCustomerList)
                {
                    List<POAnalyzerOTTemplate> tempOtTemplateList = OtmStartUp.GetOTRequestExcelTemplateByCustomer_V2600(customer.IdCustomer);
                    otTemplateList.AddRange(tempOtTemplateList);
                }

                List<POAnalyzerOTTemplate> combinedMatchingList = MatchingCustomerList
                        .SelectMany(customer => otTemplateList
                        .Where(ot =>
                       (ot.Countries == "All" && ot.Region == "All" && ot.Plant == "All") ||  // Handle "All" case
                       (ot.Countries != "All" && ot.Countries == customer.Countries) &&           // Handle specific customer match
                       (ot.Region != "All" && ot.Region == customer.Region) &&
                       (ot.Plant != "All" && ot.Plant == customer.Site) &&
                       ot.IdCustomer == customer.IdCustomer
                         )
                           )
                          .GroupBy(ot => new { ot.IdCustomer, ot.Countries, ot.Region, ot.Plant })  // Group by unique fields
                           .Select(g => g.First())  // Select the first item from each group (removes duplicates)
                           .ToList();


                if (combinedMatchingList.Count > 0)
                {
                    foreach (var customer in MatchingCustomerList)
                    {
                        if (otTemplateList != null)
                        {
                            foreach (var SelectedTemplate in combinedMatchingList)
                            {
                                if (SelectedTemplate != null)
                                {
                                    ExcelTemplateDetailsExtraction(customer, excelText, SelectedTemplate);
                                    isPOOk = true;
                                }
                            }
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  CheckTemplatesRawData(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }

        
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public void CheckTemplatesRawData(List<CustomerCountriesDetails> MatchingCustomerList, Emailattachment pdfText)
        {
            try
            {
                List<CustomerCountriesDetails> DistinctMatchingCustomerList = MatchingCustomerList
                .GroupBy(i => i.IdCustomer)
                .Select(g => g.First())  // Select the first element of each group
                .ToList();
                List<POAnalyzerOTTemplate> otTemplateList = new List<POAnalyzerOTTemplate>();
                foreach (var customer in DistinctMatchingCustomerList)
                {
                     otTemplateList = OtmStartUp.GetOTRequestTemplateByCustomer_V2600(customer.IdCustomer);
                }

                List<POAnalyzerOTTemplate> combinedMatchingList = MatchingCustomerList
                .SelectMany(customer => otTemplateList
                    .Where(ot =>
                        (ot.Countries == "All" && ot.Region == "All" && ot.Plant == "All") ||  // Handle "All" case
                        (ot.Countries != "All" && ot.Countries == customer.Countries) &&           // Handle specific customer match
                        (ot.Region != "All" && ot.Region == customer.Region) &&
                        (ot.Plant != "All" && ot.Plant == customer.Site) &&
                        ot.IdCustomer == customer.IdCustomer
                    )
                )
               .GroupBy(ot => new { ot.IdCustomer, ot.Countries, ot.Region, ot.Plant })  // Group by unique fields
                .Select(g => g.First())  // Select the first item from each group (removes duplicates)
                .ToList();
                if (combinedMatchingList.Count > 0)
                {
                    foreach (var customer in DistinctMatchingCustomerList)
                    {
                        if (otTemplateList != null)
                        {
                            foreach (var SelectedTemplate in combinedMatchingList)
                            {
                                if (SelectedTemplate != null)
                                {
                                    TemplateDetailsExtraction(customer, pdfText, SelectedTemplate);
                                    isPOOk = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  CheckTemplatesRawData(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void CheckRawTExt(List<Customer> MatchingCustomerList, string pdfText)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("CheckRawTExt started.. "), category: Category.Info, priority: Priority.Low);

                foreach (var customer in MatchingCustomerList)
                {
                    List<TemplateSetting> templatesettingList = OtmStartUp.GetTemplateByCustomer(customer.IdCustomer);
                    if (templatesettingList != null)
                    {
                        foreach (var SelectedTemplate in templatesettingList)
                        {
                            if (SelectedTemplate != null)
                            {
                                DetailExtraction(customer, pdfText, SelectedTemplate);
                                isPOOk = true;
                            }
                        }
                    }
                }

                Log4NetLogger.Logger.Log(string.Format("CheckRawTExt completed.. "), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                isPOOk = false;
                Log4NetLogger.Logger.Log(string.Format("ERROR in  ProcessAttachments(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void TemplateDetailsExtraction(CustomerCountriesDetails cust, Emailattachment attchedFile,POAnalyzerOTTemplate Temp)
        {
            CustomerCountriesDetails custDetails = new CustomerCountriesDetails();
            try
            {
                CustomerDetail detail = new CustomerDetail();
                PdfReader reader = GetPdfReader(attchedFile);
                foreach (TextFileTemplateValue textFile in Temp.TextValue)
                {
                    if (textFile.FieldTypeValue == "Text")
                    {
                        switch (textFile.FieldValue.ToLower())
                        {
                            case "po_number":
                             detail.PONumber = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "customer":
                                detail.Customer = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "email":
                                string email = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                try
                                {
                                    string pattern1 = $@"\b[A-Za-z0-9._%+-]+@{Regex.Escape(detail.Customer)}\b";
                                    Regex regex = new Regex(pattern1, RegexOptions.IgnoreCase);
                                    MatchCollection matches = regex.Matches(email);
                                    foreach (Match match1 in matches)
                                    {
                                        detail.Email = match1.Value;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("ERROR in TemplateDetailsExtraction(). email ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                }
                                break;
                            case "currency":
                                detail.Currency = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "total":
                                detail.TotalNetValue = Convert.ToDouble(ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader));
                                break;
                            case "incoterm":
                                detail.Incoterm = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "date":
                                detail.DateIssued = Convert.ToDateTime(ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader));
                                break;
                            case "offer":
                                detail.Offer = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "ship to":
                                detail.ShipTo = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "invoice to":
                                detail.InvoiceTo = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "invoice address":
                                detail.InvoiceAddress = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "payment terms":
                                detail.PaymentTypeName = ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                                break;
                            default:
                                break;
                        }
                        // ExtractedDataPDFByTextFields(cust, textFile, Temp, attchedFile, reader);
                    }
                    else if(textFile.FieldTypeValue == "Location")
                    {
                        switch (textFile.FieldValue.ToLower())
                        {
                            case "po_number":
                                detail.PONumber = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "customer":
                                detail.Customer = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "email":
                                detail.Email = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "Currency":
                                detail.Currency = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "total":
                                detail.TotalNetValue = Convert.ToDouble(ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader));
                                break;
                            case "incoterm":
                                detail.Incoterm = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "date":
                                detail.DateIssued = Convert.ToDateTime(ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader));
                                break;
                            case "offer":
                                detail.Offer = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "ship to":
                                detail.ShipTo = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "invoice to":
                                detail.InvoiceTo = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "invoice address":
                                detail.InvoiceAddress = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            case "payment terms":
                                detail.PaymentTypeName = ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                                break;
                            default:
                                break;
                        }
                        // ExtractedDataPDFByLocations(cust, textFile, Temp, attchedFile, reader);
                    }
                }
                DetailList.Add(detail);
            }
            catch(Exception ex)
            {
                isPOOk = false;
                Log4NetLogger.Logger.Log(string.Format("ERROR in  TemplateDetailsExtraction(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);

            }
        }
        private PdfReader GetPdfReader(Emailattachment attchedFile)
        {
            string docsPath = OtmStartUp.GetPdfFilePath(attchedFile);
            string fileUploadPath = System.IO.Path.Combine(docsPath, attchedFile.IdEmail.ToString(), attchedFile.AttachmentName);
            if (File.Exists(fileUploadPath))
            {
                docsPath = @"C:\EAESFS01\geos$\Workbench\Documents\EmailAttachments\6958\10.pdf";
                PdfReader reader = new PdfReader(docsPath);
                return reader;
            }
            else
                return null;
        }
        private string ExtractedDataPDFByTextFields(CustomerCountriesDetails cust, TextFileTemplateValue textFile, POAnalyzerOTTemplate Temp, Emailattachment attchFile, PdfReader reader)
        {
            string value = "";
            try
            {
                StringBuilder text = new StringBuilder();
                bool isExtracting = false;
                if (textFile.FieldTypeValue == "Text")
                {
                    string startKeyword = textFile.KeywordAndCoordinatesValues;  // Keyword to start extracting text from
                    string endKeyword = textFile.Delimiter; // Delimiter keyword to stop extraction
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(reader, i, strategy);

                        // Look for the start keyword and stop when we find the end keyword
                        if (currentText.ToLower().Contains(startKeyword.ToLower()))
                        {
                            isExtracting = true; // Start extracting after finding startKeyword
                            //currentText = currentText.Substring(currentText.IndexOf(startKeyword) + startKeyword.Length); // Skip the start keyword
                            currentText = currentText.Substring(currentText.ToLower().IndexOf(startKeyword.ToLower())); // Start from the startKeyword
                        }
                        if (isExtracting)
                        {
                            // Append text while we're between the keywords
                            text.Append(currentText);

                            // Stop extracting when we encounter the end keyword
                            if (currentText.ToLower().Contains(endKeyword.ToString().ToLower().Trim()))
                            {
                                int endIndex = currentText.ToLower().IndexOf(endKeyword.ToLower()) + endKeyword.Length; // Include the endKeyword in the extraction
                                text.Remove(text.Length - (currentText.Length - endIndex), currentText.Length - endIndex); // Remove everything after the endKeyword
                                break; // Stop extracting after finding the end keyword
                                //int endIndex = currentText.IndexOf(endKeyword);
                                //text.Remove(text.Length - (currentText.Length - endIndex), currentText.Length - endIndex);

                                //text.Length = text.ToString().IndexOf(endKeyword); // Truncate text after the end keyword
                            }
                        }
                    }
                    value = text.ToString();
                    if (textFile.FieldValue.ToLower() == "currency")
                    {
                        string pattern = @"\b(?:EUR|RON|USD|MXN|CNY|HNL|BRL|TND|MAD|PYG|RUB|INR|GBP|CAD|CHF|RSD|NIO|EGP|IDR)\b";
                        Match match = Regex.Match(value, pattern);
                        if (match.Success)
                        {
                            if (match.Value.Equals("Euro", StringComparison.OrdinalIgnoreCase))
                            {
                                value = "EUR";
                            }
                            value = match.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in ExtractedDataPDFByTextFields(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return value;
        }
        private string ExtractedDataPDFByLocations(CustomerCountriesDetails cust, TextFileTemplateValue textFile, POAnalyzerOTTemplate Temp, Emailattachment attchFile, PdfReader reader)
        {
            string value = "";
            try
            {
                if (textFile.FieldTypeValue == "Location")
                {
                    string location = textFile.KeywordAndCoordinatesValues;
                    string[] coordinates = location.Split(';');

                    // Extract x and y values from the coordinate pairs
                    int x1 = int.Parse(coordinates[0].Split(',')[0].Trim());
                    int y1 = int.Parse(coordinates[0].Split(',')[1].Trim());
                    int x2 = int.Parse(coordinates[1].Split(',')[0].Trim());
                    int y2 = int.Parse(coordinates[1].Split(',')[1].Trim());
                    int x3 = int.Parse(coordinates[2].Split(',')[0].Trim());
                    int y3 = int.Parse(coordinates[2].Split(',')[1].Trim());
                    int x4 = int.Parse(coordinates[3].Split(',')[0].Trim());
                    int y4 = int.Parse(coordinates[3].Split(',')[1].Trim());

                    int x = x1;  // X-coordinate (from the bottom-left corner)
                    int y = y1;  // Y-coordinate (from the bottom-left corner)
                    int width = x2 - x1; // Width of the rectangle
                    int height = y3 - y1; // height of the rectangle

                    //float x = (float)439.15;  // X-coordinate (from the bottom-left corner)
                    //float y = (float)744.62;  // Y-coordinate (from the bottom-left corner)
                    //float width = (float)479.62; // Width of the rectangle
                    //float height = (float)754.52; // height of the rectangle

                    Rectangle rect = new Rectangle(x,y,width,height);

                    RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                    ITextExtractionStrategy strategy;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                        sb.AppendLine(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                    }
                   value = sb.ToString();
                    if (textFile.FieldValue == "Currency")
                    {
                        string pattern = @"\b(?:EUR|RON|USD|MXN|CNY|HNL|BRL|TND|MAD|PYG|RUB|INR|GBP|CAD|CHF|RSD|NIO|EGP|IDR)\b";
                        Match match = Regex.Match(value, pattern);
                        if (match.Success)
                        {
                            if (match.Value.Equals("Euro", StringComparison.OrdinalIgnoreCase))
                            {
                                value = "EUR";
                            }
                            value = match.Value;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in ExtractedDataPDFByLocations(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return value;
        }
        private void DetailExtraction(Customer cust, string extracted_text, TemplateSetting xml_config)
        {
            Log4NetLogger.Logger.Log(string.Format("DetailExtraction started.. "), category: Category.Info, priority: Priority.Low);

            CustomerDetail poDetail = new CustomerDetail();
            poDetail.Customer = cust.CustomerName;
            foreach (var item in xml_config.TagList)
            {
                if (item.TagValueList?.Count > 0)
                {
                    foreach (var tag in item.TagValueList)
                    {
                        try
                        {
                            var test = tag.TagValue.Split(',');
                        
                            if (test.Any(j=> extracted_text.ToLower().Contains(j.ToLower())))
                            {
                                tag.TagValue = test.FirstOrDefault(j => extracted_text.ToLower().Contains(j.ToLower()));
                                string data_after_word = string.Empty;
                                #region To take Last Matching record
                                if (item.IdLookupValue == 2068 || item.IdLookupValue == 2072)
                                {
                                    string searchTerm = tag.TagValue.ToLower();
                                    string lowerData = extracted_text.ToLower();
                                    int lastIndex = lowerData.LastIndexOf(searchTerm);
                                    if (lastIndex != -1)
                                    {
                                        data_after_word = extracted_text.Substring(lastIndex);
                                        string pattern = @"\b(?:EUR|RON|USD|MXN|CNY|HNL|BRL|TND|MAD|PYG|RUB|INR|GBP|CAD|CHF|RSD|NIO|EGP|IDR)\b";
                                        Match match = Regex.Match(data_after_word, pattern);
                                        if (match.Success)
                                        {                                    
                                            string currency = match.Value.Equals("Euro", StringComparison.OrdinalIgnoreCase) ? "EUR" : match.Value;
                                            if (item.IdLookupValue == 2072)
                                            {
                                                poDetail.Currency = currency;
                                                continue;
                                            }
                                            else
                                            {
                                                poDetail.Currency = currency;
                                                if (string.IsNullOrEmpty(tag.NextValue))
                                                    tag.NextValue = match.Value;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                else
                                    data_after_word = extracted_text.Substring(extracted_text.ToLower().IndexOf(tag.TagValue.ToLower()) + 1);
                                string filteredData = filterData(data_after_word, tag, item);

                                #region Assign Properties To save in database
                                switch (item.Value.ToLower())
                                {
                                    case "po_number":
                                        poDetail.PONumber = filteredData;
                                        break;

                                    case "incoterm":
                                        poDetail.Incoterm = filteredData;
                                        break;

                                    case "email":
                                        if(string.IsNullOrEmpty(tag.TagValue))
                                        {
                                            try
                                            {
                                                string pattern1 = $@"\b[A-Za-z0-9._%+-]+@{Regex.Escape(poDetail.Customer)}\b";
                                                Regex regex = new Regex(pattern1, RegexOptions.IgnoreCase);
                                                MatchCollection matches = regex.Matches(extracted_text);

                                                List<string> emails = new List<string>();

                                                foreach (Match match1 in matches)
                                                {
                                                    poDetail.Email = match1.Value;
                                                    continue;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("ERROR in DetailExtraction(). email ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                            }
                                        }
                                        else
                                        poDetail.Email = filteredData;
                                        break;

                                    case "offer":
                                        poDetail.Offer = filteredData;
                                        break;

                                    case "date":
                                        DateTime parsedDate;
                                        if (IsDate(filteredData, out parsedDate) && DateTime.TryParse(filteredData, out parsedDate))
                                        {
                                            poDetail.DateIssued = parsedDate;
                                        }
                                        else if(DateTime.TryParseExact(filteredData, DateFormats, new CultureInfo("es-ES"), DateTimeStyles.None, out parsedDate))
                                        {
                                            try
                                            {
                                                poDetail.DateIssued = parsedDate;
                                            }
                                            catch (Exception ex) 
                                            {
                                                Log4NetLogger.Logger.Log(string.Format("ERROR in DetailExtraction(). date ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                                            }
                                        }
                                        break;

                                    case "total":
                                        decimal Amount = 0;
                                        if (decimal.TryParse(filteredData, NumberStyles.Any, new CultureInfo("es-ES"), out Amount))
                                        {
                                            poDetail.TotalNetValue = Convert.ToDouble(Amount);
                                        }
                                        else
                                        {                                          
                                            string amountpattern = @"\d{1,3}(,\d{3})*(\.\d{2})?";
                                            Match amountmatch = Regex.Match(filteredData, amountpattern);
                                            if (amountmatch.Success)
                                            {

                                                bool isParsed = decimal.TryParse(amountmatch.Value, NumberStyles.Any, new CultureInfo("en-US"), out Amount);
                                                if (!isParsed)
                                                {
                                                    isParsed = decimal.TryParse(amountmatch.Value, NumberStyles.Any, new CultureInfo("de-DE"), out Amount);
                                                }
                                                poDetail.TotalNetValue = Convert.ToDouble(Amount);

                                            }
                                        }

                                        break;

                                    case "currency":
                                        string pattern = @"\b(?:EUR|RON|USD|MXN|CNY|HNL|BRL|TND|MAD|PYG|RUB|INR|GBP|CAD|CHF|RSD|NIO|EGP|IDR)\b";
                                        Match match = Regex.Match(filteredData, pattern);
                                        if (match.Success)
                                        {
                                            if (match.Value.Equals("Euro", StringComparison.OrdinalIgnoreCase))
                                            {
                                                poDetail.Currency = "EUR";
                                            }
                                            poDetail.Currency = match.Value;
                                        }
                                        break;
                                    case "ship to":
                                        poDetail.ShipTo = filteredData;
                                        break;

                                    case "invoice to":
                                        poDetail.InvoiceTo = filteredData;
                                        break;

                                    case "invoice address":
                                        poDetail.InvoiceAddress = filteredData;
                                        break;
                                    case "payment terms":
                                        poDetail.PaymentTerms = filteredData;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion


                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("ERROR in DetailExtraction(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
            }

            DetailList.Add(poDetail);
            Log4NetLogger.Logger.Log(string.Format("DetailExtraction Completed.. "), category: Category.Info, priority: Priority.Low);

        }
        static bool IsDate(string input, out DateTime parsedDate)
        {
            string cleanedInput = Regex.Replace(input, @"\b(\d{1,2})(st|nd|rd|th)\b", "$1");

            if (DateTime.TryParse(cleanedInput, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return true;
            }
          
            if (DateTime.TryParseExact(cleanedInput, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return true;
            }
            return false;
        }
        //[rdixit][GEOS2-5868][15.10.2024]
        private string filterData(string value, TemplateTag tagVal, LookupValue tag)
        {
            Log4NetLogger.Logger.Log(string.Format("filterData Method Started.... "), category: Category.Info, priority: Priority.Low);
            string target = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(tagVal.NextValue))
                {
                    if (tagVal.NextValue == "END")
                    {
                        string[] delimiters = new string[] { "\r\n", "\r", "\n", ":", " " };
                        string pattern = $@"(?<!\b\s)({string.Join("|", delimiters.Select(Regex.Escape))})";
                        string[] lines = Regex.Split(value, pattern).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        target = string.Join(" ", lines);
                       
                    }
                    else
                    {
                        string[] words = tagVal.NextValue.Split(',');

                        foreach (string word in words)
                        {
                            int position = value.IndexOf(word.Trim(), StringComparison.OrdinalIgnoreCase);

                            if (position != -1)
                            {
                                value = value.Substring(0, position);
                                break;
                            }
                        }
                        string[] delimiters = new string[] { "\r\n", "\r", "\n", ":", " " };
                        string pattern = $@"(?<!\b\s)({string.Join("|", delimiters.Select(Regex.Escape))})";
                        string[] lines = Regex.Split(value, pattern).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        int targetIndex = tagVal.SkipValue + 2;
                        if (targetIndex >= 0 && targetIndex < lines.Length)
                        {
                            if (tag.IdLookupValue == 2068)
                                target = string.Join("", lines.Skip(targetIndex));
                            else
                            target = string.Join(" ", lines.Skip(targetIndex));
                        }
                    }
                }
                else
                {
                    string[] delimiters = new string[] { "\r\n", "\r", "\n", ":", " " };
                    string pattern = $@"(?<!\b\s)({string.Join("|", delimiters.Select(Regex.Escape))})";
                    string[] lines = Regex.Split(value, pattern).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    int targetIndex = tagVal.SkipValue + 2;
                    if (targetIndex >= 0 && targetIndex < lines.Length)
                    {
                        target = lines[targetIndex];
                    }
                }
                Log4NetLogger.Logger.Log(string.Format("filterData Method Executed.... "), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  filterData() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
            return target;
        }
        public void FillTemplateSettingList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillTemplateSettingList()...", category: Category.Info, priority: Priority.Low);
                TemplateSettingList = OtmStartUp.GetAllTags();
            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillAllCompaniesList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillAllCompaniesList()...", category: Category.Info, priority: Priority.Low);
                CompanyList = OtmStartUp.GetAllCompanyDetailsList_V2660();
                Log4NetLogger.Logger.Log(string.Format("FillAllCompaniesList() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillAllCompaniesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillAllCompaniesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillAllCompaniesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillPoStatusList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillPoStatusList()...", category: Category.Info, priority: Priority.Low);
                PoStatusList = OtmStartUp.GetLookupValues(159).ToList();
                Log4NetLogger.Logger.Log(string.Format("FillPoStatusList() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillCustomerList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillCustomerList()...", category: Category.Info, priority: Priority.Low);
                CustomerList = OtmStartUp.GetAllCustomers();
                Log4NetLogger.Logger.Log(string.Format("FillCustomerList() Method Executed.... "), category: Category.Info, priority: Priority.Low);


            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCustomerList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCustomerList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCustomerList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillUntrustedExtensionList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillUntrustedExtensionList()...", category: Category.Info, priority: Priority.Low);
                UntrustedExtensionList = OtmStartUp.GetLookupValues(157);
                Log4NetLogger.Logger.Log(string.Format("FillUntrustedExtensionList() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillCurrencyList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillCurrencyList()...", category: Category.Info, priority: Priority.Low);
                CurrencyList = OtmStartUp.GetAllCurrencies().ToList();
                Log4NetLogger.Logger.Log(string.Format("FillCurrencyList() Method Executed.... "), category: Category.Info, priority: Priority.Low);


            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillBlackListEmails()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillBlackListEmails()...", category: Category.Info, priority: Priority.Low);
                BlackListEmailList = OtmStartUp.GetAllBlackListEmails();
                Log4NetLogger.Logger.Log(string.Format("FillBlackListEmails() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillUnprocessedEmails()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillUnprocessedEmails()...", category: Category.Info, priority: Priority.Low);

                //EmailList = OtmStartUp.GetAllUnprocessedEmails_V2570();
                //[pramod.misal][GEOS2-6735][27-01-2025]
                //OtmStartUp = new OTMServiceController("localhost:6699");
                EmailList = OtmStartUp.GetAllUnprocessedEmails_V2660();   //[rdixit][GEOS2-9020][23.07.2025]
                Log4NetLogger.Logger.Log(string.Format("FillUnprocessedEmails() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public CustomerDetail CalculatePercentage(Email email)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CalculatePercentage()...", category: Category.Info, priority: Priority.Low);

                foreach (var detail in DetailList)
                {
                    int per = 0;
                    string emailpattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!string.IsNullOrEmpty(detail.Email) && Regex.IsMatch(detail.Email, emailpattern))
                        per++;
                    if (!string.IsNullOrEmpty(detail.PONumber) && detail.PONumber != "0")
                        per++;
                    if (detail.TotalNetValue != 0)
                        per++;
                    if (!string.IsNullOrEmpty(detail.Incoterm))
                        per++;
                    if (!string.IsNullOrEmpty(detail.Offer) && detail.Offer != "0")
                        per++;
                    if (!string.IsNullOrEmpty(detail.Currency))
                        per++;
                    if (!string.IsNullOrEmpty(detail.Customer))
                        per++;
                    if (detail.DateIssued != DateTime.MinValue)
                        per++;
                    if (!string.IsNullOrEmpty(detail.ShipTo))
                        per++;
                    if (!string.IsNullOrEmpty(detail.InvoiceTo))
                        per++;
                    if (!string.IsNullOrEmpty(detail.InvoiceAddress))
                        per++;

                    detail.Percentage = (double)per / 11;
                }

                Log4NetLogger.Logger.Log(string.Format("CalculatePercentage() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method CalculatePercentage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            var MaxPercentagePo = DetailList.OrderByDescending(i => i.Percentage).FirstOrDefault();
            MaxPercentagePo.IdPORequest = email.IdPORequest;
            return MaxPercentagePo;
        }
        #endregion
        public CustomerDetail CalculatePercentageByExtractingTemplate(Email email)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CalculatePercentageByExtractingTemplate()...", category: Category.Info, priority: Priority.Low);

                foreach (var detail in DetailList)
                {
                    int per = 0;
                    string emailpattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!string.IsNullOrEmpty(detail.Email) && Regex.IsMatch(detail.Email, emailpattern))
                        per++;
                    if (!string.IsNullOrEmpty(detail.PONumber) && detail.PONumber != "0")
                        per++;
                    if (detail.TotalNetValue != 0)
                        per++;
                    if (!string.IsNullOrEmpty(detail.Incoterm))
                        per++;
                    if (!string.IsNullOrEmpty(detail.Offer) && detail.Offer != "0")
                        per++;
                    if (!string.IsNullOrEmpty(detail.Currency))
                        per++;
                    if (!string.IsNullOrEmpty(detail.Customer))
                        per++;
                    if (detail.DateIssued != DateTime.MinValue)
                        per++;
                    if (!string.IsNullOrEmpty(detail.ShipTo))
                        per++;
                    //if (!string.IsNullOrEmpty(detail.InvoiceTo))
                    //    per++;
                    //if (!string.IsNullOrEmpty(detail.InvoiceAddress))
                    //    per++;

                    detail.Percentage = (double)per / 9;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method CalculatePercentageByExtractingTemplate()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            var MaxPercentagePo = DetailList.OrderByDescending(i => i.Percentage).FirstOrDefault();
            MaxPercentagePo.IdPORequest = email.IdPORequest;
            return MaxPercentagePo;
        }

        #region Start & Stop Methods
        protected override void OnStart(string[] args)
        {
            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Staring"), category: Category.Info, priority: Priority.Low);

            InitiaizeOnStartService();

            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Executed"), category: Category.Info, priority: Priority.Low);
        }

        protected override void OnStop()
        {
            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stopping"), category: Category.Info, priority: Priority.Low);

            if (PoAnalyzerTimer != null)
                PoAnalyzerTimer.Enabled = false;

            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stoped"), category: Category.Info, priority: Priority.Low);
        }

        //[GEOS2-9189][rdixit][12.08.2025]
        private void PoAnalyzer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log4NetLogger.Logger.Log("PoAnalyzer_Tick() started.", Category.Info, Priority.Low);

                if (PoAnalyzerTimer != null)
                    PoAnalyzerTimer.Enabled = false;

                lock (PoAnalyzerObject)
                {
                    AnalyseEmails();
                }

                Log4NetLogger.Logger.Log("PoAnalyzer_Tick() executed.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"PoAnalyzer_Tick() - Error: {ex}", Category.Exception, Priority.Low);
            }
            finally
            {
                if (PoAnalyzerTimer != null)
                    PoAnalyzerTimer.Enabled = true;
            }
        }
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        private string ExtractOfferCode_V2610(Email item, Regex offerRegex)
        {
            Log4NetLogger.Logger.Log("Method ExtractOfferCode_V2610()...", category: Category.Info, priority: Priority.Low);
            bool offerFound = false;
            // Check for offer code in subject
            Match match = offerRegex.Match(item.Subject ?? "");
            //string offerCode = ExtractOfferCode_V2610(item, offerRegex);
            if (match.Success==true)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    //OtmStartUp = new OTMServiceController("localhost:6699");
                    var offerDetails = OtmStartUp.GetOfferDetails_V2620(match.Value);
                    if (offerDetails != null)
                    {
                        int? idOffer = offerDetails;
                        if (idOffer > 0)
                        {
                            //OtmStartUp = new OTMServiceController("localhost:6699");
                            OtmStartUp.OTM_InsertPORequestLinkedOffer_V2620(item.IdPORequest, idOffer);
                            offerFound = true;
                            Log4NetLogger.Logger.Log(string.Format($"Inserted Linked Offer: PORequest {item.IdPORequest}, Offer {idOffer}"), category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
            }
            // Check in email body if not found in subject or attachments
            Match match1 = offerRegex.Match(item.Body ?? "");
            if (match1.Success==true)
            {
                if (!string.IsNullOrEmpty(match1.Value))
                {
                    //OtmStartUp = new OTMServiceController("localhost:6699");
                    var offerDetails = OtmStartUp.GetOfferDetails_V2620(match1.Value);
                    if (offerDetails != null)
                    {
                        int? idOffer = offerDetails;
                        if (idOffer > 0)
                        {
                            //OtmStartUp = new OTMServiceController("localhost:6699");
                            OtmStartUp.OTM_InsertPORequestLinkedOffer_V2620(item.IdPORequest, idOffer);
                            offerFound = true;
                            Log4NetLogger.Logger.Log(string.Format($"Inserted Linked Offer: PORequest {item.IdPORequest}, Offer {idOffer}"), category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
            }
            if (offerFound == false)
            {
                poLog.Add(new LogEntryByPORequest()
                {
                    IdPORequest = item.IdPORequest,
                    IdUser = 164,
                    DateTime = DateTime.Now,
                    Comments = "It is not possible to determine Offer Code in Subject And Body.",
                    IdLogEntryType = 25,
                    IdEmail = item.IdEmail
                });
            }

            Log4NetLogger.Logger.Log(string.Format("ExtractOfferCode_V2610() Method Executed.... "), category: Category.Info, priority: Priority.Low);
            return match.Success ? match.Value : match1.Success ? match1.Value : null;

           

        }
        public void FillBlankColumnsEmailList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillBlankColumnsEmailList()...", category: Category.Info, priority: Priority.Low);
                //EmailUpdatedList = OtmStartUp.GetAllEmailsBlankColumns();

                EmailUpdatedList = OtmStartUp.GetAllEmailsBlankColumns_V2630();

                if (EmailUpdatedList.Count > 0)
                {
                    foreach(Email item in emailUpdatedList)
                    {
                        #region Update Group and Plant
                        //BodyMail
                        CheckByBodyMail(item);
                        //Sender
                        CheckBySenderEmail(item);
                        //To Email
                        CheckByTOEmail(item);
                        //CC Email
                        CheckByCCEmail(item);
                        //Domain
                        CheckByDomain(item);

                        #endregion

                        //[pramod.misal][GEOS2 -7718][28.03.2025]
                        #region Update Sender,To and CC idperson
                        MatchWithSendername(item);
                        MatchWithToName(item);
                        MatchWithCCName(item);
                        #endregion
                    }

                }

                Log4NetLogger.Logger.Log(string.Format("FillBlankColumnsEmailList() Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlankColumnsEmailList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlankColumnsEmailList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillBlankColumnsEmailList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Group-Plant Methods

        //[pramod.misal][11.03.2025][GEOS2 - 6719]
        //[ashish.malkhede][17.03.2025][GEOS2 - 7042]
        private void CheckByBodyMail(Email poRequest)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckByBodyMail ...", category: Category.Info, priority: Priority.Low);

                // Regular expression to match email addresses
                //string emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";

                // Pre-build a lookup dictionary for customer emails to improve lookup performance
                var customerEmailLookup = CustomerContactsList
              .GroupBy(c => c.Email)
              .ToDictionary(g => g.Key, g => g.ToList());

                // Only process requests without a group
                if (poRequest.IdCustomer == null || poRequest.IdCustomer == 0)
                {
                    //MatchCollection matches = Regex.Matches(poRequest.Body, emailPattern);
                    //var emailSet = new HashSet<string>(matches.Cast<Match>().Select(m => m.Value));

                    // Find the first matching customer from the pre-built dictionary
                    //var customer = emailSet
                    //    .Select(email => customerEmailLookup.ContainsKey(email) ? customerEmailLookup[email].FirstOrDefault() : null)
                    //    .FirstOrDefault(c => c != null);
                    var l = poRequest.Body.Length;
                    string largeText = poRequest.Body;
                    int chunkSize = 10000; // Define chunk size (adjust based on data size)

                    List<string> chunks = SplitTextIntoChunks(largeText, chunkSize);
                    Regex regex = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    List<string> extractedEmails = new List<string>();

                    foreach (string chunk in chunks)
                    {
                        MatchCollection matches = regex.Matches(chunk);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                extractedEmails.Add(match.Value);
                            }
                        }
                    }

                    var customer = extractedEmails
                     .Select(email => customerEmailLookup.ContainsKey(email) ? customerEmailLookup[email].FirstOrDefault() : null)
                     .FirstOrDefault(c => c != null);

                    if (customer != null)
                    {
                        //poRequest.Group = customer.GroupName;
                        //poRequest.Plant = customer.Plant;
                        poRequest.IdCustomer = customer.IdCustomer;
                        poRequest.IdPlant = customer.IdPlant;
                        Log4NetLogger.Logger.Log(
                         string.Format("In Body Mail, found IdCustomer = {0}, IdPlant = {1}", poRequest.IdCustomer, poRequest.IdPlant),
                         category: Category.Info,
                         priority: Priority.Low);



                        bool IsSave = OtmStartUp.UpdatePORequestGroupPlant_V2660(poRequest);   //[rdixit][GEOS2-9020][23.07.2025]
                        Log4NetLogger.Logger.Log("Method CheckByBodyMail() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else //[pooja.jadhav][GEOS2-8342][11-06-2025]
                    {
                        poLog.Add(new LogEntryByPORequest() 
                        { 
                            IdPORequest = poRequest.IdPORequest, 
                            IdUser = 164, 
                            DateTime = DateTime.Now,
                            Comments = "It is not possible to determine the sender plant and group in email body.",
                            IdLogEntryType = 25, 
                            IdEmail = poRequest.IdEmail 
                        });
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in CheckByBodyMail Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        List<string> SplitTextIntoChunks(string text, int chunkSize)
        {
            List<string> chunks = new List<string>();
            for (int i = 0; i < text.Length; i += chunkSize)
            {
                chunks.Add(text.Substring(i, Math.Min(chunkSize, text.Length - i)));
            }
            return chunks;
        }
        //[pramod.misal][11.03.2025][GEOS2 - 6719]
        private void CheckBySenderEmail(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckBySenderEmail ...", category: Category.Info, priority: Priority.Low);
                if (item.IdCustomer == null || item.IdCustomer == 0)
                {
                    //var contact = CustomerContactsList.FirstOrDefault(c => c.Email == x.Sender);
                    var contact = CustomerContactsList.FirstOrDefault(c => string.Equals(c.Email, item.SenderEmail, StringComparison.OrdinalIgnoreCase));
                    if (contact != null)
                    {
                        item.IdCustomer = contact.IdCustomer;
                        item.IdPlant = contact.IdPlant;
                        Log4NetLogger.Logger.Log(
                         string.Format("In Sender mail, found IdCustomer = {0}, IdPlant = {1}", item.IdCustomer, item.IdPlant),
                         category: Category.Info,
                         priority: Priority.Low);

                        bool IsSave = OtmStartUp.UpdatePORequestGroupPlant_V2660(item);   //[rdixit][GEOS2-9020][23.07.2025]
                        Log4NetLogger.Logger.Log("Method CheckBySenderEmail() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else //[pooja.jadhav][GEOS2-8342][11-06-2025]
                    {
                        poLog.Add(new LogEntryByPORequest() 
                        { 
                            IdPORequest = item.IdPORequest, 
                            IdUser = 164, 
                            DateTime = DateTime.Now, 
                            Comments = "It is not possible to determine the sender plant and group in sender email.", 
                            IdLogEntryType = 25, 
                            IdEmail = item.IdEmail 
                        });
                    }
                }
                
            }
            catch (Exception ex)
            {

                Log4NetLogger.Logger.Log("Get an error in CheckBySenderEmail Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        //[pramod.misal][11.03.2025][GEOS2 - 6719]
        private void CheckByTOEmail(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckByTOEmail ...", category: Category.Info, priority: Priority.Low);

                if ((item.IdCustomer == null || item.IdCustomer == 0) && !string.IsNullOrEmpty(item.RecipientEmail))
                {
                    // Split Recipient string into an array of emails
                    var recipientEmails = item.RecipientEmail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Find the first matching contact where any of the emails match
                    //var contact = CustomerContactsList.FirstOrDefault(c => recipientEmails.Contains(c.Email.Trim()));
                    var contact = CustomerContactsList.FirstOrDefault(c => recipientEmails.Any(email => string.Equals(email, c.Email, StringComparison.OrdinalIgnoreCase)));
                    if (contact != null)
                    {
                        //item.Group = contact.GroupName;
                        //item.Plant = contact.Plant;
                        item.IdCustomer = contact.IdCustomer;
                        item.IdPlant = contact.IdPlant;
                        Log4NetLogger.Logger.Log(
                         string.Format("In TO Email , found IdCustomer = {0}, IdPlant = {1}", item.IdCustomer, item.IdPlant),
                         category: Category.Info,
                         priority: Priority.Low);
                        bool IsSave = OtmStartUp.UpdatePORequestGroupPlant_V2660(item);   //[rdixit][GEOS2-9020][23.07.2025]
                        Log4NetLogger.Logger.Log("Method CheckByTOEmail() executed successfully", category: Category.Info, priority: Priority.Low);

                    }
                    else //[pooja.jadhav][GEOS2-8342][11-06-2025]
                    {
                        poLog.Add(new LogEntryByPORequest() 
                        { 
                            IdPORequest = item.IdPORequest, 
                            IdUser = 164, 
                            DateTime = DateTime.Now,
                            Comments = "It is not possible to determine the sender plant and group in To email.", 
                            IdLogEntryType = 25,
                            IdEmail = item.IdEmail
                        });
                    }
                }
                Log4NetLogger.Logger.Log("Method CheckByTOEmail() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in CheckByTOEmail Method " + ex.Message, category: Category.Info, priority: Priority.Low);

            }


        }

        //[pramod.misal][11.03.2025][GEOS2 - 6719]
        //[ashish.malkhede][17.03.2025][GEOS2 - 7042]
        private void CheckByCCEmail(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckByCCEmail ...", category: Category.Info, priority: Priority.Low);
                if ((item.IdCustomer == null || item.IdCustomer == 0) && !string.IsNullOrEmpty(item.CCEmail))
                {
                    // Split Recipient string into an array of emails
                    var ccRecipientEmails = item.CCEmail.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);


                    //var contact = CustomerContactsList.FirstOrDefault(c => ccRecipientEmails.Contains(c.Email.Trim()));
                    // Find the first matching contact where any of the emails match (case-insensitive)
                    var contact = CustomerContactsList.FirstOrDefault(c => ccRecipientEmails.Any(email => string.Equals(email, c.Email, StringComparison.OrdinalIgnoreCase)));

                    if (contact != null)
                    {
                        //item.Group = contact.GroupName;
                        //item.Plant = contact.Plant;
                        item.IdCustomer = contact.IdCustomer;
                        item.IdPlant = contact.IdPlant;
                        Log4NetLogger.Logger.Log(
                         string.Format("In CC mail, found IdCustomer = {0}, IdPlant = {1}", item.IdCustomer, item.IdPlant),
                         category: Category.Info,
                         priority: Priority.Low);


                        bool IsSave = OtmStartUp.UpdatePORequestGroupPlant_V2660(item);   //[rdixit][GEOS2-9020][23.07.2025]
                        Log4NetLogger.Logger.Log("Method CheckByCCEmail() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else //[pooja.jadhav][GEOS2-8342][11-06-2025]
                    {
                        poLog.Add(new LogEntryByPORequest() 
                        { 
                            IdPORequest = item.IdPORequest, 
                            IdUser = 164, 
                            DateTime = DateTime.Now, 
                            Comments = "It is not possible to determine the sender plant and group in CC email.", 
                            IdLogEntryType = 25,
                            IdEmail = item.IdEmail
                        });
                    }
                }

                Log4NetLogger.Logger.Log("Method CheckByCCEmail() executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {

                Log4NetLogger.Logger.Log("Get an error in CheckByCCEmail Method " + ex.Message, category: Category.Info, priority: Priority.Low);

            }



        }

        private void CheckByDomain(Email request)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckByDomain ...", category: Category.Info, priority: Priority.Low);

                // Regular expression to extract domain name from email
                string domainPattern = @"@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})";
                string emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"; // Find email addresses

                // Cache matching customers for each domain
                var customerCache = CustomerContactsList
                        .Where(c => c.Email.Contains('@')) // Ensure '@' is present
                        .GroupBy(c => c.Email.Split('@').Length > 1 ? c.Email.Split('@')[1].ToLower() : string.Empty) // Prevent IndexOutOfRangeException
                        .Where(g => !string.IsNullOrEmpty(g.Key)) // Exclude invalid emails
                        .ToDictionary(g => g.Key, g => g.ToList());
                //if (string.IsNullOrEmpty(request.Group))
                if (request.IdCustomer == null || request.IdCustomer == 0 && !string.IsNullOrEmpty(request.Body))
                {
                    // Process Sender, Recipient, CCRecipient, and Emailbody
                    ProcessField(request.Body, customerCache, request);
                }
                if (request.IdCustomer == null || request.IdCustomer == 0 && !string.IsNullOrEmpty(request.SenderEmail))
                {
                    // Process Sender, Recipient, CCRecipient, and Emailbody
                    ProcessField(request.SenderEmail, customerCache, request);
                }
                if (request.IdCustomer == null || request.IdCustomer == 0 && !string.IsNullOrEmpty(request.RecipientEmail))
                {
                    // Process Sender, Recipient, CCRecipient, and Emailbody
                    ProcessField(request.RecipientEmail, customerCache, request);
                }
                if (request.IdCustomer == null || request.IdCustomer == 0 && !string.IsNullOrEmpty(request.CCEmail))
                {
                    // Process Sender, Recipient, CCRecipient, and Emailbody
                    ProcessField(request.CCEmail, customerCache, request);
                }
                if(request.IdCustomer == null || request.IdCustomer == 0)
                {
                    poLog.Add(new LogEntryByPORequest()
                    {
                        IdPORequest = request.IdPORequest,
                        IdUser = 164,
                        DateTime = DateTime.Now,
                        Comments = "It was not possible to determine the Sender Plant and Group. Multiple entries for the Domain.",
                        IdLogEntryType = 25,
                        IdEmail = request.IdEmail
                    });
                }
                Log4NetLogger.Logger.Log("Method CheckByDomain() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in CheckByDomain Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ProcessField(string field, Dictionary<string, List<CustomerContacts>> customerCache, Email request)
        {
            Log4NetLogger.Logger.Log("Method ProcessField from CheckByDomain() ...", category: Category.Info, priority: Priority.Low);

            if (string.IsNullOrEmpty(field)) return;

            var domainMatches = Regex.Matches(field, @"@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.ToLower())
                .Distinct()
                .Where(domain => !domain.Equals("emdep.com", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var uniqueDomains = new HashSet<string>();
            foreach (var domain in domainMatches)
            {
                string baseDomain = domain.Split('.')[0];
                if (!uniqueDomains.Any(d => d.StartsWith(baseDomain)))
                {
                    uniqueDomains.Add(domain);
                }
            }

            if (uniqueDomains.Count > 0)
            {
                var allMatchingCustomers = new List<CustomerContacts>();

                foreach (var domain in uniqueDomains)
                {
                    if (customerCache.TryGetValue(domain, out var matchingCustomers))
                    {
                        allMatchingCustomers.AddRange(matchingCustomers);
                    }
                }

                var distinctGroups = allMatchingCustomers.Select(c => c.IdCustomer).Distinct().ToList();
                var distinctPlants = allMatchingCustomers.Select(c => c.IdPlant).Distinct().ToList();

                if (distinctGroups.Count > 1 && uniqueDomains.Count > 0)
                {
                    request.IdCustomer = 0;
                    request.IdPlant = 0;
                    
                    poLog.Add(new LogEntryByPORequest() 
                    { 
                        IdPORequest = request.IdPORequest,
                        IdUser = 164, 
                        DateTime = DateTime.Now, 
                        Comments = "It was not possible to determine the Sender Plant and Group. Multiple entries for the Domain.", 
                        IdLogEntryType = 25,
                        IdEmail = request.IdEmail
                    });
                }
                else if (distinctGroups.Count == 1 && distinctPlants.Count > 1)
                {
                    request.IdCustomer = distinctGroups.First();
                    request.IdPlant =0;
                   
                    poLog.Add(new LogEntryByPORequest() 
                    {
                        IdPORequest = request.IdPORequest, 
                        IdUser = 164, 
                        DateTime = DateTime.Now,
                        Comments = "It was not possible to determine the Sender Plant. Multiple entries for the Domain.", 
                        IdLogEntryType = 25,
                        IdEmail = request.IdEmail
                    });
                }
                else if (distinctGroups.Count == 1)
                {
                    request.IdCustomer = distinctGroups.First();
                    request.IdPlant = 0;
                    
                    poLog.Add(new LogEntryByPORequest() 
                    { 
                        IdPORequest = request.IdPORequest, 
                        IdUser = 164, 
                        DateTime = DateTime.Now,
                        Comments = "It was not possible to determine the Sender Plant. Multiple entries for the Domain.", 
                        IdLogEntryType = 25,
                        IdEmail = request.IdEmail
                    });
                }
                else if (distinctGroups.Count == 1 && distinctPlants.Count == 1)
                {
                    request.IdCustomer = distinctGroups.First();
                    request.IdPlant = distinctPlants.First();
                }
                else
                {
                    request.IdCustomer = 0;
                    request.IdPlant = 0;
                    
                    poLog.Add(new LogEntryByPORequest() 
                    { 
                        IdPORequest = request.IdPORequest, 
                        IdUser = 164, 
                        DateTime = DateTime.Now, 
                        Comments = "It was not possible to determine the Sender Plant and Group. Multiple entries for the Domain.", 
                        IdLogEntryType = 25,
                        IdEmail = request.IdEmail
                    });
                }
                
                bool IsSave = OtmStartUp.UpdatePORequestGroupPlant_V2660(request);   //[rdixit][GEOS2-9020][23.07.2025]

                Log4NetLogger.Logger.Log("Method ProcessField from CheckByDomain() executed successfully", category: Category.Info, priority: Priority.Low);


            }
        }

        public void FillCustomerContactsList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillCustomerContactsList()...", category: Category.Info, priority: Priority.Low);
                //OtmStartUp = new OTMServiceController("localhost:6699");
                CustomerContactsList = new ObservableCollection<CustomerContacts>(OtmStartUp.GetAllCustomerContactsList_V2620());
                Log4NetLogger.Logger.Log("Method FillCustomerContactsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {

                Log4NetLogger.Logger.Log("Get an error in FillCustomerContactsList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);

            }
            catch (ServiceUnexceptedException ex)
            {

                Log4NetLogger.Logger.Log("Get an error in FillCustomerContactsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                Log4NetLogger.Logger.Log("Get an error in Method FillCustomerContactsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        // [pramod.misal][GEOS2 -7718][28.03.2025]
        void MatchWithToName(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("MatchWithToName Method Started.... "), category: Category.Info, priority: Priority.Low);

                //[pramod.misal][07-08-2025][GEOS2-9187]https://helpdesk.emdep.com/browse/GEOS2-9187
                if (!string.IsNullOrWhiteSpace(item.RecipientName) && !string.IsNullOrEmpty(item.RecipientName))
                {
                    var ToNames = item.RecipientName.Split(',').Select(name => name.Trim());

                    var matchedIds = new List<string>();

                    foreach (var name in ToNames)
                    {
                        //var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => string.Equals(emp.FullName, name, StringComparison.OrdinalIgnoreCase));
                        var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.FullName?.ToLower().Contains(name.ToLower()) == true);
                        

                        if (matchedEmployee != null)
                        {
                            matchedIds.Add(Convert.ToString(matchedEmployee.IdEmployee));
                        }
                        else
                        {
                            matchedIds.Add("null"); // Add "Null" for unmatched names
                        }

                    }
                    // Assign matched IDs as comma separated string
                    item.ToIdPerson = string.Join(",", matchedIds);

                    if (!string.IsNullOrEmpty(item.ToIdPerson))
                    {
                        OtmStartUp.UpdateToIdPerson_V2630(item);
                    }


                }
                Log4NetLogger.Logger.Log(string.Format("MatchWithToName Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method MatchWithToName()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[pramod.misal][GEOS2 -7718][28.03.2025]
        void MatchWithCCName(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("MatchWithCCName Method Started.... "), category: Category.Info, priority: Priority.Low);
                //[pramod.misal][07-08-2025][GEOS2-9187]https://helpdesk.emdep.com/browse/GEOS2-9187
                if (!string.IsNullOrWhiteSpace(item.CCName) && !string.IsNullOrEmpty(item.CCName))
                {
                    var ccNames = item.CCName.Split(',').Select(name => name.Trim());

                    var matchedIds = new List<string>();

                    foreach (var name in ccNames)
                    {
                        //var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => string.Equals(emp.FullName, name, StringComparison.OrdinalIgnoreCase));
                        var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => emp.FullName?.ToLower().Contains(name.ToLower()) == true);

                        if (matchedEmployee != null)
                        {
                            matchedIds.Add(Convert.ToString(matchedEmployee.IdEmployee));
                        }
                        else
                        {
                            matchedIds.Add("null"); // Add "Null" for unmatched names
                        }

                    }

                    // Assign matched IDs as comma separated string
                    item.CCIdPerson = string.Join(",", matchedIds);

                    if (!string.IsNullOrEmpty(item.CCIdPerson))
                    {
                        OtmStartUp.UpdateCCIdPerson_V2630(item);
                    }

                }

                Log4NetLogger.Logger.Log(string.Format("MatchWithCCName Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method MatchWithCCName()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[pramod.misal][28.03.2025][GEOS2-]
        public void FillPOEmployeeInfo()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillPOEmployeeInfo()...", category: Category.Info, priority: Priority.Low);
                //OtmStartUp = new OTMServiceController("localhost:6699");

                //POEmployeeInfoList = new ObservableCollection<POEmployeeInfo>(OtmStartUp.GetPOEmployeeInfoList_V2610());
                POEmployeeInfoList = new ObservableCollection<POEmployeeInfo>(OtmStartUp.GetPOEmployeeInfoList_V2630());
                Log4NetLogger.Logger.Log(string.Format("FillPOEmployeeInfo() Method Executed.... "), category: Category.Info, priority: Priority.Low);


            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPOEmployeeInfo()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPOEmployeeInfo()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method FillPOEmployeeInfo()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; d[i, 0] = i++) { }
            for (int j = 0; j <= t.Length; d[0, j] = j++) { }

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }
            return d[s.Length, t.Length];
        }

        // [pramod.misal][GEOS2 -7718][28.03.2025]
        void MatchWithSendername(Email item)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("MatchWithSendername Method Started.... "), category: Category.Info, priority: Priority.Low);

                //[pramod.misal][07-08-2025][GEOS2-9187]https://helpdesk.emdep.com/browse/GEOS2-9187
                if (!string.IsNullOrWhiteSpace(item.SenderName) && !string.IsNullOrEmpty(item.SenderName))
                {
                    //var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp => string.Equals(emp.FullName, item.SenderName, StringComparison.OrdinalIgnoreCase));
                    var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp =>emp.FullName?.ToLower().Contains(item.SenderName.ToLower()) == true);

                    if (matchedEmployee != null)
                    {
                        item.SenderIdPerson = Convert.ToString(matchedEmployee.IdEmployee);
                        OtmStartUp.UpdateSenderIdPerson_V2630(item);
                    }

                }

                //if (!string.IsNullOrWhiteSpace(item.SenderName))
                //{
                //    Split sender name into individual words
                //    var senderNameParts = item.SenderName.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

                //    var matchedEmployee = POEmployeeInfoList.FirstOrDefault(emp =>
                //        senderNameParts.All(part => emp.FullName.IndexOf(part, StringComparison.OrdinalIgnoreCase) >= 0));

                //    if (matchedEmployee != null)
                //    {
                //        item.SenderIdPerson = matchedEmployee.IdEmployee.ToString();
                //        OtmStartUp.UpdateSenderIdPerson_V2630(item);
                //    }
                //}

                Log4NetLogger.Logger.Log(string.Format("MatchWithSendername Method Executed.... "), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in Method MatchWithSendername()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void CheckInformationFromXMlBodyMailOffer(XmlNode quotationNode, XmlNode revisionNode, XmlNode otNode)
        {
            try
            {
                Log4NetLogger.Logger.Log("Method CheckInformationFromXMlBodyMailOffer ...", category: Category.Info, priority: Priority.Low);
                try
                {
                    if (revisionNode != null)
                    {
                        //[rdixit][GEOS2-9141][02.08.2025]
                        bool isOfferExist = false;
                        int idPerson = Convert.ToInt32(revisionNode.SelectSingleNode("madeby")?.InnerText);
                        int IdCompany = OtmStartUp.GetUserPlant_V2660(idPerson);
                        var importCompany = CompanyList.FirstOrDefault(i => i.IdCompany == IdCompany);
                        if (importCompany != null && importCompany.ServiceProviderUrl != null)
                        {
                            IOTMService importOTMService = new OTMServiceController(importCompany.ServiceProviderUrl);
                            int year = Convert.ToInt32(quotationNode.SelectSingleNode("year")?.InnerText);
                            int number = Convert.ToInt32(quotationNode.SelectSingleNode("number")?.InnerText);
                            if (year != 0 && number != 0)
                            {
                                Quotation quo = null;
                                string quoCode = Convert.ToString(quotationNode.SelectSingleNode("code")?.InnerText);
                                int numOt = Convert.ToInt32(otNode.SelectSingleNode("numot")?.InnerText);
                                string OtCode = Convert.ToString(otNode.SelectSingleNode("code")?.InnerText);

                                Offer importOffer = importOTMService.GetOfferDetailsByYearAndNumber_V2660(year, number); // export Offer By Source Plant
                                if (importOffer != null)
                                {
                                    quo = new Quotation();
                                    quo.Offer = importOffer;
                                    quo = importOTMService.OTM_GetQuotationByCode_V2660(quoCode, numOt, OtCode, quo); // export Quotation By Source Plant
                                }
                                if (quo != null)
                                {
                                    quo.Offer.IdStatus = 2;
                                    quo.Offer.Site.ConnectPlantId = Properties.Settings.Default.SERVICE_PLANT_ID;
                                    //CheckRevisionItemNum(quo);
                                    //OtmStartUp.InsertOffer_V2660(quo); // Insert Offer By destination Plant
                                    //[pramod.misal][24.09.2025][GEOS2-9576]https://helpdesk.emdep.com/browse/GEOS2-9576
                                    OtmStartUp.InsertOffer_V2670(quo); // Insert Offer By destination Plant
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("ERROR in CheckInformationFromXMlBodyMailOffer() - OnStart() in Offer. ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                }
                Log4NetLogger.Logger.Log("Method CheckInformationFromXMlBodyMailOffer  executed Succesfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in CheckInformationFromXMlBodyMail Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void CheckRevisionItemNum(Quotation q)
        {
            if(q !=null)
            {
                if (q.Revisions.Count > 0)
                {
                    foreach(Revision rev in q.Revisions)
                    {
                        if (rev.Items != null)
                        {
                            int numitem = 1;
                            foreach(RevisionItem item in rev.Items.Values)
                            {
                                item.NumItem = numitem.ToString();
                                numitem++;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }

    #region Methods
    /*
    void CreateIfNotExists(string config_path)
    {
        string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <configuration>
                                      <configSections>
                                        <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                      </configSections>
                                      <log4net debug=""true"">
                                        <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                          <file value=""C:\Temp\Emdep\Geos\GeosPOAnalyzerServiceLogs.txt""/>
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

    internal void InitiaizeOnStartService()
    {
        try
        {
            PoAnalyzerTimer = new Timer();
            this.PoAnalyzerTimer.Interval = Convert.ToInt32(Properties.Settings.Default.INTERVAL_REFRESH);
            this.PoAnalyzerTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.PoAnalyzer_Tick);
            PoAnalyzerTimer.Enabled = true;
        }
        catch (FaultException<ServiceException> ex)
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

    public void AnalyseEmails()
    {
        try
        {
            Log4NetLogger.Logger.Log(string.Format("AnalyseEmails Method Started.... "), category: Category.Info, priority: Priority.Low);
            FillUnprocessedEmails();
            FillBlackListEmails();
            FillCurrencyList();
            FillUntrustedExtensionList();
            FillPoStatusList();
            FillTemplateSettingList();
            if (EmailList?.Count > 0)
            {
                #region Emails
                foreach (var item in EmailList)
                {
                    PORequest poReq = new PORequest();
                    try
                    {
                        string domain = item.SenderEmail?.Split('@')[1].Trim();
                        EmailStatus status = GetEmailStatus(item, domain);
                        poReq.IdEmail = item.IdEmail;
                        switch (status)
                        {
                            case EmailStatus.Blacklisted:
                                poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "blacklisted");
                                break;

                            case EmailStatus.MissingAttachment:
                                poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "missingattachment");
                                break;

                            case EmailStatus.UnsupportedAttachment:
                                poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "unsupportedattachment");
                                break;

                            case EmailStatus.Valid:
                                ProcessAttachments(item);
                                if (isPOOk)
                                    poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "pook");
                                else
                                    poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "ponotok");
                                break;

                            default:
                                poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "canceled");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        poReq.PORequestStatus = PoStatusList.FirstOrDefault(i => i.Value?.ToLower() == "internalservererror");
                        PORequestList.Add(poReq);
                        Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() for Email id {1}. ErrorMessage- {0}", ex.ToString(), item.IdEmail), category: Category.Exception, priority: Priority.Low);
                    }

                    isSavePoReq = OtmStartUp.AddPORequest(poReq);
                    if (isSavePoReq)
                        Log4NetLogger.Logger.Log(string.Format("Po Attachment status updated in table.... "), category: Category.Info, priority: Priority.Low);
                }
                #endregion
            }
            Log4NetLogger.Logger.Log(string.Format("AnalyseEmails Method Executed.... "), category: Category.Info, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
        }
    }
    EmailStatus GetEmailStatus(Email item, string domain)
    {
        try
        {
            Log4NetLogger.Logger.Log(string.Format("GetEmailStatus Method Started.... "), category: Category.Info, priority: Priority.Low);
            if (BlackListEmailList.Any(i =>
                i.SenderEmail?.Trim() == item.SenderEmail?.Trim() ||
                i.Domain?.Trim() == domain))
            {
                return EmailStatus.Blacklisted;
            }

            if (item.EmailattachmentList == null || item.EmailattachmentList.Count == 0)
            {
                return EmailStatus.MissingAttachment;
            }

            if (item.EmailattachmentList.Any(i =>
                UntrustedExtensionList.Any(j =>
                    j.Value?.Trim() == i.AttachmentExtension?.Trim())))
            {
                return EmailStatus.UnsupportedAttachment;
            }
            Log4NetLogger.Logger.Log(string.Format("GetEmailStatus Method Executed.... "), category: Category.Info, priority: Priority.Low);
            return EmailStatus.Valid;
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log(string.Format("ERROR in  AnalyseEmails() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            throw;
        }
    }
    void ProcessAttachments(Email item)
    {
        try
        {
            CustomerDetailList = new List<CustomerDetail>();
            Log4NetLogger.Logger.Log(string.Format("ProcessAttachments Method Started.... "), category: Category.Info, priority: Priority.Low);
            foreach (var attach in item.EmailattachmentList)
            {
                if (attach.AttachmentExtension == ".pdf" && attach.FileText != null)
                {
                    string pdfText = attach.FileText;
                    TemplateSetting SelectedTemplate = TemplateSettingList.FirstOrDefault(i => pdfText.Contains(i.TemplateName));

                    if (SelectedTemplate != null)
                    {
                        DetailExtraction(pdfText, SelectedTemplate);
                        isPOOk = true;
                    }
                    else
                        isPOOk = false;
                }
            }
            if (CustomerDetailList?.Count > 0 && isPOOk == true)
            {
                isSavePoDetails = OtmStartUp.AddPODetails(CustomerDetailList);
                if (isSavePoDetails)
                    isPOOk = true;
            }
            Log4NetLogger.Logger.Log(string.Format("ProcessAttachments Method Executed.... "), category: Category.Info, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            isPOOk = false;
            Log4NetLogger.Logger.Log(string.Format("ERROR in  ProcessAttachments(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            throw;
        }
    }
    private void DetailExtraction(string extracted_text, TemplateSetting xml_config)
    {
        CustomerDetail poDetail = new CustomerDetail();
        poDetail.Customer = xml_config.TemplateName;
        foreach (var item in xml_config.TagList)
        {
            if (item.TagValueList?.Count > 0)
            {
                foreach (var tag in item.TagValueList)
                {
                    try
                    {
                        if (extracted_text.ToLower().Contains(tag.TagValue?.ToLower()))
                        {
                            string data_after_word = extracted_text.Substring(extracted_text.IndexOf(tag.TagValue) + 1);
                            string filteredData = filterData(data_after_word, tag.SkipValue, tag.TagValue);

                            #region Assign Properties To save in database
                            switch (item.Value.ToLower())
                            {
                                case "po_number":
                                    poDetail.PONumber = filteredData;
                                    break;

                                case "incoterm":
                                    poDetail.Incoterm = filteredData;
                                    break;

                                case "email":
                                    poDetail.Email = filteredData;
                                    break;

                                case "offer":
                                    poDetail.Offer = filteredData;
                                    break;

                                case "date":
                                    DateTime parsedDate;
                                    if (IsDate(filteredData, out parsedDate) && DateTime.TryParse(filteredData, out parsedDate))
                                    {
                                        poDetail.DateIssued = parsedDate;
                                    }
                                    break;

                                case "total":
                                    decimal Amount = 0;
                                    string amountpattern = @"\d{1,3}(,\d{3})*(\.\d{2})?";
                                    Match amountmatch = Regex.Match(filteredData, amountpattern);
                                    if (amountmatch.Success)
                                    {
                                        bool isParsed = decimal.TryParse(amountmatch.Value, NumberStyles.Any, new CultureInfo("en-US"), out Amount);
                                        if (!isParsed)
                                        {
                                            isParsed = decimal.TryParse(amountmatch.Value, NumberStyles.Any, new CultureInfo("de-DE"), out Amount);
                                        }
                                        poDetail.TotalNetValue = Convert.ToDouble(Amount);
                                    }

                                    break;

                                case "currency":
                                    string pattern = @"\b(?:EUR|RON|USD|MXN|CNY|HNL|BRL|TND|MAD|PYG|RUB|INR|GBP|CAD|CHF|RSD|NIO|EGP|IDR)\b";
                                    Match match = Regex.Match(filteredData, pattern);
                                    if (match.Success)
                                    {
                                        poDetail.Currency = match.Value;
                                    }
                                    break;

                                default:
                                    break;
                            }
                            #endregion


                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("ERROR in DetailExtraction(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
        }
        CustomerDetailList.Add(poDetail);
    }
    static bool IsDate(string input, out DateTime parsedDate)
    {
        string cleanedInput = Regex.Replace(input, @"\b(\d{1,2})(st|nd|rd|th)\b", "$1");

        if (DateTime.TryParse(cleanedInput, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return true;
        }

        string[] dateFormats = { "dd-MMM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd", "dd-mm-yyyy", "dd MMM yyyy", "yyyy.MM.dd", "dd.mm.yyyy" };

        if (DateTime.TryParseExact(cleanedInput, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return true;
        }
        return false;
    }
    private string filterData(string value, int skip_val, string wordToKeep)
    {
        Log4NetLogger.Logger.Log(string.Format("filterData Method Started.... "), category: Category.Info, priority: Priority.Low);
        string target = string.Empty;
        try
        {
            string[] delimiters = new string[] { "\r\n", "\r", "\n", ":", " " };
            string pattern = $@"(?<!\b{Regex.Escape(wordToKeep)}\s)({string.Join("|", delimiters.Select(Regex.Escape))})";
            string[] lines = Regex.Split(value, pattern).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            int targetIndex = skip_val + 2;
            if (targetIndex >= 0 && targetIndex < lines.Length)
            {
                target = lines[targetIndex];
            }
            Log4NetLogger.Logger.Log(string.Format("filterData Method Executed.... "), category: Category.Info, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log(string.Format("ERROR in  filterData() - OnStart(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
        }
        return target;
    }
    public void FillTemplateSettingList()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillTemplateSettingList()...", category: Category.Info, priority: Priority.Low);
            TemplateSettingList = OtmStartUp.GetAllTags();
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillTemplateSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    public void FillPoStatusList()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillPoStatusList()...", category: Category.Info, priority: Priority.Low);
            PoStatusList = OtmStartUp.GetLookupValues(159).ToList();
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillPoStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    public void FillUntrustedExtensionList()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillUntrustedExtensionList()...", category: Category.Info, priority: Priority.Low);
            UntrustedExtensionList = OtmStartUp.GetLookupValues(157);
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUntrustedExtensionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    public void FillCurrencyList()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillCurrencyList()...", category: Category.Info, priority: Priority.Low);
            CurrencyList = OtmStartUp.GetAllCurrencies().ToList();
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillCurrencyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    public void FillBlackListEmails()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillBlackListEmails()...", category: Category.Info, priority: Priority.Low);

            BlackListEmailList = OtmStartUp.GetAllBlackListEmails();
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillBlackListEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    public void FillUnprocessedEmails()
    {
        try
        {
            Log4NetLogger.Logger.Log("Method FillUnprocessedEmails()...", category: Category.Info, priority: Priority.Low);

            EmailList = OtmStartUp.GetAllUnprocessedEmails();
        }
        catch (FaultException<ServiceException> ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (ServiceUnexceptedException ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
        catch (Exception ex)
        {
            Log4NetLogger.Logger.Log("Get an error in Method FillUnprocessedEmails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        }
    }
    */
    #endregion
    //public enum EmailStatus
    //{
    //    None,
    //    Blacklisted,
    //    MissingAttachment,
    //    UnsupportedAttachment,
    //    Valid
    //}
}
