using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class SplitSetting
    {
        public int IdPlant { get; set; }
        public string SplitLimit { get; set; }
        public int SplitRounding { get; set; }
    }
    public class SplitEmployeeAttendanceGridViewModel : INotifyPropertyChanged
    {
        #region Services
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
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

        #region Declaration
        SplitSetting selectedSplitSetting = new SplitSetting();
        int rounding_Block = 1;
        int rounding_Minutes = 1;
        private List<SplitSetting> splitSettingList;
        private List<GeosAppSetting> geosAppSettingList;
        EmployeeAttendance newSplitEmployeeAttendance;
        bool result;
        private ObservableCollection<EmployeeAttendance> existEmployeeAttendanceList;
        private ObservableCollection<EmployeeLeave> employeeLeaves;
        ObservableCollection<Task> tasks;
        private string windowHeader;
        private ObservableCollection<EmployeeAttendance> splitAttendanceList;
        private string fromDate;
        private string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";
        private bool isBusy;
        DateTime startDate;
        DateTime endDate;
        EmployeeAttendance selectedAttendance;
        DateTime? startTime;
        DateTime? endTime;
        private TimeSpan sTime;
        private TimeSpan eTime;
        EmployeeAttendance updatesplitEmployeeAttendance;
        DateTime? breakTime;
        #endregion

        #region Properties
        public bool IsSave = false;
        public int Rounding_Block
        {
            get
            {
                return rounding_Block;
            }

            set
            {
                rounding_Block = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Rounding_Block"));
            }
        }
        public int Rounding_Minutes
        {
            get
            {
                return rounding_Minutes;
            }

            set
            {
                rounding_Minutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Rounding_Minutes"));
            }
        }
        public SplitSetting SelectedSplitSetting
        {
            get
            {
                return selectedSplitSetting;
            }

            set
            {
                selectedSplitSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSplitSetting"));
            }
        }
        public List<SplitSetting> SplitSettingList
        {
            get
            {
                return splitSettingList;
            }

            set
            {
                splitSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SplitSettingList"));
            }
        }
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        public EmployeeAttendance UpdatesplitEmployeeAttendance
        {
            get { return updatesplitEmployeeAttendance; }
            set
            {
                updatesplitEmployeeAttendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatesplitEmployeeAttendance"));
            }
        }
        public EmployeeAttendance NewSplitEmployeeAttendance
        {
            get { return newSplitEmployeeAttendance; }
            set
            {
                newSplitEmployeeAttendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewSplitEmployeeAttendance"));
            }
        }
        public bool Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Result"));
            }
        }
        public ObservableCollection<EmployeeAttendance> ExistEmployeeAttendanceList
        {
            get { return existEmployeeAttendanceList; }
            set
            {
                existEmployeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeAttendanceList"));
            }
        }
        public ObservableCollection<EmployeeLeave> EmployeeLeaves
        {
            get { return employeeLeaves; }
            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
        }
        public TimeSpan STime
        {
            get { return sTime; }
            set
            {
                sTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("STime"));
            }
        }
        public TimeSpan ETime
        {
            get { return eTime; }
            set
            {
                eTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETime"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public ObservableCollection<EmployeeAttendance> SplitAttendanceList
        {
            get { return splitAttendanceList; }
            set
            {
                splitAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SplitAttendanceList"));
            }
        }
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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        public EmployeeAttendance SelectedAttendance
        {
            get { return selectedAttendance; }
            set
            {
                selectedAttendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendance"));
            }
        }
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
            }
        }

        public DateTime? BreakTime
        {
            get { return breakTime; }
            set
            {
                breakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BreakTime"));
            }
        }
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tasks"));
            }
        }
        bool deductBreak = true;
        public bool DeductBreak
        {
            get { return deductBreak; }
            set
            {
                deductBreak = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeductBreak"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand PropertyCheckBoxSelectedCommand { get; set; }
        public ICommand PropertyUnCheckBoxSelectedCommand { get; set; }
        #endregion

        #region Constructor
        public SplitEmployeeAttendanceGridViewModel()
        {
            try
            {
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);
                PropertyCheckBoxSelectedCommand = new RelayCommand(new Action<object>(PropertyCheckBoxSelectedCommandAction));
                PropertyUnCheckBoxSelectedCommand = new RelayCommand(new Action<object>(PropertyUnCheckBoxSelectedCommandAction));
                //[GEOS2-7973][30.05.2025][rdixit]
                //setDefaultPeriod();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;
                IsButtonStatus = 7;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SplitEmployeeAttendanceGridViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods
        private void PropertyUnCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedAttendance != null)
                {
                    SplitAttendanceList.Where(x => x.IdEmployeeAttendance == SelectedAttendance.IdEmployeeAttendance && x.IsNotRegularShift == false).ToList().ForEach(v => v.IsAttendanceChecked = false);
                }
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PropertyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PropertyCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedAttendance != null)
                {
                    SplitAttendanceList.Where(x => x.IdEmployeeAttendance == SelectedAttendance.IdEmployeeAttendance && x.IsNotRegularShift == false).ToList().ForEach(v => v.IsAttendanceChecked = true);
                }
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PropertyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region [rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        public void ConvertStringToSplitSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConvertStringToSplitSetting()...", category: Category.Info, priority: Priority.Low);
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("119,120");
                string[] compwise = GeosAppSettingList.FirstOrDefault(i => i.IdAppSetting == 119)?.DefaultValue.Split(';');
                foreach (var item in compwise)
                {
                    string trimmedItem = item.Trim();
                    string[] parts = trimmedItem.Split(',');
                    int idPlant = int.Parse(parts[0].Trim('(', ')').Trim());
                    string splitLimit = parts[1].Trim().ToLower();
                    int splitRounding = int.Parse(parts[2].Trim('(', ')').Trim());
                    if (SplitSettingList == null)
                        SplitSettingList = new List<SplitSetting>();
                    SplitSettingList.Add(new SplitSetting { IdPlant = idPlant, SplitLimit = splitLimit, SplitRounding = splitRounding });
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ConvertStringToSplitSetting() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ConvertStringToSplitSetting() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ConvertStringToSplitSetting()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        public bool CalculateWorkingHoursAndShift(DateTime WorkstartDate, DateTime WorkendDate, DateTime ShiftStart, DateTime ShiftEnd)
        {
            TimeSpan totalWorkingHours;
            bool exceedsShiftHours = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateWorkingHoursAndShift()...", category: Category.Info, priority: Priority.Low);
                if (ShiftStart < ShiftEnd)
                {
                    totalWorkingHours = WorkendDate - WorkstartDate;// Day shift
                }
                else
                {
                    // Night shift
                    TimeSpan hoursUntilShiftStart = ShiftStart - WorkstartDate;
                    TimeSpan hoursFromShiftEnd = WorkendDate - ShiftEnd;
                    totalWorkingHours = hoursUntilShiftStart + hoursFromShiftEnd;
                }

                TimeSpan shiftDuration = ShiftEnd - ShiftStart;

                if (totalWorkingHours > shiftDuration)
                {
                    exceedsShiftHours = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateWorkingHoursAndShift() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return exceedsShiftHours;
        }
        public void Init()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
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
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    ConvertStringToSplitSetting();
                    SplitAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    ObservableCollection<EmployeeAttendance> ExtraHourTimeEmployeesList = new ObservableCollection<EmployeeAttendance>();
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    foreach (var plant in plantOwners)
                    {
                        if (!string.IsNullOrEmpty(GeosAppSettingList.FirstOrDefault(i => i.IdAppSetting == 120).DefaultValue))
                        {
                            if (GeosAppSettingList.FirstOrDefault(i => i.IdAppSetting == 120).DefaultValue.Split(';').ToList().Any(p => Convert.ToInt32(p) == plant.IdCompany))
                            {
                                DeductBreak = false;
                            }
                        }
                        //Updated service by [GEOS2-5275][09.02.2024][rdixit]
                        //Service GetEmployeeSplitAttendance_V2480 updated with GetEmployeeSplitAttendance_V2500 by []
                        //Service GetEmployeeSplitAttendance_V2500 updated with GetEmployeeSplitAttendance_V2530 by [rdixit][06.06.2024][GEOS2-5786]
                        ExtraHourTimeEmployeesList = new ObservableCollection<EmployeeAttendance>(
                            HrmService.GetEmployeeSplitAttendance_V2530(plant.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod,
                            HrmCommon.Instance.ActiveEmployee.Organization,
                            HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission,
                            DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));

                        foreach (EmployeeAttendance item in ExtraHourTimeEmployeesList)
                        {
                            SelectedSplitSetting = SplitSettingList.FirstOrDefault(i => i.IdPlant == plant.IdCompany);
                            #region SelectedSplitSetting
                            if (SelectedSplitSetting != null)
                            {
                                if (SelectedSplitSetting.SplitLimit?.ToLower() == "minute")
                                    Rounding_Block = 1;
                                if (SelectedSplitSetting.SplitLimit?.ToLower() == "halfhour")
                                    Rounding_Block = 30;
                                if (SelectedSplitSetting.SplitLimit?.ToLower() == "quarter")
                                    Rounding_Block = 15;
                                if (SelectedSplitSetting.SplitLimit?.ToLower() == "hour")
                                    Rounding_Block = 60;
                                Rounding_Minutes = SelectedSplitSetting.SplitRounding;
                            }
                            #endregion
                            EndTime = item.EndDate;
                            StartTime = item.StartDate;
                            #region [rdixit][06.06.2024][GEOS2-5786]
                            if (item.StartDate.Date == item.EndDate.Date)
                            {
                                STime = (TimeSpan)item.StartTime;
                                ETime = (TimeSpan)item.EndTime;
                                DateTime shiftStartTime = GetShiftStartTime((int)item.StartDate.DayOfWeek, item.CompanyShift, 1);
                                DateTime shiftEndTime = GetShiftStartTime((int)item.StartDate.DayOfWeek, item.CompanyShift, 2);
                                DateTime shiftBreakTime = GetShiftStartTime((int)item.EndDate.DayOfWeek, item.CompanyShift, 3);
                                TimeSpan TotalDayTime_Break_Deducted = new TimeSpan();
                                if (DeductBreak)
                                    TotalDayTime_Break_Deducted = (item.TotalDayTime - shiftBreakTime.TimeOfDay) > new TimeSpan(0, 0, 0) ? item.TotalDayTime - shiftBreakTime.TimeOfDay : new TimeSpan(0, 0, 0);
                                else
                                    TotalDayTime_Break_Deducted = item.TotalDayTime;
                                TimeSpan Day_Excess_Time = new TimeSpan();

                                Day_Excess_Time = CalculateDayExcessTime(shiftStartTime.TimeOfDay, shiftEndTime.TimeOfDay, shiftBreakTime.TimeOfDay, TotalDayTime_Break_Deducted, DeductBreak);//TotalDayTime_Break_Deducted - (shiftEndTime.TimeOfDay - shiftStartTime.TimeOfDay - shiftBreakTime.TimeOfDay) < new TimeSpan(0, 0, 0) ? new TimeSpan(0, 0, 0) : TotalDayTime_Break_Deducted - (shiftEndTime.TimeOfDay - shiftStartTime.TimeOfDay - shiftBreakTime.TimeOfDay);

                                int Total_Hours = TotalDayTime_Break_Deducted.Hours;
                                int Total_Minute = TotalDayTime_Break_Deducted.Minutes;

                                double FinalTime = Total_Hours + Math.Floor((Total_Minute + Rounding_Minutes) / (double)Rounding_Block) * (Rounding_Block / 60.0);

                                bool WORKING_DAY = (shiftStartTime.TimeOfDay == TimeSpan.Zero && shiftEndTime.TimeOfDay == TimeSpan.Zero) ? false : true;
                                //#region
                                //if (WORKING_DAY)
                                //{
                                bool isCorrectShift = true;
                                double Total_Shift_Hours;

                                #region isCorrectShift
                                if ((shiftEndTime.TimeOfDay + shiftStartTime.TimeOfDay).Ticks == 0)
                                    isCorrectShift = true;
                                else
                                {
                                    if (item.RegisterNumber == 1)
                                    {
                                        double timeDifference = (STime - shiftStartTime.TimeOfDay).TotalMinutes / (STime.TotalMinutes + 0.01);
                                        if (timeDifference < -1 || timeDifference > 0.4)
                                            isCorrectShift = false;
                                        else
                                            isCorrectShift = true;
                                    }
                                    else
                                        isCorrectShift = true;
                                }
                                #endregion

                                #region totalHours

                                if (shiftEndTime < shiftStartTime)
                                    Total_Shift_Hours = ((shiftEndTime + TimeSpan.FromHours(24)) - shiftStartTime - shiftBreakTime.TimeOfDay).TotalHours;
                                else
                                    Total_Shift_Hours = ((shiftEndTime - shiftStartTime) - shiftBreakTime.TimeOfDay).TotalHours;
                                #endregion

                                bool Split = FinalTime > Total_Shift_Hours ? true : false;
                                if (Split)
                                {
                                    if (item.EndDate < item.StartDate)
                                    {
                                        item.RegisterTime = ((TimeSpan)item.EndTime + TimeSpan.FromHours(24) - (TimeSpan)item.StartTime);
                                    }
                                    else
                                    {
                                        item.RegisterTime = (TimeSpan)item.EndTime - (TimeSpan)item.StartTime;
                                    }

                                    TimeSpan Total_Split = Split ? TimeSpan.FromHours(FinalTime - Total_Shift_Hours) : new TimeSpan();

                                    TimeSpan Time_before_start_shift = (!WORKING_DAY || item.RegisterNumber != 1) ? TimeSpan.Zero : (shiftStartTime.TimeOfDay > STime ? shiftStartTime.TimeOfDay - STime : TimeSpan.Zero);

                                    TimeSpan Time_after_end_shift = TimeSpan.Zero;

                                    TimeSpan Total_Time_Accumulated_Before = TimeSpan.Zero;
                                    if (item.RegisterNumber == 1)
                                        Total_Time_Accumulated_Before = TimeSpan.Zero;
                                    else
                                    {
                                        List<TimeSpan> TotalRegisterTime = ExtraHourTimeEmployeesList.Where(i => i.IdEmployee == item.IdEmployee
                                        && i.StartDate.Date == item.StartDate.Date && i.RegisterNumber < item.RegisterNumber).Select(j => j.RegisterTime).ToList();
                                        if (TotalRegisterTime?.Count > 0)
                                        {
                                            foreach (var timeSpan in TotalRegisterTime)
                                            {
                                                Total_Time_Accumulated_Before += timeSpan;
                                            }
                                        }
                                    }
                                    TimeSpan TotalTimeAccumulatedafter = Total_Time_Accumulated_Before + item.RegisterTime;

                                    #region Time_after_end_shift
                                    if (!WORKING_DAY)
                                    {
                                        Time_after_end_shift = TimeSpan.Zero;
                                    }
                                    else if (item.TotalRegistersInTheDay != item.RegisterNumber)
                                    {
                                        Time_after_end_shift = TimeSpan.Zero;
                                    }
                                    else if (Time_before_start_shift >= Total_Split)
                                    {
                                        Time_after_end_shift = TimeSpan.Zero;
                                    }
                                    else if (TotalDayTime_Break_Deducted > shiftEndTime.TimeOfDay - shiftStartTime.TimeOfDay - shiftBreakTime.TimeOfDay)
                                    {
                                        Time_after_end_shift = TotalDayTime_Break_Deducted - (shiftEndTime.TimeOfDay - shiftStartTime.TimeOfDay - shiftBreakTime.TimeOfDay - Total_Time_Accumulated_Before) - Time_before_start_shift;
                                    }
                                    else if (EndTime > shiftEndTime)
                                    {
                                        Time_after_end_shift = (DateTime)EndTime - shiftEndTime;
                                    }
                                    else
                                    {
                                        Time_after_end_shift = TimeSpan.Zero;
                                    }

                                    #endregion

                                    ShiftAction re = CalculateShiftAction(WORKING_DAY, Split, isCorrectShift, item.RegisterNumber,
                                        Time_before_start_shift, Time_after_end_shift, Total_Split, item.TotalDayTime, Total_Shift_Hours, item.TotalRegistersInTheDay);

                                    #region Time1
                                    TimeSpan _StartTime1 = STime;
                                    TimeSpan _EndTime1 = CalculateSplit_EndTime1(re, ETime, Day_Excess_Time, Total_Split, _StartTime1, shiftEndTime.TimeOfDay, shiftStartTime.TimeOfDay,
                                        shiftBreakTime.TimeOfDay, item.RegisterNumber, DeductBreak, Total_Time_Accumulated_Before);

                                    if (_StartTime1 > TimeSpan.Zero && _EndTime1 > TimeSpan.Zero)
                                    {
                                        EmployeeAttendance t1 = (EmployeeAttendance)item.Clone();
                                        t1.StartDate = (DateTime)StartTime;
                                        t1.StartTime = _StartTime1;
                                        t1.EndDate = (DateTime)shiftStartTime;
                                        t1.EndTime = _EndTime1;

                                        switch (re)
                                        {
                                            case ShiftAction.SplitOvertimeAtStartOfDay:
                                            case ShiftAction.SplitIn3RegistersStartShiftEnd:
                                            case ShiftAction.ConvertAllToOvertime:
                                                t1.CompanyWork.Name = t1.ExtraHours.Value;
                                                t1.IsNotRegularShift = true;
                                                break;
                                            default:
                                                // Specific logic for the default case if any, else just break
                                                break;
                                        }

                                        SplitAttendanceList.Add((EmployeeAttendance)t1.Clone());
                                    }
                                    #endregion

                                    #region Time2
                                    TimeSpan _StartTime2 = CalculateSplit_StartTime2(re, Day_Excess_Time, Total_Split, _EndTime1, ETime);
                                    TimeSpan _EndTime2 = CalculateSplit_EndTime2(re, _StartTime2, Day_Excess_Time, Total_Split, ETime, shiftEndTime.TimeOfDay, shiftBreakTime.TimeOfDay, DeductBreak);

                                    if (_StartTime2 > TimeSpan.Zero && _EndTime2 > TimeSpan.Zero)
                                    {
                                        EmployeeAttendance t1 = (EmployeeAttendance)item.Clone();
                                        t1.StartDate = (DateTime)StartTime;
                                        t1.StartTime = _StartTime2;
                                        t1.EndDate = (DateTime)shiftStartTime;
                                        t1.EndTime = _EndTime2;
                                        switch (re)
                                        {
                                            case ShiftAction.SplitAfterTotalTimeShift:
                                            case ShiftAction.SplitOvertimeAtEndOfDay:
                                                t1.CompanyWork.Name = t1.ExtraHours.Value;
                                                t1.IsNotRegularShift = true;
                                                break;
                                            default:
                                                break;
                                        }

                                        SplitAttendanceList.Add((EmployeeAttendance)t1.Clone());
                                    }
                                    #endregion

                                    #region Time3
                                    TimeSpan _StartTime3 = CalculateSplit_StartTime3(re, _EndTime2);
                                    TimeSpan _EndTime3 = (re == ShiftAction.SplitIn3RegistersStartShiftEnd ? ETime : TimeSpan.Zero);
                                    if (_StartTime3 > TimeSpan.Zero && _EndTime3 > TimeSpan.Zero)
                                    {
                                        EmployeeAttendance t1 = (EmployeeAttendance)item.Clone();
                                        t1.StartDate = (DateTime)StartTime;
                                        t1.StartTime = _StartTime3;
                                        t1.EndDate = (DateTime)shiftStartTime;
                                        t1.EndTime = _EndTime3;
                                        if (re == ShiftAction.SplitIn3RegistersStartShiftEnd)
                                        {
                                            t1.CompanyWork.Name = t1.ExtraHours.Value;
                                            t1.IsNotRegularShift = true;
                                        }
                                        SplitAttendanceList.Add((EmployeeAttendance)t1.Clone());
                                    }

                                    #endregion
                                }
                                //}
                                //#endregion
                                //else
                                //{

                                //    EmployeeAttendance t1 = (EmployeeAttendance)item.Clone();
                                //    t1.StartDate = (DateTime)StartDate;
                                //    t1.StartTime = STime;
                                //    t1.EndDate = EndDate;
                                //    t1.EndTime = ETime;
                                //    t1.CompanyWork.Name = t1.ExtraHours.Value;
                                //    t1.IsNotRegularShift = true;
                                //    t1.IsNotWorkingDay = true;
                                //    SplitAttendanceList.Add((EmployeeAttendance)t1.Clone());
                                //}
                            }
                            #endregion
                        }
                    }
                }
                if (SplitAttendanceList.Count() == 0)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeSplitAttendanceEmptyGridWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public TimeSpan CalculateDayExcessTime(TimeSpan shiftStartTime, TimeSpan shiftEndTime, TimeSpan shiftBreakTime, TimeSpan totalDayTimeBreakDeducted, bool DeductBreak)
        {

            TimeSpan excessTime = new TimeSpan();
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateDayExcessTime()...", category: Category.Info, priority: Priority.Low);
                excessTime = totalDayTimeBreakDeducted - (shiftEndTime - shiftStartTime - shiftBreakTime);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateDayExcessTime() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return excessTime < TimeSpan.Zero ? TimeSpan.Zero : excessTime;
        }
        public TimeSpan CalculateSplit_StartTime2(ShiftAction splitStrategy, TimeSpan dayExcessTime, TimeSpan totalSplitTime, TimeSpan end1, TimeSpan checkOutTime)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateSplit_StartTime2()...", category: Category.Info, priority: Priority.Low);
                if (splitStrategy == ShiftAction.ConvertAllToOvertime && dayExcessTime > totalSplitTime)
                {
                    return end1;
                }

                switch (splitStrategy)
                {
                    case ShiftAction.KeepLikeOriginal:
                    case ShiftAction.ConvertAllToOvertime:
                        return TimeSpan.Zero;
                    case ShiftAction.SplitOvertimeAtStartOfDay:
                    case ShiftAction.SplitAfterTotalTimeShift:
                    case ShiftAction.SplitIn3RegistersStartShiftEnd:
                    case ShiftAction.SplitOvertimeAtEndOfDay:
                        return end1 - TimeSpan.FromDays(Math.Floor(end1.TotalDays));
                    default:
                        return checkOutTime;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateSplit_StartTime2() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return new TimeSpan();
            }
        }
        public TimeSpan CalculateSplit_EndTime1(ShiftAction splitStrategy, TimeSpan checkOutTime, TimeSpan dayExcessTime, TimeSpan totalSplitTime, TimeSpan start1, TimeSpan end,
            TimeSpan start, TimeSpan breakTime, int totalRegisters, bool DeductBreak, TimeSpan totalRegularTimeAccumulatedBefore)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateSplit_EndTime1()...", category: Category.Info, priority: Priority.Low);
                if (splitStrategy == ShiftAction.KeepLikeOriginal)
                {
                    return checkOutTime;
                }
                else if (splitStrategy == ShiftAction.ConvertAllToOvertime)
                {
                    return checkOutTime - dayExcessTime + totalSplitTime;
                }
                else if (splitStrategy == ShiftAction.SplitAfterTotalTimeShift)
                {
                    return start1 + (end < start ? end + TimeSpan.FromDays(1) : end) - start - breakTime;
                }
                else if (splitStrategy == ShiftAction.SplitOvertimeAtStartOfDay && totalRegisters == 1)
                {
                    return totalSplitTime + start1;
                }
                else if (splitStrategy == ShiftAction.SplitOvertimeAtStartOfDay)
                {
                    return TimeSpan.FromTicks(Math.Min(totalSplitTime.Ticks + start1.Ticks, start.Ticks + totalSplitTime.Ticks - dayExcessTime.Ticks));
                }
                else if (splitStrategy == ShiftAction.SplitIn3RegistersStartShiftEnd)
                {
                    return start;
                }
                else if (splitStrategy == ShiftAction.SplitOvertimeAtEndOfDay)
                {
                    TimeSpan timeDifference = end < start ? end + TimeSpan.FromDays(1) - start : end - start;
                    return start1 + timeDifference - ((!DeductBreak) ? breakTime : TimeSpan.Zero) - totalRegularTimeAccumulatedBefore + dayExcessTime - totalSplitTime;
                }
                else
                {
                    return checkOutTime;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateSplit_EndTime1() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return new TimeSpan();
            }
        }
        public TimeSpan CalculateSplit_EndTime2(ShiftAction splitStrategy, TimeSpan start2, TimeSpan dayExcessTime, TimeSpan totalSplitTime, TimeSpan checkOutTime, TimeSpan end, TimeSpan breakTime, bool DeductBreak)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateSplit_EndTime2()...", category: Category.Info, priority: Priority.Low);
                if (splitStrategy == ShiftAction.KeepLikeOriginal)
                {
                    return TimeSpan.Zero;
                }
                else if (splitStrategy == ShiftAction.ConvertAllToOvertime && dayExcessTime > totalSplitTime)
                {
                    return start2 + (dayExcessTime - totalSplitTime);
                }
                else if (splitStrategy == ShiftAction.ConvertAllToOvertime)
                {
                    return TimeSpan.Zero;
                }
                else if (splitStrategy == ShiftAction.SplitOvertimeAtStartOfDay)
                {
                    return checkOutTime;
                }
                else if (splitStrategy == ShiftAction.SplitIn3RegistersStartShiftEnd)
                {
                    return end - ((!DeductBreak) ? breakTime : TimeSpan.Zero) + dayExcessTime - totalSplitTime;
                }
                else if (splitStrategy == ShiftAction.SplitOvertimeAtEndOfDay)
                {
                    return checkOutTime;
                }
                else
                {
                    return checkOutTime;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateSplit_EndTime2() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return new TimeSpan();
            }
        }
        public TimeSpan CalculateSplit_StartTime3(ShiftAction splitStrategy, TimeSpan end2)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateSplit_StartTime3()...", category: Category.Info, priority: Priority.Low);
                if (splitStrategy == ShiftAction.SplitIn3RegistersStartShiftEnd)
                {
                    return end2;
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateSplit_StartTime3() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return new TimeSpan();
            }
        }
        public ShiftAction CalculateShiftAction(bool workingDay, bool split, bool correctShift, int registerNumber, TimeSpan timeBeforeStartShift,
            TimeSpan timeAfterEndShift, TimeSpan totalSplitTime, TimeSpan totalTimeRegister, double totalShiftHours, double totalRegisters)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateShiftAction()...", category: Category.Info, priority: Priority.Low);
                if (!workingDay)
                {
                    return ShiftAction.ConvertAllToOvertime;
                }
                else if (!split)
                {
                    return ShiftAction.KeepLikeOriginal;
                }
                else if (!correctShift)
                {
                    return ShiftAction.SplitAfterTotalTimeShift;
                }
                else if (totalRegisters == 1 && timeBeforeStartShift == TimeSpan.Zero)
                {
                    return ShiftAction.SplitOvertimeAtEndOfDay;
                }
                else if (totalRegisters == 1 && timeAfterEndShift == TimeSpan.Zero)
                {
                    return ShiftAction.SplitOvertimeAtStartOfDay;
                }
                else if (totalRegisters == 1)
                {
                    return ShiftAction.SplitIn3RegistersStartShiftEnd;
                }
                else if (registerNumber < totalRegisters && timeBeforeStartShift == TimeSpan.Zero)
                {
                    return ShiftAction.KeepLikeOriginal;
                }
                else if (registerNumber < totalRegisters && totalSplitTime < timeBeforeStartShift && totalTimeRegister.TotalHours < totalShiftHours * 24)
                {
                    return ShiftAction.SplitOvertimeAtStartOfDay;
                }
                else if (registerNumber < totalRegisters && totalSplitTime < timeBeforeStartShift)
                {
                    return ShiftAction.SplitIn3RegistersStartShiftEnd;
                }
                else if (registerNumber == totalRegisters && timeAfterEndShift == TimeSpan.Zero)
                {
                    return ShiftAction.KeepLikeOriginal;
                }
                else if (registerNumber < totalRegisters && totalTimeRegister.TotalHours < totalShiftHours * 24)
                {
                    return ShiftAction.KeepLikeOriginal;
                }
                else
                {
                    return ShiftAction.SplitOvertimeAtEndOfDay;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CalculateShiftAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return ShiftAction.KeepLikeOriginal;
            }
        }
        private DateTime GetShiftStartTime(int dayOfWeek, CompanyShift selectedCompanyShift, int time)
        {
            try
            {
                BreakTime = new DateTime();
                GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()...", category: Category.Info, priority: Priority.Low);
                switch (dayOfWeek)
                {
                    case 0:
                        TimeSpan SunStartTime = selectedCompanyShift.SunStartTime;
                        TimeSpan SunEndTime = selectedCompanyShift.SunEndTime;
                        TimeSpan SunBreakTime = selectedCompanyShift.SunBreakTime;
                        #region Commented Code
                        /*
                        if (selectedCompanyShift.SunStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.SunEndTime == new TimeSpan(0, 0, 0))
                        {
                            if (!(selectedCompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.SunEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = selectedCompanyShift.MonStartTime;
                                SunEndTime = selectedCompanyShift.MonEndTime;

                            }
                            else if (!(selectedCompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = selectedCompanyShift.TueStartTime;
                                SunEndTime = selectedCompanyShift.TueEndTime;

                            }
                            else if (!(selectedCompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = selectedCompanyShift.WedStartTime;
                                SunEndTime = selectedCompanyShift.WedEndTime;

                            }
                            else if (!(selectedCompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = selectedCompanyShift.ThuStartTime;
                                SunEndTime = selectedCompanyShift.ThuEndTime;

                            }
                            else if (!(selectedCompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = selectedCompanyShift.FriStartTime;
                                SunEndTime = selectedCompanyShift.FriStartTime;
                            }
                            if (time == 1)
                                return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                            else if (time == 2)
                                return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);
                            else
                                return BreakTime.Value.Date.AddHours(SunBreakTime.Hours).AddMinutes(SunBreakTime.Minutes);
                        }
                        else
                        {
                            if (time == 1)
                                return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                            else if (time == 2)
                                return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);
                            else
                                return BreakTime.Value.Date.AddHours(SunBreakTime.Hours).AddMinutes(SunBreakTime.Minutes);
                        }
                        */
                        #endregion
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(SunBreakTime.Hours).AddMinutes(SunBreakTime.Minutes);

                    case 1:
                        TimeSpan MonStartTime = selectedCompanyShift.MonStartTime;
                        TimeSpan MonEndTime = selectedCompanyShift.MonEndTime;
                        TimeSpan MonBreakTime = selectedCompanyShift.MonBreakTime;
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(MonStartTime.Hours).AddMinutes(MonStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(MonEndTime.Hours).AddMinutes(MonEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(MonBreakTime.Hours).AddMinutes(MonBreakTime.Minutes);

                    case 2:
                        TimeSpan TueStartTime = selectedCompanyShift.TueStartTime;
                        TimeSpan TueEndTime = selectedCompanyShift.TueEndTime;
                        TimeSpan TueBreakTime = selectedCompanyShift.TueBreakTime;
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(TueStartTime.Hours).AddMinutes(TueStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(TueEndTime.Hours).AddMinutes(TueEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(TueBreakTime.Hours).AddMinutes(TueBreakTime.Minutes);

                    case 3:
                        TimeSpan WedStartTime = selectedCompanyShift.WedStartTime;
                        TimeSpan WedEndTime = selectedCompanyShift.WedEndTime;
                        TimeSpan WedBreakTime = selectedCompanyShift.WedBreakTime;
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(WedStartTime.Hours).AddMinutes(WedStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(WedEndTime.Hours).AddMinutes(WedEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(WedBreakTime.Hours).AddMinutes(WedBreakTime.Minutes);

                    case 4:
                        TimeSpan ThuStartTime = selectedCompanyShift.ThuStartTime;
                        TimeSpan ThuEndTime = selectedCompanyShift.ThuEndTime;
                        TimeSpan ThuBreakTime = selectedCompanyShift.ThuBreakTime;
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(ThuStartTime.Hours).AddMinutes(ThuStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(ThuEndTime.Hours).AddMinutes(ThuEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(ThuBreakTime.Hours).AddMinutes(ThuBreakTime.Minutes);

                    case 5:
                        TimeSpan FriStartTime = selectedCompanyShift.FriStartTime;
                        TimeSpan FriEndTime = selectedCompanyShift.FriEndTime;
                        TimeSpan FriBreakTime = selectedCompanyShift.FriBreakTime;
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(FriStartTime.Hours).AddMinutes(FriStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(FriEndTime.Hours).AddMinutes(FriEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(FriBreakTime.Hours).AddMinutes(FriBreakTime.Minutes);

                    case 6:
                        TimeSpan SatStartTime = selectedCompanyShift.SatStartTime;
                        TimeSpan SatEndTime = selectedCompanyShift.SatEndTime;
                        TimeSpan SatBreakTime = selectedCompanyShift.SatBreakTime;
                        #region Commented
                        /*
                        if (selectedCompanyShift.SatStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.SatEndTime == new TimeSpan(0, 0, 0))
                        {
                            if (!(selectedCompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.SunEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = selectedCompanyShift.MonStartTime;
                                SatEndTime = selectedCompanyShift.MonEndTime;

                            }
                            else if (!(selectedCompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = selectedCompanyShift.TueStartTime;
                                SatEndTime = selectedCompanyShift.TueEndTime;

                            }
                            else if (!(selectedCompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = selectedCompanyShift.WedStartTime;
                                SatEndTime = selectedCompanyShift.WedEndTime;

                            }
                            else if (!(selectedCompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = selectedCompanyShift.ThuStartTime;
                                SatEndTime = selectedCompanyShift.ThuEndTime;
                            }
                            else if (!(selectedCompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && selectedCompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = selectedCompanyShift.FriStartTime;
                                SatEndTime = selectedCompanyShift.FriEndTime;
                            }
                            if (time == 1)
                                return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                            else if (time == 2)
                                return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);
                            else
                                return BreakTime.Value.Date.AddHours(SatBreakTime.Hours).AddMinutes(SatBreakTime.Minutes);
                        }
                        else
                        {
                            if (time == 1)
                                return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                            else if (time == 2)
                                return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);
                            else
                                return BreakTime.Value.Date.AddHours(SatBreakTime.Hours).AddMinutes(SatBreakTime.Minutes);
                        }
                        */
                        #endregion
                        if (time == 1)
                            return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                        else if (time == 2)
                            return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);
                        else
                            return BreakTime.Value.Date.AddHours(SatBreakTime.Hours).AddMinutes(SatBreakTime.Minutes);
                    default:
                        return (DateTime)StartTime;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new DateTime();
            }
        }
        #endregion
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

                Init();

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
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindow()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                var SelectedSplitAttendanceList = SplitAttendanceList?.Where(i => i.IsAttendanceChecked).ToList();
                if (SelectedSplitAttendanceList?.Count > 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["EmployeeSplitAttendanceAcceptButtonMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        bool IsLeave = true;
                        bool IsAttendance = true;
                        foreach (var item in SelectedSplitAttendanceList)
                        {
                            #region [GEOS2-5275][09.02.2024][rdixit]
                            var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(i => i.IdCompany));
                            var SelectedEmployeeListJoined = string.Join(",", SplitAttendanceList?.Select(i => i.IdEmployee)?.Distinct().ToArray());

                            #region Leaves check 
                            if (EmployeeLeaves == null || EmployeeLeaves.Count == 0)
                            {
                                EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
                                EmployeeLeaves.AddRange(HrmService.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, item.StartDate, item.EndDate, SelectedEmployeeListJoined));
                            }


                            var ExistEmpLeaveList = EmployeeLeaves.Where(x => x.IdEmployee == item.IdEmployee && x.StartDate.Value.Date == item.StartDate.Date).OrderBy(x => x.StartDate).ToList();
                            var _StartDate = item.StartDate.Date.AddHours(item.StartTime.Value.Hours).AddMinutes(item.StartTime.Value.Minutes);
                            var _EndDate = item.EndDate.Date.AddHours(item.EndTime.Value.Hours).AddMinutes(item.EndTime.Value.Minutes);
                            for (int i = 0; i < ExistEmpLeaveList.Count; i++)
                            {
                                if (ExistEmpLeaveList[i].IsAllDayEvent == 1)
                                {
                                    IsLeave = false;
                                    break;
                                }

                                if (i == 0)
                                {
                                    if (_StartDate < ExistEmpLeaveList[i].StartDate && _EndDate <= ExistEmpLeaveList[i].StartDate)
                                    {
                                        IsLeave = true;
                                        break;
                                    }

                                    if (ExistEmpLeaveList.Count == 1)
                                    {
                                        if (_StartDate >= ExistEmpLeaveList[i].EndDate && _EndDate > ExistEmpLeaveList[i].EndDate)
                                        {
                                            IsLeave = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (i <= ExistEmpLeaveList.Count - 1)
                                    {
                                        if (_StartDate >= ExistEmpLeaveList[i - 1].EndDate && _EndDate <= ExistEmpLeaveList[i].StartDate)
                                        {
                                            IsLeave = true;
                                            break;
                                        }
                                        else if (i == ExistEmpLeaveList.Count - 1)
                                        {
                                            if (_StartDate >= ExistEmpLeaveList[i].EndDate && _EndDate > ExistEmpLeaveList[i].EndDate)
                                            {
                                                IsLeave = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                IsLeave = false;
                            }


                            if (IsLeave == false)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            #endregion

                            #region Attendance check 
                            if (ExistEmployeeAttendanceList == null || ExistEmployeeAttendanceList.Count == 0)
                            {
                                ExistEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                                ExistEmployeeAttendanceList.AddRange(HrmService.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, item.StartDate, item.EndDate, SelectedEmployeeListJoined));
                            }

                            List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
                            ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == item.IdEmployee && x.StartDate.Date == item.StartDate.Date && x.IdEmployeeAttendance != item.IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();

                            var _StartDate1 = item.StartDate.Date.AddHours(item.StartTime.Value.Hours).AddMinutes(item.StartTime.Value.Minutes);
                            var _EndDate1 = item.EndDate.Date.AddHours(item.EndTime.Value.Hours).AddMinutes(item.EndTime.Value.Minutes);
                            for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
                            {

                                if (i == 0)
                                {
                                    if (_StartDate1 < ExistEmpAttendanceList[i].StartDate && _EndDate1 <= ExistEmpAttendanceList[i].StartDate)
                                    {
                                        IsAttendance = true;
                                        break;
                                    }
                                    if (ExistEmpAttendanceList.Count == 1)
                                    {
                                        if (_StartDate1 >= ExistEmpAttendanceList[i].EndDate && _EndDate1 > ExistEmpAttendanceList[i].EndDate)
                                        {
                                            IsAttendance = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (i <= ExistEmpAttendanceList.Count - 1)
                                    {
                                        if (_StartDate1 >= ExistEmpAttendanceList[i - 1].EndDate && _EndDate1 <= ExistEmpAttendanceList[i].StartDate)
                                        {
                                            IsAttendance = true;
                                            break;
                                        }
                                        else if (i == ExistEmpAttendanceList.Count - 1)
                                        {
                                            if (_StartDate1 >= ExistEmpAttendanceList[i].EndDate && _EndDate1 > ExistEmpAttendanceList[i].StartDate)
                                            {
                                                IsAttendance = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                IsAttendance = false;
                            }


                            if (IsAttendance == false)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            var test = SplitAttendanceList.Where(j => j.IsAttendanceChecked).ToList();
                            #region 
                            //for (int i = 0; i < test.ToList().Count - 1; i++)
                            //{
                            //    var _TaskStartTime = test[i].StartDate.AddHours(test[i].StartTime.Value.Hours).AddMinutes(test[i].StartTime.Value.Minutes);
                            //    var _TaskEndTime = test[i].EndDate.AddHours(test[i].EndTime.Value.Hours).AddMinutes(test[i].EndTime.Value.Minutes);

                            //    var _StartTime = test[i + 1].StartDate.AddHours(test[i + 1].StartTime.Value.Hours).AddMinutes(test[i + 1].StartTime.Value.Minutes);
                            //    var _EndTime = test[i + 1].EndDate.AddHours(test[i + 1].EndTime.Value.Hours).AddMinutes(test[i + 1].EndTime.Value.Minutes);

                            //    if (_TaskStartTime == _StartTime && _TaskEndTime == _EndTime)
                            //    {
                            //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            //        return;
                            //    }
                            //    else if (_StartTime < _TaskStartTime && test[i + 1].StartDate < _TaskEndTime)
                            //    {
                            //        if (_EndTime > _TaskStartTime)
                            //        {
                            //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            //            return;
                            //        }
                            //    }
                            //    else if (_StartTime < _TaskEndTime)
                            //    {
                            //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            //        return;
                            //    }

                            //}
                            #endregion

                            #endregion

                            #region Save
                            if (item.IsNotRegularShift == true && item.IsNotWorkingDay == false)
                            {
                                NewSplitEmployeeAttendance = new EmployeeAttendance()
                                {
                                    Employee = item.Employee,  // EmployeeListFinal[SelectedIndexForEmployee],
                                    IdEmployee = item.IdEmployee,
                                    StartDate = item.StartDate.Date.AddHours(item.StartTime.Value.Hours).AddMinutes(item.StartTime.Value.Minutes),
                                    EndDate = item.EndDate.Date.AddHours(item.EndTime.Value.Hours).AddMinutes(item.EndTime.Value.Minutes),

                                    IdCompanyWork = item.IdCompanyWork,
                                    IdCompanyShift = item.CompanyShift.IdCompanyShift,
                                    AccountingDate = item.AccountingDate,
                                };
                                NewSplitEmployeeAttendance.IsManual = 1;
                                NewSplitEmployeeAttendance.Creator = GeosApplication.Instance.ActiveUser.IdUser;
                                NewSplitEmployeeAttendance.CreationDate = GeosApplication.Instance.ServerDateTime;
                                NewSplitEmployeeAttendance.Employee.CompanyShift = item.CompanyShift;
                                if (item.ExtraHours?.IdLookupValue == 1897)//Convert to Overtime
                                {
                                    NewSplitEmployeeAttendance.IdCompanyWork = 188;
                                }
                                else
                                {
                                    NewSplitEmployeeAttendance.IdCompanyWork = 305;
                                }
                                NewSplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == NewSplitEmployeeAttendance.IdCompanyWork));

                                NewSplitEmployeeAttendance.Employee.EmployeeChangelogs = new List<EmployeeChangelog>();

                                NewSplitEmployeeAttendance.Employee.EmployeeChangelogs.Add(
                                     new EmployeeChangelog()
                                     {
                                         IdEmployee = item.IdEmployee,
                                         ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                         ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                         ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("SplitEmployeesAttendanceChangeLog").ToString(),
                                         item.CompanyWork.Name, item.AccountingDate.Value.ToShortDateString(), NewSplitEmployeeAttendance.StartDate.ToShortTimeString(), NewSplitEmployeeAttendance.EndDate.ToShortTimeString())
                                     });
                                EmployeeAttendance addEmployeeAttendance = HrmService.SplittedEmployeeAttendanceAdd(NewSplitEmployeeAttendance);

                            }
                            else if (item.IsNotRegularShift == true && item.IsNotWorkingDay == true)
                            {
                                UpdatesplitEmployeeAttendance = new EmployeeAttendance()
                                {
                                    Employee = item.Employee,
                                    IdEmployee = item.IdEmployee,
                                    StartDate = item.StartDate.Date.AddHours(item.StartTime.Value.Hours).AddMinutes(item.StartTime.Value.Minutes),
                                    EndDate = item.EndDate.Date.AddHours(item.EndTime.Value.Hours).AddMinutes(item.EndTime.Value.Minutes),
                                    IdEmployeeAttendance = item.IdEmployeeAttendance,
                                    IdCompanyWork = item.IdCompanyWork,
                                    IdCompanyShift = item.CompanyShift.IdCompanyShift,
                                    AccountingDate = item.AccountingDate,
                                    Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                                    ModificationDate = GeosApplication.Instance.ServerDateTime
                                };
								// [nsatpute][03-10-2024][GEOS2-6451]
                                if (ExistEmployeeAttendanceList.Any(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance))
                                {
                                    UpdatesplitEmployeeAttendance.IsManual = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance).FirstOrDefault().IsManual;
                                    UpdatesplitEmployeeAttendance.IsMobileApiAttendance = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance).FirstOrDefault().IsMobileApiAttendance;
                                }
                                UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));

                                UpdatesplitEmployeeAttendance.Employee.EmployeeChangelogs = new List<EmployeeChangelog>();

                                if (item.ExtraHours?.IdLookupValue == 1897)//Convert to Overtime
                                {
                                    UpdatesplitEmployeeAttendance.IdCompanyWork = 188;
                                }
                                else
                                {
                                    UpdatesplitEmployeeAttendance.IdCompanyWork = 305;
                                }

                                UpdatesplitEmployeeAttendance.Employee.EmployeeChangelogs.Add(
                                  new EmployeeChangelog()
                                  {
                                      IdEmployee = item.IdEmployee,
                                      ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                      ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                      ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("SplitEmployeesAttendanceChangeLog").ToString(),
                                      item.CompanyWork.Name, item.AccountingDate.Value.ToShortDateString(), UpdatesplitEmployeeAttendance.StartDate.ToShortTimeString(), UpdatesplitEmployeeAttendance.EndDate.ToShortTimeString())
                                  });

                                UpdatesplitEmployeeAttendance.Employee.CompanyShift = item.CompanyShift;
                                UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));
                                Result = HrmService.SplittedEmployeeAttendanceUpdate(UpdatesplitEmployeeAttendance);
                                UpdatesplitEmployeeAttendance.Employee.TotalWorkedHours = (UpdatesplitEmployeeAttendance.EndDate - UpdatesplitEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                            }
                            else
                            {
                                UpdatesplitEmployeeAttendance = new EmployeeAttendance()
                                {
                                    Employee = item.Employee,
                                    IdEmployee = item.IdEmployee,
                                    StartDate = item.StartDate.Date.AddHours(item.StartTime.Value.Hours).AddMinutes(item.StartTime.Value.Minutes),
                                    EndDate = item.EndDate.Date.AddHours(item.EndTime.Value.Hours).AddMinutes(item.EndTime.Value.Minutes),
                                    IdEmployeeAttendance = item.IdEmployeeAttendance,
                                    IdCompanyWork = item.IdCompanyWork,
                                    IdCompanyShift = item.CompanyShift.IdCompanyShift,
                                    AccountingDate = item.AccountingDate,
                                    Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                                    ModificationDate = GeosApplication.Instance.ServerDateTime
                                };
								// [nsatpute][03-10-2024][GEOS2-6451]
                                if (ExistEmployeeAttendanceList.Any(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance))
                                {
                                    UpdatesplitEmployeeAttendance.IsManual = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance).FirstOrDefault().IsManual;
                                    UpdatesplitEmployeeAttendance.IsMobileApiAttendance = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == item.IdEmployeeAttendance).FirstOrDefault().IsMobileApiAttendance;
                                }
                                UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));

                                UpdatesplitEmployeeAttendance.Employee.EmployeeChangelogs = new List<EmployeeChangelog>();
                                if (UpdatesplitEmployeeAttendance.IsNotWorkingDay)
                                {
                                    if (item.ExtraHours?.IdLookupValue == 1897)//Convert to Overtime
                                    {
                                        UpdatesplitEmployeeAttendance.IdCompanyWork = 188;
                                    }
                                    else
                                    {
                                        UpdatesplitEmployeeAttendance.IdCompanyWork = 305;
                                    }
                                }
                                UpdatesplitEmployeeAttendance.Employee.EmployeeChangelogs.Add(
                                  new EmployeeChangelog()
                                  {
                                      IdEmployee = item.IdEmployee,
                                      ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                      ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                      ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("SplitEmployeesAttendanceChangeLog").ToString(),
                                      item.CompanyWork.Name, item.AccountingDate.Value.ToShortDateString(), UpdatesplitEmployeeAttendance.StartDate.ToShortTimeString(), UpdatesplitEmployeeAttendance.EndDate.ToShortTimeString())
                                  });

                                UpdatesplitEmployeeAttendance.Employee.CompanyShift = item.CompanyShift;
                                UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));
                                Result = HrmService.SplittedEmployeeAttendanceUpdate(UpdatesplitEmployeeAttendance);
                                UpdatesplitEmployeeAttendance.Employee.TotalWorkedHours = (UpdatesplitEmployeeAttendance.EndDate - UpdatesplitEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                            }
                            #endregion

                            #endregion
                        }
                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SplitEmployeesAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public CompanyWork GetCompanyWork(LookupValue obj)
        {
            CompanyWork companywork = new CompanyWork();
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCompanyWork()...", category: Category.Info, priority: Priority.Low);
                companywork.IdCompanyWork = obj.IdLookupValue;
                companywork.Name = obj.Value;
                companywork.HtmlColor = obj.HtmlColor;
                GeosApplication.Instance.Logger.Log("Method GetCompanyWork()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetCompanyWork()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return companywork;
        }
        public enum ShiftAction
        {
            ConvertAllToOvertime,
            KeepLikeOriginal,
            SplitAfterTotalTimeShift,
            SplitOvertimeAtEndOfDay,
            SplitOvertimeAtStartOfDay,
            SplitIn3RegistersStartShiftEnd
        }
        #endregion
    }
}
