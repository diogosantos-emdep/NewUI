using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Emdep.Geos.Modules.Crm.Views;
using DevExpress.Xpf.Grid;
using System.Windows.Media;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddOrEditActionCommentsViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Task
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Decleration

        private Int64 idActionPlanItem;
        private Int64 idLogEntriesByActionItem;
        private string header;
        private Visibility textboxnormal;
        private bool isNormal = true;
        private string oldActionComment;
        private string newActionComment;
        private Visibility richtextboxrtf;
        private bool isRtf;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
        private object selectedComment;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private string commentText;
        private string visible;
        #endregion

        #region Properties

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

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }

        public string NewActionComment
        {
            get { return newActionComment; }
            set
            {
                newActionComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewActionComment"));
            }
        }

        public string OldActionComment
        {
            get { return oldActionComment; }
            set
            {
                oldActionComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldActionComment"));
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

        public string CommentText
        {
            get
            {
                return commentText;
            }

            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }

        public ObservableCollection<LogEntriesByActionItem> ActionPlanItemCommentsList
        {
            get { return actionPlanItemCommentsList; }
            set
            {
                actionPlanItemCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemCommentsList"));
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
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        #endregion

        #region ICommands
        public ICommand AddorEditActionsViewCancelButtonCommand { get; set; }
        public ICommand IsnormalPreviewMouseRightButtonDown { get; set; }
        public ICommand AddorEditActionsViewSaveCommentCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
            }
        }

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

        #endregion

        #region Constructor

        public AddOrEditActionCommentsViewModel()
        {
            AddorEditActionsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            IsnormalPreviewMouseRightButtonDown = new DelegateCommand<object>(IsnormalCommandAction);
            AddorEditActionsViewSaveCommentCommand = new DelegateCommand<object>(SaveCommentAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void AddInit()
        {
            Header = System.Windows.Application.Current.FindResource("ActionAddComment").ToString();
        }

        public void EditInit(LogEntriesByActionItem logentriesByActionItem)
        {
            Header = System.Windows.Application.Current.FindResource("ActionEditComment").ToString();
            idActionPlanItem = logentriesByActionItem.IdActionPlanItem;
            idLogEntriesByActionItem = logentriesByActionItem.IdLogEntryByActionItem;

            if (logentriesByActionItem.IsRtfText == true)
            {
                IsNormal = false;
                IsRtf = true;
                Textboxnormal = Visibility.Collapsed;
                Richtextboxrtf = Visibility.Visible;
            }
            else
            {
                Textboxnormal = Visibility.Visible;
                Richtextboxrtf = Visibility.Collapsed;
            }

            NewActionComment = logentriesByActionItem.Comment;
        }

        private void IsnormalCommandAction(object obj)
        {
            string convertedText = string.Empty;

            var a = IsNormal;
            var b = IsRtf;

            //if (IsNormal)
            //{
            //    var document = ((RichTextBox)obj).Document;
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
            //        range2.Save(ms, DataFormats.Text);
            //        ms.Seek(0, SeekOrigin.Begin);
            //        using (StreamReader sr = new StreamReader(ms))
            //        {
            //            convertedText = sr.ReadToEnd();
            //        }
            //    }
            //    NewActionComment = convertedText;
            //}



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

            if (IsRtf)
            {
                var document = ((RichTextBox)obj).Document;
                NewActionComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                //string convertedText = string.Empty;
                if (!string.IsNullOrEmpty(NewActionComment.Trim()))      /*|| IsRtf == true*/
                {
                    //if (IsRtf)
                    //{
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

                NewActionComment = convertedText;
            }

            if (IsNormal)
            {
                var document = ((RichTextBox)obj).Document;
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
                NewActionComment = convertedText;
            }
        }


        private void SaveCommentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert SaveCommentAction ...", category: Category.Info, priority: Priority.Low);

            if (IsRtf)
            {
                FlowDocument document = ((RichTextBox)obj).Document;
                NewActionComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                string convertedText = string.Empty;

                if (!string.IsNullOrEmpty(NewActionComment.Trim()))
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
                NewActionComment = convertedText;
            }

            if (OldActionComment != null && !string.IsNullOrEmpty(OldActionComment.Trim()) && OldActionComment.Equals(NewActionComment.Trim()))
            {
                //ShowCommentsFlyout = false;
                return;
            }


            //Add new comment
            //else if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString()) //Add comment.
            //{

            if (!string.IsNullOrEmpty(NewActionComment) && !string.IsNullOrEmpty(NewActionComment.Trim())) // Add Comment
            {
                if (IsRtf)
                {
                    LogEntriesByActionItem comment = new LogEntriesByActionItem()
                    {
                        IdLogEntryByActionItem = idLogEntriesByActionItem,
                        IdActionPlanItem = idActionPlanItem,
                        Creator = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName,
                        IdCreator = GeosApplication.Instance.ActiveUser.IdUser,
                        CreationDate = GeosApplication.Instance.ServerDateTime,
                        Comment = string.Copy(NewActionComment.Trim()),
                        IdLogEntryType = 257,
                        TransactionOperation = ModelBase.TransactionOperations.Add,
                        IsRtfText = true
                    };
                    comment.PeopleCreator = new People();
                    comment.PeopleCreator.OwnerImage = SetUserProfileImage();

                    if (comment != null)
                    {
                        ActionPlanItemCommentsList.Add(comment);
                    }

                    SelectedComment = comment;
                    RtfToPlaintext();
                    ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(ActionPlanItemCommentsList.OrderByDescending(c => c.CreationDate).ToList());
                }
                else if (IsNormal)
                {
                    LogEntriesByActionItem comment = new LogEntriesByActionItem()
                    {
                        IdLogEntryByActionItem = idLogEntriesByActionItem,
                        IdActionPlanItem = idActionPlanItem,
                        Creator = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName,
                        IdCreator = GeosApplication.Instance.ActiveUser.IdUser,
                        CreationDate = GeosApplication.Instance.ServerDateTime,
                        Comment = string.Copy(NewActionComment.Trim()),
                        IdLogEntryType = 257,
                        TransactionOperation = ModelBase.TransactionOperations.Add,
                        IsRtfText = false
                    };

                    comment.PeopleCreator = new People();
                    comment.PeopleCreator.OwnerImage = SetUserProfileImage();

                    if (comment != null)
                    {
                        ActionPlanItemCommentsList.Add(comment);
                    }

                    SelectedComment = comment;
                    ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(ActionPlanItemCommentsList.OrderByDescending(c => c.CreationDate).ToList());
                }

                OldActionComment = null;
                NewActionComment = null;
            }

            IsRtf = false;
            IsNormal = true;

            RequestClose(null, null);

            GeosApplication.Instance.Logger.Log("Method SaveCommentAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void RtfToPlaintext()
        {
            TextRange range = null;
            if (ActionPlanItemCommentsList.Count > 0)
            {
                if (ActionPlanItemCommentsList[0].IsRtfText)
                {
                    var rtb = new RichTextBox();
                    var doc = new FlowDocument();
                    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ActionPlanItemCommentsList[0].Comment.ToString()));
                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
                else
                {
                    CommentText = ActionPlanItemCommentsList[0].Comment.ToString();
                }
            }

            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                CommentText = range.Text;
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
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


        /// <summary>
        ///  This method is for to get image in bitmap by path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
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
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

    }
}

