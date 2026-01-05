using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.ComponentModel;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Crm.Views;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.IO;
using System.Windows.Documents;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using System.Drawing;
using DevExpress.XtraPrinting;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Tasks
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private ObservableCollection<Company> companies;
        private List<LookupValue> BusinessProductList { get; set; }
        private Visibility isCalendarVisible = Visibility.Collapsed;
        private DateTime closeDate;
        bool isBusy;
        private object selectedObject;
        private ObservableCollection<ActionPlanItem> actionPlanItemList;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemCommentsList;
        #endregion

        #region  public Properties
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Companies"));
            }
        }

        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }
            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
        }
        public DateTime CloseDate
        {
            get
            {
                return closeDate;
            }

            set
            {
                closeDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CloseDate"));
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

        public object SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }

        public ObservableCollection<ActionPlanItem> ActionPlanItemList
        {
            get { return actionPlanItemList; }
            set
            {
                actionPlanItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemList"));
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

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

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
        #endregion


        #region Public Commands
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand CommandNewActionsClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandGetThreeMonthsDate { get; set; }
        public ICommand CommandGetSixMonthsDate { get; set; }
        public ICommand CommandGetOneYearDate { get; set; }
        public ICommand CommandOpenCalendar { get; set; }
        public ICommand RefreshActionsViewCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DeleteActionsCommand { get; set; }
        public ICommand AddCommentWindowCommand { get; set; }
        public ICommand EditCommentWindowCommand { get; set; }
        public ICommand DeleteCommentWindowCommand { get; set; }

        #endregion

        #region Constructor

        public ActionsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ActionsViewModel ...", category: Category.Info, priority: Priority.Low);

                PlantOwnerPopupClosedCommand = new DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SalesOwnerPopupClosedCommand = new DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);

                CommandNewActionsClick = new DelegateCommand<object>(AddActionsViewWindowShow);

                CommandGetThreeMonthsDate = new DelegateCommand<object>(GetThreeMonthsDate);

                CommandGetSixMonthsDate = new RelayCommand(new Action<object>(GetSixMonthsDate));
                CommandGetOneYearDate = new RelayCommand(new Action<object>(GetOneYearDate));
                CommandOpenCalendar = new RelayCommand(new Action<object>(OpenCalendar));
                CommandGridDoubleClick = new DelegateCommand<object>(EditActionsViewWindowShow);
                RefreshActionsViewCommand = new RelayCommand(new Action<object>(RefreshActionsDetails));
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportActionsGridButtonCommandAction));
                DeleteActionsCommand = new DelegateCommand<object>(DeleteActionsRowCommandAction);

                AddCommentWindowCommand = new DelegateCommand<object>(AddCommentWindowShow);
                EditCommentWindowCommand = new DelegateCommand<object>(EditCommentWindowShow);
                DeleteCommentWindowCommand = new DelegateCommand<object>(DeleteCommentWindowShow);

                CloseDate = DateTime.Today.AddMonths(-1);
                FillCmbSalesOwner();
                GeosApplication.Instance.Logger.Log("Constructor ActionsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActionsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCommentWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddCommentWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
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
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
                AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
                EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
                addOrEditActionCommentsViewModel.RequestClose += handle;

                addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
                addOrEditActionCommentsViewModel.AddInit();
                addOrEditActionCommentsView.ShowDialog();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                if (addOrEditActionCommentsViewModel.SelectedComment != null)
                {
                    ActionPlanItem api = obj as ActionPlanItem;

                    if (api == null)
                    {
                        LogEntriesByActionItem lebai = obj as LogEntriesByActionItem;
                        api = ActionPlanItemList.FirstOrDefault(x => x.IdActionPlanItem == lebai.IdActionPlanItem);
                    }

                    if (api.LogEntriesByActionItems == null)
                        api.LogEntriesByActionItems = new List<LogEntriesByActionItem>();

                    LogEntriesByActionItem lenai = addOrEditActionCommentsViewModel.SelectedComment as LogEntriesByActionItem;

                    if (lenai != null)
                    {
                        if (lenai.PeopleCreator != null)
                            lenai.PeopleCreator.OwnerImage = null;

                        lenai.IdActionPlanItem = api.IdActionPlanItem;

                        List<LogEntriesByActionItem> logEntries = new List<LogEntriesByActionItem>();
                        logEntries.Add(lenai);

                        //Added log.
                        LogEntriesByActionItem log = new LogEntriesByActionItem();
                        log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                        log.CreationDate = GeosApplication.Instance.ServerDateTime;
                        log.IdLogEntryType = 258;

                        if (lenai.IsRtfText)
                        {
                            log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentAdded").ToString(), RtfToPlaintext(lenai));
                        }
                        else
                        {
                            log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentAdded").ToString(), lenai.Comment);
                        }

                        logEntries.Add(log);

                        logEntries = CrmStartUp.AddUpdateDeleteLogEntriesByActionItem(lenai.IdActionPlanItem, logEntries);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionCommentAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        api.LogEntriesByActionItems.Add(logEntries.FirstOrDefault());
                    }

                    api.LogEntriesByActionItems = new List<LogEntriesByActionItem>(api.LogEntriesByActionItems);
                    SelectedObject = api.LogEntriesByActionItems.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method AddCommentWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCommentWindowShow() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        public void EditCommentWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditCommentWindowShow...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            TableView detailView = (TableView)obj;

            LogEntriesByActionItem comment = detailView.DataControl.CurrentItem as LogEntriesByActionItem;

            if (comment.IdCreator != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show(Application.Current.Resources["NotAllowedDeleteActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
            AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
            EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
            addOrEditActionCommentsViewModel.RequestClose += handle;
            addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
            addOrEditActionCommentsViewModel.EditInit(comment);
            addOrEditActionCommentsView.ShowDialog();

            if (addOrEditActionCommentsViewModel.SelectedComment != null)
            {
                LogEntriesByActionItem selectedComment = (LogEntriesByActionItem)addOrEditActionCommentsViewModel.SelectedComment;
                selectedComment.TransactionOperation = ModelBase.TransactionOperations.Update;

                if (selectedComment.PeopleCreator != null)
                    selectedComment.PeopleCreator.OwnerImage = null;

                ActionPlanItem api = ActionPlanItemList.FirstOrDefault(x => x.IdActionPlanItem == selectedComment.IdActionPlanItem);

                if (api != null)
                {
                    List<LogEntriesByActionItem> comments = new List<LogEntriesByActionItem>();
                    comments.Add(selectedComment);

                    //Updated log.
                    LogEntriesByActionItem log = new LogEntriesByActionItem();
                    log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    log.CreationDate = GeosApplication.Instance.ServerDateTime;
                    log.IdLogEntryType = 258;

                    if (selectedComment.IsRtfText)
                    {
                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentUpdated").ToString(), RtfToPlaintext(comment), RtfToPlaintext(selectedComment));
                    }
                    else
                    {
                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentUpdated").ToString(), comment.Comment, selectedComment.Comment);
                    }

                    comments.Add(log);

                    CrmStartUp.AddUpdateDeleteLogEntriesByActionItem(selectedComment.IdActionPlanItem, comments);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionCommentUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    int index = api.LogEntriesByActionItems.FindIndex(x => x.IdLogEntryByActionItem == selectedComment.IdLogEntryByActionItem);
                    api.LogEntriesByActionItems[index].Comment = selectedComment.Comment;
                    api.LogEntriesByActionItems = new List<LogEntriesByActionItem>(api.LogEntriesByActionItems);
                    SelectedObject = api.LogEntriesByActionItems[index];
                }
            }

            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void DeleteCommentWindowShow(object obj)
        {
            LogEntriesByActionItem comment = obj as LogEntriesByActionItem;

            if (comment.IdCreator != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show(Application.Current.Resources["NotAllowedDeleteActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            MessageBoxResult messageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteActionPlanItemComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes && comment != null)
            {
                ActionPlanItem api = ActionPlanItemList.FirstOrDefault(x => x.IdActionPlanItem == comment.IdActionPlanItem);

                if (api != null)
                {
                    comment.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    List<LogEntriesByActionItem> comments = new List<LogEntriesByActionItem>();
                    comments.Add(comment);

                    //Removed log.
                    LogEntriesByActionItem log = new LogEntriesByActionItem();
                    log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    log.CreationDate = GeosApplication.Instance.ServerDateTime;
                    log.IdLogEntryType = 258;

                    if (comment.IsRtfText)
                    {
                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentRemoved").ToString(), RtfToPlaintext(comment));
                    }
                    else
                    {
                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentRemoved").ToString(), comment.Comment);
                    }

                    comments.Add(log);

                    CrmStartUp.AddUpdateDeleteLogEntriesByActionItem(comment.IdActionPlanItem, comments);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionCommentDeletedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    api.LogEntriesByActionItems.Remove(comment);
                    api.LogEntriesByActionItems = new List<LogEntriesByActionItem>(api.LogEntriesByActionItems);
                }
            }
        }

        private void DeleteActionsRowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisableActionsRowCommandAction() ...", category: Category.Info, priority: Priority.Low);

                ActionPlanItem ObjAction = (ActionPlanItem)obj;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteActionPlanItem"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsBusy = true;
                    if (ActionPlanItemList != null && ActionPlanItemList.Count > 0)
                    {
                        ObjAction.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        ObjAction.IsDeleted = 1;
                        CrmStartUp.DeleteActionPlanItem(ObjAction);
                        ActionPlanItemList.Remove(ObjAction);
                    }

                    IsBusy = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionDeletedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableActionsRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableActionsRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableActionsRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods

        /// <summary>
        /// Method for fill Account Age.
        /// </summary>
        private void FillActionsAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsAge ...", category: Category.Info, priority: Priority.Low);
                int i = 0;
                if (Companies != null)
                {
                    foreach (Company age in Companies)
                    {
                        Companies[i].Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - Companies[i].CreatedIn.Value.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - Companies[i].CreatedIn.Value.Year)) / 12, 1);
                        i++;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsAge() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsAge() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsAge() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCmbSalesOwner()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                FillActionsListByUser();
            }

            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillActionsListByPlant();
            }
            else
            {
                FillActionsList();
            }

            //  actionPlansList = new ObservableCollection<ActionPlan>(CrmStartUp.GetSalesActivityRegister);
            //BusinessProductList = CrmStartUp.GetLookupValues(7).ToList();
        }

        /// <summary>
        /// Method for fill account details from database by plant.
        /// </summary>
        public void FillActionsListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister(null, plantOwnersIds, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number));
                }
                else
                {
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
                    // ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                }

                GeosApplication.Instance.Logger.Log("Method FillActionsListByPlant executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }


        /// <summary>
        /// Methdo for fill account details from database.
        /// </summary>
        private void FillActionsListByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsListByUser ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    PreviouslySelectedSalesOwners = salesOwnersIds;

                    //ActionPlanItemList = new ObservableCollection<ActionPlan>(CrmStartUp.GetSalesActivityRegister(salesOwnersIds,null , GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, Convert.ToDateTime(CloseDate)));
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister(salesOwnersIds, null, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number));
                    // ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                }
                else
                {

                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
                    // ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                }
                GeosApplication.Instance.Logger.Log("Method FillActionsListByUser executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsListByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsListByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }


        /// <summary>
        ///  Methdo for fill account details from database.
        /// </summary>
        private void FillActionsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsList ...", category: Category.Info, priority: Priority.Low);

                ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister(null, null, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number));

                // ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                GeosApplication.Instance.Logger.Log("Method FillActionsList executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for action after close plant owner popup.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FillActionsListByPlant();
            }
            else
            {
                ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
                //Companies = new ObservableCollection<Company>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        ///  This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

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
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FillActionsListByUser();
            }
            else
            {
                //Companies = new ObservableCollection<Company>();
                ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        private void AddActionsViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddActionsViewWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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

                AddNewActionsViewModel addNewActionsViewModel = new AddNewActionsViewModel();
                AddNewActionsView addNewActionsView = new AddNewActionsView();
                EventHandler handle = delegate { addNewActionsView.Close(); };
                addNewActionsViewModel.RequestClose += handle;
                addNewActionsView.DataContext = addNewActionsViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                addNewActionsView.ShowDialog();
                if (addNewActionsViewModel.IsSave == true)
                {
                    if (addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems != null)
                    {
                        addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems = addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems.Where(x => x.IdLogEntryType == 257).ToList();
                    }

                    addNewActionsViewModel.ActionPlanItem.CreationDate = GeosApplication.Instance.ServerDateTime;
                    ActionPlanItemList.Add(addNewActionsViewModel.ActionPlanItem);
                    SelectedObject = addNewActionsViewModel.ActionPlanItem;
                    ((TableView)obj).BestFitColumns();
                }

                GeosApplication.Instance.Logger.Log("Method AddActionsViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddActionsViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetThreeMonthsDate(object obj)
        {
            try
            {
                CloseDate = DateTime.Today.AddMonths(-3);
                IsCalendarVisible = Visibility.Collapsed;

            }
            catch (Exception ex)
            {

            }
        }

        private void GetSixMonthsDate(object obj)
        {
            try
            {
                CloseDate = DateTime.Today.AddMonths(-6);
                IsCalendarVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

        private void GetOneYearDate(object obj)
        {
            try
            {
                CloseDate = DateTime.Today.AddYears(-1);
                IsCalendarVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

        private void OpenCalendar(object obj)
        {
            try
            {
                IsCalendarVisible = Visibility.Visible;
                DateEditCalendar dateEditCalender = new DateEditCalendar();
                //CloseDate = dateEditCalender
                // IsCalendarVisible = Visibility.Collapsed;

            }
            catch (Exception ex)
            {

            }
        }

        private void EditActionsViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditActionsViewWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (detailView.DataControl.CurrentItem is ActionPlanItem)
                {
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
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ActionPlanItem actionPlanItem = (ActionPlanItem)detailView.DataControl.CurrentItem;

                    EditActionsViewModel editActionsViewModel = new EditActionsViewModel();
                    EditActionsView editActionsView = new EditActionsView();
                    editActionsViewModel.Init(actionPlanItem);

                    EventHandler handle = delegate { editActionsView.Close(); };
                    editActionsViewModel.RequestClose += handle;
                    editActionsView.DataContext = editActionsViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    editActionsView.ShowDialog();

                    if (editActionsViewModel.IsSave)
                    {
                        actionPlanItem.IdActionPlan = editActionsViewModel.ObjActionPlanItem.IdActionPlan;
                        actionPlanItem.IdCreator = editActionsViewModel.ObjActionPlanItem.IdCreator;
                        actionPlanItem.IdGroup = editActionsViewModel.ObjActionPlanItem.IdGroup;
                        actionPlanItem.IdSite = editActionsViewModel.ObjActionPlanItem.IdSite;
                        actionPlanItem.IdScope = editActionsViewModel.ObjActionPlanItem.IdScope;
                        actionPlanItem.IdStatus = editActionsViewModel.ObjActionPlanItem.IdStatus;
                        actionPlanItem.CloseDate = editActionsViewModel.ObjActionPlanItem.CloseDate;
                        actionPlanItem.IdReporter = editActionsViewModel.ObjActionPlanItem.IdReporter;
                        actionPlanItem.IdAssignee = editActionsViewModel.ObjActionPlanItem.IdAssignee;
                        actionPlanItem.CurrentDueDate = editActionsViewModel.ObjActionPlanItem.CurrentDueDate;
                        actionPlanItem.Title = editActionsViewModel.ObjActionPlanItem.Title;
                        actionPlanItem.ModificationDate = GeosApplication.Instance.ServerDateTime;

                        if (editActionsViewModel.ObjActionPlanItem.Assignee != null)
                        {
                            actionPlanItem.Assignee = editActionsViewModel.ObjActionPlanItem.Assignee;
                        }

                        actionPlanItem.Group = editActionsViewModel.ObjActionPlanItem.Group;
                        actionPlanItem.Plant = editActionsViewModel.ObjActionPlanItem.Plant;
                        actionPlanItem.Scope = editActionsViewModel.ObjActionPlanItem.Scope;
                        actionPlanItem.Status = editActionsViewModel.ObjActionPlanItem.Status;
                        actionPlanItem.Reporter = editActionsViewModel.ObjActionPlanItem.Reporter;

                        actionPlanItem.LogEntriesByActionItems = new List<LogEntriesByActionItem>(editActionsViewModel.ActionPlanItemCommentsList);
                    }

                    detailView.BestFitColumns();

                    // actionPlanItem.IdGroup = editActionsViewModel.SelectedIndexCompanyGroup;
                    GeosApplication.Instance.Logger.Log("Method EditActionsViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else if (detailView.DataControl.CurrentItem is LogEntriesByActionItem)
                {
                    EditCommentWindowShow(obj);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditActionsViewWindowShow()" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {

        }

        private void RefreshActionsDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshActionDetails...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                FillCmbSalesOwner();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method RefreshActionDetails() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }

        }

        public void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ActionsViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ActionsViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private List<ActionPlanItem> tempActionPlanItems = null;

        private void ExportActionsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportActionsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Actions";
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
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = (saveFile.FileName);

                    TableView actionsTableView = ((TableView)obj);

                    GridControl grid = new GridControl();
                    List<ActionPlanItem> List = new List<ActionPlanItem>(ActionPlanItemList.OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number));

                    foreach (var item in List)
                    {
                        string comments = "";
                        DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);

                        if (item.LogEntriesByActionItems != null)
                        {
                            //List<LogEntriesByActionItem> commentList = item.LogEntriesByActionItems.Where(a => a.IdActionPlanItem == item.IdActionPlanItem).ToList();
                            foreach (LogEntriesByActionItem CommentRecord in item.LogEntriesByActionItems)
                            {
                                if (CommentRecord.IsRtfText == true)
                                {
                                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                                    var doc = new FlowDocument();
                                    TextRange range = null;
                                    if (CommentRecord.Comment != null)
                                    {
                                        if (CommentRecord.Comment.ToString() != string.Empty)
                                        {
                                            doc = new FlowDocument();
                                            doc.FontFamily = GeosApplication.Instance.FontFamilyAsPerTheme;
                                            MemoryStream stream = new MemoryStream(encoding.GetBytes(System.Convert.ToString(CommentRecord.Comment)));
                                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                            range.Load(stream, DataFormats.Rtf);
                                        }
                                    }
                                    comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + range.Text.ToString() + Environment.NewLine.ToString();
                                }
                                else
                                {
                                    comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Comment + Environment.NewLine.ToString();
                                }
                            }
                        }

                        item.SubjectOrComment = "" + item.Title + "" + Environment.NewLine.ToString() + comments;
                        item.DueDateWeekly = "WK" + format.Calendar.GetWeekOfYear(item.CurrentDueDate, format.CalendarWeekRule, format.FirstDayOfWeek).ToString();
                    }

                    grid.ItemsSource = List;

                    GridColumn Item = new GridColumn();
                    Item.Header = "Code";
                    Item.FieldName = "ActionPlan.Code";
                    Item.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(Item);

                    GridColumn Group = new GridColumn();
                    Group.Header = "Group";
                    Group.FieldName = "Group";
                    Group.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(Group);

                    GridColumn Plant = new GridColumn();
                    Plant.Header = "Plant";
                    Plant.FieldName = "Plant";
                    Plant.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(Plant);

                    GridColumn SubjectOrComment = new GridColumn();
                    SubjectOrComment.Header = "Subject / Comments";
                    SubjectOrComment.FieldName = "SubjectOrComment";
                    SubjectOrComment.HorizontalHeaderContentAlignment = HorizontalAlignment.Left;
                    SubjectOrComment.Width = 470;

                    //FrameworkElementFactory tb = new FrameworkElementFactory(typeof(TextBlock));
                    //Binding b2 = new Binding("SubjectOrComment");
                    //tb.SetBinding(TextBlock.TextProperty, b2);
                    ////tb.TextBlock.FontWeightProperty = FontWeights.Bold;

                    //FrameworkElementFactory tb1 = new FrameworkElementFactory(typeof(TextBlock));
                    //Binding b1 = new Binding();
                    //b1.Path = new PropertyPath("RowData.Row.SubjectOrComment");
                    //tb1.SetBinding(TextBlock.TextProperty, b1);

                    //FrameworkElementFactory sb = new FrameworkElementFactory(typeof(StackPanel));
                    //sb.AppendChild(tb);
                    //sb.AppendChild(tb1);

                    //DataTemplate dt = new DataTemplate { VisualTree = sb };
                    //SubjectOrComment.CellTemplate = dt;

                    grid.Columns.Add(SubjectOrComment);

                    GridColumn ReviewDate = new GridColumn();
                    ReviewDate.Header = "Review Date";
                    ReviewDate.FieldName = "ModificationDate";
                    ReviewDate.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(ReviewDate);

                    GridColumn Responsible = new GridColumn();
                    Responsible.Header = "Responsible";
                    Responsible.FieldName = "Assignee";
                    Responsible.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(Responsible);

                    GridColumn DueDate = new GridColumn();
                    DueDate.Header = "Due Date";
                    DueDate.FieldName = "DueDateWeekly";
                    DueDate.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(DueDate);

                    GridColumn Status = new GridColumn();
                    Status.Header = "Status";
                    Status.FieldName = "Status.Value";
                    Status.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                    grid.Columns.Add(Status);


                    tempActionPlanItems = new List<ActionPlanItem>();

                    foreach (ActionPlanItem item in ((GridControl)actionsTableView.DataControl).VisibleItems)
                    {
                        tempActionPlanItems.Add(item);
                    }


                    //for (int i = 0; i < ((GridControl)actionsTableView.DataControl).VisibleRowCount; i++)
                    //{
                    //    int rowHandle = ((GridControl)actionsTableView.DataControl).GetRowHandleByListIndex(i);
                    //    ActionPlanItem p = ((GridControl)actionsTableView.DataControl).GetRow(rowHandle) as ActionPlanItem;
                    //    tempActionPlanItems.Add(p);
                    //}

                    //(GridControl)actionsTableView.datac

                    DevExpress.Export.ExportSettings.DefaultExportType = ExportType.DataAware;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeSheetHeader += options_CustomizeSheetHeaderNew;

                    options.CustomizeCell += args =>
                    {
                        if (args.AreaType == SheetAreaType.DataArea && args.ColumnFieldName == "SubjectOrComment")
                        {
                            args.Formatting.Alignment = new XlCellAlignment() { WrapText = true, HorizontalAlignment = XlHorizontalAlignment.Left, VerticalAlignment = XlVerticalAlignment.Top };
                            args.Handled = true;
                        }
                        if (args.AreaType == SheetAreaType.Header && args.ColumnFieldName == "SubjectOrComment")
                        {
                            args.Formatting.BackColor = XlColor.FromArgb(99, 160, 207);
                            args.Formatting.Alignment = new XlCellAlignment { HorizontalAlignment = XlHorizontalAlignment.Left, VerticalAlignment = XlVerticalAlignment.Center };
                            args.Formatting.Font = new XlCellFont { Bold = true, Size = 13 };
                            args.Handled = true;
                        }
                    };

                    options.CustomizeCell += args =>
                    {
                        if (args.AreaType == SheetAreaType.DataArea && (args.ColumnFieldName == "ActionPlan.Code" || args.ColumnFieldName == "Group" || args.ColumnFieldName == "Plant"
                        || args.ColumnFieldName == "ModificationDate" || args.ColumnFieldName == "Assignee" || args.ColumnFieldName == "DueDateWeekly" || args.ColumnFieldName == "Status.Value"))
                        {
                            if (args.ColumnFieldName == "ModificationDate")
                            {
                                args.Formatting.NumberFormat = "dd-mmm-yyyy";
                            }

                            args.Formatting.Alignment = new XlCellAlignment() { HorizontalAlignment = XlHorizontalAlignment.Center, VerticalAlignment = XlVerticalAlignment.Top };

                            if (args.ColumnFieldName == "Status.Value")
                            {
                                LookupValue value = ActionPlanItemList.Where(a => a.Status.Value == args.Value.ToString()).FirstOrDefault().Status;
                                if (args.Value.ToString() == value.Value)
                                {
                                    if (value.HtmlColor != "" && value.HtmlColor != null)
                                    {
                                        System.Drawing.Color color = ColorTranslator.FromHtml(value.HtmlColor);
                                        byte r = Convert.ToByte(color.R);
                                        byte g = Convert.ToByte(color.G);
                                        byte b = Convert.ToByte(color.B);
                                        args.Formatting.BackColor = XlColor.FromArgb(r, g, b);
                                    }
                                }
                            }

                            args.Handled = true;
                        }
                        if (args.AreaType == SheetAreaType.Header && (args.ColumnFieldName == "ActionPlan.Code" || args.ColumnFieldName == "Group" || args.ColumnFieldName == "Plant"
                        || args.ColumnFieldName == "ModificationDate" || args.ColumnFieldName == "Assignee" || args.ColumnFieldName == "DueDateWeekly" || args.ColumnFieldName == "Status.Value"))
                        {
                            args.Formatting.BackColor = XlColor.FromArgb(99, 160, 207);
                            args.Formatting.Alignment = new XlCellAlignment { HorizontalAlignment = XlHorizontalAlignment.Center, VerticalAlignment = XlVerticalAlignment.Center };
                            args.Formatting.Font = new XlCellFont { Bold = true, Size = 13 };
                            args.Handled = true;
                        }
                    };


                    grid.FilterCriteria = actionsTableView.Grid.FilterCriteria;
                    ((TableView)grid.View).ExportToXlsx(ResultFileName, options);

                    IsBusy = false;

                    actionsTableView.ShowTotalSummary = true;
                    actionsTableView.ShowFixedTotalSummary = true;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportActionsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportActionsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        void options_CustomizeSheetHeaderNew(DevExpress.Export.ContextEventArgs e)
        {
            CellObject row = new CellObject();
            CellObject row1 = new CellObject();
            CellObject row2 = new CellObject();
            CellObject row3 = new CellObject();
            CellObject row4 = new CellObject();
            CellObject row5 = new CellObject();
            CellObject row6 = new CellObject();
            CellObject row7 = new CellObject();


            //row5.Value = "";
            //row6.Value = "";
            XlFormattingObject rowFormatting = new XlFormattingObject();
            rowFormatting.Font = new XlCellFont { Bold = true, Size = 20, Color = XlColor.FromArgb(56, 77, 218) };
            rowFormatting.Border = new XlBorder { Outline = false };
            rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Center };
            row3.Formatting = rowFormatting;

            row3.Value = "";
            row5.Value = "Date :";
            row6.Value = GeosApplication.Instance.ServerDateTime;
            XlFormattingObject rowFormattingRow5_1 = new XlFormattingObject();
            XlFormattingObject rowFormattingRow6_1 = new XlFormattingObject();
            rowFormattingRow5_1.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow6_1.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Left, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow6_1.NumberFormat = "dd-mmm-yyyy";
            rowFormattingRow5_1.Border = new XlBorder { Outline = false };
            rowFormattingRow6_1.Border = new XlBorder { Outline = false };
            rowFormattingRow6_1.Font = new XlCellFont { Bold = true };
            row5.Formatting = rowFormattingRow5_1;
            row6.Formatting = rowFormattingRow6_1;
            e.ExportContext.AddRow(new[] { row, row1, row2, row3, row4, row5, row6, row7 });

            row3.Value = "Action Plan / Meeting Minutes";
            row5.Value = "Total Action Items :";
            row6.Value = tempActionPlanItems.Count;
            row7.Value = "% No. AP Closed";
            XlFormattingObject rowFormattingRow5_2 = new XlFormattingObject();
            XlFormattingObject rowFormattingRow6_2 = new XlFormattingObject();
            XlFormattingObject rowFormattingRow7_2 = new XlFormattingObject();
            rowFormattingRow5_2.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow6_2.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow7_2.Alignment = new DevExpress.Export.Xl.XlCellAlignment { WrapText = true, HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow5_2.Border = new XlBorder { Outline = false };
            rowFormattingRow6_2.Border = new XlBorder { Outline = false };
            rowFormattingRow6_2.Font = new XlCellFont { Bold = true, Size = 13 };
            rowFormattingRow7_2.Border = new XlBorder { Outline = true };
            rowFormattingRow7_2.BackColor = XlColor.FromTheme(XlThemeColor.Accent1, 0.4);
            rowFormattingRow7_2.Font = new XlCellFont { Bold = true };
            row5.Formatting = rowFormattingRow5_2;
            row6.Formatting = rowFormattingRow6_2;
            row7.Formatting = rowFormattingRow7_2;
            e.ExportContext.AddRow(new[] { row, row1, row2, row3, row4, row5, row6, row7 });

            row3.Value = "";
            row5.Value = "Total Open Items :";

            row6.Value = tempActionPlanItems.Where(a => a.IdStatus != 266).ToList().Count;
            int done = tempActionPlanItems.Where(a => a.IdStatus == 266).ToList().Count;
            if (tempActionPlanItems.Count > 0)
            {
                row7.Value = (done * 100) / tempActionPlanItems.Count;
            }
            else
            {
                row7.Value = "0";
            }

            XlFormattingObject rowFormattingRow5_3 = new XlFormattingObject();
            XlFormattingObject rowFormattingRow6_3 = new XlFormattingObject();
            XlFormattingObject rowFormattingRow7_3 = new XlFormattingObject();
            rowFormattingRow5_3.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow6_3.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow7_3.Alignment = new DevExpress.Export.Xl.XlCellAlignment { WrapText = true, HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            rowFormattingRow5_3.Border = new XlBorder { Outline = false };
            rowFormattingRow6_3.Border = new XlBorder { Outline = false };
            rowFormattingRow6_3.Font = new XlCellFont { Bold = true, Size = 13 };
            rowFormattingRow7_3.Border = new XlBorder { Outline = true };
            rowFormattingRow7_3.BackColor = XlColor.FromArgb(250, 90, 90);
            rowFormattingRow7_3.Font = new XlCellFont { Bold = true, Size = 13 };
            row5.Formatting = rowFormattingRow5_3;
            row6.Formatting = rowFormattingRow6_3;
            row7.Formatting = rowFormattingRow7_3;
            e.ExportContext.AddRow(new[] { row, row1, row2, row3, row4, row5, row6, row7 });

            var imageToHeaderRange = new XlCellRange(new XlCellPosition(0, 0), new XlCellPosition(1, 1));
            e.ExportContext.MergeCells(imageToHeaderRange);
            //e.ExportContext.InsertImage(Crm.Properties.Resources.Emdep_logo, imageToHeaderRange);

        }

        public string RtfToPlaintext(LogEntriesByActionItem comment)
        {
            if (comment.IsRtfText)
            {
                TextRange range = null;

                var doc = new FlowDocument();
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(comment.Comment));
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Load(stream, DataFormats.Rtf);

                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                    return range.Text.Trim();
            }
            else
            {
                return comment.Comment;
            }

            return null;
        }


        #endregion
    }
}
