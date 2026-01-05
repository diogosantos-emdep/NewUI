using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Emdep.Geos.Workbench.Downloader.ViewModels;
using Emdep.Geos.Workbench.Installer.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.UI.Adapters.Logging;
using System.Net.NetworkInformation;
using System.Net;
using System.Xml.Serialization;
using System.Text;
using Emdep.Geos.Utility.Text;
using System.Reflection;
using System.Security.Principal;
using Emdep.Geos.UI.CustomControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;

namespace Emdep.Geos.Workbench.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GeosWorkbenchVersion geosWorkbenchVersionNumber;

        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
        }
        //private GeosServiceProviders geosServiceProviders;

        //public GeosServiceProviders GeosServiceProviders
        //{
        //    get { return geosServiceProviders; }
        //    set { geosServiceProviders = value; }
        //}
        private bool isNetworkIP;
        public bool IsNetworkIP
        {
            get
            {
                return isNetworkIP;
            }

            set
            {
                isNetworkIP = value;
            }
        }
        public List<GeosServiceProvider> GeosServiceProviderList { get; set; }
        public List<GeosServiceProvider> GeosServiceProvideroldList { get; set; }
        string myIP = ApplicationOperation.GetEmdepGroupIP("10.0.");

        //List<string> PublicNetworkIPList = new List<string>();

        //List<string> NetworkIPList = new List<string>();
        public string FontFamilyAsPerTheme { get; set; }

        private ObservableCollection<GeosReleaseNote> listReleaseNotes; //list of ReleaseNotes
        private ObservableCollection<string> listGeosServiceProviders;
        public ObservableCollection<GeosReleaseNote> ReleaseNotesList
        {
            get { return listReleaseNotes; }
            set
            {
                listReleaseNotes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNotesList"));
            }
        }
        private int selectedViewIndex = 0;
        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }
        int releaseNoteType = 0;
        public int ReleaseNoteType
        {
            get
            {
                return releaseNoteType;
            }

            set
            {
                releaseNoteType = value; OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNoteType"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private void GetApplicationSettings()
        {

            GeosApplication.Instance.ApplicationSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.ApplicationSettingFilePath);

        }


        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {

            //if (IsAdministrator())
            //{
            try
            {
                //MessageBox.Show("I am In StartUp");
                bool isworkbench = false;

                //bool IsNetworkIP = false;
                if (e.Args.Length == 1 && e.Args[0] == "1")
                {
                    isworkbench = true;
                }

                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                // string ThemeName = GeosApplication.Instance.UserSettings["ThemeName"].ToString();
                //set Language 
                //MessageBox.Show("Setting up Language Dictionary");
                SetLanguageDictionary();
                //MessageBox.Show("SetLanguageDictionary Completed");
                #region Theme and Language

                Theme themeBlackAndBlue = new Theme("BlackAndBlue", "DevExpress.Xpf.Themes.BlackAndBlue.v19.2");
                themeBlackAndBlue.AssemblyName = "DevExpress.Xpf.Themes.BlackAndBlue.v19.2";
                Theme.RegisterTheme(themeBlackAndBlue);

                Theme themeWhiteAndBlue = new Theme("WhiteAndBlue", "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2");
                themeWhiteAndBlue.AssemblyName = "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2";
                Theme.RegisterTheme(themeWhiteAndBlue);
                //ThemeManager.ApplicationThemeName = "BlackAndBlue";
                //ResourceDictionary dict = new ResourceDictionary();

                //if (!string.IsNullOrEmpty(ThemeName) && ThemeName == "WhiteAndBlue")
                //{
                //    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
                //}
                //else
                //{
                //    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                //}
                //this.Resources.MergedDictionaries.Add(dict);

                ThemeManager.ApplicationThemeName = "BlackAndBlue";

                ResourceDictionary dict = new ResourceDictionary();
                if (GeosApplication.Instance.UserSettings.ContainsKey("ThemeName") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ThemeName"].ToString()))
                {
                    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                    {
                        ThemeManager.ApplicationThemeName = GeosApplication.Instance.UserSettings["ThemeName"].ToString();
                        dict.Source = new Uri("/GeosWorkbenchInstaller;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
                    }
                    else
                    {

                        ThemeManager.ApplicationThemeName = "BlackAndBlue";
                        dict.Source = new Uri("/GeosWorkbenchInstaller;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);

                    }
                }
                else
                {
                    ThemeManager.ApplicationThemeName = "BlackAndBlue";
                    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                }
                this.Resources.MergedDictionaries.Add(dict);


                string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;


                if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
                {
                    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);


                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                }
                FileInfo logfile = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
                GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, logfile);



                //string logfilepath = GeosApplication.Instance.ApplicationLogFilePath; //log File path
                //if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
                //{
                //    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                //        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                //    File.Copy(Directory.GetCurrentDirectory() + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                //}
                //   FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
                //GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                string Appfilepath = GeosApplication.Instance.ApplicationSettingFilePath;// application setting file path

                string s = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Emdep\Geos\";
                // MessageBox.Show(s + " File path exist");
                if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
                {

                    if (!Directory.Exists(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(GeosApplication.Instance.ApplicationSettingFilePath).FullName);

                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName, Appfilepath);

                    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
                    GeosServiceProviderList = new List<GeosServiceProvider>();
                    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

                }
                else
                {


                    GeosApplication.Instance.GeosServiceProvidersold = GeosApplication.Instance.GetGeosServiceProviders(GeosApplication.Instance.ApplicationSettingFilePath);
                    GeosServiceProvideroldList = new List<GeosServiceProvider>();
                    GeosServiceProvideroldList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                    GeosServiceProvider objGeosServiceProvider = GeosServiceProvideroldList.Where(x => x.IsSelected == true).FirstOrDefault();


                    GeosApplication.Instance.GeosServiceProviders = GeosApplication.Instance.GetGeosServiceProviders(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationSettingFileName);
                    GeosServiceProviderList = new List<GeosServiceProvider>();
                    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                    if (objGeosServiceProvider != null)
                    {
                        foreach (var item in GeosServiceProviderList)
                        {
                            if (item.Name == objGeosServiceProvider.Name)
                            {
                                item.IsSelected = true;
                            }
                            else
                            {
                                item.IsSelected = false;
                            }

                        }

                        GeosApplication.Instance.GeosServiceProviderList = GeosServiceProviderList;
                        GeosApplication.Instance.WriteApplicationSettingFile(GeosServiceProviderList);
                    }
                    else
                    {
                        VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders.Insert(0, "---");
                        objCheckForVersionWindowViewModel.SelectedIndexGeosServiceProviders = "---";
                        objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                        VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                        objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;
                        if (DXSplashScreen.IsActive)
                        {
                            DXSplashScreen.Close();
                        }
                        objCheckForVersionWindow.ShowDialog();
                    }
                }

                //  GeosApplication.Instance.ApplicationSettingFilePath

                DXSplashScreen.Show<SplashScreenView>();
                #endregion
                #region GeosServiceProviders


                bool IsSelectedServiceProvider = GeosServiceProviderList.Any(serviceProvider => serviceProvider.IsSelected == true);// check is ServiceProvider is selected 
                if (IsSelectedServiceProvider == false)// it is selected false
                {
                    try
                    {

                        VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders.Insert(0, "---");
                        objCheckForVersionWindowViewModel.SelectedIndexGeosServiceProviders = "---";
                        objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                        VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                        objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;
                        if (DXSplashScreen.IsActive)
                        {
                            DXSplashScreen.Close();
                        }
                        objCheckForVersionWindow.ShowDialog();


                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive)
                        {
                            DXSplashScreen.Close();
                        }
                        VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(l => l.Name).ToList());
                        objCheckForVersionWindowViewModel.SelectedIndexGeosServiceProviders = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();
                        objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                        objCheckForVersionWindowViewModel.ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                        VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                        objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;

                        objCheckForVersionWindow.ShowDialog();

                    }

                }

                else
                {
                    GeosServiceProvider objGeosServiceProvider1 = GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.IsSelected == true).FirstOrDefault();
                    //GeosApplication.Instance.ApplicationSettings.Add("ServiceProviderIP", string.Empty);
                    //GeosApplication.Instance.ApplicationSettings.Add("ServiceProviderPort", string.Empty);
                    GeosApplication.Instance.ApplicationSettings.Add("ServicePath", objGeosServiceProvider1.ServiceProviderUrl);
                    try
                    {
                        //IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderUrl"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString(), GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        GeosApplication.Instance.ServerDateTime = workbenchControl.GetServerDateTime();

                        GeosApplication.Instance.UIThemeList = workbenchControl.GetAllThemes();
                        FontFamilyAsPerTheme = GeosApplication.Instance.UIThemeList.Where(uithe => uithe.ThemeName == ThemeManager.ApplicationThemeName).Select(ui => ui.FontFamily).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive)
                        {
                            DXSplashScreen.Close();
                        }
                        VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                        objCheckForVersionWindowViewModel.ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                        objCheckForVersionWindowViewModel.SelectedIndexGeosServiceProviders = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();
                        objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                        objCheckForVersionWindowViewModel.ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                        VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                        objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;

                        objCheckForVersionWindow.ShowDialog();

                    }
                }




                #endregion



                #region Display version information

                string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

                if (!File.Exists(GeosApplication.Instance.ApplicationSettingFilePath))
                {

                    //VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                    //objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                    //objCheckForVersionWindowViewModel.ProgressbarVisibility = Visibility.Hidden;
                    //objCheckForVersionWindowViewModel.DoneButtonVisibility = Visibility.Hidden;

                    //VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                    //objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;

                    //if (DXSplashScreen.IsActive)
                    //    DXSplashScreen.Close();

                    //objCheckForVersionWindow.ShowDialog();

                }
                else
                {
                    try
                    {
                        //GetApplicationSettings();
                        FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);

                        GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);

                        GeosApplication.Instance.Logger.Log("Message = Updater Application Start...,Path=" + GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString() + "", category: Category.Info, priority: Priority.Low);
                        IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        CreateApplicationFile(control);
                        string value = null;
                        try
                        {
                            value = control.GetWorkbenchInstallerVersion();
                        }
                        catch (Exception)
                        {

                        }
                        if (value != null)
                        {
                            if (AssemblyVersion.CompareAssemblyVersions(new Version(value), AssemblyVersion.GetAssemblyVersion(System.Reflection.Assembly.GetExecutingAssembly().Location)) != 0)
                            {
                                if (DXSplashScreen.IsActive)
                                {
                                    DXSplashScreen.Close();
                                }

                                this.Shutdown();
                                Environment.Exit(0);
                            }

                            Version installedversion = AssemblyVersion.GetAssemblyVersion(path);
                            GeosWorkbenchVersionNumber = control.GetLatestVersion();

                            Version letestversion = AssemblyVersion.GetAssemblyVersionbyString(GeosWorkbenchVersionNumber.VersionNumber);
                            GeosApplication.Instance.GeosWorkbenchVersion = GeosWorkbenchVersionNumber;
                            int CheckForVersionWindowshow = AssemblyVersion.CompareAssemblyVersions(installedversion, letestversion);


                            if (CheckForVersionWindowshow != 0)
                            {
                                if (DXSplashScreen.IsActive)
                                {
                                    DXSplashScreen.Close();
                                }
                                VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);
                                objCheckForVersionWindowViewModel.ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                                objCheckForVersionWindowViewModel.SelectedIndexGeosServiceProviders = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderName => serviceProviderName.Name).FirstOrDefault();
                                objCheckForVersionWindowViewModel.SelectedViewIndex = 0;
                                objCheckForVersionWindowViewModel.ProgressbarVisibility = Visibility.Hidden;
                                objCheckForVersionWindowViewModel.DoneButtonVisibility = Visibility.Hidden;
                                objCheckForVersionWindowViewModel.ReleaseNotesVisibility = Visibility.Visible;
                                VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                                objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;


                                objCheckForVersionWindow.ShowDialog();
                            }
                        }
                        else
                        {

                            if (DXSplashScreen.IsActive)
                            {
                                DXSplashScreen.Close();
                            }
                            VersionUpdateWindowViewModel objCheckForVersionWindowViewModel = new VersionUpdateWindowViewModel(isworkbench);

                            objCheckForVersionWindowViewModel.ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"];

                            if (!string.IsNullOrEmpty(GeosApplication.Instance.ApplicationSettings["ServicePath"]))
                                //objCheckForVersionWindowViewModel.Port = decimal.Parse(GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"]);
                                objCheckForVersionWindowViewModel.ListGeosServiceProviders
                                    = new System.Collections.ObjectModel.ObservableCollection<string>(GeosServiceProviderList.Select(serviceProviderName => serviceProviderName.Name).ToList());
                            objCheckForVersionWindowViewModel.SelectedViewIndex = 3;
                            objCheckForVersionWindowViewModel.ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

                            //VersionUpdateWindow objCheckForVersionWindow = new VersionUpdateWindow();
                            //objCheckForVersionWindow.DataContext = objCheckForVersionWindowViewModel;
                            //objCheckForVersionWindow.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {

                        if (DXSplashScreen.IsActive)
                            DXSplashScreen.Close();


                    }
                }
                if (DXSplashScreen.IsActive)
                    DXSplashScreen.Close();
                #endregion
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                    DXSplashScreen.Close();
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================

            }
            finally
            {
                Environment.ExitCode = 110;
                this.Shutdown();
                // MessageBox.Show(" Reached Finally");
            }
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
            //}
            //else
            //{
            //    MessageBox.Show("Run a program with Administrator","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            //    //CustomMessageBox.Show("Run a program with Administrator", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //}
        }

        /// <summary>
        /// Method if there is some changes in application setting then create new one as per database.
        /// </summary>
        private void CreateApplicationFile(IWorkbenchStartUp control)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile setting ", category: Category.Info, priority: Priority.Low);

                int count = 0;
                List<GeosProvider> geosProviderList = new List<GeosProvider>();

                List<Company> CompanyList = control.GetCompanyList();

                List<Company> UnCommonCompanyList = new List<Company>();

                UnCommonCompanyList = CompanyList.Where(p => !GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(emp => p.Alias == emp.Name)).ToList();
                GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(g => CompanyList.Any(p => g.Name == p.Alias)).ToList();
                // List<Company> UnCommonCompanyList = CompanyList.Where(p => !GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(emp => p.Alias == emp.Name)).ToList();

                List<GeosProvider> finalGeosProviderList = control.GetGeosProviderList();

                geosProviderList = finalGeosProviderList.Where(gs => UnCommonCompanyList.Any(unc => unc.IdCompany == gs.IdCompany)).ToList();

                geosProviderList = geosProviderList.Where(gs => UnCommonCompanyList.Any(unc => unc.IdCompany == gs.IdCompany)).ToList();

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

                GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile successfully ", category: Category.Info, priority: Priority.Low);
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
        /// Set application language 
        /// </summary>
        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            string lang = "";

            if (GeosApplication.Instance.UserSettings.ContainsKey("Language") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Language"].ToString()))
            {
                lang = GeosApplication.Instance.UserSettings["Language"].ToString();

                try
                {
                    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Resources/Language." + lang + ".xaml", UriKind.Relative);
                }
                catch (Exception)
                {
                    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Resources/Language.xaml", UriKind.Relative);

                }
            }

            else
            {
                lang = Thread.CurrentThread.CurrentCulture.ToString();
                try
                {
                    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Resources/Language." + lang + ".xaml", UriKind.Relative);
                }
                catch (Exception)
                {
                    dict.Source = new Uri("/GeosWorkbenchInstaller;component/Resources/Language.xaml", UriKind.Relative);

                }

            }


            this.Resources.MergedDictionaries.Add(dict);
        }

    }
}

