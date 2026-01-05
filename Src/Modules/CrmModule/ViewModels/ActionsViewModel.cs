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
using DevExpress.Data.Filtering;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net;
using System.Runtime.Serialization.Json;
using Emdep.Geos.UI.Helper;
using System.Data;
using DevExpress.Spreadsheet;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Tasks
        #endregion

        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private ObservableCollection<Company> companies;
        private List<LookupValue> BusinessProductList { get; set; }
        private Visibility isCalendarVisible = Visibility.Collapsed;
        private DateTime closeDate;
        bool isBusy;
        private bool isAppointment;
        private bool addActionButtonVsible = true;
        private object selectedObject;
        private ObservableCollection<ActionPlanItem> actionPlanItemList;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemCommentsList;
        private ActionPlanItem _actionPlanItem;
        private ObservableCollection<People> responsibleList;
        private int selectedResponsibleListIndex;
        private Company selectedPlant;
        TableView view;
        private List<LogEntriesByActionItem> changeLogsEntry;
        private List<ActionPlanItem> tempActionPlanItems = null;
        private bool isActionSave;
        private bool isAllSave;
        private string excelFilePath = string.Empty;
        private readonly object item;
        private bool focusUserControl;

        #endregion

        #region  public Properties

        public string ExcelFilePath
        {
            get { return excelFilePath; }
            set { excelFilePath = value; }
        }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }

        public List<LookupValue> StatusList { get; set; }

        public Company SelectedPlant
        {
            get { return selectedPlant; }
            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        public ObservableCollection<People> ResponsibleList
        {
            get { return responsibleList; }

            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));

            }
        }

        public int SelectedResponsibleListIndex
        {
            get { return selectedResponsibleListIndex; }
            set
            {
                selectedResponsibleListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsibleListIndex"));
            }
        }
        public ActionPlanItem _ActionPlanItem
        {
            get
            {
                return _actionPlanItem;
            }
            set
            {
                _actionPlanItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("_ActionPlanItem"));
            }
        }

        public bool IsAppointment
        {
            get
            {
                return isAppointment;
            }
            set
            {
                isAppointment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAppointment"));
            }
        }

        public bool AddActionButtonVsible
        {
            get
            {
                return addActionButtonVsible;
            }
            set
            {
                addActionButtonVsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddActionButtonVsible"));
            }
        }
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
        //public List<LogEntriesByActionItem> ChangedLogsEntries { get; set; }

        public List<LogEntriesByActionItem> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }

        public bool IsActionSave
        {
            get { return isActionSave; }
            set
            {
                isActionSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionSave"));
            }
        }

        public bool IsAllSave
        {
            get { return isAllSave; }
            set
            {
                isAllSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllSave"));
            }
        }

        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
            }
        }

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
        public ICommand ButtonCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand FilterActionCancelCommand { get; private set; }

        public ICommand FilterActionAcceptCommand { get; private set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand CommandNewActionsClick { get; set; }

        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandGetThreeMonthsDate { get; set; }
        public ICommand CommandGetSixMonthsDate { get; set; }
        public ICommand FilterButtonCommand { get; set; }
        public ICommand CommandGetOneYearDate { get; set; }
        public ICommand CommandOpenCalendar { get; set; }
        public ICommand RefreshActionsViewCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DeleteActionsCommand { get; set; }
        public ICommand AddCommentWindowCommand { get; set; }
        public ICommand EditCommentWindowCommand { get; set; }
        public ICommand DeleteCommentWindowCommand { get; set; }

        public ICommand UpdateMultipleRowsActionsGridCommand { get; set; }

        public ICommand CustomUnboundColumnDataCommand1 { get; set; }

        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CanSelectCellCommand { get; set; }

        
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
                FilterButtonCommand = new DelegateCommand<object>(ShowAppointments);
                CommandGetSixMonthsDate = new RelayCommand(new Action<object>(GetSixMonthsDate));
                CommandGetOneYearDate = new RelayCommand(new Action<object>(GetOneYearDate));
                CommandOpenCalendar = new RelayCommand(new Action<object>(OpenCalendar));
                CommandGridDoubleClick = new DelegateCommand<object>(EditActionsViewWindowShow);
                RefreshActionsViewCommand = new RelayCommand(new Action<object>(RefreshActionsDetails));
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportActionsGridButtonCommandAction));
                DeleteActionsCommand = new DelegateCommand<object>(DeleteActionsRowCommandAction);
                UpdateMultipleRowsActionsGridCommand = new DelegateCommand<object>(UpdateMultipleRowsActionsGridCommandAction);
                CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                //CustomUnboundColumnDataCommand1 = new DelegateCommand<object>(CustomUnboundColumnDataCommandAction);

                AddCommentWindowCommand = new DelegateCommand<object>(AddCommentWindowShow);
                //ButtonCommand = new RelayCommand(o => AddCommentWindowShowEvent(object sender,e));
                //ButtonCommand = new RelayCommand(new Action<object>(AddCommentWindowShowEvent));
                //ButtonCommand = new DelegateCommand<object>(AddCommentWindowShowEvent);
                EditCommentWindowCommand = new DelegateCommand<object>(EditCommentWindowShow);
                DeleteCommentWindowCommand = new DelegateCommand<object>(DeleteCommentWindowShow);
                //FilterActivityCancelCommand = new DelegateCommand<object>(FilterActivityCancel);
                FilterActionCancelCommand = new RelayCommand(new Action<object>(FilterActionCancel));
                FilterActionAcceptCommand = new RelayCommand(new Action<object>(FilterActionOk));
                //CanSelectCellCommand = new DelegateCommand<object>(CanSelectCellCommandAction);
                CloseDate = DateTime.Today.AddMonths(-1);
                FillStatusList();
                FillCmbSalesOwner();
                //FillResponsibleList();
                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Constructor ActionsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActionsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CanSelectCellCommandAction(object obj)
        {
           
        }

        #endregion


        #region Methods

        /// <summary>
        /// Method for fill Account Age.
        /// </summary>
        /// 


        private void AddCommentWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddCommentWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                //TableView detailView = (TableView)obj;
                //GridControl gridControl = (detailView).Grid;
                if (obj is ActionPlanItem)
                {
                    ActionPlanItem actionPlanItem = (ActionPlanItem)obj;// detailView.DataControl.CurrentItem;
                    if (actionPlanItem.IdSalesActivityType == 273)
                    {
                        CustomMessageBox.Show(Application.Current.Resources["NotAllowedAddActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                
                AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
                AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
                EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
                addOrEditActionCommentsViewModel.RequestClose += handle;

                addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
                addOrEditActionCommentsViewModel.AddInit();
                addOrEditActionCommentsView.ShowDialog();

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
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddCommentWindowShow() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddCommentWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method AddCommentWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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


            ActionPlanItem apiItem = ActionPlanItemList.FirstOrDefault(x => x.IdActionPlanItem == comment.IdActionPlanItem);
            if (apiItem.LogEntriesByActionItems.OrderBy(apl => apl.IdLogEntryByActionItem).FirstOrDefault().IdLogEntryByActionItem != comment.IdLogEntryByActionItem)
            {
                if (comment.IdCreator != GeosApplication.Instance.ActiveUser.IdUser)
                {
                    CustomMessageBox.Show(Application.Current.Resources["NotAllowedUpdateActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["NotAllowedUpdateFirstActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void DeleteCommentWindowShow(object obj)
        {
            LogEntriesByActionItem comment = obj as LogEntriesByActionItem;

            ActionPlanItem apiItem = ActionPlanItemList.FirstOrDefault(x => x.IdActionPlanItem == comment.IdActionPlanItem);
            if (apiItem.LogEntriesByActionItems.OrderBy(apl => apl.IdLogEntryByActionItem).FirstOrDefault().IdLogEntryByActionItem != comment.IdLogEntryByActionItem)
            {
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
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["NotAllowedDeleteFirstActionPlanItemComment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
        }

        private void DeleteActionsRowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisableActionsRowCommandAction() ...", category: Category.Info, priority: Priority.Low);

                ActionPlanItem ObjAction = (ActionPlanItem)obj;
                if (ObjAction.IdSalesActivityType != 273)
                {
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
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["NotAllowedDeleteAppointment"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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

        private void FilterActionCancel(object obj)
        {
            ((DropDownButton)obj).IsPopupOpen = false;
        }
        private void FilterActionOk(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterActionOk ...", category: Category.Info, priority: Priority.Low);

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
                ((DropDownButton)obj).IsPopupOpen = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FilterActionOk() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FilterActionOk() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Filter()
        {
            if (IsAppointment)
            {
                ObservableCollection<ActionPlanItem> TempActionList = new ObservableCollection<ActionPlanItem>();
                ActivityParams objActivityParams = new ActivityParams();
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    objActivityParams.idPlant = plantOwnersIds;
                }

                objActivityParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                objActivityParams.idOwner = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                objActivityParams.idPermission = GeosApplication.Instance.IdUserPermission;

                objActivityParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                objActivityParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                TempActionList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetAppointmentActivities(objActivityParams).ToList());
                //TempActionList = GetAppointment(objActivityParams);
                foreach (ActionPlanItem Action in TempActionList)
                {
                    //AddActionButtonVsible = false;
                    ActionPlanItemList.Add(Action);
                }
            }
        }
        private void FillCmbSalesOwner()
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
			if(ActionPlanItemList!=null)
            ActionPlanItemList.ToList().ForEach(i => i.StatusList = new List<LookupValue>(StatusList));//[rdixit][GEOS2-4667][18.07.2023]
            //  actionPlansList = new ObservableCollection<ActionPlan>(CrmStartUp.GetSalesActivityRegister);
            //BusinessProductList = CrmStartUp.GetLookupValues(7).ToList();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// Method for fill account details from database by plant.
        /// [001][cpatil][GEOS2-6664][05-02-2025]
        /// </summary>
        public void FillActionsListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsListByPlant ...", category: Category.Info, priority: Priority.Low);

                view = ActionPlanMultipleCellEditHelper.Viewtableview;
                if (ActionPlanMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsActionsGridCommandAction(ActionPlanMultipleCellEditHelper.Viewtableview);
                    }
                    ActionPlanMultipleCellEditHelper.IsValueChanged = false;
                }

                ActionPlanMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ActionPlanMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    //ActionPlanItemList = null;
                    //[001]Changed service method
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister_V2650(null, plantOwnersIds, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number));  	//[nsatpute][19.06.2025][GEOS2-7032]
                    //FillResponsibleList();
                }
                else
                {
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
                    // ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                }
                Filter();
                FillResponsibleList();

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
        /// [001][cpatil][GEOS2-6664][05-02-2025]
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
                    //[001]Changed service method
                    //ActionPlanItemList = new ObservableCollection<ActionPlan>(CrmStartUp.GetSalesActivityRegister(salesOwnersIds,null , GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, Convert.ToDateTime(CloseDate)));
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister_V2650(salesOwnersIds, null, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number)); 	//[nsatpute][19.06.2025][GEOS2-7032]
                }
                else
                {
                    ActionPlanItemList = new ObservableCollection<ActionPlanItem>();
                }
                Filter();
                FillResponsibleList();


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

        private ObservableCollection<ActionPlanItem> GetAppointment(ActivityParams objActivityParams)
        {

            ObservableCollection<ActionPlanItem> Appointments = new ObservableCollection<ActionPlanItem>();
            string ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();

            string ServiceUrl = null;

            if (ServicePath.Contains("http://") || ServicePath.Contains("https://"))
                ServiceUrl = ServicePath + "/CrmRestService.svc" + "/GetAppointmentActivities";
            else
                ServiceUrl = "http://" + ServicePath + "/CrmRestService.svc" + "/GetAppointmentActivities";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
           
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ActivityParams));
            json.WriteObject(request.GetRequestStream(), objActivityParams);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer jsonResp = new DataContractJsonSerializer(typeof(ObservableCollection<ActionPlanItem>));

            Appointments = (ObservableCollection<ActionPlanItem>)jsonResp.ReadObject(stream);

            stream.Flush();
            stream.Close();
            return Appointments;
        }

        /// <summary>
        ///  Methdo for fill account details from database.
        /// [001][cpatil][GEOS2-6664][05-02-2025]
        /// </summary>
        private void FillActionsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActionsList ...", category: Category.Info, priority: Priority.Low);
                //[001]Changed service method
                ActionPlanItemList = new ObservableCollection<ActionPlanItem>(CrmStartUp.GetSalesActivityRegister_V2650(null, null, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission, CloseDate).OrderBy(x => x.IdActionPlan).ThenBy(y => y.Number)); 	//[nsatpute][19.06.2025][GEOS2-7032]
                Filter();
                FillResponsibleList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillActionsList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

            if (ActionPlanMultipleCellEditHelper.IsValueChanged)
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    UpdateMultipleRowsActionsGridCommandAction(ActionPlanMultipleCellEditHelper.Viewtableview);
                }
                ActionPlanMultipleCellEditHelper.IsValueChanged = false;
            }

            ActionPlanMultipleCellEditHelper.IsValueChanged = false;

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

        private void ShowAppointments(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ShowAppointments...", category: Category.Info, priority: Priority.Low);
            try
            {


                GeosApplication.Instance.Logger.Log("Method ShowAppointments() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAppointments() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddActionsViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddActionsViewWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                TableView detailView = (TableView)obj;
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
                var ownerInfo = (detailView as FrameworkElement);
                addNewActionsView.Owner = Window.GetWindow(ownerInfo);
                addNewActionsView.ShowDialog();

                List<Tag> ActionTagList = new List<Tag>();
                if (addNewActionsViewModel.IsSave == true)
                {
                    if (addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems != null)
                    {
                        addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems = addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems.Where(x => x.IdLogEntryType == 257).ToList();

                        addNewActionsViewModel.ActionPlanItem.LogEntriesByActionItems.ForEach(i => i.IdActionPlanItem = addNewActionsViewModel.ActionPlanItem.IdActionPlanItem);
                    }

                    addNewActionsViewModel.ActionPlanItem.CreationDate = GeosApplication.Instance.ServerDateTime;
                    addNewActionsViewModel.ActionPlanItem.Creator = GeosApplication.Instance.ActiveUser.FullName;
                    #region [rgadhave][GEOS2-3801][17.01.2024]
                    foreach (Tag actionTag in addNewActionsViewModel.SelectedTagList)
                    {
                        ActionTagList.Add(actionTag);
                    }
                    addNewActionsViewModel.ActionPlanItem.Name = String.Join("\n", ActionTagList.Select(a => a.Name));
                    #endregion
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
                    ActionPlanItem actionPlanItem = (ActionPlanItem)detailView.DataControl.CurrentItem;
                    if (actionPlanItem.IdSalesActivityType != 273)
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


                        EditActionsViewModel editActionsViewModel = new EditActionsViewModel();
                        EditActionsView editActionsView = new EditActionsView();
                        editActionsViewModel.IsEdit = true;//[Sudhir.jangra]
                        editActionsViewModel.Init(actionPlanItem);

                        EventHandler handle = delegate { editActionsView.Close(); };
                        editActionsViewModel.RequestClose += handle;
                        editActionsView.DataContext = editActionsViewModel;

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        {
                            DXSplashScreen.Close();
                        }
                        var ownerInfo = (detailView as FrameworkElement);
                        editActionsView.Owner = Window.GetWindow(ownerInfo);
                        editActionsView.ShowDialog();

                        List<Tag> ActionTagList = new List<Tag>();  //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                        if (editActionsViewModel.IsSave)
                        {
                            actionPlanItem.IdActionPlan = editActionsViewModel.ObjActionPlanItem.IdActionPlan;
                            actionPlanItem.IdCreator = editActionsViewModel.ObjActionPlanItem.IdCreator;
                            actionPlanItem.IdGroup = editActionsViewModel.ObjActionPlanItem.IdGroup;
                            //actionPlanItem.IdTag = editActionsViewModel.ObjActionPlanItem.IdTag;
                            actionPlanItem.IdSite = editActionsViewModel.ObjActionPlanItem.IdSite;
                            actionPlanItem.IdScope = editActionsViewModel.ObjActionPlanItem.IdScope;
                            actionPlanItem.IdSalesActivityType = editActionsViewModel.ObjActionPlanItem.IdSalesActivityType;
                            actionPlanItem.SalesActivityType = editActionsViewModel.SalesActivityTypeList[editActionsViewModel.SelectedIndexSalesActivityType];
                            actionPlanItem.IdStatus = editActionsViewModel.ObjActionPlanItem.IdStatus;
                            actionPlanItem.CloseDate = editActionsViewModel.ObjActionPlanItem.CloseDate;
                            actionPlanItem.IdReporter = editActionsViewModel.ObjActionPlanItem.IdReporter;
                            actionPlanItem.IdAssignee = editActionsViewModel.ObjActionPlanItem.IdAssignee;
                            actionPlanItem.CurrentDueDate = editActionsViewModel.ObjActionPlanItem.CurrentDueDate;
                            actionPlanItem.Title = editActionsViewModel.ObjActionPlanItem.Title;
                            actionPlanItem.ModificationDate = GeosApplication.Instance.ServerDateTime;
                        //actionPlanItem.Title = editActionsViewModel.ObjActionPlanItem.Title;
                            if (editActionsViewModel.ObjActionPlanItem.Assignee != null)
                            {
                                actionPlanItem.Assignee = editActionsViewModel.ObjActionPlanItem.Assignee;
                            }

                            actionPlanItem.LstResponsible = editActionsViewModel.ObjActionPlanItem.LstResponsible;
                            //actionPlanItem.Tag = editActionsViewModel.ObjActionPlanItem.Tag;
                            //actionPlanItem.Tag.Name = editActionsViewModel.ObjActionPlanItem.Tag.Name;
                            actionPlanItem.Group = editActionsViewModel.ObjActionPlanItem.Group;
                            actionPlanItem.Plant = editActionsViewModel.ObjActionPlanItem.Plant;
                            //actionPlanItem.SalesActivityType = editActionsViewModel.ObjActionPlanItem.SalesActivityType;
                            actionPlanItem.Scope = editActionsViewModel.ObjActionPlanItem.Scope;
                            actionPlanItem.Status = editActionsViewModel.ObjActionPlanItem.Status;
                            actionPlanItem.Reporter = editActionsViewModel.ObjActionPlanItem.Reporter;

                            actionPlanItem.LogEntriesByActionItems = new List<LogEntriesByActionItem>(editActionsViewModel.ActionPlanItemCommentsList);

                            if(actionPlanItem.LogEntriesByActionItems!=null && actionPlanItem.LogEntriesByActionItems.Count>0)
                            {
                                actionPlanItem.LogEntriesByActionItems.ForEach(i => i.IdActionPlanItem = actionPlanItem.IdActionPlanItem);
                            }
                            actionPlanItem.Modifier = GeosApplication.Instance.ActiveUser.FullName;

                            #region chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                            foreach (Tag actionTag in editActionsViewModel.SelectedTagList)
                            {
                                ActionTagList.Add(actionTag);
                            }
                            actionPlanItem.Name = String.Join("\n", ActionTagList.Select(a => a.Name));
                            #endregion
                        }

                        detailView.BestFitColumns();
                    }
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

                view = ActionPlanMultipleCellEditHelper.Viewtableview;
                if (ActionPlanMultipleCellEditHelper.IsValueChanged)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeadseditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        UpdateMultipleRowsActionsGridCommandAction(ActionPlanMultipleCellEditHelper.Viewtableview);
                    }
                    ActionPlanMultipleCellEditHelper.IsValueChanged = false;
                }

                ActionPlanMultipleCellEditHelper.IsValueChanged = false;

                if (view != null)
                {
                    ActionPlanMultipleCellEditHelper.SetIsValueChanged(view, false);
                }

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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
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

        public void StartPleaseWaitIndicator()
        {

            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = System.Windows.ResizeMode.NoResize,
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
        }

        #region Old code
        //private void ExportActionsGridButtonCommandAction(object obj)
        //{

        //    Workbook workbook = new Workbook();
        //    FileStream stream = null;

        //    GeosApplication.Instance.Logger.Log("Method ActionPlanExportToExcel ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {

        //        SaveFileDialog saveFile = new SaveFileDialog();
        //        saveFile.DefaultExt = "xlsx";
        //        saveFile.FileName = "Actions";
        //        saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
        //        saveFile.FilterIndex = 1;
        //        saveFile.Title = "Save Action Plan Excel Report";

        //        if (!(Boolean)saveFile.ShowDialog())
        //        {
        //            ExcelFilePath = string.Empty;
        //        }
        //        else
        //        {
        //            StartPleaseWaitIndicator();

        //            byte[] excelTemplateInByte = GeosService.GetActionPlanExcel();

        //            List<ActionPlanItem> List = new List<ActionPlanItem>(ActionPlanItemList);

        //            if (List != null && List.Count > 0)
        //            {
        //                ExcelFilePath = (saveFile.FileName);
        //                workbook.LoadDocument(excelTemplateInByte, DocumentFormat.Xlsm);
        //                DevExpress.Spreadsheet.Worksheet wsDl = workbook.Worksheets[1];

        //                // string cellPercentageFormat = @"0.00%";

        //                using (stream = new FileStream(ExcelFilePath, FileMode.Create, FileAccess.ReadWrite))
        //                {
        //                    for (int i = 0; i < List.Count; i++)
        //                    {
        //                        wsDl.Cells[i + 2, 0].Value = List[i].ActionPlan.Code;
        //                        wsDl.Cells[i + 2, 1].Value = List[i].Status.Value;
        //                        wsDl.Cells[i + 2, 2].Value = List[i].SalesActivityType.Value;
        //                        if (List[i].Tag==null)
        //                            wsDl.Cells[i + 2, 3].Value = string.Empty;
        //                       else
        //                            wsDl.Cells[i + 2, 3].Value = List[i].Tag.Name;
        //                        wsDl.Cells[i + 2, 4].Value = List[i].Group;
        //                        wsDl.Cells[i + 2, 5].Value = List[i].Plant;
        //                        wsDl.Cells[i + 2, 6].Value = List[i].Scope;
        //                        wsDl.Cells[i + 2, 7].Value = List[i].Title;
        //                        string comments = "";
        //                        DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);

        //                        if (List[i].LogEntriesByActionItems != null)
        //                        {
        //                            //List<LogEntriesByActionItem> commentList = item.LogEntriesByActionItems.Where(a => a.IdActionPlanItem == item.IdActionPlanItem).ToList();
        //                            foreach (LogEntriesByActionItem CommentRecord in List[i].LogEntriesByActionItems.OrderByDescending(leba => leba.CreationDate))
        //                            {
        //                                if (CommentRecord.IsRtfText == true)
        //                                {
        //                                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        //                                    var doc = new FlowDocument();
        //                                    TextRange range = null;
        //                                    if (CommentRecord.Comment != null)
        //                                    {
        //                                        if (CommentRecord.Comment.ToString() != string.Empty)
        //                                        {
        //                                            doc = new FlowDocument();
        //                                            doc.FontFamily = GeosApplication.Instance.FontFamilyAsPerTheme;
        //                                            MemoryStream streamcomment = new MemoryStream(encoding.GetBytes(System.Convert.ToString(CommentRecord.Comment)));
        //                                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
        //                                            range.Load(streamcomment, DataFormats.Rtf);
        //                                        }
        //                                    }
        //                                    if(String.IsNullOrEmpty(comments))
        //                                        comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + range.Text.ToString();
        //                                    else
        //                                        comments += Environment.NewLine.ToString() + "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + range.Text.ToString();
        //                                }
        //                                else
        //                                {
        //                                    if (String.IsNullOrEmpty(comments))
        //                                        comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Comment;
        //                                    else
        //                                        comments += Environment.NewLine.ToString() + "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Comment;
        //                                }
        //                            }
        //                        }

        //                        wsDl.Cells[i + 2, 8].Value = comments;
        //                        wsDl.Cells[i + 2, 9].Value = List[i].Reporter;
        //                        wsDl.Cells[i + 2, 10].Value = List[i].CreationDate;
        //                        wsDl.Cells[i + 2, 11].Value = List[i].ModificationDate;
        //                        wsDl.Cells[i + 2, 12].Value = List[i].CurrentDueDate;
        //                        wsDl.Cells[i + 2, 13].Value = List[i].CloseDate;
        //                        wsDl.Cells[i + 2, 14].Value = List[i].Assignee;

        //                    }

        //                    DevExpress.Spreadsheet.Worksheet wsGeneralInfo = workbook.Worksheets[2];
        //                    if (GeosApplication.Instance.IdUserPermission == 21)
        //                    {
        //                        if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
        //                        {
        //                            List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
        //                            string salesOwnersFullName = string.Join(",", salesOwners.Select(i => i.User.FullName));

        //                            wsGeneralInfo.Cells[1, 1].Value = salesOwnersFullName;
        //                        }
        //                    }
        //                    else if (GeosApplication.Instance.IdUserPermission == 22)
        //                    {
        //                        if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
        //                        {
        //                            List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
        //                            string plantOwnersAlias = string.Join(",", plantOwners.Select(i => i.Alias));
        //                            wsGeneralInfo.Cells[1, 1].Value = plantOwnersAlias;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        wsGeneralInfo.Cells[1, 1].Value = GeosApplication.Instance.ActiveUser.FullName;
        //                    }

        //                    wsGeneralInfo.Cells[2, 1].Value = DateTime.Now.Date;
        //                    wsGeneralInfo.Cells[17, 1].Value = 9;

        //                    DevExpress.Spreadsheet.Worksheet wsSAP = workbook.Worksheets[0];
        //                    int row = 9;

        //                    wsSAP.Rows.Insert(10, List.Count() - 1, RowFormatMode.FormatAsPrevious);


        //                    for (int i = 0; i < List.Count; i++)
        //                    {

        //                        wsSAP.Cells[row, 1].Value = List[i].ActionPlan.Code;

        //                        wsSAP.Cells[row, 2].Fill.BackgroundColor = ColorTranslator.FromHtml(List[i].SalesActivityType.HtmlColor);
        //                        wsSAP.Cells[row, 2].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
        //                        wsSAP.Cells[row, 2].Alignment.WrapText = true;
        //                        wsSAP.Cells[row, 2].Value = List[i].SalesActivityType.Value;
        //                        wsSAP.Cells[row, 3].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
        //                        if (List[i].Tag == null)
        //                            wsSAP.Cells[row, 3].Value = string.Empty;
        //                        else
        //                            wsSAP.Cells[row, 3].Value = List[i].Tag.Name;
        //                        wsSAP.Cells[row, 4].Value = List[i].Group;
        //                        wsSAP.Cells[row, 5].Value = List[i].Plant;
        //                        string comments = "";
        //                        RichTextString richText = new RichTextString();
        //                        DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);
        //                        if (List[i].Title != null)
        //                        {
        //                            richText.AddTextRun("Subject : " + List[i].Title, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
        //                            richText.Characters(0, List[i].Title.Length + 10).Font.Bold = true;
        //                        }
        //                        else
        //                        {
        //                            richText.AddTextRun("Subject : ", new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
        //                            richText.Characters(0, 10).Font.Bold = true;
        //                        }
        //                        richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
        //                        if (List[i].CloseDate != null)
        //                        {
        //                            richText.AddTextRun("[" + List[i].CloseDate.Value.ToShortDateString() + "] " + (String.IsNullOrEmpty(List[i].Modifier) ? List[i].Creator : List[i].Modifier) + " : " + "Action Closed.", new RichTextRunFont("Calibri", 11, System.Drawing.Color.Green));
        //                            richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
        //                        }


        //                        //# EE1010
        //                        if (List[i].LogEntriesByActionItems != null)
        //                        {
        //                            //List<LogEntriesByActionItem> commentList = item.LogEntriesByActionItems.Where(a => a.IdActionPlanItem == item.IdActionPlanItem).ToList();
        //                            foreach (LogEntriesByActionItem CommentRecord in List[i].LogEntriesByActionItems.OrderByDescending(leba => leba.CreationDate))
        //                            {
        //                                if (CommentRecord.IsRtfText == true)
        //                                {
        //                                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        //                                    var doc = new FlowDocument();
        //                                    TextRange range = null;
        //                                    if (CommentRecord.Comment != null)
        //                                    {
        //                                        if (CommentRecord.Comment.ToString() != string.Empty)
        //                                        {
        //                                            doc = new FlowDocument();
        //                                            doc.FontFamily = GeosApplication.Instance.FontFamilyAsPerTheme;
        //                                            MemoryStream streamComment = new MemoryStream(encoding.GetBytes(System.Convert.ToString(CommentRecord.Comment)));
        //                                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
        //                                            range.Load(streamComment, DataFormats.Rtf);
        //                                        }
        //                                    }
        //                                    comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator + " " + range.Text.ToString() + Environment.NewLine.ToString();
        //                                    richText.AddTextRun("[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Blue));
        //                                    richText.AddTextRun(" : " + range.Text.ToString(), new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
        //                                    richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
        //                                }
        //                                else
        //                                {

        //                                    comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator + " " + CommentRecord.Comment + Environment.NewLine.ToString();
        //                                    richText.AddTextRun("[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Blue));
        //                                    richText.AddTextRun(" : " + CommentRecord.Comment, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
        //                                    richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
        //                                }
        //                            }
        //                        }

        //                        wsSAP.Cells[row, 6].SetRichText(richText);
        //                        //wsSAP.Cells[row, 5].Value =  List[i].Title  + Environment.NewLine.ToString() + comments;
        //                        wsSAP.Cells[row, 7].Value = List[i].ModificationDate;
        //                        wsSAP.Cells[row, 8].Value = List[i].Assignee;

        //                        if ((List[i].CurrentDueDate != null && ((DateTime)(List[i].CurrentDueDate) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date) && (List[i].CloseDate==null) ))
        //                        {
        //                            RichTextString richText_DueDate = new RichTextString();
        //                            richText_DueDate.AddTextRun(List[i].CurrentDueDate.ToShortDateString(), new RichTextRunFont("Calibri", 11, System.Drawing.Color.Red));
        //                            wsSAP.Cells[row, 9].SetRichText(richText_DueDate);
        //                        }
        //                        else
        //                        {
        //                            wsSAP.Cells[row, 9].Value = List[i].CurrentDueDate.ToShortDateString();
        //                        }
        //                        wsSAP.Cells[row, 10].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
        //                        wsSAP.Cells[row, 10].Alignment.WrapText = true;
        //                        wsSAP.Cells[row, 10].Value = List[i].Status.Value;
        //                        wsSAP.Cells[row, 10].Fill.BackgroundColor = ColorTranslator.FromHtml(List[i].Status.HtmlColor);
        //                        wsSAP.Cells[row, 11].CopyFrom(wsSAP.Cells[9, 11]);
        //                        row = row + 1;


        //                    }
        //                    wsSAP.ScrollTo(0, 0);
        //                    workbook.SaveDocument(stream, DocumentFormat.Xlsx);
        //                }




        //            }

        //            System.Diagnostics.Process.Start(excelFilePath);

        //            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        }





        //        GeosApplication.Instance.Logger.Log("Method ActionsExportToExcel() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (System.IO.IOException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //    finally
        //    {
        //        workbook.Dispose();
        //        if (stream != null)
        //            stream.Dispose();
        //    }

        //}

        #endregion Old Code

        #region[GEOS2-5072]
        /// <summary>
        //[pramod.misal][GEOS2-5072][12-01-2024][In Actions Excel file not generated]
        /// Method to expoert excel for Action Plan
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void ExportActionsGridButtonCommandAction(object obj)
        {

            Workbook workbook = new Workbook();
            FileStream stream = null;

            GeosApplication.Instance.Logger.Log("Method ActionPlanExportToExcel ...", category: Category.Info, priority: Priority.Low);

            try
            {

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Actions";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Action Plan Excel Report";

                const int MaxCellLength = 32767;


                if (!(Boolean)saveFile.ShowDialog())
                {
                    ExcelFilePath = string.Empty;
                }
                else
                {
                    StartPleaseWaitIndicator();

                    byte[] excelTemplateInByte = GeosService.GetActionPlanExcel();

                    List<ActionPlanItem> List = new List<ActionPlanItem>(ActionPlanItemList);

                    if (List != null && List.Count > 0)
                    {
                        ExcelFilePath = (saveFile.FileName);
                        workbook.LoadDocument(excelTemplateInByte, DocumentFormat.Xlsm);
                        DevExpress.Spreadsheet.Worksheet wsDl = workbook.Worksheets[1];

                        // string cellPercentageFormat = @"0.00%";

                        using (stream = new FileStream(ExcelFilePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            for (int i = 0; i < List.Count; i++)
                            {
                                wsDl.Cells[i + 2, 0].Value = List[i].ActionPlan.Code;
                                wsDl.Cells[i + 2, 1].Value = List[i].Status.Value;
                                wsDl.Cells[i + 2, 2].Value = List[i].SalesActivityType.Value;
                                wsDl.Cells[i + 2, 3].Value = List[i].Name;
                                wsDl.Cells[i + 2, 4].Value = List[i].Group;
                                wsDl.Cells[i + 2, 5].Value = List[i].Plant;
                                wsDl.Cells[i + 2, 6].Value = List[i].Scope;
                                wsDl.Cells[i + 2, 7].Value = List[i].Title;
                                //const int MaxCellLength = 32767;//PRM
                                string comments = "";
                                DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);


                                if (List[i].LogEntriesByActionItems != null)
                                {
                                    //List<LogEntriesByActionItem> commentList = item.LogEntriesByActionItems.Where(a => a.IdActionPlanItem == item.IdActionPlanItem).ToList();
                                    foreach (LogEntriesByActionItem CommentRecord in List[i].LogEntriesByActionItems.OrderByDescending(leba => leba.CreationDate))
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
                                                    MemoryStream streamcomment = new MemoryStream(encoding.GetBytes(System.Convert.ToString(CommentRecord.Comment)));
                                                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                                    range.Load(streamcomment, DataFormats.Rtf);
                                                }
                                            }
                                            if (String.IsNullOrEmpty(comments))
                                                comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + range.Text.ToString();
                                            else
                                                comments += Environment.NewLine.ToString() + "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + range.Text.ToString();
                                        }
                                        else
                                        {
                                            if (String.IsNullOrEmpty(comments))
                                                comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Comment;
                                            else
                                                comments += Environment.NewLine.ToString() + "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Comment;
                                        }
                                    }
                                }

                                //wsDl.Cells[i + 2, 8].Value = comments;
                                if (comments.Length <= MaxCellLength)
                                {
                                    wsDl.Cells[i + 2, 8].Value = comments;
                                }
                                wsDl.Cells[i + 2, 9].Value = List[i].Reporter;
                                wsDl.Cells[i + 2, 10].Value = List[i].CreationDate;
                                wsDl.Cells[i + 2, 11].Value = List[i].ModificationDate;
                                wsDl.Cells[i + 2, 12].Value = List[i].CurrentDueDate;
                                wsDl.Cells[i + 2, 13].Value = List[i].CloseDate;
                                wsDl.Cells[i + 2, 14].Value = List[i].Assignee;

                            }

                            DevExpress.Spreadsheet.Worksheet wsGeneralInfo = workbook.Worksheets[2];
                            if (GeosApplication.Instance.IdUserPermission == 21)
                            {
                                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                                {
                                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                    string salesOwnersFullName = string.Join(",", salesOwners.Select(i => i.User.FullName));

                                    wsGeneralInfo.Cells[1, 1].Value = salesOwnersFullName;
                                }
                            }
                            else if (GeosApplication.Instance.IdUserPermission == 22)
                            {
                                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                                {
                                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                                    string plantOwnersAlias = string.Join(",", plantOwners.Select(i => i.Alias));
                                    wsGeneralInfo.Cells[1, 1].Value = plantOwnersAlias;
                                }
                            }
                            else
                            {
                                wsGeneralInfo.Cells[1, 1].Value = GeosApplication.Instance.ActiveUser.FullName;
                            }

                            wsGeneralInfo.Cells[2, 1].Value = DateTime.Now.Date;
                            wsGeneralInfo.Cells[17, 1].Value = 9;

                            DevExpress.Spreadsheet.Worksheet wsSAP = workbook.Worksheets[0];
                            int row = 9;

                            wsSAP.Rows.Insert(10, List.Count() - 1, RowFormatMode.FormatAsPrevious);


                            for (int i = 0; i < List.Count; i++)
                            {

                                wsSAP.Cells[row, 1].Value = List[i].ActionPlan.Code;

                                wsSAP.Cells[row, 2].Fill.BackgroundColor = ColorTranslator.FromHtml(List[i].SalesActivityType.HtmlColor);
                                wsSAP.Cells[row, 2].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                wsSAP.Cells[row, 2].Alignment.WrapText = true;
                                wsSAP.Cells[row, 2].Value = List[i].SalesActivityType.Value;
                                wsSAP.Cells[row, 3].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                wsSAP.Cells[row, 3].Value = List[i].Name;
                                wsSAP.Cells[row, 4].Value = List[i].Group;
                                wsSAP.Cells[row, 5].Value = List[i].Plant;
                                string comments = "";
                                RichTextString richText = new RichTextString();
                                DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);
                                if (List[i].Title != null)
                                {
                                    richText.AddTextRun("Subject : " + List[i].Title, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
                                    richText.Characters(0, List[i].Title.Length + 10).Font.Bold = true;
                                }
                                else
                                {
                                    richText.AddTextRun("Subject : ", new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
                                    richText.Characters(0, 10).Font.Bold = true;
                                }
                                richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
                                if (List[i].CloseDate != null)
                                {
                                    richText.AddTextRun("[" + List[i].CloseDate.Value.ToShortDateString() + "] " + (String.IsNullOrEmpty(List[i].Modifier) ? List[i].Creator : List[i].Modifier) + " : " + "Action Closed.", new RichTextRunFont("Calibri", 11, System.Drawing.Color.Green));
                                    richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
                                }


                                //# EE1010
                                if (List[i].LogEntriesByActionItems != null)
                                {
                                    //List<LogEntriesByActionItem> commentList = item.LogEntriesByActionItems.Where(a => a.IdActionPlanItem == item.IdActionPlanItem).ToList();
                                    foreach (LogEntriesByActionItem CommentRecord in List[i].LogEntriesByActionItems.OrderByDescending(leba => leba.CreationDate))
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
                                                    MemoryStream streamComment = new MemoryStream(encoding.GetBytes(System.Convert.ToString(CommentRecord.Comment)));
                                                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                                    range.Load(streamComment, DataFormats.Rtf);
                                                }
                                            }
                                            comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator + " " + range.Text.ToString() + Environment.NewLine.ToString();
                                            richText.AddTextRun("[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Blue));
                                            richText.AddTextRun(" : " + range.Text.ToString(), new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
                                            richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
                                        }
                                        else
                                        {

                                            comments += "[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator + " " + CommentRecord.Comment + Environment.NewLine.ToString();
                                            richText.AddTextRun("[" + CommentRecord.CreationDate.ToShortDateString() + "] " + CommentRecord.Creator, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Blue));
                                            richText.AddTextRun(" : " + CommentRecord.Comment, new RichTextRunFont("Calibri", 11, System.Drawing.Color.Black));
                                            richText.AddTextRun(Environment.NewLine.ToString(), new RichTextRunFont());
                                        }
                                    }
                                }

                                if (richText.Text.Length <= MaxCellLength)
                                {
                                    wsSAP.Cells[row, 6].SetRichText(richText);
                                }

                                //wsSAP.Cells[row, 5].Value =  List[i].Title  + Environment.NewLine.ToString() + comments;
                                wsSAP.Cells[row, 7].Value = List[i].ModificationDate;
                                wsSAP.Cells[row, 8].Value = List[i].Assignee;

                                if ((List[i].CurrentDueDate != null && ((DateTime)(List[i].CurrentDueDate) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date) && (List[i].CloseDate == null)))
                                {
                                    RichTextString richText_DueDate = new RichTextString();
                                    richText_DueDate.AddTextRun(List[i].CurrentDueDate.ToShortDateString(), new RichTextRunFont("Calibri", 11, System.Drawing.Color.Red));
                                    wsSAP.Cells[row, 9].SetRichText(richText_DueDate);
                                }
                                else
                                {
                                    wsSAP.Cells[row, 9].Value = List[i].CurrentDueDate.ToShortDateString();
                                }
                                wsSAP.Cells[row, 10].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                wsSAP.Cells[row, 10].Alignment.WrapText = true;
                                wsSAP.Cells[row, 10].Value = List[i].Status.Value;
                                wsSAP.Cells[row, 10].Fill.BackgroundColor = ColorTranslator.FromHtml(List[i].Status.HtmlColor);
                                wsSAP.Cells[row, 11].CopyFrom(wsSAP.Cells[9, 11]);
                                row = row + 1;


                            }
                            wsSAP.ScrollTo(0, 0);
                            workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                        }




                    }

                    System.Diagnostics.Process.Start(excelFilePath);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }





                GeosApplication.Instance.Logger.Log("Method ActionsExportToExcel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (System.IO.IOException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(ex.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ActionsExportToExcel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                workbook.Dispose();
                if (stream != null)
                    stream.Dispose();
            }

        }

        #endregion [GEOS2-5072]
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

        /// <summary>
        /// [001][GEOS2-2265][avpawar][20-08-2020][Fields are not editable from columns]
        /// Method to save data of multiple rows on main ActionPlan Grid
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateMultipleRowsActionsGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsActionsGridCommandAction ...", category: Category.Info, priority: Priority.Low);

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

                view = obj as TableView;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                IsActionSave = false;
                IsAllSave = false;

                int? cellIdStatus = null;
                int? cellIdResponsible = null;
                string cellSubject = null;
                DateTime? CellCurrentDueDate = null;
                int? idSalesActivityType = null;

                cellIdStatus = StatusList.Where(sl => sl.Value == ActionPlanMultipleCellEditHelper.Status).Select(u => u.IdLookupValue).FirstOrDefault();

                cellIdResponsible = ResponsibleList.Where(al => al.IdPerson == ActionPlanMultipleCellEditHelper.Assignee).Select(u => u.IdPerson).FirstOrDefault();

                cellSubject = ActionPlanMultipleCellEditHelper.Subject;
                CellCurrentDueDate = ActionPlanMultipleCellEditHelper.ExpectedDueDate;

                IList<ActionPlanItem> TempActionPlanItemList = new List<ActionPlanItem>();

                ActionPlanItem[] foundRow = ActionPlanItemList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();

                foreach (ActionPlanItem item in foundRow)
                {
                    ActionPlanItem AP = item;
                    ChangeLogsEntry = new List<LogEntriesByActionItem>();
                    ActionPlanItem _ActionPlanItem = new ActionPlanItem();
                    _ActionPlanItem.IdActionPlanItem = item.IdActionPlanItem;
                    _ActionPlanItem.LogEntriesByActionItems = new List<LogEntriesByActionItem>();

                    if (item.IdSalesActivityType == 273)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotAllowedToMakeChangesActionPlanItemGrid").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        ActionPlanMultipleCellEditHelper.SetIsValueChanged(view, false);
                        ActionPlanMultipleCellEditHelper.IsValueChanged = false;
                        FillCmbSalesOwner();
                        return;
                    }

                    //TempActionPlanItemList.Add(CrmStartUp.GetActionPlanItem_V2120(_ActionPlanItem.IdActionPlanItem));
                    TempActionPlanItemList.Add(CrmStartUp.GetActionPlanItem_V2480(_ActionPlanItem.IdActionPlanItem));   //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:


                    if (AP.Title != null && TempActionPlanItemList[0].Title != AP.Title)
                    {
                        string SubjectOld = TempActionPlanItemList[0].Title;
                        _ActionPlanItem.Title = AP.Title; //cellSubject;
                        string SubjectNew = _ActionPlanItem.Title;

                        ChangeLogsEntry.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogSubject").ToString(), SubjectOld, SubjectNew), IdLogEntryType = 258 });
                    }

                    else if (TempActionPlanItemList[0].Title == AP.Title)
                    {
                        string SubjectOld = TempActionPlanItemList[0].Title;
                        _ActionPlanItem.Title = AP.Title; //cellSubject;
                        string SubjectNew = _ActionPlanItem.Title;
                       // ChangeLogsEntry.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogSubject").ToString(), SubjectOld, SubjectNew), IdLogEntryType = 258 });
                    }

                    if (TempActionPlanItemList[0].CurrentDueDate != AP.CurrentDueDate && CellCurrentDueDate != null)  //AP.CurrentDueDate != null //CellCurrentDueDate && CellCurrentDueDate != null)
                    {
                        _ActionPlanItem.ExpectedDueDate = TempActionPlanItemList[0].CurrentDueDate.Date;
                        _ActionPlanItem.DueDateChangeCount = TempActionPlanItemList[0].DueDateChangeCount + 1;
                        DateTime CurrentDueDateOld = TempActionPlanItemList[0].CurrentDueDate.Date; // _ActionPlanItem.CurrentDueDate.Date;
                        _ActionPlanItem.CurrentDueDate = Convert.ToDateTime(AP.CurrentDueDate);
                        DateTime CurrentDueDateNew = _ActionPlanItem.CurrentDueDate.Date;

                        ChangeLogsEntry.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogDueDate").ToString(), CurrentDueDateOld.ToShortDateString(), CurrentDueDateNew.ToShortDateString()), IdLogEntryType = 258 });
                    }
                    else
                    {
                        _ActionPlanItem.CurrentDueDate = TempActionPlanItemList[0].CurrentDueDate.Date;
                        _ActionPlanItem.DueDateChangeCount = TempActionPlanItemList[0].DueDateChangeCount;
                        _ActionPlanItem.ExpectedDueDate = TempActionPlanItemList[0].ExpectedDueDate;
                    }

                    if (cellIdStatus.HasValue && cellIdStatus != 0)
                    {
                        int StatusListIdCurrent = StatusList.Where(sl => sl.IdLookupValue == AP.Status.IdLookupValue).Select(u => u.IdLookupValue).FirstOrDefault();

                        if (TempActionPlanItemList[0].IdStatus != StatusListIdCurrent)
                        {
                            string StatusNameOld = string.Empty;
                            string StatusNameNew = string.Empty;

                            if (_ActionPlanItem.IdStatus != null)
                            {
                                int IdStatusNameOld = StatusList.Where(sl => sl.IdLookupValue == TempActionPlanItemList[0].IdStatus).Select(u => u.IdLookupValue).SingleOrDefault();
                                StatusNameOld = StatusList.Where(sl => sl.IdLookupValue == TempActionPlanItemList[0].IdStatus).Select(u => u.Value).SingleOrDefault();
                            }

                            _ActionPlanItem.IdStatus = StatusListIdCurrent;

                            if (_ActionPlanItem.IdStatus != null)
                            {
                                int IdStatusNameNew = StatusList.Where(sl => sl.IdLookupValue == _ActionPlanItem.IdStatus).Select(u => u.IdLookupValue).SingleOrDefault();
                                StatusNameNew = StatusList.Where(sl => sl.IdLookupValue == _ActionPlanItem.IdStatus).Select(u => u.Value).SingleOrDefault();
                            }

                            ChangeLogsEntry.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogStatus").ToString(), StatusNameOld, StatusNameNew), IdLogEntryType = 258 });
                        }

                        else
                        {
                            _ActionPlanItem.IdStatus = TempActionPlanItemList[0].IdStatus;
                        }
                    }
                    else
                    {
                        _ActionPlanItem.IdStatus = TempActionPlanItemList[0].IdStatus;
                    }

                    if (cellIdResponsible.HasValue && cellIdResponsible != 0)
                    {
                        int ResponsibleListIdCurrent = ResponsibleList.Where(rl => rl.IdPerson == AP.IdAssignee).Select(u => u.IdPerson).FirstOrDefault();

                        if (TempActionPlanItemList[0].IdAssignee != ResponsibleListIdCurrent)
                        {
                            string ResponsibleNameOld = string.Empty;
                            string ResponsibleNameNew = string.Empty;

                            if (_ActionPlanItem.IdAssignee != null)
                            {
                                int IdResponsibleNameOld = ResponsibleList.Where(rl => rl.IdPerson == TempActionPlanItemList[0].IdAssignee).Select(u => u.IdPerson).SingleOrDefault();
                                ResponsibleNameOld = ResponsibleList.Where(rl => rl.IdPerson == TempActionPlanItemList[0].IdAssignee).Select(u => u.FullName).SingleOrDefault();
                            }

                            _ActionPlanItem.IdAssignee = ResponsibleListIdCurrent;

                            if (_ActionPlanItem.IdAssignee != null)
                            {
                                int IdResponsibleNameNew = ResponsibleList.Where(rl => rl.IdPerson == _ActionPlanItem.IdAssignee).Select(u => u.IdPerson).SingleOrDefault();
                                ResponsibleNameNew = ResponsibleList.Where(rl => rl.IdPerson == _ActionPlanItem.IdAssignee).Select(u => u.FullName).SingleOrDefault();
                            }

                            ChangeLogsEntry.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogResponsible").ToString(), ResponsibleNameOld, ResponsibleNameNew), IdLogEntryType = 258 });
                        }
                        else
                        {
                            _ActionPlanItem.IdAssignee = TempActionPlanItemList[0].IdAssignee;
                        }
                    }
                    else
                    {
                        _ActionPlanItem.IdAssignee = TempActionPlanItemList[0].IdAssignee;
                    }

                    if (ChangeLogsEntry != null)
                        _ActionPlanItem.LogEntriesByActionItems.AddRange(ChangeLogsEntry);
                    _ActionPlanItem.ModificationDate = GeosApplication.Instance.ServerDateTime;
                    _ActionPlanItem.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                    _ActionPlanItem.TransactionOperation = ModelBase.TransactionOperations.Update;

                    if (string.IsNullOrEmpty(_ActionPlanItem.Title))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotAllowedToSaveEmptySubjectActionPlanItemComment").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        ActionPlanMultipleCellEditHelper.SetIsValueChanged(view, false);
                        ActionPlanMultipleCellEditHelper.IsValueChanged = false;
                        FillCmbSalesOwner();
                        return;
                    }
                     IsActionSave = CrmStartUp.UpdateActionPlanItemForGrid_V2380(_ActionPlanItem); //service UpdateActionPlanItemForGrid updated with UpdateActionPlanItemForGrid_V2380 by [rdixit][GEOS2-4372][27.04.2023]
                    if (IsActionSave)
                    {
                        ActionPlanItemList.FirstOrDefault(i => i.IdActionPlanItem == _ActionPlanItem.IdActionPlanItem).Status = StatusList.FirstOrDefault(j => j.IdLookupValue == _ActionPlanItem.IdStatus);
                        if (_ActionPlanItem.IdStatus == 266)
                            ActionPlanItemList.FirstOrDefault(i => i.IdActionPlanItem == _ActionPlanItem.IdActionPlanItem).CloseDate = DateTime.Now;
                    }
                }

                if (IsActionSave)
                    IsAllSave = true;
                else
                    IsAllSave = false;

                if (IsAllSave)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                ActionPlanMultipleCellEditHelper.SetIsValueChanged(view, false);
                ActionPlanMultipleCellEditHelper.IsValueChanged = false;
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsActionsGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsActionsGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsActionsGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsActionsGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][GEOS2-2265][avpawar][20-08-2020][Fields are not editable from columns]
        /// Method to fill responsibleList
        /// </summary>
        private void FillResponsibleList()
        {
            try
            { 
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList...", category: Category.Info, priority: Priority.Low);
                if (ActionPlanItemList != null)
                {
                    var AllIdSiteList = string.Join(",", ActionPlanItemList.Select(i =>       
                        {          
                            if (i.IsInternal == 1)               
                                return "0";           
                            else               
                                return i.IdSite.ToString();       
                        }).Distinct());
                    //[rdixit][GEOS2-6602][26.11.2024]
                    List<People> AllResponsibleList = new List<People>(CrmStartUp.GetActionPlanItemResponsible_V2590(AllIdSiteList));
                    if (AllResponsibleList != null)
                    {
                        foreach (ActionPlanItem item in ActionPlanItemList)
                        {
                            var currentIdSite = item.IsInternal == 1 ? 0 : Convert.ToInt32(item.IdSite);
                            var responsibles = AllResponsibleList.Where(r => r.IdSite == currentIdSite || r.IdSite == 0)?.ToList();
                            ResponsibleList = new ObservableCollection<People>(responsibles);
                            item.LstResponsible = ResponsibleList.ToList();
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillResponsibleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][GEOS2-2265][avpawar][20-08-2020][Fields are not editable from columns]
        /// Method to fill StatusList
        /// </summary>
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList() ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(48);
                StatusList = new List<LookupValue>(tempStatusList);

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][GEOS2-2265][avpawar][20-08-2020][Fields are not editable from columns]
        /// Method to add checked list for column Status and Responsible
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "Status.Value")
                {
                    #region Status
                    List<object> filterItems = new List<object>();

                    foreach (ActionPlanItem item in ActionPlanItemList)
                    {
                        string StatusValue = item.Status.Value;

                        if (StatusValue == null)
                        {
                            continue;
                        }

                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == StatusValue))
                        {
                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = StatusValue;
                            customComboBoxItem.EditValue = item.Status.Value;
                            filterItems.Add(customComboBoxItem);
                        }
                    }

                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                    #endregion
                }

                if (e.Column.FieldName == "IdAssignee")
                {
                    #region IdAssignee
                    // Added By Rahul[rgadhave] for GEOS2-5132 Date-[10-01-2024]
                    HashSet<string> uniqueAssignees = new HashSet<string>();
                    List<object> filterItems = new List<object>();
                    if (ActionPlanItemList != null)
                    {
                        foreach (ActionPlanItem item in ActionPlanItemList)
                        {
                            string AssigneeValue = item.Assignee;

                            if (AssigneeValue == null || !uniqueAssignees.Add(AssigneeValue))
                            {
                                continue;
                            }

                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                            customComboBoxItem.DisplayValue = AssigneeValue;
                            customComboBoxItem.EditValue = item.IdAssignee;
                            filterItems.Add(customComboBoxItem);
                        }
                    }
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                    #endregion
                }

                else if (e.Column.FieldName == "Name")//[GEOS2-5449][rdixit][14.03.2024] removed visual grid items selection in filter
                {
                    #region Tag
                    List<object> filterItems = new List<object>();

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Name = ''")
                    });
                    if (ActionPlanItemList != null)
                    {
                        HashSet<string> uniqueValues = new HashSet<string>();
                        foreach (ActionPlanItem item in ActionPlanItemList)
                        {
                            if (!string.IsNullOrEmpty(item.Name) && item.Name != "")
                            {
                                string[] values = item.Name.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string value in values)
                                {
                                    string displayValue = value.Trim();
                                    if (uniqueValues.Add(displayValue))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = displayValue;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Name Like '%{0}%'", displayValue));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }
                    }
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                    #endregion
                }
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
