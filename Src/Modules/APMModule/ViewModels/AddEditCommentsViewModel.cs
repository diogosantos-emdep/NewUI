using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit.Import.OpenXml;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DevExpress.Mvvm.DataAnnotations.PredefinedMasks;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    //[nsatpute][24-10-2024][GEOS2-6018]
    public class AddEditCommentsViewModel : INotifyPropertyChanged, IDisposable
    {
        #region service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());



        #endregion

        #region Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Declaration
        private string windowHeader;
        private string commentText;
        byte[] userProfileImageByte = null;
        ImageSource userProfileImage;
        UInt32 idLogEntryByCpType;
        public bool IsEditingMode = false;
        public bool IsSaved = false;
        ActionPlanComment newComment;
        ActionPlanComment editedComment;
        #endregion

        #region Properties 
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }
        public ActionPlanComment NewComment
        {
            get { return newComment; }
            set
            {
                newComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewComment"));
            }
        }
        public ActionPlanComment EditedComment
        {
            get { return editedComment; }
            set
            {
                editedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditedComment"));
            }
        }
        public byte[] UserProfileImageByte
        {
            get { return userProfileImageByte; }
            set
            {
                userProfileImageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImageByte"));
            }
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

        public UInt32 IdLogEntryByCpType
        {
            get { return idLogEntryByCpType; }
            set
            {
                idLogEntryByCpType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdLogEntryByCpType"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        #endregion

        #region Constructor
        public AddEditCommentsViewModel()
        {
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
        }
        #endregion

        #region Methods
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method convert AcceptButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (CommentText == null || commentText.Trim() == string.Empty)
                {
                    CustomMessageBox.Show(Application.Current.Resources["EmptyCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if (IsEditingMode)
                {
                    EditedComment.Comment = CommentText == null ? "" : CommentText.Trim();
                    EditedComment.Datetime = GeosApplication.Instance.ServerDateTime;
                    if (EditedComment.Id == 0)
                        EditedComment.TransactionOperation = ModelBase.TransactionOperations.Add;
                    else
                        EditedComment.TransactionOperation = ModelBase.TransactionOperations.Update;
                    IsSaved = true;
                }
                else
                {
                    NewComment = new ActionPlanComment();
                    NewComment.UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName;
                    NewComment.IdUser = NewComment.CreatedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                    NewComment.Datetime = GeosApplication.Instance.ServerDateTime;
                    NewComment.Comment = CommentText == null ? "" : CommentText.Trim();
                    NewComment.People = new Data.Common.People();
                    NewComment.People.OwnerImage = SetUserProfileImage();
                    NewComment.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    NewComment.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                    NewComment.People.FullName = NewComment.UserName;
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ImageSource SetUserProfileImage()
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(GeosApplication.Instance.ActiveUser.Login);

                if (UserProfileImageByte != null)
                    UserProfileImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return UserProfileImage;
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(byteArrayIn))
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

                //GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit()
        {
            CommentText = EditedComment.Comment;
        }
        #endregion
    }
}
