using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduling;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class WorklogReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        private List<TempWorklog> worklogs;
        private List<WorklogUser> users;
        private Visibility isAccordionControlVisible;
        private Duration _currentDuration;
        private WorklogUser selectedUser;

        //puja
        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;

        private List<TempWorklog> workLogReportGridList;
        private TempWorklog selectedWorkLogReport;
        private bool isBusy;
        #endregion

        #region  public Properties
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public string FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }

        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
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
        public List<TempWorklog> Worklogs
        {
            get { return worklogs; }
            set
            {
                worklogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Worklogs"));
            }
        }
        public List<WorklogUser> Users
        {
            get
            {
                return users;
            }

            set
            {
                users = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Users"));
            }
        }
        public Visibility IsAccordionControlVisible
        {
            get
            {
                return isAccordionControlVisible;
            }

            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }
        public WorklogUser SelectedUser
        {
            get
            {
                return selectedUser;
            }

            set
            {
                selectedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUser"));
            }
        }
        public Visibility IsSchedulerViewVisible
        {
            get
            {
                return isSchedulerViewVisible;
            }

            set
            {
                isSchedulerViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSchedulerViewVisible"));
            }
        }

        public Visibility IsGridViewVisible
        {
            get
            {
                return isGridViewVisible;
            }

            set
            {
                isGridViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridViewVisible"));
            }
        }

        public List<TempWorklog> WorkLogReportGridList
        {
            get
            {
                return workLogReportGridList;
            }

            set
            {
                workLogReportGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogReportGridList"));
            }
        }

        public TempWorklog SelectedWorkLogReport
        {
            get
            {
                return selectedWorkLogReport;
            }

            set
            {
                selectedWorkLogReport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkLogReport"));
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

        #region //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6) 15 10 2025

        List<string> failedPlants;
        List<string> successPlantList;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        public List<string> SuccessPlantList
        {
            get { return successPlantList; }
            set { successPlantList = value; }
        }

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }
        #endregion
        #endregion

        #region  public Commands
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowSchedulerViewCommand { get; set; }
        public ICommand ShowGridViewCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; set; }
        public ICommand OTCodeHyperlinkClickCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand DisableAppointmentCommand { get; set; }
        
        #endregion

        #region  public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Event

        #region Constructor

        public WorklogReportViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorklogReportViewModel ...", category: Category.Info, priority: Priority.Low);
                Processing();

                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                ShowSchedulerViewCommand = new RelayCommand(new Action<object>(ShowSchedulerViewAction));
                ShowGridViewCommand = new RelayCommand(new Action<object>(ShowGridViewAction));

                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshWorkLogReoprtAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintWorkLogReoprtAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportWorkLogReoprtAction));
                PlantOwnerPopupClosedCommand = new RelayCommand(new Action<object>(PlantOwnerPopupClosedAction));
                OTCodeHyperlinkClickCommand=new RelayCommand(new Action<object>(OTCodeHyperlinkClickCommandAction));
                PopupMenuShowingCommand = new DelegateCommand<PopupMenuShowingEventArgs>(PopupMenuShowing);
                DisableAppointmentCommand = new DelegateCommand<AppointmentWindowShowingEventArgs>(AppointmentWindowShowing);

                setDefaultPeriod();
                //comment for scheduler
                //WorkLogUserList();
                //WorklogUserOTAndHours();
                //FillWorkLogReportGrid();
                SelectedUser = new WorklogUser();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Collapsed;

                IsCalendarVisible = Visibility.Collapsed;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Constructor WorklogReportViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in AnnualSalesPerformanceViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        #endregion



        #region Methods
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task InitAsync()
        {
            GeosApplication.Instance.Logger.Log("Constructor Init....", category: Category.Info, priority: Priority.Low);
            try
            {
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
                await FillWorkLogReportGridAsync();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitAsync() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task GetWorkLogReportGridAsync()
        {
            try
            {
                await FillWorkLogReportGridAsync();
                SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method GetWorkLogReportGridAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task PlantOwnerPopupClosedActionAsync()
        {
            try
            {
                await FillWorkLogReportGridAsync();
                SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerPopupClosedActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task ShowGridViewActionAsync(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewActionAsync ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                await FillWorkLogReportGridAsync();
                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method ShowGridViewActionAsync() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task RefreshWorkLogReoprtActionAsync(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtActionAsync ...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                await FillWorkLogReportGridAsync();
                SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                detailView.SearchString = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtActionAsync() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkLogReoprtActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AppointmentWindowShowing(AppointmentWindowShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()...", category: Category.Info, priority: Priority.Low);

                obj.Cancel = true;

                GeosApplication.Instance.Logger.Log("Method AppointmentWindowShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method AppointmentWindowShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PopupMenuShowing(PopupMenuShowingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()...", category: Category.Info, priority: Priority.Low);

                if (obj.MenuType == ContextMenuType.CellContextMenu)
                {
                    PopupMenu menu = (PopupMenu)obj.Menu;
                    object open = menu.Items.FirstOrDefault(x => x is BarItem && (string)((BarItem)x).Content == "Change View To");

                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Add((BarItem)open);
                }
                else if (obj.MenuType == ContextMenuType.AppointmentContextMenu)
                {
                    ((DevExpress.Xpf.Bars.PopupMenu)obj.Menu).Items.Clear();
                }

                GeosApplication.Instance.Logger.Log("Method PopupMenuShowing()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PopupMenuShowing()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void OTCodeHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OTCodeHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
                Processing();
                TableView detailView = (TableView)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();

                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                Ots ot = new Ots();
                TempWorklog temp = (TempWorklog)detailView.DataControl.CurrentItem;
                ot.IdOT = temp.IdOT;
                ot.Site = temp.Site;
                ot.IdSite = temp.IdSite;
                //Ots ot = (Ots)detailView.DataControl.CurrentItem;
                workOrderItemDetailsViewModel.Init(ot);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                workOrderItemDetailsView.Owner = Window.GetWindow(ownerInfo);
                workOrderItemDetailsView.ShowDialogWindow();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method OTCodeHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OTCodeHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        public async Task SchedulerActionAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtActionAsync ...", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                var successPlants = new List<string>();
                var failedPlants = new List<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants :";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    /*
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                    */
                    //await WorkLogUserListAsync();
                    //await WorklogUserOTAndHoursAsync();
                    await WorklogUserOTAndHoursAndWorkLogUserListAsync();
                    IsGridViewVisible = Visibility.Hidden;
                    IsSchedulerViewVisible = Visibility.Visible;
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtActionAsync() executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkLogReoprtActionAsync()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }
        private void WorkLogUserList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorkLogUserList...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<WorklogUser> TempUserList = new List<WorklogUser>();
                    Users = new List<WorklogUser>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            TempUserList = new List<WorklogUser>(SAMService.GetWorkLogUserListByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany,plant));
                            Users.AddRange(TempUserList);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in WorkLogUserList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in WorkLogUserList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method WorkLogUserList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    for (int i = 0; i < Users.Count; i++)
                    {
                        if (Users[i].ProfileImageInBytes != null)
                        {
                            Users[i].Image = SAMCommon.Instance.ByteArrayToBitmapImage(Users[i].ProfileImageInBytes);
                        }
                        else
                        {
                            if (Users[i].IdGender == 1)
                                Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                            else if (Users[i].IdGender == 2)
                                Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                            else
                                Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bEmptyImage.png");
                        }
                    }
                    if (Users.Count > 0)
                    {
                        WorklogUser tempUser = new WorklogUser();
                        tempUser.IdUser = -1;
                        tempUser.FirstName = "ALL";
                        var seconds = Users.Sum(a => a.Seconds);
                        seconds = seconds - Users.Sum(a => a.ExtraSeconds);
                        TimeSpan ts = TimeSpan.FromSeconds(seconds);
                        var float_number = ts.TotalHours;
                        var result = float_number - Math.Truncate(float_number);
                        var anwer = result * 60;
                        tempUser.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer,0)) + "M";
                        Users.Insert(0, tempUser);
                    }
                    Users = new List<WorklogUser>(Users);
                }
                else
                {
                    Users = new List<WorklogUser>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WorkLogUserList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkLogUserList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        private async Task WorkLogUserListAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorkLogUserListAsync...", Category.Info, Priority.Low);
                Users = new List<WorklogUser>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    try
                    {
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                var tempUserList = await Task.Run(() =>SAMService.GetWorkLogUserListByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),DateTime.ParseExact(ToDate, "dd/MM/yyyy", null),plant.IdCompany,plant));
                                lock (Users)
                                {
                                    Users.AddRange(tempUserList);
                                    //var status = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    //if (status != null) status.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                lock (FailedPlants)
                                {
                                    if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                        FailedPlants.Add(plant.Alias);
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                    var status = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (status != null) status.IsSuccess = 2;
                                }
                                GeosApplication.Instance.Logger.Log($"Error fetching user list for {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                            }
                        });

                        await Task.WhenAll(tasks);

                        for (int i = 0; i < Users.Count; i++)
                        {
                            if (Users[i].ProfileImageInBytes != null)
                            {
                                Users[i].Image = SAMCommon.Instance.ByteArrayToBitmapImage(Users[i].ProfileImageInBytes);
                            }
                            else
                            {
                                if (Users[i].IdGender == 1)
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (Users[i].IdGender == 2)
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                                else
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bEmptyImage.png");
                            }
                        }
                        if (Users.Count > 0)
                        {
                            WorklogUser tempUser = new WorklogUser();
                            tempUser.IdUser = -1;
                            tempUser.FirstName = "ALL";
                            var seconds = Users.Sum(a => a.Seconds);
                            seconds = seconds - Users.Sum(a => a.ExtraSeconds);
                            TimeSpan ts = TimeSpan.FromSeconds(seconds);
                            var float_number = ts.TotalHours;
                            var result = float_number - Math.Truncate(float_number);
                            var anwer = result * 60;
                            tempUser.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";
                            Users.Insert(0, tempUser);
                        }
                        Users = new List<WorklogUser>(Users);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in WorkLogUserListAsync() " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in WorkLogUserListAsync() - ServiceUnexceptedException " + ex.Message, Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in WorkLogUserListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                    }
                }
                else
                {
                    Users = new List<WorklogUser>();
                }
                GeosApplication.Instance.Logger.Log("Method WorkLogUserListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in WorkLogUserListAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void WorklogUserOTAndHours()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UserSelectCommandAction...", category: Category.Info, priority: Priority.Low);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<TempWorklog> TempWorklogs = new List<TempWorklog>();
                    Worklogs = new List<TempWorklog>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            TempWorklogs = new List<TempWorklog>(SAMService.GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany,plant));
                            Worklogs.AddRange(TempWorklogs);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in UserSelectCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in UserSelectCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method UserSelectCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    //total hours by date and user
                    var records = Worklogs.GroupBy(a => new { a.IdUser, a.StartTime })
                        .Select(a => new TempWorklog()
                        {
                            IdUser = a.Key.IdUser,
                            StartTime=a.Key.StartTime
                        });
                    foreach (var rec in records)
                    {
                        var record = Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).FirstOrDefault();
                        if (record != null)
                        {
                            var seconds = Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.Seconds);
                            seconds = seconds - Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.ExtraSeconds);
                            TimeSpan ts = TimeSpan.FromSeconds(seconds);
                            var float_number = ts.TotalHours;
                            var result = float_number - Math.Truncate(float_number);
                            var anwer = result * 60;
                            Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).ToList().ForEach(a => { a.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer,0)) + "M"; a.Description = "Total Hrs : " + Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer,0)) + "M"; });
                        }
                    }

                    //grand hours by date
                    var dates = Worklogs.Select(a => a.StartTime).Distinct();

                    foreach(DateTime date in dates.ToList())
                    {
                        TempWorklog worklog = new TempWorklog();
                        worklog.IdOT = -1;
                        worklog.StartTime = date;
                        worklog.EndTime = date;
                        worklog.IdUser = -1;
                        worklog.OtCode = "Total Hours";
                        var seconds = Worklogs.Where(a=>a.StartTime==date).Sum(a => a.Seconds);
                        seconds =seconds - Worklogs.Where(a => a.StartTime == date).Sum(a => a.ExtraSeconds);
                        TimeSpan ts = TimeSpan.FromSeconds(seconds);
                        var float_number = ts.TotalHours;
                        var result = float_number - Math.Truncate(float_number);
                        var anwer = result * 60;
                        worklog.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer,0)) + "M";
                        worklog.Description = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer,0)) + "M";

                        Worklogs.Add(worklog);
                    }
                    //day view sequence
                    Worklogs = new List<TempWorklog>(Worklogs.OrderBy(a => a.IdUser));
                    DateTime? prevDate = DateTime.Now;
                    int count = 0;
                    int min = 30;
                    int max = 30;
                    for (int i = 0; i < Worklogs.Count; i++)
                    {
                        if (count == 0)
                        {
                            Worklogs[i].StartTime = Worklogs[i].StartTime;
                            Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                            prevDate = Worklogs[i].EndTime;
                            count = 1;
                        }
                        else
                        {
                            if (Worklogs[i].StartTime.Value.Date == prevDate.Value.Date)
                            {
                                Worklogs[i].StartTime = prevDate;
                                Worklogs[i].EndTime = prevDate.Value.AddMinutes(max);
                                prevDate = Worklogs[i].EndTime;
                            }
                            else
                            {
                                min = 30;
                                max = 30;
                                Worklogs[i].StartTime = Worklogs[i].StartTime;
                                Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                                prevDate = Worklogs[i].EndTime;
                            }
                        }
                    }


                    Worklogs = new List<TempWorklog>(Worklogs);
                }
                else
                {
                    Worklogs = new List<TempWorklog>();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method UserSelectCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UserSelectCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        private async Task WorklogUserOTAndHoursAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogUserOTAndHoursAsync...", Category.Info, Priority.Low);

                Worklogs = new List<TempWorklog>();
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    try
                    {
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Where(plant => !FailedPlants.Contains(plant.Alias, StringComparer.OrdinalIgnoreCase))
                            .Select(async plant =>
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                var tempWorklogs = await Task.Run(() =>SAMService.GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),DateTime.ParseExact(ToDate, "dd/MM/yyyy", null),plant.IdCompany,plant));
                                lock (Worklogs)
                                {
                                    Worklogs.AddRange(tempWorklogs);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                lock (FailedPlants)
                                {
                                    if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                        FailedPlants.Add(plant.Alias);
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 2;
                                }
                                GeosApplication.Instance.Logger.Log($"Error fetching OT data for {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                            }
                        });
                        await Task.WhenAll(tasks);
                        //total hours by date and user
                        var records = Worklogs.GroupBy(a => new { a.IdUser, a.StartTime })
                            .Select(a => new TempWorklog()
                            {
                                IdUser = a.Key.IdUser,
                                StartTime = a.Key.StartTime
                            });
                        foreach (var rec in records)
                        {
                            var record = Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).FirstOrDefault();
                            if (record != null)
                            {
                                var seconds = Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.Seconds);
                                seconds = seconds - Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.ExtraSeconds);
                                TimeSpan ts = TimeSpan.FromSeconds(seconds);
                                var float_number = ts.TotalHours;
                                var result = float_number - Math.Truncate(float_number);
                                var anwer = result * 60;
                                Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).ToList().ForEach(a => { a.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M"; a.Description = "Total Hrs : " + Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M"; });
                            }
                        }

                        //grand hours by date
                        var dates = Worklogs.Select(a => a.StartTime).Distinct();

                        foreach (DateTime date in dates.ToList())
                        {
                            TempWorklog worklog = new TempWorklog();
                            worklog.IdOT = -1;
                            worklog.StartTime = date;
                            worklog.EndTime = date;
                            worklog.IdUser = -1;
                            worklog.OtCode = "Total Hours";
                            var seconds = Worklogs.Where(a => a.StartTime == date).Sum(a => a.Seconds);
                            seconds = seconds - Worklogs.Where(a => a.StartTime == date).Sum(a => a.ExtraSeconds);
                            TimeSpan ts = TimeSpan.FromSeconds(seconds);
                            var float_number = ts.TotalHours;
                            var result = float_number - Math.Truncate(float_number);
                            var anwer = result * 60;
                            worklog.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";
                            worklog.Description = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";

                            Worklogs.Add(worklog);
                        }
                        //day view sequence
                        Worklogs = new List<TempWorklog>(Worklogs.OrderBy(a => a.IdUser));
                        DateTime? prevDate = DateTime.Now;
                        int count = 0;
                        int min = 30;
                        int max = 30;
                        for (int i = 0; i < Worklogs.Count; i++)
                        {
                            if (count == 0)
                            {
                                Worklogs[i].StartTime = Worklogs[i].StartTime;
                                Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                                prevDate = Worklogs[i].EndTime;
                                count = 1;
                            }
                            else
                            {
                                if (Worklogs[i].StartTime.Value.Date == prevDate.Value.Date)
                                {
                                    Worklogs[i].StartTime = prevDate;
                                    Worklogs[i].EndTime = prevDate.Value.AddMinutes(max);
                                    prevDate = Worklogs[i].EndTime;
                                }
                                else
                                {
                                    min = 30;
                                    max = 30;
                                    Worklogs[i].StartTime = Worklogs[i].StartTime;
                                    Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                                    prevDate = Worklogs[i].EndTime;
                                }
                            }
                        }
                        Worklogs = new List<TempWorklog>(Worklogs);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in WorklogUserOTAndHoursAsync() " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("ServiceUnexceptedException in WorklogUserOTAndHoursAsync " + ex.Message, Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in WorklogUserOTAndHoursAsync()...." + ex.Message, Category.Exception, Priority.Low);
                    }
                }
                else
                {
                    Worklogs = new List<TempWorklog>();
                }
                GeosApplication.Instance.Logger.Log("Method WorklogUserOTAndHoursAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in WorklogUserOTAndHoursAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }

        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        private async Task WorklogUserOTAndHoursAndWorkLogUserListAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorklogUserOTAndHoursAndWorkLogUserListAsync...", Category.Info, Priority.Low);

                Worklogs = new List<TempWorklog>();
                Users = new List<WorklogUser>();
                var semaphore = new SemaphoreSlim(15);
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    try
                    {
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                //var tempWorklogs = await Task.Run(() => SAMService.GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany, plant));
                                //var tempUserList = await Task.Run(() =>SAMService.GetWorkLogUserListByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),DateTime.ParseExact(ToDate, "dd/MM/yyyy", null),plant.IdCompany,plant));
                                //lock (Worklogs)
                                //{
                                //    Worklogs.AddRange(tempWorklogs);
                                //}
                                //lock (Users)
                                //{
                                //    Users.AddRange(tempUserList);
                                //}
                                var tempWorklogs = await Task.Run(() => SAMService.GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany, plant));
                                var tempUserList = await Task.Run(() => SAMService.GetWorkLogUserListByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany, plant));
                                lock (Worklogs) Worklogs.AddRange(tempWorklogs);
                                lock (Users) Users.AddRange(tempUserList);
                                var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                if (statusMsg != null) statusMsg.IsSuccess = 1;
                            }
                            catch (Exception ex)
                            {
                                lock (FailedPlants)
                                {
                                    if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                        FailedPlants.Add(plant.Alias);
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 2;
                                }
                                GeosApplication.Instance.Logger.Log($"Error fetching OT data for {plant.Alias}: {ex.Message}", Category.Exception, Priority.Low);
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });
                        await Task.WhenAll(tasks);
                        LoadingData();
                        for (int i = 0; i < Users.Count; i++)
                        {
                            if (Users[i].ProfileImageInBytes != null)
                            {
                                Users[i].Image = SAMCommon.Instance.ByteArrayToBitmapImage(Users[i].ProfileImageInBytes);
                            }
                            else
                            {
                                if (Users[i].IdGender == 1)
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (Users[i].IdGender == 2)
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                                else
                                    Users[i].Image = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bEmptyImage.png");
                            }
                        }
                        if (Users.Count > 0)
                        {
                            WorklogUser tempUser = new WorklogUser();
                            tempUser.IdUser = -1;
                            tempUser.FirstName = "ALL";
                            var seconds = Users.Sum(a => a.Seconds);
                            seconds = seconds - Users.Sum(a => a.ExtraSeconds);
                            TimeSpan ts = TimeSpan.FromSeconds(seconds);
                            var float_number = ts.TotalHours;
                            var result = float_number - Math.Truncate(float_number);
                            var anwer = result * 60;
                            tempUser.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";
                            Users.Insert(0, tempUser);
                        }
                        Users = new List<WorklogUser>(Users);



                        //total hours by date and user
                        var records = Worklogs.GroupBy(a => new { a.IdUser, a.StartTime })
                            .Select(a => new TempWorklog()
                            {
                                IdUser = a.Key.IdUser,
                                StartTime = a.Key.StartTime
                            });
                        foreach (var rec in records)
                        {
                            var record = Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).FirstOrDefault();
                            if (record != null)
                            {
                                var seconds = Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.Seconds);
                                seconds = seconds - Worklogs.Where(a => a.StartTime == record.StartTime && a.IdUser == record.IdUser).Sum(a => a.ExtraSeconds);
                                TimeSpan ts = TimeSpan.FromSeconds(seconds);
                                var float_number = ts.TotalHours;
                                var result = float_number - Math.Truncate(float_number);
                                var anwer = result * 60;
                                Worklogs.Where(a => a.IdUser == rec.IdUser && a.StartTime == rec.StartTime && a.IdOT < 0).ToList().ForEach(a => { a.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M"; a.Description = "Total Hrs : " + Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M"; });
                            }
                        }

                        //grand hours by date
                        var dates = Worklogs.Select(a => a.StartTime).Distinct();

                        foreach (DateTime date in dates.ToList())
                        {
                            TempWorklog worklog = new TempWorklog();
                            worklog.IdOT = -1;
                            worklog.StartTime = date;
                            worklog.EndTime = date;
                            worklog.IdUser = -1;
                            worklog.OtCode = "Total Hours";
                            var seconds = Worklogs.Where(a => a.StartTime == date).Sum(a => a.Seconds);
                            seconds = seconds - Worklogs.Where(a => a.StartTime == date).Sum(a => a.ExtraSeconds);
                            TimeSpan ts = TimeSpan.FromSeconds(seconds);
                            var float_number = ts.TotalHours;
                            var result = float_number - Math.Truncate(float_number);
                            var anwer = result * 60;
                            worklog.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";
                            worklog.Description = Math.Truncate(float_number) + "H " + Math.Truncate(Math.Round(anwer, 0)) + "M";

                            Worklogs.Add(worklog);
                        }
                        //day view sequence
                        Worklogs = new List<TempWorklog>(Worklogs.OrderBy(a => a.IdUser));
                        DateTime? prevDate = DateTime.Now;
                        int count = 0;
                        int min = 30;
                        int max = 30;
                        for (int i = 0; i < Worklogs.Count; i++)
                        {
                            if (count == 0)
                            {
                                Worklogs[i].StartTime = Worklogs[i].StartTime;
                                Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                                prevDate = Worklogs[i].EndTime;
                                count = 1;
                            }
                            else
                            {
                                if (Worklogs[i].StartTime.Value.Date == prevDate.Value.Date)
                                {
                                    Worklogs[i].StartTime = prevDate;
                                    Worklogs[i].EndTime = prevDate.Value.AddMinutes(max);
                                    prevDate = Worklogs[i].EndTime;
                                }
                                else
                                {
                                    min = 30;
                                    max = 30;
                                    Worklogs[i].StartTime = Worklogs[i].StartTime;
                                    Worklogs[i].EndTime = Worklogs[i].StartTime.Value.AddMinutes(max);
                                    prevDate = Worklogs[i].EndTime;
                                }
                            }
                        }
                        Worklogs = new List<TempWorklog>(Worklogs);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in WorklogUserOTAndHoursAndWorkLogUserListAsync() " + ex.Detail.ErrorMessage, Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("ServiceUnexceptedException in WorklogUserOTAndHoursAndWorkLogUserListAsync " + ex.Message, Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in WorklogUserOTAndHoursAndWorkLogUserListAsync()...." + ex.Message, Category.Exception, Priority.Low);
                    }
                }
                else
                {
                    Worklogs = new List<TempWorklog>();
                }
                GeosApplication.Instance.Logger.Log("Method WorklogUserOTAndHoursAndWorkLogUserListAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in WorklogUserOTAndHoursAndWorkLogUserListAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }
        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    ToDate = thisMonthEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString("dd/MM/yyyy");
                    ToDate = EndDate.ToString("dd/MM/yyyy");
                }
                if (IsSchedulerViewVisible == Visibility.Visible)
                {
                    /*
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                    */
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    SchedulerActionAsync();
                }
                else
                {
                    //FillWorkLogReportGrid();
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    GetWorkLogReportGridAsync();
                }


                IsBusy = false;
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }

        private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            IsCalendarVisible = Visibility.Collapsed;
        }


        private void PlantOwnerPopupClosedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupOpenedAction()...", category: Category.Info, priority: Priority.Low);

                Processing();
                if (IsSchedulerViewVisible == Visibility.Visible)
                {
                    /*
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                    */
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    SchedulerActionAsync();
                }
                else
                {
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    PlantOwnerPopupClosedActionAsync();
                    /*
                    FillWorkLogReportGrid();
                    SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                    */
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowGridViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                /*
                FillWorkLogReportGrid();
                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                */
				//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                ShowGridViewActionAsync(obj);
                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowSchedulerViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                /*
                WorkLogUserList();
                WorklogUserOTAndHours();
                */
				//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                SchedulerActionAsync();

                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSchedulerViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Worklog Report List";
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
                    Processing();

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
                    GeosApplication.Instance.Logger.Log("Method ExportWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        private void PrintWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Processing();

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["WorkLogReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["WorkLogReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method PrintWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshWorkLogReoprtAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtAction()...", category: Category.Info, priority: Priority.Low);

                Processing();
                if (IsSchedulerViewVisible == Visibility.Visible)
                {
                    /*
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                    */
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    SchedulerActionAsync();
                }
                else
                {
					//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
                    RefreshWorkLogReoprtActionAsync(obj);
                    /*
                    TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    FillWorkLogReportGrid();
                    SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                    detailView.SearchString = null;
                    */
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method RefreshWorkLogReoprtAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkLogReoprtAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshWorkLogReoprtAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshWorkLogReoprtAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        private async Task FillWorkLogReportGridAsync()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportGridAsync...", Category.Info, Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                GeosApplication.Instance.CustomeSplashScreenMessage = "Please wait";
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Collecting the information from plants ...";
                GeosApplication.Instance.StatusMessages = new ObservableCollection<Data.Common.Crm.StatusMessage>();
                WorkLogReportGridList = new List<TempWorklog>();
                var successPlants = new List<string>();
                var failedPlants = new List<string>();
                FailedPlants = new List<string>();
                SuccessPlantList = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    // Initialize plant status
                    foreach (Company com in SAMCommon.Instance.SelectedPlantOwnerList)
                    {
                        GeosApplication.Instance.StatusMessages.Add(new Data.Common.Crm.StatusMessage() { Symbol = "", Message = com.ShortName, IsSuccess = 0 });
                    }
                    try
                    {
                        var tasks = SAMCommon.Instance.SelectedPlantOwnerList.Cast<Company>().Select(async plant =>
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + plant.Alias;
                                // Run service call asynchronously
                                var tempList = await Task.Run(() =>SAMService.GetOTWorkLogTimesByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),DateTime.ParseExact(ToDate, "dd/MM/yyyy", null),plant.IdCompany,plant));
                                lock (WorkLogReportGridList)
                                {
                                    WorkLogReportGridList.AddRange(tempList);
                                    successPlants.Add(plant.Alias);
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                lock (FailedPlants)
                                {
                                    if (!FailedPlants.Any(a => a.Equals(plant.Alias, StringComparison.OrdinalIgnoreCase)))
                                        FailedPlants.Add(plant.Alias);
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Format((string)Application.Current.FindResource("DataLoadingFailMessage"), string.Join(",", FailedPlants));
                                    var statusMsg = GeosApplication.Instance.StatusMessages.FirstOrDefault(x => x.Message == plant.Alias);
                                    if (statusMsg != null) statusMsg.IsSuccess = 2;
                                }
                                GeosApplication.Instance.Logger.Log($"Error fetching work log data for plant {plant.Alias}: {ex.Message}",Category.Exception, Priority.Low);
                            }
                        });
                        await Task.WhenAll(tasks);
                        //LoadingData();
                        WorkLogReportGridList = new List<TempWorklog>(WorkLogReportGridList);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in FillWorkLogReportGridAsync() method " + ex.Detail.ErrorMessage,Category.Exception, Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null),"Red",CustomMessageBox.MessageImagePath.NotOk,MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in FillWorkLogReportGridAsync() Method - ServiceUnexceptedException " + ex.Message,Category.Exception, Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType,GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                        GeosApplication.Instance.Logger.Log("Error in Method FillWorkLogReportGridAsync()...." + ex.Message,Category.Exception,Priority.Low);
                    }
                }
                else
                {
                    WorkLogReportGridList = new List<TempWorklog>();
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportGridAsync executed successfully.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkLogReportGridAsync() method " + ex.Message, Category.Exception, Priority.Low);
            }
            finally
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            }
        }
		//Shubham[skadam] [V.2.6.8.0] GEOS2-8851 SAM module very slow when trying to load informations - Reports-> Worklog Report (4/6)	17 10 2025
        private void LoadingData()
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                }, x => { return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() }; }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = "Loading data...";
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                GeosApplication.Instance.Logger.Log("Get an error in LoadingData() method " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        private void FillWorkLogReportGrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportGrid...", category: Category.Info, priority: Priority.Low);

                if (SAMCommon.Instance.SelectedPlantOwnerList != null)
                {
                    List<TempWorklog> TempWorkLogReportGridList = new List<TempWorklog>();
                    WorkLogReportGridList = new List<TempWorklog>();

                    try
                    {
                        foreach (Company plant in SAMCommon.Instance.SelectedPlantOwnerList)
                        {
                            TempWorkLogReportGridList = new List<TempWorklog>(SAMService.GetOTWorkLogTimesByPeriodAndSite(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plant.IdCompany,plant));
                            WorkLogReportGridList.AddRange(TempWorkLogReportGridList);
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillWorkLogReportGrid() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in FillWorkLogReportGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillWorkLogReportGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    WorkLogReportGridList = new List<TempWorklog>(WorkLogReportGridList);
                }
                else
                {
                    WorkLogReportGridList = new List<TempWorklog>();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method FillWorkLogReportGrid executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkLogReportGrid() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void setDefaultPeriod()
        {
            DateTime baseDate = DateTime.Today;
            var today = baseDate;
            //this week
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            FromDate = thisWeekStart.ToString("dd/MM/yyyy");
            ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
        }

        //method for Hide unhide Pannel
        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                // IsSchedulerViewVisible = Visibility.Visible;
                // IsGridViewVisible = Visibility.Hidden;
                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private Action Processing()
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
            return null;
        }
        public void Dispose()
        {
        }
        #endregion
    }
}
