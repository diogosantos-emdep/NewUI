using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.CommonClass;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class ReadImportedAttendanceViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //GEOS2-48 sprint-60 Add startdate and enddate filter in import attendance [adadibathina]
        //GEOS2-248  Import attendance support for date custom format [adadibathina]
        //GEOS2-49 Smart import attendance [adadibathina]
        //[004][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        #endregion

        #region Services      
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion 

        #region Declaration
        private bool isSave;
        private bool isBusy;
        private DateTime filterStartDate;
        private DateTime filterEndDate;
        private ObservableCollection<ImportAttendance> employeeAttendanceData;
        private ObservableCollection<EmployeeAttandance> attendanceSheetData;
        private ObservableCollection<LookupValue> attendanceTypeList;
        private EmployeeAttandance selectedAttendance = null;
        private List<EmployeeShift> employeeShiftList;

        #endregion

        #region Properties

        private DateTime AttendanceInTolerance { get; set; }
        private DateTime AttendanceOutTolerance { get; set; }

        private List<IDictionary<string, object>> SheetData { get; set; }
        private List<EmployeeAttendance> EmployeeAttendanceList { get; set; }
        public List<EmployeeAttendance> EmpAddedAttendanceList { get; set; }

        public EmployeeAttandance SelectedAttendance
        {
            get { return selectedAttendance; }
            set
            {
                selectedAttendance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendance"));
            }
        }

        public DateTime FilterStartDate
        {
            get { return filterStartDate; }
            set
            {
                filterStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStartDate"));
            }
        }

        public DateTime FilterEndDate
        {
            get { return filterEndDate; }
            set
            {
                filterEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterEndDate"));
            }
        }

        public ObservableCollection<LookupValue> AttendanceTypeList
        {
            get { return attendanceTypeList; }
            set
            {
                attendanceTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceTypeList"));
            }
        }
        public string MappingFailMessage { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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

        public ObservableCollection<EmployeeAttandance> AttendanceSheetData
        {
            get { return attendanceSheetData; }
            set
            {
                attendanceSheetData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceSheetData"));
            }
        }

        public ObservableCollection<ImportAttendance> EmployeeAttendanceData
        {
            get { return employeeAttendanceData; }
            set
            {
                employeeAttendanceData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceData"));
            }
        }

        public string TimeEditMask { get; set; }
        public List<EmployeeAttendance> NewEmployeeAttendanceList { get; set; }
        //[004]added
        public List<EmployeeShift> EmployeeShiftList
        {
            get { return employeeShiftList; }
            set { employeeShiftList = value; OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList")); }
        }
        public GeosAppSetting Setting { get; set; }

        #endregion

        #region Events

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

        #region Public Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand DeleteAttendanceRowCommand { get; set; }
        public ICommand InOutChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// [001][skhade][2019-09-13][GEOS2-31] Change the time format in attendance import option
        /// </summary>
        public ReadImportedAttendanceViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReadImportedAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                DeleteAttendanceRowCommand = new DelegateCommand<object>(DeleteAttendanceRowCommandAction);
                InOutChangedCommand = new DelegateCommand<CellValueChangedEventArgs>(InOutChangedCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillAttendanceTypeList();
                //TimeEditMask = "HH:mm:ss";
                TimeEditMask = "HH:mm"; // 001 CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                GeosApplication.Instance.Logger.Log("Method ReadImportedAttendanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ReadImportedAttendanceViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Init Method
        /// [001][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        /// [002][cpatil][2020-04-02][GEOS2-2151] Attendance not read
        /// </summary>
        /// <param name="colData"></param>
        /// <param name="sheetData">Imported data</param>
        /// <param name="employeeAttendanceList">total attendance to compare</param>
        public void Init(ObservableCollection<ImportAttendance> colData, List<IDictionary<string, object>> sheetData, List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  Init()...", category: Category.Info, priority: Priority.Low);
                EmployeeAttendanceData = new ObservableCollection<ImportAttendance>(colData);
                AttendanceSheetData = new ObservableCollection<EmployeeAttandance>();
                SheetData = sheetData;
                EmployeeAttendanceList = employeeAttendanceList;
                ObservableCollection<EmployeeAttandance> listObj = new ObservableCollection<EmployeeAttandance>();
                MappingFailMessage = string.Empty;

                EmployeeShiftList = new List<EmployeeShift>();

                GetTolerance();

                #region sorting and removing unwanted data

                //List<IDictionary<string, object>> sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeClockTimeID"]))).ThenBy(row => Convert.ToDateTime(row["Date"])).ThenBy(row => Convert.ToDateTime(row["Time"])).ToList();
                List<IDictionary<string, object>> sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeClockTimeID"]))).ThenBy(row => Convert.ToDateTime(row["Converted_Date_Time"])).ToList();

                #endregion

                foreach (IDictionary<string, object> item2 in sortedSheetData)
                {
                    EmployeeAttandance obj = new EmployeeAttandance();

                    foreach (var item3 in item2)
                    {

                        if (item3.Key == "EmployeeClockTimeID")
                        {
                            obj.EmployeeClockTimeID = item3.Value.ToString();
                            obj.Employee = item2.FirstOrDefault(X => X.Key == "EmployeeName").Value.ToString();
                        }
                        else if (item3.Key == "Date")
                        {
                            try
                            {
                                //obj.Date = Convert.ToDateTime(item3.Value.ToString()).Date;
                                obj.Date = Convert.ToDateTime(item2["Converted_Date_Time"]).Date;

                            }
                            catch (Exception)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("AttendanceFieldMappingFailed").ToString());
                                break;
                            }
                        }
                        else if (item3.Key == "Time")
                        {
                            try
                            {
                                //obj.Time = default(DateTime).Add(Convert.ToDateTime(item3.Value.ToString()).TimeOfDay);
                                obj.Time = Convert.ToDateTime(item2["Converted_Date_Time"]);
                            }
                            catch (Exception)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("AttendanceFieldMappingFailed").ToString());
                                break;
                            }
                        }
                        //[001]added
                        else if (item3.Key == "Employee")
                        {
                            try
                            {

                                Employee employee = (Employee)item3.Value;
                                obj.idEmployee = employee.IdEmployee;

                               

                            }
                            catch (Exception)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("AttendanceFieldMappingFailed").ToString());
                                break;
                            }
                        }

                    }
                    obj.Type = AttendanceTypeList[0];
                    listObj.Add(obj);
                }

                AttendanceSheetData = listObj;
                Setting = WorkbenchService.GetGeosAppSettings(32);
                //[001]added
                if (Convert.ToInt32(Setting.DefaultValue) == 1)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                    var EmpDocumentNumbers = string.Join(",", AttendanceSheetData.Select(i => i.EmployeeClockTimeID).Distinct());

                    List<Employee> Employees = new List<Employee>();

                    List<EmployeeShift> EmployeeNightShift = new List<EmployeeShift>();

                    List<Company> CompanyScheduleList = HrmService.GetCompaniesDetailByIds(plantOwnersIds);
                    List<CompanyShift> CompanyShiftList = CompanyScheduleList.ToList().SelectMany(comp => comp.CompanySchedules.SelectMany(csche => csche.CompanyShifts)).ToList().Where(cshift => cshift.IsNightShift == 1).ToList();

                    if (CompanyShiftList.Count > 0)
                    {
                        // [002] Changed service method GetEmpDtlByEmpDocNoAndPeriod_V2039 to GetEmpDtlByEmpDocNoAndPeriod_V2041
                        // Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2036(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                        Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2041(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);

                        var EmployeeIds = string.Join(",", Employees.Select(i => i.IdEmployee).Distinct());

                        List<Employee> EmployeeShiftsList = HrmService.GetAllEmployeeShifts(EmployeeIds);

                        EmployeeNightShift = EmployeeShiftsList.SelectMany(es => es.EmployeeShifts.Where(x => x.CompanyShift.IsNightShift == 1)).ToList();

                        EmployeeShiftList = EmployeeShiftsList.SelectMany(es => es.EmployeeShifts).ToList();

                        if (EmployeeNightShift.Count > 0)
                        {
                            SetNightShiftInOut(EmployeeShiftList);
                        }
                        else
                        {
                            SetInOut();
                        }
                    }
                    else
                    {
                        SetInOut();
                    }
                }
                else
                {
                    SetInOut();
                }
                //End
                AttendanceSheetData = new ObservableCollection<EmployeeAttandance>(AttendanceSheetData.OrderBy(x => x.Employee).ThenBy(x => x.Date).ThenBy(x => x.Time).ToList());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()-" + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()- ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetTolerance()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  GetTolerance()...", category: Category.Info, priority: Priority.Low);
                DateTime result;
                var val = CrmStartUp.GetGeosAppSettings(19).DefaultValue;
                DateTime.TryParse(CrmStartUp.GetGeosAppSettings(19).DefaultValue, out result);
                AttendanceInTolerance = result;
                DateTime.TryParse(CrmStartUp.GetGeosAppSettings(20).DefaultValue, out result);
                AttendanceOutTolerance = result;
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  GetTolerance()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetTolerance Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Smart import attendance
        /// [001]GEOS2-49
        /// [003][cpatil][12-04-2020][GEOS2-2191] Don’t import correctly attendance (file/odbc) if we have employees with one entry to one single day and others employees with night Shifts
        /// </summary>
        private void SetInOut()
        {
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                for (int i = 0; i < AttendanceSheetData.Count; i++)
                {
                    var attendancesPerDay = AttendanceSheetData.Where(x => x.Date.Date == AttendanceSheetData[i].Date.Date && x.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID).ToList();

                    if (attendancesPerDay.Count == 1)
                    {
                        attendancesPerDay[0].IsRowError = true;
                        i = AttendanceSheetData.IndexOf(attendancesPerDay[attendancesPerDay.Count - 1]);
                        continue;
                    }

                    #region old Code
                    //for (int record = 0; record < attendancesPerDay.Count; record++)
                    //{
                    //    if (lastCheckedRecord == null)
                    //    {
                    //        attendancesPerDay[record].In = true;
                    //        lastCheckedRecord = attendancesPerDay[record];

                    //    }
                    //    else if (lastCheckedRecord.Out)
                    //    {
                    //        if (attendancesPerDay[record].Time > (lastCheckedRecord.Time.AddMinutes(AttendanceOutTolerance.Minute)))
                    //        {
                    //            attendancesPerDay[record].In = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (attendancesPerDay[record].Time >= (lastCheckedRecord.Time.AddMinutes(AttendanceInTolerance.Minute)) && lastCheckedRecord.In == false)
                    //        {
                    //            attendancesPerDay[record].Out = true;
                    //            lastCheckedRecord = attendancesPerDay[record];
                    //        }
                    //        else
                    //        {
                    //            for (int skipRecord = record + 1; skipRecord < attendancesPerDay.Count; skipRecord++)
                    //            {
                    //                if (attendancesPerDay[skipRecord].Time >= (lastCheckedRecord.Time.AddMinutes(AttendanceInTolerance.Minute)) || (attendancesPerDay.Last() == attendancesPerDay[skipRecord]))
                    //                {
                    //                    attendancesPerDay[skipRecord].Out = true;
                    //                    record = skipRecord;
                    //                    break;
                    //                }
                    //                else
                    //                {
                    //                    lastCheckedRecord = attendancesPerDay[record];
                    //                    record = skipRecord;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    //if (attendancesPerDay[record].In != false || attendancesPerDay[record].Out != false)
                    //    //{
                    //    //    lastCheckedRecord = attendancesPerDay[record];
                    //    //}
                    //}
                    #endregion
                    for (int record = 0; record < attendancesPerDay.Count; record++)
                    {
                        // Added new code [003]
                        if (record > 0 && attendancesPerDay[record - 1].In && attendancesPerDay[record - 1].Date.Date != attendancesPerDay[record].Date.Date)
                        {
                            attendancesPerDay[record - 1].IsRowError = true;
                            attendancesPerDay[record - 1].In = false;
                        }
                        //Added new code[003]
                        if (record == 0 || attendancesPerDay[record - 1].IsRowError)
                        {
                            var inRecords = attendancesPerDay.Where(s => s.Time >= attendancesPerDay[record].Time && s.Time <= attendancesPerDay[record].Time.AddMinutes(AttendanceInTolerance.Minute));
                            inRecords.First().In = true;
                            record = attendancesPerDay.IndexOf(inRecords.Last());
                            continue;
                        }

                        if (attendancesPerDay[record - 1].Out)
                        {
                            var inRecords = attendancesPerDay.Where(s => s.Time >= attendancesPerDay[record].Time && s.Time <= attendancesPerDay[record].Time.AddMinutes(AttendanceInTolerance.Minute));
                            inRecords.First().In = true;
                            if (record == attendancesPerDay.Count() - 1)
                            {
                                attendancesPerDay[record].In = false;
                            }
                            record = attendancesPerDay.IndexOf(inRecords.Last());
                          
                        }
                        else
                        {
                            //code added on 11 - 08 - 20 IdCompany == 11558 || IdCompany == 559
                            var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                            if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                            {
                                var outRecords = attendancesPerDay.Where(s => s.Time >= attendancesPerDay[record].Time && s.Time <= attendancesPerDay[record].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                var currentDateRecords = attendancesPerDay.Where(s => s.Time.Date == attendancesPerDay[record].Time.Date).ToList();
                                if (currentDateRecords.Exists(a => a.In == true))
                                {
                                    currentDateRecords.Last().Out = true;
                                    record = attendancesPerDay.IndexOf(currentDateRecords.Last());
                                }
                            }
                            else
                            {
                                var outRecords = attendancesPerDay.Where(s => s.Time >= attendancesPerDay[record].Time && s.Time <= attendancesPerDay[record].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                outRecords.Last().Out = true;
                                record = attendancesPerDay.IndexOf(outRecords.Last());
                           }
                            
                        }
                    }

                    i = AttendanceSheetData.IndexOf(attendancesPerDay[attendancesPerDay.Count - 1]);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// this method use for set in/out for Night shift
        /// [000][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        /// [002][skale][10-01-2020][GEOS2-2002] If only one entry in one day can't show in red colour
        /// [003][cpatil][12-04-2020][GEOS2-2191] Don’t import correctly attendance (file/odbc) if we have employees with one entry to one single day and others employees with night Shifts
        /// </summary>
        /// <param name="EmployeeShiftList"></param>
        private void SetNightShiftInOut(List<EmployeeShift> EmployeeShiftList)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method SetNightShiftInOut()...", category: Category.Info, priority: Priority.Low);
                for (int i = 0; i < AttendanceSheetData.Count; i++)
                {
                    var attendancesPerEmployee = AttendanceSheetData.Where(x => x.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID).ToList();

                    if (EmployeeShiftList.Any(x => x.CompanyShift.IsNightShift == 1 && x.IdEmployee == AttendanceSheetData[i].idEmployee))
                    {
                        for (int record = 0; record < attendancesPerEmployee.Count; record++)
                        {

                            Dictionary<long, TimeSpan> tempDictionary = new Dictionary<long, TimeSpan>();
                            TimeSpan empAttendanceTime;
                            TimeSpan differenceShiftTime_AttendanceTime;

                            #region Find nearest Shift By Employee Attendance
                            // this code use for get Employee nearest Shif
                            foreach (EmployeeShift EmpShift in EmployeeShiftList.Where(x => x.IdEmployee == attendancesPerEmployee[record].idEmployee).ToList())
                            {
                                try
                                {
                                    if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Monday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.MonStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.MonStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.MonStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Tuesday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.TueStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.TueStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.TueStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Wednesday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.WedStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.WedStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.WedStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Thursday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.ThuStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.ThuStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.ThuStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);

                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Friday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.FriStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.FriStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.FriStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Saturday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;

                                        if (empAttendanceTime >= EmpShift.CompanyShift.SatStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.SatStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.SatStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                                    {
                                        empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                        if (empAttendanceTime >= EmpShift.CompanyShift.SunStartTime)
                                            differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.SunStartTime;
                                        else
                                            differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.SunStartTime - empAttendanceTime;

                                        tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }

                            long keyOfMinValue = 0;
                            if (tempDictionary.Count > 0)
                                keyOfMinValue = tempDictionary.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;

                            #endregion

                            #region Get Employee Shift Start Time And End Time
                            //this code use for Get Employee Shift Starttime and End Time
                            EmployeeShift employeeShift = EmployeeShiftList.Where(x => x.IdCompanyShift == keyOfMinValue).FirstOrDefault();

                            TimeSpan shiftStartTime = new TimeSpan();
                            TimeSpan shiftEndTime = new TimeSpan();
                            DateTime shiftStartDateTime = new DateTime();
                            DateTime shiftEndDateTime = new DateTime();

                            if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Monday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                //shiftStartTime = employeeShift.CompanyShift.MonStartTime;
                                shiftEndTime = employeeShift.CompanyShift.MonEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Tuesday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                //shiftStartTime = employeeShift.CompanyShift.TueStartTime;
                                shiftEndTime = employeeShift.CompanyShift.ThuEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Wednesday.ToString())
                            {

                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                // shiftStartTime = employeeShift.CompanyShift.WedStartTime;
                                shiftEndTime = employeeShift.CompanyShift.WedEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Thursday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                //shiftStartTime = employeeShift.CompanyShift.ThuStartTime;
                                shiftEndTime = employeeShift.CompanyShift.ThuEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Friday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                // shiftStartTime = employeeShift.CompanyShift.FriStartTime;
                                shiftEndTime = employeeShift.CompanyShift.FriEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Saturday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                // shiftStartTime = employeeShift.CompanyShift.SatStartTime;
                                shiftEndTime = employeeShift.CompanyShift.SatEndTime;
                            }
                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                            {
                                shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                //shiftStartTime = employeeShift.CompanyShift.SunStartTime;
                                shiftEndTime = employeeShift.CompanyShift.SunEndTime;
                            }

                            if (employeeShift.CompanyShift.IsNightShift == 1)
                            {
                                shiftStartDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftStartTime);
                                shiftEndDateTime = attendancesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime);
                            }
                            else
                            {
                                shiftStartDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftStartTime);
                                //code added on 12 - 08 - 20 IdCompany == 11558 || IdCompany == 559
                                //var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                //if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                //{
                                    var getEndDateTime = DateTime.Now.Date;
                                    TimeSpan empty = new TimeSpan(0, 0, 0);
                                    if (shiftEndTime == empty)
                                    {
                                        getEndDateTime = attendancesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime).AddHours(3);
                                    }
                                    else
                                    {
                                        getEndDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftEndTime).AddHours(3);
                                    }
                                    shiftEndDateTime = getEndDateTime;
                                    TimeSpan time = new TimeSpan(23, 59, 59);
                                    if (shiftEndTime != empty)
                                    {
                                        if (getEndDateTime.TimeOfDay > time)
                                        {
                                            shiftEndDateTime = attendancesPerEmployee[record].Time.Date.AddDays(1).Add(shiftEndTime);
                                        }
                                        else
                                        {
                                            shiftEndDateTime = attendancesPerEmployee[record].Time.Add(shiftEndTime);
                                        }
                                    }
                                //}
                                //else
                                //{
                                //    shiftEndDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftEndTime);
                                //}
                            }
                            #endregion

                            #region Set In/Out For Night Shift Employee 


                            var GetShiftEntryList = attendancesPerEmployee.Where(x => x.Time >= shiftStartDateTime && x.Time <= shiftEndDateTime.AddHours(3)).ToList();

                            // var attendancesPerDay = AttendanceSheetData.Where(x => x.Date.Date == AttendanceSheetData[i].Date.Date && x.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID).ToList();
                            //[002] added
                            if (GetShiftEntryList.Count == 1)
                            {
                                GetShiftEntryList[0].IsRowError = true;
                                i = AttendanceSheetData.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                                continue;
                            }

                            for (int count = 0; count < GetShiftEntryList.Count; count++)
                            {
                                if (count == 0)
                                {
                                    var inRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(AttendanceInTolerance.Minute));
                                    inRecords.First().In = true;
                                    inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                    inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                    count = GetShiftEntryList.IndexOf(inRecords.Last());
                                    inRecords.First().Out = false;
                                    continue;
                                }
                                if (GetShiftEntryList[count - 1].Out)
                                {
                                    var inRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(AttendanceInTolerance.Minute));

                                    if (GetShiftEntryList.Count != inRecords.Count() + count)
                                    {
                                        inRecords.First().In = true;
                                        inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                        inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(inRecords.Last());
                                        inRecords.First().Out = false;
                                    }
                                    else
                                    {
                                        inRecords.First().In = false;
                                        inRecords.First().CompanyShift = employeeShift.CompanyShift;
                                        inRecords.First().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(inRecords.Last());
                                        inRecords.First().Out = false;
                                    }
                                }
                                else
                                {
                                    ////code added on 12 - 08 - 20 IdCompany == 11558 || IdCompany == 559
                                    var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                    if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                    {
                                        var outRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                        if (GetShiftEntryList.Exists(a => a.In == true))
                                        {
                                            GetShiftEntryList.Last().Out = true;
                                            GetShiftEntryList.Last().CompanyShift = employeeShift.CompanyShift;
                                            GetShiftEntryList.Last().AccountingDate = GetShiftEntryList[0].Time.Date;
                                            count = GetShiftEntryList.IndexOf(GetShiftEntryList.Last());
                                            GetShiftEntryList.Last().In = false;
                                        }
                                    }
                                    else
                                    {
                                        var outRecords = GetShiftEntryList.Where(s => s.Time >= GetShiftEntryList[count].Time && s.Time <= GetShiftEntryList[count].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                        outRecords.Last().Out = true;
                                        outRecords.Last().CompanyShift = employeeShift.CompanyShift;
                                        outRecords.Last().AccountingDate = GetShiftEntryList[0].Time.Date;
                                        count = GetShiftEntryList.IndexOf(outRecords.Last());
                                        outRecords.Last().In = false;
                                    }
                                }
                            }
                            record = attendancesPerEmployee.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                        }

                        i = AttendanceSheetData.IndexOf(attendancesPerEmployee[attendancesPerEmployee.Count - 1]);
                        #endregion

                    }
                    else
                    {
                        #region Set  In/Out Attendance For Non Morining shift and afternoon shift Employee 
                        //[002] added
                        if (attendancesPerEmployee.Count == 1)
                        {
                            attendancesPerEmployee[0].IsRowError = true;
                            i = AttendanceSheetData.IndexOf(attendancesPerEmployee[attendancesPerEmployee.Count - 1]);
                            continue;
                        }
                        //end
                        else
                        {
                            for (int nonNightShiftRecordCount = 0; nonNightShiftRecordCount < attendancesPerEmployee.Count; nonNightShiftRecordCount++)
                            {
                                // Added new code [003]
                                if (nonNightShiftRecordCount > 0 && attendancesPerEmployee[nonNightShiftRecordCount - 1].In && attendancesPerEmployee[nonNightShiftRecordCount - 1].Date.Date != attendancesPerEmployee[nonNightShiftRecordCount].Date.Date)
                                {
                                    attendancesPerEmployee[nonNightShiftRecordCount - 1].IsRowError = true;
                                    attendancesPerEmployee[nonNightShiftRecordCount - 1].In = false;
                                }
                                //Added new code[003]
                                if (nonNightShiftRecordCount == 0 || attendancesPerEmployee[nonNightShiftRecordCount - 1].IsRowError)
                                {
                                    var inRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID== AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceInTolerance.Minute));
                                    inRecords.First().In = true;
                                    nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(inRecords.Last());
                                    inRecords.First().AccountingDate = inRecords.First().Time.Date;

                                    continue;
                                }


                                if (attendancesPerEmployee[nonNightShiftRecordCount - 1].Out)
                                {
                                    var inRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceInTolerance.Minute));
                                    inRecords.First().In = true;
                                    if (nonNightShiftRecordCount == attendancesPerEmployee.Count() - 1)
                                    {
                                        attendancesPerEmployee[nonNightShiftRecordCount].In = false;
                                    }
                                    inRecords.First().AccountingDate = inRecords.First().Time.Date;
                                    nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(inRecords.Last());
                                }
                                else
                                {
                                    //code added on 11-08-20 IdCompany == 11558 || IdCompany == 559
                                    var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                    if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                    {
                                        var outRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                        var currentDateRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time.Date == attendancesPerEmployee[nonNightShiftRecordCount].Time.Date).ToList();
                                        if (currentDateRecords.Exists(a => a.In == true))
                                        {
                                            currentDateRecords.Last().Out = true;
                                            currentDateRecords.Last().AccountingDate = currentDateRecords.Last().Time.Date;
                                            nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(currentDateRecords.Last());
                                        }
                                    }
                                    else
                                    {
                                        var outRecords = attendancesPerEmployee.Where(s => s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                        outRecords.Last().Out = true;
                                        outRecords.Last().AccountingDate = outRecords.Last().Time.Date;
                                        nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(outRecords.Last());
                                    }
                                }
                            }
                        }

                        i = AttendanceSheetData.IndexOf(attendancesPerEmployee[attendancesPerEmployee.Count - 1]);
                        #endregion
                    }
                }
               
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method SetNightShiftInOut()....executed successfully", category: Category.Info, priority: Priority.Low);
              
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method SetNightShiftInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }

        /// <summary>
        /// Method to set In/Out Checkbox as Radio 
        /// </summary>
        /// <param name="args"></param>
        public void InOutChangedCommandAction(CellValueChangedEventArgs args)
        {
            GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method InOutChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

            TableView dxTableView = args.OriginalSource as TableView;
            GridControl dxGrid = dxTableView.DataControl as GridControl;

            if (args.Column.FieldName == "In")
            {
                dxGrid.SetFocusedRowCellValue(dxGrid.Columns["Out"], "False");
            }
            else if (args.Column.FieldName == "Out")
            {
                dxGrid.SetFocusedRowCellValue(dxGrid.Columns["In"], "False");
            }

            GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method InOutChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to Fill Attendance Type from lookup 
        /// </summary>
        public void FillAttendanceTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  FillAttendanceTypeList()...", category: Category.Info, priority: Priority.Low);
                AttendanceTypeList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(33).AsEnumerable());
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method FillAttendanceTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method FillAttendanceTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][24-06-2019][GEOS2-1598]Wrong message style when importing attendance 
        /// [002][skale][28-11-2019][GEOS2-1923] Employee attendance added to another employee in import attendance
        /// [003][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        /// [004][skale][10-01-2020][GEOS2-2001] Wrong attendance import
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                List<EmployeeAttandance> FilterdRows = new List<EmployeeAttandance>();
                var FilterdView = (DevExpress.Xpf.Grid.TableView)obj;
                IList Rows = FilterdView.Grid.DataController.GetAllFilteredAndSortedRows();

                foreach (var row in Rows)
                {
                    var employeeAttandance = (EmployeeAttandance)row;

                    FilterdRows.Add(employeeAttandance);
                }

                if (FilterdRows.Count != AttendanceSheetData.Count)
                {
                    //[001]added
                    MessageBoxResult Result = CustomMessageBox.Show(string.Format(Application.Current.Resources["AttendanceFilterMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (Result != MessageBoxResult.Yes)
                        return;
                }

                FilterdRows = new List<EmployeeAttandance>();
                foreach (var row in Rows)
                {
                    var employeeAttandance = (EmployeeAttandance)row;
                    if (employeeAttandance.Time == (null) || employeeAttandance.Date == (null))
                        continue;
                    if ((employeeAttandance.In == true || employeeAttandance.Out == true))
                        FilterdRows.Add(employeeAttandance);
                }

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

                List<EmployeeAttendanceImportField> tmpList = new List<EmployeeAttendanceImportField>();
                IsBusy = true;
                for (int i = 0; i < FilterdRows.Count; i++)
                {
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    EmployeeAttendanceImportField objEmp = new EmployeeAttendanceImportField() { Name = FilterdRows[i].EmployeeClockTimeID, IdCompanyWork = FilterdRows[i].Type.IdLookupValue };

                    if (FilterdRows[i].In == true)
                    {
                        string sdt = FilterdRows[i].Date.ToShortDateString() + " " + FilterdRows[i].Time.ToShortTimeString();
                        startDate = Convert.ToDateTime(sdt);
                        objEmp.StartDate = startDate;
                        EmployeeAttandance EmployeeAttandanceOutRecord;
                        //[004] added
                        if (FilterdRows[i].CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        {
                            if (FilterdRows[i].CompanyShift.IsNightShift != 1)
                            {
                                //Night Employee (Check out time present or not with same day attendance)
                                EmployeeAttandanceOutRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay > FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                                if (EmployeeAttandanceOutRecord == null)//No out time is found
                                    continue;
                            }
                            else
                            {
                                EmployeeAttandanceOutRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true).FirstOrDefault();
                                if (EmployeeAttandanceOutRecord == null)
                                    continue;
                            }
                        }
                        else
                        {
                            // Non Night Shift Employee (Check out time with same day attendance)
                            EmployeeAttandanceOutRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay > FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                            if (EmployeeAttandanceOutRecord == null)//No out time is found
                                continue;
                        }
                        //old Code Comment by skale
                        //EmployeeAttandance EmployeeAttandanceOutRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay > FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                        //if (EmployeeAttandanceOutRecord == null)//No out time is found
                        //    continue;
                        //[003]added
                        objEmp.CompanyShift = FilterdRows[i].CompanyShift;
                        objEmp.AccountingDate = FilterdRows[i].AccountingDate;
                        //end
                        objEmp.EndDate = DateTime.Parse(EmployeeAttandanceOutRecord.Date.ToShortDateString() + " " + EmployeeAttandanceOutRecord.Time.ToShortTimeString());
                        FilterdRows.Remove(EmployeeAttandanceOutRecord);
                    }
                    else if (FilterdRows[i].Out == true)
                    {
                        string edt = FilterdRows[i].Date.ToShortDateString() + " " + FilterdRows[i].Time.ToShortTimeString();
                        endDate = Convert.ToDateTime(edt);
                        objEmp.EndDate = endDate;
                        EmployeeAttandance EmployeeAttandanceIntRecord;
                        //code comment by skale
                        //EmployeeAttandance EmployeeAttandanceIntRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay < FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                        //if (EmployeeAttandanceIntRecord == null)
                        //    continue;
                        //[004] added
                        //for night shift
                        if (FilterdRows[i].CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        {
                            if (FilterdRows[i].CompanyShift.IsNightShift != 1)
                            {
                                EmployeeAttandanceIntRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay < FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                                if (EmployeeAttandanceIntRecord == null)
                                    continue;
                            }
                            else
                            {
                                EmployeeAttandanceIntRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true).FirstOrDefault();
                                if (EmployeeAttandanceIntRecord == null)
                                    continue;
                            }
                        }
                        else
                        {
                            // Non Night Shift Employee (Check out time with same day attendance)
                            EmployeeAttandanceIntRecord = FilterdRows.Where(x => x.EmployeeClockTimeID == FilterdRows[i].EmployeeClockTimeID && x.Out == true && x.Date == FilterdRows[i].Date && x.Time.TimeOfDay < FilterdRows[i].Time.TimeOfDay).FirstOrDefault();
                            if (EmployeeAttandanceIntRecord == null)
                                continue;
                        }

                        objEmp.CompanyShift = FilterdRows[i].CompanyShift;
                        objEmp.AccountingDate = FilterdRows[i].AccountingDate;

                        objEmp.StartDate = DateTime.Parse(EmployeeAttandanceIntRecord.Date + " " + EmployeeAttandanceIntRecord.Time);
                        FilterdRows.Remove(EmployeeAttandanceIntRecord);
                    }

                    tmpList.Add(objEmp);

                    #region Old Code
                    //if (startDate < endDate)
                    //    tmpList.Add(objEmp);

                    //var query = tmpList.Where(e => e.Name == FilterdRows[i].EmployeeClockTimeID && (e.StartDate.Date == DateTime.Parse(FilterdRows[i].Date) || e.EndDate.Date == DateTime.Parse(FilterdRows[i].Date))).ToList();
                    //bool isExist = query.Exists(x => x.Name == FilterdRows[i].EmployeeClockTimeID);
                    //EmployeeAttendanceImportField objEmpl = query.Find(x => x.Name == FilterdRows[i].EmployeeClockTimeID);
                    //if (isExist)
                    //{
                    //    EmployeeAttendanceImportField objEmployee = new EmployeeAttendanceImportField();
                    //    objEmployee = objEmpl;
                    //    tmpList.Remove(objEmpl);
                    //    if (objEmployee.StartDate == DateTime.MinValue)
                    //    {
                    //        if (FilterdRows[i].In == true)
                    //        {
                    //            string sdt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //            startDate = Convert.ToDateTime(sdt);
                    //            objEmployee.StartDate = startDate;
                    //            objEmployee.IdCompanyWork = objEmp.IdCompanyWork;
                    //        }
                    //    }
                    //    else if (objEmployee.EndDate == DateTime.MinValue || objEmployee.EndDate != DateTime.MinValue)
                    //    {
                    //        if (FilterdRows[i].Out == true)
                    //        {
                    //            string edt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //            endDate = Convert.ToDateTime(edt);
                    //            objEmployee.EndDate = endDate;
                    //            objEmployee.IdCompanyWork = objEmp.IdCompanyWork;
                    //        }
                    //    }
                    //    tmpList.Add(objEmployee);
                    //}




                    //    EmployeeAttendanceImportField objEmp = new EmployeeAttendanceImportField() { Name = FilterdRows[i].EmployeeClockTimeID, IdCompanyWork = FilterdRows[i].Type.IdLookupValue };

                    //if ((tmpList.Count == 0) && (FilterdRows[i].In == true || FilterdRows[i].Out == true))
                    //{
                    //    if (FilterdRows[i].In == true)
                    //    {
                    //        string sdt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //        startDate = Convert.ToDateTime(sdt);
                    //        objEmp.StartDate = startDate;
                    //    }
                    //    else if (FilterdRows[i].Out == true)
                    //    {
                    //        string edt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //        endDate = Convert.ToDateTime(edt);
                    //        objEmp.EndDate = endDate;
                    //    }
                    //    tmpList.Add(objEmp);
                    //}
                    //else
                    //{
                    //    if (tmpList.Count > 0)
                    //    {
                    //     //   var query = tmpList.Where(e => e.Name == FilterdRows[i].EmployeeClockTimeID && (e.StartDate.Date == DateTime.Parse(FilterdRows[i].Date) || e.EndDate.Date == DateTime.Parse(FilterdRows[i].Date)));
                    //        List<EmployeeAttendanceImportField> list = new List<EmployeeAttendanceImportField>();
                    //        list = query.ToList();
                    //       // bool isExist = list.Exists(x => x.Name == FilterdRows[i].EmployeeClockTimeID);
                    //        EmployeeAttendanceImportField objEmpl = list.Find(x => x.Name == FilterdRows[i].EmployeeClockTimeID);
                    //        if (isExist)
                    //        {
                    //            EmployeeAttendanceImportField objEmployee = new EmployeeAttendanceImportField();
                    //            objEmployee = objEmpl;
                    //            tmpList.Remove(objEmpl);
                    //            if (objEmployee.StartDate == DateTime.MinValue)
                    //            {
                    //                if (FilterdRows[i].In == true)
                    //                {
                    //                    string sdt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //                    startDate = Convert.ToDateTime(sdt);
                    //                    objEmployee.StartDate = startDate;
                    //                    objEmployee.IdCompanyWork = objEmp.IdCompanyWork;
                    //                }
                    //            }
                    //            else if (objEmployee.EndDate == DateTime.MinValue || objEmployee.EndDate != DateTime.MinValue)
                    //            {
                    //                if (FilterdRows[i].Out == true)
                    //                {
                    //                    string edt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //                    endDate = Convert.ToDateTime(edt);
                    //                    objEmployee.EndDate = endDate;
                    //                    objEmployee.IdCompanyWork = objEmp.IdCompanyWork;
                    //                }
                    //            }
                    //            tmpList.Add(objEmployee);
                    //        }
                    //        else
                    //        {
                    //            tmpList.Add(objEmp);
                    //            if (FilterdRows[i].In == true)
                    //            {
                    //                string sdt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //                startDate = Convert.ToDateTime(sdt);
                    //                objEmp.StartDate = startDate;
                    //            }
                    //            else if (FilterdRows[i].Out == true)
                    //            {
                    //                string edt = FilterdRows[i].Date + " " + FilterdRows[i].Time;
                    //                endDate = Convert.ToDateTime(edt);
                    //                objEmp.EndDate = endDate;
                    //            }
                    //            else
                    //            {
                    //                tmpList.Remove(objEmp);
                    //            }
                    //        }
                    //    }
                    //} 
                    #endregion
                }

                EmpAddedAttendanceList = new List<EmployeeAttendance>();
                string errorMessage = string.Empty;
                bool isAttendanceOverlapped = false;

                foreach (EmployeeAttendanceImportField item in tmpList)
                {
                    Employee employee = new Employee();
                    if (item.StartDate != DateTime.MinValue && item.EndDate != DateTime.MinValue && item.StartDate < item.EndDate)
                    {
                        foreach (var item2 in SheetData)
                        {
                            foreach (var item3 in item2)
                            {
                                if (item3.Key == "EmployeeClockTimeID" && item3.Value.ToString().Equals(item.Name.ToString()))
                                {
                                    employee = (Employee)item2["Employee"];
                                    break;
                                }
                            }
                        }
                        //[003]added
                        CompanyShift CompanyShift;
                        int IdCompanyShift;
                        DateTime? AccountingDate;

                        if (item.CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                        {
                            CompanyShift = item.CompanyShift;
                            IdCompanyShift = item.CompanyShift.IdCompanyShift;
                            AccountingDate = item.AccountingDate;
                        }
                        else
                        {
                            CompanyShift = employee.CompanyShift;
                            IdCompanyShift = employee.IdCompanyShift;
                            AccountingDate = item.StartDate;

                        }
                        //end
                        EmployeeAttendance employeeAttendance = new EmployeeAttendance
                        {
                            ClockID = item.Name.ToString(),
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Employee = employee,
                            IdCompanyWork = item.IdCompanyWork,
                            //CompanyShift = employee.CompanyShift,
                            //IdCompanyShift = employee.IdCompanyShift,
                            //AccountingDate = item.StartDate
                            CompanyShift = CompanyShift,
                            IdCompanyShift = IdCompanyShift,
                            IdEmployee = employee.IdEmployee,
                            AccountingDate = AccountingDate
                        };

                        employeeAttendance.Employee.TotalWorkedHours = (item.EndDate - item.StartDate).ToString();
                        //Attendance exist on that time to emp
                        if (!(EmployeeAttendanceList.Where(x => x.IdEmployee == employeeAttendance.IdEmployee).Any(x => (x.StartDate < employeeAttendance.EndDate && employeeAttendance.StartDate < x.EndDate))
                            || EmpAddedAttendanceList.Where(x => x.IdEmployee == employeeAttendance.IdEmployee).Any(x => (x.StartDate < employeeAttendance.EndDate && employeeAttendance.StartDate < x.EndDate))))
                            EmpAddedAttendanceList.Add(employeeAttendance);
                        else
                            isAttendanceOverlapped = true;
                    }
                    else
                    {
                        if (item.StartDate == DateTime.MinValue)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("AttendanceCheckInError").ToString() + "\n", item.Name, item.EndDate.Date.ToShortDateString());
                        else if (item.EndDate == DateTime.MinValue)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("AttendanceCheckOutError").ToString() + "\n", item.Name, item.StartDate.Date.ToShortDateString());
                        else if (item.StartDate > item.EndDate)
                            errorMessage += string.Format(System.Windows.Application.Current.FindResource("AttendanceCheckInOutError").ToString() + "\n", item.Name, item.StartDate.Date.ToShortDateString());
                    }
                }

                if (EmpAddedAttendanceList.Count > 0)
                {
                    //[002] added change method
                    NewEmployeeAttendanceList = HrmService.AddEmployeeImportAttendance_V2038(EmpAddedAttendanceList);
                    IsSave = true;
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttendanceSaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    if (errorMessage != "")
                        CustomMessageBox.Show(string.Format(errorMessage.ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    else if (isAttendanceOverlapped)
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeAttendanceOverlapped"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    else
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["AttendanceSaveFailed"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteAttendanceRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method DeleteAttendanceRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                EmployeeAttandance ObjAttendance = (EmployeeAttandance)parameter;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttendanceDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ObjAttendance != null)
                    {
                        AttendanceSheetData.Remove(ObjAttendance);
                        //[001]added
                        if (ObjAttendance.CompanyShift != null)
                        {
                            SetNightShiftInOut(EmployeeShiftList);
                        }
                        else
                            SetInOut();
                    }
                }
                else
                {
                    IsBusy = false;
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method DeleteAttendanceRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method DeleteAttendanceRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method DeleteAttendanceRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method DeleteAttendanceRowCommandAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
               
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

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

