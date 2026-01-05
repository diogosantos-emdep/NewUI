using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class SubTaskCommentsViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        //[Shweta.Thube][GEOS2-7004]

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        #endregion

        #region public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Declarations
        private string windowHeader;
        private bool isSave;
        private string commentButtonText;
        private bool isAdd;
        private bool showCommentsFlyout;
        private string newTaskComment;
        private string oldTaskComment;
        private bool isRtf;
        private bool isNormal = true;
        private ObservableCollection<CommentsByTask> subTaskCommentsList;
        private List<CommentsByTask> clonedSubTaskCommentsList;
        private Object selectedComment;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private int commentboxHeight;
        private int screenHeight;
        private int windowHeight;
        private Visibility textboxnormal;
        private Visibility richtextboxrtf;
        private double dialogHeight;
        private double dialogWidth;
        private bool isOpen;
        private bool isBusy;
        private Int64 idSubTask;
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

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public string CommentButtonText
        {
            get { return commentButtonText; }
            set
            {
                commentButtonText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentButtonText"));
            }
        }
        public bool IsAdd
        {
            get { return isAdd; }
            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }

        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
            }
        }

        public string NewTaskComment
        {
            get { return newTaskComment; }
            set
            {
                newTaskComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewTaskComment"));
            }
        }

        public string OldTaskComment
        {
            get { return oldTaskComment; }
            set
            {
                oldTaskComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldTaskComment"));
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

        public ObservableCollection<CommentsByTask> SubTaskCommentsList
        {
            get { return subTaskCommentsList; }
            set
            {
                subTaskCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubTaskCommentsList"));
            }
        }
        public List<CommentsByTask> ClonedSubTaskCommentsList
        {
            get { return clonedSubTaskCommentsList; }
            set
            {
                clonedSubTaskCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedSubTaskCommentsList"));
            }
        }

        public Object SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
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

        public int CommentboxHeight
        {
            get
            {
                return commentboxHeight;
            }

            set
            {
                commentboxHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentboxHeight"));
            }
        }
        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }

            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }

        public Visibility Textboxnormal
        {
            get
            {
                return textboxnormal;
            }

            set
            {
                textboxnormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Textboxnormal"));
            }
        }
        public Visibility Richtextboxrtf
        {
            get
            {
                return richtextboxrtf;
            }

            set
            {
                richtextboxrtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Richtextboxrtf"));
            }
        }
        public bool IsRtf
        {
            get { return isRtf; }
            set
            {
                isRtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRtf"));
                if (isRtf == true)
                {

                    Textboxnormal = Visibility.Collapsed;
                    Richtextboxrtf = Visibility.Visible;
                }
                else
                {
                    Textboxnormal = Visibility.Visible;
                    Richtextboxrtf = Visibility.Collapsed;
                }
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOpen"));
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

        public Int64 IdSubTask
        {
            get { return idSubTask; }
            set
            {
                idSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSubTask"));
            }
        }

        #endregion

        #region Public ICommands
        public ICommand CancelButtonCommand { get; set; }

        public ICommand CommentButtonCheckedCommand { get; set; }

        public ICommand CommentButtonUncheckedCommand { get; set; }

        public ICommand AddNewCommentCommand { get; set; }

        public ICommand AcceptButtonActionCommand { get; set; }

        public ICommand IsnormalPreviewMouseRightButtonDown { get; set; }

        public ICommand CommentsGridDoubleClickCommand { get; set; }

        public ICommand DeleteCommentRowCommand { get; set; }

        public ICommand RichTextResizingCommand { get; set; }
        #endregion

        #region Constructor
        public SubTaskCommentsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SubTaskCommentsViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 300;
                screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100;
                WindowHeight = screenHeight - 340;
                CommentboxHeight = screenHeight - 550;
                AcceptButtonActionCommand = new RelayCommand(new Action<object>(AcceptButtonActionCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                CommentButtonCheckedCommand = new RelayCommand(new Action<object>(CommentButtonCheckedCommandAction));
                CommentButtonUncheckedCommand = new RelayCommand(new Action<object>(CommentButtonUncheckedCommandAction));
                AddNewCommentCommand = new RelayCommand(new Action<object>(AddCommentCommandAction));
                IsnormalPreviewMouseRightButtonDown = new DelegateCommand<object>(IsnormalCommandAction);
                CommentsGridDoubleClickCommand = new RelayCommand(new Action<object>(CommentDoubleClickCommandAction));
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);

                GeosApplication.Instance.Logger.Log("Constructor SubTaskCommentsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SubTaskCommentsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Methods

        public void Init(APMActionPlanSubTask selectedSubTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
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
                }
                //IdActionPlan = selectedSubTask.IdActionPlan;
                //TaskNumber = selectedSubTask.TaskNumber;
                IdSubTask = selectedSubTask.IdActionPlanTask;
                FillSubTaskCommentsList(IdSubTask);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillSubTaskCommentsList(Int64 idSubTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCommentsList ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController("localhost:6699");
                SubTaskCommentsList = new ObservableCollection<CommentsByTask>(APMService.GetSubTaskCommentsByIdSubTask_V2650(idSubTask));

                if (SubTaskCommentsList?.Count > 0)
                {
                    SubTaskCommentsList = new ObservableCollection<CommentsByTask>(SubTaskCommentsList.OrderByDescending(c => c.CreatedIn).ToList());
                    foreach (var item in SubTaskCommentsList)
                    {
                        if (item.IdUser == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
                            item.IsDeleted = true;
                        else
                            item.IsDeleted = false;
                    }
                    SetUserProfileImageForIdUser(SubTaskCommentsList);

                    if (ClonedSubTaskCommentsList == null)
                    {
                        ClonedSubTaskCommentsList = new List<CommentsByTask>();
                    }
                    ClonedSubTaskCommentsList = SubTaskCommentsList.Select(i => (CommentsByTask)i.Clone()).ToList();
                }
                //RtfToPlaintext();
                GeosApplication.Instance.Logger.Log("Method FillSubTaskCommentsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskCommentsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskCommentsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskCommentsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction ...", category: Category.Info, priority: Priority.Low);
                List<CommentsByTask> commentsList = new List<CommentsByTask>();
                if (ClonedSubTaskCommentsList == null)
                {
                    ClonedSubTaskCommentsList = new List<CommentsByTask>();
                }

                //Add
                foreach (CommentsByTask item in SubTaskCommentsList)
                {

                    if (!ClonedSubTaskCommentsList.Any(x => x.IdActionPlanTaskComment == item.IdActionPlanTaskComment))
                    {
                        CommentsByTask commentsByTask = (CommentsByTask)item.Clone();
                        commentsByTask.IdTask = IdSubTask;
                        commentsByTask.TransactionOperation = ModelBase.TransactionOperations.Add;
                        commentsByTask.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                        commentsList.Add(commentsByTask);
                    }
                }

                //Update
                foreach (CommentsByTask item in ClonedSubTaskCommentsList)
                {
                    if (SubTaskCommentsList.Any(x => x.IdActionPlanTaskComment == item.IdActionPlanTaskComment))
                    {
                        CommentsByTask updated = SubTaskCommentsList.FirstOrDefault(a => a.IdActionPlanTaskComment == item.IdActionPlanTaskComment);
                        if (updated.Comments != item.Comments || updated.IsRtfText != item.IsRtfText)
                        {
                            CommentsByTask updateClone = (CommentsByTask)updated.Clone();
                            updateClone.IdTask = IdSubTask;
                            updateClone.TransactionOperation = ModelBase.TransactionOperations.Update;
                            updateClone.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                            commentsList.Add(updateClone);


                        }
                    }
                }

                //Delete
                foreach (CommentsByTask item in ClonedSubTaskCommentsList)
                {
                    if (!SubTaskCommentsList.Any(x => x.IdActionPlanTaskComment == item.IdActionPlanTaskComment))
                    {
                        CommentsByTask commentsByTask = (CommentsByTask)item.Clone();
                        commentsByTask.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        commentsByTask.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                        commentsList.Add(commentsByTask);


                    }
                }

                if (commentsList != null)
                {
                    commentsList.ToList().ForEach(X => X.People.OwnerImage = null);
                    //APMService = new APMServiceController("localhost:6699");
                    commentsList = APMService.AddUpdateDeleteCommentsByIdSubTask_V2650(commentsList);

                    IsSave = true;
                }
                if (IsSave)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanTaskCommentsAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonActionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetUserProfileImageForIdUser(ObservableCollection<CommentsByTask> TaskCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImageForIdUser ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in TaskCommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_White.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_White.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/wUnknownGender.png");

                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_Blue.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_Blue.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/blueUnknownGender.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImageForIdUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private ImageSource SetUserProfileImage()
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
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_White.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/blueUnknownGender.png");
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

        BitmapImage GetImage(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }
        public void AddCommentCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (SubTaskCommentsList == null)
                {
                    SubTaskCommentsList = new ObservableCollection<CommentsByTask>();
                }
                string TempOldTaskComment = string.Empty;
                string TempNewTaskComment = string.Empty;
                if (IsRtf)
                {
                    var document = ((RichTextBox)gcComments).Document;
                    NewTaskComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                    TempNewTaskComment = NewTaskComment;
                    string convertedText = string.Empty;

                    if (!string.IsNullOrEmpty(NewTaskComment.Trim()))
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                            TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                            range2.Save(ms, DataFormats.Rtf);
                            ms.Seek(0, SeekOrigin.Begin);
                            using (StreamReader sr = new StreamReader(ms))
                            {
                                convertedText = sr.ReadToEnd();
                            }
                        }
                    }
                    NewTaskComment = convertedText;
                }
                else
                {
                    TempNewTaskComment = NewTaskComment;
                }

                if (OldTaskComment != null && !string.IsNullOrEmpty(OldTaskComment.Trim()) && OldTaskComment.Equals(NewTaskComment.Trim()))
                {
                    ShowCommentsFlyout = false;
                    return;
                }

                // Update comment.
                if (CommentButtonText == System.Windows.Application.Current.FindResource("APMViewUpdateComment").ToString())
                {
                    if (!string.IsNullOrEmpty(NewTaskComment) && !string.IsNullOrEmpty(NewTaskComment.Trim()))
                    {
                        CommentsByTask comment = SubTaskCommentsList.FirstOrDefault(x => x.Comments == OldTaskComment);

                        if (comment != null)
                        {
                            comment.Comments = string.Copy(NewTaskComment.Trim());
                            comment.CreatedIn = GeosApplication.Instance.ServerDateTime;

                            SelectedComment = comment;
                            if (comment.IdActionPlanTaskComment != 0)
                                comment.IsUpdated = true;
                            else
                                comment.IsUpdated = false;
                            comment.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                            comment.IsDeleted = true;
                            comment.IsRtfText = comment.IsRtfText;

                            if (comment.IsRtfText)
                            {
                                TextRange range = null;
                                var rtb = new RichTextBox();
                                var doc = new FlowDocument();
                                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(OldTaskComment.ToString()));
                                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                range.Load(stream, DataFormats.Rtf);

                                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                                    TempOldTaskComment = range.Text;
                            }
                            else
                            {
                                TempOldTaskComment = OldTaskComment;
                            }

                            if (IsRtf)
                            {
                                comment.IsRtfText = true;
                                IsRtf = false;

                            }
                            else if (IsNormal)
                            {
                                comment.IsRtfText = false;
                            }

                            TempOldTaskComment = "'" + TempOldTaskComment + "'";
                            TempOldTaskComment = TempOldTaskComment.Replace("\r\n", "").TrimEnd();

                            TempNewTaskComment = "'" + TempNewTaskComment + "'";
                            TempNewTaskComment = TempNewTaskComment.TrimEnd();

                            //ChangedLogsEntries = new List<LogEntriesByActivity>();
                            //ChangedLogsEntries.Add(new LogEntriesByActivity() { IdLogEntryByActivity = comment.IdLogEntryByActivity, IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentUpdated").ToString(), TempOldActivityComment, TempNewActivityComment), IdLogEntryType = 2 });
                        }

                        OldTaskComment = null;
                        NewTaskComment = null;
                    }

                    SubTaskCommentsList = new ObservableCollection<CommentsByTask>(SubTaskCommentsList.OrderByDescending(c => c.CreatedIn).ToList());
                }
                else if (CommentButtonText == System.Windows.Application.Current.FindResource("APMViewAddComment").ToString()) //Add comment.
                {
                    if (!string.IsNullOrEmpty(NewTaskComment) && !string.IsNullOrEmpty(NewTaskComment.Trim())) // Add Comment
                    {
                        if (IsRtf)
                        {
                            CommentsByTask comment = new CommentsByTask()
                            {
                                People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                CreatedIn = GeosApplication.Instance.ServerDateTime,
                                Comments = string.Copy(NewTaskComment.Trim()),
                                //  IdActivity = _Activity.IdActivity,
                                //  IdLogEntryType = 1,
                                IsUpdated = false,
                                IsDeleted = false,
                                IsRtfText = true
                            };
                            comment.People.OwnerImage = SetUserProfileImage();
                            SubTaskCommentsList.Add(comment);
                            SubTaskCommentsList = new ObservableCollection<CommentsByTask>(SubTaskCommentsList.OrderByDescending(c => c.CreatedIn).ToList());
                            SelectedComment = comment;
                        }
                        else if (IsNormal)
                        {
                            CommentsByTask comment = new CommentsByTask()
                            {
                                People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                CreatedIn = GeosApplication.Instance.ServerDateTime,
                                Comments = string.Copy(NewTaskComment.Trim()),
                                //  IdActivity = _Activity.IdActivity,
                                //  IdLogEntryType = 1,
                                IsUpdated = false,
                                IsDeleted = false,
                                IsRtfText = false
                            };
                            comment.People.OwnerImage = SetUserProfileImage();
                            SubTaskCommentsList.Add(comment);
                            SubTaskCommentsList = new ObservableCollection<CommentsByTask>(SubTaskCommentsList.OrderByDescending(c => c.CreatedIn).ToList());
                            SelectedComment = comment;
                        }

                        OldTaskComment = null;
                        NewTaskComment = null;
                    }
                }


                ShowCommentsFlyout = false;
                NewTaskComment = "";
                IsRtf = false;
                IsNormal = true;
                IsOpen = false;

                GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCommentCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCommentCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCommentCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void CommentButtonUncheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CommentButtonUncheckedCommandAction()..."), category: Category.Info, priority: Priority.Low);
                CommentButtonText = System.Windows.Application.Current.FindResource("APMViewAddComment").ToString();

                IsOpen = false;

                IsAdd = true;
                ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
                if (string.IsNullOrEmpty(NewTaskComment))
                {
                    NewTaskComment = null;
                }

                OldTaskComment = null;
                IsRtf = false;
                IsNormal = true;
                if (IsNormal == true)
                {
                    Textboxnormal = Visibility.Visible;
                    Richtextboxrtf = Visibility.Collapsed;
                }
                else
                {
                    Textboxnormal = Visibility.Collapsed;
                    Richtextboxrtf = Visibility.Visible;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method CommentButtonUncheckedCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommentButtonUncheckedCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommentButtonCheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CommentButtonCheckedCommandAction()..."), category: Category.Info, priority: Priority.Low);

                CommentButtonText = System.Windows.Application.Current.FindResource("APMViewAddComment").ToString();
                IsOpen = true;
                IsAdd = true;
                ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
                if (string.IsNullOrEmpty(NewTaskComment))
                {
                    NewTaskComment = null;
                }

                OldTaskComment = "";
                IsRtf = false;
                IsNormal = true;
                if (IsNormal == true)
                {
                    Textboxnormal = Visibility.Visible;
                    Richtextboxrtf = Visibility.Collapsed;
                }
                else
                {
                    Textboxnormal = Visibility.Collapsed;
                    Richtextboxrtf = Visibility.Visible;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method CommentButtonCheckedCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommentButtonCheckedCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert ByteArrayToBitmapImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
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
                GeosApplication.Instance.Logger.Log("Method ByteArrayToBitmapImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToBitmapImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void IsnormalCommandAction(object gcComments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method IsnormalCommandAction()..."), category: Category.Info, priority: Priority.Low);
                string convertedText = string.Empty;
                if (IsNormal)
                {
                    var document = ((RichTextBox)gcComments).Document;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Text);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            convertedText = sr.ReadToEnd();
                        }
                    }
                    NewTaskComment = convertedText;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method IsnormalCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method IsnormalCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void CommentDoubleClickCommandAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CommentDoubleClickCommandAction()..."), category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (obj == null) return;
                CommentsByTask commentOffer = (CommentsByTask)obj;
                if ((!GeosApplication.Instance.IsPermissionReadOnly && (commentOffer.IdUser == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)))
                {

                    CommentButtonText = System.Windows.Application.Current.FindResource("APMViewUpdateComment").ToString();
                    IsAdd = false;
                    OldTaskComment = String.Copy(commentOffer.Comments);
                    NewTaskComment = String.Copy(commentOffer.Comments);

                    if (commentOffer.IsRtfText == true)
                        IsRtf = true;
                    else
                        IsNormal = true;

                    ShowCommentsFlyout = true;
                    IsOpen = true;
                }
                IsBusy = false;

                GeosApplication.Instance.Logger.Log(string.Format("Method CommentDoubleClickCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommentDoubleClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void DeleteCommentCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteCommentCommandAction()..."), category: Category.Info, priority: Priority.Low);
                if (parameter == null) return;
                CommentsByTask commentOffer = (CommentsByTask)parameter;
                if ((!GeosApplication.Instance.IsPermissionReadOnly && (commentOffer.IdUser == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteCommentMessageBox").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (SubTaskCommentsList != null && SubTaskCommentsList.Count > 0)
                        {
                            SubTaskCommentsList.Remove(SubTaskCommentsList.FirstOrDefault(x => x.IdActionPlanTaskComment == commentOffer.IdActionPlanTaskComment && x.Comments == commentOffer.Comments));
                        }
                    }
                }
                ShowCommentsFlyout = false;
                NewTaskComment = null;
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteCommentCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteCommentCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ResizeRichTextEditor(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method ResizeRichTextEditor()..."), category: Category.Info, priority: Priority.Low);
                RichEditControl edit = (RichEditControl)obj;
                DevExpress.XtraRichEdit.API.Native.Document currentDocument = edit.Document;
                currentDocument.DefaultCharacterProperties.FontName = GeosApplication.Instance.FontFamilyAsPerTheme.ToString();
                DocumentLayout currentDocumentLayout = edit.DocumentLayout;

                edit.BeginInvoke(() =>
                {
                    SubDocument subDocument = currentDocument.CaretPosition.BeginUpdateDocument();
                    DocumentPosition docPosition = subDocument.CreatePosition(((currentDocument.CaretPosition.ToInt() == 0) ? 0 : currentDocument.CaretPosition.ToInt() - 1));

                    double height = 0;
                    System.Drawing.Point pos = PageLayoutHelper.GetInformationAboutCurrentPage(currentDocumentLayout, docPosition);
                    height = DevExpress.Office.Utils.Units.TwipsToPixels(pos, edit.DpiX, edit.DpiY).Y;

                    edit.Height = height + 50;
                    edit.VerticalScrollValue = 0;
                    currentDocument.CaretPosition.EndUpdateDocument(subDocument);
                });
                GeosApplication.Instance.Logger.Log(string.Format("Method ResizeRichTextEditor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ResizeRichTextEditor...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}