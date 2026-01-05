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
using System.IO;
using System.Windows.Media.Imaging;
using NodaTime;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Management;
using System.Windows.Media;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class ContractSituationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isNew;
        private Visibility isHeader = Visibility.Visible;
        private DateTime? maxStartDate;
        private DateTime? minStartDate;
        private DateTime? maxEndDate;
        private DateTime? minEndDate;
        private string employeeCode;
        private string timeEditMask;
        private List<object> attachmentList;
        private byte[] contractSituationFileInBytes;
        private string contractSituationFileName;
        public bool flag;
        public bool flagEdit;
        public bool flagEditDate;
        private string remarks;
        private int selectedIndexContractSituation;
        private int selectedIndexCompany;
        public int selectedIndexProfessionalCategory;
        private ObservableCollection<String> employeeContractSituationFileList;
        private ObservableCollection<EmployeeContractSituation> existEmployeeContractSituation;
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
        //private List<EmployeeContractSituation> GetAllUpdatedEmployeeContracts;
        //private string lengthOfService;

        private int selectedViewIndex;
        public string tempfilepath;
        bool isMergeVisible = false;
        bool isMerge = false;
        private int selectedEmpolyeeStatus;
        #endregion

        #region Properties
        //public EmployeeContractSituation CurrentContractSituation { get; set; }
        public bool IsBusy { get; set; }
        //public string WorkingPlantId { get; set; }
        //public long IdEmployeeAttendance { get; private set; }
        public List<Company> CompanyList { get; set; }
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCode"));
            }
        }


        public ObservableCollection<String> EmployeeContractSituationFileList
        {
            get { return employeeContractSituationFileList; }
            set
            {
                employeeContractSituationFileList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituationFileList"));
            }
        }

        public Visibility IsHeader
        {
            get
            {
                return isHeader;
            }

            set
            {
                isHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHeader"));
            }
        }
        public byte[] ContractSituationFileInBytes
        {
            get
            {
                return contractSituationFileInBytes;
            }

            set
            {
                contractSituationFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileInBytes"));
            }
        }
        //public List<Company> SelectedPlantList { get; set; }
        public DateTime? MinStartDate
        {
            get
            {
                return minStartDate;
            }

            set
            {
                minStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinStartDate"));
            }
        }
        public DateTime? MaxStartDate
        {
            get
            {
                return maxStartDate;
            }

            set
            {
                maxStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxStartDate"));
            }
        }
        public DateTime? MaxEndDate
        {
            get
            {
                return maxEndDate;
            }

            set
            {
                maxEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxEndDate"));
            }
        }
        public DateTime? MinEndDate
        {
            get
            {
                return minEndDate;
            }

            set
            {
                minEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinEndDate"));
            }
        }
        public EmployeeContractSituation EditEmployeeContractSituation { get; set; }
        public int SelectedIndexProfessionalCategory
        {
            get
            {
                return selectedIndexProfessionalCategory;
            }

            set
            {
                selectedIndexProfessionalCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexProfessionalCategory"));
            }
        }
        public List<ContractSituation> ContractSituationList { get; set; }
        public List<ProfessionalCategory> ProfessionalCategoryList { get; set; }
        public int SelectedIndexCompany
        {
            get
            {
                return selectedIndexCompany;
            }

            set
            {
                selectedIndexCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompany"));
            }
        }
        public int SelectedIndexContractSituation
        {
            get
            {
                return selectedIndexContractSituation;
            }

            set
            {
                selectedIndexContractSituation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexContractSituation"));
            }
        }
        public ObservableCollection<EmployeeContractSituation> ExistEmployeeContractSituation
        {
            get
            {
                return existEmployeeContractSituation;
            }

            set
            {
                existEmployeeContractSituation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContractSituation"));
            }
        }
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }

        public string ContractSituationFileName
        {
            get
            {
                return contractSituationFileName;
            }

            set
            {
                contractSituationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileName"));
            }
        }
        public List<object> AttachmentList
        {
            get
            {
                return attachmentList;
            }

            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        //public Company Company
        //{
        //    get { return company; }
        //    set
        //    {
        //        company = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Company"));
        //    }
        //}

        //public ObservableCollection<EmployeeAttendance> NewEmployeeAttendanceList
        //{
        //    get { return newEmployeeAttendanceList; }
        //    set
        //    {
        //        newEmployeeAttendanceList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("NewEmployeeAttendanceList"));
        //    }
        //}

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

        //public int SelectedIndexForAttendanceType
        //{
        //    get { return selectedIndexForAttendanceType; }
        //    set
        //    {
        //        selectedIndexForAttendanceType = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
        //    }
        //}

        //public long SelectedPeriod
        //{
        //    get { return selectedPeriod; }
        //    set
        //    {
        //        selectedPeriod = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriod"));
        //    }
        //}

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

        //public ObservableCollection<EmployeeAttendance> ExistEmployeeAttendanceList
        //{
        //    get { return existEmployeeAttendanceList; }
        //    set
        //    {
        //        existEmployeeAttendanceList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeAttendanceList"));
        //    }
        //}

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

        //public TimeSpan STime
        //{
        //    get { return sTime; }
        //    set
        //    {
        //        sTime = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("STime"));
        //    }
        //}

        //public TimeSpan ETime
        //{
        //    get { return eTime; }
        //    set
        //    {
        //        eTime = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ETime"));
        //    }
        //}

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        //public bool Result
        //{
        //    get { return result; }
        //    set
        //    {
        //        result = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Result"));
        //    }
        //}

        //public ObservableCollection<EmployeeLeave> EmployeeLeaves
        //{
        //    get { return employeeLeaves; }
        //    set
        //    {
        //        employeeLeaves = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLeaves"));
        //    }
        //}

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

        //public CompanyShift CompanyShiftDetails
        //{
        //    get { return companyShifDetails; }
        //    set
        //    {
        //        companyShifDetails = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifDetails"));
        //    }
        //}

        //private bool _addNewTaskbtnVisibility;
        //public bool AddNewTaskbtnVisibility
        //{
        //    get { return _addNewTaskbtnVisibility; }
        //    set { _addNewTaskbtnVisibility = value; }
        //}
        //public DateTime? AccountingDate
        //{
        //    get
        //    {
        //        return accountingDate;
        //    }

        //    set
        //    {
        //        accountingDate = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AccountingDate"));
        //    }
        //}
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

        //public bool IsAcceptButton
        //{
        //    get { return isAcceptButton; }
        //    set
        //    {
        //        isAcceptButton = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButton"));
        //    }
        //}

        private EmployeeContractSituation selectedContractSituationRow;
        public EmployeeContractSituation SelectedContractSituationRow
        {
            get { return selectedContractSituationRow; }
            set
            {
                selectedContractSituationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContractSituationRow"));
            }
        }
        private ObservableCollection<EmployeeContractSituation> updatedEmployeeContractSituationList;
        public ObservableCollection<EmployeeContractSituation> UpdatedEmployeeContractSituationList
        {
            get { return updatedEmployeeContractSituationList; }
            set
            {
                updatedEmployeeContractSituationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeContractSituationList"));
            }
        }

        //public string LengthOfService
        //{
        //    get { return lengthOfService; }
        //    set
        //    {
        //        lengthOfService = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LengthOfService"));
        //    }
        //}

        private ObservableCollection<EmployeeContractSituation> employeeContractSituationList;
        public ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList
        {
            get { return employeeContractSituationList; }
            set
            {
                employeeContractSituationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituationList"));
            }
        }

        private ObservableCollection<EmployeeContractSituation> selectedRowEmployeeContractSituationList;
        public ObservableCollection<EmployeeContractSituation> SelectedRowEmployeeContractSituationList
        {
            get { return selectedRowEmployeeContractSituationList; }
            set
            {
                selectedRowEmployeeContractSituationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRowEmployeeContractSituationList"));
            }
        }

        //private string contractSituationError;
        //public string ContractSituationError
        //{
        //    get { return contractSituationError; }
        //    set
        //    {
        //        contractSituationError = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationError"));
        //    }
        //}

        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }

        public bool IsMergeVisible
        {
            get { return isMergeVisible; }
            set
            {
                isMergeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMergeVisible"));
            }
        }

        private bool isAddVisible = false;
        public bool IsAddVisible
        {
            get { return isAddVisible; }
            set
            {
                isAddVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddVisible"));
            }
        }

        public int SelectedEmpolyeeStatus
        {
            get { return selectedEmpolyeeStatus; }
            set
            {
                selectedEmpolyeeStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmpolyeeStatus"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand AddContractSituationViewCancelButtonCommand { get; set; }
        public EmployeeContractSituation NewEmployeeContractSituation { get; set; }
        public ICommand EmployeeDocumentViewCommand { get; set; }
        //public ICommand OnDateFocusCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        //public ICommand OnTextEditValueChangingCommand { get; set; }
        //public ICommand SelectedIndexChangedCommand { get; set; }
        //public ICommand SelectedIndexChangedForCompanyShiftCommand { get; set; }
        public ICommand DeleteContractSituationInformationCommand { get; set; }
        public ICommand MergeButtonCommand { get; set; }
        //public ICommand ContractMergeButtonCommand { get; set; }
        public ICommand MergeAcceptButtonCommand { get; set; }
        public ICommand MergeCancelButtonCommand { get; set; }
        //public ICommand AddNewTaskCommand { get; set; }
        //public ICommand CloseTaskCommand { get; set; }
        //public ICommand SelectionChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand AddContractSituationViewAcceptButtonCommand { get; set; }

        public ICommand OnDragRecordOverCommand { get; set; }
        public ICommand OnDropRecordCommand { get; set; }

        #endregion

        #region Constructor
        public ContractSituationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ContractSituationViewModel()...", category: Category.Info, priority: Priority.Low);
                SetUserPermission();
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                AddContractSituationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                DeleteContractSituationInformationCommand = new RelayCommand(new Action<object>(DeleteContractSituationInformationRecord));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                //OnDateFocusCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateFocusAction);
                //OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
                //SelectedIndexChangedCommand = new DelegateCommand<RoutedEventArgs>(SelectedIndexChangedCommandAction);
                EmployeeDocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeEducationDocument));
                AddContractSituationViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddContractSituationInformation));
                //Split commands.
                MergeButtonCommand = new RelayCommand(new Action<object>(MergeFiles));
                //ContractMergeButtonCommand = new RelayCommand(new Action<object>(MergeContractFiles));
                MergeAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMergeFiles));
                MergeCancelButtonCommand = new DelegateCommand<object>(CloseMergeWindow);
                //AddNewTaskCommand = new RelayCommand(new Action<object>(AddNewSplitTaskCommandAction));

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                OnDragRecordOverCommand = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverCommandAction);

                OnDropRecordCommand = new DelegateCommand<DropRecordEventArgs>(OnDropRecordCommandAction);

                FillEmployeeWorkType();
                //IsMergeImageVisibility = Visibility.Visible;
                //IsAddImageVisibility = Visibility.Collapsed;
                IsAddVisible = false;

                GeosApplication.Instance.Logger.Log("Constructor ContractSituationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ContractSituationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void OpenEmployeeEducationDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                EmployeeDocumentView employeeEducationDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel employeeEducationDocumentViewModel = new EmployeeDocumentViewModel();
                employeeEducationDocumentViewModel.OpenPdfByEmployeeCode(EmployeeCode, obj);
                employeeEducationDocumentView.DataContext = employeeEducationDocumentViewModel;
                employeeEducationDocumentView.Show();
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddContractSituationInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddContractSituationInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexContractSituation"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexProfessionalCategory"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompany"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));

                if (error != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();
                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    ContractSituationFileName = null;
                    ContractSituationFileInBytes = null;
                }


                EmployeeContractSituation TempNewContractSituationForFile = new EmployeeContractSituation();
                var ExistFilesRecord = ExistEmployeeContractSituation.Where(x => x.ContractSituationFileName != null);
                TempNewContractSituationForFile = ExistFilesRecord.FirstOrDefault(x => x.ContractSituationFileName == ContractSituationFileName);

                if (TempNewContractSituationForFile == null)
                {
                    if (IsNew == true)
                    {
                        NewEmployeeContractSituation = new EmployeeContractSituation()
                        {

                            ContractSituation = ContractSituationList[SelectedIndexContractSituation],
                            IdContractSituation = ContractSituationList[SelectedIndexContractSituation].IdContractSituation,
                            IdCompany = CompanyList[selectedIndexCompany].IdCompany,
                            Company = new Company() { IdCompany = CompanyList[selectedIndexCompany].IdCompany, Alias = CompanyList[selectedIndexCompany].Alias },
                            ProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory],
                            IdProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory].IdProfessionalCategory,
                            ContractSituationStartDate = StartDate,
                            ContractSituationEndDate = EndDate,
                            ContractSituationFileName = ContractSituationFileName,
                            ContractSituationFileInBytes = ContractSituationFileInBytes,
                            ContractSituationRemarks = Remarks,

                            TransactionOperation = ModelBase.TransactionOperations.Add
                        };

                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditEmployeeContractSituation = new EmployeeContractSituation()
                        {
                            ContractSituation = ContractSituationList[SelectedIndexContractSituation],
                            IdContractSituation = ContractSituationList[SelectedIndexContractSituation].IdContractSituation,
                            IdCompany = CompanyList[selectedIndexCompany].IdCompany,
                            Company = new Company() { IdCompany = CompanyList[selectedIndexCompany].IdCompany, Alias = CompanyList[selectedIndexCompany].Alias },
                            ProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory],
                            IdProfessionalCategory = ProfessionalCategoryList[SelectedIndexProfessionalCategory].IdProfessionalCategory,
                            ContractSituationStartDate = StartDate,
                            ContractSituationEndDate = EndDate,
                            ContractSituationFileName = ContractSituationFileName,
                            ContractSituationFileInBytes = ContractSituationFileInBytes,
                            ContractSituationRemarks = Remarks,
                            TransactionOperation = ModelBase.TransactionOperations.Update
                        };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContractSituationInformationFileExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddContractSituationInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddContractSituationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillContractSituationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContractSituationList()...", category: Category.Info, priority: Priority.Low);
                IList<ContractSituation> tempCountryList = HrmService.GetAllContractSituations();
                ContractSituationList = new List<ContractSituation>();
                ContractSituationList.Insert(0, new ContractSituation() { Name = "---" });
                ContractSituationList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillContractSituationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContractSituationList() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContractSituationList() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillContractSituationList()....executed successfully" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        private void FillProfessionalCategoryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProfessionalCategoryList()...", category: Category.Info, priority: Priority.Low);
                //[GEOS2-3846][rdixt][31.10.2022]
                //IList<ProfessionalCategory> tempCountryList = HrmService.GetAllProfessionalCategories();
                IList<ProfessionalCategory> tempCountryList = HrmService.GetAllProfessionalCategoriesWithParents();
                ProfessionalCategoryList = new List<ProfessionalCategory>();
                ProfessionalCategoryList.Insert(0, new ProfessionalCategory() { Name = "---" });
                ProfessionalCategoryList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillProfessionalCategoryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalCategoryList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalCategoryList() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillProfessionalCategoryList()....executed successfully" + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        private void FillCompanyList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyList()...", category: Category.Info, priority: Priority.Low);

                IList<Company> tempList = HrmCommon.Instance.IsCompanyList;
                CompanyList = new List<Company>();
                CompanyList.Insert(0, new Company() { Alias = "---" });
                CompanyList.AddRange(tempList);

                GeosApplication.Instance.Logger.Log("Method FillCompanyList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
      
        public void EditInit(ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList, EmployeeContractSituation empContractSituation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                SelectedContractSituationRow = empContractSituation;
                ExistEmployeeContractSituation = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList.ToList());
                ExistEmployeeContractSituation.Remove(empContractSituation);
                //WindowHeader = Application.Current.FindResource("EditContractSituationInformation").ToString();
                //IsHeader = true;
                FillContractSituationList();
                FillProfessionalCategoryList();
                FillCompanyList();
                SelectedIndexCompany = CompanyList.FindIndex(x => x.IdCompany == empContractSituation.IdCompany);
                if (SelectedIndexCompany == -1)
                    SelectedIndexCompany = 0;

                SelectedIndexContractSituation = ContractSituationList.FindIndex(x => x.IdContractSituation == empContractSituation.IdContractSituation);
                SelectedIndexProfessionalCategory = ProfessionalCategoryList.FindIndex(x => x.IdProfessionalCategory == empContractSituation.IdProfessionalCategory);
                StartDate = empContractSituation.ContractSituationStartDate;
                EndDate = empContractSituation.ContractSituationEndDate;
                Remarks = empContractSituation.ContractSituationRemarks;
                ContractSituationFileName = empContractSituation.ContractSituationFileName;
                ContractSituationFileInBytes = empContractSituation.ContractSituationFileInBytes;
                //EmployeeCode = empContractSituation.IdEmployee.ToString();
                EmployeeContractSituationFileList = new ObservableCollection<string>();
                EmployeeContractSituationFileList.Add(ContractSituationFileName);
                int index = EmployeeContractSituationList.IndexOf(EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationStartDate == empContractSituation.ContractSituationStartDate && x.ContractSituationEndDate == empContractSituation.ContractSituationEndDate));
                if (index == 0)
                {
                    if (EmployeeContractSituationList.Count == 1)
                    {
                        MaxStartDate = null;
                        MinStartDate = null;
                        MaxEndDate = null;
                        MinEndDate = null;
                    }
                    else
                    {
                        MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                        MinStartDate = null;
                        MaxEndDate = MaxStartDate;
                    }
                }
                if (EmployeeContractSituationList.Count > 1)
                {
                    if (index + 1 == EmployeeContractSituationList.Count)
                    {
                        MaxStartDate = null;
                        MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                        MinStartDate = MinStartDate.Value.AddDays(1);
                    }
                }
                if (index > 0 && index < EmployeeContractSituationList.Count - 1)
                {
                    MinStartDate = EmployeeContractSituationList[index - 1].ContractSituationEndDate;
                    MaxStartDate = EmployeeContractSituationList[index + 1].ContractSituationStartDate;
                    MinEndDate = MinStartDate;
                    MaxEndDate = MaxStartDate;
                    MinStartDate = MinStartDate.Value.AddDays(1);
                }
                AttachmentList = new List<object>();
                if (empContractSituation.Attachment != null)
                {
                    AttachmentList.Add(empContractSituation.Attachment);
                }
                else if (!string.IsNullOrEmpty(ContractSituationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empContractSituation.ContractSituationFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }
                //if (AttachmentList.Count == 0)
                //    MergeVisble = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void AddAttendanceInformation(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()...", category: Category.Info, priority: Priority.Low);
        //        error = EnableValidationAndGetError();

        //        IsAcceptButton = true;

        //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
        //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));
        //        if (error != null)
        //        {
        //            return;
        //        }

        //        StartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
        //        EndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
        //        //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
        //        //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);
        //        STime = StartTime.Value.TimeOfDay;
        //        ETime = EndDate.Value.TimeOfDay;

        //        List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
        //        //ExistEmpAttendanceList.FirstOrDefault().is
        //        if (IsNew)
        //        {
        //            ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
        //            //if(ExistEmpAttendanceList.Count == 0)
        //            //{
        //            //    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.EndDate.Date == StartDate.Value.Date).OrderBy(x => x.EndDate).ToList();
        //            //}
        //        }
        //        else
        //        {
        //            ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Date == StartDate.Value.Date && x.IdEmployeeAttendance != IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();
        //        }

        //        if (ExistEmpAttendanceList.Count() > 0)
        //        {
        //            if (ExistEmpAttendanceList.FirstOrDefault().StartDate.Date == StartDate.Value.Date && ExistEmpAttendanceList.FirstOrDefault().EndDate.Date == EndDate.Value.Date && ExistEmpAttendanceList.FirstOrDefault().IdCompanyShift != EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift)
        //            {
        //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceAlreadyAddedShift").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                return;
        //            }
        //        }

        //        bool IsLeave = true;
        //        bool IsAttendance = true;

        //        for (int i = 0; i < ExistEmpAttendanceList.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                if (StartDate < ExistEmpAttendanceList[i].StartDate && EndDate <= ExistEmpAttendanceList[i].StartDate)
        //                {
        //                    IsAttendance = true;
        //                    break;
        //                }
        //                if (ExistEmpAttendanceList.Count == 1)
        //                {
        //                    if (StartDate >= ExistEmpAttendanceList[i].EndDate && EndDate > ExistEmpAttendanceList[i].EndDate)
        //                    {
        //                        IsAttendance = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (i <= ExistEmpAttendanceList.Count - 1)
        //                {
        //                    if (StartDate >= ExistEmpAttendanceList[i - 1].EndDate && EndDate <= ExistEmpAttendanceList[i].StartDate)
        //                    {
        //                        IsAttendance = true;
        //                        break;
        //                    }
        //                    else if (i == ExistEmpAttendanceList.Count - 1)
        //                    {
        //                        if (StartDate >= ExistEmpAttendanceList[i].EndDate && EndDate > ExistEmpAttendanceList[i].StartDate)
        //                        {
        //                            IsAttendance = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //            IsAttendance = false;
        //        }

        //        if (IsAttendance == false)
        //        {
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //            return;
        //        }

        //        var ExistEmpLeaveList = EmployeeLeaves.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.StartDate.Value.Date == StartDate.Value.Date).OrderBy(x => x.StartDate).ToList();
        //        for (int i = 0; i < ExistEmpLeaveList.Count; i++)
        //        {
        //            if (ExistEmpLeaveList[i].IsAllDayEvent == 1)
        //            {
        //                IsLeave = false;
        //                break;
        //            }

        //            if (i == 0)
        //            {
        //                if (StartDate < ExistEmpLeaveList[i].StartDate && EndDate <= ExistEmpLeaveList[i].StartDate)
        //                {
        //                    IsLeave = true;
        //                    break;
        //                }

        //                if (ExistEmpLeaveList.Count == 1)
        //                {
        //                    if (StartDate >= ExistEmpLeaveList[i].EndDate && EndDate > ExistEmpLeaveList[i].EndDate)
        //                    {
        //                        IsLeave = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (i <= ExistEmpLeaveList.Count - 1)
        //                {
        //                    if (StartDate >= ExistEmpLeaveList[i - 1].EndDate && EndDate <= ExistEmpLeaveList[i].StartDate)
        //                    {
        //                        IsLeave = true;
        //                        break;
        //                    }
        //                    else if (i == ExistEmpLeaveList.Count - 1)
        //                    {
        //                        if (StartDate >= ExistEmpLeaveList[i].EndDate && EndDate > ExistEmpLeaveList[i].EndDate)
        //                        {
        //                            IsLeave = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //            IsLeave = false;
        //        }

        //        if (IsLeave == false)
        //        {
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceOverlapped").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //            return;
        //        }
        //        //[002] added
        //        bool IsAddInActiveEmployeeAttendance = true;
        //        Employee Employee = EmployeeListFinal[SelectedIndexForEmployee];
        //        if (Employee != null)
        //        {
        //            if (Employee.EmployeeContractSituations != null && Employee.EmployeeContractSituations.Count > 0)
        //            {
        //                for (int i = 0; i < Employee.EmployeeContractSituations.Count; i++)
        //                {
        //                    if (Employee.EmployeeContractSituations[i].ContractSituationEndDate == null)
        //                    {

        //                        DateTime? EmployeeContractEndDate = Employee.EmployeeContractSituations[i].ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : Employee.EmployeeContractSituations[i].ContractSituationEndDate;

        //                        if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= EmployeeContractEndDate.Value.Date)
        //                        {
        //                            IsAddInActiveEmployeeAttendance = true;
        //                            break;
        //                        }
        //                        else
        //                            IsAddInActiveEmployeeAttendance = false;

        //                    }
        //                    else if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && EndDate.Value.Date <= Employee.EmployeeContractSituations[i].ContractSituationEndDate.Value.Date)
        //                    {
        //                        IsAddInActiveEmployeeAttendance = true;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        IsAddInActiveEmployeeAttendance = false;
        //                    }
        //                }
        //                if (!IsAddInActiveEmployeeAttendance)
        //                {
        //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessagforInactiveEmployeeAttendance").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessagforInactiveEmployeeAttendance").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                return;
        //            }
        //        }
        //        //end

        //        if (IsNew == true)
        //        {
        //            NewEmployeeAttendance = new EmployeeAttendance()
        //            {
        //                Employee = EmployeeListFinal[SelectedIndexForEmployee],




        //                IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
        //                StartDate = (DateTime)StartDate,
        //                EndDate = (DateTime)EndDate,
        //                //[001] code comment
        //                //CompanyWork = AttendanceTypeList[SelectedIndexForAttendanceType],
        //                IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue,
        //                //IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
        //                IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
        //                CurrentContractForAttendance = EmployeeListFinal[SelectedIndexForEmployee].EmployeeContractSituations.Where(ecs => ecs.ContractSituationStartDate.Value.Date <= (DateTime)StartDate.Value.Date).Select(i => i.Company.Alias).LastOrDefault(),

        //            };

        //            NewEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
        //            DateTime tempEndDate = NewEmployeeAttendance.EndDate;
        //            //if (CompanyShifts[SelectedIndexForCompanyShift].IsNightShift == 0 || ((NewEmployeeAttendance.EndDate.Date- NewEmployeeAttendance.StartDate.Date).TotalDays) <= 0)
        //            if (EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IsNightShift == 0 || ((NewEmployeeAttendance.EndDate.Date - NewEmployeeAttendance.StartDate.Date).TotalDays) <= 0)
        //            {
        //                for (var item = NewEmployeeAttendance.StartDate; item.Date <= tempEndDate.Date; item = NewEmployeeAttendance.StartDate.AddDays(1))
        //                {

        //                    NewEmployeeAttendance.StartDate = item;
        //                    NewEmployeeAttendance.EndDate = NewEmployeeAttendance.StartDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
        //                    // NewEmployeeAttendance.Employee.EmployeeJobDescription.Company = NewEmployeeAttendance.Employee.EmployeeContractSituation.Company;

        //                    //NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
        //                    NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
        //                    NewEmployeeAttendance.AccountingDate = item;
        //                    NewEmployeeAttendance.IsManual = 1;
        //                    IsSave = true;
        //                    //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
        //                    EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
        //                    NewEmployeeAttendanceList.Add(updateEmployeeAttendance);

        //                }
        //            }
        //            else
        //            {

        //                //if(((NewEmployeeAttendance.EndDate.Date - NewEmployeeAttendance.StartDate.Date).TotalDays) > 1)
        //                //{
        //                //    tempEndDate = tempEndDate.AddDays(1);
        //                //}

        //                for (var item = NewEmployeeAttendance.StartDate; item <= tempEndDate; item = NewEmployeeAttendance.StartDate.AddDays(1))
        //                {

        //                    NewEmployeeAttendance.StartDate = item;
        //                    DateTime _endDate = item.AddDays(1);
        //                    NewEmployeeAttendance.EndDate = _endDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);

        //                    // NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
        //                    NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
        //                    NewEmployeeAttendance.AccountingDate = item;
        //                    NewEmployeeAttendance.IsManual = 1;
        //                    IsSave = true;
        //                    //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
        //                    EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
        //                    NewEmployeeAttendanceList.Add(updateEmployeeAttendance);

        //                }
        //            }
        //            CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(NewEmployeeAttendance.IdCompanyShift));
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
        //            RequestClose(null, null);
        //        }
        //        else
        //        {
        //            UpdateEmployeeAttendance = new EmployeeAttendance()
        //            {
        //                Employee = EmployeeListFinal[SelectedIndexForEmployee],
        //                CurrentContractForAttendance = EmployeeListFinal[SelectedIndexForEmployee].EmployeeContractSituations.Where(ecs => ecs.ContractSituationStartDate.Value.Date <= (DateTime)StartDate.Value.Date).Select(i => i.Company.Alias).LastOrDefault(),
        //                IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee,
        //                StartDate = (DateTime)StartDate,
        //                EndDate = (DateTime)EndDate,
        //                //[001] code Comment
        //                //CompanyWork = AttendanceTypeList[SelectedIndexForAttendanceType],
        //                IdEmployeeAttendance = IdEmployeeAttendance,
        //                IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue,
        //                //IdCompanyShift = CompanyShifts[SelectedIndexForCompanyShift].IdCompanyShift,
        //                IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift,
        //                AccountingDate = (DateTime)StartDate
        //            };
        //            // UpdateEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
        //            UpdateEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;

        //            IsSave = true;
        //            Result = HrmService.UpdateEmployeeAttendance_V2036(UpdateEmployeeAttendance);
        //            UpdateEmployeeAttendance.Employee.TotalWorkedHours = (UpdateEmployeeAttendance.EndDate - UpdateEmployeeAttendance.StartDate).ToString(@"hh\:mm");
        //            CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdateEmployeeAttendance.IdCompanyShift));
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
        //            RequestClose(null, null);
        //        }

        //        GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method AddAttendanceInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void OnDateFocusAction(Object obj)
        {
            flagEditDate = true;
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
        private void DeleteContractSituationInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()...", category: Category.Info, priority: Priority.Low);
                //[001]Added
                EmployeeContractSituation empContractSituation = (EmployeeContractSituation)obj;
                if (empContractSituation != null)
                {
                    if (empContractSituation.IdEmployeeExitEvent != null)
                    {
                        if (SelectedContractSituationRow.ContractSituationEndDate < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date)
                        {
                            return;
                        }
                    }
                    if (empContractSituation.ContractSituationFileInBytes == null)
                    {
                        MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["DeleteOriginalContractSituationMessage"].ToString(), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                        return;
                    }
                }
                //end
                
                if ((SelectedRowEmployeeContractSituationList[0].ContractSituationFileInBytes == empContractSituation.ContractSituationFileInBytes) && (SelectedRowEmployeeContractSituationList[0].ContractSituationFileName == empContractSituation.ContractSituationFileName))
                {
                	MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["DeleteOriginalContractSituationMessage"].ToString(), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
                    return;
				}
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteContractSituationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empContractSituation.IdContractSituation != 0)
                    {
                        empContractSituation.IsContractSituationFileDeleted = true;
                        empContractSituation.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        if(updatedEmployeeContractSituationList == null)
                        {
                            UpdatedEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                        }
                        UpdatedEmployeeContractSituationList.Add(empContractSituation);

                    }
                    EmployeeContractSituationList.Remove(empContractSituation);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteContractSituationInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
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
        //public void Init(ObservableCollection<EmployeeAttendance> employeeAttendanceList, object selectedEmployee, DateTime selectedStartDate, DateTime selectedEndDate, ObservableCollection<EmployeeLeave> employeeLeavesList = null)
        //{
        //    try
        //    {

        //        GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

        //        IsEditInit = false;
        //        ExistEmployeeAttendanceList = employeeAttendanceList;

        //        if (employeeLeavesList != null)
        //        {
        //            if (EmployeeLeaves == null)
        //            {
        //                EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
        //                EmployeeLeaves = employeeLeavesList;
        //            }

        //        }

        //        EmployeeListFinal = new ObservableCollection<Employee>();
        //        foreach (var item in SelectedPlantList)
        //        {

        //            var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] Added
        //            EmployeeListFinal.AddRange(tempEmployeeListFinal);
        //        }

        //        EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
        //        EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });

        //        Employee obj = selectedEmployee as Employee;

        //        if (obj != null)
        //            SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == obj.IdEmployee));

        //        if (selectedStartDate != DateTime.MinValue && selectedEndDate != DateTime.MinValue)
        //        {
        //            StartDate = selectedStartDate;
        //            EndDate = selectedEndDate;
        //            //StartTime = FromDates[0];
        //            //EndTime = ToDates[0];
        //            StartTime = Convert.ToDateTime(StartDate.ToString());
        //            EndTime = Convert.ToDateTime(EndDate.ToString());
        //        }

        //        else
        //        {
        //            //StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
        //            //EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
        //            //if (StartTime == DateTime.MinValue && EndTime == DateTime.MinValue)
        //            //{
        //            //    StartTime = FromDates[0];
        //            //    EndTime = ToDates[0];
        //            //    if (StartDate == selectedEndDate.AddDays(-1))
        //            //        EndDate = selectedEndDate.AddDays(-1);
        //            //}
        //            StartTime = Convert.ToDateTime(StartDate.ToString());
        //            EndTime = Convert.ToDateTime(EndDate.ToString());
        //        }
        //        //Company = Company;

        //        //[001] code comment
        //        //AttendanceTypeList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorksByIdCompany(WorkingPlantId));
        //        //AttendanceTypeList.Insert(0, new CompanyWork() { Name = "---", IdCompanyWork = 0 });

        //        GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }

        //}

        /// <summary>
        /// Method to edit Employee Attendance Information
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [avpawar][2020-07-24][GEOS2-2432] To solve the error message when try edit the attendance shift.
        /// </summary>
        /// <param name="SelectedEmployeeAttendance"></param>
        //public void EditInit(EmployeeAttendance SelectedEmployeeAttendance, ObservableCollection<EmployeeAttendance> employeeAttendanceList)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
        //        IsEditInit = true;
        //        EmployeeListFinal = new ObservableCollection<Employee>();
        //        foreach (var item in SelectedPlantList)
        //        {
        //            var tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
        //            EmployeeListFinal.AddRange(tempEmployeeListFinal);
        //        }
        //        EmployeeListFinal = new ObservableCollection<Employee>(EmployeeListFinal.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
        //        EmployeeListFinal.Insert(0, new Employee() { FirstName = "---", IdEmployee = 0 });

        //        //[001] code Comment
        //        //AttendanceTypeList = new ObservableCollection<CompanyWork>(HrmService.GetAllCompanyWorksByIdCompany(WorkingPlantId));
        //        //AttendanceTypeList.Insert(0, new CompanyWork() { Name = "---", IdCompanyWork = 0 });
        //        //SelectedIndexForAttendanceType = AttendanceTypeList.FindIndex(x => x.IdCompanyWork == SelectedEmployeeAttendance.IdCompanyWork);

        //        SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.ToList().FindIndex(x => x.IdLookupValue == SelectedEmployeeAttendance.IdCompanyWork);
        //        SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == SelectedEmployeeAttendance.IdEmployee));

        //        StartDate = SelectedEmployeeAttendance.StartDate;
        //        EndDate = SelectedEmployeeAttendance.EndDate;

        //        //if (FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute) == -1)
        //        //    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour)];
        //        //else
        //        //    StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];

        //        //if (ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute) == -1)
        //        //    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour)];
        //        //else
        //        //    EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];

        //        StartTime = Convert.ToDateTime(SelectedEmployeeAttendance.StartDate.ToString());
        //        EndTime = Convert.ToDateTime(SelectedEmployeeAttendance.EndDate.ToString());

        //        IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
        //        ExistEmployeeAttendanceList = employeeAttendanceList;
        //        // CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
        //        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployeeAttendance.IdEmployee));

        //        //[002] start
        //        if (EmployeeShiftList != null)
        //        {
        //            if (!EmployeeShiftList.Any(a => a.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
        //            {
        //                EmployeeShift ObjShift = new EmployeeShift();
        //                ObjShift.IdCompanyShift = SelectedEmployeeAttendance.IdCompanyShift;
        //                ObjShift.CompanyShift = SelectedEmployeeAttendance.CompanyShift;
        //                EmployeeShiftList.Add(ObjShift);
        //                ObjShift.IsEnabled = false;
        //            }
        //        }
        //        //[002] End

        //        if (EmployeeShiftList.Any(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
        //            SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));

        //        GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>
        //public void InitReadOnly(EmployeeAttendance SelectedEmployeeAttendance)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
        //        IsEditInit = true;
        //        EmployeeListFinal = new ObservableCollection<Employee>();
        //        EmployeeListFinal.Add(SelectedEmployeeAttendance.Employee);

        //        SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.ToList().FindIndex(x => x.IdLookupValue == SelectedEmployeeAttendance.IdCompanyWork);
        //        SelectedIndexForEmployee = EmployeeListFinal.IndexOf(EmployeeListFinal.FirstOrDefault(x => x.IdEmployee == SelectedEmployeeAttendance.IdEmployee));

        //        StartDate = SelectedEmployeeAttendance.StartDate;
        //        EndDate = SelectedEmployeeAttendance.EndDate;
        //        //StartTime = FromDates[FromDates.FindIndex(x => x.Hour == StartDate.Value.Hour && x.Minute == StartDate.Value.Minute)];
        //        //EndTime = ToDates[ToDates.FindIndex(x => x.Hour == EndDate.Value.Hour && x.Minute == EndDate.Value.Minute)];
        //        StartTime = Convert.ToDateTime(StartDate.ToString());
        //        EndTime = Convert.ToDateTime(EndDate.ToString());
        //        IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
        //        //CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
        //        //SelectedIndexForCompanyShift = CompanyShifts.IndexOf(CompanyShifts.FirstOrDefault(x => x.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));

        //        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployeeAttendance.IdEmployee));
        //        if (EmployeeShiftList.Any(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
        //            SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));
        //        GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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

        //private void SelectedIndexChangedCommandAction(RoutedEventArgs obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

        //        if (SelectedIndexForEmployee > 0)
        //        {
        //            if (!IsEditInit)
        //            {
        //                var SelectedEmployee = EmployeeListFinal[SelectedIndexForEmployee];
        //                //CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployee.IdEmployee));
        //                EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee(SelectedEmployee.IdEmployee));
        //                if (EmployeeShiftList.Count == 2)
        //                    SelectedIndexForCompanyShift = 1;

        //            }
        //        }
        //        else
        //        {
        //            //CompanyShifts = new ObservableCollection<CompanyShift>();
        //            //CompanyShift companyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
        //            //CompanyShifts.Insert(0, companyShift);
        //            //SelectedIndexForCompanyShift = 0;
        //            //SelectedCompanyShift = companyShift;

        //            EmployeeShiftList = new ObservableCollection<EmployeeShift>();
        //            EmployeeShift tempEmployeeShift = new EmployeeShift();
        //            tempEmployeeShift.CompanyShift = new CompanyShift() { Name = "---", IdCompanyShift = 0 };
        //            EmployeeShiftList.Insert(0, tempEmployeeShift);
        //            SelectedIndexForCompanyShift = 0;
        //            SelectedEmployeeShift = tempEmployeeShift;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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

        private void MergeFiles(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method MergeFiles ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!isMerge)
                {
                    SelectedViewIndex = 1;
                    IsMergeVisible = false;
                    isMerge = true;
                    IsAddVisible = true;

                    //IsHeader = Visibility.Hidden;
                    WindowHeader = "Merge Files";
                    EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                    //EmployeeContractSituationList.Add(SelectedContractSituationRow);
                    EmployeeContractSituationList.Add(new EmployeeContractSituation()
                    {
                        Attachment = SelectedContractSituationRow.Attachment,
                        Company = SelectedContractSituationRow.Company,
                        ContractExpirationWarningDate = SelectedContractSituationRow.ContractExpirationWarningDate,
                        ContractSituation = SelectedContractSituationRow.ContractSituation,
                        ContractSituationEndDate = SelectedContractSituationRow.ContractSituationEndDate,
                        ContractSituationFileInBytes = SelectedContractSituationRow.ContractSituationFileInBytes,
                        ContractSituationFileName = SelectedContractSituationRow.ContractSituationFileName,
                        ContractSituationRemarks = SelectedContractSituationRow.ContractSituationRemarks,
                        ContractSituationStartDate = SelectedContractSituationRow.ContractSituationStartDate,
                        Employee = SelectedContractSituationRow.Employee,
                        EmployeeExitEvent = SelectedContractSituationRow.EmployeeExitEvent,
                        IdCompany = SelectedContractSituationRow.IdCompany,
                        IdContractSituation = SelectedContractSituationRow.IdContractSituation,
                        IdEmployee = SelectedContractSituationRow.IdEmployee,
                        IdEmployeeContractSituation = SelectedContractSituationRow.IdEmployeeContractSituation,
                        IdEmployeeExitEvent = SelectedContractSituationRow.IdEmployeeExitEvent,
                        IdProfessionalCategory = SelectedContractSituationRow.IdProfessionalCategory,
                        IdSerialNo = SelectedContractSituationRow.IdSerialNo,
                        IsContractSituationFileDeleted = SelectedContractSituationRow.IsContractSituationFileDeleted,
                        OldFileName = SelectedContractSituationRow.OldFileName,
                        ProfessionalCategory = SelectedContractSituationRow.ProfessionalCategory,
                        TransactionOperation = SelectedContractSituationRow.TransactionOperation
                    });
                    if(SelectedRowEmployeeContractSituationList == null)
                    {
                        SelectedRowEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                        SelectedRowEmployeeContractSituationList.Add(EmployeeContractSituationList[0]);
                    }
                    BrowseFilesForMergingCommandAction(obj);
                }
                else
                {
                    BrowseFilesForMergingCommandAction(obj);
                    if (result == true)
                    {
                        EmployeeContractSituationList.Add(new EmployeeContractSituation()
                        {
                            ContractSituationFileName = ContractSituationFileName,
                            ContractSituationFileInBytes = ContractSituationFileInBytes,
                        });
                        if (SelectedRowEmployeeContractSituationList == null)
                        {
                            SelectedRowEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                            SelectedRowEmployeeContractSituationList.Add(EmployeeContractSituationList[0]);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method MergeFiles executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method MergeFiles()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void BrowseFiles(bool multiSelect)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (multiSelect)
                {
                    dlg.Multiselect = true;
                }
                else
                {
                    dlg.Multiselect = false;
                }
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";
                Nullable<bool> result = dlg.ShowDialog();

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

                if (result == true)
                {
                    List<object> newAttachmentList = new List<object>();
                    if (EmployeeContractSituationList == null)
                        EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                    else
                        EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList);

                    foreach (var eachFile in dlg.FileNames)
                    {
                        AttachmentList = new List<object>();
                        ContractSituationFileInBytes = File.ReadAllBytes(eachFile);
                        FileInfo file = new FileInfo(eachFile);
                        ContractSituationFileName = file.Name;

                        Attachment attachment = new Attachment();
                        attachment.FilePath = file.FullName;
                        attachment.OriginalFileName = file.Name;
                        attachment.IsDeleted = false;
                        attachment.FileByte = ContractSituationFileInBytes;
                        newAttachmentList.Add(attachment);

                        EmployeeContractSituationList.Add(new EmployeeContractSituation()
                        {
                            ContractSituationFileInBytes = attachment.FileByte,
                            ContractSituationFileName = attachment.OriginalFileName,
                        });
                    }
                    SelectedRowEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                    foreach (var eachContractList in EmployeeContractSituationList)
                    {
                        if (eachContractList.ContractSituationFileInBytes == selectedContractSituationRow.ContractSituationFileInBytes)
                        {
                            SelectedRowEmployeeContractSituationList.Add(eachContractList);
                        }
                    }
                    AttachmentList = newAttachmentList;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseFiles()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
            BrowseFiles(false);
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void BrowseFilesForMergingCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFilesForMergingCommandAction() ...", category: Category.Info, priority: Priority.Low);
            BrowseFiles(true);
            GeosApplication.Instance.Logger.Log("Method BrowseFilesForMergingCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void SaveMergeFiles(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SaveMergeFiles()....", category: Category.Info, priority: Priority.Low);
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

                tempfilepath = CreateTempFilePath();
                MergeMultiplePDFFiles(EmployeeContractSituationList, tempfilepath);
                ExistEmployeeContractSituation = EmployeeContractSituationList;
                FileInfo file = new FileInfo(tempfilepath);
                long tempFileSize = file.Length >> 10;
                if(tempFileSize > 500000)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["FileSizeExceed"].ToString(), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);

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

                    if (File.Exists(tempfilepath))
                    {
                        File.Delete(tempfilepath);
                    }
                }
                else
                {
                    EditEmployeeContractSituation = new EmployeeContractSituation()
                    {
                        Company = SelectedContractSituationRow.Company,
                        ContractExpirationWarningDate = SelectedContractSituationRow.ContractExpirationWarningDate,
                        ContractSituation = SelectedContractSituationRow.ContractSituation,
                        ContractSituationEndDate = SelectedContractSituationRow.ContractSituationEndDate,
                        ContractSituationFileInBytes = ReadFile(tempfilepath),
                        ContractSituationFileName = file.Name,
                        ContractSituationRemarks = SelectedContractSituationRow.ContractSituationRemarks,
                        ContractSituationStartDate = SelectedContractSituationRow.ContractSituationStartDate,
                        Employee = SelectedContractSituationRow.Employee,
                        EmployeeExitEvent = SelectedContractSituationRow.EmployeeExitEvent,
                        IdCompany = SelectedContractSituationRow.IdCompany,
                        IdContractSituation = SelectedContractSituationRow.IdContractSituation,
                        IdEmployee = SelectedContractSituationRow.IdEmployee,
                        IdEmployeeContractSituation = SelectedContractSituationRow.IdEmployeeContractSituation,
                        IdEmployeeExitEvent = SelectedContractSituationRow.IdEmployeeExitEvent,
                        IdProfessionalCategory = SelectedContractSituationRow.IdProfessionalCategory,
                        IdSerialNo = SelectedContractSituationRow.IdSerialNo,
                        IsContractSituationFileDeleted = SelectedContractSituationRow.IsContractSituationFileDeleted,
                        OldFileName = SelectedContractSituationRow.OldFileName,
                        ProfessionalCategory = SelectedContractSituationRow.ProfessionalCategory,
                        TransactionOperation = ModelBase.TransactionOperations.Update
                    };
                }
                GC.Collect();
                //EditEmployeeContractSituation.ContractSituationFileInBytes = ReadFile(tempfilepath);
                IsSave = true;
                RequestClose(null, null);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SaveMergeFiles()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveMergeFiles() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveMergeFiles() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveMergeFiles()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // get the filebytes
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        // Create a Temp PDF File Path Method
        public string CreateTempFilePath()
        {
            if (EmployeeContractSituationList == null)
            {
                EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                return string.Empty;
            }
            else
            {
                return Path.Combine(Path.GetTempPath(), EmployeeContractSituationList[0].ContractSituationFileName);
            }
        }

        public void MergeMultiplePDFFiles(ObservableCollection<EmployeeContractSituation> fileNamesList, string tempfilepath)
        {
            GeosApplication.Instance.Logger.Log("Method MergeMultiplePDFFiles()....", category: Category.Exception, priority: Priority.Low);
            try
            {
                GC.Collect();
                ulong pageOffset = 0;
                ulong FilesCount = 0;
                Document document = null;
                PdfCopy writer = null;
                foreach (var eachFile in fileNamesList)
                {
                    if (eachFile.ContractSituationFileInBytes == null)
                    {
                        byte[] eachfileBytes = HrmService.GetEmployeeDocumentFile(EmployeeCode, eachFile);
                        eachFile.ContractSituationFileInBytes = eachfileBytes;
                    }
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(eachFile.ContractSituationFileInBytes);
                    reader.ConsolidateNamedDestinations();
                    // we retrieve the total number of pages
                    ulong n = Convert.ToUInt64 (reader.NumberOfPages);
                    pageOffset += n;
                    if (FilesCount == 0)
                    {
                        // step 1: creation of a document-object
                        document = new Document(reader.GetPageSizeWithRotation(1));
                        // step 2: we create a writer that listens to the document
                        //writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                        writer = new PdfCopy(document, new FileStream(tempfilepath, FileMode.Create));
                        // step 3: we open the document
                        document.Open();
                    }
                    // step 4: we add content
                    for (ulong i = 0; i < n;)
                    {
                        ++i;
                        if (writer != null)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, Convert.ToInt32(i));
                            writer.AddPage(page);
                        }
                    }
                    FilesCount++;
                }
                // step 5: we close the document
                if (document != null)
                {
                    document.Close();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method MergeMultiplePDFFiles()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                GC.Collect();
            }
            GeosApplication.Instance.Logger.Log("Method MergeMultiplePDFFiles()....executed successfully", category: Category.Exception, priority: Priority.Low);
        }

        private void CloseMergeWindow(object obj)
        {
            Attachment attachment = new Attachment();
            List<object> newAttachmentList = new List<object>();

            SelectedViewIndex = 0;
            IsMergeVisible = true;
            //IsMergeImageVisibility = Visibility.Visible;
            //IsAddImageVisibility = Visibility.Collapsed;
            IsAddVisible = false;
            IsHeader = Visibility.Visible;
            WindowHeader = Application.Current.FindResource("EditContractSituationInformation").ToString();
            isMerge = false;
            attachment.OriginalFileName = EmployeeContractSituationList[0].ContractSituationFileName;
            ContractSituationFileName = EmployeeContractSituationList[0].ContractSituationFileName;
            ContractSituationFileInBytes = EmployeeContractSituationList[0].ContractSituationFileInBytes;
            newAttachmentList.Add(attachment);

            EmployeeContractSituationList.Clear();
            AttachmentList.Clear();
            
            AttachmentList = newAttachmentList;
            //foreach (TaskContractSituation task in Tasks.ToList())
            //{
            //    Tasks.Remove(task);
            //}
        }

        private void OnDragRecordOverCommandAction(DragRecordOverEventArgs e)
        {
            object data = e.Data.GetData(typeof(RecordDragDropData));
            int dropIndex = e.TargetRowHandle;
            foreach (EmployeeContractSituation employee in ((RecordDragDropData)data).Records)
            {
                if (SelectedRowEmployeeContractSituationList.Count == 1 && EmployeeContractSituationList[0] == employee)
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                    MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["DeleteOriginalContractSituationMessage"].ToString(), Application.Current.Resources["PopUpOverlapColor"].ToString(), CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
            }
        }

        private void OnDropRecordCommandAction(DropRecordEventArgs e)
        {
            int dropIndex = e.TargetRowHandle;
            //object data = e.Data.GetData(typeof(RecordDragDropData));
            if (e.DropPosition == DropPosition.Before && dropIndex == 0)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = false;
            }

        }

        //private void AddNewSplitTaskCommandAction(object obj)
        //{
        //    int count = EmployeeContractSituationList.Count + 1;

        //    //Tasks.Add(new TaskContractSituation()
        //    //{
        //    //    //IsNew = true,                
        //    //    //ImageSource = EmployeeShiftList,               
        //    //});

        //}

        //public bool CanCloseTask(TaskContractSituation task)
        //{
        //    if (task != null)
        //    {
        //        //return task.IsComplete;
        //    }

        //    return true;
        //}

        //public void CloseTask(TaskContractSituation task)
        //{

        //    GeosApplication.Instance.Logger.Log("Method CloseTask ...", category: Category.Info, priority: Priority.Low);

        //    //if (task.IsComplete == true)
        //    //{
        //    //    Tasks.Remove(task);
        //    //}

        //    GeosApplication.Instance.Logger.Log("Method CloseTask() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        //public EmployeeAttendance NewSplitEmployeeAttendance;
        //public EmployeeAttendance UpdatesplitEmployeeAttendance;
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
                 me[BindableBase.GetPropertyName(() => SelectedIndexContractSituation)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexProfessionalCategory)] +
                      me[BindableBase.GetPropertyName(() => SelectedIndexCompany)];


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

                string empcontractStartDate = BindableBase.GetPropertyName(() => StartDate);
                string empcontractEndDate = BindableBase.GetPropertyName(() => EndDate);
                string empselectedIndexContractSituation = BindableBase.GetPropertyName(() => SelectedIndexContractSituation);
                string empselectedIndexProfessionalCategory = BindableBase.GetPropertyName(() => SelectedIndexProfessionalCategory);
                string empselectedIndexCompany = BindableBase.GetPropertyName(() => SelectedIndexCompany);

                if (columnName == empselectedIndexContractSituation)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexContractSituation, SelectedIndexContractSituation);
                }
                if (columnName == empselectedIndexProfessionalCategory)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexProfessionalCategory, SelectedIndexProfessionalCategory);
                }

                if (columnName == empcontractStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empcontractStartDate, StartDate);
                    }
                }
                if (columnName == empcontractEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                }
                if (columnName == empselectedIndexCompany)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexCompany, SelectedIndexCompany);
                }

                return null;
            }
        }
        #endregion
    }

    //public class TaskContractSituation : INotifyPropertyChanged, IDataErrorInfo
    //{
    //    #region Declaration       
    //    private bool isNew;
    //    private string contractSituationFileName;
    //    private ObservableCollection<EmployeeShift> imageSourceList;
    //    private ObservableCollection<EmployeeContractSituation> employeeContractSituationList;
    //    //public EmployeeContractSituation EmployeeContractSituationList { get; set; }
    //    private EmployeeContractSituation selectedContractSituationRow;
    //    public EmployeeContractSituation SelectedContractSituationRow
    //    {
    //        get { return selectedContractSituationRow; }
    //        set
    //        {
    //            selectedContractSituationRow = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("SelectedContractSituationRow"));
    //        }
    //    }
    //    private Company company { get; set; }
    //    private List<object> attachmentList;
    //    public ContractSituation ContractSituation { get; set; }

    //    private byte[] contractSituationFileInBytes;
    //    #endregion

    //    #region Properties
    //    public byte[] ContractSituationFileInBytes
    //    {
    //        get
    //        {
    //            return contractSituationFileInBytes;
    //        }

    //        set
    //        {
    //            contractSituationFileInBytes = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileInBytes"));
    //        }
    //    }
    //    //public bool IsNew
    //    //{
    //    //    get { return isNew; }
    //    //    set
    //    //    {
    //    //        isNew = value;
    //    //        OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
    //    //    }
    //    //}
    //    //public bool IsComplete { get; set; }

    //    public string ContractSituationFileName
    //    {
    //        get
    //        {
    //            return contractSituationFileName;
    //        }
    //        set
    //        {
    //            contractSituationFileName = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileName"));
    //        }
    //    }
    //    public ObservableCollection<EmployeeShift> ImageSourceList
    //    {
    //        get
    //        {
    //            return imageSourceList;
    //        }

    //        set
    //        {
    //            imageSourceList = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("ImageSourceList"));
    //        }
    //    }

    //    //public ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList
    //    //{
    //    //    get
    //    //    {
    //    //        return employeeContractSituationList;
    //    //    }

    //    //    set
    //    //    {
    //    //        employeeContractSituationList = value;
    //    //        OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituationList"));
    //    //    }
    //    //}

    //    public List<object> AttachmentList
    //    {
    //        get
    //        {
    //            return attachmentList;
    //        }

    //        set
    //        {
    //            attachmentList = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
    //        }
    //    }

    //    public Company Company
    //    {
    //        get { return company; }
    //        set
    //        {
    //            company = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("Company"));
    //        }
    //    }

    //    #endregion

    //    #region Public ICommand

    //    //public ICommand OnTextEditValueChangingCommand { get; set; }

    //    #endregion

    //    #region Constructor

    //    public TaskContractSituation()
    //    {
    //        //OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTimeEditValueChanging);
    //    }
    //    #endregion

    //    #region Validation

    //    private string startDateErrorMessage = string.Empty;
    //    private string endDateErrorMessage = string.Empty;
    //    private string startTimeErrorMessage = string.Empty;
    //    private string endTimeErrorMessage = string.Empty;

    //    bool allowValidation = false;

    //    string EnableValidationAndGetError()
    //    {
    //        allowValidation = true;
    //        string error = ((IDataErrorInfo)this).Error;
    //        if (!string.IsNullOrEmpty(error))
    //        {
    //            return error;
    //        }
    //        return null;
    //    }

    //    string IDataErrorInfo.Error
    //    {
    //        get
    //        {
    //            if (!allowValidation) return null;
    //            IDataErrorInfo me = (IDataErrorInfo)this;
    //            //string error =
    //            //me[BindableBase.GetPropertyName(() => SelectedIndexForEmployee)];


    //            //if (!string.IsNullOrEmpty(error))
    //            //    return "Please check inputted data.";

    //            return null;
    //        }
    //    }

    //    string IDataErrorInfo.this[string columnName]
    //    {
    //        get
    //        {
    //            if (!allowValidation) return null;
    //            //string selectedIndexForEmployeeProp = BindableBase.GetPropertyName(() => SelectedIndexForEmployee);

    //            //if (columnName == taskEndDateProp)
    //            //{
    //            //    if (!string.IsNullOrEmpty(endDateErrorMessage))
    //            //    {
    //            //        return endDateErrorMessage;
    //            //    }
    //            //    else
    //            //    {
    //            //        return AttendanceValidation.GetErrorMessage(taskEndDateProp, TaskEndDate);
    //            //    }
    //            //}            

    //            return null;
    //        }
    //    }

    //    #endregion

    //    #region Events

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void OnPropertyChanged(PropertyChangedEventArgs e)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, e);
    //        }
    //    }

    //    #endregion

    //    #region Methods

    //    internal string CheckValidation()
    //    {
    //        string error = EnableValidationAndGetError();

    //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
    //        PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));

    //        return error;
    //    }

    //    #endregion
    //}
}
