using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.LayoutControl;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.Hrm;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.Windows.Controls;
using System.Windows.Documents;


namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EditTravelExpenseCommentViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region TaskComments
        //[GEOS2-4016][23.03.2023][rdixit]
        #endregion   

        #region Command
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptActionCommand { get; set; }

        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // Events

        #region Declaration
        bool isUpdated;
        string windowHeader;        
        string comment;
        //comment add //chitra.girigosavi[geos2-4824][26/12/2023]
        private string oldItemComment;
        private string newItemComment;
        private Int64 idLogEntriesByItem;
        private Int64 idPlanItem;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private LogEntriesByTravelExpense selectedComment;
        private bool isRtf;
        ObservableCollection<LogEntriesByTravelExpense> commentsList;
        //end comment
        #endregion

        #region Properties
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }
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
        public bool IsUpdated
        {
            get
            {
                return isUpdated;
            }

            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }

        //chitra.girigosavi[geos2-4824][26/12/2023]
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
        public ObservableCollection<LogEntriesByTravelExpense> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }
        public LogEntriesByTravelExpense SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        #endregion

        #region Constructor
        public EditTravelExpenseCommentViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor EditTravelExpenseCommentViewModel()...", category: Category.Info, priority: Priority.Low);
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            AcceptActionCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
            GeosApplication.Instance.Logger.Log("Constructor EditTravelExpenseCommentViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        #endregion

        #region Methods 
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private void CancelButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
            IsUpdated = false;
            RequestClose(null, null);
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Comment"));                
                if (error != null)
                {
                    return;
                }

                //#region
                if (OldItemComment != null && !string.IsNullOrEmpty(OldItemComment.Trim()) && OldItemComment.Equals(Comment.Trim()))
                {
                    //ShowCommentsFlyout = false;
                    return;
                }
                //Add new comment
                if (!string.IsNullOrEmpty(Comment) && !string.IsNullOrEmpty(Comment.Trim())) // Add Comment
                {

                    LogEntriesByTravelExpense comment = new LogEntriesByTravelExpense()

                    {
                        IdEmployeeExpenseReportChangeLog = idLogEntriesByItem,
                        UserName = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Copy(Comment.Trim()),

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
                        CommentsList = new ObservableCollection<LogEntriesByTravelExpense>();
                    }
                    if (comment != null)
                    {
                        CommentsList.Add(comment);
                    }
                    // comment.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add;
                    SelectedComment = comment;
                    CommentsList = new ObservableCollection<LogEntriesByTravelExpense>(CommentsList.OrderByDescending(c => c.Datetime).ToList());
                }
                //#endregion
                IsUpdated = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                        //if (user.IdUserGender == 1)
                        //    UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        ////UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        //else if (user.IdUserGender == 2)
                        //    UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        //else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        //if (user.IdUserGender == 1)
                        //    UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        //else if (user.IdUserGender == 2)
                        //    UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        //else if (user.IdUserGender == null)
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
        #endregion

        #region Validation 
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
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => Comment)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

          
                string ExpComment = BindableBase.GetPropertyName(() => Comment);
                
                if (columnName == ExpComment)
                {
                    return HRMTravelExpenseValidation.GetErrorMessage(ExpComment, Comment, null);
                }
                //}
                return null;
            }
        }
        #endregion
    }
}
