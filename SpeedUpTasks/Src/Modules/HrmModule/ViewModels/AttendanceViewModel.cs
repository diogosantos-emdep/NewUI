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
using System.Data;
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
using Emdep.Geos.Modules.Hrm.Views;


namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AttendanceViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region TaskLog
        // [001][HRM M042-06][sdesai] Add and Edit Attendance.
        // [002][GEOS Workbench / GEOS2-1805][09-10-2019][sdesai][New employee shift values (new table) in attendance [#IES14]]
        #endregion

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isNew;
        //private List<DateTime> fromDates;
        private string timeEditMask;
        private bool flag;
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
        // private ObservableCollection<CompanyShift> companyShifts;
        private ObservableCollection<EmployeeLeave> employeeLeaves;
        private int selectedIndexForCompanyShift;
        private bool isEditInit;
        // private CompanyShift selectedCompanyShift;
        private CompanyShift companyShifDetails;
        private ObservableCollection<EmployeeAttendance> newEmployeeAttendanceList;
        private DateTime? accountingDate;
        private bool isValidation;
        // [002]
        private ObservableCollection<EmployeeShift> employeeShiftList;
        private EmployeeShift selectedEmployeeShift;

        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private bool isAcceptButton;
        #endregion

        #region Properties
        public string WorkingPlantId { get; set; }
        public long IdEmployeeAttendance { get; private set; }
        public List<Company> SelectedPlantList { get; set; }

        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Company"));
            }
        }

        public ObservableCollection<EmployeeAttendance> NewEmployeeAttendanceList
        {
            get { return newEmployeeAttendanceList; }
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
            get { return selectedIndexForEmployee; }
            set
            {
                selectedIndexForEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEmployee"));
            }
        }

        public int SelectedIndexForAttendanceType
        {
            get { return selectedIndexForAttendanceType; }
            set
            {
                selectedIndexForAttendanceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
            }
        }

        public long SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                selectedPeriod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
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

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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

        public ObservableCollection<Employee> EmployeeListFinal
        {
            get { return employeeListFinal; }
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
            get { return timeEditMask; }
            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
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

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
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

        public ObservableCollection<EmployeeLeave> EmployeeLeaves
        {
            get { return employeeLeaves; }
            set
            {
                employeeLeaves = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
            }
        }

        //public ObservableCollection<CompanyShift> CompanyShifts
        //{
        //    get { return companyShifts; }
        //    set
        //    {
        //        companyShifts = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifts"));
        //    }
        //}

        public int SelectedIndexForCompanyShift
        {
            get { return selectedIndexForCompanyShift; }
            set
            {
                selectedIndexForCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));

                //if (SelectedIndexForCompanyShift > -1)
                //{
                //    if (SelectedIndexForCompanyShift == EmployeeShiftList.
                //}
            }
        }

        public bool IsEditInit
        {
            get { return isEditInit; }
            set
            {
                isEditInit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditInit"));
            }
        }

        //public CompanyShift SelectedCompanyShift
        //{
        //    get { return selectedCompanyShift; }
        //    set
        //    {
        //        selectedCompanyShift = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyShift"));
        //    }
        //}

        public CompanyShift CompanyShiftDetails
        {
            get { return companyShifDetails; }
            set
            {
                companyShifDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifDetails"));
            }
        }

        private bool _addNewTaskbtnVisibility;
        public bool AddNewTaskbtnVisibility
        {
            get { return _addNewTaskbtnVisibility; }
            set { _addNewTaskbtnVisibility = value; }
        }
        public DateTime? AccountingDate
        {
            get
            {
                return accountingDate;
            }

            set
            {
                accountingDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccountingDate"));
            }
        }
        public ObservableCollection<EmployeeShift> EmployeeShiftList
        {
            get
            {
                return employeeShiftList;
            }

            set
            {
                employeeShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList"));
            }
        }
        public EmployeeShift SelectedEmployeeShift
        {
            get
            {
                return selectedEmployeeShift;
            }

            set
            {
                selectedEmployeeShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeShift"));
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

        public bool IsAcceptButton
        {
            get { return isAcceptButton; }
            set
            {
                isAcceptButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButton"));
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

        public ICommand SplitButtonCommand { get; set; }
        public ICommand SplitAttendanceViewAcceptButtonCommand { get; set; }
        public ICommand SplitAttendanceViewCancelButtonCommand { get; set; }
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand CloseTaskCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public AttendanceViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);
                SetUserPermission();
                AddAttendanceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddAttendanceViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddAttendanceInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
                SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedCommandAction);
                SelectedIndexChangedForCompanyShiftCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedForCompanyShiftCommandAction);

                //Split commands.
                SplitButtonCommand = new RelayCommand(new Action<object>(SplitAttendanceInformation));
                SplitAttendanceViewAcceptButtonCommand = new DelegateCommand<object>(SaveAttendanceWithSplit);
                SplitAttendanceViewCancelButtonCommand = new DelegateCommand<object>(CloseAttemdanceWindowForSplit);
                AddNewTaskCommand = new RelayCommand(new Action<object>(AddNewSplitTaskCommandAction));
                CloseTaskCommand = new DelegateCommand<Task>(CloseTask, CanCloseTask);
                SelectionChangedCommand = new RelayCommand(new Action<object>(SelectionChangedCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

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
        /// [002][skale][31/01/2020][GEOS2-1959]- GHRM - Leaves/Attendance inactive employees
        /// </summary>
        /// <param name="obj"></param>
        private void AddAttendanceInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();

                IsAcceptButton = true;

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
                //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
                //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);
                STime = StartTime.Value.TimeOfDay;
                ETime = EndDate.Value.TimeOfDay;

                List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
                //ExistEmpAttendanceList.FirstOrDefault().is
                if (IsNew)
                {
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                    //if(ExistEmpAttendanceList.Count == 0)
                    //{
                    //    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.EndDate.Date == StartDate.Value.Date).OrderBy(x => x.EndDate).ToList();
                    //}
                }
                else
                {
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date && x.IdEmployeeAttendance != IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();
                }

                if (ExistEmpAttendanceList.Count() > 0)
                {
                    if (ExistEmpAttendanceList.FirstOrDefault().StartDate.Date == StartDate.Value.Date && ExistEmpAttendanceList.FirstOrDefault().EndDate.Date == EndDate.Value.Date && ExistEmpAttendanceList.FirstOrDefault().IdCompanyShift != EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceAlreadyAddedShift").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
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
                //[002] added
                bool IsAddInActiveEmployeeAttendance = true;
                Employee Employee = EmployeeListFinal[SelectedIndexForEmployee];
                if (Employee != null)
                {
                    if (Employee.EmployeeContractSituations != null && Employee.EmployeeContractSituations.Count > 0)
                    {
                        for (int i = 0; i < Employee.EmployeeContractSituations.Count; i++)
                        {
                            if (Employee.EmployeeContractSituations[i].ContractSituationEndDate == null)
                            {

                                DateTime? EmployeeContractEndDate = Employee.EmployeeContractSituations[i].ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : Employee.EmployeeContractSituations[i].ContractSituationEndDate;

                                if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= EmployeeContractEndDate.Value.Date)
                                {
                                    IsAddInActiveEmployeeAttendance = true;
                                    break;
                                }
                                else
                                    IsAddInActiveEmployeeAttendance = false;

                            }
                            else if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= Employee.EmployeeContractSituations[i].ContractSituationEndDate.Value.Date)
                            {
                                IsAddInActiveEmployeeAttendance = true;
                                break;
                            }
                            else
                            {
                                IsAddInActiveEmployeeAttendance = false;
                            }
                        }
                        if (!IsAddInActiveEmployeeAttendance)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessagforInactiveEmployeeAttendance").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessagforInactiveEmployeeAttendance").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                //end

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
                        //IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
                        IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                        CurrentContractForAttendance = EmployeeListFinal[SelectedIndexForEmployee].EmployeeContractSituations.Where(ecs => ecs.ContractSituationStartDate.Value.Date <= (DateTime)StartDate.Value.Date).Select(i => i.Company.Alias).LastOrDefault(),

                    };

                    NewEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    DateTime tempEndDate = NewEmployeeAttendance.EndDate;
                    //if (CompanyShifts[SelectedIndexForCompanyShift].IsNightShift == 0 || ((NewEmployeeAttendance.EndDate.Date- NewEmployeeAttendance.StartDate.Date).TotalDays) <= 0)
                    if (EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IsNightShift == 0 || ((NewEmployeeAttendance.EndDate.Date - NewEmployeeAttendance.StartDate.Date).TotalDays) <= 0)
                    {
                        for (var item = NewEmployeeAttendance.StartDate; item.Date <= tempEndDate.Date; item = NewEmployeeAttendance.StartDate.AddDays(1))
                        {

                            NewEmployeeAttendance.StartDate = item;
                            NewEmployeeAttendance.EndDate = NewEmployeeAttendance.StartDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                            // NewEmployeeAttendance.Employee.EmployeeJobDescription.Company = NewEmployeeAttendance.Employee.EmployeeContractSituation.Company;

                            //NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                            NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                            NewEmployeeAttendance.AccountingDate = item;
                            NewEmployeeAttendance.IsManual = 1; 
                            IsSave = true;
                            //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
                            EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
                            NewEmployeeAttendanceList.Add(updateEmployeeAttendance);

                        }
                    }
                    else
                    {

                        //if(((NewEmployeeAttendance.EndDate.Date - NewEmployeeAttendance.StartDate.Date).TotalDays) > 1)
                        //{
                        //    tempEndDate = tempEndDate.AddDays(1);
                        //}

                        for (var item = NewEmployeeAttendance.StartDate; item <= tempEndDate; item = NewEmployeeAttendance.StartDate.AddDays(1))
                        {

                            NewEmployeeAttendance.StartDate = item;
                            DateTime _endDate = item.AddDays(1);
                            NewEmployeeAttendance.EndDate = _endDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);

                            // NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                            NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                            NewEmployeeAttendance.AccountingDate = item;
                            NewEmployeeAttendance.IsManual = 1;
                            IsSave = true;
                            //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
                            EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
                            NewEmployeeAttendanceList.Add(updateEmployeeAttendance);

                        }
                    }
                    CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(NewEmployeeAttendance.IdCompanyShift));
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    UpdateEmployeeAttendance = new EmployeeAttendance()
                    {
                        Employee = EmployeeListFinal[SelectedIndexForEmployee],
                        CurrentContractForAttendance = EmployeeListFinal[SelectedIndexForEmployee].EmployeeContractSituations.Where(ecs => ecs.ContractSituationStartDate.Value.Date <= (DateTime)StartDate.Value.Date).Select(i => i.Company.Alias).LastOrDefault(),
                        IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                        StartDate = (DateTime)StartDate,
                        EndDate = (DateTime)EndDate,
                        //[001] code Comment
                        //CompanyWork = AttendanceTypeList[SelectedIndexForAttendanceType],
                        IdEmployeeAttendance = IdEmployeeAttendance,
                        IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue,
                        //IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
                        IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                        AccountingDate = (DateTime)StartDate
                    };
                    // UpdateEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                    UpdateEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                   
                    IsSave = true;
                    Result = HrmService.UpdateEmployeeAttendance_V2036(UpdateEmployeeAttendance);
                    UpdateEmployeeAttendance.Employee.TotalWorkedHours = (UpdateEmployeeAttendance.EndDate - UpdateEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                    CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdateEmployeeAttendance.IdCompanyShift));
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

                //sjadhav
                if (flag)
                {
                    StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                    EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
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
                    if (SelectedEmployeeShift.CompanyShift.IsNightShift == 0)//if (SelectedCompanyShift.IsNightShift == 0)
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
                    if (_TempStartDate.Date > _TimeEndDate.Date)
                    {
                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndTimeError").ToString();
                    }
                    else
                    {
                        if (SelectedEmployeeShift.CompanyShift.IsNightShift == 0) //if (SelectedCompanyShift.IsNightShift == 0)
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
        /// [002][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>
        /// <param name="employeeAttendanceList"></param>
        /// <param name="selectedEmployee"></param>
        /// <param name="selectedStartDate"></param>
        /// <param name="selectedEndDate"></param>
        public void Init(ObservableCollection<EmployeeAttendance> employeeAttendanceList, object selectedEmployee, DateTime selectedStartDate, DateTime selectedEndDate, ObservableCollection<EmployeeLeave> employeeLeavesList=null, ObservableCollection<Employee> employeeListFinal = null)
        {
            try
            {
                
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                IsEditInit = false;
                ExistEmployeeAttendanceList = employeeAttendanceList;

                if (employeeLeavesList != null)
                {
                    if (EmployeeLeaves == null)
                    {
                        EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
                        EmployeeLeaves = employeeLeavesList;
                    }
                   
                }
                if(employeeListFinal == null)
                {
                    employeeListFinal = new ObservableCollection<Employee>();
                    foreach (var item in SelectedPlantList)
                    {

                        ObservableCollection<Employee> tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] Added
                        employeeListFinal.AddRange(tempEmployeeListFinal);
                    }

                }
                else
                {
                    EmployeeListFinal = employeeListFinal;
                }

                //EmployeeListFinal = new ObservableCollection<Employee>();
                //foreach (var item in SelectedPlantList)
                //{

                //    //ObservableCollection<Employee> tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] Added
                //    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                //}

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
        /// [002] [avpawar][2020-07-24][GEOS2-2432] To solve the error message when try edit the attendance shift.
        /// </summary>
        /// <param name="SelectedEmployeeAttendance"></param>
        public void EditInit(EmployeeAttendance SelectedEmployeeAttendance, ObservableCollection<EmployeeAttendance> employeeAttendanceList,
                             ObservableCollection<Employee> employeeListFinal)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                IsEditInit = true;
                if(employeeListFinal == null)
                {
                    EmployeeListFinal = new ObservableCollection<Employee>();
                    foreach (var item in SelectedPlantList)
                    {
                        var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                        EmployeeListFinal.AddRange(tempEmployeeListFinal);
                    }
                    EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                    EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });
                }
                else
                {
                    EmployeeListFinal = employeeListFinal;
                }
                //EmployeeListFinal = new ObservableCollection<Employee>();
                //foreach (var item in SelectedPlantList)
                //{
                //    var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                //    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                //}
                //EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                //EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });

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

                StartTime = Convert.ToDateTime(SelectedEmployeeAttendance.StartDate.ToString());
                EndTime = Convert.ToDateTime(SelectedEmployeeAttendance.EndDate.ToString());

                IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
                ExistEmployeeAttendanceList = employeeAttendanceList;
                // CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
                EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployeeAttendance.IdEmployee));

                //[002] start
                if (EmployeeShiftList != null)
                {
                    if (!EmployeeShiftList.Any(a => a.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
                    {
                        EmployeeShift ObjShift = new EmployeeShift();
                        ObjShift.IdCompanyShift = SelectedEmployeeAttendance.IdCompanyShift;
                        ObjShift.CompanyShift = SelectedEmployeeAttendance.CompanyShift;
                        EmployeeShiftList.Add(ObjShift);
                        ObjShift.IsEnabled = false;
                    }
                }
                //[002] End

                if (EmployeeShiftList.Any(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
                SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));

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
                //if(employeeListFinal == null)
                //{
                //    EmployeeListFinal = new ObservableCollection<Employee>();
                //    EmployeeListFinal.Add(SelectedEmployeeAttendance.Employee);
                //}
                //else
                //{
                //    EmployeeListFinal = employeeListFinal;
                //}
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
                //CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
                //SelectedIndexForCompanyShift = CompanyShifts.IndexOf(CompanyShifts.FirstOrDefault(x => x.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));

                EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployeeAttendance.IdEmployee));
                if (EmployeeShiftList.Any(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
                    SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));
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
                    GeosApplication.Instance.AttendanceTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
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
                        //CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployee.IdEmployee));
                        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployee.IdEmployee));
                        if (EmployeeShiftList.Count == 2)
                            SelectedIndexForCompanyShift = 1;

                    }
                }
                else
                {
                    //CompanyShifts = new ObservableCollection<CompanyShift>();
                    //CompanyShift companyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
                    //CompanyShifts.Insert(0, companyShift);
                    //SelectedIndexForCompanyShift = 0;
                    //SelectedCompanyShift = companyShift;

                    EmployeeShiftList = new ObservableCollection<EmployeeShift>();
                    EmployeeShift tempEmployeeShift = new EmployeeShift();
                    tempEmployeeShift.CompanyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
                    EmployeeShiftList.Insert(0, tempEmployeeShift);
                    SelectedIndexForCompanyShift = 0;
                    SelectedEmployeeShift = tempEmployeeShift;
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
                //SelectedCompanyShift = CompanyShifts[SelectedIndexForCompanyShift];
                SelectedEmployeeShift = EmployeeShiftList[SelectedIndexForCompanyShift];

                if (flag)
                {
                    StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                    EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                }
                flag = true;

                if (isValidation)
                    CheckDateTimeValidation();
                isValidation = true;
                
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetUserPermission()
        {

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

        #region Split attendance logic

        private ObservableCollection<Task> tasks = new ObservableCollection<Task>();
        private bool isSplit;
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tasks"));
            }
        }

        private int selectedViewIndex;
        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }

        //private bool isBusy;
        //public bool IsBusy
        //{
        //    get { return isBusy; }
        //    set
        //    {
        //        isBusy = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
        //    }
        //}

        bool isSplitVisible = true;

        public bool IsSplitVisible
        {
            get { return isSplitVisible; }
            set
            {
                isSplitVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSplitVisible"));
            }
        }
        public bool IsSplit
        {
            get
            {
                return isSplit;
            }

            set
            {
                isSplit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSplit"));
            }
        }

        /// <summary>
        /// [001][HRM GEOS2-1642][avpawar] Improvement Split Option.
        /// </summary>
        /// <param name="obj"></param>
        private void SplitAttendanceInformation(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow ...", category: Category.Info, priority: Priority.Low);
            EmployeeAttendance selectedEmpAttendance = new EmployeeAttendance();

            SelectedViewIndex = 1;
            IsSplitVisible = false;
            AddNewTaskbtnVisibility = false;

            STime = StartTime.Value.TimeOfDay;
            ETime = EndTime.Value.TimeOfDay;

            DateTime? _StartTime = null;

            DateTime? _EndTime = null;

            //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
            //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);

            DateTime shiftStartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, SelectedEmployeeShift, true);
            DateTime shiftEndTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, SelectedEmployeeShift, false);

            int _IndexForAttendanceType = 0;

            AccountingDate = StartDate;

            if (StartTime > shiftStartTime && EndTime > shiftEndTime && StartTime < shiftEndTime)
            {
                _StartTime = StartTime;
                _EndTime = shiftEndTime;
            }

            else if (StartTime > shiftStartTime)
            {
                _StartTime = StartTime;
                _EndTime = EndTime;
            }

            else if (StartTime < shiftStartTime && EndTime < shiftStartTime)
            {
                _StartTime = StartTime;
                _EndTime = EndTime;
            }

            else if (StartTime > shiftEndTime && EndTime > shiftEndTime)
            {
                _StartTime = StartTime;
                _EndTime = EndTime;
            }

            else if (StartTime < shiftStartTime && EndTime < shiftEndTime && EndTime > shiftStartTime)
            {
                _StartTime = shiftStartTime;
                _EndTime = EndTime;
            }

            else if (StartTime <= shiftStartTime && EndTime > shiftEndTime)
            {
                _StartTime = shiftStartTime;
                _EndTime = shiftEndTime;
            }

            else if (StartTime < shiftStartTime && EndTime >= shiftEndTime)
            {
                _StartTime = shiftStartTime;
                _EndTime = shiftEndTime;
            }

            else
            {
                _StartTime = StartTime;
                _EndTime = EndTime;
            }

            Tasks.Add(new Task()
            {

                Name = "Header",
                Header = "Attendance1",
                IsComplete = false,
                IsNew = false,
                TaskEmployeeListFinal = EmployeeListFinal,
                IsEnabaledEmployee = false,
                SelectedIndexForEmployee = SelectedIndexForEmployee,
                //CompanyShifts = CompanyShifts,
                EmployeeShiftList = EmployeeShiftList,
                // TaskSelectedCompanyShift = SelectedCompanyShift,
                TaskSelectedEmployeeShift = SelectedEmployeeShift,
                SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                IsEnabledShift = false,

                TaskStartDate = _StartTime,
                TaskStartTime = _StartTime,
                //STime = STime,

                TaskEndDate = _EndTime,
                TaskEndTime = _EndTime,
                //ETime = ETime,

                SelectedIndexForAttendanceType = SelectedIndexForAttendanceType,
                TaskAccountingDate = AccountingDate
            });

            _IndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.IndexOf(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == 188));

            //if (StartTime > shiftStartTime && EndTime > shiftEndTime)
            //{
            //}
            //else if (StartTime < shiftStartTime && EndTime < shiftStartTime)
            // {   
            // }
            // else if (StartTime > shiftEndTime && EndTime > shiftEndTime)
            // { 
            // }
            //else if (StartTime < shiftStartTime && EndTime < shiftEndTime && EndTime > shiftStartTime)
            //{
            //}
            //else
            //{
            if (shiftEndTime.TimeOfDay < ETime && StartTime < shiftEndTime)
            {
                _StartTime = shiftStartTime;
                _EndTime = shiftEndTime;

                Tasks.Add(new Task()
                {

                    Name = "Header",
                    Header = "Attendance2",
                    IsComplete = true,
                    IsNew = true,
                    IsEnabaledEmployee = false,
                    TaskEmployeeListFinal = EmployeeListFinal,
                    SelectedIndexForEmployee = SelectedIndexForEmployee,
                    //CompanyShifts = CompanyShifts,
                    EmployeeShiftList = EmployeeShiftList,
                    // TaskSelectedCompanyShift = SelectedCompanyShift,
                    TaskSelectedEmployeeShift = SelectedEmployeeShift,
                    SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                    IsEnabledShift = false,

                    TaskStartDate = EndDate,
                    TaskStartTime = _EndTime,
                    //STime = STime,

                    TaskEndDate = EndDate,
                    TaskEndTime = EndTime,
                    //ETime = ETime,

                    SelectedIndexForAttendanceType = _IndexForAttendanceType,
                    TaskAccountingDate = AccountingDate
                });
            }

            //[001][avpawar] added
            if (STime < shiftStartTime.TimeOfDay && EndTime > shiftStartTime)
            {
                _StartTime = StartTime;
                _EndTime = shiftStartTime;

                string headerTitle = string.Empty;

                if (STime < shiftStartTime.TimeOfDay && shiftEndTime.TimeOfDay >= ETime)
                    headerTitle = "Attendance2";
                else
                    headerTitle = "Attendance3";

                Tasks.Add(new Task()
                {
                    Name = "Header",
                    Header = headerTitle,
                    IsComplete = true,
                    IsNew = true,
                    IsEnabaledEmployee = false,
                    TaskEmployeeListFinal = EmployeeListFinal,
                    SelectedIndexForEmployee = SelectedIndexForEmployee,
                    //CompanyShifts = CompanyShifts,
                    EmployeeShiftList = EmployeeShiftList,
                    //  TaskSelectedCompanyShift = SelectedCompanyShift,
                    TaskSelectedEmployeeShift = SelectedEmployeeShift,
                    SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                    IsEnabledShift = false,

                    TaskStartDate = StartDate,
                    TaskStartTime = StartTime,

                    TaskEndDate = _EndTime,
                    TaskEndTime = _EndTime,

                    SelectedIndexForAttendanceType = _IndexForAttendanceType,
                    TaskAccountingDate = AccountingDate
                });
            }
            //end

            GeosApplication.Instance.Logger.Log("Method SplitAttendanceInformation executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //}


        public EmployeeAttendance NewSplitEmployeeAttendance;
        public EmployeeAttendance UpdatesplitEmployeeAttendance;
        private void SaveAttendanceWithSplit(object obj)
        {
            try
            {

                bool IsLeave = true;
                bool IsAttendance = true;
                //This is called to check validations;
                foreach (var item in Tasks)
                {
                    string error = item.CheckValidation();
                    if (error != null)
                    {
                        return;
                    }
                }

                var ExistEmpLeaveList = EmployeeLeaves.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Value.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                foreach (var item in Tasks)
                {
                    var _StartDate = item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes);
                    var _EndDate = item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes);
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
                }

                if (IsLeave == false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
                ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date && x.IdEmployeeAttendance != IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();
                foreach (var item in Tasks)
                {
                    var _StartDate = item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes);
                    var _EndDate = item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes);
                    for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
                    {

                        if (i == 0)
                        {
                            if (_StartDate < ExistEmpAttendanceList[i].StartDate && _EndDate <= ExistEmpAttendanceList[i].StartDate)
                            {
                                IsAttendance = true;
                                break;
                            }
                            if (ExistEmpAttendanceList.Count == 1)
                            {
                                if (_StartDate >= ExistEmpAttendanceList[i].EndDate && _EndDate > ExistEmpAttendanceList[i].EndDate)
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
                                if (_StartDate >= ExistEmpAttendanceList[i - 1].EndDate && _EndDate <= ExistEmpAttendanceList[i].StartDate)
                                {
                                    IsAttendance = true;
                                    break;
                                }
                                else if (i == ExistEmpAttendanceList.Count - 1)
                                {
                                    if (_StartDate >= ExistEmpAttendanceList[i].EndDate && _EndDate > ExistEmpAttendanceList[i].StartDate)
                                    {
                                        IsAttendance = true;
                                        break;
                                    }
                                }
                            }
                        }

                        IsAttendance = false;
                    }
                }

                if (IsAttendance == false)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                for (int i = 0; i < Tasks.Count - 1; i++)
                {
                    var _TaskStartTime = Tasks[i].TaskStartDate.Value.AddHours(Tasks[i].TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i].TaskStartTime.Value.TimeOfDay.Minutes);
                    var _TaskEndTime = Tasks[i].TaskEndDate.Value.AddHours(Tasks[i].TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i].TaskEndTime.Value.TimeOfDay.Minutes);

                    var _StartTime = Tasks[i + 1].TaskStartDate.Value.AddHours(Tasks[i + 1].TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i + 1].TaskStartTime.Value.TimeOfDay.Minutes);
                    var _EndTime = Tasks[i + 1].TaskEndDate.Value.AddHours(Tasks[i + 1].TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i + 1].TaskEndTime.Value.TimeOfDay.Minutes);

                    if (_TaskStartTime == _StartTime && _TaskEndTime == _EndTime)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    else if (_StartTime < _TaskStartTime && Tasks[i + 1].TaskStartTime < _TaskEndTime)
                    {
                        if (_EndTime > _TaskStartTime)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                    else if (_StartTime < _TaskEndTime)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                }

                NewEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                //This is used to update current entry and add one extra for overtime.
                foreach (var item in Tasks)
                {
                    //string error = item.CheckValidation();
                    //if (error != null)
                    //{
                    //    return;
                    //}

                    if (item.IsNew == true)
                    {
                        NewSplitEmployeeAttendance = new EmployeeAttendance()
                        {
                            Employee = item.TaskEmployeeListFinal[SelectedIndexForEmployee],  // EmployeeListFinal[SelectedIndexForEmployee],
                            IdEmployee = item.TaskEmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                            StartDate = item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes),
                            EndDate = item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes),

                            IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[item.SelectedIndexForAttendanceType].IdLookupValue,
                            IdCompanyShift = item.EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,// CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
                            AccountingDate = item.TaskAccountingDate
                        };
                        //DateTime tempEndDate = NewSplitEmployeeAttendance.EndDate;
                        //NewSplitEmployeeAttendance.StartDate = (DateTime)item.TaskStartDate;
                        //NewSplitEmployeeAttendance.EndDate = (DateTime)item.TaskEndDate;

                        //NewSplitEmployeeAttendance.Employee.EmployeeJobDescription.Company = NewSplitEmployeeAttendance.Employee.EmployeeContractSituation.Company;

                        //NewSplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedCompanyShift;
                        NewSplitEmployeeAttendance.IsManual = 1;
                        NewSplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedEmployeeShift.CompanyShift;
                        NewSplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == NewSplitEmployeeAttendance.IdCompanyWork));
                        //EmployeeAttendance addEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewSplitEmployeeAttendance);
                        EmployeeAttendance addEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewSplitEmployeeAttendance);
                        CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(NewSplitEmployeeAttendance.IdCompanyShift));
                        NewEmployeeAttendanceList.Add(addEmployeeAttendance);
                    }
                    else
                    {
                        UpdatesplitEmployeeAttendance = new EmployeeAttendance()
                        {
                            Employee = item.TaskEmployeeListFinal[SelectedIndexForEmployee],
                            IdEmployee = item.TaskEmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                            StartDate = item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes),
                            EndDate = item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes),
                            IdEmployeeAttendance = IdEmployeeAttendance,
                            IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[item.SelectedIndexForAttendanceType].IdLookupValue,
                            IdCompanyShift = item.EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                            AccountingDate = item.TaskAccountingDate
                        };
                        if(ExistEmployeeAttendanceList.Any(a => a.IdEmployeeAttendance == IdEmployeeAttendance))
                        UpdatesplitEmployeeAttendance.IsManual = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == IdEmployeeAttendance).FirstOrDefault().IsManual;

                        //UpdatesplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedCompanyShift;
                        UpdatesplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedEmployeeShift.CompanyShift;
                        UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));
                        Result = HrmService.UpdateEmployeeAttendance_V2036(UpdatesplitEmployeeAttendance);
                        UpdatesplitEmployeeAttendance.Employee.TotalWorkedHours = (UpdatesplitEmployeeAttendance.EndDate - UpdatesplitEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                        CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdatesplitEmployeeAttendance.IdCompanyShift));
                        NewEmployeeAttendanceList.Add(UpdatesplitEmployeeAttendance);
                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SplitEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                IsSave = true;
                IsSplit = true;
                RequestClose(null, null);
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

        private void CloseAttemdanceWindowForSplit(object obj)
        {
            SelectedViewIndex = 0;
            IsSplitVisible = true;
            foreach (Task task in Tasks.ToList())
            {
                Tasks.Remove(task);
            }
        }

        private void AddNewSplitTaskCommandAction(object obj)
        {
            int count = Tasks.Count + 1;

            Tasks.Add(new Task()
            {
                Name = "Header",
                Header = "Attendance" + count.ToString(),
                IsComplete = true,
                IsNew = true,
                TaskEmployeeListFinal = EmployeeListFinal,
                SelectedIndexForEmployee = SelectedIndexForEmployee,
                // CompanyShifts = CompanyShifts,
                EmployeeShiftList = EmployeeShiftList,
                // TaskSelectedCompanyShift = SelectedCompanyShift,
                TaskSelectedEmployeeShift = SelectedEmployeeShift,
                SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                IsEnabledShift = false,
                TaskStartDate = StartDate,
                TaskStartTime = StartTime,
                //STime = STime,
                TaskEndDate = EndDate,
                TaskEndTime = EndTime,
                //ETime = ETime,
                SelectedIndexForAttendanceType = SelectedIndexForAttendanceType
            });

        }

        public bool CanCloseTask(Task task)
        {
            if (task != null)
            {
                return task.IsComplete;
            }

            return true;
        }

        public void CloseTask(Task task)
        {

            GeosApplication.Instance.Logger.Log("Method CloseTask ...", category: Category.Info, priority: Priority.Low);

            if (task.IsComplete == true)
            {
                Tasks.Remove(task);
            }

            GeosApplication.Instance.Logger.Log("Method CloseTask() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SelectionChangedCommandAction(object obj)
        {
            //throw new NotImplementedException();
        }
        public CompanyWork GetCompanyWork(LookupValue obj)
        {
            CompanyWork companyWork = new CompanyWork
            {
                IdCompanyWork = obj.IdLookupValue,
                Name = obj.Value,
                HtmlColor = obj.HtmlColor
            };

            return companyWork;
        }

        private DateTime GetShiftStartTime(int dayOfWeek, EmployeeShift selectedEmployeeShift, bool isStartTime)
        {
            //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
            //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);

            try
            {
                switch (dayOfWeek)
                {
                    case 0:
                        TimeSpan SunStartTime = SelectedEmployeeShift.CompanyShift.SunStartTime;
                        TimeSpan SunEndTime = SelectedEmployeeShift.CompanyShift.SunEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);


                    case 1:
                        TimeSpan MonStartTime = SelectedEmployeeShift.CompanyShift.MonStartTime;
                        TimeSpan MonEndTime = SelectedEmployeeShift.CompanyShift.MonEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(MonStartTime.Hours).AddMinutes(MonStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(MonEndTime.Hours).AddMinutes(MonEndTime.Minutes);

                    case 2:
                        TimeSpan TueStartTime = SelectedEmployeeShift.CompanyShift.TueStartTime;
                        TimeSpan TueEndTime = SelectedEmployeeShift.CompanyShift.TueEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(TueStartTime.Hours).AddMinutes(TueStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(TueEndTime.Hours).AddMinutes(TueEndTime.Minutes);

                    case 3:
                        TimeSpan WedStartTime = SelectedEmployeeShift.CompanyShift.WedStartTime;
                        TimeSpan WedEndTime = SelectedEmployeeShift.CompanyShift.WedEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(WedStartTime.Hours).AddMinutes(WedStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(WedEndTime.Hours).AddMinutes(WedEndTime.Minutes);

                    case 4:
                        TimeSpan ThuStartTime = SelectedEmployeeShift.CompanyShift.ThuStartTime;
                        TimeSpan ThuEndTime = SelectedEmployeeShift.CompanyShift.ThuEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(ThuStartTime.Hours).AddMinutes(ThuStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(ThuEndTime.Hours).AddMinutes(ThuEndTime.Minutes);

                    case 5:
                        TimeSpan FriStartTime = SelectedEmployeeShift.CompanyShift.FriStartTime;
                        TimeSpan FriEndTime = SelectedEmployeeShift.CompanyShift.FriEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(FriStartTime.Hours).AddMinutes(FriStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(FriEndTime.Hours).AddMinutes(FriEndTime.Minutes);

                    case 6:
                        TimeSpan SatStartTime = SelectedEmployeeShift.CompanyShift.SatStartTime;
                        TimeSpan SatEndTime = SelectedEmployeeShift.CompanyShift.SatEndTime;
                        if (isStartTime)
                            return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                        else
                            return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);

                    default:
                        return (DateTime)StartTime;
                }

            }
            catch (Exception ex)
            {
                return new DateTime();
            }
        }
        #endregion

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
    }

    public class Task : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Declaration

        private bool _isEnabaledEmployee;
        private bool isEnabledShift;
        private ObservableCollection<Employee> taskEmployeeListFinal;
        private int selectedIndexForEmployee;
        // private ObservableCollection<CompanyShift> companyShifts;
        // private CompanyShift taskSelectedCompanyShift;
        private int selectedIndexForCompanyShift;
        private DateTime? taskStartDate;
        private DateTime? taskEndDate;
        private DateTime? taskStartTime;
        private DateTime? taskEndTime;
        private int selectedIndexForAttendanceType;
        private bool isNew;
        private ObservableCollection<Task> SplitedAttendanceList { get; set; }
        private DateTime? taskAccountingDate;
        private ObservableCollection<EmployeeShift> employeeShiftList;
        private EmployeeShift taskSelectedEmployeeShift;
        #endregion

        #region Properties

        public string Header { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public bool IsEnabaledEmployee
        {
            get { return _isEnabaledEmployee; }
            set
            {
                _isEnabaledEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabaledEmployee"));
            }
        }

        public bool IsEnabledShift
        {
            get { return isEnabledShift; }
            set
            {
                isEnabledShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledShift"));
            }
        }

        public ObservableCollection<Employee> TaskEmployeeListFinal
        {
            get { return taskEmployeeListFinal; }
            set
            {
                taskEmployeeListFinal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskEmployeeListFinal"));
            }
        }

        public int SelectedIndexForEmployee
        {
            get { return selectedIndexForEmployee; }
            set
            {
                selectedIndexForEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForEmployee"));
            }
        }

        //public ObservableCollection<CompanyShift> CompanyShifts
        //{
        //    get { return companyShifts; }
        //    set
        //    {
        //        companyShifts = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifts"));
        //    }
        //}

        //public CompanyShift TaskSelectedCompanyShift
        //{
        //    get { return taskSelectedCompanyShift; }
        //    set
        //    {
        //        taskSelectedCompanyShift = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TaskSelectedCompanyShift"));
        //    }
        //}

        public int SelectedIndexForCompanyShift
        {
            get { return selectedIndexForCompanyShift; }
            set
            {
                selectedIndexForCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));
            }
        }

        public DateTime? TaskStartDate
        {
            get { return taskStartDate; }
            set
            {
                taskStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskStartDate"));
            }
        }

        public DateTime? TaskEndDate
        {
            get { return taskEndDate; }
            set
            {
                taskEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskEndDate"));
            }
        }

        public DateTime? TaskStartTime
        {
            get { return taskStartTime; }
            set
            {
                taskStartTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskStartTime"));
            }
        }

        public DateTime? TaskEndTime
        {
            get { return taskEndTime; }
            set
            {
                taskEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskEndTime"));
            }
        }

        public int SelectedIndexForAttendanceType
        {
            get { return selectedIndexForAttendanceType; }
            set
            {
                selectedIndexForAttendanceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public DateTime? TaskAccountingDate
        {
            get
            {
                return taskAccountingDate;
            }

            set
            {
                taskAccountingDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskAccountingDate"));
            }
        }
        public ObservableCollection<EmployeeShift> EmployeeShiftList
        {
            get
            {
                return employeeShiftList;
            }

            set
            {
                employeeShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList"));
            }
        }
        public EmployeeShift TaskSelectedEmployeeShift
        {
            get
            {
                return taskSelectedEmployeeShift;
            }

            set
            {
                taskSelectedEmployeeShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskSelectedEmployeeShift"));
            }
        }
        #endregion

        #region Public ICommand

        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }

        #endregion

        #region Constructor

        public Task()
        {
            OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
            OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
        }
        #endregion

        #region Validation

        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;

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
                me[BindableBase.GetPropertyName(() => TaskStartDate)] +
                me[BindableBase.GetPropertyName(() => TaskEndDate)] +
                me[BindableBase.GetPropertyName(() => TaskStartTime)] +
                me[BindableBase.GetPropertyName(() => TaskEndTime)] +
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
                string selectedIndexForEmployeeProp = BindableBase.GetPropertyName(() => SelectedIndexForEmployee);
                string taskStartDateProp = BindableBase.GetPropertyName(() => TaskStartDate);
                string taskEndDateProp = BindableBase.GetPropertyName(() => TaskEndDate);
                string taskStartTimeProp = BindableBase.GetPropertyName(() => TaskStartTime);
                string taskEndTimeProp = BindableBase.GetPropertyName(() => TaskEndTime);
                string selectedIndexForAttendanceTypeProp = BindableBase.GetPropertyName(() => SelectedIndexForAttendanceType);
                string selectedIndexForCompanyShiftProp = BindableBase.GetPropertyName(() => SelectedIndexForCompanyShift);

                if (columnName == selectedIndexForEmployeeProp)
                {
                    return AttendanceValidation.GetErrorMessage(selectedIndexForEmployeeProp, SelectedIndexForEmployee);
                }

                if (columnName == taskStartDateProp)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(taskStartDateProp, TaskStartDate);
                    }
                }

                if (columnName == taskEndDateProp)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(taskEndDateProp, TaskEndDate);
                    }
                }

                if (columnName == taskStartTimeProp)
                {
                    if (!string.IsNullOrEmpty(startTimeErrorMessage))
                    {
                        return startTimeErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(taskStartTimeProp, TaskStartTime);
                    }
                }

                if (columnName == taskEndTimeProp)
                {
                    if (!string.IsNullOrEmpty(endTimeErrorMessage))
                    {
                        return endTimeErrorMessage;
                    }
                    else
                    {
                        return AttendanceValidation.GetErrorMessage(taskEndTimeProp, TaskEndTime);
                    }
                }

                //if (columnName == attendanceStartDate)
                //{
                //    return AttendanceValidation.GetErrorMessage(attendanceStartDate, TaskStartDate);
                //}

                //if (columnName == attendanceEndDate)
                //{
                //    return AttendanceValidation.GetErrorMessage(attendanceEndDate, TaskEndDate);
                //}

                if (columnName == selectedIndexForAttendanceTypeProp)
                {
                    return AttendanceValidation.GetErrorMessage(selectedIndexForAttendanceTypeProp, SelectedIndexForAttendanceType);
                }

                if (columnName == selectedIndexForCompanyShiftProp)
                {
                    return AttendanceValidation.GetErrorMessage(selectedIndexForCompanyShiftProp, SelectedIndexForCompanyShift);
                }

                return null;
            }
        }

        #endregion

        #region Events

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

        internal string CheckValidation()
        {
            string error = EnableValidationAndGetError();

            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
            PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
            PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));

            return error;
        }

        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startDateErrorMessage = string.Empty;

                if (TaskStartDate != null && TaskEndDate != null)
                {
                    if (TaskStartDate.Value.Date > TaskEndDate.Value.Date)
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

                ////sjadhav
                //StartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                //EndTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);

                CheckDateTimeValidation();

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnTimeEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startTimeErrorMessage = string.Empty;


                if (TaskStartTime != null && TaskEndTime != null)
                {
                    TimeSpan _StartTime = TaskStartTime.Value.TimeOfDay;
                    TimeSpan _EndTime = TaskEndTime.Value.TimeOfDay;

                    if (TaskSelectedEmployeeShift.CompanyShift.IsNightShift == 0)//if (TaskSelectedCompanyShift.IsNightShift == 0)
                    {
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
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }

                CheckDateTimeValidation();
                //CheckValidation

                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void CheckDateTimeValidation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);

                if (TaskStartDate != null && TaskEndDate != null && TaskStartTime != null && TaskEndTime != null)
                {
                    DateTime _TempStartDate = TaskStartDate.Value.Date.AddHours(TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(TaskStartTime.Value.TimeOfDay.Minutes);
                    DateTime _TimeEndDate = TaskEndDate.Value.Date.AddHours(TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(TaskEndTime.Value.TimeOfDay.Minutes);
                    if (_TempStartDate > _TimeEndDate)
                    {
                        startTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceStartTimeError").ToString();
                        endTimeErrorMessage = System.Windows.Application.Current.FindResource("AddAttendanceEndTimeError").ToString();
                    }
                    else
                    {
                        TimeSpan _StartTime = TaskStartTime.Value.TimeOfDay;
                        TimeSpan _EndTime = TaskEndTime.Value.TimeOfDay;

                        if (TaskSelectedEmployeeShift.CompanyShift.IsNightShift == 0)//if (TaskSelectedCompanyShift.IsNightShift == 0)
                        {
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
                    }
                }
                else
                {
                    startTimeErrorMessage = string.Empty;
                    endTimeErrorMessage = string.Empty;
                }

                EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("TaskStartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("TaskEndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("TaskStartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("TaskEndTime"));

                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
