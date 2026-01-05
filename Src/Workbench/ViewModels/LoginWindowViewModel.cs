using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Emdep.Geos.Utility.Text;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Workbench.Views;


namespace Workbench.ViewModels
{
    class LoginWindowViewModel : INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");

        #endregion  // Services

        #region Declaration

        private int selectedViewIndex = 0;  //  Selected View for as per windows 
        private int selectedIndexImpersonateUser = 0;   //Selected Index for ImpersonateUser
        //public System.Windows.Forms.Timer tmrDelay;     //Timer for send mail 
        public static string GeneratePasswordCode { get; set; }     // Generate Password
        private string txtRecoveryPassword = string.Empty;          //RecoveryPassword
        private bool isForgotPasswordbtnNext;   // Forgot Password NextButton 
        private string loginName;               //login Name
        private string loginNameErrorColor;    //[nsatpute][01.09.2025][GEOS2-9342]
        private string loginPassword;           // login Password
        private string loginPasswordErrorColor;  //[nsatpute][01.09.2025][GEOS2-9342]
        private string newPassword;             //New Password
        private string confirmPassword;         // Confirm Password
        private string userNameError;           // forgot password user name error
        private string passwordStrenthColor;    // New Password check password strenth
        private string userEmailOrUserName;     //User Email Or UserName
        private bool isRememberMe;              //Remember me in this computer
        private string productionUserLoginCode; // Production User LoginCode
        private int pageViewItemHeight;         //PageView Item Height

        private Visibility shutDownButtonVisibility;                        //ShutDown Button Visibility      
        private Visibility sendMailWaitImageVisibility = Visibility.Hidden; //Send Mail Wait Image Visibility 
        private Visibility sendMailErrorVisibility = Visibility.Hidden;     //Send Mail Error Visibility
        private Visibility maxiMizeButtonVisibility = Visibility.Hidden;    //MaxiMize Button Visibility
        private Visibility restoreButtonVisibility = Visibility.Hidden;     //Restore Button Visibility
        private Visibility minMaxClosePanelVisibility;                      //MinMaxClosePanel Visibility;
        private Visibility sendMailMessageVisibility = Visibility.Hidden;   //Send Mail Message Visibility
        private Visibility shutDownButtonVisible;
        private List<string> loginUserList;                                 // List loginUser

        private ObservableCollection<string> impersonateUserList = new ObservableCollection<string>();

        public ObservableCollection<string> ImpersonateUserList
        {
            get { return impersonateUserList; }
            set
            {
                impersonateUserList = value;
                //SetProperty(ref impersonateUserList, value, () => ImpersonateUserList);
            }
        }
        private string loginImpersonateName;//login Impersonate Name
        private string loginImpersonateNameErrorColor; //[nsatpute][01.09.2025][GEOS2-9342]
        private List<User> userList;//List user
        private WindowState currentWindowState;//current WindowState
        Thread threadSendMail;//thread of SendMail
        public event EventHandler RequestClose;
        public System.Windows.Forms.DialogResult DialogResult { get; set; }
        public string MessageCopyright { get; set; }
        public string VersionTitle { get; set; }
        private string passwordStrenth; // New Password check password strenth
        //public static string UserId;
        private string deskTopBackgroundImagePath;
        private string desktopBackgroungColor;
        private string officeUserLoginViewBackgroungColor;
        public Workstation LoginWorkstation { get; set; }
        private User user;
        //public int CountTime { get; set; }
        private bool isBusy;
        private bool isServiceActive = true;
        private Visibility serviceIndicatorVisibility = Visibility.Visible;
        private string myIp;
        private string workbenchLocation;
        private GeosWorkbenchVersion geosWorkbenchVersionNumber;
        private Version version;
        private bool _showPassword;//chitra.girigosavi GEOS2-7914 02/07/2025
        private Visibility _isImpersonateViewVisible = Visibility.Hidden;
		//[nsatpute][01.09.2025][GEOS2-9342]
        private Visibility showUserNameLabel = Visibility.Hidden;
        private Visibility showLoginImpersonateNameLabel = Visibility.Hidden; //[nsatpute][01.09.2025][GEOS2-9342]
        private Visibility showPasswordLabel = Visibility.Hidden;
		//[nsatpute][03.09.2025][GEOS2-9342]
        public string this[string columnName]
        {
            get
            {
                if (IsImpersonateViewVisible == Visibility.Visible)
                {
                    switch (columnName)
                    {
                        case nameof(LoginName):
                            if (string.IsNullOrWhiteSpace(LoginImpersonateName))
                            {

                                return "Login name is required";
                            }
                            break;
                    }
                }
                else
                {
                    switch (columnName)
                    {
                        case nameof(LoginName):
                            if (string.IsNullOrWhiteSpace(LoginName))
                            {

                                return "Login name is required";
                            }
                            break;

                        case nameof(LoginPassword):
                            if (string.IsNullOrWhiteSpace(LoginPassword))
                                return "Password is required";
                            break;
                    }
                }
                return null;
            }
        }

        public string Error => null; // Not used

        public bool IsValid
        {
            get
            {
                if (IsImpersonateViewVisible == Visibility.Visible)
                {
                    return string.IsNullOrEmpty(this[nameof(LoginName)]);
                }
                else
                {
                    return string.IsNullOrEmpty(this[nameof(LoginName)]) &&
                           string.IsNullOrEmpty(this[nameof(LoginPassword)]);
                }
            }
        }
        #endregion  // Declaration

        #region Enum

        /// <summary>
        /// dwfine Enum for check password strenth 
        /// </summary>
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        #endregion  // Declaration

        #region  public Properties
        public string MyIP
        {
            get { return myIp; }
            set { myIp = value; OnPropertyChanged(new PropertyChangedEventArgs("MyIP")); }
        }
        public string WorkbenchLocation
        {
            get { return workbenchLocation; }
            set { workbenchLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("WorkbenchLocation")); }
        }
        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
        }
        public Version Version
        {
            get { return version; }
            set { version = value; OnPropertyChanged(new PropertyChangedEventArgs("Version")); }
        }
        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                if (selectedViewIndex == 1)
                {
                    ShutDownButtonVisible = Visibility.Visible;
                }
                else
                {
                    ShutDownButtonVisible = Visibility.Hidden;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }

        public int SelectedIndexImpersonateUser
        {
            get { return selectedIndexImpersonateUser; }
            set
            {
                selectedIndexImpersonateUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexImpersonateUser"));
            }
        }

        public int PageViewItemHeight
        {
            get { return pageViewItemHeight; }
            set
            {
                pageViewItemHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageViewItemHeight"));
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        public string LoginName
        {
            get { return loginName; }
            set {   loginName = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginName"));
                if (LoginName == null)
                    ShowUserNameLabel = Visibility.Hidden;
                else
                    ShowUserNameLabel = Visibility.Visible;

                SetLoginNameErrorCode();
            }
        }


        //[nsatpute][01.09.2025][GEOS2-9342]
        public string LoginNameErrorColor
        {
            get { return loginNameErrorColor; }
            set
            {
                loginNameErrorColor = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginNameErrorColor"));
                
            }
        }


        //[nsatpute][01.09.2025][GEOS2-9342]
        public string LoginPassword
        {
            get { return loginPassword; }
            set { loginPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginPassword"));

                if (LoginPassword == null)
                    ShowPasswordLabel = Visibility.Hidden;
                else
                    ShowPasswordLabel = Visibility.Visible;

                SetLoginPasswordErrorCode();
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        public string LoginPasswordErrorColor
        {
            get { return loginPasswordErrorColor; }
            set
            {
                loginPasswordErrorColor = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginPasswordErrorColor"));
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("NewPassword")); }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("ConfirmPassword")); }
        }

        public string UsrEmailOrUserName
        {
            get { return userEmailOrUserName; }
            set { userEmailOrUserName = value; OnPropertyChanged(new PropertyChangedEventArgs("UsrEmailOrUserName")); }
        }

        public string TxtRecoveryPassword
        {
            get { return txtRecoveryPassword; }
            set { txtRecoveryPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("TxtRecoveryPassword")); }
        }

        public string ProductionUserLoginCode
        {
            get { return productionUserLoginCode; }
            set
            {
                productionUserLoginCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionUserLoginCode"));
            }
        }

        public string LoginImpersonateName
        {
            get { return loginImpersonateName; }
            set { loginImpersonateName = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginImpersonateName"));
				//[nsatpute][01.09.2025][GEOS2-9342]
                if (LoginImpersonateName == null)
                    ShowLoginImpersonateNameLabel = Visibility.Hidden;
                else
                    ShowLoginImpersonateNameLabel = Visibility.Visible;

                SetLoginPersonateNameErrorCode();
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        public string LoginImpersonateNameErrorColor
        {
            get { return loginImpersonateNameErrorColor; }
            set
            {
                loginImpersonateNameErrorColor = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginImpersonateNameErrorColor"));

            }
        }

        public bool IsForgotPasswordbtnNext
        {
            get { return isForgotPasswordbtnNext; }
            set { isForgotPasswordbtnNext = value; OnPropertyChanged(new PropertyChangedEventArgs("IsForgotPasswordbtnNext")); }
        }

        public bool IsRememberMe
        {
            get { return isRememberMe; }
            set { isRememberMe = value; OnPropertyChanged(new PropertyChangedEventArgs("IsRememberMe")); }
        }

        public List<string> LoginUserList
        {
            get { return loginUserList; }
            set { loginUserList = value; OnPropertyChanged(new PropertyChangedEventArgs("LoginUserList")); }
        }

        public List<User> UserList
        {
            get { return userList; }
            set { userList = value; OnPropertyChanged(new PropertyChangedEventArgs("UserList")); }
        }

        //public List<string> ImpersonateUserList
        //{
        //    get { return impersonateUserList; }
        //    set { impersonateUserList = value; OnPropertyChanged(new PropertyChangedEventArgs("ImpersonateUserList")); }
        //}



        public Visibility SendMailMessageVisibility
        {
            get { return sendMailMessageVisibility; }
            set { sendMailMessageVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("SendMailMessageVisibility")); }
        }

        public Visibility SendMailWaitImageVisibility
        {
            get { return sendMailWaitImageVisibility; }
            set { sendMailWaitImageVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("SendMailWaitImageVisibility")); }
        }

        public Visibility MaximizeButtonVisibility
        {
            get { return maxiMizeButtonVisibility; }
            set { maxiMizeButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("MaximizeButtonVisibility")); }
        }

        public Visibility RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set { restoreButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("RestoreButtonVisibility")); }
        }

        public Visibility ShutDownButtonVisibility
        {
            get { return shutDownButtonVisibility; }
            set { shutDownButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("ShutDownButtonVisibility")); }
        }

        public Visibility SendMailErrorVisibility
        {
            get { return sendMailErrorVisibility; }
            set { sendMailErrorVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("SendMailErrorVisibility")); }
        }

        public Visibility MinMaxClosePanelVisibility
        {
            get { return minMaxClosePanelVisibility; }
            set { minMaxClosePanelVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("MinMaxClosePanelVisibility")); }
        }
        public WindowState CurrentWindowState
        {
            get
            { return currentWindowState; }
            set
            { currentWindowState = value; OnPropertyChanged(new PropertyChangedEventArgs("CurrentWindowState")); }
        }

        public string PasswordStrenth
        {
            get { return passwordStrenth; }
            set { passwordStrenth = value; OnPropertyChanged(new PropertyChangedEventArgs("PasswordStrenth")); }
        }

        public string PasswordStrenthColor
        {
            get { return passwordStrenthColor; }
            set { passwordStrenthColor = value; OnPropertyChanged(new PropertyChangedEventArgs("PasswordStrenthColor")); }
        }

        public string UserNameError
        {
            get { return userNameError; }
            set { userNameError = value; OnPropertyChanged(new PropertyChangedEventArgs("UserNameError")); }
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public string DeskTopBackgroundImagePath
        {
            get
            { return deskTopBackgroundImagePath; }

            set
            { deskTopBackgroundImagePath = value; OnPropertyChanged(new PropertyChangedEventArgs("DeskTopBackgroundImagePath")); }
        }

        public string DesktopbackgroungColor
        {
            get
            {
                return desktopBackgroungColor;
            }
            set
            {
                desktopBackgroungColor = value; OnPropertyChanged(new PropertyChangedEventArgs("DesktopbackgroungColor"));
            }
        }

        public string OfficeUserLoginViewBackgroungColor
        {
            get
            {
                return officeUserLoginViewBackgroungColor;
            }
            set
            {
                officeUserLoginViewBackgroungColor = value; OnPropertyChanged(new PropertyChangedEventArgs("OfficeUserLoginViewBackgroungColor"));
            }
        }
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public bool IsServiceActive
        {
            get
            {
                return isServiceActive;
            }

            set
            {
                isServiceActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsServiceActive"));
            }
        }

        public Visibility ServiceIndicatorVisibility
        {
            get
            {
                return serviceIndicatorVisibility;
            }

            set
            {
                serviceIndicatorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServiceIndicatorVisibility"));
            }
        }

        public Visibility ShutDownButtonVisible
        {
            get
            {
                return shutDownButtonVisible;
            }

            set
            {
                shutDownButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShutDownButtonVisible"));
            }
        }
        //chitra.girigosavi GEOS2-7914 02/07/2025
        public bool ShowPassword
        {
            get
            {
                return _showPassword;
            }
            set
            {
                _showPassword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowPassword"));
            }
        }

        //chitra.girigosavi GEOS2-7914 02/07/2025
        public Visibility IsImpersonateViewVisible
        {
            get { return _isImpersonateViewVisible; }
            set
            {
                _isImpersonateViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsImpersonateViewVisible)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoginViewVisible)));
            }
        }

        //chitra.girigosavi GEOS2-7914 02/07/2025
        public Visibility IsLoginViewVisible
        {
            get
            {
                if (_isImpersonateViewVisible == Visibility.Visible)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        public Visibility ShowLoginImpersonateNameLabel
        {
            get { return showLoginImpersonateNameLabel; }
            set
            {
                showLoginImpersonateNameLabel = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShowLoginImpersonateNameLabel)));
            }
        }

        public Visibility ShowUserNameLabel
        {
            get { return showUserNameLabel; }
            set
            {
                showUserNameLabel = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShowUserNameLabel)));
            }
        }

        //[nsatpute][01.09.2025][GEOS2-9342]
        public Visibility ShowPasswordLabel
        {
            get { return showPasswordLabel; }
            set
            {
                showPasswordLabel = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShowPasswordLabel)));
            }
        }

        #endregion  // Properties

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Command

        public ICommand LoginAcceptButtonCommand { get; set; } //Accept on Login page
        public ICommand LoginCloseButtonCommand { get; set; } //Close on Login page
        public ICommand ForgotPasswordButtonCommand { get; set; } //ForgotPassword on Login page 
        public ICommand ForgotPasswordBackButtonCommand { get; set; } //Back on ForgotPassword page
        public ICommand ForgotPasswordNextButtonCommand { get; set; } //Next on ForgotPassword page
        public ICommand ForgotPasswordsendMailButtonCommand { get; set; } //Cancel on  NewPassrowd page
        public ICommand ForgotPasswordRecoveryPasswordtxtCommand { get; set; }//Recover password textbox on ForgotPassword page
        public ICommand NewPassrowdAcceptButtonCommand { get; set; } //Accept on  NewPassrowd page
        public ICommand NewPassrowdCancelButtonCommand { get; set; } //Cancel on  NewPassrowd page
        public ICommand ProductionUserLoginCodeCommand { get; set; }// Barcode Textbox on productionUserLogin
        public ICommand LoginImpersonateAcceptButtonCommand { get; set; }//Login Impersonate Accept Button
        public ICommand LoginImpersonateCancelButtonCommand { get; set; }//Login Impersonate Cancel Button      
        public ICommand LoginMiniMizeButtonCommand { get; set; }//Login minimize Button
        public ICommand LoginMaxiMizeButtonCommand { get; set; }//Login maximize Button
        public ICommand RestorButtonCommand { get; set; }// Login Restore Button
        public ICommand LoginshutdownButtonCommand { get; set; } //Close on Login page
        public ICommand EmdepSitesWindowCommand { get; set; } //Emdep Sites Window
        public ICommand EmdepSitesMapWindowCommand { get; set; } //Emdep Sites Map Window
        public ICommand NewPasswordCodeCommand { get; set; }
        public ICommand LoginImpersonateEscapeActionCommand { get; set; } //Close on Login page
        public ICommand PageUnLoadedCommand { get; set; } //Close on Login page
        public ICommand NeedHelpCommand { get; set; } //Open Help link //chitra.girigosavi GEOS2-7914 25/06/2025
        #endregion

        #region Constructor

        //private RelayCommand m_command;
        //public RelayCommand M_command
        //{
        //    get { return m_command; }
        //    set
        //    {
        //        if (m_command == value)
        //            return;
        //        m_command = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("M_command"));
        //    }
        //}
        public LoginWindowViewModel()
        {
            string _currentYear = GeosApplication.Instance.ServerDateTime.Year.ToString();

            MessageCopyright = string.Format("Copyright  2003-{0} EMDEP Engineering.  ", _currentYear);// "Copyright  2003-2015 EMDEP Development Team.  ";
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

            CurrentWindowState = WindowState.Maximized;
            RestoreButtonVisibility = Visibility.Visible;
            if (Properties.Settings.Default.OfflineUser == null)
            {
                Properties.Settings.Default.OfflineUser = new List<string>();
            }

            LoginUserList = Properties.Settings.Default.OfflineUser.ToList();
            //M_command = new RelayCommand(new Action<object>(ShowForgotPassword));


            ForgotPasswordButtonCommand = new RelayCommand(new Action<object>(ShowForgotPassword));
            ForgotPasswordBackButtonCommand = new RelayCommand(new Action<object>(BackForgotPasswordAction));
            ForgotPasswordNextButtonCommand = new RelayCommand(new Action<object>(NextForgotPasswordAction));
            ForgotPasswordsendMailButtonCommand = new RelayCommand(new Action<object>(SendForgotPasswordMail));
            ForgotPasswordRecoveryPasswordtxtCommand = new RelayCommand(new Action<object>(CheckRecoveryPassword));
            NewPassrowdAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptNewPassword));
            NewPassrowdCancelButtonCommand = new RelayCommand(new Action<object>(CancelNewPasswordAction));
            //LoginAcceptButtonCommand = new RelayCommand(new Action<object>(LoginAccept));
            LoginAcceptButtonCommand = new RelayCommand(new Action<object>(LoginAcceptButton));
            LoginCloseButtonCommand = new RelayCommand(new Action<object>(LoginClose));
            ProductionUserLoginCodeCommand = new RelayCommand(new Action<object>(ValidateProductionUserLogin));
            LoginImpersonateAcceptButtonCommand = new RelayCommand(new Action<object>(LoginImpersonateAccept));
            LoginImpersonateCancelButtonCommand = new RelayCommand(new Action<object>(LoginImpersonateClose));
            LoginMiniMizeButtonCommand = new RelayCommand(new Action<object>(LoginMiniMizeButton));
            LoginMaxiMizeButtonCommand = new RelayCommand(new Action<object>(LoginMaxiMizeButton));
            RestorButtonCommand = new RelayCommand(new Action<object>(RestoreButton));
            LoginshutdownButtonCommand = new RelayCommand(new Action<object>(Loginshutdown));
            LoginImpersonateEscapeActionCommand = new RelayCommand(new Action<object>(ImpersonateEscapeAction));
            NewPasswordCodeCommand = new RelayCommand(new Action<object>(NewPasswordStrenth));
            PageUnLoadedCommand = new RelayCommand(new Action<object>(OnPageUnLoadedCommand));
            NeedHelpCommand = new RelayCommand(new Action<object>(OpenHelpLink));//chitra.girigosavi GEOS2-7914 25/06/2025
            //tmrDelay = new System.Windows.Forms.Timer();
            //tmrDelay.Interval = 1000;
            //tmrDelay.Enabled = false;

            WorkbenchLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            version = AssemblyVersion.GetAssemblyVersion(WorkbenchLocation);
            GeosWorkbenchVersionNumber = WorkbenchStartUp.GetWorkbenchVersionByVersionNumber(version.ToString());


            //try
            //{
            //    //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            //    GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser", category: Category.Info, priority: Priority.Low);
            //    UserList = WorkbenchStartUp.GetImpersonateUser().ToList();
            //    GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser successfully", category: Category.Info, priority: Priority.Low);

            //    ImpersonateUserList = new List<string>();
            //    for (int i = 0; i < UserList.Count; i++)
            //    {
            //        ImpersonateUserList.Add(UserList[i].Login);
            //    }

            //    GeosApplication.Instance.ServerActiveMethod();
            //}
            //catch (FaultException<ServiceException> ex)
            //{
            //    if (DXSplashScreen.IsActive)
            //    {
            //        DXSplashScreen.Close();
            //    }
            //    GeosApplication.Instance.Logger.Log("Get an error in LoginWindowViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
            //    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //}
            //catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error On LoginWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
            //    if (DXSplashScreen.IsActive)
            //    {
            //        DXSplashScreen.Close();
            //    }
            //    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            //    IsServiceActive = GeosApplication.Instance.IsServiceActive;
            //}
            //catch (Exception ex)
            //{
            //    GeosApplication.Instance.Logger.Log("Get an error in LoginWindowViewModel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            //}

            GeosApplication.Instance.Logger.Log("End LoginWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
        }

        private void OnPageUnLoadedCommand(object obj)
        {
            //GC.Collect();
            //GC.WaitForFullGCApproach(100);
            //GC.WaitForPendingFinalizers();
            this.PropertyChanged -= PropertyChanged;
            //LoginAcceptButtonCommand = null;
            GC.SuppressFinalize(this);



        }


        #endregion  // Constructor

        #region public Methods

        /// <summary>
        ///  This method is to Show Forgot Password  ForgetPasswordView
        /// </summary>
        /// <param name="obj"></param>
        public void ShowForgotPassword(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Show Forgot Passwrd", category: Category.Info, priority: Priority.Low);
            LoginName = string.Empty;
            LoginPassword = string.Empty;
            IsRememberMe = false;
            SendMailWaitImageVisibility = Visibility.Hidden;
            SendMailMessageVisibility = Visibility.Hidden;
            SendMailErrorVisibility = Visibility.Hidden;
            SelectedViewIndex = 2;
        }

        /// <summary>
        /// This method is to Back Button click  ForgetPasswordView
        /// </summary>
        /// <param name="obj"></param>
        public void BackForgotPasswordAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Back  button of ForgotPassword ", category: Category.Info, priority: Priority.Low);
            LoginName = string.Empty;
            LoginPassword = string.Empty;
            UserNameError = string.Empty;
            IsRememberMe = false;
            SelectedViewIndex = 0;
            UsrEmailOrUserName = string.Empty;
            TxtRecoveryPassword = string.Empty;
            SendMailMessageVisibility = Visibility.Hidden;
            SendMailErrorVisibility = Visibility.Hidden;
            IsForgotPasswordbtnNext = false;
            GeneratePasswordCode = string.Empty;
        }

        /// <summary>
        /// This method is to Cancel Button click in  NewPasswordView
        /// </summary>
        /// <param name="obj"></param>
        public void CancelNewPasswordAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Cancel  button of NewPasswor ", category: Category.Info, priority: Priority.Low);
            SelectedViewIndex = 0;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// This method is to Next Button click in  NewPasswordView
        /// </summary>
        /// <param name="obj"></param>
        public void NextForgotPasswordAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Next  button of ForgotPassword ", category: Category.Info, priority: Priority.Low);
            IsBusy = true;

            SelectedViewIndex = 3;
            //UsrEmailOrUserName = string.Empty;
            TxtRecoveryPassword = string.Empty;
            UserNameError = string.Empty;
            SendMailMessageVisibility = Visibility.Hidden;
            SendMailErrorVisibility = Visibility.Hidden;
            IsForgotPasswordbtnNext = false;

            IsBusy = false;
        }

        /// <summary>
        /// This method is to  Close window
        /// </summary>
        /// <param name="obj"></param>
        public void LoginClose(object obj)
        {
            GeosApplication.Instance.Logger.Log("close window ", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);

        }

        /// <summary>
        ///  This method is to shutdown Button click in  LoginWindow
        /// </summary>
        /// <param name="obj"></param>
        public void Loginshutdown(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on shutdown Button ", category: Category.Info, priority: Priority.Low);

            CustomMessageBox.Show(Workbench.App.Current.Resources["ProductionUserLoginViewSwitchOff"].ToString(), "#2AB7FF", CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);
            if (CustomMessageBox.msgboxresult == MessageBoxResult.Yes)
            {
                RequestClose(null, null);
            }
        }

        /// <summary>
        ///  This method is to Close Button click in  AuthenticationView 
        /// </summary>
        /// <param name="obj"></param>
        public void LoginImpersonateClose(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Close LoginImpersonate ", category: Category.Info, priority: Priority.Low);
            //SelectedViewIndex = 0;
            //LoginImpersonateName = "";
            //User = null;
            //IsRememberMe = false;

            //chitra.girigosavi GEOS2-7914 23/06/2025 // 👇 Hide impersonate view
            LoginName = LoginPassword = string.Empty; //[nsatpute][03.09.2025][GEOS2-9342]
            IsImpersonateViewVisible = Visibility.Hidden; // This will auto-show login view
            LoginNameErrorColor = "Transparent"; //[nsatpute][01.09.2025][GEOS2-9342]
            LoginPasswordErrorColor = "Transparent"; //[nsatpute][01.09.2025][GEOS2-9342]
        }

        /// <summary>
        /// This method is to MiniMize Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void LoginMiniMizeButton(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Login MiniMize Button ", category: Category.Info, priority: Priority.Low);
            CurrentWindowState = WindowState.Normal;
            RestoreButtonVisibility = Visibility.Hidden;
            MaximizeButtonVisibility = Visibility.Visible;
        }

        /// <summary>
        /// This method is to MaxiMize Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void LoginMaxiMizeButton(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Login MaxiMize Button ", category: Category.Info, priority: Priority.Low);
            CurrentWindowState = WindowState.Minimized;
        }

        /// <summary>
        ///  This method is to Restore Button click in  Login window  
        /// </summary>
        /// <param name="obj"></param>
        public void RestoreButton(object obj)
        {
            CurrentWindowState = WindowState.Maximized;
            RestoreButtonVisibility = Visibility.Visible;
            MaximizeButtonVisibility = Visibility.Hidden;
        }

        //Shubham[skadam] GEOS2-5620 User authentication through Active Directory 13 06 2024
        public void LoginAcceptButton(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Click on LoginAcceptButton of Login ", category: Category.Info, priority: Priority.Low);
				//[nsatpute][03.09.2025][GEOS2-9342]
                string error = EnableValidationAndGetError();
                SetLoginNameErrorCode(); //[nsatpute][01.09.2025][GEOS2-9342]
                SetLoginPasswordErrorCode(); //[nsatpute][01.09.2025][GEOS2-9342]
                SetLoginPersonateNameErrorCode();//[nsatpute][01.09.2025][GEOS2-9342]
                OnPropertyChanged(new PropertyChangedEventArgs("LoginName"));
                OnPropertyChanged(new PropertyChangedEventArgs("LoginPassword"));
                if (!IsValid)
                {
                    IsBusy = false;
                    return;
                }


                string loginPasswordEncrypt = string.Empty;
                #region isLogin
                // Both LoginPassword and LoginName have valid values
                // Proceed with the login process
                if (!string.IsNullOrEmpty(LoginPassword) && !string.IsNullOrEmpty(LoginName))
                {
                    //WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                    User = WorkbenchStartUp.GetUserByLoginName(LoginName.Trim());
                    //loginPasswordEncrypt = Encrypt.Encryption(LoginPassword.Trim());
                    //User = WorkbenchStartUp.GetUserByLogin(LoginName.Trim(), loginPasswordEncrypt);
                    if (User != null)
                    {
                        if (User.IsAuthenticatedUsingLDAP)
                        {
                            bool isAuthenticated = WorkbenchStartUp.UserAuthenticate(LoginName.Trim(), LoginPassword.Trim());
                            if (isAuthenticated)
                            {
                                IsAuthenticated(null);
                            }
                            else
                            {
                                CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserWindowscredentialsValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        else
                        {
                            LoginAccept(null);
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(Workbench.App.Current.Resources["OfficeUserLoginNameValidation"].ToString(), LoginName.Trim()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                } //[nsatpute][03.09.2025][GEOS2-9342]
                //else
                //{
                //    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //}
                #endregion


            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Login LoginAcceptButton() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error On Login Accept Button ", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Login Accept Button() Method " + ex, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("End Login Accept Button ", category: Category.Info, priority: Priority.Low);
        }

        //Shubham[skadam] GEOS2-5620 User authentication through Active Directory 13 06 2024
        public void IsAuthenticated(object obj)
        {

            GeosApplication.Instance.Logger.Log("Click on Accept  button of Login ", category: Category.Info, priority: Priority.Low);
            string loginPasswordEncrypt = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(LoginPassword) && !string.IsNullOrEmpty(LoginName))
                {
                    loginPasswordEncrypt = Encrypt.Encryption(LoginPassword.Trim());
                    GeosApplication.Instance.Logger.Log("Getting User details by login name ", category: Category.Info, priority: Priority.Low);
                    //User = WorkbenchStartUp.GetUserByLogin(LoginName.Trim(), loginPasswordEncrypt);
                    //if user is null then don't fill other data.
                    if (User != null)
                    {
                        //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                        //List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId(User.IdUser);
                        //WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                        List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId_V2680(User.IdUser);
                       // WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                        GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();
                        if (listCmpny.Contains(emdepCompany.Name))
                        {
                            GeosApplication.Instance.Logger.Log("Getting User details by login name successfully ", category: Category.Info, priority: Priority.Low);
                            if (User != null && User.IsEnabled == 1)
                            {
                                GeosApplication.Instance.ActiveUser = User;
                                SetUserPermission();
                                if (!Properties.Settings.Default.OfflineUser.Contains(loginName.Trim()))
                                {
                                    Properties.Settings.Default.OfflineUser.Add(loginName.Trim());
                                    Properties.Settings.Default.Save();
                                }

                                // code for user login entry
                                if (User.IsImpersonate == null || User.IsImpersonate == 0)
                                {
                                    string hostName = Dns.GetHostName();    // Retrive the Name of HOST  
                                    MyIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP

                                    UserLoginEntry userEntry = new UserLoginEntry();
                                    userEntry.IdUser = User.IdUser;
                                    userEntry.IpAddress = MyIP;
                                    userEntry.LoginTime = DateTime.Now;
                                    userEntry.LogoutTime = null;
                                    userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;

                                    // userEntry = WorkbenchStartUp.AddUserLoginEntry(userEntry);
                                    userEntry = WorkbenchStartUp.AddUserLoginEntry_V2210(userEntry);
                                }

                                if (IsRememberMe)
                                {
                                    //[Start update setting] code for update usersetting if user signout then remove save user id from setting.
                                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                                    List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                                    GeosApplication.Instance.UserSettings["UserSessionDetail"] = User.IdUser.ToString();
                                    GeosApplication.Instance.UserSettings["UserSessionPassword"] = "";//[rdixit][GEOS2-2742][20.06.2023]
                                    GeosApplication.Instance.UserSettings["AesUserSessionPassword"] = Encrypt.AesEncrypt(LoginPassword.Trim());  //[nsatpute][29.08.2025][GEOS2-9342]
                                    GeosApplication.Instance.UserSettings["WindowsUserSessionPassword"] = EncodeToBase64(LoginPassword.Trim().ToString());
                                    foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                                    {
                                        lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                                    }
                                    ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                                    //[End update setting]
                                    Properties.Settings.Default.UserSessionId = User.IdUser;
                                    Properties.Settings.Default.Save();

                                }
                                if (!IsRememberMe)
                                {
									//[nsatpute][29.08.2025][GEOS2-9342]
                                    RemoveSavedPassword();
                                }
                                if (User.IsImpersonate == 1)
                                {
                                    //chitra.girigosavi GEOS2-7914 23/06/2025
                                    IsImpersonateViewVisible = Visibility.Visible;  // 👈 SHOW impersonation view
                                    SelectedViewIndex = 0;

                                    LoginImpersonateName = LoginName;
                                    isBusy = true;
                                    try
                                    {
                                        GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser", category: Category.Info, priority: Priority.Low);
                                        UserList = WorkbenchStartUp.GetImpersonateUser().ToList();
                                        GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser successfully", category: Category.Info, priority: Priority.Low);
                                        // Clear before filling (to avoid duplicates if method called multiple times)
                                        ImpersonateUserList.Clear();
                                        foreach (var user in UserList.OrderBy(u => u.Login, StringComparer.OrdinalIgnoreCase))
                                        {
                                            ImpersonateUserList.Add(user.Login);
                                        }

                                        GeosApplication.Instance.ServerActiveMethod();
                                    }
                                    catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                                    {
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                        GeosApplication.Instance.Logger.Log("Get an error On LoginWindowViewModel constructor", category: Category.Exception, priority: Priority.Low);
                                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                        IsServiceActive = false;
                                    }

                                    for (int i = 0; i < ImpersonateUserList.Count; i++)
                                    {
                                        if (ImpersonateUserList[i].Equals(loginName.Trim(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            SelectedIndexImpersonateUser = i;
                                        }
                                    }
                                    isBusy = false;
                                }
                                else
                                {
                                    IsImpersonateViewVisible = Visibility.Hidden; //chitra.girigosavi GEOS2-7914 23/06/2025 // 👈 HIDE impersonation view
                                    DXSplashScreen.Show(x =>
                                    {
                                        Window win = new Window()
                                        {
                                            ShowActivated = false,
                                            WindowStyle = WindowStyle.None,
                                            ResizeMode = ResizeMode.NoResize,
                                            AllowsTransparency = true,
                                            Background = new SolidColorBrush(Colors.Transparent),
                                            ShowInTaskbar = false,
                                            Topmost = true,
                                            SizeToContent = SizeToContent.WidthAndHeight,
                                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                        };
                                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                        win.Topmost = false;
                                        return win;
                                    }, x =>
                                    {
                                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                                    }, null, null);
                                    ShowWorkbenchWindow();
                                }

                                LoginName = string.Empty;
                                LoginPassword = string.Empty;
                                GeosApplication.Instance.ServerActiveMethod();
                            }
                            else
                            {
                                if (User == null)
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    LoginName = string.Empty;
                                    LoginPassword = string.Empty;
                                    loginPasswordEncrypt = string.Empty;
                                    IsRememberMe = false;
                                }
                                else
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameEnabledUserValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                                    LoginName = string.Empty;
                                    LoginPassword = string.Empty;
                                    loginPasswordEncrypt = string.Empty;
                                    IsRememberMe = false;
                                }
                            }

                        }
                        else
                        {
                            CustomMessageBox.Show(Workbench.App.Current.Resources["UnauthorizedUser"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            LoginName = string.Empty;
                            LoginPassword = string.Empty;
                            loginPasswordEncrypt = string.Empty;
                            IsRememberMe = false;
                        }

                    }
                    else
                    {
                        CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Login Accept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error On Login Accept ", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = false;
                LoginName = string.Empty;
                LoginPassword = string.Empty;
            }

            GeosApplication.Instance.Logger.Log("End Login Accept ", category: Category.Info, priority: Priority.Low);
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

        /// <summary>
        /// This method is to Accept Button click in  OfficeUserLoginView   
        /// </summary>
        /// <param name="obj"></param>
        public void LoginAccept(object obj)
        {

            GeosApplication.Instance.Logger.Log("Click on Accept  button of Login ", category: Category.Info, priority: Priority.Low);
            string loginPasswordEncrypt = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(LoginPassword) && !string.IsNullOrEmpty(LoginName))
                {
                    loginPasswordEncrypt = Encrypt.Encryption(LoginPassword.Trim());

                    //clsCommon.Login = LoginName.Trim();
                    //clsCommon.Password = LoginPassword.Trim();
                    //loginPasswordEncrypt = "a18c5dd9e59b3d37772d7d664da4f81f";
                    //loginPasswordEncrypt = "4fd9dc81e6de43ff8cea1922d859116c";//lfaneca x69y
                    //loginPasswordEncrypt = "c6b6c548c35d2394cbc220f5de8ead90";//ccorvalan
                    //loginPasswordEncrypt = "181b5ce79be3ff844119f88959b06c50";//imozonov
                    //loginPasswordEncrypt = "e908defa8dc3d22a067b8f2ae0a0e9bc";//csora
                    //loginPasswordEncrypt = "4b7c5effd6a20fb0f4014e2535705dbc";
                    //loginPasswordEncrypt = "4b7c5effd6a20fb0f4014e2535705dbc"; //ksingh
                    //Encrypt.Encryption(LoginPassword.Trim());

                    GeosApplication.Instance.Logger.Log("Getting User details by login name ", category: Category.Info, priority: Priority.Low);
                    User = WorkbenchStartUp.GetUserByLogin(LoginName.Trim(), loginPasswordEncrypt);
                    //if user is null then don't fill other data.
                    if (User != null)
                    {
                        //List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId(User.IdUser);
                        //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                        //WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                        List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId_V2680(User.IdUser);
                        //WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                        GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();
                        if (listCmpny.Contains(emdepCompany.Name))
                        {

                            GeosApplication.Instance.Logger.Log("Getting User details by login name successfully ", category: Category.Info, priority: Priority.Low);
                            if (User != null && User.IsEnabled == 1)
                            {
                                GeosApplication.Instance.ActiveUser = User;
                                SetUserPermission();
                                if (!Properties.Settings.Default.OfflineUser.Contains(loginName.Trim()))
                                {
                                    Properties.Settings.Default.OfflineUser.Add(loginName.Trim());
                                    Properties.Settings.Default.Save();
                                }

                                // code for user login entry
                                if (User.IsImpersonate == null || User.IsImpersonate == 0)
                                {
                                    string hostName = Dns.GetHostName();    // Retrive the Name of HOST  
                                    MyIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP

                                    UserLoginEntry userEntry = new UserLoginEntry();
                                    userEntry.IdUser = User.IdUser;
                                    userEntry.IpAddress = MyIP;
                                    userEntry.LoginTime = DateTime.Now;
                                    userEntry.LogoutTime = null;
                                    userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;

                                    // userEntry = WorkbenchStartUp.AddUserLoginEntry(userEntry);
                                    userEntry = WorkbenchStartUp.AddUserLoginEntry_V2210(userEntry);
                                }

                                if (IsRememberMe)
                                {
                                    //[Start update setting] code for update usersetting if user signout then remove save user id from setting.
                                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                                    List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                                    GeosApplication.Instance.UserSettings["UserSessionDetail"] = User.IdUser.ToString();
                                    GeosApplication.Instance.UserSettings["UserSessionPassword"] = User.Password.ToString();//[rdixit][GEOS2-2742][20.06.2023]
                                    GeosApplication.Instance.UserSettings["AesUserSessionPassword"] = Encrypt.AesEncrypt(LoginPassword); //[nsatpute][29.08.2025][GEOS2-9342]
                                    GeosApplication.Instance.UserSettings["WindowsUserSessionPassword"] = "";
                                    foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                                    {
                                        lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                                    }
                                    ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                                    //[End update setting]
                                    Properties.Settings.Default.UserSessionId = User.IdUser;
                                    Properties.Settings.Default.Save();

                                }
                                if (!IsRememberMe)
                                {
									//[nsatpute][29.08.2025][GEOS2-9342]
                                    RemoveSavedPassword();
                                }
                                //chitra.girigosavi GEOS2-7914 23/06/2025
                                if (User.IsImpersonate == 1)
                                {
                                    //chitra.girigosavi GEOS2-7914 23/06/2025
                                    IsImpersonateViewVisible = Visibility.Visible;  // 👈 SHOW impersonation view
                                    SelectedViewIndex = 0;

                                    LoginImpersonateName = LoginName;
                                    isBusy = true;
                                    try
                                    {
                                        GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser", category: Category.Info, priority: Priority.Low);
                                        UserList = WorkbenchStartUp.GetImpersonateUser().ToList();
                                        GeosApplication.Instance.Logger.Log("Getting User List for ImpersonateUser successfully", category: Category.Info, priority: Priority.Low);
                                        // Clear before filling (to avoid duplicates if method called multiple times)
                                        ImpersonateUserList.Clear();
                                        foreach (var user in UserList.OrderBy(u => u.Login, StringComparer.OrdinalIgnoreCase))
                                        {
                                            ImpersonateUserList.Add(user.Login);
                                        }
                                        GeosApplication.Instance.ServerActiveMethod();
                                    }
                                    catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                                    {
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                        GeosApplication.Instance.Logger.Log("Get an error On LoginWindowViewModel constructor", category: Category.Exception, priority: Priority.Low);
                                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                        IsServiceActive = false;
                                    }

                                    for (int i = 0; i < ImpersonateUserList.Count; i++)
                                    {
                                        if (ImpersonateUserList[i].Equals(loginName.Trim(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            SelectedIndexImpersonateUser = i;
                                        }
                                    }
                                    isBusy = false;
                                }
                                else
                                {
                                    IsImpersonateViewVisible = Visibility.Hidden; //chitra.girigosavi GEOS2-7914 23/06/2025 // 👈 HIDE impersonation view
                                    DXSplashScreen.Show(x =>
                                    {
                                        Window win = new Window()
                                        {
                                            ShowActivated = false,
                                            WindowStyle = WindowStyle.None,
                                            ResizeMode = ResizeMode.NoResize,
                                            AllowsTransparency = true,
                                            Background = new SolidColorBrush(Colors.Transparent),
                                            ShowInTaskbar = false,
                                            Topmost = true,
                                            SizeToContent = SizeToContent.WidthAndHeight,
                                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                        };
                                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                        win.Topmost = false;
                                        return win;
                                    }, x =>
                                    {
                                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                                    }, null, null);
                                    ShowWorkbenchWindow();
                                }
								//[nsatpute][03.09.2025][GEOS2-9342]		
                                //LoginName = string.Empty;
                                //LoginPassword = string.Empty;
                                GeosApplication.Instance.ServerActiveMethod();
                            }
                            else
                            {
                                if (User == null)
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    LoginName = string.Empty;
                                    LoginPassword = string.Empty;
                                    loginPasswordEncrypt = string.Empty;
                                    IsRememberMe = false;
                                }
                                else
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameEnabledUserValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                                    LoginName = string.Empty;
                                    LoginPassword = string.Empty;
                                    loginPasswordEncrypt = string.Empty;
                                    IsRememberMe = false;
                                }
                            }

                        }
                        else
                        {
                            CustomMessageBox.Show(Workbench.App.Current.Resources["UnauthorizedUser"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            LoginName = string.Empty;
                            LoginPassword = string.Empty;
                            loginPasswordEncrypt = string.Empty;
                            IsRememberMe = false;
                        }

                    }

                    else
                    {
                        //CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserGEOScredentialsValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Login Accept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error On Login Accept ", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = false;
                LoginName = string.Empty;
                LoginPassword = string.Empty;
            }

            GeosApplication.Instance.Logger.Log("End Login Accept ", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for set user permission as per user setting file.
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selecte
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

            GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Hidden;
            GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Hidden;

            GeosApplication.Instance.IsPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 23);
            GeosApplication.Instance.IsPermissionAdminOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 9);
            GeosApplication.Instance.IsCommercialUser = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 24);
            GeosApplication.Instance.IsPermissionAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 29 && up.Permission != null && up.Permission.IdGeosModule == 5);
            GeosApplication.Instance.IsPermissionNameEditInPCMArticle = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 53 && up.Permission.IdGeosModule == 8);
            GeosApplication.Instance.IsPermissionWMSgridValueColumn = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 54 && up.Permission.IdGeosModule == 6);
            GeosApplication.Instance.IsPermissionForManageItemPrice = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 55 && up.Permission.IdGeosModule == 8);

            GeosApplication.Instance.IsManageTrainingPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 58 && up.Permission.IdGeosModule == 7);
            //[skadam[11-02-2022][GEOS2-3439]
            GeosApplication.Instance.IsPermissionForSRMEdit_Supplier = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 64 && up.Permission.IdGeosModule == 10);
            //[pjadhav[07-11-2022][GEOS2-2566]
            GeosApplication.Instance.IsPermissionReadOnlyForPCM = !GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 70 && up.Permission.IdGeosModule == 8);

            //[pjadhav[22-11-2022][GEOS2-3969]
            GeosApplication.Instance.IsPermissionReadOnlyForPLM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 33 && up.Permission.IdGeosModule == 8);
            //[pjadhav[29-11-2022][GEOS2-3550]
            GeosApplication.Instance.IsAdminInventoryAuditPermissionForWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 32 && up.Permission.IdGeosModule == 6);
            //[pramod.misal][GEOS2-][12.10.2023]
            GeosApplication.Instance.IsWMSDeliveryNoteLockUnlockPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 91 && up.Permission.IdGeosModule == 6);
            //[pramod.misal][GEOS2-5477][06.05.2024]
            GeosApplication.Instance.IsSCMViewConfigurationPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 110 && up.Permission.IdGeosModule == 1);

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
            //[Sudhir.Jangra][GEOS2-4414][08/09/2023]
            GeosApplication.Instance.IsEditBulkPickingPermissionWMS = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 89 && up.Permission.IdGeosModule == 6);
            //[rdixit][17.05.2023][GEOS2-4273]
            GeosApplication.Instance.IsCRMactionsLauncherPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 85 && up.Permission.IdGeosModule == 5);

            #region HRM Permission
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 38 && up.Permission.IdGeosModule == 7);
            //[rdixit][22.11.2022][GEOS2-3943]
            GeosApplication.Instance.IsChangeOrAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38) && up.Permission.IdGeosModule == 7);
            //[rdixit][09.01.2024][GEOS2-5112]
            GeosApplication.Instance.IsTravel_AssistantPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 96) && up.Permission.IdGeosModule == 7);
            //[rdixit][09.01.2024][GEOS2-5112]
            GeosApplication.Instance.IsControlPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 97) && up.Permission.IdGeosModule == 7);
            //[rdixit][09.01.2024][GEOS2-5112]
            GeosApplication.Instance.IsPlant_FinancePermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 98) && up.Permission.IdGeosModule == 7);
            //[rdixit][16.01.2024][GEOS2-5074]
            GeosApplication.Instance.IsWatchMySelfOnlyPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 99) && up.Permission.IdGeosModule == 7);

            //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
            GeosApplication.Instance.IsChangeAndAdminPermissionForHRM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 30 || up.IdPermission == 38));

            #endregion

            //[cpatil[20-09-2021][GEOS2-3342]
            GeosApplication.Instance.IsPCMPermissionNameECOS_Synchronization = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 59 && up.Permission.IdGeosModule == 8);
            //[cpatil[20-09-2021][GEOS2-3336]
            GeosApplication.Instance.IsPLMPermissionNameECOS_Synchronization = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 60 && up.Permission.IdGeosModule == 11);
            //[sdeshpande][11-08-2022][GEOS2-3874]
            GeosApplication.Instance.IsAdminPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 57 && up.Permission.IdGeosModule == 12);

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

            if (GeosApplication.Instance.IsAdminPermissionERM)
            {
                GeosApplication.Instance.IsReadSODPermissionERM = true;
                GeosApplication.Instance.IsReadWOPermissionERM = true;
                GeosApplication.Instance.IsEditSODPermissionERM = true;
                GeosApplication.Instance.IsEditWOPermissionERM = true;
                GeosApplication.Instance.IsTimeTrackingReadPermissionERM = true;
                GeosApplication.Instance.IsReadProductionPlanPermissionERM = true; //[Rupali Sarode][GEOS2-4155][02-03-2023]
                GeosApplication.Instance.IsEditProductionPlanPermissionERM = true; //[Rupali Sarode][GEOS2-4155][02-03-2023]
                GeosApplication.Instance.IsReadWorkStagesPermissionERM = true; //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditWorkStagesPermissionERM = true; //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditDeliveryTimeDistributionERM = true;//[Pallavi jadhav][GEOS2-5269][02-02-2024]
                GeosApplication.Instance.IsViewPermissionERM = true;//[Pallavi jadhav][GEOS2-5269][02-02-2024]

                //  GeosApplication.Instance.IsViewSupervisorERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 119 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5910][17-07-2024]

            }
            else
            {
                GeosApplication.Instance.IsReadSODPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 73 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsReadWOPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 71 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsEditSODPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 74 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsEditWOPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 72 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsTimeTrackingReadPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 66 && up.Permission.IdGeosModule == 12);
                GeosApplication.Instance.IsReadProductionPlanPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 80 && up.Permission.IdGeosModule == 12); //[Rupali Sarode][GEOS2-4155][02-03-2023]
                GeosApplication.Instance.IsEditProductionPlanPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 81 && up.Permission.IdGeosModule == 12); //[Rupali Sarode][GEOS2-4155][02-03-2023]
                GeosApplication.Instance.IsReadWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 78 && up.Permission.IdGeosModule == 12); //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 79 && up.Permission.IdGeosModule == 12); //[Pallavi Jadhav][GEOS2-4618][28-06-2023]
                GeosApplication.Instance.IsEditDeliveryTimeDistributionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 102 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5269][02-02-2024]
                GeosApplication.Instance.IsViewPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 103 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5269][02-02-2024]
                GeosApplication.Instance.IsViewSupervisorERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 119 && up.Permission.IdGeosModule == 12); //[Pallavi jadhav][GEOS2-5910][17-07-2024]

            }

            GeosApplication.Instance.IsPLMPermissionView = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 50 && up.Permission.IdGeosModule == 11);
            GeosApplication.Instance.IsPLMPermissionChange = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 62 && up.Permission.IdGeosModule == 11);
            GeosApplication.Instance.IsPCMEditFreePluginsPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 88 && up.Permission.IdGeosModule == 8);
            GeosApplication.Instance.IsPLMPermissionAdmin = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 63 && up.Permission.IdGeosModule == 11);

            //[rdixit][GEOS2-4971][15.12.2023]
            GeosApplication.Instance.IsSCMPermissionAdmin = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 94 && up.Permission.IdGeosModule == 1);
            GeosApplication.Instance.IsSCMPermissionReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 108 && up.Permission.IdGeosModule == 1);
            //[rdixit][GEOS2-5479][06.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorStatus = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 109 && up.Permission.IdGeosModule == 1);
            //[rdixit][07.05.2024][GEOS2-5699]
            GeosApplication.Instance.IsSCMReportsAuditor = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 111 && up.Permission.IdGeosModule == 1);
            //[rdixit][20.05.2024][GEOS2-5477]
            GeosApplication.Instance.IsSCMREditConnectorBasic = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 112 && up.Permission.IdGeosModule == 1);
            //GeosApplication.Instance.IsReadWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 78 && up.Permission.IdGeosModule == 12); //[sdeshpande][GEOS2-3841][11/1/2023]
            //GeosApplication.Instance.IsEditWorkStagesPermissionERM = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 79 && up.Permission.IdGeosModule == 12); //[sdeshpande][GEOS2-3841][11/1/2023]

            //[Sudhir.Jangra][GEOS2-4901]
            GeosApplication.Instance.IsPCMAddEditPermissionForHardLockLicense = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 93 && up.Permission.IdGeosModule == 8);
            //[Pramod.misal][GEOS2-5481][23.05.2024]
            GeosApplication.Instance.IsSCMEditFiltersManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 113 && up.Permission.IdGeosModule == 1);
            //[Pramod.misal][GEOS2-5482][24.05.2024]
            GeosApplication.Instance.IsSCMEditFamiliesManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 114 && up.Permission.IdGeosModule == 1);
            //[Pramod.misal][GEOS2-5483][27.05.2024]
            GeosApplication.Instance.IsSCMEditPropertiesManager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 115 && up.Permission.IdGeosModule == 1);
            //[rdixit][27.12.2023][GEOS2-4875]
            GeosApplication.Instance.PCMEditArticleCategoryMapping = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 95 && up.Permission.IdGeosModule == 8);
            //[rdixit][GEOS2-5480][29.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorLinks = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 116 && up.Permission.IdGeosModule == 1);
            //[rdixit][GEOS2-5478][29.05.2024]
            GeosApplication.Instance.IsSCMEditConnectorAdvanced = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 117 && up.Permission.IdGeosModule == 1);
            //[rdixit][04.04.2024][GEOS2-5278]
            GeosApplication.Instance.IsHRMAttendanceSplitPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 107 && up.Permission.IdGeosModule == 7);
            //[Pramod.misal][GEOS2-5525][12.08.2024]
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
            GeosApplication.Instance.IsTSMUsersEdit = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 140 && up.Permission.IdGeosModule == 13);
            GeosApplication.Instance.IsTSMUsersViewOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 139 && up.Permission.IdGeosModule == 13);

            //[rahul.gadhave][GEOS2-6829][03.01.2025]
            GeosApplication.Instance.PO_Template_Manager = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 138 && up.Permission.IdGeosModule == 14);



            if (GeosApplication.Instance.IsPermissionReadOnly)
                GeosApplication.Instance.IsPermissionEnabled = false;
            else
                GeosApplication.Instance.IsPermissionEnabled = true;
            // Start (SalesOwner) - Selected Sales Owners User list for CRM. 
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                GeosApplication.Instance.SalesOwnerUsersList = WorkbenchStartUp.GetManagerUsers(GeosApplication.Instance.ActiveUser.IdUser);

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
            //Shubham[skadam] GEOS2-7896 For Sales Assistant permission Engineering Analysis data not showing.  18 04 2025.
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
            GeosApplication.Instance.IsSAMReport = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 124 && up.Permission.IdGeosModule == 9);

            GeosApplication.Instance.IsHRMTravelManagerPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 143 && up.Permission.IdGeosModule == 7);
            GeosApplication.Instance.IsWMManagerPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 146 && up.Permission.IdGeosModule == 10);
            GeosApplication.Instance.IsWMSAdminPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 32 && up.Permission.IdGeosModule == 6);
            //[nsatpute][12.09.2025][GEOS2-8789]
            GeosApplication.Instance.IsWMS_SchedulePermission = GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 147 && up.Permission.IdGeosModule == 6);
        }


        public void ShowWorkbenchWindow()
        {
            GeosApplication.Instance.Logger.Log("Start ShowWorkbenchWindow Method ", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log("Initialising workbench window", category: Category.Info, priority: Priority.Low);
            WorkbenchWindowViewModel workbenchWindowViewModel = new WorkbenchWindowViewModel();

            workbenchWindowViewModel.LoginWorkstation = LoginWorkstation;
            WorkbenchWindow loginSuccess = new WorkbenchWindow();
            // workbenchWindowViewModel.Tempclose();
            EventHandler handle1 = delegate { loginSuccess.Close(); };

            workbenchWindowViewModel.RequestClose += handle1;
            try
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
                //GC.Collect();
                //GC.WaitForFullGCApproach(100);
                //GC.WaitForPendingFinalizers();
                RequestClose(null, null);


            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Get an error ShowWorkbenchWindow Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            loginSuccess.DataContext = workbenchWindowViewModel;
            GeosApplication.Instance.Logger.Log("Initialising workbench window Successfully", category: Category.Info, priority: Priority.Low);
            loginSuccess.ShowDialog();
        }

        /// <summary>
        /// This method is to Accept Button click in  Impersonate user in  AuthenticationView   
        /// </summary>
        /// <param name="obj"></param>
        public void LoginImpersonateAccept(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Accept button of Login Impersonate", category: Category.Info, priority: Priority.Low);

            User User = null;
            try
            {
                //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.Logger.Log("Getting User details by login name ", category: Category.Info, priority: Priority.Low);
				//[nsatpute][03.09.2025][GEOS2-9342]
                string error = EnableValidationAndGetError();

                OnPropertyChanged(new PropertyChangedEventArgs("LoginImpersonateName"));
                if (!IsValid)
                {
                    IsBusy = false;
                    return;
                }


                User = WorkbenchStartUp.GetUserByLoginName(LoginImpersonateName.Trim());
                if (User != null && User.IsEnabled == 1)
                {
                    //List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId(User.IdUser);
                    //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                    //WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                    List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId_V2680(User.IdUser);
                   // WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                    GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();
                    if (listCmpny.Contains(emdepCompany.Name))
                    {
                        GeosApplication.Instance.Logger.Log("Getting User details by login name successfully ", category: Category.Info, priority: Priority.Low);

                        GeosApplication.Instance.ServerActiveMethod();
                        GeosApplication.Instance.ActiveUser = User;
                        //DXSplashScreen.Show<SplashScreenView>();
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);

                        // code for user login entry
                        string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                        MyIP = Dns.GetHostByName(hostName).AddressList[0].ToString();   // Get the IP

                        UserLoginEntry userEntry = new UserLoginEntry();
                        userEntry.IdUser = user.IdUser;
                        userEntry.IpAddress = MyIP;
                        userEntry.LoginTime = DateTime.Now;
                        userEntry.LogoutTime = null;
                            userEntry.IdCurrentGeosVersion = GeosWorkbenchVersionNumber.IdGeosWorkbenchVersion;

                        // userEntry = WorkbenchStartUp.AddUserLoginEntry(userEntry);
                        userEntry = WorkbenchStartUp.AddUserLoginEntry_V2210(userEntry);
                        //fill IdUserPermission
                        //List<int> userPermissionlst = new List<int>() { 20, 21, 22 };
                        //GeosApplication.Instance.IdUserPermission = GeosApplication.Instance.ActiveUser.UserPermissions.Where(usr => userPermissionlst.Contains(usr.IdPermission)).OrderByDescending(ord => ord.IdPermission).Select(slt => slt.IdPermission).FirstOrDefault();

                        SetUserPermission();
                        ShowWorkbenchWindow();
                    }
                    else
                    {
                        CustomMessageBox.Show(Workbench.App.Current.Resources["UnauthorizedUser"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameEnabledUserValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in LoginImpersonateAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error On LoginImpersonateAccept Method", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = false;
            }

            //if (User == null)
            //{
            //    CustomMessageBox.Show(Workbench.App.Current.Resources["OfficeUserLoginViewLoginNameValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //}
            //else
            //{
            //    GeosApplication.Instance.ActiveUser = User;
            //    DXSplashScreen.Show<SplashScreenView>();
            //    ShowWorkbenchWindow();
            //}

            GeosApplication.Instance.Logger.Log("End LoginImpersonateAccept Method", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// This method is to Accept Button click   for NewPassword in  NewPasswordView   
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptNewPassword(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start ShowWorkbench Method", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            List<string> errorList = TextUtility.CheckRequiredCharacterInPassword(NewPassword, PasswordErrorMessageDictionary());

            if (errorList.Count > 0)
            {
                string errorMessage = "";
                foreach (string error in errorList)
                {
                    if (errorMessage == "")
                        errorMessage = error;
                    else
                        errorMessage = errorMessage + "\n" + error;
                }

                CustomMessageBox.Show(errorMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            else
            {
                if (!String.IsNullOrEmpty(NewPassword) && !String.IsNullOrEmpty(ConfirmPassword))
                {
                    if (NewPassword.Trim() == ConfirmPassword.Trim())
                    {
                        try
                        {
                            string loginPasswordEncrypt = string.Empty;
                            loginPasswordEncrypt = Encrypt.Encryption(NewPassword.Trim());
                            //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            GeosApplication.Instance.Logger.Log("Getting Update user password", category: Category.Info, priority: Priority.Low);
                            WorkbenchStartUp.UpdateUserPassword(UsrEmailOrUserName, loginPasswordEncrypt);
                            GeosApplication.Instance.Logger.Log("Getting Update user password successfully", category: Category.Info, priority: Priority.Low);

                            NewPassword = string.Empty;
                            ConfirmPassword = string.Empty;
                            CustomMessageBox.Show(Workbench.App.Current.Resources["NewPasswordViewPasswordChangeValidation"].ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            SelectedViewIndex = 0;
                            GeosApplication.Instance.ServerActiveMethod();
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Get an error in AcceptNewPassword() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Format("Get an error On AcceptNewPassword Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            NewPassword = string.Empty;
                            ConfirmPassword = string.Empty;
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                            IsServiceActive = false;
                        }
                    }
                    else
                    {
                        NewPassword = string.Empty;
                        ConfirmPassword = string.Empty;
                        CustomMessageBox.Show(Workbench.App.Current.Resources["NewPasswordViewPasswordNotMatchChangeValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(Workbench.App.Current.Resources["ChangePasswordWindowpleaseenterpassword"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }

            GeosApplication.Instance.Logger.Log("End AcceptNewPassword Method", category: Category.Info, priority: Priority.Low);
            IsBusy = false;
        }

        /// <summary>
        /// Added method for to create list of error message of Password missing charactor.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> PasswordErrorMessageDictionary()
        {
            Dictionary<string, string> MessageDictionary = new Dictionary<string, string>();
            MessageDictionary.Add("minLen", Workbench.App.Current.Resources["ChangePasswordErrorcharacters"].ToString());
            MessageDictionary.Add("minDigit", Workbench.App.Current.Resources["ChangePasswordErrorDigit"].ToString());
            MessageDictionary.Add("minLower", Workbench.App.Current.Resources["ChangePasswordErrorLower"].ToString());
            MessageDictionary.Add("minUpper", Workbench.App.Current.Resources["ChangePasswordErrorUpper"].ToString());
            MessageDictionary.Add("minSpChar", Workbench.App.Current.Resources["ChangePasswordErrorSpecialCharacter"].ToString());

            return MessageDictionary;
        }

        /// <summary>
        /// This method is to Send ForgetPassword Mail Generate Password Code
        /// </summary>
        public void SendForgetPasswordMail()
        {
            try
            {
                SendMailWaitImageVisibility = Visibility.Visible;
                GeneratePasswordCode = GeneratePassword();
                SendMailErrorVisibility = Visibility.Hidden;
                SendMailMessageVisibility = Visibility.Hidden;

                try
                {
                    //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Send Forget password mail to user", category: Category.Info, priority: Priority.Low);
                    WorkbenchStartUp.SendForgetPasswordMail(UsrEmailOrUserName, GeneratePasswordCode);
                    GeosApplication.Instance.Logger.Log("Send Forget password mail to user successfully", category: Category.Info, priority: Priority.Low);

                    TxtRecoveryPassword = string.Empty;
                    SendMailWaitImageVisibility = Visibility.Hidden;
                    SendMailMessageVisibility = Visibility.Visible;
                    GeosApplication.Instance.ServerActiveMethod();
                    threadSendMail.Abort();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in SendForgetPasswordMail() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error On SendForgetPasswordMail Method", category: Category.Exception, priority: Priority.Low);

                    TxtRecoveryPassword = string.Empty;
                    SendMailMessageVisibility = Visibility.Hidden;
                    SendMailWaitImageVisibility = Visibility.Hidden;
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    IsServiceActive = false;

                    threadSendMail.Abort();
                }
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SendForgetPasswordMail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

                if (!string.IsNullOrEmpty(UsrEmailOrUserName))
                {
                    if (Regex.Match(UsrEmailOrUserName, @"[.,@]", RegexOptions.ECMAScript).Success)
                    {
                        UserNameError = Workbench.App.Current.Resources["ForgetPasswordUserEmailIdWrong"].ToString();
                    }
                    else
                    {
                        UserNameError = Workbench.App.Current.Resources["ForgetPasswordUserNameWrong"].ToString();
                    }
                }

                UsrEmailOrUserName = string.Empty;
                SendMailMessageVisibility = Visibility.Hidden;
                SendMailErrorVisibility = Visibility.Visible;
                SendMailWaitImageVisibility = Visibility.Hidden;
                threadSendMail.Abort();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendForgetPasswordMail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for send forgot password mail for set new password
        /// </summary>
        /// <param name="obj"></param>
        public void SendForgotPasswordMail(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start SendForgotPasswordMail Method", category: Category.Info, priority: Priority.Low);
            UserNameError = string.Empty;

            if (!String.IsNullOrEmpty(UsrEmailOrUserName) && !String.IsNullOrEmpty(UsrEmailOrUserName.Trim()))
            {
                try
                {
                    threadSendMail = new Thread(SendForgetPasswordMail);
                    threadSendMail.SetApartmentState(ApartmentState.STA);
                    threadSendMail.Start();
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in SendForgotPasswordMail() Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    UsrEmailOrUserName = string.Empty;
                    SendMailMessageVisibility = Visibility.Hidden;
                    SendMailErrorVisibility = Visibility.Visible;
                    // CustomMessageBox.Show(Workbench.App.Current.Resources["ForgetPasswordViewCheckMailValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            else
            {
                UserNameError = Workbench.App.Current.Resources["ForgetPasswordUserNameEmpaty"].ToString();
                UsrEmailOrUserName = string.Empty;
                SendMailMessageVisibility = Visibility.Hidden;
                SendMailErrorVisibility = Visibility.Visible;
            }

            GeosApplication.Instance.Logger.Log("End SendForgotPasswordMail Method", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method is to Generate Password
        /// </summary>
        /// <returns></returns>
        private String GeneratePassword()
        {
            int length = 10;
            const string validCharactor = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder strBuilder = new StringBuilder();
            Random random = new Random();
            while (0 < length--)
            {
                strBuilder.Append(validCharactor[random.Next(validCharactor.Length)]);
            }

            return strBuilder.ToString();
        }

        /// <summary>
        ///  this method is to Check Generate Password Code and  Recovery Password same 
        /// </summary>
        /// <param name="obj"></param>
        private void CheckRecoveryPassword(object obj)
        {
            if (GeneratePasswordCode == TxtRecoveryPassword)
            {
                IsForgotPasswordbtnNext = true;
            }
            else
            {
                CustomMessageBox.Show(Workbench.App.Current.Resources["ForgetPasswordViewEnterValidCodeValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                TxtRecoveryPassword = string.Empty;
                IsForgotPasswordbtnNext = false;
            }
        }

        /// <summary>
        /// this method is  Production UserLogin in ProductionUserLoginView
        /// </summary>
        /// <param name="obj"></param>
        private void ValidateProductionUserLogin(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start ValidateProductionUserLogin Method", category: Category.Info, priority: Priority.Low);

            try
            {
                //if (((System.Windows.Input.KeyEventArgs)obj).Key == System.Windows.Input.Key.Enter)
                //{
                //    if (!string.IsNullOrEmpty(ProductionUserLoginCode))
                //    {
                //        CheckProductionLogin();
                //    }
                //}

                //chitra.girigosavi GEOS2-7914 23/06/2025
                if (!string.IsNullOrEmpty(ProductionUserLoginCode))
                {
                    CheckProductionLogin();
                }
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in ValidateProductionUserLogin Method - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = false;
            }

            GeosApplication.Instance.Logger.Log("End ValidateProductionUserLogin Method", category: Category.Info, priority: Priority.Low);
        }

        private void CheckProductionLogin()
        {
            GeosApplication.Instance.Logger.Log("Start CheckProductionLogin Method for taking value from user", category: Category.Info, priority: Priority.Low);

            if (!string.IsNullOrEmpty(ProductionUserLoginCode))
            {
                try
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);

                    GeosApplication.Instance.Logger.Log("Getting User details by ProductionUserLoginCode - CheckProductionLogin()", category: Category.Info, priority: Priority.Low);
                    User User = WorkbenchStartUp.GetUserByCode(ProductionUserLoginCode);

                    if (User == null)
                    {
                        //string error = EnableValidationAndGetError();
                        //PropertyChanged(this, new PropertyChangedEventArgs("ProductionUserLoginCode"));

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(Workbench.App.Current.Resources["ProductionUserLoginViewsProductionCode"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        ProductionUserLoginCode = string.Empty;
                    }
                    else if (User.IsEnabled == 0)
                    {
                        ProductionUserLoginCode = string.Empty;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(Workbench.App.Current.Resources["ProductionUserLoginViewsUserValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        GeosApplication.Instance.ActiveUser = User;
                        ProductionUserLoginCode = string.Empty;

                        //List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId(User.IdUser);
                        //[Rahul.gadhave][Date:12-11-2025][GEOS2-8713]
                        //WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
                        List<Company> companysuserwise = WorkbenchStartUp.GetAllCompanyByUserId_V2680(User.IdUser);
                       // WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        List<string> listCmpny = companysuserwise.Select(o => o.Alias).ToList();
                        GeosServiceProvider emdepCompany = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(i => i.IsSelected == true).FirstOrDefault();

                        if (listCmpny.Contains(emdepCompany.Name))
                        {
                            SetUserPermission();
                            ShowWorkbenchWindow();
                        }
                        else
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(Workbench.App.Current.Resources["UnauthorizedUser"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            ProductionUserLoginCode = string.Empty;
                        }
                    }

                    GeosApplication.Instance.ServerActiveMethod();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in CheckProductionLogin() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error On CheckProductionLogin Method", category: Category.Exception, priority: Priority.Low);
                    ProductionUserLoginCode = string.Empty;
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    IsServiceActive = false;
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in CheckProductionLogin() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        /// <summary>
        /// Method for check new password strenth.
        /// </summary>
        /// <param name="obj"></param>
        private void NewPasswordStrenth(object obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (NewPassword.Length >= 1)
                    {
                        string checkstrength = TextUtility.CheckPasswordStrength(NewPassword).ToString();
                        PasswordStrenth = checkstrength.ToString();
                        PasswordStrenthColorShow(PasswordStrenth);
                    }
                }
                else
                {
                    PasswordStrenth = "";
                }

                GeosApplication.Instance.ServerActiveMethod();
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                IsServiceActive = false;
            }
        }

        private void PasswordStrenthColorShow(string passwordLegnth)
        {
            if (passwordLegnth == "VeryWeak")
            {
                PasswordStrenthColor = "#ff1a1a";
            }

            if (passwordLegnth == "Weak")
            {
                PasswordStrenthColor = "#ff471a";
            }

            if (passwordLegnth == "Medium")
            {
                PasswordStrenthColor = "#c6ff1a";
            }

            if (passwordLegnth == "Strong")
            {
                PasswordStrenthColor = "#8cff1a";
            }

            if (passwordLegnth == "VeryStrong")
            {
                PasswordStrenthColor = "#1aff53";
            }

        }

        /// <summary>
        /// method for open window if user is Impersonate 
        /// </summary>
        /// <param name="obj"></param>
        public void ImpersonateEscapeAction(object obj)
        {
            //SelectedViewIndex = 0;
            IsImpersonateViewVisible = Visibility.Hidden; //chitra.girigosavi GEOS2-7914 02/07/2025
            //IsLoginViewVisible = Visibility.Hidden;
        }

        //chitra.girigosavi GEOS2-7914 25/06/2025
        private void OpenHelpLink(object obj)
        {
            string url = "https://helpdesk.emdep.com/servicedesk/customer/portal/1/group/5";
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Failed to open URL: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][29.08.2025][GEOS2-9342]
        private void RemoveSavedPassword()
        {
            Properties.Settings.Default.UserSessionId = 0;
            Properties.Settings.Default.Save();
            GeosApplication.Instance.UserSettings["UserSessionDetail"] = "";
            GeosApplication.Instance.UserSettings["UserSessionPassword"] = "";
            List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
            {
                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
            }
            ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void SetLoginNameErrorCode()
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                LoginNameErrorColor = "#F8274D";
            }
            else
            {
                LoginNameErrorColor = "Transparent";
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void SetLoginPasswordErrorCode()
        {
            if (string.IsNullOrWhiteSpace(loginPassword))
            {
                LoginPasswordErrorColor = "#F8274D";
            }
            else
            {
                LoginPasswordErrorColor = "Transparent";
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void SetLoginPersonateNameErrorCode()
        {
            if (string.IsNullOrWhiteSpace(loginImpersonateName))
            {
                LoginImpersonateNameErrorColor = "#F8274D";
            }
            else
            {
                LoginImpersonateNameErrorColor = "Transparent";
            }
        }
        #endregion  // Methods

        #region validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                string error = null;

                IDataErrorInfo me = (IDataErrorInfo)this;
				//[nsatpute][03.09.2025][GEOS2-9342]
                if (IsImpersonateViewVisible == Visibility.Visible)
                {
                    try
                    {
                        error =  me[BindableBase.GetPropertyName(() => LoginImpersonateName)];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        error = me[BindableBase.GetPropertyName(() => LoginPassword)]
                       + me[BindableBase.GetPropertyName(() => LoginName)];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

		//[nsatpute][03.09.2025][GEOS2-9342]
        string IDataErrorInfo.this[string columnName]
        {            
            get
            {
                if (IsImpersonateViewVisible == Visibility.Visible)
                {
                    if (!allowValidation) return null;
                   
                    if (columnName == "LoginImpersonateName")
                    {
                        return CustomRequiredFieldValidation.GetErrorMessage("LoginImpersonateName", LoginImpersonateName);
                    }
                }
                else
                {
                    if (!allowValidation) return null;
                    if (columnName == "LoginName")
                    {
                        return CustomRequiredFieldValidation.GetErrorMessage("LoginName", LoginName);
                    }
                    if (columnName == "LoginPassword")
                    {
                        return CustomRequiredFieldValidation.GetErrorMessage("LoginPassword", LoginPassword);
                    }
                }
                return null;
            }
        }

       


        #endregion

        ~LoginWindowViewModel()
        {

        }

    }
}
