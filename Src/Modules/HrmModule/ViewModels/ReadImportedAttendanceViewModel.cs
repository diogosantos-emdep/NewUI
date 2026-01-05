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
       
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");

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
        private CompanyShift companyShiftNone;

        #endregion

        #region Properties

        private List<int> CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport { get; set; }
        private Dictionary<string, string> LstImportAttendanceCodes { get; set; }

        private bool UseFirstAndLastDayRecordInAttendanceImport { get; set; }

        private DateTime AttendanceInTolerance { get; set; }
        private DateTime AttendanceOutTolerance { get; set; }

        private List<IDictionary<string, object>> SheetData { get; set; }

        private ObservableCollection<EmployeeAttendance> employeeAttendanceList;
        public ObservableCollection<EmployeeAttendance> EmployeeAttendanceList
        {
            get { return employeeAttendanceList; }
            set
            {
                employeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceList"));
            }
        }
        private ObservableCollection<EmployeeAttendance> empAddedAttendanceList;
        public ObservableCollection<EmployeeAttendance> EmpAddedAttendanceList
        {
            get { return empAddedAttendanceList; }
            set
            {
                empAddedAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmpAddedAttendanceList"));
            }
        }

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
        public bool IsMatchTypeWithDataSourceField { get; set; }
        public string TimeEditMask { get; set; }
        public List<EmployeeAttendance> NewEmployeeAttendanceList { get; set; }
        //[004]added
        public List<EmployeeShift> EmployeeShiftList
        {
            get { return employeeShiftList; }
            set { employeeShiftList = value; OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList")); }
        }
        public GeosAppSetting Setting { get; set; }
        // shubham[skadam] GEOS2-3537 HRM - Communication with PSD software -  Import attendance using the employee company e-mail [#IES20]
        private string gridColHeader;
        public string GridColHeader
        {
            get
            {
                return gridColHeader;
            }
            set
            {
                gridColHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridColHeader"));
            }
        }
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
        private void SetShiftDataToShowInGridcolumnShift()
        {
            foreach (EmployeeAttandance item in AttendanceSheetData)
            {
                if (!item.CompanyShiftList.Contains(companyShiftNone))
                    item.CompanyShiftList.Insert(0, companyShiftNone);

                if (item.CompanyShiftList != null && item.CompanyShiftList.Count >= 1 && item.CompanyShift != null)
                {
                    item.CompanyShift = item.CompanyShiftList.First(x => x.IdCompanyShift == item.CompanyShift.IdCompanyShift);
                }
                else if (item.CompanyShiftList != null && item.CompanyShiftList.Count == 2)
                {
                    if (item.In == true || item.Out == true)
                    {
                        item.CompanyShift = item.CompanyShiftList[1];
                    }
                }

                if (item.CompanyShift == null)
                {
                    item.CompanyShift = item.CompanyShiftList[0];
                }
            }

            EmployeeAttandance inRecord = new EmployeeAttandance();
            // bool errorFound = false;
            for (int i = 0; i < AttendanceSheetData.Count; i++)
            {
                if (AttendanceSheetData[i].In)
                    inRecord = AttendanceSheetData[i];

                if (AttendanceSheetData[i].Out && inRecord != null)
                {
                    string error = AttendanceSheetData[i].EnableValidationAndGetError(inRecord);
                    //if (!string.IsNullOrEmpty(error))
                    //    errorFound = true;
                }
            }
        }

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

                try
                {
                    string ImportAttendanceCodes = CrmStartUp.GetGeosAppSettings(87).DefaultValue;
                    string[] ArrayImportAttendanceCodes = ImportAttendanceCodes.Split(',').ToArray();
                    if (LstImportAttendanceCodes == null)
                    {
                        LstImportAttendanceCodes = new Dictionary<string, string>();
                    }
                    foreach (string item in ArrayImportAttendanceCodes)
                    {
                        string[] tempCodes = null;
                        if (item.Contains(';'))
                            tempCodes = item.Split(';').ToArray();
                        if (tempCodes.Count() == 2 && tempCodes[0] != null && tempCodes[1] != null)
                            LstImportAttendanceCodes.Add(tempCodes[0].Replace('(', ' ').Trim(), tempCodes[1].Replace(')', ' ').Trim());
                    }

                    CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport = new List<int>();
                    var Setting62 = WorkbenchService.GetGeosAppSettings(62);
                    var CompanyIdsStringArray = Setting62.DefaultValue.Split(',');

                    for (int i = 0; i < CompanyIdsStringArray.Length; i++)
                    {
                        CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport.Add(int.Parse(CompanyIdsStringArray[i]));
                    }

                    SetUseFirstAndLastDayRecordInAttendanceImport();
                }
                catch (Exception ex)
                {
                    // if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init(). " +
                        "Check IdAppSetting 62 'UseFirstAndLastDayRecordInAttendanceImport' added correctly in table geos_app_settings. Now, Application will use default value 559,11558,11633. Error=" +
                        ex.Message, category: Category.Exception, priority: Priority.Low);
                    CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport = new List<int> { 559, 11558, 11633 }; // set default values. 559 is EPIN, 11558 is EEIN, 11633 is ETCN
                }

                EmployeeAttendanceData = new ObservableCollection<ImportAttendance>(colData);
                AttendanceSheetData = new ObservableCollection<EmployeeAttandance>();
                SheetData = sheetData;

                EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                EmployeeAttendanceList.AddRange(employeeAttendanceList);
                ObservableCollection<EmployeeAttandance> listObj = new ObservableCollection<EmployeeAttandance>();
                MappingFailMessage = string.Empty;

                EmployeeShiftList = new List<EmployeeShift>();
                List<Employee> EmployeeShiftsList = new List<Employee>();// HrmService.GetAllEmployeeShifts(EmployeeIds);

                companyShiftNone = new CompanyShift { IdCompanyShift = 0 };
                GetTolerance();

                #region sorting and removing unwanted data

                //List<IDictionary<string, object>> sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeClockTimeID"]))).ThenBy(row => Convert.ToDateTime(row["Date"])).ThenBy(row => Convert.ToDateTime(row["Time"])).ToList();
                // shubham[skadam] GEOS2-3537 HRM - Communication with PSD software -  Import attendance using the employee company e-mail [#IES20]
                List<IDictionary<string, object>> sortedSheetData = new List<IDictionary<string, object>>();
                if (sheetData.Any(a => a != null && a.Any(aa => aa.Key.Equals("EmployeeCompanyEmail"))))
                {
                    GridColHeader = "EmployeeCompanyEmail";
                    sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeCompanyEmail"]))).ThenBy(row => Convert.ToDateTime(row["Converted_Date_Time"])).ToList();

                }
                else
                {
                    GridColHeader = "EmployeeClockTimeID";
                    sortedSheetData = sheetData.OrderBy(row => (Convert.ToString(row["EmployeeClockTimeID"]))).ThenBy(row => Convert.ToDateTime(row["Converted_Date_Time"])).ToList();
                }


                #endregion
                Setting = WorkbenchService.GetGeosAppSettings(32);
                foreach (IDictionary<string, object> item2 in sortedSheetData)
                {
                    EmployeeAttandance obj = new EmployeeAttandance();

                    foreach (var item3 in item2)
                    {
                        // shubham[skadam] GEOS2-3537 HRM - Communication with PSD software -  Import attendance using the employee company e-mail [#IES20]
                        if (item3.Key == "EmployeeClockTimeID" || item3.Key == "EmployeeCompanyEmail")
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

                                obj.CompanyShiftList = new ObservableCollection<CompanyShift>();
                                foreach (EmployeeShift itemEmployeeShift in employee.EmployeeShifts)
                                {
                                    obj.CompanyShiftList.Add(itemEmployeeShift.CompanyShift);
                                }

                                if (!EmployeeShiftsList.Exists(x => x.IdEmployee == employee.IdEmployee))
                                {
                                    EmployeeShiftsList.Add(employee);
                                }

                            }
                            catch (Exception ex)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("AttendanceFieldMappingFailed").ToString());
                                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                break;
                            }
                        }
                        else if (IsMatchTypeWithDataSourceField && item3.Key == "Type") //[002]
                        {
                            try
                            {
                                if (LstImportAttendanceCodes.Any(i => i.Value.ToString() == item3.Value.ToString().PadLeft(4, '0')))
                                {
                                    string idLookvalue = LstImportAttendanceCodes.Where(i => i.Value == item3.Value.ToString().PadLeft(4, '0').ToString()).FirstOrDefault().Key;
                                    obj.Type = AttendanceTypeList.Where(i => i.IdLookupValue == Convert.ToInt32(idLookvalue)).FirstOrDefault();
                                }
                            }
                            catch (Exception ex)
                            {
                                MappingFailMessage = string.Format(System.Windows.Application.Current.FindResource("AttendanceFieldMappingFailed").ToString());
                                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                break;
                            }
                        }

                    }

                    if (!IsMatchTypeWithDataSourceField)
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

                    //List<Company> CompanyScheduleList = HrmService.GetCompaniesDetailByIds(plantOwnersIds);
                    //List<CompanyShift> CompanyShiftList = CompanyScheduleList.ToList().SelectMany(comp => comp.CompanySchedules.SelectMany(csche => csche.CompanyShifts)).ToList().Where(cshift => cshift.IsNightShift == 1).ToList();

                    if (EmployeeShiftsList.Count > 0)
                    {
                        // [002] Changed service method GetEmpDtlByEmpDocNoAndPeriod_V2039 to GetEmpDtlByEmpDocNoAndPeriod_V2041
                        // Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2036(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);
                        // Employees = HrmService.GetEmpDtlByEmpDocNoAndPeriod_V2090(EmpDocumentNumbers, plantOwnersIds, HrmCommon.Instance.SelectedPeriod);

                        // var EmployeeIds = string.Join(",", Employees.Select(i => i.IdEmployee).Distinct());

                        //List<Employee> EmployeeShiftsList = HrmService.GetAllEmployeeShifts(EmployeeIds);

                        //EmployeeNightShift = EmployeeShiftsList.SelectMany(es => es.EmployeeShifts.Where(x => x.CompanyShift.IsNightShift == 1)).ToList();

                        EmployeeShiftList = EmployeeShiftsList.SelectMany(es => es.EmployeeShifts).ToList();

                        //if (EmployeeNightShift.Count > 0)
                        //{
                        SetNightShiftInOut(EmployeeShiftList);
                        //}
                        //else
                        //{
                        //    SetInOut();
                        //}
                    }
                    //else
                    //{
                    //    SetInOut();
                    //}
                }
                else
                {
                    SetInOut();
                }
                //Clear and set AttendanceSheetData to refresh the data on UI
                var temp = (AttendanceSheetData.OrderBy(x => x.Employee).ThenBy(x => x.Date).ThenBy(x => x.Time)).ToList();
                AttendanceSheetData.Clear();
                AttendanceSheetData.AddRange(temp);
                //EmployeeShiftList = new List<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(AttendanceSheetData[0].idEmployee));
                SetShiftDataToShowInGridcolumnShift();
                #region GEOS2-3843
                //Shubham[skadam] GEOS2-3843 Error in attendance report  22 12 2022
                try
                {
                    foreach (var itemAttendance in AttendanceSheetData.Where(w => w.In == false && w.Out == false))
                    {
                        var tempAttendanceSheetData = AttendanceSheetData.Where(w => w.Employee.Equals(itemAttendance.Employee) && w.Date.Date == itemAttendance.Date);
                        if (tempAttendanceSheetData.Count() == 1)
                        {
                            itemAttendance.IsRowError = true;
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion
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
                            // if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                            if (this.UseFirstAndLastDayRecordInAttendanceImport)
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

        private void SetUseFirstAndLastDayRecordInAttendanceImport()
        {
            //code added on 11 - 08 - 20 IdCompany == 11558 || IdCompany == 559
            var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

            bool useFirstAndLastDayRecordInAttendanceImport = false;
            for (int j = 0; j < CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport.Count; j++)
            {
                if (companies.Any(a => a.IdCompany == CompanyIdsForSettingUseFirstAndLastDayRecordInAttendanceImport[j]))
                {
                    useFirstAndLastDayRecordInAttendanceImport = true;
                    break;
                }
            }

            UseFirstAndLastDayRecordInAttendanceImport = useFirstAndLastDayRecordInAttendanceImport;
        }

        /// <summary>
        /// this method use for set in/out for Night shift
        /// [000][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
        /// [002][skale][10-01-2020][GEOS2-2002] If only one entry in one day can't show in red colour
        /// [003][cpatil][12-04-2020][GEOS2-2191] Don’t import correctly attendance (file/odbc) if we have employees with one entry to one single day and others employees with night Shifts
        /// [004][cpatil][16-08-2023][GEOS2-4715] HRM not taking attendance properly in shift.
        /// </summary>
        /// <param name="EmployeeShiftList"></param>
        private void SetNightShiftInOut(List<EmployeeShift> EmployeeShiftList)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method SetNightShiftInOut()...", category: Category.Info, priority: Priority.Low);
                for (int i = 0; i < AttendanceSheetData.Count; i++)
                {
                    try
                    {
                        var attendancesPerEmployee = AttendanceSheetData.Where(x => x.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID).ToList();

                        if (attendancesPerEmployee[0].CompanyShiftList.Count > 1 || EmployeeShiftList.Any(x => x.CompanyShift.IsNightShift == 1 && x.IdEmployee == AttendanceSheetData[i].idEmployee))
                        {
                            for (int record = 0; record < attendancesPerEmployee.Count; record++)
                            {
                                try
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
                                                //Shubham[skadam] GEOS2-4112 HRM-add one exception to assign the registered attendance shift when the selected day has the shift with start and end date = 00:00  27 02 2023
                                                if (EmpShift.CompanyShift.SatStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.SatEndTime == new TimeSpan(0, 0, 0))
                                                {
                                                    List<string> LstWeek = new List<string>() { "Mon", "Tue", "Wed", "Thur", "Fri" };

                                                    foreach (var item in LstWeek)
                                                    {

                                                        if (!(EmpShift.CompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.MonEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SatStartTime = EmpShift.CompanyShift.MonStartTime;
                                                            EmpShift.CompanyShift.SatEndTime = EmpShift.CompanyShift.MonEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SatStartTime = EmpShift.CompanyShift.TueStartTime;
                                                            EmpShift.CompanyShift.SatEndTime = EmpShift.CompanyShift.TueEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SatStartTime = EmpShift.CompanyShift.WedStartTime;
                                                            EmpShift.CompanyShift.SatEndTime = EmpShift.CompanyShift.WedEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SatStartTime = EmpShift.CompanyShift.ThuStartTime;
                                                            EmpShift.CompanyShift.SatEndTime = EmpShift.CompanyShift.ThuEndTime;
                                                            break;
                                                        }
                                                        else if (!(EmpShift.CompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SatStartTime = EmpShift.CompanyShift.FriStartTime;
                                                            EmpShift.CompanyShift.SatEndTime = EmpShift.CompanyShift.FriEndTime;
                                                            break;
                                                        }


                                                    }


                                                }
                                                if (empAttendanceTime >= EmpShift.CompanyShift.SatStartTime)
                                                    differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.SatStartTime;
                                                else
                                                    differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.SatStartTime - empAttendanceTime;

                                                tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                            }
                                            else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                                            {
                                                empAttendanceTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                                //Shubham[skadam] GEOS2-4112 HRM-add one exception to assign the registered attendance shift when the selected day has the shift with start and end date = 00:00  27 02 2023
                                                if (EmpShift.CompanyShift.SunStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.SunEndTime == new TimeSpan(0, 0, 0))
                                                {
                                                    List<string> LstWeek = new List<string>() { "Mon", "Tue", "Wed", "Thur", "Fri" };

                                                    foreach (var item in LstWeek)
                                                    {

                                                        if (!(EmpShift.CompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.MonEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SunStartTime = EmpShift.CompanyShift.MonStartTime;
                                                            EmpShift.CompanyShift.SunEndTime = EmpShift.CompanyShift.MonEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SunStartTime = EmpShift.CompanyShift.TueStartTime;
                                                            EmpShift.CompanyShift.SunEndTime = EmpShift.CompanyShift.TueEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SunStartTime = EmpShift.CompanyShift.WedStartTime;
                                                            EmpShift.CompanyShift.SunEndTime = EmpShift.CompanyShift.WedEndTime;
                                                            break;
                                                        }

                                                        else if (!(EmpShift.CompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SunStartTime = EmpShift.CompanyShift.ThuStartTime;
                                                            EmpShift.CompanyShift.SunEndTime = EmpShift.CompanyShift.ThuEndTime;
                                                            break;
                                                        }
                                                        else if (!(EmpShift.CompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && EmpShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                                                        {
                                                            EmpShift.CompanyShift.SunStartTime = EmpShift.CompanyShift.FriStartTime;
                                                            EmpShift.CompanyShift.SunEndTime = EmpShift.CompanyShift.FriEndTime;
                                                            break;
                                                        }


                                                    }


                                                }

                                                if (empAttendanceTime >= EmpShift.CompanyShift.SunStartTime)
                                                    differenceShiftTime_AttendanceTime = empAttendanceTime - EmpShift.CompanyShift.SunStartTime;
                                                else
                                                    differenceShiftTime_AttendanceTime = EmpShift.CompanyShift.SunStartTime - empAttendanceTime;

                                                tempDictionary.Add(EmpShift.IdCompanyShift, differenceShiftTime_AttendanceTime);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method SetNightShiftInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                                        //[004]
                                        if (employeeShift.CompanyShift.SatEndTime == new TimeSpan(0, 0, 0))
                                        {
                                            List<string> LstWeek = new List<string>() { "Mon", "Tue", "Wed", "Thur", "Fri" };

                                            foreach (var item in LstWeek)
                                            {

                                                if (!(employeeShift.CompanyShift.MonEndTime == new TimeSpan(0, 0, 0)))
                                                {
                                                    employeeShift.CompanyShift.SatEndTime = employeeShift.CompanyShift.MonEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SatEndTime = employeeShift.CompanyShift.TueEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SatEndTime = employeeShift.CompanyShift.WedEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SatEndTime = employeeShift.CompanyShift.ThuEndTime;
                                                    break;
                                                }
                                                else if (!(employeeShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SatEndTime = employeeShift.CompanyShift.FriEndTime;
                                                    break;
                                                }


                                            }


                                        }
                                        shiftEndTime = employeeShift.CompanyShift.SatEndTime;
                                    }
                                    else if (attendancesPerEmployee[record].Time.DayOfWeek.ToString() == DaysOfWeek.Sunday.ToString())
                                    {
                                        shiftStartTime = attendancesPerEmployee[record].Time.TimeOfDay;
                                        //shiftStartTime = employeeShift.CompanyShift.SunStartTime;
                                        //[004]
                                        if (employeeShift.CompanyShift.SunEndTime == new TimeSpan(0, 0, 0))
                                        {
                                            List<string> LstWeek = new List<string>() { "Mon", "Tue", "Wed", "Thur", "Fri" };

                                            foreach (var item in LstWeek)
                                            {

                                                if (!(employeeShift.CompanyShift.MonEndTime == new TimeSpan(0, 0, 0)))
                                                {
                                                    employeeShift.CompanyShift.SunEndTime = employeeShift.CompanyShift.MonEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SunEndTime = employeeShift.CompanyShift.TueEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SunEndTime = employeeShift.CompanyShift.WedEndTime;
                                                    break;
                                                }

                                                else if (!(employeeShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SunEndTime = employeeShift.CompanyShift.ThuEndTime;
                                                    break;
                                                }
                                                else if (!(employeeShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                                                {

                                                    employeeShift.CompanyShift.SunEndTime = employeeShift.CompanyShift.FriEndTime;
                                                    break;
                                                }
                                            }
                                        }
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
                                        shiftEndDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftEndTime); //[GEOS2-5734][02.10.2024][rdixit]
                                        #region  Commented code for bug task [GEOS2-5734][02.10.2024][rdixit]
                                        //code added on 12 - 08 - 20 IdCompany == 11558 || IdCompany == 559
                                        //var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                        //if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                        //{
                                        /*
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
                                                if (shiftEndTime.Hours >= 23)
                                                {
                                                    shiftEndDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftEndTime).AddHours(3);
                                                }
                                                else
                                                {
                                                    shiftEndDateTime = attendancesPerEmployee[record].Time.Add(shiftEndTime).AddHours(-3);
                                                }
                                            }
                                        } */
                                        //}
                                        //else
                                        //{
                                        //    shiftEndDateTime = attendancesPerEmployee[record].Time.Date.Add(shiftEndTime);
                                        //}
                                        #endregion
                                    }
                                    #endregion

                                    #region Set In/Out For Night Shift Employee 


                                    var GetShiftEntryList = attendancesPerEmployee.Where(x => x.Time >= shiftStartDateTime && x.Time <= shiftEndDateTime.AddHours(4)).ToList();//[GEOS2-5734][02.10.2024][rdixit]

                                    // var attendancesPerDay = AttendanceSheetData.Where(x => x.Date.Date == AttendanceSheetData[i].Date.Date && x.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID).ToList();
                                    //[002] added                                
                                    if (GetShiftEntryList.Count == 1)
                                    {
                                        GetShiftEntryList[0].IsRowError = true;
                                        i = AttendanceSheetData.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                                        continue;
                                    }

                                    for (int count = 0; count < GetShiftEntryList?.Count; count++)
                                    {
                                        try
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
                                                //var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                                //if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                                if (this.UseFirstAndLastDayRecordInAttendanceImport)
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
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method SetNightShiftInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                    if (GetShiftEntryList?.Count > 0)
                                        record = attendancesPerEmployee.IndexOf(GetShiftEntryList[GetShiftEntryList.Count - 1]);
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method SetNightShiftInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }

                            i = AttendanceSheetData.IndexOf(attendancesPerEmployee[attendancesPerEmployee.Count - 1]);


                        }
                        else
                        {

                            #region Set  In/Out Attendance For Non Morining shift and afternoon shift Employee 
                            int LastRecordOfEmployee = 0;  //24-12-2020 

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
                                        var inRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceInTolerance.Minute));
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

                                        LastRecordOfEmployee = attendancesPerEmployee.IndexOf(inRecords.First()) - 1;
                                    }
                                    else
                                    {
                                        ////code added on 11-08-20 IdCompany == 11558 || IdCompany == 559
                                        //var companies = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                                        //if (companies.Any(a => a.IdCompany == 11558 || a.IdCompany == 559))
                                        if (this.UseFirstAndLastDayRecordInAttendanceImport)
                                        {
                                            var outRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                            var currentDateRecords = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time.Date == attendancesPerEmployee[nonNightShiftRecordCount].Time.Date).ToList();
                                            if (currentDateRecords.Exists(a => a.In == true))
                                            {
                                                currentDateRecords.Last().Out = true;
                                                currentDateRecords.Last().AccountingDate = currentDateRecords.Last().Time.Date;
                                                nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(currentDateRecords.Last());
                                                LastRecordOfEmployee = nonNightShiftRecordCount;
                                            }
                                            else
                                            {
                                                //24-12-2020 GEOS2-2809
                                                attendancesPerEmployee[nonNightShiftRecordCount - 1].IsRowError = true;

                                                currentDateRecords.First().In = true;
                                                if (nonNightShiftRecordCount == attendancesPerEmployee.Count() - 1)
                                                {
                                                    attendancesPerEmployee[nonNightShiftRecordCount].In = false;
                                                }
                                                currentDateRecords.First().AccountingDate = currentDateRecords.First().Time.Date;

                                                currentDateRecords.Last().Out = true;
                                                currentDateRecords.Last().AccountingDate = currentDateRecords.Last().Time.Date;
                                                nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(currentDateRecords.Last());
                                                LastRecordOfEmployee = nonNightShiftRecordCount;
                                            }
                                        }
                                        else
                                        {
                                            var outRecords = attendancesPerEmployee.Where(s => s.Time >= attendancesPerEmployee[nonNightShiftRecordCount].Time && s.Time <= attendancesPerEmployee[nonNightShiftRecordCount].Time.AddMinutes(AttendanceOutTolerance.Minute));
                                            outRecords.Last().Out = true;
                                            outRecords.Last().AccountingDate = outRecords.Last().Time.Date;
                                            nonNightShiftRecordCount = attendancesPerEmployee.IndexOf(outRecords.Last());
                                            LastRecordOfEmployee = nonNightShiftRecordCount;
                                        }
                                    }
                                }
                            }
                            //24-12-2020 for last employee entry red
                            var inRecords_Last = attendancesPerEmployee.Where(s => s.EmployeeClockTimeID == AttendanceSheetData[i].EmployeeClockTimeID && s.Time >= attendancesPerEmployee[LastRecordOfEmployee].Time && s.Time <= attendancesPerEmployee[LastRecordOfEmployee].Time.AddMinutes(AttendanceInTolerance.Minute)).ToList();

                            if (inRecords_Last.Exists(a => a.In == true) && !inRecords_Last.Exists(a => a.Out == true))
                            {
                                if (inRecords_Last.Count > 1)
                                {
                                    LastRecordOfEmployee = attendancesPerEmployee.IndexOf(inRecords_Last.Last());
                                    attendancesPerEmployee[LastRecordOfEmployee].IsRowError = true;
                                }
                            }

                            i = AttendanceSheetData.IndexOf(attendancesPerEmployee[attendancesPerEmployee.Count - 1]);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method SetNightShiftInOut()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

            if (args.Column.FieldName == "In" ||
                args.Column.FieldName == "Out" ||
                args.Column.FieldName == "CompanyShift")
            {
                bool invalue;
                CompanyShift companyShift;
                if (args.Column.FieldName == "In")
                {
                    invalue = (bool)args.Value;
                }
                else
                {
                    invalue = (bool)dxGrid.GetFocusedRowCellValue(dxGrid.Columns["In"]);
                }
                if (args.Column.FieldName == "CompanyShift")
                {
                    companyShift = (CompanyShift)args.Value;
                }
                else
                {
                    companyShift = (CompanyShift)dxGrid.GetFocusedRowCellValue(dxGrid.Columns["CompanyShift"]);
                }


                EmployeeAttandance currentInRow = (EmployeeAttandance)dxGrid.GetRow(args.RowHandle);
                if (invalue)
                {
                    for (int i = args.RowHandle; i < dxGrid.VisibleRowCount; i++)
                    {
                        EmployeeAttandance row = (EmployeeAttandance)dxGrid.GetRow(i);
                        if (row.Out)
                        {
                            EmployeeAttandance nextOutRow = row;
                            nextOutRow.CompanyShift = companyShift;
                            EmployeeAttandance currentInRowAfterUpdate = currentInRow;
                            currentInRowAfterUpdate.CompanyShift = companyShift;
                            nextOutRow.EnableValidationAndGetError(currentInRowAfterUpdate);
                            dxGrid.RefreshData();
                            break;
                        }
                    }

                }
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
        /// [005][avpawar][30-07-2021][GEOS2-3095] Add new information in Leaves and Attendance Create, Modify user who did and date when it done
        /// [006][cpatil][06-01-2023][GEOS2-3906] HRM not taking attendance properly.
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ReadImportedAttendanceViewModel Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                List<EmployeeAttandance> FilterdRows = new List<EmployeeAttandance>();
                List<EmployeeAttandance> FilterdRows1 = new List<EmployeeAttandance>();
                List<EmployeeAttandance> OverlappedRows = new List<EmployeeAttandance>();
                List<EmployeeAttandance> finalRows = new List<EmployeeAttandance>();
                var FilterdView = (DevExpress.Xpf.Grid.TableView)obj;
                IList Rows = FilterdView.Grid.DataController.GetAllFilteredAndSortedRows();
                //[GEOS2-5791][rdixit][21.06.2024]
                var itemsSource = FilterdView.Grid.ItemsSource as IEnumerable<EmployeeAttandance>;
                if (itemsSource != null)
                {
                    IList rows = FilterdView.Grid.DataController.GetAllFilteredAndSortedRows();

                    var employeeRows = rows.Cast<EmployeeAttandance>().Where(i => (!(i.In == false && i.Out == false)))?.Cast<EmployeeAttandance>();//[GEOS2-5963][rdixit][18.07.2024]
                    if (employeeRows != null)
                    {
                        finalRows = employeeRows.GroupBy(r => new { r.idEmployee, r.AccountingDate })
                                                    .Where(g => g.Select(r => r.CompanyShift).Distinct().Count() == 1).SelectMany(g => g).ToList();

                        FilterdRows1 = employeeRows.GroupBy(r => new { r.idEmployee, r.AccountingDate })
                                                   .Where(g => g.Select(r => r.CompanyShift).Distinct().Count() > 1).SelectMany(g => g).ToList();

                        if (FilterdRows1?.Count > 0)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceAlreadyAddedShift").ToString()
                           ), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }

                        foreach (var row in finalRows)
                        {
                            FilterdRows.Add(row);
                        }
                    }
                }

                //[Sudhir.Jangra][GEOS2-6541]
                if (Rows.Count != AttendanceSheetData.Count)
                {
                    //[001]added
                    MessageBoxResult Result = CustomMessageBox.Show(string.Format(Application.Current.Resources["AttendanceFilterMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (Result != MessageBoxResult.Yes)
                        return;
                }

                FilterdRows = new List<EmployeeAttandance>();
                //[GEOS2-5791][rdixit][21.06.2024]
                if (finalRows?.Count > 0)
                {
                    foreach (var row in finalRows)
                    {
                        var employeeAttandance = (EmployeeAttandance)row;
                        if (employeeAttandance.Time == (null) || employeeAttandance.Date == (null))
                            continue;
                        if ((employeeAttandance.In == true || employeeAttandance.Out == true))
                            FilterdRows.Add(employeeAttandance);
                    }

                    bool errorFound = false;
                    AttendanceSheetData.Clear();
                    AttendanceSheetData.AddRange(FilterdRows);
                    EmployeeAttandance inRecord = new EmployeeAttandance();

                    for (int i = 0; i < AttendanceSheetData.Count; i++)
                    {
                        if (AttendanceSheetData[i].In)
                            inRecord = AttendanceSheetData[i];

                        if (AttendanceSheetData[i].Out && inRecord != null)
                        {
                            string error = AttendanceSheetData[i].EnableValidationAndGetError(inRecord);
                            if (!string.IsNullOrEmpty(error))
                                errorFound = true;
                        }
                    }

                    if (errorFound)
                    {
                        CustomMessageBox.Show(
                            System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorCheckTheDataEntered").ToString() + Environment.NewLine +
                            System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorShiftValueIsRequired").ToString() + Environment.NewLine +
                            System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorShiftValueMustBeSameForInOut").ToString()
                            , Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
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
                        #region for
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
                            //Shubham[skadam] GEOS2-4938 Import Attendance Overlapped message with Employee name + Date with already registered Attendance record 08 01 2024
                            OverlappedRows.Add(EmployeeAttandanceOutRecord);
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
                            //Shubham[skadam] GEOS2-4938 Import Attendance Overlapped message with Employee name + Date with already registered Attendance record 08 01 2024
                            OverlappedRows.Add(EmployeeAttandanceIntRecord);
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

                        #endregion
                    }

                    EmpAddedAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    string errorMessage = string.Empty;
                    bool isAttendanceOverlapped = false;

                    foreach (EmployeeAttendanceImportField item in tmpList)
                    {
                        #region foreach 
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
                                    // shubham[skadam] GEOS2-3537 HRM - Communication with PSD software -  Import attendance using the employee company e-mail [#IES20]
                                    if (item3.Key == "EmployeeCompanyEmail" && item3.Value.ToString().Equals(item.Name.ToString()))
                                    {
                                        employee = (Employee)item2["Employee"];
                                        break;
                                    }
                                }
                            }
                            //[003]added
                            //CompanyShift CompanyShift;
                            //int IdCompanyShift;
                            DateTime? AccountingDate;

                            if (item.CompanyShift != null && Convert.ToInt32(Setting.DefaultValue) == 1)
                            {
                                //CompanyShift = item.CompanyShift;
                                //IdCompanyShift = item.CompanyShift.IdCompanyShift;
                                #region GEOS2-4070
                                //Shubham[skadam] GEOS2-4070 HRM not showing correct break time properly.  22 12 2022
                                if (item.AccountingDate != null)
                                {
                                    AccountingDate = item.AccountingDate;
                                }
                                else
                                {
                                    AccountingDate = item.StartDate;
                                }
                                #endregion
                            }
                            else
                            {
                                //CompanyShift = employee.CompanyShift;
                                //IdCompanyShift = employee.IdCompanyShift;
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
                                CompanyShift = item.CompanyShift,
                                IdCompanyShift = item.CompanyShift.IdCompanyShift,
                                IdEmployee = employee.IdEmployee,
                                AccountingDate = AccountingDate,

                                //[GEOS2-3095]
                                Creator = GeosApplication.Instance.ActiveUser.IdUser,
                                CreationDate = GeosApplication.Instance.ServerDateTime

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
                        #endregion
                    }

                    #region if (EmpAddedAttendanceList.Count > 0)
                    if (EmpAddedAttendanceList.Count > 0)
                    {
                        foreach (var item in EmpAddedAttendanceList)
                        {
                            if (item.IdCompanyWork == 305)
                            {
                                const int HoursThreshold = 8;
                                if (GeosApplication.Instance.CompensationLeave == null)
                                {
                                    GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                                }
                                TimeSpan timedifference = item.EndDate - item.StartDate;
                                // Calculate days and remaining hours
                                int convertedDays = timedifference.Days;
                                int convertedHours = timedifference.Hours;

                                // Adjust for negative values if enddate is before startdate
                                if (timedifference.TotalHours < 0)
                                {
                                    convertedDays = -convertedDays;
                                    convertedHours = -convertedHours;
                                }

                                // If less than HoursThreshold hours, assign all to convertedHours
                                if (convertedHours < HoursThreshold && convertedHours >= 0)
                                {
                                    convertedDays = 0;
                                }
                                // If more than HoursThreshold hours, adjust convertedDays and convertedHours
                                else if (convertedHours >= HoursThreshold)
                                {
                                    convertedDays += 1;
                                    convertedHours -= HoursThreshold;
                                }
                                GeosApplication.Instance.CompensationLeave.IdEmployee = item.IdEmployee;
                                GeosApplication.Instance.CompensationLeave.ConvertedDays = convertedDays;
                                GeosApplication.Instance.CompensationLeave.ConvertedHours = convertedHours;
                                if (!string.IsNullOrEmpty(item.Remark))
                                {
                                    GeosApplication.Instance.CompensationLeave.Comments = item.Remark;
                                }

                            }
                        }

                        #region [rdixit][GEOS2-5945][23.08.2024] commented
                        //[002] added change method
                        //NewEmployeeAttendanceList = HrmService.AddEmployeeImportAttendance_V2038(EmpAddedAttendanceList.ToList());
                        //[005] added change method
                        //[006] added change method
                        //Shubham[skadam] GEOS2-2366 Employee attendance importing failing in ESCN 27 12 2023
                        //IHrmService HrmServiceNew = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        //try
                        //{
                        //    GeosServiceProvider geosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(w =>
                        //     w.Name.Trim().ToUpper().Equals("EAES".ToUpper())).FirstOrDefault();
                        //    HrmServiceNew = new HrmServiceController(geosServiceProvider.ServiceProviderUrl);
                        //}
                        //catch (Exception ex)
                        //{
                        //    HrmServiceNew = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        //    GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        //}
                        //NewEmployeeAttendanceList = HrmService.AddEmployeeImportAttendance_V2350(EmpAddedAttendanceList.ToList());
                        //NewEmployeeAttendanceList = HrmServiceNew.AddEmployeeImportAttendance_V2350(EmpAddedAttendanceList.ToList());
                        //[Rahul.Gadhave][GEOS2-5945][Date:16-08-2024]
                        #endregion
                        string WorkbenchMainServiceProvider = HrmService.GetWorkbenchMainServiceProvider();
                        IWorkbenchMainService WorkbenchMainService = new WorkbenchMainServiceController(WorkbenchMainServiceProvider);
                        NewEmployeeAttendanceList = WorkbenchMainService.AddEmployeeImportAttendance_V2550(EmpAddedAttendanceList.ToList());
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
                        //Shubham[skadam] GEOS2-4938 Import Attendance Overlapped message with Employee name + Date with already registered Attendance record 08 01 2024
                        string EmployeeAttendanceOverlapped = Application.Current.Resources["EmployeeAttendanceOverlapped"].ToString();
                        string OverlappedMessage = string.Empty;
                        try
                        {
                            if (OverlappedRows.Count > 0)
                            {
                                foreach (var item in OverlappedRows)
                                {
                                    OverlappedMessage = OverlappedMessage + Convert.ToString(item.Date.Date.ToString("d")) + " \t " + item.Employee + "\n";
                                }
                                EmployeeAttendanceOverlapped = EmployeeAttendanceOverlapped + "\n\n" + OverlappedMessage;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in ReadImportedAttendanceViewModel Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //[pramod.misal][GEOS2-2821][27-02-2024]
                        if (errorMessage != "")
                        {
                            //CustomMessageBox.Show(string.Format(errorMessage.ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            CustomMessageBox.ShowWithEdit(string.Format(errorMessage.ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else if (isAttendanceOverlapped)
                        {
                            //CustomMessageBox.Show(string.Format(EmployeeAttendanceOverlapped), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            CustomMessageBox.ShowWithEdit(string.Format(EmployeeAttendanceOverlapped), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(Application.Current.Resources["AttendanceSaveFailed"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }

                    }
                    #endregion
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceNoRecord").ToString()),
                        Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
                        {
                            SetInOut();
                        }
                        SetShiftDataToShowInGridcolumnShift();
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

