using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using Emdep.Geos.UI.Common;
//using Workbench;
using Emdep.Geos.Utility;
using System.IO;
using Emdep.Geos.UI.Adapters.Logging;
using Prism.Logging;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.CustomControls;
using System.Windows;

namespace GEOS_Workbench_Outlook_Addin
{
    public partial class ThisAddIn
    {
        ICrmService CrmStartUp;
        IWorkbenchStartUp WStartUp;

        public List<GeosServiceProvider> GeosServiceProviderList { get; set; }

        //protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        //{
        //    return new Ribbon();
        //}

        private void CreateServiceInstances()
        {
            CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            WStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                Outlook.Application application = this.Application;
                Outlook.Inspectors inspectors = application.Inspectors;

                // if (!File.Exists(GeosApplication.Instance.OutlookAddInLogFilePath))
                //{
                //    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                //        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                //    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                //}
                FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
                GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);

                GetApplicationSettings();
                CreateServiceInstances();
                GeosApplication.Instance.ServerDateTime = WStartUp.GetServerDateTime();
                SetDate();

                Outlook.Accounts accounts = application.Session.Accounts;
                foreach (Outlook.Account account in accounts)
                {
                    //Outlook.ExchangeUser exchangeUser = account.CurrentUser.AddressEntry.GetExchangeUser();
                    //Console.WriteLine(account.DisplayName);
                    //Console.WriteLine(account.UserName);
                    //Console.WriteLine(exchangeUser.Name);
                    //if (exchangeUser != null)
                    //    string fullname = exchangeUser.Name;

                    GeosApplication.Instance.ActiveUser = WStartUp.GetUserDetailsByEmailId(account.DisplayName);
                    //GeosApplication.Instance.ActiveUser = WStartUp.GetUserDetailsByEmailId("fpinas@emdep.com");

                    if (GeosApplication.Instance.ActiveUser == null)
                    {
                        Outlook.ExchangeUser exchangeUser = account.CurrentUser.AddressEntry.GetExchangeUser();
                        //Console.WriteLine(exchangeUser.Name);
                        CustomMessageBox.Show(string.Format("The Outlook account {0} does not have GEOS account.", exchangeUser.Name), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
                    {
                        int value;
                        if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                            GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());
                        else
                        {
                            GeosApplication.Instance.IdUserPermission = GeosApplication.Instance.ActiveUser.UserPermissions.OrderBy(ord => ord.IdPermission).Select(slt => slt.IdPermission).FirstOrDefault();
                        }
                    }
                    else
                    {
                        GeosApplication.Instance.IdUserPermission = GeosApplication.Instance.ActiveUser.UserPermissions.OrderBy(ord => ord.IdPermission).Select(slt => slt.IdPermission).FirstOrDefault();
                    }

                    FillCommonDetails();

                    // Start (SalesOwner) - Selected Sales Owners User list for CRM. 
                    if (GeosApplication.Instance.IdUserPermission == 21)
                    {
                        GeosApplication.Instance.SalesOwnerUsersList = WStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);
                        GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();
                        UserManagerDtl usrDefault = GeosApplication.Instance.SalesOwnerUsersList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                        if (usrDefault != null)
                        {
                            GeosApplication.Instance.SelectedSalesOwnerUsersList.Add(usrDefault);
                        }
                        else
                        {
                            GeosApplication.Instance.SelectedSalesOwnerUsersList.AddRange(GeosApplication.Instance.SalesOwnerUsersList);
                        }
                    }
                    // End (SalesOwner)

                    // Start (PlantOwner) - Selected Plant Owners User list for CRM. 
                    if (GeosApplication.Instance.IdUserPermission == 22)
                    {
                        //GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2490(GeosApplication.Instance.ActiveUser.IdUser);
                        //Shubham[skadam] GEOS2-5399 CRM Outlook integration verification 13 08 2024
                        GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                        GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                        GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                        Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                        if (usrDefault != null)
                        {
                            GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                        }
                        else
                        {
                            GeosApplication.Instance.SelectedPlantOwnerUsersList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                        }
                    }
                    // End (PlantOwner)

                    GeosApplication.Instance.IsPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 23);
                    GeosApplication.Instance.IsPermissionAdminOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 9);
                    GeosApplication.Instance.IsCommercialUser = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 24);
                    GeosApplication.Instance.IsPermissionAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 29 && up.Permission.IdGeosModule == 5);

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                        GeosApplication.Instance.IsPermissionEnabled = false;
                    else
                        GeosApplication.Instance.IsPermissionEnabled = true;

                    break;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ThisAddIn_Startup() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }

            // Get the active Inspector object
            //Outlook.Inspector activeInspector = application.ActiveInspector();
            //if (activeInspector != null)
            //{
            //    // Get the title of the active item when the Outlook start.
            //    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ContactDisableFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //    MessageBox.Show("Active inspector: " + activeInspector.Caption);
            //}

            // Get the Explorer objects
            //Outlook.Explorers explorers = application.Explorers;

            // Get the active Explorer object
            //Outlook.Explorer activeExplorer = application.ActiveExplorer();
            //if (activeExplorer != null)
            //{
            // Get the title of the active folder when the Outlook start.
            //MessageBox.Show("Active explorer: " + activeExplorer.Caption);
            //}

            //inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_AddTextToNewMail);
        }

        /// <summary>
        /// Method for check application setting and and create user setting if not created.
        /// Set language dictionary as per user setting.
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        private void GetApplicationSettings()
        {

            GeosApplication.Instance.Logger.Log("Get user setting ", category: Category.Info, priority: Priority.Low);
            try
            {
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                //string pathUserSetting = GeosApplication.Instance.UserSettingFilePath;

                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings.Count == 0)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", "BlackAndBlue"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Language", Thread.CurrentThread.CurrentCulture.Name.ToString()));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedModule", string.Empty));
                    lstUserConfiguration.Add(new Tuple<string, string>("NotificationPageCount", "10"));
                    lstUserConfiguration.Add(new Tuple<string, string>("IsServiceIconShow", "false"));
                    lstUserConfiguration.Add(new Tuple<string, string>("ServiceRefreshSeconds", "20"));

                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferPeriod", DateTime.Now.Year.ToString()));
                    lstUserConfiguration.Add(new Tuple<string, string>("CustomPeriodOption", "0"));
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferFromInterval", DateTime.Now.ToShortDateString()));
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferToInterval", DateTime.Now.ToShortDateString()));

                    lstUserConfiguration.Add(new Tuple<string, string>("CrmTopOffers", "10"));
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmTopCustomers", "10"));
                    lstUserConfiguration.Add(new Tuple<string, string>("UserSessionDetail", string.Empty));

                    var regionCulture = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency", regionCulture.ISOCurrencySymbol));
                    lstUserConfiguration.Add(new Tuple<string, string>("AutoRefresh", "Yes"));
                    lstUserConfiguration.Add(new Tuple<string, string>("CurrentProfile", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("LoadDataOn", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedCRMSectionLoadData", ""));

                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedwarehouseId", "0"));
                    //PrinterSettings settings = new PrinterSettings();
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", string.Empty)); // settings.PrinterName));
                    lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModel", string.Empty));
                    lstUserConfiguration.Add(new Tuple<string, string>("ParallelPort", string.Empty));

                    lstUserConfiguration.Add(new Tuple<string, string>("PickingTimer", false.ToString()));
                    lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", "0"));
                    lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceTextSeparator", "0"));
                    lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcDns", "0"));
                    lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcTableName", "0"));
                    lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceSourceFieldSelectedIndex", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedScaleModel", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedPort", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedParity", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedStopBit", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedBaudRate", ""));
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedDataBit", ""));
                    //[001] added
                    lstUserConfiguration.Add(new Tuple<string, string>("Appearance", ""));
                    //PCM_Appearence
                    lstUserConfiguration.Add(new Tuple<string, string>("PCM_Appearance", ""));

                    //CRM Shortcuts
                    lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", "Shift + O"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Contact", "Shift + C"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Account", "Shift + A"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Appointment", "Shift + P"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Call", "Shift + L"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Task", "Shift + T"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Email", "Shift + E"));
                    lstUserConfiguration.Add(new Tuple<string, string>("Action", "Shift + S"));
                    lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", "Ctrl + O"));


                    var regionCulture_warehouse = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                    lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", regionCulture_warehouse.ISOCurrencySymbol));


                    ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    GeosApplication.Instance.CrmOfferYear = DateTime.Now.Year;
                    GeosApplication.Instance.CrmTopCustomers = 10;
                    GeosApplication.Instance.CrmTopOffers = 10;
                    //For Printer setting.s
                    GeosApplication.Instance.SelectedPrinter = null;// settings.PrinterName;
                }
                else
                {
                    //CreateUserSettingFileAfterVarification();

                    GeosApplication.Instance.CrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["CrmOfferPeriod"].ToString());
                    GeosApplication.Instance.CrmTopCustomers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopCustomers"].ToString());
                    GeosApplication.Instance.CrmTopOffers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopOffers"].ToString());
                    GeosApplication.Instance.SelectedPrinter = GeosApplication.Instance.UserSettings["SelectedPrinter"].ToString();
                    GeosApplication.Instance.LabelPrinter = GeosApplication.Instance.UserSettings["LabelPrinter"];
                    GeosApplication.Instance.LabelPrinterModel = GeosApplication.Instance.UserSettings["LabelPrinterModel"];
                    GeosApplication.Instance.ParallelPort = GeosApplication.Instance.UserSettings["ParallelPort"];
                }

                GeosApplication.Instance.Logger.Log("Get user setting successfully.", category: Category.Info, priority: Priority.Low);

                ////SetLanguageDictionary();
                //FillTheme();

                GeosApplication.Instance.Logger.Log("Get application setting ", category: Category.Info, priority: Priority.Low);
                // GeosApplication.Instance.ApplicationSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.ApplicationSettingFilePath);
                string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;// application setting file path 

                if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
                {
                    if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);
                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);
                    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);

                    //GeosServiceProviderList = new List<GeosServiceProvider>();
                    //GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

                    //UserConfigurationView objUserConfigurationView = new UserConfigurationView();

                    //UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
                    //// UserConfigurationViewModel.ListGeosServiceProviders  = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                    //EventHandler handle = delegate { objUserConfigurationView.Close(); };
                    //UserConfigurationViewModel.RequestClose += handle;
                    //objUserConfigurationView.DataContext = UserConfigurationViewModel;
                    //objUserConfigurationView.ShowDialogWindow();

                }
                else
                {
                    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
                    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                    GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();

                    //if (GeosApplication.Instance.ServicePath == null)
                    //{
                    //    UserConfigurationView objUserConfigurationView = new UserConfigurationView();
                    //    UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
                    //    EventHandler handle = delegate { objUserConfigurationView.Close(); };
                    //    UserConfigurationViewModel.RequestClose += handle;
                    //    objUserConfigurationView.DataContext = UserConfigurationViewModel;
                    //    objUserConfigurationView.ShowDialogWindow();
                    //    GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();
                    //}

                    if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                    {
                        GeosApplication.Instance.ApplicationSettings["ServicePath"] = GeosApplication.Instance.ServicePath;
                    }
                    else
                    {
                        GeosApplication.Instance.ApplicationSettings.Add("ServicePath", GeosApplication.Instance.ServicePath);
                    }
                }

                GeosApplication.Instance.SiteName = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in GetApplicationSettings() Method " + ex.Message, category: Prism.Logging.Category.Exception, priority: Priority.Low);
            }
        }

        private async void FillCommonDetails()
        {

            GeosApplication.Instance.Logger.Log("Method FillCommonDetails ...", category: Category.Info, priority: Priority.Low);
            //[001]Changed service method
            //GeosApplication.Instance.CompanyList = CrmStartUp.GetAllCompaniesDetails_V2490(GeosApplication.Instance.ActiveUser.IdUser);
            //Shubham[skadam] GEOS2-5399 CRM Outlook integration verification 13 08 2024
            GeosApplication.Instance.CompanyList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
            //[001] Added
            if (GeosApplication.Instance.CompanyList.Any(i => i.ShortName == GeosApplication.Instance.SiteName))
                GeosApplication.Instance.ActiveIdSite = GeosApplication.Instance.CompanyList.Where(i => i.ShortName == GeosApplication.Instance.SiteName).FirstOrDefault().IdCompany;

            GeosApplication.Instance.Logger.Log("Method FillCommonDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SetDate()
        {
            TimeSpan timeSpanStart = new TimeSpan(0, 0, 0);
            TimeSpan timeSpanEnd = new TimeSpan(23, 59, 59);

            if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption") && GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("0"))
            {
                DateTime Start = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 1, 1);
                Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                DateTime End = new DateTime(Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), 12, 31);
                End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
            }
            else if (GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption") && GeosApplication.Instance.UserSettings["CustomPeriodOption"].Equals("1"))
            {
                if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]) || string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]))
                {
                    DateTime Start = new DateTime(Convert.ToInt32(DateTime.Now.Year), 1, 1);
                    Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                    DateTime End = new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31);
                    End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                    GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                    GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
                }
                else
                {
                    try
                    {
                        DateTime Start = Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]);
                        Start = Convert.ToDateTime(Convert.ToDateTime(Start).ToString("yyyy/MM/dd"));
                        DateTime End = Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]);
                        End = Convert.ToDateTime(Convert.ToDateTime(End).ToString("yyyy/MM/dd"));
                        GeosApplication.Instance.SelectedyearStarDate = Start.Add(timeSpanStart);
                        GeosApplication.Instance.SelectedyearEndDate = End.Add(timeSpanEnd);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
