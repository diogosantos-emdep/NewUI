using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Emdep.Geos.UI.Commands;
using Workbench.Views;
using System.Threading;
using Emdep.Geos.Modules;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Hrm;
using DevExpress.Xpf.Core;
using System.Text.RegularExpressions;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.POCO;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;
using Emdep.Geos.Modules.Ticket.ViewModels;
using Emdep.Geos.Modules.Ticket.Views;
using Emdep.Geos.UI;
using System.Windows.Markup;
using Emdep.Geos.UI.Common;
using Prism.Logging;

namespace Workbench.ViewModels
{
    public class SettingWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  // Services

        #region Declaration

        private int settingWindowLanguageSelectedIndex;
        private bool isThemWhiteAndBlue;
        private List<Language> languages;
        string language;
        private List<GeosModule> geosModuleList;
        public List<GeosModule> TempGeosModuleList;
        private int moduleSelectedIndex;    //Setting Windos Module SelectedIndex
        private ObservableCollection<string> notificationNumberList;
        private int notificationNumberSelectedIndex;
        private bool isBusy;
        private bool isShowServiceIcon;
        private decimal refreshServiceSeconds;
        private bool isOfficeLogin;

        #endregion  // Declaration

        #region  public Properties

        public bool IsNotificationChange { get; set; }
        public bool IsThemeChange { get; set; }
        public bool IsInit { get; set; }
        public bool IsLanguagteChange { get; set; }

        public int SettingWindowLanguageSelectedIndex
        {
            get { return settingWindowLanguageSelectedIndex; }
            set
            {
                settingWindowLanguageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SettingWindowLanguageSelectedIndex"));
            }
        }

        public bool IsThemWhiteAndBlue
        {
            get { return isThemWhiteAndBlue; }
            set { isThemWhiteAndBlue = value; }
        }

        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public List<GeosModule> GeosModuleList
        {
            get { return geosModuleList; }
            set
            {
                geosModuleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosModuleList"));
            }
        }

        public int ModuleSelectedIndex
        {
            get { return moduleSelectedIndex; }
            set { moduleSelectedIndex = value; }
        }

        public ObservableCollection<string> NotificationNumberList
        {
            get { return notificationNumberList; }
            set { notificationNumberList = value; }
        }

        public int NotificationNumberSelectedIndex
        {
            get { return notificationNumberSelectedIndex; }
            set { notificationNumberSelectedIndex = value; }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public bool IsShowServiceIcon
        {
            get { return isShowServiceIcon; }
            set
            {
                isShowServiceIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowServiceIcon"));
            }
        }

        public decimal RefreshServiceSeconds
        {
            get { return refreshServiceSeconds; }
            set
            {
                refreshServiceSeconds = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefreshServiceSeconds"));
            }
        }

        public bool IsOfficeLogin
        {
            get { return isOfficeLogin; }
            set
            {
                isOfficeLogin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOfficeLogin"));
            }
        }

        #endregion  // Properties

        #region Commands

        public ICommand WorkbenchWindowSettingsCloseButtonCommand { get; set; } // WorkbenchWindow Change Password
        public ICommand WorkbenchWindowSettingsThemeBlackCommand { get; set; }  // WorkbenchWindow Settings for Theme Black and Blue
        public ICommand WorkbenchWindowSettingsThemeWhiteCommand { get; set; }  // WorkbenchWindow Settings for Theme White and Blue
        public ICommand SettingWindowAcceptButtonCommand { get; set; }  //WorkbenchWindow Settings Accept
        public ICommand WorkbenchWindowSettingsLoginCommand { get; set; }

        #endregion  // Commands

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose; // for close window

        #endregion  // Events

        #region Constructor

        public SettingWindowViewModel()
        {
            IsInit = true;
            if (GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow")
                && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds")
                && GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString() != string.Empty
                && int.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString()) > 0)
            {
                IsShowServiceIcon = bool.Parse(GeosApplication.Instance.UserSettings["IsServiceIconShow"].ToString());
                RefreshServiceSeconds = decimal.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString());
            }

            GeosApplication.Instance.Logger.Log("Start SettingWindowViewModel constructor", category: Category.Info, priority: Priority.Low);

            WorkbenchWindowSettingsCloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            SettingWindowAcceptButtonCommand = new RelayCommand(new Action<object>(SettingWindowAccept));
            WorkbenchWindowSettingsThemeBlackCommand = new RelayCommand(new Action<object>(GetThemeBlackAndBlue));
            WorkbenchWindowSettingsThemeWhiteCommand = new RelayCommand(new Action<object>(GetThemeWhiteAndBlue));
            WorkbenchWindowSettingsLoginCommand = new RelayCommand(new Action<object>(GetLogin));

            try
            {
                GeosApplication.Instance.Logger.Log("Fill Language and Module List", category: Category.Info, priority: Priority.Low);
                FillLanguage();
                FillModuleList();
                GeosApplication.Instance.Logger.Log("Fill Language and Module List successfully", category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ServerActiveMethod();
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                if (!GeosApplication.Instance.IsServiceActive)
                {
                    GeosApplication.Instance.ServerDeactiveMethod();
                }
                IsInit = false;
            }

            NotificationNumberList = new ObservableCollection<string>() { "5", "10", "15", "20", "30", "40", "50", "100", "150" };
            for (int i = 0; i < NotificationNumberList.Count; i++)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("NotificationPageCount"))
                {
                    if (NotificationNumberList[i].ToString() == GeosApplication.Instance.UserSettings["NotificationPageCount"].ToString())
                    {
                        NotificationNumberSelectedIndex = i;
                        break;
                    }
                }
            }

            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
            {
                IsThemWhiteAndBlue = true;
            }
            else
            {
                IsThemWhiteAndBlue = false;
            }

            if (GeosApplication.Instance.UserSettings.ContainsKey("Login"))
            {
                string login = GeosApplication.Instance.UserSettings["Login"];
                if (login == "Office")
                    IsOfficeLogin = true;
                else
                    IsOfficeLogin = false;
            }
            else
            {
                IsOfficeLogin = true;
            }

            GeosApplication.Instance.Logger.Log("End SettingWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region public Methods

        /// <summary>
        /// This method is for to Close Setting Window
        /// </summary>
        /// <param name="obj"></param>
        public void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// This method is for to set Theme
        /// </summary>
        /// <param name="obj"></param>
        public void GetThemeBlackAndBlue(object obj)
        {
            IsThemWhiteAndBlue = false;
        }

        /// <summary>
        /// This method is for to set Theme
        /// </summary>
        /// <param name="obj"></param>
        public void GetThemeWhiteAndBlue(object obj)
        {
            IsThemWhiteAndBlue = true;
        }

        public void GetLogin(object obj)
        {
            //IsThemWhiteAndBlue = true;
        }

        /// <summary>
        /// Method for select them and language by setting window
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void SettingWindowAccept(object obj)
        {
            IsBusy = true;

            try
            {
                GeosApplication.Instance.Logger.Log("Started SettingWindowAccept Method", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                string pathUserSetting = GeosApplication.Instance.UserSettingFilePath;

                int TempNotificationPageCount = 5;
                if (GeosApplication.Instance.UserSettings.ContainsKey("NotificationPageCount"))
                {
                    TempNotificationPageCount = int.Parse(GeosApplication.Instance.UserSettings["NotificationPageCount"].ToString());
                }

                string TempThemeName = ApplicationThemeHelper.ApplicationThemeName;
                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                if (IsThemWhiteAndBlue)
                {
                    ApplicationThemeHelper.ApplicationThemeName = "WhiteAndBlue";

                    if (TempThemeName != ApplicationThemeHelper.ApplicationThemeName)
                    {

                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ThemeName"))
                        {
                            GeosApplication.Instance.UserSettings["ThemeName"] = "WhiteAndBlue";
                        }
                        else
                        {
                            GeosApplication.Instance.UserSettings.Add("ThemeName", "WhiteAndBlue");
                        }

                        ResourceDictionary dict = new ResourceDictionary();
                        dict.Source = new Uri("/GeosWorkbench;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
                        App.Current.Resources.MergedDictionaries.Add(dict);
                    }
                }
                else
                {
                    ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";

                    if (TempThemeName != ApplicationThemeHelper.ApplicationThemeName)
                    {

                        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ThemeName"))
                        {
                            GeosApplication.Instance.UserSettings["ThemeName"] = "BlackAndBlue";
                        }
                        else
                        {
                            GeosApplication.Instance.UserSettings.Add("ThemeName", "BlackAndBlue");
                        }

                        ResourceDictionary dict = new ResourceDictionary();
                        dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                        App.Current.Resources.MergedDictionaries.Add(dict);
                    }
                }

                if (TempThemeName != ApplicationThemeHelper.ApplicationThemeName)
                {
                    IsThemeChange = true;
                }

                if (TempNotificationPageCount != Convert.ToInt32(NotificationNumberList[NotificationNumberSelectedIndex].ToString()))
                {
                    IsNotificationChange = true;
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("NotificationPageCount"))
                {
                    GeosApplication.Instance.UserSettings["NotificationPageCount"] = NotificationNumberList[NotificationNumberSelectedIndex].ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings.Add("NotificationPageCount", NotificationNumberList[NotificationNumberSelectedIndex].ToString());
                }

                if (IsShowServiceIcon)
                {

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow"))
                    {
                        GeosApplication.Instance.UserSettings["IsServiceIconShow"] = IsShowServiceIcon.ToString();
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("IsServiceIconShow", IsShowServiceIcon.ToString());
                    }

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds"))
                    {
                        GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"] = RefreshServiceSeconds.ToString();
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("ServiceRefreshSeconds", RefreshServiceSeconds.ToString());
                    }
                }
                else
                {
                    IsShowServiceIcon = false;

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow"))
                    {
                        GeosApplication.Instance.UserSettings["IsServiceIconShow"] = IsShowServiceIcon.ToString();
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("IsServiceIconShow", IsShowServiceIcon.ToString());
                    }
                }

                SetLanguageDictionary();

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds"))
                {
                    GeosApplication.Instance.UserSettings["SelectedModule"] = GeosModuleList[ModuleSelectedIndex].Name.ToString();
                }
                else
                {
                    GeosApplication.Instance.UserSettings.Add("SelectedModule", GeosModuleList[ModuleSelectedIndex].Name.ToString());
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Login"))
                {
                    if (IsOfficeLogin)
                        GeosApplication.Instance.UserSettings["Login"] = "Office";
                    else
                        GeosApplication.Instance.UserSettings["Login"] = "Production";
                }
                else
                {
                    GeosApplication.Instance.UserSettings.Add("Login", "Office");
                }

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("End SettingWindowAccept Method", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SettingWindowAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to set Languge Dictionary
        /// </summary>
        private void SetLanguageDictionary()
        {
            string TempLanguage = GeosApplication.Instance.CurrentCulture;
            ResourceDictionary dict = new ResourceDictionary();

            Language = Languages[SettingWindowLanguageSelectedIndex].CultureName;
            GeosApplication.Instance.CurrentCulture = Language;

            if (TempLanguage != GeosApplication.Instance.CurrentCulture)
            {
                try
                {
                    GeosApplication.Instance.SetLanguageDictionary();
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                    dict.Source = new Uri("/GeosWorkbench;component/Resources/Language." + Language + ".xaml", UriKind.Relative);

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Language"))
                    {
                        GeosApplication.Instance.UserSettings["Language"] = GeosApplication.Instance.CurrentCulture;
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("Language", GeosApplication.Instance.CurrentCulture);
                    }
                }
                catch (Exception)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                    dict.Source = new Uri("/GeosWorkbench;component/Resources/Language.xaml", UriKind.Relative);

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Language"))
                    {
                        GeosApplication.Instance.UserSettings["Language"] = GeosApplication.Instance.CurrentCulture;
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("Language", GeosApplication.Instance.CurrentCulture);
                    }
                }

                App.Current.Resources.MergedDictionaries.Add(dict);
                IsLanguagteChange = true;
            }
        }

        /// <summary>
        /// This method is for to Fill language image in list as per Culture
        /// </summary>
        private void FillLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Getting All Language on list - FillLanguage()", category: Category.Info, priority: Priority.Low);
                Languages = WorkbenchStartUp.GetAllLanguage();

                for (int i = 0; i < languages.Count; i++)
                {
                    Languages[i].LanguageImage = GetImage("/Assets/Images/" + Languages[i].Name + ".gif");

                    if (GeosApplication.Instance.CurrentCulture.Substring(0, 2).ToUpper() == Languages[i].TwoLetterISOLanguage.ToString().ToUpper())
                    {
                        SettingWindowLanguageSelectedIndex = i;
                    }
                }

                GeosApplication.Instance.Logger.Log("Getting All Language on list successfully - FillLanguage()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for to fill modules list .
        /// </summary>
        private void FillModuleList()
        {
            try
            {
                GeosModuleList = new List<GeosModule>();

                GeosApplication.Instance.Logger.Log("Getting Geos Module list User Id", category: Category.Info, priority: Priority.Low);
                GeosModuleList.Add(new GeosModule() { IdGeosModule = 0, Name = "---" });
                GeosModuleList = WorkbenchStartUp.GetUserModulesPermissions_V2220(GeosApplication.Instance.ActiveUser.IdUser).Distinct().ToList();

                TempGeosModuleList = new List<GeosModule>();
                TempGeosModuleList.Add(new GeosModule() { IdGeosModule = 0, Name = "---" });
                GeosModuleList.InsertRange(0, TempGeosModuleList);

                for (int i = 0; i < GeosModuleList.Count; i++)
                {
                    //GeosModuleList.Add(new GeosModule() { IdGeosModule = TempGeosModuleList[i].IdGeosModule, Name = TempGeosModuleList[i].Name });

                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedModule")
                        && GeosModuleList[i].Name.ToString() == GeosApplication.Instance.UserSettings["SelectedModule"].ToString())
                    {
                        ModuleSelectedIndex = i;
                    }
                    //else
                    //{
                    //    //GeosModuleList.Add(new GeosModule() { IdGeosModule = 0, Name = "---" });
                    //    //ModuleSelectedIndex = 0;
                    //}
                }

                //GeosModuleList.Add(new GeosModule() { IdGeosModule = 0, Name = "---" });
                //ModuleSelectedIndex = 0;\

                GeosApplication.Instance.Logger.Log("Getting Geos Module list User Id successfully - FillModuleList()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillModuleList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillModuleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        #endregion  // Methods
    }
}
