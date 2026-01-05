using System;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.SAM;
using System.Windows.Input;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using Emdep.Geos.UI.Common;
using System.Windows;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common;
using System.ServiceModel;
using Emdep.Geos.UI.Helper;
using System.Linq;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.PCM;
using System.IO;
using System.Collections.Generic;
using Emdep.Geos.Modules.SAM.Views;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class EditItemViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
        //IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
        #endregion // End Services Region

        #region Declaration
        private double dialogHeight;
        private double dialogWidth;
        private Int64 idLogEntriesByItem;
        private OTs ot;
        private MaximizedElementPosition maximizedElementPosition;
        private ObservableCollection<OTAttachment> listAttachment;
        private ObservableCollection<CommentEntriesByItems> actionPlanItemCommentsList;
        private string newItemComment;
        private OtItemsComment commentItem;
        private OTWorkingTime oTWorkingTimeItem;
        private OtItem tempOtItem;
        private SAMLogEntries selectedComment;
        private bool isSave = false;
        private bool isDeleted;
        private bool isBusy;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private Company site;
        private string oldItemComment;
        private ObservableCollection<OTWorkingTime> workLogItemList;
        private string worklogTotalTime;
        private List<UserShortDetail> userImageList;
        private OTWorkingTime selectedWorkLog;
        private long otItems;
        Int64 idOT;
        //private string newItemComment;
        #endregion // End Of Declaration

        #region Properties
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

        public long OtItems
        {
            get { return otItems; }
            set
            {
                otItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItems"));
            }
        }
        public Company Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Site"));
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

        public OTs OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }

        public ObservableCollection<OTAttachment> ListAttachment
        {
            get { return listAttachment; }

            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }

        string code;
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        string customerEI;
        public string Customer
        {
            get { return customerEI; }
            set
            {
                customerEI = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerEI"));
            }
        }

        string reference;
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }
        string comments;
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
            }
        }

        string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        string item;
        public string Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Item"));
            }
        }

        decimal quantity;
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Quantity"));
            }
        }

        private ImageSource referenceImage;
        public ImageSource ReferenceImage
        {
            get { return referenceImage; }
            set
            {
                referenceImage = value;
                if (referenceImage != null)
                {
                    //IsReferenceImageExist = true;
                }
                else
                {
                    ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/ImageEditLogo.png"));
                   // IsReferenceImageExist = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceImage"));
            }
        }

        private ObservableCollection<PCMArticleImage> imagesList;
        public ObservableCollection<PCMArticleImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }

        bool isOrientativePicture;
        public bool IsOrientativePicture
        {
            get { return isOrientativePicture; }
            set
            {
                isOrientativePicture = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOrientativePicture"));
            }
        }
        List<SAMLogEntries> articleChangeLogList;
        public List<SAMLogEntries> ArticleChangeLogList
        {
            get { return articleChangeLogList; }
            set
            {
                articleChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("articleChangeLogList"));
            }
        }


        ObservableCollection<SAMLogEntries> commentsList;
        public ObservableCollection<SAMLogEntries> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }

        ObservableCollection<SAMLogEntries> deleteCommentsList;
        public ObservableCollection<SAMLogEntries> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }

        List<ArticleDecomposition> articleComponents;
        public List<ArticleDecomposition> ArticleComponents
        {
            get { return articleComponents; }
            set
            {
                articleComponents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleComponents"));
            }
        }

        //Comments List
        public ObservableCollection<CommentEntriesByItems> ActionPlanItemCommentsList
        {
            get { return actionPlanItemCommentsList; }
            set
            {
                actionPlanItemCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemCommentsList"));
            }
        }


        public SAMLogEntries SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        public OtItemsComment CommentItem
        {
            get { return commentItem; }
            set
            {
                commentItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentItem"));
            }
        }
        public OTWorkingTime OTWorkingTimeItem
        {
            get { return oTWorkingTimeItem; }
            set
            {
                oTWorkingTimeItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTWorkingTimeItem"));
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
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
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
        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
            }
        }
        public OtItem TempOtItem
        {
            get { return tempOtItem; }
            set
            {
                tempOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempOtItem"));
            }
        }

        public ObservableCollection<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set
            {
                workLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItemList"));
            }
        }
        public string WorklogTotalTime
        {
            get { return worklogTotalTime; }
            set
            {
                worklogTotalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorklogTotalTime"));
            }
        }
        public List<UserShortDetail> UserImageList
        {
            get { return userImageList; }
            set { userImageList = value; }
        }

        List<SAMLogEntries> commentsDeletedList;
        public List<SAMLogEntries> CommentsDeletedList
        {
            get { return commentsDeletedList; }
            set
            {
                commentsDeletedList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsDeletedList"));
            }
        }

        List<SAMLogEntries> commentsCloneList;
        public List<SAMLogEntries> CommentsCloneList
        {
            get { return commentsCloneList; }
            set
            {
                commentsCloneList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsCloneList"));
            }
        }


        SAMLogEntries samComments;
        public SAMLogEntries SamComments
        {
            get { return samComments; }
            set
            {
                samComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SamComments"));
            }
        }

        public OTWorkingTime SelectedWorkLog
        {
            get { return selectedWorkLog; }
            set
            {
                selectedWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkLog"));
            }
        }
        public Int64 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOT"));
            }
        }
        #endregion //End Of Properties

        #region Icommands

        public ICommand CustomSummaryCommand { get; set; }
        public ICommand AddEditWorkOrderItemViewCancel { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddNewItemViewAcceptButtonCommand { get; set; }
        public ICommand AddCommentsCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand DeleteWorkLogCommand { get; set; }

        public ICommand AddWorkLogCommand { get; set; }
        public bool IsNew { get;  set; }
        public string WindowHeader { get;  set; }

        #endregion //End Of Icommand
        public EditItemViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                MaximizedElementPosition = MaximizedElementPosition.Right;

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                CancelButtonCommand = new DelegateCommand<object>(AddEditWorkOrderItemViewCancelAction);
                AddCommentsCommand = new DelegateCommand<object>(AddCommentsCommandAction);
                AddNewItemViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddNewItemsAccept);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                AddWorkLogCommand = new DelegateCommand<object>(AddWorkLogCommandAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddEditWorkOrderItemViewCancelAction(object obj)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEditWorkOrderItemViewCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);

                ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetOTAttachment(company, idOT).ToList());

                foreach (OTAttachment items in ListAttachment)
                {
                    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                    items.AttachmentImage = imageObj;
                }

                GeosApplication.Instance.Logger.Log("Method FillListAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Prism.Logging.Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddCommentsView addCommentsView = new AddCommentsView();
                AddCommentsViewModel addCommentsViewModel = new AddCommentsViewModel();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                //addCommentsViewModel.IsNew = true;
                //addCommentsViewModel.Init();

                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();
                if (addCommentsViewModel.SelectedComment != null)
                {
                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<SAMLogEntries>();

                    CommentsList.Add(addCommentsViewModel.SelectedComment as SAMLogEntries);
                    SelectedComment = addCommentsViewModel.SelectedComment;
                }

                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void DeleteCommentCommandAction(object parameter)
        {

            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            SAMLogEntries commentObject = (SAMLogEntries)parameter;


            bool result = false;
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        SAMLogEntries Comment = (SAMLogEntries)commentObject;
                        //result = SAMService.DeleteComment_V2340(Comment.IdComment,Site);
                        CommentsList.Remove(Comment);

                        if (DeleteCommentsList == null)
                            DeleteCommentsList = new ObservableCollection<SAMLogEntries>();

                        DeleteCommentsList.Add(Comment);
                        CommentsList = new ObservableCollection<SAMLogEntries>(CommentsList);
                        SamComments = Comment;
                        IsDeleted = true;
                    }

                   
                }
            }

            

            //NewItemComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void AddNewItemsAccept(object obj)
        {
            try
            {

                if (CommentsList != null && CommentsList.Count > 0)
                {
                    foreach (var item in CommentsList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                    {
                        OtItemsComment OtComment = new OtItemsComment();
                        OtComment.Comments = item.Comments;
                        OtComment.CommentDate = Convert.ToDateTime(item.Datetime);
                        OtComment.IdUser = Convert.ToInt32(item.IdUser);
                        OtComment.Idrevisionitem = Convert.ToInt32(TempOtItem.IdRevisionItem);
                        OtComment.IdEntryType = 1719;
                        CommentItem = SAMService.AddObservationCommentItem(OtComment, Site);
                    }
                }
                  
                    if (DeleteCommentsList != null && DeleteCommentsList.Count > 0)
                    {
                        foreach (var item in DeleteCommentsList)
                        {
                            bool result = SAMService.DeleteComment_V2340(item.IdComment, Site);
                        }
                     }

                if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                {
                    foreach (var item in WorkLogItemList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                    {
                        OTWorkingTime OTWorkingTime = new OTWorkingTime();
                        OTWorkingTime.StartTime = item.StartTime;
                        OTWorkingTime.EndTime = item.EndTime;
                        OTWorkingTime.TotalTime = item.TotalTime;
                        OTWorkingTime.IdOperator = item.UserShortDetail.IdUser;
                        //OTWorkingTime.IdOTItem = item.IdOTItem;
                        OTWorkingTime.IdOTItem = OtItems;
                       OTWorkingTimeItem = SAMService.AddOtWorkingTimeWorkLogItem(IdOT,OtItems, OTWorkingTime, Site );
                    }
                }

                IsSave = true;

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ItemsAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);



                RequestClose(null, null);


                GeosApplication.Instance.Logger.Log("Method AddNewActionsAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActionsAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        private void AddWorkLogCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddWorkLogView addWorkLogView = new AddWorkLogView();
                AddWorkLogViewModel addWorkLogViewModel = new AddWorkLogViewModel();
                EventHandler handle = delegate { addWorkLogView.Close(); };
                addWorkLogViewModel.RequestClose += handle;
                addWorkLogViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWorkLogHeader").ToString();
                //addCommentsViewModel.IsNew = true;
                //addCommentsViewModel.Init();

                addWorkLogView.DataContext = addWorkLogViewModel;
                addWorkLogView.ShowDialog();
                if (addWorkLogViewModel.SelectedWorkLog != null)
                {
                    if (WorkLogItemList == null)
                        WorkLogItemList = new ObservableCollection<OTWorkingTime> ();

                    WorkLogItemList.Add(addWorkLogViewModel.SelectedWorkLog as OTWorkingTime);
                    SelectedWorkLog = addWorkLogViewModel.SelectedWorkLog;
                }

                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        internal void Init()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

    
        public void Dispose()
        {
        }

        #region Public Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void EditInit(Ots Ots,Int32 IdArticle, Int32 IdRevisionItem)
        {
            try
            {
                OT = SAMService.GetSAMOrderItemsInformationByIdOt_V2340(Ots.IdOT, Ots.Site, Convert.ToUInt32(IdArticle));
                Site = Ots.Site;
                OtItem temOtItem = OT.OtItems.Where(w => w.IdRevisionItem == IdRevisionItem).FirstOrDefault();
                TempOtItem=OT.OtItems.Where(w => w.IdRevisionItem == IdRevisionItem).FirstOrDefault();
                OtItems = temOtItem.IdOTItem;
                IdOT = temOtItem.IdOT;
                Code = OT.Code;
                Customer = OT.Quotation.Site.Name;
                Comments = OT.Comments;
                Quantity = temOtItem.RevisionItem.Quantity;
                Item = temOtItem.RevisionItem.NumItem;
                Reference = temOtItem.RevisionItem.WarehouseProduct.Article.Reference;
                Description = temOtItem.RevisionItem.WarehouseProduct.Article.Description;
                ImagesList = new ObservableCollection<PCMArticleImage>(SAMService.GetPCMArticleImagesByIdPCMArticle
                    (temOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, temOtItem.RevisionItem.WarehouseProduct.Article.Reference));
                try
                {
                    ReferenceImage = ByteArrayToBitmapImage(ImagesList.FirstOrDefault().PCMArticleImageInBytes);
                }
                catch (Exception ex)
                {

                }


                
                //OtItem temOtItem = OT.OtItems.Where(w => w.RevisionItem.WarehouseProduct.Article.IdArticle == IdArticle).FirstOrDefault();
                if (Convert.ToBoolean(temOtItem.RevisionItem.WarehouseProduct.Article.IsOrientativeVisualAid))
                {
                    IsOrientativePicture = true;
                }
                else
                {
                    IsOrientativePicture = false;
                }
                try
                {
                    ArticleChangeLogList = new List<SAMLogEntries>();
                    if(temOtItem.LogEntriesByOT!=null)
                    ArticleChangeLogList.AddRange(temOtItem.LogEntriesByOT);
                    if (CommentsList == null)
                    {
                        CommentsList = new ObservableCollection<SAMLogEntries>();
                    }
                    if(temOtItem.Comments!=null)
                    CommentsList.AddRange(temOtItem.Comments);
                    ArticleComponents = new List<ArticleDecomposition>();
                    if(OT.ArticleDecompostionList!=null)
                    ArticleComponents.AddRange( OT.ArticleDecompostionList.Where(de => de.IdParent == IdArticle).ToList());
                    CommentsCloneList = CommentsList.Select(item => (SAMLogEntries)item.Clone()).ToList();
                    foreach (SAMLogEntries item in CommentsList)
                    {
                        item.People.OwnerImage = SetUserProfileImage();
                    }
                    //ArticleComponents.AddRange(OT.ArticleDecompostionList);

                    WorkLogItemList = new ObservableCollection<OTWorkingTime>();
                    UserImageList = new  List<UserShortDetail>();
                    WorkLogItemList.AddRange(SAMService.GetOTWorkingTimeDetails_V2350(OtItems, Ots.Site));
                    List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                    UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();
                    TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));
                    //[001] changed the date formate
                    int Hours = worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                    WorklogTotalTime = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitItem(OtItem tempOtItem, Ots ots, Company OtSite)
        {
            try
            {
                OT = SAMService.GetSAMOrderItemsInformationByIdOt_V2340(tempOtItem.IdOT, OtSite, Convert.ToUInt32(tempOtItem.RevisionItem.WarehouseProduct.IdArticle));
                Code = OT.Code;
                Customer = OT.Quotation.Site.Name;
                Comments= OT.Comments;
                Quantity = tempOtItem.RevisionItem.Quantity;
                Item = tempOtItem.RevisionItem.NumItem;
                Reference = tempOtItem.RevisionItem.WarehouseProduct.Article.Reference;
                Description = tempOtItem.RevisionItem.WarehouseProduct.Article.Description;
                ImagesList = new ObservableCollection<PCMArticleImage>(SAMService.GetPCMArticleImagesByIdPCMArticle
                    (tempOtItem.RevisionItem.WarehouseProduct.Article.IdArticle,tempOtItem.RevisionItem.WarehouseProduct.Article.Reference));

                
                if (OT.ArticleImageInBytes != null)
                {
                    if (ImagesList != null && ImagesList.Count > 0)
                    {
                        for (ulong pos = 1; pos <= ImagesList.Max(a => a.Position) + 1; pos++)
                        {
                            if (!(ImagesList.Any(a => a.Position == pos)))
                            {
                                PCMArticleImage Image = new PCMArticleImage();
                                Image.PCMArticleImageInBytes = OT.ArticleImageInBytes;
                                Image.IsWarehouseImage = 1;
                                Image.SavedFileName = Reference;
                                Image.OriginalFileName = Reference;
                                Image.Position = pos;
                                //Image.IsImageShareWithCustomer = OT.IsImageShareWithCustomer;
                                ImagesList.Add(Image);
                                //OldWarehouseImage = ImagesList.FirstOrDefault(a => a.IsWarehouseImage == 1);
                                //oldWarehouseposition = OldWarehouseImage.Position;

                                break;
                            }
                        }
                    }
                    else
                    {
                        PCMArticleImage Image = new PCMArticleImage();
                        Image.PCMArticleImageInBytes = OT.ArticleImageInBytes;
                        Image.IsWarehouseImage = 1;
                        Image.SavedFileName = Reference;
                        Image.OriginalFileName = Reference;
                        Image.Position = 1;
                        //Image.IsImageShareWithCustomer = OT.IsImageShareWithCustomer;
                        ImagesList.Add(Image);
                    }
                }
                try
                {
                    ReferenceImage = ByteArrayToBitmapImage(ImagesList.FirstOrDefault().PCMArticleImageInBytes);
                }
                catch (Exception ex)
                {

                }

                //ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).ToList());

                //if (OT.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                //{
                //    ReferenceImage = ByteArrayToBitmapImage(OT.ArticleImageInBytes);
                //}
                //else
                //{
                //    if (OT.ArticleImageInBytes == null && (ImagesList.Count == 0))
                //    {
                //       // ReferenceImage = ByteArrayToBitmapImage(selectedArticle.ArticleImageInBytes);
                //        //OldReferenceImage = ReferenceImage;
                //    }
                //    else
                //        ReferenceImage = ByteArrayToBitmapImage(ImagesList.FirstOrDefault(a => a.Position == 0).PCMArticleImageInBytes);
                //}

                OtItem temOtItem = OT.OtItems.Where(w => w.IdRevisionItem == tempOtItem.IdRevisionItem).FirstOrDefault();
                if (Convert.ToBoolean(temOtItem.RevisionItem.WarehouseProduct.Article.IsOrientativeVisualAid))
                {
                    IsOrientativePicture = true;
                }
                else
                {
                    IsOrientativePicture = false;
                }
                try
                {
                    ArticleChangeLogList = new List<SAMLogEntries>();
                    ArticleChangeLogList.AddRange(temOtItem.LogEntriesByOT);
                    CommentsList = new ObservableCollection<SAMLogEntries>();
                    CommentsList.AddRange(temOtItem.Comments);
                    ArticleComponents = new List<ArticleDecomposition>();
                    ArticleComponents = new List<ArticleDecomposition>();
                    ArticleComponents.AddRange(OT.ArticleDecompostionList.Where(de => de.IdParent == tempOtItem.RevisionItem.WarehouseProduct.IdArticle).ToList());
                    foreach (SAMLogEntries item in CommentsList)
                    {
                        item.People.OwnerImage = SetUserProfileImage();
                    }

                    SetUserProfileImage(CommentsList);
                    WorkLogItemList = new ObservableCollection<OTWorkingTime>();
                    UserImageList = new List<UserShortDetail>();

                    WorkLogItemList.AddRange(SAMService.GetOTWorkingTimeDetails_V2350(ot.IdOT, ot.Site));
                    List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                    UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();
                    TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));
                    //[001] changed the date formate
                    int Hours = worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                    WorklogTotalTime = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);
                    //ArticleComponents.AddRange(OT.ArticleDecompostionList);
                    //ArticleComponents.AddRange(OT.OtItems.Where(r => r.RevisionItem.WarehouseProduct.IdArticle != tempOtItem.RevisionItem.WarehouseProduct.IdArticle));
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInitItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource SetUserProfileImage(ObservableCollection<SAMLogEntries> CommentsList)
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
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/blueUnknownGender.png");
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

      
        #endregion // End Of Events 
    }
}