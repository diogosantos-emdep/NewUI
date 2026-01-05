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
                GeosApplication.Instance.Logger.Log("Workbench Application Start...", category: Category.Info, priority: Priority.Low);

                GetApplicationSettings();
                FillCurrencyDetails();
                FillWarehouseCurrencyDetails();
                #region Display Login

                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

             CreateApplicationFile();

                try
                {
                    GeosApplication.Instance.UIThemeList = objWorkbenchStartUp.GetAllThemes();
                    GeosApplication.Instance.FontFamilyAsPerTheme = new FontFamily(GeosApplication.Instance.UIThemeList.Where(uithe => uithe.ThemeName == ApplicationThemeHelper.ApplicationThemeName).Select(ui => ui.FontFamily).FirstOrDefault());
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    CustomMessageBox.Show(Workbench.App.Current.Resources["WorkbenchServiceError"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in Application_Startup() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    int CheckForVersionWindowshow = 0;

                    if (latestversion != null)
                        CheckForVersionWindowshow = AssemblyVersion.CompareAssemblyVersions(latestversion, installedversion);

                    GeosApplication.Instance.Logger.Log("Check For Version Windowshow", category: Category.Info, priority: Priority.Low);

                    //        if (CheckForVersionWindowshow > 0)
                    //        {
                    //            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkbenchUpdate"].ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    //            if (MessageBoxResult == MessageBoxResult.Yes)
                    //            {
                    //                if (File.Exists(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe"))
                    //                {
                    //                    ProcessControl.ProcessStart(updateInstalledFolder + @"\GeosWorkbenchInstaller.exe", "1", "");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                try
                    //                {
                    //                    DXSplashScreen.Show<MainSplashScreenView>();
                    //                    ShowWorkbench();
                    //                }
                    //                catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                    //                {
                    //                    GeosApplication.Instance.Logger.Log("Get an error On Application_Startup Method", category: Category.Info, priority: Priority.Low);
                    //                    if (DXSplashScreen.IsActive)
                    //                    {
                    //                        DXSplashScreen.Close();
                    //                    }
                    //                }
                    //            }
                    //            Environment.Exit(0);
                    //        }
                    //        else
                    //        {
                    //            DXSplashScreen.Show<MainSplashScreenView>();
                    //            ShowWorkbench();
                    //        }
                    //        Application.Current.Shutdown();
                    //    }
                    //    else
                    //    {
                    //        DXSplashScreen.Show<MainSplashScreenView>();
                    //        ShowWorkbench();
                    //    }
                    //}

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
                                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkbenchUpdateExpiryDate"].ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
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
                        DXSplashScreen.Show<MainSplashScreenView>();
                        ShowWorkbench();
                    }

                    Application.Current.Shutdown();
                }
                else
                {
                    DXSplashScreen.Show<MainSplashScreenView>();
                    ShowWorkbench();
                }
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error On Application_Startup Method", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            finally
            {
                GeosApplication.Instance.Logger.Log("End Application_Startup Method", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Application.Current.Shutdown();
            }

            #endregion

            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        }

        private void VersionDifferentDownLoad(string updateInstalledFolder)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkbenchUpdate"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

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
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                WorkbenchWindow loginSuccess = new WorkbenchWindow();
                LoginWindow loginWindow = new LoginWindow();
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
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (GeosWorkbenchVersionNumber != null && GeosWorkbenchVersionNumber.IsBeta == 1)
                {
                    loginWindowViewModel.VersionTitle = "    V " + installedversion + " (Beta)";
                }
                else
                {
                    loginWindowViewModel.VersionTitle = "    V " + installedversion;
                }

                //[start is userremenber check]
                if (!string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["UserSessionDetail"].ToString()))
                {
                    int _idUser;
                    bool isNumeric = int.TryParse(GeosApplication.Instance.UserSettings["UserSessionDetail"], out _idUser);

                    if (isNumeric)
                    {
                        User user = new User();
                        user = objWorkbenchStartUp.GetUserById(_idUser);

                        if (user != null && user.IsEnabled != null && user.IsEnabled == 1)
                        {
                            List<Company> companysuserwise = objWorkbenchStartUp.GetAllCompanyByUserId(user.IdUser);
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

                            userEntry = objWorkbenchStartUp.AddUserLoginEntry(userEntry);
                        }
                        else
                        {
                            Workbench.Properties.Settings.Default.UserSessionId = 0;
                            Workbench.Properties.Settings.Default.Save();
                        }
                    }
                }

                //[End is userremenber check]

                if (Workbench.Properties.Settings.Default.UserSessionId > 0)
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Getting User detail By Id ", category: Category.Info, priority: Priority.Low);
                        User user = new User();
                        user = objWorkbenchStartUp.GetUserById(Workbench.Properties.Settings.Default.UserSessionId);

                        if (user != null && user.IsEnabled != null && user.IsEnabled == 1)
                        {
                            GeosApplication.Instance.Logger.Log("Getting User detail By Id Successfully", category: Category.Info, priority: Priority.Low);

                            if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["UserSessionDetail"].ToString()))
                            {
                                // code for user login entry
                                string hostName = Dns.GetHostName();                                    // Retrive the Name of HOST  
                                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP

                                UserLoginEntry userEntry = new UserLoginEntry();
                                userEntry.IdUser = user.IdUser;
                                userEntry.IpAddress = myIP;
                                userEntry.LoginTime = DateTime.Now;
                                userEntry.LogoutTime = null;
                                userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;

                                userEntry = objWorkbenchStartUp.AddUserLoginEntry(userEntry);
                            }

                            GeosApplication.Instance.ActiveUser = user;
                            SetUserPermission();
                            GeosApplication.Instance.Logger.Log("Initialising workbench window", category: Category.Info, priority: Priority.Low);
                            WorkbenchWindowViewModel workbenchWindowViewModel = new WorkbenchWindowViewModel();
                            workbenchWindowViewModel.LoginWorkstation = workstation;
                            EventHandler handle = delegate { loginSuccess.Close(); };
                            workbenchWindowViewModel.RequestClose += handle;
                            loginSuccess.DataContext = workbenchWindowViewModel;

                            //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Initialising workbench window Successfully", category: Category.Info, priority: Priority.Low);
                            loginSuccess.ShowDialog();
                        }

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in ShowWorkbench() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error On ShowWorkbench Method", category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.PlantOwnerUsersList = CrmStartUp.GetAllCompaniesDetails_V2040(GeosApplication.Instance.ActiveUser.IdUser);
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
            // End (PlantOwner)
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
        /// </summary>
        public void GetApplicationSettings()
        {
            GeosApplication.Instance.Logger.Log("Get user setting ", category: Category.Info, priority: Priority.Low);

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
                PrinterSettings settings = new PrinterSettings();
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedPrinter", settings.PrinterName));
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
                lstUserConfiguration.Add(new Tuple<string, string>("Opportunity", "Ctrl + O"));
                lstUserConfiguration.Add(new Tuple<string, string>("Contact", "Ctrl + C"));
                lstUserConfiguration.Add(new Tuple<string, string>("Account", "Ctrl + A"));
                lstUserConfiguration.Add(new Tuple<string, string>("Appointment", "Ctrl + P"));
                lstUserConfiguration.Add(new Tuple<string, string>("Call", "Ctrl + L"));
                lstUserConfiguration.Add(new Tuple<string, string>("Task", "Ctrl + T"));
                lstUserConfiguration.Add(new Tuple<string, string>("Email", "Ctrl + E"));
                lstUserConfiguration.Add(new Tuple<string, string>("Action", "Ctrl + S"));
                lstUserConfiguration.Add(new Tuple<string, string>("SearchOpportunityOrOrder", "Ctrl + F"));

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

                var regionCulture_warehouse = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);
                lstUserConfiguration.Add(new Tuple<string, string>("SelectedCurrency_Warehouse", regionCulture_warehouse.ISOCurrencySymbol));


                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                GeosApplication.Instance.CrmOfferYear = DateTime.Now.Year;
                GeosApplication.Instance.CrmTopCustomers = 10;
                GeosApplication.Instance.CrmTopOffers = 10;
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
            }

            GeosApplication.Instance.Logger.Log("Get user setting successfully.", category: Category.Info, priority: Priority.Low);

            SetLanguageDictionary();
            FillTheme();

            GeosApplication.Instance.Logger.Log("Get application setting ", category: Category.Info, priority: Priority.Low);
            // GeosApplication.Instance.ApplicationSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.ApplicationSettingFilePath);
            string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;// application setting file path 

            if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
            {
                if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);
                File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);
                GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
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
                GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
                GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();

                if (GeosApplication.Instance.ServicePath == null)
                {
                    UserConfigurationView objUserConfigurationView = new UserConfigurationView();
                    UserConfigurationViewModel UserConfigurationViewModel = new UserConfigurationViewModel();
                    EventHandler handle = delegate { objUserConfigurationView.Close(); };
                    UserConfigurationViewModel.RequestClose += handle;
                    objUserConfigurationView.DataContext = UserConfigurationViewModel;
                    objUserConfigurationView.ShowDialogWindow();
                    GeosApplication.Instance.ServicePath = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetwork => serviceProviderPrivateNetwork.ServiceProviderUrl).FirstOrDefault();
                }

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
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
        /// </summary>
        private void CreateUserSettingFileAfterVarification()
        {
            int elseCount = 0;
            GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

            //For Theme Name
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ThemeName"))
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", GeosApplication.Instance.UserSettings["ThemeName"]));
            }
            else
            {
                lstUserConfiguration.Add(new Tuple<string, string>("ThemeName", "BlackAndBlue"));
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

            //For create user file on condition.
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
                GeosApplication.Instance.Currencies = crmControl.GetCurrencies();
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

        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillWarehouseCurrencyDetails()
        {
            try
            {
                ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.Currencies = crmControl.GetCurrencies();
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

            ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";

            ResourceDictionary dict = new ResourceDictionary();
            if (GeosApplication.Instance.UserSettings.ContainsKey("ThemeName") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ThemeName"].ToString()))
            {
                if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                {
                    ApplicationThemeHelper.ApplicationThemeName = GeosApplication.Instance.UserSettings["ThemeName"].ToString();
                    dict.Source = new Uri("/GeosWorkbench;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
                }
                else
                {
                    ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
                    dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);

                }
            }
            else
            {
                ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
                dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}
