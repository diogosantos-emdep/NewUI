using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Emdep.Geos.UI.Commands;
using Workbench.Views;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Utility;
using Emdep.Geos.Utility.Text;
using Emdep.Geos.Modules.Ticket.ViewModels;
using Emdep.Geos.Modules.Ticket.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common.Glpi;
using Prism.Logging;
using Emdep.Geos.UI.Helper;

namespace Workbench.ViewModels
{
    [POCOViewModel]
    public class WorkbenchWindowViewModel : DevExpress.Mvvm.ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private System.ComponentModel.Container components;
        System.Drawing.Graphics graphicsObj;

        private List<GeosModule> geosModuleList;

        private string serverTime; //DashboardView show time
        private string serverDate; //DashboardView show date
        private string serverWeek; //DashboardView show week number
        private DispatcherTimer Notificationtimer; // for Notification check
        private DispatcherTimer dateTimeTimer; // for take current date time

        private string tileBackgroundColor;
        private object imageSource;
        private string companyAlias;
        private string loginUserFirstName;
        private string loginUserLastName;
        private string loginUserEmail;
        private string loginUserFullName;

        private ImageSource userProfileImage;
        byte[] UserProfileImageByte = null;
        private string installedversion;
        private GlpiUser gLPIUser;
        private ObservableCollection<Notification> notificationsList = new ObservableCollection<Notification>();
        private List<Notification> unreadNotificationsList;

        System.Timers.Timer tt;

        private string settingWindowLanguageSelectedItem;

        private WindowState currentWindowState;//current WindowState
        private Visibility restoreButtonVisibility = Visibility.Hidden;//Restore Button Visibility
        private Visibility maxiMizeButtonVisibility = Visibility.Hidden;//MaxiMize Button Visibility
        private Visibility notificationVisibility = Visibility.Hidden;
        private Visibility serviceIndicatorVisibility = Visibility.Visible;
        private ImageSource workbenchBackgroundImagePath;
        private ImageSource emdepLogoImagePath;
        private ImageSource desktopLogoImagePath;
        private ImageSource geosLogoImagePath;
        private ImageSource userInitialsImage;

        List<Notification> allNotification;
        private int newNotificationCount;
        private string notificationInString;
        private ImageSource notificationImage;
        private bool isNotificationViewMoreButtonEnable = true;
        private bool isNotificationClearAllButtonEnable = true;
        private int timerCount = 0;
        private int lastNotificationsId = 0;
        private bool isEnableSupport;
        private int messageIndex;

        private Version version;
        private ImageSource editUserProfileImage;

        private int totalNotification;

        private string dispayNotification;
        private ImageSource impersonateUserProfileImage;

        private bool IsNotificationsActive = false;
        private bool isBusy;

        private bool isServiceActive = true;
        private string selectedModule;
        private INavigationService NavigationService { get { return this.GetService<INavigationService>(); } }
        Dictionary<string, string> Moduledictionary = new Dictionary<string, string>();
        public bool IsUserSignOut = false;
        public bool IsSettingChange = false;
        long LastIdNotification = long.MaxValue;
        int notificationId;

        private Notification selectedNotification;
        private GeosWorkbenchVersion geosWorkbenchVersionNumber;
        public Dictionary<long, ImageSource> CollectionFromUserImages = new Dictionary<long, ImageSource>();
        public Dictionary<string, ImageSource> NotAvailableImagesDictionary = new Dictionary<string, ImageSource>();

        #endregion

        #region  public Properties

        public string IP { get; set; }
        public string MachineName { get; set; }
        public string WorkbenchLocation { get; set; }
        public int ShownNotificationCount { get; set; }
        public Workstation LoginWorkstation { get; set; }
        public bool IsNotificationNumberChange { get; set; }
        public bool IsThemeChange { get; set; }
        public bool IsLanguagteChange { get; set; }
        public ObservableCollection<GeosModule> GeosModuleTiles { get; set; }
        public System.Windows.Forms.DialogResult DialogResult { get; set; }

        public object ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; }
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

        public string ServerTime
        {
            get { return serverTime; }
            set
            {
                serverTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServerTime"));
            }
        }

        public string ServerDate
        {
            get { return serverDate; }
            set
            {
                serverDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServerDate"));
            }
        }

        public string ServerWeek
        {
            get { return serverWeek; }
            set
            {
                serverWeek = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServerWeek"));
            }
        }

        public string TileBackgroundColor
        {
            get { return tileBackgroundColor; }
            set
            {
                tileBackgroundColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TileBackgroundColor"));
            }
        }

        public string CompanyAlias
        {
            get { return companyAlias; }
            set
            {
                companyAlias = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyAlias"));
            }
        }

        public WindowState CurrentWindowState
        {
            get
            { return currentWindowState; }
            set
            {
                currentWindowState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentWindowState"));
            }
        }

        public Visibility RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set
            {
                restoreButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RestoreButtonVisibility"));
            }
        }

        public Visibility MaximizeButtonVisibility
        {
            get { return maxiMizeButtonVisibility; }
            set
            {
                maxiMizeButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizeButtonVisibility"));
            }
        }

        public Visibility NotificationVisibility
        {
            get { return notificationVisibility; }
            set
            {
                notificationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotificationVisibility"));
            }
        }

        public Visibility ServiceIndicatorVisibility
        {
            get { return serviceIndicatorVisibility; }
            set
            {
                serviceIndicatorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServiceIndicatorVisibility"));
            }
        }
        public string LoginUserFirstName
        {
            get { return loginUserFirstName; }
            set
            {
                loginUserFirstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoginUserName"));
            }
        }

        public string LoginUserLastName
        {
            get { return loginUserLastName; }
            set { loginUserLastName = value; }
        }

        public string LoginUserFullName
        {
            get { return loginUserFullName; }
            set { loginUserFullName = value; }
        }

        public string LoginUserEmail
        {
            get { return loginUserEmail; }
            set { loginUserEmail = value; }
        }

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
            }
        }

        public string Installedversion
        {
            get { return installedversion; }
            set
            {
                installedversion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Installedversion"));
            }
        }

        public GlpiUser GLPIUser
        {
            get { return gLPIUser; }
            set { gLPIUser = value; }
        }

        public ObservableCollection<Notification> NotificationsList
        {
            get { return notificationsList; }
            set
            {
                notificationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotificationsList"));
            }
        }

        public List<Notification> UnreadNotificationsList
        {
            get { return unreadNotificationsList; }
            set
            {
                unreadNotificationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnreadNotificationsList"));
            }
        }

        public string SettingWindowLanguageSelectedItem
        {
            get { return settingWindowLanguageSelectedItem; }
            set
            {
                settingWindowLanguageSelectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SettingWindowLanguageSelectedItem"));
            }
        }

        public ImageSource NotificationImage
        {
            get { return notificationImage; }
            set
            {
                notificationImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotificationImage"));
            }
        }

        public string NotificationInString
        {
            get { return notificationInString; }
            set
            {
                notificationInString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotificationInString"));
            }
        }

        public int NewNotificationCount
        {
            get { return newNotificationCount; }
            set
            {
                newNotificationCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewNotificationCount"));
            }
        }

        public ImageSource GeosLogoImagePath
        {
            get { return geosLogoImagePath; }
            set
            {
                geosLogoImagePath = value; ;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosLogoImagePath"));
            }
        }
        public ImageSource WorkbenchBackgroundImagePath
        {
            get { return workbenchBackgroundImagePath; }
            set
            {
                workbenchBackgroundImagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkbenchBackgroundImagePath"));
            }
        }

        public ImageSource DesktopLogoImagePath
        {
            get { return desktopLogoImagePath; }
            set
            {
                desktopLogoImagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DesktopLogoImagePath"));
            }
        }

        public ImageSource EmdepLogoImagePath
        {
            get { return emdepLogoImagePath; }
            set
            {
                emdepLogoImagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmdepLogoImagePath"));
            }
        }

        public ImageSource UserInitialsImage
        {
            get { return userInitialsImage; }
            set
            {
                userInitialsImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserInitialsImage"));
            }
        }

        public List<Notification> AllNotification
        {
            get { return allNotification; }
            set
            {
                allNotification = value; ;
                OnPropertyChanged(new PropertyChangedEventArgs("AllNotification"));
            }
        }

        public bool IsEnableSupport
        {
            get { return isEnableSupport; }
            set
            {
                isEnableSupport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableSupport"));
            }
        }

        public int MessageIndex
        {
            get { return messageIndex; }
            set
            {
                messageIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MessageIndex"));
            }
        }

        public ImageSource EditUserProfileImage
        {
            get { return editUserProfileImage; }
            set
            {
                editUserProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditUserProfileImage"));
            }
        }

        public int TotalNotification
        {
            get { return totalNotification; }
            set
            {
                totalNotification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalNotification"));
            }
        }

        public string DispayNotification
        {
            get { return dispayNotification; }
            set
            {
                dispayNotification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DispayNotification"));
            }
        }

        public ImageSource ImpersonateUserProfileImage
        {
            get { return impersonateUserProfileImage; }
            set
            {
                impersonateUserProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImpersonateUserProfileImage"));
            }
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

        public bool IsServiceActive
        {
            get { return isServiceActive; }
            set
            {
                isServiceActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsServiceActive"));
            }
        }

        public bool IsNotificationViewMoreButtonEnable
        {
            get { return isNotificationViewMoreButtonEnable; }
            set
            {
                isNotificationViewMoreButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNotificationViewMoreButtonEnable"));
            }
        }

        public bool IsNotificationClearAllButtonEnable
        {
            get { return isNotificationClearAllButtonEnable; }
            set
            {
                isNotificationClearAllButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNotificationClearAllButtonEnable"));
            }
        }

        public string SelectedModule
        {
            get { return selectedModule; }
            set
            {
                selectedModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedModule"));
            }
        }

        public Notification SelectedNotification
        {
            get { return selectedNotification; }
            set
            {
                selectedNotification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNotification"));
            }
        }

        public int NotificationId
        {
            get { return notificationId; }
            set
            {
                notificationId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotificationId"));
            }
        }

        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
        }
        #endregion

        #region Commands

        public ICommand EmdepSitesWindowCommand { get; set; } //Emdep Sites Window
        public ICommand SupportWindowCommand { get; set; } //Emdep Sites Window
        public ICommand WorkbenchWindowCloseButtonCommand { get; set; } //Close on WorkbenchWindow page
        public ICommand WorkbenchWindowMiniMizeButtonCommand { get; set; }//WorkbenchWindow minimize Button
        public ICommand WorkbenchWindowMaxiMizeButtonCommand { get; set; }//WorkbenchWindow maximize Button
        public ICommand RestorButtonCommand { get; set; }// Login Restore Button
        public ICommand WorkbenchWindowEditProfileButtonCommand { get; set; }// WorkbenchWindow Edit Profile
        public ICommand WorkbenchWindowSignOutButtonCommand { get; set; }// WorkbenchWindow Edit Profile
        public ICommand WorkbenchWindowSettingsButtonCommand { get; set; }// WorkbenchWindow Change Password
        public ICommand WorkbenchWindowChangePasswordCloseButtonCommand { get; set; }
        public ICommand WorkbenchWindowNotification { get; set; }
        public ICommand UpdateUserNotificationCommand { get; set; }
        public ICommand ShowDefaultNotificationCommand { get; set; }
        public ICommand FeedbackWindowOpenCommand { get; set; } //Feeadack Window open
        public ICommand AboutWindowOpenCommand { get; set; } //Feeadack Window open
        public ICommand WorkbenchWindowViewAllCommand { get; set; } //For notification
        public ICommand WorkbenchWindowClearAllCommand { get; set; } //For notification
      //  public ICommand WorkbenchWindowRemoveNotificationCommand { get; set; } //For notification
        public ICommand WorkbenchWindowOpenEmdepWebSiteCommand { get; set; } //Feeadack Window open
        public ICommand WorkbenchWindowCloseCommand { get; set; } //Feeadack Window open
        public ICommand ValidateProfilePicture { get; set; }//temporary use for vailidation.
        public ICommand OnViewLoadedCommand { get; set; }
        public ICommand NotificationDeleteButtonCommand { get; set; }
        public ICommand SingleNotificationDeleteButtonCommand { get; set; }
        public ICommand PageUnLoadedCommand { get; set; } //Close on Login page

        #endregion

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose; //supportWindow for close window

        #endregion

        #region  Constructor

        public WorkbenchWindowViewModel()
        {
            GeosApplication.Instance.Logger.Log("Initialising workbenchViewModel constructor .....", category: Category.Info, priority: Priority.Low);

            //clear old data.
            GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>();

            FillNotAvailableImages();

            if (GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow") && bool.Parse(GeosApplication.Instance.UserSettings["IsServiceIconShow"].ToString()))
            {
                ServiceIndicatorVisibility = Visibility.Visible;
            }
            else
            {
                ServiceIndicatorVisibility = Visibility.Hidden;
            }

            GeosApplication.Instance.ServerDeactiveMethod = delegate ()
            {
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            };

            GeosApplication.Instance.ServerActiveMethod = delegate ()
            {
                GeosApplication.Instance.IsServiceActive = true;
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            };

            IsServiceActive = true;
            MachineName = ApplicationOperation.GetCurrentComputerName();
            WorkbenchLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            version = AssemblyVersion.GetAssemblyVersion(WorkbenchLocation);

            try
            {
                GeosApplication.Instance.Logger.Log("Getting GeosWorkbench Version Number", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosWorkbenchVersionNumber = WorkbenchStartUp.GetWorkbenchVersionByVersionNumber(version.ToString());
                GeosApplication.Instance.GeosWorkbenchVersionNumber = GeosWorkbenchVersionNumber;

                GeosApplication.Instance.Logger.Log("Getting GeosWorkbench Version Number successfully", category: Category.Info, priority: Priority.Low);

                if (GeosWorkbenchVersionNumber != null && GeosWorkbenchVersionNumber.IsBeta == 1)
                {
                    Installedversion = "    V " + version + " (Beta)";
                }
                else
                {
                    Installedversion = "    V " + version;
                }

                GeosApplication.Instance.Logger.Log("Getting Geos Module List by user Id", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosModuleList = WorkbenchStartUp.GetUserModulesPermissions(GeosApplication.Instance.ActiveUser.IdUser).Distinct().ToList();
                if (!GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
                    GeosApplication.Instance.ObjectPool.Add("GeosModuleNameList", GeosModuleList);
                else
                    GeosApplication.Instance.ObjectPool["GeosModuleNameList"] = GeosModuleList;

                GeosApplication.Instance.Logger.Log("Getting Geos Module List by user Id successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkbenchWindowViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkbenchWindowViewModel() Constructor - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }

            LoginUserFirstName = GeosApplication.Instance.ActiveUser.FirstName;
            LoginUserLastName = GeosApplication.Instance.ActiveUser.LastName;
            LoginUserFullName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName;
            LoginUserEmail = GeosApplication.Instance.ActiveUser.CompanyEmail;
            SetUserProfileImage();
            IGlpiService GLPIControl = new GlpiController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

            try
            {
                GeosApplication.Instance.Logger.Log("Getting GLPI User By Login ", category: Category.Info, priority: Priority.Low);

                try
                {
                    GLPIUser = GLPIControl.GetGLPIUserByName(GeosApplication.Instance.ActiveUser.Login);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in GetGLPIUserByName() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                GeosApplication.Instance.Logger.Log("Getting GLPI User By Name successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkbenchWindowViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkbenchWindowViewModel() Constructor - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }

            if (GLPIUser == null)
            {
                GeosApplication.Instance.Logger.Log("Getting GLPI User By Name ", category: Category.Info, priority: Priority.Low);
                try
                {
                    GLPIUser = GLPIControl.GetGLPIUserByName(System.Environment.UserName.ToString());
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in GetGLPIUserByName() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                GeosApplication.Instance.Logger.Log("Getting GLPI User By Name successfully", category: Category.Info, priority: Priority.Low);
            }

            CurrentWindowState = WindowState.Maximized;
            RestoreButtonVisibility = Visibility.Visible;
            EmdepSitesWindowCommand = new RelayCommand(new Action<object>(EmdepSitesWindow));
            SupportWindowCommand = new RelayCommand(new Action<object>(OpenSupportWindow));
            WorkbenchWindowCloseButtonCommand = new RelayCommand(new Action<object>(WorkbenchWindowClose));
            WorkbenchWindowMiniMizeButtonCommand = new RelayCommand(new Action<object>(WorkbenchWindowMiniMizeButton));
            WorkbenchWindowMaxiMizeButtonCommand = new RelayCommand(new Action<object>(WorkbenchWindowMaxiMizeButton));
            WorkbenchWindowEditProfileButtonCommand = new RelayCommand(new Action<object>(OpenEditProfile));
            WorkbenchWindowSignOutButtonCommand = new RelayCommand(new Action<object>(UserSignOut));
            RestorButtonCommand = new RelayCommand(new Action<object>(RestorButton));
            WorkbenchWindowSettingsButtonCommand = new RelayCommand(new Action<object>(ShowSettingsWindow));
            WorkbenchWindowChangePasswordCloseButtonCommand = new RelayCommand(new Action<object>(ChangePasswordCloseButton));
            UpdateUserNotificationCommand = new RelayCommand(new Action<object>(UpdateUserNotification));
            FeedbackWindowOpenCommand = new RelayCommand(new Action<object>(OpenFeedbackWindow));
            AboutWindowOpenCommand = new RelayCommand(new Action<object>(OpenAboutWindow));
            WorkbenchWindowViewAllCommand = new RelayCommand(new Action<object>(ViewAllNotification));
            WorkbenchWindowClearAllCommand = new RelayCommand(new Action<object>(ClearAllNotification));
          //  WorkbenchWindowRemoveNotificationCommand = new RelayCommand(new Action<object>(RemoveNotification));
            WorkbenchWindowOpenEmdepWebSiteCommand = new RelayCommand(new Action<object>(OpenEmdepWebSite));
            WorkbenchWindowCloseCommand = new RelayCommand(new Action<object>(CloseWindows));
            OnViewLoadedCommand = new RelayCommand(new Action<object>(OnViewLoaded));
            NotificationDeleteButtonCommand = new RelayCommand(new Action<object>(OnViewLoaded));
            SingleNotificationDeleteButtonCommand = new RelayCommand(new Action<object>(DeleteSingleNotification));
            PageUnLoadedCommand = new RelayCommand(new Action<object>(OnPageUnLoadedCommand));
            SetTileImageBackground();
            GetCurrentTimeDetail();

            GeosApplication.Instance.Logger.Log("Getting total notification By user Id ", category: Category.Info, priority: Priority.Low);
            WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            TotalNotification = WorkbenchStartUp.GetAllNotificationCount(GeosApplication.Instance.ActiveUser.IdUser);
            GeosApplication.Instance.Logger.Log("Getting total notification By user Id successfully ", category: Category.Info, priority: Priority.Low);

            SelectcDefaultSite();
            ViewAllNotification(null);
            SetServerTimeLabel();

            ViewLocator.Default = new NavigationViewLocator();
            ValidateProfilePicture = new RelayCommand(new Action<object>(ValidateUserProfilePicture));
            FillModuleDictionary();

            GeosApplication.Instance.Logger.Log("Initialising workbenchViewModel constructor successfully", category: Category.Info, priority: Priority.Low);
        }

        private void OnPageUnLoadedCommand(object obj)
        {
            this.PropertyChanged -= PropertyChanged;
        }

        #endregion

        #region  Methods

        /// <summary>
        /// Method for change Module tile color as per theme change.
        /// </summary>
        private void SetTileImageBackground()
        {
            GeosApplication.Instance.Logger.Log("Fill Tile content ...", category: Category.Info, priority: Priority.Low);
            System.Drawing.ColorConverter ColorConverter = new System.Drawing.ColorConverter();

            for (int i = 0; i < GeosModuleList.Count; i++)
            {
                if (String.IsNullOrEmpty(GeosModuleList[i].Acronym))
                {
                    GeosModuleList[i].Acronym = GeosModuleList[i].Name;
                }

                // HARNESS PARTS
                if (GeosModuleList[i].IdGeosModule == 1)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();

                    GeosModuleList[i].NavigateTo = "Workbench.Views.HarnessPartsView";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.HarnessPartsLogo, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("/Assets/Images/HarnessPartsLogo.png");
                }

                // WORKSHOP
                if (GeosModuleList[i].IdGeosModule == 2)
                {
                    GeosModuleList[i].NavigateTo = "Workbench.Views.WorkshopView";
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.Assembly, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // DESIGN
                if (GeosModuleList[i].IdGeosModule == 3)
                {
                    GeosModuleList[i].NavigateTo = "Workbench.Views.DesignView";
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.Design, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // ENGINEERING PROJECTS CONTROL
                if (GeosModuleList[i].IdGeosModule == 4)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.EpcWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.Epc, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // CUSTOMER RELATIONSHIP MANAGEMENT
                if (GeosModuleList[i].IdGeosModule == 5)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.CrmWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.CRM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // WAREHOUSE 
                if (GeosModuleList[i].IdGeosModule == 6)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.WarehouseWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.Warehouse, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // HRM 
                if (GeosModuleList[i].IdGeosModule == 7)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.HrmWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.HRM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor)); //GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Assembly.png");
                }

                // PCM
                if (GeosModuleList[i].IdGeosModule == 8)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.PCMWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.PCM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor));
                }

                // SAM
                if (GeosModuleList[i].IdGeosModule == 9)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.SAMWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.SAM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor));
                }

                // SRM
                if (GeosModuleList[i].IdGeosModule == 10)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.SRMWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.SRM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor));
                }

                // PLM
                if (GeosModuleList[i].IdGeosModule == 11)
                {
                    var v = GeosModuleList[i].UIModuleThemes.Where(x => x.UITheme.ThemeName == ApplicationThemeHelper.ApplicationThemeName.ToString()).FirstOrDefault();
                    GeosModuleList[i].NavigateTo = "Workbench.Views.PLMWindow";
                    GeosModuleList[i].ForeColor = v.ForeColor;
                    GeosModuleList[i].BackColor = v.BackColor;
                    GeosModuleList[i].SiteImageSource = ChangeImageColor(Workbench.Properties.Resources.PLM, (System.Drawing.Color)ColorConverter.ConvertFromString(v.ForeColor));
                }

                GeosModuleTiles = new ObservableCollection<GeosModule>(GeosModuleList);
                GeosApplication.Instance.Logger.Log("Fill Tile content successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for validate User pending Image.
        /// </summary>
        /// <param name="obj"></param>
        public void ValidateUserProfilePicture(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method ", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WorkbenchStartUp.SetUserProfileImage(GeosApplication.Instance.ActiveUser);
                GeosApplication.Instance.ActiveUser.IsValidated = 1;

                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method ", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImageAbsolute(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        /// <summary>
        ///  This Method for fill NaImage for  User & map
        /// </summary>
        private void FillNotAvailableImages()
        {
            if (NotAvailableImagesDictionary.Count == 0)
            {
                NotAvailableImagesDictionary.Add("NotAvailable", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/NotAvailable.png"));
                NotAvailableImagesDictionary.Add("NotAvailableBlue", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/NotAvailableBlue.png"));
                NotAvailableImagesDictionary.Add("FemaleUserBlue", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserBlue.png"));
                NotAvailableImagesDictionary.Add("FemaleUserWhite", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png"));
                NotAvailableImagesDictionary.Add("MaleUserWhite", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png"));
                NotAvailableImagesDictionary.Add("MaleUserBlue", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserBlue.png"));
                NotAvailableImagesDictionary.Add("Notifications", GetImage("pack://application:,,,/GeosWorkbench;component/Assets/Images/Notifications.png"));
            }
        }

        /// <summary>
        /// This method change the Image color . 
        /// </summary>
        /// <param name="sourcebitmap"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        ImageSource ChangeImageColor(Bitmap sourcebitmap, System.Drawing.Color color)
        {
            MemoryStream memoryStream = new MemoryStream();
            Bitmap bitmap = ImageUtil.ChangeColor(sourcebitmap, color);
            MemoryStream MemoryStream = new MemoryStream();
            bitmap.Save(MemoryStream, System.Drawing.Imaging.ImageFormat.Png);

            MemoryStream.Position = 0;
            BitmapImage BitmapImage = new BitmapImage();
            BitmapImage.BeginInit();
            BitmapImage.StreamSource = MemoryStream;
            BitmapImage.EndInit();

            return BitmapImage;
        }

        /// <summary>
        ///  This method is for to convert Image to ImageSource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource ConvertImageToImageSource(string path)
        {
            BitmapImage biImg = new BitmapImage(new Uri(path, UriKind.Relative));
            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            biImg.DecodePixelHeight = 10;
            biImg.DecodePixelWidth = 10;

            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        /// <summary>
        /// This method is for to get server current datetime detail.
        /// </summary>
        public void GetCurrentTimeDetail()
        {
            GeosApplication.Instance.Logger.Log("Initializing timer...", category: Category.Info, priority: Priority.Low);

            dateTimeTimer = new DispatcherTimer();
            dateTimeTimer.Tick += new EventHandler(tt_Elapsed);

            if (GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow")
                && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds")
                && GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString() != string.Empty
                && int.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString()) > 0)
            {
                dateTimeTimer.Interval = new TimeSpan(0, 0, 0, int.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString()), 0);
            }
            else
            {
                dateTimeTimer.Interval = new TimeSpan(0, 0, 1, 0, 0);
            }

            timerCount = 0;
            Notificationtimer = new DispatcherTimer();
            Notificationtimer.Tick += new EventHandler(dispatcherTimer_Tick);
            Notificationtimer.Interval = new TimeSpan(0, 0, 1, 0, 0);
            Notificationtimer.Start();
            dateTimeTimer.Start();
            GeosApplication.Instance.Logger.Log("Initializing timer successfully", category: Category.Info, priority: Priority.Low);
        }

        private async void tt_Elapsed(object sender, EventArgs e)
        {
            await WorkerMethod();
        }

        private Task WorkerMethod()
        {
            return Task.Run(() =>
            {
                SetServerTimeLabel();
            });
        }

        private void SetServerTimeLabel()
        {
            try
            {
                IsServiceActive = true;
                GeosApplication.Instance.Logger.Log("Get current date time from server", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.ServerDateTime = WorkbenchStartUp.GetServerDateTime();
                CultureInfo CultureInfo = new System.Globalization.CultureInfo(GeosApplication.Instance.CurrentCulture);
                ServerDate = GeosApplication.Instance.ServerDateTime.ToString("dddd, d MMMM, yyyy", CultureInfo);
                ServerTime = GeosApplication.Instance.ServerDateTime.ToString("HH:mm");
                int weekNum = CultureInfo.Calendar.GetWeekOfYear(GeosApplication.Instance.ServerDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                ServerWeek = Workbench.App.Current.Resources["DashboardViewWeek"].ToString() + " " + weekNum.ToString();
                GeosApplication.Instance.ServerActiveMethod();

                GeosApplication.Instance.Logger.Log("Get current date time from server successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetServerTimeLabel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                Application.Current.Dispatcher.Invoke(delegate { CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK); });
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetServerTimeLabel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Application.Current.Dispatcher.Invoke(delegate { GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null); });

                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetServerTimeLabel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to get system IP
        /// </summary>
        public void GetSystemIP()
        {
            GeosApplication.Instance.Logger.Log("Get current System hostnamme and Ip", category: Category.Info, priority: Priority.Low);
            string hostName = Dns.GetHostName();
            Dns.GetHostByName(hostName).AddressList[0].ToString();
            IPHostEntry hostname = Dns.GetHostByName(hostName.ToString());
            IPAddress[] ip = hostname.AddressList;
            IP = ip[0].ToString();
            GeosApplication.Instance.Logger.Log("Get current System hostnamme and Ip successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [001]This method is for to set Emdep site.
        /// [002][2018-07-06][skhade][WORKBENCH-M042-12] some plants in the plant selector has no underline in the plant short name
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("open EmdepSitesWindow .....", category: Category.Info, priority: Priority.Low);
                GetEmdepSiteImage();

                EmdepSitesMapWindowViewModel EmdepSitesMapWindowViewModel = new EmdepSitesMapWindowViewModel();
                EmdepSitesWindow EmdepSitesWindow = new EmdepSitesWindow();
                EventHandler handle = delegate { EmdepSitesWindow.Close(); };
                EmdepSitesMapWindowViewModel.RequestClose += handle;

                //002
                //CompanyList = CompanyList.OrderBy(cl => cl.IsPermission == true).ThenBy(x => x.IdCountry).ThenBy(x => x.Name).ToList();
                CompanyList = CompanyList.OrderBy(x => x.IdCountry).ThenBy(x => x.Name).ToList();

                EmdepSitesMapWindowViewModel.CompanyObservableCollection = new ObservableCollection<Company>(CompanyList);
                EmdepSitesWindow.DataContext = EmdepSitesMapWindowViewModel;

                for (int i = 0; i < CompanyList.Count; i++)
                {
                    if (CompanyList[i].Alias.Equals(CompanyAlias))
                    {
                        EmdepSitesMapWindowViewModel.ActiveItemIndexValue = i;
                        break;
                    }
                }

                EmdepSitesWindow.ShowDialogWindow();
                GeosApplication.Instance.SiteName = EmdepSitesMapWindowViewModel.CompanyAlias;
                CompanyAlias = GeosApplication.Instance.SiteName;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("open EmdepSitesWindow successfully", category: Category.Info, priority: Priority.Low);
        }

        bool isSiteImageFill = false;
        public List<Company> CompanyList; //list of site 

        /// <summary>
        /// method for get all emdep site image
        /// </summary>
        public void GetEmdepSiteImage()
        {
            GeosApplication.Instance.Logger.Log("Get Emdep company image successfully", category: Category.Info, priority: Priority.Low);

            try
            {
                DXSplashScreen.Show<SplashScreenView>();

                if (CompanyList.Count > 0 && isSiteImageFill == false && IsThemeChange == false)
                {
                    for (int i = 0; i < CompanyList.Count; i++)
                    {
                        if (CompanyList[i].IsPermission == true)
                        {
                            try
                            {
                                GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                byte[] bytesite = GeosRepositoryServiceController.GetCompanyImage(CompanyList[i].IdCompany);
                                CompanyList[i].SiteImage = ByteToImage(bytesite);
                                CompanyList[i].IsSiteImageAvailable = true;
                            }
                            catch (Exception)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    CompanyList[i].SiteImage = NotAvailableImagesDictionary["NotAvailable"];
                                    CompanyList[i].IsSiteImageAvailable = false;
                                }
                                else
                                {
                                    CompanyList[i].SiteImage = NotAvailableImagesDictionary["NotAvailableBlue"];
                                    CompanyList[i].IsSiteImageAvailable = false;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                byte[] bytesite = GeosRepositoryServiceController.GetCompanyImage(CompanyList[i].IdCompany);
                                CompanyList[i].SiteImage = ConvertImageToGrayScaleImage(bytesite);
                                CompanyList[i].IsSiteImageAvailable = true;
                            }
                            catch (Exception)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    byte[] bytesite = BitmapToByteArray(Workbench.Properties.Resources.NotAvailable);
                                    CompanyList[i].SiteImage = ConvertImageToGrayScaleImage(bytesite);
                                    CompanyList[i].IsSiteImageAvailable = false;
                                }
                                else
                                {
                                    byte[] bytesite = BitmapToByteArray(Workbench.Properties.Resources.NotAvailableBlue);
                                    CompanyList[i].SiteImage = ConvertImageToGrayScaleImage(bytesite);
                                    CompanyList[i].IsSiteImageAvailable = false;
                                }
                            }
                        }
                    }

                    isSiteImageFill = true;
                }

                GeosApplication.Instance.Logger.Log("Get Emdep company image successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetEmdepSiteImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On GetEmdepSiteImage Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On GetEmdepSiteImage Method" + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// method for get all emdep site image after theme change. 
        /// </summary>
        public void GetEmdepSiteImageAfterThemeChange()
        {
            if (CompanyList.Count > 0)
            {
                for (int i = 0; i < CompanyList.Count; i++)
                {
                    if (CompanyList[i].IsSiteImageAvailable == false)
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            CompanyList[i].SiteImage = NotAvailableImagesDictionary["NotAvailable"];
                            CompanyList[i].IsSiteImageAvailable = false;
                        }
                        else
                        {
                            CompanyList[i].SiteImage = NotAvailableImagesDictionary["NotAvailableBlue"];
                            CompanyList[i].IsSiteImageAvailable = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// method for convert image from Byte to ImageSource
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage BitmapImage = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            BitmapImage.BeginInit();
            BitmapImage.StreamSource = ms;
            BitmapImage.EndInit();
            ImageSource ImageSource = BitmapImage as ImageSource;

            return ImageSource;
        }

        private ImageSource ConvertImageToGrayScaleImage(byte[] imageData)
        {
            BitmapImage BitmapImage = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            BitmapImage.BeginInit();
            BitmapImage.StreamSource = ms;
            BitmapImage.EndInit();
            FormatConvertedBitmap grayBitmap = new FormatConvertedBitmap();
            grayBitmap.BeginInit();
            grayBitmap.Source = BitmapImage;
            grayBitmap.DestinationFormat = PixelFormats.Gray8;
            grayBitmap.EndInit();
            ImageSource ImageSource = grayBitmap as ImageSource;
            return ImageSource;
        }

        /// <summary>
        /// [000][]
        /// This method is for to open Support window for generate new ticket or to open emdep Jira Helpdesk website
        /// </summary>
        /// <param name="obj"></param>
        public void OpenSupportWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("open emdep Jira Helpdesk website ....", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosAppSetting SupportSetting = WorkbenchStartUp.GetGeosAppSettings(26);
                System.Diagnostics.Process.Start(SupportSetting.DefaultValue);
                GeosApplication.Instance.Logger.Log("open emdep Jira Helpdesk website successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenSupportWindow() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On OpenSupportWindow Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenSupportWindow Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            //GeosApplication.Instance.Logger.Log("open Support Window ....", category: Category.Info, priority: Priority.Low);

            //IsBusy = true;
            //SupportWindowViewModel SupportWindowViewModel = new SupportWindowViewModel();
            //SupportWindow SupportWindow = new SupportWindow();
            //EventHandler handle = delegate { SupportWindow.Close(); };
            //SupportWindowViewModel.RequestClose += handle;
            //SupportWindowViewModel.GLPIUser = GLPIUser;

            //SupportWindow.DataContext = SupportWindowViewModel;
            //if (gLPIUser == null)
            //{
            //    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("OpenSupportWindowAlert").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //    IsBusy = false;
            //    return;
            //}

            //IsBusy = false;
            //SupportWindow.ShowDialogWindow();
            //GeosApplication.Instance.Logger.Log("open Support Window successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is for to open Support window for generate new ticket
        /// </summary>
        /// <param name="obj"></param>
        public void OpenEmdepWebSite(object obj)
        {
            GeosApplication.Instance.Logger.Log("open emdep website ....", category: Category.Info, priority: Priority.Low);
            System.Diagnostics.Process.Start("http://www.emdep.com");
            GeosApplication.Instance.Logger.Log("open emdep website successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is for to open Support window for generate new ticket
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteSingleNotification(object obj)
        {
            IsServiceActive = true;
            IsBusy = true;
            try
            {
                GeosApplication.Instance.Logger.Log("Delete selected Notification by id ....", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WorkbenchStartUp.DeleteNotificationById(Convert.ToInt16(SelectedNotification.Id));
                ViewAllNotificationAfterDeleteSingleNotification();
                GeosApplication.Instance.ServerActiveMethod();
                GeosApplication.Instance.Logger.Log("Delete selected Notification by id successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteSingleNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on DeleteSingleNotification Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteSingleNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }
        public void ClearAllNotifications()
        {
        }

        /// <summary>
        /// This method is for to open Support window for generate new ticket
        /// </summary>
        /// <param name="obj"></param>
        public void CloseWindows(object obj)
        {
            if (IsUserSignOut == true)
            {
                GC.SuppressFinalize(this);
            }
            else
            {
                if (MultipleCellEditHelper.IsValueChanged)
                {
                    Emdep.Geos.Modules.Crm.ViewModels.CrmMainViewModel objCrmMainViewModel = (Emdep.Geos.Modules.Crm.ViewModels.CrmMainViewModel)GeosApplication.Instance.ObjectPool["CrmMainViewModel"];
                    objCrmMainViewModel.SavechangesLeadOrOrder();
                    GeosApplication.Instance.ObjectPool = new Dictionary<string, object>();
                    objCrmMainViewModel = null;
                }

                if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
                {
                    Emdep.Geos.Modules.SAM.ViewModels.SAMMainViewModel objSamMainViewModel = (Emdep.Geos.Modules.SAM.ViewModels.SAMMainViewModel)GeosApplication.Instance.ObjectPool["SAMMainViewModel"];
                    objSamMainViewModel.SavechangesOrder();
                    GeosApplication.Instance.ObjectPool = new Dictionary<string, object>();
                    objSamMainViewModel = null;
                }

                Application.Current.Shutdown();
            }
        }

        public void OnViewLoaded(object obj)
        {
            if (!IsSettingChange)
            {
                if (!GeosApplication.Instance.UserSettings.ContainsKey("SelectedModule"))
                {
                    NavigationService.Navigate("Workbench.Views.DashboardView", null, this);
                }
                else
                {
                    string moduleName = Moduledictionary.Where(a => a.Key == GeosApplication.Instance.UserSettings["SelectedModule"].ToString()).FirstOrDefault().Key;
                    bool ismoduleName = GeosModuleList.Exists(x => x.Name == moduleName);

                    if (ismoduleName)
                    {
                        NavigationService.Navigate("Workbench.Views.DashboardView", null, this);
                        NavigationService.Navigate(Moduledictionary.Where(a => a.Key == GeosApplication.Instance.UserSettings["SelectedModule"].ToString()).FirstOrDefault().Value, null, this);
                    }
                    else
                    {
                        NavigationService.Navigate("Workbench.Views.DashboardView", null, this);
                    }
                }
            }

        }
        public void FillModuleDictionary()
        {
            Moduledictionary.Add("DashboardView", "Workbench.Views.DashboardView");
            Moduledictionary.Add("HARNESS PARTS", "Workbench.Views.HarnessPartsView");
            Moduledictionary.Add("WORKSHOP", "Workbench.Views.WorkshopView");
            Moduledictionary.Add("DESIGN", "Workbench.Views.DesignView");
            Moduledictionary.Add("HUMAN RESOURCES MANAGER", "Workbench.Views.HrmWindow");
            Moduledictionary.Add("WAREHOUSE MANAGEMENT SYSTEM", "Workbench.Views.WarehouseWindow");
            Moduledictionary.Add("ENGINEERING PROJECTS CONTROL", "Workbench.Views.EpcWindow");
            Moduledictionary.Add("CUSTOMER RELATIONSHIP MANAGEMENT", "Workbench.Views.CrmWindow");
            Moduledictionary.Add("PRODUCT CATALOGUE MANAGER", "Workbench.Views.PCMWindow");
            Moduledictionary.Add("STRUCTURE ASSEMBLY MANAGER", "Workbench.Views.SAMWindow");
            Moduledictionary.Add("SUPPLIER RELATIONSHIP MANAGEMENT", "Workbench.Views.SRMWindow");
            Moduledictionary.Add("PRICE LIST MANAGER", "Workbench.Views.PLMWindow");

        }

        /// <summary>
        /// This method is for to open feedback window or to open emdep Jira feedback website
        /// </summary>
        /// <param name="obj"></param>
        public void OpenFeedbackWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("open emdep Jira feedback website ....", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosAppSetting FeedbackSetting = WorkbenchStartUp.GetGeosAppSettings(27);
                System.Diagnostics.Process.Start(FeedbackSetting.DefaultValue);
                GeosApplication.Instance.Logger.Log("open emdep Jira feedback website successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenFeedbackWindow() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenFeedbackWindow Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On OpenFeedbackWindow Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            //GeosApplication.Instance.Logger.Log("open Feedback Window  ....", category: Category.Info, priority: Priority.Low);

            //IsBusy = true;
            //FeedbackWindowViewModel feedbackWindowViewModel = new FeedbackWindowViewModel();
            //if (feedbackWindowViewModel.IsInit)
            //{
            //    FeedbackWindow FeedbackWindow = new FeedbackWindow();
            //    EventHandler handle = delegate { FeedbackWindow.Close(); };
            //    feedbackWindowViewModel.GLPIUser = GLPIUser;
            //    feedbackWindowViewModel.Installedversion = Installedversion;
            //    feedbackWindowViewModel.RequestClose += handle;
            //    FeedbackWindow.DataContext = feedbackWindowViewModel;

            //    if (GLPIUser == null)
            //    {
            //        CustomMessageBox.Show(System.Windows.Application.Current.FindResource("OpenSupportWindowAlert").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //        IsBusy = false;
            //        return;
            //    }

            //    IsBusy = false;
            //    FeedbackWindow.ShowDialogWindow();
            //    GeosApplication.Instance.Logger.Log("open Feedback Window  successfully", category: Category.Info, priority: Priority.Low);
            //}
            //}
            //    catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            //    {
            //        IsBusy = false;
            //        GeosApplication.Instance.Logger.Log("Get an error On FillModuleList Method", category: Category.Exception, priority: Priority.Low);
            //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            //    }
            //    catch (Exception ex)
            //    {
            //        IsBusy = false;
            //        GeosApplication.Instance.Logger.Log(string.Format("Get an error On FillModuleList Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            //    }
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log(string.Format("Get an error to open emdep Jira feedback website ....", ex.Message), category: Category.Exception, priority: Priority.Low);
            //}
        }

        /// [001][cpatil][11-06-2020][GEOS2-1788] The notifications popup many times crashes or give wrong item count
        private void GetLatestNotifications()
        {
            IsServiceActive = true;
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;

            GeosApplication.Instance.Logger.Log("Call dispatcherTimer for get latest notification", category: Category.Info, priority: Priority.Low);
            int countNotification = 0;

            if (GeosApplication.Instance.ActiveUser != null && GeosApplication.Instance.ActiveUser.IsEnabled == 1)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Get All Unread Notification ....", category: Category.Info, priority: Priority.Low);
                     WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    UnreadNotificationsList = new List<Notification>();
                    UnreadNotificationsList = WorkbenchStartUp.GetAllUnreadNotificationTest(GeosApplication.Instance.ActiveUser.IdUser);
                    GeosApplication.Instance.Logger.Log("Get All Unread Notification successfully", category: Category.Info, priority: Priority.Low);

                    FillImageCollection(UnreadNotificationsList);

                    
                    NewNotificationCount = UnreadNotificationsList.Count;
                    if (NewNotificationCount > 0)
                    {
                        NotificationVisibility = Visibility.Visible;
                        NotificationInString = Workbench.App.Current.Resources["NotificationAlertPart1"].ToString() + " " + NewNotificationCount.ToString() + " " + Workbench.App.Current.Resources["NotificationAlertPart2"].ToString();
                    }
                    else
                    {
                        NotificationVisibility = Visibility.Hidden;
                    }

                    if (UnreadNotificationsList != null)
                    {
                        //NotificationService service = new NotificationService();
                        GeosApplication.Instance.Logger.Log("Get All Notification Count ....", category: Category.Info, priority: Priority.Low);
                        WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        //for get total notification count
                        TotalNotification = WorkbenchStartUp.GetAllNotificationCount(GeosApplication.Instance.ActiveUser.IdUser);
                        GeosApplication.Instance.Logger.Log("Get All Notification Count successfully....", category: Category.Info, priority: Priority.Low);
                     
                        foreach (Notification Notification in UnreadNotificationsList)
                        {
                            if (Notification.IsNew == 1)
                            {
                                //if (LastIdNotification < Notification.Id)
                                //{
                                //LastIdNotification = Convert.ToInt32(Notification.Id);
                                //[001] Added
                                if (!GeosApplication.Instance.NotificationsListCommon.Any(n => n.Id == Notification.Id))
                                {
                                    Notification.TimeInElapsed = TextUtility.GetDateTimeTextFormat(Notification.Time);
                                    Notification.FromUserImage = CollectionFromUserImages[Convert.ToInt64(Notification.FromUser)];
                                    GeosApplication.Instance.NotificationsListCommon.Insert(0, Notification);
                                    GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>(GeosApplication.Instance.NotificationsListCommon.OrderByDescending(a => a.Id).ToList());

                                    ShownNotificationCount = GeosApplication.Instance.NotificationsListCommon.Count;//ShownNotificationCount + NewNotificationCount;
                                    DispayNotification = (ShownNotificationCount).ToString() + "/" + TotalNotification.ToString();

                                    if (TotalNotification == 0)
                                        IsNotificationClearAllButtonEnable = false;
                                    else
                                        IsNotificationClearAllButtonEnable = true;

                                    if (ShownNotificationCount < TotalNotification)
                                    {
                                        IsNotificationViewMoreButtonEnable = true;
                                    }

                                    if (ShownNotificationCount == TotalNotification)
                                    {
                                        IsNotificationViewMoreButtonEnable = false;
                                    }
                                }
                                //}

                                //service.UseWin8NotificationsIfAvailable = false;
                                //service.PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndTwoTextFields;
                             

                                //if (countNotification < 3)
                                //{
                               //   var notification = this.GetService<INotificationService>("NotificationService").CreateCustomNotification(Notification);
                                //  notification.ShowAsync();
                                //    Notification.IsNew = 0;
                                //}

                                //countNotification++;
                            }
                        }
                        if (UnreadNotificationsList.Count > 0)
                        {
                            //List<long> notificationids = UnreadNotificationsList.Select(c => c.Id).ToList();
                            //WorkbenchStartUp.UpdateListOfNotification(notificationids);
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Get latest notification successfully by New code", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in dispatcherTimer_Tick() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    Application.Current.Dispatcher.Invoke(delegate { CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK); });
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error on dispatcherTimer_Tick Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    IsServiceActive = false;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error on dispatcherTimer_Tick Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// This method is for to open About window 
        /// </summary>
        /// <param name="obj"></param>
        public void OpenAboutWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("open About Window  ....", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            AboutWindowViewModel aboutWindowViewModel = new AboutWindowViewModel();

            if (aboutWindowViewModel.IsInit == true)
            {
                AboutWindow AboutWindow = new AboutWindow();
                EventHandler handle = delegate { AboutWindow.Close(); };
                aboutWindowViewModel.RequestClose += handle;
                AboutWindow.DataContext = aboutWindowViewModel;
                AboutWindow.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("open About Window  successfully", category: Category.Info, priority: Priority.Low);
            }

            IsBusy = false;
        }

        public void SetNotificationUserImages()
        {
            GeosApplication.Instance.Logger.Log("Set Notification User Images ....", category: Category.Info, priority: Priority.Low);
            FillImageCollection(GeosApplication.Instance.NotificationsListCommon.ToList());

            foreach (Notification Notification in GeosApplication.Instance.NotificationsListCommon)
            {
                Notification.TimeInElapsed = TextUtility.GetDateTimeTextFormat(Notification.Time);
                Notification.FromUserImage = CollectionFromUserImages[Convert.ToInt32(Notification.FromUser)];
            }
            GeosApplication.Instance.Logger.Log("Set Notification User Images Successfully....", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is for to Delete All notifiaction 
        /// </summary>
        /// <param name="obj"></param>
        public void ClearAllNotification(object obj)
        {
            try
            {
                IsServiceActive = true;
                GeosApplication.Instance.Logger.Log("Click on clear all notification", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WorkbenchStartUp.DeleteAllNotification(GeosApplication.Instance.ActiveUser.IdUser);
                MessageIndex = 0;
                ShownNotificationCount = 0;
                TotalNotification = 0;
                DispayNotification = ShownNotificationCount.ToString() + "/" + TotalNotification.ToString();
                GeosApplication.Instance.NotificationsListCommon.Clear();
                IsNotificationClearAllButtonEnable = false;
                IsNotificationViewMoreButtonEnable = false;
                GeosApplication.Instance.ServerActiveMethod();
                GeosApplication.Instance.Logger.Log("Clear all notification successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ClearAllNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on ClearAllNotification Method", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
                IsNotificationClearAllButtonEnable = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ClearAllNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to Remove one notifiaction  
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveNotification(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Delete notification ....", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WorkbenchStartUp.DeleteNotificationById(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.ServerActiveMethod();
                GeosApplication.Instance.Logger.Log("Delete notification successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RemoveNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on RemoveNotification Method", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive =  GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLeadsGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            await NotificationWorkerMethod();
        }

        private Task NotificationWorkerMethod()
        {
            return Task.Run(() =>
            {
                GetLatestNotifications();
            });
        }

        /// <summary>
        /// Method for to collect image fromUser for notification.
        /// </summary>
        /// <param name="lstNotifications"></param>
        private void FillImageCollection(List<Notification> lstNotifications)
        {
            GeosApplication.Instance.ServerActiveMethod();
            GeosApplication.Instance.Logger.Log("Get an FillImageCollection Method ", category: Category.Info, priority: Priority.Low);

            for (int i = 0; i < lstNotifications.Count; i++)
            {
                try
                {

                    if (!CollectionFromUserImages.ContainsKey(Convert.ToInt32(lstNotifications[i].FromUser)))
                    {
                        WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(lstNotifications[i].FromUser));
                        if (user != null)
                        {
                            try
                            {
                                GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
                                CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), byteArrayToImage(UserProfileImageByte));
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (user.IdUserGender != null)
                                    {
                                        if (user.IdUserGender == 1)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["FemaleUserWhite"]);
                                        else if (user.IdUserGender == 2)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["MaleUserWhite"]);
                                    }
                                    else
                                    {
                                        string name = user.FirstName.Substring(0, 1).ToUpper() + user.LastName.Substring(0, 1).ToUpper();
                                        string path = System.IO.Path.GetTempPath();

                                        if (!NotAvailableImagesDictionary.ContainsKey(name))
                                        {
                                            CreateProfilePicture(name);
                                            NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                        }

                                        CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);
                                    }
                                }
                                else
                                {
                                    if (user.IdUserGender != null)
                                    {
                                        if (user.IdUserGender == 1)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["FemaleUserBlue"]);
                                        else if (user.IdUserGender == 2)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["MaleUserBlue"]);
                                    }
                                    else
                                    {
                                        string name = user.FirstName.Substring(0, 1).ToUpper() + user.LastName.Substring(0, 1).ToUpper();
                                        string path = System.IO.Path.GetTempPath();

                                        if (!NotAvailableImagesDictionary.ContainsKey(name))
                                        {
                                            CreateProfilePicture(name);
                                            NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                        }

                                        CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);
                                    }
                                }
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    if (user.IdUserGender != null)
                                    {
                                        if (user.IdUserGender == 1)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["FemaleUserWhite"]);
                                        else if (user.IdUserGender == 2)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["MaleUserWhite"]);
                                    }
                                    else
                                    {
                                        string name = user.FirstName.Substring(0, 1).ToUpper() + user.LastName.Substring(0, 1).ToUpper();
                                        string path = System.IO.Path.GetTempPath();

                                        if (!NotAvailableImagesDictionary.ContainsKey(name))
                                        {
                                            CreateProfilePicture(name);
                                            NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                        }
                                        CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);
                                    }
                                }
                                else
                                {
                                    if (user.IdUserGender != null)
                                    {
                                        if (user.IdUserGender == 1)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["FemaleUserBlue"]);
                                        else if (user.IdUserGender == 2)
                                            CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary["MaleUserBlue"]);
                                    }
                                    else
                                    {
                                        string name = user.FirstName.Substring(0, 1).ToUpper() + user.LastName.Substring(0, 1).ToUpper();
                                        string path = System.IO.Path.GetTempPath();

                                        if (!NotAvailableImagesDictionary.ContainsKey(name))
                                        {
                                            CreateProfilePicture(name);
                                            NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                        }

                                        CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);
                                    }
                                }
                            }
                        }

                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {

                                string name = "";
                                string path = System.IO.Path.GetTempPath();

                                if (!NotAvailableImagesDictionary.ContainsKey(name))
                                {
                                    CreateProfilePicture(name);
                                    NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                }
                                CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);

                            }
                            else
                            {

                                string name = "";
                                string path = System.IO.Path.GetTempPath();

                                if (!NotAvailableImagesDictionary.ContainsKey(name))
                                {
                                    CreateProfilePicture(name);
                                    NotAvailableImagesDictionary.Add(name, GetImageAbsolute(path + name + ".Png"));
                                }

                                CollectionFromUserImages.Add(Convert.ToInt64(lstNotifications[i].FromUser), NotAvailableImagesDictionary[name]);

                            }
                        }
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillImageCollection() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillImageCollection() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillImageCollection() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        public void CreateProfilePicture(string name)
        {
            Font font = new Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, 45, System.Drawing.FontStyle.Bold);
            System.Drawing.Color fontcolor = ColorTranslator.FromHtml("#083493");
            System.Drawing.Color bgcolor = ColorTranslator.FromHtml("#FFFFFF");
            GenerateAvtarImage(name, font, fontcolor, bgcolor, "test");
        }

        private System.Drawing.Image GenerateAvtarImage(String text, Font font, System.Drawing.Color textColor, System.Drawing.Color backColor, string filename)
        {
            //first, create a dummy bitmap just to get a graphics object  
            System.Drawing.Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be  
            System.Drawing.SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object  
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size  
            img = new Bitmap(110, 110);

            drawing = Graphics.FromImage(img);

            //paint the background  
            drawing.Clear(backColor);

            //create a brush for the text  
            System.Drawing.Brush textBrush = new SolidBrush(textColor);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            drawing.DrawString(text, font, textBrush, new System.Drawing.Rectangle(0, 0, 110, 110), sf);
            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            string path = System.IO.Path.GetTempPath();

            if (!File.Exists(path + text + ".Png"))
                img.Save(path + text + ".Png");

            return img;
        }

        public Bitmap CreateBitmapImage(string sImageText, bool isImageColor)
        {
            Bitmap objBmpImage = new Bitmap(200, 200);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            System.Drawing.Font objFont = new System.Drawing.Font("Calibri", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new System.Drawing.Size(intWidth, intHeight));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            if (isImageColor == false)
                objGraphics.Clear(System.Drawing.Color.White);
            else
                objGraphics.Clear(System.Drawing.Color.CornflowerBlue);
            // objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);

            objGraphics.Flush();

            return (objBmpImage);
        }

        /// <summary>
        /// This method is to  Close window
        /// </summary>
        /// <param name="obj"></param>
        public void WorkbenchWindowClose(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// This method is to MiniMize Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void WorkbenchWindowMiniMizeButton(object obj)
        {
            CurrentWindowState = WindowState.Normal;
            RestoreButtonVisibility = Visibility.Hidden;
            MaximizeButtonVisibility = Visibility.Visible;
        }

        /// <summary>
        /// This method is to MaxiMize Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void WorkbenchWindowMaxiMizeButton(object obj)
        {
            CurrentWindowState = WindowState.Minimized;
        }

        /// <summary>
        ///  This method is to Restor Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void RestorButton(object obj)
        {
            CurrentWindowState = WindowState.Maximized;
            RestoreButtonVisibility = Visibility.Visible;
            MaximizeButtonVisibility = Visibility.Hidden;
        }

        /// <summary>
        /// This method is for open Edit User profile Window
        /// </summary>
        /// <param name="obj"></param>
        public void OpenEditProfile(object obj)
        {
            GeosApplication.Instance.Logger.Log("Open EditProfile Window ....", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            EditProfile editProfile = new EditProfile();
            EditProfileViewModel EditProfileViewModel = new EditProfileViewModel();
            EventHandler handle = delegate { editProfile.Close(); };
            EditProfileViewModel.RequestClose += handle;
            editProfile.DataContext = EditProfileViewModel;

            IsBusy = false;
            editProfile.ShowDialogWindow();
            SetUserProfileImage();
            GeosApplication.Instance.Logger.Log("Open EditProfile Window successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is for to ShowSettings Window
        /// </summary>
        /// <param name="obj"></param>
        public void ShowSettingsWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Open Settings Window ....", category: Category.Info, priority: Priority.Low);
            SettingWindowViewModel SettingWindowViewModel = new SettingWindowViewModel();

            if (SettingWindowViewModel.IsInit == true)
            {
                IsBusy = true;
                SettingWindow settingWindow = new SettingWindow();
                EventHandler handle = delegate { settingWindow.Close(); };
                SettingWindowViewModel.RequestClose += handle;
                settingWindow.DataContext = SettingWindowViewModel;
                //settingWindow.Owner = settingWindow;
                settingWindow.ShowDialogWindow();
                IsNotificationNumberChange = SettingWindowViewModel.IsNotificationChange;
                IsThemeChange = SettingWindowViewModel.IsThemeChange;
                IsLanguagteChange = SettingWindowViewModel.IsLanguagteChange;

                if (GeosApplication.Instance.UserSettings.ContainsKey("IsServiceIconShow")
                && GeosApplication.Instance.UserSettings.ContainsKey("ServiceRefreshSeconds")
                && bool.Parse(GeosApplication.Instance.UserSettings["IsServiceIconShow"].ToString())
                && GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString() != string.Empty
                && int.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString()) > 0)
                {
                    ServiceIndicatorVisibility = Visibility.Visible;
                    dateTimeTimer.Stop();
                    dateTimeTimer.Interval = new TimeSpan(0, 0, 0, int.Parse(GeosApplication.Instance.UserSettings["ServiceRefreshSeconds"].ToString()), 0);
                    dateTimeTimer.Start();
                }
                else
                {
                    ServiceIndicatorVisibility = Visibility.Hidden;
                    dateTimeTimer.Stop();
                    dateTimeTimer.Interval = new TimeSpan(0, 0, 1, 0, 0);
                    dateTimeTimer.Start();
                }

                if (IsThemeChange)
                {
                    SetTileImageBackground();
                    CollectionFromUserImages.Clear();
                    SetNotificationUserImages();
                    GetEmdepSiteImageAfterThemeChange();
                    GeosApplication.Instance.FontFamilyAsPerTheme = new System.Windows.Media.FontFamily(GeosApplication.Instance.UIThemeList.Where(uithe => uithe.ThemeName == ApplicationThemeHelper.ApplicationThemeName).Select(ui => ui.FontFamily).FirstOrDefault());
                }

                if (IsLanguagteChange)
                {
                    SetServerTimeLabel();
                }

                SetUserProfileImage();
                IsSettingChange = true;
                IsThemeChange = false;
                IsBusy = false;
            }

            GeosApplication.Instance.Logger.Log("Open Settings Window successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is for to Close Change Password Window
        /// </summary>
        /// <param name="obj"></param>
        public void ChangePasswordCloseButton(object obj)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            RequestClose(null, null);
        }

        /// <summary>
        /// This method is for to Close Setting Window
        /// </summary>
        /// <param name="obj"></param>
        public void SettingsCloseButton(object obj)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            RequestClose(null, null);
        }

        /// <summary>
        /// This method is for to User SignOut
        /// </summary>
        /// <param name="obj"></param>
        public void UserSignOut(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on signOut button ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            IsUserSignOut = true;

            //GC.Collect();
            //GC.WaitForFullGCApproach(100);
            //GC.WaitForPendingFinalizers();

            if (MultipleCellEditHelper.IsValueChanged)
            {
                Emdep.Geos.Modules.Crm.ViewModels.CrmMainViewModel objCrmMainViewModel = (Emdep.Geos.Modules.Crm.ViewModels.CrmMainViewModel)GeosApplication.Instance.ObjectPool["CrmMainViewModel"];
                objCrmMainViewModel.SavechangesLeadOrOrder();
                GeosApplication.Instance.ObjectPool = new Dictionary<string, object>();
                objCrmMainViewModel = null;
            }

            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                Emdep.Geos.Modules.SAM.ViewModels.SAMMainViewModel objSamMainViewModel = (Emdep.Geos.Modules.SAM.ViewModels.SAMMainViewModel)GeosApplication.Instance.ObjectPool["SAMMainViewModel"];
                objSamMainViewModel.SavechangesOrder();
                GeosApplication.Instance.ObjectPool = new Dictionary<string, object>();
                objSamMainViewModel = null;
            }

            RequestClose(null, null);

            //GC.Collect(0);
            //GC.SuppressFinalize(this);

            LoginWindow loginWindow = new LoginWindow();
            LoginWindowViewModel loginWindowViewModel = new LoginWindowViewModel();

            if (LoginWorkstation == null)
            {
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
            }
            else if (LoginWorkstation != null)
            {
                if (LoginWorkstation.IsManufacturingStation == 0)   //Office
                {
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

                    loginWindowViewModel.SelectedViewIndex = 0;
                    loginWindowViewModel.ShutDownButtonVisibility = Visibility.Hidden;
                }
                if (LoginWorkstation.IsManufacturingStation == 1)//production
                {
                    loginWindowViewModel.PageViewItemHeight = 0;
                    loginWindowViewModel.SelectedViewIndex = 1;
                    loginWindowViewModel.ShutDownButtonVisibility = Visibility.Visible;
                }
            }

            loginWindowViewModel.VersionTitle = Installedversion;
            loginWindow.DataContext = loginWindowViewModel;
            EventHandler handle = delegate { loginWindow.Close(); };
            loginWindowViewModel.RequestClose += handle;

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Properties.Settings.Default.UserSessionId = 0;
            Properties.Settings.Default.Save();

            //[Start update setting] code for update usersetting if user signout then remove save user id from setting.

            if (!string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["UserSessionDetail"].ToString()))
            {
                GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                GeosApplication.Instance.UserSettings["UserSessionDetail"] = string.Empty;

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            }

            //[End update setting]

            IsBusy = false;
            loginWindow.ShowDialog();
            Tempclose();

            GeosApplication.Instance.Logger.Log("SignOut successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for get and set User Image.
        /// </summary>
        public void SetUserProfileImage()
        {
            User user = new User();

            try
            {
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(GeosApplication.Instance.ActiveUser.Login);
                UserProfileImage = byteArrayToImage(UserProfileImageByte);
                EditUserProfileImage = byteArrayToImage(UserProfileImageByte);
                ImpersonateUserProfileImage = byteArrayToImage(UserProfileImageByte);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    if (user.IdUserGender != null)
                    {
                        if (user.IdUserGender == 1)
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/femaleUserBlue.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/maleUserBlue.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                        }
                    }
                }
                else
                {
                    if (user.IdUserGender != null)
                    {
                        if (user.IdUserGender == 1)
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/femaleUserWhite.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/maleUserWhite.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                        }
                    }

                    if (DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Close();
                    }
                    GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method Added difaul Image of User " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/UserProfile.png", UriKind.RelativeOrAbsolute));

                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    if (user.IdUserGender != null)
                    {
                        if (user.IdUserGender == 1)
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/femaleUserBlue.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/maleUserBlue.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                        }
                    }
                }
                else
                {
                    if (user.IdUserGender != null)
                    {
                        if (user.IdUserGender == 1)
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/femaleUserWhite.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            UserProfileImage = ConvertImageToImageSource("/Assets/Images/maleUserWhite.png");
                            ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                            EditUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLeadsGrid() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  This method is to Selectc Default Site By User
        /// </summary>
        /// <param name="obj"></param>
        public void SelectcDefaultSite()
        {
            try
            {
                EmdepSitesMapWindowViewModel emdepSitesMapWindowViewModel = new EmdepSitesMapWindowViewModel();
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                CompanyList = new List<Company>();
                CompanyList = WorkbenchStartUp.GetCompanyList();

                if (CompanyList != null)
                    GeosApplication.Instance.EmdepSiteList = CompanyList;

                CompanyList = CompanyList.OrderBy(ik => ik.IdCountry).ToList();

                if (CompanyList != null)
                    GeosApplication.Instance.EmdepSiteList = CompanyList;

                CompanyList = CompanyList.OrderBy(ik => ik.IdCountry).ToList();

                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId(GeosApplication.Instance.ActiveUser.IdUser);
                CompanyList = CompanyList.Select(s =>
                {
                    s.IsPermission = companysuserwise.Any(p2 => p2.IdCompany == s.IdCompany); return s;

                }).ToList();

                for (int i = 0; i < CompanyList.Count; i++)
                {
                    if (CompanyList[i].Alias == GeosApplication.Instance.SiteName)
                    {
                        GeosApplication.Instance.SiteName = CompanyList[i].Alias;
                        CompanyAlias = GeosApplication.Instance.SiteName;
                        emdepSitesMapWindowViewModel.ActiveItemIndexValue = i;
                        break;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectcDefaultSite() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectcDefaultSite() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectcDefaultSite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to Update Notifications
        /// [001][cpatil][11-06-2020][GEOS2-1788] The notifications popup many times crashes or give wrong item count
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateUserNotification(object obj)
        {
            
            GeosApplication.Instance.Logger.Log("Update user Notification", category: Category.Info, priority: Priority.Low);
            IsServiceActive = true;
            if (NewNotificationCount > 0)
            {
                try
                {
                    WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    //[001] Added
                    bool isupdated =  WorkbenchStartUp.UpdateUserNotification_V2043(GeosApplication.Instance.ActiveUser.IdUser);
                     GeosApplication.Instance.ServerActiveMethod();
                    GeosApplication.Instance.Logger.Log("Update user Notification successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in UpdateUserNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error UpdateUserNotification Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    IsServiceActive = GeosApplication.Instance.IsServiceActive;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error UpdateUserNotification Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    IsServiceActive = GeosApplication.Instance.IsServiceActive;
                }
            }

            if (IsNotificationNumberChange == true)
            {
                GeosApplication.Instance.NotificationsListCommon.Clear();
                LastIdNotification = long.MaxValue;
                ShownNotificationCount = 0;
                ViewAllNotification(null);
                IsNotificationNumberChange = false;
            }

            if (NewNotificationCount > 0)
            {
                NotificationInString = System.Windows.Application.Current.FindResource("NotificationAlertStart").ToString() + " " + NewNotificationCount.ToString() + " " + System.Windows.Application.Current.FindResource("NotificationAlertEnd").ToString();
            }
            else
            {
                NotificationImage = NotAvailableImagesDictionary["NotAvailable"];
                NotificationInString = System.Windows.Application.Current.FindResource("NotificationInString").ToString();
            }

            if (NewNotificationCount > 0)
            {
                try
                {
                    SetNotificationUserImages();
                    NotificationVisibility = Visibility.Hidden;
                    GeosApplication.Instance.ServerActiveMethod();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in UpdateUserNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error UpdateUserNotification Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    IsServiceActive = GeosApplication.Instance.IsServiceActive;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in UpdateUserNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>(GeosApplication.Instance.NotificationsListCommon.OrderByDescending(a => a.Id).ToList());
        }

        /// <summary>
        /// This method is for to Get user's all Unread  Notification
        /// </summary>
        public async void GetAllUnreadNotification()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Get unread Notification successfully", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                UnreadNotificationsList = WorkbenchStartUp.GetAllUnreadNotificationTest(GeosApplication.Instance.ActiveUser.IdUser);
                var query = await WorkbenchStartUp.GetAllUnreadNotificationAsync(GeosApplication.Instance.ActiveUser.IdUser);
                List<Notification> data = query;
                GeosApplication.Instance.ServerActiveMethod();

                GeosApplication.Instance.Logger.Log("Get unread Notification successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetAllUnreadNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error unread GetAllUnreadNotification Method", category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllUnreadNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill notification as per per page NotificationPageCount 
        /// [001][cpatil][11-06-2020][GEOS2-1788] The notifications popup many times crashes or give wrong item count
        /// </summary>
        /// <param name="obj"></param>
        public async void ViewAllNotification(object obj)
        {
            GeosApplication.Instance.Logger.Log("Get Notification as per page show number ...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            IsServiceActive = true;
            int page = 5;

            if (GeosApplication.Instance.UserSettings.ContainsKey("NotificationPageCount"))
            {
                page = int.Parse(GeosApplication.Instance.UserSettings["NotificationPageCount"].ToString());
            }

            try
            {
                ObservableCollection<Notification> TempNotificationsList;

                if (page < 0)
                    page = 5;
                GeosApplication.Instance.Logger.Log("Get All Notification in view all notification ...", category: Category.Info, priority: Priority.Low);
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                var query = await WorkbenchStartUp.GetNotificationWithRecordIdTestAsync(GeosApplication.Instance.ActiveUser.IdUser, LastIdNotification, true, 0, page, "Id", OrderBy.Descending);
                TempNotificationsList = new ObservableCollection<Notification>(query);
                GeosApplication.Instance.Logger.Log("Get All Notification in view all notification successfully", category: Category.Info, priority: Priority.Low);

                if (TempNotificationsList != null)
                {

                    foreach (Notification Notification in TempNotificationsList)
                    {
                        //[001] Added
                        if (!GeosApplication.Instance.NotificationsListCommon.Any(n => n.Id == Notification.Id))
                        {
                            GeosApplication.Instance.NotificationsListCommon.Add(Notification);
                            LastIdNotification = Notification.Id;
                        }

                    }


                    ShownNotificationCount = GeosApplication.Instance.NotificationsListCommon.Count;    // ShownNotificationCount + TempNotificationsList.Count;
                    GeosApplication.Instance.Logger.Log("Get All Notification Count in view all notification ...", category: Category.Info, priority: Priority.Low);
                    WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    TotalNotification = WorkbenchStartUp.GetAllNotificationCount(GeosApplication.Instance.ActiveUser.IdUser);
                    GeosApplication.Instance.Logger.Log("Get All Notification Count in view all notification successfully", category: Category.Info, priority: Priority.Low);
                    DispayNotification = ShownNotificationCount.ToString() + "/" + TotalNotification.ToString();
                    SetNotificationUserImages();

                    GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>(GeosApplication.Instance.NotificationsListCommon.OrderByDescending(a => a.Id).ToList());

                    if (GeosApplication.Instance.NotificationsListCommon.Count > 0)
                    {
                        SelectedNotification = GeosApplication.Instance.NotificationsListCommon[0];
                    }

                    if (ShownNotificationCount < TotalNotification)
                    {
                        IsNotificationViewMoreButtonEnable = true;
                    }

                    if (ShownNotificationCount == TotalNotification)
                    {
                        IsNotificationViewMoreButtonEnable = false;
                    }

                    if (TotalNotification == 0)
                        IsNotificationClearAllButtonEnable = false;
                    else
                        IsNotificationClearAllButtonEnable = true;

                }
        
                GeosApplication.Instance.ServerActiveMethod();
             
                GeosApplication.Instance.Logger.Log("Get Notification as per page show number  successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ViewAllNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                Application.Current.Dispatcher.Invoke(delegate { CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK); });
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewAllNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Application.Current.Dispatcher.Invoke(delegate { GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null); });

                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewAllNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Method for refresh notification list after delete single Notification
        /// </summary>
        /// <param name="obj"></param>
        public void ViewAllNotificationAfterDeleteSingleNotification()
        {
            IsBusy = true;
            IsServiceActive = true;
            GeosApplication.Instance.Logger.Log("Get Notification after delete one ...", category: Category.Info, priority: Priority.Low);
            try
            {
                LastIdNotification = long.MaxValue;
                GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>();
                ObservableCollection<Notification> TempNotificationsList;
                WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                TempNotificationsList = new ObservableCollection<Notification>(WorkbenchStartUp.GetNotificationWithRecordId(GeosApplication.Instance.ActiveUser.IdUser, LastIdNotification, true, 0, (ShownNotificationCount > 0 ? ShownNotificationCount - 1 : 0), "Id", OrderBy.Descending));

                if (TempNotificationsList != null)  //&& TempNotificationsList.Count >0
                {
                    foreach (Notification Notification in TempNotificationsList)
                    {
                        if (!GeosApplication.Instance.NotificationsListCommon.Any(n => n.Id == Notification.Id))
                        {
                            GeosApplication.Instance.NotificationsListCommon.Add(Notification);
                            LastIdNotification = Notification.Id;
                        }
                       
                    }

                    ShownNotificationCount = GeosApplication.Instance.NotificationsListCommon.Count;
                    WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    TotalNotification = WorkbenchStartUp.GetAllNotificationCount(GeosApplication.Instance.ActiveUser.IdUser);
                    DispayNotification = ShownNotificationCount.ToString() + "/" + TotalNotification.ToString();
                    SetNotificationUserImages();
                    GeosApplication.Instance.NotificationsListCommon = new ObservableCollection<Notification>(GeosApplication.Instance.NotificationsListCommon.OrderByDescending(a => a.Id).ToList());

                    if (GeosApplication.Instance.NotificationsListCommon.Count > 0)
                        SelectedNotification = GeosApplication.Instance.NotificationsListCommon[0];

                    if (ShownNotificationCount == TotalNotification)
                    {
                        IsNotificationViewMoreButtonEnable = false;
                    }

                    if (TotalNotification == 0)
                        IsNotificationClearAllButtonEnable = false;
                    else
                        IsNotificationClearAllButtonEnable = true;
                }

                GeosApplication.Instance.Logger.Log("Get Notification after delete one successfully", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ServerActiveMethod();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ViewAllNotificationAfterDeleteSingleNotification() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on ViewAllNotificationAfterDeleteSingleNotification Method "+ ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = GeosApplication.Instance.IsServiceActive;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewAllNotificationAfterDeleteSingleNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Method for  ImageSource To Bytes
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }

        public byte[] BitmapToByteArray(Image img)
        {
            byte[] byteArray = null;
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }


        public void Tempclose()
        {
            // GC.SuppressFinalize(this);
            GC.Collect();
        }

        #endregion

        ~WorkbenchWindowViewModel()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

    }
}
