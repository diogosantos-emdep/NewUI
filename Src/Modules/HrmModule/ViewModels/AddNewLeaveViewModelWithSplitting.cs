using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Emdep.Geos.UI.Validations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Globalization;
using DevExpress.Xpf.Editors;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewLeaveViewModelWithSplitting : INotifyPropertyChanged, IDataErrorInfo
    {
        //[GEOS2-6571][rdixit][18.12.2024] Done Chnages for EmployeeLeaveList
        #region TaskLog
        /// <summary>
        /// [0001][25/06/2018][lsharma][SPRINT 41][HRM-M041-21]Simplify the way to add leaves]
        /// In this task default values loaded on Add new leave for Employee and StartDate,EndDate & AllDayEvent as per active view selection
        /// </summary>
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private EmployeeShift selectedEmployeeShift;
        private ObservableCollection<EmployeeShift> employeeShiftList;
        private bool isEditInit;
        private int selectedIndexForCompanyShift;
        private CompanyShift selectedCompanyShift;
        private ObservableCollection<CompanyShift> companyShifts;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private int selectedIndexForEmployee=-1; //<!--[pramod.misal][GEOS2-4584][25-08-2023]-->
        private bool isSave;
        private bool isNew;
        private bool IsBusy;
        private bool isNewLeave;
        private bool flag;
        private ObservableCollection<Attachment> attachmentList;
        private byte[] leaveFileInBytes;
        private CompanyShift[] companyShiftArray;
        private Int32[] companyShiftIdArray;
        private string leaveFileName;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? startTime;
        private DateTime? endTime;
        private Visibility shiftVisibility;
        private TimeSpan sTime;
        private TimeSpan eTime;
        
        private int selectedLeaveType;
        private string startTimeErrorMessage = string.Empty;
        private string endTimeErrorMessage = string.Empty;
        private string error = string.Empty;
        private string remarks;
        private bool isAllDayEvent;
        private List<DateTime> _FromDates;
        private bool invertIsAllDayEvent;
        private List<DateTime> _ToDates;
        private string timeEditMask;
        private ObservableCollection<EmployeeLeave> existEmployeeLeave;
        private ObservableCollection<Employee> employeeListFinal;
        private ObservableCollection<CompanyLeave> companyLeavesList;
        private ObservableCollection<EmployeeLeave> existEmployeeLeaveList;
        
        private string leaveTitle;
        private bool isEditEmployee;
        private EmployeeLeave updateEmployeeLeave;
        private EmployeeLeave oldEmployeeLeaveDetatils;
        private long selectedPeriod;
        private List<Company> companyList;
        private ObservableCollection<EmployeeChangelog> employeeLeaveChangeLogList;
        private ObservableCollection<EmployeeAttendance> employeeAttendanceList;
        private List<Object> attachmentObjectList;
        private Visibility isVisible;
        private Attachment attachedFile;
        private List<object> selectedEmployeeList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private List<EmployeeLeave> listOfleaveStartTimeAndEndTime = new List<EmployeeLeave>();
        private int defaultIdCompanyShift = 0;
        private object selectedEmployee;
        private LookupValue defaultLeaveType;
        #endregion

        #region Public Icommands
        public ICommand EmployeePopupClosedCommand { get; private set; }
        public ICommand ChooseFileCommandForLeave { get; set; }
        public ICommand CancelCommandForLeave { get; set; }
        public ICommand AddNewLeaveAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnTimeEditValueChangingCommand { get; set; }
        public ICommand AddNewLeaveViewCancelButtonCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }

        public ICommand SelectedIndexChangedForCompanyShiftCommand { get; set; }
        public ICommand DocumentViewCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        //public ICommand SplitButtonCommand { get; set; }
        public ICommand SplitAttendanceViewAcceptButtonCommand { get; set; }
        public ICommand SplitAttendanceViewCancelButtonCommand { get; set; }
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand CloseTaskCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        // public ICommand CommandTextInput { get; set; }

        #endregion

        #region Properties
        ObservableCollection<LookupValue> employeeLeaveList;
        public ObservableCollection<LookupValue> EmployeeLeaveList
        {
            get { return employeeLeaveList; }
            set
            {
                employeeLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaveList"));
            }
        }
        private LookupValue DefaultLeaveType
        {
            get { return defaultLeaveType; }
            set
            {
                defaultLeaveType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultLeaveType"));
            }
        }

        private object SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployee"));
            }
        }

        private List<EmployeeLeave> ListOfleaveStartTimeAndEndTime
        {
            get { return listOfleaveStartTimeAndEndTime; }
            set
            {
                listOfleaveStartTimeAndEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfleaveStartTimeAndEndTime"));
            }
        }

        public int DefaultIdCompanyShift
        {
            get { return defaultIdCompanyShift; }
            set
            {
                defaultIdCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultIdCompanyShift"));
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

        public Visibility ShiftVisibility
        {
            get
            {
                return shiftVisibility;
            }

            set
            {
                shiftVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShiftVisibility"));
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
        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }
        private string WorkingPlantId { get; set; }
        private List<Company> SelectedPlantList { get; set; }
        public List<EmployeeLeave> NewEmployeeLeaveList { get; set; }

        private bool IsNewLeave
        {
            get { return isNewLeave; }
            set
            {
                isNewLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewLeave"));
            }
        }

        public EmployeeLeave UpdateEmployeeLeave
        {
            get { return updateEmployeeLeave; }
            set
            {
                updateEmployeeLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateEmployeeLeave"));
            }
        }

        public EmployeeLeave OldEmployeeLeaveDetatils
        {
            get { return oldEmployeeLeaveDetatils; }
            set
            {
                oldEmployeeLeaveDetatils = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateEmployeeLeave"));
            }
        }

        public ulong IdEmployeeLeave { get; private set; }
        public string OldLeaveFileName { get; set; }
        public bool ChangeFileUpload { get; set; }

        

        public ObservableCollection<EmployeeLeave> ExistEmployeeLeave
        {
            get { return existEmployeeLeave; }
            set
            {
                existEmployeeLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeLeave"));
            }
        }

        public string TimeEditMask
        {
            get { return timeEditMask; }
            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        public bool InvertIsAllDayEvent
        {
            get { return invertIsAllDayEvent; }
            set
            {
                invertIsAllDayEvent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InvertIsAllDayEvent"));
            }
        }

        public List<DateTime> FromDates
        {
            get { return _FromDates; }
            set
            {
                if (value != _FromDates)
                {
                    _FromDates = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("_FromDates"));
                }
            }
        }

        public List<DateTime> ToDates
        {
            get { return _ToDates; }
            set
            {
                if (value != _ToDates)
                {
                    _ToDates = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("_ToDates"));
                }
            }
        }

        public bool IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllDayEvent"));
                // CheckDateTimeValidation();
                if (value)
                {
                    InvertIsAllDayEvent = false;
                    //StartTimeErrorMessage = string.Empty;
                    //EndTimeErrorMessage = string.Empty;
                    //StartTime = FromDates[0];
                    //EndTime = ToDates[0];
                }
                else
                {
                    //StartTime = FromDates[0];
                    //EndTime = ToDates[0];
                    InvertIsAllDayEvent = true;
                }
            }
        }

        private bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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

        public ObservableCollection<Employee> EmployeeListFinal
        {
            get { return employeeListFinal; }
            set
            {
                employeeListFinal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeListFinal"));
            }
        }

        public ObservableCollection<CompanyLeave> CompanyLeavesList
        {
            get { return companyLeavesList; }
            set
            {
                companyLeavesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyLeavesList"));
            }
        }

        public ObservableCollection<EmployeeLeave> ExistEmployeeLeaveList
        {
            get { return existEmployeeLeaveList; }
            set
            {
                existEmployeeLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeLeaveList"));
            }
        }

        public ObservableCollection<Attachment> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public byte[] LeaveFileInBytes
        {
            get { return leaveFileInBytes; }
            set
            {
                leaveFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveFileInBytes"));
            }
        }

        public Int32[] CompanyShiftIdArray
        {
            get { return companyShiftIdArray; }
            set
            {
                companyShiftIdArray = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftIdArray"));
            }
        }

        public CompanyShift[] CompanyShiftArray
        {
            get { return companyShiftArray; }
            set
            {
                companyShiftArray = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftArray"));
            }
        }

        public string LeaveFileName
        {
            get { return leaveFileName; }
            set
            {
                leaveFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveFileName"));
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

        public TimeSpan STime
        {
            get { return sTime; }
            set
            {
                sTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("STime"));
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

        public TimeSpan ETime
        {
            get { return eTime; }
            set
            {
                eTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ETime"));
            }
        }

        public int SelectedLeaveType
        {
            get { return selectedLeaveType; }
            set
            {
                selectedLeaveType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveType"));
            }
        }

        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }

        public string LeaveTitle
        {
            get { return leaveTitle; }
            set
            {
                leaveTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeaveTitle"));
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

        public string StartDateErrorMessage
        {
            get { return startDateErrorMessage; }
            set
            {
                startDateErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDateErrorMessage"));
            }
        }

        public string EndDateErrorMessage
        {
            get { return endDateErrorMessage; }
            set
            {
                endDateErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDateErrorMessage"));
            }
        }

        public string StartTimeErrorMessage
        {
            get { return startTimeErrorMessage; }
            set
            {
                startTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeErrorMessage"));
            }
        }

        public ObservableCollection<EmployeeChangelog> EmployeeLeaveChangeLogList
        {
            get { return employeeLeaveChangeLogList; }
            set
            {
                employeeLeaveChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaveChangeLogList"));
            }
        }

        public string EndTimeErrorMessage
        {
            get { return endTimeErrorMessage; }
            set
            {
                endTimeErrorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeErrorMessage"));
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

        public List<Company> CompanyList
        {
            get { return companyList; }
            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }

        public ObservableCollection<EmployeeAttendance> EmployeeAttendanceList
        {
            get { return employeeAttendanceList; }
            set
            {
                employeeAttendanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAttendanceList"));
            }
        }

        public List<object> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }

        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }

        public bool IsAdd { get; set; }

        public Attachment AttachedFile
        {
            get { return attachedFile; }
            set
            {
                attachedFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFile"));
            }

        }

        public List<object> SelectedEmployeeList
        {
            get { return selectedEmployeeList; }
            set
            {
                selectedEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeList"));
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

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event EventHandler RequestClose;

        #endregion

        #region Constructor

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri  
        /// </summary>     
        public AddNewLeaveViewModelWithSplitting(AddNewLeaveViewWithSplitting addNewLeaveView)
        {
            try
            {
                /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]        
                GeosApplication.Instance.Logger.Log("Method AddNewLeaveViewModel()...", category: Category.Info, priority: Priority.Low);
                EventHandler handle = delegate { addNewLeaveView.Close(); };
                this.RequestClose += handle;
                addNewLeaveView.DataContext = this;
                ShiftVisibility = Visibility.Visible;
                SetUserPermission();                
                CancelCommandForLeave = new RelayCommand(new Action<object>(CloseWindow));
                AddNewLeaveAcceptButtonCommand = new RelayCommand(new Action<object>(AddNewLeaveInformation));
                ChooseFileCommandForLeave = new RelayCommand(new Action<object>(BrowseFileAction));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedCommandAction);
                OnTimeEditValueChangingCommand = new DelegateCommand<RoutedEventArgs>(OnTimeEditValueChanging);
                AddNewLeaveViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SelectedIndexChangedForCompanyShiftCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedForCompanyShiftCommandAction);
                CheckedCommand = new RelayCommand(new Action<object>(CheckedCommandAction));
                EmployeePopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(EmployeePopupClosedCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                
                //Split commands.
                //SplitButtonCommand = new RelayCommand(new Action<object>(SplitAttendanceInformation));
                SplitAttendanceViewAcceptButtonCommand = new DelegateCommand<object>(SaveAttendanceWithSplit);
                SplitAttendanceViewCancelButtonCommand = new DelegateCommand<object>(CloseAttemdanceWindowForSplit);
              //  AddNewTaskCommand = new RelayCommand(new Action<object>(AddNewSplitTaskCommandAction));
                CloseTaskCommand = new DelegateCommand<AddNewLeaveViewModelWithSplittingTask>(CloseTask, CanCloseTask);
                SelectionChangedCommand = new RelayCommand(new Action<object>(SelectionChangedCommandAction));
                //CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                FillFromDates();
                FillToDates();
                InvertIsAllDayEvent = true;

                //List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

                //var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                //EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>(HrmService.GetEmployeeAttendanceForNewLeave(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                DocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeLeaveDocument));
                SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
                //[001] Fill Employee leave as per lookup value
                FillEmployeeLeaveType();
                this.SelectedPlantList = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                this.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
               
                //EmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesByIdCompany(plantOwnersIds));
                //EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });
                //CompanyLeavesList = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(plantOwnersIds));
                //CompanyLeavesList.Insert(0, new CompanyLeave() { Name = "---", IdCompanyLeave = 0 });

                GeosApplication.Instance.Logger.Log("Method AddNewLeaveViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewLeaveViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods
        public void FillEmployeeLeaveTypeByLocation(List<int> selectedEmployeeIdList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);
                if (selectedEmployeeIdList != null)
                {
                    EmployeeLeaveList = new ObservableCollection<LookupValue>(HrmService.GetLeavesByLocations_V2590(selectedEmployeeIdList));
                    EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, InUse = true });
                    EmployeeLeaveList = new ObservableCollection<LookupValue>(EmployeeLeaveList);
                    SelectedLeaveType = 0;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
        /// [002][01-10-2020][cpatil][SPRINT 86][GEOS2-2622][In some employees when we try add Leaves the system show the message “Current Date range is Overlapped with another entries”,]
        /// Code for Selection of Shift
        private void SelectedIndexChangedForCompanyShiftCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedForCompanyShiftCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (StartTime != null && EndTime != null && SelectedViewIndex==0)
                {
                    SelectedEmployeeShift = EmployeeShiftList[SelectedIndexForCompanyShift];
                    CheckDateTimeValidation();
                    //[002]
                    if (flag)
                    {
                        StartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                        EndTime = GetShiftStartTime((int)EndTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                    }
                    flag = true;
                    if (CompanyShiftArray == null)
                    {
                        CompanyShiftIdArray = new Int32[SelectedEmployeeList.Count];
                        CompanyShiftArray = new CompanyShift[SelectedEmployeeList.Count];
                    }
                    CompanyShiftIdArray[0] = EmployeeShiftList[SelectedIndexForCompanyShift].IdCompanyShift;
                    CompanyShiftArray[0] = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift;
                }
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedForCompanyShiftCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedForCompanyShiftCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private DateTime GetShiftStartTime(int dayOfWeek, EmployeeShift selectedEmployeeShift, bool isStartTime)
        {
            //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
            //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);

            try
            {
                GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()...", category: Category.Info, priority: Priority.Low);
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
                        TimeSpan MonStartTime = selectedEmployeeShift.CompanyShift.MonStartTime;
                        TimeSpan MonEndTime = selectedEmployeeShift.CompanyShift.MonEndTime;
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
                GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                return new DateTime();

            }
        }
        
        private void EmployeePopupClosedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EmployeePopupClosedCommandAction() ...", category: Category.Info, priority: Priority.Low);

                if (SelectedEmployeeList.Count > 1)
                {
                    ShiftVisibility = Visibility.Collapsed;
                }
                else
                {
                    ShiftVisibility = Visibility.Visible;
                }
                //[GEOS2-6571][rdixit][18.12.2024]
                if (SelectedEmployeeList?.Count > 0)
                {
                    var employeeList = SelectedEmployeeList?.Cast<int>()?.ToList();
                    FillEmployeeLeaveTypeByLocation(employeeList);
                }
                LoadShiftData();
                LoadAnyPreShift();
                GeosApplication.Instance.Logger.Log("Method EmployeePopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeePopupClosedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LoadAnyPreShift(int defaultIdCompanyShift = 0)
        {
            if(defaultIdCompanyShift!=0 && EmployeeShiftList!=null)
            {
                if (EmployeeShiftList.Any(esl => esl.IdCompanyShift == defaultIdCompanyShift))
                {
                    SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.Where(esl => esl.IdCompanyShift == defaultIdCompanyShift).FirstOrDefault());

                    //CompanyShiftIdArray[j] = EmployeeShiftList[SelectedIndexForCompanyShift].IdCompanyShift;
                    //CompanyShiftArray[j] = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift;

                }
            }
            else if (IsNew && ExistEmployeeLeaveList!=null && ExistEmployeeLeaveList.Count> 0 && EmployeeShiftList != null)
            {                
                List<EmployeeLeave> EmpLeaves = new List<EmployeeLeave>();
                if (ExistEmployeeLeaveList.Any(x => StartDate.Value.Date >= x.StartDate.Value.Date && EndDate.Value.Date <= x.EndDate.Value.Date))
                {
                    EmpLeaves = ExistEmployeeLeaveList.Where(x => StartDate.Value.Date >= x.StartDate.Value.Date && EndDate.Value.Date <= x.EndDate.Value.Date).ToList();                
                }
                else if (ExistEmployeeLeaveList.Any(x => StartDate.Value.Date == x.StartDate.Value.Date && EndDate.Value.Date == x.EndDate.Value.Date))
                {
                    EmpLeaves = ExistEmployeeLeaveList.Where(x => StartDate.Value.Date == x.StartDate.Value.Date && EndDate.Value.Date == x.EndDate.Value.Date).ToList();                
                }

                if(EmpLeaves!=null)
                {   
                    for (int j = 0; j < SelectedEmployeeList.Count; j++)
                    {                    
                        if (EmpLeaves.Any(el => el.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j])))
                        {
                            EmployeeLeave empLeave = EmpLeaves.Where(el => el.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j])).FirstOrDefault();
                            if (EmployeeShiftList.Any(esl => esl.IdCompanyShift == empLeave.IdCompanyShift))
                            {
                                SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.Where(esl => esl.IdCompanyShift == empLeave.IdCompanyShift).FirstOrDefault());
                                
                                    CompanyShiftIdArray[j] = EmployeeShiftList[SelectedIndexForCompanyShift].IdCompanyShift;
                                    CompanyShiftArray[j] = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift;
                                
                            }
                        }
                    }
                }
            }
        }
        private void LoadShiftData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadShiftData()...", category: Category.Info, priority: Priority.Low);
                CompanyShiftIdArray = new Int32[SelectedEmployeeList.Count];
                CompanyShiftArray = new CompanyShift[SelectedEmployeeList.Count];

                if (SelectedEmployeeList.Count > 0)
                {
                    for (int j = 0; j < SelectedEmployeeList.Count; j++)
                    {
                        EmployeeShiftList = new ObservableCollection<EmployeeShift>();
                        ObservableCollection<EmployeeShift> TempEmpShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(Convert.ToInt32(SelectedEmployeeList[j])));

                        foreach (EmployeeShift Shift in TempEmpShiftList)
                        {
                            if (Shift.CompanyShift.Name != "---")
                                EmployeeShiftList.Add(Shift);
                        }

                        if (EmployeeShiftList.Count > 0)
                        {
                            CompanyShiftIdArray[j] = EmployeeShiftList[0].IdCompanyShift;
                            CompanyShiftArray[j] = EmployeeShiftList[0].CompanyShift;
                            SelectedIndexForCompanyShift = 0;
                        }

                        if (EmployeeShiftList.Count == 0)
                        {
                            SelectedIndexForCompanyShift = 0;
                        }
                        else
                        {
                            Double Hour = 0;
                            for (int i = 0; i < EmployeeShiftList.Count; i++)
                            {

                                if (EmployeeShiftList[i].CompanyShift.Name != "---")
                                {
                                    DateTime shiftStartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, EmployeeShiftList[i], true);
                                    DateTime shiftEndTime = GetShiftStartTime((int)EndTime.Value.Date.DayOfWeek, EmployeeShiftList[i], false);

                                    TimeSpan Diff = new TimeSpan();
                                    if (shiftStartTime > shiftEndTime)
                                    {
                                        Diff = new TimeSpan(0, (24 - shiftStartTime.TimeOfDay.Hours) + shiftEndTime.TimeOfDay.Hours, 0, 0);
                                        Diff = Diff.Subtract(new TimeSpan(0, 0, Math.Abs(shiftStartTime.TimeOfDay.Minutes - shiftEndTime.TimeOfDay.Minutes), 0));
                                    }
                                    else
                                        Diff = shiftEndTime.Subtract(shiftStartTime);

                                    if (Hour == 0)
                                        Hour = Convert.ToDouble(Diff.TotalHours);

                                    if (Hour > Diff.TotalHours)
                                    {
                                        Hour = Diff.TotalHours;
                                        CompanyShiftIdArray[j] = EmployeeShiftList[i].IdCompanyShift;
                                        CompanyShiftArray[j] = EmployeeShiftList[i].CompanyShift;
                                        SelectedIndexForCompanyShift = i;
                                    }
                                    else if (Hour == Diff.TotalHours && i > 0)
                                    {
                                        try
                                        {
                                            if (EmployeeShiftList[i].IdCompanyShift < EmployeeShiftList[i - 1].IdCompanyShift)
                                            {
                                                CompanyShiftIdArray[j] = EmployeeShiftList[i - 1].IdCompanyShift;
                                                CompanyShiftArray[j] = EmployeeShiftList[i - 1].CompanyShift;
                                                SelectedIndexForCompanyShift = i - 1;
                                            }
                                            else
                                            {
                                                CompanyShiftIdArray[j] = EmployeeShiftList[i].IdCompanyShift;
                                                CompanyShiftArray[j] = EmployeeShiftList[i].CompanyShift;
                                                SelectedIndexForCompanyShift = i;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in Method LoadShiftData()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!StartTime.HasValue || (StartTime.HasValue && StartTime.Value.Hour==0 && StartTime.Value.Minute == 0))
                    {
                        StartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                    }
                    if (!EndTime.HasValue || (EndTime.HasValue && EndTime.Value.Hour == 0 && EndTime.Value.Minute == 0))
                    {
                        EndTime = GetShiftStartTime((int)EndTime.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                    }
                }
                else
                {
                    EmployeeShiftList = new ObservableCollection<EmployeeShift>();
                    EmployeeShift tempEmployeeShift = new EmployeeShift();
                    tempEmployeeShift.CompanyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
                    EmployeeShiftList.Insert(0, tempEmployeeShift);

                    SelectedEmployeeShift = tempEmployeeShift;
                    SelectedIndexForCompanyShift = 0;
                }
                GeosApplication.Instance.Logger.Log("Method LoadShiftData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadShiftData()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployee.IdEmployee));
                        if (EmployeeShiftList.Count == 2)
                            SelectedIndexForCompanyShift = 1;

                    }
                }
                else
                {                    
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
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeEndDateError").ToString();
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

        private void OnTimeEditValueChanging(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                StartTimeErrorMessage = string.Empty;

                if (StartTime != null && EndTime != null)
                {
                   // if (SelectedEmployeeShift.CompanyShift.IsNightShift != 1)
                   // {

                        if (StartTime > EndTime)
                        {
                            StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveStartTimeError").ToString();
                            EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveEndTimeError").ToString();
                        }
                        else
                        {
                            StartTimeErrorMessage = string.Empty;
                            EndTimeErrorMessage = string.Empty;
                        }
                   // }
                    //else
                    //{
                    //    StartTimeErrorMessage = string.Empty;
                    //    EndTimeErrorMessage = string.Empty;
                    //}
                }
                else
                {
                    StartTimeErrorMessage = string.Empty;
                    EndTimeErrorMessage = string.Empty;
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
        ///Validation for Date & time done by mayuri
        /// </summary>
        //public void CheckDateTimeValidation()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);


        //        if (!HrmCommon.Instance.IsPermissionReadOnly)
        //        {
        //            if (!IsAllDayEvent)
        //            {
        //                if (StartDate != null && EndDate != null && StartTime != null && EndTime != null)
        //                {
        //                    DateTime _TempStartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
        //                    DateTime _TimeEndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
        //                    if (SelectedEmployeeShift.CompanyShift.IsNightShift != 1)
        //                    {
        //                        if (StartDate.Value.Date > EndDate.Value.Date)
        //                        {
        //                            startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeStartDateError").ToString();
        //                            endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeEndDateError").ToString();

        //                        }
        //                        else
        //                        {
        //                            StartDateErrorMessage = string.Empty;
        //                            EndDateErrorMessage = string.Empty;
        //                        }
        //                        if (_TempStartDate >= _TimeEndDate)
        //                        {
        //                            StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveStartTimeError").ToString();
        //                            EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveEndTimeError").ToString();
        //                        }
        //                        else
        //                        {
        //                            StartTimeErrorMessage = string.Empty;
        //                            EndTimeErrorMessage = string.Empty;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (StartDate.Value.Date >= EndDate.Value.Date)
        //                        {
        //                            startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftStartDateError").ToString();
        //                            endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftEndDateError").ToString();

        //                        }
        //                        else
        //                        {
        //                            StartDateErrorMessage = string.Empty;
        //                            EndDateErrorMessage = string.Empty;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    StartTimeErrorMessage = string.Empty;
        //                    EndTimeErrorMessage = string.Empty;
        //                }
        //             }
        //            else
        //            {
        //                StartTimeErrorMessage = string.Empty;
        //                EndTimeErrorMessage = string.Empty;
        //                if (SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
        //                {
        //                    if (StartDate.Value.Date >= EndDate.Value.Date)
        //                    {
        //                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftStartDateError").ToString();
        //                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftEndDateError").ToString();
        //                    }
        //                    else
        //                    {
        //                        StartDateErrorMessage = string.Empty;
        //                        EndDateErrorMessage = string.Empty;
        //                    }
        //                }

        //                else
        //                {
        //                    if (StartDate.Value.Date > EndDate.Value.Date)
        //                    {
        //                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeStartDateError").ToString();
        //                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeEndDateError").ToString();

        //                    }
        //                    else
        //                    {
        //                        StartDateErrorMessage = string.Empty;
        //                        EndDateErrorMessage = string.Empty;
        //                    }

        //                }
        //            }

        //            error = EnableValidationAndGetError();
        //            PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
        //            PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
        //            PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
        //            PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
        //        }
        //        GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        public void CheckDateTimeValidation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()...", category: Category.Info, priority: Priority.Low);


                if (!HrmCommon.Instance.IsPermissionReadOnly)
                {
                    DateTime _TempStartDate = DateTime.Today;
                    DateTime _TimeEndDate = DateTime.Today;

                    if (StartDate != null && EndDate != null && StartTime != null && EndTime != null)
                    {
                        _TempStartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                        _TimeEndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                    }

                    if (!IsAllDayEvent)
                    {
                        if (StartDate != null && EndDate != null && StartTime != null && EndTime != null)
                        {
                            if (SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                            {
                                if (StartDate.Value.Date >= EndDate.Value.Date && _TempStartDate >= _TimeEndDate)
                                {
                                    StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveStartTimeError").ToString();
                                    EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveEndTimeError").ToString();
                                    startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftStartDateError").ToString();
                                    endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftEndDateError").ToString();


                                }
                                else
                                {
                                    StartTimeErrorMessage = string.Empty;
                                    EndTimeErrorMessage = string.Empty;
                                    startDateErrorMessage = string.Empty;
                                    endDateErrorMessage = string.Empty;
                                }
                            }
                            else if (_TempStartDate >= _TimeEndDate)
                            {
                                StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveStartTimeError").ToString();
                                EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveEndTimeError").ToString();
                            }
                            else
                            {
                                StartTimeErrorMessage = string.Empty;
                                EndTimeErrorMessage = string.Empty;
                                startDateErrorMessage = string.Empty;
                                endDateErrorMessage = string.Empty;

                            }
                        }
                        else
                        {
                            StartTimeErrorMessage = string.Empty;
                            EndTimeErrorMessage = string.Empty;
                        }
                    }
                    else
                    {
                        StartTimeErrorMessage = string.Empty;
                        EndTimeErrorMessage = string.Empty;
                        if (SelectedEmployeeShift.CompanyShift.IsNightShift == 1)
                        {
                            if (StartDate.Value.Date >= EndDate.Value.Date && _TempStartDate >= _TimeEndDate)
                            {
                                // StartTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveStartTimeError").ToString();
                                // EndTimeErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveEndTimeError").ToString();
                                startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftStartDateError").ToString();
                                endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeShiftEndDateError").ToString();

                            }
                            else
                            {
                                StartTimeErrorMessage = string.Empty;
                                EndTimeErrorMessage = string.Empty;
                                startDateErrorMessage = string.Empty;
                                endDateErrorMessage = string.Empty;
                            }
                        }
                        else if (StartDate.Value.Date > EndDate.Value.Date)
                        {
                            startDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveShiftStartTimeError").ToString();
                            endDateErrorMessage = System.Windows.Application.Current.FindResource("AddEmployeeLeaveShiftEndTimeError").ToString();
                        }
                        else
                        {
                            startDateErrorMessage = string.Empty;
                            endDateErrorMessage = string.Empty;
                        }

                    }


                    if (allowValidation)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                        PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CheckDateTimeValidation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        /// <summary>
        ///This function use to add change log in employee detail.
        ///Sprint 46 AddNewLeaveViewModelWithSplittingTask -- Changelog when managing employee leaves--by Amit
        /// </summary>
        public void AddEmployeeLeaveChangeLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedDetailsInLog()...", category: Category.Info, priority: Priority.Low);

                EmployeeLeaveChangeLogList = new ObservableCollection<EmployeeChangelog>();
                if (EmployeeLeaveList != null)
                {
                    LookupValue newType = EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == EmployeeLeaveList[SelectedLeaveType].IdLookupValue);

                    //New Attachment Removed
                    if (IsNewLeave == false)
                    {
                        LookupValue oldType = EmployeeLeaveList.FirstOrDefault(x => x.IdLookupValue == OldEmployeeLeaveDetatils.IdLeave);

                        //Start Date 
                        if (!OldEmployeeLeaveDetatils.StartDate.Value.ToShortDateString().Equals(StartDate.Value.ToShortDateString()))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveStartDateChangeLog").ToString(), OldEmployeeLeaveDetatils.StartDate.Value.ToShortDateString(), StartDate.Value.ToShortDateString()) });
                        }
                        //End Date 
                        if (!OldEmployeeLeaveDetatils.EndDate.Value.ToShortDateString().Equals(EndDate.Value.ToShortDateString()))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveEndDateChangeLog").ToString(), OldEmployeeLeaveDetatils.EndDate.Value.ToShortDateString(), EndDate.Value.ToShortDateString()) });
                        }
                        //Start Time 

                        if (!OldEmployeeLeaveDetatils.StartTime.Equals(StartTime.Value.TimeOfDay))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveStartTimeChangeLog").ToString(), OldEmployeeLeaveDetatils.StartTime, StartTime.Value.TimeOfDay) });
                        }
                        //End Time 
                        if (!OldEmployeeLeaveDetatils.EndTime.Equals(EndTime.Value.TimeOfDay))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveEndTimeChangeLog").ToString(), OldEmployeeLeaveDetatils.EndTime, EndTime.Value.TimeOfDay) });
                        }
                        //Leave Type

                        if (!oldType.Value.Equals(newType.Value))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveTypeChangeLog").ToString(), oldType.Value, newType.Value) });
                        }
                        //Remarks 
                        if (!string.IsNullOrEmpty(OldEmployeeLeaveDetatils.Remark) && !string.IsNullOrEmpty(Remarks) && !OldEmployeeLeaveDetatils.Remark.Equals(Remarks))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveRemarksChangeLog").ToString(), OldEmployeeLeaveDetatils.Remark, Remarks) });
                        }
                        //Remarks Null
                        if (string.IsNullOrEmpty(OldEmployeeLeaveDetatils.Remark) && !string.IsNullOrEmpty(Remarks))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveRemarksChangeLog").ToString(), "None", Remarks) });
                        }
                        //New Remarks Null
                        if (!string.IsNullOrEmpty(OldEmployeeLeaveDetatils.Remark) && string.IsNullOrEmpty(Remarks))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveRemarksChangeLog").ToString(), OldEmployeeLeaveDetatils.Remark, "None") });
                        }
                        //Attachment
                        if (!string.IsNullOrEmpty(OldEmployeeLeaveDetatils.FileName) && !string.IsNullOrEmpty(leaveFileName) && !OldEmployeeLeaveDetatils.FileName.Equals(leaveFileName))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveAttachementChangeLog").ToString(), OldEmployeeLeaveDetatils.FileName, LeaveFileName) });
                        }
                        //Attachment Null
                        if (string.IsNullOrEmpty(OldEmployeeLeaveDetatils.FileName) && !string.IsNullOrEmpty(leaveFileName))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveAttachementChangeLog").ToString(), "None", LeaveFileName) });
                        }
                        //New Attachment Removed
                        if (!string.IsNullOrEmpty(OldEmployeeLeaveDetatils.FileName) && string.IsNullOrEmpty(leaveFileName))
                        {
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveAttachementChangeLog").ToString(), OldEmployeeLeaveDetatils.FileName, "None") });
                        }
                    }
                    //Leave Created
                    if (IsNewLeave == true)
                    {
                        EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveAddChangeLog").ToString(), newType.Value, StartDate.Value.ToShortDateString(), EndDate.Value.ToShortDateString()) });
                    }
                    //Leave Deleted
                    //if (UpdateEmployeeLeave.TransactionOperation == ModelBase.TransactionOperations.Delete)
                    //{
                    //    EmployeeLeaveChangeLogList.Add(new EmployeeChangelog() { IdEmployee = OldEmployeeLeaveDetatils.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveDeleteChangeLog").ToString(), newType.Value) });
                    //}
                }
                GeosApplication.Instance.Logger.Log("Method AddChangedDetailsInLog()...Executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddChangedDetailsInLog()..." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [HRM-M041-21] added new parameters
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri  
        /// [002][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// [003] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// </summary>
        public void Init(ObservableCollection<EmployeeLeave> employeeLeaveList, 
            object selectedEmployee, DateTime selectedStartDate, DateTime selectedEndDate, 
            byte activeViewType, string currentWorkingPlantId, 
            ObservableCollection<Employee> employeeListFinalForLeaves,
            LookupValue defaultLeaveType = null,
            bool? isAllDayEventParameter = null,
            List<EmployeeLeave> listOfleaveStartTimeAndEndTime = null,
            int defaultIdCompanyShift = 0)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                this.ListOfleaveStartTimeAndEndTime = listOfleaveStartTimeAndEndTime;

                var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().AsEnumerable().Select(i => i.IdCompany));

                if (defaultLeaveType != null)
                {
                    this.DefaultLeaveType = DefaultLeaveType;
                    SelectedLeaveType = EmployeeLeaveList !=null ? EmployeeLeaveList.ToList().FindIndex(x => x.IdLookupValue == defaultLeaveType.IdLookupValue) :-1;
                }

                this.SelectedEmployee = selectedEmployee;
                this.DefaultIdCompanyShift = defaultIdCompanyShift;
                this.ExistEmployeeLeaveList = employeeLeaveList;
                this.IsNewLeave = true;
                this.IsNew = true;
                this.LeaveTitle = System.Windows.Application.Current.FindResource("AddNewLeave").ToString();
                this.WorkingPlantId = currentWorkingPlantId;
                this.EmployeeAttendanceList = employeeAttendanceList; //new ObservableCollection<EmployeeAttendance>(HrmService.GetEmployeeAttendanceForNewLeave(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                
                if (employeeListFinalForLeaves == null)
                {
                    EmployeeListFinal = new ObservableCollection<Employee>();
                    //foreach (var item in SelectedPlantList)
                    //{
                    // [003] Changed service method GetAllEmployeesForLeaveByIdCompany_V2032 to GetAllEmployeesForLeaveByIdCompany_V2039
                    EmployeeListFinal.AddRange(HrmService.GetAllEmployeesForLeaveByIdCompany_V2420(
                        plantOwnersIdsJoined, SelectedPeriod, 
                        HrmCommon.Instance.ActiveEmployee.Organization, 
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, 
                        HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] added

                    //    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                    //}
                }
                else
                {
                    this.EmployeeListFinal = employeeListFinalForLeaves;
                }
                this.EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                
                //EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });
                Employee obj = selectedEmployee as Employee;


                if (obj != null)
                {
                    SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == obj.IdEmployee));
                    
                }

                if (selectedStartDate != DateTime.MinValue && selectedEndDate != DateTime.MinValue)
                {
                    StartDate = selectedStartDate;
                    EndDate = selectedEndDate;

                    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];

                    if(isAllDayEventParameter.HasValue)
                    {
                        IsAllDayEvent = isAllDayEventParameter.Value;
                    }
                    else
                    {
                        if (activeViewType == 1)
                            IsAllDayEvent = true;
                        else
                        {
                            if (StartTime == DateTime.MinValue && EndTime == DateTime.MinValue)
                            {
                                IsAllDayEvent = true;
                            }
                        }
                    }

                    if (activeViewType != 1)
                    {
                        if (StartTime == DateTime.MinValue && EndTime == DateTime.MinValue)
                        {
                            if (StartDate == selectedEndDate.AddDays(-1))
                                EndDate = selectedEndDate.AddDays(-1);
                        }
                    }
                }
                //[001] Code Comment [important]
                //CompanyLeavesList = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(WorkingPlantId));
                //CompanyLeavesList.Insert(0, new CompanyLeave() { Name = "---", IdCompanyLeave = 0 });

                IsVisible = Visibility.Hidden;
                IsAdd = true;
                /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                /// For Selected Employee, load shift
                SelectedEmployeeList = new List<object>();
                if (obj != null)
                {                    
                    SelectedEmployeeList.Add(obj.IdEmployee);                    
                }
                else if (SelectedIndexForEmployee > -1)
                {
                    SelectedEmployeeList.Add(EmployeeListFinal[SelectedIndexForEmployee].IdEmployee);
                }
                LoadShiftData();
                LoadAnyPreShift(defaultIdCompanyShift);

                // Split - show one leave only
                if (listOfleaveStartTimeAndEndTime.Count <= 1)
                {
                    SelectedViewIndex = 0;
                }
                else
                {
                   // SelectedViewIndex = 1;
                    SplitInformation();
                }
                FillEmployeeLeaveTypeByLocation(SelectedEmployeeList?.Select(i => (int)i)?.ToList());//[rdixit][29.01.2024][GEOS2-6571]
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
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri   
        /// [003] [cpatil][2020-01-22][GEOS2-2008] We can not see some the Employees in LEAVES and ATTENDANCE with Period = 2019, but the same employees in 2020 we can see.
        /// </summary>
        public void EditInit(EmployeeLeave EmployeeLeave, 
            ObservableCollection<EmployeeLeave> employeeLeaveList,
            string currentWorkingPlantId, 
            ObservableCollection<Employee> employeeListFinalForLeaves)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                this.IsNew = false;
                this.IsNewLeave = false;
                this.LeaveTitle = System.Windows.Application.Current.FindResource("EditLeave").ToString();
                this.WorkingPlantId = currentWorkingPlantId;
                EmployeeListFinal = new ObservableCollection<Employee>();
                IsNewLeave = false;
                OldEmployeeLeaveDetatils = new EmployeeLeave();
                OldEmployeeLeaveDetatils = (EmployeeLeave)EmployeeLeave.Clone();
                EmployeeAttendanceList = employeeAttendanceList;             
                EmployeeListFinal = employeeListFinalForLeaves;              
                EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                //EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });
                //[001] Code Comment [important]
                //CompanyLeavesList = new ObservableCollection<CompanyLeave>(HrmService.GetSelectedIdCompanyLeaves(WorkingPlantId));
                //CompanyLeavesList.Insert(0, new CompanyLeave() { Name = "---", IdCompanyLeave = 0 });
                SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == EmployeeLeave.IdEmployee));
                StartDate = EmployeeLeave.StartDate;
                EndDate = EmployeeLeave.EndDate;
                
                StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
                //StartTime = Convert.ToDateTime(EmployeeLeave.StartTime.ToString());
                //EndTime = Convert.ToDateTime(EmployeeLeave.EndTime.ToString());
                IsAllDayEvent = Convert.ToBoolean(EmployeeLeave.IsAllDayEvent);
                //[001] Code Comment [important]
                // SelectedLeaveType = companyLeavesList.FindIndex(x => x.IdCompanyLeave == Convert.ToUInt16(EmployeeLeave.IdLeave));
                var ind = EmployeeLeaveList?.ToList()?.FindIndex(x => x.IdLookupValue == Convert.ToInt32(EmployeeLeave.IdLeave));
                SelectedLeaveType = ind != null ? (int)ind : -1;
                Remarks = EmployeeLeave.Remark;
                LeaveFileName = EmployeeLeave.FileName;
                OldLeaveFileName = EmployeeLeave.FileName;                
                IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;
                EmployeeShiftList = new ObservableCollection<EmployeeShift>();
                ObservableCollection<EmployeeShift> TempEmpShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(EmployeeListFinal[SelectedIndexForEmployee].IdEmployee));
                foreach (EmployeeShift Shift in TempEmpShiftList)
                {
                    if (Shift.CompanyShift.Name != "---")
                        EmployeeShiftList.Add(Shift);
                }                
                StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];                
                EndTime = FromDates[FromDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];                
                if (EmployeeShiftList != null)
                {
                    EmployeeShiftList.ToList().ForEach(esl => esl.IsEnabled = true);
                    if (!EmployeeShiftList.Any(a => a.CompanyShift.IdCompanyShift == EmployeeLeave.IdCompanyShift))
                    //if(SelectedIndexForCompanyShift<0)
                    {
                        EmployeeShift ObjShift = new EmployeeShift();
                        ObjShift.IdCompanyShift = EmployeeLeave.IdCompanyShift;
                        ObjShift.CompanyShift = EmployeeLeave.CompanyShift;
                        EmployeeShiftList.Add(ObjShift);
                        ObjShift.IsEnabled = false;
                    }
                }
                SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.IdCompanyShift == EmployeeLeave.IdCompanyShift));

                AttachmentList = new ObservableCollection<Attachment>();
                if (!string.IsNullOrEmpty(EmployeeLeave.FileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = EmployeeLeave.FileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = LeaveFileInBytes;
                    AttachmentList.Add(attachment);
                    AttachedFile = AttachmentList[0];
                }
                ExistEmployeeLeaveList = employeeLeaveList;
                if (AttachmentList.Count > 0)
                    IsVisible = Visibility.Visible;
                else
                    IsVisible = Visibility.Hidden;
                IsAdd = false;
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
        /// This method use for Add Read only permission
        ///  [HRM-M046-07] Add new permission ReadOnly--by Amit
        /// </summary>
        /// <param name="EmployeeLeave"></param>
        public void InitReadOnly(EmployeeLeave EmployeeLeave, string currentWorkingPlantId)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                this.IsNew = false;
                this.IsNewLeave = false;
                this.LeaveTitle = System.Windows.Application.Current.FindResource("EditLeave").ToString();
                //EmployeeListFinal = new ObservableCollection<Employee>();
                this.WorkingPlantId = currentWorkingPlantId;
                IsNewLeave = false;
                OldEmployeeLeaveDetatils = new EmployeeLeave();
                OldEmployeeLeaveDetatils = (EmployeeLeave)EmployeeLeave.Clone();

                EmployeeListFinal = new ObservableCollection<Employee>();
                EmployeeListFinal.Add(EmployeeLeave.Employee);

                SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == EmployeeLeave.IdEmployee));
                StartDate = EmployeeLeave.StartDate;
                EndDate = EmployeeLeave.EndDate;

                StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
                EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];

                IsAllDayEvent = Convert.ToBoolean(EmployeeLeave.IsAllDayEvent);
                var ind = EmployeeLeaveList?.ToList()?.FindIndex(x => x.IdLookupValue == Convert.ToUInt16(EmployeeLeave.IdLeave));
                SelectedLeaveType = ind != null ? (int)ind : -1;

                Remarks = EmployeeLeave.Remark;
                // Company = EmployeeLeave.CompanyLeave.Company;

                LeaveFileName = EmployeeLeave.FileName;
                OldLeaveFileName = EmployeeLeave.FileName;

                IdEmployeeLeave = EmployeeLeave.IdEmployeeLeave;

                AttachmentList = new ObservableCollection<Attachment>();
                if (!string.IsNullOrEmpty(EmployeeLeave.FileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = EmployeeLeave.FileName;
                    attachment.IsDeleted = false;
                    // attachment.FileByte = LeaveFileInBytes;
                    AttachmentList.Add(attachment);
                    AttachedFile = AttachmentList[0];
                }
                if (AttachmentList.Count > 0)
                    IsVisible = Visibility.Visible;
                else
                    IsVisible = Visibility.Hidden;
                IsAdd = false;
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

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);
            if (obj != null)
            {
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
            }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    LeaveFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    AttachmentList = new ObservableCollection<Attachment>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    LeaveFileName = file.Name;

                    ObservableCollection<Attachment> newAttachmentList = new ObservableCollection<Attachment>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = LeaveFileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    AttachmentList = newAttachmentList;

                    if (AttachmentList.Count > 0)
                    {
                        AttachedFile = AttachmentList[0];
                        IsVisible = Visibility.Visible;
                    }
                    else
                        IsVisible = Visibility.Hidden;

                    //ChangeFileUpload = true;
                }
                if (obj != null)
                {
                    IsBusy = false;
                }
                IsAdd = true;
            }
            catch (Exception ex)
            {
                if (obj != null)
                {
                    IsBusy = false;
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseFileAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri     
        /// [002][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// [003][cpatil][25-12-2019][GEOS2-1941] GRHM - Calculation of holidays
        /// [004][skale][31/01/2020][GEOS2-1959]- GHRM - Leaves/Attendance inactive employees
        /// [005][cpatil][27-08-2020][GEOS2-2486] Wrong counting holidays - if many plants are selected, the program counts all official holidays for a selected employee.
        /// </summary>
        private void AddNewLeaveInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewLeaveInformation()...", category: Category.Info, priority: Priority.Low);

                var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(i => i.IdCompany));
                var SelectedEmployeeListJoined = string.Join(",", SelectedEmployeeList);

              // if (ExistEmployeeLeaveList == null || ExistEmployeeLeaveList.Count == 0)
              //{
                    //shubham[skadam] GEOS2-3919 HRM - Register different leaves at the same time 11 OCT 2022
                    ExistEmployeeLeaveList = new ObservableCollection<EmployeeLeave>();
                    ExistEmployeeLeaveList.AddRange(HrmService.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate.Value, EndDate.Value, SelectedEmployeeListJoined));
//}
            //   if (EmployeeAttendanceList == null || EmployeeAttendanceList.Count == 0)
            //    {
                    EmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    EmployeeAttendanceList.AddRange(HrmService.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate.Value, EndDate.Value, SelectedEmployeeListJoined));
          //    }

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLeaveType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));

                List<Attachment> temp = new List<Attachment>();
                if (error != null)
                {
                    return;
                }

                IsBusy = true;
                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();

                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    LeaveFileName = null;
                    LeaveFileInBytes = null;
                }

                StartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                EndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                STime = StartTime.Value.TimeOfDay;
                ETime = EndDate.Value.TimeOfDay;
                AddEmployeeLeaveChangeLogDetails();

                //[GEOS2-3944][rdixit][19.10.2022]
                if ((EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue :0) == 171)
                {
                    string BackLogEmployeeLeaveMsg = "";
                    foreach (int item in SelectedEmployeeList)
                    {
                        Employee employee = EmployeeListFinal.Where(i => i.IdEmployee == item).FirstOrDefault();
                        if (StartDate != null && EndDate != null)
                        {
                            EmployeeWithBackupEmployeeDetails EmployeeWithBackupEmployeeDetails = HrmService.GetEmployeeWithBackupEmployeeDetails(employee.IdEmployee, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                            if (EmployeeWithBackupEmployeeDetails != null)
                            {
                                if (EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves != null && IsAllDayEvent == true)
                                {
                                    string StartDate = string.Empty;
                                    string EndDate = string.Empty;
                                    if (EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().StartDate != null)
                                        StartDate = EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().StartDate.Value.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

                                    if (EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().EndDate != null)
                                        EndDate = EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().EndDate.Value.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

                                    if (BackLogEmployeeLeaveMsg == "")
                                    {
                                        BackLogEmployeeLeaveMsg = Application.Current.Resources["BackupEmpLeaveExist"].ToString() + "\n";
                                    }
                                    BackLogEmployeeLeaveMsg = BackLogEmployeeLeaveMsg + string.Format
                                        (
                                        Application.Current.Resources["BackupEmpLeaveDates"].ToString(),
                                        (EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().Employee.FirstName + " " +
                                        EmployeeWithBackupEmployeeDetails.BackupEmployeeLeaves.FirstOrDefault().Employee.LastName),
                                        StartDate,
                                        EndDate
                                        ) + "\n";
                                }
                            }
                        }
                    }
                    if (BackLogEmployeeLeaveMsg != "" && BackLogEmployeeLeaveMsg != null)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(BackLogEmployeeLeaveMsg, "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    }
                }
                //[004] added 
                List<int> SelectedEmployeesList = SelectedEmployeeList.Cast<int>().ToList();
                var EmployeeIds = string.Join(",", SelectedEmployeesList.Select(i => i));
                List<EmployeeContractSituation> SelectedEmployeeContractList = HrmService.GetEmployeeContracts(EmployeeIds);
                //end

                NewEmployeeLeaveList = new List<EmployeeLeave>();
                string EmployeeOverlappedLeavesWarning = "\n";
                List<EmployeeLeave> ExistEmpLeaveList = new List<EmployeeLeave>();
                bool IsLeave = true;
                bool IsAttendance = true;
                bool IsAuthorizedLeave;
                bool IsEmployeeOverlappedLeavesWarning = false;
                bool IsEmployeeAuthorizedLeveError = false;
                string EmployeeAuthorizedLeveError = "\n";
                
                bool IsInactiveEmployeeLeave = true;
                string EmployeeInactiveLeaveError = "\n";

                List<string> ErrorMessageList = new List<string>();


                for (int j = 0; j < SelectedEmployeeList.Count; j++)
                {
                    if (IsNew)
                    {
                        ExistEmpLeaveList = ExistEmployeeLeaveList.Where(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]) && (StartDate >= x.StartDate && StartDate <= x.EndDate || EndDate <= x.StartDate && EndDate >= x.EndDate)).ToList();
                    }
                    else
                    {
                        ExistEmpLeaveList = ExistEmployeeLeaveList.Where(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]) && x.IdEmployeeLeave != IdEmployeeLeave && (StartDate >= x.StartDate && StartDate <= x.EndDate || EndDate <= x.StartDate && EndDate >= x.EndDate)).ToList();
                    }

                    IsLeave = true;
                    IsAttendance = true;

                    IsAuthorizedLeave = false;

                    Employee tempEmployee = EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j]));

                    if (tempEmployee != null)
                    {
                        if (tempEmployee.EmployeeAnnualLeaves != null && tempEmployee.EmployeeAnnualLeaves.Count > 0)
                        {
                            if (tempEmployee.EmployeeAnnualLeaves.Any(x => x.IdLeave == (EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0)))
                                IsAuthorizedLeave = true;
                            else
                            {
                                EmployeeAuthorizedLeveError = EmployeeAuthorizedLeveError + tempEmployee.FullName + "\n";
                                IsEmployeeAuthorizedLeveError = true;
                            }
                        }
                        else
                        {
                            EmployeeAuthorizedLeveError = EmployeeAuthorizedLeveError + tempEmployee.FullName + "\n";
                            IsEmployeeAuthorizedLeveError = true;
                        }
                    }

                    if (IsEmployeeAuthorizedLeveError && SelectedEmployeeList.Count == 1)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    for (int i = 0; i < ExistEmpLeaveList.Count; i++)
                    {
                        if (IsAllDayEvent == true)
                        {
                            IsLeave = false;
                            break;
                        }
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
                    if (IsLeave == false && SelectedEmployeeList.Count == 1)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    var ExistEmpAttendanceList = EmployeeAttendanceList.Where(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]) && x.StartDate.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                    for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
                    {
                        if (IsAllDayEvent == true)
                        {
                            IsAttendance = false;
                            break;
                        }

                        if (i == 0)
                        {
                            if (StartDate < ExistEmpAttendanceList[i].StartDate && EndDate <= ExistEmpAttendanceList[i].StartDate)
                            {
                                IsAttendance = true;
                                break;
                            }
                            else if (ExistEmpAttendanceList.Count == 1)
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
                                    if (StartDate >= ExistEmpAttendanceList[i].EndDate && EndDate > ExistEmpAttendanceList[i].EndDate)
                                    {
                                        IsAttendance = true;
                                        break;
                                    }
                                }

                            }
                        }

                        IsAttendance = false;
                    }

                    if (IsAttendance == false && SelectedEmployeeList.Count == 1)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    //[004] added 
                    if (tempEmployee != null)
                    {

                        if (SelectedEmployeeContractList != null && SelectedEmployeeContractList.Count > 0)
                        {
                            List<EmployeeContractSituation> EmployeeContractSituationList = SelectedEmployeeContractList.Where(x=>x.IdEmployee== tempEmployee.IdEmployee).ToList();

                            if (EmployeeContractSituationList != null && EmployeeContractSituationList.Count > 0)
                            {
                                for (int i = 0; i < EmployeeContractSituationList.Count; i++)
                                {

                                    if (EmployeeContractSituationList[i].ContractSituationEndDate == null)
                                    {
                                        DateTime? EmployeeContractEndDate = EmployeeContractSituationList[i].ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : EmployeeContractSituationList[i].ContractSituationEndDate;

                                        if (StartDate.Value.Date >= EmployeeContractSituationList[i].ContractSituationStartDate.Value.Date && 
                                            (EndDate.Value.Date <= EmployeeContractEndDate || EndDate.Value.Date >= EmployeeContractEndDate))
                                        {
                                            IsInactiveEmployeeLeave = true;
                                            break;
                                        }
                                        else
                                        {
                                            IsInactiveEmployeeLeave = false;
                                        }
                                    }
                                   else if (StartDate.Value.Date >= EmployeeContractSituationList[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= EmployeeContractSituationList[i].ContractSituationEndDate.Value.Date)
                                    {
                                        IsInactiveEmployeeLeave = true;
                                        break;
                                    }
                                    else
                                    {
                                        IsInactiveEmployeeLeave = false;
                                    }
                                }

                                if (!IsInactiveEmployeeLeave)
                                {
                                    EmployeeInactiveLeaveError = EmployeeInactiveLeaveError + tempEmployee.FullName +"\n";
                                    ErrorMessageList.Add(EmployeeInactiveLeaveError);
                                }

                                if (!IsInactiveEmployeeLeave && SelectedEmployeeList.Count == 1)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }

                        }
                      
                    }//end

                    if (IsNew == true && IsLeave == true && IsAttendance == true && IsAuthorizedLeave && IsInactiveEmployeeLeave) 
                    //  if (IsNew == true && IsLeave == true && IsAttendance == true && IsAuthorizedLeave)
                    {
                        //if StartDate and end Date year not same split in two 
                        List<Tuple<DateTime, DateTime>> dtSeperatedByYear = new List<Tuple<DateTime, DateTime>>(SeperateStartEndDateIfYearNotSame(new Tuple<DateTime, DateTime>(StartDate.Value, EndDate.Value)));
                        if (dtSeperatedByYear != null && dtSeperatedByYear.Count > 0)
                        {
                            foreach (var item in dtSeperatedByYear)
                            {
                                EmployeeLeave TempEmployeeLeave = new EmployeeLeave()
                                {
                                    //Employee = EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))],
                                    Employee = EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j])),
                                    //[001] Code Comment and Get Company Leave using Lookup value
                                    //CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                                    CompanyLeave = EmployeeLeaveList != null ? GetCompanyLeave(EmployeeLeaveList[SelectedLeaveType]) : null,
                                    IdEmployee = Convert.ToInt16(SelectedEmployeeList[j]),
                                    StartDate = item.Item1,
                                    EndDate = item.Item2,
                                    //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                                    IdLeave = EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0,
                                    Remark = Remarks,
                                    FileName = LeaveFileName,
                                    IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),

                                    EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                                    /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                                    IdCompanyShift = CompanyShiftIdArray[j],
                                    CompanyShift = CompanyShiftArray[j],
                                    //[GEOS2-3095]
                                    Creator = GeosApplication.Instance.ActiveUser.IdUser,
                                    CreationDate = GeosApplication.Instance.ServerDateTime


                                };
                                //[002]added
                                string[] IdEmployeeCompany = TempEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                                CompanyList = new List<Company>(SelectedPlantList.Where(x => x.IdCompany == Convert.ToInt32(IdEmployeeCompany[0])).ToList());
                                TempEmployeeLeave.CompanyLeave.Company = CompanyList.FirstOrDefault();
                                NewEmployeeLeaveList.Add(TempEmployeeLeave);

                            }
                        }
                        else
                        {
                            EmployeeLeave TempEmployeeLeave = new EmployeeLeave()
                            {
                                //Employee = EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))],
                                Employee = EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j])),
                                //[001] Code Comment and Get Company Leave using Lookup value
                                //CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                                CompanyLeave = EmployeeLeaveList !=null?GetCompanyLeave(EmployeeLeaveList[SelectedLeaveType]) : null,
                                IdEmployee = Convert.ToInt16(SelectedEmployeeList[j]),
                                StartDate = StartDate,
                                EndDate = EndDate,
                                //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                                
                                IdLeave = EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0,
                                Remark = Remarks,
                                FileName = LeaveFileName,
                                IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                                EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                                IdCompanyShift = CompanyShiftIdArray[j],
                                CompanyShift = CompanyShiftArray[j],
                                //[GEOS2-3095]
                                Creator = GeosApplication.Instance.ActiveUser.IdUser,
                                CreationDate = GeosApplication.Instance.ServerDateTime
                            };
                            //[002]added
                            string[] IdEmployeeCompany = TempEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                            CompanyList = new List<Company>(SelectedPlantList.Where(x => x.IdCompany == Convert.ToInt32(IdEmployeeCompany[0])).ToList());
                            
                            NewEmployeeLeaveList.Add(TempEmployeeLeave);
                        }
                    }
                    else if (IsLeave == false || IsAttendance == false)
                    {
                        //EmployeeOverlappedLeavesWarning = EmployeeOverlappedLeavesWarning + EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))].FullName + "\n";
                        if (IsAuthorizedLeave)
                        {
                            EmployeeOverlappedLeavesWarning = EmployeeOverlappedLeavesWarning + EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j])).FullName + "\n";
                            IsEmployeeOverlappedLeavesWarning = true;
                        }

                    }
                }

                if (IsNew == true)
                {
                    IsBusy = false;
                    if (NewEmployeeLeaveList.Count < SelectedEmployeeList.Count)
                    {
                        if (NewEmployeeLeaveList.Count >= 1)
                        {
                            if (IsEmployeeOverlappedLeavesWarning && IsEmployeeAuthorizedLeveError)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            }
                            else if (IsEmployeeAuthorizedLeveError)
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            else if (IsEmployeeOverlappedLeavesWarning)
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                           else  if (ErrorMessageList !=null && ErrorMessageList.Count >0)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                        else
                        {
                            if (IsEmployeeOverlappedLeavesWarning && IsEmployeeAuthorizedLeveError)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }

                            if (IsEmployeeAuthorizedLeveError)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }

                            if (IsEmployeeOverlappedLeavesWarning)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            else if (ErrorMessageList != null && ErrorMessageList.Count > 0)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }

                   
                    IsBusy = true;
                    //NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2045(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                    //AddEmployeeLeavesFromList_V2170 Service Updated with AddEmployeeLeavesFromList_V2340 by [rdixit][GEOS2-3263][28.11.2022]
                    //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
                    NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2410(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                    IsSave = true;

                    string EmployeeExceedAnnualLeavesWarning = string.Empty;
                    foreach (var newEmpLeave in NewEmployeeLeaveList)
                    {
                        // var employeeJobDescription = newEmpLeave.EmployeeJobDescription.LastOrDefault();

                        //[002] added
                        string[] IdCompanys = newEmpLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                        //[003] service method changed IsEmployeeEnjoyedAllAnnualLeavesSprint60 to IsEmployeeEnjoyedAllAnnualLeaves_V2038
                        //[005] service method changed IsEmployeeEnjoyedAllAnnualLeaves_V2038 to IsEmployeeEnjoyedAllAnnualLeaves_V2050
                      //  bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2050(newEmpLeave.IdEmployee, (int)(newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil
                        bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2140(newEmpLeave.IdEmployee, (int)(newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil

                        if (IsEnjoyedAllAnnualLeaves)
                        {
                            EmployeeAnnualLeave TempEmployeeAnnualLeave = new EmployeeAnnualLeave();
                            if (newEmpLeave.Employee.CompanyShift != null)
                            {
                                //[003] service method changed GetEmployeeEnjoyedLeaveHours to GetEmployeeEnjoyedLeaveHours_V2038
                                //[005] service method changed GetEmployeeEnjoyedLeaveHours_V2038 to GetEmployeeEnjoyedLeaveHours_V2050
                                //[3009] service method changed GetEmployeeEnjoyedLeaveHours_V2050 to GetEmployeeEnjoyedLeaveHours_V2140

                                TempEmployeeAnnualLeave = HrmService.GetEmployeeEnjoyedLeaveHours_V2140(newEmpLeave.IdEmployee, (newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil
                                decimal RemainingHours = TempEmployeeAnnualLeave.Remaining;

                                if (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount == 0)
                                {
                                    continue;
                                }

                                int Days = (int)(RemainingHours / (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount));
                                decimal Hours = RemainingHours % (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
                                string ExceededDaysAnsHours = string.Empty;

                                if (Days == 0)
                                {
                                    ExceededDaysAnsHours = Hours + "h";
                                }
                                else
                                {
                                    ExceededDaysAnsHours = Days + "d" + " and " + Hours + "H";
                                }

                                EmployeeExceedAnnualLeavesWarning = EmployeeExceedAnnualLeavesWarning + "   " + newEmpLeave.Employee.FullName + " - " + ExceededDaysAnsHours;
                            }
                        }
                    }
                    IsBusy = false;

                    if (!string.IsNullOrEmpty(EmployeeExceedAnnualLeavesWarning))
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeExceedAnnualLeavesWarning").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value : null, EmployeeExceedAnnualLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    //While update if StartDate and end Date year not same split in two (First one updated and second inserted)
                    List<Tuple<DateTime, DateTime>> dtSeperatedByYear = new List<Tuple<DateTime, DateTime>>(SeperateStartEndDateIfYearNotSame(new Tuple<DateTime, DateTime>(StartDate.Value, EndDate.Value)));
                    if (dtSeperatedByYear != null && dtSeperatedByYear.Count > 0)
                    {

                        //[001] Code Comment  and changes as per lookup value
                        UpdateEmployeeLeave = new EmployeeLeave()
                        {

                            Employee = EmployeeListFinal[SelectedIndexForEmployee],
                            // CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                            CompanyLeave = EmployeeLeaveList !=null ? GetCompanyLeave(EmployeeLeaveList[SelectedLeaveType]) : null,
                            IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                            StartDate = dtSeperatedByYear[0].Item1,
                            EndDate = dtSeperatedByYear[0].Item2,
                            //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                            
                            IdLeave = EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0,
                            Remark = Remarks,
                            FileName = LeaveFileName,
                            IdEmployeeLeave = IdEmployeeLeave,
                            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                            EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
							IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                            CompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift,
                            //[GEOS2-3095]
                            Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                            ModificationDate = GeosApplication.Instance.ServerDateTime
                        };

                        //UpdateEmployeeLeave = HrmService.UpdateEmployeeLeave_V2045(UpdateEmployeeLeave, SelectedPeriod);
                        //UpdateEmployeeLeave_V2170 Service Updated with UpdateEmployeeLeave_V2340 by [rdixit][GEOS2-3263][28.11.2022]
                        UpdateEmployeeLeave = HrmService.UpdateEmployeeLeave_V2340(UpdateEmployeeLeave, SelectedPeriod);

                        EmployeeLeave TempEmployeeLeave = new EmployeeLeave()
                        {
                            //Employee = EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))],
                            Employee = EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(EmployeeListFinal[SelectedIndexForEmployee].IdEmployee)),
                            //[001] Code Comment and Get Company Leave using Lookup value
                            //CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                            CompanyLeave = EmployeeLeaveList != null ? GetCompanyLeave(EmployeeLeaveList[SelectedLeaveType]) : null,
                            IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                            StartDate = dtSeperatedByYear[1].Item1,
                            EndDate = dtSeperatedByYear[1].Item2,
                            //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                            IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                            IdLeave = EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0,
                            Remark = Remarks,
                            FileName = LeaveFileName,
                            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                            EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                            CompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift,

                            //[GEOS2-3095]
                            Creator = GeosApplication.Instance.ActiveUser.IdUser,
                            CreationDate = GeosApplication.Instance.ServerDateTime
                    };
                        //[002]added
                        string[] IdEmployeeCompany = TempEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                        CompanyList = new List<Company>(SelectedPlantList.Where(x => x.IdCompany == Convert.ToInt32(IdEmployeeCompany[0])).ToList());
                        TempEmployeeLeave.CompanyLeave.Company = CompanyList.FirstOrDefault();
                        NewEmployeeLeaveList.Add(TempEmployeeLeave);
                        IsBusy = true;
                        //NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2045(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                        //AddEmployeeLeavesFromList_V2170 Service Updated with AddEmployeeLeavesFromList_V2340 by [rdixit][GEOS2-3263][28.11.2022]
                        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
                        NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2410(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                        IsSave = true;
                    }
                    else
                    {
                        UpdateEmployeeLeave = new EmployeeLeave()
                        {
                            Employee = EmployeeListFinal[SelectedIndexForEmployee],
                            // CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                            CompanyLeave = EmployeeLeaveList != null ? GetCompanyLeave(EmployeeLeaveList[SelectedLeaveType]) : null,
                            IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                            StartDate = StartDate,
                            EndDate = EndDate,
                            //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                            IdLeave = EmployeeLeaveList !=null ? EmployeeLeaveList[SelectedLeaveType].IdLookupValue : 0,
                            IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
                            Remark = Remarks,
                            FileName = LeaveFileName,
                            IdEmployeeLeave = IdEmployeeLeave,
                            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),
                            EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                            CompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift,
                            //[GEOS2-3095]
                            Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                            ModificationDate = GeosApplication.Instance.ServerDateTime
                        };

                        //UpdateEmployeeLeave = HrmService.UpdateEmployeeLeave_V2045(UpdateEmployeeLeave, SelectedPeriod);
                        //UpdateEmployeeLeave_V2170 Service Updated with UpdateEmployeeLeave_V2340 by [rdixit][GEOS2-3263][28.11.2022]
                        UpdateEmployeeLeave = HrmService.UpdateEmployeeLeave_V2340(UpdateEmployeeLeave, SelectedPeriod);
                    }

                    UpdateEmployeeLeave.CompanyLeave.Company = OldEmployeeLeaveDetatils.CompanyLeave.Company;

                    if (LeaveFileName == null || OldLeaveFileName != LeaveFileName)
                    {
                        HrmService.DeleteEmployeeLeaveAttachment(EmployeeListFinal[SelectedIndexForEmployee].EmployeeCode, UpdateEmployeeLeave.IdEmployeeLeave, OldLeaveFileName);
                    }

                    if (LeaveFileInBytes != null)
                    {
                        HrmService.SaveEmployeeLeaveAttachment(EmployeeListFinal[SelectedIndexForEmployee].EmployeeCode, UpdateEmployeeLeave.IdEmployeeLeave, LeaveFileName, LeaveFileInBytes);
                    }

                    IsSave = true;

                    //[002]added
                    string[] idEmployeeCompany = UpdateEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                    //[003] service method changed IsEmployeeEnjoyedAllAnnualLeavesSprint60 to IsEmployeeEnjoyedAllAnnualLeaves_V2038
                    //[005] service method changed IsEmployeeEnjoyedAllAnnualLeaves_V2038 to IsEmployeeEnjoyedAllAnnualLeaves_V2050
                   // bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2050(UpdateEmployeeLeave.IdEmployee, (int)(UpdateEmployeeLeave.IdLeave), SelectedPeriod, Convert.ToInt32(idEmployeeCompany[0]));//swapnil
                    bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2140(UpdateEmployeeLeave.IdEmployee, (int)(UpdateEmployeeLeave.IdLeave), SelectedPeriod, Convert.ToInt32(idEmployeeCompany[0]));//swapnil

                    if (IsEnjoyedAllAnnualLeaves)
                    {
                        EmployeeAnnualLeave TempEmployeeAnnualLeave = new EmployeeAnnualLeave();
                        if (UpdateEmployeeLeave.Employee.CompanyShift != null)
                        {
                            decimal RemainingHours = 0;
                            TempEmployeeAnnualLeave = UpdateEmployeeLeave.Employee.EmployeeAnnualLeaves.FirstOrDefault(x => x.IdLeave == UpdateEmployeeLeave.IdLeave);
                            //[005] service method changed GetEmployeeEnjoyedLeaveHours_V2038 to GetEmployeeEnjoyedLeaveHours_V2050
                            //[005] service method changed GetEmployeeEnjoyedLeaveHours_V2050 to GetEmployeeEnjoyedLeaveHours_V2140
                            if (TempEmployeeAnnualLeave == null)
                                TempEmployeeAnnualLeave = HrmService.GetEmployeeEnjoyedLeaveHours_V2140(UpdateEmployeeLeave.IdEmployee, (UpdateEmployeeLeave.IdLeave), SelectedPeriod, Convert.ToInt32(idEmployeeCompany[0]));
                            RemainingHours = TempEmployeeAnnualLeave.Remaining;
                            int Days = (int)(RemainingHours / (UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount));
                            decimal Hours = (RemainingHours % (UpdateEmployeeLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount));
                            string ExceededDaysAnsHours = string.Empty;
                            string EmployeeExceedAnnualLeavesWarning = string.Empty;

                            if (Days == 0)
                            {
                                ExceededDaysAnsHours = Hours + "h";
                            }
                            else
                            {
                                ExceededDaysAnsHours = Days + "d" + " and " + Hours + "H";
                            }

                            EmployeeExceedAnnualLeavesWarning = EmployeeExceedAnnualLeavesWarning + "\n" + UpdateEmployeeLeave.Employee.FullName + " - " + ExceededDaysAnsHours;
                            if (!string.IsNullOrEmpty(EmployeeExceedAnnualLeavesWarning))
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeExceedAnnualLeavesWarning").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[SelectedLeaveType].Value: null, EmployeeExceedAnnualLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                        }
                    }

                    IsBusy = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }

                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Method AddNewLeaveInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewLeaveInformation() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewLeaveInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewLeaveInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //Check if startdate and enddate year not same then split it in two seperate leaves
        private List<Tuple<DateTime, DateTime>> SeperateStartEndDateIfYearNotSame(Tuple<DateTime, DateTime> tupStartEnd)
        {
            List<Tuple<DateTime, DateTime>> ListTuple = new List<Tuple<DateTime, DateTime>>();

            if (tupStartEnd.Item1.Year != tupStartEnd.Item2.Year)
            {
                List<DateTime> dtStart = new List<DateTime>();
                for (DateTime i = tupStartEnd.Item1; i <= tupStartEnd.Item2;)
                {
                    dtStart.Add(i);
                    i = i.AddDays(1);
                    if (i.Year == tupStartEnd.Item2.Year)
                        break;
                }

                ListTuple.Add(new Tuple<DateTime, DateTime>(dtStart.FirstOrDefault(), dtStart.LastOrDefault()));
                ListTuple.Add(new Tuple<DateTime, DateTime>(dtStart.LastOrDefault().AddDays(1), tupStartEnd.Item2));

            }
            return ListTuple;
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        private void FillFromDates()
        {
            FromDates = new List<DateTime>();

            double addMinutes = 0;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            for (DateTime i = today; i < tomorrow; i = i.AddMinutes(1))
            {
                DateTime dtotherdate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, i.Hour, i.Minute, i.Second);
                FromDates.Add(dtotherdate);
            }
        }

        private void FillToDates()
        {
            ToDates = new List<DateTime>();

            double addMinutes = 0;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            for (DateTime i = today; i < tomorrow; i = i.AddMinutes(1))
            {
                DateTime dtotherdate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, i.Hour, i.Minute, i.Second);
                ToDates.Add(dtotherdate);
            }
        }

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
                me[BindableBase.GetPropertyName(() => SelectedLeaveType)]+
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
                string leaveStartTime = BindableBase.GetPropertyName(() => StartTime);
                string leaveEndTime = BindableBase.GetPropertyName(() => EndTime);
                string leaveStartDate = BindableBase.GetPropertyName(() => StartDate);
                string leaveEndDate = BindableBase.GetPropertyName(() => EndDate);
                string selectedType = BindableBase.GetPropertyName(() => SelectedLeaveType);
                string selectedShift = BindableBase.GetPropertyName(() => SelectedIndexForCompanyShift);

                if (columnName == empName)
                {
                    return LeaveValidations.GetErrorMessage(empName, SelectedIndexForEmployee);
                }

                if (columnName == leaveStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return LeaveValidations.GetErrorMessage(leaveStartDate, StartDate);
                    }
                }

                if (columnName == leaveEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                }

                if (columnName == leaveStartTime)
                {
                    if (!string.IsNullOrEmpty(StartTimeErrorMessage))
                    {
                        return StartTimeErrorMessage;
                    }
                    else
                    {
                        return LeaveValidations.GetErrorMessage(leaveStartTime, StartTime);
                    }
                }

                if (columnName == leaveEndTime)
                {
                    if (!string.IsNullOrEmpty(EndTimeErrorMessage))
                    {
                        return EndTimeErrorMessage;
                    }
                    else
                    {
                        return LeaveValidations.GetErrorMessage(leaveEndTime, EndTime);
                    }
                }

                if (columnName == selectedType)
                {
                    return LeaveValidations.GetErrorMessage(selectedType, SelectedLeaveType);
                }

                if (columnName == leaveStartDate)
                {
                    return LeaveValidations.GetErrorMessage(leaveStartDate, StartDate);
                }

                if (columnName == leaveEndDate)
                {
                    return LeaveValidations.GetErrorMessage(leaveEndDate, EndDate);
                }
				/// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                if (columnName == selectedShift)
                {
                    return LeaveValidations.GetErrorMessage(selectedShift, SelectedIndexForCompanyShift);
                }

                return null;
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to fill EmployeeLeave from Lookup values
        /// </summary>
        public void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {
                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32));
                    GeosApplication.Instance.EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// Function Created to Fill CompanyWork from Lookup values
        /// </summary>
        public CompanyLeave GetCompanyLeave(LookupValue obj)
        {
            CompanyLeave companyLeave = new CompanyLeave
            {
                IdCompanyLeave = (ulong)obj.IdLookupValue,
                Name = obj.Value
            };
            return companyLeave;
        }

        /// <summary>
        /// HRM	It must not be possible to put the same hour for a leave by sdesai
        /// Method to check date time validation on all day event
        /// </summary>
        /// <param name="obj"></param>
        private void CheckedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckedCommandAction()...", category: Category.Info, priority: Priority.Low);

                CheckDateTimeValidation();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectedItemChangedCommandAction(EditValueChangedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (obj.NewValue == null)
                {

                    AttachmentList.Clear();
                    IsVisible = Visibility.Collapsed;
                }
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedItemChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenEmployeeLeaveDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()...", category: Category.Info, priority: Priority.Low);
                if (IsAdd)
                {
                    byte[] EmployeeLeaveAttachmentBytes = LeaveFileInBytes;
                    EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                    EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                    if (EmployeeLeaveAttachmentBytes != null)
                    {
                        employeeDocumentViewModel.OpenPdfFromBytes(EmployeeLeaveAttachmentBytes, LeaveFileName);

                        employeeDocumentView.DataContext = employeeDocumentViewModel;
                        employeeDocumentView.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Could not find file {0}", LeaveFileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    byte[] EmployeeLeaveAttachmentBytes = HrmService.GetEmployeeLeaveAttachment(OldEmployeeLeaveDetatils);
                    EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                    EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                    if (EmployeeLeaveAttachmentBytes != null)
                    {
                        employeeDocumentViewModel.OpenPdfFromBytes(EmployeeLeaveAttachmentBytes, OldEmployeeLeaveDetatils.FileName);

                        employeeDocumentView.DataContext = employeeDocumentViewModel;
                        employeeDocumentView.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Could not find file {0}", OldEmployeeLeaveDetatils.FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeLeaveDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeLeaveDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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


        #region Split attendance logic

        private ObservableCollection<AddNewLeaveViewModelWithSplittingTask> tasks = new ObservableCollection<AddNewLeaveViewModelWithSplittingTask>();
        private bool isSplit;
        public ObservableCollection<AddNewLeaveViewModelWithSplittingTask> Tasks
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

        bool isSplitVisible;

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
        private void SplitInformation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow ...", category: Category.Info, priority: Priority.Low);

                SelectedViewIndex = 1;
                for (int i = 0; i < this.ListOfleaveStartTimeAndEndTime.Count; i++)
                {
                    AddNewLeaveViewModelWithSplittingTask addNewLeaveViewModelWithSplittingTask = new AddNewLeaveViewModelWithSplittingTask(null)
                    {
                        SelectedLeaveType = this.SelectedLeaveType,
                        EmployeeAttendanceList = this.EmployeeAttendanceList,                      
                        IsVisible = this.IsVisible,
                        IsAdd = this.IsAdd,
                        SelectedEmployeeList = this.SelectedEmployeeList,
                        CompanyShiftIdArray = this.CompanyShiftIdArray,
                        CompanyShiftArray = this.CompanyShiftArray,
                        EmployeeShiftList = this.EmployeeShiftList,
                        SelectedEmployeeShift = this.SelectedEmployeeShift,
                        Header = $"Leave{i+1}"
                    };
              
                    addNewLeaveViewModelWithSplittingTask.Init(
                        this.ExistEmployeeLeaveList,this.SelectedEmployee,
                        this.ListOfleaveStartTimeAndEndTime[i].StartDate.Value, this.ListOfleaveStartTimeAndEndTime[i].EndDate.Value,
                        1, this.WorkingPlantId, EmployeeListFinal, this.DefaultLeaveType,
                         this.IsAllDayEvent, null, defaultIdCompanyShift);

                    Tasks.Add(addNewLeaveViewModelWithSplittingTask);
                }            
                            
                GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SplitLeadOfferViewWindowShow()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            //end
        }
        
        public EmployeeAttendance NewSplitEmployeeAttendance;
        public EmployeeAttendance UpdatesplitEmployeeAttendance;
        private void SaveAttendanceWithSplit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveAttendanceWithSplit()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                //bool IsLeave = true;
                //bool IsAttendance = true;
                bool errorFound = false;
                //This is called to check validations;
                foreach (var itemTask in Tasks)
                {
                    string error = itemTask.CheckValidation();
                    if (error != null)
                    {
                        errorFound = true;
                    }
                }

                if(errorFound)
                {
                    return;
                }
                
                var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(i => i.IdCompany));
                var alltasksSelectedEmployeeList = new List<object>();

                foreach (var itemTask in Tasks)
                {
                    foreach (var itemTaskSelectedEmployeeID in itemTask.SelectedEmployeeList)
                    {
                        if (!alltasksSelectedEmployeeList.Contains(itemTaskSelectedEmployeeID))
                        {
                            alltasksSelectedEmployeeList.Add(itemTaskSelectedEmployeeID);
                        }
                        //else
                        //{ }
                    }
                }

                var SelectedEmployeeListJoined = string.Join(",", alltasksSelectedEmployeeList);

                var minimumStartDateofAllTasks = Tasks[0].StartDate.Value;
                var maximumEndDateofAllTasks = Tasks[0].EndDate.Value;
                
                foreach (var itemTask in Tasks)
                {
                    if (itemTask.StartDate.Value < minimumStartDateofAllTasks)
                    {
                        minimumStartDateofAllTasks = itemTask.StartDate.Value;
                    }

                    if (itemTask.EndDate.Value > maximumEndDateofAllTasks)
                    {
                        maximumEndDateofAllTasks = itemTask.EndDate.Value;
                    }

                }
                // if (ExistEmployeeLeaveList == null || ExistEmployeeLeaveList.Count == 0)
                //{
                    //shubham[skadam] GEOS2-3919 HRM - Register different leaves at the same time 11 OCT 2022
                var allTasksExistEmployeeLeaveList = new ObservableCollection<EmployeeLeave>();
                allTasksExistEmployeeLeaveList.AddRange(HrmService.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(
                    plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, 
                    HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, 
                    HrmCommon.Instance.IdUserPermission, minimumStartDateofAllTasks, maximumEndDateofAllTasks, SelectedEmployeeListJoined));
                //}
                //   if (EmployeeAttendanceList == null || EmployeeAttendanceList.Count == 0)
                //    {
                var allTasksEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                allTasksEmployeeAttendanceList.AddRange(HrmService.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(
                    plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, 
                    HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, 
                    HrmCommon.Instance.IdUserPermission, minimumStartDateofAllTasks, maximumEndDateofAllTasks, SelectedEmployeeListJoined));
                //    }
                foreach (var itemTask in Tasks)
                {
                    List<Attachment> temp = new List<Attachment>();

                    IsBusy = true;
                    if (!string.IsNullOrEmpty(itemTask.Remarks))
                        itemTask.Remarks = itemTask.Remarks.Trim();

                    if (itemTask.AttachmentList != null && itemTask.AttachmentList.Count == 0)
                    {
                        itemTask.LeaveFileName = null;
                        itemTask.LeaveFileInBytes = null;
                    }
                }

                // AddEmployeeLeaveOverlapped
                bool overlappingLeavesNotFound = false;
                for (int i = 0; i < Tasks.Count; i++)
                {
                    for (int j = 0; j < Tasks.Count; j++)
                    {
                        if(i != j)
                        {
                            var SelectedStartDateStartOfDay = Tasks[i].StartDate.Value.Date.AddHours(Tasks[i].StartTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i].StartTime.Value.TimeOfDay.Minutes);
                            var SelectedEndDateEndOfDay = Tasks[i].EndDate.Value.Date.AddHours(Tasks[i].EndTime.Value.TimeOfDay.Hours).AddMinutes(Tasks[i].EndTime.Value.TimeOfDay.Minutes);
                            var x = Tasks[j];

                            var overlappingLeavesFound = 
                                    (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay <= x.EndDate) ||
                                    (SelectedStartDateStartOfDay < x.StartDate && SelectedEndDateEndOfDay <= x.EndDate && SelectedEndDateEndOfDay >= x.StartDate) ||
                                    (SelectedStartDateStartOfDay >= x.StartDate && SelectedEndDateEndOfDay > x.EndDate && SelectedStartDateStartOfDay <= x.EndDate) ||
                                    (SelectedStartDateStartOfDay <= x.StartDate && SelectedEndDateEndOfDay >= x.EndDate);

                            if(overlappingLeavesFound)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }
                }

                foreach (var itemTask in Tasks)
                {
                    var itemStartDate = itemTask.StartDate.Value.Date.AddHours(itemTask.StartTime.Value.TimeOfDay.Hours).AddMinutes(itemTask.StartTime.Value.TimeOfDay.Minutes);
                    var itemEndDate = itemTask.EndDate.Value.Date.AddHours(itemTask.EndTime.Value.TimeOfDay.Hours).AddMinutes(itemTask.EndTime.Value.TimeOfDay.Minutes);
                    var itemSTime = itemTask.StartTime.Value.TimeOfDay;
                    var itemETime = itemTask.EndDate.Value.TimeOfDay;

                    List<int> itemSelectedEmployeesList = itemTask.SelectedEmployeeList.Cast<int>().ToList();
                    var itemEmployeeIds = string.Join(",", itemSelectedEmployeesList.Select(i => i));
                    List<EmployeeContractSituation> itemSelectedEmployeeContractList = HrmService.GetEmployeeContracts(itemEmployeeIds);

                    NewEmployeeLeaveList = new List<EmployeeLeave>();
                    string EmployeeOverlappedLeavesWarning = "\n";
                    List<EmployeeLeave> ExistEmpLeaveList = new List<EmployeeLeave>();
                    bool IsLeave = true;
                    bool IsAttendance = true;
                    bool IsAuthorizedLeave;
                    bool IsEmployeeOverlappedLeavesWarning = false;
                    bool IsEmployeeAuthorizedLeveError = false;
                    string EmployeeAuthorizedLeveError = "\n";

                    bool IsInactiveEmployeeLeave = true;
                    string EmployeeInactiveLeaveError = "\n";

                    List<string> ErrorMessageList = new List<string>();

                    for (int j = 0; j < itemSelectedEmployeesList.Count; j++)
                    {
                        //if (itemTask.IsNew)
                        //{
                        ExistEmpLeaveList = allTasksExistEmployeeLeaveList.Where(
                            x => x.IdEmployee == Convert.ToInt16(itemTask.SelectedEmployeeList[j]) &&
                            (itemTask.StartDate >= x.StartDate && itemTask.StartDate <= x.EndDate ||
                            itemTask.EndDate <= x.StartDate && itemTask.EndDate >= x.EndDate)).ToList();
                        //}
                        //else
                        //{
                        //    ExistEmpLeaveList = allTasksExistEmployeeLeaveList.Where(
                        //        x => x.IdEmployee == Convert.ToInt16(itemTask.SelectedEmployeeList[j]) && 
                        //        x.IdEmployeeLeave != itemTask.IdEmployeeLeave && 
                        //        (itemTask.StartDate >= x.StartDate && itemTask.StartDate <= x.EndDate || itemTask.EndDate <= x.StartDate && itemTask.EndDate >= x.EndDate)).ToList();
                        //}

                        IsLeave = true;
                        IsAttendance = true;

                        IsAuthorizedLeave = false;

                        Employee tempEmployee = itemTask.EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(itemTask.SelectedEmployeeList[j]));

                        if (tempEmployee != null)
                        {
                            if (tempEmployee.EmployeeAnnualLeaves != null && tempEmployee.EmployeeAnnualLeaves.Count > 0)
                            {
                                if (tempEmployee.EmployeeAnnualLeaves.Any(x => x.IdLeave == (EmployeeLeaveList != null ? EmployeeLeaveList[itemTask.SelectedLeaveType]?.IdLookupValue : 0)))
                                    IsAuthorizedLeave = true;
                                else
                                {
                                    EmployeeAuthorizedLeveError = EmployeeAuthorizedLeveError + tempEmployee.FullName + "\n";
                                    IsEmployeeAuthorizedLeveError = true;
                                }
                            }
                            else
                            {
                                EmployeeAuthorizedLeveError = EmployeeAuthorizedLeveError + tempEmployee.FullName + "\n";
                                IsEmployeeAuthorizedLeveError = true;
                            }
                        }
                        //Created by shubham[skadam] for GEOS2 https://helpdesk.emdep.com/browse/GEOS2-3555 DATE 31 03 2022
                        // Change  GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].Value)  to GeosApplication.Instance.EmployeeLeaveList[itemTask.SelectedLeaveType].Value)
                        if (IsEmployeeAuthorizedLeveError && SelectedEmployeeList.Count == 1)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), EmployeeLeaveList != null ? EmployeeLeaveList[itemTask.SelectedLeaveType].Value : null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        for (int i = 0; i < ExistEmpLeaveList.Count; i++)
                        {
                            if (itemTask.IsAllDayEvent == true)
                            {
                                IsLeave = false;
                                break;
                            }
                            if (ExistEmpLeaveList[i].IsAllDayEvent == 1)
                            {
                                IsLeave = false;
                                break;
                            }
                            if (i == 0)
                            {
                                if (itemTask.StartDate < ExistEmpLeaveList[i].StartDate && itemTask.EndDate <= ExistEmpLeaveList[i].StartDate)
                                {
                                    IsLeave = true;
                                    break;
                                }
                                if (ExistEmpLeaveList.Count == 1)
                                {
                                    if (itemTask.StartDate >= ExistEmpLeaveList[i].EndDate && itemTask.EndDate > ExistEmpLeaveList[i].EndDate)
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
                                    if (itemTask.StartDate >= ExistEmpLeaveList[i - 1].EndDate && itemTask.EndDate <= ExistEmpLeaveList[i].StartDate)
                                    {
                                        IsLeave = true;
                                        break;
                                    }
                                    else if (i == ExistEmpLeaveList.Count - 1)
                                    {
                                        if (itemTask.StartDate >= ExistEmpLeaveList[i].EndDate && itemTask.EndDate > ExistEmpLeaveList[i].EndDate)
                                        {
                                            IsLeave = true;
                                            break;
                                        }

                                    }
                                }

                            }

                            IsLeave = false;
                        }
                        if (IsLeave == false && itemTask.SelectedEmployeeList.Count == 1)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        var ExistEmpAttendanceList = allTasksEmployeeAttendanceList.Where(x => x.IdEmployee == Convert.ToInt16(itemTask.SelectedEmployeeList[j]) && x.StartDate.Date == itemTask.StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
                        for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
                        {
                            if (IsAllDayEvent == true)
                            {
                                IsAttendance = false;
                                break;
                            }

                            if (i == 0)
                            {
                                if (itemStartDate < ExistEmpAttendanceList[i].StartDate && itemEndDate <= ExistEmpAttendanceList[i].StartDate)
                                {
                                    IsAttendance = true;
                                    break;
                                }
                                else if (ExistEmpAttendanceList.Count == 1)
                                {
                                    if (itemStartDate >= ExistEmpAttendanceList[i].EndDate && itemEndDate > ExistEmpAttendanceList[i].EndDate)
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
                                    if (itemStartDate >= ExistEmpAttendanceList[i - 1].EndDate && itemEndDate <= ExistEmpAttendanceList[i].StartDate)
                                    {
                                        IsAttendance = true;
                                        break;
                                    }
                                    else if (i == ExistEmpAttendanceList.Count - 1)
                                    {
                                        if (itemStartDate >= ExistEmpAttendanceList[i].EndDate && itemEndDate > ExistEmpAttendanceList[i].EndDate)
                                        {
                                            IsAttendance = true;
                                            break;
                                        }
                                    }

                                }
                            }

                            IsAttendance = false;
                        }

                        if (IsAttendance == false && itemTask.SelectedEmployeeList.Count == 1)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                        ////[004] added 
                        //if (tempEmployee != null)
                        //{

                        //    if (itemSelectedEmployeeContractList != null && itemSelectedEmployeeContractList.Count > 0)
                        //    {
                        //        List<EmployeeContractSituation> EmployeeContractSituationList = itemSelectedEmployeeContractList.Where(x => x.IdEmployee == tempEmployee.IdEmployee).ToList();

                        //        if (EmployeeContractSituationList != null && EmployeeContractSituationList.Count > 0)
                        //        {
                        //            for (int i = 0; i < EmployeeContractSituationList.Count; i++)
                        //            {

                        //                if (EmployeeContractSituationList[i].ContractSituationEndDate == null)
                        //                {
                        //                    DateTime? EmployeeContractEndDate = EmployeeContractSituationList[i].ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : EmployeeContractSituationList[i].ContractSituationEndDate;

                        //                    if (StartDate.Value.Date >= EmployeeContractSituationList[i].ContractSituationStartDate.Value.Date &&
                        //                        (EndDate.Value.Date <= EmployeeContractEndDate || EndDate.Value.Date >= EmployeeContractEndDate))
                        //                    {
                        //                        IsInactiveEmployeeLeave = true;
                        //                        break;
                        //                    }
                        //                    else
                        //                    {
                        //                        IsInactiveEmployeeLeave = false;
                        //                    }
                        //                }
                        //                else if (StartDate.Value.Date >= EmployeeContractSituationList[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= EmployeeContractSituationList[i].ContractSituationEndDate.Value.Date)
                        //                {
                        //                    IsInactiveEmployeeLeave = true;
                        //                    break;
                        //                }
                        //                else
                        //                {
                        //                    IsInactiveEmployeeLeave = false;
                        //                }
                        //            }

                        //            if (!IsInactiveEmployeeLeave)
                        //            {
                        //                EmployeeInactiveLeaveError = EmployeeInactiveLeaveError + tempEmployee.FullName + "\n";
                        //                ErrorMessageList.Add(EmployeeInactiveLeaveError);
                        //            }

                        //            if (!IsInactiveEmployeeLeave && SelectedEmployeeList.Count == 1)
                        //            {
                        //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //                return;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //            return;
                        //        }

                        //    }

                        //}//end
                    }
                }
                    foreach (var itemTask in Tasks)
                    {
                      //  if (IsNew == true && IsLeave == true && IsAttendance == true && IsAuthorizedLeave && IsInactiveEmployeeLeave)
                        //  if (IsNew == true && IsLeave == true && IsAttendance == true && IsAuthorizedLeave)
                        {
                            //if StartDate and end Date year not same split in two 
                            //List<Tuple<DateTime, DateTime>> dtSeperatedByYear = new List<Tuple<DateTime, DateTime>>(SeperateStartEndDateIfYearNotSame(new Tuple<DateTime, DateTime>(StartDate.Value, EndDate.Value)));
                            //if (dtSeperatedByYear != null && dtSeperatedByYear.Count > 0)
                            //{
                            //    foreach (var item in dtSeperatedByYear)
                            //    {
                            //        EmployeeLeave TempEmployeeLeave = new EmployeeLeave()
                            //        {
                            //            //Employee = EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))],
                            //            Employee = EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(SelectedEmployeeList[j])),
                            //            //[001] Code Comment and Get Company Leave using Lookup value
                            //            //CompanyLeave = CompanyLeavesList[SelectedLeaveType],
                            //            CompanyLeave = GetCompanyLeave(GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType]),
                            //            IdEmployee = Convert.ToInt16(SelectedEmployeeList[j]),
                            //            StartDate = item.Item1,
                            //            EndDate = item.Item2,
                            //            //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                            //            IdLeave = GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].IdLookupValue,
                            //            Remark = Remarks,
                            //            FileName = LeaveFileName,
                            //            IsAllDayEvent = Convert.ToSByte(IsAllDayEvent),

                            //            EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                            //            /// [0001][04/08/2020][sjadhav][SPRINT 84][GEOS2-2437][GHRM - Employee Leaves export report [#ERF59]]
                            //            IdCompanyShift = CompanyShiftIdArray[j],
                            //            CompanyShift = CompanyShiftArray[j],
                            //            //[GEOS2-3095]
                            //            Creator = GeosApplication.Instance.ActiveUser.IdUser,
                            //            CreationDate = GeosApplication.Instance.ServerDateTime


                            //        };
                            //        //[002]added
                            //        string[] IdEmployeeCompany = TempEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                            //        CompanyList = new List<Company>(SelectedPlantList.Where(x => x.IdCompany == Convert.ToInt32(IdEmployeeCompany[0])).ToList());
                            //        TempEmployeeLeave.CompanyLeave.Company = CompanyList.FirstOrDefault();
                            //        NewEmployeeLeaveList.Add(TempEmployeeLeave);

                            //    }
                            //}
                            // else
                            {
                            int j = 0;
                            //shubham[skadam] GEOS2-3598 HRM Bug - GEOS2-3081 not working properly 10 OCT 2022
                            var itemStartDate = itemTask.StartDate.Value.Date.AddHours(itemTask.StartTime.Value.TimeOfDay.Hours).AddMinutes(itemTask.StartTime.Value.TimeOfDay.Minutes);
                            var itemEndDate = itemTask.EndDate.Value.Date.AddHours(itemTask.EndTime.Value.TimeOfDay.Hours).AddMinutes(itemTask.EndTime.Value.TimeOfDay.Minutes);
                            var itemSTime = itemTask.StartTime.Value.TimeOfDay;
                            var itemETime = itemTask.EndDate.Value.TimeOfDay;

                            EmployeeLeaveChangeLogList = new ObservableCollection<EmployeeChangelog>();
                            EmployeeLeaveChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = itemTask.EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLeaveAddChangeLog").ToString(),
                                EmployeeLeaveList != null ? EmployeeLeaveList[itemTask.SelectedLeaveType].Value : null,
                                StartDate.Value.ToShortDateString(), EndDate.Value.ToShortDateString())
                            });

                            EmployeeLeave TempEmployeeLeave = new EmployeeLeave()
                            {
                                //Employee = EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))],
                                Employee = itemTask.EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt32(itemTask.SelectedEmployeeList[j])),
                                //[001] Code Comment and Get Company Leave using Lookup value
                                //CompanyLeave = CompanyLeavesList[itemTask.SelectedLeaveType],
                                CompanyLeave = EmployeeLeaveList != null ? GetCompanyLeave(EmployeeLeaveList[itemTask.SelectedLeaveType]) : null,
                                IdEmployee = Convert.ToInt16(itemTask.SelectedEmployeeList[j]),
                                //shubham[skadam] GEOS2-3598 HRM Bug - GEOS2-3081 not working properly 10 OCT 2022
                                StartDate = itemStartDate,
                                EndDate = itemEndDate,
                                //IdLeave = Convert.ToInt16(CompanyLeavesList[SelectedLeaveType].IdCompanyLeave),
                                IdLeave = EmployeeLeaveList != null ? EmployeeLeaveList[itemTask.SelectedLeaveType].IdLookupValue : 0,
                                Remark = itemTask.Remarks,
                                FileName = itemTask.LeaveFileName,
                                IsAllDayEvent = Convert.ToSByte(itemTask.IsAllDayEvent),
                                EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeLeaveChangeLogList),
                                IdCompanyShift = itemTask.CompanyShiftIdArray[j],
                                CompanyShift = itemTask.CompanyShiftArray[j],
                                //[GEOS2-3095]
                                Creator = GeosApplication.Instance.ActiveUser.IdUser,
                                CreationDate = GeosApplication.Instance.ServerDateTime
                            };
                                //[002]added
                                string[] IdEmployeeCompany = TempEmployeeLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                                CompanyList = new List<Company>(SelectedPlantList.Where(x => x.IdCompany == Convert.ToInt32(IdEmployeeCompany[0])).ToList());
                            
                            IsBusy = true;
                            //NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2045(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                            List<EmployeeLeave> TempEmployeeLeaveList = new List<EmployeeLeave>();
                            TempEmployeeLeaveList.Add(TempEmployeeLeave);

                            // AddEmployeeLeaveChangeLogDetails();
                            //AddEmployeeLeavesFromList_V2170 Service Updated with AddEmployeeLeavesFromList_V2340 by [rdixit][GEOS2-3263][28.11.2022]
                            //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
                            NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2410(TempEmployeeLeaveList, itemTask.LeaveFileInBytes, itemTask.SelectedPeriod);
                            IsSave = true;

                           // NewEmployeeLeaveList.Add(TempEmployeeLeave); //[pallavi.kale] [GEOS2-6830][07-03-2025]
                            }
                        }
                        //else if (IsLeave == false || IsAttendance == false)
                        //{
                        //    //EmployeeOverlappedLeavesWarning = EmployeeOverlappedLeavesWarning + EmployeeListFinal[EmployeeListFinal.FindIndex(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j]))].FullName + "\n";
                        //    if (IsAuthorizedLeave)
                        //    {
                        //        EmployeeOverlappedLeavesWarning = EmployeeOverlappedLeavesWarning + EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == Convert.ToInt16(SelectedEmployeeList[j])).FullName + "\n";
                        //        IsEmployeeOverlappedLeavesWarning = true;
                        //    }

                        //}
                    }


                    //if (IsNew == true)
                    //{
                    //    IsBusy = false;
                    //    if (NewEmployeeLeaveList.Count < SelectedEmployeeList.Count)
                    //    {
                    //        if (NewEmployeeLeaveList.Count >= 1)
                    //        {
                    //            if (IsEmployeeOverlappedLeavesWarning && IsEmployeeAuthorizedLeveError)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].Value, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    //            }
                    //            else if (IsEmployeeAuthorizedLeveError)
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].Value, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    //            else if (IsEmployeeOverlappedLeavesWarning)
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                    //            else if (ErrorMessageList != null && ErrorMessageList.Count > 0)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (IsEmployeeOverlappedLeavesWarning && IsEmployeeAuthorizedLeveError)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].Value, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                return;
                    //            }

                    //            if (IsEmployeeAuthorizedLeveError)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveAuthorizedLeaveNotRegistered").ToString(), GeosApplication.Instance.EmployeeLeaveList[SelectedLeaveType].Value, EmployeeAuthorizedLeveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                return;
                    //            }

                    //            if (IsEmployeeOverlappedLeavesWarning)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddMultipleEmployeeLeaveOverlapped").ToString(), EmployeeOverlappedLeavesWarning), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                return;
                    //            }
                    //            else if (ErrorMessageList != null && ErrorMessageList.Count > 0)
                    //            {
                    //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForInactiveEmployeeLeave").ToString(), EmployeeInactiveLeaveError), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //}
                //}

                        //IsBusy = true;
                        ////NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2045(NewEmployeeLeaveList, LeaveFileInBytes, SelectedPeriod);
                        //NewEmployeeLeaveList = HrmService.AddEmployeeLeavesFromList_V2170(NewEmployeeLeaveList, itemTask.LeaveFileInBytes, itemTask.SelectedPeriod);
                        //IsSave = true;

                        string EmployeeExceedAnnualLeavesWarning = string.Empty;
                        foreach (var newEmpLeave in NewEmployeeLeaveList)
                        {
                            // var employeeJobDescription = newEmpLeave.EmployeeJobDescription.LastOrDefault();

                            //[002] added
                            string[] IdCompanys = newEmpLeave.Employee.EmployeeCompanyIds.ToString().Split(',');
                            //[003] service method changed IsEmployeeEnjoyedAllAnnualLeavesSprint60 to IsEmployeeEnjoyedAllAnnualLeaves_V2038
                            //[005] service method changed IsEmployeeEnjoyedAllAnnualLeaves_V2038 to IsEmployeeEnjoyedAllAnnualLeaves_V2050
                            //  bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2050(newEmpLeave.IdEmployee, (int)(newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil
                            bool IsEnjoyedAllAnnualLeaves = HrmService.IsEmployeeEnjoyedAllAnnualLeaves_V2140(newEmpLeave.IdEmployee, (int)(newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil

                            if (IsEnjoyedAllAnnualLeaves)
                            {
                                EmployeeAnnualLeave TempEmployeeAnnualLeave = new EmployeeAnnualLeave();
                                if (newEmpLeave.Employee.CompanyShift != null)
                                {
                                    //[003] service method changed GetEmployeeEnjoyedLeaveHours to GetEmployeeEnjoyedLeaveHours_V2038
                                    //[005] service method changed GetEmployeeEnjoyedLeaveHours_V2038 to GetEmployeeEnjoyedLeaveHours_V2050
                                    //[3009] service method changed GetEmployeeEnjoyedLeaveHours_V2050 to GetEmployeeEnjoyedLeaveHours_V2140

                                    TempEmployeeAnnualLeave = HrmService.GetEmployeeEnjoyedLeaveHours_V2140(newEmpLeave.IdEmployee, (newEmpLeave.IdLeave), SelectedPeriod, Convert.ToInt32(IdCompanys[0]));//swapnil
                                    decimal RemainingHours = TempEmployeeAnnualLeave.Remaining;

                                    if (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount == 0)
                                    {
                                        continue;
                                    }

                                    int Days = (int)(RemainingHours / (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount));
                                    decimal Hours = RemainingHours % (newEmpLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
                                    string ExceededDaysAnsHours = string.Empty;

                                    if (Days == 0)
                                    {
                                        ExceededDaysAnsHours = Hours + "h";
                                    }
                                    else
                                    {
                                        ExceededDaysAnsHours = Days + "d" + " and " + Hours + "H";
                                    }

                                    EmployeeExceedAnnualLeavesWarning = EmployeeExceedAnnualLeavesWarning + "   " + newEmpLeave.Employee.FullName + " - " + ExceededDaysAnsHours;
                                }
                            }
                        }
                        IsBusy = false;

                        if (!string.IsNullOrEmpty(EmployeeExceedAnnualLeavesWarning))
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeExceedAnnualLeavesWarning").ToString(), EmployeeLeaveList !=null? EmployeeLeaveList[SelectedLeaveType].Value:null, EmployeeExceedAnnualLeavesWarning), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                //    }
                //}

                GeosApplication.Instance.Logger.Log("Method SaveAttendanceWithSplit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveAttendanceWithSplit() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveAttendanceWithSplit() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveAttendanceWithSplit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;
        }

        private void CloseAttemdanceWindowForSplit(object obj)
        {
            try
            {
                 GeosApplication.Instance.Logger.Log("Method CloseAttemdanceWindowForSplit()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null,null);
               // SelectedViewIndex = 0;
               //// IsSplitVisible = true;
               // foreach (AddNewLeaveViewModelWithSplittingTask task in Tasks.ToList())
               // {
               //     Tasks.Remove(task);
               // }
                GeosApplication.Instance.Logger.Log("Method CloseAttemdanceWindowForSplit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseAttemdanceWindowForSplit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }

        //private void AddNewSplitTaskCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddNewSplitTaskCommandAction()...", category: Category.Info, priority: Priority.Low);

        //        int count = Tasks.Count + 1;

        //        Tasks.Add(new AddNewLeaveViewModelWithSplittingTask()
        //        {
        //            Name = "Header",
        //            Header = "Attendance" + count.ToString(),
        //            IsComplete = true,
        //            IsNew = true,
        //            EmployeeListFinal = EmployeeListFinal,
        //            SelectedIndexForEmployee = SelectedIndexForEmployee,
        //            // CompanyShifts = CompanyShifts,
        //            EmployeeShiftList = EmployeeShiftList,
        //            // TaskSelectedCompanyShift = SelectedCompanyShift,
        //            TaskSelectedEmployeeShift = SelectedEmployeeShift,
        //            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
        //            IsEnabledShift = false,
        //            StartDate = StartDate,
        //            StartTime = StartTime,
        //            //STime = STime,
        //            EndDate = EndDate,
        //            EndTime = EndTime // ,
        //                                 //ETime = ETime,
        //                                 //  SelectedIndexForAttendanceType = SelectedIndexForAttendanceType
        //        });
        //        GeosApplication.Instance.Logger.Log("Method AddNewSplitTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method AddNewSplitTaskCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //    }


        //}

        public bool CanCloseTask(AddNewLeaveViewModelWithSplittingTask task)
        {
            //if (task != null)
            //{
            //    return task.IsComplete;
            //}
            if (task.Header != "Leave1")
            {
                return true;
            }
            return false;
        }

        public void CloseTask(AddNewLeaveViewModelWithSplittingTask task)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseTask ...", category: Category.Info, priority: Priority.Low);
               // RequestClose(null, null);

                //if (task.IsComplete == true)
                if(task.Header!="Leave1")
                {
                    Tasks.Remove(task);
                }

                GeosApplication.Instance.Logger.Log("Method CloseTask() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseTask()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectionChangedCommandAction(object obj)
        {
            //throw new NotImplementedException();
        }
        public CompanyWork GetCompanyWork(LookupValue obj)
        {
            //CompanyWork companyWork = new CompanyWork
            //{
            //    IdCompanyWork = obj.IdLookupValue,
            //    Name = obj.Value,
            //    HtmlColor = obj.HtmlColor
            //};

            //return companyWork;

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

        //private DateTime GetShiftStartTime(int dayOfWeek, EmployeeShift selectedEmployeeShift, bool isStartTime)
        //{
        //    //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
        //    //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);

        //    try
        //    {

        //        GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()...", category: Category.Info, priority: Priority.Low);

        //        switch (dayOfWeek)
        //        {
        //            case 0:
        //                TimeSpan SunStartTime = SelectedEmployeeShift.CompanyShift.SunStartTime;
        //                TimeSpan SunEndTime = SelectedEmployeeShift.CompanyShift.SunEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);


        //            case 1:
        //                TimeSpan MonStartTime = SelectedEmployeeShift.CompanyShift.MonStartTime;
        //                TimeSpan MonEndTime = SelectedEmployeeShift.CompanyShift.MonEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(MonStartTime.Hours).AddMinutes(MonStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(MonEndTime.Hours).AddMinutes(MonEndTime.Minutes);

        //            case 2:
        //                TimeSpan TueStartTime = SelectedEmployeeShift.CompanyShift.TueStartTime;
        //                TimeSpan TueEndTime = SelectedEmployeeShift.CompanyShift.TueEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(TueStartTime.Hours).AddMinutes(TueStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(TueEndTime.Hours).AddMinutes(TueEndTime.Minutes);

        //            case 3:
        //                TimeSpan WedStartTime = SelectedEmployeeShift.CompanyShift.WedStartTime;
        //                TimeSpan WedEndTime = SelectedEmployeeShift.CompanyShift.WedEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(WedStartTime.Hours).AddMinutes(WedStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(WedEndTime.Hours).AddMinutes(WedEndTime.Minutes);

        //            case 4:
        //                TimeSpan ThuStartTime = SelectedEmployeeShift.CompanyShift.ThuStartTime;
        //                TimeSpan ThuEndTime = SelectedEmployeeShift.CompanyShift.ThuEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(ThuStartTime.Hours).AddMinutes(ThuStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(ThuEndTime.Hours).AddMinutes(ThuEndTime.Minutes);

        //            case 5:
        //                TimeSpan FriStartTime = SelectedEmployeeShift.CompanyShift.FriStartTime;
        //                TimeSpan FriEndTime = SelectedEmployeeShift.CompanyShift.FriEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(FriStartTime.Hours).AddMinutes(FriStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(FriEndTime.Hours).AddMinutes(FriEndTime.Minutes);

        //            case 6:
        //                TimeSpan SatStartTime = SelectedEmployeeShift.CompanyShift.SatStartTime;
        //                TimeSpan SatEndTime = SelectedEmployeeShift.CompanyShift.SatEndTime;
        //                if (isStartTime)
        //                    return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
        //                else
        //                    return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);

        //            default:
        //                return (DateTime)StartTime;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method GetShiftStartTime()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GetShiftStartTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        return new DateTime();
        //    }
        //}
        #endregion

    }

}
