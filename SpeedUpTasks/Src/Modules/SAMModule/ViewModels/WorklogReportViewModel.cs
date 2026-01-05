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
                FillWorkLogReportGrid();
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
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                }
                else
                {
                    FillWorkLogReportGrid();
                }


                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                }
                else
                {
                    FillWorkLogReportGrid();
                    SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
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
                FillWorkLogReportGrid();
                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Hidden;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

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
                WorkLogUserList();
                WorklogUserOTAndHours();
                IsGridViewVisible = Visibility.Hidden; 
                IsSchedulerViewVisible = Visibility.Visible;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

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
                    WorkLogUserList();
                    WorklogUserOTAndHours();
                }
                else
                {
                    TableView detailView = (TableView)obj;
                    GridControl gridControl = (detailView).Grid;
                    FillWorkLogReportGrid();
                    SelectedWorkLogReport = WorkLogReportGridList.FirstOrDefault();
                    detailView.SearchString = null;
                }
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

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
