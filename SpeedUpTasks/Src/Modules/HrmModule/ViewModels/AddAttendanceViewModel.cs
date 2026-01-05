using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.Validations;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Editors;
using System.Windows;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    /// <summary>
    ///Sprint 42--- HRM	M042-06-----Add and Edit Attendance----sdesai
    /// </summary>
    public class AddAttendanceViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isNew;
        //private List<DateTime> fromDates;
        private string timeEditMask;
        //private List<DateTime> _ToDates;
        private ObservableCollection<EmployeeAttendance> existEmployeeAttendanceList;
        private ObservableCollection<Employee> employeeListFinal;
        private long selectedPeriod;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? startTime;
        private DateTime? endTime;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;
        private string error = string.Empty;
        private int selectedIndexForEmployee;
        // private ObservableCollection<CompanyWork> attendanceTypeList;
        //private ObservableCollection<LookupValue> attendanceTypeList;
        private int selectedIndexForAttendanceType;
        private TimeSpan sTime;
        private TimeSpan eTime;
        public EmployeeAttendance NewEmployeeAttendance;
        public EmployeeAttendance UpdateEmployeeAttendance;
        private bool isSave;
        private bool result;
        private Company company;
        private ObservableCollection<CompanyShift> companyShifts;
        private ObservableCollection<EmployeeLeave> employeeLeaves;
        private int selectedIndexForCompanyShift;
        private bool isEditInit;
        private CompanyShift selectedCompanyShift;
        private CompanyShift companyShifDetails;

        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties

        private ObservableCollection<EmployeeAttendance> newEmployeeAttendanceList;
        public string WorkingPlantId { get; set; }
        public long IdEmployeeAttendance { get; private set; }
        public List<Company> SelectedPlantList { get; set; }

        public Company Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Company"));
            }
        }
        public ObservableCollection<EmployeeAttendance> NewEmployeeAttendanceList
        {
            get
            {
                return newEmployeeAttendanceList;
            }

            set
            {
                newEmployeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewEmployeeAttendanceList"));

            }
        }
        //public ObservableCollection<CompanyWork> AttendanceTypeList
        //{
        //    get
        //    {
        //        return attendanceTypeList;
        //    }

        //    set
        //    {
        //        attendanceTypeList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AttendanceTypeList"));

        //    }
        //}

        //public ObservableCollection<LookupValue> AttendanceTypeList
        //{
        //    get
        //    {
        //        return attendanceTypeList;
        //    }

        //    set
        //    {
        //        attendanceTypeList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AttendanceTypeList"));

        //    }
        //}

        public int SelectedIndexForEmployee
        {
            get
            {
                return selectedIndexForEmployee;
            }

            set
            {
                selectedIndexForEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEmployee"));
            }
        }

        public int SelectedIndexForAttendanceType
        {
            get
            {
                return selectedIndexForAttendanceType;
            }

            set
            {
                selectedIndexForAttendanceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
            }
        }
        public long SelectedPeriod
        {
            get
            {
                return selectedPeriod;
            }

            set
            {
                selectedPeriod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
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

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public ObservableCollection<EmployeeAttendance> ExistEmployeeAttendanceList
        {
            get
            {
                return existEmployeeAttendanceList;
            }

            set
            {
                existEmployeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeAttendanceList"));

            }
        }

        public ObservableCollection<Employee> EmployeeListFinal
        {
            get
            {
                return employeeListFinal;
            }

            set
            {
                employeeListFinal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeListFinal"));
            }
        }
        //private void FillFromDates()
        //{
        //    FromDates = new List<DateTime>();

        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        FromDates.Add(otherDate);
        //        addMinutes += 30;
        //    }

        //}

        //private void FillToDates()
        //{
        //    ToDates = new List<DateTime>();

        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        ToDates.Add(otherDate);
        //        addMinutes += 30;
        //    }
        //}

        //public List<DateTime> FromDates
        //{
        //    get { return fromDates; }
        //    set
        //    {
        //        if (value != fromDates)
        //        {
        //            fromDates = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("FromDates"));
        //        }
        //    }
        //}

        //public List<DateTime> ToDates
        //{
        //    get { return _ToDates; }
        //    set
        //    {
        //        if (value != _ToDates)
        //        {
        //            _ToDates = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("ToDates"));
        //        }
        //    }
        //}

        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        public DateTime? StartDate
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
        public DateTime? EndDate
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



        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }

        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));
            }
        }
        public TimeSpan STime
        {
            get
            {
                return sTime;
            }

            set
            {
                sTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("STime"));
            }
        }


        public TimeSpan ETime
        {
            get
            {
                return eTime;
            }

            set
            {
                eTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETime"));
            }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public bool Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Result"));
            }
        }

        public ObservableCollection<EmployeeLeave> EmployeeLeaves
        {
            get
            {
                return employeeLeaves;
            }

            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
        }

        public ObservableCollection<CompanyShift> CompanyShifts
        {
            get
            {
                return companyShifts;
            }

            set
            {
                companyShifts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifts"));

            }
        }
        public int SelectedIndexForCompanyShift
        {
            get
            {
                return selectedIndexForCompanyShift;
            }

            set
            {
                selectedIndexForCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));
            }
        }

        public bool IsEditInit
        {
            get
            {
                return isEditInit;
            }

            set
            {
                isEditInit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditInit"));
            }
        }

        public CompanyShift SelectedCompanyShift
        {
            get
            {
                return selectedCompanyShift;
            }

            set
            {
                selectedCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyShift"));
            }
        }

        public CompanyShift CompanyShifDetails
        {
            get
            {
                return companyShifDetails;
            }

            set
            {
                companyShifDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifDetails"));
            }
        }

        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }

        #endregion

        #region Public ICommand
        public ICommand AddAttendanceViewCancelButtonCommand { get; set; }
        public ICommand AddAttendanceViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }
        public ICommand SelectedIndexChangedForCompanyShiftCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public AddAttendanceViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);

                AddAttendanceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddAttendanceViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddAttendanceInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
                SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedCommandAction);
                SelectedIndexChangedForCompanyShiftCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedForCompanyShiftCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                //FillFromDates();
                //FillToDates();
                FillEmployeeWorkType();
                GeosApplication.Instance.Logger.Log("Constructor AddAttendanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddAttendanceViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


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

        #region Methods


        private void CloseWindow(object obj)
        {

            RequestClose(null, null);
        }

        /// <summary>
        /// Method to Add Attendance Information
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// </summary>
        /// <param name="obj"></param>
        private void AddAttendanceInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));
                if (error != null)
                {
                    return;
                }
                StartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                EndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                STime = StartTime.Value.TimeOfDay;
                ETime = EndDate.Value.TimeOfDay;

                List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
                if (IsNew)
                {
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                }
                else
                {
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date && x.IdEmployeeAttendance != IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();
                }


                bool IsLeave = true;
                bool IsAttendance = true;


                for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
                {

                    if (i == 0)
                    {
                        if (StartDate < ExistEmpAttendanceList[i].StartDate && EndDate <= ExistEmpAttendanceList[i].StartDate)
                        {
                            IsAttendance = true;
                            break;
                        }
                        if (ExistEmpAttendanceList.Count == 1)
                        {
                            if (StartDate >= ExistEmpAttendanceList[i].EndDate && EndDate > ExistEmpAttendanceList[i].EndDate)
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
                            if (StartDate >= ExistEmpAttendanceList[i - 1].EndDate && EndDate <= ExistEmpAttendanceList[i].StartDate)
                            {
                                IsAttendance = true;
                                break;
                            }
                            else if (i == ExistEmpAttendanceList.Count - 1)
                            {
                                if (StartDate >= ExistEmpAttendanceList[i].EndDate && EndDate > ExistEmpAttendanceList[i].StartDate)
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

                var ExistEmpLeaveList = EmployeeLeaves.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Value.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                for (int i = 0; i < ExistEmpLeaveList.Count; i++)
                {
                    if (ExistEmpLeaveList[i].IsAllDayEvent == 1)
                    {
                        IsLeave = false;
                        break;
                    }

                    if (i == 0)
                    {
                        if (StartDate < ExistEmpLeaveList[i].StartDate && EndDate <= ExistEmpLeaveList[i].StartDate)
                        {
                            IsLeave = true;
                            break;
                        }

                        if (ExistEmpLeaveList.Count == 1)
                        {
                            if (StartDate >= ExistEmpLeaveList[i].EndDate && EndDate > ExistEmpLeaveList[i].EndDate)
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
                            if (StartDate >= ExistEmpLeaveList[i - 1].EndDate && EndDate <= ExistEmpLeaveList[i].StartDate)
                            {
                                IsLeave = true;
                                break;
                            }
                            else if (i == ExistEmpLeaveList.Count - 1)
                            {
                                if (StartDate >= ExistEmpLeaveList[i].EndDate && EndDate > ExistEmpLeaveList[i].EndDate)
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

                if (IsNew == true)
                {
                    NewEmployeeAttendance = new EmployeeAttendance()
                    {
                        Employee = EmployeeListFinal[SelectedIndexForEmployee],
                        IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                        StartDate = (DateTime)StartDate,
                        EndDate = (DateTime)EndDate,
                        //[001] code comment
                        //CompanyWork = AttendanceTypeList[SelectedIndexForAttendanceType],
                        IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue,
                        IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
                    };

                    NewEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    DateTime tempEndDate = NewEmployeeAttendance.EndDate;
                    for (var item = NewEmployeeAttendance.StartDate; item <= tempEndDate; item = NewEmployeeAttendance.StartDate.AddDays(1))
                    {

                        NewEmployeeAttendance.StartDate = item;
                        NewEmployeeAttendance.EndDate = NewEmployeeAttendance.StartDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                        NewEmployeeAttendance.Employee.EmployeeJobDescription.Company = (SelectedPlantList.Where(x => x.IdCompany == NewEmployeeAttendance.Employee.EmployeeJobDescription.IdCompany).ToList())[0];
                        NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                        IsSave = true;
                        var updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
                        NewEmployeeAttendanceList.Add(updateEmployeeAttendance);

                    }
                    CompanyShifDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(NewEmployeeAttendance.IdCompanyShift));
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    UpdateEmployeeAttendance = new EmployeeAttendance()
                    {
                        Employee = EmployeeListFinal[SelectedIndexForEmployee],
                        IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                        StartDate = (DateTime)StartDate,
                        EndDate = (DateTime)EndDate,
                        //[001] code Comment
                        //CompanyWork = AttendanceTypeList[SelectedIndexForAttendanceType],
                        IdEmployeeAttendance = IdEmployeeAttendance,
                        IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue,
                        IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift
                    };
                    UpdateEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                    isSave = true;
                    Result = HrmService.UpdateEmployeeAttendance_V2036(UpdateEmployeeAttendance);
                    UpdateEmployeeAttendance.Employee.TotalWorkedHours = (UpdateEmployeeAttendance.EndDate - UpdateEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                    CompanyShifDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdateEmployeeAttendance.IdCompanyShift));
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAttendanceInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to check Date validation  
        /// </summary>
        /// <param name="obj"></param>
        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startDateErrorMessage = string.Empty;


                if (StartDate != null && EndDate != null)
                {


                    if (StartDate.Value.Date > EndDate.Value.Date)
                    {
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndDateError").ToString();
                    }
                    else
                    {
                        startDateErrorMessage = string.Empty;
                        endDateErrorMessage = string.Empty;
                    }

                }
                else
                {
                    startDateErrorMessage = string.Empty;
                    endDateErrorMessage = string.Empty;
                }

                CheckDateTimeValidation();
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to check Time validation 
        /// </summary>
        /// <param name="obj"></param>
        private void OnTimeEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging ...", category: Category.Info, priority: Priority.Low);


                startTimeErrorMessage = string.Empty;

                if (StartTime != null && EndTime != null)
                {

                    TimeSpan _StartTime = StartTime.Value.TimeOfDay;
                    TimeSpan _EndTime = EndTime.Value.TimeOfDay;
                    if (_StartTime > _EndTime)
                    {
                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndTimeError").ToString();
                    }
                    else
                    {
                        startTimeErrorMessage = string.Empty;
                        endTimeErrorMessage = string.Empty;
                    }
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }


                CheckDateTimeValidation();
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Check Date Time Validation
        /// </summary>
        public void CheckDateTimeValidation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);

                if (StartDate != null && EndDate != null && StartTime != null && EndTime != null)
                {
                    DateTime _TempStartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                    DateTime _TimeEndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                    if (_TempStartDate > _TimeEndDate)
                    {

                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndTimeError").ToString();

                    }
                    else
                    {
                        TimeSpan _StartTime = StartTime.Value.TimeOfDay;
                        TimeSpan _EndTime = EndTime.Value.TimeOfDay;
                        if (_StartTime > _EndTime)
                        {
                            startTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartTimeError").ToString();
                            endTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndTimeError").ToString();
                        }
                        else
                        {
                            startTimeErrorMessage = string.Empty;
                            endTimeErrorMessage = string.Empty;
                        }

                    }
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }



                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));

                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// </summary>
        /// <param name="employeeAttendanceList"></param>
        /// <param name="selectedEmployee"></param>
        /// <param name="selectedStartDate"></param>
        /// <param name="selectedEndDate"></param>
        public void Init(ObservableCollection<EmployeeAttendance> employeeAttendanceList, object selectedEmployee, DateTime selectedStartDate, DateTime selectedEndDate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                IsEditInit = false;
                ExistEmployeeAttendanceList = employeeAttendanceList;
                EmployeeListFinal = new ObservableCollection<Employee>();
                foreach (var item in SelectedPlantList)
                {
                    // [002] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2031 to GetAllEmployeesForAttendanceByIdCompany_V2039
                    var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2041(item.IdCompany.ToString(), SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission)).OrderBy(x => x.FullName);
                    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                }

                EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });

                Employee obj = selectedEmployee as Employee;

                if (obj != null)
                    SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == obj.IdEmployee));

                if (selectedStartDate != DateTime.MinValue && selectedEndDate != DateTime.MinValue)
                {
                    StartDate = selectedStartDate;
                    EndDate = selectedEndDate;
                    //StartTime = FromDates[0];
                    //EndTime = ToDates[0];
                    StartTime = Convert.ToDateTime(StartDate.ToString());
                    EndTime = Convert.ToDateTime(EndDate.ToString());
                }

                else
                {
                    //StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                    //EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
                    //if (StartTime == DateTime.MinValue && EndTime == DateTime.MinValue)
                    //{
                    //    StartTime = FromDates[0];
                    //    EndTime = ToDates[0];
                    //    if (StartDate == selectedEndDate.AddDays(-1))
                    //        EndDate = selectedEndDate.AddDays(-1);
                    //}
                    StartTime = Convert.ToDateTime(StartDate.ToString());
                    EndTime = Convert.ToDateTime(EndDate.ToString());
                }
                //Company = Company;

                //[001] code comment
                //AttendanceTypeList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorksByIdCompany(WorkingPlantId));
                //AttendanceTypeList.Insert(0, new CompanyWork() { Name = "---", IdCompanyWork = 0 });




                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to edit Employee Attendance Information
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// </summary>
        /// <param name="SelectedEmployeeAttendance"></param>
        public void EditInit(EmployeeAttendance SelectedEmployeeAttendance, ObservableCollection<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                IsEditInit = true;
                EmployeeListFinal = new ObservableCollection<Employee>();
                foreach (var item in SelectedPlantList)
                {
                    // [002] Changed service method GetAllEmployeesForAttendanceByIdCompany_V2031 to GetAllEmployeesForAttendanceByIdCompany_V2039
                    var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2041(item.IdCompany.ToString(), SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission)).OrderBy(x => x.FullName);
                    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                }
                EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });

                //[001] code Comment
                //AttendanceTypeList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorksByIdCompany(WorkingPlantId));
                //AttendanceTypeList.Insert(0, new CompanyWork() { Name = "---", IdCompanyWork = 0 });
                //SelectedIndexForAttendanceType = AttendanceTypeList.FindIndex(x => x.IdCompanyWork == SelectedEmployeeAttendance.IdCompanyWork);


                SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.ToList().FindIndex(x => x.IdLookupValue == SelectedEmployeeAttendance.IdCompanyWork);

                SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == SelectedEmployeeAttendance.IdEmployee));

                StartDate = SelectedEmployeeAttendance.StartDate;
                EndDate = SelectedEmployeeAttendance.EndDate;

                //if (FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute) == -1)
                //    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour)];
                //else
                //    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];

                //if (ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute) == -1)
                //    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour)];
                //else
                //    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];

                StartTime = Convert.ToDateTime(SelectedEmployeeAttendance.StartTime.ToString());
                EndTime = Convert.ToDateTime(SelectedEmployeeAttendance.EndTime.ToString());

                IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
                ExistEmployeeAttendanceList = employeeAttendanceList;
                CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));

                if (CompanyShifts.Any(x => x.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
                    SelectedIndexForCompanyShift = CompanyShifts.IndexOf(CompanyShifts.FirstOrDefault(x => x.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>
        public void InitReadOnly(EmployeeAttendance SelectedEmployeeAttendance)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                IsEditInit = true;
                EmployeeListFinal = new ObservableCollection<Employee>();
                EmployeeListFinal.Add(SelectedEmployeeAttendance.Employee);

                SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.ToList().FindIndex(x => x.IdLookupValue == SelectedEmployeeAttendance.IdCompanyWork);

                SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == SelectedEmployeeAttendance.IdEmployee));

                StartDate = SelectedEmployeeAttendance.StartDate;
                EndDate = SelectedEmployeeAttendance.EndDate;
                //StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                //EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
                StartTime = Convert.ToDateTime(StartDate.ToString());
                EndTime = Convert.ToDateTime(EndDate.ToString());
                IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
                CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
                SelectedIndexForCompanyShift = CompanyShifts.IndexOf(CompanyShifts.FirstOrDefault(x => x.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to fill EmployeeWork from Lookup values
        /// </summary>
        public void FillEmployeeWorkType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeWorkType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.AttendanceTypeList == null)
                {
                    GeosApplication.Instance.AttendanceTypeList = new ObservableCollection<LookupValue>(CrmService.GetLookupValues(33));
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedIndexChangedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (SelectedIndexForEmployee > 0)
                {
                    if (!IsEditInit)
                    {
                        var SelectedEmployee = EmployeeListFinal[SelectedIndexForEmployee];
                        CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployee.IdEmployee));

                    }

                }
                else
                {
                    CompanyShifts = new ObservableCollection<CompanyShift>();
                    CompanyShift companyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
                    CompanyShifts.Insert(0, companyShift);
                    SelectedIndexForCompanyShift = 0;
                    SelectedCompanyShift = companyShift;
                }
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedIndexChangedForCompanyShiftCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                SelectedCompanyShift = CompanyShifts[SelectedIndexForCompanyShift];
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                  me[BindableBase.GetPropertyName(() => SelectedIndexForEmployee)] +
                  me[BindableBase.GetPropertyName(() => StartDate)] +
                me[BindableBase.GetPropertyName(() => EndDate)] +
                me[BindableBase.GetPropertyName(() => StartTime)] +
                me[BindableBase.GetPropertyName(() => EndTime)] +
                me[BindableBase.GetPropertyName(() => SelectedIndexForAttendanceType)] +
                me[BindableBase.GetPropertyName(() => SelectedIndexForCompanyShift)];

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
                string empName = BindableBase.GetPropertyName(() => SelectedIndexForEmployee);
                string attendanceStartDate = BindableBase.GetPropertyName(() => StartDate);
                string attendanceEndDate = BindableBase.GetPropertyName(() => EndDate);
                string attendanceStartTime = BindableBase.GetPropertyName(() => StartTime);
                string attendanceEndTime = BindableBase.GetPropertyName(() => EndTime);
                string type = BindableBase.GetPropertyName(() => SelectedIndexForAttendanceType);
                string shift = BindableBase.GetPropertyName(() => SelectedIndexForCompanyShift);

                if (columnName == empName)
                {
                    return AttendanceValidation.GetErrorMessage(empName, SelectedIndexForEmployee);
                }
                if (columnName == attendanceStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(attendanceStartDate, StartDate);
                    }
                }

                if (columnName == attendanceEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(attendanceEndDate, EndDate);
                    }
                }

                if (columnName == attendanceStartTime)
                {
                    if (!string.IsNullOrEmpty(startTimeErrorMessage))
                    {
                        return startTimeErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(attendanceStartTime, StartTime);
                    }
                }

                if (columnName == attendanceEndTime)
                {
                    if (!string.IsNullOrEmpty(endTimeErrorMessage))
                    {
                        return endTimeErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(attendanceEndTime, EndTime);
                    }
                }

                if (columnName == attendanceStartDate)
                {
                    return AttendanceValidation.GetErrorMessage(attendanceStartDate, StartDate);
                }

                if (columnName == attendanceEndDate)
                {
                    return AttendanceValidation.GetErrorMessage(attendanceEndDate, EndDate);
                }
                if (columnName == type)
                {
                    return AttendanceValidation.GetErrorMessage(type, SelectedIndexForAttendanceType);
                }
                if (columnName == shift)
                {
                    return AttendanceValidation.GetErrorMessage(shift, SelectedIndexForCompanyShift);
                }

                return null;
            }
        }





        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }
    }
}
