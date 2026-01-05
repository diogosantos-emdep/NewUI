using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Emdep.Geos.Utility.Text;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Workbench.Views;

namespace Workbench.ViewModels
{
    class EditProfileViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  //Services

        #region Declaration

        private byte[] profileImage = null;
        private ImageSource profileImageSource;
        private string isProfileAlertShow;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;

        private string newPassword;             // New Password
        private string passwordStrenthColor;    // New Password check password strenth
        private string passwordStrenth;         // New Password check password strenth
        private string confirmPassword;
        private string oldPassword;
        private string login;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string mobileNumber;
        private string location;
        private bool isBusy;

        #endregion  // Declaration

        #region public Properties

        public System.Windows.Forms.DialogResult DialogResult { get; set; }

        public byte[] ProfileImage
        {
            get { return profileImage; }
            set { profileImage = value; OnPropertyChanged(new PropertyChangedEventArgs("ProfileImage")); }
        }

        public ImageSource ProfileImageSource
        {
            get { return profileImageSource; }
            set { profileImageSource = value; OnPropertyChanged(new PropertyChangedEventArgs("ProfileImageSource")); }
        }

        public string IsProfileAlertShow
        {
            get { return isProfileAlertShow; }
            set { isProfileAlertShow = value; ; OnPropertyChanged(new PropertyChangedEventArgs("IsProfileAlertShow")); }
        }

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set { userProfileImage = value; OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage")); }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("NewPassword")); }
        }

        public string PasswordStrenthColor
        {
            get { return passwordStrenthColor; }
            set { passwordStrenthColor = value; OnPropertyChanged(new PropertyChangedEventArgs("PasswordStrenthColor")); }
        }

        public string PasswordStrenth
        {
            get { return passwordStrenth; }
            set { passwordStrenth = value; OnPropertyChanged(new PropertyChangedEventArgs("PasswordStrenth")); }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("ConfirmPassword")); }
        }

        public string OldPassword
        {
            get { return oldPassword; }
            set { oldPassword = value; OnPropertyChanged(new PropertyChangedEventArgs("OldPassword")); }
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
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(new PropertyChangedEventArgs("Login")); }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(new PropertyChangedEventArgs("FirstName")); }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(new PropertyChangedEventArgs("LastName")); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(new PropertyChangedEventArgs("Email")); }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(new PropertyChangedEventArgs("Email")); }
        }

        public string MobileNumber
        {
            get { return mobileNumber; }
            set { mobileNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("MobileNumber")); }
        }

        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(new PropertyChangedEventArgs("Location")); }
        }

        #endregion  // Properties

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

        #endregion  // Enum

        #region Events

        public event EventHandler RequestClose; //supportWindow for close window

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion  // Events

        #region Commands

        public ICommand EditProfileAcceptButtonCommand { get; set; }
        public ICommand EditProfileCancelButtonCommand { get; set; }
        public ICommand EditProfileRemoveButtonCommand { get; set; }
        public ICommand ValidateProfilePicture { get; set; }
        public ICommand WorkbenchWindowChangePasswordCommand { get; set; }      // WorkbenchWindow Change Password
        public ICommand WorkbenchWindowChangePasswordCloseButtonCommand { get; set; }
        public ICommand ChangePassrowdAcceptButtonCommand { get; set; }
        public ICommand NewPasswordCodeCommand { get; set; }

        #endregion  // Commands

        #region Constructor

        public EditProfileViewModel()
        {
            GeosApplication.Instance.Logger.Log("Method EditProfileViewModel...", category: Category.Info, priority: Priority.Low);
            try
            {
                User userinfo = WorkbenchStartUp.GetUserProfileDetailsById(GeosApplication.Instance.ActiveUser.IdUser);
                FirstName = userinfo.FirstName;
                LastName = userinfo.LastName;
                Login = userinfo.Login;
                Email = userinfo.CompanyEmail;
                Phone = userinfo.Phone;
                Location = userinfo.Company.ShortName;

                try
                {
                    //IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login - EditProfileViewModel() ", category: Category.Info, priority: Priority.Low);
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(GeosApplication.Instance.ActiveUser.Login);
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login successfully - EditProfileViewModel() ", category: Category.Info, priority: Priority.Low);

                    UserProfileImage = byteArrayToImage(UserProfileImageByte);
                }
                catch (FaultException<ServiceException> ex)
                {
                    //GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    if (UserProfileImageByte == null)
                    {
                        bool isImageColor;
                        try
                        {
                            User user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                            if (ThemeManager.ApplicationThemeName == "WhiteAndBlue")
                            {
                                if (user.IdUserGender != null)
                                {
                                    if (user.IdUserGender == 1)
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/femaleUserBlue.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                                    }
                                    else
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/maleUserBlue.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                                    }
                                }

                            }
                            else
                            {
                                if (user.IdUserGender != null)
                                {
                                    if (user.IdUserGender == 1)
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/femaleUserWhite.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                                    }
                                    else
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/maleUserWhite.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                                    }
                                }
                            }
                        }
                        catch (FaultException<ServiceException> innerEx)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method " + innerEx.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(innerEx.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException innerEx)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(innerEx.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                    }
                }
                catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                {
                    if (UserProfileImageByte == null)
                    {
                        bool isImageColor;
                        try
                        {
                            User user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);

                            if (ThemeManager.ApplicationThemeName == "WhiteAndBlue")
                            {
                                if (user.IdUserGender != null)
                                {
                                    if (user.IdUserGender == 1)
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/femaleUserBlue.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                                    }
                                    else
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/maleUserBlue.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginBlue.png", UriKind.RelativeOrAbsolute));
                                    }
                                }

                            }
                            else
                            {
                                if (user.IdUserGender != null)
                                {
                                    if (user.IdUserGender == 1)
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/femaleUserWhite.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                                    }
                                    else
                                    {
                                        ProfileImageSource = ConvertImageToImageSource("/Assets/Images/maleUserWhite.png");
                                        //ImpersonateUserProfileImage = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png", UriKind.RelativeOrAbsolute));
                                        ProfileImageSource = new BitmapImage(new Uri("pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserLoginWhite.png", UriKind.RelativeOrAbsolute));
                                    }
                                }
                            }
                        }
                        catch (FaultException<ServiceException> innerEx)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method " + innerEx.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(innerEx.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException innerEx)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                    }
                    // UserProfileImage = ConvertImageToImageSource("/Assets/Images/UserProfile.png");
                }

                if (GeosApplication.Instance.ActiveUser.IsValidated != null && GeosApplication.Instance.ActiveUser.IsValidated == 0)
                {
                    //IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    IsProfileAlertShow = "Visible";
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login if ActiveUser Is Validated ", category: Category.Info, priority: Priority.Low);
                    ProfileImage = GeosRepositoryServiceController.GetUserProfileImage(GeosApplication.Instance.ActiveUser.Login, 0);
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login if ActiveUser Is Validated successfully ", category: Category.Info, priority: Priority.Low);

                    ProfileImageSource = byteArrayToImage(ProfileImage);
                }
                else
                {
                    //IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    IsProfileAlertShow = "Hidden";
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login if ActiveUser Is not Validated ", category: Category.Info, priority: Priority.Low);
                    ProfileImage = GeosRepositoryServiceController.GetUserProfileImage(GeosApplication.Instance.ActiveUser.Login);
                    GeosApplication.Instance.Logger.Log("Getting User GetUserProfileImage by login if ActiveUser Is not Validated successfully ", category: Category.Info, priority: Priority.Low);
                    ProfileImageSource = byteArrayToImage(ProfileImage);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                // CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //ProfileImageSource = ConvertImageToImageSource("/Assets/Images/UserProfile.png");
                UserProfileImage = ConvertImageToImageSource("/Assets/Images/UserProfile.png");
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditProfileViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                UserProfileImage = ConvertImageToImageSource("/Assets/Images/UserProfile.png");
            }

            EditProfileAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptEditProfile));
            EditProfileCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ValidateProfilePicture = new RelayCommand(new Action<object>(ValidateUserProfilePicture));
            EditProfileRemoveButtonCommand = new RelayCommand(new Action<object>(RemoveUserProfilePicture));
            WorkbenchWindowChangePasswordCommand = new RelayCommand(new Action<object>(ShowChangePasswordWindow));
            WorkbenchWindowChangePasswordCloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ChangePassrowdAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptNewPassword));
            NewPasswordCodeCommand = new RelayCommand(new Action<object>(NewPasswordStrenth));

            GeosApplication.Instance.Logger.Log("Method EditProfileViewModel() executed successfully...", category: Category.Info, priority: Priority.Low);
        }

        #endregion  // Constructor

        #region Methods

        /// <summary>
        ///  /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
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
        /// This method is convert bytearry to ImageSource
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
        /// This method is for to accept edit profile window changes and save it to database
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptEditProfile(object obj)
        {
            IsBusy = true;
            try
            {
                GeosApplication.Instance.Logger.Log("Click on accept button of editprofile ", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("Start AcceptEditProfile Method", category: Category.Info, priority: Priority.Low);

                if (ProfileImageSource == null && profileImage != null)
                {
                    //IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Getting Delete old User Profile Image", category: Category.Info, priority: Priority.Low);
                    WorkbenchStartUp.DeleteUserProfileImage(GeosApplication.Instance.ActiveUser);
                    GeosApplication.Instance.Logger.Log("Getting Delete old User Profile Image successfully", category: Category.Info, priority: Priority.Low);

                    GeosApplication.Instance.ActiveUser.IsValidated = 1;
                    IsProfileAlertShow = "Hidden";
                }

                if (ProfileImageSource != null)
                {
                    ProfileImage = ImageSourceToBytes(ProfileImageSource);

                    byte[] profileImageInByte = (byte[])profileImage;

                    FileUploader userProfileFileUploader = new FileUploader();
                    userProfileFileUploader.FileByte = profileImageInByte;// user profile image file in bytes (byte[] Byte)
                    userProfileFileUploader.FileUploadName = GeosApplication.Instance.ActiveUser.Login;//login name of user
                    //GeosRepositoryServiceController fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Getting Upload new User Profile Image", category: Category.Info, priority: Priority.Low);
                    GeosRepositoryServiceController.UploadIsValidateFalseUserImage(userProfileFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload new User Profile Image successfully", category: Category.Info, priority: Priority.Low);

                    //IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    //GeosApplication.Instance.Logger.Log("set user uploaded new Profile Image not validete", category: Category.Info, priority: Priority.Low);
                    //WorkbenchStartUp.UpdateUserIsValidateFalse(GeosApplication.Instance.ActiveUser);
                    //GeosApplication.Instance.Logger.Log("set user uploaded new Profile Image not validete successfully", category: Category.Info, priority: Priority.Low);

                    GeosApplication.Instance.Logger.Log("set user uploaded new Profile Image is validete", category: Category.Info, priority: Priority.Low);
                    WorkbenchStartUp.UpdateUserIsValidateTrue(GeosApplication.Instance.ActiveUser);
                    GeosApplication.Instance.Logger.Log("set user uploaded new Profile Image is validete successfully", category: Category.Info, priority: Priority.Low);
                    
                    //GeosApplication.Instance.ActiveUser.IsValidated = 0;
                    GeosApplication.Instance.ActiveUser.IsValidated = 1;
                    GeosApplication.Instance.ServerActiveMethod();
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptEditProfile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error On AcceptEditProfile Method - ServiceUnexceptedException ", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //if (!GeosApplication.Instance.IsServiceActive)
                //{
                    GeosApplication.Instance.ServerDeactiveMethod();
                //}
            }

            IsBusy = false;

            GeosApplication.Instance.Logger.Log("End AcceptEditProfile Method", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
        }

        /// <summary>
        /// This method is for to convert ImageSource to ByteArray
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
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

        /// <summary>
        /// This method is buffer from imageSource
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public Byte[] BufferFromImage(BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            Byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }

            return buffer;
        }

        /// <summary>
        /// Method for Remove user profile picture 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveUserProfilePicture(object obj)
        {
        }

        /// <summary>
        /// Method for validate User pending Image.
        /// </summary>
        /// <param name="obj"></param>
        public void ValidateUserProfilePicture(object obj)
        {
            try
            {
                //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                WorkbenchStartUp.SetUserProfileImage(GeosApplication.Instance.ActiveUser);
                GeosApplication.Instance.ActiveUser.IsValidated = 1;
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ValidateUserProfilePicture() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for show password change window on Edite Profile
        /// </summary>
        /// <param name="obj"></param>
        public void ShowChangePasswordWindow(object obj)
        {
            IsBusy = true;
            GeosApplication.Instance.Logger.Log("Click on Change Password link", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log("Initialising ChangePass wordWindow", category: Category.Info, priority: Priority.Low);
            ChangePasswordWindow ChangePasswordWindow = new ChangePasswordWindow();
            EditProfileViewModel EditProfileViewModel = new EditProfileViewModel();
            EventHandler handle = delegate { ChangePasswordWindow.Close(); };
            EditProfileViewModel.RequestClose += handle;
            ChangePasswordWindow.DataContext = EditProfileViewModel;
            GeosApplication.Instance.Logger.Log("Initialising ChangePass wordWindow Successfully", category: Category.Info, priority: Priority.Low);
            IsBusy = false;
            ChangePasswordWindow.ShowDialogWindow();
        }

        /// <summary>
        /// This method is to Accept Button click   for NewPassword in  NewPasswordView   
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptNewPassword(object obj)
        {
            IsBusy = true;
            try
            {
                GeosApplication.Instance.Logger.Log("Click on Accept button of Change Password", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("Start AcceptNewPassword Method", category: Category.Info, priority: Priority.Low);
                if (NewPassword != null && OldPassword != null && ConfirmPassword != null)
                {
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
                                string loginPasswordEncrypt = string.Empty;
                                string OldPasswordEncrypt = string.Empty;
                                loginPasswordEncrypt = Encrypt.Encryption(ConfirmPassword.Trim());
                                OldPasswordEncrypt = Encrypt.Encryption(OldPassword.Trim());
                                //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                GeosApplication.Instance.Logger.Log("Getting change user passrord ", category: Category.Info, priority: Priority.Low);
                                User User = WorkbenchStartUp.ChangeUserPassword(loginPasswordEncrypt, OldPasswordEncrypt, GeosApplication.Instance.ActiveUser.IdUser);
                                GeosApplication.Instance.Logger.Log("Getting change user passrord successfully", category: Category.Info, priority: Priority.Low);

                                NewPassword = string.Empty;
                                ConfirmPassword = string.Empty;
                                OldPassword = string.Empty;
                                if (User != null)
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["NewPasswordViewPasswordChangeValidation"].ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                                    //reset remember password set UserSessionId 0
                                    Properties.Settings.Default.UserSessionId = 0;
                                    Properties.Settings.Default.Save();
                                    RequestClose(null, null);
                                }
                                else
                                {
                                    CustomMessageBox.Show(Workbench.App.Current.Resources["NewPasswordViewPasswordNotMatchChangeValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(Workbench.App.Current.Resources["NewPasswordViewPasswordNotMatchChangeValidation"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                NewPassword = string.Empty;
                                ConfirmPassword = string.Empty;
                                OldPassword = string.Empty;
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(Workbench.App.Current.Resources["ChangePasswordWindowpleaseenterpassword"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }

                        GeosApplication.Instance.ServerActiveMethod();
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptNewPassword() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error On AcceptNewPassword Method", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //if (!GeosApplication.Instance.IsServiceActive)
                //{
                    GeosApplication.Instance.ServerDeactiveMethod();
                //}
            }

            GeosApplication.Instance.Logger.Log("End AcceptNewPassword Method", category: Category.Info, priority: Priority.Low);
            IsBusy = false;
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
                    PasswordStrenthColorShow(PasswordStrenth);
                }

                GeosApplication.Instance.ServerActiveMethod();
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //if (!GeosApplication.Instance.IsServiceActive)
                //{
                    GeosApplication.Instance.ServerDeactiveMethod();
                //}
            }
        }

        /// <summary>
        /// This method is for to calculate password strenth
        /// </summary>
        /// <param name="passwordLegnth"></param>
        private void PasswordStrenthColorShow(string passwordLegnth)
        {
            if (passwordLegnth == "")
            {
                PasswordStrenthColor = "#000000";
            }
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
        /// Method for close Edit Profile window
        /// </summary>
        /// <param name="obj"></param>
        public void CloseWindow(object obj)
        {
            RequestClose(null, null);
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

        public Bitmap CreateBitmapImage(string sImageText, bool isImageColor)
        {
            Bitmap objBmpImage = new Bitmap(2, 2);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

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
                objGraphics.Clear(System.Drawing.Color.Blue);
            else
                objGraphics.Clear(System.Drawing.Color.White);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;

            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);

            objGraphics.Flush();

            return (objBmpImage);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        #endregion   // Methods
    }
}
