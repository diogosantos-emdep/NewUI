using Emdep.Geos.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Workbench;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;
using System.IO;
using Workbench.Views;
using Workbench.ViewModels;
using System.Net;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Threading;
using System.Globalization;
using System.Windows.Markup;
using Emdep.Geos.UI.Common;
using DevExpress.Mvvm.UI;
//using Emdep.Geos.Modules.Epc.ViewModels;
using System.Reflection;
using System.Windows.Media;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Adapters.Logging;
using Prism.Logging;
using System.ServiceModel;
using System.Diagnostics;
using System.Drawing.Printing;
using Emdep.Geos.Modules.OTM.CommonClass;


namespace Workbench
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region geosWorkbenchVersionNumber

        private GeosWorkbenchVersion geosWorkbenchVersionNumber;
        public List<GeosServiceProvider> GeosServiceProviderList { get; set; }
        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
        }

        #endregion

        Workstation workstation;
        Version installedversion;
        Version latestversion;
        public ImageSource img;

        IWorkbenchStartUp objWorkbenchStartUp;
        ICrmService CrmStartUp;
        IPLMService PLMService;
        IWarehouseService WarehouseService;//[Sudhir.Jangra][GEOS2-4414][08/09/2023]

        string myIP;

        public App()
        { 
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;

            GeosApplication.Instance.Logger.Log("Workbench Application CurrentDomain_UnhandledException...", category: Category.Exception, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log(string.Format("UnhandledException caught : {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log(string.Format("UnhandledException StackTrace : {0}", ex.StackTrace), category: Category.Exception, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log(string.Format("Runtime terminating: {0}", e.IsTerminating), category: Category.Exception, priority: Priority.Low);
        }

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            try
    
                {

                string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;

                if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
                {
                    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);

                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                }

                FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
                GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                GeosApplication.Instance.Logger.Log("Workbench Application Start...Application_Startup()...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                SetLanguageDictionary();
                FillTheme();
                DXSplashScreen.Show<MainSplashScreenView>();

                temp();
                GetApplicationSettings();
                FillCurrencyDetails();
                FillPCMCurrencyDetails();
                FillWarehouseCurrencyDetails();
                #region Display Login
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //objWorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());//[sudhir.jangra][GEOS2-4414]

                CreateApplicationFile();

                try
                {
                        // Additional direct write tests to diagnose file permissions/paths
                        try
                        {
                            string pdDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ActionPlanLogs");
                            string pdTest = Path.Combine(pdDir, "test-write.txt");
                            if (!Directory.Exists(pdDir)) Directory.CreateDirectory(pdDir);
                            File.WriteAllText(pdTest, "test write at " + DateTime.UtcNow.ToString("O"));
                            try { EventLog.WriteEntry("GeosWorkbench", "Wrote test file to ProgramData: " + pdTest, EventLogEntryType.Information); } catch { }

                            string tempPath = Path.Combine(Path.GetTempPath(), "test-write.txt");
                            File.WriteAllText(tempPath, "test write at " + DateTime.UtcNow.ToString("O"));
                            try { EventLog.WriteEntry("GeosWorkbench", "Wrote test file to Temp: " + tempPath, EventLogEntryType.Information); } catch { }

                            string desktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "test-write.txt");
                            File.WriteAllText(desktop, "test write at " + DateTime.UtcNow.ToString("O"));
                            try { EventLog.WriteEntry("GeosWorkbench", "Wrote test file to Desktop: " + desktop, EventLogEntryType.Information); } catch { }
                        }
                        catch (Exception ex)
                        {
                            try { EventLog.WriteEntry("GeosWorkbench", "Direct write tests failed: " + ex.Message, EventLogEntryType.Error); } catch { }
                        }
                    GeosApplication.Instance.UIThemeList = objWorkbenchStartUp.GetAllThemes();
                    GeosApplication.Instance.FontFamilyAsPerTheme = new FontFamily(GeosApplication.Instance.UIThemeList.Where(uithe => uithe.ThemeName == ApplicationThemeHelper.ApplicationThemeName).Select(ui => ui.FontFamily).FirstOrDefault());
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.ShowMessage(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    CustomMessageBox.ShowMessage(Workbench.App.Current.Resources["WorkbenchServiceError"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                }

                string updateInstalledFolder;
                string workbenchInstalledFolder;

                workbenchInstalledFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\";
                var directory = System.IO.Path.GetDirectoryName(workbenchInstalledFolder);
                DirectoryInfo directoryInfo = System.IO.Directory.GetParent(directory);
                updateInstalledFolder = directoryInfo.FullName + "\\Installer";

                string WorkbenchLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                installedversion = AssemblyVersion.GetAssemblyVersion(WorkbenchLocation);

                if (e.Args.Length == 0)
                {
                    try
                    {
                        User user = new User();
                       // objWorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                        user = objWorkbenchStartUp.GetUserByLoginName(System.Environment.UserName.ToString());

                        GeosApplication.Instance.Logger.Log("Getting installed version information", category: Category.Info, priority: Priority.Low);

                        GeosWorkbenchVersionNumber = objWorkbenchStartUp.GetLatestVersion();

                        if (GeosWorkbenchVersionNumber.IsPublish == 1)
                        {
                            GeosWorkbenchVersionNumber = objWorkbenchStartUp.GetCurrentPublishVersion();
                            latestversion = AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumber.VersionNumber);
                        }
                        else
                        {
                            if (user != null)
                            {
                                GeosWorkbenchVersionNumber = objWorkbenchStartUp.GetUserIsBetaCurrentVersion(user.IdUser);
                                int CheckForVersion = 0;
                                GeosWorkbenchVersion GeosWorkbenchVersionNumberPublish = objWorkbenchStartUp.GetCurrentPublishVersion();
                                CheckForVersion = AssemblyVersion.CompareAssemblyVersions(AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumber.VersionNumber), AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumberPublish.VersionNumber));
                                if (CheckForVersion > 0)
                                {
                                    latestversion = AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumber.VersionNumber);
                                }
                                else
                                {
                                    latestversion = AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumberPublish.VersionNumber);
                                }
                            }
                            else
                            {
                                GeosWorkbenchVersion GeosWorkbenchVersionNumbernew = objWorkbenchStartUp.GetCurrentPublishVersion();
                                latestversion = AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumbernew.VersionNumber);
                                GeosWorkbenchVersionNumber = GeosWorkbenchVersionNumbernew;
                            }

                            int Check = AssemblyVersion.CompareAssemblyVersions(latestversion, installedversion);
                            if (Check <= 0)
                            {
                                latestversion = installedversion;
                            }
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.ShowMessage(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }

                    int CheckForVersionWindowshow = 0;

                    if (latestversion != null)
                        CheckForVersionWindowshow = AssemblyVersion.CompareAssemblyVersions(latestversion, installedversion);

                    GeosApplication.Instance.Logger.Log("Check For Version Windowshow", category: Category.Info, priority: Priority.Low);

                    if (CheckForVersionWindowshow > 0)
                    {
                        GeosWorkbenchVersion geosWorkbenchVersionExpiryDate = objWorkbenchStartUp.GetWorkbenchVersionByVersionNumber(installedversion.ToString());
                        if (geosWorkbenchVersionExpiryDate.ExpiryDate != null)
                        {
                            GeosApplication.Instance.Logger.Log("Getting Server date and time from server", category: Category.Info, priority: Priority.Low);
                            GeosApplication.Instance.ServerDateTime = objWorkbenchStartUp.GetServerDateTime();
                            GeosApplication.Instance.Logger.Log("Getting Server date and time from server Successfully", category: Category.Info, priority: Priority.Low);

                            GeosApplication.Instance.Logger.Log("Check For Version Expiry Date", category: Category.Info, priority: Priority.Low);
                            if (GeosApplication.Instance.ServerDateTime.Date >= geosWorkbenchVersionExpiryDate.ExpiryDate.Value.Date)
                            {
                                if (DXSplashScreen.IsActive)
                                    DXSplashScreen.Close();
                                MessageBoxResult MessageBoxResult = CustomMessageBox.ShowMessage(Application.Current.Resources["WorkbenchUpdateExpiryDate"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                                if (MessageBoxResult == MessageBoxResult.OK)
                                {
                                    if (File.Exists(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe"))
                                    {
                                        ProcessControl.ProcessStart(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe", "1", "");
                                    }
                                    Environment.Exit(0);
                                }
                            }
                            else
                            {
                                VersionDifferentDownLoad(updateInstalledFolder);
                                Environment.Exit(0);
                            }
                        }
                        else
                        {
                            VersionDifferentDownLoad(updateInstalledFolder);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        if (!DXSplashScreen.IsActive)
                            DXSplashScreen.Show<MainSplashScreenView>();
                        ShowWorkbench();
                    }

                    GeosApplication.Instance.Logger.Log("Workbench Application Start...Application_Startup() executed successfully", category: Category.Info, priority: Priority.Low);

                    Application.Current.Shutdown();
                }
                else
                {
                    if (!DXSplashScreen.IsActive)
                        DXSplashScreen.Show<MainSplashScreenView>();
                    ShowWorkbench();
                    GeosApplication.Instance.Logger.Log("Workbench Application Start...Application_Startup() executed successfully", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error On Application_Startup Method" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            finally
            {
                GeosApplication.Instance.Logger.Log("Application_Startup Method end finally.", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Application.Current.Shutdown();
            }

            #endregion

            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        }

        public void temp()
        {
            string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;
            if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
            {
                if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath)
                        .FullName))
                    Directory.CreateDirectory(Directory
                        .GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);
                File.Copy(
                    System.AppDomain.CurrentDomain.BaseDirectory + @"\" +
                    GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);
                GeosApplication.Instance.GeosServiceProviders =
                    GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance
                        .ApplicationSettingFilePath);
                GeosServiceProviderList = new List<GeosServiceProvider>();
                GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

                UserConfigurationView objUserConfigurationView = new UserConfigurationView();

                UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
                // UserConfigurationViewModel.ListGeosServiceProviders  = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                EventHandler handle = delegate { objUserConfigurationView.Close(); };
                UserConfigurationViewModel.RequestClose += handle;
                objUserConfigurationView.DataContext = UserConfigurationViewModel;
                objUserConfigurationView.ShowDialogWindow();

            }
            else
            {
                GeosApplication.Instance.GeosServiceProviders =
                    GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance
                        .ApplicationSettingFilePath);
                GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                GeosApplication.Instance.ServicePath = GeosServiceProviderList
                    .Where(serviceProvider => serviceProvider.IsSelected == true)
                    .Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl)
                    .FirstOrDefault();
                // GeosApplication.Instance.ServicePath = "localhost:6699";
                if (GeosApplication.Instance.ServicePath == null)
                {
                    UserConfigurationView objUserConfigurationView = new UserConfigurationView();
                    UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
                    EventHandler handle = delegate { objUserConfigurationView.Close(); };
                    UserConfigurationViewModel.RequestClose += handle;
                    objUserConfigurationView.DataContext = UserConfigurationViewModel;
                    objUserConfigurationView.ShowDialogWindow();
                    GeosApplication.Instance.ServicePath = GeosServiceProviderList
                        .Where(serviceProvider => serviceProvider.IsSelected == true)
                        .Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl)
                        .FirstOrDefault();
                }

                // Force ServicePath to the desired endpoint
                GeosApplication.Instance.ServicePath = "10.13.3.33:90";

                if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                {
                    GeosApplication.Instance.ApplicationSettings["ServicePath"] =
                        GeosApplication.Instance.ServicePath;
                }
                else
                {
                    GeosApplication.Instance.ApplicationSettings.Add("ServicePath",
                        GeosApplication.Instance.ServicePath);
                }

            }
        }
        private void VersionDifferentDownLoad(string updateInstalledFolder)
        {
            if (DXSplashScreen.IsActive)
                DXSplashScreen.Close();
            MessageBoxResult MessageBoxResult = CustomMessageBox.ShowMessage(Application.Current.Resources["WorkbenchUpdate"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);//[GEOS2-4012][24.04.2023][rdixit]

            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (File.Exists(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe"))
                {
                    ProcessControl.ProcessStart(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe", "1", "");
                }
            }
            else
            {
                try
                {
                    DXSplashScreen.Show<MainSplashScreenView>();
                    ShowWorkbench();
                }
                catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(String.Format("Get an error On Application_Startup Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
            }
        }

        public void ShowWorkbench()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Start ShowWorkbench Method", category: Category.Info, priority: Priority.Low);

                try
                {
                    GeosApplication.Instance.Logger.Log("Getting Server date and time from server", category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ServerDateTime = objWorkbenchStartUp.GetServerDateTime();
                    GeosApplication.Instance.Logger.Log("Getting Server date and time from server Successfully", category: Category.Info, priority: Priority.Low);

                    GeosApplication.Instance.Logger.Log("Getting Workstation detail By IP", category: Category.Info, priority: Priority.Low);
                    workstation = objWorkbenchStartUp.GetWorkstationByIP(myIP);
                    GeosApplication.Instance.Logger.Log("Getting Workstation detail By IP Successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.ShowMessage(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                }

                WorkbenchWindow loginSuccess = new WorkbenchWindow();
                LoginViews loginWindow = new LoginViews();//chitra.girigosavi GEOS2-7914 23/06/2025
                LoginWindowViewModel loginWindowViewModel = new LoginWindowViewModel();

                try
                {
                    GeosApplication.Instance.Logger.Log("Getting GeosWorkbench Version Number", category: Category.Info, priority: Priority.Low);
                    GeosWorkbenchVersionNumber = objWorkbenchStartUp.GetWorkbenchVersionByVersionNumber(installedversion.ToString());
                    GeosApplication.Instance.Logger.Log("Getting GeosWorkbench Version Number successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.ShowMessage(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                }

                if (GeosWorkbenchVersionNumber != null && GeosWorkbenchVersionNumber.IsBeta == 1)
                {
                    loginWindowViewModel.VersionTitle = "GEOS - V " + installedversion + " (Beta)";
                }
                else
                {
                    loginWindowViewModel.VersionTitle = "GEOS - V " + installedversion;
                }

                //[start is userremenber check]
                if (!string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["UserSessionDetail"].ToString()))
                {
                    int _idUser;
                    bool isNumeric = int.TryParse(GeosApplication.Instance.UserSettings["UserSessionDetail"], out _idUser);
                    #region[rdixit][GEOS2-2742][20.06.2023]
                    string _UserPassword = string.Empty;
                    if (GeosApplication.Instance.UserSettings.Any(i => i.Key == "UserSessionPassword"))
                    {
                        _UserPassword = GeosApplication.Instance.UserSettings["UserSessionPassword"];
                    }
                    #endregion
                    if (isNumeric)
                    {
                        User user = new User();
                        user = objWorkbenchStartUp.GetUserById(_idUser);

                        if (user != null && user.IsEnabled != null && user.IsEnabled == 1 && user.Password.ToLower() == _UserPassword.ToLower() && user.IsAuthenticatedUsingLDAP == false)
                        {
                            //List<Company> companysuserwise = objWorkbenchStartUp.GetAllCompanyByUserId(user.IdUser);
                            //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                            //objWorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                            List<Company> companysuserwise = objWorkbenchStartUp.GetAllCompanyByUserId_V2680(user.IdUser);
                            //objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                            GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();
                            if (listCmpny.Contains(emdepCompany.Name))
                            {
                                Workbench.Properties.Settings.Default.UserSessionId = _idUser;
                                Workbench.Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Workbench.Properties.Settings.Default.UserSessionId = 0;
                                Workbench.Properties.Settings.Default.Save();
                            }

                            // code for user login entry
                            string hostName = Dns.GetHostName();                                    // Retrive the Name of HOST  
                            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP
                            //Dns.GetHostEntry()

                            UserLoginEntry userEntry = new UserLoginEntry();
                            userEntry.IdUser = user.IdUser;
                            userEntry.IpAddress = myIP;
                            userEntry.LoginTime = DateTime.Now;
                            userEntry.LogoutTime = null;
                            userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;
                            // userEntry = objWorkbenchStartUp.AddUserLoginEntry(userEntry);
                            userEntry = objWorkbenchStartUp.AddUserLoginEntry_V2210(userEntry);
                        }
                        else
                        {
                            if (user != null && user.IsEnabled != null && user.IsEnabled == 1 && user.IsAuthenticatedUsingLDAP)
                            {
                                try
                                {
                                    #region IsAuthenticatedUsingLDAP
                                    string LoginPassword = DecodeFromBase64(GeosApplication.Instance.UserSettings["WindowsUserSessionPassword"]);
                                    bool isAuthenticated = objWorkbenchStartUp.UserAuthenticate(user.Login.Trim(), LoginPassword.Trim());
                                    if (isAuthenticated)
                                    {
                                        //List<Company> companysuserwise = objWorkbenchStartUp.GetAllCompanyByUserId(user.IdUser);
                                        //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                                        //objWorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                                        List<Company> companysuserwise = objWorkbenchStartUp.GetAllCompanyByUserId_V2680(user.IdUser);
                                        //objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                        List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                                        GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();
                                        if (listCmpny.Contains(emdepCompany.Name))
                                        {
                                            Workbench.Properties.Settings.Default.UserSessionId = _idUser;
                                            Workbench.Properties.Settings.Default.Save();
                                        }
                                        else
                                        {
                                            Workbench.Properties.Settings.Default.UserSessionId = 0;
                                            Workbench.Properties.Settings.Default.Save();
                                        }

                                        // code for user login entry
                                        string hostName = Dns.GetHostName();                                    // Retrive the Name of HOST  
                                        string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP
                                                                                                               //Dns.GetHostEntry()

                                        UserLoginEntry userEntry = new UserLoginEntry();
                                        userEntry.IdUser = user.IdUser;
                                        userEntry.IpAddress = myIP;
                                        userEntry.LoginTime = DateTime.Now;
                                        userEntry.LogoutTime = null;
                                        userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;
                                        // userEntry = objWorkbenchStartUp.AddUserLoginEntry(userEntry);
                                        userEntry = objWorkbenchStartUp.AddUserLoginEntry_V2210(userEntry);
                                    }
                                    else
                                    {
                                        //CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserWindowscredentialsValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        Workbench.Properties.Settings.Default.UserSessionId = 0;
                                        Workbench.Properties.Settings.Default.Save();
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    Workbench.Properties.Settings.Default.UserSessionId = 0;
                                    Workbench.Properties.Settings.Default.Save();
                                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() IsAuthenticatedUsingLDAP Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }
                            else
                            {
                                Workbench.Properties.Settings.Default.UserSessionId = 0;
                                Workbench.Properties.Settings.Default.Save();
                            }
                        }
                    }
                }

                //[End is userremenber check]


				//[nsatpute][29.08.2025][GEOS2-9342]
                if (Workbench.Properties.Settings.Default.UserSessionId > 0)
                {
                    if (workstation == null)
                    {
                        GeosApplication.Instance.Logger.Log("Initialising Login window", category: Category.Info, priority: Priority.Low);
                        User user = new User();                        
                        user = objWorkbenchStartUp.GetUserById(Workbench.Properties.Settings.Default.UserSessionId);

                        if (GeosApplication.Instance.UserSettings.ContainsKey("Login"))
                        {
                            string login = GeosApplication.Instance.UserSettings["Login"];
                            if (login == "Office")
                                loginWindowViewModel.SelectedViewIndex = 0;
                            else
                                loginWindowViewModel.SelectedViewIndex = 1;
                        }
                        else
                        {
                            loginWindowViewModel.SelectedViewIndex = 0;
                        }

                        loginWindowViewModel.PageViewItemHeight = 50;
                        loginWindowViewModel.ShutDownButtonVisibility = Visibility.Hidden;
                        loginWindowViewModel.LoginName = user.Login.Trim();
                        loginWindowViewModel.IsRememberMe = true;
                        if (GeosApplication.Instance.UserSettings.ContainsKey("AesUserSessionPassword") && GeosApplication.Instance.UserSettings.ContainsKey("AesUserSessionPassword").ToString() != string.Empty)
                        {
                            string password = Encrypt.AesDecrypt(GeosApplication.Instance.UserSettings["AesUserSessionPassword"].ToString());
                            loginWindowViewModel.LoginPassword = password;
                        }
                        loginWindow.DataContext = loginWindowViewModel;
                        EventHandler handle = delegate { loginWindow.Close(); };
                        loginWindowViewModel.RequestClose += handle;
                        loginWindowViewModel.LoginWorkstation = workstation;

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        loginWindow.ShowDialog();
                    }
                    else if (workstation != null)
                    {
                        if (workstation.IsManufacturingStation == 0)//Office
                        {
                            //loginWindowViewModel.SelectedViewIndex = 0;
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Login"))
                            {
                                string login = GeosApplication.Instance.UserSettings["Login"];
                                if (login == "Office")
                                    loginWindowViewModel.SelectedViewIndex = 0;
                                else
                                    loginWindowViewModel.SelectedViewIndex = 1;
                            }
                            else
                            {
                                loginWindowViewModel.SelectedViewIndex = 0;
                            }

                            loginWindowViewModel.PageViewItemHeight = 0;
                            loginWindowViewModel.ShutDownButtonVisibility = Visibility.Hidden;
                            EventHandler handle = delegate { loginWindow.Close(); };
                            loginWindowViewModel.RequestClose += handle;
                            loginWindow.DataContext = loginWindowViewModel;
                            loginWindowViewModel.LoginWorkstation = workstation;

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Initialising login window successfully", category: Category.Info, priority: Priority.Low);
                            loginWindow.ShowDialog();
                        }

                        if (workstation.IsManufacturingStation == 1)//production
                        {
                            GeosApplication.Instance.Logger.Log("Initialising Login window For Manufacturing Station", category: Category.Info, priority: Priority.Low);
                            loginWindowViewModel.PageViewItemHeight = 0;
                            loginWindowViewModel.SelectedViewIndex = 1;
                            loginWindowViewModel.ShutDownButtonVisibility = Visibility.Visible;
                            loginWindowViewModel.MinMaxClosePanelVisibility = Visibility.Hidden;
                            EventHandler handle = delegate { loginWindow.Close(); };
                            loginWindowViewModel.RequestClose += handle;
                            loginWindowViewModel.LoginWorkstation = workstation;
                            loginWindow.DataContext = loginWindowViewModel;

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Initialising Login window For Manufacturing Station Successfully", category: Category.Info, priority: Priority.Low);
                            loginWindow.ShowDialog();
                        }
                    }
                }
                else
                {
                    if (workstation == null)
                    {
                        GeosApplication.Instance.Logger.Log("Initialising Login window", category: Category.Info, priority: Priority.Low);

                        //loginWindowViewModel.SelectedViewIndex = 0;

                        if (GeosApplication.Instance.UserSettings.ContainsKey("Login"))
                        {
                            string login = GeosApplication.Instance.UserSettings["Login"];
                            if (login == "Office")
                                loginWindowViewModel.SelectedViewIndex = 0;
                            else
                                loginWindowViewModel.SelectedViewIndex = 1;
                        }
                        else
                        {
                            loginWindowViewModel.SelectedViewIndex = 0;
                        }

                        loginWindowViewModel.PageViewItemHeight = 50;
                        loginWindowViewModel.ShutDownButtonVisibility = Visibility.Hidden;
                        loginWindow.DataContext = loginWindowViewModel;
                        EventHandler handle = delegate { loginWindow.Close(); };
                        loginWindowViewModel.RequestClose += handle;
                        loginWindowViewModel.LoginWorkstation = workstation;

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        loginWindow.ShowDialog();
                    }
                    else if (workstation != null)
                    {
                        if (workstation.IsManufacturingStation == 0)//Office
                        {
                            //loginWindowViewModel.SelectedViewIndex = 0;
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Login"))
                            {
                                string login = GeosApplication.Instance.UserSettings["Login"];
                                if (login == "Office")
                                    loginWindowViewModel.SelectedViewIndex = 0;
                                else
                                    loginWindowViewModel.SelectedViewIndex = 1;
                            }
                            else
                            {
                                loginWindowViewModel.SelectedViewIndex = 0;
                            }

                            loginWindowViewModel.PageViewItemHeight = 0;
                            loginWindowViewModel.ShutDownButtonVisibility = Visibility.Hidden;
                            EventHandler handle = delegate { loginWindow.Close(); };
                            loginWindowViewModel.RequestClose += handle;
                            loginWindow.DataContext = loginWindowViewModel;
                            loginWindowViewModel.LoginWorkstation = workstation;

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Initialising login window successfully", category: Category.Info, priority: Priority.Low);
                            loginWindow.ShowDialog();
                        }

                        if (workstation.IsManufacturingStation == 1)//production
                        {
                            GeosApplication.Instance.Logger.Log("Initialising Login window For Manufacturing Station", category: Category.Info, priority: Priority.Low);
                            loginWindowViewModel.PageViewItemHeight = 0;
                            loginWindowViewModel.SelectedViewIndex = 1;
                            loginWindowViewModel.ShutDownButtonVisibility = Visibility.Visible;
                            loginWindowViewModel.MinMaxClosePanelVisibility = Visibility.Hidden;
                            EventHandler handle = delegate { loginWindow.Close(); };
                            loginWindowViewModel.RequestClose += handle;
                            loginWindowViewModel.LoginWorkstation = workstation;
                            loginWindow.DataContext = loginWindowViewModel;

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Initialising Login window For Manufacturing Station Successfully", category: Category.Info, priority: Priority.Low);
                            loginWindow.ShowDialog();
                        }
                    }
                }
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On ShowWorkbench Method" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }

            GeosApplication.Instance.Logger.Log("End ShowWorkbench Method", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for set user permission as per user setting file.
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selected site
        /// </summary>
        private void SetUserPermission()
        {
            List<int> userPermissionlst = new List<int>() { 20, 21, 22 };

            bool isCurrentProfile = false;
            int value1;

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile")
                && int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value1))
            {
                isCurrentProfile = GeosApplication.Instance.ActiveUser.UserPermissions.Any(usr => usr.IdPermission == Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString()));
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile") && isCurrentProfile)
            {
                int value;
                if (int.TryParse(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString(), out value))
                    GeosApplication.Instance.IdUserPermission = Convert.ToInt32(GeosApplication.Instance.UserSettings["CurrentProfile"].ToString());
                else
                {
                    GeosApplication.Instance.IdUserPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Where(usr => userPermissionlst.Contains(usr.IdPermission)).OrderBy(ord => ord.IdPermission).Select(slt => slt.IdPermission).FirstOrDefault(); ;
                }
            }
            else
            {
                GeosApplication.Instance.IdUserPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Where(usr => userPermissionlst.Contains(usr.IdPermission)).OrderByDescending(ord => ord.IdPermission).Select(slt => slt.IdPermission).FirstOrDefault();
            }

            GeosApplication.Instance.IsPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 23);
            GeosApplication.Instance.IsPermissionAdminOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 9);
            GeosApplication.Instance.IsCommercialUser = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 24);
            GeosApplication.Instance.IsPermissionAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 29 && up.Permission.IdGeosModule == 5);
            GeosApplication.Instance.IsPermissionNameEditInPCMArticle = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 53 && up.Permission.IdGeosModule == 8);
            GeosApplication.Instance.IsPermissionWMSgridValueColumn = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 54 && up.Permission.IdGeosModule == 6);
            GeosApplication.Instance.IsPermissionForManageItemPrice = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 55 && up.Permission.IdGeosModule == 8);
            //[cpatil[20-09-2021][GEOS2-3342]
            GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 59 && up.Permission.IdGeosModule == 8);
            //[cpatil[06-10-2021][GEOS2-3336]
            GeosApplication.Instance.IsPLMPermissionNameECOS_Synchronization = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 60 && up.Permission.IdGeosModule == 11);

            GeosApplication.Instance.IsPLMPermissionView = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 50 && up.Permission.IdGeosModule == 11);
            GeosApplication.Instance.IsPLMPermissionChange = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 62 && up.Permission.IdGeosModule == 11);
            GeosApplication.Instance.IsPCMEditFreePluginsPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 88 && up.Permission.IdGeosModule == 8);
            //[Sudhir.Jangra][GEOS2-4901]
            GeosApplication.Instance.IsPCMAddEditPermissionForHardLockLicense = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 93 && up.Permission.IdGeosModule == 8);

            GeosApplication.Instance.IsPLMPermissionAdmin = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 63 && up.Permission.IdGeosModule == 11);
            //[rdixit][GEOS2-4971][15.12.2023]
            GeosApplication.Instance.IsSCMPermissionAdmin = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 94 && up.Permission.IdGeosModule == 1);

            //[rdixit][GEOS2-4971][15.12.2023]
            GeosApplication.Instance.IsSCMPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 108 && up.Permission.IdGeosModule == 1);
            //[rdixit][GEOS2-5479][06.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorStatus = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 109 && up.Permission.IdGeosModule == 1);
            //[rdixit][07.05.2024][GEOS2-5699]
            GeosApplication.Instance.IsSCMReportsAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 111 && up.Permission.IdGeosModule == 1);
            //[rdixit][20.05.2024][GEOS2-5477]
            GeosApplication.Instance.IsSCMREditConnectorBasic = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 112 && up.Permission.IdGeosModule == 1);
            //[pjadhav[07-11-2022][GEOS2-2566]
            GeosApplication.Instance.IsPermissionReadOnlyForPCM = !GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 70 && up.Permission.IdGeosModule == 8);
            //[pjadhav[22-11-2022][GEOS2-3969]
            GeosApplication.Instance.IsPermissionReadOnlyForPLM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 33 && up.Permission.IdGeosModule == 8);
            //[pjadhav[29-11-2022][GEOS2-3550]


            GeosApplication.Instance.IsAdminInventoryAuditPermissionForWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 32 && up.Permission.IdGeosModule == 6);
            //[pramod.misal][GEOS2-][12.10.2023]
            GeosApplication.Instance.IsWMSDeliveryNoteLockUnlockPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 91 && up.Permission.IdGeosModule == 6);
            //[pramod.misal][GEOS2-5481][12.10.2023]
            GeosApplication.Instance.IsSCMEditFiltersManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 113 && up.Permission.IdGeosModule == 1);
            //[pramod.misal][GEOS2-5482][24.10.2023]
            GeosApplication.Instance.IsSCMEditFamiliesManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 114 && up.Permission.IdGeosModule == 1);
            //[pramod.misal][GEOS2-5483][27.10.2023]
            GeosApplication.Instance.IsSCMEditPropertiesManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 115 && up.Permission.IdGeosModule == 1);
            //[rdixit][GEOS2-5480][29.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorLinks = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 116 && up.Permission.IdGeosModule == 1);
            //[rdixit][GEOS2-5478][29.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorAdvanced = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 117 && up.Permission.IdGeosModule == 1);
            //[pramod.misal][GEOS2-5525][12.08.2024]
            GeosApplication.Instance.IsSCMEditLocationsManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 123 && up.Permission.IdGeosModule == 1);

            //[pramod.misal][GEOS2-6463][07.12.2024]
            GeosApplication.Instance.IsOTMCancelPO = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 131 && up.Permission.IdGeosModule == 14);

            //[pramod.misal][GEOS2-6463][07.12.2024]
            GeosApplication.Instance.IsOTMFullUpdatePO = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 132 && up.Permission.IdGeosModule == 14);

            //[pramod.misal][GEOS2-6463][07.12.2024]
            GeosApplication.Instance.IsOTMUnlinkOfferfromPO = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 133 && up.Permission.IdGeosModule == 14);

            //[pramod.misal][GEOS2-6463][07.12.2024]
            GeosApplication.Instance.IsOTMViewOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 127 && up.Permission.IdGeosModule == 14);



            //[pallavi.kale][GEOS2-5386][16.01.2025]
            GeosApplication.Instance.IsTSMUsersViewOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 139 && up.Permission.IdGeosModule == 13);

            GeosApplication.Instance.IsTSMUsersEdit = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 140 && up.Permission.IdGeosModule == 13);

            if (GeosApplication.Instance.IsAdminInventoryAuditPermissionForWMS)
            {
                GeosApplication.Instance.IsEditInventoryAuditPermissionForWMS = true;
                GeosApplication.Instance.IsRemoveInventoryAuditPermissionForWMS = true;
                GeosApplication.Instance.IsCreateInventoryAuditPermissionForWMS = true;
            }
            else
            {
                GeosApplication.Instance.IsEditInventoryAuditPermissionForWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 75 && up.Permission.IdGeosModule == 6);
                GeosApplication.Instance.IsRemoveInventoryAuditPermissionForWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 76 && up.Permission.IdGeosModule == 6);
                GeosApplication.Instance.IsCreateInventoryAuditPermissionForWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 77 && up.Permission.IdGeosModule == 6);

            }
            //[rdixit][17.05.2023][GEOS2-4273]
            GeosApplication.Instance.IsCRMactionsLauncherPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 85 && up.Permission.IdGeosModule == 5);

            #region HRM Permission
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38 && up.Permission.IdGeosModule == 7);
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsChangeOrAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38) && up.Permission.IdGeosModule == 7);
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsTravel_AssistantPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 96) && up.Permission.IdGeosModule == 7);
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsControlPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 97) && up.Permission.IdGeosModule == 7);
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsPlant_FinancePermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 98) && up.Permission.IdGeosModule == 7);
            //[rdixit][16.01.2024][GEOS2-5074]
            GeosApplication.Instance.IsWatchMySelfOnlyPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 99) && up.Permission.IdGeosModule == 7);

            //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
            GeosApplication.Instance.IsChangeAndAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38));

            #endregion


            GeosApplication.Instance.IsManageTrainingPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 58 && up.Permission.IdGeosModule == 7);
            //[skadam[11-02-2022][GEOS2-3439]
            GeosApplication.Instance.IsPermissionForSRMEdit_Supplier = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 64 && up.Permission.IdGeosModule == 10);
            //[sdeshpande][11-08-2022][GEOS2-3874]
            GeosApplication.Instance.IsAdminPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 57 && up.Permission.IdGeosModule == 12);
            GeosApplication.Instance.IsEditBulkPickingPermissionWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 89 && up.Permission.IdGeosModule == 6);//[Sudhir.jangra][GEOS2-4414][08/09/2023]
            //[rdixit][27.12.2023][GEOS2-4875]
            GeosApplication.Instance.PCMEditArticleCategoryMapping = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 95 && up.Permission.IdGeosModule == 8);
            //[rdixit][04.04.2024][GEOS2-5278]
            GeosApplication.Instance.IsHRMAttendanceSplitPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 107 && up.Permission.IdGeosModule == 7);
            //[pramod.misal][GEOS2-5477][06.05.2024]
            GeosApplication.Instance.IsSCMViewConfigurationPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 110 && up.Permission.IdGeosModule == 1);
            //[rushikesh.gaikwad][GEOS2-5801][13.09.2024]
            GeosApplication.Instance.IsSCMSampleRegistrationPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 125 && up.Permission.IdGeosModule == 1);
            //[Shweta.Thube][GEOS2-5981][05/10/2024]
            GeosApplication.Instance.IsAPMActionPlanPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15);


            //[GEOS2-6499][rdixit][04.11.2024]
            GeosApplication.Instance.IsHRMManageEmployeeContactsPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 128 && up.IdPermission != 38) && up.Permission.IdGeosModule == 7);

            //[GEOS2-6509][rdixit][05.11.2024]
            GeosApplication.Instance.IsHRMManageShiftPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 129 || up.IdPermission == 38) && up.Permission.IdGeosModule == 7);

            //[/GEOS2-5906][rdixit][15.11.2024]
            GeosApplication.Instance.IsWMSManageInspectionPoints = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 130 || up.IdPermission == 32) && up.Permission.IdGeosModule == 6);

            //[rahul.gadhave][GEOS2-6829][03.01.2025]
            GeosApplication.Instance.PO_Template_Manager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 138 && up.Permission.IdGeosModule == 14);

            GeosApplication.Instance.IsHRMTravelManagerPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 143 && up.Permission.IdGeosModule == 7);

            GeosApplication.Instance.IsWMManagerPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 146 && up.Permission.IdGeosModule == 10);
            GeosApplication.Instance.IsWMSAdminPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 32 && up.Permission.IdGeosModule == 6);

            GeosApplication.Instance.IsWMS_SchedulePermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 147 && up.Permission.IdGeosModule == 6);
            

            if (GeosApplication.Instance.IsAdminPermissionERM)
            {
                GeosApplication.Instance.IsReadSODPermissionERM = true;
                GeosApplication.Instance.IsReadWOPermissionERM = true;
                GeosApplication.Instance.IsEditSODPermissionERM = true;
                GeosApplication.Instance.IsEditWOPermissionERM = true;
                GeosApplication.Instance.IsTimeTrackingReadPermissionERM = true;

                GeosApplication.Instance.IsReadProductionPlanPermissionERM = true; //[Rupali Sarode][GEOS2-4155][6-2-2023]
                GeosApplication.Instance.IsEditProductionPlanPermissionERM = true; //[Rupali Sarode][GEOS2-4155][6-2-2023]
                GeosApplication.Instance.IsReadWorkStagesPermissionERM = true; //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditWorkStagesPermissionERM = true; //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditDeliveryTimeDistributionERM = true;//[Aishwarya Ingale][GEOS2-5269][29-01-2024]
                GeosApplication.Instance.IsViewPermissionERM = true;//[Pallavi jadhav][GEOS2-5269][02-02-2024]
                                                                    // GeosApplication.Instance.IsViewSupervisorERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 119 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5910][17-07-2024]

            }
            else
            {
                GeosApplication.Instance.IsReadSODPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 73 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsReadWOPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 71 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsEditSODPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 74 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsEditWOPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 72 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsTimeTrackingReadPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 66 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsReadProductionPlanPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 80 && up.Permission.IdGeosModule == 12); //[Rupali Sarode][GEOS2-4155][6-2-2023]
                GeosApplication.Instance.IsEditProductionPlanPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 81 && up.Permission.IdGeosModule == 12); //[Rupali Sarode][GEOS2-4155][6-2-2023]
                GeosApplication.Instance.IsReadWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 78 && up.Permission.IdGeosModule == 12); //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 79 && up.Permission.IdGeosModule == 12); //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditDeliveryTimeDistributionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 102 && up.Permission.IdGeosModule == 12);//[Aishwarya Ingale][GEOS2-5269][29-01-2024]
                GeosApplication.Instance.IsViewPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 103 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5269][02-02-2024]
                GeosApplication.Instance.IsViewSupervisorERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 119 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5910][17-07-2024]

            }

            //GeosApplication.Instance.IsReadWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 78 && up.Permission.IdGeosModule == 12); //[sdeshpande][GEOS2-3841][11/1/2023]
            //GeosApplication.Instance.IsEditWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 79 && up.Permission.IdGeosModule == 12); //[sdeshpande][GEOS2-3841][11/1/2023]


            if (GeosApplication.Instance.IsPermissionReadOnly)
                GeosApplication.Instance.IsPermissionEnabled = false;
            else
                GeosApplication.Instance.IsPermissionEnabled = true;

            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

            // Start (SalesOwner) - Selected Sales Owners User list for CRM. 
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                GeosApplication.Instance.SalesOwnerUsersList = objWorkbenchStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.SelectedSalesOwnerUsersList = new List<object>();
                UserManagerDtl usrDefault = GeosApplication.Instance.SalesOwnerUsersList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Visible;

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

                //[001] Changed service method GetAllCompaniesDetails to GetAllCompaniesDetails_V2040
                //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.Country.IdCountry).ToList();
                // GeosApplication.Instance.PlantOwnerUsersList = GeosApplication.Instance.PlantOwnerUsersList.OrderBy(o => o.IsPermission == true).ThenBy(n => n.ShortName).ToList();
                GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();

                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Visible;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

                // EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(GeosApplication.Instance.ActiveUser...Site.ConnectPlantId));
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
            //Shubham[skadam] GEOS2-7896 For Sales Assistant permission Engineering Analysis data not showing.  18 04 2025
            try
            {
                if (GeosApplication.Instance.IdUserPermission == 20)
                {
                    GeosApplication.Instance.SelectedPlantOwnerUsersList = new List<object>();
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);
                    if (GeosApplication.Instance.PlantOwnerUsersList != null)
                    {
                        Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                        if (usrDefault != null)
                        {
                            GeosApplication.Instance.SelectedPlantOwnerUsersList.Add(usrDefault);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error On SetUserPermission Method", category: Category.Exception, priority: Priority.Low);
            }
            // End (PlantOwner)
            //[rdixit][28.08.2024][GEOS2-5410]
            // GeosApplication.Instance.IsSAMReport = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 124 && up.Permission.IdGeosModule == 9);

        }

        //public void LoadStyleDictionaryFromFile(string inFileName)
        //{
        //    if (File.Exists(inFileName))
        //    {
        //        try
        //        {
        //            using (var fs = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        //            {
        //                // Read in ResourceDictionary File
        //                var dic = (ResourceDictionary)XamlReader.Load(fs);
        //                // Clear any previous dictionaries loaded
        //                //App.Current.Resources.MergedDictionaries.Clear();
        //                // Add in newly loaded Resource Dictionary
        //                App.Current.Resources.MergedDictionaries.Add(dic);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

        #region  Language
        private void SetLanguageDictionary()
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo((CultureInfo.CurrentCulture.IetfLanguageTag)); // set the current region setting to Application for date and numeric value
            ResourceDictionary dict = new ResourceDictionary();
            string Language = "";
            CultureInfo tempCulture = Thread.CurrentThread.CurrentCulture;
            if (GeosApplication.Instance.UserSettings.ContainsKey("Language")
                && !String.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Language"].ToString()))
            {
                Language = GeosApplication.Instance.UserSettings["Language"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            }
            else
            {
                Language = Thread.CurrentThread.CurrentCulture.Name.ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            }

            //[start] Below code is for change NumberFormat of Culture as per region of system

            Thread.CurrentThread.CurrentUICulture.NumberFormat = tempCulture.NumberFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = tempCulture.NumberFormat;

            //[End] Below code is for change NumberFormat of Culture as per region of system

            GeosApplication.Instance.CurrentCulture = Language;
            GeosApplication.Instance.SetLanguageDictionary();
            try
            {
                dict.Source = new Uri("/GeosWorkbench;component/Resources/Language." + Language + ".xaml", UriKind.Relative);
            }
            catch (Exception)
            {
                dict.Source = new Uri("/GeosWorkbench;component/Resources/Language.xaml", UriKind.Relative);

            }

            this.Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// Method for check application setting and and create user setting if not created.
        /// Set language dictionary as per user setting.
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// [002][avpawar][GEOS2-2700]-
        /// </summary>
        public void GetApplicationSettings()
        {
            GeosApplication.Instance.Logger.Log("Get user setting ", category: Category.Info, priority: Priority.Low);


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
                PrinterSettings settings = new PrinterSettings();

                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter1", settings.PrinterName)); //[002] Added
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", settings.PrinterName));
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModel", string.Empty));
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPort", string.Empty));

                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter2", settings.PrinterName));               //[002] Added
                /*lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinterForPrinter2", settings.PrinterName));  */             //[002] Added
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModelForPrinter2", string.Empty));                     //[002] Added
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPortForPrinter2", string.Empty));                          //[002] Added

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
                //[rdixit][GEOS2-3697][11.07.2022 ]
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveDataSourceSelectedIndex", ""));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSeparator", "0"));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcDns", "0"));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcTableName", "0"));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveSourceFieldSelectedIndex", ""));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSourceFieldSelectedIndex", ""));
                //[001] added
                lstUserConfiguration.Add(new Tuple<string, string>("Appearance", ""));
                //PCM_Appearence
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_Appearance", ""));
                //PCM_SelectedCurrency
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_SelectedCurrency", ""));
                //PCM_Images
                lstUserConfiguration.Add(new Tuple<string, string>("PCMImage", "4"));
                //PCM_Attachments
                lstUserConfiguration.Add(new Tuple<string, string>("PCMAttachment", "4"));
                //PCM_Links
                lstUserConfiguration.Add(new Tuple<string, string>("PCMLinks", "4"));

                //CRM Shortcuts
                lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", "Ctrl + O"));
                lstUserConfiguration.Add(new Tuple<string, string>("Contact", "Ctrl + C"));
                lstUserConfiguration.Add(new Tuple<string, string>("Account", "Ctrl + A"));
                lstUserConfiguration.Add(new Tuple<string, string>("Appointment", "Ctrl + P"));
                lstUserConfiguration.Add(new Tuple<string, string>("Call", "Ctrl + L"));
                lstUserConfiguration.Add(new Tuple<string, string>("Task", "Ctrl + T"));
                lstUserConfiguration.Add(new Tuple<string, string>("Email", "Ctrl + E"));
                lstUserConfiguration.Add(new Tuple<string, string>("Action", "Ctrl + S"));
                lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", "Ctrl + F"));
                lstUserConfiguration.Add(new Tuple<string, string>("MatrixList", "Alt + P"));

                //HRM Shortcuts     [sjadhav]
                lstUserConfiguration.Add(new Tuple<string, string>("Attendance", "Ctrl + A"));
                lstUserConfiguration.Add(new Tuple<string, string>("Employee", "Ctrl + E"));
                lstUserConfiguration.Add(new Tuple<string, string>("Holiday", "Ctrl + H"));
                lstUserConfiguration.Add(new Tuple<string, string>("JobDescriptions", "Ctrl + J"));
                lstUserConfiguration.Add(new Tuple<string, string>("Leave", "Ctrl + L"));
                lstUserConfiguration.Add(new Tuple<string, string>("AttendanceList", "Alt + A"));
                lstUserConfiguration.Add(new Tuple<string, string>("OrganizationChart", "Alt + O"));
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendance", "Alt + T"));
                lstUserConfiguration.Add(new Tuple<string, string>("SearchEmployee", "Alt + E"));

                //PCM_Appearence 
                lstUserConfiguration.Add(new Tuple<string, string>("PLM_Appearance", ""));
                //Shubham[skadam] GEOS2-2597 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 1  03 02 2023
                lstUserConfiguration.Add(new Tuple<string, string>("Announcement", "Ctrl + S"));
                var regionCulture_warehouse = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", regionCulture_warehouse.ISOCurrencySymbol));

                //SAM setting
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefresh", "Yes"));
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefreshInterval", "5"));

                //ERM_SelectedCurrency   //[GEOS2-3997][Rupali Sarode][02-12-2022]
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_SelectedCurrency", "EUR"));

                //ERM_Appearance   //[GEOS2- 4920][Aishwarya Ingale][30-11-2023]
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_Appearance", "Top"));

                //var regionCulture_PCM = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                //lstUserConfiguration.Add(new Tuple<string, string>("PCM_SelectedCurrency", regionCulture_PCM.ISOCurrencySymbol));

                //ERMTimeTraking_IsFileDeleted   //[GEOS2- 5466][pallavi jadhav][02-04-2024]
                lstUserConfiguration.Add(new Tuple<string, string>("ERMTimeTraking_IsFileDeleted", "0"));

                lstUserConfiguration.Add(new Tuple<string, string>("ERMProductionTimeLine_IsFileDeleted", "0"));

                //SCM_Docklayout_SettingFilePath   //[GEOS2-5392][pramod.misal][18-09-2024]
                lstUserConfiguration.Add(new Tuple<string, string>("SCM_Docklayout_SettingFilePath_V2570", "0"));

                //APMActionPlan_IsFileDeleted_V2570 //[Shweta.Thube][GEOS2-5981][01-10-2024]
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlan_IsFileDeleted_V2570", "0"));

                //OTM_SelectedCurrency    // [pramod.misal][25-10-2024][GEOS2-6461]
                lstUserConfiguration.Add(new Tuple<string, string>("OTM_SelectedCurrency", "EUR"));

                //SCM Shortcuts [shweta.thube][GEOS2-6630]

                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCRMSectionLoadData", ""));
                lstUserConfiguration.Add(new Tuple<string, string>("Create", "Shift + C"));
                lstUserConfiguration.Add(new Tuple<string, string>("Search", "Shift + S"));
                lstUserConfiguration.Add(new Tuple<string, string>("Locations", "Shift + L"));
                lstUserConfiguration.Add(new Tuple<string, string>("Properties", "Shift + P"));
                lstUserConfiguration.Add(new Tuple<string, string>("Families", "Shift + F"));
                lstUserConfiguration.Add(new Tuple<string, string>("SearchManager", "Shift + E"));
                lstUserConfiguration.Add(new Tuple<string, string>("NewSamples", "Shift + N"));
                lstUserConfiguration.Add(new Tuple<string, string>("ModifiedSamples", "Ctrl + M"));
                lstUserConfiguration.Add(new Tuple<string, string>("Connectors3D", "Ctrl + O"));


                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                GeosApplication.Instance.CrmOfferYear = DateTime.Now.Year;
                GeosApplication.Instance.CrmTopCustomers = 10;
                GeosApplication.Instance.CrmTopOffers = 10;
                GeosApplication.Instance.PCMImage = "4";
                GeosApplication.Instance.PCMAttachment = "4";
                GeosApplication.Instance.PCMLinks = "4";
                //For Printer setting.
                GeosApplication.Instance.SelectedPrinter = settings.PrinterName;
            }
            else
            {
                CreateUserSettingFileAfterVarification();

                GeosApplication.Instance.CrmOfferYear = Convert.ToInt64(GeosApplication.Instance.UserSettings["CrmOfferPeriod"].ToString());
                GeosApplication.Instance.CrmTopCustomers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopCustomers"].ToString());
                GeosApplication.Instance.CrmTopOffers = Convert.ToUInt16(GeosApplication.Instance.UserSettings["CrmTopOffers"].ToString());
                GeosApplication.Instance.SelectedPrinter = GeosApplication.Instance.UserSettings["SelectedPrinter"].ToString();
                GeosApplication.Instance.LabelPrinter = GeosApplication.Instance.UserSettings["LabelPrinter"];
                GeosApplication.Instance.LabelPrinterModel = GeosApplication.Instance.UserSettings["LabelPrinterModel"];
                GeosApplication.Instance.ParallelPort = GeosApplication.Instance.UserSettings["ParallelPort"];

                GeosApplication.Instance.PCMImage = Convert.ToString(GeosApplication.Instance.UserSettings["PCMImage"].ToString());//[Sudhir.Jangra][GEOS2-1960][02/03/2023]
                GeosApplication.Instance.PCMAttachment = Convert.ToString(GeosApplication.Instance.UserSettings["PCMAttachment"].ToString());//[Sudhir.Jangra][GEOS2-1960][02/03/2023]
                GeosApplication.Instance.PCMLinks = Convert.ToString(GeosApplication.Instance.UserSettings["PCMLinks"].ToString());//[Sudhir.Jangra][GEOS2-1960][02/03/2023]

                GeosApplication.Instance.LabelSizeSettingForPrinter1 = GeosApplication.Instance.UserSettings["LabelSizeSettingForPrinter1"].ToString();
                GeosApplication.Instance.LabelSizeSettingForPrinter2 = GeosApplication.Instance.UserSettings["LabelSizeSettingForPrinter2"];
                //GeosApplication.Instance.SelectedPrinterForPrinter2 = GeosApplication.Instance.UserSettings["SelectedPrinterForPrinter2"].ToString();
                GeosApplication.Instance.LabelPrinterForPrinter2 = GeosApplication.Instance.UserSettings["LabelPrinterForPrinter2"];
                GeosApplication.Instance.LabelPrinterModelForPrinter2 = GeosApplication.Instance.UserSettings["LabelPrinterModelForPrinter2"];
                GeosApplication.Instance.ParallelPortForPrinter2 = GeosApplication.Instance.UserSettings["ParallelPortForPrinter2"];
            }

            GeosApplication.Instance.Logger.Log("Get user setting successfully.", category: Category.Info, priority: Priority.Low);

            //SetLanguageDictionary();
            //FillTheme();

            GeosApplication.Instance.Logger.Log("Get application setting ", category: Category.Info, priority: Priority.Low);
            // GeosApplication.Instance.ApplicationSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.ApplicationSettingFilePath);
            //string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;// application setting file path 

            //if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
            //{
            //    if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName))
            //        Directory.CreateDirectory(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);
            //    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);
            //    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
            //    GeosServiceProviderList = new List<GeosServiceProvider>();
            //    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

            //    UserConfigurationView objUserConfigurationView = new UserConfigurationView();

            //    UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
            //    // UserConfigurationViewModel.ListGeosServiceProviders  = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
            //    EventHandler handle = delegate { objUserConfigurationView.Close(); };
            //    UserConfigurationViewModel.RequestClose += handle;
            //    objUserConfigurationView.DataContext = UserConfigurationViewModel;
            //    objUserConfigurationView.ShowDialogWindow();

            //}
            //else
            //{
            //    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
            //    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
            //    GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();
            //    // GeosApplication.Instance.ServicePath = "localhost:6699";
            //    if (GeosApplication.Instance.ServicePath == null)
            //    {
            //        UserConfigurationView objUserConfigurationView = new UserConfigurationView();
            //        UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
            //        EventHandler handle = delegate { objUserConfigurationView.Close(); };
            //        UserConfigurationViewModel.RequestClose += handle;
            //        objUserConfigurationView.DataContext = UserConfigurationViewModel;
            //        objUserConfigurationView.ShowDialogWindow();
            //        GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();
            //    }

            //    if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
            //    {
            //        GeosApplication.Instance.ApplicationSettings["ServicePath"] = GeosApplication.Instance.ServicePath;
            //    }
            //    else
            //    {
            //        GeosApplication.Instance.ApplicationSettings.Add("ServicePath", GeosApplication.Instance.ServicePath);
            //    }

            //}

            GeosApplication.Instance.SiteName = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();

        }

        /// <summary>
        /// Method if there is some changes in application setting then create new one as per database.
        /// </summary>
        private void CreateApplicationFile()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile setting ", category: Category.Info, priority: Priority.Low);

                List<Company> CompanyList = objWorkbenchStartUp.GetCompanyList();

                List<Company> UnCommonCompanyList = new List<Company>();

                UnCommonCompanyList = CompanyList.Where(p => !GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(emp => p.Alias == emp.Name)).ToList();
                GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(g => CompanyList.Any(p => g.Name == p.Alias)).ToList();

                //List<Company> UnCommonCompanyList = CompanyList.Where(p => !GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(emp => p.Alias == emp.Name)).ToList();
                List<GeosProvider> geosProviderList = new List<GeosProvider>();
                List<GeosProvider> finalGeosProviderList = objWorkbenchStartUp.GetGeosProviderList();

                geosProviderList = finalGeosProviderList.Where(gs => UnCommonCompanyList.Any(unc => unc.IdCompany == gs.IdCompany)).ToList();
                int count = 0;

                if (geosProviderList != null && geosProviderList.Count > 0)
                {
                    foreach (var item in geosProviderList)
                    {
                        GeosServiceProvider geosServiceProvider = new GeosServiceProvider();
                        geosServiceProvider.Name = UnCommonCompanyList.Where(u => u.IdCompany == item.IdCompany).Select(i => i.Alias).FirstOrDefault();
                        geosServiceProvider.IsSelected = false;
                        geosServiceProvider.ServiceProviderUrl = UnCommonCompanyList.Where(u => u.IdCompany == item.IdCompany).Select(i => i.Alias).FirstOrDefault().ToString().ToLower() + "." + "emdep.com:" + item.ServiceServerPublicPort + "/WebServices";

                        GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Add(geosServiceProvider);
                        count = 1;
                    }
                }

                foreach (var itemGS in finalGeosProviderList.Where(p => GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(gs => gs.ServiceProviderUrl != p.ServiceProviderUrl)).ToList())
                {
                    GeosServiceProvider geosServiceProvider = new GeosServiceProvider();
                    geosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(gs => gs.Name == itemGS.Company.Alias).FirstOrDefault();
                    int indexof = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.FindIndex(gs => gs.Name == itemGS.Company.Alias);

                    GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider[indexof].ServiceProviderUrl = itemGS.ServiceProviderUrl;

                    count = 1;
                }

                if (count == 1)
                    GeosApplication.Instance.WriteApplicationSettingFile(GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider);

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile sussessfully ", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateApplicationFile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.ShowMessage(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);//[GEOS2-4012][24.04.2023][rdixit]
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateApplicationFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateApplicationFile() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for create User setting file after chnages verification.
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// [002][avpawar][GEOS2-2700]- 
        /// </summary>
        private void CreateUserSettingFileAfterVarification()
        {
            int elseCount = 0;
            GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

            //For Theme Name
            //[rdixit][GEOS2-9057][03.10.2025]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ThemeName"))
            {
                GeosApplication.Instance.UserSettings["ThemeName"] = "WhiteAndBlue";
                lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", "WhiteAndBlue"));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", "WhiteAndBlue"));
                elseCount++;
            }

            //For Language
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Language"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Language", GeosApplication.Instance.UserSettings["Language"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Language", Thread.CurrentThread.CurrentCulture.Name.ToString()));
                elseCount++;
            }

            //For Selected Module
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedModule"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedModule", GeosApplication.Instance.UserSettings["SelectedModule"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedModule", string.Empty));
                elseCount++;
            }

            //For Notification Page Count
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("NotificationPageCount"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("NotificationPageCount", GeosApplication.Instance.UserSettings["NotificationPageCount"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("NotificationPageCount", "10"));
                elseCount++;
            }

            //For Service Icon Show
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("IsServiceIconShow", GeosApplication.Instance.UserSettings["IsServiceIconShow"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("IsServiceIconShow", "False"));
                elseCount++;
            }

            //For Service Refresh Seconds
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ServiceRefreshSeconds", GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ServiceRefreshSeconds", "20"));
                elseCount++;
            }

            //For Offer Period
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferPeriod"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferPeriod", GeosApplication.Instance.UserSettings["CrmOfferPeriod"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferPeriod", DateTime.Now.Year.ToString()));
                elseCount++;
            }

            //For CustomPeriodOption
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CustomPeriodOption"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CustomPeriodOption", GeosApplication.Instance.UserSettings["CustomPeriodOption"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CustomPeriodOption", "0"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferFromInterval"))
            {
                try
                {
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferFromInterval", Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferFromInterval"]).ToString("yyyy/MM/dd")));
                }
                catch (Exception ex)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferFromInterval", new DateTime(Convert.ToInt32(DateTime.Now.Year.ToString()), 1, 1).ToString("yyyy/MM/dd")));
                    elseCount++;
                }
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferFromInterval", new DateTime(Convert.ToInt32(DateTime.Now.Year.ToString()), 1, 1).ToString("yyyy/MM/dd")));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmOfferToInterval"))
            {
                try
                {
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferToInterval", Convert.ToDateTime(GeosApplication.Instance.UserSettings["CrmOfferToInterval"]).ToString("yyyy/MM/dd")));
                }
                catch (Exception ex)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferToInterval", new DateTime(Convert.ToInt32(DateTime.Now.Year.ToString()), 12, 31).ToString("yyyy/MM/dd")));
                    elseCount++;
                }
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmOfferToInterval", new DateTime(Convert.ToInt32(DateTime.Now.Year.ToString()), 12, 31).ToString("yyyy/MM/dd")));
                elseCount++;
            }

            //For Top Offers
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmTopOffers"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmTopOffers", GeosApplication.Instance.UserSettings["CrmTopOffers"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmTopOffers", "10"));
                elseCount++;
            }

            //For Top Customers
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CrmTopCustomers"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmTopCustomers", GeosApplication.Instance.UserSettings["CrmTopCustomers"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CrmTopCustomers", "10"));
                elseCount++;
            }

            //For Crrency
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency", GeosApplication.Instance.UserSettings["SelectedCurrency"]));
            }
            else
            {
                var regionCulture = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency", regionCulture.ISOCurrencySymbol));
                elseCount++;
            }

            //For Crrency
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("UserSessionDetail"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("UserSessionDetail", GeosApplication.Instance.UserSettings["UserSessionDetail"]));
            }
            else
            {
                if (Workbench.Properties.Settings.Default.UserSessionId > 0)
                    lstUserConfiguration.Add(new Tuple<string, string>("UserSessionDetail", Workbench.Properties.Settings.Default.UserSessionId.ToString()));
                else
                    lstUserConfiguration.Add(new Tuple<string, string>("UserSessionDetail", string.Empty));

                elseCount++;
            }

            //For Auto Refresh
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AutoRefresh"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("AutoRefresh", GeosApplication.Instance.UserSettings["AutoRefresh"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("AutoRefresh", "Yes"));
                elseCount++;
            }

            //For Selected CRM Section LoadData
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCRMSectionLoadData"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCRMSectionLoadData", GeosApplication.Instance.UserSettings["SelectedCRMSectionLoadData"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCRMSectionLoadData", ""));
                elseCount++;
            }

            //For LoadDataOn
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LoadDataOn"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LoadDataOn", GeosApplication.Instance.UserSettings["LoadDataOn"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LoadDataOn", ""));
                elseCount++;
            }

            //For Current Profile
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("CurrentProfile"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CurrentProfile", GeosApplication.Instance.UserSettings["CurrentProfile"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("CurrentProfile", ""));
                elseCount++;
            }

            // Selected Warehouse Id
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedwarehouseId", GeosApplication.Instance.UserSettings["SelectedwarehouseId"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedwarehouseId", "0"));
                elseCount++;
            }

            //[002] Added
            // Label Size Setting for Printer 1
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelSizeSettingForPrinter1"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter1", GeosApplication.Instance.UserSettings["LabelSizeSettingForPrinter1"]));
            }
            else
            {
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter1", settings.PrinterName));

                elseCount++;
            }
            //[002] End

            // For Printer Setting
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPrinter"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", GeosApplication.Instance.UserSettings["SelectedPrinter"]));
            }
            else
            {
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", settings.PrinterName));

                elseCount++;
            }

            //LabelPrinter
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinter"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinter", GeosApplication.Instance.UserSettings["LabelPrinter"]));
            }
            else
            {
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinter", settings.PrinterName));

                elseCount++;
            }

            //LabelPrinterModel
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinterModel"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModel", GeosApplication.Instance.UserSettings["LabelPrinterModel"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModel", null));
                elseCount++;
            }

            //UseZebraPrinter
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("UseZebraPrinter"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("UseZebraPrinter", GeosApplication.Instance.UserSettings["UseZebraPrinter"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("UseZebraPrinter", null));
                elseCount++;
            }

            //ParallelPort
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ParallelPort"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPort", GeosApplication.Instance.UserSettings["ParallelPort"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPort", null));
                elseCount++;
            }

            //Login
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Login"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Login", GeosApplication.Instance.UserSettings["Login"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Login", "Office"));
                elseCount++;
            }

            //StartTimer
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PickingTimer"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PickingTimer", GeosApplication.Instance.UserSettings["PickingTimer"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PickingTimer", false.ToString()));
                elseCount++;
            }

            //ImportAttendanceDataSourceSelectedIndex
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceDataSourceSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", GeosApplication.Instance.UserSettings["ImportAttendanceDataSourceSelectedIndex"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceDataSourceSelectedIndex", "0"));
                elseCount++;
            }

            //ImportAttendanceTextSeparator
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceTextSeparator"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceTextSeparator", GeosApplication.Instance.UserSettings["ImportAttendanceTextSeparator"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceTextSeparator", "0"));
                elseCount++;
            }

            //ImportAttendanceOdbcDns
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceOdbcDns"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcDns", GeosApplication.Instance.UserSettings["ImportAttendanceOdbcDns"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcDns", "0"));
                elseCount++;
            }

            //ImportAttendanceOdbcTableName
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceOdbcTableName"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcTableName", GeosApplication.Instance.UserSettings["ImportAttendanceOdbcTableName"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceOdbcTableName", "0"));
                elseCount++;
            }

            //ImportAttendanceSourceFieldSelectedIndex
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendanceSourceFieldSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceSourceFieldSelectedIndex", GeosApplication.Instance.UserSettings["ImportAttendanceSourceFieldSelectedIndex"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendanceSourceFieldSelectedIndex", ""));
                elseCount++;
            }

            //ImportLeaveDataSourceSelectedIndex
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveDataSourceSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveDataSourceSelectedIndex", GeosApplication.Instance.UserSettings["ImportLeaveDataSourceSelectedIndex"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveDataSourceSelectedIndex", "0"));
                elseCount++;
            }

            //ImportLeaveTextSeparator
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveTextSeparator"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSeparator", GeosApplication.Instance.UserSettings["ImportLeaveTextSeparator"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSeparator", "0"));
                elseCount++;
            }

            //ImportLeaveOdbcDns
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcDns"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcDns", GeosApplication.Instance.UserSettings["ImportLeaveOdbcDns"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcDns", "0"));
                elseCount++;
            }

            //ImportLeaveOdbcTableName
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveOdbcTableName"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcTableName", GeosApplication.Instance.UserSettings["ImportLeaveOdbcTableName"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveOdbcTableName", "0"));
                elseCount++;
            }

            //ImportLeaveSourceFieldSelectedIndex
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveSourceFieldSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveSourceFieldSelectedIndex", GeosApplication.Instance.UserSettings["ImportLeaveSourceFieldSelectedIndex"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveSourceFieldSelectedIndex", ""));
                elseCount++;
            }

            //ImportLeaveTextSourceFieldSelectedIndex
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportLeaveTextSourceFieldSelectedIndex"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSourceFieldSelectedIndex", GeosApplication.Instance.UserSettings["ImportLeaveTextSourceFieldSelectedIndex"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportLeaveTextSourceFieldSelectedIndex", ""));
                elseCount++;
            }
            //SelectedScaleModel
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedScaleModel"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedScaleModel", GeosApplication.Instance.UserSettings["SelectedScaleModel"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedScaleModel", ""));
                elseCount++;
            }

            //SelectedPort
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPort"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPort", GeosApplication.Instance.UserSettings["SelectedPort"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPort", ""));
                elseCount++;
            }

            //SelectedParity
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedParity"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedParity", GeosApplication.Instance.UserSettings["SelectedParity"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedParity", ""));
                elseCount++;
            }

            //SelectedStopBit
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedStopBit"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedStopBit", GeosApplication.Instance.UserSettings["SelectedStopBit"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedStopBit", ""));
                elseCount++;
            }

            //SelectedBaudRate
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedBaudRate"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedBaudRate", GeosApplication.Instance.UserSettings["SelectedBaudRate"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedBaudRate", ""));
                elseCount++;
            }

            //SelectedBaudRate
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedDataBit"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedDataBit", GeosApplication.Instance.UserSettings["SelectedDataBit"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedDataBit", ""));
                elseCount++;
            }
            //[001] added
            //Appearance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Appearance", GeosApplication.Instance.UserSettings["Appearance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Appearance", ""));
                elseCount++;
            }

            //PCM_Appearance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_Appearance", GeosApplication.Instance.UserSettings["PCM_Appearance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_Appearance", ""));
                elseCount++;
            }

            //PCM_Images
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMImage"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMImage", GeosApplication.Instance.UserSettings["PCMImage"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMImage", "4"));
                elseCount++;
            }

            //PCM_Attachment
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMAttachment"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMAttachment", GeosApplication.Instance.UserSettings["PCMAttachment"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMAttachment", "4"));
                elseCount++;
            }
            //PCM_Links
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMLinks"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMLinks", GeosApplication.Instance.UserSettings["PCMLinks"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMLinks", "4"));
                elseCount++;
            }

            //PCM_SelectedCurrency
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_SelectedCurrency"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_SelectedCurrency", GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]));
            }
            else
            {
                var regionCulture_PCM = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                lstUserConfiguration.Add(new Tuple<string, string>("PCM_SelectedCurrency", regionCulture_PCM.ISOCurrencySymbol));
                elseCount++;
            }


            //PLM_Appearance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PLM_Appearance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PLM_Appearance", GeosApplication.Instance.UserSettings["PLM_Appearance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PLM_Appearance", ""));
                elseCount++;
            }

            //CRM shortcuts
            //For Opportunity
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Opportunity"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", GeosApplication.Instance.UserSettings["Opportunity"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", "Ctrl + O"));
                elseCount++;
            }

            //For Contact
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Contact"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Contact", GeosApplication.Instance.UserSettings["Contact"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Contact", "Ctrl + C"));
                elseCount++;
            }

            //For Account
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Account"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Account", GeosApplication.Instance.UserSettings["Account"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Account", "Ctrl + A"));
                elseCount++;
            }

            //For Appointment
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Appointment"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Appointment", GeosApplication.Instance.UserSettings["Appointment"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Appointment", "Ctrl + P"));
                elseCount++;
            }

            //For Call
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Call"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Call", GeosApplication.Instance.UserSettings["Call"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Call", "Ctrl + L"));
                elseCount++;
            }

            //For Task
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Task"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Task", GeosApplication.Instance.UserSettings["Task"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Task", "Ctrl + T"));
                elseCount++;
            }

            //For Email
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Email"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Email", GeosApplication.Instance.UserSettings["Email"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Email", "Ctrl + E"));
                elseCount++;
            }

            //For Action
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Action"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Action", GeosApplication.Instance.UserSettings["Action"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Action", "Ctrl + S"));
                elseCount++;
            }

            //For Search Opprtunity/Order
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchOpportunityOrOrder"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", "Ctrl + F"));
                elseCount++;
            }


            //For MatrixList
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("MatrixList"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("MatrixList", GeosApplication.Instance.UserSettings["MatrixList"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("MatrixList", "Alt + P"));
                elseCount++;
            }

            //HRM shortcuts     [sjadhav]
            //For Attendance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Attendance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Attendance", GeosApplication.Instance.UserSettings["Attendance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Attendance", "Ctrl + A"));
                elseCount++;
            }
            //Shubham[skadam] GEOS2-2597 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 1  03 02 2023
            //For Announcement
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Announcement"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Announcement", GeosApplication.Instance.UserSettings["Announcement"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Announcement", "Ctrl + S"));
                elseCount++;
            }

            //For Employee
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Employee"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Employee", GeosApplication.Instance.UserSettings["Employee"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Employee", "Ctrl + E"));
                elseCount++;
            }

            //For Holiday
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Holiday"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Holiday", GeosApplication.Instance.UserSettings["Holiday"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Holiday", "Ctrl + H"));
                elseCount++;
            }

            //For JobDescriptions
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("JobDescriptions"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("JobDescriptions", GeosApplication.Instance.UserSettings["JobDescriptions"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("JobDescriptions", "Ctrl + J"));
                elseCount++;
            }

            //For Leave
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Leave"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Leave", GeosApplication.Instance.UserSettings["Leave"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Leave", "Ctrl + L"));
                elseCount++;
            }

            //For Export AttendanceList
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("AttendanceList"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("AttendanceList", GeosApplication.Instance.UserSettings["AttendanceList"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("AttendanceList", "Alt + A"));
                elseCount++;
            }

            //For Export OrganizationChart
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("OrganizationChart"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("OrganizationChart", GeosApplication.Instance.UserSettings["OrganizationChart"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("OrganizationChart", "Alt + O"));
                elseCount++;
            }

            //For Import Attendance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImportAttendance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendance", GeosApplication.Instance.UserSettings["ImportAttendance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ImportAttendance", "Alt + T"));
                elseCount++;
            }

            //For Search Employee
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchEmployee"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchEmployee", GeosApplication.Instance.UserSettings["SearchEmployee"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchEmployee", "Alt + E"));
                elseCount++;
            }


            //For Currency warehouse
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]));
            }
            else
            {
                var regionCulture_Warehouse = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", regionCulture_Warehouse.ISOCurrencySymbol));
                elseCount++;
            }

            //end
            // Custom filter in TimeLine
            List<KeyValuePair<string, string>> CRMCustomFilterList = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains("CRM_Timeline_")).ToList();
            if (CRMCustomFilterList != null)
            {
                foreach (var item in CRMCustomFilterList)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(item.Key.ToString(), item.Value.ToString()));
                    //elseCount++;
                }
            }

            //Custom filter in Location
            List<KeyValuePair<string, string>> locationFilterList = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains("WMS_Location_")).ToList();
            foreach (var item in locationFilterList)
            {
                lstUserConfiguration.Add(new Tuple<string, string>(item.Key.ToString(), item.Value.ToString()));
                //elseCount++;
            }

            // Custom filter in Work Order
            List<KeyValuePair<string, string>> WMSCustomFilterList = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains("WMS_WorkOrder_")).ToList();
            if (WMSCustomFilterList != null)
            {
                foreach (var item in WMSCustomFilterList)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(item.Key.ToString(), item.Value.ToString()));
                    //elseCount++;
                }
            }

            //[002] start
            // Label Size Setting for Printer 2
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelSizeSettingForPrinter2"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter2", GeosApplication.Instance.UserSettings["LabelSizeSettingForPrinter2"]));
            }
            else
            {
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("LabelSizeSettingForPrinter2", settings.PrinterName));

                elseCount++;
            }

            // Selected Printer Setting for Printer 2
            //if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPrinterForPrinter2"))
            //{
            //    lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinterForPrinter2", GeosApplication.Instance.UserSettings["SelectedPrinterForPrinter2"]));
            //}
            //else
            //{
            //    PrinterSettings settings = new PrinterSettings();
            //    lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinterForPrinter2", settings.PrinterName));

            //    elseCount++;
            //}

            //LabelPrinter Setting for Printer 2
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinterForPrinter2"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterForPrinter2", GeosApplication.Instance.UserSettings["LabelPrinterForPrinter2"]));
            }
            else
            {
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterForPrinter2", settings.PrinterName));

                elseCount++;
            }

            //LabelPrinterModel Setting for Printer 2
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinterModelForPrinter2"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModelForPrinter2", GeosApplication.Instance.UserSettings["LabelPrinterModelForPrinter2"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("LabelPrinterModelForPrinter2", null));
                elseCount++;
            }

            //ParallelPort Setting for Printer 2
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ParallelPortForPrinter2"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPortForPrinter2", GeosApplication.Instance.UserSettings["ParallelPortForPrinter2"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ParallelPortForPrinter2", null));
                elseCount++;
            }
            //[002] End

            //SAM Setting
            //For AutoRefresh
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAMAutoRefresh"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefresh", GeosApplication.Instance.UserSettings["SAMAutoRefresh"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefresh", "Yes"));
                elseCount++;
            }

            //For AutoRefresh Interval
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAMAutoRefreshInterval"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefreshInterval", GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SAMAutoRefreshInterval", "5"));
                elseCount++;
            }

            //ERM_SelectedCurrency //[GEOS2-3997][Rupali Sarode][02-12-2022]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_SelectedCurrency", GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_SelectedCurrency", "EUR"));
                elseCount++;
            }


            //OTM_SelectedCurrency  // [pramod.misal][25-10-2024][GEOS2-6461]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("OTM_SelectedCurrency", GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("OTM_SelectedCurrency", "EUR"));
                elseCount++;
            }


            //ERM_Appearance   //[GEOS2- 4920][Aishwarya Ingale][30-11-2023]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_Appearance", GeosApplication.Instance.UserSettings["ERM_Appearance"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_Appearance", "Top"));
                elseCount++;
            }
            //ERMTimeTraking_IsFileDeleted   //[GEOS2- 5466][pallavi jadhav][02-04-2024]
            //if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERMTimeTraking_IsFileDeleted"))
            //{
            //    lstUserConfiguration.Add(new Tuple<string, string>("ERMTimeTraking_IsFileDeleted", GeosApplication.Instance.UserSettings["ERMTimeTraking_IsFileDeleted"]));
            //}
            //else
            //{
            //    lstUserConfiguration.Add(new Tuple<string, string>("ERMTimeTraking_IsFileDeleted", "0"));
            //    elseCount++;
            //}
            #region [GEOS2-6685][rani dhamankar][16-05-2025]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERMTimeTraking_IsFileDeleted_V2640"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERMTimeTraking_IsFileDeleted_V2640", GeosApplication.Instance.UserSettings["ERMTimeTraking_IsFileDeleted_V2640"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERMTimeTraking_IsFileDeleted_V2640", "0"));
                elseCount++;
            }
            #endregion
            #region [dhawal bhalerao][GEOS2-][15-05-2025]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640", GeosApplication.Instance.UserSettings["ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERM_CAM_CAD_TimeTraking_IsFileDeleted_V2640", "0"));
                elseCount++;
            }
            #endregion

            #region[pramod.misal][GEOS2-9196][08-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9196
            //For PoRequest
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2660"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2660", GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2660"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2660", "0"));
                elseCount++;
            }

            //For PoRequest Beta
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2660Beta"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2660Beta", GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2660Beta"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2660Beta", "0"));
                elseCount++;
            }

            //For PoRequest for V2.6.7.0 [pramod.misal][GEOS2-01-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9043]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2670"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2670", GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2670"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2670", "0"));
                elseCount++;
            }

            //For PoRequest for V2.6.8.0 [pramod.misal][GEOS2-01-09-2025]https://helpdesk.emdep.com/browse/GEOS2-9293
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PORequestesGridSetting_IsFileDeleted_V2680"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2680", GeosApplication.Instance.UserSettings["PORequestesGridSetting_IsFileDeleted_V2680"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORequestesGridSetting_IsFileDeleted_V2680", "0"));
                elseCount++;
            }

            //For Po registered
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PORegisteredGridSetting_IsFileDeleted_V2660"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORegisteredGridSetting_IsFileDeleted_V2660", GeosApplication.Instance.UserSettings["PORegisteredGridSetting_IsFileDeleted_V2660"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PORegisteredGridSetting_IsFileDeleted_V2660", "0"));
                elseCount++;
            }

            #endregion

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERMProductionTimeLine_IsFileDeleted_V2670")) //[GEOS2-9197][pallavi jadhav][29-08-2025]
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERMProductionTimeLine_IsFileDeleted_V2670", GeosApplication.Instance.UserSettings["ERMProductionTimeLine_IsFileDeleted_V2670"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ERMProductionTimeLine_IsFileDeleted_V2670", "0"));
                elseCount++;
            }

            // SCM_Docklayout_SettingFilePath //[GEOS2-5392][pramod.misal][18-09-2024]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SCM_Docklayout_SettingFilePath_V2570"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SCM_Docklayout_SettingFilePath_V2570", GeosApplication.Instance.UserSettings["SCM_Docklayout_SettingFilePath_V2570"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SCM_Docklayout_SettingFilePath_V2570", "0"));
                elseCount++;
            }
            //APMActionPlan_IsFileDeleted_V2570 //[Shweta.Thube][GEOS2-5981][01-10-2024]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlan_IsFileDeleted_V2570"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlan_IsFileDeleted_V2570", GeosApplication.Instance.UserSettings["APMActionPlan_IsFileDeleted_V2570"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlan_IsFileDeleted_V2570", "0"));
                elseCount++;
            }
            #region To Delete Old version file [rdixit][GEOS2-6574][30.12.2023] 
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMModuleDetectionTemplate_V2590"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMModuleDetectionTemplate_V2590", GeosApplication.Instance.UserSettings["PCMModuleDetectionTemplate_V2590"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMModuleDetectionTemplate_V2590", "0"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMStructureTemplate_V2590"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMStructureTemplate_V2590", GeosApplication.Instance.UserSettings["PCMStructureTemplate_V2590"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMStructureTemplate_V2590", "0"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMModuleTemplate_V2590"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMModuleTemplate_V2590", GeosApplication.Instance.UserSettings["PCMModuleTemplate_V2590"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("PCMModuleTemplate_V2590", "0"));
                elseCount++;
            }
            #endregion
            //For create user file on condition.

            //APM Action plan child Grid Control [Sudhir.jangra]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlanChild_IsFileDeleted_V2590"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlanChild_IsFileDeleted_V2590", GeosApplication.Instance.UserSettings["APMActionPlanChild_IsFileDeleted_V2590"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlanChild_IsFileDeleted_V2590", "0"));
                elseCount++;
            }

            //APM Action plan Task Grid Control [Sudhir.jangra]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlanTask_IsFileDeleted_V2620"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlanTask_IsFileDeleted_V2620", GeosApplication.Instance.UserSettings["APMActionPlanTask_IsFileDeleted_V2620"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("APMActionPlanTask_IsFileDeleted_V2620", "0"));
                elseCount++;
            }
            //SCM My Preferences [shweta.thube][GEOS2-6630]
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedSCMSectionLoadData"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedSCMSectionLoadData", GeosApplication.Instance.UserSettings["SelectedSCMSectionLoadData"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedSCMSectionLoadData", ""));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Create"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Create", GeosApplication.Instance.UserSettings["Create"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Create", "Shift + C"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Search"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Search", GeosApplication.Instance.UserSettings["Search"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Search", "Shift + S"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Locations", GeosApplication.Instance.UserSettings["Locations"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Locations", "Shift + L"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Properties", GeosApplication.Instance.UserSettings["Properties"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Properties", "Shift + P"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Families"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Families", GeosApplication.Instance.UserSettings["Families"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Families", "Shift + F"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchManager", GeosApplication.Instance.UserSettings["SearchManager"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("SearchManager", "Shift + E"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("NewSamples", GeosApplication.Instance.UserSettings["NewSamples"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("NewSamples", "Shift + N"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ModifiedSamples", GeosApplication.Instance.UserSettings["ModifiedSamples"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ModifiedSamples", "Shift + M"));
                elseCount++;
            }

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Connectors3D", GeosApplication.Instance.UserSettings["Connectors3D"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("Connectors3D", "Ctrl + O"));
                elseCount++;
            }
            if (elseCount > 0)
            {
                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            }

        }

        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //GeosApplication.Instance.Currencies = crmControl.GetCurrencies();              
                //ICrmService crmControl = new CrmServiceController("localhost:6699");
                // [pramod.misal][25-10-2024][GEOS2-6461]
                GeosApplication.Instance.Currencies = crmControl.GetCurrencies_V2580();
                GeosApplication.Instance.IdCurrencyByRegion = GeosApplication.Instance.Currencies.Where(currency => currency.Name.ToString() == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(curr => curr.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();

                if (GeosApplication.Instance.IdCurrencyByRegion == 0)
                {
                    GeosApplication.Instance.IdCurrencyByRegion = 1;
                    GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion).Select(cur => cur.Symbol).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }
        //[rdixit][04.12.2023][GEOS2-4897]
        public void FillPCMCurrencyDetails()
        {
            try
            {
                ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //GeosApplication.Instance.Currencies = crmControl.GetCurrencies();
                //ICrmService crmControl = new CrmServiceController("localhost:6699");
                // [pramod.misal][25-10-2024][GEOS2-6461]
                GeosApplication.Instance.Currencies = crmControl.GetCurrencies_V2580();
                GeosApplication.Instance.PCMCurrentCurrency = GeosApplication.Instance.Currencies.FirstOrDefault(currency => currency.Name.ToString() == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"].ToString());
                if (GeosApplication.Instance.PCMCurrentCurrency == null)
                {
                    GeosApplication.Instance.PCMCurrentCurrency = GeosApplication.Instance.Currencies.Where(i => i.IdCurrency == 1).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillWarehouseCurrencyDetails()
        {
            try
            {
                ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //GeosApplication.Instance.Currencies = crmControl.GetCurrencies();
                //ICrmService crmControl = new CrmServiceController("localhost:6699");
                // [pramod.misal][25-10-2024][GEOS2-6461]
                GeosApplication.Instance.Currencies = crmControl.GetCurrencies_V2580();
                GeosApplication.Instance.IdWMSCurrencyByRegion = GeosApplication.Instance.Currencies.Where(currency => currency.Name.ToString() == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"].ToString()).Select(curr => curr.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.WMSCurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();

                if (GeosApplication.Instance.IdWMSCurrencyByRegion == 0)
                {
                    GeosApplication.Instance.IdWMSCurrencyByRegion = 1;
                    GeosApplication.Instance.WMSCurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.IdCurrency == GeosApplication.Instance.IdWMSCurrencyByRegion).Select(cur => cur.Symbol).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        public void FillTheme()
        {
            // #region Theme and Language

            //Themes.BlackAndBlue.v19.2
            Theme theme = new Theme("BlackAndBlue", "DevExpress.Xpf.Themes.BlackAndBlue.v19.2");
            theme.AssemblyName = "DevExpress.Xpf.Themes.BlackAndBlue.v19.2";
            Theme.RegisterTheme(theme);

            //Themes.WhiteAndBlue.v19.2
            Theme themeWhiteAndBlue = new Theme("WhiteAndBlue", "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2");
            themeWhiteAndBlue.AssemblyName = "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2";
            Theme.RegisterTheme(themeWhiteAndBlue);

            ApplicationThemeHelper.ApplicationThemeName = "WhiteAndBlue";

            //[rdixit][GEOS2-9057][03.10.2025]
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("/GeosWorkbench;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
            //if (GeosApplication.Instance.UserSettings.ContainsKey("ThemeName") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ThemeName"].ToString()))
            //{
            //    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
            //    {
            //        ApplicationThemeHelper.ApplicationThemeName = GeosApplication.Instance.UserSettings["ThemeName"].ToString();
            //        dict.Source = new Uri("/GeosWorkbench;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
            //    }
            //    else
            //    {
            //        ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
            //        dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);

            //    }
            //}
            //else
            //{
            //    ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
            //    dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
            //}
            this.Resources.MergedDictionaries.Add(dict);
        }
        // Method to encode a string to Base64
        public static string EncodeToBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        // Method to decode a Base64 string to the original string
        public static string DecodeFromBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
