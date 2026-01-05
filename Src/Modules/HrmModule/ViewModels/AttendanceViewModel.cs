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
using DevExpress.Xpf.Map;
using System.Threading;
using System.Net;
using ServiceStack;
using System.Diagnostics;
using System.Security.AccessControl;
using DevExpress.XtraSpreadsheet.Forms;
using Emdep.Geos.Data.Common.APM;
using DevExpress.Xpf.Editors.Helpers;

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
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        // ICrmService CrmService = new CrmServiceController("localhost:6699");
        #endregion

        #region Declaration
        int originalIndexForAttendanceType = 0;
        DateTime startTimeOriginal = new DateTime();
        DateTime endTimeOriginal = new DateTime();
        private string windowHeader;
        private bool isNew;
        //private List<DateTime> fromDates;
        private string timeEditMask;
        public bool flag;
        public bool flagEdit;
        public bool flagEditDate;
        private bool EmployeeChange = false;
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
        private int selectedForAttendanceStatus;
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
        private bool isAnyStatusInUse;
        #region GEOS2-4018/GEOS2-4019
        private bool isAdd;
        private byte[] attendanceFileInBytes;
        private string attendanceFileName;
        private bool isBusy;
        private ObservableCollection<Attachment> attachmentList;
        private List<object> attachmentObjectList;
        private Visibility isVisible;
        private Attachment attachedFile;

        private string remarks;

        private EmployeeAttendance oldEmployeeAttendanceDetails;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        #endregion

        double PreviousDays { get; set; }//[Sudhir.jangra][GEOS2-5336]
        double PreviousHours { get; set; }//[Sudhir.jangra][GEOS2-5336]

        double TotalPreviousHours { get; set; }//[Sudhir.jangra][GEOS2-5336]

        int PreviousIdType { get; set; }//[Sudhir.jangra][GEOS2-5336]
        string locationvalue;
        bool isLocationAvailable;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private string address;
        private LookupValue selectedAttendanceStatus;
        EmployeeAttendance EmployeeAttendance { get; set; }

        bool IsFlagTrue { get; set; }

        //[nsatpute][19-12-2024][GEOS2-6635]
        private bool isPartialTime;
        private Visibility partialTimeInformationVisible;
        private bool isPartialTimeEnable;
        private bool enableEndDateTime;
        private LookupValue previousSelectedForAttendanceStatus;
        private EmployeeAttendance previousEmployeeSpliteAttendanceDetails;
        #endregion

        #region Properties

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }
        public GeoPoint MapLatitudeAndLongitude
        {
            get { return mapLatitudeAndLongitude; }
            set
            {
                mapLatitudeAndLongitude = value;

                if (mapLatitudeAndLongitude != null)
                    IsCoordinatesNull = true;
                else
                    IsCoordinatesNull = false;

                OnPropertyChanged(new PropertyChangedEventArgs("MapLatitudeAndLongitude"));
            }
        }
        public bool IsAnyStatusInUse
        {
            get { return isAnyStatusInUse; }
            set
            {
                isAnyStatusInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAnyStatusInUse"));
            }
        }
        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
        }
        public double? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Latitude"));
            }
        }

        public double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Longitude"));
            }
        }

        public int OriginalIndexForAttendanceType
        {
            get { return originalIndexForAttendanceType; }
            set
            {
                originalIndexForAttendanceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalIndexForAttendanceType"));
            }
        }

        public bool IsLocationAvailable
        {
            get { return isLocationAvailable; }
            set
            {
                isLocationAvailable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationAvailable"));
            }
        }
        public string LocationValue
        {
            get { return locationvalue; }
            set
            {
                locationvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationValue"));
            }
        }

        public DateTime StartTimeOriginal
        {
            get { return startTimeOriginal; }
            set
            {
                startTimeOriginal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTimeOriginal"));
            }
        }

        public DateTime EndTimeOriginal
        {
            get { return endTimeOriginal; }
            set
            {
                endTimeOriginal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndTimeOriginal"));
            }
        }
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
                IsPartialTimeEnable = !HrmService.IsPartialAttendanceExists(EmployeeListFinal[selectedIndexForEmployee].IdEmployee); //[nsatpute][19-12-2024][GEOS2-6635]
                if (!IsPartialTimeEnable)
                    IsPartialTime = false;
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

        public int SelectedForAttendanceStatus
        {
            get { return selectedForAttendanceStatus; }
            set
            {
                selectedForAttendanceStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedForAttendanceStatus"));
            }
        }

        public LookupValue SelectedAttendanceStatus
        {
            get { return selectedAttendanceStatus; }
            set
            {
                selectedAttendanceStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendanceStatus"));
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


        #region GEOS2-4018/GEOS2-4019
        public bool IsAdd
        {
            get { return isAdd; }
            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }
        public byte[] AttendanceFileInBytes
        {
            get { return attendanceFileInBytes; }
            set
            {
                attendanceFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceFileInBytes"));
            }
        }
        public string AttendanceFileName
        {
            get { return attendanceFileName; }
            set
            {
                attendanceFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendanceFileName"));
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
        public ObservableCollection<Attachment> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
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
        public Attachment AttachedFile
        {
            get { return attachedFile; }
            set
            {
                attachedFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFile"));
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
        public EmployeeAttendance OldEmployeeAttendanceDetails
        {
            get { return oldEmployeeAttendanceDetails; }
            set
            {
                oldEmployeeAttendanceDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldEmployeeAttendanceDetails"));
            }
        }

        string creatorName;
        public string CreatorName
        {
            get { return creatorName; }
            set
            {
                creatorName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatorName"));
            }
        }
        Int32 idReporterSource;
        public Int32 IdReporterSource
        {
            get { return idReporterSource; }
            set
            {
                idReporterSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdReporterSource"));
            }
        }
        Int32? creator;
        public Int32? Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Creator"));
            }
        }

        DateTime? creationDate;
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDate"));
            }
        }

        public string FormattedCreationDate
        {
            get
            {
                if (creationDate.HasValue)
                {
                    CultureInfo cn = Thread.CurrentThread.CurrentCulture;
                    return creationDate.Value.ToString("d", cn);
                }
                return string.Empty;
            }
        }

        DateTime? modificationDate;
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModificationDate"));
            }
        }

        Int32? modifier;
        public Int32? Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Modifier"));
            }
        }

        string modifierName;
        public string ModifierName
        {
            get { return modifierName; }
            set
            {
                modifierName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifierName"));
            }
        }

        string formattedDate;
        public string FormattedDate
        {
            get { return formattedDate; }
            set
            {
                formattedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FormattedDate"));
            }
        }

        byte[] modifierProfilePhotoBytes;
        public byte[] ModifierProfilePhotoBytes
        {
            get { return modifierProfilePhotoBytes; }
            set
            {
                modifierProfilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifierProfilePhotoBytes"));
            }
        }

        byte[] creatorprofilePhotoBytes;
        public byte[] CreatorProfilePhotoBytes
        {
            get { return creatorprofilePhotoBytes; }
            set
            {
                creatorprofilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatorProfilePhotoBytes"));
            }
        }

        Visibility isCreatorVisibility;
        public Visibility IsCreatorVisibility
        {
            get { return isCreatorVisibility; }
            set
            {
                isCreatorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCreatorVisibility"));
            }
        }


        Visibility isModifierVisibility;
        public Visibility IsModifierVisibility
        {
            get { return isModifierVisibility; }
            set
            {
                isModifierVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModifierVisibility"));
            }
        }

        string mapLatitudeAndLongitudeURL;
        public string MapLatitudeAndLongitudeURL
        {
            get { return mapLatitudeAndLongitudeURL; }
            set
            {
                mapLatitudeAndLongitudeURL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MapLatitudeAndLongitudeURL"));
            }
        }

        Visibility isMapVisibility;
        public Visibility IsMapVisibility
        {
            get { return isMapVisibility; }
            set
            {
                isMapVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMapVisibility"));
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6635]
        public bool IsPartialTime
        {
            get
            {
                return isPartialTime;
            }

            set
            {
                isPartialTime = value;
                EnableEndDateTime = !isPartialTime;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPartialTime"));
                if (isPartialTime)
                {
                    EndDate = null; EndTime = null;
                }

            }
        }
        //[nsatpute][19-12-2024][GEOS2-6635]
        public Visibility PartialTimeInformationVisible
        {
            get
            {
                return partialTimeInformationVisible;
            }

            set
            {
                partialTimeInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PartialTimeInformationVisible"));
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6635]
        public bool IsPartialTimeEnable
        {
            get
            {
                return isPartialTimeEnable;
            }

            set
            {
                isPartialTimeEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPartialTimeEnable"));
                if (isPartialTimeEnable)
                    PartialTimeInformationVisible = Visibility.Hidden;
                else
                    PartialTimeInformationVisible = Visibility.Visible;
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6635]
        public bool EnableEndDateTime
        {
            get
            {
                return enableEndDateTime;
            }

            set
            {
                enableEndDateTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableEndDateTime"));
            }
        }
        #endregion

        public LookupValue PreviousSelectedForAttendanceStatus
        {
            get { return previousSelectedForAttendanceStatus; }
            set
            {
                previousSelectedForAttendanceStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedForAttendanceStatus"));
            }
        }

        public EmployeeAttendance PreviousEmployeeSpliteAttendanceDetails
        {
            get { return previousEmployeeSpliteAttendanceDetails; }
            set
            {
                previousEmployeeSpliteAttendanceDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousEmployeeSpliteAttendanceDetails"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand AddAttendanceViewCancelButtonCommand { get; set; }
        public ICommand AddAttendanceViewAcceptButtonCommand { get; set; }
        public ICommand OnDateFocusCommand { get; set; }
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

        public ICommand DocumentViewCommand { get; set; }//[Sudhir.Jangra][GEOS2-4018]

        public ICommand ChooseFileCommandForAttendance { get; set; }//[Sudhir.Jangra][GEOS2-4018]

        public ICommand SelectedItemChangedCommand { get; set; }//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        public ICommand SearchButtonClickCommand { get; set; }

        public ICommand CopyButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AttendanceViewModel(AttendanceView addAttendanceView)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddAttendanceViewModel()...", category: Category.Info, priority: Priority.Low);
                EventHandler handle = delegate { addAttendanceView.Close(); };
                this.RequestClose += handle;
                addAttendanceView.DataContext = this;
                this.SelectedPeriod = HrmCommon.Instance.SelectedPeriod;
                this.SelectedPlantList = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

                SetUserPermission();
                AddAttendanceViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddAttendanceViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddAttendanceInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                OnDateFocusCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateFocusAction);
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

                DocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeAttendanceDocument));
                ChooseFileCommandForAttendance = new RelayCommand(new Action<object>(BrowseFileAction));

                TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
                SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
                FillEmployeeWorkType();
                FillAttendanceStatus();
                IsModifierVisibility = Visibility.Collapsed;
                IsCreatorVisibility = Visibility.Collapsed;
                IsMapVisibility = Visibility.Collapsed;
                CopyButtonCommand = new RelayCommand(new Action<object>(CopyButtonClickCommandAction));
                IsFlagTrue = false;
                //[nsatpute][19-12-2024][GEOS2-6635]
                IsPartialTimeEnable = true;
                EnableEndDateTime = true;
                GrantPermissions();
                GeosApplication.Instance.Logger.Log("Constructor AddAttendanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddAttendanceViewModel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
        private void CopyButtonClickCommandAction(object obj)
        {
            try
            {
                if (Latitude != null && Longitude != null)
                {
                    string textToCopy = $"{Latitude},{Longitude}";
                    Clipboard.SetText(textToCopy);
                    //GeoPoint GeoPoint = new GeoPoint(Latitude.Value, Longitude.Value);
                    // Format the text to be copied
                    //string textToCopy = textToCopy = $"{GeoPoint.Latitude},{GeoPoint.Longitude}";
                    // Copy the text to the clipboard
                    //Clipboard.SetText(textToCopy);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CopyButtonClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchButtonClickCommandAction(object obj)
        {
            try
            {
                if (Latitude != null && Longitude != null)
                {
                    string url = $"https://www.google.com/maps?q={latitude},{longitude}";
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true // Ensures it opens in the default browser
                    });
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchButtonClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        private void SearchButtonClickCommandActionOld(object obj)
        {
            if (isCoordinatesNull == true)
            {
                EmployeeSetLocationMapViewModel employeeSetLocationMapViewModel = new EmployeeSetLocationMapViewModel();
                EmployeeSetLocationMapView employeeSetLocationMapView = new EmployeeSetLocationMapView();
                EventHandler handle = delegate { employeeSetLocationMapView.Close(); };
                employeeSetLocationMapViewModel.RequestClose += handle;

                if (MapLatitudeAndLongitude != null)
                {
                    employeeSetLocationMapViewModel.MapLatitudeAndLongitude = MapLatitudeAndLongitude;
                    employeeSetLocationMapViewModel.LocalMapLatitudeAndLongitude = MapLatitudeAndLongitude;
                }

                employeeSetLocationMapView.DataContext = employeeSetLocationMapViewModel;
                var ownerInfo = (obj as FrameworkElement);
                employeeSetLocationMapView.Owner = Window.GetWindow(ownerInfo);
                employeeSetLocationMapView.ShowDialog();

                if (employeeSetLocationMapViewModel.MapLatitudeAndLongitude != null
                    && !string.IsNullOrEmpty(employeeSetLocationMapViewModel.LocationAddress))
                {
                    if (!string.IsNullOrEmpty(Address))
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            Latitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                            Longitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                            MapLatitudeAndLongitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude;
                            LocationValue = "(" + MapLatitudeAndLongitude.ToString() + ")";
                        }
                    }
                    else
                    {
                        Latitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                        Longitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                        MapLatitudeAndLongitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude;
                        LocationValue = "(" + MapLatitudeAndLongitude.ToString() + ")";
                    }
                }
            }
        }

        public static double CalculateTotalHours(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = new TimeSpan();
            try
            {
                if (endTime < startTime)
                    duration = (endTime.AddDays(1) - startTime);

                else
                    duration = (endTime - startTime);
            }
            catch (Exception ex)
            {

            }
            return duration.TotalHours;
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
        public static void FillAttendanceStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttendanceStatus()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.AttendanceStatusList == null)
                {
                    ICrmService myCrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    GeosApplication.Instance.AttendanceStatusList = new ObservableCollection<LookupValue>(myCrmStartUp.GetLookupValues(97));

                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendanceStatus() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendanceStatus() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAttendanceStatus()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Add Attendance Information
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002][skale][31/01/2020][GEOS2-1959]- GHRM - Leaves/Attendance inactive employees
        /// [003][cpatil][18/04/2022][GEOS2-3043]- HRM - Attendance introduction manual
		/// [004][nsatpute][19/12/2024][GEOS2-6635] HRM - Need support for partial attendance (1 of 5)
        /// </summary>
        /// <param name="obj"></param>
        private void AddAttendanceInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()...", category: Category.Info, priority: Priority.Low);
                DateTime attendanceEndDate = EndDate == null ? StartDate.Value : EndDate.Value;
                error = EnableValidationAndGetError();
                if (error != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForEmployee"));
                    PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                    PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                    PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForAttendanceType"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForCompanyShift"));
                    return;
                }

                var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(i => i.IdCompany));
                var SelectedEmployee = EmployeeListFinal[SelectedIndexForEmployee];
                var SelectedEmployeeListJoined = string.Join(",", SelectedEmployee.IdEmployee);
                //shubham[skadam] GEOS2-3919 HRM - Register different leaves at the same time 11 OCT 2022
                if (EmployeeLeaves == null || EmployeeLeaves.Count == 0)
                {
                    EmployeeLeaves = new ObservableCollection<EmployeeLeave>();
                    EmployeeLeaves.AddRange(HrmService.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate.Value, attendanceEndDate, SelectedEmployeeListJoined));
                }

                if (ExistEmployeeAttendanceList == null || ExistEmployeeAttendanceList.Count == 0)
                {
                    ExistEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    ExistEmployeeAttendanceList.AddRange(HrmService.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2190(plantOwnersIdsJoined, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission, StartDate.Value, attendanceEndDate, SelectedEmployeeListJoined)); //[nsatpute][19-12-2024][GEOS2-6636]
                }
                IsAcceptButton = true;

                #region GEOS2-4018
                if (!string.IsNullOrEmpty(Remarks))
                {
                    Remarks = Remarks.Trim();
                }
                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    AttendanceFileName = null;
                    AttendanceFileInBytes = null;
                }
                #endregion

                StartDate = StartDate.Value.Date.AddHours(StartTime.Value.TimeOfDay.Hours).AddMinutes(StartTime.Value.TimeOfDay.Minutes);
                if (EndDate != null)
                    EndDate = EndDate.Value.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);


                //var shiftStartTime = StartTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].StartTime1.Minutes);
                //var shiftEndTime = EndTime.Value.Date.AddHours(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Hours).AddMinutes(CompanyShifts[SelectedIndexForCompanyShift].EndTime1.Minutes);
                STime = StartTime.Value.TimeOfDay;
                if (EndDate != null)
                    ETime = EndDate.Value.TimeOfDay;
                else
                    ETime = StartDate.Value.TimeOfDay;

                List<EmployeeAttendance> ExistEmpAttendanceList = new List<EmployeeAttendance>();
                //ExistEmpAttendanceList.FirstOrDefault().is
                if (IsNew)
                {
                    //[003]
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && (x.StartDate.Date >= StartDate.Value.Date && x.EndDate.Date <= attendanceEndDate.Date)).OrderBy(x => x.StartDate).ToList();
                    //if(ExistEmpAttendanceList.Count == 0)
                    //{
                    //    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && x.EndDate.Date == StartDate.Value.Date).OrderBy(x => x.EndDate).ToList();
                    //}
                }
                else
                {
                    //[003]
                    ExistEmpAttendanceList = ExistEmployeeAttendanceList.Where(x => x.IdEmployee == EmployeeListFinal[SelectedIndexForEmployee].IdEmployee && (x.StartDate.Date >= StartDate.Value.Date && x.EndDate.Date <= attendanceEndDate.Date) && x.IdEmployeeAttendance != IdEmployeeAttendance).OrderBy(x => x.StartDate).ToList();
                }

                if (ExistEmpAttendanceList.Count() > 0)
                {
                    //[003]
                    if (ExistEmpAttendanceList.FirstOrDefault().StartDate.Date >= StartDate.Value.Date && ExistEmpAttendanceList.FirstOrDefault().EndDate.Date <= attendanceEndDate.Date && ExistEmpAttendanceList.FirstOrDefault().IdCompanyShift != EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift)
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
                DateTime endDate = EndDate == null ? StartDate.Value : EndDate.Value;
                if (Employee != null)
                {
                    if (Employee.EmployeeContractSituations != null && Employee.EmployeeContractSituations.Count > 0)
                    {
                        for (int i = 0; i < Employee.EmployeeContractSituations.Count; i++)
                        {
                            if (Employee.EmployeeContractSituations[i].ContractSituationEndDate == null)
                            {

                                DateTime? EmployeeContractEndDate = Employee.EmployeeContractSituations[i].ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : Employee.EmployeeContractSituations[i].ContractSituationEndDate;

                                if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && endDate.Date <= EmployeeContractEndDate.Value.Date)
                                {
                                    IsAddInActiveEmployeeAttendance = true;
                                    break;
                                }
                                else
                                    IsAddInActiveEmployeeAttendance = false;

                            }
                            else if (StartDate.Value.Date >= Employee.EmployeeContractSituations[i].ContractSituationStartDate.Value.Date && attendanceEndDate.Date <= Employee.EmployeeContractSituations[i].ContractSituationEndDate.Value.Date)
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
                    NewEmployeeAttendance = new EmployeeAttendance();
                    NewEmployeeAttendance.Employee = EmployeeListFinal[SelectedIndexForEmployee];
                    NewEmployeeAttendance.IdEmployee = EmployeeListFinal[SelectedIndexForEmployee].IdEmployee;
                    NewEmployeeAttendance.StartDate = (DateTime)StartDate;
                    if (EndDate != null)
                        NewEmployeeAttendance.EndDate = (DateTime)EndDate;

                    NewEmployeeAttendance.Remark = Remarks;
                    NewEmployeeAttendance.FileName = AttendanceFileName;
                    NewEmployeeAttendance.IdCompanyWork = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue;
                    NewEmployeeAttendance.IdCompanyShift = EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift;
                    NewEmployeeAttendance.CurrentContractForAttendance = EmployeeListFinal[SelectedIndexForEmployee].EmployeeContractSituations.Where(ecs => ecs.ContractSituationStartDate.Value.Date <= (DateTime)StartDate.Value.Date).Select(i => i.Company.Alias).LastOrDefault();
                    NewEmployeeAttendance.Latitude = Latitude;
                    NewEmployeeAttendance.Langitude = Longitude;
                    NewEmployeeAttendance.IdStatus = GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue;
                    NewEmployeeAttendance.AttendanceStatus = GeosApplication.Instance.AttendanceStatusList.Where(i => i.IdLookupValue == GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue).FirstOrDefault();
                    NewEmployeeAttendanceList = new ObservableCollection<EmployeeAttendance>();
                    DateTime tempEndDate = NewEmployeeAttendance.EndDate;

                    if (EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IsNightShift == 0 || ((NewEmployeeAttendance.EndDate.Date - NewEmployeeAttendance.StartDate.Date).TotalDays) <= 0)
                    {
                        if (NewEmployeeAttendance.EndDate.Date == DateTime.MinValue)
                        {
                            NewEmployeeAttendance.StartDate = Convert.ToDateTime(StartDate);
                            NewEmployeeAttendance.EndDate = DateTime.MinValue;
                            NewEmployeeAttendance.Remark = Remarks;
                            NewEmployeeAttendance.FileName = AttendanceFileName;
                            NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                            NewEmployeeAttendance.AccountingDate = StartDate;
                            NewEmployeeAttendance.IsManual = 1;
                            NewEmployeeAttendance.Creator = GeosApplication.Instance.ActiveUser.IdUser;
                            NewEmployeeAttendance.CreationDate = GeosApplication.Instance.ServerDateTime;
                            NewEmployeeAttendance.Location = LocationValue;
                            NewEmployeeAttendance.AttendanceStatus = GeosApplication.Instance.AttendanceStatusList.Where(i => i.IdLookupValue == GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue).FirstOrDefault();
                            IsSave = true;

                            EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2590(NewEmployeeAttendance, AttendanceFileInBytes);

                            NewEmployeeAttendanceList.Add(updateEmployeeAttendance);
                            if (GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue == 305)
                            {
                                EmployeeChangelog changelog = new EmployeeChangelog();
                                int currentDays = 0;
                                double currentHours = 0;
                                double dayshours = 0;
                                TimeSpan timedifference = NewEmployeeAttendance.EndDate - NewEmployeeAttendance.StartDate;
                                if (timedifference != null)
                                {
                                    dayshours = timedifference.TotalHours;
                                    TotalPreviousHours += timedifference.TotalHours;
                                    int days = (int)(dayshours / 8);
                                    double remainingHours = dayshours % 8;

                                    if (days > 0)
                                    {
                                        currentDays = days;
                                    }
                                    else
                                    {
                                        currentDays = 0;
                                    }
                                    if (remainingHours > 0)
                                    {
                                        currentHours = remainingHours;
                                    }
                                    else
                                    {
                                        currentHours = 0;
                                    }
                                }

                                changelog.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                                changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                changelog.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceChangeLog").ToString(), currentDays, currentHours);
                                if (GeosApplication.Instance.CompensationLeave == null)
                                {
                                    GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                                }

                                GeosApplication.Instance.CompensationLeave.Comments = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompensatoryAutomaticallyComment").ToString());

                                bool isAdded = HrmService.AddEmployeeChangelogs_V2500(changelog);

                                EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                temp.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                temp.Year = (Int32)SelectedPeriod;
                                temp.IdLeave = 241;
                                temp.RegularHoursCount = 0;
                                temp.BacklogHoursCount = 0;
                                bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, TotalPreviousHours);
                            }
                        }
                        else
                        {
                            for (var item = NewEmployeeAttendance.StartDate; item.Date <= tempEndDate.Date; item = NewEmployeeAttendance.StartDate.AddDays(1))
                            {

                                NewEmployeeAttendance.StartDate = item;
                                NewEmployeeAttendance.EndDate = NewEmployeeAttendance.StartDate.Date.AddHours(EndTime.Value.TimeOfDay.Hours).AddMinutes(EndTime.Value.TimeOfDay.Minutes);
                                NewEmployeeAttendance.Remark = Remarks;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                                NewEmployeeAttendance.FileName = AttendanceFileName;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                                // NewEmployeeAttendance.Employee.EmployeeJobDescription.Company = NewEmployeeAttendance.Employee.EmployeeContractSituation.Company;
                                //NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                                NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                                NewEmployeeAttendance.AccountingDate = item;
                                NewEmployeeAttendance.IsManual = 1;
                                //[GEOS2-3095]
                                NewEmployeeAttendance.Creator = GeosApplication.Instance.ActiveUser.IdUser;
                                NewEmployeeAttendance.CreationDate = GeosApplication.Instance.ServerDateTime;
                                NewEmployeeAttendance.Location = LocationValue;
                                NewEmployeeAttendance.AttendanceStatus = GeosApplication.Instance.AttendanceStatusList.Where(i => i.IdLookupValue == GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue).FirstOrDefault();
                                IsSave = true;
                                //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
                                //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
                                // Shubham[skadam] GEOS2 - 4473 Improvements in Attendance and Leave registration using mobile APP (16 / 20)  11 07 2023
                                //    EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2410(NewEmployeeAttendance);
                                //    EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2420(NewEmployeeAttendance);
                                //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                                EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2520(NewEmployeeAttendance, AttendanceFileInBytes);

                                NewEmployeeAttendanceList.Add(updateEmployeeAttendance);
                                if (GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue == 305)
                                {

                                    //EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                    //temp.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                    //temp.Year = (Int32)SelectedPeriod;
                                    //temp.IdLeave = 241;
                                    //temp.RegularHoursCount = 0;
                                    //temp.BacklogHoursCount = 0;
                                    EmployeeChangelog changelog = new EmployeeChangelog();
                                    int currentDays = 0;
                                    double currentHours = 0;
                                    double dayshours = 0;
                                    TimeSpan timedifference = NewEmployeeAttendance.EndDate - NewEmployeeAttendance.StartDate;
                                    if (timedifference != null)
                                    {
                                        dayshours = timedifference.TotalHours;
                                        TotalPreviousHours += timedifference.TotalHours;
                                        int days = (int)(dayshours / 8);
                                        double remainingHours = dayshours % 8;

                                        if (days > 0)
                                        {
                                            currentDays = days;
                                        }
                                        else
                                        {
                                            currentDays = 0;
                                        }
                                        if (remainingHours > 0)
                                        {
                                            currentHours = remainingHours;
                                        }
                                        else
                                        {
                                            currentHours = 0;
                                        }
                                    }

                                    changelog.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                    changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                                    changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    changelog.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceChangeLog").ToString(), currentDays, currentHours);
                                    if (GeosApplication.Instance.CompensationLeave == null)
                                    {
                                        GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                                    }
                                    GeosApplication.Instance.CompensationLeave.Comments = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompensatoryAutomaticallyComment").ToString());

                                    bool isAdded = HrmService.AddEmployeeChangelogs_V2500(changelog);

                                    EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                    temp.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                    temp.Year = (Int32)SelectedPeriod;
                                    temp.IdLeave = 241;
                                    temp.RegularHoursCount = 0;
                                    temp.BacklogHoursCount = 0;

                                    bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, dayshours);//[GEOS2-6654][rdixit][20.01.2025]

                                }
                            }
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
                            NewEmployeeAttendance.Remark = Remarks;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                            NewEmployeeAttendance.FileName = AttendanceFileName;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                            // NewEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                            NewEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;
                            NewEmployeeAttendance.AccountingDate = item;
                            NewEmployeeAttendance.IsManual = 1;
                            //[GEOS2-3095]
                            NewEmployeeAttendance.Creator = GeosApplication.Instance.ActiveUser.IdUser;
                            NewEmployeeAttendance.CreationDate = GeosApplication.Instance.ServerDateTime;
                            NewEmployeeAttendance.Latitude = Latitude;
                            NewEmployeeAttendance.Langitude = Longitude;
                            IsSave = true;
                            //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewEmployeeAttendance);
                            //EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewEmployeeAttendance);
                            //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
                            //  EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2410(NewEmployeeAttendance);
                            //  EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2420(NewEmployeeAttendance);
                            //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                            EmployeeAttendance updateEmployeeAttendance = HrmService.AddEmployeeAttendance_V2520(NewEmployeeAttendance, AttendanceFileInBytes);

                            NewEmployeeAttendanceList.Add(updateEmployeeAttendance);
                            if (GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue == 305)
                            {
                                //EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                //temp.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                //temp.Year = (Int32)SelectedPeriod;
                                //temp.IdLeave = 241;
                                //temp.RegularHoursCount = 0;
                                //temp.BacklogHoursCount = 0;

                                EmployeeChangelog changelog = new EmployeeChangelog();
                                int currentDays = 0;
                                double currentHours = 0;
                                double dayshours = 0;
                                TimeSpan timedifference = NewEmployeeAttendance.EndDate - NewEmployeeAttendance.StartDate;
                                if (timedifference != null)
                                {
                                    dayshours = timedifference.TotalHours;
                                    TotalPreviousHours += timedifference.TotalHours;
                                    int days = (int)(dayshours / 8);
                                    double remainingHours = dayshours % 8;

                                    if (days > 0)
                                    {
                                        currentDays = days;
                                    }
                                    else
                                    {
                                        currentDays = 0;
                                    }
                                    if (remainingHours > 0)
                                    {
                                        currentHours = remainingHours;
                                    }
                                    else
                                    {
                                        currentHours = 0;
                                    }

                                }
                                changelog.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                                changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                changelog.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceChangeLog").ToString(), currentDays, currentHours);
                                bool isAdded = HrmService.AddEmployeeChangelogs_V2500(changelog);
                                if (GeosApplication.Instance.CompensationLeave == null)
                                {
                                    GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                                }

                                GeosApplication.Instance.CompensationLeave.Comments = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompensatoryAutomaticallyComment").ToString());
                                EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                                temp.IdEmployee = NewEmployeeAttendance.IdEmployee;
                                temp.Year = (Int32)SelectedPeriod;
                                temp.IdLeave = 241;
                                temp.RegularHoursCount = 0;
                                temp.BacklogHoursCount = 0;

                                bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, dayshours);//[GEOS2-6654][rdixit][20.01.2025]
                            }

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
                        AccountingDate = (DateTime)StartDate,
                        //[GEOS2-3095]
                        Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                        ModificationDate = GeosApplication.Instance.ServerDateTime,
                        Remark = Remarks,
                        FileName = AttendanceFileName,
                        Latitude = Latitude,
                        Langitude = Longitude,
                        IdStatus = GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue,
                        AttendanceStatus = GeosApplication.Instance.AttendanceStatusList.Where(i => i.IdLookupValue == GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue).FirstOrDefault()
                    };
                    // UpdateEmployeeAttendance.Employee.CompanyShift = SelectedCompanyShift;
                    UpdateEmployeeAttendance.Employee.CompanyShift = SelectedEmployeeShift.CompanyShift;

                    IsSave = true;
                    //Result = HrmService.UpdateEmployeeAttendance_V2036(UpdateEmployeeAttendance);
                    // Result = HrmService.UpdateEmployeeAttendance_V2170(UpdateEmployeeAttendance);
                    //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
                    #region //Changelog of Employee Attendance
                    //[shweta.thube][GEOS2-5511]

                    UpdateEmployeeAttendance.ChangeLogList = new List<EmployeeChangelog>();

                    if (OldEmployeeAttendanceDetails.IdStatus != GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].IdLookupValue)
                    {
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceStatusupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.Status, GeosApplication.Instance.AttendanceStatusList[SelectedForAttendanceStatus].Value, ((DateTime)StartDate).ToShortDateString())
                        });                        

                    }
                    if (OldEmployeeAttendanceDetails.IdEmployee != EmployeeListFinal[SelectedIndexForEmployee].IdEmployee)
                    {
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        { 
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceEmployeeupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.Employee, EmployeeListFinal[SelectedIndexForEmployee].FullName, ((DateTime)StartDate).ToShortDateString())
                        });
                    }
                    if (OldEmployeeAttendanceDetails.IdCompanyShift != EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.IdCompanyShift)
                    {
                        //string oldType = EmployeeShiftList.FirstOrDefault(i =>i.IdCompanyShift == OldEmployeeAttendanceDetails.IdCompanyWork).CompanyShift.DisplayName;
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceShiftupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.CompanyShift.DisplayName, EmployeeShiftList[SelectedIndexForCompanyShift].CompanyShift.DisplayName, ((DateTime)StartDate).ToShortDateString())
                        });

                    }
                    if (OldEmployeeAttendanceDetails.StartDate != (DateTime)StartDate)
                    {
                        if(OldEmployeeAttendanceDetails.StartDate.ToShortDateString() != ((DateTime)StartDate).ToShortDateString())
                        {
                            UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceStartTimeupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.StartDate, (DateTime)StartDate, OldEmployeeAttendanceDetails.StartDate.ToShortDateString())
                            });
                        }
                        else
                        {
                            UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceStartTimeupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.StartDate, (DateTime)StartDate, ((DateTime)StartDate).ToShortDateString())
                            });
                        }
                        
                    }                   
                    if (OldEmployeeAttendanceDetails.EndDate != (DateTime)EndDate)
                    {
                        if(OldEmployeeAttendanceDetails.EndDate.ToShortDateString() != ((DateTime)EndDate).ToShortDateString())
                        {
                            UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceEndTimeupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.EndDate, (DateTime)EndDate, OldEmployeeAttendanceDetails.EndDate.ToShortDateString())
                            });
                        }
                        else
                        {
                            UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceEndTimeupdatedChangeLog").ToString(), OldEmployeeAttendanceDetails.EndDate, (DateTime)EndDate, ((DateTime)EndDate).ToShortDateString())
                            });
                        }                            
                    }                    
                    if (OldEmployeeAttendanceDetails.IdCompanyWork != GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue)
                    {
                        string oldType = GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(i => i.IdLookupValue == OldEmployeeAttendanceDetails.IdCompanyWork).Value;
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceTypeupdatedChangeLog").ToString(), oldType, GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].Value, ((DateTime)StartDate).ToShortDateString())
                        });

                    }
                    if (OldEmployeeAttendanceDetails.Remark != Remarks)
                    {
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime =GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceRemarkupdatedChangeLog").ToString(), string.IsNullOrEmpty(OldEmployeeAttendanceDetails.Remark) ? "None" :OldEmployeeAttendanceDetails.Remark, Remarks==null ? "None":Remarks, ((DateTime)StartDate).ToShortDateString())
                        });
                    }
                    if (OldEmployeeAttendanceDetails.FileName != AttendanceFileName)
                    {
                        UpdateEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = UpdateEmployeeAttendance.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceAttachmentupdatedChangeLog").ToString(), string.IsNullOrEmpty(OldEmployeeAttendanceDetails.FileName) ? "None":OldEmployeeAttendanceDetails.FileName, AttendanceFileName==null ? "None" : AttendanceFileName, ((DateTime)StartDate).ToShortDateString())
                        });
                    }
                    #endregion
                    //Result = HrmService.UpdateEmployeeAttendance_V2520(UpdateEmployeeAttendance);
                    //[shweta.thube][GEOS2-5511]
                    Result = HrmService.UpdateEmployeeAttendance_V2620(UpdateEmployeeAttendance);
                    if (GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue == 305) 
                    {

                        EmployeeChangelog changelog = new EmployeeChangelog();
                        int currentDays = 0;
                        double currentHours = 0;
                        double dayshours = 0;
                        double totalHours = 0;
                        TimeSpan timedifference = UpdateEmployeeAttendance.EndDate - UpdateEmployeeAttendance.StartDate;
                        if (timedifference != null)
                        {
                            dayshours = timedifference.TotalHours;
                            int days = (int)(dayshours / 8);
                            double remainingHours = dayshours % 8;
                            if (TotalPreviousHours > dayshours)
                            {
                                totalHours = (dayshours - TotalPreviousHours);
                            }
                            else
                            {
                                totalHours = (dayshours - TotalPreviousHours);
                            }

                            if (days > 0)
                            {
                                currentDays = days;
                            }
                            else
                            {
                                currentDays = 0;
                            }
                            if (remainingHours > 0)
                            {
                                currentHours = remainingHours;
                            }
                            else
                            {
                                currentHours = 0;
                            }
                        }

                        changelog.IdEmployee = UpdateEmployeeAttendance.IdEmployee;
                        changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                        changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        changelog.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompansationAttendanceupdatedChangeLog").ToString(), PreviousDays, PreviousHours, currentDays, currentHours);
                        if (GeosApplication.Instance.CompensationLeave == null)
                        {
                            GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                        }
                        GeosApplication.Instance.CompensationLeave.Comments = string.Format(System.Windows.Application.Current.FindResource("EmployeeCompensatoryAutomaticallyComment").ToString());
                        bool isAdded = HrmService.AddEmployeeChangelogs_V2500(changelog);
                        EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                        temp.IdEmployee = UpdateEmployeeAttendance.IdEmployee;
                        temp.Year = (Int32)SelectedPeriod;
                        temp.IdLeave = 241;
                        temp.RegularHoursCount = 0;
                        temp.BacklogHoursCount = 0;
                        if (PreviousIdType == GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue)
                        {
                            bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, totalHours);
                        }
                        else
                        {
                            bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, dayshours);
                        }
                    }
                    #region [rdixit][[GEOS2-5336]][19.04.2024]
                    else if (GeosApplication.Instance.AttendanceTypeList[OriginalIndexForAttendanceType].IdLookupValue == 305)
                    {
                        try
                        {
                            double currentHours = CalculateTotalHours(StartTimeOriginal, EndTimeOriginal);
                            bool isUpdated = HrmService.DeleteEmployeeCompensatoryHours_V2501(UpdateEmployeeAttendance.IdEmployee, SelectedPeriod, currentHours);
                        }
                        catch (Exception ex)
                        { }
                    }
                    #endregion
                    UpdateEmployeeAttendance.Employee.TotalWorkedHours = (UpdateEmployeeAttendance.EndDate - UpdateEmployeeAttendance.StartDate).ToString(@"hh\:mm");

                    #region GEOS2-4019
                    if (AttendanceFileName == null || OldEmployeeAttendanceDetails.FileName != AttendanceFileName)
                    {
                        HrmService.DeleteEmployeeAttendanceAttachment(EmployeeListFinal[SelectedIndexForEmployee].EmployeeCode, UpdateEmployeeAttendance.IdEmployeeAttendance, OldEmployeeAttendanceDetails.FileName);
                    }
                    if (AttendanceFileInBytes != null)
                    {
                        HrmService.SaveEmployeeAttendanceAttachment(EmployeeListFinal[SelectedIndexForEmployee].EmployeeCode, UpdateEmployeeAttendance.IdEmployeeAttendance, AttendanceFileName, AttendanceFileInBytes);
                    }
                    #endregion
                    CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdateEmployeeAttendance.IdCompanyShift));



                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method AddAttendanceInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                //[rdixit][02.09.2024][GEOS2-5050]
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendanceInformation() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAttendanceInformation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDateFocusAction(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateFocusAction()...", category: Category.Info, priority: Priority.Low);
                flagEditDate = true;
                GeosApplication.Instance.Logger.Log("Method OnDateFocusAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateFocusAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                //if (flag)
                //{
                //    StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                //    EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                //}
                if (IsNew)
                {
                    if (flag && !IsAcceptButton)
                    {
                        StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                        EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                    }

                }
                else
                {
                    if (flagEditDate && !IsAcceptButton)
                    {
                        StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                        EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                    }

                }
                CheckDateTimeValidation();
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void loadShiftTime()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method loadShiftTime()...", category: Category.Info, priority: Priority.Low);
                StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                GeosApplication.Instance.Logger.Log("Method loadShiftTime()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method loadShiftTime()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                    if (SelectedEmployeeShift != null && SelectedEmployeeShift.CompanyShift != null &&
                          SelectedEmployeeShift.CompanyShift.IsNightShift == 0)                          //if (SelectedEmployeeShift.CompanyShift.IsNightShift == 0)//if (SelectedCompanyShift.IsNightShift == 0)
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
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                        if (SelectedEmployeeShift != null && SelectedEmployeeShift.CompanyShift != null &&
                           SelectedEmployeeShift.CompanyShift.IsNightShift == 0)                                 //if (SelectedEmployeeShift.CompanyShift.IsNightShift == 0) //if (SelectedCompanyShift.IsNightShift == 0)
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
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002][SP-65][skale][20-06-2019][GEOS2-1588]Add a new filter criterion to Leaves and Attendance Window in GHRM
        /// </summary>
        public void Init(ObservableCollection<EmployeeAttendance> employeeAttendanceList,
            object selectedEmployee, DateTime selectedStartDate, DateTime selectedEndDate,
            string workingPlantId,
            ObservableCollection<EmployeeLeave> employeeLeavesList,
            ObservableCollection<Employee> employeeListFinal)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                var plantOwnersIdsJoined = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().AsEnumerable().Select(i => i.IdCompany));
                this.IsNew = true;
                this.WindowHeader = System.Windows.Application.Current.FindResource("AddNewAttendance").ToString();
                this.IsEditInit = false;
                this.ExistEmployeeAttendanceList = employeeAttendanceList;
                this.EmployeeLeaves = employeeLeavesList;
                this.WorkingPlantId = workingPlantId;

                //if(employeeListFinal == null)
                //{
                //    employeeListFinal = new ObservableCollection<Employee>();
                //    foreach (var item in SelectedPlantList)
                //    {

                //        ObservableCollection<Employee> tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2110(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] Added
                //        employeeListFinal.AddRange(tempEmployeeListFinal);
                //    }

                //}
                //else
                //{
                //    EmployeeListFinal = employeeListFinal;
                //}

                //EmployeeListFinal = new ObservableCollection<Employee>();
                //foreach (var item in SelectedPlantList)
                //{

                //    //ObservableCollection<Employee> tempEmployeeListFinal = new ObservableCollection<Employee>(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2044(item.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));//[002] Added
                //    EmployeeListFinal.AddRange(tempEmployeeListFinal);
                //}

                if (employeeListFinal == null)
                {
                    EmployeeListFinal = new ObservableCollection<Employee>();
                    EmployeeListFinal.AddRange(HrmService.GetAllEmployeesForAttendanceByIdCompany_V2110(
                        plantOwnersIdsJoined, SelectedPeriod,
                        HrmCommon.Instance.ActiveEmployee.Organization,
                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments,
                        HrmCommon.Instance.IdUserPermission).OrderBy(x => x.FullName));
                }
                else
                {
                    this.EmployeeListFinal = employeeListFinal;

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


                IsVisible = Visibility.Hidden;


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to edit Employee Attendance Information
        /// [001] Sprint46 HRM Take values from lookup values instead of the existing tables by Mayuri
        /// [002] [avpawar][2020-07-24][GEOS2-2432] To solve the error message when try edit the attendance shift.
        /// </summary>
        public void EditInit(EmployeeAttendance SelectedEmployeeAttendance,
            ObservableCollection<EmployeeAttendance> employeeAttendanceList,
            ObservableCollection<Employee> currentEmployeeListFinal,
            ObservableCollection<EmployeeLeave> currentEmployeeLeaves,
            string currentWorkingPlantId)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                IsModifierVisibility = Visibility.Collapsed;
                IsCreatorVisibility = Visibility.Collapsed;
                IsMapVisibility = Visibility.Collapsed;
                this.IsEditInit = true;
                this.IsSplitVisible = true;
                this.IsNew = false;
                OldEmployeeAttendanceDetails = new EmployeeAttendance(); ;
                OldEmployeeAttendanceDetails = (EmployeeAttendance)SelectedEmployeeAttendance.Clone();
                this.WindowHeader = System.Windows.Application.Current.FindResource("EditAttendance").ToString();

                this.EmployeeLeaves = currentEmployeeLeaves;
                if (currentEmployeeListFinal == null)
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
                    EmployeeListFinal = currentEmployeeListFinal;
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

                SelectedForAttendanceStatus = GeosApplication.Instance.AttendanceStatusList.ToList().FindIndex(x => x.IdLookupValue == SelectedEmployeeAttendance.IdStatus);
               
                SelectedAttendanceStatus = GeosApplication.Instance.AttendanceStatusList.FirstOrDefault(x => x.IdLookupValue == SelectedEmployeeAttendance.IdStatus);
                PreviousSelectedForAttendanceStatus = SelectedAttendanceStatus;

                StartDate = SelectedEmployeeAttendance.StartDate;
                EndDate = SelectedEmployeeAttendance.EndDate;
                #region [rdixit][[GEOS2-5336]][19.04.2024]
                EndTimeOriginal = SelectedEmployeeAttendance.EndDate;
                StartTimeOriginal = SelectedEmployeeAttendance.StartDate;
                Latitude = SelectedEmployeeAttendance.Latitude;
                Longitude = SelectedEmployeeAttendance.Langitude;

                try
                {
                    if (Latitude != null && Longitude != null)
                    {
                        MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);
                        IsMapVisibility = Visibility.Visible;
                    }
                    if (Latitude != null && Longitude != null)
                    {
                        // https://www.google.com/maps/search/?api=1&query=41.3291493616998+ -8.65912581793964
                        //MapLatitudeAndLongitudeURL = $"https://www.google.com/maps/search/?api=1&query={Latitude.Value}+ {Longitude.Value}&hl=en&output=embed";
                        MapLatitudeAndLongitudeURL = $"https://www.google.com/maps/search/?api=1&query={Latitude.Value.ToString(CultureInfo.GetCultureInfo("en-GB"))}+{Longitude.Value.ToString(CultureInfo.GetCultureInfo("en-GB"))}"; //[nsatpute][22-05-2025][GEOS2-8143]
                        //MapLatitudeAndLongitudeURL = $"http://maps.google.com/maps?z=11&t=k&q={Latitude.Value}+ {Longitude.Value}";
                        IsFlagTrue = false;
                        //IsMapVisibility = Visibility.Visible;
                    }
                    else
                    {
                        IsMapVisibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }


                if (string.IsNullOrEmpty(SelectedEmployeeAttendance.Location))
                {
                    IsLocationAvailable = false;
                    LocationValue = "(Not Available)";
                    IsFlagTrue = true;
                    IsMapVisibility = Visibility.Collapsed;
                }
                else
                {
                    IsLocationAvailable = true;
                    LocationValue = SelectedEmployeeAttendance.Location;
                }
                OriginalIndexForAttendanceType = SelectedIndexForAttendanceType;
                #endregion
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

                if (SelectedEmployeeAttendance.IdEmployeeAttendance != 0)
                {
                    #region GEOS2-6447
                    try
                    {
                        //HrmService = new HrmServiceController("localhost:6699");
                        EmployeeAttendance = HrmService.GetEmployeeAttendanceByIdEmployeeAttendance_V2580(SelectedEmployeeAttendance.IdEmployeeAttendance);
                        Creator = EmployeeAttendance.Creator;
                        if (!string.IsNullOrEmpty(Creator.ToString()))
                        {
                            IsCreatorVisibility = Visibility.Visible;
                        }
                        CreationDate = Convert.ToDateTime(EmployeeAttendance.CreationDate.ToString());
                        CreatorName = EmployeeAttendance.CreatorName;
                        IdReporterSource = EmployeeAttendance.IdReporterSource;
                        if (IdReporterSource == 0)
                        {
                            if (SelectedEmployeeAttendance.IsMobileApiAttendance == false)
                            {
                                IdReporterSource = 1747;
                            }
                        }
                        if (EmployeeAttendance.Modifier != null)
                        {
                            Modifier = EmployeeAttendance.Modifier;
                            ModificationDate = Convert.ToDateTime(EmployeeAttendance.ModificationDate.ToString());
                            ModifierName = EmployeeAttendance.ModifierName;
                            IsModifierVisibility = Visibility.Visible;
                        }
                        if (!string.IsNullOrEmpty(EmployeeAttendance.CreatorCode))
                            CreatorProfilePhotoBytes = GetEmployeesImage_V2520(EmployeeAttendance.CreatorCode);
                        if (!string.IsNullOrEmpty(EmployeeAttendance.ModifierCode))
                            ModifierProfilePhotoBytes = GetEmployeesImage_V2520(EmployeeAttendance.ModifierCode);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion
                }
                else
                {
                    #region GEOS2-6447
                    try
                    {

                        if (SelectedEmployeeAttendance.IdCreator != null)
                        {
                            Creator = SelectedEmployeeAttendance.IdCreator;
                        }
                        else
                        {
                            Creator = SelectedEmployeeAttendance.Creator;
                            if (SelectedEmployeeAttendance.Creator > 0)
                            {
                                SelectedEmployeeAttendance.IdCreator = Creator;
                            }
                        }
                        if (!string.IsNullOrEmpty(Creator.ToString()))
                        {
                            IsCreatorVisibility = Visibility.Visible;
                        }
                        CreationDate = Convert.ToDateTime(SelectedEmployeeAttendance.CreationDate.ToString());
                        //FormattedDate = Convert.ToDateTime(CreationDate).ToString("d", CultureInfo.CurrentCulture);
                        CreatorName = SelectedEmployeeAttendance.CreatorName;
                        try
                        {
                            if (string.IsNullOrEmpty(CreatorName))
                            {
                                if (GeosApplication.Instance.ActiveUser.IdUser == SelectedEmployeeAttendance.Creator)
                                {
                                    CreatorName = GeosApplication.Instance.ActiveUser.FullName.ToString();
                                    if (string.IsNullOrEmpty(SelectedEmployeeAttendance.CreatorCode))
                                        SelectedEmployeeAttendance.CreatorCode = employeeAttendanceList.FirstOrDefault(w => w.Creator == SelectedEmployeeAttendance.Creator && !string.IsNullOrEmpty(w.CreatorCode))?.CreatorCode;
                                }
                            }
                            //CreatorName = SelectedEmployeeAttendance.CreatorName + Environment.NewLine + CreationDate;
                            IdReporterSource = SelectedEmployeeAttendance.IdReporterSource;
                            if (IdReporterSource == 0)
                            {
                                if (SelectedEmployeeAttendance.IsMobileApiAttendance == false)
                                {
                                    IdReporterSource = 1747;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        }
                        if (SelectedEmployeeAttendance.Modifier != null)
                        {
                            Modifier = SelectedEmployeeAttendance.Modifier;
                            ModificationDate = Convert.ToDateTime(SelectedEmployeeAttendance.ModificationDate.ToString());
                            ModifierName = SelectedEmployeeAttendance.ModifierName;
                            IsModifierVisibility = Visibility.Visible;
                        }
                        if (!string.IsNullOrEmpty(SelectedEmployeeAttendance.CreatorCode))
                            CreatorProfilePhotoBytes = GetEmployeesImage_V2520(SelectedEmployeeAttendance.CreatorCode);
                        if (!string.IsNullOrEmpty(SelectedEmployeeAttendance.ModifierCode))
                            ModifierProfilePhotoBytes = GetEmployeesImage_V2520(SelectedEmployeeAttendance.ModifierCode);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion
                }

                #region GEOS2-5336 Sudhir.jangra
                TimeSpan timediff = Convert.ToDateTime(SelectedEmployeeAttendance.EndDate.ToString()) - Convert.ToDateTime(SelectedEmployeeAttendance.StartDate.ToString());
                if (timediff != null)
                {
                    double dayshours = timediff.TotalHours;
                    TotalPreviousHours = timediff.TotalHours;
                    int days = (int)(dayshours / 8);
                    double remainingHours = dayshours % 8;

                    if (days > 0)
                    {
                        PreviousDays = days;
                    }
                    else
                    {
                        PreviousDays = 0;
                    }
                    if (remainingHours > 0)
                    {
                        PreviousHours = remainingHours;
                    }
                    else
                    {
                        PreviousHours = 0;
                    }
                }

                PreviousIdType = GeosApplication.Instance.AttendanceTypeList[SelectedIndexForAttendanceType].IdLookupValue;
                #endregion

                IdEmployeeAttendance = SelectedEmployeeAttendance.IdEmployeeAttendance;
                ExistEmployeeAttendanceList = employeeAttendanceList;

                // CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployeeAttendance.IdEmployee));
                EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee_V2370(SelectedEmployeeAttendance.IdEmployee));
                if (string.IsNullOrEmpty(SelectedEmployeeAttendance.Location))
                {
                    IsLocationAvailable = false;
                    LocationValue = "(Not Available)";
                }
                else
                {
                    IsLocationAvailable = true;
                    LocationValue = SelectedEmployeeAttendance.Location;
                }

                Remarks = SelectedEmployeeAttendance.Remark;//[Sudhir.Jangra][GEOS2-4018][08/08/2023]
                AttendanceFileName = SelectedEmployeeAttendance.FileName;
                AttachmentList = new ObservableCollection<Attachment>();
                if (!string.IsNullOrEmpty(SelectedEmployeeAttendance.FileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = SelectedEmployeeAttendance.FileName;
                    attachment.IsDeleted = false;
                    AttachmentList.Add(attachment);
                    AttachedFile = AttachmentList[0];
                }
                if (AttachmentList.Count > 0)
                {
                    IsVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-4019][08/08/2023]
                }
                else
                {
                    IsVisible = Visibility.Hidden;//[Sudhir.Jangra][GEOS2-4019][08/08/2023]
                }


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

                TimeSpan? ts = EndDate - StartDate;
                if (ts.HasValue && ts.Value.TotalSeconds == 1)
                {
                    EndDate = null;
                    EndTime = null;
                    IsPartialTime = true;
                    EnableEndDateTime = true;
                }
                IsPartialTimeEnable = false;
                PartialTimeInformationVisible = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>
        public void InitReadOnly(EmployeeAttendance SelectedEmployeeAttendance, string currentWorkingPlantId)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                this.IsNew = false;
                this.WindowHeader = System.Windows.Application.Current.FindResource("EditAttendance").ToString();
                this.IsEditInit = true;
                this.WorkingPlantId = currentWorkingPlantId;
                OldEmployeeAttendanceDetails = new EmployeeAttendance();
                OldEmployeeAttendanceDetails = (EmployeeAttendance)SelectedEmployeeAttendance.Clone();

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

                #region GEOS2-4018
                Remarks = SelectedEmployeeAttendance.Remark;
                AttendanceFileName = SelectedEmployeeAttendance.FileName;

                Latitude = SelectedEmployeeAttendance.Latitude;
                Longitude = SelectedEmployeeAttendance.Langitude;

                if (Latitude != null && Longitude != null)
                    MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);

                if (string.IsNullOrEmpty(SelectedEmployeeAttendance.Location))
                {
                    IsLocationAvailable = false;
                    LocationValue = "(Not Available)";
                }
                else
                {
                    IsLocationAvailable = true;
                    LocationValue = SelectedEmployeeAttendance.Location;
                }

                AttachmentList = new ObservableCollection<Attachment>();
                if (!string.IsNullOrEmpty(SelectedEmployeeAttendance.FileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = SelectedEmployeeAttendance.FileName;
                    attachment.IsDeleted = false;
                    // attachment.FileByte = LeaveFileInBytes;
                    AttachmentList.Add(attachment);
                    AttachedFile = AttachmentList[0];
                }
                if (AttachmentList.Count > 0)
                    IsVisible = Visibility.Visible;
                else
                    IsVisible = Visibility.Hidden;
                #endregion

                EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee_V2370(SelectedEmployeeAttendance.IdEmployee));
                if (EmployeeShiftList.Any(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift))
                    SelectedIndexForCompanyShift = EmployeeShiftList.IndexOf(EmployeeShiftList.FirstOrDefault(x => x.CompanyShift.IdCompanyShift == SelectedEmployeeAttendance.IdCompanyShift));
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeWorkType()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }


        //[001][05-12-2020][GEOS2-2780]The shift don't appear when edit the attendance record, but we have shift same shift in attendance, employees and shift table.
        private void SelectedIndexChangedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                //[001]
                if ((SelectedIndexForEmployee == 0 && IsEditInit == true) || SelectedIndexForEmployee > 0)
                {
                    if (!IsEditInit)
                    {
                        var SelectedEmployee = EmployeeListFinal[SelectedIndexForEmployee];
                        SelectedIndexForCompanyShift = -1;

                        //CompanyShifts = new ObservableCollection<CompanyShift>(HrmService.GetEmployeeRelatedCompanyShifts_V2035(SelectedEmployee.IdEmployee));
                        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee_V2370(SelectedEmployee.IdEmployee));
                        if (EmployeeShiftList.Count > 1)
                        {
                            flag = true;
                            SelectedIndexForCompanyShift = 1;
                        }

                    }
                    else if (EmployeeChange)
                    {
                        var SelectedEmployee = EmployeeListFinal[SelectedIndexForEmployee];
                        SelectedIndexForCompanyShift = -1;
                        EmployeeShiftList = new ObservableCollection<EmployeeShift>(HrmService.GetEmployeeShiftsByIdEmployee_V2370(SelectedEmployee.IdEmployee));
                        if (EmployeeShiftList.Count > 1)
                        {
                            SelectedIndexForCompanyShift = 1;
                            //  flag = true;
                        }

                    }
                    EmployeeChange = true;
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
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedIndexChangedForCompanyShiftCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedForCompanyShiftCommandAction()...", category: Category.Info, priority: Priority.Low);
                //SelectedCompanyShift = CompanyShifts[SelectedIndexForCompanyShift];
                if (SelectedIndexForCompanyShift >= 0)
                {
                    SelectedEmployeeShift = EmployeeShiftList[SelectedIndexForCompanyShift];
                    if (IsNew)
                    {
                        if (flag)
                        {
                            StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                            EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);
                        }

                        flag = true;

                    }
                    //[rdixit][GEOS2-4117][24.01.2023]  Commented previous code No need to change start and End Time as per shift. needs to keep as it is.
                    //else 
                    //{
                    //    if (flagEdit)
                    //    {

                    //        StartTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], true);
                    //        EndTime = GetShiftStartTime((int)StartDate.Value.Date.DayOfWeek, EmployeeShiftList[SelectedIndexForCompanyShift], false);

                    //    }
                    //    flagEdit = true;
                    //}
                    if (isValidation)
                        CheckDateTimeValidation();
                    isValidation = true;
                }
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedForCompanyShiftCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedIndexChangedForCompanyShiftCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetUserPermission()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()...", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("Method SetUserPermission()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetUserPermission()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
        private void SplitAttendanceInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow ...", category: Category.Info, priority: Priority.Low);
                EmployeeAttendance selectedEmpAttendance = new EmployeeAttendance();
                SelectedViewIndex = 1;
                IsPartialTimeEnable = true;
                IsSplitVisible = false;
                AddNewTaskbtnVisibility = false;
                STime = StartTime.Value.TimeOfDay;
                ETime = EndTime.Value.TimeOfDay;
                #region
                if (StartTime.Value.Date == EndTime.Value.Date)
                {
                    DateTime? _StartTime = null;
                    DateTime? _EndTime = null;
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
                        EmployeeShiftList = EmployeeShiftList,
                        TaskSelectedEmployeeShift = SelectedEmployeeShift,
                        SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                        IsEnabledShift = false,
                        TaskStartDate = _StartTime,
                        TaskStartTime = _StartTime,
                        TaskEndDate = _EndTime,
                        TaskEndTime = _EndTime,
                        SelectedIndexForAttendanceType = SelectedIndexForAttendanceType,
                        TaskAccountingDate = AccountingDate
                    });

                    _IndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.IndexOf(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == 188));

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
                            EmployeeShiftList = EmployeeShiftList,
                            TaskSelectedEmployeeShift = SelectedEmployeeShift,
                            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                            IsEnabledShift = false,
                            TaskStartDate = EndDate,
                            TaskStartTime = _EndTime,
                            TaskEndDate = EndDate,
                            TaskEndTime = EndTime,
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
                            EmployeeShiftList = EmployeeShiftList,
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
                }
                #endregion

                #region If more than one day
                else
                {
                    STime = StartTime.Value.TimeOfDay;
                    ETime = EndTime.Value.TimeOfDay;
                    DateTime shiftStartTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, SelectedEmployeeShift, true);
                    DateTime shiftEndTime = GetShiftStartTime((int)StartTime.Value.Date.DayOfWeek, SelectedEmployeeShift, false);
                    if ((shiftEndTime - shiftStartTime) > new TimeSpan(8, 0, 0))
                    {
                        shiftEndTime = new DateTime(shiftStartTime.Year, shiftStartTime.Month, shiftStartTime.Day, shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second);
                    }

                    #region Time before ShiftStart
                    if (StartTime < shiftStartTime)
                    {
                        Tasks.Add(new Task()
                        {
                            Name = "Header",
                            Header = "Attendance3",
                            IsComplete = true,
                            IsNew = true,
                            IsEnabaledEmployee = false,
                            TaskEmployeeListFinal = EmployeeListFinal,
                            SelectedIndexForEmployee = SelectedIndexForEmployee,
                            EmployeeShiftList = EmployeeShiftList,
                            TaskSelectedEmployeeShift = SelectedEmployeeShift,
                            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                            IsEnabledShift = false,
                            TaskStartDate = StartTime,
                            TaskStartTime = StartTime,
                            TaskEndDate = shiftStartTime,
                            TaskEndTime = shiftStartTime,
                            SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.IndexOf(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == 188)),
                            TaskAccountingDate = AccountingDate
                        });
                    }
                    #endregion

                    #region ShiftTime
                    if (EndTime >= shiftStartTime && EndTime <= shiftEndTime)
                    {
                        Tasks.Add(new Task()
                        {
                            Name = "Header",
                            Header = "Attendance1",
                            IsComplete = false,
                            IsNew = false,
                            TaskEmployeeListFinal = EmployeeListFinal,
                            IsEnabaledEmployee = false,
                            SelectedIndexForEmployee = SelectedIndexForEmployee,
                            EmployeeShiftList = EmployeeShiftList,
                            TaskSelectedEmployeeShift = SelectedEmployeeShift,
                            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                            IsEnabledShift = false,
                            TaskStartDate = shiftStartTime,
                            TaskStartTime = shiftStartTime,
                            TaskEndDate = EndTime,
                            TaskEndTime = EndTime,
                            SelectedIndexForAttendanceType = SelectedIndexForAttendanceType,
                            TaskAccountingDate = AccountingDate
                        });
                    }
                    else if (EndTime > shiftStartTime && EndTime > shiftEndTime)
                    {
                        Tasks.Add(new Task()
                        {
                            Name = "Header",
                            Header = "Attendance1",
                            IsComplete = false,
                            IsNew = false,
                            TaskEmployeeListFinal = EmployeeListFinal,
                            IsEnabaledEmployee = false,
                            SelectedIndexForEmployee = SelectedIndexForEmployee,
                            EmployeeShiftList = EmployeeShiftList,
                            TaskSelectedEmployeeShift = SelectedEmployeeShift,
                            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                            IsEnabledShift = false,
                            TaskStartDate = shiftStartTime,
                            TaskStartTime = shiftStartTime,
                            TaskEndDate = shiftEndTime,
                            TaskEndTime = shiftEndTime,
                            SelectedIndexForAttendanceType = SelectedIndexForAttendanceType,
                            TaskAccountingDate = AccountingDate
                        });
                    }
                    #endregion

                    #region Time After ShiftEnd
                    // Calculate extra hours after shift
                    if (EndTime > shiftEndTime)
                    {
                        Tasks.Add(new Task()
                        {
                            Name = "Header",
                            Header = "Attendance2",
                            IsComplete = true,
                            IsNew = true,
                            IsEnabaledEmployee = false,
                            TaskEmployeeListFinal = EmployeeListFinal,
                            SelectedIndexForEmployee = SelectedIndexForEmployee,
                            EmployeeShiftList = EmployeeShiftList,
                            TaskSelectedEmployeeShift = SelectedEmployeeShift,
                            SelectedIndexForCompanyShift = SelectedIndexForCompanyShift,
                            IsEnabledShift = false,
                            TaskStartDate = shiftEndTime,
                            TaskStartTime = shiftEndTime,
                            TaskEndDate = EndTime,
                            TaskEndTime = EndTime,
                            SelectedIndexForAttendanceType = GeosApplication.Instance.AttendanceTypeList.IndexOf(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == 188)),
                            TaskAccountingDate = AccountingDate
                        });
                    }
                    #endregion
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method SplitLeadOfferViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SplitLeadOfferViewWindowShow()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }


        public EmployeeAttendance NewSplitEmployeeAttendance;
        public EmployeeAttendance UpdatesplitEmployeeAttendance;
        private void SaveAttendanceWithSplit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveAttendanceWithSplit()...", category: Category.Info, priority: Priority.Low);

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
                            AccountingDate = item.TaskAccountingDate,
                        };
                        NewSplitEmployeeAttendance.IsManual = 1;
                        //[GEOS2-3095]
                        NewSplitEmployeeAttendance.Creator = GeosApplication.Instance.ActiveUser.IdUser;
                        NewSplitEmployeeAttendance.CreationDate = GeosApplication.Instance.ServerDateTime;
                        NewSplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedEmployeeShift.CompanyShift;
                        NewSplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == NewSplitEmployeeAttendance.IdCompanyWork));
                        //EmployeeAttendance addEmployeeAttendance = HrmService.AddEmployeeAttendance_V2036(NewSplitEmployeeAttendance);
                        //EmployeeAttendance addEmployeeAttendance = HrmService.AddEmployeeAttendance_V2060(NewSplitEmployeeAttendance);
                        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
                        EmployeeAttendance addEmployeeAttendance = HrmService.AddEmployeeAttendance_V2410(NewSplitEmployeeAttendance);
                        CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(NewSplitEmployeeAttendance.IdCompanyShift));
                        NewEmployeeAttendanceList.Add(addEmployeeAttendance);
                        ApplyCompensatoryOff(item, NewSplitEmployeeAttendance); //[GEOS2-6904][rdixit][11.03.2025]
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
                            AccountingDate = item.TaskAccountingDate,
                            //[GEOS2-3095]
                            Modifier = GeosApplication.Instance.ActiveUser.IdUser,
                            ModificationDate = GeosApplication.Instance.ServerDateTime
                        };
                        //[Sudhir.Jangra][GEOS2-5336]
                        if (GeosApplication.Instance.CompensationLeave != null)
                        {
                            GeosApplication.Instance.CompensationLeave.AccountingDate = UpdatesplitEmployeeAttendance.AccountingDate;
                            GeosApplication.Instance.CompensationLeave.IdEmployeeAnnualLeave = UpdatesplitEmployeeAttendance.IdEmployeeAttendance;
                        }
                        else
                        {
                            GeosApplication.Instance.CompensationLeave = new EmployeeAnnualAdditionalLeave();
                            GeosApplication.Instance.CompensationLeave.AccountingDate = UpdatesplitEmployeeAttendance.AccountingDate;
                            GeosApplication.Instance.CompensationLeave.IdEmployeeAnnualLeave = UpdatesplitEmployeeAttendance.IdEmployeeAttendance;
                        }
                        // [nsatpute][03-10-2024][GEOS2-6451]
                        if (ExistEmployeeAttendanceList.Any(a => a.IdEmployeeAttendance == IdEmployeeAttendance))
                        {
                            UpdatesplitEmployeeAttendance.IsManual = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == IdEmployeeAttendance).FirstOrDefault().IsManual;
                            UpdatesplitEmployeeAttendance.IsMobileApiAttendance = ExistEmployeeAttendanceList.Where(a => a.IdEmployeeAttendance == IdEmployeeAttendance).FirstOrDefault().IsMobileApiAttendance;
                        }
                        UpdatesplitEmployeeAttendance.Employee.CompanyShift = item.TaskSelectedEmployeeShift.CompanyShift;
                        UpdatesplitEmployeeAttendance.CompanyWork = GetCompanyWork(GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(x => x.IdLookupValue == UpdatesplitEmployeeAttendance.IdCompanyWork));

                        PreviousEmployeeSpliteAttendanceDetails = ExistEmployeeAttendanceList.FirstOrDefault(x => x.IdEmployeeAttendance == IdEmployeeAttendance);

                        UpdatesplitEmployeeAttendance.ChangeLogList = new List<EmployeeChangelog>();

                        if (PreviousEmployeeSpliteAttendanceDetails.StartDate != item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes))
                        {
                            if((PreviousEmployeeSpliteAttendanceDetails.StartDate).ToShortDateString() != (item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes)).ToShortDateString())
                            {
                                UpdatesplitEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = UpdatesplitEmployeeAttendance.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceStartTimeupdatedChangeLog").ToString(), PreviousEmployeeSpliteAttendanceDetails.StartDate, item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes), PreviousEmployeeSpliteAttendanceDetails.StartDate.ToShortDateString())
                                });
                            }
                            else
                            {
                                UpdatesplitEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = UpdatesplitEmployeeAttendance.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceStartTimeupdatedChangeLog").ToString(), PreviousEmployeeSpliteAttendanceDetails.StartDate, item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes), (item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes)).ToShortDateString())
                                });
                            }
                        }                        
                        if (PreviousEmployeeSpliteAttendanceDetails.EndDate != item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes))
                        {
                            if(PreviousEmployeeSpliteAttendanceDetails.EndDate.ToShortDateString() != (item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes)).ToShortDateString())
                            {
                                UpdatesplitEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = UpdatesplitEmployeeAttendance.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceEndTimeupdatedChangeLog").ToString(), PreviousEmployeeSpliteAttendanceDetails.EndDate, item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes), PreviousEmployeeSpliteAttendanceDetails.EndDate.ToShortDateString())
                                });
                            }
                            else
                            {
                                UpdatesplitEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = UpdatesplitEmployeeAttendance.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceEndTimeupdatedChangeLog").ToString(), PreviousEmployeeSpliteAttendanceDetails.EndDate, item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes), (item.TaskEndDate.Value.Date.AddHours(item.TaskEndTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskEndTime.Value.TimeOfDay.Minutes)).ToShortDateString())
                                });
                            }
                            
                        }
                        
                        if (PreviousEmployeeSpliteAttendanceDetails.IdCompanyWork != GeosApplication.Instance.AttendanceTypeList[item.SelectedIndexForAttendanceType].IdLookupValue)
                        {
                            string oldType = GeosApplication.Instance.AttendanceTypeList.FirstOrDefault(i => i.IdLookupValue == PreviousEmployeeSpliteAttendanceDetails.IdCompanyWork).Value;
                            UpdatesplitEmployeeAttendance.ChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = UpdatesplitEmployeeAttendance.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAttendanceTypeupdatedChangeLog").ToString(), oldType, GeosApplication.Instance.AttendanceTypeList[item.SelectedIndexForAttendanceType].Value, (item.TaskStartDate.Value.Date.AddHours(item.TaskStartTime.Value.TimeOfDay.Hours).AddMinutes(item.TaskStartTime.Value.TimeOfDay.Minutes)).ToShortDateString())
                            });

                        }

                        //Result = HrmService.UpdateEmployeeAttendance_V2036(UpdatesplitEmployeeAttendance);
                        // Result = HrmService.UpdateEmployeeAttendance_V2170(UpdatesplitEmployeeAttendance);
                        //[shweta.thube][GEOS2-5511]
                        Result = HrmService.UpdateEmployeeAttendance_V2620(UpdatesplitEmployeeAttendance);



                        UpdatesplitEmployeeAttendance.Employee.TotalWorkedHours = (UpdatesplitEmployeeAttendance.EndDate - UpdatesplitEmployeeAttendance.StartDate).ToString(@"hh\:mm");
                        CompanyShiftDetails = (HrmService.GetCompanyShiftDetailByIdCompanyShift(UpdatesplitEmployeeAttendance.IdCompanyShift));
                        NewEmployeeAttendanceList.Add(UpdatesplitEmployeeAttendance);
                        ApplyCompensatoryOff(item, UpdatesplitEmployeeAttendance); //[GEOS2-6904][rdixit][11.03.2025]
                    }
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SplitEmployeeAttendanceSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                IsSave = true;
                IsSplit = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method SaveAttendanceWithSplit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveAttendanceWithSplit() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                //[rdixit][02.09.2024][GEOS2-5050]
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
        }
        //[GEOS2-6904][rdixit][11.03.2025]
        void ApplyCompensatoryOff(Task item, EmployeeAttendance empAttendance)
        {
            try
            {
                var oldattendancetype = OldEmployeeAttendanceDetails.CompanyWork?.IdCompanyWork;
                var newattendancetype = GeosApplication.Instance.AttendanceTypeList[item.SelectedIndexForAttendanceType].IdLookupValue;
                if (oldattendancetype != 305 && newattendancetype == 305)
                {
                    EmployeeChangelog changelog = new EmployeeChangelog();
                    TimeSpan timedifference = empAttendance.EndDate - empAttendance.StartDate;

                    changelog.IdEmployee = empAttendance.IdEmployee;
                    changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                    changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    changelog.ChangeLogChange = string.Format(Application.Current.FindResource("EmployeeCompansationAttendanceChangeLog").ToString(),
                        (int)(timedifference.TotalHours / 8), (timedifference.TotalHours % 8));

                    bool isAdded = HrmService.AddEmployeeChangelogs_V2500(changelog);

                    EmployeeAnnualLeave temp = new EmployeeAnnualLeave();
                    temp.IdEmployee = empAttendance.IdEmployee;
                    temp.Year = (Int32)SelectedPeriod;
                    temp.IdLeave = 241;
                    temp.RegularHoursCount = 0;
                    temp.BacklogHoursCount = 0;
                    bool isAdd = HrmService.AddEmployeeAttendanceLeave_V2500(temp, timedifference.TotalHours);
                }
                else if (oldattendancetype == 305 && newattendancetype != 305)
                {
                    try
                    {
                        double currentHours = CalculateTotalHours(empAttendance.StartDate, empAttendance.EndDate);
                        bool isUpdated = HrmService.DeleteEmployeeCompensatoryHours_V2501(empAttendance.IdEmployee, SelectedPeriod, currentHours);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method CloseTask()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseTask()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseAttemdanceWindowForSplit(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseAttemdanceWindowForSplit()...", category: Category.Info, priority: Priority.Low);
                SelectedViewIndex = 0;
                IsSplitVisible = true;
                foreach (Task task in Tasks.ToList())
                {
                    Tasks.Remove(task);
                }
                GeosApplication.Instance.Logger.Log("Method CloseAttemdanceWindowForSplit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseAttemdanceWindowForSplit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void AddNewSplitTaskCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewSplitTaskCommandAction()...", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("Method AddNewSplitTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewSplitTaskCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }


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
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseTask ...", category: Category.Info, priority: Priority.Low);

                if (task.IsComplete == true)
                {
                    //[rdixit][GEOS2-3755][28.07.2022]
                    if (task.Header == "Attendance2")
                    {
                        Tasks.Where(i => i.Header == "Attendance1").FirstOrDefault().TaskEndTime = task.TaskEndTime;
                        Tasks.Where(i => i.Header == "Attendance1").FirstOrDefault().TaskEndDate = task.TaskEndDate;
                    }
                    if (task.Header == "Attendance3")
                    {
                        Tasks.Where(i => i.Header == "Attendance1").FirstOrDefault().TaskStartTime = task.TaskStartTime;
                        Tasks.Where(i => i.Header == "Attendance1").FirstOrDefault().TaskStartDate = task.TaskStartDate;
                    }

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
                        if (SelectedEmployeeShift.CompanyShift.SunStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.SunEndTime == new TimeSpan(0, 0, 0))
                        {
                            if (!(SelectedEmployeeShift.CompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.SunEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = SelectedEmployeeShift.CompanyShift.MonStartTime;
                                SunEndTime = SelectedEmployeeShift.CompanyShift.MonEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = SelectedEmployeeShift.CompanyShift.TueStartTime;
                                SunEndTime = SelectedEmployeeShift.CompanyShift.TueEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = SelectedEmployeeShift.CompanyShift.WedStartTime;
                                SunEndTime = SelectedEmployeeShift.CompanyShift.WedEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = SelectedEmployeeShift.CompanyShift.ThuStartTime;
                                SunEndTime = SelectedEmployeeShift.CompanyShift.ThuEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SunStartTime = SelectedEmployeeShift.CompanyShift.FriStartTime;
                                SunEndTime = SelectedEmployeeShift.CompanyShift.FriStartTime;
                            }
                            if (isStartTime)
                                return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                            else
                                return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);
                        }
                        else
                        {
                            if (isStartTime)
                                return StartTime.Value.Date.AddHours(SunStartTime.Hours).AddMinutes(SunStartTime.Minutes);
                            else
                                return EndTime.Value.Date.AddHours(SunEndTime.Hours).AddMinutes(SunEndTime.Minutes);
                        }


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
                        if (SelectedEmployeeShift.CompanyShift.SatStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.SatEndTime == new TimeSpan(0, 0, 0))
                        {
                            if (!(SelectedEmployeeShift.CompanyShift.MonStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.SunEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = SelectedEmployeeShift.CompanyShift.MonStartTime;
                                SatEndTime = SelectedEmployeeShift.CompanyShift.MonEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.TueStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.TueEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = SelectedEmployeeShift.CompanyShift.TueStartTime;
                                SatEndTime = SelectedEmployeeShift.CompanyShift.TueEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.WedStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.WedEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = SelectedEmployeeShift.CompanyShift.WedStartTime;
                                SatEndTime = SelectedEmployeeShift.CompanyShift.WedEndTime;

                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.ThuStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.ThuEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = SelectedEmployeeShift.CompanyShift.ThuStartTime;
                                SatEndTime = SelectedEmployeeShift.CompanyShift.ThuEndTime;
                            }
                            else if (!(SelectedEmployeeShift.CompanyShift.FriStartTime == new TimeSpan(0, 0, 0) && SelectedEmployeeShift.CompanyShift.FriEndTime == new TimeSpan(0, 0, 0)))
                            {
                                SatStartTime = SelectedEmployeeShift.CompanyShift.FriStartTime;
                                SatEndTime = SelectedEmployeeShift.CompanyShift.FriEndTime;
                            }
                            if (isStartTime)
                                return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                            else
                                return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);
                        }
                        else
                        {
                            if (isStartTime)
                                return StartTime.Value.Date.AddHours(SatStartTime.Hours).AddMinutes(SatStartTime.Minutes);
                            else
                                return EndTime.Value.Date.AddHours(SatEndTime.Hours).AddMinutes(SatEndTime.Minutes);
                        }

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
        #endregion

        //[Sudhir.Jangra][GEOS2-4018]
        private void OpenEmployeeAttendanceDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()...", category: Category.Info, priority: Priority.Low);
                if (IsAdd)
                {
                    byte[] EmployeeLeaveAttachmentBytes = AttendanceFileInBytes;
                    EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                    EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                    if (EmployeeLeaveAttachmentBytes != null)
                    {
                        employeeDocumentViewModel.OpenPdfFromBytes(EmployeeLeaveAttachmentBytes, AttendanceFileName);

                        employeeDocumentView.DataContext = employeeDocumentViewModel;
                        employeeDocumentView.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Could not find file {0}", AttendanceFileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    byte[] EmployeeLeaveAttachmentBytes = HrmService.GetEmployeeAttendanceAttachment(OldEmployeeAttendanceDetails);
                    EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                    EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                    if (EmployeeLeaveAttachmentBytes != null)
                    {
                        employeeDocumentViewModel.OpenPdfFromBytes(EmployeeLeaveAttachmentBytes, OldEmployeeAttendanceDetails.FileName);

                        employeeDocumentView.DataContext = employeeDocumentViewModel;
                        employeeDocumentView.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("Could not find file {0}", OldEmployeeAttendanceDetails.FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
                    AttendanceFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    AttachmentList = new ObservableCollection<Attachment>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    AttendanceFileName = file.Name;

                    ObservableCollection<Attachment> newAttachmentList = new ObservableCollection<Attachment>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = AttendanceFileInBytes;

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

        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
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
        //[nsatpute][19-12-2024][GEOS2-6635]
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
                    if (IsPartialTime && EndDate == null)
                        return null;
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
                    if (IsPartialTime && EndTime == null)
                        return null;

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

        private byte[] GetEmployeesImage_V2520(string EmployeeCode)
        {
            try
            {
                bool isProfileUpdate = false;
                //HrmService = new HrmServiceController("localhost:6699");
                isProfileUpdate = HrmService.IsProfileUpdate(EmployeeCode);
                string ProfileImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/Rounded/" + EmployeeCode + ".png";
                byte[] ImageBytes = null;
                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage_V2520()...", category: Category.Info, priority: Priority.Low);
                try
                {
                    if (isProfileUpdate)
                    {
                        #region isProfileUpdate
                        ImageBytes = GeosRepositoryServiceController.GetEmployeesImage(EmployeeCode);
                        #endregion
                    }
                    else
                    {
                        #region ImageUrlBytePair
                        if (GeosApplication.ImageUrlBytePair == null)
                            GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                        if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()))
                        {
                            ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()).Value;
                        }
                        else
                        {
                            if (GeosApplication.IsImageURLException == false)
                            {
                                //using (WebClient webClient = new WebClient())
                                //{
                                //    ImageBytes = webClient.DownloadData(ProfileImagePath);
                                //}
                                ImageBytes = Utility.ImageUtil.GetImageByWebClient(ProfileImagePath);
                            }
                            else
                            {
                                ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                            }
                            if (ImageBytes.Length > 0)
                            {
                                GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                                return ImageBytes;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {

                    if (isProfileUpdate)
                    {
                        #region isProfileUpdate
                        ImageBytes = GeosRepositoryServiceController.GetEmployeesImage(EmployeeCode);
                        #endregion
                    }
                    else
                    {
                        #region ImageUrlBytePair
                        if (GeosApplication.IsImageURLException == false)
                            GeosApplication.IsImageURLException = true;
                        if (!string.IsNullOrEmpty(ProfileImagePath))
                        {
                            ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                            GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                        }
                        #endregion
                    }
                }

                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage_V2520()....executed successfully", category: Category.Info, priority: Priority.Low);
                return ImageBytes;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetEmployeesImage_V2520()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }

        public void GrantPermissions()
        {
            //string folderPath = @"C:\Program Files (x86)\Emdep\GEOS\Workbench";
            string folderPath = System.AppDomain.CurrentDomain.BaseDirectory;
            GeosApplication.Instance.Logger.Log($"GrantPermissions() {folderPath}", category: Category.Exception, priority: Priority.Low);
            //string specificUser = Environment.MachineName + "\\Username";
            /* string specificUserName = "YourDomain\\YourUsername";*/ // Specify the user in "Domain\\Username" format
            //string specificUserName = "aapatil";
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            try
            {
                // Get the current ACL for the folder
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // Create a new rule to grant Modify permissions to the Users group
                //FileSystemAccessRule accessRule = new FileSystemAccessRule(
                //    new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                //    FileSystemRights.Modify,
                //    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                //    PropagationFlags.None,
                //    AccessControlType.Allow);

                // Create a new rule to grant Modify permissions to the specific user
                FileSystemAccessRule accessRule = new FileSystemAccessRule(
                    userName,
                    FileSystemRights.Modify,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                // Add the new rule to the ACL
                directorySecurity.AddAccessRule(accessRule);

                // Apply the updated ACL to the folder
                directoryInfo.SetAccessControl(directorySecurity);

                //Console.WriteLine($"Permissions successfully updated for {folderPath}");
                GeosApplication.Instance.Logger.Log($"Permissions successfully updated for {folderPath}", category: Category.Exception, priority: Priority.Low);

            }
            catch (UnauthorizedAccessException ex)
            {
                //Console.WriteLine("You need to run this program as an administrator.");
                //Console.WriteLine($"Error: {ex.Message}");
                GeosApplication.Instance.Logger.Log($"Error: {ex.Message}" + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GrantPermissionsNew();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"An error occurred: {ex.Message}" + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GrantPermissionsNew();
            }
        }

        public void GrantPermissionsNew()
        {
            string folderPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            // Log the action
            GeosApplication.Instance.Logger.Log($"GrantPermissionsNew() started for folder: {folderPath}", category: Category.Exception, priority: Priority.Low);

            try
            {
                // Get the current ACL for the folder
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // Define a new rule to grant Modify permissions to the current user
                FileSystemAccessRule accessRule = new FileSystemAccessRule(
                    userName,
                    FileSystemRights.Modify,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                // Add the new rule to the ACL safely
                bool modified;
                directorySecurity.ModifyAccessRule(AccessControlModification.Add, accessRule, out modified);

                // Apply the updated ACL to the folder
                if (modified)
                {
                    directoryInfo.SetAccessControl(directorySecurity);
                    GeosApplication.Instance.Logger.Log($"Permissions successfully updated for {folderPath}", category: Category.Exception, priority: Priority.Low);
                }
                else
                {
                    GeosApplication.Instance.Logger.Log($"No modifications were made to the ACL for {folderPath}", category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Log permission issues
                GeosApplication.Instance.Logger.Log($"Access denied: {ex.Message}" + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                // Log other exceptions
                GeosApplication.Instance.Logger.Log($"An error occurred: {ex.Message}" + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
        public ICommand OnDateFocusCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }

        #endregion

        #region Constructor

        public Task()
        {
            OnDateFocusCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateFocusAction);
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

        private void OnDateFocusAction(Object obj)
        {
            //flagEditDate = true;
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
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method OnTimeEditValueChanging()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method CheckDateTimeValidation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
