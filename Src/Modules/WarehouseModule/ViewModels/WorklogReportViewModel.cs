using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WorklogReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        //[Sudhir.Jangra][GEOS2-4271][24/05/2023]
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Declaration
        string fromDate;
        string toDate;
        int isButtonStatus;
        DateTime startDate;
        DateTime endDate;
        Visibility isCalendarVisible;
        private List<WarehouseWorklogReport> worklogs;
        private List<WarehouseWorkLogUser> users;
        private Visibility isAccordionControlVisible;
        private Duration _currentDuration;
        private WarehouseWorkLogUser selectedUser;

        private Visibility isSchedulerViewVisible;
        private Visibility isGridViewVisible;

        private List<WarehouseWorklogReport> workLogReportGridList;
        private WarehouseWorklogReport selectedWorkLogReport;
        private bool isBusy;

        const string shortDateFormat = "dd/MM/yyyy";

        private string totalTimeINHoursAndMinutes;
        #endregion

        #region Properties
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
        public List<WarehouseWorklogReport> Worklogs
        {
            get { return worklogs; }
            set
            {
                worklogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Worklogs"));
            }
        }
        public List<WarehouseWorkLogUser> Users
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
        public WarehouseWorkLogUser SelectedUser
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
 		//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
        Visibility isGraphViewVisible;
        public Visibility IsGraphViewVisible
        {
            get
            {
                return isGraphViewVisible;
            }

            set
            {
                isGraphViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGraphViewVisible"));
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

        public List<WarehouseWorklogReport> WorkLogReportGridList
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

        public WarehouseWorklogReport SelectedWorkLogReport
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

        public string TotalTimeInHoursAndMinutes
        {
            get { return totalTimeINHoursAndMinutes; }
            set
            {
                totalTimeINHoursAndMinutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTimeInHoursAndMinutes"));
            }
        }
 		//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
        List<WarehouseWorklogReport> workLogReportGraphList;
        public List<WarehouseWorklogReport> WorkLogReportGraphList
        {
            get
            {
                return workLogReportGraphList;
            }

            set
            {
                workLogReportGraphList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogReportGraphList"));
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
        public ICommand ChartItemsForecastLoadCommand { get; set; }

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
                OTCodeHyperlinkClickCommand = new RelayCommand(new Action<object>(OTCodeHyperlinkClickCommandAction));

                ChartItemsForecastLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChatControlLoadedEvent);

                setDefaultPeriod();
                FillWorkLogReportGrid();
                ChartViewModel();
                SelectedUser = new WarehouseWorkLogUser();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                IsGridViewVisible = Visibility.Visible;
                IsSchedulerViewVisible = Visibility.Collapsed;
                IsGraphViewVisible = Visibility.Collapsed;
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
                WarehouseWorklogReport temp = (WarehouseWorklogReport)detailView.DataControl.CurrentItem;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(temp.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
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

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<WarehouseWorkLogUser> TempUserList = new List<WarehouseWorkLogUser>();
                    Users = new List<WarehouseWorkLogUser>();

                    try
                    {
                        TempUserList = new List<WarehouseWorkLogUser>(WarehouseService.GetWorkLogUserListByPeriodAndSite_V2390(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), WarehouseCommon.Instance.Selectedwarehouse.IdSite, WarehouseCommon.Instance.Selectedwarehouse));
                        Users.AddRange(TempUserList);
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
                            Users[i].Image = WarehouseCommon.Instance.ByteArrayToBitmapImage(Users[i].ProfileImageInBytes);
                        }
                        else
                        {
                            if (Users[i].IdGender == 1)
                                Users[i].Image = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bFemaleUser.png");
                            else if (Users[i].IdGender == 2)
                                Users[i].Image = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bMaleUser.png");
                            else
                                Users[i].Image = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bEmptyImage.png");
                        }
                    }
                    if (Users.Count > 0)
                    {
                        WarehouseWorkLogUser tempUser = new WarehouseWorkLogUser();
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
                    Users = new List<WarehouseWorkLogUser>(Users);
                }
                else
                {
                    Users = new List<WarehouseWorkLogUser>();
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
                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<WarehouseWorklogReport> TempWorklogs = new List<WarehouseWorklogReport>();
                    Worklogs = new List<WarehouseWorklogReport>();

                    try
                    {
                        TempWorklogs = new List<WarehouseWorklogReport>(WarehouseService.GetWorkLogOTWithHoursAndUserByPeriodAndSite_V2390(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), WarehouseCommon.Instance.Selectedwarehouse.IdSite, WarehouseCommon.Instance.Selectedwarehouse));
                        Worklogs.AddRange(TempWorklogs);
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
                        .Select(a => new WarehouseWorklogReport()
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
                        WarehouseWorklogReport worklog = new WarehouseWorklogReport();
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
                    Worklogs = new List<WarehouseWorklogReport>(Worklogs.OrderBy(a => a.IdUser));
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


                    Worklogs = new List<WarehouseWorklogReport>(Worklogs);
                }
                else
                {
                    Worklogs = new List<WarehouseWorklogReport>();
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

                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString(shortDateFormat);
                    ToDate = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString(shortDateFormat);
                    ToDate = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString(shortDateFormat);
                    ToDate = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString(shortDateFormat);
                    ToDate = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString(shortDateFormat);
                    ToDate = EndDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString(shortDateFormat);
                    ToDate = EndToDate.ToString(shortDateFormat);
                }

                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }

 				//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
                // if (IsSchedulerViewVisible == Visibility.Visible)
                if (IsGraphViewVisible == Visibility.Visible)
                {
                    FillWorkLogReportGrid();
                    ChartViewModel();
                    //WorkLogUserList();
                    //WorklogUserOTAndHours();
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
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Collapsed;

        }


        private void PlantOwnerPopupClosedAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupOpenedAction()...", category: Category.Info, priority: Priority.Low);

                Processing();
 				//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
                if (IsGraphViewVisible == Visibility.Visible)
                {
                    FillWorkLogReportGrid();
                    ChartViewModel();
                    //WorkLogUserList();
                    //WorklogUserOTAndHours();
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
                IsGraphViewVisible = Visibility.Hidden;
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
                //Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
                //WorkLogUserList();
                //WorklogUserOTAndHours();
                InitChartControl();
                FillWorkLogReportGrid();
                ChartViewModel();   
                IsGridViewVisible = Visibility.Hidden;
                IsGraphViewVisible = Visibility.Visible;
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
                    activityTableView.ShowTotalSummary = true;
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
 				//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
                if (IsGraphViewVisible == Visibility.Visible)
                {
                    FillWorkLogReportGrid();
                    ChartViewModel();
                    //WorkLogUserList();
                    //WorklogUserOTAndHours();
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

                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    List<WarehouseWorklogReport> TempWorkLogReportGridList = new List<WarehouseWorklogReport>();
                    WorkLogReportGridList = new List<WarehouseWorklogReport>();

                    try
                    {
                        TempWorkLogReportGridList = new List<WarehouseWorklogReport>(WarehouseService.GetOTWorkLogTimesByPeriodAndSite_V2390(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), WarehouseCommon.Instance.Selectedwarehouse.IdSite, WarehouseCommon.Instance.Selectedwarehouse));
                        WorkLogReportGridList.AddRange(TempWorkLogReportGridList);


                        TimeSpan total = TimeSpan.Zero;
                        foreach (var item in WorkLogReportGridList)
                        {
                            total += item.TotalTime;
                        }
                        var TotalTime = total;


                        if (TotalTime.Days > 0)
                        {
                            int Hours = TotalTime.Days * 24 + TotalTime.Hours;
                            string totalHours = string.Format("{0}H {1}M", Hours, TotalTime.Minutes);
                            TotalTimeInHoursAndMinutes = totalHours;
                        }
                        else
                        {
                            string totalHours = string.Format("{0}H {1}M", TotalTime.Hours, TotalTime.Minutes);
                            TotalTimeInHoursAndMinutes = totalHours;
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
                    WorkLogReportGridList = new List<WarehouseWorklogReport>(WorkLogReportGridList);
                }
                else
                {
                    WorkLogReportGridList = new List<WarehouseWorklogReport>();
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
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

 		//Shubham[skadam] GEOS2-4272 Add a new Picking Worklog Report section (3/3) 17 08 2023
        //public ObservableCollection<DataSeries> Data { get; private set; }
        ObservableCollection<DataSeries> data;
        public ObservableCollection<DataSeries> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }

        ObservableCollection<DataPoint> avgPickingTime;
        public ObservableCollection<DataPoint> AVGPickingTime
        {
            get
            {
                return avgPickingTime;
            }

            set
            {
                avgPickingTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGPickingTime"));
            }
        }
        public void ChartViewModel()
        {
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
            List<Int64> WorkLogReportWeekDistinct = new List<Int64>();
            WorkLogReportGraphList = new List<WarehouseWorklogReport>();
            Data = new ObservableCollection<DataSeries>();
            try
            {
                foreach (WarehouseWorklogReport itemWarehouseWorklogReport in WorkLogReportGridList)
                {

                    Int32 Year = Convert.ToDateTime(itemWarehouseWorklogReport.StartTime).Year;
                    Int64 WeekOfYear = GetIso8601WeekOfYear(Convert.ToDateTime(itemWarehouseWorklogReport.StartTime));
                    itemWarehouseWorklogReport.Week = Convert.ToInt64(Year.ToString()+ WeekOfYear.ToString());
                    WorkLogReportGraphList.Add(itemWarehouseWorklogReport);
                }
                var sortedList = WorkLogReportGraphList.OrderBy(q => q.Week).ToList();
                WorkLogReportGraphList = new List<WarehouseWorklogReport>();
                WorkLogReportGraphList.AddRange(sortedList);
            }
            catch (Exception ex)
            {

            }
            try
            {
                List<int> DistinctIdUser = WorkLogReportGraphList.Select(x => x.IdUser).Distinct().ToList();
                foreach (int IdUser in DistinctIdUser)
                {
                    List<Int64> WorkLogReportWeek = new List<Int64>();
                    List<WarehouseWorklogReport> tempWorkLogReportGraphList = WorkLogReportGraphList.Where(x => x.IdUser == IdUser).ToList();
                    DataSeries DataSeries = new DataSeries();
                    DataSeries.Name = "Picking Time " + tempWorkLogReportGraphList.FirstOrDefault().WorklogUser.FullName;
                    //List<string> WorkLogReportWeek = new List<string>();
                    
                    foreach (WarehouseWorklogReport item in tempWorkLogReportGraphList)
                    {
                        if (DataSeries.Values == null)
                        {
                            DataSeries.Values = new ObservableCollection<DataPoint>();
                        }
                        DataPoint newDataPoint=null;
                        TimeSpan sum = new TimeSpan();
                        for (int i = 0; i < tempWorkLogReportGraphList.Count; i++)
                        {
                            if (tempWorkLogReportGraphList[i].Week == item.Week)
                            {
                                sum += TimeSpan.Parse(tempWorkLogReportGraphList[i].TotalTime.ToString());
                            }
                        }
                        // var v = (tempWorkLogReportGraphList.Where(w => w.Week == item2));
                        if (!WorkLogReportWeek.Any(a => a.Equals(item.Week)))
                        {
                            WorkLogReportWeek.Add(item.Week);
                            //newDataPoint = new DataPoint(Convert.ToDateTime(item.StartTime), sum, item.Week);
                            //newDataPoint = new DataPoint(Convert.ToDateTime(item.StartTime), sum.TotalMinutes, item.Week.ToString());
                            newDataPoint = new DataPoint(Convert.ToDateTime(item.StartTime), sum.TotalMinutes, item.Week.ToString(), sum);
                        }
                        if (!WorkLogReportWeekDistinct.Any(a => a.Equals(item.Week)))
                        {
                            WorkLogReportWeekDistinct.Add(item.Week);
                        }
                        if (newDataPoint!=null)
                        DataSeries.Values.Add(newDataPoint);
                    }
                   
                    Data.Add(DataSeries);

                }
                try
                {
                    AVGPickingTime = new ObservableCollection<DataPoint>();
                    DataSeries TotalDataSeries = new DataSeries();
                    TotalDataSeries.Name = "Total Picking Time";
                    foreach (Int64 item in WorkLogReportWeekDistinct)
                    {
                        TimeSpan sum = new TimeSpan();
                       
                        foreach (var weekWiseTotal in Data)
                        {
                            if (weekWiseTotal != null)
                            {
                                var t = weekWiseTotal.Values.Where(w => w.Week.Equals(item.ToString())).Sum(s => s.ValueInMinutes);
                                sum += TimeSpan.FromMinutes(t);
                            }

                        }
                        if (TotalDataSeries.Values == null)
                        {
                            TotalDataSeries.Values = new ObservableCollection<DataPoint>();
                        }
                        List<WarehouseWorklogReport> tempWarehouseWorklogReport = WorkLogReportGraphList.Where(w=>w.Week==item).ToList();
                        var DistinctWarehouseWorklogReport = tempWarehouseWorklogReport.Select(s=>s.IdUser).Distinct().ToList(); 
                       // DataPoint newDataPointForTotal = new DataPoint(Convert.ToDateTime(DateTime.Now.Date), sum.TotalMinutes, item.ToString());
                        DataPoint newDataPointForTotal = new DataPoint(Convert.ToDateTime(DateTime.Now.Date), sum.TotalMinutes, item.ToString(), sum);
                        int count = DistinctWarehouseWorklogReport.Count();
                        if (count==0)
                        {
                            count = 1;
                        }
                        //DataPoint newDataPointForAVGPickingTime =new DataPoint(newDataPointForTotal.Argument, (newDataPointForTotal.ValueInMinutes / count) , newDataPointForTotal.Week);
                        TimeSpan half = new TimeSpan();
                        half = new TimeSpan(newDataPointForTotal.Value.Ticks / count);
                        DataPoint newDataPointForAVGPickingTime = new DataPoint(newDataPointForTotal.Argument, (newDataPointForTotal.ValueInMinutes / count), newDataPointForTotal.Week, half);
                        newDataPointForAVGPickingTime.ValueInMinutes= Math.Round(newDataPointForAVGPickingTime.ValueInMinutes, 2, MidpointRounding.AwayFromZero);
                        AVGPickingTime.Add(newDataPointForAVGPickingTime);
                        TotalDataSeries.Values.Add(newDataPointForTotal);
                    }
                    Data.Add(TotalDataSeries);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //Data = new ObservableCollection<DataSeries> {
            //    new DataSeries{
            //        Name = "DevAV North",
            //        Values = new ObservableCollection<DataPoint> {
            //            new DataPoint (new DateTime(2013,12,31), 362.5,"201352"),
            //            new DataPoint (new DateTime(2014,12,31), 348.4,"201452"),
            //            new DataPoint (new DateTime(2015,12,31), 279.0,"201552"),
            //            new DataPoint (new DateTime(2016,12,31), 230.9,"201652"),
            //            new DataPoint (new DateTime(2017,12,31), 203.5,"201752"),
            //            new DataPoint (new DateTime(2018,12,31), 197.1,"201852")
            //        }
            //    }
            //};


            #region TestData
            //Data = new ObservableCollection<DataSeries> {
            //    new DataSeries{
            //        Name = "DevAV North",
            //        Values = new ObservableCollection<DataPoint> {
            //            new DataPoint (new DateTime(2013,12,31), 362.5,"201352"),
            //            new DataPoint (new DateTime(2014,12,31), 348.4,"201452"),
            //            new DataPoint (new DateTime(2015,12,31), 279.0,"201552"),
            //            new DataPoint (new DateTime(2016,12,31), 230.9,"201652"),
            //            new DataPoint (new DateTime(2017,12,31), 203.5,"201752"),
            //            new DataPoint (new DateTime(2018,12,31), 197.1,"201852")
            //        }
            //    },
            //    new DataSeries{
            //        Name = "DevAV South",
            //        Values = new ObservableCollection<DataPoint> {
            //            new DataPoint (new DateTime(2013,12,31), 277.0,"201352"),
            //            new DataPoint (new DateTime(2014,12,31), 328.5,"201452"),
            //            new DataPoint (new DateTime(2015,12,31), 297.0,"201552"),
            //            new DataPoint (new DateTime(2016,12,31), 255.3,"201652"),
            //            new DataPoint (new DateTime(2017,12,31), 173.5,"201752"),
            //            new DataPoint (new DateTime(2018,12,31), 131.8,"201852")
            //        }
            //    },
            //    new DataSeries{
            //        Name = "sk",
            //        Values = new ObservableCollection<DataPoint> {
            //            new DataPoint (new DateTime(2013,12,31), 277.02,"201352"),
            //            new DataPoint (new DateTime(2014,12,31), 328.5,"201452"),
            //            new DataPoint (new DateTime(2015,12,31), 297.0,"201552")
            //        }
            //    }
            //};
            #endregion
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        ChartControl chartcontrol;
        string filterString;
        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
                InitChartControl();
            }
        }
        private void ChatControlLoadedEvent(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChatControlLoadedEvent ...", category: Category.Info, priority: Priority.Low);
                //FillWorkLogReportGrid();
                //ChartViewModel();
                chartcontrol = (ChartControl)obj;
                InitChartControl();

                GeosApplication.Instance.Logger.Log("Method ChartDashboardSalebyCustomerAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSalebyCustomerAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void InitChartControl()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitChartControl ...", category: Category.Info, priority: Priority.Low);
                if (chartcontrol != null)
                {
                    chartcontrol.BeginInit();
                    DevExpress.Xpf.Charts.XYDiagram2D diagram = (XYDiagram2D)chartcontrol.Diagram;
                    chartcontrol.HorizontalAlignment = HorizontalAlignment.Stretch;
                    chartcontrol.VerticalAlignment = VerticalAlignment.Stretch;
                    chartcontrol.Legend = new Legend();
                    chartcontrol.Legend.HorizontalPosition = HorizontalPosition.RightOutside;
                    diagram.ActualAxisY.Label = new AxisLabel();
                    diagram.ActualAxisY.Label.Formatter = new YAxisLabelFormatterNew();
                    chartcontrol.Diagram = diagram;
                    chartcontrol.EndInit();
                }
                GeosApplication.Instance.Logger.Log("Method InitChartControl() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InitChartControl() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion
    }
    public class DataSeries
    {
        [System.Runtime.Serialization.DataMember]
        public string Name { get; set; }
        [System.Runtime.Serialization.DataMember]
        public ObservableCollection<DataPoint> Values { get; set; }
    }
    
    public class DataPoint
    {
        [System.Runtime.Serialization.DataMember]
        public DateTime Argument { get; set; }
        [System.Runtime.Serialization.DataMember]
        public TimeSpan Value { get; set; }
        //public string Value { get; set; }
        public Double ValueInMinutes { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Week { get; set; }
        //public Int64 Week { get; set; }
        //public DataPoint(DateTime argument, TimeSpan value, string week)
        //{
        //    Argument = argument;
        //    Value = value;
        //    Week = week;
        //}
        //public DataPoint(DateTime argument, TimeSpan value)
        //{
        //    Argument = argument;
        //    Value = value;
        //}
        public DataPoint(DateTime argument, Double value, string week)
        {
            Argument = argument;
            ValueInMinutes = value;
            Week = week;
        }
        public DataPoint(DateTime argument, Double valueInMinutes, string week, TimeSpan value)
        {
            Argument = argument;
            ValueInMinutes = valueInMinutes;
            Week = week;
            Value = value;
            //Value= string.Format("{0}:{1:mm}:{1:ss}", Math.Floor(value.TotalHours), value);
        }
        public DataPoint(DateTime argument, Double value)
        {
            Argument = argument;
            ValueInMinutes = value;
        }
    }

    public class YAxisLabelFormatterNew : IAxisLabelFormatter
    {
        public string GetAxisLabelText(object axisValue)
        {
            if (axisValue != null)
            {
                try
                {
                    var totalTimeValue = (TimeSpan)axisValue;
                    var c = System.Threading.Thread.CurrentThread.CurrentCulture;
                    if (totalTimeValue.Days > 0)
                        return ((totalTimeValue.Hours + (totalTimeValue.Days * 24)) + "H" + " " + totalTimeValue.Minutes + "M");
                    else
                        return (totalTimeValue.Hours + "H" + " " + totalTimeValue.Minutes + "M");
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }

            }
            else
            {
                return string.Empty;
            }
        }
    }
}
