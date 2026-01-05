using ActivityReminder.Classes;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ActivityReminderServices
{
    /// <summary>
    /// 
    /// [001][skhade][30-12-2019][GEOS2-1924] Welcome on board notification
    /// [002][skhade][26-04-2020][GEOS2-2226] Activity reminder fails for employee leave task
    /// </summary>
    public partial class ActivityReminderServices : ServiceBase
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(ActivityReminder.Properties.Settings.Default.SERVICE_PROVIDER_URL);
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(ActivityReminder.Properties.Settings.Default.SERVICE_PROVIDER_URL);
        IHrmService HrmStartUp = new HrmServiceController(ActivityReminder.Properties.Settings.Default.SERVICE_PROVIDER_URL);

        #endregion // Services

        #region Declaration

        private Timer timer1 = null;
        public List<Activity> listActivity = null;
        public IList<LookupValue> TypeList { get; set; }
        DateTime currentDateTime;
        public IList<Currency> Currencies { get; set; }
        public ImageSource UserProfileImage { get; set; }
        public List<LinkedResource> ImgResourceList { get; set; }
        List<Employee> EmployeesBirthday { get; set; }
        List<Company> Companies { get; set; }

        CultureInfo CultureEnglish = new CultureInfo("en-GB");
        List<Employee> EmployeeCompanyAnniversaries { get; set; }

        List<AutomaticReport> automaticReportListGobal = new List<AutomaticReport>();

        static readonly object _object = new object();

        #endregion // Declaration

        #region Constructor

        /// <summary>
        /// [001] Constructor
        /// [002][skhade][03-09-2018] Added try catch to log exceptions.
        /// </summary>
        public ActivityReminderServices()
        {
            InitializeComponent();
            
           
            //try
            //{
            //    Currencies = CrmStartUp.GetCurrencies();
            //}
            //catch (FaultException<ServiceException> ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -GetCurrencies- FaultException - {0}", ex.Detail.ErrorMessage));
            //}
            //catch (ServiceUnexceptedException ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -GetCurrencies- ServiceUnexceptedException - {0}", ex.Message));
            //}
            //catch (Exception ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -GetCurrencies- Exception - {0}", ex.Message));
            //}

            ////Timer
            //try
            //{
            //    timer1 = new Timer();
            //    this.timer1.Interval = ActivityReminder.Properties.Settings.Default.INTERVAL_REFRESH;
            //    this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            //    timer1.Enabled = true;
            //}
            //catch (FaultException<ServiceException> ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -Timer- FaultException - {0}", ex.Detail.ErrorMessage));
            //}
            //catch (ServiceUnexceptedException ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -Timer- ServiceUnexceptedException - {0}", ex.Message));
            //}
            //catch (Exception ex)
            //{
            //    clsLogFile.WriteErrorLog(string.Format("[ERROR] ActivityReminderServices() -Timer- Exception - {0}", ex.Message));
            //}
        }

        #endregion

        /// <summary>
        /// Need to initialize while start service
        /// <para>[000][Ganaraj Chavan][27-05-2020][GEOS2-2361] GeosWokbenchActivityReminderService service stopped then never start again.</para>
        /// </summary>
        internal void InitiaizeOnStartService()
        {
            //Timer
            try
            {
                timer1 = new Timer();
                this.timer1.Interval = ActivityReminder.Properties.Settings.Default.INTERVAL_REFRESH;
                this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
                timer1.Enabled = true;
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -Timer- ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -Timer- Exception - {0}", ex.Message));
            }
        }

        #region Methods

        /// <summary>
        /// [001] Timer for Service.
        /// [002][skhade][02-10-2018] Added lock on object as to excute function once completely then move to another.
        /// [003][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The elapsed event args.</param>
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            if (timer1 != null)
                timer1.Enabled = false;

            lock (_object)
            {
                if (automaticReportListGobal != null && automaticReportListGobal.Count == 0)
                {
                    automaticReportListGobal = WorkbenchStartUp.GetAutomaticReports();
                    clsLogFile.WriteErrorLog(string.Format("[INFO] timer1_Tick() -Automatic Report List count - {0}", automaticReportListGobal.Count));
                }
                //Report
                GetActivities();
                AutomaticReportMailSendToSalesPerson();
                //Currency
                CurrencyConvert();
                //HRM - Mails
                GetTodayBirthdayOfEmployees();
                GetUpcomingCompanyHolidaysEmployees();
                GetUpcomingEmployeeLeavesByCompany();
                GetCompanyAnniversariesOfEmployees();
                //HRM-Employee inactive
                SetEmployeeInactive();
                //001
                GetEmployeesWelcomeOnBoard();

                //employee contract expiration
                GetEmployeeContractExpirations();
            }

            if (timer1 != null)
                timer1.Enabled = true;
        }

        /// <summary>
        /// Currency Conversion logic.
        /// [002][skhade][04-09-2018] Added try catch blocks to catch errors.
        /// <para>[003][Ganaraj Chavan][27-05-2020][GEOS2-2361] GeosWokbenchActivityReminderService service stopped then never start again.</para>
        /// </summary>
        public void CurrencyConvert()
        {
            AutomaticReport objAutomaticReport = null;
            //[003]
            try
            {
                objAutomaticReport = automaticReportListGobal.FirstOrDefault(x => x.IdAutomaticReport == 2);
                if (Currencies == null || Currencies.Count == 0)
                {
                    Currencies = CrmStartUp.GetCurrencies();
                    clsLogFile.WriteErrorLog(string.Format("[INFO] InIt Currencies Count - CurrencyConvert() - {0}", Currencies.Count));
                }
                if (objAutomaticReport != null && objAutomaticReport.IdAutomaticReport == 2)
                {
                    if (objAutomaticReport.IsEnabled == 1 && objAutomaticReport.StartDate.Date < currentDateTime.Date)
                    {
                        Currencies = CrmStartUp.GetCurrencies();
                        clsLogFile.WriteErrorLog(string.Format("[INFO] After Start Currencies Count - CurrencyConvert() - {0}", Currencies.Count));
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -GetCurrencies- FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -GetCurrencies- ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] InitiaizeOnStartService() -GetCurrencies- Exception - {0}", ex.Message));
            }
            //if (timer1 != null)
            //    timer1.Enabled = false;

            clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - CurrencyConvert()"));

            try
            {
                string nameOfString = (string.Join(",", Currencies.Select(x => x.Name.ToString()).ToArray()));

                List<DailyCurrencyConversion> dailyCurrencyConversionList = new List<DailyCurrencyConversion>();
                List<DailyCurrencyConversion> Listcur = new List<DailyCurrencyConversion>();

                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                currentDateTime = WorkbenchStartUp.GetServerDateTime();
                objAutomaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 2);

                // 2-id currency report.
                if (objAutomaticReport != null && objAutomaticReport.IdAutomaticReport == 2)
                {
                    //if Some data is missing then fill gap with last added currrency_conversion.
                    if (objAutomaticReport.IsEnabled == 1 && objAutomaticReport.StartDate.Date < currentDateTime.Date)
                    {
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updating earlier currency data with latest"));

                        Listcur = CrmStartUp.GetLatestCurrencyConversion();
                        double count = (currentDateTime.Date.AddDays(-1) - Listcur[0].CurrencyConversionDate).TotalDays;

                        for (int i = 0; i < count; i++)
                        {
                            foreach (DailyCurrencyConversion item in Listcur)
                            {
                                DailyCurrencyConversion objDailyCurrencyConversion = new DailyCurrencyConversion();

                                objDailyCurrencyConversion.IdCurrencyConversionFrom = item.IdCurrencyConversionFrom;
                                objDailyCurrencyConversion.IdCurrencyConversionTo = item.IdCurrencyConversionTo;
                                objDailyCurrencyConversion.CurrencyConversionDate = item.CurrencyConversionDate.AddDays(i + 1);
                                objDailyCurrencyConversion.IsCorrect = 0;
                                objDailyCurrencyConversion.CurrencyConversationRate = item.CurrencyConversationRate;

                                //CrmStartUp.UpdateCurrencyConversionDaily(objDailyCurrencyConversion);
                                dailyCurrencyConversionList.Add(objDailyCurrencyConversion);
                            }
                        }

                        if (dailyCurrencyConversionList.Count > 0)
                            CrmStartUp.UpdateCurrencyConversionListDaily(dailyCurrencyConversionList);

                        objAutomaticReport.StartDate = currentDateTime.Date.AddDays(-1);
                        WorkbenchStartUp.UpdateAutomaticReport(objAutomaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date to today"));
                    }

                    automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                    objAutomaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 2);

                    //Add latest currency conversion from api.
                    if (objAutomaticReport.IsEnabled == 1 && objAutomaticReport.StartDate.Date.Equals(currentDateTime.Date))
                    {
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updating currency data for today"));

                        foreach (Currency itemCurrencies in Currencies)
                        {
                            dailyCurrencyConversionList = new List<DailyCurrencyConversion>();

                            string svalue = ActivityReminder.Properties.Settings.Default.CurrencyLayer.ToString();
                            string skey = ActivityReminder.Properties.Settings.Default.CurrencyLayerKey.ToString();
                            var instance = new Emdep.Geos.CurrencyConvertApi.CurrencyLayerApi(svalue, skey);
                            string currencysource = itemCurrencies.Name;

                            string finalstring = svalue + "live?" + "access_key=" + skey + "&source=" + currencysource + "&format=1" + "&currencies=" + nameOfString;
                            string json = new WebClient().DownloadString(finalstring);

                            var todayRatesQueried = JsonConvert.DeserializeObject<Emdep.Geos.CurrencyConvertApi.Models.HistoryModel>(json);

                            //var todayRatesQueried = instance.Invoke<Emdep.Geos.CurrencyConvertApi.Models.LiveModel>
                            //    ("live", new Dictionary<string, string>
                            //     {
                            //    { "Source",currencysource },
                            //    { "currencies",nameOfString },
                            //      }
                            //);

                            if (todayRatesQueried.Success)
                            {
                                string consource = String.Empty;

                                try
                                {
                                    consource = Convert.ToString(todayRatesQueried.Source);
                                }
                                catch (Exception e)
                                {
                                    clsLogFile.WriteErrorLog("[ERROR] todayRatesQueried " + e.Message.ToString());
                                }

                                //objDailyCurrencyConversion.IdCurrencyConversionFrom
                                byte IdCurrencyConversionFrom = Currencies.FirstOrDefault(x => x.Name == consource).IdCurrency;

                                foreach (var item in todayRatesQueried.quotes)
                                {
                                    DailyCurrencyConversion objDailyCurrencyConversion = new DailyCurrencyConversion();

                                    string[] tokens = item.Key.ToString().Split(new[] { consource }, StringSplitOptions.None);
                                    if (tokens[1] != "")
                                    {
                                        objDailyCurrencyConversion.IdCurrencyConversionTo = Currencies.FirstOrDefault(x => x.Name == tokens[1]).IdCurrency;
                                    }
                                    else
                                    {
                                        objDailyCurrencyConversion.IdCurrencyConversionTo = IdCurrencyConversionFrom;
                                    }

                                    objDailyCurrencyConversion.IdCurrencyConversionFrom = IdCurrencyConversionFrom;

                                    objDailyCurrencyConversion.CurrencyConversionDate = System.DateTime.Now;
                                    float value = float.Parse(item.Value, CultureInfo.CreateSpecificCulture(ActivityReminder.Properties.Settings.Default.CultureInfo));
                                    objDailyCurrencyConversion.CurrencyConversationRate = value;
                                    objDailyCurrencyConversion.IsCorrect = 1;

                                    //CrmStartUp.UpdateCurrencyConversionDaily(objDailyCurrencyConversion); //remove
                                    dailyCurrencyConversionList.Add(objDailyCurrencyConversion);
                                }

                                if (dailyCurrencyConversionList.Count > 0)
                                {
                                    CrmStartUp.UpdateCurrencyConversionListDaily(dailyCurrencyConversionList);
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date."));
                                }
                            }
                            else
                            {
                                Listcur = CrmStartUp.GetLatestCurrencyConversion();

                                foreach (DailyCurrencyConversion item in Listcur)
                                {
                                    DailyCurrencyConversion objDailyCurrencyConversion = new DailyCurrencyConversion();

                                    objDailyCurrencyConversion.IdCurrencyConversionFrom = item.IdCurrencyConversionFrom;
                                    objDailyCurrencyConversion.IdCurrencyConversionTo = item.IdCurrencyConversionTo;
                                    objDailyCurrencyConversion.CurrencyConversionDate = System.DateTime.Now;
                                    objDailyCurrencyConversion.IsCorrect = 0;
                                    objDailyCurrencyConversion.CurrencyConversationRate = item.CurrencyConversationRate;

                                    dailyCurrencyConversionList.Add(objDailyCurrencyConversion);

                                    //CrmStartUp.UpdateCurrencyConversionDaily(objDailyCurrencyConversion); //remove
                                }

                                if (dailyCurrencyConversionList.Count > 0)
                                {
                                    CrmStartUp.UpdateCurrencyConversionListDaily(dailyCurrencyConversionList);
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date."));
                                }

                                //send mail 
                                string SystemNotifications = "";
                                SystemNotifications = "[CRM] " + objAutomaticReport.Name + "_" + System.DateTime.Now.ToString("dd / MM / yyyy");
                                StringBuilder EmailBodyWithTable = new StringBuilder();
                                EmailBodyWithTable.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
                                EmailBodyWithTable.Append("<tr style=\" background-color:#D3D3D3;\">");
                                EmailBodyWithTable.Append("<td colspan=\"2\"> <b>" + "Common API errors" + "</td>");
                                EmailBodyWithTable.Append("</ tr >");
                                EmailBodyWithTable.Append("<tr>");
                                EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=20%>\n");
                                EmailBodyWithTable.Append("<b> Code </td>\n");
                                EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=65%>\n");
                                EmailBodyWithTable.Append("<b> Info [affected API endpoints] </td>\n");
                                EmailBodyWithTable.Append("<tr>\n");
                                EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + todayRatesQueried.Error.Code + "</td>\n");
                                EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + todayRatesQueried.Error.Info + "</td>\n");
                                EmailBodyWithTable.Append("</table>");
                                EmailBodyWithTable.Append("<br>\n");

                                string[] tokens = ActivityReminder.Properties.Settings.Default.SystemNotificationsRecipients.ToString().Split(new[] { ';' }, StringSplitOptions.None);

                                try
                                {
                                    foreach (var item in tokens)
                                    {
                                        MailControl.SendHtmlMail(SystemNotifications, EmailBodyWithTable.ToString(), item.ToString(), "CRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, new Dictionary<string, string>());
                                        clsLogFile.WriteErrorLog(string.Format("[INFO] Currency conversion API errors - mail sent to {0}", item.ToString()));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    clsLogFile.WriteErrorLog("[ERROR] CurrencyConvert() SendHtmlMail " + ex.Message);
                                }

                                break;
                            }
                        }

                        WorkbenchStartUp.UpdateAutomaticReport(objAutomaticReport);
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] CurrencyConvert() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] CurrencyConvert() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] CurrencyConvert() Exception - {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }

            clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - CurrencyConvert()"));
        }

        /// <summary>
        /// Method for send mail for new activity.
        /// </summary>
        private void GetActivities()
        {
            //if (timer1 != null)
            //    timer1.Enabled = false;

            clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetActivities()"));

            try
            {
                listActivity = CrmStartUp.GetActivitiesForActivityReminder();

                if (listActivity != null && listActivity.Count > 0)
                {
                    foreach (Activity itemActivity in listActivity)
                    {
                        ActivityMail activityMail = new ActivityMail();

                        Activity act = new Activity();
                        act.IdActivity = itemActivity.IdActivity;
                        act.IsSentMail = 1;
                        CrmStartUp.UpdateActivityReminder(act);

                        activityMail.SendToUserName = itemActivity.People.FullName;
                        activityMail.ActivitySentToMail = itemActivity.People.Email;
                        activityMail.ActivityType = itemActivity.LookupValue.Value;
                        activityMail.ActivitySubject = itemActivity.Subject;
                        activityMail.ActivityDescription = itemActivity.Description;
                        activityMail.ActivityDueDate = itemActivity.ToDate.ToString();

                        CrmStartUp.SendActivityReminderMail(activityMail);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Activity email sent to - {0} email - {1}", itemActivity.People.FullName, itemActivity.People.Email));
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetActivities() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetActivities() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetActivities() - Exception - {0} ", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }

            clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetActivities()"));
        }

        /// <summary>
        /// Method for send mail Automaticaly on perticular interval. 
        /// </summary>
        private void AutomaticReportMailSendToSalesPerson()
        {
            //if (timer1 != null)
            //    timer1.Enabled = false;

            clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - AutomaticReportMailSendToSalesPerson()"));

            try
            {
                if (TypeList == null)
                    TypeList = CrmStartUp.GetLookupValues(9);

                currentDateTime = WorkbenchStartUp.GetServerDateTime();

                List<AutomaticReport> automaticReportLst = WorkbenchStartUp.GetAutomaticReports();

                AutomaticReport automaticReport = automaticReportLst.FirstOrDefault(x => x.IdAutomaticReport == 1);

                //foreach (AutomaticReport automaticReport in automaticReportLst)

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date.Equals(currentDateTime.Date))
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        List<SalesUser> salesUserLst = CrmStartUp.GetAllSalesUsersForReport();

                        //prev timer disabled

                        if (salesUserLst != null && salesUserLst.Count > 0)
                        {
                            List<Activity> activityList = CrmStartUp.GetActivitiesGoingToDueInInverval(automaticReport.Interval);
                            List<Offer> offers;

                            foreach (SalesUser item in salesUserLst)
                            {
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing data for sales user - {0}", item.People.FullName));

                                List<Activity> usersActivity = activityList.Where(ac => ac.IdOwner == item.IdSalesUser).Select(act => act).ToList();
                                offers = new List<Offer>();

                                List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails(item.IdSalesUser);

                                foreach (var itemCompaniesDetails in CompanyList)
                                {
                                    try
                                    {
                                        List<Offer> offlst = new List<Offer>();

                                        offlst = CrmStartUp.GetReportOffersPerSalesUser(1, item.IdSalesUser, 1, currentDateTime.Year, itemCompaniesDetails, automaticReport.Interval);
                                        offers.AddRange(offlst);
                                    }
                                    catch (Exception ex)
                                    {
                                        clsLogFile.WriteErrorLog("[ERROR] Method AutomaticReportMailSendToSalesPerson " + ex.Message);
                                    }
                                }

                                if (usersActivity.Count > 0 || offers.Count > 0)
                                    SendMailActivity(usersActivity, offers, automaticReport, item);
                                else
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] No data found for sales user - {0}", item.People.FullName));
                            }
                        }

                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date."));

                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is earlier than today."));
                    }
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] AutomaticReportMailSendToSalesPerson() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] AutomaticReportMailSendToSalesPerson() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] AutomaticReportMailSendToSalesPerson() - Exception - {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }

            clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - AutomaticReportMailSendToSalesPerson()"));
        }

        /// <summary>
        /// Method for send mail Automaticaly. 
        /// </summary>
        private void SendMailActivity(List<Activity> ActivityList, List<Offer> userOffersList, AutomaticReport automaticReport, SalesUser username)
        {
            try
            {
                // DateTime currentDateTime = WorkbenchStartUp.GetServerDateTime();
                //clsLogFile.WriteErrorLog("[INFORMATION] Start creating report subject ... ");

                string reportSubject = string.Empty;
                CultureInfo ciCurr = CultureInfo.CurrentCulture;

                //[Start] Set Mail Subject.
                if (automaticReport.Interval.Equals("daily"))
                {
                    reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("dd / MM / yyyy");
                }
                else if (automaticReport.Interval.Equals("weekly"))
                {
                    int weekNum = ciCurr.Calendar.GetWeekOfYear(automaticReport.StartDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("yyyy") + "_W" + weekNum;
                }
                else if (automaticReport.Interval.Equals("monthly"))
                {
                    reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("yyyy") + "_" + automaticReport.StartDate.ToString("MMMM");
                }
                else if (automaticReport.Interval.Equals("quarterly"))
                {
                    reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("yyyy") + "_Q" + Math.Ceiling(automaticReport.StartDate.Month / 3.0) + "";
                }
                else if (automaticReport.Interval.Equals("yearly"))
                {
                    reportSubject = "[CRM] " + automaticReport.Name + "_" + automaticReport.StartDate.ToString("yyyy");
                }

                //[End] Set Mail Sublect.

                List<Offer> offersWaitingforQuoteLst = new List<Offer>();
                List<Offer> offersCloseDateOverdueLst = new List<Offer>();
                List<Offer> offersUnactivityListFinal = new List<Offer>();

                if (userOffersList.Count > 0)
                {
                    offersWaitingforQuoteLst = userOffersList.Where(of => of.GeosStatus.IdOfferStatusType == 2).Select(i => i).ToList();
                    offersCloseDateOverdueLst = userOffersList.Where(of => of.DeliveryDate < currentDateTime).Select(i => i).ToList();
                    List<Offer> offersUnactivityLst = new List<Offer>();
                    offersUnactivityLst = userOffersList.Where(of => of.LastActivityDate.HasValue && of.LastActivityDate < currentDateTime).Select(i => i).ToList();

                    foreach (Offer item in offersUnactivityLst)
                    {
                        if (Convert.ToInt32((currentDateTime - item.LastActivityDate.Value.Date).TotalDays) > 30)
                        {
                            //unactivity add
                            offersUnactivityListFinal.Add(item);
                        }
                    }
                }

                StringBuilder EmailBodyWithTable = new StringBuilder();

                //EmailBodyWithTable.Append("<B>user Name "+ username.People.Name + "\n");

                foreach (var item1 in TypeList)
                {
                    List<Activity> TempActivityList = ActivityList.Where(x => x.LookupValue.IdLookupValue == item1.IdLookupValue).ToList();

                    // for create activity table template.
                    if (TempActivityList.Count > 0)
                    {
                        //EmailBodyWithTable.Append(@"<style>
                        //                                table, th, td { font-family : Segoe UI; }
                        //                            </style>");
                        EmailBodyWithTable.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
                        EmailBodyWithTable.Append("<tr style=\" background-color:#D3D3D3;\">");

                        if (item1.IdLookupValue == 40)
                        {
                            EmailBodyWithTable.Append("<td colspan=\"3\"> <b>" + item1.Value + "</td>");
                            EmailBodyWithTable.Append("<td style=\" text-align:right; \"> <b> " + TempActivityList.Count + " </td>");
                        }
                        else
                        {
                            EmailBodyWithTable.Append("<td colspan=\"2\"> <b>" + item1.Value + "</td>");
                            EmailBodyWithTable.Append("<td style=\" text-align:right; \"> <b> " + TempActivityList.Count + " </td>");
                        }

                        EmailBodyWithTable.Append("</ tr >");

                        EmailBodyWithTable.Append("<tr>");
                        EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=20%>\n");
                        EmailBodyWithTable.Append("<b> Title </td>\n");
                        EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=65%>\n");
                        EmailBodyWithTable.Append("<b> Description </td>\n");
                        EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
                        EmailBodyWithTable.Append("<b> Date &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>\n");

                        if (item1.IdLookupValue == 40)
                        {
                            EmailBodyWithTable.Append("<td style=\" border-color:#5c87b2;text-align:left; border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
                            EmailBodyWithTable.Append("<b> Status &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>\n");
                        }

                        EmailBodyWithTable.Append("</tr>\n");

                        foreach (Activity activity in TempActivityList)
                        {
                            EmailBodyWithTable.Append("<tr>\n");
                            EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.Subject + "</td>\n");
                            EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.Description + "</td>\n");
                            EmailBodyWithTable.Append("<td style=\"text-align:right; \">" + activity.ToDate.Value.ToShortDateString() + "</td>\n");

                            if (item1.IdLookupValue == 40)
                            {
                                EmailBodyWithTable.Append("<td style=\"text-align:left; \">" + activity.ActivityStatus.Value + "</td>\n");
                            }

                            EmailBodyWithTable.Append("</tr>\n");
                        }
                    }

                    EmailBodyWithTable.Append("</table>");
                    EmailBodyWithTable.Append("<br>\n");
                }

                //Start Created offers Tables.
                if (offersWaitingforQuoteLst.Count > 0)
                {
                    string offerTableStr = CreateOfferTables(offersWaitingforQuoteLst, "Offer Waiting for Quote").ToString();
                    EmailBodyWithTable.Append(offerTableStr);
                }

                if (offersCloseDateOverdueLst.Count > 0)
                {
                    string offerTableStr = CreateOfferTables(offersCloseDateOverdueLst, "Offer Close Date Overdue").ToString();
                    EmailBodyWithTable.Append(offerTableStr);
                }

                if (offersUnactivityListFinal.Count > 0)
                {
                    string offerTableStr = CreateOfferTables(offersUnactivityListFinal, "Offer Unactivity").ToString();
                    EmailBodyWithTable.Append(offerTableStr);
                }
                //End Created offers Tables.

                if (ActivityList.Count > 0 ||
                    offersWaitingforQuoteLst.Count > 0 ||
                    offersCloseDateOverdueLst.Count > 0 ||
                    offersUnactivityListFinal.Count > 0)
                {
                    MailControl.SendHtmlMail(reportSubject, EmailBodyWithTable.ToString(), username.People.Email, "CRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, new Dictionary<string, string>());
                    clsLogFile.WriteErrorLog(string.Format("[INFO] Report prepared and mail sent to {0} - {1}", username.People.FullName, username.People.Email));
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog("[ERROR] Method SendMailActivity " + ex.Message);
            }
        }

        /// <summary>
        /// Method for create create offers table template for mail.
        /// </summary>
        /// <param name="offerlst"></param>
        /// <param name="tableHeader"></param>
        /// <returns></returns>
        private StringBuilder CreateOfferTables(List<Offer> offerlst, string tableHeader)
        {
            StringBuilder emailbodyStr = new StringBuilder();

            try
            {
                double amount = offerlst.Sum(of => of.Value);

                CultureInfo culture_Euro = new CultureInfo("es-Es");
                string amountstr = amount.ToString("C", culture_Euro);

                //For Created offers.
                if (offerlst.Count > 0)
                {
                    emailbodyStr.Append("<br>\n");
                    //emailbodyStr.Append(@"<style>
                    //                        table, th, td { font-family : Segoe UI; }
                    //                    </style>");
                    emailbodyStr.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
                    emailbodyStr.Append("<tr style=\" background-color:#D3D3D3;\">");

                    emailbodyStr.Append("<td colspan=\"5\"> <b>" + tableHeader + "</td>");
                    emailbodyStr.Append("<td style=\" text-align:right; \"> <b> " + offerlst.Count + " </td>");

                    emailbodyStr.Append("<td style=\" text-align:right; \"> <b>" + amountstr + " </td>");

                    emailbodyStr.Append("</ tr >");

                    emailbodyStr.Append("<tr>");
                    emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=20%>\n");
                    emailbodyStr.Append("<b> Code </td>\n");
                    emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=65%>\n");
                    emailbodyStr.Append("<b> Description </td>\n");
                    emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
                    emailbodyStr.Append("<b> Group </td>\n");

                    emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
                    emailbodyStr.Append("<b> Plant </td>\n");

                    emailbodyStr.Append("<th style=\" border-color:#5c87b2; text-align:left; border-style:solid; border-width:thin; padding: 2px;\" >\n");
                    emailbodyStr.Append("<b> Confidence Level(%) </th>\n");

                    emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 2px;\" width=30px>\n");
                    emailbodyStr.Append("<b> Date &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>\n");

                    emailbodyStr.Append("<td style=\" border-color:#5c87b2;  border-style:solid; border-width:thin; padding: 2px;\" width=15%>\n");
                    emailbodyStr.Append("<b> Amount &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>\n");

                    emailbodyStr.Append("</tr>\n");

                    foreach (var item in offerlst)
                    {
                        emailbodyStr.Append("<tr>\n");
                        emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Code + "</td>\n");
                        emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Description + "</td>\n");
                        emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Site.Customer.CustomerName + "</td>\n");
                        emailbodyStr.Append("<td style=\"text-align:left; \">" + item.Site.Name + "</td>\n");

                        emailbodyStr.Append("<td style=\"text-align:left; \">" + item.ProbabilityOfSuccess + "</td>\n");

                        if (item.DeliveryDate != null)
                            emailbodyStr.Append("<td style=\"text-align:right; \" >" + item.DeliveryDate.Value.ToShortDateString() + "</td>\n");
                        else
                            emailbodyStr.Append("<td style=\"text-align:right; \">" + string.Empty + "</td>\n");

                        emailbodyStr.Append("<td style=\"text-align:right; \">" + item.Value.ToString("C", culture_Euro) + "</td>\n");


                        emailbodyStr.Append("</tr>\n");
                    }

                    emailbodyStr.Append("</table>");
                    emailbodyStr.Append("<br>\n");
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog("[ERROR] Method CreateOfferTables() " + ex.Message);
            }

            return emailbodyStr;
        }

        /// <summary>
        /// [001][skhade][2018-08-20][HRM-M045-15] New automatic report for birthdays.
        /// [002][skale][18-07-2019][GEOS2-1600]Some emails are not sent
        /// [003][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        /// </summary>
        public void GetTodayBirthdayOfEmployees()
        {
            try
            {
                //if (timer1 != null)
                //    timer1.Enabled = false;
                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetTodayBirthdayOfEmployees()"));

                currentDateTime = DateTime.Now;
                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 3);

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date == DateTime.Now.Date)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        if (EmployeesBirthday == null)
                        {
                            //[003]Changed service method
                            EmployeesBirthday = HrmStartUp.GetTodayBirthdayOfEmployees_V2045(0, DateTime.Now);
                            if (EmployeesBirthday.Count > 0) //[002] added
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Birthday email to send - {0}", String.Join(",", EmployeesBirthday.ToList().Select(x => x.FirstName + " " + x.LastName))));
                        }
                        else if (EmployeesBirthday != null && EmployeesBirthday.Count == 0)
                        {
                            //[003]Changed service method
                            EmployeesBirthday = HrmStartUp.GetTodayBirthdayOfEmployees_V2045(0, DateTime.Now);
                            if (EmployeesBirthday.Count > 0)// //[002] added added
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Birthday email to send - {0}", String.Join(",", EmployeesBirthday.ToList().Select(x => x.FirstName + " " + x.LastName))));
                        }

                        foreach (Employee employee in EmployeesBirthday)
                        {
                            try
                            {
                                if (employee.IsEnabled == 0)  // isenable updated when email send to avoid repetation
                                {
                                    string timeZoneId = employee.EmployeeJobDescriptions[0].Company.TimeZoneIdentifier;
                                    DateTime localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentDateTime, timeZoneId);
                                    //[002] added added
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Employee name is {0} and company TimeZoneIdentifier is {1} and current date time {2} and local date time is {3}", employee.FullName, timeZoneId, currentDateTime, localDateTime));

                                    if (automaticReport.StartDate.Date == localDateTime.Date && localDateTime.Hour >= 00 && localDateTime.Hour <= 23)
                                    {
                                        clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing Birthday email - {0}", employee.FullName));

                                        string reportSubject = "[HRM] Happy Birthday - " + employee.FullName;

                                        ImgResourceList = new List<LinkedResource>();
                                        //Emdep_logo
                                        Bitmap emdepLogo = new Bitmap(ActivityReminder.Properties.Resources.logo, 200, 59);
                                        ImageConverter icEmdepLogo = new ImageConverter();
                                        Byte[] emdepLogoBytes = (Byte[])icEmdepLogo.ConvertTo(emdepLogo, typeof(Byte[]));
                                        MemoryStream imgEmdepLogo = new MemoryStream(emdepLogoBytes);

                                        LinkedResource LREmdepLogo = new LinkedResource(imgEmdepLogo);
                                        LREmdepLogo.ContentId = "EmbeddedContent_1";
                                        LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                                        ImgResourceList.Add(LREmdepLogo);

                                        //Employee Image
                                        MemoryStream msEmployee = null;
                                        if (employee.ProfileImageInBytes != null)
                                        {
                                            msEmployee = new MemoryStream(employee.ProfileImageInBytes);
                                        }
                                        else
                                        {
                                            Bitmap employeeBitmap = new Bitmap(ActivityReminder.Properties.Resources.No_Photo, 250, 250);
                                            ImageConverter icEmployee = new ImageConverter();
                                            Byte[] employeeByte = (Byte[])icEmployee.ConvertTo(employeeBitmap, typeof(Byte[]));
                                            msEmployee = new MemoryStream(employeeByte);
                                        }

                                        LinkedResource LREmployee = new LinkedResource(msEmployee, "image/png");
                                        LREmployee.ContentId = "EmbeddedContent_2";
                                        LREmployee.ContentLink = new Uri("cid:" + LREmployee.ContentId);
                                        ImgResourceList.Add(LREmployee);

                                        //Cake
                                        Bitmap cake = new Bitmap(ActivityReminder.Properties.Resources.cake, 128, 128);
                                        ImageConverter iccake = new ImageConverter();
                                        Byte[] cakeba = (Byte[])iccake.ConvertTo(cake, typeof(Byte[]));
                                        MemoryStream imgCAKE = new MemoryStream(cakeba);

                                        LinkedResource imageCake = new LinkedResource(imgCAKE);
                                        imageCake.ContentId = "EmbeddedContent_3";
                                        imageCake.ContentLink = new Uri("cid:" + imageCake.ContentId);
                                        ImgResourceList.Add(imageCake);

                                        try
                                        {
                                            MailControl.SendHtmlMail(reportSubject, PrepareBirthdayMailFromTemplate(employee).ToString(), employee.EmployeeContactEmail, employee.EmployeeContactCompanyEmailList.ToList(), "HRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, ImgResourceList);

                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[INFO] Birthday email sent to - {0} email - {1}", employee.FullName, employee.EmployeeContactEmail));
                                        }
                                        catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                        {
                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[ERROR] GetTodayBirthdayOfEmployees() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message));
                                        }
                                        catch (Exception ex)
                                        {
                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[ERROR] GetTodayBirthdayOfEmployees() in MailControl.SendHtmlMail - {0} - {1}", employee.FullName, ex.Message));
                                        }
                                    }
                                    else
                                    {
                                        clsLogFile.WriteErrorLog(string.Format("[INFO] Local date {0} and automatic report start date {1} is not same and local DateTime Hour {2} >= 00 && {3} <= 23", localDateTime.Date, automaticReport.StartDate.Date, localDateTime.Hour, localDateTime.Hour));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                employee.IsEnabled = 1;
                                clsLogFile.WriteErrorLog(string.Format("[ERROR] Failed to send Birthday email - {0} - {1}", employee.FullName, ex.Message));
                            }
                        }
                        if (EmployeesBirthday != null && !EmployeesBirthday.Any(x => x.IsEnabled == 0))
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            EmployeesBirthday = null;
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date after email sent to all employees"));
                        }
                        else if (EmployeesBirthday != null && EmployeesBirthday.Count == 0)
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            EmployeesBirthday = null;
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date if no birthday today"));
                        }
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        EmployeesBirthday = null;
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date {0} is earlier than today {1} date", automaticReport.StartDate.Date, DateTime.Now.Date));
                    }
                }
                clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetTodayBirthdayOfEmployees()"));
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetTodayBirthdayOfEmployees() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetTodayBirthdayOfEmployees() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetTodayBirthdayOfEmployees - Exception {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }
        }

        private StringBuilder PrepareBirthdayMailFromTemplate(Employee employee)
        {
            StringBuilder emailbody = new StringBuilder();

            try
            {
                if (employee != null)
                {
                    string text = HrmStartUp.ReadMailTemplate("HappyBirthdayMailFormat.html");

                    if (employee.ProfileImageInBytes != null)
                        UserProfileImage = ByteArrayToBitmapImage(employee.ProfileImageInBytes);

                    text = text.Replace("[Emplyee_Name]", employee.FullName);

                    string daysuffix = GetDaySuffix(employee.DateOfBirth.Value.Day);
                    string Birthday = string.Format("{0}{1} {2}", employee.DateOfBirth.Value.Day, daysuffix, employee.DateOfBirth.Value.ToString("MMMM", CultureEnglish));
                    text = text.Replace("[Emplyee_Birthday]", Birthday);

                    emailbody.Append(text);
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Method PrepareBirthdayMail - {0}", ex.Message));
                throw;
            }

            return emailbody;
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                BitmapImage image = new BitmapImage();

                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(String.Format("[ERROR] Get an error in ByteArrayToBitmapImage() Method - {0}", ex.Message));
            }

            return null;
        }

        /// <summary>
        /// [001][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        /// [001][cpatil][26-07-2020] [GEOS2-2414]Custom recipients for “Company Holidays” notification.
        /// </summary>
        public void GetUpcomingCompanyHolidaysEmployees()
        {
            try
            {
                //if (timer1 != null)
                //    timer1.Enabled = false;

                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetUpcomingCompanyHolidaysEmployees()."));

                currentDateTime = DateTime.Now;
                List<AutomaticReport> automaticReportLst = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportLst.FirstOrDefault(x => x.IdAutomaticReport == 4);

                int weekNum = CultureEnglish.Calendar.GetWeekOfYear(currentDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date == DateTime.Now.Date)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        List<string> emails = new List<string>();
                        //[001]Changed service method
                        List<CompanyHoliday> cmpnyHolidays = HrmStartUp.GetUpcomingCompanyHolidays_V2045(ref emails, DateTime.Now);

                        string holidaySubject = "[HRM] Company Holidays";

                        if (automaticReport.Interval.Equals("weekly"))
                            holidaySubject = string.Format("{0} CW {1}", holidaySubject, weekNum);

                        if (cmpnyHolidays != null && cmpnyHolidays.Count > 0)
                        {
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing mail for company holidays."));
                            StringBuilder temp = PrepareCompanyHolidayMail(cmpnyHolidays, weekNum);

                            if (emails.Count > 0)
                            {
                                try
                                {
                                    MailControl.SendHtmlMail(holidaySubject, temp.ToString(), string.Join(";", emails), new List<string>(), "HRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, new List<LinkedResource>());
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Company holidays mail sent to - {0}", string.Join(";", emails)));
                                }
                                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                {
                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingCompanyHolidaysEmployees() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message));
                                }
                            }
                        }
                        else
                        {
                            clsLogFile.WriteErrorLog(string.Format("[INFO] No company holidays this week."));
                        }

                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date."));
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is earlier than today"));
                    }
                }

                clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetUpcomingCompanyHolidaysEmployees()."));
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingCompanyHolidaysEmployees() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingCompanyHolidaysEmployees() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingCompanyHolidaysEmployees() - Exception - {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }
        }

        private StringBuilder PrepareCompanyHolidayMail(List<CompanyHoliday> companyHolidays, int weekNo)
        {
            companyHolidays = companyHolidays.OrderByDescending(s => s.StartDate).ToList();
            var groupedHolidaysByDate = companyHolidays.GroupBy(x => x.StartDate);

            StringBuilder emailbodyStr = new StringBuilder();
            //CultureInfo culture_Euro = new CultureInfo("es-Es");

            try
            {
                if (companyHolidays != null)
                {
                    emailbodyStr.Append(@"<style>
                                        .header1 {  font-size: 32px;
                                                    line-height: 1.2em;
                                                    color: #111111; 
                                                    font-weight: 200;
                                                    margin: 15px 0 10px;
                                                    padding: 0;
                                        }
                                        .header2 {  font-size: 24px;
                                                    line-height: 1em;
                                                    font-weight: normal;
                                                    margin: 15px 0px 0px 0px;
                                                    padding: 0;
                                        }
                                        .header3 {  font-size: 19px;
                                                    line-height: 1.0em;
                                                    font-weight: normal;
                                                    margin: 0 0 0px;
                                                    padding: 0;
                                        }
                                        .footer {  font-size: 15px; 
                                                   font-weight: normal; 
                                                   margin: 0 0 10px; 
                                                   padding: 0;
                                        }
                                        </style> ");

                    emailbodyStr.Append("<P>");
                    emailbodyStr.Append("<h1 class=\"header1\">");
                    emailbodyStr.Append("<b>" + "WEEK " + weekNo + "</b>");
                    emailbodyStr.Append("</h1>\n");
                    emailbodyStr.Append("</P>\n");

                    foreach (var group in groupedHolidaysByDate)
                    {
                        emailbodyStr.Append("<h2 class=\"header2\"> \n");
                        string daysuffix = GetDaySuffix(group.Key.Value.Day);
                        emailbodyStr.Append("<b>" + string.Format("{0}, {1}{2}", CultureEnglish.DateTimeFormat.GetDayName(group.Key.Value.DayOfWeek), group.Key.Value.Day, daysuffix) + "</b>\n");
                        emailbodyStr.Append("</h2>\n");

                        foreach (CompanyHoliday companyHoliday in group)
                        {
                            emailbodyStr.Append("<P>");
                            emailbodyStr.Append("<h3 class=\"header3\">");
                            emailbodyStr.Append("<span>" + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + "[" + companyHoliday.Company.Alias + "]" + " " + companyHoliday.Name + "</span>\n");
                            emailbodyStr.Append("</h3>\n</P>\n");
                        }
                    }

                    emailbodyStr.Append("<br>");
                    emailbodyStr.Append("<P class=\"footer\">");
                    emailbodyStr.Append("<b>" + "Best Regards," + "<b>\n");
                    emailbodyStr.Append("<br>\n");
                    emailbodyStr.Append("<b>" + "EMDEP" + "</b>\n");
                    emailbodyStr.Append("</P>\n");
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog("[ERROR] Method PrepareCompanyHolidayMail " + ex.Message);
            }

            return emailbodyStr;

        }

        private string GetDaySuffix(int day)
        {
            if (day > 0)
            {
                if (day % 10 == 1 && day % 100 != 11)
                    return "st";
                else if (day % 10 == 2 && day % 100 != 12)
                    return "nd";
                else if (day % 10 == 3 && day % 100 != 13)
                    return "rd";
                else
                    return "th";
            }
            else
                return string.Empty;
        }

        //[001][cpatil][17-06-2020][GEOS2-2367] Notification of Leaves is not ignoring the closed JD
        // [002][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        public void GetUpcomingEmployeeLeavesByCompany()
        {
            try
            {
                //if (timer1 != null)
                //    timer1.Enabled = false;

                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetUpcomingEmployeeLeavesByCompany()"));

                currentDateTime = WorkbenchStartUp.GetServerDateTime();
                List<AutomaticReport> automaticReportLst = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportLst.FirstOrDefault(x => x.IdAutomaticReport == 5);

                int weekNum = CultureEnglish.Calendar.GetWeekOfYear(currentDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date == DateTime.Now.Date)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        currentDateTime = DateTime.Now;  //.Date.AddHours(3).AddMinutes(28);

                        if (Companies == null)
                            Companies = WorkbenchStartUp.GetAllCompanyList();
                        else if (Companies != null && Companies.Count == 0)
                            Companies = WorkbenchStartUp.GetAllCompanyList();

                        foreach (Company company in Companies)
                        {
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing Department Leaves email - {0}", company.Alias));
                            string deptLeaveSubject = "[HRM] Department Leaves";

                            try
                            {
                                if (!company.IsExist)
                                {
                                    DateTime localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentDateTime, company.TimeZoneIdentifier);

                                    if (automaticReport.StartDate.Date == localDateTime.Date && localDateTime.Hour >= 00 && localDateTime.Hour <= 23)
                                    {
                                        List<string> emailTo = new List<string>();
                                        List<string> emailCC = new List<string>();
                                        // [001] changed service method
                                        // [002] changed service method
                                        List<EmployeeLeave> employeeLeaves = HrmStartUp.GetUpcomingEmployeeLeaves_V2045(company.IdCompany, ref emailTo, ref emailCC, DateTime.Now);

                                        if (employeeLeaves.Count > 0)
                                        {
                                            if (automaticReport.Interval.Equals("weekly"))
                                                deptLeaveSubject = string.Format("{0} CW {1}", deptLeaveSubject, weekNum);

                                            StringBuilder temp = PrepareDepartmentLeavesMail(employeeLeaves, weekNum);

                                            if (emailTo.Count > 0)
                                            {
                                                try
                                                {
                                                    MailControl.SendHtmlMail(deptLeaveSubject, temp.ToString(), string.Join(";", emailTo), emailCC, "HRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, new List<LinkedResource>());
                                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Department Leaves mail sent to - {0}", string.Join(";", emailTo)));
                                                }
                                                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                                {
                                                    company.IsExist = true;
                                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message));
                                                }
                                                catch (Exception ex)
                                                {
                                                    company.IsExist = true;
                                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany() in MailControl.SendHtmlMail. - {0} - {1}", company.Alias, ex.Message));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clsLogFile.WriteErrorLog(string.Format("[INFO] No employee leaves - {0}", company.Alias));
                                        }

                                        company.IsExist = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany() - {0} - {1}", company.Alias, ex.Message));
                            }
                        }

                        if (Companies != null && !Companies.Any(x => x.IsExist == false))
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            Companies = new List<Company>();
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date after email sent to all companies"));
                        }
                        else if (Companies == null || Companies.Count == 0)
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            Companies = new List<Company>();
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date if no company found"));
                        }
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        Companies = null;
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is earlier than today"));
                    }
                }

                clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetUpcomingEmployeeLeavesByCompany()"));
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetUpcomingEmployeeLeavesByCompany(). - Exception - {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }
        }

        private StringBuilder PrepareDepartmentLeavesMail(List<EmployeeLeave> employeeLeaves, int weekNo)
        {
            employeeLeaves = employeeLeaves.OrderBy(s => s.StartDate).ToList();
            var groupedLeavesByDate = employeeLeaves.GroupBy(x => x.StartDate.Value.Date);

            StringBuilder emailbodyStr = new StringBuilder();

            try
            {
                if (employeeLeaves != null)
                {
                    emailbodyStr.Append(@"<style>
                                        .header1 {  font-size: 32px;
                                                    line-height: 1.2em;
                                                    color: #111111; 
                                                    font-weight: 200;
                                                    margin: 15px 0 10px;
                                                    padding: 0;
                                        }
                                        .header2 {  font-size: 24px;
                                                    line-height: 1em;
                                                    font-weight: normal;
                                                    margin: 15px 0px 0px 0px;
                                                    padding: 0;
                                        }
                                        .header3 {  font-size: 19px;
                                                    line-height: 1.0em;
                                                    font-weight: normal;
                                                    margin: 0 0 0px;
                                                    padding: 0;
                                        }
                                        .footer {  font-size: 15px; 
                                                   font-weight: normal; 
                                                   margin: 0 0 10px; 
                                                   padding: 0;
                                        }
                                        </style> ");

                    emailbodyStr.Append("<P>");
                    emailbodyStr.Append("<h1 class=\"header1\">");
                    emailbodyStr.Append("<b>" + "WEEK " + weekNo + "</b>");
                    emailbodyStr.Append("</h1>\n");
                    emailbodyStr.Append("</P>\n");

                    foreach (var group in groupedLeavesByDate)
                    {
                        emailbodyStr.Append("<h2 class=\"header2\"> \n");
                        string daysuffix = GetDaySuffix(group.Key.Day);
                        emailbodyStr.Append("<b>" + string.Format("{0}, {1}{2}", CultureEnglish.DateTimeFormat.GetDayName(group.Key.DayOfWeek), group.Key.Day, daysuffix) + "</b>\n");
                        emailbodyStr.Append("</h2>\n");

                        //var culture = new System.Globalization.CultureInfo("en-GB");
                        var day = CultureEnglish.DateTimeFormat.GetDayName(group.Key.DayOfWeek);

                        foreach (EmployeeLeave employeeLeave in group)
                        {
                            emailbodyStr.Append("<P>");
                            emailbodyStr.Append("<h3 class=\"header3\">");
                            emailbodyStr.Append("<span>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" + string.Format("[{0}-{1}] {2}_{3}", employeeLeave.StartDate.Value.TimeOfDay.ToString(@"hh\:mm"), employeeLeave.EndDate.Value.TimeOfDay.ToString(@"hh\:mm"), employeeLeave.Employee.FullName, employeeLeave.CompanyLeave.Name) + "</span>\n");
                            emailbodyStr.Append("</h3>\n</P>\n");
                        }
                    }

                    emailbodyStr.Append("<br>");
                    emailbodyStr.Append("<P class=\"footer\">");
                    emailbodyStr.Append("<b>" + "Best Regards," + "<b>\n");
                    emailbodyStr.Append("<br>\n");
                    emailbodyStr.Append("<b>" + "EMDEP" + "</b>\n");
                    emailbodyStr.Append("</P>\n");
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Method PrepareDepartmentLeavesMail - {0}", ex.Message));
            }

            return emailbodyStr;
        }

        /// <summary>
        /// [001][01-10-2018][skhade][HRM M048-12] Automatic Report for the Company Anniversaries.
        /// Added complete method.
        /// [002][skale][18-07-2019][GEOS2-1600]Some emails are not sent
        ///  [003][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        /// </summary>
        public void GetCompanyAnniversariesOfEmployees()
        {
            try
            {
                //if (timer1 != null)
                //    timer1.Enabled = false;

                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetCompanyAnniversariesOfEmployees()"));

                currentDateTime = DateTime.Now;
                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 6);

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date == DateTime.Now.Date)
                {

                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        if (EmployeeCompanyAnniversaries == null)
                        {
                            //[003]Changed service method
                            // EmployeeCompanyAnniversaries = HrmStartUp.GetTodayEmployeeCompanyAnniversaries(0, DateTime.Now);
                            EmployeeCompanyAnniversaries = HrmStartUp.GetTodayEmployeeCompanyAnniversaries_V2045(0, DateTime.Now);

                            if (EmployeeCompanyAnniversaries.Count > 0)//[002] added
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Company anniversaries email to send - {0}", String.Join(",", EmployeeCompanyAnniversaries.ToList().Select(x => x.FirstName + " " + x.LastName))));
                        }

                        else if (EmployeeCompanyAnniversaries != null && EmployeeCompanyAnniversaries.Count == 0)
                        {
                            //EmployeeCompanyAnniversaries = HrmStartUp.GetTodayEmployeeCompanyAnniversaries(0, DateTime.Now);

                            EmployeeCompanyAnniversaries = HrmStartUp.GetTodayEmployeeCompanyAnniversaries_V2045(0, DateTime.Now);

                            if (EmployeeCompanyAnniversaries.Count > 0)//[002] added
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Company anniversaries email to send - {0}", String.Join(",", EmployeeCompanyAnniversaries.ToList().Select(x => x.FirstName + " " + x.LastName))));
                        }

                        foreach (Employee employee in EmployeeCompanyAnniversaries)
                        {
                            try
                            {
                                if (employee.IsEnabled == 0)    //isenable updated when email send to avoid repetation
                                {
                                    string timeZoneId = employee.EmployeeJobDescription.Company.TimeZoneIdentifier;
                                    DateTime localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentDateTime, timeZoneId);
                                    //[002] added
                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Employee name is {0} and company TimeZoneIdentifier is {1} and current date time {2} and local date time is {3}", employee.FullName, timeZoneId, currentDateTime, localDateTime));

                                    if (automaticReport.StartDate.Date == localDateTime.Date && localDateTime.Hour >= 00 && localDateTime.Hour <= 23)
                                    {
                                        clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing Anniversary email - {0}", employee.FullName));
                                        string reportSubject = "[HRM] Company Anniversary - " + employee.FullName;

                                        ImgResourceList = new List<LinkedResource>();

                                        //Emdep_logo
                                        Bitmap emdepLogo = new Bitmap(ActivityReminder.Properties.Resources.logo, 200, 59);
                                        ImageConverter icEmdepLogo = new ImageConverter();
                                        Byte[] emdepLogoBytes = (Byte[])icEmdepLogo.ConvertTo(emdepLogo, typeof(Byte[]));
                                        MemoryStream imgEmdepLogo = new MemoryStream(emdepLogoBytes);

                                        LinkedResource LREmdepLogo = new LinkedResource(imgEmdepLogo);
                                        LREmdepLogo.ContentId = "EmbeddedContent_1";
                                        LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                                        ImgResourceList.Add(LREmdepLogo);

                                        //Employee Image
                                        MemoryStream msEmployee = null;
                                        if (employee.ProfileImageInBytes != null)
                                        {
                                            msEmployee = new MemoryStream(employee.ProfileImageInBytes);
                                        }
                                        else
                                        {
                                            Bitmap employeeBitmap = new Bitmap(ActivityReminder.Properties.Resources.No_Photo, 250, 250);
                                            ImageConverter icEmployee = new ImageConverter();
                                            Byte[] employeeByte = (Byte[])icEmployee.ConvertTo(employeeBitmap, typeof(Byte[]));
                                            msEmployee = new MemoryStream(employeeByte);
                                        }

                                        LinkedResource LREmployee = new LinkedResource(msEmployee, "image/png");
                                        LREmployee.ContentId = "EmbeddedContent_2";
                                        LREmployee.ContentLink = new Uri("cid:" + LREmployee.ContentId);
                                        ImgResourceList.Add(LREmployee);

                                        //Anniversary image.
                                        Bitmap anniversary = new Bitmap(ActivityReminder.Properties.Resources.Anniversary, 140, 100);
                                        ImageConverter icAnniversary = new ImageConverter();
                                        Byte[] cakeba = (Byte[])icAnniversary.ConvertTo(anniversary, typeof(Byte[]));
                                        MemoryStream imgAnniversary = new MemoryStream(cakeba);

                                        LinkedResource imageAnniversary = new LinkedResource(imgAnniversary);
                                        imageAnniversary.ContentId = "EmbeddedContent_3";
                                        imageAnniversary.ContentLink = new Uri("cid:" + imageAnniversary.ContentId);
                                        ImgResourceList.Add(imageAnniversary);

                                        try
                                        {
                                            MailControl.SendHtmlMail(reportSubject, PrepareCompanyAnniversaryMailFromTemplate(employee).ToString(), employee.EmployeeContactEmail, new List<string>(), "HRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, ImgResourceList);
                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[INFO] Company anniversaries email to send - {0} email - {1}", employee.FullName, employee.EmployeeContactEmail));
                                        }
                                        catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                        {
                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[ERROR] GetCompanyAnniversariesOfEmployees() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message));
                                        }
                                        catch (ObjectDisposedException e)
                                        {
                                            clsLogFile.WriteErrorLog(string.Format("For SendHtmlMail ObjectDisposedException : {0}", e.Message));
                                        }
                                        catch (Exception ex)
                                        {
                                            employee.IsEnabled = 1;
                                            clsLogFile.WriteErrorLog(string.Format("[ERROR] GetCompanyAnniversariesOfEmployees() in MailControl.SendHtmlMail - {0} - {1}", employee.FullName, ex.Message));
                                        }
                                    }
                                    else
                                    {
                                        clsLogFile.WriteErrorLog(string.Format("[INFO] Local date {0} and automatic report start date {1} is not same and local DateTime Hour {2} >= 00 && {3} <= 23", localDateTime.Date, automaticReport.StartDate.Date, localDateTime.Hour, localDateTime.Hour));
                                    }
                                }
                            }
                            catch (ObjectDisposedException e)
                            {
                                clsLogFile.WriteErrorLog(string.Format("For Inner ObjectDisposedException : {0}", e.Message));
                            }
                            catch (Exception ex)
                            {
                                clsLogFile.WriteErrorLog(string.Format("[ERROR] Failed to send Anniversary email - {0} - {1}", employee.FullName, ex.Message));
                            }
                        }

                        if (EmployeeCompanyAnniversaries != null && EmployeeCompanyAnniversaries.Count == 0)
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            EmployeeCompanyAnniversaries = null;
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date if no anniversary today"));
                        }
                        else if (EmployeeCompanyAnniversaries != null && !EmployeeCompanyAnniversaries.Any(x => x.IsEnabled == 0))
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            EmployeeCompanyAnniversaries = null;
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date after email sent to all employees"));
                        }
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        EmployeeCompanyAnniversaries = null;
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date {0} is earlier than today {1} date", automaticReport.StartDate.Date, DateTime.Now.Date));
                    }
                }

                clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetCompanyAnniversariesOfEmployees()"));
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetCompanyAnniversariesOfEmployees() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetCompanyAnniversariesOfEmployees() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetCompanyAnniversariesOfEmployees() - Exception {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }
        }

        /// <summary>
        /// [001][01-10-2018][skhade][HRM M048-12] Automatic Report for the Company Anniversaries.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>Template into String</returns>
        private StringBuilder PrepareCompanyAnniversaryMailFromTemplate(Employee employee)
        {
            StringBuilder emailbody = new StringBuilder();

            try
            {
                if (employee != null)
                {
                    string text = HrmStartUp.ReadMailTemplate("CompanyAnniversaryMailFormat.html");     //File.ReadAllText(@"C:\Temp\Templates\CompanyAnniversaryMailFormat.html");

                    if (employee.ProfileImageInBytes != null)
                        UserProfileImage = ByteArrayToBitmapImage(employee.ProfileImageInBytes);

                    if (employee.LengthOfService > 1)
                        text = text.Replace("[YEARS]", string.Format("{0} {1}", employee.LengthOfService, "years"));
                    else
                        text = text.Replace("[YEARS]", string.Format("{0} {1}", employee.LengthOfService, "year"));

                    text = text.Replace("[EMPLOYEE_NAME]", employee.FullName);

                    string daysuffix = GetDaySuffix(employee.EmployeeContractSituation.ContractSituationStartDate.Value.Day);
                    string anniversaryDate = string.Format("{0}{1} {2}", employee.EmployeeContractSituation.ContractSituationStartDate.Value.Day, daysuffix, employee.EmployeeContractSituation.ContractSituationStartDate.Value.ToString("MMMM", CultureEnglish));
                    text = text.Replace("[ANNIVERSARY_DATE]", anniversaryDate);

                    emailbody.Append(text);
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] PrepareCompanyAnniversaryMailFromTemplate() - {0}", ex.Message));
                throw;
            }

            return emailbody;
        }

        /// <summary>
        /// [000][skale][04-10-2019][GEOS2-1778] Add a new automatic task to set the employees as Inactive.
        /// [001][skhade][26-04-2020][GEOS2-2226] Activity reminder fails for employeeleave task.
        /// </summary>
        public void SetEmployeeInactive()
        {
            try
            {
                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - SetEmployeeInactive()"));

                currentDateTime = DateTime.Now;
                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 7);

                if (automaticReport != null && automaticReport.IsEnabled == 1)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        List<Employee> ExitEmployeeList = HrmStartUp.GetExitEmployeeToUpdateStatusInActive(DateTime.Now.Date);

                        clsLogFile.WriteErrorLog(string.Format("[INFO] Total employee count - {0}", ExitEmployeeList.Count));

                        if (ExitEmployeeList.Count > 0)
                        {
                            HrmStartUp.UpdateExitEmployeeStatusInActive(DateTime.Now.Date);
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        }
                        else
                        {
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updating automatic report date to today for employee leaves."));
                            automaticReport.StartDate = DateTime.Today;
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is less than today."));
                        }

                        clsLogFile.WriteErrorLog(string.Format("[INFO] Method executed successfully - SetEmployeeInactive()"));
                    }
                    //001
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == -1)
                    {
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updating automatic report date to today for employee leaves."));
                        automaticReport.StartDate = DateTime.Today.AddDays(-1);
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is less than today."));
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] SetEmployeeInactive() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] SetEmployeeInactive() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] SetEmployeeInactive - Exception {0}", ex.Message));
            }
        }

        #region Has welcome message been received

        /// <summary>
        /// 001 - Added method to Welcome on board employees
         // [002][cpatil][26-07-2020] [GEOS2-2452]The email notifications from HRM are sent also to the users with Inactive status.
        /// </summary>
        private void GetEmployeesWelcomeOnBoard()
        {
            clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetEmployeesWelcomeOnBoard()"));

            try
            {
                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                AutomaticReport automaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 8);

                if (automaticReport != null && automaticReport.IsEnabled == 1)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        //[002]Changed service method
                        Tuple<List<Employee>, Employee> welcomeOnBoardEmployees = HrmStartUp.GetEmployeeForWelcomeBoard_V2045();

                        if (welcomeOnBoardEmployees != null && welcomeOnBoardEmployees.Item1 != null && welcomeOnBoardEmployees.Item1.Count > 0)
                        {
                            ImgResourceList = new List<LinkedResource>();

                            // create image resource from image path using LinkedResource class. c:\\attachment\\image1.jpg
                            //Emdep logo
                            //Development path
                            //LinkedResource imageResource = new LinkedResource(string.Format("{0}\\..\\..\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "logo.png"),
                            //                                                    "image/png");

                            LinkedResource imageResource = new LinkedResource(string.Format("{0}\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "logo.png"),
                                                    "image/png");
                            imageResource.ContentId = "EmbeddedContent_1";
                            imageResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                            ImgResourceList.Add(imageResource);

                            //Shakehand
                            //Development path
                            //LinkedResource imageShakeHand = new LinkedResource(string.Format("{0}\\..\\..\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "ShakeHand.png"),
                            //                                                    "image/png");

                            LinkedResource imageShakeHand = new LinkedResource(string.Format("{0}\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "ShakeHand.png"),
                                                    "image/png");
                            imageShakeHand.ContentId = "EmbeddedContent_2";
                            imageShakeHand.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                            ImgResourceList.Add(imageShakeHand);

                            //informer
                            if (welcomeOnBoardEmployees.Item2 != null)
                            {
                                if (welcomeOnBoardEmployees.Item2.ProfileImageInBytes != null)
                                {
                                    MemoryStream msEmployee = null;
                                    msEmployee = new MemoryStream(welcomeOnBoardEmployees.Item2.ProfileImageInBytes);
                                    LinkedResource LREmployee = new LinkedResource(msEmployee, "image/png");
                                    LREmployee.ContentId = "EmbeddedContent_3";
                                    LREmployee.ContentLink = new Uri("cid:" + LREmployee.ContentId);
                                    ImgResourceList.Add(LREmployee);
                                }
                                else if (welcomeOnBoardEmployees.Item2.IdGender == 1)
                                {
                                    //LinkedResource imageInformerJD = new LinkedResource(string.Format("{0}\\..\\..\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "FemaleUser_White.png"),
                                    //                                                    "image/png");

                                    LinkedResource imageInformerJD = new LinkedResource(string.Format("{0}\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "FemaleUser_White.png"),
                                                    "image/png");
                                    imageInformerJD.ContentId = "EmbeddedContent_3";
                                    imageInformerJD.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                                    ImgResourceList.Add(imageInformerJD);
                                }
                                else if (welcomeOnBoardEmployees.Item2.IdGender == 2)
                                {
                                    //LinkedResource imageInformerJD = new LinkedResource(string.Format("{0}\\..\\..\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "MaleUser_White.png"),
                                    //                                                    "image/png");

                                    LinkedResource imageInformerJD = new LinkedResource(string.Format("{0}\\..\\Resources\\{1}", System.Reflection.Assembly.GetExecutingAssembly().Location, "MaleUser_White.png"),
                                                    "image/png");
                                    imageInformerJD.ContentId = "EmbeddedContent_3";
                                    imageInformerJD.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                                    ImgResourceList.Add(imageInformerJD);
                                }
                            }

                            foreach (Employee employee in welcomeOnBoardEmployees.Item1)
                            {
                                clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing WelcomeOnBoard email - {0}", employee.FullName));

                                string reportSubject = string.Format("[HRM] Welcome on board {0}", employee.FullName);

                                try
                                {
                                    MailControl.SendHtmlMail(reportSubject,
                                                            PrepareEmployeesWelcomeOnBoardTemplate(employee, welcomeOnBoardEmployees.Item2).ToString(),
                                                            employee.EmployeeContactEmail,
                                                            //"skhade@emdep.com",
                                                            new List<string>(),
                                                            "HRM-noreply@emdep.com",
                                                            ActivityReminder.Properties.Settings.Default.MailServerName,
                                                            ActivityReminder.Properties.Settings.Default.MailServerPort,
                                                            ImgResourceList);

                                    HrmStartUp.UpdateEmployeeHasWelcomeMessageBeenReceived(employee.IdEmployee);

                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Welcome email sent to - {0} email - {1} and flag is updated", employee.FullName, employee.EmployeeContactEmail));
                                }
                                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                {
                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeesWelcomeOnBoard() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message));
                                }
                                catch (Exception ex)
                                {
                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeesWelcomeOnBoard() in MailControl.SendHtmlMail - {0} - {1}", employee.FullName, ex.Message));
                                }
                            }

                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date after welcome email sent to all employees"));
                        }
                        else
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            clsLogFile.WriteErrorLog(string.Format("[INFO] EmployeesWelcomeOnBoard - Updated automatic report date {0} as no employees found to send mail.", automaticReport.StartDate.Date));
                        }
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == -1)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        clsLogFile.WriteErrorLog(string.Format("[INFO] EmployeesWelcomeOnBoard - Updated automatic report date {0} is earlier than today {1} date", automaticReport.StartDate.Date, DateTime.Now.Date));
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeesWelcomeOnBoard() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeesWelcomeOnBoard() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeesWelcomeOnBoard() - Exception - {0} ", ex.Message));
            }
            finally
            {
                ImgResourceList = null;
            }

            clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetEmployeesWelcomeOnBoard()"));
        }

        /// <summary>
        /// 001 - Added method to Welcome on board employees
        /// </summary>
        /// <param name="employee">The employee with details.</param>
        /// <returns>String with updated values.</returns>
        private StringBuilder PrepareEmployeesWelcomeOnBoardTemplate(Employee employee, Employee informer)
        {
            StringBuilder emailbody = new StringBuilder();

            try
            {
                if (employee != null)
                {
                    string text = HrmStartUp.ReadMailTemplate("WelcomeOnBoardMailFormat.html");

                    text = text.Replace("[Employee_Name]", employee.FullName);
                    text = text.Replace("[Employee_Welcome_Date]", string.Format("{0}{1} {2}", employee.ContractSituation.HireDate.Value.Day, GetDaySuffix(employee.ContractSituation.HireDate.Value.Day), employee.ContractSituation.HireDate.Value.ToString("MMMM yyyy", CultureEnglish)));
                    text = text.Replace("[Employee_Departments]", employee.EmployeeDepartments.Replace("\n", " and "));

                    if (informer != null)
                    {
                        text = text.Replace("[Welcome_Message_Informer_Name]", informer.FullName);

                        if (informer.EmployeeJobDescription != null && informer.EmployeeJobDescription.JobDescription != null)
                            text = text.Replace("[Welcome_Message_Informer_JD]", informer.EmployeeJobDescription.JobDescription.JobDescriptionTitle);
                    }

                    emailbody.Append(text);
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] Method PrepareEmployeesWelcomeOnBoardTemplate - {0}", ex.Message));
                throw;
            }

            return emailbody;
        }

        #endregion

        #endregion // Methods

        #region Events
        /// <summary>
        /// 
        /// <para>[001][Ganaraj Chavan][27-05-2020][GEOS2-2361] GeosWokbenchActivityReminderService service stopped then never start again.</para>
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            clsLogFile.WriteErrorLog("[INFO] OnStart() Service Staring");
            //[001]
            InitiaizeOnStartService();
            clsLogFile.WriteErrorLog("[INFO] OnStart() Service Started");
        }

        protected override void OnStop()
        {
            //if (listActivity != null && listActivity.Count > 0)
            //{
            //}

            clsLogFile.WriteErrorLog("[INFO] OnStop() Service Stopping");

            if (timer1 != null)
                timer1.Enabled = false;

            clsLogFile.WriteErrorLog("[INFO] OnStop() Service Stoped");
        }

        protected override void OnShutdown()
        {
            //if (listActivity != null && listActivity.Count > 0)
            //{
            //}

            clsLogFile.WriteErrorLog("[INFO] OnShutdown() Service Shutting down");

            if (timer1 != null)
                timer1.Enabled = false;

            clsLogFile.WriteErrorLog("[INFO] OnShutdown() Service Shutdown");
        }

        #endregion


        /// <summary>
        /// [002][adhatkar][29-07-2020][GEOS2-2141] Emd of contract reminder
        /// </summary>
        public void GetEmployeeContractExpirations()
        {
            try
            {
                //if (timer1 != null)
                //    timer1.Enabled = false;
                clsLogFile.WriteErrorLog(string.Format("[INFO] Started execution - GetEmployeeContractExpirations()"));

                currentDateTime = DateTime.Now;
                List<AutomaticReport> automaticReportList = WorkbenchStartUp.GetAutomaticReports();
                GeosAppSetting appsetting = WorkbenchStartUp.GetGeosAppSettings(39);
                AutomaticReport automaticReport = automaticReportList.FirstOrDefault(x => x.IdAutomaticReport == 9);

                if (automaticReport != null && automaticReport.IsEnabled == 1) // && automaticReport.StartDate.Date == DateTime.Now.Date)
                {
                    if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) == 0)
                    {
                        if (Companies == null)
                            Companies = WorkbenchStartUp.GetAllCompanyList();
                        else if (Companies != null && Companies.Count == 0)
                            Companies = WorkbenchStartUp.GetAllCompanyList();

                        foreach (Company company in Companies)
                        {
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing Contract Expiration Reminder email - {0}", company.Alias));
                            string ContractSubject = "[HRM] Contract Expiration Reminder";

                            try
                            {
                                if (!company.IsExist)
                                {
                                    DateTime localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentDateTime, company.TimeZoneIdentifier);

                                    if (automaticReport.StartDate.Date == localDateTime.Date && localDateTime.Hour >= 00 && localDateTime.Hour <= 23)
                                    {
                                        List<string> emails = new List<string>();
                                        List<EmployeeContractSituation> employeeContracts = HrmStartUp.GetEmployeeContractExpirations(company.IdCompany, ref emails, DateTime.Now);



                                        if (employeeContracts != null && employeeContracts.Count > 0)
                                        {
                                            clsLogFile.WriteErrorLog(string.Format("[INFO] Preparing mail for Contract Expiration Reminder."));

                                            List<LinkedResource> ImgResourceList = new List<LinkedResource>();
                                            //Emdep_logo
                                            Bitmap emdepLogo = new Bitmap(ActivityReminder.Properties.Resources.GEOS, 64, 64);
                                            ImageConverter icEmdepLogo = new ImageConverter();
                                            Byte[] emdepLogoBytes = (Byte[])icEmdepLogo.ConvertTo(emdepLogo, typeof(Byte[]));
                                            MemoryStream imgEmdepLogo = new MemoryStream(emdepLogoBytes);
                                            LinkedResource LREmdepLogo = new LinkedResource(imgEmdepLogo);
                                            LREmdepLogo.ContentId = "EmbeddedContent_1";
                                            LREmdepLogo.ContentLink = new Uri("cid:" + LREmdepLogo.ContentId);
                                            ImgResourceList.Add(LREmdepLogo);

                                            StringBuilder temp = PrepareEmployeeContractMail(employeeContracts, appsetting);

                                            if (emails.Count > 0)
                                            {
                                                try
                                                {
                                                    MailControl.SendHtmlMail(ContractSubject, temp.ToString(), string.Join(";", emails), new List<string>(), "HRM-noreply@emdep.com", ActivityReminder.Properties.Settings.Default.MailServerName, ActivityReminder.Properties.Settings.Default.MailServerPort, ImgResourceList);
                                                    //update warning date
                                                    HrmStartUp.UpdateEmployeeContractWarningDate(employeeContracts, DateTime.Now);
                                                    clsLogFile.WriteErrorLog(string.Format("[INFO] Contract Expiration Reminder mail sent to - {0}", string.Join(";", emails)));
                                                }
                                                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                                {
                                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", company.Alias, ex.Message));
                                                    company.IsExist = true;
                                                }
                                                catch (Exception ex)
                                                {
                                                    company.IsExist = true;
                                                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations() in MailControl.SendHtmlMail - {0} - {1}", company.Alias, ex.Message));
                                                }

                                            }
                                        }
                                        else
                                        {
                                            clsLogFile.WriteErrorLog(string.Format("[INFO] No employee Contract Expiration Reminders.- {0}", company.Alias));
                                        }

                                        company.IsExist = true;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations() - {0} - {1}", company.Alias, ex.Message));
                            }
                        }
                        if (Companies != null && !Companies.Any(x => x.IsExist == false))
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            Companies = new List<Company>();
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date after email sent to all companies"));
                        }
                        else if (Companies == null || Companies.Count == 0)
                        {
                            WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                            Companies = new List<Company>();
                            clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date if no company found"));
                        }
                    }
                    else if (DateTime.Compare(automaticReport.StartDate.Date, DateTime.Now.Date) < 0)
                    {
                        WorkbenchStartUp.UpdateAutomaticReport(automaticReport);
                        Companies = null;
                        clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date as date is earlier than today"));
                    }

                    clsLogFile.WriteErrorLog(string.Format("[INFO] Updated automatic report date."));
                }
                   
                
                clsLogFile.WriteErrorLog(string.Format("[INFO] Completed execution - GetEmployeeContractExpirations()"));
            }
            catch (FaultException<ServiceException> ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations() - FaultException - {0}", ex.Detail.ErrorMessage));
            }
            catch (ServiceUnexceptedException ex)
            {
                if (ex.ExceptionType != ServiceExceptionType.ObjectDisposedException)
                    clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations() - ServiceUnexceptedException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] GetEmployeeContractExpirations - Exception {0}", ex.Message));
            }
            finally
            {
                //if (timer1 != null)
                //    timer1.Enabled = true;
            }
        }



        private StringBuilder PrepareEmployeeContractMail(List<EmployeeContractSituation> EmployeeContracts,GeosAppSetting geosAppSetting)
        {
            StringBuilder emailbody = new StringBuilder();
            StringBuilder emailbodyStr = new StringBuilder();
            string text = HrmStartUp.ReadMailTemplate("EmployeeContractExpiryMailFormat.html");
           
            //CultureInfo culture_Euro = new CultureInfo("es-Es");
            string dayStr = string.Empty;
            if(Convert.ToInt64(geosAppSetting.DefaultValue)>1)
            { 
                dayStr = geosAppSetting.DefaultValue + " Days";

            }
            else
            {
                dayStr = geosAppSetting.DefaultValue +  " Day";
            }
            text = text.Replace("[Days]", dayStr);
            try
            {
                if (EmployeeContracts != null)
                {
                    //emailbodyStr.Append(@"<style>
                    //                    .header1 {  font-size: 32px;
                    //                                line-height: 1.2em;
                    //                                color: #111111; 
                    //                                font-weight: 200;
                    //                                margin: 15px 0 10px;
                    //                                padding: 0;
                    //                    }
                    //                    .header2 {  font-size: 24px;
                    //                                line-height: 1em;
                    //                                font-weight: normal;
                    //                                margin: 15px 0px 0px 0px;
                    //                                padding: 0;
                    //                    }
                    //                    .header3 {  font-size: 19px;
                    //                                line-height: 1.0em;
                    //                                font-weight: normal;
                    //                                margin: 0 0 0px;
                    //                                padding: 0;
                    //                    }
                    //                    .footer {  font-size: 15px; 
                    //                               font-weight: normal; 
                    //                               margin: 0 0 10px; 
                    //                               padding: 0;
                    //                    }
                    //                    </style> ");

                    //emailbodyStr.Append("<P>");
                    //emailbodyStr.Append("<h1 class=\"header3\">");
                    //emailbodyStr.Append("Please be informed that the contract of the following employees will expire in "+ geosAppSetting.DefaultValue + " "+ dayStr + ":");
                    //emailbodyStr.Append("</h1>\n");
                    //emailbodyStr.Append("</P>\n");
                    ////table
                    //emailbodyStr.Append("<table style=\"border-collapse:collapse; \" width=100%; border=1>");
                    //emailbodyStr.Append("<tr style=\" background-color:#D3D3D3;\">");
                    //emailbodyStr.Append("<th style=\" border: 1px solid #5c87b2; text-align:center; padding: 2px; \"><b> Code </th>\n");
                    //emailbodyStr.Append("<th style=\" border: 1px solid #5c87b2; text-align:center; padding: 2px; \"><b> Full Name </th>\n");
                    //emailbodyStr.Append("<th style=\" border: 1px solid #5c87b2; text-align:center; padding: 2px; \"><b> Contract Expiry Date </th>\n");
                    //emailbodyStr.Append("</tr>");

                    foreach (EmployeeContractSituation Contract in EmployeeContracts)
                    {
                        emailbodyStr.Append("<tr>");
                        emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; text-align:center; border-width:thin; padding: 2px;\" width=20%>\n");
                        emailbodyStr.Append("" + Contract.Employee.EmployeeCode + "</td>\n");
                        emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; text-align:center; border-width:thin; padding: 2px;\" width=20%>\n");
                        emailbodyStr.Append("" + Contract.Employee.FullName + "</td>\n");
                        emailbodyStr.Append("<td style=\" border-color:#5c87b2; border-style:solid; text-align:center; border-width:thin; padding: 2px;\" width=20%>\n");
                        emailbodyStr.Append("" + Contract.ContractSituationEndDate.Value.Date.ToString("dd/MM/yyyy") + "</td>\n");
                        emailbodyStr.Append("</tr>");
                    }

                    //  emailbodyStr.Append("</table>");
                    //  emailbodyStr.Append("<br>");

                    //  emailbodyStr.Append("<P>");
                    //  emailbodyStr.Append("<h1 class=\"header3\">");
                    //  emailbodyStr.Append("This is an automatically generated email. Please DO NOT reply to this message. This is only for your information.");
                    //  emailbodyStr.Append("</h1>\n");
                    //  emailbodyStr.Append("</P>\n");

                    //  emailbodyStr.Append("<br>");
                    //  emailbodyStr.Append("<P class=\"footer\">");
                    //  emailbodyStr.Append("<b>" + "Best Regards," + "<b>\n");
                    //  emailbodyStr.Append("<br>\n");
                    //  emailbodyStr.Append("<b>" + "EMDEP" + "</b>\n");
                    ////  emailbodyStr.Append("<b>"+ "<div style=\"text - align:right; height: 70px; width: 500px; \">< img src = \"cid:EmbeddedContent_1\" style = \"margin-left : 100px; float:right;\" /></ div> " + " </b>\n");
                    //  emailbodyStr.Append("</P>\n");
                    text = text.Replace("[Dynamic Data]", emailbodyStr.ToString());
                }
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog("[ERROR] Method PrepareEmployeeContractMail " + ex.Message);
            }

            return emailbody.Append(text);

        }
    }

    }

