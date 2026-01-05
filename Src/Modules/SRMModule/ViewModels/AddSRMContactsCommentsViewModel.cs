using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Core;
using System.ServiceModel;
using System.Windows;
using System.ComponentModel;
using System;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using Emdep.Geos.Data.Common.SRM;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class AddSRMContactsCommentsViewModel : INotifyPropertyChanged, IDisposable
    {
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
        #endregion

        #region service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isNormal = true;
        private string oldItemComment;
        private string newItemComment;
        private Int64 idLogEntriesByItem;
        private Int64 idPlanItem;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private ObservableCollection<CommentEntriesByItemsSRM> actionPlanItemCommentsList = new ObservableCollection<CommentEntriesByItemsSRM>();
        private LogEntriesByArticleSuppliers selectedComment;
        private bool isRtf;
        ObservableCollection<LogEntriesByArticleSuppliers> commentsList;
        #endregion

        #region Properties

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public string OldItemComment
        {
            get { return oldItemComment; }
            set
            {
                oldItemComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldItemComment"));
            }
        }
        public string NewItemComment
        {
            get { return newItemComment; }
            set
            {
                newItemComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewItemComment"));
            }
        }
        public Int64 IdLogEntryByItem
        {
            get { return idLogEntriesByItem; }
            set
            {
                idLogEntriesByItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdLogEntryByItem"));
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
        public ObservableCollection<LogEntriesByArticleSuppliers> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }
        public LogEntriesByArticleSuppliers SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }
        #endregion

        #region Command  
        public ICommand CloseWindowCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AddItemsSaveCommentCommand { get; set; }
        #endregion

        #region Constructor 

        public AddSRMContactsCommentsViewModel()
        {

            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindowAction);
            AddItemsSaveCommentCommand = new DelegateCommand<object>(SaveCommentAction);
        }
        #endregion

        #region Methods
        private void CloseWindowAction(object obj)
        {
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        private void SaveCommentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert SaveCommentAction ...", category: Category.Info, priority: Priority.Low);



            if (OldItemComment != null && !string.IsNullOrEmpty(OldItemComment.Trim()) && OldItemComment.Equals(NewItemComment.Trim()))
            {
                //ShowCommentsFlyout = false;
                return;
            }
            //Add new comment
            if (!string.IsNullOrEmpty(NewItemComment) && !string.IsNullOrEmpty(NewItemComment.Trim())) // Add Comment
            {

                LogEntriesByArticleSuppliers comment = new LogEntriesByArticleSuppliers()

                {
                    IdLogEntryByArticleSuppliers = idLogEntriesByItem,
                    UserName = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName,
                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    Datetime = GeosApplication.Instance.ServerDateTime,
                    Comments = string.Copy(NewItemComment.Trim()),

                    IdEntryType = 1719,
                    TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add,
                    IsRtfText = false
                };

                comment.People = new People();
                comment.People.OwnerImage = SetUserProfileImage();
                comment.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                comment.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                String name = comment.UserName;

                comment.People.FullName = comment.UserName;

                if (CommentsList == null)
                {
                    CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>();
                }
                if (comment != null)
                {
                    CommentsList.Add(comment);
                }
                // comment.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add;
                SelectedComment = comment;
                CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(CommentsList.OrderByDescending(c => c.Datetime).ToList());
            }
            IsNormal = true;
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("Method SaveCommentAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
        public void Dispose()
        {
        }
        #endregion
    }
}
