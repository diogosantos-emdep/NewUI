using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
//using Emdep.Geos.Modules.Warehouse.Reports;
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

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    /// <summary>
    /// [000][skhade][18-12-2019][GEOS2-1760] Open Work Order details when double-click in Order
    /// </summary>
    public class WorkOrderItemDetailsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Task log
        //
        #endregion

        #region Services

        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion //End Of Services

        #region Declaration

        private Ots ot;
        private double dialogHeight;
        private double dialogWidth;
        private List<OTWorkingTime> workLogItemList;
        private string worklogTotalTime;
        private List<UserShortDetail> userImageList;
        private bool isBusy;
        private bool isAssignedUsers;
        private MaximizedElementPosition maximizedElementPosition;
        private OtItem selectedOtItem;
        
        private List<LogEntryBySite> tempListChangeLog = new List<LogEntryBySite>();
      
        private ObservableCollection<OTAssignedUser> userToAssingedUser;
        private ObservableCollection<OTAssignedUser> otAssignedUser;
        List<OTAssignedUser> cloneOTAssignedUser = new List<OTAssignedUser>();
        private ObservableCollection<OTAssignedUser> otAddDeleteAssignedUser;
        Ots objOT = new Ots();
        OTAssignedUser OTAssignedUser = new OTAssignedUser();
      //  private bool showAttachmentFlyout;
        private ObservableCollection<OTAttachment> listAttachment;

        private Company otSite;
        private OTWorkingTime selectedWorklogRow;
        private bool isRemoveWorkLog;
        private bool isEditWorkLog;

        #endregion // End Of Declaration

        #region Public Properties

        public Warehouses WarehouseDetails { get; set; }

        public Ots OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
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


        public List<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set
            {
                workLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItemList"));
            }
        }

        public List<UserShortDetail> UserImageList
        {
            get { return userImageList; }
            set { userImageList = value; }
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

        public virtual bool DialogResult { get; set; }

        public virtual string ResultFileName { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }        
        public bool IsAssignedUsers
        {
            get { return isAssignedUsers; }
            set
            {
                isAssignedUsers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAssignedUsers"));
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

        public OtItem SelectedOtItem
        {
            get { return selectedOtItem; }
            set
            {
                selectedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtItem"));
            }
        }
       
        public ObservableCollection<OTAssignedUser> OtAssignedUser
        {
            get { return otAssignedUser; }
            set
            {
                otAssignedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtAssignedUser"));
            }
        }


        public ObservableCollection<OTAssignedUser> UserToAssingedUser
        {
            get { return userToAssingedUser; }
            set { userToAssingedUser = value; OnPropertyChanged(new PropertyChangedEventArgs("UserToAssingedUser")); }
        }

        public ObservableCollection<OTAssignedUser> OtAddDeleteAssignedUser
        {
            get { return otAddDeleteAssignedUser; }
            set
            {
                otAddDeleteAssignedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtAddDeleteAssignedUser"));
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

        public Company OtSite
        {
            get { return otSite; }
            set
            {
                otSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtSite"));
            }
        }
        public OTWorkingTime SelectedWorklogRow
        {
            get { return selectedWorklogRow; }
            set
            {
                selectedWorklogRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorklogRow"));
            }
        }

        public bool IsRemoveWorkLog
        {
            get { return isRemoveWorkLog; }
            set
            {
                isRemoveWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemoveWorkLog"));

            }
        }
        public bool IsEditWorkLog
        {
            get { return isEditWorkLog; }
            set
            {
                isEditWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditWorkLog"));

            }
        }

        #endregion

        #region ICommands

        public ICommand CommandWorkOrderCancel { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand ScanMaterialsCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand PrintWorkOrderItemCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand CurrentItemChangedCommand { get; set; }
        public ICommand GetWorkOrderUserContactCommand { get; set; }
        public ICommand AssignedWorkOrderItemCancelCommand { get; set; }

        public ICommand HyperlinkForEmail { get; set; }
        public ICommand EditWorkOrderViewAcceptButtonCommand { get; set; }

        //Attachment
        public ICommand DownloadFileCommand { get; set; }
        public ICommand CustomSortColumnDataCommand { get; set; }
        public ICommand ExportWorkLogCommand { get; set; }
        public ICommand DeleteWorkLogCommand { get; set; }
        public ICommand EditWorkLogDoubleClickCommand { get; set; }



        #endregion // End Of ICommands

        #region Events


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event EventHandler RequestClose;

        #endregion // End Of Events

        #region Constructor

        public WorkOrderItemDetailsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                //GeosApplication.CurrentCultureInfo
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;

                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandWorkOrderCancel = new DelegateCommand<object>(CommandWorkOrderCancelAction);
                CurrentItemChangedCommand = new DelegateCommand<object>(CurrentItemChangedCommandAction);
                GetWorkOrderUserContactCommand = new DelegateCommand<object>(AssignedUserWorkOrderUserContactCheckedAction);
                AssignedWorkOrderItemCancelCommand = new DelegateCommand<object>(AssignedUserWorkOrderItemCancelCommandAction);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                EditWorkOrderViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveWorkOrderDetails));

                DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
                CustomSortColumnDataCommand = new DelegateCommand<object>(CustomSortColumnDataAction);

                
                ExportWorkLogCommand = new RelayCommand(new Action<object>(ExportWorkLogCommandAction));

                DeleteWorkLogCommand = new DelegateCommand<object>(DeletWorklogRowCommandAction);
                EditWorkLogDoubleClickCommand = new DelegateCommand<object>(EditWorklog);


                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor WorkOrderItemDetailsViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        

        #endregion // End Of Constructor


        #region Command Actions

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CommandWorkOrderCancelAction(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
        }

        private void CurrentItemChangedCommandAction(object obj)
        {
            try
            {
                if (SelectedOtItem != null &&
                    SelectedOtItem.RevisionItem != null &&
                    SelectedOtItem.RevisionItem.WarehouseProduct != null
                    && SelectedOtItem.RevisionItem.WarehouseProduct.Article != null)
                {
                    if (SAMCommon.Instance.Articles.Any(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle))
                    {
                        Article article = SAMCommon.Instance.Articles.FirstOrDefault(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;
                    }
                    else
                    {
                        Article article = SAMService.GetArticleDetails(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);

                        if (article.ArticleImageInBytes == null)
                        {
                            article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SAM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                        }
                        else if (article.ArticleImageInBytes != null)
                        {
                            article.ArticleImage = SAMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                        }

                        SAMCommon.Instance.Articles.Add(article);
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;

                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeletWorklogRowCommandAction(object obj)
        {
            try
            {
                bool result = false;
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkLogDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                        {
                            OTWorkingTime otworktime = (OTWorkingTime)obj;
                            result = SAMService.DeleteWorkLog(OtSite, otworktime.IdOTWorkingTime);
                            WorkLogItemList.Remove(otworktime);
                            WorkLogItemList = new List<OTWorkingTime>(WorkLogItemList);

                        }
                        if (result)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method DeletWorklogRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeletWorklogRowCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [000][20200727]][srowlo][GEOS2-2374] Add a new permission to Edit work log entries
        /// </summary>
        /// <param name="obj"></param>
        private void EditWorklog(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method EditWorklog()...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            OTWorkingTime worklog = (OTWorkingTime)detailView.DataControl.CurrentItem;
            SelectedWorklogRow = worklog;

            if (worklog != null)
            {
                EditWorkLogView editWorklLogView = new EditWorkLogView();
                EditWorkLogViewModel editWorkLogViewModel = new EditWorkLogViewModel();
                EventHandler handle = delegate { editWorklLogView.Close(); };
                editWorkLogViewModel.RequestClose += handle;
                editWorklLogView.DataContext = editWorkLogViewModel;
               // ExistWorkLogItemList = new List<OTWorkingTime>(WorkLogItemList.ToList());
                editWorkLogViewModel.EditInit(WorkLogItemList, worklog, OtAssignedUser, OtSite);

                var ownerInfo = (obj as FrameworkElement);
                editWorklLogView.Owner = Window.GetWindow(ownerInfo);
                editWorklLogView.ShowDialog();
                if (editWorkLogViewModel.IsSave == true)
                {
                    SelectedWorklogRow.UserShortDetail = editWorkLogViewModel.EditWorkLogItem.UserShortDetail;
                    SelectedWorklogRow.TotalTime = editWorkLogViewModel.EditWorkLogItem.TotalTime;
                    // SelectedWorklogRow.TotalTime = new TimeSpan(SelectedWorklogRow.TotalTime.Days, SelectedWorklogRow.TotalTime.Hours, SelectedWorklogRow.TotalTime.Minutes, 00);

                    if (SelectedWorklogRow.TotalTime.Days > 0)
                    {
                        int Hours = SelectedWorklogRow.TotalTime.Days * 24 + SelectedWorklogRow.TotalTime.Hours;
                        SelectedWorklogRow.Hours = string.Format("{0}H {1}M", Hours, SelectedWorklogRow.TotalTime.Minutes);
                    }
                    else
                    {
                        SelectedWorklogRow.Hours = string.Format("{0}H {1}M", SelectedWorklogRow.TotalTime.Hours, SelectedWorklogRow.TotalTime.Minutes);
                    }
                    if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                    {
                        WorkLogItemList = new List<OTWorkingTime>(WorkLogItemList);
                    }
                }
                //else
                //{
                //    SelectedWorklogRow =  worklog;
                //}
            }
            GeosApplication.Instance.Logger.Log("Method EditWorklog()....executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method to restrict dateEdit Popup.
        /// </summary>
        /// <param name="obj"></param>
        private void OpenPopupDateEditAction(OpenPopupEventArgs obj)
        {
            obj.Cancel = true;
            obj.Handled = true;
        }

        #endregion

        #region Methods
        //[001][16-06-2020][smazhar][GEOS2-3254]  Wrong time count
        public void Init(Ots ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                MaximizedElementPosition = MaximizedElementPosition.Right;

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
                {
                    IsRemoveWorkLog = true;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 48))
                {
                    IsEditWorkLog = true;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                {
                    IsAssignedUsers = true;
                }
                //SetMaximizedElementPosition();

                //var temp =  GeosApplication.Instance.CurrentCultureInfo;
                OtSite = ot.Site;
                OT = SAMService.GetWorkOrderByIdOt(ot.IdOT, ot.Site);
                SelectedOtItem = OT.OtItems.FirstOrDefault();

                WorkLogItemList = new List<OTWorkingTime>();
                UserImageList = new List<UserShortDetail>();

                WorkLogItemList = SAMService.GetOTWorkingTimeDetails(ot.IdOT, ot.Site);
                List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();
                TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));
                //[001] changed the date formate
                int Hours= worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                WorklogTotalTime = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);



                FillAssignedUsers(ot.Site,ot.IdSite);
                FillOtAssignUsersList(ot.Site, ot.IdOT);
                FillListAttachment(ot.Site, ot.IdOT);
                objOT.IdOT = ot.IdOT;
                objOT.Site = ot.Site;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to fill Attachments Section
        /// </summary>
        /// <param name="company"></param>
        /// <param name="idOT"></param>
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);
                
                ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetOTAttachment(company, idOT).ToList());

                foreach(OTAttachment items in ListAttachment)
                {
                    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                    items.AttachmentImage = imageObj;
                }
               
                GeosApplication.Instance.Logger.Log("Method FillListAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
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

        /// <summary>
        /// Method to Set maximized element position.
        /// </summary>
        public void SetMaximizedElementPosition()
        {
            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["Appearance"].ToString(), true);
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                }
            }
        }

        /// <summary>
        /// Method for Fill Ot Assign Users List.
        /// </summary>
        private void FillOtAssignUsersList(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOtAssignUsersList ...", category: Category.Info, priority: Priority.Low);

                OtAssignedUser = new ObservableCollection<OTAssignedUser>(SAMService.GetOTAssignedUsers(company, idOT).ToList());

                if (OtAssignedUser.Count > 0)
                {
                    foreach (OTAssignedUser item in OtAssignedUser)
                    {
                        OTAssignedUser OTAssignedUser = UserToAssingedUser.FirstOrDefault(x => x.UserShortDetail.IdUser == item.IdUser);
                        UserToAssingedUser.Remove(OTAssignedUser);
                    }

                }

                ///Copy selected record in clone object.
                if (OtAssignedUser != null)
                    cloneOTAssignedUser = OtAssignedUser.Select(x => (OTAssignedUser)x.Clone()).ToList();

                for (int i = 0; i < OtAssignedUser.Count; i++)
                {

                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(OtAssignedUser[i].UserShortDetail.Login);

                        if (UserProfileImageByte != null)
                        {
                            OtAssignedUser[i].UserShortDetail.UserImage = SAMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                            }
                            else
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillOtAssignUsersList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for Fill Assigned Users list depend on Work Order. 
        /// [001][17-06-2020][smazhar][GEOS2-2376]  Filter the users by the selected plant
        /// </summary>
        private void FillAssignedUsers(Company company,Int32 idSite)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAssignedUsers ...", category: Category.Info, priority: Priority.Low);
              
                
                //[001] change the service function
                UserToAssingedUser = new ObservableCollection<OTAssignedUser>(SAMService.GetUsersToAssignedOT_V2044(company, idSite));

                GeosApplication.Instance.Logger.Log("Method FillAssignedUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAssignedUsers() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAssignedUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// This Method for Add Work Order User Contact Checked Action
        /// </summary>
        /// <param name="e"></param>
        public void AssignedUserWorkOrderUserContactCheckedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderUserContactCheckedAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is OTAssignedUser)
                {

                    OTAssignedUser = obj as OTAssignedUser;  // (OTAssignedUser)obj;
                    
                    OTAssignedUser.IdOT = objOT.IdOT;
                    OTAssignedUser.IdStage = 4;
                    OTAssignedUser.IdUser = OTAssignedUser.UserShortDetail.IdUser;
                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(OTAssignedUser.UserShortDetail.Login);

                        if (UserProfileImageByte == null)
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (OTAssignedUser.UserShortDetail.IdUserGender == 1)
                                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                                else if (OTAssignedUser.UserShortDetail.IdUserGender == 2)
                                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                            }
                            else
                            {
                                if (OTAssignedUser.UserShortDetail.IdUserGender == 1)
                                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (OTAssignedUser.UserShortDetail.IdUserGender == 2)
                                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                            }
                        }
                        else
                        {
                            OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                        }

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }

                    OtAssignedUser.Add(OTAssignedUser);

                    UserToAssingedUser.Remove(OTAssignedUser);

                    GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderUserContactCheckedAction() executed successfully", category: Category.Info, priority: Priority.Low);

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// This Method for Assigned Work Order Item Cancel Command Action.
        /// </summary>
        /// <param name="obj"></param>
        private void AssignedUserWorkOrderItemCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderItemCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is OTAssignedUser)
                {

                    OTAssignedUser = obj as OTAssignedUser;  // (OTAssignedUser)obj;
                   
                    OtAssignedUser.Remove(OTAssignedUser);
                    UserToAssingedUser.Add(OTAssignedUser);
                    UserToAssingedUser.OrderBy(i => i.UserShortDetail.UserName);
                }

                GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method for Save Work Order Details.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveWorkOrderDetails(object obj)
        {
            try
            {
                OtAddDeleteAssignedUser = new ObservableCollection<OTAssignedUser>();
                foreach (OTAssignedUser otAssignedUser in OtAssignedUser)
                {
                    if (!cloneOTAssignedUser.Any(x => x.IdOT == otAssignedUser.IdOT && x.IdUser == otAssignedUser.IdUser && x.IdStage == otAssignedUser.IdStage))
                    {
                        OTAssignedUser OTAssignedUser = (OTAssignedUser)otAssignedUser.Clone();
                        OTAssignedUser.TransactionOperation = ModelBase.TransactionOperations.Add;
                        OTAssignedUser.UserShortDetail.UserImage = null;
                        OtAddDeleteAssignedUser.Add(OTAssignedUser);
                    }
                }
                foreach (OTAssignedUser otAssignedUser in cloneOTAssignedUser)
                {
                    if (!OtAssignedUser.Any(x => x.IdOT == otAssignedUser.IdOT && x.IdUser == otAssignedUser.IdUser && x.IdStage == otAssignedUser.IdStage))
                    {
                        OTAssignedUser OTAssignedUser = (OTAssignedUser)otAssignedUser.Clone();
                        OTAssignedUser.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        OTAssignedUser.UserShortDetail.UserImage = null;
                        OtAddDeleteAssignedUser.Add(OTAssignedUser);
                    }
                }

                SAMService.UpdateOTAssignedUser(objOT.Site, OtAddDeleteAssignedUser.ToList());

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveWorkOrderDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method for Send Mail to Person
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;
        }

        /// <summary>
        /// save Downloaded file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));

                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }

        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }

        /// <summary>
        /// Method to download the file
        /// </summary>
        /// <param name="obj"></param>
        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentsFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    OTAttachment attachmentObject = (OTAttachment)obj;

                    SaveFileDialogService.DefaultExt = attachmentObject.FileExtension;
                    SaveFileDialogService.DefaultFileName = attachmentObject.FileName;
                    SaveFileDialogService.Filter = "All Files|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
                        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }
                        
                        Byte[] byteObj = SAMService.GetOTAttachmentInBytes(attachmentObject.FileName, attachmentObject.QuotationYear, attachmentObject.QuotationCode);

                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, byteObj);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       

        /// <summary>
        /// method to sort FileSize Column
        /// </summary>
        /// <param name="obj"></param>
        private void CustomSortColumnDataAction(object obj)
        {
            CustomColumnSortEventArgs e = obj as CustomColumnSortEventArgs;

            if(e.Column.FieldName == "FileSize")
            {
                string FirstFile = Convert.ToString(e.Value1);
                string SecondFile = Convert.ToString(e.Value2);

                int FirstFileSpaceIndex = FirstFile.IndexOf(" ");
                string FirstFileSizeType = FirstFile.Substring(FirstFileSpaceIndex+ 1, 1);

                int SecondFileSpaceIndex = SecondFile.IndexOf(" ");              
                string SecondFileSizeType = SecondFile.Substring(SecondFileSpaceIndex + 1, 1);

               if (FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("K"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=K)").Value); 
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=K)").Value); 

                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }

                if (FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("M"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=M)").Value);
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=M)").Value); 

                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }

               if (FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("B"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=B)").Value);
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=B)").Value);

                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }

                if ((FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("K")) )
                {
                    e.Result = -1;
                }

                if (FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("B"))
                {
                    e.Result = 1;
                }

                if ((FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("M"))  )
                {
                    e.Result = -1;
                }

                if((FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("B")))
                {
                    e.Result = 1;
                }

                if ((FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("M"))  )
                {
                    e.Result = -1;
                }

                if (FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("K"))
                {
                    e.Result = 1;
                }

                    e.Handled = true;
            }
        }

        private void ExportWorkLogCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()...", category: Category.Info, priority: Priority.Low);
                
                    Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                    saveFile.DefaultExt = "xlsx";
                    saveFile.FileName = "Work Log List" + " - " + OT.Code;
                    saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    saveFile.FilterIndex = 1;
                    saveFile.Title = "Save Excel Report";
                    DialogResult = (Boolean)saveFile.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
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

                        ResultFileName = (saveFile.FileName);
                        TableView activityTableView = ((TableView)obj);
                        activityTableView.ShowTotalSummary = false;
                        activityTableView.ShowFixedTotalSummary = false;
                        XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                        options.CustomizeCell += Options_CustomizeCell;
                        activityTableView.ExportToXlsx(ResultFileName, options);
                        IsBusy = false;
                        activityTableView.ShowFixedTotalSummary = true;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        System.Diagnostics.Process.Start(ResultFileName);
                    }
                
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        #endregion // End Of Methods

    }
}
