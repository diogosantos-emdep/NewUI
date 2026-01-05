using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.CodeView;
using System.Windows.Media.Imaging;
using System.IO;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.ServiceModel;
using DevExpress.Xpf.Map;
using DevExpress.Data.Filtering;
using Emdep.Geos.Modules.Hrm.Reports;
using DevExpress.Xpf.Printing;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Net.Mail;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraReports.UI;
using System.Threading;
using DevExpress.XtraPrinting.Shape;
using DevExpress.XtraPrinting.BarCode;
using System.Windows.Automation;
using System.Windows.Data;
using System.Windows.Controls;
using System.Net;
using NodaTime;
using DevExpress.CodeParser;
using DevExpress.XtraRichEdit.Commands.Internal;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{

    public class EmployeeProfileDetailViewModel : INotifyPropertyChanged, IDataErrorInfo, ISupportServices
    {

        #region TaskLog

        /// <summary>
        /// [HRM-M049-35] Display length of service months in employee profile [adadibathina][23102018]
        /// [HRM-M049-30] Work schedule not registered in changelog [adadibathina][24/10/2018]
        /// [HRM] Set status automatically when clearing exist date [25/10/2018][adadibathina]
        /// [M053-02][Employee shifts in employee profile] [adadibathina]
        /// [GEOS2-280](#63986) Long term leaves[adadibathina]
        /// [000][skale][2019-17-04][HRM][GEOS2-1468] Add polyvalence section in employee profile.
        /// [001][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// [003][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// [004][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [005][skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        /// [avpawar][12-02-2020][GEOS2-2111] HRM Bug- Length of service 
        /// </summary>

        #endregion

        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected IOpenFileDialogService OpenFileDialogService { get { return this.GetService<IOpenFileDialogService>(); } }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }


        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        //IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");

        #endregion

        #region Public ICommand

        public ICommand CommonValueChanged { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand OnExitDateEditValueChangedCommand { get; set; }
        public ICommand OnExitReasonEditValueChangingCommand { get; set; }
        public ICommand EditEmployeeViewCancelButtonCommand { get; set; }
        public ICommand EditEmployeetViewAcceptButtonCommand { get; set; }
        public ICommand AddNewIdentificationDocumentCommand { get; set; }
        public ICommand EditDocumentInformationDoubleClickCommand { get; set; }
        public ICommand DeleteDocumentInformationCommand { get; set; }
        public ICommand EmployeeDocumentViewCommand { get; set; }
        public ICommand AddNewLanguageInformationCommand { get; set; }
        public ICommand EditLanguageInformationDoubleClickCommand { get; set; }
        public ICommand DeleteLanguageInformationCommand { get; set; }
        public ICommand AddNewContactInformationCommand { get; set; }
        public ICommand EditPersonalContactInformationDoubleClickCommand { get; set; }
        public ICommand DeleteContactInformationCommand { get; set; }
        public ICommand AddNewProfessionalContactInformationCommand { get; set; }
        public ICommand EditProfessionalContactInformationDoubleClickCommand { get; set; }
        public ICommand DeleteProfessionalContactCommand { get; set; }
        public ICommand AddNewContractSituationInformationCommand { get; set; }
        public ICommand EditContractSituationInformationDoubleClickCommand { get; set; }
        public ICommand DeleteContractSituationInformationCommand { get; set; }
        public ICommand AddNewFamilyMemberInformationCommand { get; set; }
        public ICommand EditFamilyMemberInformationDoubleClickCommand { get; set; }
        public ICommand DeleteFamilyMemberInformationCommand { get; set; }
        public ICommand AddNewEducationQualificationInformationCommand { get; set; }
        public ICommand EditEducationInformationDoubleClickCommand { get; set; }
        public ICommand DeleteEducationInformationCommand { get; set; }
        public ICommand AddNewJobDescriptionCommand { get; set; }
        public ICommand EditJobDescriptionDoubleClickCommand { get; set; }
        public ICommand DeleteJobDescriptionCommand { get; set; }
        public ICommand AddNewProfessionalEducationCommand { get; set; }
        public ICommand EditProfessionalEducationDoubleClickCommand { get; set; }
        public ICommand DeleteProfessionalEducationCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand HyperlinkForEmailInGroupBox { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand CustomRowFilterCommand { get; set; }
        public ICommand PrintEmployeeProfileCommand { get; set; }
        public ICommand DeleteAuthorizedLeave { get; set; }
        public ICommand AddAuthorizedLeaveCommand { get; set; }
        public ICommand EditAuthorizedLeaveDoubleClickCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand DocumentViewCommand { get; set; }
        public ICommand EmployeeStatusSelectedIndexChangedCommand { get; set; }
        //[000] Added
        public ICommand AddNewPolyvalenceCommand { get; set; }
        public ICommand EditPolyvalenceDoubleClickCommand { get; set; }
        public ICommand DeletePolyvalenceCommand { get; set; }
        //[01] added
        public ICommand PrintEmployeeIdBadgeCommand { get; set; }

        //[003] added
        public ICommand AddNewEmployeeShiftCommand { get; set; }

        public ICommand DeleteEmployeeShiftCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
        public ICommand LoadedEventCommand { get; set; }

        public ICommand PrintEmployeeERFFormCommand { get; set; }
        public ICommand ExportEmployeeProfileCommand { get; set; }
        public ICommand ExitEventDeleteCommand { get; set; }

        #region chitra.girigosavi[21/08/2024] GEOS2-5579 HRM - New section in employee profile (1 of 3)
        public ICommand AddEquipmentAndToolsFileCommand { get; set; }
        public ICommand OpenEquipmentPDFDocumentCommand { get; set; }
        public ICommand EditEquipmentFileCommand { get; set; }
        public ICommand DeleteEquipmentFileCommand { get; set; }

        #endregion

        public ICommand AddEquipmentAndToolsFileFromRowCommand { get; set; }//[Sudhir.Jangra][GEOS2-5579]
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
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.Today;
        private bool isInformationError;
        private string informationError;
        private bool isBusy;
        private bool isSave;
        private bool isSaveChanges;
        private string professionalEmail;
        private string professionalPhone;
        private int contactCount;
        private string tempMergeFile = string.Empty;
        EmployeeContact ProfContact;
        private LookupValue selectedGender;
        DateTime? minExitDate;
        private ObservableCollection<LookupValue> userGenderList;
        private ObservableCollection<EmployeeContact> employeeContactList;
        private ObservableCollection<EmployeeContact> employeeProfessionalContactList;
        private ObservableCollection<EmployeeDocument> employeeDocumentList;
        private ObservableCollection<EmployeeLanguage> employeeLanguageList;
        private ObservableCollection<EmployeeEducationQualification> employeeEducationQualificationList;
        private ObservableCollection<EmployeeContractSituation> employeeContractSituationList;
        private ObservableCollection<EmployeeFamilyMember> employeeFamilyMemberList;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionList;
        private ObservableCollection<EmployeeJobDescription> employeeTopFourJobDescriptionList;
        private ObservableCollection<EmployeeContact> updatedEmployeeContactList;
        private ObservableCollection<EmployeeContact> updatedEmployeeProfessionalContactList;
        private ObservableCollection<EmployeeDocument> updatedEmployeeDocumentList;
        private ObservableCollection<EmployeeLanguage> updatedEmployeeLanguageList;
        private ObservableCollection<EmployeeEducationQualification> updatedEmployeeEducationQualificationList;
        private ObservableCollection<EmployeeContractSituation> updatedEmployeeContractSituationList;
        ObservableCollection<EmployeeContractSituation> ExistEmployeeContarctSituationFromMergeWindow;
        private ObservableCollection<EmployeeFamilyMember> updatedEmployeeFamilyMemberList;
        private ObservableCollection<EmployeeJobDescription> updatedEmployeeJobDescriptionList;
        private ObservableCollection<EmployeeChangelog> employeeChangeLogList;
        private ObservableCollection<EmployeeChangelog> employeeAllChangeLogList;
        private ObservableCollection<EmployeeProfessionalEducation> employeeProfessionalEducationList;
        private ObservableCollection<EmployeeProfessionalEducation> updatedemployeeProfessionalEducationList;

        //[SP63-000] Added
        private ObservableCollection<EmployeePolyvalence> updatedEmployeePolyvalenceList;
        private ObservableCollection<EmployeePolyvalence> employeePolyvalenceList;
        //END
        private List<CountryRegion> countryRegionList;
        public object TempExitDate;
        private EmployeeContact selectedContactRow;
        private EmployeeContact selectedProfessionalContactRow;
        private EmployeeDocument selectedDocumentRow;
        private EmployeeLanguage selectedLanguageRow;
        private EmployeeEducationQualification selectedEducationQualificationRow;
        private EmployeeContractSituation selectedContractSituationRow;
        private EmployeeFamilyMember selectedFamilyMemberRow;
        private EmployeeJobDescription selectedJobDescriptionRow;
        private EmployeeProfessionalEducation selectedProfessionalEducationRow;
        private EmployeePolyvalence selectedPolyvalenceRow;

        private int selectedIndexGender = -1;
        private bool isMale;
        private bool isFemale;

        //profile image.
        private ImageSource defaultPhotoSource = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
        private ImageSource profilePhotoSource;
        private ImageSource oldprofilePhotoSource;
        private byte[] profilePhotoBytes = null;
        private byte[] oldProfilePhotoBytes = null;

        private Employee employeeDetail;
        private Employee employeeUpdatedDetail;
        private Employee employeeExistDetail;
        //private List<int> disabilityLevelList;
        private int selectedDisability;
        private string firstName;
        private string lastName;
        private string nativeName;
        private string city;
        private string zipCode;
        private string region;
        private DateTime? birthDate;
        //private DateTime? exitDate;
        private string remark;
        private string address;
        private int selectedNationalityIndex;
        private int selectedCountryIndex;
        private int selectedLocationIndex;
        private int selectedMaritalStatusIndex;
        private int selectedCountryRegionIndex;
        public bool isEnableSave;
        private bool readOnly;
        private string employeeCode;
        private string birthDateErrorMessage = string.Empty;
        //private string exitReasonErrorMessage = string.Empty;

        private string error = string.Empty;
        private Regex regex;
        private string firstNameErrorMsg = string.Empty;
        private string lastNameErrorMsg = string.Empty;
        private string nativeNameErrorMsg = string.Empty;
        private string cityErrorMsg = string.Empty;
        private string regionErrorMsg = string.Empty;
        private List<string> countryWiseRegions;
        private string selectedCountryRegion;
        private int selectedBoxIndex;
        private string jobDescriptionError;
        private int selectedEmpolyeeStatusIndex;
        //private int selectedReasonIndex;
        //private string exitRemark;
        private string lengthOfService;
        private bool isEnableReason;
        //private bool isEnableRemark;
        private string age;
        private ObservableCollection<EmployeeAnnualLeave> employeeAnnualLeaveList;
        private string contractSituationError;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private List<City> cityList;
        private List<string> countryWiseCities;
        private string selectedCity;

        //private List<CompanySchedule> companySchedules;
        //private List<CompanyShift> companyShifts;
        //private int selectedCompanyShiftIndex;
        //private int selectedCompanyScheduleIndex = -1; 
        private long selectedPeriod;
        private int totalJobDescriptionUsage;
        private ObservableCollection<EmployeeAnnualLeave> tempEmployeeAnnualLeaveList;
        private string ZipCodeErrorMsg = string.Empty;
        private string SelectedRegionErrorMsg = string.Empty;
        private string SelectedCityErrorMsg = string.Empty;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionsList;
        private bool isPhoto;
        private ObservableCollection<EmployeeAnnualLeave> updatedEmployeeAnnualLeaveList;
        private EmployeeAnnualLeave selectedEmployeeAnnualLeave;
        //private byte[] fileInBytes;
        private List<Object> attachmentObjectList;
        private ObservableCollection<Data.Common.Attachment> attachmentList;
        private string fileName;
        private Data.Common.Attachment attachedFile;
        private Visibility isVisible;
        private string JobDescriptionName = string.Empty;
        private bool isLongTermAbsent;
        private bool IsJobDescriptionOrganizationChanged = true;
        private List<int> ExistJobDescriptionCompanyIdList;
        private double dialogHeight;
        private double dialogWidth;
        //[003] added
        private ObservableCollection<EmployeeShift> employeeShiftList;
        private string employeeShiftError;

        //[004] Added
        private ObservableCollection<EmployeeExitEvent> exitEventList;
        private EmployeeExitEvent selectedExitEventItem;
        private ObservableCollection<EmployeeExitEvent> updateEmployeeExitEventsList;

        //[005] added
        private ObservableCollection<EmployeeShift> topFourEmployeeShiftList;
        private bool isShiftAsteriskVisible;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private string displayName;
        private List<EmployeeContractSituation> GetAllUpdatedEmployeeContracts;
        //private byte isRemote;
        private bool isRemote;
        private byte Remote;
        private bool dynamicColumnsAdded;
        private ObservableCollection<GridColumn> additionalBandGridColumns;
        private int profTypeId;
        private bool isEnableForDeleteBtn;
        private int idEmployeeStatus;
        private ObservableCollection<Employee> lstBackupEmployee;
        private int selectedIndexBackupEmployee;
        private Employee selectedItemBackupEmployee;
        private string employeeStatusWithLongTermAbsent;
        private ObservableCollection<JobDescription> jobDescriptionList;//[Sudhir.Jangra][GEOS2-4846]
        public int selectedJobDescription;//[Sudhir.Jangra][GEOS2-4846]
        private List<LookupValue> extraHoursTimeList;//[Sudhir.Jangra][GEOS2-5273]
        private LookupValue selectedExtraHoursTime;//[Sudhir.Jangra][GEOS2-5273]
        private ObservableCollection<Employee> existProfessionalEmailEmployeeList;//[Sudhir.Jangra][GEOS2-3418]

        private ObservableCollection<EmployeeEquipmentAndTools> requiredEquipmentList;//[Sudhir.Jangra][GEOS2-5579]
        private EmployeeEquipmentAndTools selectedRequiredEquipment;//[Sudhir.Jangra][GEOS2-5579]
        private ObservableCollection<EmployeeEquipmentAndTools> employeeEquipmentList;//[Sudhir.Jangra][GEOS2-5579]
        private EmployeeEquipmentAndTools selectedEmployeeEquipment;//[Sudhir.Jangra][GEOS2-5579]
        private List<EmployeeEquipmentAndTools> clonedEmployeeEquipmmentList;//[Sudhir.jangra][GEOS2-5579]
        private Int32 IdEmployee { get; set; }//[Sudhir.Jangra][GEOS2-5579]
        int idCompany;
        //[nsatpute][26-03-2025][GEOS2-7011]
        IServiceContainer serviceContainer = null;
        Dictionary<int, string> dictPetronalNumbers;
        Visibility exportButtonVisible;
        #endregion

        #region Properties
        public int IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompany"));
            }
        }
        ObservableCollection<Company> locationsList;
        ObservableCollection<Company> locationList;
        public ObservableCollection<Company> LocationsList
        {
            get
            {
                return locationsList;
            }

            set
            {
                locationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationsList"));

            }
        }
        public int ProfTypeId
        {
            get { return profTypeId; }
            set
            {
                profTypeId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfTypeId"));
            }
        }
        public bool IsEnableForDeleteBtn
        {
            get { return isEnableForDeleteBtn; }
            set
            {
                isEnableForDeleteBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableForDeleteBtn"));
            }
        }

        public int IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployeeStatus"));
            }
        }
        public ObservableCollection<GridColumn> AdditionalBandGridColumns
        {
            get
            {
                return additionalBandGridColumns;
            }

            set
            {
                this.additionalBandGridColumns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalBandGridColumns"));

            }
        }

        public bool IsRemote
        {
            get { return isRemote; }
            set
            {
                isRemote = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemote"));
            }
        }
        public DateTime? JobDescriptionStartDate { get; set; }
        public EmployeeContractSituation CurrentContractSituation { get; set; }
        public IList<LookupValue> GenderList { get; set; }
        public List<LookupValue> EmployeeStatusList { get; set; }
        public List<LookupValue> MaritalStatusList { get; set; }
        public List<LookupValue> ExitReasonList { get; set; }

        public List<Country> NationalityList { get; set; }
        public List<Country> CountryList { get; set; }
        public List<CountryRegion> AllCountryRegionList { get; set; }
        public bool IsFromFirstName { get; set; }

        public bool IsFromLastName { get; set; }
        public bool IsFromNativeName { get; set; }
        public bool IsFromCity { get; set; }
        public bool IsFromRegion { get; set; }
        public bool IsFromExitDate { get; set; }
        public bool IsFromExitReason { get; set; }
        public bool IsFromExitRemark { get; set; }
        //[GEOS2-6493][rdixit][17.01.2025]
        public ObservableCollection<Company> LocationList
        {
            get
            {
                return locationList;
            }

            set
            {
                locationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationList"));

            }
        }
        public string InformationError
        {
            get
            {
                return informationError;
            }

            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }


        public bool IsInformationError
        {
            get
            {
                return isInformationError;
            }

            set
            {
                isInformationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationError"));
            }
        }

        public ObservableCollection<EmployeeAnnualLeave> UpdatedEmployeeAnnualLeaveList
        {
            get { return updatedEmployeeAnnualLeaveList; }
            set
            {
                updatedEmployeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeAnnualLeaveList"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionsList
        {
            get { return employeeJobDescriptionsList; }
            set
            {
                employeeJobDescriptionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionsList"));
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

        public ObservableCollection<EmployeeAnnualLeave> TempEmployeeAnnualLeaveList
        {
            get { return tempEmployeeAnnualLeaveList; }
            set
            {
                tempEmployeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempEmployeeAnnualLeaveList"));
            }
        }

        public DateTime? MinExitDate
        {
            get { return minExitDate; }
            set
            {
                minExitDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinExitDate"));
            }
        }


        //public DateTime? ExitDate1
        //{
        //    get { return exitDate; }
        //    set
        //    {
        //        exitDate = value;
        //        IsFromExitDate = true;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ExitDate1"));
        //    }
        //}

        //public int SelectedReasonIndex
        //{
        //    get { return selectedReasonIndex; }
        //    set
        //    {
        //        selectedReasonIndex = value;
        //        IsFromExitReason = true;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedReasonIndex"));
        //    }
        //}

        //public string ExitRemark
        //{
        //    get { return exitRemark; }
        //    set
        //    {
        //        exitRemark = value;
        //        IsFromExitRemark = true;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ExitRemark"));
        //    }
        //}

        public int ContactCount
        {
            get { return contactCount; }
            set
            {
                contactCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactCount"));
            }
        }

        public string LengthOfService
        {
            get { return lengthOfService; }
            set
            {
                lengthOfService = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LengthOfService"));
            }
        }

        public int SelectedEmpolyeeStatusIndex
        {
            get { return selectedEmpolyeeStatusIndex; }
            set
            {
                selectedEmpolyeeStatusIndex = value;

                if (EmployeeStatusList != null && SelectedEmpolyeeStatusIndex >= 0)
                {
                    IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmpolyeeStatusIndex"));
            }
        }

        public string ProfessionalEmail
        {
            get { return professionalEmail; }
            set
            {
                professionalEmail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalEmail"));
            }
        }

        public string ProfessionalPhone
        {
            get { return professionalPhone; }
            set
            {
                professionalPhone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalPhone"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContractSituation"));
            }
        }

        public int SelectedBoxIndex
        {
            get
            {
                return selectedBoxIndex;
            }

            set
            {
                selectedBoxIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBoxIndex"));
            }
        }

        public string JobDescriptionError
        {
            get { return jobDescriptionError; }
            set
            {
                jobDescriptionError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionError"));
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

        public ObservableCollection<EmployeeProfessionalEducation> EmployeeProfessionalEducationList
        {
            get { return employeeProfessionalEducationList; }
            set
            {
                employeeProfessionalEducationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfessionalEducationList"));
            }
        }

        public ObservableCollection<EmployeeProfessionalEducation> UpdatedemployeeProfessionalEducationList
        {
            get { return updatedemployeeProfessionalEducationList; }
            set
            {
                updatedemployeeProfessionalEducationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedemployeeProfessionalEducationList"));
            }
        }
        //[SP63-000] Added
        public ObservableCollection<EmployeePolyvalence> UpdatedEmployeePolyvalenceList
        {
            get { return updatedEmployeePolyvalenceList; }
            set
            {
                updatedEmployeePolyvalenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeePolyvalenceList"));
            }
        }

        public ObservableCollection<EmployeePolyvalence> EmployeePolyvalenceList
        {
            get { return employeePolyvalenceList; }
            set
            {
                employeePolyvalenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeePolyvalenceList"));
            }
        }
        public EmployeePolyvalence SelectedPolyvalenceRow
        {
            get { return selectedPolyvalenceRow; }
            set
            {
                selectedPolyvalenceRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPolyvalenceRow"));
            }
        }
        //END
        public List<string> CountryWiseRegions
        {
            get { return countryWiseRegions; }
            set
            {
                countryWiseRegions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryWiseRegions"));
            }
        }

        public Employee EmployeeExistDetail
        {
            get { return employeeExistDetail; }
            set
            {
                employeeExistDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExistDetail"));
            }
        }

        public string SelectedCountryRegion
        {
            get { return selectedCountryRegion; }
            set
            {
                selectedCountryRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryRegion"));
            }
        }

        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }

        public ObservableCollection<EmployeeContact> EmployeeContactList
        {
            get { return employeeContactList; }
            set
            {
                employeeContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContactList"));
            }
        }

        public ObservableCollection<EmployeeContact> UpdatedEmployeeContactList
        {
            get { return updatedEmployeeContactList; }
            set
            {
                updatedEmployeeContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeContactList"));
            }
        }

        public ObservableCollection<EmployeeContact> EmployeeProfessionalContactList
        {
            get { return employeeProfessionalContactList; }
            set
            {
                employeeProfessionalContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfessionalContactList"));
            }
        }

        public ObservableCollection<EmployeeContact> UpdatedEmployeeProfessionalContactList
        {
            get { return updatedEmployeeProfessionalContactList; }
            set
            {
                updatedEmployeeProfessionalContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeProfessionalContactList"));
            }
        }

        public ObservableCollection<EmployeeDocument> UpdatedEmployeeDocumentList
        {
            get { return updatedEmployeeDocumentList; }
            set
            {
                updatedEmployeeDocumentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeDocumentList"));
            }
        }

        public ObservableCollection<EmployeeLanguage> UpdatedEmployeeLanguageList
        {
            get { return updatedEmployeeLanguageList; }
            set
            {
                updatedEmployeeLanguageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeLanguageList"));
            }
        }

        public ObservableCollection<EmployeeEducationQualification> UpdatedEmployeeEducationQualificationList
        {
            get { return updatedEmployeeEducationQualificationList; }
            set
            {
                updatedEmployeeEducationQualificationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeEducationQualificationList"));
            }
        }

        public ImageSource OldprofilePhotoSource
        {
            get { return oldprofilePhotoSource; }
            set
            {

                oldprofilePhotoSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldprofilePhotoSource"));
            }
        }

        public ObservableCollection<EmployeeContractSituation> UpdatedEmployeeContractSituationList
        {
            get { return updatedEmployeeContractSituationList; }
            set
            {
                updatedEmployeeContractSituationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeContractSituationList"));
            }
        }

        public ObservableCollection<EmployeeFamilyMember> UpdatedEmployeeFamilyMemberList
        {
            get { return updatedEmployeeFamilyMemberList; }
            set
            {
                updatedEmployeeFamilyMemberList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeFamilyMemberList"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> UpdatedEmployeeJobDescriptionList
        {
            get { return updatedEmployeeJobDescriptionList; }
            set
            {
                updatedEmployeeJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeJobDescriptionList"));
            }
        }
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        public ObservableCollection<EmployeeDocument> EmployeeDocumentList
        {
            get { return employeeDocumentList; }
            set
            {
                employeeDocumentList = value;
                //[001]
                foreach (EmployeeDocument itemEmployeeDocument in EmployeeDocumentList)
                {
                    if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                    {

                        if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate >= DateTime.Now) && i.EmployeeDocumentIdType == itemEmployeeDocument.EmployeeDocumentIdType) && (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138))
                        {
                            itemEmployeeDocument.IsgreaterJobDescriptionthanToday = true;
                        }
                        else if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate < DateTime.Now)))
                        {
                            itemEmployeeDocument.IsgreaterJobDescriptionthanToday = false;
                        }
                    }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDocumentList"));
            }
        }

        public ObservableCollection<EmployeeLanguage> EmployeeLanguageList
        {
            get { return employeeLanguageList; }
            set
            {
                employeeLanguageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLanguageList"));
            }
        }

        public bool IsBackupEmployeeNotExist
        {
            get;
            set
           ;
        }

        public ObservableCollection<EmployeeEducationQualification> EmployeeEducationQualificationList
        {
            get { return employeeEducationQualificationList; }
            set
            {
                employeeEducationQualificationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEducationQualificationList"));
            }
        }

        public ObservableCollection<EmployeeChangelog> EmployeeAllChangeLogList
        {
            get { return employeeAllChangeLogList; }
            set
            {
                employeeAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAllChangeLogList"));
            }
        }
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        public ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList
        {
            get { return employeeContractSituationList; }
            set
            {
                employeeContractSituationList = value;
                //[001]
                foreach (EmployeeContractSituation itemEmployeeContractSituation in EmployeeContractSituationList)
                {
                    if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                        itemEmployeeContractSituation.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                    if (EmployeeContractSituationList.Any(i => i.ContractSituationEndDate == null || i.ContractSituationEndDate >= DateTime.Now))
                    {
                        itemEmployeeContractSituation.IsgreaterJobDescriptionthanToday = true;
                    }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituationList"));
            }
        }

        public ObservableCollection<EmployeeFamilyMember> EmployeeFamilyMemberList
        {
            get { return employeeFamilyMemberList; }
            set
            {
                employeeFamilyMemberList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeFamilyMemberList"));
            }
        }


        public ObservableCollection<Employee> LstBackupEmployee
        {
            get { return lstBackupEmployee; }
            set
            {
                lstBackupEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstBackupEmployee"));
            }
        }

        public Int32 SelectedIndexBackupEmployee
        {
            get { return selectedIndexBackupEmployee; }
            set
            {
                selectedIndexBackupEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBackupEmployee"));
            }
        }

        public Employee SelectedItemBackupEmployee
        {
            get { return selectedItemBackupEmployee; }
            set
            {
                selectedItemBackupEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemBackupEmployee"));
            }
        }

        // [001][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1
        ///  [002][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList
        {
            get { return employeeJobDescriptionList; }
            set
            {
                employeeJobDescriptionList = value;
                if (employeeJobDescriptionList != null)
                {
                    //  EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList().Take(4).ToList());
                    // [001]
                    var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList();
                    if (tempEmployeeTopFourJobDescriptionList.Count > 0)
                    {
                        int TotalPercentage = tempEmployeeTopFourJobDescriptionList.Sum(a => a.JobDescriptionUsage);
                        if (TotalPercentage == 100)
                        {
                            EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(tempEmployeeTopFourJobDescriptionList);
                        }
                    }
                    else
                    {
                        var tempEmployeeTopFourJobDescription = EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionEndDate).ToList();
                        if (tempEmployeeTopFourJobDescription.Count > 0)
                        {
                            if (tempEmployeeTopFourJobDescription[0].JobDescriptionUsage == 100)
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                            }
                            else
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                                int TotalPercentage = tempEmployeeTopFourJobDescription[0].JobDescriptionUsage;
                                for (int i = 1; i < tempEmployeeTopFourJobDescription.Count; i++)
                                {
                                    TotalPercentage = TotalPercentage + tempEmployeeTopFourJobDescription[i].JobDescriptionUsage;
                                    if (TotalPercentage < 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);

                                    }
                                    else if (TotalPercentage == 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }
                                    else if (TotalPercentage > 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var tempEmployeeTopFourJobDescriptiontemp = EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionEndDate).ToList();
                            if (tempEmployeeTopFourJobDescriptiontemp.Count > 0)
                            {
                                if (tempEmployeeTopFourJobDescriptiontemp[0].JobDescriptionUsage == 100)
                                {
                                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                    EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescriptiontemp[0]);
                                }
                                else
                                {
                                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                    EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescriptiontemp[0]);
                                    int TotalPercentage = tempEmployeeTopFourJobDescriptiontemp[0].JobDescriptionUsage;
                                    for (int i = 1; i < tempEmployeeTopFourJobDescriptiontemp.Count; i++)
                                    {
                                        TotalPercentage = TotalPercentage + tempEmployeeTopFourJobDescriptiontemp[i].JobDescriptionUsage;
                                        if (TotalPercentage < 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescriptiontemp[i]);

                                        }
                                        else if (TotalPercentage == 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescriptiontemp[i]);
                                            break;
                                        }
                                        else if (TotalPercentage > 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescriptiontemp[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (EmployeeTopFourJobDescriptionList == null)
                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();

                int CheckAliasCount = EmployeeTopFourJobDescriptionList.Select(a => a.Company.Alias).Distinct().Count();
                if (EmployeeTopFourJobDescriptionList.Count == 1)
                {
                    EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                }
                else
                {
                    if (CheckAliasCount > 1)
                    {
                        //set visible column
                        EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = true; });
                    }
                    else
                    {
                        EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                    }
                }
                //[002]
                foreach (EmployeeJobDescription itemEmployeeJobDescription in EmployeeJobDescriptionList)
                {
                    if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                        itemEmployeeJobDescription.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                    if (EmployeeJobDescriptionList.Any(i => (i.JobDescriptionEndDate == null && i.JobDescriptionStartDate <= DateTime.Now) || i.JobDescriptionEndDate >= DateTime.Now))
                    {
                        itemEmployeeJobDescription.IsgreaterJobDescriptionthanToday = true;
                    }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> EmployeeTopFourJobDescriptionList
        {
            get { return employeeTopFourJobDescriptionList; }
            set
            {
                employeeTopFourJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTopFourJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeeChangelog> EmployeeChangeLogList
        {
            get { return employeeChangeLogList; }
            set
            {
                employeeChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeChangeLogList"));
            }
        }

        public int SelectedNationalityIndex
        {
            get { return selectedNationalityIndex; }
            set
            {
                selectedNationalityIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNationalityIndex"));
            }
        }

        public List<CountryRegion> CountryRegionList
        {
            get { return countryRegionList; }
            set
            {
                countryRegionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryRegionList"));
            }
        }

        public int SelectedCountryIndex
        {
            get { return selectedCountryIndex; }
            set
            {
                selectedCountryIndex = value;
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                {
                    if (selectedCountryIndex != -1)
                    {
                        CountryRegionList = AllCountryRegionList.FindAll(x => x.IdCountry == CountryList[selectedCountryIndex].IdCountry);
                        CountryWiseRegions = new List<string>();
                        CountryWiseRegions = CountryRegionList.Select(x => x.Name).ToList();

                        CityList = GeosApplication.Instance.CityList.FindAll(x => x.IdCountry == CountryList[selectedCountryIndex].IdCountry).ToList();
                        CountryWiseCities = new List<string>();
                        CountryWiseCities = CityList.Select(x => x.Name).ToList();
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryIndex"));
            }
        }
        public int SelectedLocationIndex
        {
            get { return selectedLocationIndex; }
            set
            {
                selectedLocationIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationIndex"));
            }
        }


        public int SelectedMaritalStatusIndex
        {
            get { return selectedMaritalStatusIndex; }
            set
            {
                selectedMaritalStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMaritalStatusIndex"));
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                IsFromFirstName = true;
                IsFromLastName = false;
                IsFromNativeName = false;
                IsFromCity = false;
                IsFromRegion = false;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
                GenerateDisplayName();  //chitra.girigosavi[GEOS2-3401][10/01/2024]
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                IsFromLastName = true;
                IsFromFirstName = false;
                IsFromNativeName = false;
                IsFromCity = false;
                IsFromRegion = false;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
                GenerateDisplayName();  //chitra.girigosavi[GEOS2-3401][10/01/2024]
            }
        }

        public string NativeName
        {
            get { return nativeName; }
            set
            {
                nativeName = value;
                IsFromLastName = false;
                IsFromFirstName = false;
                IsFromNativeName = true;
                IsFromCity = false;
                IsFromRegion = false;
                OnPropertyChanged(new PropertyChangedEventArgs("NativeName"));
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                IsFromLastName = false;
                IsFromFirstName = false;
                IsFromNativeName = false;
                IsFromCity = true;
                IsFromRegion = false;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ZipCode"));
            }
        }

        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                IsFromLastName = false;
                IsFromFirstName = false;
                IsFromNativeName = false;
                IsFromCity = false;
                IsFromRegion = true;
                OnPropertyChanged(new PropertyChangedEventArgs("Region"));
            }
        }

        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BirthDate"));
            }
        }

        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }

        public LookupValue SelectedGender
        {
            get { return selectedGender; }
            set
            {
                selectedGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGender"));
            }
        }

        public bool IsMale
        {
            get { return isMale; }
            set
            {
                isMale = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMale"));
            }
        }

        public bool IsFemale
        {
            get { return isFemale; }
            set
            {
                isFemale = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFemale"));
            }
        }

        public ObservableCollection<LookupValue> UserGenderList
        {
            get { return userGenderList; }
            set
            {
                userGenderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserGenderList"));
            }
        }

        public Employee EmployeeDetail
        {
            get { return employeeDetail; }
            set
            {
                employeeDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDetail"));
            }
        }

        public int SelectedCountryRegionIndex
        {
            get { return selectedCountryRegionIndex; }
            set
            {
                selectedCountryRegionIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryRegionIndex"));
            }
        }

        public ImageSource ProfilePhotoSource
        {
            get { return profilePhotoSource; }
            set
            {
                profilePhotoSource = value;
                if (profilePhotoSource != null)
                {
                    IsPhoto = true;
                }
                else
                {
                    ProfilePhotoSource = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
                    IsPhoto = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ProfilePhotoSource"));
            }
        }


        public bool IsPhoto
        {
            get { return isPhoto; }
            set
            {
                isPhoto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPhoto"));
            }
        }

        public byte[] ProfilePhotoBytes
        {
            get { return profilePhotoBytes; }
            set
            {
                profilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfilePhotoBytes"));
            }
        }

        public byte[] OldProfilePhotoBytes
        {
            get { return oldProfilePhotoBytes; }
            set
            {
                oldProfilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldProfilePhotoBytes"));
            }
        }

        public int SelectedDisability
        {
            get { return selectedDisability; }
            set
            {
                selectedDisability = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDisability"));
            }
        }

        public int SelectedIndexGender
        {
            get { return selectedIndexGender; }
            set
            {
                selectedIndexGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGender"));
            }
        }

        public bool IsEnableSave
        {
            get { return isEnableSave; }
            set
            {
                isEnableSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableSave"));
            }
        }

        public bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadOnly"));
            }
        }

        public Employee EmployeeUpdatedDetail
        {
            get { return employeeUpdatedDetail; }
            set
            {
                employeeUpdatedDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeUpdatedDetail"));
            }
        }

        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCode"));
            }
        }

        public string EmployeeStatusWithLongTermAbsent
        {
            get { return employeeStatusWithLongTermAbsent; }
            set
            {
                employeeStatusWithLongTermAbsent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeStatusWithLongTermAbsent"));
            }
        }

        public EmployeeContact SelectedContactRow
        {
            get { return selectedContactRow; }
            set
            {
                selectedContactRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContactRow"));
            }
        }

        public EmployeeDocument SelectedDocumentRow
        {
            get { return selectedDocumentRow; }
            set
            {
                selectedDocumentRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDocumentRow"));
            }
        }

        public EmployeeLanguage SelectedLanguageRow
        {
            get { return selectedLanguageRow; }
            set
            {
                selectedLanguageRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLanguageRow"));
            }
        }

        public EmployeeEducationQualification SelectedEducationQualificationRow
        {
            get { return selectedEducationQualificationRow; }
            set
            {
                selectedEducationQualificationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEducationQualificationRow"));
            }
        }

        public EmployeeContractSituation SelectedContractSituationRow
        {
            get { return selectedContractSituationRow; }
            set
            {
                selectedContractSituationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContractSituationRow"));
            }
        }

        public EmployeeFamilyMember SelectedFamilyMemberRow
        {
            get { return selectedFamilyMemberRow; }
            set
            {
                selectedFamilyMemberRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamilyMemberRow"));
            }
        }

        public EmployeeJobDescription SelectedJobDescriptionRow
        {
            get { return selectedJobDescriptionRow; }
            set
            {
                selectedJobDescriptionRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescriptionRow"));
            }
        }

        public EmployeeProfessionalEducation SelectedProfessionalEducationRow
        {
            get { return selectedProfessionalEducationRow; }
            set
            {
                selectedProfessionalEducationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfessionalEducationRow"));
            }
        }

        public EmployeeContact SelectedProfessionalContactRow
        {
            get { return selectedProfessionalContactRow; }
            set
            {
                selectedProfessionalContactRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfessionalContactRow"));
            }
        }

        //public bool IsEnableRemark
        //{
        //    get { return isEnableRemark; }
        //    set
        //    {
        //        isEnableRemark = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsEnableRemark"));
        //    }
        //}

        public bool IsEnableReason
        {
            get { return isEnableReason; }
            set
            {
                isEnableReason = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableReason"));
            }
        }

        public string Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
        }

        public ObservableCollection<EmployeeAnnualLeave> EmployeeAnnualLeaveList
        {
            get { return employeeAnnualLeaveList; }
            set
            {
                employeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAnnualLeaveList"));
            }
        }

        public string ContractSituationError
        {
            get { return contractSituationError; }
            set
            {
                contractSituationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationError"));
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

        public List<City> CityList
        {
            get { return cityList; }
            set
            {
                cityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CityList"));
            }
        }

        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCity"));
            }
        }

        public List<string> CountryWiseCities
        {
            get { return countryWiseCities; }
            set
            {
                countryWiseCities = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryWiseCities"));
            }
        }
        //[005] added
        //public List<CompanySchedule> CompanySchedules
        //{
        //    get { return companySchedules; }
        //    set
        //    {
        //        companySchedules = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanySchedules"));
        //    }
        //}

        //public int SelectedCompanyScheduleIndex
        //{
        //    get { return selectedCompanyScheduleIndex; }
        //    set
        //    {
        //        selectedCompanyScheduleIndex = value;

        //        if (SelectedCompanyScheduleIndex > -1)
        //        {
        //            // CompanyShifts = CompanySchedules[SelectedCompanyScheduleIndex].CompanyShifts;
        //            SelectedCompanyShiftIndex = 0;
        //        }

        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyScheduleIndex"));
        //    }
        //}

        //public List<CompanyShift> CompanyShifts
        //{
        //    get { return companyShifts; }
        //    set
        //    {
        //        companyShifts = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifts"));
        //    }
        //}
        //public int SelectedCompanyShiftIndex
        //{
        //    get { return selectedCompanyShiftIndex; }
        //    set
        //    {
        //        selectedCompanyShiftIndex = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyShiftIndex"));
        //    }
        //}

        public int TotalJobDescriptionUsage
        {
            get { return totalJobDescriptionUsage; }
            set
            {
                totalJobDescriptionUsage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalJobDescriptionUsage"));
            }
        }

        public EmployeeAnnualLeave SelectedEmployeeAnnualLeave
        {
            get { return selectedEmployeeAnnualLeave; }
            set
            {
                selectedEmployeeAnnualLeave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeAnnualLeave"));
            }
        }

        public ObservableCollection<Data.Common.Attachment> AttachmentList
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

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public Data.Common.Attachment AttachedFile
        {
            get { return attachedFile; }
            set
            {
                attachedFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFile"));
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

        public bool IsLongTermAbsent
        {
            get
            {
                return isLongTermAbsent;

            }
            set
            {
                isLongTermAbsent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLongTermAbsent"));
            }
        }


        private int indexSun;
        public int IndexSun
        {
            get { return indexSun; }
            set { indexSun = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSun")); }
        }
        private int indexMon;
        public int IndexMon
        {
            get { return indexMon; }
            set { indexMon = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexMon")); }
        }

        private int indexTue;
        public int IndexTue
        {
            get { return indexTue; }
            set { indexTue = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexTue")); }
        }

        private int indexWed;
        public int IndexWed
        {
            get { return indexWed; }
            set { indexWed = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexWed")); }
        }

        private int indexThu;
        public int IndexThu
        {
            get { return indexThu; }
            set { indexThu = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexThu")); }
        }

        private int indexFri;
        public int IndexFri
        {
            get { return indexFri; }
            set { indexFri = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexFri")); }
        }

        private int indexSat;
        public int IndexSat
        {
            get { return indexSat; }
            set { indexSat = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSat")); }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        //[003] added
        public ObservableCollection<EmployeeShift> EmployeeShiftList
        {
            get { return employeeShiftList; }
            set
            {
                employeeShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftList"));
            }
        }
        public string EmployeeShiftError
        {
            get { return employeeShiftError; }
            set
            {
                employeeShiftError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeShiftError"));

            }
        }
        //[004] Added 
        public ObservableCollection<EmployeeExitEvent> ExitEventList
        {
            get { return exitEventList; }
            set
            {
                exitEventList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExitEventList"));
            }
        }
        public EmployeeExitEvent SelectedExitEventItem
        {
            get { return selectedExitEventItem; }
            set { selectedExitEventItem = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedExitEventItem")); }
        }
        public ObservableCollection<EmployeeExitEvent> UpdateEmployeeExitEventsList
        {
            get { return updateEmployeeExitEventsList; }
            set
            {
                updateEmployeeExitEventsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateEmployeeExitEventsList"));
            }
        }
        //[005] added
        public ObservableCollection<EmployeeShift> TopFourEmployeeShiftList
        {
            get { return topFourEmployeeShiftList; }
            set
            {
                topFourEmployeeShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopFourEmployeeShiftList"));
            }
        }
        public bool IsShiftAsteriskVisible
        {
            get { return isShiftAsteriskVisible; }
            set
            {
                isShiftAsteriskVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShiftAsteriskVisible"));

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
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;

                OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
            }
        }

        string EmployeeID_Companies_GeosAppSetting { get; set; }
        List<string> EmployeeID_CompaniestGeosAppSettingList { get; set; }

        //End

        //[Sudhir.Jangra][GEOS2-4846]
        public ObservableCollection<JobDescription> JobDescriptionList
        {
            get { return jobDescriptionList; }
            set
            {
                jobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4846]
        public int SelectedJobDescription
        {
            get { return selectedJobDescription; }
            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5273]
        public List<LookupValue> ExtraHoursTimeList
        {
            get { return extraHoursTimeList; }
            set
            {
                extraHoursTimeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExtraHoursTimeList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5273]
        public LookupValue SelectedExtraHoursTime
        {
            get { return selectedExtraHoursTime; }
            set
            {
                selectedExtraHoursTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedExtraHoursTime"));
            }
        }

        //[Sudhir.jangra][GEOS2-3418]
        public ObservableCollection<Employee> ExistProfessionalEmailEmployeeList
        {
            get { return existProfessionalEmailEmployeeList; }
            set
            {
                existProfessionalEmailEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistProfessionalEmailEmployeeList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public ObservableCollection<EmployeeEquipmentAndTools> RequiredEquipmentList
        {
            get { return requiredEquipmentList; }
            set
            {
                requiredEquipmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RequiredEquipmentList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public EmployeeEquipmentAndTools SelectedRequiredEquipment
        {
            get { return selectedRequiredEquipment; }
            set
            {
                selectedRequiredEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRequiredEquipment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public ObservableCollection<EmployeeEquipmentAndTools> EmployeeEquipmentList
        {
            get { return employeeEquipmentList; }
            set
            {
                employeeEquipmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEquipmentList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public EmployeeEquipmentAndTools SelectedEmployeeEquipment
        {
            get { return selectedEmployeeEquipment; }
            set
            {
                selectedEmployeeEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeEquipment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public List<EmployeeEquipmentAndTools> ClonedEmployeeEquipmentList
        {
            get { return clonedEmployeeEquipmmentList; }
            set
            {
                clonedEmployeeEquipmmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedEmployeeEquipmentList"));
            }
        }
        //[nsatpute][26-03-2025][GEOS2-7011]
        IServiceContainer ISupportServices.ServiceContainer
        {
            get
            {
                if (serviceContainer == null)
                    serviceContainer = new ServiceContainer(this);
                return serviceContainer;
            }
        }
        public Visibility ExportButtonVisible
        {
            get { return exportButtonVisible; }
            set
            {
                exportButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExportButtonVisible"));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        public EmployeeProfileDetailViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EmployeeProfileDetailViewModel()...", category: Category.Info, priority: Priority.Low);

                //System.Windows.Forms.Screen screen = GeosApplication.Instance.GetWorkingScreenFrom();
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;

                // CommonValueChanged = new RelayCommand(new Action<object>(CommonValidation)); 
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                //OnExitDateEditValueChangingCommand = new DelegateCommand<EditValueChangedEventArgs>(OnExitDateEditValueChanging);
                OnExitDateEditValueChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(OnExitDateEditValueChanged);
                EditEmployeeViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                EditEmployeetViewAcceptButtonCommand = new DelegateCommand(EditEmployee);
                AddNewIdentificationDocumentCommand = new DelegateCommand<object>(AddNewIdentificationDocument);
                EditDocumentInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditDocumentInformation));
                DeleteDocumentInformationCommand = new RelayCommand(new Action<object>(DeleteDocumentInformationRecord));
                EmployeeDocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeEducationDocument));
                AddNewLanguageInformationCommand = new DelegateCommand<object>(AddNewLanguageInformation);
                EditLanguageInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditLanguageInformation));
                DeleteLanguageInformationCommand = new RelayCommand(new Action<object>(DeleteLanguageInformationRecord));
                AddNewContactInformationCommand = new DelegateCommand<object>(AddNewContactInformation);
                EditPersonalContactInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditPersonalContactInformation));
                DeleteContactInformationCommand = new RelayCommand(new Action<object>(DeleteContactInformationRecord));
                AddNewProfessionalContactInformationCommand = new DelegateCommand<object>(AddNewProfessionalContactInformation);
                EditProfessionalContactInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditProfessionalContactInformation));
                DeleteProfessionalContactCommand = new RelayCommand(new Action<object>(DeleteProfessionalContactInformationRecord));
                AddNewContractSituationInformationCommand = new DelegateCommand<object>(AddNewEmployeeContractSituation);
                EditContractSituationInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeContractSituation));
                DeleteContractSituationInformationCommand = new RelayCommand(new Action<object>(DeleteContractSituationInformationRecord));
                AddNewFamilyMemberInformationCommand = new DelegateCommand<object>(AddNewEmployeeFamilyMember);
                EditFamilyMemberInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeFamilyMember));
                DeleteFamilyMemberInformationCommand = new RelayCommand(new Action<object>(DeleteFamilyMemberInformationRecord));
                AddNewEducationQualificationInformationCommand = new DelegateCommand<object>(AddNewEmployeeEducationQualification);
                EditEducationInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeEducationQualification));
                DeleteEducationInformationCommand = new RelayCommand(new Action<object>(DeleteEducationQualificationInformationRecord));
                AddNewJobDescriptionCommand = new DelegateCommand<object>(AddNewEmployeeJobDescription);
                EditJobDescriptionDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeJobDescription));
                DeleteJobDescriptionCommand = new RelayCommand(new Action<object>(DeleteJobDescriptionRecord));
                AddNewProfessionalEducationCommand = new DelegateCommand<object>(AddNewProfessionalEducation);
                EditProfessionalEducationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeProfessionalEducation));
                DeleteProfessionalEducationCommand = new RelayCommand(new Action<object>(DeleteProfessionalEducationRecord));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                HyperlinkForEmailInGroupBox = new DelegateCommand<object>(new Action<object>((obj) => { SendMail(obj); }));
                SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
                // CompanyScheduleSelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { CompanyScheduleSelectedIndexChanged(obj); }));
                DeleteAuthorizedLeave = new RelayCommand(new Action<object>(DeleteAuthorizedLeaveRecord));
                ExitEventDeleteCommand = new RelayCommand(new Action<object>(ExitEventDeleteRecord));
                regex = new Regex(@"[~`!@#$%^&*_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);
                PrintEmployeeProfileCommand = new DelegateCommand<object>(PrintEmployeeProfileReport);
                AddAuthorizedLeaveCommand = new DelegateCommand<object>(AddAuthorizedLeave);
                EditAuthorizedLeaveDoubleClickCommand = new RelayCommand(new Action<object>(EditAuthorizedLeave));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileAction));
                SelectedItemChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SelectedItemChangedCommandAction);
                DocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeLeaveDocument));
                EmployeeStatusSelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { EmployeeStatusSelectedIndexChanged(obj); }));

                //[001] Added
                AddNewPolyvalenceCommand = new DelegateCommand<object>(AddNewEmployeePolyvalence);
                EditPolyvalenceDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeePolyvalence));
                DeletePolyvalenceCommand = new RelayCommand(new Action<object>(DeletePolyvalenceRecord));
                //[000] added
                PrintEmployeeIdBadgeCommand = new DelegateCommand<object>(PrintEmployeeIdBadgeAction);
                //[002]added
                AddNewEmployeeShiftCommand = new DelegateCommand<object>(AddEmployeeShift);
                DeleteEmployeeShiftCommand = new RelayCommand(new Action<object>(DeleteEmployeeShift));

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                //   LoadedEventCommand = new RelayCommand(new Action<object>(LoadedAction));
                LoadedEventCommand = new DelegateCommand<EventArgs>(LoadedAction);

                PrintEmployeeERFFormCommand = new DelegateCommand<object>(PrintEmployeeERFForm);
                ExportEmployeeProfileCommand = new DelegateCommand<object>(ExportEmployeeProfileCommandAction); //[nsatpute][26-03-2025][GEOS2-7011]
                #region chitra.girigosavi[21/08/2024] GEOS2-5579 HRM - New section in employee profile (1 of 3)
                AddEquipmentAndToolsFileCommand = new RelayCommand(new Action<object>(AddEquipmentAndToolsFileCommandAction));
                OpenEquipmentPDFDocumentCommand = new RelayCommand(new Action<object>(OpenEquipmentPDFDocument));
                EditEquipmentFileCommand = new RelayCommand(new Action<object>(EditEquipmentFile));
                DeleteEquipmentFileCommand = new RelayCommand(new Action<object>(DeleteEquipmentFile));
                #endregion

                AddEquipmentAndToolsFileFromRowCommand = new RelayCommand(new Action<object>(AddEquipmentAndToolsFileFromRowCommandAction));//[Sudhir.Jangra][GEOS2-5579]
                GeosApplication.Instance.Logger.Log("Constructor EmployeeProfileDetailViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeProfileDetailViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// When ever the changes in view values the command will execute
        /// </summary>
        //private void CommonValidation(object obj)
        //{
        //    if (allowValidation)
        //        EnableValidationAndGetError();
        //}

        /// <summary>
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// </summary>
        /// <param name="obj"></param>
        private void OpenEmployeeLeaveDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()...", category: Category.Info, priority: Priority.Low);
                //[001] added
                EmployeeExitEvent employeeExitEventObj = obj as EmployeeExitEvent;
                if (employeeExitEventObj != null)
                {
                    if (employeeExitEventObj.ExitEventBytes != null)
                    {
                        byte[] EmployeeExitAttachmentBytes = employeeExitEventObj.ExitEventBytes;
                        EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                        EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                        if (EmployeeExitAttachmentBytes != null)
                        {
                            employeeDocumentViewModel.OpenPdfFromBytes(EmployeeExitAttachmentBytes, employeeExitEventObj.FileName);
                            employeeDocumentView.DataContext = employeeDocumentViewModel;
                            employeeDocumentView.ShowDialog();
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }


                }

                GeosApplication.Instance.Logger.Log("Method OpenEmployeeLeaveDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// </summary>
        /// <param name="e"></param>
        public void SelectedItemChangedCommandAction(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()...", category: Category.Info, priority: Priority.Low);

                EmployeeExitEvent employeeExitEventObj = SelectedExitEventItem;

                if (e.NewValue == null)
                {

                    employeeExitEventObj.ExitEventattachmentList.Clear();
                    employeeExitEventObj.ExitEventBytes = null;
                    employeeExitEventObj.IsVisible = Visibility.Collapsed;
                    employeeExitEventObj.FileName = string.Empty;

                    // employeeExitEventObj.IsExitEventFileDeleted = true;
                    //old Code
                    //AttachmentList.Clear();
                    //FileInBytes = null;
                    //IsVisible = Visibility.Collapsed;
                    //FileName = string.Empty;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedItemChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            //[001]Added
            EmployeeExitEvent employeeExitEventObj = obj as EmployeeExitEvent;

            if (employeeExitEventObj != null)
            {

                if (!employeeExitEventObj.IsReadOnly)
                {

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    IsBusy = true;
                    try
                    {
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.DefaultExt = "Pdf Files|*.pdf";
                        dlg.Filter = "Pdf Files|*.pdf";

                        Nullable<bool> result = dlg.ShowDialog();

                        if (result == true)
                        {

                            employeeExitEventObj.ExitEventBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                            FileInfo file = new FileInfo(dlg.FileName);
                            //employeeExitEventObj.FileName = file.Name;
                            employeeExitEventObj.FileName = file.Name.Replace(".", "_" + employeeExitEventObj.IdExitEvent + ".");

                            ObservableCollection<Data.Common.Attachment> newAttachmentList = new ObservableCollection<Data.Common.Attachment>();
                            Data.Common.Attachment attachment = new Data.Common.Attachment();

                            attachment.FilePath = file.FullName;
                            attachment.OriginalFileName = file.Name;
                            attachment.IsDeleted = false;
                            attachment.FileByte = employeeExitEventObj.ExitEventBytes;

                            AttachmentObjectList = new List<object>();
                            AttachmentObjectList.Add(attachment);
                            newAttachmentList.Add(attachment);
                            employeeExitEventObj.ExitEventattachmentList = newAttachmentList;

                        }
                        IsBusy = false;


                        if (employeeExitEventObj.ExitEventattachmentList.Count > 0)
                        {
                            employeeExitEventObj.AttachedFile = employeeExitEventObj.ExitEventattachmentList[0];

                            employeeExitEventObj.IsVisible = Visibility.Visible;
                        }
                        else
                            employeeExitEventObj.IsVisible = Visibility.Hidden;

                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log(string.Format("Error in BrowseFile() BrowseFileAction - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                }


            }

            //old code
            //if (IsEnableRemark)
            //  {
            //      if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            //      IsBusy = true;
            //      try
            //      {
            //          Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //          dlg.DefaultExt = "Pdf Files|*.pdf";
            //          dlg.Filter = "Pdf Files|*.pdf";

            //          Nullable<bool> result = dlg.ShowDialog();

            //          if (result == true)
            //          {
            //              FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
            //              AttachmentList = new ObservableCollection<Data.Common.Attachment>();

            //              FileInfo file = new FileInfo(dlg.FileName);
            //              FileName = file.Name;

            //              ObservableCollection<Data.Common.Attachment> newAttachmentList = new ObservableCollection<Data.Common.Attachment>();

            //              Data.Common.Attachment attachment = new Data.Common.Attachment();
            //              attachment.FilePath = file.FullName;
            //              attachment.OriginalFileName = file.Name;
            //              attachment.IsDeleted = false;
            //              attachment.FileByte = FileInBytes;

            //              AttachmentObjectList = new List<object>();
            //              AttachmentObjectList.Add(attachment);

            //              newAttachmentList.Add(attachment);
            //              AttachmentList = newAttachmentList;

            //          }
            //          IsBusy = false;
            //          if (AttachmentList.Count > 0)
            //          {
            //              AttachedFile = AttachmentList[0];
            //              IsVisible = Visibility.Visible;
            //          }
            //          else
            //              IsVisible = Visibility.Hidden;

            //      }
            //      catch (Exception ex)
            //      {
            //          IsBusy = false;
            //          if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //          GeosApplication.Instance.Logger.Log(string.Format("Error in BrowseFile() BrowseFileAction - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            //      }
            //  }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// [003][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// [004][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [005][cpatil][25-12-2019][GEOS2-1941] GRHM - Calculation of holidays
        ///  [006][smazhar][08-11-2020][GEOS2-2498] Employee display name [#ERF67]
        ///  [007][cpatil][18-04-2022][GEOS2-3708] If user change clock id then system show validation error
        ///  [008][cpatil][13-09-2025][GEOS2-6971]
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="SelectedPeriod"></param>
        /// <param name="companyIds"></param>
        public void Init(Employee emp, long SelectedPeriod, string companyIds)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillGenderList();
                FillMaritalStatusList();
                FillExitReasonList();
                FillNationalityList();
                FillCountryList();
                FillLocationList();
                FillCountryRegionList();
                FillEmployeeStatusList();
                FillCityList();
                FillExtraHoursTime();//[Sudhir.Jangra][GEOS2-5273]
                #region Service Comments
                //   EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2035(emp.IdEmployee, SelectedPeriod, companyIds);//[004][002] Change Method
                //  EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2036(emp.IdEmployee, SelectedPeriod, companyIds);//[004][002] Change Method

                //[005] service method changed GetEmployeeByIdEmployee_V2036 to GetEmployeeByIdEmployee_V2038
                //EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2038(emp.IdEmployee, SelectedPeriod, companyIds);
                //change for main JD
                //[006] change service method
                // EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2046(emp.IdEmployee, SelectedPeriod, companyIds);

                //[2844]change service method
                //[007]service method changed GetEmployeeByIdEmployee_V2320 to GetEmployeeByIdEmployee_V2330 [GEOS2-2716][rdixit][31.10.2022]
                //[007]service method changed GetEmployeeByIdEmployee_V2330 to GetEmployeeByIdEmployee_V2400 [GEOS2-2456][rdixit][09.06.2023]
                //  EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2400(emp.IdEmployee, SelectedPeriod, companyIds);

                //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
                //   EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2410(emp.IdEmployee, SelectedPeriod, companyIds);

                //[Sudhir.jangra][GEOS2-4846]
                // EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2480(emp.IdEmployee, SelectedPeriod, companyIds);
                //[Sudhir.Jangra][GEOS2-5579]
                //[007]service method changed GetEmployeeByIdEmployee_V2580 to GetEmployeeByIdEmployee_V2590 [GEOS2-6571][rdixit][18.12.2024]
                //[008][cpatil][GEOS2-6971]service method changed for regularhr mismatch companyshift
                #endregion
                EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2670(emp.IdEmployee, SelectedPeriod, companyIds); // [nsatpute][09-01-2025][GEOS2-6776]                
                IdEmployee = emp.IdEmployee;
                RequiredEquipmentList = new ObservableCollection<EmployeeEquipmentAndTools>(EmployeeDetail.RequiredEquipmentList);
                EmployeeEquipmentList = new ObservableCollection<EmployeeEquipmentAndTools>(EmployeeDetail.EmployeeEquipmentList);

                if (EmployeeEquipmentList != null && EmployeeEquipmentList.Count > 0)
                {
                    if (ClonedEmployeeEquipmentList == null)
                    {
                        ClonedEmployeeEquipmentList = new List<EmployeeEquipmentAndTools>();
                    }
                    foreach (EmployeeEquipmentAndTools item in EmployeeEquipmentList)
                    {
                        ClonedEmployeeEquipmentList.Add((EmployeeEquipmentAndTools)item.Clone());
                    }
                }
                SelectedExtraHoursTime = ExtraHoursTimeList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.IdExtraHoursTime);
                JobDescriptionList = new ObservableCollection<JobDescription>();
                JobDescriptionList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---", JobDescriptionInUse = 1 });
                FillJobDescriptionList(EmployeeDetail.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate == null || x.JobDescriptionEndDate >= DateTime.Now).ToList());//[Sudhir.Jangra][GEOS2-4846]
                SelectedJobDescription = JobDescriptionList.IndexOf(x => x.IdJobDescription == EmployeeDetail.IdApprovalResponsible);

                string idsEmployeeJobDescription = null;
                string idsDepartment = null;
                if (EmployeeDetail.EmployeeJobDescriptions.Any(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)))
                {
                    idsEmployeeJobDescription = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Where(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)).Select(i => i.IdCompany).ToList());
                    idsDepartment = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Where(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)).Select(i => i.JobDescription.IdDepartment).ToList());
                }
                LstBackupEmployee = new ObservableCollection<Employee>(HrmService.GetBackupEmployeeDetails(idsEmployeeJobDescription, idsDepartment));
                LstBackupEmployee.Insert(0, new Employee() { FirstName = "---" });
                if (LstBackupEmployee.Any(i => i.IdEmployee == EmployeeDetail.IdEmployee))
                {
                    LstBackupEmployee.Remove(LstBackupEmployee.Where(i => i.IdEmployee == EmployeeDetail.IdEmployee).FirstOrDefault());
                }
                if (EmployeeDetail.IdEmployeeBackup != null && EmployeeDetail.IdEmployeeBackup > 0)
                {
                    if (!LstBackupEmployee.Any(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup))
                    {
                        Employee EmployeeBackup = new Employee();
                        EmployeeBackup.IdEmployee = EmployeeDetail.IdEmployeeBackup.Value;
                        if (EmployeeDetail.EmployeeBackup != null)
                        {
                            EmployeeBackup.EmployeeCode = EmployeeDetail.EmployeeBackup.EmployeeCode;
                            EmployeeBackup.IdEmployee = EmployeeDetail.EmployeeBackup.IdEmployee;
                            EmployeeBackup.FirstName = EmployeeDetail.EmployeeBackup.FirstName;
                            EmployeeBackup.LastName = EmployeeDetail.EmployeeBackup.LastName;
                            EmployeeBackup.Organization = EmployeeDetail.EmployeeBackup.Organization;
                            EmployeeBackup.IdEmployeeStatus = 137;

                            LstBackupEmployee.Add(EmployeeBackup);
                            SelectedIndexBackupEmployee = LstBackupEmployee.ToList().FindIndex(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup);
                        }
                    }
                    else
                    {
                        SelectedIndexBackupEmployee = LstBackupEmployee.ToList().FindIndex(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup); ;
                    }
                }
                else
                {
                    SelectedIndexBackupEmployee = 0;
                }

                IsLongTermAbsent = (EmployeeDetail.IsLongTermAbsent == 1) ? true : false;
                // FillCompanySchedule(string.Empty);
                int IdGender = Convert.ToInt32(EmployeeDetail.IdGender);
                if (IdGender == 1)
                {
                    SelectedIndexGender = 0;
                }
                else
                {
                    SelectedIndexGender = 1;
                }
                //for task 2844
                Remote = Convert.ToByte(EmployeeDetail.IsTrainer);

                if (Remote == 1)
                {
                    IsRemote = true;
                }
                else
                {
                    IsRemote = false;
                }
                // IsRemote = EmployeeDetail.IsTrainer;
                SelectedBoxIndex = 0;
                JobDescriptionError = null;
                ContractSituationError = null;

                EmployeeShiftError = null;

                SelectedMaritalStatusIndex = MaritalStatusList.FindIndex(x => x.IdLookupValue == EmployeeDetail.IdMaritalStatus);
                SelectedNationalityIndex = NationalityList.FindIndex(x => x.IdCountry == EmployeeDetail.IdNationality);
                Company SelectedCompany = null;
                //[GEOS2-6493][rdixit][17.01.2025]
                if (LocationList == null) LocationList = new ObservableCollection<Company>();
                if (LocationsList != null)
                    SelectedCompany = LocationsList.FirstOrDefault(x => x.IdCompany == EmployeeDetail.IdCompanyLocation);
                if (SelectedCompany != null)
                {
                    if (!LocationList.Any(i => i.IdCompany == SelectedCompany.IdCompany))
                    {
                        SelectedCompany.IsStillActive = 0;
                        LocationList.Add(SelectedCompany);
                    }
                    SelectedLocationIndex = LocationList.ToList().FindIndex(x => x.IdCompany == EmployeeDetail.IdCompanyLocation);
                }
                else
                {
                    if (EmployeeDetail.CompanyLocation != null)
                    {
                        EmployeeDetail.CompanyLocation.IsStillActive = 0;
                        LocationList.Add(EmployeeDetail.CompanyLocation);
                        SelectedLocationIndex = LocationList.ToList().FindIndex(x => x.IdCompany == EmployeeDetail.IdCompanyLocation);
                    }
                }

                if (SelectedLocationIndex == -1)
                    SelectedLocationIndex = 0;

                SelectedCountryIndex = CountryList.FindIndex(x => x.IdCountry == EmployeeDetail.AddressIdCountry);
                SelectedCountryRegion = EmployeeDetail.AddressRegion;
                SelectedDisability = EmployeeDetail.Disability;

                SelectedEmpolyeeStatusIndex = EmployeeStatusList.FindIndex(x => x.IdLookupValue == EmployeeDetail.IdEmployeeStatus);
                ExistJobDescriptionCompanyIdList = EmployeeDetail.EmployeeJobDescriptions.Select(x => x.Company.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();
                EmployeeContactList = new ObservableCollection<EmployeeContact>(EmployeeDetail.EmployeePersonalContacts.ToList());
                EmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>(EmployeeDetail.EmployeeProfessionalContacts.ToList());
                //[GEOS2-6499][04.11.2024][rdixit]
                if (GeosApplication.Instance.IsHRMManageEmployeeContactsPermission)
                    EmployeeDocumentList = new ObservableCollection<EmployeeDocument>(EmployeeDetail.EmployeeDocuments?.Where(i => i.EmployeeDocumentType.IdLookupValue == 80 || i.EmployeeDocumentType.IdLookupValue == 81));
                else
                    EmployeeDocumentList = new ObservableCollection<EmployeeDocument>(EmployeeDetail.EmployeeDocuments);
                EmployeeLanguageList = new ObservableCollection<EmployeeLanguage>(EmployeeDetail.EmployeeLanguages);
                EmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>(EmployeeDetail.EmployeeEducationQualifications);
                EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeDetail.EmployeeContractSituations);

                EmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>(EmployeeDetail.EmployeeFamilyMembers);

                //[sshegaonkar][GEOS2-2733][13-01-23]
                EmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeDetail.EmployeeJobDescriptions.OrderByDescending(x => x.IdEmployeeJobDescription).ToList());

                if (EmployeeDetail.EmployeeStatus.IdLookupValue == 137)
                {
                    foreach (EmployeeJobDescription Per in EmployeeJobDescriptionList)
                    {
                        if (Per.JobDescriptionUsage == 100)
                        {
                            Per.IsMainVisible = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).Select(a => a.JobDescriptionUsage).ToList();

                    if (Percentages.Count == 1 && Percentages.FirstOrDefault() == 100)
                    {
                        // if(EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                        EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList().ForEach(a => { a.IsMainVisible = Visibility.Hidden; });
                    }
                    else
                    {
                        if (!EmployeeJobDescriptionList.Any(a => (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null) && a.IsMainJobDescription == 1))
                        {
                            if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                            {
                                if (EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().JobDescriptionUsage == 100)
                                {
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Hidden;
                                }
                                else
                                {
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date && a.IsMainJobDescription == 1).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Visible;
                                }
                            }
                            else
                            {
                                if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)))
                                {
                                    if (EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().JobDescriptionUsage == 100)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Hidden;
                                    }
                                }
                                else if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionList.OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().JobDescriptionUsage == 100)
                                {
                                    EmployeeJobDescriptionList.OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Hidden;
                                }
                            }
                        }
                        else if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                        {
                            if (EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().JobDescriptionUsage == 100)
                            {
                                EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Hidden;
                            }
                            else
                            {
                                if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date && a.IsMainJobDescription == 1))
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date && a.IsMainJobDescription == 1).OrderByDescending(a => a.JobDescriptionUsage).FirstOrDefault().IsMainVisible = Visibility.Visible;
                            }
                        }
                    }
                }
                #region GreyImageVisible
                EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                //EmployeeJobDescriptionList.First().IsMainJobDescriptionGreyImageVisible = 1;
                //EmployeeJobDescriptionList.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                List<EmployeeJobDescription> finalEmployeeJobDescriptionList = new List<EmployeeJobDescription>();
                List<EmployeeJobDescription> EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                int JobDescriptionUsageCount = 0;
            Loop:
                foreach (EmployeeJobDescription employeeJobDescription in EmployeeJobDescriptionList)
                {
                    if (!finalEmployeeJobDescriptionList.Any(a => a == employeeJobDescription))
                        if (!EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a == employeeJobDescription))
                        {
                            if (employeeJobDescription.JobDescriptionEndDate == null)
                            {
                                if (employeeJobDescription.JobDescriptionUsage == 100)
                                {

                                }
                                else
                                {
                                    EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                    JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                    if (JobDescriptionUsageCount == 100)
                                    {
                                        if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                        {
                                            EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                            IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                            IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                            EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                            JobDescriptionUsageCount = 0;
                                        }
                                        else
                                        {
                                            if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                            {
                                                finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                JobDescriptionUsageCount = 0;
                                            }
                                            else
                                            {
                                                //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                                //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                JobDescriptionUsageCount = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        goto Loop;
                                    }
                                }

                            }
                            else
                            {

                                if (employeeJobDescription.JobDescriptionEndDate != null && employeeJobDescription.JobDescriptionStartDate != null)
                                {

                                    if (employeeJobDescription.JobDescriptionUsage == 100)
                                    {

                                    }
                                    else
                                    {
                                        EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                        JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                        if (JobDescriptionUsageCount == 100)
                                        {
                                            if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                            {
                                                EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                JobDescriptionUsageCount = 0;
                                            }
                                            else
                                            {
                                                if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                {
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }
                                                else
                                                {
                                                    //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                                    //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                    EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                    MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                    MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            goto Loop;
                                        }
                                    }

                                }
                            }

                        }

                }
                #region yellowstar
                if (EmployeeJobDescriptionListforIsMainJobDescription.Count > 0)
                {
                    foreach (EmployeeJobDescription EmployeeJobDescription in EmployeeJobDescriptionListforIsMainJobDescription)
                    {
                        if (EmployeeJobDescription.IsMainJobDescription == 1)
                        {
                            EmployeeJobDescription.IsMainJobDescription = 0;
                            var maxEmployeeJobDescriptionValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                            EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxEmployeeJobDescriptionValue);
                            MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                            MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                        }
                    }
                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                    if (EmployeeJobDescriptionListforIsMainJobDescription.Count == 1)
                    {
                        //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                        //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                    }
                    EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                    DateTime currentdatetime = DateTime.Now;
                    List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                    if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                    {
                        IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                        //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                        //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                        //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                        //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                        //var temp = EmployeeJobDescriptionList;
                        if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                        }
                        else
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }

                        }
                    }
                    else
                    {
                        IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                        if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }

                        }
                        else
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }

                        }
                        //if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                        //{
                        //    IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                        //    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                        //    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                        //    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                        //    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                        //    var temp = EmployeeJobDescriptionList;
                        //}
                    }
                }
                else
                {
                    DateTime currentdatetime = DateTime.Now;
                    List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                    if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                    {
                        IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                        if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                        }
                        else
                        {
                            EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            if (EmployeeJobDescription != null)
                            {
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }

                        }
                        //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                        //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                        //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                        //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                        //var temp = EmployeeJobDescriptionList;
                    }
                    else
                    {
                        IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                        if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                        {
                            IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                            if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }

                        }
                    }


                }
                #endregion
                #endregion
                EmployeeAllChangeLogList = new ObservableCollection<EmployeeChangelog>(EmployeeDetail.EmployeeChangelogs.OrderByDescending(x => x.IdEmployeeChangeLog).ToList());
                EmployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>(EmployeeDetail.EmployeeProfessionalEducations);
                EmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(EmployeeDetail.EmployeeAnnualLeaves);
                EmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(EmployeeDetail.EmployeePolyvalences);
                TempEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>();



                ////GEOS2-3456
                //foreach (EmployeeProfessionalEducation item in EmployeeProfessionalEducationList)
                //{
                //    if (item.IdType == 120)
                //        IsEnableForDeleteBtn = false;
                //    else
                //        IsEnableForDeleteBtn = true;
                //}
                //[003]Added
                // EmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeDetail.EmployeeShifts);
                //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
                EmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeDetail.EmployeeShifts);

                DateTime maxDate = EmployeeShiftList.Max(obj => obj.AccountingDate);
                DateTime minDate = EmployeeShiftList.Min(obj => obj.AccountingDate);
                bool ismorethanone = false;
                bool isFirstResult = true;
                if (EmployeeShiftList.Count > 1)
                {
                    ismorethanone = true;
                }

                foreach (var obj in EmployeeShiftList)
                {
                    if (ismorethanone == true)
                    {
                        if (obj.AccountingDate == maxDate && obj.AccountingDate != minDate && isFirstResult)
                        {
                            obj.IsMaxVisible = Visibility.Visible;
                            isFirstResult = false;
                        }
                        else
                        {
                            obj.IsMaxVisible = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        obj.IsMaxVisible = Visibility.Hidden;
                    }

                    // obj.IsMaxVisible = obj.AccountingDate == maxDate;
                }


                // TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());
                //[Sudhir.Jangra][GEOS2-4477][01/06/2023]
                TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.Where(use => use.CompanyShift.IsInUse == 1).OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());

                #region [rdixit][25.07.2023][GEOS4037]
                if (EmployeeShiftList != null)
                {
                    if (EmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                    {
                        if (!TopFourEmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                        {
                            if (TopFourEmployeeShiftList.Count == 4)
                                TopFourEmployeeShiftList.RemoveAt(3);
                            TopFourEmployeeShiftList.Insert(0, EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible));
                        }
                        else
                        {
                            EmployeeShift Fev = EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible);
                            TopFourEmployeeShiftList.Remove(Fev);
                            TopFourEmployeeShiftList.Insert(0, Fev);
                        }
                    }
                }
                #endregion

                if (EmployeeShiftList.Count > 4)
                    IsShiftAsteriskVisible = true;
                else
                    IsShiftAsteriskVisible = false;

                if (EmployeeAnnualLeaveList.Count > 0)
                {
                    if (EmployeeAnnualLeaveList[0].Employee.CompanyShift != null)
                    {
                        TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                    }
                    else
                    {
                        EmployeeAnnualLeaveList.Clear();
                    }
                }

                CurrentContractSituation = EmployeeContractSituationList.LastOrDefault();


                if (EmployeeContractSituationList.Count > 0)
                {
                    MinExitDate = EmployeeContractSituationList.OrderBy(i => i.ContractSituationStartDate).FirstOrDefault().ContractSituationStartDate;
                }

                EmployeeExistDetail = new Employee();
                EmployeeExistDetail = (Employee)EmployeeDetail.Clone();

                UpdatedEmployeeContactList = new ObservableCollection<EmployeeContact>();
                UpdatedEmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>();
                UpdatedEmployeeDocumentList = new ObservableCollection<EmployeeDocument>();
                UpdatedEmployeeLanguageList = new ObservableCollection<EmployeeLanguage>();
                UpdatedEmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>();
                UpdatedEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                UpdatedEmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>();
                // UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                UpdatedemployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>();
                UpdatedEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>();
                EmployeeChangeLogList = new ObservableCollection<EmployeeChangelog>();
                //[001] added
                UpdatedEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>();
                //end
                //ProfilePhotoBytes = GeosRepositoryServiceController.GetEmployeesImage(emp.EmployeeCode);
                //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
                //ProfilePhotoBytes = GetEmployeesImage(emp.EmployeeCode);
                //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                ProfilePhotoBytes = GetEmployeesImage_V2520(emp.EmployeeCode);
                UpdateEmployeeExitEventsList = new ObservableCollection<EmployeeExitEvent>();

                if (ProfilePhotoBytes != null)
                    OldProfilePhotoBytes = ProfilePhotoBytes.ToArray();

                ProfilePhotoSource = ByteArrayToPhotoSource(ProfilePhotoBytes);
                OldprofilePhotoSource = ProfilePhotoSource.Clone();
                //defaultPhotoSource = ProfilePhotoSource.Clone();

                FirstName = EmployeeDetail.FirstName;
                DisplayName = EmployeeDetail.DisplayName;
                LastName = EmployeeDetail.LastName;
                NativeName = EmployeeDetail.NativeName;

                SelectedCity = EmployeeDetail.AddressCity;
                Address = EmployeeDetail.AddressStreet;
                ZipCode = EmployeeDetail.AddressZipCode;
                BirthDate = EmployeeDetail.DateOfBirth;

                Latitude = EmployeeDetail.Latitude;
                Longitude = EmployeeDetail.Longitude;
                Remote = EmployeeDetail.IsTrainer;

                if (Latitude != null && Longitude != null)
                    MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);

                EmployeeCode = EmployeeDetail.EmployeeCode;
                if (!string.IsNullOrEmpty(EmployeeDetail.Remarks))
                    Remark = EmployeeDetail.Remarks.Trim();


                //if (CompanySchedules.FindIndex(c => c.CompanyShifts.Any(m => m.IdCompanyShift == EmployeeDetail.IdCompanyShift)) == -1)
                //    SelectedCompanyScheduleIndex = 0;
                //else
                //    SelectedCompanyScheduleIndex = CompanySchedules.FindIndex(c => c.CompanyShifts.Any(m => m.IdCompanyShift == EmployeeDetail.IdCompanyShift));

                // FillCompanyShift(); 

                //if (CompanyShifts.FindIndex(x => x.IdCompanyShift == EmployeeDetail.IdCompanyShift) == -1)
                //    SelectedCompanyShiftIndex = 0;
                //else
                //    SelectedCompanyShiftIndex = CompanyShifts.FindIndex(x => x.IdCompanyShift == EmployeeDetail.IdCompanyShift);

                DisplayEmployeeProfEmailAndPhone();
                //[004] added
                FillExitEventList();
                CalculateLengthOfService();
                CalculateAge();
                FillGetAllUpdatedEmployeeContract();
                //Shifts days order calc
                List<object> Days = GeosApplication.Instance.GetWeekNames();
                IndexSun = Days.FindIndex(x => x.ToString() == "Sunday") + 3;
                IndexMon = Days.FindIndex(x => x.ToString() == "Monday") + 3;
                IndexTue = Days.FindIndex(x => x.ToString() == "Tuesday") + 3;
                IndexWed = Days.FindIndex(x => x.ToString() == "Wednesday") + 3;
                IndexThu = Days.FindIndex(x => x.ToString() == "Thursday") + 3;
                IndexFri = Days.FindIndex(x => x.ToString() == "Friday") + 3;
                IndexSat = Days.FindIndex(x => x.ToString() == "Saturday") + 3;

                AddAdditionalBandGridColumns();
                if (EmployeeDetail.IsLongTermAbsent == 1)
                {
                    EmployeeDetail.EmployeeStatusWithLongTermAbsent = EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value + " - " + "Long Term Absent";
                }
                else
                {
                    EmployeeDetail.EmployeeStatusWithLongTermAbsent = EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value;
                }

                FillEmployeeListForProfessionalContactEmail();
                //HrmService = new HrmServiceController("localhost:6699");
                //[nsatpute][26-03-2025][GEOS2-7011]
                dictPetronalNumbers = HrmService.GetSitesPetronalNumbers();

                if (dictPetronalNumbers.ContainsKey(Convert.ToInt32(EmployeeDetail.IdCompanyLocation)))
                    ExportButtonVisible = Visibility.Visible;
                else
                    ExportButtonVisible = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillGetAllUpdatedEmployeeContract()
        {
            GetAllUpdatedEmployeeContracts = new List<EmployeeContractSituation>();
            foreach (EmployeeContractSituation item in EmployeeContractSituationList)
            {
                GetAllUpdatedEmployeeContracts.Add(item);
            }
        }
        /// <summary>
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// [003][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// [004][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [005][cpatil][25-12-2019][GEOS2-1941] GRHM - Calculation of holidays
        /// [007][cpatil][13-9-2025][GEOS2-6971]
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="SelectedPeriod"></param>
        /// <param name="companyIds"></param>
        public void InitReadOnly(Employee emp, long SelectedPeriod, string companyIds)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                FillGenderList();
                //[002],[004] added

                //  EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2035(emp.IdEmployee, SelectedPeriod, companyIds);
                //[005] service method changed GetEmployeeByIdEmployee_V2036 to GetEmployeeByIdEmployee_V2038
                //[006]service method changed GetEmployeeByIdEmployee_V2320 to GetEmployeeByIdEmployee_V2330 [GEOS2-2716][rdixit][31.10.2022]
                //[007][cpatil][GEOS2-6971]service method changed for regularhr mismatch companyshift
                EmployeeDetail = HrmService.GetEmployeeByIdEmployee_V2670(emp.IdEmployee, SelectedPeriod, companyIds);
                string idsEmployeeJobDescription = null;
                string idsDepartment = null;
                if (EmployeeDetail.EmployeeJobDescriptions.Any(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)))
                {
                    idsEmployeeJobDescription = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Where(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)).Select(i => i.IdCompany).ToList());
                    idsDepartment = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Where(i => DateTime.Now.Date >= i.JobDescriptionStartDate.Value.Date && DateTime.Now.Date <= (i.JobDescriptionEndDate == null ? DateTime.Now.Date : i.JobDescriptionEndDate.Value.Date)).Select(i => i.JobDescription.IdDepartment).ToList());
                }
                LstBackupEmployee = new ObservableCollection<Employee>(HrmService.GetBackupEmployeeDetails(idsEmployeeJobDescription, idsDepartment));
                LstBackupEmployee.Insert(0, new Employee() { FirstName = "---" });
                if (LstBackupEmployee.Any(i => i.IdEmployee == EmployeeDetail.IdEmployee))
                {
                    LstBackupEmployee.Remove(LstBackupEmployee.Where(i => i.IdEmployee == EmployeeDetail.IdEmployee).FirstOrDefault());
                }
                if (EmployeeDetail.IdEmployeeBackup != null && EmployeeDetail.IdEmployeeBackup > 0)
                {
                    if (!LstBackupEmployee.Any(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup))
                    {
                        Employee EmployeeBackup = new Employee();
                        EmployeeBackup.IdEmployee = EmployeeDetail.IdEmployeeBackup.Value;
                        if (EmployeeDetail.EmployeeBackup != null)
                        {
                            EmployeeBackup.EmployeeCode = EmployeeDetail.EmployeeBackup.EmployeeCode;
                            EmployeeBackup.IdEmployee = EmployeeDetail.EmployeeBackup.IdEmployee;
                            EmployeeBackup.FirstName = EmployeeDetail.EmployeeBackup.FirstName;
                            EmployeeBackup.LastName = EmployeeDetail.EmployeeBackup.LastName;
                            EmployeeBackup.Organization = EmployeeDetail.EmployeeBackup.Organization;
                            EmployeeBackup.IdEmployeeStatus = 137;
                            LstBackupEmployee.Add(EmployeeBackup);
                            SelectedIndexBackupEmployee = LstBackupEmployee.ToList().FindIndex(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup);
                        }
                    }
                    else
                    {
                        SelectedIndexBackupEmployee = LstBackupEmployee.ToList().FindIndex(i => i.IdEmployee == EmployeeDetail.IdEmployeeBackup); ;
                    }
                }
                else
                {
                    SelectedIndexBackupEmployee = 0;
                }
                //[003] Added
                EmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeDetail.EmployeeShifts);

                //FillCompanySchedule(string.Empty); 
                IsLongTermAbsent = (EmployeeDetail.IsLongTermAbsent == 1) ? true : false;

                if (ProfilePhotoSource == null)
                {
                    ProfilePhotoSource = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
                    IsPhoto = false;
                }

                int IdGender = Convert.ToInt32(EmployeeDetail.IdGender);

                if (IdGender == 1)
                {
                    SelectedIndexGender = 0;
                }
                else
                {
                    SelectedIndexGender = 1;
                }

                SelectedBoxIndex = 0;
                JobDescriptionError = null;
                ContractSituationError = null;
                EmployeeShiftError = null;

                MaritalStatusList = new List<LookupValue>();
                MaritalStatusList.Add(EmployeeDetail.MaritalStatus);

                NationalityList = new List<Country>();
                NationalityList.Add(EmployeeDetail.Nationality);

                CountryList = new List<Country>();
                CountryList.Add(EmployeeDetail.AddressCountry);

                CountryWiseRegions = new List<string>();
                CountryWiseRegions.Add(EmployeeDetail.AddressRegion);

                EmployeeStatusList = new List<LookupValue>();
                EmployeeStatusList.Add(EmployeeDetail.EmployeeStatus);

                CityList = new List<Data.Common.City>();
                CityList.Insert(0, new City() { Name = EmployeeDetail.AddressCity });

                SelectedMaritalStatusIndex = 0;
                SelectedNationalityIndex = 0;
                SelectedCountryIndex = 0;
                SelectedLocationIndex = 0;

                SelectedCountryRegion = EmployeeDetail.AddressRegion;
                SelectedDisability = EmployeeDetail.Disability;

                SelectedEmpolyeeStatusIndex = 0;
                ExistJobDescriptionCompanyIdList = EmployeeDetail.EmployeeJobDescriptions.Select(x => x.Company.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();
                EmployeeContactList = new ObservableCollection<EmployeeContact>(EmployeeDetail.EmployeePersonalContacts.ToList());
                EmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>(EmployeeDetail.EmployeeProfessionalContacts.ToList());
                EmployeeDocumentList = new ObservableCollection<EmployeeDocument>(EmployeeDetail.EmployeeDocuments);
                EmployeeLanguageList = new ObservableCollection<EmployeeLanguage>(EmployeeDetail.EmployeeLanguages);
                EmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>(EmployeeDetail.EmployeeEducationQualifications);
                EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeDetail.EmployeeContractSituations);
                EmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>(EmployeeDetail.EmployeeFamilyMembers);

                EmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeDetail.EmployeeJobDescriptions);
                if (!EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                {
                    List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();
                    if (Percentages.Count > 1)
                    {
                        List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList();
                        if (JDList_Top.Count == 1)
                        {
                            EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                        }
                    }
                }

                EmployeeAllChangeLogList = new ObservableCollection<EmployeeChangelog>(EmployeeDetail.EmployeeChangelogs.OrderByDescending(x => x.IdEmployeeChangeLog).ToList());
                EmployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>(EmployeeDetail.EmployeeProfessionalEducations);
                EmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(EmployeeDetail.EmployeeAnnualLeaves);
                EmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>(EmployeeDetail.EmployeePolyvalences);
                TempEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>();

                if (EmployeeAnnualLeaveList.Count > 0)
                {
                    if (EmployeeAnnualLeaveList[0].Employee.CompanyShift != null)
                    {
                        TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                    }
                    else
                    {
                        EmployeeAnnualLeaveList.Clear();
                    }
                }

                if (EmployeeContractSituationList.Count > 0)
                {
                    MinExitDate = EmployeeContractSituationList.OrderBy(i => i.ContractSituationStartDate).FirstOrDefault().ContractSituationStartDate;
                }

                EmployeeExistDetail = new Employee();
                EmployeeExistDetail = (Employee)EmployeeDetail.Clone();

                UpdatedEmployeeContactList = new ObservableCollection<EmployeeContact>();
                UpdatedEmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>();
                UpdatedEmployeeDocumentList = new ObservableCollection<EmployeeDocument>();
                UpdatedEmployeeLanguageList = new ObservableCollection<EmployeeLanguage>();
                UpdatedEmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>();
                UpdatedEmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                UpdatedEmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>();
                //UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                UpdatedemployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>();
                //[001] Added
                UpdatedEmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>();
                EmployeeChangeLogList = new ObservableCollection<EmployeeChangelog>();
                //ProfilePhotoBytes = GeosRepositoryServiceController.GetEmployeesImage(emp.EmployeeCode);
                //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
                //ProfilePhotoBytes = GetEmployeesImage(emp.EmployeeCode);
                //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                ProfilePhotoBytes = GetEmployeesImage_V2520(emp.EmployeeCode);
                ProfilePhotoSource = ByteArrayToPhotoSource(ProfilePhotoBytes);
                OldprofilePhotoSource = ProfilePhotoSource.Clone();

                UpdateEmployeeExitEventsList = new ObservableCollection<EmployeeExitEvent>();


                FirstName = EmployeeDetail.FirstName;
                LastName = EmployeeDetail.LastName;
                DisplayName = employeeDetail.DisplayName;
                NativeName = EmployeeDetail.NativeName;

                SelectedCity = EmployeeDetail.AddressCity;
                Address = EmployeeDetail.AddressStreet;
                ZipCode = EmployeeDetail.AddressZipCode;
                BirthDate = EmployeeDetail.DateOfBirth;

                Latitude = EmployeeDetail.Latitude;
                Longitude = EmployeeDetail.Longitude;
                Remote = EmployeeDetail.IsTrainer;
                if (Latitude != null && Longitude != null)
                    MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);

                EmployeeCode = EmployeeDetail.EmployeeCode;
                if (!string.IsNullOrEmpty(EmployeeDetail.Remarks))
                    Remark = EmployeeDetail.Remarks.Trim();

                //if (CompanySchedules.FindIndex(c => c.CompanyShifts.Any(m => m.IdCompanyShift == EmployeeDetail.IdCompanyShift)) == -1)
                //    SelectedCompanyScheduleIndex = 0;
                //else
                //    SelectedCompanyScheduleIndex = CompanySchedules.FindIndex(c => c.CompanyShifts.Any(m => m.IdCompanyShift == EmployeeDetail.IdCompanyShift));

                //FillCompanyShift(); 

                //if (CompanyShifts.FindIndex(x => x.IdCompanyShift == EmployeeDetail.IdCompanyShift) == -1)
                //    SelectedCompanyShiftIndex = 0;
                //else
                //    SelectedCompanyShiftIndex = CompanyShifts.FindIndex(x => x.IdCompanyShift == EmployeeDetail.IdCompanyShift);

                DisplayEmployeeProfEmailAndPhone();
                //[004] added
                FillExitEventList();
                CalculateLengthOfService();
                CalculateAge();
                FillExitReasonList();
                AddAdditionalBandGridColumns();
                if (EmployeeDetail.IsLongTermAbsent == 1)
                {
                    EmployeeDetail.EmployeeStatusWithLongTermAbsent = EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value + " - " + "Long Term Absent";
                }
                else
                {
                    EmployeeDetail.EmployeeStatusWithLongTermAbsent = EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value;
                }
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InitReadOnly() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Display Employee Professional Email And Phone
        /// </summary>
        private void DisplayEmployeeProfEmailAndPhone()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeProfEmailAndPhone()...", category: Category.Info, priority: Priority.Low);
                if (EmployeeProfessionalContactList.Count > 0)
                {
                    ProfContact = EmployeeProfessionalContactList.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88);
                    if (ProfContact != null)
                    {
                        ProfessionalEmail = ProfContact.EmployeeContactValue;
                    }
                    else
                    {
                        ProfessionalEmail = string.Empty;
                    }
                    ProfContact = EmployeeProfessionalContactList.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90);
                    if (ProfContact != null)
                    {
                        ProfessionalPhone = ProfContact.EmployeeContactValue;
                    }
                    else
                    {
                        ProfessionalPhone = string.Empty;
                    }
                }

                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeProfEmailAndPhone()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DisplayEmployeeProfEmailAndPhone()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Calculate Length Of Servoce in Contract Situation
        /// [HRM-M049-35] Display length of service months in employee profile [adadibathina][23102018]
        /// [GEOS2-2111] HRM Bug- Length of service [avpawar][12-02-2020]
        /// Added lengthOfServiceYears and lengthOfServiceMonths
        /// </summary>
        /// 
        private void CalculateLengthOfService()
        {
            try
            {
                //[rdixit][GEOS2-5657][11.03.2025]
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()...", category: Category.Info, priority: Priority.Low);


                // Calculate year and month difference
                if (EmployeeContractSituationList != null)
                {
                    List<EmployeeContractSituation> ContractList = EmployeeContractSituationList.Select(i => (EmployeeContractSituation)i.Clone()).OrderBy(j => j.ContractSituationStartDate).ToList();

                    if (ContractList.Count > 0)
                    {
                        var lastExitEvent = ExitEventList?.OrderByDescending(i => i.ExitDate).FirstOrDefault();

                        if (lastExitEvent?.ExitDate != null)
                        {
                            var Newcontract = ContractList.Where(i => i.ContractSituationStartDate.Value.Date > lastExitEvent.ExitDate.Value.Date).FirstOrDefault();

                            if (Newcontract == null)
                            {
                                startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                endDate = contract?.ContractSituationEndDate ?? DateTime.MinValue; // or handle null properly
                            }
                            else
                            {
                                startDate = Convert.ToDateTime(Newcontract.ContractSituationStartDate);
                                var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                //[rdixit][GEOS2-7877][16.04.2025]
                                if (startDate > DateTime.Today)
                                    endDate = startDate;
                                else
                                    endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                            }
                        }
                        else
                        {
                            startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                            var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                            //[rdixit][GEOS2-7877][16.04.2025]
                            if (startDate > DateTime.Today)
                                endDate = startDate;
                            else
                                endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                        }
                        int year = endDate.Year - startDate.Year;
                        int month = endDate.Month - startDate.Month;
                        int day = endDate.Day - startDate.Day;
                        if (day < 0)
                        {
                            month -= 1;
                            DateTime previousMonth = endDate.AddMonths(-1);
                            day += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                        }
                        if (month < 0)
                        {
                            year -= 1;
                            month += 12;
                        }
                        LengthOfService = Convert.ToString(year) + "Y  " + Convert.ToString(month) + "M";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateLengthOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill Employee Status List ---Sprint-40 ---Task-[HRM-M040-07] Add employee status field---sdesai
        /// </summary>
        private void FillEmployeeStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(29);
                EmployeeStatusList = new List<LookupValue>();
                EmployeeStatusList = new List<LookupValue>(tempCountryList);
                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 138);

                GeosApplication.Instance.Logger.Log("Method FillEmployeeStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Gender List . 
        /// </summary>
        private void FillGenderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGenderList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(1);
                GenderList = new List<LookupValue>();
                //GenderList.Insert(0, new LookupValue() { Value = "---" });
                GenderList.AddRange(temptypeList);

                GeosApplication.Instance.Logger.Log("Method FillGenderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGenderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Fill MaritalStatus List . 
        /// </summary>
        private void FillMaritalStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMaritalStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(13);
                MaritalStatusList = new List<LookupValue>();
                MaritalStatusList = new List<LookupValue>(tempCountryList);
                MaritalStatusList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillMaritalStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMaritalStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Exit Reson List . 
        /// </summary>
        private void FillExitReasonList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExitReasonList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmStartUp.GetLookupValues(31);
                ExitReasonList = new List<LookupValue>();
                ExitReasonList = new List<LookupValue>(tempList);
                ExitReasonList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillExitReasonList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillExitReasonList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Nationality List . 
        /// </summary>
        private void FillNationalityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillNationalityList()...", category: Category.Info, priority: Priority.Low);

                IList<Country> temptypeList = HrmService.GetAllCountries();
                NationalityList = new List<Country>();
                NationalityList.Insert(0, new Country() { Name = "---" });
                NationalityList.AddRange(temptypeList);

                GeosApplication.Instance.Logger.Log("Method FillNationalityList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillNationalityList() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Country Region List . 
        /// </summary>
        private void FillCountryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountryList()...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = HrmService.GetAllCountries();
                //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 18 12 2023
                foreach (Country Countryitem in tempCountryList)
                {
                    Countryitem.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + Countryitem.Iso + ".png";
                }
                CountryList = new List<Country>();
                CountryList.Insert(0, new Country() { Name = "---" });
                CountryList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillCountryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Method FillCountryList().- ", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// this method use for fill location
        /// </summary>
        private void FillLocationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationList()...", category: Category.Info, priority: Priority.Low);
                //Service called here to get updated changes.[rdixit][25.09.2023]
                LocationsList = new ObservableCollection<Company>(HrmService.GetAuthorizedPlantsByIdUser_V2480(GeosApplication.Instance.ActiveUser.IdUser)); ////[GEOS2-6493][rdixit][17.01.2025] updated latest version of sp
                LocationList = new ObservableCollection<Company>();
                LocationList.Insert(0, new Company() { Alias = "---" });
                if (LocationsList != null)
                {
                    LocationList.AddRange(LocationsList.Where(i => i.IsStillActive == 1 && i.IsLocation == 1).ToList()?.OrderBy(x => x.Country.Name).ThenBy(x => x.Alias));
                }

                GeosApplication.Instance.Logger.Log("Method FillLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Validation on First Name Last Name and Native Name . 
        /// </summary>
        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                if (IsFromFirstName)
                    firstNameErrorMsg = string.Empty;
                if (IsFromLastName)
                    lastNameErrorMsg = string.Empty;
                if (IsFromNativeName)
                    nativeNameErrorMsg = string.Empty;
                if (IsFromCity)
                    cityErrorMsg = string.Empty;
                if (IsFromRegion)
                    regionErrorMsg = string.Empty;

                var newInput = (string)e.NewValue;

                if (!string.IsNullOrEmpty(newInput))
                {
                    MatchCollection matches = regex.Matches(newInput.ToLower().ToString());
                    if (newInput.Count(char.IsDigit) > 0 || matches.Count > 0)
                    {
                        if (IsFromFirstName)
                        {
                            firstNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }

                        if (IsFromLastName)
                        {
                            lastNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }

                        if (IsFromNativeName)
                        {
                            nativeNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }

                        if (IsFromCity)
                        {
                            cityErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }

                        if (IsFromRegion)
                        {
                            regionErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }
                    }

                    if (!HrmCommon.Instance.IsPermissionReadOnly)
                        error = EnableValidationAndGetError();

                    if (IsFromFirstName)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                    if (IsFromLastName)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                    if (IsFromNativeName)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NativeName"));
                    }
                    if (IsFromCity)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("City"));
                    }
                    if (IsFromRegion)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Region"));
                    }

                    IsFromFirstName = false;
                    IsFromLastName = false;
                    IsFromNativeName = false;
                    IsFromCity = false;
                    IsFromRegion = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnTextEditValueChanging() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Country Region List . 
        /// </summary>
        private void FillCountryRegionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountryRegionList()...", category: Category.Info, priority: Priority.Low);

                IList<CountryRegion> tempCountryList = HrmService.GetAllCountryRegions();
                AllCountryRegionList = new List<CountryRegion>();
                AllCountryRegionList.Insert(0, new CountryRegion() { Name = "---" });
                AllCountryRegionList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillCountryRegionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryRegionList() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method flor ByteArray To PhotoSource . 
        /// </summary>
        public ImageSource ByteArrayToPhotoSource(byte[] byteArrayIn)
        {
            //BitmapImage bitmapImg = new BitmapImage();
            //if (byteArrayIn != null)
            //{
            //    MemoryStream ms = new MemoryStream(byteArrayIn);
            //    bitmapImg.BeginInit();
            //    bitmapImg.StreamSource = ms;
            //    bitmapImg.EndInit();
            //    //bitmapImg.DecodePixelHeight = 10;
            //    //bitmapImg.DecodePixelWidth = 10;

            //    ImageSource imgSrc = bitmapImg as ImageSource;
            //    return imgSrc;
            //}

            try
            {
                GeosApplication.Instance.Logger.Log("Method convert ByteArrayToPhotoSource ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToPhotoSource() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for Add New Identification Document.
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void AddNewIdentificationDocument(object obj)
        {
            try
            {
                SetUserPermission();

                GeosApplication.Instance.Logger.Log("Method AddNewIdentificationDocument()...", category: Category.Info, priority: Priority.Low);

                AddIdentificationDocumentView addIdentificationDocumentView = new AddIdentificationDocumentView();
                AddIdentificationDocumentViewModel addIdentificationDocumentViewModel = new AddIdentificationDocumentViewModel();
                EventHandler handle = delegate { addIdentificationDocumentView.Close(); };
                addIdentificationDocumentViewModel.RequestClose += handle;
                addIdentificationDocumentView.DataContext = addIdentificationDocumentViewModel;

                EmployeeContractSituation empActiveContract = (EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationEndDate == null || (x.ContractSituationEndDate.HasValue && x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)));

                addIdentificationDocumentViewModel.Init(EmployeeDocumentList, EmployeeDetail.IdEmployee, empActiveContract);
                addIdentificationDocumentViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addIdentificationDocumentView.Owner = Window.GetWindow(ownerInfo);
                addIdentificationDocumentView.ShowDialog();

                if (addIdentificationDocumentViewModel.IsSave == true)
                {
                    addIdentificationDocumentViewModel.NewDocument.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeDocumentList.Add(addIdentificationDocumentViewModel.NewDocument);
                    UpdatedEmployeeDocumentList.Add(addIdentificationDocumentViewModel.NewDocument);
                    SelectedDocumentRow = addIdentificationDocumentViewModel.NewDocument;
                    //[001]
                    foreach (EmployeeDocument itemEmployeeDocument in EmployeeDocumentList)
                    {
                        if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                        {

                            if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate >= DateTime.Now) && i.EmployeeDocumentIdType == itemEmployeeDocument.EmployeeDocumentIdType) && (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138))
                            {
                                itemEmployeeDocument.IsgreaterJobDescriptionthanToday = true;
                            }
                            else if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate < DateTime.Now)))
                            {
                                itemEmployeeDocument.IsgreaterJobDescriptionthanToday = false;
                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddNewIdentificationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddNewIdentificationDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Document Information. 
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void EditDocumentInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDocumentInformation()...", category: Category.Info, priority: Priority.Low);
                string ExistFileName = null;
                TableView detailView = (TableView)obj;

                EmployeeDocument empDocument = (EmployeeDocument)detailView.DataControl.CurrentItem;
                SelectedDocumentRow = empDocument;

                if (empDocument != null)
                {
                    var existEmployeeDocumentRecord = EmployeeExistDetail.EmployeeDocuments.Where(x => x.IdEmployeeDocument == empDocument.IdEmployeeDocument).FirstOrDefault();
                    if (existEmployeeDocumentRecord != null)
                    {
                        ExistFileName = existEmployeeDocumentRecord.EmployeeDocumentFileName;
                    }

                    AddIdentificationDocumentView addIdentificationDocumentView = new AddIdentificationDocumentView();
                    AddIdentificationDocumentViewModel addIdentificationDocumentViewModel = new AddIdentificationDocumentViewModel();
                    EventHandler handle = delegate { addIdentificationDocumentView.Close(); };
                    addIdentificationDocumentViewModel.RequestClose += handle;
                    addIdentificationDocumentView.DataContext = addIdentificationDocumentViewModel;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addIdentificationDocumentViewModel.InitReadOnly(empDocument);
                    else
                    {
                        EmployeeContractSituation empActiveContract = (EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationEndDate == null || (x.ContractSituationEndDate.HasValue && x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)));
                        addIdentificationDocumentViewModel.EditInit(EmployeeDocumentList, empDocument, EmployeeDetail.IdEmployee, empActiveContract);
                    }

                    addIdentificationDocumentViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addIdentificationDocumentView.Owner = Window.GetWindow(ownerInfo);
                    addIdentificationDocumentView.ShowDialog();

                    if (addIdentificationDocumentViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeDocumentList.Count != 0)
                        {
                            var existEmployeeDocument = UpdatedEmployeeDocumentList.Where(x => x.IdEmployeeDocument == empDocument.IdEmployeeDocument).FirstOrDefault();
                            UpdatedEmployeeDocumentList.Remove(existEmployeeDocument);
                        }

                        if (empDocument.IdEmployeeDocument == 0)
                        {
                            addIdentificationDocumentViewModel.EditDocument.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empDocument.EmployeeDocumentType = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentType;
                        empDocument.EmployeeDocumentIdType = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentIdType;
                        empDocument.EmployeeDocumentName = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentName;
                        empDocument.EmployeeDocumentNumber = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentNumber;
                        empDocument.EmployeeDocumentIssueDate = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentIssueDate;
                        empDocument.EmployeeDocumentExpiryDate = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentExpiryDate;
                        empDocument.EmployeeDocumentFileName = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentFileName;
                        empDocument.EmployeeDocumentFileInBytes = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentFileInBytes;
                        empDocument.EmployeeDocumentRemarks = addIdentificationDocumentViewModel.EditDocument.EmployeeDocumentRemarks;
                        empDocument.TransactionOperation = addIdentificationDocumentViewModel.EditDocument.TransactionOperation;

                        if (!string.IsNullOrEmpty(ExistFileName) && ExistFileName != empDocument.EmployeeDocumentFileName)
                        {
                            empDocument.OldFileName = ExistFileName;
                        }
                        else
                        {
                            empDocument.OldFileName = null;
                        }

                        UpdatedEmployeeDocumentList.Add(empDocument);
                        SelectedDocumentRow = empDocument;

                        //[001]
                        foreach (EmployeeDocument itemEmployeeDocument in EmployeeDocumentList)
                        {
                            if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                            {

                                if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate >= DateTime.Now) && i.EmployeeDocumentIdType == itemEmployeeDocument.EmployeeDocumentIdType) && (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138))
                                {
                                    itemEmployeeDocument.IsgreaterJobDescriptionthanToday = true;
                                }
                                else if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate < DateTime.Now)))
                                {
                                    itemEmployeeDocument.IsgreaterJobDescriptionthanToday = false;
                                }
                            }

                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditDocumentInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditDocumentInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Document Information Record. 
        /// [Hrm] [25/10/2018] [adadibathina]
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void DeleteDocumentInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDocumentInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeDocument empDoc = (EmployeeDocument)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empDoc.IdEmployeeDocument != 0)
                    {
                        empDoc.IsEmployeeDocumentFileDeleted = true;
                        empDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeDocumentList.Add(empDoc);
                    }
                    else
                    {
                        UpdatedEmployeeDocumentList.Remove(empDoc);
                    }
                    EmployeeDocumentList.Remove(empDoc);

                    //[001]
                    foreach (EmployeeDocument itemEmployeeDocument in EmployeeDocumentList)
                    {
                        if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                        {

                            if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate >= DateTime.Now) && i.EmployeeDocumentIdType == itemEmployeeDocument.EmployeeDocumentIdType) && (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138))
                            {
                                itemEmployeeDocument.IsgreaterJobDescriptionthanToday = true;
                            }
                            else if (EmployeeDocumentList.Any(i => (i.EmployeeDocumentExpiryDate == null || i.EmployeeDocumentExpiryDate < DateTime.Now)))
                            {
                                itemEmployeeDocument.IsgreaterJobDescriptionthanToday = false;
                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteDocumentInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDocumentInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Open Employee Education Document. 
        /// </summary>
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

        /// <summary>
        /// Method for Add New Language Information. 
        /// </summary>

        private void AddNewLanguageInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewLanguage()...", category: Category.Info, priority: Priority.Low);

                AddLanguagesView addLanguagesView = new AddLanguagesView();
                AddLanguagesViewModel addLanguagesViewModel = new AddLanguagesViewModel();
                EventHandler handle = delegate { addLanguagesView.Close(); };
                addLanguagesViewModel.RequestClose += handle;
                addLanguagesView.DataContext = addLanguagesViewModel;
                addLanguagesViewModel.Init(EmployeeLanguageList);
                addLanguagesViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addLanguagesView.Owner = Window.GetWindow(ownerInfo);
                addLanguagesView.ShowDialog();

                if (addLanguagesViewModel.IsSave == true)
                {
                    addLanguagesViewModel.NewLanguage.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeLanguageList.Add(addLanguagesViewModel.NewLanguage);
                    UpdatedEmployeeLanguageList.Add(addLanguagesViewModel.NewLanguage);
                    SelectedLanguageRow = addLanguagesViewModel.NewLanguage;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Edit Language Information. 
        /// </summary>
        private void EditLanguageInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditLanguageInformation()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeLanguage empLanguage = (EmployeeLanguage)detailView.DataControl.CurrentItem;
                SelectedLanguageRow = empLanguage;
                if (empLanguage != null)
                {
                    AddLanguagesView addLanguagesView = new AddLanguagesView();
                    AddLanguagesViewModel addLanguagesViewModel = new AddLanguagesViewModel();
                    EventHandler handle = delegate { addLanguagesView.Close(); };
                    addLanguagesViewModel.RequestClose += handle;
                    addLanguagesView.DataContext = addLanguagesViewModel;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addLanguagesViewModel.InitReadOnly(empLanguage);
                    else
                        addLanguagesViewModel.EditInit(EmployeeLanguageList, empLanguage);

                    addLanguagesViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addLanguagesView.Owner = Window.GetWindow(ownerInfo);
                    addLanguagesView.ShowDialog();
                    if (addLanguagesViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeLanguageList.Count != 0)
                        {
                            var existEmployeeLanguage = UpdatedEmployeeLanguageList.Where(x => x.IdEmployeeLanguage == empLanguage.IdEmployeeLanguage).FirstOrDefault();
                            UpdatedEmployeeLanguageList.Remove(existEmployeeLanguage);
                        }
                        addLanguagesViewModel.EditLanguage.IdEmployee = EmployeeDetail.IdEmployee;
                        addLanguagesViewModel.EditLanguage.IdEmployeeLanguage = empLanguage.IdEmployeeLanguage;
                        if (empLanguage.IdEmployeeLanguage == 0)
                        {
                            addLanguagesViewModel.EditLanguage.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }
                        empLanguage.IdLanguage = addLanguagesViewModel.EditLanguage.IdLanguage;
                        empLanguage.Language = addLanguagesViewModel.EditLanguage.Language;
                        empLanguage.LanguageRemarks = addLanguagesViewModel.EditLanguage.LanguageRemarks;
                        empLanguage.SpeakingIdLanguageLevel = addLanguagesViewModel.EditLanguage.SpeakingIdLanguageLevel;
                        empLanguage.SpeakingLevel = addLanguagesViewModel.EditLanguage.SpeakingLevel;
                        empLanguage.UnderstandingIdLanguageLevel = addLanguagesViewModel.EditLanguage.UnderstandingIdLanguageLevel;
                        empLanguage.UnderstandingLevel = addLanguagesViewModel.EditLanguage.UnderstandingLevel;
                        empLanguage.WritingIdLanguageLevel = addLanguagesViewModel.EditLanguage.WritingIdLanguageLevel;
                        empLanguage.WritingLevel = addLanguagesViewModel.EditLanguage.WritingLevel;
                        empLanguage.TransactionOperation = addLanguagesViewModel.EditLanguage.TransactionOperation;

                        UpdatedEmployeeLanguageList.Add(addLanguagesViewModel.EditLanguage);
                        SelectedLanguageRow = empLanguage;


                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditLanguageInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditLanguageInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Language Information Record. 
        /// </summary>
        private void DeleteLanguageInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteLanguageInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLanguageInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeLanguage empEmployeeLanguage = (EmployeeLanguage)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empEmployeeLanguage.IdEmployeeLanguage != 0)
                    {
                        empEmployeeLanguage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeLanguageList.Add(empEmployeeLanguage);
                    }
                    EmployeeLanguageList.Remove(empEmployeeLanguage);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteLanguageInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DeleteLanguageInformationRecord() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add New Personal Contact Information. 
        /// </summary>
        private void AddNewContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewContactInformation()...", category: Category.Info, priority: Priority.Low);
                // GridCard detailView = (GridCard)obj;

                AddContactInformationView addContactInformationView = new AddContactInformationView();
                AddContactInformationViewModel addContactInformationViewModel = new AddContactInformationViewModel();
                EventHandler handle = delegate { addContactInformationView.Close(); };
                addContactInformationViewModel.RequestClose += handle;
                addContactInformationView.DataContext = addContactInformationViewModel;
                addContactInformationViewModel.Init(EmployeeContactList);
                addContactInformationViewModel.WindowHeader = Application.Current.FindResource("AddPersonalContactInformation").ToString();
                addContactInformationViewModel.IsNew = true;
                addContactInformationViewModel.IsPersonalContact = true;
                addContactInformationViewModel.IsCompanyUse = false;
                var ownerInfo = (obj as FrameworkElement);
                addContactInformationView.Owner = Window.GetWindow(ownerInfo);
                addContactInformationView.ShowDialog();

                if (addContactInformationViewModel.IsSave == true)
                {
                    addContactInformationViewModel.NewContact.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeContactList.Add(addContactInformationViewModel.NewContact);
                    UpdatedEmployeeContactList.Add(addContactInformationViewModel.NewContact);
                    SelectedContactRow = addContactInformationViewModel.NewContact;
                }

                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method AddNewContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddNewContactInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit New Personal Contact Information. 
        /// </summary>
        private void EditPersonalContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditPersonalContactInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeContact empContact = (EmployeeContact)detailView.DataControl.CurrentItem;
                SelectedContactRow = empContact;
                if (empContact != null)
                {
                    AddContactInformationView addContactInformationView = new AddContactInformationView();
                    AddContactInformationViewModel addContactInformationViewModel = new AddContactInformationViewModel();
                    EventHandler handle = delegate { addContactInformationView.Close(); };
                    addContactInformationViewModel.RequestClose += handle;
                    addContactInformationView.DataContext = addContactInformationViewModel;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addContactInformationViewModel.InitReadOnly(empContact);
                    else
                        addContactInformationViewModel.EditInit(empContact);
                    addContactInformationViewModel.WindowHeader = Application.Current.FindResource("EditPersonalContactInformation").ToString();
                    addContactInformationViewModel.IsNew = false;
                    addContactInformationViewModel.IsPersonalContact = true;
                    addContactInformationViewModel.IsCompanyUse = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addContactInformationView.Owner = Window.GetWindow(ownerInfo);
                    addContactInformationView.ShowDialog();
                    if (addContactInformationViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeContactList.Count != 0)
                        {
                            var existEmpContact = UpdatedEmployeeContactList.Where(x => x.IdEmployeeContact == empContact.IdEmployeeContact).FirstOrDefault();
                            UpdatedEmployeeContactList.Remove(existEmpContact);
                        }
                        addContactInformationViewModel.EditContact.IdEmployee = EmployeeDetail.IdEmployee;
                        addContactInformationViewModel.EditContact.IdEmployeeContact = empContact.IdEmployeeContact;
                        if (empContact.IdEmployeeContact == 0)
                        {
                            addContactInformationViewModel.EditContact.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }
                        empContact.EmployeeContactIdType = addContactInformationViewModel.EditContact.EmployeeContactIdType;
                        empContact.EmployeeContactType = addContactInformationViewModel.EditContact.EmployeeContactType;
                        empContact.EmployeeContactValue = addContactInformationViewModel.EditContact.EmployeeContactValue;
                        empContact.EmployeeContactRemarks = addContactInformationViewModel.EditContact.EmployeeContactRemarks;
                        empContact.IsCompanyUse = addContactInformationViewModel.EditContact.IsCompanyUse;
                        empContact.TransactionOperation = addContactInformationViewModel.EditContact.TransactionOperation;
                        UpdatedEmployeeContactList.Add(addContactInformationViewModel.EditContact);
                        SelectedContactRow = empContact;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditPersonalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditPersonalContactInformation() - {0}" + ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Personal Contact Information Record. 
        /// </summary>
        private void DeleteContactInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteContactInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePersonalContactMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeContact empContact = (EmployeeContact)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empContact.IdEmployeeContact != 0)
                    {
                        empContact.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeContactList.Add(empContact);
                    }
                    EmployeeContactList.Remove(empContact);
                }
                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method DeleteContactInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteContactInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        // <summary>
        /// Method for Delete Polyvalence Record. 
        /// [000][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// </summary>
        private void DeletePolyvalenceRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeletePolyvalenceRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePolyvalenceMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeePolyvalence empPolyvalence = (EmployeePolyvalence)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empPolyvalence.IdEmployeePolyvalence != 0)
                    {
                        empPolyvalence.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeePolyvalenceList.Add(empPolyvalence);
                    }
                    else
                        UpdatedEmployeePolyvalenceList.Remove(empPolyvalence);

                    EmployeePolyvalenceList.Remove(empPolyvalence);
                }
                GeosApplication.Instance.Logger.Log("Method DeletePolyvalenceRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeletePolyvalenceRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Professional Contact Information. 
        /// </summary>
        private void AddNewProfessionalContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewProfessionalContactInformation()...", category: Category.Info, priority: Priority.Low);

                AddContactInformationView addContactInformationView = new AddContactInformationView();
                AddContactInformationViewModel addContactInformationViewModel = new AddContactInformationViewModel();
                EventHandler handle = delegate { addContactInformationView.Close(); };
                addContactInformationViewModel.RequestClose += handle;
                addContactInformationView.DataContext = addContactInformationViewModel;
                addContactInformationViewModel.Init(EmployeeProfessionalContactList);
                addContactInformationViewModel.WindowHeader = Application.Current.FindResource("AddProfessionalContactInformation").ToString();
                addContactInformationViewModel.IsNew = true;
                addContactInformationViewModel.IsPersonalContact = false;
                addContactInformationViewModel.IsCompanyUse = true;
                var ownerInfo = (obj as FrameworkElement);
                addContactInformationView.Owner = Window.GetWindow(ownerInfo);
                addContactInformationView.ShowDialog();

                if (addContactInformationViewModel.IsSave == true)
                {
                    //addContactInformationViewModel.NewContact.IdEmployee = EmployeeDetail.IdEmployee;
                    //EmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                    //UpdatedEmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                    //SelectedProfessionalContactRow = addContactInformationViewModel.NewContact;
                    //DisplayEmployeeProfEmailAndPhone();

                    #region chitra.girigosavi [29/12/2023] [GEOS2-5131 Avoid duplicating a professional contact
                    bool contactExists = EmployeeProfessionalContactList.Any(ec =>
                        ec.EmployeeContactIdType == addContactInformationViewModel.NewContact.EmployeeContactIdType &&
                        ec.EmployeeContactValue == addContactInformationViewModel.NewContact.EmployeeContactValue);

                    if (contactExists)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        addContactInformationViewModel.NewContact.IdEmployee = EmployeeDetail.IdEmployee;
                        EmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                        UpdatedEmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                        SelectedProfessionalContactRow = addContactInformationViewModel.NewContact;
                        DisplayEmployeeProfEmailAndPhone();
                    }
                    #endregion
                }
                GeosApplication.Instance.Logger.Log("Method AddNewProfessionalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewProfessionalContactInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Professional Contact Information. 
        /// </summary>
        private void EditProfessionalContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProfessionalContactInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeContact empContact = (EmployeeContact)detailView.DataControl.CurrentItem;
                SelectedProfessionalContactRow = empContact;

                if (empContact != null)
                {
                    AddContactInformationView addContactInformationView = new AddContactInformationView();
                    AddContactInformationViewModel addContactInformationViewModel = new AddContactInformationViewModel();
                    EventHandler handle = delegate { addContactInformationView.Close(); };
                    addContactInformationViewModel.RequestClose += handle;
                    addContactInformationView.DataContext = addContactInformationViewModel;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addContactInformationViewModel.InitReadOnly(empContact);
                    else
                        addContactInformationViewModel.EditInit(empContact);
                    addContactInformationViewModel.WindowHeader = Application.Current.FindResource("EditProfessionalContactInformation").ToString();
                    addContactInformationViewModel.IsNew = false;
                    addContactInformationViewModel.IsPersonalContact = false;
                    addContactInformationViewModel.IsCompanyUse = true;
                    var ownerInfo = (detailView as FrameworkElement);
                    addContactInformationView.Owner = Window.GetWindow(ownerInfo);
                    addContactInformationView.ShowDialog();

                    if (addContactInformationViewModel.IsSave == true)
                    {
                        #region chitra.girigosavi [20/02/2024] [GEOS2-5131 Avoid duplicating a professional contact
                        bool contactExists = EmployeeProfessionalContactList.Any(ec =>
                            ec.EmployeeContactIdType == addContactInformationViewModel.EditContact.EmployeeContactIdType &&
                            ec.EmployeeContactValue == addContactInformationViewModel.EditContact.EmployeeContactValue &&
                            ec.IdEmployeeContact != empContact.IdEmployeeContact);

                        if (contactExists)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else
                        {
                            // Update contact information if no duplicate is found
                            if (UpdatedEmployeeProfessionalContactList.Count != 0)
                            {
                                var existEmpProfessionalContact = UpdatedEmployeeProfessionalContactList.FirstOrDefault(x => x.IdEmployeeContact == empContact.IdEmployeeContact);
                                if (existEmpProfessionalContact != null)
                                    UpdatedEmployeeProfessionalContactList.Remove(existEmpProfessionalContact);
                            }

                            addContactInformationViewModel.EditContact.IdEmployee = EmployeeDetail.IdEmployee;
                            addContactInformationViewModel.EditContact.IdEmployeeContact = empContact.IdEmployeeContact;
                            if (empContact.IdEmployeeContact == 0)
                            {
                                addContactInformationViewModel.EditContact.TransactionOperation = ModelBase.TransactionOperations.Add;
                            }
                            // Update contact information
                            empContact.EmployeeContactIdType = addContactInformationViewModel.EditContact.EmployeeContactIdType;
                            empContact.EmployeeContactType = addContactInformationViewModel.EditContact.EmployeeContactType;
                            empContact.EmployeeContactValue = addContactInformationViewModel.EditContact.EmployeeContactValue;
                            empContact.EmployeeContactRemarks = addContactInformationViewModel.EditContact.EmployeeContactRemarks;
                            empContact.IsCompanyUse = addContactInformationViewModel.EditContact.IsCompanyUse;
                            empContact.TransactionOperation = addContactInformationViewModel.EditContact.TransactionOperation;

                            UpdatedEmployeeProfessionalContactList.Add(addContactInformationViewModel.EditContact);
                            SelectedProfessionalContactRow = empContact;
                            DisplayEmployeeProfEmailAndPhone();
                        }
                        #endregion
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditProfessionalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Method EditProfessionalContactInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for Delete Professional Contact Information Record. 
        /// </summary>
        private void DeleteProfessionalContactInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalContactInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfessionalContactMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeContact empContact = (EmployeeContact)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empContact.IdEmployeeContact != 0)
                    {
                        empContact.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeProfessionalContactList.Add(empContact);
                    }
                    EmployeeProfessionalContactList.Remove(empContact);
                    DisplayEmployeeProfEmailAndPhone();
                }
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalContactInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteProfessionalContactInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Employee Contract Situation. 
        /// [001][01-10-2020][cpatil][GEOS2-2495] Validate the Start Date of the employee Job Description [#ERF66]
        ///  [001][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void AddNewEmployeeContractSituation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeContractSituation()...", category: Category.Info, priority: Priority.Low);

                AddContractSituationView addContractSituationView = new AddContractSituationView();
                AddContractSituationViewModel addContractSituationViewModel = new AddContractSituationViewModel();

                EventHandler handle = delegate { addContractSituationView.Close(); };
                addContractSituationViewModel.RequestClose += handle;
                addContractSituationView.DataContext = addContractSituationViewModel;

                addContractSituationViewModel.IsNew = true;

                EmployeeContractSituation employeeContractSituation = new EmployeeContractSituation();
                if (EmployeeContractSituationList.Count > 0)
                {
                    employeeContractSituation = EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationEndDate == null);
                    if (employeeContractSituation == null)
                    {
                        addContractSituationViewModel.Init(EmployeeContractSituationList);
                        var ownerInfo = (obj as FrameworkElement);
                        addContractSituationView.Owner = Window.GetWindow(ownerInfo);
                        addContractSituationView.ShowDialog();
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContractSituationAlreadyOpenExistingOne").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    addContractSituationViewModel.Init(EmployeeContractSituationList);
                    addContractSituationView.ShowDialog();
                }

                if (addContractSituationViewModel.IsSave == true)
                {
                    addContractSituationViewModel.NewEmployeeContractSituation.IdEmployee = EmployeeDetail.IdEmployee;
                    //[001]
                    GetAllUpdatedEmployeeContracts.Add(addContractSituationViewModel.NewEmployeeContractSituation);
                    EmployeeContractSituationList.Add(addContractSituationViewModel.NewEmployeeContractSituation);
                    UpdatedEmployeeContractSituationList.Add(addContractSituationViewModel.NewEmployeeContractSituation);
                    SelectedContractSituationRow = addContractSituationViewModel.NewEmployeeContractSituation;

                    EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList);

                    CurrentContractSituation = EmployeeContractSituationList.LastOrDefault();
                    CalculateLengthOfService();
                    //[002]
                    foreach (EmployeeContractSituation itemEmployeeContractSituation in EmployeeContractSituationList)
                    {
                        if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                            itemEmployeeContractSituation.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                        if (EmployeeContractSituationList.Any(i => i.ContractSituationEndDate == null || i.ContractSituationEndDate >= DateTime.Now))
                        {
                            itemEmployeeContractSituation.IsgreaterJobDescriptionthanToday = true;
                        }

                    }
                }
                if (EmployeeContractSituationList.Count < 1)
                {
                    ContractSituationError = "";
                }
                else if (EmployeeContractSituationList.Any(i => i.Company == null))
                {
                    ContractSituationError = "";
                }
                else
                {
                    ContractSituationError = null;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeContractSituation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeContractSituation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Employee Contract Situation. 
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [002][01-10-2020][cpatil][GEOS2-2495] Validate the Start Date of the employee Job Description [#ERF66]
        ///  [003][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void EditEmployeeContractSituation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeContractSituation()...", category: Category.Info, priority: Priority.Low);
                string ExistFileName = null;
                TableView detailView = (TableView)obj;
                EmployeeContractSituation empContractSituation = (EmployeeContractSituation)detailView.DataControl.CurrentItem;
                SelectedContractSituationRow = empContractSituation;

                //[001]added
                //[003]Changes in Condition
                if (SelectedContractSituationRow.ContractSituationEndDate != null
                       && SelectedContractSituationRow.IdEmployeeExitEvent != null
                       //&& (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 137 EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 137 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138)
                       && SelectedContractSituationRow.ContractSituationEndDate < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date)
                //&& SelectedContractSituationRow.ContractSituationEndDate < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date)
                {
                    return;
                }

                //end
                if (empContractSituation != null)
                {
                    var existEmployeeContractSituationRecord = EmployeeExistDetail.EmployeeContractSituations.Where(x => x.IdEmployeeContractSituation == empContractSituation.IdEmployeeContractSituation).FirstOrDefault();
                    if (existEmployeeContractSituationRecord != null)
                    {
                        ExistFileName = existEmployeeContractSituationRecord.ContractSituationFileName;
                    }

                    //AddContractSituationView addContractSituationView = new AddContractSituationView();
                    //AddContractSituationViewModel addContractSituationViewModel = new AddContractSituationViewModel();
                    ContractSituationView addContractSituationView = new ContractSituationView();
                    ContractSituationViewModel addContractSituationViewModel = new ContractSituationViewModel();
                    addContractSituationViewModel.EmployeeCode = EmployeeCode;
                    addContractSituationViewModel.SelectedEmpolyeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                    EventHandler handle = delegate { addContractSituationView.Close(); };
                    addContractSituationViewModel.RequestClose += handle;
                    addContractSituationView.DataContext = addContractSituationViewModel;

                    if (empContractSituation.ContractSituationFileName != null)
                        addContractSituationViewModel.IsMergeVisible = true;
                    //addContractSituationViewModel.IsMergeImageVisibility = Visibility.Visible;
                    //addContractSituationViewModel.IsAddImageVisibility = Visibility.Collapsed;
                    addContractSituationViewModel.WindowHeader = Application.Current.FindResource("EditContractSituationInformation").ToString();

                    //if (HrmCommon.Instance.IsPermissionReadOnly)
                    //    addContractSituationViewModel.InitReadOnly(EmployeeContractSituationList, empContractSituation);
                    //else
                    addContractSituationViewModel.EditInit(EmployeeContractSituationList, empContractSituation);

                    addContractSituationViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addContractSituationView.Owner = Window.GetWindow(ownerInfo);
                    addContractSituationView.ShowDialog();
                    tempMergeFile = addContractSituationViewModel.tempfilepath;
                    ExistEmployeeContarctSituationFromMergeWindow = addContractSituationViewModel.ExistEmployeeContractSituation;
                    if (addContractSituationViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeContractSituationList.Count != 0)
                        {
                            var existEmployeeContractSituation = UpdatedEmployeeContractSituationList.Where(x => x.IdEmployeeContractSituation == empContractSituation.IdEmployeeContractSituation).FirstOrDefault();
                            UpdatedEmployeeContractSituationList.Remove(existEmployeeContractSituation);
                            //[002]
                            if (GetAllUpdatedEmployeeContracts.Any(ga => ga.IdEmployeeContractSituation == existEmployeeContractSituation.IdEmployeeContractSituation))
                                GetAllUpdatedEmployeeContracts.Remove(existEmployeeContractSituation);
                        }

                        if (empContractSituation.IdEmployeeContractSituation == 0)
                        {
                            //addContractSituationViewModel.EditEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empContractSituation.ContractSituation = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituation;
                        empContractSituation.IdContractSituation = addContractSituationViewModel.EditEmployeeContractSituation.IdContractSituation;
                        empContractSituation.IdCompany = addContractSituationViewModel.EditEmployeeContractSituation.IdCompany;
                        empContractSituation.Company = addContractSituationViewModel.EditEmployeeContractSituation.Company;
                        empContractSituation.ProfessionalCategory = addContractSituationViewModel.EditEmployeeContractSituation.ProfessionalCategory;
                        empContractSituation.IdProfessionalCategory = addContractSituationViewModel.EditEmployeeContractSituation.IdProfessionalCategory;
                        empContractSituation.ContractSituationStartDate = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationStartDate;
                        empContractSituation.ContractSituationEndDate = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationEndDate;
                        empContractSituation.ContractSituationFileName = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationFileName;
                        empContractSituation.ContractSituationFileInBytes = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationFileInBytes;
                        empContractSituation.ContractSituationRemarks = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationRemarks;
                        empContractSituation.TransactionOperation = addContractSituationViewModel.EditEmployeeContractSituation.TransactionOperation;

                        if (!string.IsNullOrEmpty(ExistFileName) && ExistFileName != empContractSituation.ContractSituationFileName)
                        {
                            empContractSituation.OldFileName = ExistFileName;
                        }
                        else
                        {
                            empContractSituation.OldFileName = null;
                        }

                        UpdatedEmployeeContractSituationList.Add(empContractSituation);
                        //[002]
                        if (GetAllUpdatedEmployeeContracts.Contains(empContractSituation))
                        {
                            GetAllUpdatedEmployeeContracts.Remove(empContractSituation);
                        }
                        GetAllUpdatedEmployeeContracts.Add(empContractSituation);
                        SelectedContractSituationRow = empContractSituation;
                        CalculateLengthOfService();

                        //[002]
                        foreach (EmployeeContractSituation itemEmployeeContractSituation in EmployeeContractSituationList)
                        {
                            if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                                itemEmployeeContractSituation.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                            if (EmployeeContractSituationList.Any(i => i.ContractSituationEndDate == null || i.ContractSituationEndDate >= DateTime.Now))
                            {
                                itemEmployeeContractSituation.IsgreaterJobDescriptionthanToday = true;
                            }

                        }

                    }

                    if (EmployeeContractSituationList.Count < 1)
                    {
                        ContractSituationError = "";
                    }
                    else if (EmployeeContractSituationList.Any(i => i.Company == null))
                    {
                        ContractSituationError = "";
                    }
                    else
                    {
                        ContractSituationError = null;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeeContractSituation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeContractSituation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Polyvalence 
        /// [000][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// </summary>
        private void EditEmployeePolyvalence(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeePolyvalence()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeePolyvalence empPolyvalence = (EmployeePolyvalence)detailView.DataControl.CurrentItem;
                SelectedPolyvalenceRow = empPolyvalence;
                if (empPolyvalence != null)
                {
                    AddPolyvalenceView addPolyvalenceView = new AddPolyvalenceView();
                    AddPolyvalenceViewModel addPolyvalenceViewModel = new AddPolyvalenceViewModel();
                    EventHandler handle = delegate { addPolyvalenceView.Close(); };
                    addPolyvalenceViewModel.RequestClose += handle;
                    addPolyvalenceView.DataContext = addPolyvalenceViewModel;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addPolyvalenceViewModel.InitReadOnly(EmployeePolyvalenceList, empPolyvalence);
                    else
                        addPolyvalenceViewModel.EditInit(EmployeePolyvalenceList, empPolyvalence);

                    addPolyvalenceViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addPolyvalenceView.Owner = Window.GetWindow(ownerInfo);
                    addPolyvalenceView.ShowDialog();

                    if (addPolyvalenceViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeePolyvalenceList.Count != 0)
                        {
                            var existEmployeePolyvalence = UpdatedEmployeePolyvalenceList.Where(x => x.IdEmployeePolyvalence == empPolyvalence.IdEmployeePolyvalence).FirstOrDefault();
                            UpdatedEmployeePolyvalenceList.Remove(existEmployeePolyvalence);
                        }
                        addPolyvalenceViewModel.EditEmployeePolyvalence.IdEmployee = EmployeeDetail.IdEmployee;
                        addPolyvalenceViewModel.EditEmployeePolyvalence.IdEmployeePolyvalence = empPolyvalence.IdEmployeePolyvalence;
                        if (empPolyvalence.IdEmployeePolyvalence == 0)
                        {
                            addPolyvalenceViewModel.EditEmployeePolyvalence.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }
                        empPolyvalence.Company = addPolyvalenceViewModel.EditEmployeePolyvalence.Company;
                        empPolyvalence.IdCompany = addPolyvalenceViewModel.EditEmployeePolyvalence.IdCompany;
                        empPolyvalence.JobDescription = addPolyvalenceViewModel.EditEmployeePolyvalence.JobDescription;
                        empPolyvalence.IdJobDescription = addPolyvalenceViewModel.EditEmployeePolyvalence.IdJobDescription;
                        empPolyvalence.PolyvalenceUsage = addPolyvalenceViewModel.EditEmployeePolyvalence.PolyvalenceUsage;
                        empPolyvalence.PolyvalenceRemarks = addPolyvalenceViewModel.EditEmployeePolyvalence.PolyvalenceRemarks;
                        empPolyvalence.TransactionOperation = addPolyvalenceViewModel.EditEmployeePolyvalence.TransactionOperation;
                        UpdatedEmployeePolyvalenceList.Add(empPolyvalence);
                        SelectedPolyvalenceRow = empPolyvalence;
                    }

                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeePolyvalence()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeePolyvalence()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method for Delete Contract Situation Information Record. 
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        ///  [002][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
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
                }
                //end
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteContractSituationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empContractSituation.IdContractSituation != 0)
                    {
                        empContractSituation.IsContractSituationFileDeleted = true;
                        empContractSituation.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeContractSituationList.Add(empContractSituation);

                    }
                    EmployeeContractSituationList.Remove(empContractSituation);
                    GetAllUpdatedEmployeeContracts.Remove(empContractSituation);
                    CalculateLengthOfService();
                    EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList);
                    CurrentContractSituation = EmployeeContractSituationList.LastOrDefault();
                    if (EmployeeContractSituationList.Count < 1)
                    {
                        ContractSituationError = "";
                    }
                    else
                    {
                        ContractSituationError = null;
                    }
                    //[002]
                    foreach (EmployeeContractSituation itemEmployeeContractSituation in EmployeeContractSituationList)
                    {
                        if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                            itemEmployeeContractSituation.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                        if (EmployeeContractSituationList.Any(i => i.ContractSituationEndDate == null || i.ContractSituationEndDate >= DateTime.Now))
                        {
                            itemEmployeeContractSituation.IsgreaterJobDescriptionthanToday = true;
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteContractSituationInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Employee Family Member. 
        /// </summary>
        private void AddNewEmployeeFamilyMember(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeFamilyMember()...", category: Category.Info, priority: Priority.Low);

                AddFamilyMembersView addFamilyMembersView = new AddFamilyMembersView();
                AddFamilyMembersViewModel addFamilyMembersViewModel = new AddFamilyMembersViewModel();
                EventHandler handle = delegate { addFamilyMembersView.Close(); };
                addFamilyMembersViewModel.RequestClose += handle;
                addFamilyMembersView.DataContext = addFamilyMembersViewModel;
                addFamilyMembersViewModel.Init(EmployeeFamilyMemberList);
                addFamilyMembersViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addFamilyMembersView.Owner = Window.GetWindow(ownerInfo);
                addFamilyMembersView.ShowDialog();
                if (addFamilyMembersViewModel.IsSave == true)
                {
                    addFamilyMembersViewModel.NewFamilyMember.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeFamilyMemberList.Add(addFamilyMembersViewModel.NewFamilyMember);
                    UpdatedEmployeeFamilyMemberList.Add(addFamilyMembersViewModel.NewFamilyMember);
                    SelectedFamilyMemberRow = addFamilyMembersViewModel.NewFamilyMember;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeFamilyMember()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeFamilyMember()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Employee Family Member. 
        /// </summary>
        private void EditEmployeeFamilyMember(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeFamilyMember()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeFamilyMember empFamilyMember = (EmployeeFamilyMember)detailView.DataControl.CurrentItem;
                SelectedFamilyMemberRow = empFamilyMember;
                if (empFamilyMember != null)
                {
                    AddFamilyMembersView addFamilyMembersView = new AddFamilyMembersView();
                    AddFamilyMembersViewModel addFamilyMembersViewModel = new AddFamilyMembersViewModel();
                    EventHandler handle = delegate { addFamilyMembersView.Close(); };
                    addFamilyMembersViewModel.RequestClose += handle;
                    addFamilyMembersView.DataContext = addFamilyMembersViewModel;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addFamilyMembersViewModel.InitReadOnly(empFamilyMember);
                    else
                        addFamilyMembersViewModel.EditInit(EmployeeFamilyMemberList, empFamilyMember);

                    addFamilyMembersViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addFamilyMembersView.Owner = Window.GetWindow(ownerInfo);
                    addFamilyMembersView.ShowDialog();

                    if (addFamilyMembersViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeFamilyMemberList.Count != 0)
                        {
                            var existEmployeeFamilyMember = UpdatedEmployeeFamilyMemberList.Where(x => x.IdEmployeeFamilyMember == empFamilyMember.IdEmployeeFamilyMember).FirstOrDefault();
                            UpdatedEmployeeFamilyMemberList.Remove(existEmployeeFamilyMember);
                        }
                        addFamilyMembersViewModel.EditFamilyMember.IdEmployee = EmployeeDetail.IdEmployee;
                        addFamilyMembersViewModel.EditFamilyMember.IdEmployeeFamilyMember = empFamilyMember.IdEmployeeFamilyMember;
                        if (empFamilyMember.IdEmployeeFamilyMember == 0)
                        {
                            addFamilyMembersViewModel.EditFamilyMember.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empFamilyMember.FamilyMemberFirstName = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberFirstName;
                        empFamilyMember.FamilyMemberLastName = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberLastName;
                        empFamilyMember.FamilyMemberNativeName = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberNativeName;
                        empFamilyMember.FamilyMemberNationality = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberNationality;
                        empFamilyMember.FamilyMemberIdNationality = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberIdNationality;
                        empFamilyMember.FamilyMemberRelationshipType = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberRelationshipType;
                        empFamilyMember.FamilyMemberIdRelationshipType = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberIdRelationshipType;
                        empFamilyMember.FamilyMemberGender = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberGender;
                        empFamilyMember.FamilyMemberIdGender = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberIdGender;
                        empFamilyMember.FamilyMemberDisability = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberDisability;
                        empFamilyMember.FamilyMemberIsDependent = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberIsDependent;
                        empFamilyMember.FamilyMemberRemarks = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberRemarks;
                        empFamilyMember.FamilyMemberBirthDate = addFamilyMembersViewModel.EditFamilyMember.FamilyMemberBirthDate;
                        empFamilyMember.TransactionOperation = addFamilyMembersViewModel.EditFamilyMember.TransactionOperation;

                        UpdatedEmployeeFamilyMemberList.Add(addFamilyMembersViewModel.EditFamilyMember);
                        SelectedFamilyMemberRow = empFamilyMember;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeeFamilyMember()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeFamilyMember()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Family Member Information Record. 
        /// </summary>
        private void DeleteFamilyMemberInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFamilyMemberInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteFamilyMemberInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeFamilyMember empFamilyMember = (EmployeeFamilyMember)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empFamilyMember.IdEmployeeFamilyMember != 0)
                    {
                        empFamilyMember.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeFamilyMemberList.Add(empFamilyMember);
                    }
                    EmployeeFamilyMemberList.Remove(empFamilyMember);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteFamilyMemberInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteFamilyMemberInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Employee Education Qualification. 
        /// </summary>
        private void AddNewEmployeeEducationQualification(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewLanguage()...", category: Category.Info, priority: Priority.Low);

                AddEducationView addEducationView = new AddEducationView();
                AddEducationViewModel addEducationViewModel = new AddEducationViewModel();
                EventHandler handle = delegate { addEducationView.Close(); };
                addEducationViewModel.RequestClose += handle;
                addEducationView.DataContext = addEducationViewModel;
                addEducationViewModel.Init(EmployeeEducationQualificationList);
                addEducationViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addEducationView.Owner = Window.GetWindow(ownerInfo);
                addEducationView.ShowDialog();

                if (addEducationViewModel.IsSave == true)
                {
                    addEducationViewModel.NewEducationQualification.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeEducationQualificationList.Add(addEducationViewModel.NewEducationQualification);
                    UpdatedEmployeeEducationQualificationList.Add(addEducationViewModel.NewEducationQualification);
                    SelectedEducationQualificationRow = addEducationViewModel.NewEducationQualification;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewLanguage()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewLanguage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Employee Education Qualification. 
        /// </summary>
        private void EditEmployeeEducationQualification(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeEducationQualification()...", category: Category.Info, priority: Priority.Low);
                string ExistFileName = null;
                TableView detailView = (TableView)obj;
                EmployeeEducationQualification empEducationQualification = (EmployeeEducationQualification)detailView.DataControl.CurrentItem;
                SelectedEducationQualificationRow = empEducationQualification;

                if (empEducationQualification != null)
                {
                    var existEmployeeEducationQualificationRecord = EmployeeExistDetail.EmployeeEducationQualifications.Where(x => x.IdEmployeeEducationQualification == empEducationQualification.IdEmployeeEducationQualification).FirstOrDefault();
                    if (existEmployeeEducationQualificationRecord != null)
                    {
                        ExistFileName = existEmployeeEducationQualificationRecord.QualificationFileName;
                    }

                    AddEducationView addEducationView = new AddEducationView();
                    AddEducationViewModel addEducationViewModel = new AddEducationViewModel();
                    EventHandler handle = delegate { addEducationView.Close(); };
                    addEducationViewModel.RequestClose += handle;
                    addEducationView.DataContext = addEducationViewModel;
                    addEducationViewModel.EmployeeCode = EmployeeCode;
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addEducationViewModel.InitReadOnly(empEducationQualification);
                    else
                        addEducationViewModel.EditInit(EmployeeEducationQualificationList, empEducationQualification);

                    addEducationViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEducationView.Owner = Window.GetWindow(ownerInfo);
                    addEducationView.ShowDialog();
                    if (addEducationViewModel.IsSave == true)
                    {
                        if (UpdatedEmployeeEducationQualificationList.Count != 0)
                        {
                            var existEmployeeEducationQualification = UpdatedEmployeeEducationQualificationList.Where(x => x.IdEmployeeEducationQualification == empEducationQualification.IdEmployeeEducationQualification).FirstOrDefault();
                            UpdatedEmployeeEducationQualificationList.Remove(existEmployeeEducationQualification);
                        }

                        if (empEducationQualification.IdEmployeeEducationQualification == 0)
                        {
                            addEducationViewModel.EditEducationQualification.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empEducationQualification.EducationQualification = addEducationViewModel.EditEducationQualification.EducationQualification;
                        empEducationQualification.IdEducationQualification = addEducationViewModel.EditEducationQualification.IdEducationQualification;
                        empEducationQualification.QualificationStartDate = addEducationViewModel.EditEducationQualification.QualificationStartDate;
                        empEducationQualification.QualificationEndDate = addEducationViewModel.EditEducationQualification.QualificationEndDate;
                        empEducationQualification.QualificationEntity = addEducationViewModel.EditEducationQualification.QualificationEntity;
                        empEducationQualification.QualificationFileName = addEducationViewModel.EditEducationQualification.QualificationFileName;
                        empEducationQualification.QualificationName = addEducationViewModel.EditEducationQualification.QualificationName;
                        empEducationQualification.QualificationClassification = addEducationViewModel.EditEducationQualification.QualificationClassification;
                        empEducationQualification.QualificationFileInBytes = addEducationViewModel.EditEducationQualification.QualificationFileInBytes;
                        empEducationQualification.QualificationRemarks = addEducationViewModel.EditEducationQualification.QualificationRemarks;
                        empEducationQualification.IsQualificationFileDeleted = addEducationViewModel.EditEducationQualification.IsQualificationFileDeleted;
                        empEducationQualification.TransactionOperation = addEducationViewModel.EditEducationQualification.TransactionOperation;
                        empEducationQualification.Attachment = addEducationViewModel.EditEducationQualification.Attachment;

                        if (!string.IsNullOrEmpty(ExistFileName) && ExistFileName != empEducationQualification.QualificationFileName)
                        {
                            empEducationQualification.OldFileName = ExistFileName;
                        }
                        else
                        {
                            empEducationQualification.OldFileName = null;
                        }

                        UpdatedEmployeeEducationQualificationList.Add(empEducationQualification);
                        SelectedEducationQualificationRow = empEducationQualification;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeeEducationQualification()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeEducationQualification()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Education Qualification Information Record. 
        /// </summary>
        private void DeleteEducationQualificationInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteEducationQualificationInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteEducationQualificationInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeEducationQualification empEducationQualification = (EmployeeEducationQualification)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empEducationQualification.IdEducationQualification != 0)
                    {
                        empEducationQualification.IsQualificationFileDeleted = true;
                        empEducationQualification.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeEducationQualificationList.Add(empEducationQualification);
                    }
                    EmployeeEducationQualificationList.Remove(empEducationQualification);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteEducationQualificationInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteEducationQualificationInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add New Employee JobDescription. 
        /// [001][21-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        ///  [002][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void AddNewEmployeeJobDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeJobDescription()...", category: Category.Info, priority: Priority.Low);

                AddJobDescriptionView addJobDescriptionView = new AddJobDescriptionView();
                AddJobDescriptionViewModel addJobDescriptionViewModel = new AddJobDescriptionViewModel();

                EventHandler handle = delegate { addJobDescriptionView.Close(); };
                addJobDescriptionViewModel.RequestClose += handle;
                addJobDescriptionView.DataContext = addJobDescriptionViewModel;
                addJobDescriptionViewModel.SelectedEmpolyeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                addJobDescriptionViewModel.Init(EmployeeJobDescriptionList);
                addJobDescriptionViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addJobDescriptionView.Owner = Window.GetWindow(ownerInfo);
                addJobDescriptionView.ShowDialog();
                if (addJobDescriptionViewModel.IsSave == true)
                {

                    if (addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription == 1 && EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                    {
                        //EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                    }
                    else
                    {
                        if (addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription != 1)
                        {
                            //[001]
                            if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date))
                            {
                                List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date).Select(a => a.JobDescriptionUsage).ToList();
                                if (Percentages.Count > 0)
                                {
                                    List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date)).ToList();
                                    if (JDList_Top.Count == 1 && addJobDescriptionViewModel.NewEmployeeJobDescription.JobDescriptionUsage < JDList_Top.FirstOrDefault().JobDescriptionUsage)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                    }
                                    else
                                    {
                                        if (JDList_Top.Count == 1 && Percentages.Max() != 100)
                                        {
                                            EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Hidden && (a.JobDescriptionStartDate.Value.Date > DateTime.Now.Date)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();
                                if (Percentages.Count > 0)
                                {
                                    List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList();
                                    if (JDList_Top.Count == 1 && addJobDescriptionViewModel.NewEmployeeJobDescription.JobDescriptionUsage < JDList_Top.FirstOrDefault().JobDescriptionUsage)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                    }
                                    else
                                    {
                                        if (JDList_Top.Count == 1 && Percentages.Max() != 100)
                                        {
                                            EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Hidden && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    addJobDescriptionViewModel.NewEmployeeJobDescription.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeJobDescriptionList.Add(addJobDescriptionViewModel.NewEmployeeJobDescription);
                    SelectedJobDescriptionRow = addJobDescriptionViewModel.NewEmployeeJobDescription;
                    //[001]
                    if (addJobDescriptionViewModel.StartDate > DateTime.Now)
                    {
                        EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now));
                        if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                        {
                            TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                        }
                        else
                            TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                    }
                    else
                    {
                        EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));
                        if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                        {
                            TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                        }
                        else
                            TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                    }
                    #region GreyImageVisible
                    EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                    EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                    EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                    //EmployeeJobDescriptionList.First().IsMainJobDescriptionGreyImageVisible = 1;
                    //EmployeeJobDescriptionList.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                    List<EmployeeJobDescription> finalEmployeeJobDescriptionList = new List<EmployeeJobDescription>();
                    List<EmployeeJobDescription> EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                    int JobDescriptionUsageCount = 0;
                Loop:
                    foreach (EmployeeJobDescription employeeJobDescription in EmployeeJobDescriptionList)
                    {
                        if (!finalEmployeeJobDescriptionList.Any(a => a == employeeJobDescription))
                            if (!EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a == employeeJobDescription))
                            {
                                if (employeeJobDescription.JobDescriptionEndDate == null)
                                {
                                    if (employeeJobDescription.JobDescriptionUsage == 100)
                                    {

                                    }
                                    else
                                    {
                                        EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                        JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                        if (JobDescriptionUsageCount == 100)
                                        {
                                            if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                            {
                                                if (EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).Count() == 1)
                                                {
                                                    EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                    IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                    IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }
                                                else
                                                {
                                                    EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                                    EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                                                    EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                                                    EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescription = 0);
                                                    EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IdJobDescription == addJobDescriptionViewModel.NewEmployeeJobDescription.IdJobDescription).FirstOrDefault();
                                                    if (IsMainJobDescription == null)
                                                    {
                                                        IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                    }
                                                    IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                    IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    IsMainJobDescription.IsMainJobDescription = addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription;
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }

                                            }
                                            else
                                            {
                                                //if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                {
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }
                                                else
                                                {
                                                    //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                                    //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                    EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                    MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                    MaxEmployeeJobDescription.IsMainJobDescription = 1;
                                                    MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    JobDescriptionUsageCount = 0;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            goto Loop;
                                        }
                                    }

                                }
                                else
                                {

                                    if (employeeJobDescription.JobDescriptionEndDate != null && employeeJobDescription.JobDescriptionStartDate != null)
                                    {

                                        if (employeeJobDescription.JobDescriptionUsage == 100)
                                        {

                                        }
                                        else
                                        {
                                            EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                            JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                            if (JobDescriptionUsageCount == 100)
                                            {
                                                if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                {
                                                    //if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                    //{
                                                    //    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    //    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    //    JobDescriptionUsageCount = 0;
                                                    //}
                                                    //else
                                                    //{
                                                    //    var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                    //    EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                    //    MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                    //    MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                    //    finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                    //    EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                    //    JobDescriptionUsageCount = 0;
                                                    //}
                                                    if (EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).Count() == 1)
                                                    {
                                                        EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                        IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                    else
                                                    {
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescription = 0);
                                                        EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IdEmployeeJobDescription == addJobDescriptionViewModel.NewEmployeeJobDescription.IdEmployeeJobDescription).FirstOrDefault();
                                                        if (IsMainJobDescription == null)
                                                        {
                                                            IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                        }
                                                        IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        IsMainJobDescription.IsMainJobDescription = addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    //if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                    if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                    {
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                    else
                                                    {
                                                        var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                        EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                        MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        MaxEmployeeJobDescription.IsMainJobDescription = 1;
                                                        MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                goto Loop;
                                            }
                                        }

                                    }
                                }

                            }

                    }
                    #region yellowstar
                    if (EmployeeJobDescriptionListforIsMainJobDescription.Count > 0)
                    {
                        foreach (EmployeeJobDescription EmployeeJobDescription in EmployeeJobDescriptionListforIsMainJobDescription)
                        {
                            if (EmployeeJobDescription.IsMainJobDescription == 1)
                            {
                                EmployeeJobDescription.IsMainJobDescription = 0;
                                var maxEmployeeJobDescriptionValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxEmployeeJobDescriptionValue);
                                MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                            }
                        }
                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                        if (EmployeeJobDescriptionListforIsMainJobDescription.Count == 1)
                        {
                            //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                            //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                        }
                        EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                        DateTime currentdatetime = DateTime.Now;
                        List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                        if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                        {
                            IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                            //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                            //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                            //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                            //var temp = EmployeeJobDescriptionList;
                            if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                        }
                        else
                        {
                            IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                            if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            //if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                            //{
                            //    IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                            //    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            //    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                            //    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                            //    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                            //    var temp = EmployeeJobDescriptionList;
                            //}
                        }
                    }
                    else
                    {
                        DateTime currentdatetime = DateTime.Now;
                        List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                        if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                        {
                            IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                            if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                if (EmployeeJobDescription != null)
                                {
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }

                            }
                            //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                            //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                            //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                            //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                            //var temp = EmployeeJobDescriptionList;
                        }
                        else
                        {
                            IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                            if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                            {
                                IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }

                            }
                        }


                    }
                    #endregion
                    if (finalEmployeeJobDescriptionList.Count() == EmployeeJobDescriptionList.Count())
                    {
                        EmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                        EmployeeJobDescriptionList.AddRange(finalEmployeeJobDescriptionList);
                    }
                    #endregion
                    EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date));
                    if (EmployeeJobDescriptionList != null)
                    {
                        //  EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList().Take(4).ToList());
                        var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date).ToList();
                        if (tempEmployeeTopFourJobDescriptionList.Count > 0)
                        {
                            int TotalPercentage = tempEmployeeTopFourJobDescriptionList.Sum(a => a.JobDescriptionUsage);
                            if (TotalPercentage == 100)
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(tempEmployeeTopFourJobDescriptionList);
                            }
                        }
                        else
                        {
                            var tempEmployeeTopFourJobDescription = EmployeeJobDescriptionList.OrderByDescending(a => a.JobDescriptionEndDate).ToList();
                            if (tempEmployeeTopFourJobDescription[0].JobDescriptionUsage == 100)
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                            }
                            else
                            {

                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                                int TotalPercentage = tempEmployeeTopFourJobDescription[0].JobDescriptionUsage;

                                for (int i = 1; i < tempEmployeeTopFourJobDescription.Count; i++)
                                {
                                    TotalPercentage = TotalPercentage + tempEmployeeTopFourJobDescription[i].JobDescriptionUsage;
                                    if (TotalPercentage < 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);

                                    }
                                    else if (TotalPercentage == 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }
                                    else if (TotalPercentage > 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    if (EmployeeTopFourJobDescriptionList == null)
                        EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();

                    int CheckAliasCount = EmployeeTopFourJobDescriptionList.Select(a => a.Company.Alias).Distinct().Count();
                    if (EmployeeTopFourJobDescriptionList.Count == 1)
                    {
                        EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                    }
                    else
                    {
                        if (CheckAliasCount > 1)
                        {
                            //set visible column
                            EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = true; });
                        }
                        else
                        {
                            EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                        }
                    }
                    if (EmployeeJobDescriptionList.Count < 1)
                    {
                        JobDescriptionError = "";
                    }
                    else
                    {
                        JobDescriptionError = null;
                    }

                    //[002]
                    foreach (EmployeeJobDescription itemEmployeeJobDescription in EmployeeJobDescriptionList)
                    {
                        if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                            itemEmployeeJobDescription.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                        if (EmployeeJobDescriptionList.Any(i => (i.JobDescriptionEndDate == null && i.JobDescriptionStartDate <= DateTime.Now) || i.JobDescriptionEndDate >= DateTime.Now))
                        {
                            itemEmployeeJobDescription.IsgreaterJobDescriptionthanToday = true;
                        }

                    }

                    FillJobDescriptionList(EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null) || a.JobDescriptionEndDate >= DateTime.Now.Date).ToList());

                }

                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// summary
        /// [000][avpawar][02/05/2019][GEOS2-1468][Add polyvalence section in employee profile]
        /// [Method for adding Add New Employee Polyvalence]
        private void AddNewEmployeePolyvalence(object obj)
        {
            try
            {
                if (EmployeePolyvalenceList.Count < 4)
                {
                    GeosApplication.Instance.Logger.Log("Method AddNewEmployeePolyvalence()...", category: Category.Info, priority: Priority.Low);
                    AddPolyvalenceView addPolyvalenceView = new AddPolyvalenceView();
                    AddPolyvalenceViewModel addPolyvalenceViewModel = new AddPolyvalenceViewModel();
                    EventHandler handle = delegate { addPolyvalenceView.Close(); };
                    addPolyvalenceViewModel.RequestClose += handle;
                    addPolyvalenceView.DataContext = addPolyvalenceViewModel;
                    addPolyvalenceViewModel.Init(EmployeePolyvalenceList);
                    addPolyvalenceViewModel.IsNew = true;
                    var ownerInfo = (obj as FrameworkElement);
                    addPolyvalenceView.Owner = Window.GetWindow(ownerInfo);
                    addPolyvalenceView.ShowDialog();
                    if (addPolyvalenceViewModel.IsSave == true)
                    {
                        addPolyvalenceViewModel.NewEmployeePolyvalence.IdEmployee = EmployeeDetail.IdEmployee;
                        EmployeePolyvalenceList.Add(addPolyvalenceViewModel.NewEmployeePolyvalence);
                        UpdatedEmployeePolyvalenceList.Add(addPolyvalenceViewModel.NewEmployeePolyvalence);
                    }
                    SelectedPolyvalenceRow = addPolyvalenceViewModel.NewEmployeePolyvalence;

                    GeosApplication.Instance.Logger.Log("Method AddNewEmployeePolyvalence()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeePolyvalenceLimit").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeePolyvalence()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit New Employee JobDescription. 
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        ///  [002][28-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        ///  [003][30-03-2022][cpatil][GEOS2-3496] HRM - Add new data after exit event [#ERF94]
        /// </summary>
        private void EditEmployeeJobDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeJobDescription()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeJobDescription empJobDescription = (EmployeeJobDescription)detailView.DataControl.CurrentItem;
                SelectedJobDescriptionRow = empJobDescription;

                //[001] Added 
                //[003] changes in condition
                if (empJobDescription != null)
                {
                    if (SelectedJobDescriptionRow.JobDescriptionEndDate != null
                        && SelectedJobDescriptionRow.IdEmployeeExitEvent != null
                        //&& (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 137 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138)
                        && SelectedJobDescriptionRow.JobDescriptionEndDate < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date)
                    {
                        return;
                    }

                    AddJobDescriptionView addJobDescriptionView = new AddJobDescriptionView();
                    AddJobDescriptionViewModel addJobDescriptionViewModel = new AddJobDescriptionViewModel();
                    EventHandler handle = delegate { addJobDescriptionView.Close(); };
                    addJobDescriptionViewModel.RequestClose += handle;
                    addJobDescriptionView.DataContext = addJobDescriptionViewModel;
                    addJobDescriptionViewModel.SelectedEmpolyeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addJobDescriptionViewModel.InitReadOnly(EmployeeJobDescriptionList, empJobDescription);
                    else
                        addJobDescriptionViewModel.EditInit(EmployeeJobDescriptionList, empJobDescription);

                    addJobDescriptionViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addJobDescriptionView.Owner = Window.GetWindow(ownerInfo);
                    addJobDescriptionView.ShowDialog();

                    if (addJobDescriptionViewModel.IsSave == true)
                    {
                        if (addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription == 1 && EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                        {
                            // EmployeeJobDescriptionList.Where(w=>w.IdJobDescription == addJobDescriptionViewModel.EditEmployeeJobDescription.IdJobDescription).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Hidden; });
                        }

                        addJobDescriptionViewModel.EditEmployeeJobDescription.IdEmployee = EmployeeDetail.IdEmployee;
                        addJobDescriptionViewModel.EditEmployeeJobDescription.IdEmployeeJobDescription = empJobDescription.IdEmployeeJobDescription;

                        if (empJobDescription.IdEmployeeJobDescription == 0)
                        {
                            addJobDescriptionViewModel.EditEmployeeJobDescription.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empJobDescription.Company = addJobDescriptionViewModel.EditEmployeeJobDescription.Company;
                        empJobDescription.IdCompany = addJobDescriptionViewModel.EditEmployeeJobDescription.IdCompany;
                        empJobDescription.JobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescription;

                        //sjadhav
                        if (addJobDescriptionViewModel.CountryVisble == Visibility.Visible)
                        {
                            empJobDescription.IdCountry = addJobDescriptionViewModel.EditEmployeeJobDescription.IdCountry;
                            empJobDescription.Country = addJobDescriptionViewModel.EditEmployeeJobDescription.Country;
                            empJobDescription.StrJDScope = addJobDescriptionViewModel.EditEmployeeJobDescription.Country.Name;
                            empJobDescription.IdRegion = null;
                            empJobDescription.Region = null;
                        }
                        else if (addJobDescriptionViewModel.ScopeVisble == Visibility.Visible)
                        {
                            empJobDescription.IdRegion = addJobDescriptionViewModel.EditEmployeeJobDescription.IdRegion;
                            empJobDescription.Region = addJobDescriptionViewModel.EditEmployeeJobDescription.Region;
                            empJobDescription.JDScope = empJobDescription.Region;
                            empJobDescription.StrJDScope = addJobDescriptionViewModel.EditEmployeeJobDescription.Region.Value;
                            empJobDescription.IdCountry = null;
                            empJobDescription.Country = null;
                        }

                        empJobDescription.IdJobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.IdJobDescription;
                        empJobDescription.JobDescriptionUsage = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionUsage;
                        empJobDescription.JobDescriptionStartDate = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionStartDate;
                        empJobDescription.JobDescriptionEndDate = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionEndDate;
                        empJobDescription.JobDescriptionRemarks = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionRemarks;
                        empJobDescription.IsMainJobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription;
                        empJobDescription.IsMainVisible = addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainVisible;
                        empJobDescription.TransactionOperation = addJobDescriptionViewModel.EditEmployeeJobDescription.TransactionOperation;

                        SelectedJobDescriptionRow = empJobDescription;

                        if (addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription != 1)
                        {
                            List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Select(a => a.JobDescriptionUsage).ToList();
                            if (Percentages.Count > 0 && (addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionEndDate >= DateTime.Now.Date || (addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionEndDate == null && addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionStartDate <= DateTime.Now)))
                            {
                                List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList();
                                if (JDList_Top.Count == 1 && addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionUsage < JDList_Top.FirstOrDefault().JobDescriptionUsage)
                                {
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                }
                                else
                                {
                                    if (JDList_Top.Count == 1 && Percentages.Max() != 100)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Hidden && (a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now))).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                    }
                                }
                            }
                            else
                            {
                                if (Percentages.Count == 1)
                                {
                                    EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                                }
                            }
                        }

                        IsJobDescriptionOrganizationChanged = true;
                        if (empJobDescription.JobDescriptionStartDate < DateTime.Now)
                        {
                            JobDescriptionStartDate = empJobDescription.JobDescriptionStartDate;
                            PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));
                        }
                        #region GreyImageVisible
                        EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                        EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                        EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                        //EmployeeJobDescriptionList.First().IsMainJobDescriptionGreyImageVisible = 1;
                        //EmployeeJobDescriptionList.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                        List<EmployeeJobDescription> finalEmployeeJobDescriptionList = new List<EmployeeJobDescription>();
                        List<EmployeeJobDescription> EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                        int JobDescriptionUsageCount = 0;
                    Loop:
                        foreach (EmployeeJobDescription employeeJobDescription in EmployeeJobDescriptionList)
                        {
                            if (!finalEmployeeJobDescriptionList.Any(a => a == employeeJobDescription))
                                if (!EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a == employeeJobDescription))
                                {
                                    if (employeeJobDescription.JobDescriptionEndDate == null)
                                    {
                                        if (employeeJobDescription.JobDescriptionUsage == 100)
                                        {

                                        }
                                        else
                                        {
                                            EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                            JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                            if (JobDescriptionUsageCount == 100)
                                            {
                                                if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                {
                                                    if (EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).Count() == 1)
                                                    {
                                                        EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                        IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                    else
                                                    {
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                                                        EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescription = 0);
                                                        EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IdJobDescription == addJobDescriptionViewModel.EditEmployeeJobDescription.IdJobDescription).FirstOrDefault();
                                                        if (IsMainJobDescription == null)
                                                        {
                                                            IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                        }
                                                        IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        IsMainJobDescription.IsMainJobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }

                                                }
                                                else
                                                {
                                                    if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                    {
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                    else
                                                    {
                                                        //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                                        //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                        EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                        MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        JobDescriptionUsageCount = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                goto Loop;
                                            }
                                        }

                                    }
                                    else
                                    {

                                        if (employeeJobDescription.JobDescriptionEndDate != null && employeeJobDescription.JobDescriptionStartDate != null)
                                        {

                                            if (employeeJobDescription.JobDescriptionUsage == 100)
                                            {

                                            }
                                            else
                                            {
                                                EmployeeJobDescriptionListforIsMainJobDescription.Add(employeeJobDescription);
                                                JobDescriptionUsageCount = JobDescriptionUsageCount + employeeJobDescription.JobDescriptionUsage;
                                                if (JobDescriptionUsageCount == 100)
                                                {
                                                    if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                    {
                                                        //EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                        //IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                        //IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                        //finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                        //EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                        //JobDescriptionUsageCount = 0;
                                                        if (EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).Count() == 1)
                                                        {
                                                            EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                            IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                            IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                            EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                            JobDescriptionUsageCount = 0;
                                                        }
                                                        else
                                                        {
                                                            EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                                            EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescriptionGreyImageVisible = 0);
                                                            EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                                                            EmployeeJobDescriptionListforIsMainJobDescription.ToList().ForEach(f => f.IsMainJobDescription = 0);
                                                            EmployeeJobDescription IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IdJobDescription == addJobDescriptionViewModel.EditEmployeeJobDescription.IdJobDescription).FirstOrDefault();
                                                            if (IsMainJobDescription == null)
                                                            {
                                                                IsMainJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.Where(a => a.IsMainJobDescription == 1).FirstOrDefault();
                                                            }
                                                            IsMainJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                            IsMainJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                            IsMainJobDescription.IsMainJobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription;
                                                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                            EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                            JobDescriptionUsageCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainVisible == Visibility.Visible))
                                                        if (EmployeeJobDescriptionListforIsMainJobDescription.Any(a => a.IsMainJobDescription == 1))
                                                        {
                                                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                            EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                            JobDescriptionUsageCount = 0;
                                                        }
                                                        else
                                                        {
                                                            //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                                            //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                            var maxValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                                            EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxValue);
                                                            MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                                            MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                                                            EmployeeJobDescriptionListforIsMainJobDescription = new List<EmployeeJobDescription>();
                                                            JobDescriptionUsageCount = 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    goto Loop;
                                                }
                                            }

                                        }
                                    }

                                }

                        }
                        #region yellowstar
                        if (EmployeeJobDescriptionListforIsMainJobDescription.Count > 0)
                        {
                            foreach (EmployeeJobDescription EmployeeJobDescription in EmployeeJobDescriptionListforIsMainJobDescription)
                            {
                                if (EmployeeJobDescription.IsMainJobDescription == 1)
                                {
                                    EmployeeJobDescription.IsMainJobDescription = 0;
                                    var maxEmployeeJobDescriptionValue = EmployeeJobDescriptionListforIsMainJobDescription.Max(x => x.JobDescriptionUsage);
                                    EmployeeJobDescription MaxEmployeeJobDescription = EmployeeJobDescriptionListforIsMainJobDescription.First(x => x.JobDescriptionUsage == maxEmployeeJobDescriptionValue);
                                    MaxEmployeeJobDescription.IsMainJobDescriptionGreyImageVisible = 1;
                                    MaxEmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Visible;
                                }
                            }
                            finalEmployeeJobDescriptionList.AddRange(EmployeeJobDescriptionListforIsMainJobDescription);
                            if (EmployeeJobDescriptionListforIsMainJobDescription.Count == 1)
                            {
                                //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainJobDescriptionGreyImageVisible = 1;
                                //EmployeeJobDescriptionListforIsMainJobDescription.FirstOrDefault().IsMainVisibleGreyImageVisible = Visibility.Visible;
                            }
                            EmployeeJobDescriptionList.ToList().ForEach(f => f.IsMainVisible = Visibility.Hidden);
                            DateTime currentdatetime = DateTime.Now;
                            List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                            if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                            {
                                IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                //var temp = EmployeeJobDescriptionList;
                                if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();

                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }
                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                            }
                            else
                            {
                                IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                                if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                                //if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                                //{
                                //    IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                //    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                //    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                //    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                //    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                //    var temp = EmployeeJobDescriptionList;
                                //}
                            }
                        }
                        else
                        {
                            DateTime currentdatetime = DateTime.Now;
                            List<EmployeeJobDescription> IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate >= DateTime.Today).ToList();
                            if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                            {
                                IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    if (EmployeeJobDescription != null)
                                    {
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }

                                }
                                //EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                //EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                //EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                //EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                //var temp = EmployeeJobDescriptionList;
                            }
                            else
                            {
                                IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                                if (IsMainImageStyleList != null && IsMainImageStyleList.Count() != 0)
                                {
                                    IsMainImageStyleList.ToList().ForEach(f => f.IsMainVisibleGreyImageVisible = Visibility.Hidden);
                                    if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                    {
                                        EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                        if (EmployeeJobDescription != null)
                                        {
                                            EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                            EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                            EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                            var temp = EmployeeJobDescriptionList;
                                        }

                                    }
                                    else
                                    {
                                        EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                        if (EmployeeJobDescription != null)
                                        {
                                            EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                            EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                            EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                            var temp = EmployeeJobDescriptionList;
                                        }
                                    }

                                }
                            }


                        }
                        #endregion
                        if (finalEmployeeJobDescriptionList.Count() == EmployeeJobDescriptionList.Count())
                        {
                            EmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                            EmployeeJobDescriptionList.AddRange(finalEmployeeJobDescriptionList);
                        }
                        #endregion
                        if (EmployeeJobDescriptionList != null)
                        {
                            //  EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList().Take(4).ToList());
                            var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => ((a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date)).ToList();
                            if (tempEmployeeTopFourJobDescriptionList.Count > 0)
                            {
                                int TotalPercentage = tempEmployeeTopFourJobDescriptionList.Sum(a => a.JobDescriptionUsage);
                                if (TotalPercentage == 100)
                                {
                                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(tempEmployeeTopFourJobDescriptionList);
                                }
                            }

                            else
                            {
                                var tempEmployeeTopFourJobDescription = EmployeeJobDescriptionList.OrderByDescending(a => a.JobDescriptionEndDate).ToList();
                                if (tempEmployeeTopFourJobDescription[0].JobDescriptionUsage == 100)
                                {
                                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                    EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                                }
                                else
                                {
                                    EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                    EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                                    int TotalPercentage = tempEmployeeTopFourJobDescription[0].JobDescriptionUsage;
                                    for (int i = 1; i < tempEmployeeTopFourJobDescription.Count; i++)
                                    {
                                        TotalPercentage = TotalPercentage + tempEmployeeTopFourJobDescription[i].JobDescriptionUsage;
                                        if (TotalPercentage < 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);

                                        }
                                        else if (TotalPercentage == 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                            break;
                                        }
                                        else if (TotalPercentage > 100)
                                        {
                                            EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (EmployeeTopFourJobDescriptionList == null)
                            EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();

                        int CheckAliasCount = EmployeeTopFourJobDescriptionList.Select(a => a.Company.Alias).Distinct().Count();
                        if (EmployeeTopFourJobDescriptionList.Count == 1)
                        {
                            EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                        }
                        else
                        {
                            if (CheckAliasCount > 1)
                            {
                                //set visible column
                                EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = true; });
                            }
                            else
                            {
                                EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                            }
                        }

                        //EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));
                        //if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                        //{
                        //    TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                        //}
                        //else
                        //    TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                        //[002]
                        if (addJobDescriptionViewModel.StartDate > DateTime.Now)
                        {
                            EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now));
                            if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                            {
                                TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                            }
                            else
                                TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                        }
                        else
                        {
                            EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));
                            if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                            {
                                TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                            }
                            else
                                TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                        }
                        EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));
                        if (EmployeeJobDescriptionList.Count < 1)
                        {
                            JobDescriptionError = "";
                        }
                        else
                        {
                            JobDescriptionError = null;
                        }
                        PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));

                        //[003]
                        foreach (EmployeeJobDescription itemEmployeeJobDescription in EmployeeJobDescriptionList)
                        {
                            if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                                itemEmployeeJobDescription.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                            if (EmployeeJobDescriptionList.Any(i => (i.JobDescriptionEndDate == null && i.JobDescriptionStartDate <= DateTime.Now) || i.JobDescriptionEndDate >= DateTime.Now))
                            {
                                itemEmployeeJobDescription.IsgreaterJobDescriptionthanToday = true;
                            }

                        }
                    }

                }
                //UpdatedEmployeeJobDescriptionList





                GeosApplication.Instance.Logger.Log("Method EditEmployeeJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Job Description Record. 
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        ///  [002][28-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        /// </summary>
        private void DeleteJobDescriptionRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteJobDescriptionRecord()...", category: Category.Info, priority: Priority.Low);

                //[001]Added
                EmployeeJobDescription empJobDescription = (EmployeeJobDescription)obj;

                if (empJobDescription != null)
                {
                    if (empJobDescription.IdEmployeeExitEvent != null)
                    {
                        if (empJobDescription.JobDescriptionEndDate < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date)
                            return;
                    }
                }

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteJobDescriptionMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeJobDescriptionList.Remove(empJobDescription);

                    if (EmployeeJobDescriptionList.Count > 0)
                    {
                        var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date).ToList();
                        if (tempEmployeeTopFourJobDescriptionList.Count > 0)
                        {
                            int Total = tempEmployeeTopFourJobDescriptionList.Sum(a => a.JobDescriptionUsage);
                            if (Total == 100)
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(tempEmployeeTopFourJobDescriptionList);
                            }
                        }
                        else
                        {
                            var tempEmployeeTopFourJobDescription = EmployeeJobDescriptionList.OrderByDescending(a => a.JobDescriptionEndDate).ToList();
                            if (tempEmployeeTopFourJobDescription[0].JobDescriptionUsage == 100)
                            {
                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                            }
                            else
                            {

                                EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                                EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[0]);
                                int TotalPercentage = tempEmployeeTopFourJobDescription[0].JobDescriptionUsage;
                                for (int i = 1; i < tempEmployeeTopFourJobDescription.Count; i++)
                                {
                                    TotalPercentage = TotalPercentage + tempEmployeeTopFourJobDescription[i].JobDescriptionUsage;
                                    if (TotalPercentage < 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);

                                    }
                                    else if (TotalPercentage == 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }
                                    else if (TotalPercentage > 100)
                                    {
                                        EmployeeTopFourJobDescriptionList.Add(tempEmployeeTopFourJobDescription[i]);
                                        break;
                                    }
                                }
                            }
                        }
                        if (EmployeeTopFourJobDescriptionList == null)
                            EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();

                        int CheckAliasCount = EmployeeTopFourJobDescriptionList.Select(a => a.Company.Alias).Distinct().Count();
                        if (EmployeeTopFourJobDescriptionList.Count == 1)
                        {
                            EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                        }
                        else
                        {
                            if (CheckAliasCount > 1)
                            {
                                //set visible column
                                EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = true; });
                            }
                            else
                            {
                                EmployeeTopFourJobDescriptionList.ToList().ForEach(a => { a.IsAliasDiffEnable = false; });
                            }
                        }
                        if (EmployeeJobDescriptionList.Count > 0)
                        {
                            if (EmployeeJobDescriptionList.Count == 1)
                            {
                                EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                            }
                            else
                            {
                                if (empJobDescription.JobDescriptionEndDate >= DateTime.Now.Date || empJobDescription.JobDescriptionEndDate == null)
                                {
                                    List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Select(a => a.JobDescriptionUsage).ToList();
                                    if (Percentages.Count == 1)
                                    {
                                        EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                                    }
                                    else if (Percentages.Count > 1)
                                    {
                                        if (empJobDescription.IsMainJobDescription != 1)
                                        {
                                            List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList();
                                            if (JDList_Top.Count == 1)
                                            {
                                                EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //[002]
                if (empJobDescription.JobDescriptionStartDate > DateTime.Now)
                {
                    EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now));
                    if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                    {
                        TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                    }
                    else
                        TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                }
                else
                {
                    EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));
                    if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                    {
                        TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                    }
                    else
                        TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                }
                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date));



                if (EmployeeJobDescriptionList.Count < 1)
                {
                    JobDescriptionError = "";
                }
                else
                {
                    JobDescriptionError = null;
                }

                foreach (EmployeeJobDescription itemEmployeeJobDescription in EmployeeJobDescriptionList)
                {
                    if (EmployeeStatusList != null && EmployeeStatusList.Count > 0 && SelectedEmpolyeeStatusIndex >= 0)
                        itemEmployeeJobDescription.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;

                    if (EmployeeJobDescriptionList.Any(i => i.JobDescriptionEndDate == null || i.JobDescriptionEndDate >= DateTime.Now))
                    {
                        itemEmployeeJobDescription.IsgreaterJobDescriptionthanToday = true;
                    }

                }
                GeosApplication.Instance.Logger.Log("Method DeleteJobDescriptionRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteJobDescriptionRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add New Professional Education. 
        /// </summary>
        private void AddNewProfessionalEducation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewProfessionalEducation()...", category: Category.Info, priority: Priority.Low);

                AddProfessionalEducationView addProfessionalEducationView = new AddProfessionalEducationView();
                AddProfessionalEducationViewModel addProfessionalEducationViewModel = new AddProfessionalEducationViewModel();

                EventHandler handle = delegate { addProfessionalEducationView.Close(); };
                addProfessionalEducationViewModel.RequestClose += handle;
                addProfessionalEducationView.DataContext = addProfessionalEducationViewModel;
                addProfessionalEducationViewModel.Init(EmployeeProfessionalEducationList);
                addProfessionalEducationViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addProfessionalEducationView.Owner = Window.GetWindow(ownerInfo);
                addProfessionalEducationView.ShowDialog();

                if (addProfessionalEducationViewModel.IsSave == true)
                {
                    addProfessionalEducationViewModel.NewProfessionalEducation.IdEmployee = EmployeeDetail.IdEmployee;
                    EmployeeProfessionalEducationList.Add(addProfessionalEducationViewModel.NewProfessionalEducation);
                    UpdatedemployeeProfessionalEducationList.Add(addProfessionalEducationViewModel.NewProfessionalEducation);
                    SelectedProfessionalEducationRow = addProfessionalEducationViewModel.NewProfessionalEducation;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewProfessionalEducation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewProfessionalEducation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit New Professional Education. 
        /// </summary>
        private void EditEmployeeProfessionalEducation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeProfessionalEducation()...", category: Category.Info, priority: Priority.Low);
                string ExistFileName = null;
                TableView detailView = (TableView)obj;
                EmployeeProfessionalEducation empProfEducation = (EmployeeProfessionalEducation)detailView.DataControl.CurrentItem;
                SelectedProfessionalEducationRow = empProfEducation;
                //if (empProfEducation.IdType == 1490 || empProfEducation.IdType == 1489)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingUpdationWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    return;
                //}
                //else {    


                if (empProfEducation != null)
                {
                    var existEmployeeProfessionalEducationRecord = EmployeeExistDetail.EmployeeProfessionalEducations.Where(x => x.IdEmployeeProfessionalEducation == empProfEducation.IdEmployeeProfessionalEducation).FirstOrDefault();
                    if (existEmployeeProfessionalEducationRecord != null)
                    {
                        ExistFileName = existEmployeeProfessionalEducationRecord.FileName;
                    }

                    AddProfessionalEducationView addProfessionalEducationView = new AddProfessionalEducationView();
                    AddProfessionalEducationViewModel addProfessionalEducationViewModel = new AddProfessionalEducationViewModel();

                    EventHandler handle = delegate { addProfessionalEducationView.Close(); };
                    addProfessionalEducationViewModel.RequestClose += handle;
                    addProfessionalEducationView.DataContext = addProfessionalEducationViewModel;

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        addProfessionalEducationViewModel.InitReadOnly(empProfEducation);
                    else
                        addProfessionalEducationViewModel.EditInit(EmployeeProfessionalEducationList, empProfEducation);

                    addProfessionalEducationViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addProfessionalEducationView.Owner = Window.GetWindow(ownerInfo);
                    addProfessionalEducationView.ShowDialog();

                    if (addProfessionalEducationViewModel.IsSave == true)
                    {

                        if (UpdatedemployeeProfessionalEducationList.Count != 0)
                        {
                            var existProfessionalEducation = UpdatedemployeeProfessionalEducationList.Where(x => x.IdEmployeeProfessionalEducation == empProfEducation.IdEmployeeProfessionalEducation).FirstOrDefault();
                            UpdatedemployeeProfessionalEducationList.Remove(existProfessionalEducation);
                        }

                        if (empProfEducation.IdEmployeeProfessionalEducation == 0)
                        {
                            addProfessionalEducationViewModel.EditProfessionalEducation.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empProfEducation.Type = addProfessionalEducationViewModel.EditProfessionalEducation.Type;
                        empProfEducation.IdDurationUnit = addProfessionalEducationViewModel.EditProfessionalEducation.IdDurationUnit;
                        empProfEducation.IdType = addProfessionalEducationViewModel.EditProfessionalEducation.IdType;
                        empProfEducation.Name = addProfessionalEducationViewModel.EditProfessionalEducation.Name;
                        empProfEducation.Entity = addProfessionalEducationViewModel.EditProfessionalEducation.Entity;
                        empProfEducation.DurationValue = addProfessionalEducationViewModel.EditProfessionalEducation.DurationValue;
                        empProfEducation.DurationUnit = addProfessionalEducationViewModel.EditProfessionalEducation.DurationUnit;
                        empProfEducation.StartDate = addProfessionalEducationViewModel.EditProfessionalEducation.StartDate;
                        empProfEducation.EndDate = addProfessionalEducationViewModel.EditProfessionalEducation.EndDate;
                        empProfEducation.FileName = addProfessionalEducationViewModel.EditProfessionalEducation.FileName;
                        empProfEducation.ProfessionalFileInBytes = addProfessionalEducationViewModel.EditProfessionalEducation.ProfessionalFileInBytes;
                        empProfEducation.Remarks = addProfessionalEducationViewModel.EditProfessionalEducation.Remarks;
                        empProfEducation.IsProfessionalFileDeleted = addProfessionalEducationViewModel.EditProfessionalEducation.IsProfessionalFileDeleted;
                        empProfEducation.Attachment = addProfessionalEducationViewModel.EditProfessionalEducation.Attachment;
                        empProfEducation.TransactionOperation = addProfessionalEducationViewModel.EditProfessionalEducation.TransactionOperation;

                        if (!string.IsNullOrEmpty(ExistFileName) && ExistFileName != empProfEducation.FileName)
                        {
                            empProfEducation.OldFileName = ExistFileName;
                        }
                        else
                        {
                            empProfEducation.OldFileName = null;
                        }

                        UpdatedemployeeProfessionalEducationList.Add(empProfEducation);
                        SelectedProfessionalEducationRow = empProfEducation;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeeProfessionalEducation()....executed successfully", category: Category.Info, priority: Priority.Low);
                // }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeProfessionalEducation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Delete Professional Education Record. 
        /// </summary>
        private void DeleteProfessionalEducationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalEducationRecord()...", category: Category.Info, priority: Priority.Low);
                EmployeeProfessionalEducation empProfessionalEducation = (EmployeeProfessionalEducation)obj;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfessionalEducationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);


                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (empProfessionalEducation.IdEmployeeProfessionalEducation != 0)
                    {
                        empProfessionalEducation.IsProfessionalFileDeleted = true;
                        empProfessionalEducation.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedemployeeProfessionalEducationList.Add(empProfessionalEducation);
                    }
                    EmployeeProfessionalEducationList.Remove(empProfessionalEducation);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalEducationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
                // }


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteProfessionalEducationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for On Date Edit Value Changing. 
        /// </summary>
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (BirthDate >= DateTime.Now)
                {
                    birthDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeBirthDateError").ToString();
                }
                else
                {
                    birthDateErrorMessage = string.Empty;
                }
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                    error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("BirthDate"));
                CalculateAge();
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an Error in Method OnDateEditValueChanging() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for On Exit Date Edit Value Changing. 
        /// [HRM] Set status automatically when clearing exist date [25/10/2018][adadibathina]
        /// [002][skale][2019-04-02][GEOS2-195] Check exit date to change status to Inactive in the employee profile
        /// [003][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [004][cpatil][14-09-2021][GEOS2-3360] Exit event exit date should not be the same for 4 exit events
        /// [005][cpatil][20-04-2022][GEOS2-3713] HRM - Attached document to Exit Event
        /// </summary>
        public void OnExitDateEditValueChanged(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnExitDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                // [003] added
                EmployeeExitEvent employeeExitEventObj = SelectedExitEventItem;

                if (employeeExitEventObj != null)
                {

                    TempExitDate = e.NewValue;

                    if (employeeExitEventObj.ExitDate != null)
                    {
                        if (employeeExitEventObj.IsExist)
                        {
                            if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136)
                            {
                                if (employeeExitEventObj.ExitDate != null && employeeExitEventObj.ExitDate.Value.Date >= DateTime.Now.Date)//[005]
                                    employeeExitEventObj.IsReadOnly = false;
                                else
                                    employeeExitEventObj.IsReadOnly = true;

                                employeeExitEventObj.IsEnable = true;
                            }
                            else
                            {
                                employeeExitEventObj.IsReadOnly = false;
                                employeeExitEventObj.IsEnable = true;
                            }
                        }
                        else
                        {
                            employeeExitEventObj.IsReadOnly = false;
                            employeeExitEventObj.IsEnable = true;
                        }

                        if (employeeExitEventObj.SelectedExitEventReasonIndex == 0)
                            employeeExitEventObj.SelectedExitEventReasonIndex = 0;

                        if (employeeExitEventObj.IdExitEvent != 1)
                        {
                            EmployeeExitEvent employeeExitEvent = ExitEventList.Where(x => x.IdExitEvent == employeeExitEventObj.IdExitEvent - 1).FirstOrDefault();
                            employeeExitEvent.MaxDate = employeeExitEventObj.ExitDate.Value;
                            //[004]
                            employeeExitEventObj.MinExitEventDate = employeeExitEvent.ExitDate.Value.AddDays(1);
                        }

                        //[002] added
                        //if (employeeExitEventObj.ExitDate >= DateTime.Now.Date)
                        //{
                        //    if (SelectedEmpolyeeStatusIndex == 2 || SelectedEmpolyeeStatusIndex == 1)
                        //    {
                        //        SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                        //    }
                        //    else if (SelectedEmpolyeeStatusIndex == 0)
                        //    {
                        //        SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 138);
                        //    }
                        //}
                        //else
                        //{
                        //    SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 137);
                        //}
                        //end
                    }
                    else if (!employeeExitEventObj.IsExist)
                    {
                        employeeExitEventObj.SelectedExitEventReasonIndex = 0;
                        employeeExitEventObj.Remarks = null;
                        employeeExitEventObj.IsEnable = true;
                        if (employeeExitEventObj.ExitDate != null && employeeExitEventObj.ExitDate.Value.Date >= DateTime.Now.Date)//[005]
                            employeeExitEventObj.IsReadOnly = false;
                        else
                            employeeExitEventObj.IsReadOnly = true;
                        employeeExitEventObj.IsVisible = Visibility.Collapsed;
                        employeeExitEventObj.ExitEventBytes = null;
                        employeeExitEventObj.ExitEventattachmentList.Clear();
                        //SelectedEmpolyeeStatusIndex = 1;

                    }



                }

                #region old functionality
                //if (IsFromExitDate)
                //{
                //    if (ExitDate1 != null)
                //    {
                //        IsEnableReason = true;   //to enable only when date is selected----sprint-41---sdesai
                //        IsEnableRemark = true;
                //        if (SelectedReasonIndex == 0 || SelectedReasonIndex == -1)
                //        {
                //            exitReasonErrorMessage = System.Windows.Application.Current.FindResource("EmployeeExitReasonError").ToString();
                //        }
                //        else
                //        {
                //            exitReasonErrorMessage = string.Empty;
                //        }
                //        //[002] added
                //        if (ExitDate1 >= DateTime.Now.Date)
                //        {
                //            if (SelectedEmpolyeeStatusIndex == 2 || SelectedEmpolyeeStatusIndex == 1)
                //            {
                //                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                //            }
                //            else if (SelectedEmpolyeeStatusIndex == 0)
                //            {
                //                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 138);
                //            }
                //        }
                //        else
                //        {
                //            SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 137);
                //        }
                //        //end
                //    }
                //    else
                //    {
                //        SelectedReasonIndex = 0;
                //        ExitRemark = string.Empty;
                //        exitReasonErrorMessage = string.Empty;
                //        IsEnableReason = false;
                //        IsEnableRemark = false;
                //        SelectedEmpolyeeStatusIndex = 1;
                //        IsVisible = Visibility.Collapsed;
                //        FileInBytes = null;
                //        AttachmentList.Clear();
                //    }
                //    if (!HrmCommon.Instance.IsPermissionReadOnly)
                //        error = EnableValidationAndGetError();
                //    PropertyChanged(this, new PropertyChangedEventArgs("SelectedReasonIndex"));
                //}
                //IsFromExitDate = false;
                #endregion

                GeosApplication.Instance.Logger.Log("Method OnExitDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnExitDateEditValueChanging() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }


        public byte[] ImageSourceToByteArray(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        /// <summary>
        /// Method for Compress Profile Photo Size. 
        /// </summary>
        public byte[] PhotoSourceToByteArray(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(ms);
                    SaveTemporary(bmp, ms, 100);

                    while (ms.Length > 25000)
                    {
                        double scale = Math.Sqrt
                        ((double)25000 / (double)ms.Length);
                        ms.SetLength(0);
                        bmp = ScaleImage(bmp, scale);
                        SaveTemporary(bmp, ms, 100);
                    }

                    if (bmp != null)
                        bmp.Dispose();
                    bytes = ms.ToArray();
                }
            }

            return bytes;
        }

        private void SaveTemporary(Bitmap bmp, MemoryStream ms, int quality)
        {
            EncoderParameter qualityParam = new EncoderParameter
                (System.Drawing.Imaging.Encoder.Quality, quality);
            var codec = GetImageCodecInfo();
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            bmp.Save(ms, codec, encoderParams);
        }

        private ImageCodecInfo GetImageCodecInfo()
        {
            return ImageCodecInfo.GetImageEncoders()[1];
        }

        public Bitmap ScaleImage(Bitmap image, double scale)
        {
            int newWidth = (int)(image.Width * scale);
            int newHeight = (int)(image.Height * scale);

            Bitmap result = new Bitmap(newWidth, newHeight);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        /// <summary>
        /// Method for Accept Edit Employee. 
        /// <para>[001][skale][2019-04-02][GEOS2-195] Check exit date to change status to Inactive in the employee profile</para>
        /// <para>[002][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.</para>
        /// <para>[003][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features</para>
        /// <para>[004][skale][18-09-2019][GEOS2-1710] Memorize Exit Events</para>
        /// <para>[005][spawar][03-03-2020][GEOS2-1710] Mandatory exit event when the user changes the Employee Profile status to Inactive</para>
        /// <para>[006][smazhar][07-24-2020][GEOS2-2317] Exit Data (date, reason, remarks) doesn't appear in the employee list</para>
        ///[007][01-10-2020][cpatil][GEOS2-2495] Validate the Start Date of the employee Job Description [#ERF66]
        /// [008][28-07-2021][cpatil][GEOS2-3112] HRM - After exit event status active (again)
        /// [009][13-09-2021][cpatil][GEOS2-3358] Status should be draft to active if current date is equal to hire date
        /// [010][17-03-2022][cpatil][GEOS2-3565] HRM - Allow add future Job descriptions [#ERF97] - 1
        /// [011][06-04-2022][cpatil][GEOS2-3562] Allow negative value in Backlog Leave - 1
        /// [012][cpatil][18-04-2022][GEOS2-3708] If user change clock id then system show validation error
        /// [013][cpatil][20-04-2022][GEOS2-3714] HRM - Exit date not the same as end date JD
        /// </summary>

        public void EditEmployee()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method SaveEditProfile()...", category: Category.Info, priority: Priority.Low);

                if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Count < 1)
                {
                    JobDescriptionError = "";
                }
                else
                {
                    JobDescriptionError = null;
                }

                if (EmployeeContractSituationList != null && EmployeeContractSituationList.Count < 1)
                {
                    ContractSituationError = "";
                }
                else if (EmployeeContractSituationList.Any(i => i.Company == null))
                {
                    ContractSituationError = "";
                }
                else
                {
                    ContractSituationError = null;
                }
                if (!EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                {
                    List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Select(a => a.JobDescriptionUsage).ToList();
                    if (Percentages.Count > 0)
                    {
                        List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList();
                        if (JDList_Top.Count == 1)
                        {
                            if (Percentages.Max() != 100)
                            {
                                EmployeeTopFourJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                            }
                            else
                            {
                                EmployeeTopFourJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; });
                                EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; });
                            }
                        }
                    }
                }



                //[009]
                if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 137)
                {

                    if (EmployeeContractSituationList.Any((ecs => ecs.ContractSituationStartDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date && (ecs.ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : ecs.ContractSituationEndDate.Value.Date) >= GeosApplication.Instance.ServerDateTime.Date)))
                    {
                        SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                    }
                }

                //[007]
                List<EmployeeJobDescription> GetAllUpdatedJobDescription = new List<EmployeeJobDescription>();

                UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                foreach (EmployeeJobDescription item in EmployeeExistDetail.EmployeeJobDescriptions)
                {
                    //[007]
                    GetAllUpdatedJobDescription.Add(item);
                    if (!EmployeeJobDescriptionList.Any(x => x.IdEmployeeJobDescription == item.IdEmployeeJobDescription))
                    {
                        EmployeeJobDescription employeeJobDescription = (EmployeeJobDescription)item.Clone();
                        employeeJobDescription.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedEmployeeJobDescriptionList.Add(employeeJobDescription);
                        GetAllUpdatedJobDescription.Remove(item);
                    }
                }
                //Added 
                foreach (EmployeeJobDescription item in EmployeeJobDescriptionList)
                {
                    if (!EmployeeExistDetail.EmployeeJobDescriptions.Any(x => x.IdEmployeeJobDescription == item.IdEmployeeJobDescription))
                    {
                        EmployeeJobDescription employeeJobDescription = (EmployeeJobDescription)item.Clone();
                        employeeJobDescription.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdatedEmployeeJobDescriptionList.Add(employeeJobDescription);
                        GetAllUpdatedJobDescription.Add(item);
                    }
                }

                if (GetAllUpdatedJobDescription.Count() == EmployeeJobDescriptionList.Count())
                {
                    GetAllUpdatedJobDescription = new List<EmployeeJobDescription>();
                    GetAllUpdatedJobDescription.AddRange(EmployeeJobDescriptionList);
                }

                //Updated
                foreach (EmployeeJobDescription EmployeeJobDescription_original in EmployeeExistDetail.EmployeeJobDescriptions)
                {
                    if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Any(x => x.IdEmployeeJobDescription == EmployeeJobDescription_original.IdEmployeeJobDescription))
                    {
                        EmployeeJobDescription EmployeeJobDescription_updated = EmployeeJobDescriptionList.FirstOrDefault(x => x.IdEmployeeJobDescription == EmployeeJobDescription_original.IdEmployeeJobDescription);
                        if (EmployeeJobDescription_updated.IsMainVisible != EmployeeJobDescription_original.IsMainVisible ||
                            EmployeeJobDescription_updated.IsMainJobDescription != EmployeeJobDescription_original.IsMainJobDescription ||
                            EmployeeJobDescription_updated.IdCompany != EmployeeJobDescription_original.IdCompany ||
                            EmployeeJobDescription_updated.IdJobDescription != EmployeeJobDescription_original.IdJobDescription ||
                            EmployeeJobDescription_updated.JobDescriptionUsage != EmployeeJobDescription_original.JobDescriptionUsage ||
                            EmployeeJobDescription_updated.JobDescriptionStartDate != EmployeeJobDescription_original.JobDescriptionStartDate ||
                            EmployeeJobDescription_updated.JobDescriptionEndDate != EmployeeJobDescription_original.JobDescriptionEndDate ||
                            EmployeeJobDescription_updated.JobDescriptionRemarks != EmployeeJobDescription_original.JobDescriptionRemarks ||
                            EmployeeJobDescription_updated.IdCountry != EmployeeJobDescription_original.IdCountry ||
                            EmployeeJobDescription_updated.IdRegion != EmployeeJobDescription_original.IdRegion)
                        {
                            EmployeeJobDescription employeeJobDescription = (EmployeeJobDescription)EmployeeJobDescription_updated.Clone();
                            employeeJobDescription.TransactionOperation = ModelBase.TransactionOperations.Update;
                            UpdatedEmployeeJobDescriptionList.Add(employeeJobDescription);
                            if (GetAllUpdatedJobDescription.Any(i => i.IdEmployeeJobDescription == employeeJobDescription.IdEmployeeJobDescription))
                            {
                                GetAllUpdatedJobDescription.Remove(GetAllUpdatedJobDescription.Where(i => i.IdEmployeeJobDescription == employeeJobDescription.IdEmployeeJobDescription).FirstOrDefault());
                            }
                            GetAllUpdatedJobDescription.Add(employeeJobDescription);
                        }
                    }
                }
                if (UpdatedEmployeeJobDescriptionList.Count > 0 && IsJobDescriptionOrganizationChanged)
                {
                    List<int> UpdatedJobDescriptionCompanyIdList = EmployeeJobDescriptionList.Select(x => x.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();
                    if (!ExistJobDescriptionCompanyIdList.SequenceEqual(UpdatedJobDescriptionCompanyIdList))
                    {
                        string _idCompany = null;
                        if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Count > 0)
                        {
                            _idCompany = string.Join(",", EmployeeJobDescriptionList.Select(i => i.IdCompany));
                        }
                        // FillCompanySchedule(_idCompany);  
                        IsJobDescriptionOrganizationChanged = false;
                        ExistJobDescriptionCompanyIdList = EmployeeJobDescriptionList.Select(x => x.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();
                    }
                }

                //[007]
                // List<EmployeeJobDescription> currentJobDescription = GetAllUpdatedJobDescription.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).ToList();
                // List<EmployeeContractSituation> itemECSS = GetAllUpdatedEmployeeContracts.Where(a => a.ContractSituationEndDate >= DateTime.Now.Date || a.ContractSituationEndDate == null).ToList();
                //[012]
                EmployeeContractSituation EmpFirstContract = GetAllUpdatedEmployeeContracts.FirstOrDefault();
                if (EmpFirstContract != null && GetAllUpdatedJobDescription != null && GetAllUpdatedJobDescription.Count > 0)
                {
                    if (GetAllUpdatedJobDescription.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Any(gaujd => gaujd.JobDescriptionStartDate.Value.Date < EmpFirstContract.ContractSituationStartDate.Value.Date))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        if (GetAllUpdatedJobDescription.Any(gaujd => gaujd.JobDescriptionStartDate.Value.Date == EmpFirstContract.ContractSituationStartDate.Value.Date))
                        {
                            if (ExitEventList != null && ExitEventList.Where(eel => eel.ExitDate != null).ToList().Count() > 0)
                                foreach (EmployeeExitEvent itemExit in ExitEventList.Where(eel => eel.ExitDate != null).OrderBy(eel => eel.IdExitEvent))
                                {
                                    List<EmployeeJobDescription> empJobDescr = GetAllUpdatedJobDescription.Where(ejd => ejd.JobDescriptionStartDate.Value.Date > itemExit.ExitDate.Value.Date).ToList();
                                    EmployeeContractSituation EmpFirstContractAfterExit = GetAllUpdatedEmployeeContracts.Where(eec => eec.ContractSituationStartDate.Value.Date > itemExit.ExitDate.Value.Date).OrderBy(ec => ec.ContractSituationStartDate).FirstOrDefault();
                                    if (EmpFirstContractAfterExit != null && empJobDescr != null && empJobDescr.Count > 0)
                                    {
                                        if (empJobDescr.Any(gaujd => gaujd.JobDescriptionStartDate.Value.Date == EmpFirstContractAfterExit.ContractSituationStartDate.Value.Date))
                                        {
                                            if (empJobDescr.Any(gaujd => gaujd.JobDescriptionStartDate.Value.Date < EmpFirstContractAfterExit.ContractSituationStartDate.Value.Date))
                                            {
                                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                            return;
                                        }
                                    }
                                }
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                }


                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now.Date));

                if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                {
                    TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                }
                else
                    TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

                if (EmployeeShiftList != null && EmployeeShiftList.Count < 1)
                    EmployeeShiftError = "";
                else
                    EmployeeShiftError = null;


                //[004] added

                var CurrentItemList = ExitEventList.Where(item1 =>
               !employeeDetail.EmployeeExitEvents.Any(item2 => item1.IdExitEvent == item2.IdExitEvent)).ToList();

                // Exit event validation
                EmployeeExitEvent employeeExitEventObj = CurrentItemList.Where(x => x.IsExist != true).FirstOrDefault();

                if (employeeExitEventObj != null)
                {

                    if (employeeExitEventObj.ExitDate != null)
                    {

                        if (employeeExitEventObj.SelectedExitEventReasonIndex == 0)
                        {

                            SelectedExitEventItem = employeeExitEventObj;
                            return;
                        }
                    }
                }

                //005
                if (SelectedEmpolyeeStatusIndex == 1)
                {
                    if (EmployeeDetail.EmployeeExitEvents.Count == 0)
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 1).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                    else if (EmployeeDetail.EmployeeExitEvents.Count == 4)
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 4).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                    else
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IsExist != true).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                }

                if (EmployeeDetail.EmployeeStatus.IdLookupValue != 137 && SelectedEmpolyeeStatusIndex == 2)
                {
                    if (employeeExitEventObj.SelectedExitEventReasonIndex == 0)
                    {
                        employeeExitEventObj.SelectedEmpolyeeStatusIndex = SelectedEmpolyeeStatusIndex;
                        employeeExitEventObj.SelectedExitEventReasonIndex = 0;

                    }
                    if (employeeExitEventObj.ExitDate == null)
                    {
                        employeeExitEventObj.ExitDate = null;
                    }
                }


                if (SelectedEmpolyeeStatusIndex != 2)
                {
                    employeeExitEventObj.EmployeeProfileDetailExitEventValidationMessage = "";
                }

                if (employeeExitEventObj.ExitDate != null && employeeExitEventObj.SelectedExitEventReasonIndex != 0)
                {
                    employeeExitEventObj.EmployeeProfileDetailExitEventValidationMessage = "";
                }


                if (!string.IsNullOrEmpty(employeeExitEventObj.EmployeeProfileDetailExitEventValidationMessage))
                {
                    return;
                }



                if (SelectedEmpolyeeStatusIndex != -1)
                {

                    if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136)
                    {

                        if (EmployeeContractSituationList.Any(x => x.ContractSituationStartDate <= Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date && (x.ContractSituationEndDate >= Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date || x.ContractSituationEndDate == null)))
                        {
                            ContractSituationError = null;
                        }
                        else
                            ContractSituationError = "";

                        ObservableCollection<EmployeeJobDescription> CurrentJD = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate <= Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date && (a.JobDescriptionEndDate >= Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date || a.JobDescriptionEndDate == null)));

                        if (CurrentJD.Count > 0)
                        {
                            JobDescriptionError = null;
                        }
                        else
                        {
                            JobDescriptionError = "";
                            TotalJobDescriptionUsage = CurrentJD.Sum(x => x.JobDescriptionUsage);
                        }
                    }
                    else
                    {
                        EmployeeExitEvent employeeExitEvent = ExitEventList.Where(x => x.IsExist == true && x.ExitDate == null && x.TransactionOperation != ModelBase.TransactionOperations.Delete).FirstOrDefault();
                        if (employeeExitEvent != null)
                        {
                            SelectedExitEventItem = employeeExitEvent;
                            return;
                        }

                    }
                }

                if (!HrmCommon.Instance.IsPermissionReadOnly)
                    error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
                PropertyChanged(this, new PropertyChangedEventArgs("NativeName"));
                PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedMaritalStatusIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedNationalityIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCountryIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLocationIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGender"));
                PropertyChanged(this, new PropertyChangedEventArgs("BirthDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));
                PropertyChanged(this, new PropertyChangedEventArgs("ContractSituationError"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedCompanyScheduleIndex")); 
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedCompanyShiftIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCountryRegion"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCity"));
                PropertyChanged(this, new PropertyChangedEventArgs("EmployeeShiftError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));

                if (error != null)
                {
                    SelectedBoxIndex = 0;
                    return;
                }

                if (JobDescriptionError != null || ContractSituationError != null)
                {
                    SelectedBoxIndex = 7;
                    return;
                }
                //[012]
                int PendingCount = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now)).Count();
                if (PendingCount > 1)
                {
                    if (!EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Visible))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionMainvalidationError").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }


                try
                {
                    //[Sudhir.Jangra][GEOS2-3418]
                    //[GEOS2-5545][27.03.2024][rdixit]
                    if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 137)
                    {
                        if (EmployeeProfessionalContactList != null && ExistProfessionalEmailEmployeeList != null)
                        {
                            var ExceptCurrentEmployeeEmails = ExistProfessionalEmailEmployeeList.Where(i => i.IdEmployee != EmployeeDetail.IdEmployee).ToList();
                            if (ExceptCurrentEmployeeEmails != null)
                            {
                                foreach (var item in EmployeeProfessionalContactList.Where(c => c.EmployeeContactType?.IdLookupValue == 88).ToList())
                                {
                                    if (item != null)
                                    {
                                        if (ExceptCurrentEmployeeEmails.Any(i => i.EmployeeContactProfessionalEmail?.ToLower() == item.EmployeeContactValue?.ToLower()))
                                        {
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeDuplicateProfessionalEmailWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                            return;
                                        }
                                    }
                                }
                            }
                            //var temp = ExistProfessionalEmailEmployeeList.Select(x => x.EmployeeContactProfessionalEmail).Intersect(EmployeeProfessionalContactList.Where(x => x.EmployeeContactType.IdLookupValue == 88).Select(x => x.EmployeeContactValue));
                            //if (temp.Any())
                            //{
                            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeDuplicateProfessionalEmailWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            //    return;
                            //}
                        }
                    }
                }
                catch (Exception ex)
                { }


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

                EmployeeUpdatedDetail = new Employee();
                EmployeeUpdatedDetail.IdEmployee = EmployeeDetail.IdEmployee;
                EmployeeUpdatedDetail.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                if (!string.IsNullOrEmpty(FirstName))
                    EmployeeUpdatedDetail.FirstName = FirstName.Trim();
                if (!string.IsNullOrEmpty(LastName))
                    EmployeeUpdatedDetail.LastName = LastName.Trim();
                if (!string.IsNullOrEmpty(DisplayName))
                    EmployeeUpdatedDetail.DisplayName = DisplayName.Trim();
                if (!string.IsNullOrEmpty(NativeName))
                    EmployeeUpdatedDetail.NativeName = NativeName.Trim();
                if (!string.IsNullOrEmpty(Address))
                    EmployeeUpdatedDetail.AddressStreet = Address.Trim();
                if (!string.IsNullOrEmpty(SelectedCity))
                    EmployeeUpdatedDetail.AddressCity = SelectedCity.Trim();
                if (!string.IsNullOrEmpty(ZipCode))
                    EmployeeUpdatedDetail.AddressZipCode = ZipCode.Trim();
                if (!string.IsNullOrEmpty(Remark))
                    EmployeeUpdatedDetail.Remarks = Remark.Trim();
                if (!string.IsNullOrEmpty(SelectedCountryRegion))
                    EmployeeUpdatedDetail.AddressRegion = SelectedCountryRegion.Trim();

                SelectedGender = GenderList.FirstOrDefault(x => x.IdLookupValue == GenderList[SelectedIndexGender].IdLookupValue);
                EmployeeUpdatedDetail.IdGender = (uint)SelectedGender.IdLookupValue;
                EmployeeUpdatedDetail.Gender = SelectedGender;
                EmployeeUpdatedDetail.DateOfBirth = BirthDate;
                EmployeeUpdatedDetail.Latitude = Latitude;
                EmployeeUpdatedDetail.Longitude = Longitude;
                EmployeeUpdatedDetail.Disability = (ushort)SelectedDisability;
                EmployeeUpdatedDetail.IdMaritalStatus = (uint)MaritalStatusList[SelectedMaritalStatusIndex].IdLookupValue;
                EmployeeUpdatedDetail.IdNationality = NationalityList[SelectedNationalityIndex].IdCountry;
                EmployeeUpdatedDetail.AddressIdCountry = CountryList[SelectedCountryIndex].IdCountry;
                EmployeeUpdatedDetail.AddressCountry = CountryList[SelectedCountryIndex];
                EmployeeUpdatedDetail.IdCompanyLocation = LocationList[SelectedLocationIndex].IdCompany;

                if (SelectedMaritalStatusIndex > 0)
                {
                    EmployeeUpdatedDetail.MaritalStatus = MaritalStatusList[SelectedMaritalStatusIndex];
                }
                else
                {
                    EmployeeUpdatedDetail.MaritalStatus = null;
                }
                EmployeeUpdatedDetail.Nationality = NationalityList[SelectedNationalityIndex];
                EmployeeUpdatedDetail.IsEnabled = 1;
                EmployeeUpdatedDetail.EmployeeCode = EmployeeDetail.EmployeeCode;
                if (IsRemote == true)
                {
                    Remote = 1;
                }
                else
                {
                    Remote = 0;
                }
                EmployeeUpdatedDetail.IsTrainer = Remote;
                if (SelectedEmpolyeeStatusIndex != -1)
                {
                    EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                    EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                }


                EmployeeUpdatedDetail.IsLongTermAbsent = (IsLongTermAbsent == true) ? (byte)1 : (byte)0;

                bool isProfileImageChanged = false;

                if (IsPhoto)
                {
                    //New image added
                    if (Uri.Compare(((System.Windows.Media.Imaging.BitmapImage)defaultPhotoSource).UriSource,
                                        ((System.Windows.Media.Imaging.BitmapImage)OldprofilePhotoSource).UriSource,
                                        UriComponents.AbsoluteUri,
                                        UriFormat.SafeUnescaped,
                                        StringComparison.InvariantCulture) == 0)
                    {
                        byte[] CheckImageBytes = ImageSourceToByteArray(ProfilePhotoSource);

                        if (CheckImageBytes.Length <= 25000)
                            EmployeeUpdatedDetail.ProfileImageInBytes = CheckImageBytes;
                        else
                            EmployeeUpdatedDetail.ProfileImageInBytes = PhotoSourceToByteArray(ProfilePhotoSource);

                        isProfileImageChanged = true;
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogAddImage").ToString()) });
                    }
                    else
                    {
                        //Image updated.
                        byte[] profileImageByte = ImageSourceToByteArray(ProfilePhotoSource);
                        byte[] oldProfileImageByte = ImageSourceToByteArray(OldprofilePhotoSource);

                        bool imageResult = oldProfileImageByte.SequenceEqual(profileImageByte);

                        if (!imageResult)
                        {
                            byte[] CheckImageBytes = ImageSourceToByteArray(ProfilePhotoSource);

                            if (CheckImageBytes.Length <= 25000)
                                EmployeeUpdatedDetail.ProfileImageInBytes = CheckImageBytes;
                            else
                                EmployeeUpdatedDetail.ProfileImageInBytes = PhotoSourceToByteArray(ProfilePhotoSource);

                            isProfileImageChanged = true;
                            EmployeeChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = EmployeeDetail.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogImage").ToString())
                            });
                        }
                    }
                }
                else
                {
                    if (((System.Windows.Media.Imaging.BitmapImage)OldprofilePhotoSource).UriSource == null)
                    {
                        //If profile is image deleted
                        if (((System.Windows.Media.Imaging.BitmapImage)OldprofilePhotoSource).UriSource == null)
                        {
                            if (Uri.Compare(((System.Windows.Media.Imaging.BitmapImage)defaultPhotoSource).UriSource,
                                        ((System.Windows.Media.Imaging.BitmapImage)ProfilePhotoSource).UriSource,
                                        UriComponents.AbsoluteUri,
                                        UriFormat.SafeUnescaped,
                                        StringComparison.InvariantCulture) == 0)
                            {
                                EmployeeUpdatedDetail.ProfileImageInBytes = null;
                                EmployeeUpdatedDetail.IsProfileImageDeleted = true;
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogImageRemove").ToString()) });
                            }
                        }
                    }
                }
                //[004] added
                foreach (EmployeeExitEvent NewExitEventRecord in ExitEventList)
                {
                    if (NewExitEventRecord.IsExist)
                    {

                        List<EmployeeExitEvent> ExistingInfo = employeeDetail.EmployeeExitEvents.Where(x => x.IdExitEvent == NewExitEventRecord.IdExitEvent).ToList();

                        if (ExistingInfo.Any(oldRecord => NewExitEventRecord.ExitDate != oldRecord.ExitDate || NewExitEventRecord.Remarks != oldRecord.Remarks || NewExitEventRecord.FileName != oldRecord.FileName || NewExitEventRecord.ExitEventReasonList[NewExitEventRecord.SelectedExitEventReasonIndex].IdLookupValue != oldRecord.IdReason))
                        {
                            if (NewExitEventRecord.ExitDate == null && NewExitEventRecord.IsDeleteExitEventEnabled == true)
                                NewExitEventRecord.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            else
                                NewExitEventRecord.TransactionOperation = ModelBase.TransactionOperations.Update;

                            NewExitEventRecord.IdReason = NewExitEventRecord.ExitEventReasonList[NewExitEventRecord.SelectedExitEventReasonIndex].IdLookupValue;

                            if (ExistingInfo.Any(oldRecord => NewExitEventRecord.FileName != oldRecord.FileName))
                            {
                                // EmployeeExitEvent oldExitEvent = employeeDetail.EmployeeExitEvents.Where(x => x.IdExitEvent == NewExitEventRecord.IdExitEvent).FirstOrDefault();

                                EmployeeExitEvent oldExitEvent = ExistingInfo.FirstOrDefault();

                                if (oldExitEvent != null)
                                {
                                    NewExitEventRecord.OldFileName = oldExitEvent.FileName;

                                    if (!string.IsNullOrEmpty(oldExitEvent.FileName) && string.IsNullOrEmpty(NewExitEventRecord.FileName))
                                        NewExitEventRecord.IsExitEventFileDeleted = true;

                                }
                            }

                            UpdateEmployeeExitEventsList.Add(NewExitEventRecord);
                        }
                    }
                    else
                    {
                        if (NewExitEventRecord.ExitDate != null)
                        {
                            NewExitEventRecord.TransactionOperation = ModelBase.TransactionOperations.Add;
                            NewExitEventRecord.IdReason = NewExitEventRecord.ExitEventReasonList[NewExitEventRecord.SelectedExitEventReasonIndex].IdLookupValue;
                            UpdateEmployeeExitEventsList.Add(NewExitEventRecord);
                        }

                    }
                }


                // [004] added
                if (employeeExitEventObj != null)
                {
                    if (employeeExitEventObj.ExitDate != null)
                    {
                        if (employeeExitEventObj.SelectedExitEventReasonIndex > 0)
                        {
                            if (employeeExitEventObj.ExitDate >= GeosApplication.Instance.ServerDateTime.Date)
                            {
                                if (SelectedEmpolyeeStatusIndex == 2 || SelectedEmpolyeeStatusIndex == 1)
                                {
                                    SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                                    EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                                    EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                                }
                                else if (SelectedEmpolyeeStatusIndex == 0)
                                {
                                    SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 138);
                                    EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                                    EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                                }
                            }
                            else
                            {
                                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 137);
                                EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                                EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                            }
                            //End
                            //[013]
                            EmployeeContractSituation tempEmployeeContractSituation = EmployeeContractSituationList.LastOrDefault(x => x.ContractSituationEndDate == null || x.ContractSituationEndDate <= employeeExitEventObj.ExitDate || x.ContractSituationEndDate >= employeeExitEventObj.ExitDate);
                            if (tempEmployeeContractSituation != null)
                            {
                                if (tempEmployeeContractSituation.ContractSituationStartDate > employeeExitEventObj.ExitDate)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeContractExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    IsBusy = false;
                                    return;
                                }
                                tempEmployeeContractSituation.ContractSituationEndDate = employeeExitEventObj.ExitDate;
                                if (tempEmployeeContractSituation.IdEmployeeContractSituation > 0)
                                {
                                    tempEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedEmployeeContractSituationList.Add(tempEmployeeContractSituation);

                                }
                            }
                            //[012] [013]
                            ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeTopFourJobDescriptionList.Where(x => (x.JobDescriptionEndDate == null && x.JobDescriptionStartDate <= DateTime.Now) || x.JobDescriptionEndDate >= employeeExitEventObj.ExitDate).ToList());
                            foreach (EmployeeJobDescription item in TempEmployeeJobDescriptionsList)
                            {
                                if (UpdatedEmployeeJobDescriptionList.Contains(item))
                                {
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {

                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        IsBusy = false;
                                        return;
                                    }
                                    item.JobDescriptionEndDate = employeeExitEventObj.ExitDate;
                                    UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>((UpdatedEmployeeJobDescriptionList.Where(a => a.JobDescription.IdJobDescription != item.JobDescription.IdJobDescription)).ToList());
                                    UpdatedEmployeeJobDescriptionList.Add(item);
                                }
                                else
                                {
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        IsBusy = false;
                                        return;
                                    }
                                    item.JobDescriptionEndDate = employeeExitEventObj.ExitDate;
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedEmployeeJobDescriptionList.Add(item);
                                }
                            }
                            //[010]
                            //[012] [013]
                            ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionsListFuture = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(x => x.JobDescriptionStartDate >= employeeExitEventObj.ExitDate || ((x.JobDescriptionEndDate == null && x.JobDescriptionStartDate <= DateTime.Now) || x.JobDescriptionEndDate >= employeeExitEventObj.ExitDate)).ToList());
                            foreach (EmployeeJobDescription item in TempEmployeeJobDescriptionsListFuture)
                            {
                                if (UpdatedEmployeeJobDescriptionList.Contains(item))
                                {
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        IsBusy = false;
                                        return;
                                    }
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {
                                        item.JobDescriptionStartDate = employeeExitEventObj.ExitDate;
                                    }
                                    if (item.JobDescriptionEndDate == null || item.JobDescriptionEndDate > employeeExitEventObj.ExitDate)
                                    {
                                        item.JobDescriptionEndDate = employeeExitEventObj.ExitDate;
                                    }
                                    UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>((UpdatedEmployeeJobDescriptionList.Where(a => a.JobDescription.IdJobDescription != item.JobDescription.IdJobDescription)).ToList());
                                    UpdatedEmployeeJobDescriptionList.Add(item);
                                }
                                else
                                {
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        IsBusy = false;
                                        return;
                                    }
                                    if (item.JobDescriptionStartDate > employeeExitEventObj.ExitDate)
                                    {
                                        item.JobDescriptionStartDate = employeeExitEventObj.ExitDate;
                                    }
                                    if (item.JobDescriptionEndDate == null || item.JobDescriptionEndDate > employeeExitEventObj.ExitDate)
                                    {
                                        item.JobDescriptionEndDate = employeeExitEventObj.ExitDate;
                                    }
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdatedEmployeeJobDescriptionList.Add(item);
                                }
                            }

                            if (tempEmployeeContractSituation != null && TempEmployeeJobDescriptionsList.Count > 0)
                            {
                                MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationAndJobDescriptionClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                            }
                            else if (TempEmployeeJobDescriptionsList.Count > 0)
                            {
                                MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeJobDescriptionClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                            }
                            else if (tempEmployeeContractSituation != null)
                            {
                                MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                            }
                        }

                        EmployeeJobDescription empJD = null;

                        if (EmployeeJobDescriptionsList != null && EmployeeJobDescriptionsList.Count > 0)
                            empJD = EmployeeJobDescriptionsList.OrderByDescending(x => x.JobDescriptionUsage).First();

                        //code change for GEOS2-37[adhatkar]
                        //List<JobDescription> jobDescription = HrmService.GetJobDescriptionById_V2180(HrmCommon.Instance.EmailExitEventJDList);
                        //Get IT employee emails
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                        List<Employee> ITEmployees = new List<Employee>(HrmService.GetITEmployeeDetails_V2180(plantOwnersIds, GeosApplication.Instance.ServerDateTime.Date.Year, HrmCommon.Instance.EmailExitEventJDList));

                        if (ITEmployees != null && ITEmployees.Count > 0)
                        {
                            string JDWithComma = string.Join(", ", ITEmployees.Select(a => a.EmployeeJobDescription.JobDescription.JobDescriptionTitle).Distinct().ToList());
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyITEmployeeMessage"].ToString(), JDWithComma), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                string emailTemplate = HrmService.GetEmployeeExitEmailTemplate();
                                DateTime lastWorkingDay = (DateTime)employeeExitEventObj.ExitDate;
                                emailTemplate = emailTemplate.Replace("[EMPLOYEE_NAME]", EmployeeUpdatedDetail.FullName);

                                if (empJD != null && empJD.JobDescription != null)
                                    emailTemplate = emailTemplate.Replace("[JOB_TITLE]", empJD.JobDescription.JobDescriptionTitle);
                                else
                                    emailTemplate = emailTemplate.Replace("- [JOB_TITLE]", "");

                                emailTemplate = emailTemplate.Replace("[LAST_WORKING_DATE]", lastWorkingDay.ToShortDateString());
                                emailTemplate = emailTemplate.Replace("\r\n", "%0D%0A");
                                if (ITEmployees != null)
                                {
                                    SendMailtoITPersons(ITEmployees, emailTemplate);
                                }
                            }
                        }

                    }

                    //[004] Added - when change in exit event date thne change the jd and contract as per conditions.
                    if (UpdateEmployeeExitEventsList.Count > 0)
                    {
                        foreach (EmployeeExitEvent item in UpdateEmployeeExitEventsList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                EmployeeExitEvent OldExitEvent = employeeDetail.EmployeeExitEvents.Where(x => x.IdExitEvent == item.IdExitEvent).FirstOrDefault();

                                if (OldExitEvent != null)
                                {
                                    bool csUpdated = false, jdUpdated = false;
                                    EmployeeContractSituation tempEmployeeContractSituation = EmployeeContractSituationList.LastOrDefault(x => x.IdEmployeeExitEvent == item.IdEmployeeExitEvent);
                                    //[rdixit][02.02.2023][GEOS2-2968]
                                    // && (x.ContractSituationEndDate == null || x.ContractSituationEndDate == OldExitEvent.ExitDate)

                                    if (tempEmployeeContractSituation != null)
                                    {
                                        if (tempEmployeeContractSituation.ContractSituationEndDate != item.ExitDate)
                                        {
                                            csUpdated = true;
                                        }
                                        tempEmployeeContractSituation.ContractSituationEndDate = item.ExitDate;
                                        tempEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        UpdatedEmployeeContractSituationList.Add(tempEmployeeContractSituation);
                                    }

                                    ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(
                                        EmployeeJobDescriptionList.Where(x => x.IdEmployeeExitEvent == item.IdEmployeeExitEvent).ToList());
                                    //&& (x.JobDescriptionEndDate == null || x.JobDescriptionEndDate == OldExitEvent.ExitDate)

                                    if (TempEmployeeJobDescriptionsList != null && TempEmployeeJobDescriptionsList.Count > 0)
                                    {
                                        foreach (EmployeeJobDescription JDItem in TempEmployeeJobDescriptionsList)
                                        {
                                            if (JDItem.JobDescriptionEndDate != item.ExitDate)
                                            {
                                                jdUpdated = true;
                                            }
                                            JDItem.JobDescriptionEndDate = item.ExitDate;
                                            JDItem.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            UpdatedEmployeeJobDescriptionList.Add(JDItem);
                                        }
                                    }
                                    if (jdUpdated == true && csUpdated == true)
                                    {
                                        MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationAndJobDescriptionDateChangedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                                    }
                                    else if (jdUpdated == true && csUpdated == false)
                                    {
                                        MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeJobDescriptionDateChangedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                                    }
                                    else if (jdUpdated == false && csUpdated == true)
                                    {
                                        MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationDateChangedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                                    }
                                    if (item.ExitDate > Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)
                                    {
                                        SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                                        EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                                        EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                                    }
                                }
                            }
                        }
                    }
                }

                //[004] added
                //[006] 
                if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 136)
                {
                    EmployeeExitEvent employeeLastExitEvent = ExitEventList.Where(x => x.ExitDate != null).OrderByDescending(y => y.IdExitEvent).FirstOrDefault();
                    if (employeeLastExitEvent != null)
                    {
                        EmployeeUpdatedDetail.ExitDate = employeeLastExitEvent.ExitDate;
                        EmployeeUpdatedDetail.ExitIdReason = employeeLastExitEvent.IdReason;
                        EmployeeUpdatedDetail.ExitReason = ExitReasonList[employeeLastExitEvent.SelectedExitEventReasonIndex];
                        if (!string.IsNullOrEmpty(employeeLastExitEvent.Remarks))
                            EmployeeUpdatedDetail.ExitRemarks = employeeLastExitEvent.Remarks.Trim();

                        if (!string.IsNullOrEmpty(employeeLastExitEvent.FileName))
                            EmployeeUpdatedDetail.ExitFileName = employeeLastExitEvent.FileName.Trim();
                    }
                }

                #region Old functionality

                //old Code
                //if (ExitDate1 != null)
                //{
                //    EmployeeUpdatedDetail.ExitDate = ExitDate1;

                //    if (SelectedReasonIndex > 0)
                //    {
                //        EmployeeUpdatedDetail.ExitIdReason = ExitReasonList[SelectedReasonIndex].IdLookupValue;
                //        EmployeeUpdatedDetail.ExitReason = ExitReasonList[selectedReasonIndex];

                //        //[01] added
                //        if (ExitDate1 >= GeosApplication.Instance.ServerDateTime.Date)
                //        {
                //            if (SelectedEmpolyeeStatusIndex == 2 || SelectedEmpolyeeStatusIndex == 1)
                //            {
                //                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);
                //                EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                //                EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                //            }
                //            else if (SelectedEmpolyeeStatusIndex == 0)
                //            {
                //                SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 138);
                //                EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                //                EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                //            }
                //        }
                //        else
                //        {
                //            SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 137);
                //            EmployeeUpdatedDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                //            EmployeeUpdatedDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                //        }
                //        //End

                //        if (FileInBytes != null)
                //        {
                //            EmployeeUpdatedDetail.EmployeeExitDocInBytes = FileInBytes;
                //            EmployeeUpdatedDetail.IsemployeeExitDocDeleted = false;
                //            EmployeeUpdatedDetail.ExitFileName = FileName;
                //        }
                //        else
                //        {
                //            EmployeeUpdatedDetail.EmployeeExitDocInBytes = null;
                //            EmployeeUpdatedDetail.IsemployeeExitDocDeleted = true;
                //            EmployeeUpdatedDetail.ExitFileName = null;
                //        }

                //        // EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>(EmployeeContractSituationList.Where(w => w.ContractSituationEndDate == null).Select(s => { s.ContractSituationEndDate = ExitDate; return s;}).ToList());

                //        EmployeeContractSituation tempEmployeeContractSituation = EmployeeContractSituationList.LastOrDefault(x => x.ContractSituationEndDate == null || x.ContractSituationEndDate == EmployeeExistDetail.ExitDate);
                //        if (tempEmployeeContractSituation != null && EmployeeExistDetail.ExitDate != ExitDate1)
                //        {
                //            if (tempEmployeeContractSituation.ContractSituationStartDate > ExitDate1)
                //            {
                //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeContractExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //                IsBusy = false;
                //                return;
                //            }
                //            tempEmployeeContractSituation.ContractSituationEndDate = ExitDate1;
                //            tempEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Update;
                //            UpdatedEmployeeContractSituationList.Add(tempEmployeeContractSituation);
                //        }


                //        ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeTopFourJobDescriptionList.Where(x => x.JobDescriptionEndDate == null || x.JobDescriptionEndDate == EmployeeExistDetail.ExitDate).ToList());
                //        foreach (EmployeeJobDescription item in TempEmployeeJobDescriptionsList)
                //        {
                //            if (UpdatedEmployeeJobDescriptionList.Contains(item))
                //            {
                //                if (item.JobDescriptionStartDate > ExitDate1)
                //                {
                //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //                    IsBusy = false;
                //                    return;
                //                }
                //                item.JobDescriptionEndDate = ExitDate1;
                //                UpdatedEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>((UpdatedEmployeeJobDescriptionList.Where(a => a.JobDescription.IdJobDescription != item.JobDescription.IdJobDescription)).ToList());
                //                UpdatedEmployeeJobDescriptionList.Add(item);
                //            }
                //            else
                //            {
                //                if (item.JobDescriptionStartDate > ExitDate1)
                //                {
                //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeExitDateMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //                    IsBusy = false;
                //                    return;
                //                }
                //                item.JobDescriptionEndDate = ExitDate1;
                //                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                //                UpdatedEmployeeJobDescriptionList.Add(item);
                //            }
                //        }

                //        if (tempEmployeeContractSituation != null && TempEmployeeJobDescriptionsList.Count > 0 && EmployeeExistDetail.ExitDate != ExitDate1)
                //        {
                //            MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationAndJobDescriptionClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                //        }
                //        else if (TempEmployeeJobDescriptionsList.Count > 0 && EmployeeExistDetail.ExitDate != ExitDate1)
                //        {
                //            MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeJobDescriptionClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                //        }
                //        else if (tempEmployeeContractSituation != null && EmployeeExistDetail.ExitDate != ExitDate1)
                //        {
                //            MessageBoxResult MessageBoxResultForJobDescription = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyEmployeeContractSituationClosedMessage"].ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                //        }
                //    }

                //    if (!string.IsNullOrEmpty(ExitRemark))
                //        EmployeeUpdatedDetail.ExitRemarks = ExitRemark.Trim();

                //    EmployeeJobDescription empJD = null;

                //    if (EmployeeJobDescriptionsList != null && EmployeeJobDescriptionsList.Count > 0)
                //        empJD = EmployeeJobDescriptionsList.OrderByDescending(x => x.JobDescriptionUsage).First();

                //    if (EmployeeDetail.ExitDate != ExitDate1)
                //    {
                //        JobDescription jobDescription = HrmService.GetJobDescriptionById(26);
                //        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["NotifyITEmployeeMessage"].ToString(), jobDescription.JobDescriptionTitle), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                //        if (MessageBoxResult == MessageBoxResult.Yes)
                //        {
                //            string emailTemplate = HrmService.GetEmployeeExitEmailTemplate();
                //            DateTime lastWorkingDay = (DateTime)EmployeeUpdatedDetail.ExitDate;
                //            emailTemplate = emailTemplate.Replace("[EMPLOYEE_NAME]", EmployeeUpdatedDetail.FullName);

                //            if (empJD != null && empJD.JobDescription != null)
                //                emailTemplate = emailTemplate.Replace("[JOB_TITLE]", empJD.JobDescription.JobDescriptionTitle);
                //            else
                //                emailTemplate = emailTemplate.Replace("- [JOB_TITLE]", "");

                //            emailTemplate = emailTemplate.Replace("[LAST_WORKING_DATE]", lastWorkingDay.ToShortDateString());
                //            emailTemplate = emailTemplate.Replace("\r\n", "%0D%0A");

                //            //Get IT employee emails
                //            List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                //            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                //            List<Employee> ITEmployees = new List<Employee>(HrmService.GetITEmployeeDetails(plantOwnersIds, GeosApplication.Instance.ServerDateTime.Date.Year));
                //            if (ITEmployees != null)
                //            {
                //                SendMailtoITPersons(ITEmployees, emailTemplate);
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    if (ExitDate1 != EmployeeExistDetail.ExitDate)
                //    {
                //        ObservableCollection<EmployeeJobDescription> TempEmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeTopFourJobDescriptionList.Where(x => x.JobDescriptionEndDate == EmployeeExistDetail.ExitDate).ToList());
                //        foreach (EmployeeJobDescription JobDescription in TempEmployeeJobDescriptionsList)
                //        {
                //            JobDescription.JobDescriptionEndDate = null;
                //            JobDescription.TransactionOperation = ModelBase.TransactionOperations.Update;
                //            UpdatedEmployeeJobDescriptionList.Add(JobDescription);
                //        }

                //        EmployeeContractSituation tempEmployeeContractSituation = EmployeeContractSituationList.LastOrDefault(x => x.ContractSituationEndDate == EmployeeExistDetail.ExitDate);
                //        if (tempEmployeeContractSituation != null)
                //        {
                //            tempEmployeeContractSituation.ContractSituationEndDate = null;
                //            tempEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Update;
                //            UpdatedEmployeeContractSituationList.Add(tempEmployeeContractSituation);
                //        }
                //    }

                //}
                #endregion

                //[003] added
                List<EmployeeShift> UpdatedEmployeeShiftlist = new List<EmployeeShift>();

                List<Int64> ExistEmployeeShiftIds = employeeDetail.EmployeeShifts.Select(es => es.IdEmployeeShift).ToList();
                List<Int64> ExistEmployeeShiftPlusNewAddedIds = EmployeeShiftList.Select(es => es.IdEmployeeShift).ToList();
                employeeDetail.EmployeeShifts.Where(esl => !ExistEmployeeShiftPlusNewAddedIds.Contains(esl.IdEmployeeShift)).ToList().ForEach(x => x.TransactionOperation = ModelBase.TransactionOperations.Delete);
                UpdatedEmployeeShiftlist.AddRange(employeeDetail.EmployeeShifts.Where(esl => !ExistEmployeeShiftPlusNewAddedIds.Contains(esl.IdEmployeeShift)).ToList());
                UpdatedEmployeeShiftlist.AddRange(EmployeeShiftList.Where(esl => esl.IdEmployeeShift == 0).ToList());

                EmployeeUpdatedDetail.EmployeeShifts = UpdatedEmployeeShiftlist;

                //Update Tab Information
                EmployeeUpdatedDetail.EmployeePersonalContacts = new List<EmployeeContact>(UpdatedEmployeeContactList);
                EmployeeUpdatedDetail.EmployeeProfessionalContacts = new List<EmployeeContact>(UpdatedEmployeeProfessionalContactList);
                EmployeeUpdatedDetail.EmployeeDocuments = new List<EmployeeDocument>(UpdatedEmployeeDocumentList);
                EmployeeUpdatedDetail.EmployeeLanguages = new List<EmployeeLanguage>(UpdatedEmployeeLanguageList);
                EmployeeUpdatedDetail.EmployeeEducationQualifications = new List<EmployeeEducationQualification>(UpdatedEmployeeEducationQualificationList);
                EmployeeUpdatedDetail.EmployeeContractSituations = new List<EmployeeContractSituation>(UpdatedEmployeeContractSituationList);

                // Update flag when employee has new contract added.
                if (EmployeeUpdatedDetail.EmployeeContractSituations != null && EmployeeUpdatedDetail.EmployeeContractSituations.Count > 0)
                {
                    EmployeeContractSituation ecs = EmployeeUpdatedDetail.EmployeeContractSituations.FirstOrDefault(x => x.IdEmployeeContractSituation == 0 && x.TransactionOperation == ModelBase.TransactionOperations.Add);

                    if (ecs != null)
                    {
                        //DateTime? empexitdate = ExitEventList.Max(x => x.ExitDate);
                        List<EmployeeContractSituation> list = EmployeeContractSituationList.Where(x => (x.IdEmployeeExitEvent == null || x.IdEmployeeExitEvent == 0)).ToList();

                        if (list.Count == 1)
                        {
                            EmployeeUpdatedDetail.HasWelcomeMessageBeenReceived = 0;
                        }
                    }
                }

                //[002] added
                EmployeeUpdatedDetail.EmployeePolyvalences = new List<EmployeePolyvalence>(UpdatedEmployeePolyvalenceList);
                EmployeeUpdatedDetail.EmployeeFamilyMembers = new List<EmployeeFamilyMember>(UpdatedEmployeeFamilyMemberList);
                EmployeeUpdatedDetail.EmployeeJobDescriptions = new List<EmployeeJobDescription>(UpdatedEmployeeJobDescriptionList);

                EmployeeUpdatedDetail.EmployeeProfessionalEducations = new List<EmployeeProfessionalEducation>(UpdatedemployeeProfessionalEducationList);
                EmployeeUpdatedDetail.EmployeeAnnualLeaves = new List<EmployeeAnnualLeave>(UpdatedEmployeeAnnualLeaveList);
                EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional = new List<EmployeeAnnualAdditionalLeave>();


                for (int i = 0; i < EmployeeUpdatedDetail.EmployeeAnnualLeaves.Count; i++)
                {
                    EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional.AddRange(EmployeeUpdatedDetail.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                    //EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional[i].TransactionOperation = ModelBase.TransactionOperations.Add; //EmployeeUpdatedDetail.EmployeeAnnualLeaves[i].TransactionOperation; // ModelBase.TransactionOperations.Update;
                    //EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional[i].IdEmployeeAnnualLeave = EmployeeUpdatedDetail.EmployeeAnnualLeaves[i].IdEmployeeAnnualLeave;
                    //EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional[i].IdLeave = EmployeeUpdatedDetail.EmployeeAnnualLeaves[i].IdLeave;
                }

                //for (int i = 0; i < EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional.Count; i++)
                //{
                //    //EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional.AddRange(EmployeeUpdatedDetail.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves);
                //    EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional[i].TransactionOperation = ModelBase.TransactionOperations.Update;
                //}
                // EmployeeUpdatedDetail.EmployeeAnnualLeavesAdditional = new List<EmployeeAnnualLeave>(UpdatedEmployeeAnnualLeaveList);

                EmployeeUpdatedDetail.EmployeeExitEvents = new List<EmployeeExitEvent>(UpdateEmployeeExitEventsList);

                if (SelectedIndexBackupEmployee == 0)
                    EmployeeUpdatedDetail.IdEmployeeBackup = null;
                else
                    EmployeeUpdatedDetail.IdEmployeeBackup = LstBackupEmployee[SelectedIndexBackupEmployee].IdEmployee;
                EmployeeUpdatedDetail.ModifiedIn = GeosApplication.Instance.ServerDateTime;

                UpdateDisplayName();    //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
                if (!string.IsNullOrEmpty(DisplayName))     //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
                    EmployeeUpdatedDetail.DisplayName = DisplayName.Trim();

                AddChangedEmployeeLogDetails();

                #region GEOS2-4846
                if (SelectedJobDescription < 0)//[GEOS2-5987][rdixit][25.07.2024]
                    SelectedJobDescription = 0;
                if (EmployeeExistDetail.IdApprovalResponsible != JobDescriptionList[SelectedJobDescription].IdJobDescription)
                {
                    EmployeeUpdatedDetail.IdApprovalResponsible = JobDescriptionList[SelectedJobDescription].IdJobDescription;
                    if (EmployeeExistDetail.IdApprovalResponsible == 0)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog()
                        {
                            IdEmployee = EmployeeDetail.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeApprovalResponsibleChangeLog").ToString(), "None", JobDescriptionList[SelectedJobDescription].JobDescriptionTitleAndCode)
                        });
                    }
                    else
                    {
                        JobDescription OldJD = JobDescriptionList.FirstOrDefault(x => x.IdJobDescription == EmployeeExistDetail.IdApprovalResponsible);
                        if (OldJD != null)
                        {
                            if (JobDescriptionList[SelectedJobDescription].IdJobDescription == 0)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeApprovalResponsibleChangeLog").ToString(), OldJD.JobDescriptionTitleAndCode, "None")
                                });
                            }
                            else
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeApprovalResponsibleChangeLog").ToString(), OldJD.JobDescriptionTitleAndCode, JobDescriptionList[SelectedJobDescription].JobDescriptionTitleAndCode)
                                });
                            }
                        }
                    }

                }
                else
                {
                    EmployeeUpdatedDetail.IdApprovalResponsible = JobDescriptionList[SelectedJobDescription].IdJobDescription;
                }
                #endregion

                #region GEOS2-5273
                if (SelectedExtraHoursTime != null)
                {
                    if (EmployeeExistDetail.IdExtraHoursTime != SelectedExtraHoursTime.IdLookupValue)
                    {
                        EmployeeUpdatedDetail.IdExtraHoursTime = (UInt32)SelectedExtraHoursTime.IdLookupValue;
                        if (EmployeeExistDetail.IdExtraHoursTime == 0)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = EmployeeDetail.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,

                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExtraHourTimeChangeLog").ToString(), "None", SelectedExtraHoursTime.Value)
                            });
                        }
                        else
                        {
                            if (SelectedExtraHoursTime.IdLookupValue == 0)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,

                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExtraHourTimeChangeLog").ToString(), ExtraHoursTimeList.FirstOrDefault(x => x.IdLookupValue == EmployeeExistDetail.IdExtraHoursTime).Value, "None")
                                });
                            }
                            else
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,

                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExtraHourTimeChangeLog").ToString(), ExtraHoursTimeList.FirstOrDefault(x => x.IdLookupValue == EmployeeExistDetail.IdExtraHoursTime).Value, SelectedExtraHoursTime.Value)
                                });
                            }

                        }

                    }
                    else
                    {
                        EmployeeUpdatedDetail.IdExtraHoursTime = (UInt32)SelectedExtraHoursTime.IdLookupValue;
                    }
                }

                #endregion

                EmployeeUpdatedDetail.EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeChangeLogList);
                EmployeeUpdatedDetail.IdCompanyShift = EmployeeDetail.IdCompanyShift;
                //EmployeeUpdatedDetail.IdCompanyShift = CompanyShifts[SelectedCompanyShiftIndex].IdCompanyShift;
                //EmployeeUpdatedDetail.CompanyShift = CompanyShifts[SelectedCompanyShiftIndex];
                //EmployeeUpdatedDetail.CompanyShift.CompanySchedule = CompanySchedules[SelectedCompanyScheduleIndex];
                //(cpatil)Sprint52 - HRM-052-02	Avoid duplicated employees
                //Show warning if employee fullname exist
                if (EmployeeDetail.FirstName != EmployeeUpdatedDetail.FirstName || EmployeeDetail.LastName != EmployeeUpdatedDetail.LastName)
                {
                    List<Employee> employees = new List<Employee>();
                    employees.AddRange(HrmService.IsEmployeeExists(EmployeeUpdatedDetail.FirstName, EmployeeUpdatedDetail.LastName, EmployeeUpdatedDetail.IdEmployee));
                    if (employees.Count > 0)
                    {
                        //[rdixit][GEOS2-4387][29.08.2023]
                        MessageBoxResult result = CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeExistMsg").ToString(), string.Join(",", employees.Select(em => em.FullName).ToList())), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.Cancel)
                        {
                            IsBusy = false;
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            return;
                        }
                    }
                }

                //[Sudhir.jangra][GEOS2-5579]
                List<EmployeeEquipmentAndTools> employeeEquipment = new List<EmployeeEquipmentAndTools>();

                #region GEOS2-5579
                if (ClonedEmployeeEquipmentList == null)
                {
                    ClonedEmployeeEquipmentList = new List<EmployeeEquipmentAndTools>();
                }
                if (EmployeeUpdatedDetail.EmployeeChangelogs == null)
                    EmployeeUpdatedDetail.EmployeeChangelogs = new List<EmployeeChangelog>();
                //Delete
                foreach (EmployeeEquipmentAndTools item in ClonedEmployeeEquipmentList)
                {
                    var equip = item.Category + "," + item.EquipmentType;
                    if (!EmployeeEquipmentList.Any(x => x.IdEmployeeEquipment == item.IdEmployeeEquipment))
                    {
                        EmployeeEquipmentAndTools equipment = (EmployeeEquipmentAndTools)item.Clone();
                        equipment.IdEmployee = IdEmployee;
                        equipment.OldFileName = item.SavedFileName;
                        equipment.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        employeeEquipment.Add(equipment);

                        EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                        {
                            IdEmployee = EmployeeDetail.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("DeleteEquipmentAndToolsChangeLog").ToString(), equip)
                        });
                    }
                }
                //Added
                foreach (EmployeeEquipmentAndTools item in EmployeeEquipmentList)
                {
                    var equip = item.Category + "," + item.EquipmentType;
                    if (!ClonedEmployeeEquipmentList.Any(x => x.IdEmployeeEquipment == item.IdEmployeeEquipment))
                    {
                        EmployeeEquipmentAndTools equipment = (EmployeeEquipmentAndTools)item.Clone();
                        equipment.IdEmployee = IdEmployee;
                        equipment.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        equipment.TransactionOperation = ModelBase.TransactionOperations.Add;
                        employeeEquipment.Add(equipment);

                        EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                        {
                            IdEmployee = EmployeeDetail.IdEmployee,
                            ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                            ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("AddEquipmentAndToolsChangeLog").ToString(), equip)
                        });
                    }
                }
                //Updated
                foreach (EmployeeEquipmentAndTools original in ClonedEmployeeEquipmentList)
                {
                    if (EmployeeEquipmentList.Any(x => x.IdEmployeeEquipment == original.IdEmployeeEquipment))
                    {
                        EmployeeEquipmentAndTools docUpdated = EmployeeEquipmentList.FirstOrDefault(x => x.IdEmployeeEquipment == original.IdEmployeeEquipment);
                        if (original.SavedFileName != docUpdated.SavedFileName
                            || original.EquipmentAndToolsAttachedDocInBytes != docUpdated.EquipmentAndToolsAttachedDocInBytes
                            || original.OriginalFileName != docUpdated.OriginalFileName || original.IdEquipment != docUpdated.IdEquipment
                            || original.ExpectedDuration != docUpdated.ExpectedDuration || original.Remarks != docUpdated.Remarks
                            || original.IsMandatory != docUpdated.IsMandatory || original.EndDate != docUpdated.EndDate)
                        {
                            var equip = docUpdated.Category + "," + docUpdated.EquipmentType;
                            EmployeeEquipmentAndTools attachDoc = (EmployeeEquipmentAndTools)docUpdated.Clone();
                            attachDoc.IdEmployee = IdEmployee;
                            attachDoc.OldFileName = original.SavedFileName;
                            attachDoc.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            attachDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                            employeeEquipment.Add(attachDoc);

                            if (original.EquipmentType != docUpdated.EquipmentType)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateEquipmentTypeForEquipmentAndToolsChangeLog").ToString()
                                    , equip, original.EquipmentType, docUpdated.EquipmentType)
                                });
                            }

                            if (original.Category != docUpdated.Category)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateEquipmentCategoryForEquipmentAndToolsChangeLog").ToString()
                                   , equip, original.Category, docUpdated.Category)
                                });
                            }


                            if (original.IsMandatory != docUpdated.IsMandatory)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateIsMandatoryForEquipmentAndToolsChangeLog").ToString()
                                  , equip, original.IsMandatory ? "Yes" : "No", docUpdated.IsMandatory ? "Yes" : "No")
                                });
                            }

                            if (original.EndDate != docUpdated.EndDate)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateEndDateForEquipmentAndToolsChangeLog").ToString()
                                    , equip, original.EndDate.ToShortDateString(), docUpdated.EndDate.ToShortDateString())
                                });
                            }

                            if (original.SavedFileName != docUpdated.SavedFileName)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForEquipmentAndToolsChangeLog").ToString()
                                   , equip, original.SavedFileName, docUpdated.SavedFileName)
                                });
                            }


                            if (original.Remarks != docUpdated.Remarks)
                            {
                                EmployeeUpdatedDetail.EmployeeChangelogs.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForEquipmentAndToolsChangeLog").ToString()
                                    , equip, string.IsNullOrEmpty(original.Remarks) ? "None" : original.Remarks, string.IsNullOrEmpty(docUpdated.Remarks) ? "None" : docUpdated.Remarks)
                                });
                            }
                        }
                    }
                }

                EmployeeUpdatedDetail.EmployeeEquipmentList = new List<EmployeeEquipmentAndTools>(employeeEquipment);
                #endregion


                #region Service Comments
                //HrmService.UpdateEmployee_V2038(EmployeeUpdatedDetail); //06-05-2020
                //[006] service method change
                // HrmService.UpdateEmployee_V2046(EmployeeUpdatedDetail);
                //[2844] Service method changed
                //[008]Service method changed
                //[011]Service method changed
                // HrmService.UpdateEmployee_V2320(EmployeeUpdatedDetail);
                //[Sudhir.jangra][GEOS2-4846]
                //HrmService.UpdateEmployee_V2480(EmployeeUpdatedDetail);
                //[Sudhir.jangra][GEOS2-5579]
                //[Sudhir.Jangra][GEOS2-4477][31/05/2023]
                // HrmService.UpdateEmployee_V2400(EmployeeUpdatedDetail);
                #endregion
                HrmService.UpdateEmployee_V2610(EmployeeUpdatedDetail); //[rdixit][GEOS2-6872][10.02.2025]

                if (File.Exists(tempMergeFile))
                    File.Delete(tempMergeFile);

                IsSaveChanges = true;

                if (EmployeeUpdatedDetail.ProfileImageInBytes == null && !isProfileImageChanged && !EmployeeUpdatedDetail.IsProfileImageDeleted && OldProfilePhotoBytes != null)
                    EmployeeUpdatedDetail.ProfileImageInBytes = ImageSourceToByteArray(ProfilePhotoSource);

                if (!string.IsNullOrEmpty(FirstName))
                    FirstName = FirstName.Trim();
                if (!string.IsNullOrEmpty(LastName))
                    LastName = LastName.Trim();
                if (!string.IsNullOrEmpty(DisplayName))
                    DisplayName = DisplayName.Trim();
                if (!string.IsNullOrEmpty(NativeName))
                    NativeName = NativeName.Trim();
                if (!string.IsNullOrEmpty(Address))
                    Address = Address.Trim();
                if (!string.IsNullOrEmpty(SelectedCity))
                    SelectedCity = SelectedCity.Trim();
                if (!string.IsNullOrEmpty(ZipCode))
                    ZipCode = ZipCode.Trim();
                if (!string.IsNullOrEmpty(Remark))
                    Remark = Remark.Trim();

                IsBusy = false;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateEmployeeSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SaveEditProfile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveEditProfile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                IsBusy = false;
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveEditProfile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                IsBusy = false;
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SaveEditProfile() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Send Mail to Person . 
        /// </summary>
        private void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);

                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in SendMailtoPerson() Method, {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Send Mail to Person----Sprint43----sdesai
        /// </summary>
        /// <param name="obj"></param>
        private void SendMail(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMail ...", category: Category.Info, priority: Priority.Low);
                System.Windows.Controls.TextBlock TextBlockDetails = (System.Windows.Controls.TextBlock)obj;
                string emailAddess = TextBlockDetails.Text;
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                GeosApplication.Instance.Logger.Log("Method SendMail() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in SendMail() Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Send Mail to Person . 
        /// </summary>
        private void SendMailtoITPersons(List<Employee> itEmployee, string emailBody)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoITPersons()", category: Category.Info, priority: Priority.Low);

                string emailAddesses = string.Join(";", itEmployee.Select(i => i.EmployeeContactEmail));

                System.Diagnostics.Process.Start("mailto:" + emailAddesses + "?subject=" + "Exit Employee " + EmployeeUpdatedDetail.EmployeeCode + "" + "&body=" + emailBody);

                GeosApplication.Instance.Logger.Log("Method SendMailtoITPersons() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoITPersons() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Close Window . 
        /// </summary>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        /// <summary>
        /// Method to calculate age of employee
        ///<para>[001][skale][2019-04-03][GEOS2-46] Wrong Age in employees</para>
        /// </summary>
        private void CalculateAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateAge()...", category: Category.Info, priority: Priority.Low);

                if (BirthDate != null)
                {
                    DateTime BDate = (DateTime)BirthDate;
                    DateTime today = DateTime.Today;
                    int employeeAge = today.Year - BDate.Year;
                    //[001] Added
                    if (BDate > today.AddYears(-employeeAge))
                    {
                        employeeAge--;
                    }
                    //End
                    Age = employeeAge.ToString();
                }
                else
                {
                    Age = null;
                }

                GeosApplication.Instance.Logger.Log("Method CalculateAge()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateAge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void FillCompanySchedules()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillCompanyShift()...", category: Category.Info, priority: Priority.Low);

        //        string _idCompany = null;
        //        if (EmployeeDetail.EmployeeJobDescriptions != null && EmployeeDetail.EmployeeJobDescriptions.Count > 0)
        //        {
        //            _idCompany = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Select(i => i.IdCompany));
        //        }

        //        CompanySchedules = HrmService.GetCompanyScheduleAndCompanyShifts(_idCompany);
        //        SelectedCompanyScheduleIndex = 0;

        //        GeosApplication.Instance.Logger.Log("Method FillCompanyShift()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanyShift()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void FillCompanySchedule(string idCompany)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillCompanySchedule()...", category: Category.Info, priority: Priority.Low);
        //        if (!string.IsNullOrEmpty(idCompany))
        //        {
        //            CompanySchedules = HrmService.GetCompanyScheduleAndCompanyShifts(idCompany);
        //            SelectedCompanyScheduleIndex = 0;
        //        }
        //        else
        //        {
        //            string _idCompany = null;
        //            if (EmployeeDetail.EmployeeJobDescriptions != null && EmployeeDetail.EmployeeJobDescriptions.Count > 0)
        //            {
        //                _idCompany = string.Join(",", EmployeeDetail.EmployeeJobDescriptions.Select(i => i.IdCompany));
        //            }

        //            CompanySchedules = HrmService.GetCompanyScheduleAndCompanyShifts(_idCompany);
        //            SelectedCompanyScheduleIndex = 0;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillCompanySchedule()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanySchedule()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void FillCompanyShift()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillCompanyShift()...", category: Category.Info, priority: Priority.Low);

        //        if (SelectedCompanyScheduleIndex > -1)
        //        {
        //            CompanyShifts = CompanySchedules[SelectedCompanyScheduleIndex].CompanyShifts;
        //            SelectedCompanyShiftIndex = 0;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillCompanyShift()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CompanyShiftSelectedIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void CompanyScheduleSelectedIndexChanged(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CompanyShiftSelectedIndexChanged()...", category: Category.Info, priority: Priority.Low);

        //        GeosApplication.Instance.Logger.Log("Method CompanyShiftSelectedIndexChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CompanyShiftSelectedIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        /// Method to Show Leaves Filter Popup
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "RegularHoursCount" &&
                e.Column.FieldName != "BacklogHoursCount" &&
                e.Column.FieldName != "Remaining" &&
                e.Column.FieldName != "EnjoyedTillYesterday" &&
                e.Column.FieldName != "EnjoyingFromToday" &&
                e.Column.ParentBand.Name != "Additional")
            {
                return;
            }

            List<object> filterItems = new List<object>();
            //if (e.Column.ParentBand.Name == "Additional")
            //{
            filterItems = (List<Object>)e.ComboBoxEdit.ItemsSource;
            for (int i = 0; i < filterItems.Count; i++)
            {
                object editValueObject = ((CustomComboBoxItem)filterItems[i]).EditValue;
                string editValueString = editValueObject.ToString();
                decimal editValueDecimal;
                bool convertedFlag = Decimal.TryParse(editValueString, out editValueDecimal);
                if (convertedFlag)
                {
                    decimal Days = (Int32)(editValueDecimal / EmployeeAnnualLeaveList[i].Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
                    decimal Hours = (editValueDecimal % EmployeeAnnualLeaveList[i].Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

                    string DisplayValues = Days.ToString() + "d";

                    if (Hours != 0)
                    {
                        DisplayValues += " " + Hours.ToString() + "H";
                    }

                    ((CustomComboBoxItem)filterItems[i]).DisplayValue = DisplayValues;
                }
            }
            if (filterItems != null && filterItems.Count > 0)
            {
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue));
            }
            GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);

            //}
            //if (e.Column.FieldName == "RegularHoursCount")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("RegularHoursCount = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("RegularHoursCount <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.RegularHoursCount == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.RegularHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)(dataObject.RegularHoursCount / dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
            //            Int32 Hours = (Int32)(dataObject.RegularHoursCount % dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.RegularHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //else if (e.Column.FieldName == "BacklogHoursCount")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("BacklogHoursCount = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("BacklogHoursCount <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.BacklogHoursCount == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.BacklogHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)(dataObject.BacklogHoursCount / dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
            //            Int32 Hours = (Int32)(dataObject.BacklogHoursCount % dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.BacklogHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //else if (e.Column.FieldName == "AdditionalHoursCount")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("AdditionalHoursCount = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("AdditionalHoursCount <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.AdditionalHoursCount == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.AdditionalHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)dataObject.AdditionalHoursCount / (Int32)dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
            //            Int32 Hours = (Int32)dataObject.AdditionalHoursCount % (Int32)dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.AdditionalHoursCount;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //else if (e.Column.FieldName == "EnjoyedTillYesterday")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("EnjoyedTillYesterday = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("EnjoyedTillYesterday <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.EnjoyedTillYesterday == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.EnjoyedTillYesterday;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)(dataObject.EnjoyedTillYesterday / dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
            //            Int32 Hours = (Int32)(dataObject.EnjoyedTillYesterday % dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.EnjoyedTillYesterday;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //else if (e.Column.FieldName == "EnjoyingFromToday")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("EnjoyingFromToday = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("EnjoyingFromToday <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.EnjoyingFromToday == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.EnjoyingFromToday;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)(dataObject.EnjoyingFromToday / dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);
            //            Int32 Hours = (Int32)(dataObject.EnjoyingFromToday % dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.EnjoyingFromToday;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //else if (e.Column.FieldName == "Remaining")
            //{
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Blanks)",
            //        EditValue = CriteriaOperator.Parse("Remaining = ''")
            //    });
            //    filterItems.Add(new CustomComboBoxItem()
            //    {
            //        DisplayValue = "(Non blanks)",
            //        EditValue = CriteriaOperator.Parse("Remaining <> ''")
            //    });
            //    foreach (var dataObject in EmployeeAnnualLeaveList)
            //    {
            //        if (dataObject.Remaining == 0)
            //        {
            //            string DisplayValues = "0d";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.Remaining;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //        else
            //        {
            //            Int32 Days = (Int32)dataObject.Remaining / (Int32)dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
            //            Int32 Hours = (Int32)dataObject.Remaining % (Int32)dataObject.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

            //            string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
            //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
            //            {
            //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
            //                customComboBoxItem.DisplayValue = DisplayValues;
            //                customComboBoxItem.EditValue = dataObject.Remaining;
            //                filterItems.Add(customComboBoxItem);
            //            }
            //        }
            //    }
            //}
            //    if (filterItems != null && filterItems.Count > 0)
            //    {
            //        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue));
            //    }
            //    GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Filter Row
        /// </summary>
        /// <param name="e"></param>
        private void CustomRowFilter(RowFilterEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomRowFilter ...", category: Category.Info, priority: Priority.Low);
                var criteria = e.Source.FilterString;
                if (criteria.Contains("In"))
                {
                    if (criteria.Contains("<>"))
                    {
                        e.Visible |= EmployeeAnnualLeaveList.Count != 0;
                    }
                }

                e.Handled = true;


                GeosApplication.Instance.Logger.Log("Method CustomRowFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void AddAdditionalBandGridColumns()
        {
            if (!dynamicColumnsAdded)
            {
                AdditionalBandGridColumns = new ObservableCollection<GridColumn>();
                //GridControl gridControl = (GridControl)e.Source;
                //GridControlBand bandAdditional = gridControl.Bands.Where(x => x.Name == "Additional").FirstOrDefault();
                List<LookupValue> AnnualAdditionalLeavesLookupList = new List<LookupValue>(CrmStartUp.GetLookupValues(76));

                if (AnnualAdditionalLeavesLookupList != null && AnnualAdditionalLeavesLookupList.Count > 0)
                {
                    for (int index = 0; index < AnnualAdditionalLeavesLookupList.Count; index++)
                    {
                        if (AnnualAdditionalLeavesLookupList[index].InUse)
                        {
                            for (int i = 0; i < EmployeeAnnualLeaveList.Count; i++)
                            {
                                bool additionalleaveExistsInDatabase =
                                    EmployeeAnnualLeaveList[i].SelectedAdditionalLeaves.Exists(
                                    x => x.IdAdditionalLeaveReason == AnnualAdditionalLeavesLookupList[index].IdLookupValue
                                        );
                                if (!additionalleaveExistsInDatabase)
                                {
                                    EmployeeAnnualAdditionalLeave ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave();
                                    ObjAdditionalLeave.AdditionalLeaveReasonName = AnnualAdditionalLeavesLookupList[index].Value;
                                    ObjAdditionalLeave.IdAdditionalLeaveReason = AnnualAdditionalLeavesLookupList[index].IdLookupValue;
                                    ObjAdditionalLeave.AdditionalLeavePosition = AnnualAdditionalLeavesLookupList[index].Position;
                                    ObjAdditionalLeave.AdditionalLeaveTotalHours = 0;
                                    ObjAdditionalLeave.ConvertedDays = 0;
                                    ObjAdditionalLeave.ConvertedHours = 0;
                                    if (EmployeeDetail != null)
                                    {
                                        ObjAdditionalLeave.IdEmployee = EmployeeDetail.IdEmployee;
                                    }
                                    ObjAdditionalLeave.Year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                                    ObjAdditionalLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    EmployeeAnnualLeaveList[i].SelectedAdditionalLeaves.Add(ObjAdditionalLeave);

                                    //EmployeeAnnualLeaveList[i].SelectedAdditionalLeaves.Add(
                                    //    (EmployeeAnnualAdditionalLeave)AnnualAdditionalLeavesLookupList[index].Clone());
                                }
                            }
                        }
                    }

                    for (int i = 0; i < EmployeeAnnualLeaveList.Count; i++)
                    {
                        EmployeeAnnualLeaveList[i].SelectedAdditionalLeaves =
                            EmployeeAnnualLeaveList[i].SelectedAdditionalLeaves.OrderBy(x => x.AdditionalLeavePosition).ToList();
                    }

                    for (int i = 0; i < AnnualAdditionalLeavesLookupList.Count; i++)
                    {
                        if (AnnualAdditionalLeavesLookupList[i].InUse)
                        {
                            var columnObj = new GridColumn()
                            {
                                Header = $"{AnnualAdditionalLeavesLookupList[i].Value}",
                                MinWidth = 100,
                                ReadOnly = true,
                                FilterPopupMode = FilterPopupMode.CheckedList,
                                AllowResizing = DevExpress.Utils.DefaultBoolean.True,
                                CellTemplate = BuildTemplate($"RowData.Row.SelectedAdditionalLeaves[{AdditionalBandGridColumns.Count}].AdditionalLeaveTotalHours"),
                                AllowSorting = DevExpress.Utils.DefaultBoolean.True,
                                AllowColumnFiltering = DevExpress.Utils.DefaultBoolean.True,
                                Binding = new Binding($"RowData.Row.SelectedAdditionalLeaves[{AdditionalBandGridColumns.Count}].AdditionalLeaveTotalHours"),
                                ColumnFilterMode = ColumnFilterMode.Value,
                                SortMode = DevExpress.XtraGrid.ColumnSortMode.Value,
                                CopyValueAsDisplayText = true
                            };
                            AdditionalBandGridColumns.Add(columnObj);
                        }
                    }
                }
                // 
                // FieldName = $"SelectedAdditionalLeaves[{i}].AdditionalLeaveTotalHours",
                dynamicColumnsAdded = true;
            }
        }

        private DataTemplate BuildTemplate(string colName)
        {
            MultiBinding multiBinding = new MultiBinding();
            Binding binding1 = new Binding(colName);
            Binding binding2 = new Binding("RowData.Row.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount");
            multiBinding.Bindings.Add(binding1);
            multiBinding.Bindings.Add(binding2);
            multiBinding.Converter = (IMultiValueConverter)(new UI.Converters.HoursToDayConverter());

            FrameworkElementFactory childFactory = new FrameworkElementFactory(typeof(TextBlock));
            childFactory.SetBinding(TextBlock.TextProperty, multiBinding);
            //  childFactory.SetValue(TextBlock.BackgroundProperty, System.Drawing.Brushes.AliceBlue);

            //FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
            //factory.AppendChild(childFactory);

            DataTemplate template = new DataTemplate();
            template.VisualTree = childFactory;

            return template;
        }

        //[001][smazhar][27-07-2020][GEOS2-2018]In the section "Contract situation" from the employee's report must appear the last contract situation, but it doesn't.
        //[002][smazhar][12-08-2020][GEOS2-1815]JD wrong shown in ERF
        private void PrintEmployeeProfileReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeProfileReport()...", category: Category.Info, priority: Priority.Low);

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

                string plantName = "";
                EmployeeProfileReport EmpProfileReport = new EmployeeProfileReport();
                EmpProfileReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpProfileReport.xrLblReportName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 12, System.Drawing.FontStyle.Bold);
                EmpProfileReport.xrLblEmpCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpHireDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpPosition.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpCompanySkype.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //EmpProfileReport.xrLblEmpStatus.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpNativeName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpLengthOfService.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpContractSituation.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpGender.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpDOB.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpCompanyMobile.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpMaritalStatus.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpDisability.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpPersonalPhone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpPersonalMobile.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpPersonalSkype.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpAddr.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpCity.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpState.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpZipCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLblEmpCountry.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfileReport.xrLabel19.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, EmpProfileReport.xrLabel19.Font.Size);
                EmpProfileReport.xrLblEmpCompanyEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                EmpProfileReport.xrLblEmpCompanyEmail.PreviewClick += OnPreviewClick;
                EmpProfileReport.xrLblEmpPersonalEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                EmpProfileReport.xrLblEmpPersonalEmail.PreviewClick += OnPreviewClick;

                EmployeeDocumentReport EmpDocumentReport = new EmployeeDocumentReport();
                EmpDocumentReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpDocumentReport.xrTable2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeJobDescriptionReport EmpJobDescriptionReport = new EmployeeJobDescriptionReport();
                EmpJobDescriptionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpJobDescriptionReport.xrTable2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeProfessionalAndEducationReport EmpQualificationReport = new EmployeeProfessionalAndEducationReport();
                EmpQualificationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpQualificationReport.xrTable4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeContractSituationReport EmpContratSituationReport = new EmployeeContractSituationReport();
                EmpContratSituationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpContratSituationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeFamilyMembersReport EmpFamilyMembersReport = new EmployeeFamilyMembersReport();
                EmpFamilyMembersReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpFamilyMembersReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeLanguageReport EmpLanguageReport = new EmployeeLanguageReport();
                EmpLanguageReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpLanguageReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeePersonalContactReport EmpPersonalContactReport = new EmployeePersonalContactReport();
                EmpPersonalContactReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpPersonalContactReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpPersonalContactReport.xrTableCell2.BeforePrint += OnBeforePrint;
                EmpPersonalContactReport.xrTableCell2.PreviewClick += OnPreviewClick;

                EmployeeProfessionalContactReport EmpProfessionalContactReport = new EmployeeProfessionalContactReport();
                EmpProfessionalContactReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpProfessionalContactReport.xrTableRow1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                EmpProfessionalContactReport.xrTableCell2.BeforePrint += OnBeforePrint;
                EmpProfessionalContactReport.xrTableCell2.PreviewClick += OnPreviewClick;

                //EmployeeLeavesReport EmpLeavesReport = new EmployeeLeavesReport();
                //EmpLeavesReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //EmpLeavesReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmployeeEducationReport EmpEducationQualificationReport = new EmployeeEducationReport();
                EmpEducationQualificationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmpEducationQualificationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                //[rdixit][GEOS2-2619][11.11.2022]
                EmployeePolyvalenceReport EmployeePolyvalenceReport = new EmployeePolyvalenceReport();
                EmployeePolyvalenceReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                EmployeePolyvalenceReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                EmpProfileReport.imgLogo.Image = ResizeImage(new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo_mini));
                CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
                EmpProfileReport.xrLabel18.Text = GeosApplication.Instance.ServerDateTime.Date.ToString("d", CultureInfo);


                Employee PrintEmployee = new Employee();
                PrintEmployee = EmployeeDetail;
                //[002]
                PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.OrderByDescending(x => x.IsMainJobDescription).First();
                if (PrintEmployee.EmployeeContractSituations.Count > 0)
                {
                    //[001]
                    EmployeeContractSituation contractSituation = PrintEmployee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault();
                    if (contractSituation != null)
                    {
                        DateTime? startDate = contractSituation.ContractSituationStartDate;

                        DateTime zeroTime = new DateTime(1, 1, 1);
                        //(cpatil) Solved bug if contract is not started yet set length of service =0 
                        //[rdixit][GEOS2-5658][17.03.2025]
                        //if (contractSituation.ContractSituationStartDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)
                        //{
                        //    PrintEmployee.LengthOfService = 0;
                        //}
                        //else
                        //{
                        //    TimeSpan span = DateTime.Now - (DateTime)contractSituation.ContractSituationStartDate;
                        //    int Los = (zeroTime + span).Year - 1;
                        //   PrintEmployee.LengthOfService = Los;
                        //}
                        PrintEmployee.LengthOfServiceString = LengthOfService;

                        EmpProfileReport.xrLblEmpContractSituation.Text = contractSituation.ContractSituation.Name;
                    }
                }

                //PrintEmployee.ProfileImageInBytes = GeosRepositoryServiceController.GetEmployeesImage(PrintEmployee.EmployeeCode);
                //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
                //PrintEmployee.ProfileImageInBytes = GetEmployeesImage(PrintEmployee.EmployeeCode);
                //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                PrintEmployee.ProfileImageInBytes = GetEmployeesImage_V2520(PrintEmployee.EmployeeCode);
                if (PrintEmployee.ProfileImageInBytes != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(PrintEmployee.ProfileImageInBytes))
                    {
                        BitmapImage image = new BitmapImage();

                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = new Bitmap(image.StreamSource);
                        EmpProfileReport.xrPbEmpProfileImage.Image = img;
                    }
                }
                else
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/No_Photo.png", UriKind.RelativeOrAbsolute))));
                        enc.Save(outStream);
                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                        Bitmap image = new Bitmap(bitmap);
                        EmpProfileReport.xrPbEmpProfileImage.Image = image;
                    }
                }

                //if(PrintEmployee.EmployeeFamilyMembers.Count<=0)
                //{
                //    empProfileReport.xrLblFamilyMembers.Visible = false;
                //    empProfileReport.xrFamilyMemberSubRpt.Visible = false;
                //}

                if (PrintEmployee.EmployeeProfessionalContacts.Count > 0)
                {
                    if (PrintEmployee.EmployeeProfessionalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 88).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpCompanyEmail.Text = PrintEmployee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88).EmployeeContactValue;
                    }

                    if (PrintEmployee.EmployeeProfessionalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 87).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpCompanySkype.Text = PrintEmployee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 87).EmployeeContactValue;
                    }
                    if (PrintEmployee.EmployeeProfessionalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 90).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpCompanyMobile.Text = PrintEmployee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90).EmployeeContactValue;
                    }
                }

                if (PrintEmployee.EmployeePersonalContacts.Count > 0)
                {
                    if (PrintEmployee.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 88).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpPersonalEmail.Text = PrintEmployee.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88).EmployeeContactValue;
                    }

                    if (PrintEmployee.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 87).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpPersonalSkype.Text = PrintEmployee.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 87).EmployeeContactValue;
                    }
                    if (PrintEmployee.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 90).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpPersonalMobile.Text = PrintEmployee.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90).EmployeeContactValue;
                    }
                    if (PrintEmployee.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 89).ToList().Count > 0)
                    {
                        EmpProfileReport.xrLblEmpPersonalPhone.Text = PrintEmployee.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 89).EmployeeContactValue;
                    }
                }
                //[rdixit][GEOS2-5658][17.03.2025]
                EmpProfileReport.xrLblEmpHireDate.Text = startDate.ToShortDateString();

                List<Employee> emp = new List<Employee>();
                emp.Add(PrintEmployee);

                List<string> companyNameList = new List<string>();

                if (PrintEmployee.EmployeeJobDescriptions.Count > 0)
                {
                    companyNameList = PrintEmployee.EmployeeJobDescriptions.Select(x => x.Company.Alias).Distinct().ToList();
                    int k = 740;
                    foreach (string item in companyNameList)
                    {
                        XRLabel label = new XRLabel() { Text = item };
                        label.WidthF = 60;
                        label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, EmpProfileReport.Font.Size);
                        EmpProfileReport.Bands[BandKind.TopMargin].Controls.Add(label);
                        label.LocationF = new System.Drawing.PointF(k, 15);
                        label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        label.ForeColor = System.Drawing.Color.White;
                        label.BackColor = System.Drawing.Color.Black;
                        k = k - 70;
                    }
                }

                EmpProfileReport.xrLabel19.Text = plantName;
                EmpProfileReport.DataSource = emp;
                EmpDocumentReport.bindingSource1.DataSource = PrintEmployee.EmployeeDocuments;
                EmpJobDescriptionReport.bindingSource1.DataSource = PrintEmployee.EmployeeJobDescriptions;
                EmpQualificationReport.bindingSource1.DataSource = PrintEmployee.EmployeeProfessionalEducations;
                EmpEducationQualificationReport.bindingSource1.DataSource = PrintEmployee.EmployeeEducationQualifications;
                EmpContratSituationReport.bindingSource1.DataSource = PrintEmployee.EmployeeContractSituations;
                EmpFamilyMembersReport.bindingSource1.DataSource = PrintEmployee.EmployeeFamilyMembers;
                EmpLanguageReport.bindingSource1.DataSource = PrintEmployee.EmployeeLanguages;
                EmpPersonalContactReport.bindingSource1.DataSource = new List<EmployeeContact>(PrintEmployee.EmployeePersonalContacts.OrderBy(x => x.EmployeeContactIdType).ToList());
                EmpProfessionalContactReport.bindingSource1.DataSource = new List<EmployeeContact>(PrintEmployee.EmployeeProfessionalContacts.OrderBy(x => x.EmployeeContactIdType).ToList());
                //EmpLeavesReport.bindingSource1.DataSource = PrintEmployee.EmployeeAnnualLeaves;
                EmployeePolyvalenceReport.bindingSource1.DataSource = PrintEmployee.EmployeePolyvalences;//[rdixit][GEOS2-2619][11.11.2022]
                EmpProfileReport.xrDocumentSubRpt.ReportSource = EmpDocumentReport;
                EmpProfileReport.xrJobDescSubRpt.ReportSource = EmpJobDescriptionReport;
                EmpProfileReport.xrProfessionalAndEducationalInfoSubRpt.ReportSource = EmpQualificationReport;
                EmpProfileReport.xrContractSituationSubRpt.ReportSource = EmpContratSituationReport;
                EmpProfileReport.xrFamilyMemberSubRpt.ReportSource = EmpFamilyMembersReport;
                EmpProfileReport.xrLanguageSubRpt.ReportSource = EmpLanguageReport;
                EmpProfileReport.xrPersonalContactSubRpt.ReportSource = EmpPersonalContactReport;
                EmpProfileReport.xrProfessionalContactSubRpt.ReportSource = EmpProfessionalContactReport;
                //EmpProfileReport.xrLeavesSubRpt.ReportSource = EmpLeavesReport;
                EmpProfileReport.xrEducationQualificationSubRpt.ReportSource = EmpEducationQualificationReport;
                EmpProfileReport.xrPolSubRpt.ReportSource = EmployeePolyvalenceReport;//[rdixit][GEOS2-2619][11.11.2022]
                if (PrintEmployee.ExitDate != null)
                {
                    // EXIT label
                    XRLabel label = new XRLabel() { Text = "EXIT" };
                    label.WidthF = 60;
                    label.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                    label.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 0F);

                    // EXIT Information Panel
                    XRPanel panel = new XRPanel();
                    panel.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    panel.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 15F);
                    // panel.SizeF = new System.Drawing.SizeF(794.9995F, 100.75018F);
                    panel.SizeF = new System.Drawing.SizeF(794.9999F, 100.8334F);

                    // Date Label
                    XRLabel label1 = new XRLabel() { Text = "Date:" };
                    label1.WidthF = 60;
                    label1.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                    label1.LocationF = new DevExpress.Utils.PointFloat(6.708002F, 7F);
                    label1.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    XRLabel label4 = new XRLabel();
                    label4.Text = PrintEmployee.ExitDate.Value.Date.ToString("d", CultureInfo);
                    label4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    label4.WidthF = 60;
                    label4.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                    label4.LocationF = new DevExpress.Utils.PointFloat(55.708002F, 7F);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    // Remark Label
                    XRLabel label2 = new XRLabel() { Text = "Remarks:" };
                    label2.WidthF = 60;
                    label2.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                    label2.LocationF = new DevExpress.Utils.PointFloat(300.708002F, 7F);
                    label2.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    XRLabel label5 = new XRLabel();
                    label5.Text = PrintEmployee.ExitRemarks;
                    label5.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    label5.WidthF = 60;
                    label5.SizeF = new System.Drawing.SizeF(405.208F, 35.83335F);
                    label5.LocationF = new DevExpress.Utils.PointFloat(360.708002F, 7F);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    // Reason Label
                    XRLabel label3 = new XRLabel() { Text = "Reason:" };
                    label3.WidthF = 60;
                    label3.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                    label3.LocationF = new DevExpress.Utils.PointFloat(6.708002F, 50F);
                    label3.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    XRLabel label6 = new XRLabel();
                    label6.WidthF = 60;
                    label6.SizeF = new System.Drawing.SizeF(710.208F, 35.83335F);
                    label6.LocationF = new DevExpress.Utils.PointFloat(55.708002F, 50F);
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Text = PrintEmployee.ExitReason.Value.ToString();
                    label6.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    panel.Controls.Add(label1);
                    panel.Controls.Add(label2);
                    panel.Controls.Add(label3);
                    panel.Controls.Add(label6);
                    panel.Controls.Add(label4);
                    panel.Controls.Add(label5);

                    //ReportFooterBand to add panel at the end of report
                    ReportFooterBand band = new ReportFooterBand();
                    band.PrintAtBottom = true;
                    band.Controls.Add(label);
                    band.Controls.Add(panel);

                    EmpProfileReport.Bands.Add(band);
                }

                //[15-04-2019] [sdesai] (#66045) Name of EmployeeProfileReport and automatic email
                //set DefaultFileName and Email.Subject
                EmployeeJobDescription tempEmployeeJobDescription;
                List<EmployeeJobDescription> tempEmployeeJobDescriptionList = PrintEmployee.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate == null).ToList();
                if (tempEmployeeJobDescriptionList.Count == 0)
                {
                    tempEmployeeJobDescription = PrintEmployee.EmployeeJobDescriptions.OrderByDescending(x => x.JobDescriptionUsage).FirstOrDefault();
                }
                else
                {
                    tempEmployeeJobDescription = tempEmployeeJobDescriptionList.OrderByDescending(x => x.JobDescriptionUsage).FirstOrDefault();
                }
                string FileName = "EmployeeProfileReport";

                if (tempEmployeeJobDescription != null)
                    FileName = "ERF_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + EmployeeDetail.EmployeeCode + "_" + GeosApplication.Instance.ServerDateTime.ToString("yyyyMMdd");
                // FileName = "ERF_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + EmployeeDetail.EmployeeCode + Regex.Replace(GeosApplication.Instance.ServerDateTime.ToString("d", CultureInfo), @"[^0-9a-zA-Z]+", "");

                EmpProfileReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                EmpProfileReport.ExportOptions.Email.Subject = FileName;

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };

                window.PreviewControl.DocumentSource = EmpProfileReport;
                EmpProfileReport.CreateDocument();
                window.Show();
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeProfileReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeProfileReport()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] Sprint46 Changes in employee report by Mayuri
        /// Method Created for hyperlink For Email in Professional and Personal Contact in report By Mayuri
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.UI.XRTableCell Cell = (DevExpress.XtraReports.UI.XRTableCell)sender;
            if (Cell.PreviousCell.PreviousCell.Text == "88")
            {
                Cell.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                Cell.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                Cell.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Cell.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// [001] Sprint46 Changes in employee report by Mayuri
        /// Method Created for Click hyperlink For Email in Professional and Personal Contact in report By Mayuri
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewClick(object sender, DevExpress.XtraReports.UI.PreviewMouseEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnPreviewClick ...", category: Category.Info, priority: Priority.Low);

                if (((DevExpress.XtraPrinting.TextBrick)e.Brick).Font.Style == System.Drawing.FontStyle.Underline)
                {
                    string emailAddess = Convert.ToString(e.Brick.Text);
                    string command = "mailto:" + emailAddess;
                    System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                    myProcess.StartInfo.FileName = command;
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.RedirectStandardOutput = false;
                    myProcess.Start();
                }

                GeosApplication.Instance.Logger.Log("Method OnPreviewClick() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnPreviewClick() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for resize image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(128, 45);

            try
            {
                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);

                return resized;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }
        //[001][cpatil][GEOS2-3497]
        private void ExitEventDeleteRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExitEventDeleteRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteExitEventInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeExitEvent employeeExitEvent = (EmployeeExitEvent)((EmployeeExitEvent)obj).Clone();

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (employeeExitEvent.IdEmployeeExitEvent != 0)
                    {
                        EmployeeExitEvent employeeExitEventTemp = ExitEventList.Where(i => i.IdEmployeeExitEvent == employeeExitEvent.IdEmployeeExitEvent).FirstOrDefault();
                        employeeExitEventTemp.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        employeeExitEventTemp.ExitDate = null;
                        employeeExitEventTemp.IdReason = 0;
                        employeeExitEventTemp.SelectedExitEventReasonIndex = 0;
                        employeeExitEventTemp.Remarks = null;
                        employeeExitEventTemp.IsEnable = true;
                        employeeExitEventTemp.IsVisible = Visibility.Collapsed;
                        employeeExitEventTemp.ExitEventBytes = null;
                        employeeExitEventTemp.ExitEventattachmentList.Clear();
                    }

                }

                GeosApplication.Instance.Logger.Log("Method ExitEventDeleteRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExitEventDeleteRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete employee leaves
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAuthorizedLeaveRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAuthorizedLeaveRecord()...", category: Category.Info, priority: Priority.Low);

                EmployeeAnnualLeave employeeAnnualLeave = (EmployeeAnnualLeave)((EmployeeAnnualLeave)obj).Clone();
                if (employeeAnnualLeave.IdLeave == 241)
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeCompansationAttendanceDeleteValidation"].ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLeaveInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (employeeAnnualLeave.IdEmployeeAnnualLeave != 0)
                        {
                            employeeAnnualLeave.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            employeeAnnualLeave.SelectedAdditionalLeaves = new List<EmployeeAnnualAdditionalLeave>(); //.Clear();
                            UpdatedEmployeeAnnualLeaveList.Add(employeeAnnualLeave);
                        }
                        else
                        {
                            if (UpdatedEmployeeAnnualLeaveList.Count > 0)
                            {
                                EmployeeAnnualLeave NewEmployeeAnnualLeave = UpdatedEmployeeAnnualLeaveList.FirstOrDefault(x => x.IdLeave == employeeAnnualLeave.IdLeave);
                                if (NewEmployeeAnnualLeave != null)
                                {
                                    UpdatedEmployeeAnnualLeaveList.Remove(employeeAnnualLeave);
                                }
                                else
                                    UpdatedEmployeeAnnualLeaveList.Add(employeeAnnualLeave);
                            }
                            else
                                UpdatedEmployeeAnnualLeaveList.Add(employeeAnnualLeave);
                        }
                        EmployeeAnnualLeaveList.Remove((EmployeeAnnualLeave)obj);

                        if (EmployeeAnnualLeaveList.Count > 0)
                        {
                            if (EmployeeAnnualLeaveList[0].Employee.CompanyShift != null)
                            {
                                TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                            }
                            else
                            {
                                EmployeeAnnualLeaveList.Clear();
                            }
                        }
                        else
                        {
                            TempEmployeeAnnualLeaveList.Clear();
                        }
                    }
                }


                GeosApplication.Instance.Logger.Log("Method DeleteAuthorizedLeaveRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAuthorizedLeaveRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to add authorized leave
        /// [001][cpatil][2020-01-06][GEOS2-1997]Authorized Leaves not added in selected period when new year comes
        /// </summary>
        /// <param name="obj"></param>
        private void AddAuthorizedLeave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);

                AddAuthorizedLeaveView addAuthorizedLeaveView = new AddAuthorizedLeaveView();
                AddAuthorizedLeaveViewModel addAuthorizedLeaveViewModel = new AddAuthorizedLeaveViewModel();
                EventHandler handle = delegate { addAuthorizedLeaveView.Close(); };
                addAuthorizedLeaveViewModel.RequestClose += handle;
                addAuthorizedLeaveView.DataContext = addAuthorizedLeaveViewModel;
                addAuthorizedLeaveViewModel.IdEmployee = EmployeeDetail.IdEmployee;
                // addAuthorizedLeaveViewModel.SelectedYear = GeosApplication.Instance.ServerDateTime.Date.Year;

                //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                addAuthorizedLeaveViewModel.SelectedYear = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                //addAuthorizedLeaveViewModel.IdCompany = EmployeeDetail.EmployeeJobDescriptions[0].IdCompany;

                if (EmployeeDetail.EmployeeJobDescriptions != null)
                {
                    if (EmployeeDetail.EmployeeJobDescriptions.Count > 1)
                    {
                        addAuthorizedLeaveViewModel.IdCompany = EmployeeDetail.EmployeeJobDescriptions.LastOrDefault().IdCompany;
                    }
                }

                if (EmployeeDetail.CompanyShift != null)
                {
                    addAuthorizedLeaveViewModel.IdCompanyShift = EmployeeDetail.CompanyShift.IdCompanyShift;
                }
                else
                    addAuthorizedLeaveViewModel.IdCompanyShift = 0;

                addAuthorizedLeaveViewModel.IsAdd = true;
                addAuthorizedLeaveViewModel.Init(EmployeeAnnualLeaveList, EmployeeDetail);
                addAuthorizedLeaveViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addAuthorizedLeaveView.Owner = Window.GetWindow(ownerInfo);
                addAuthorizedLeaveView.ShowDialog();
                if (addAuthorizedLeaveViewModel.IsSave)
                {
                    addAuthorizedLeaveViewModel.NewEmployeeAnnualLeave.Employee = EmployeeDetail;
                    addAuthorizedLeaveViewModel.NewEmployeeAnnualLeave.Employee.CompanyShift = addAuthorizedLeaveViewModel.TempCompanyShift;
                    UpdatedEmployeeAnnualLeaveList.Add(addAuthorizedLeaveViewModel.NewEmployeeAnnualLeave);
                    EmployeeAnnualLeaveList.Add(addAuthorizedLeaveViewModel.NewEmployeeAnnualLeave);
                    SelectedEmployeeAnnualLeave = addAuthorizedLeaveViewModel.NewEmployeeAnnualLeave;
                    if (EmployeeAnnualLeaveList.Count > 0)
                    {
                        TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to Edit AuthorizedLeave
        /// [001][cpatil][2020-01-06][GEOS2-1997]Authorized Leaves not added in selected period when new year comes
        /// </summary>
        /// <param name="obj"></param>
        private void EditAuthorizedLeave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeAnnualLeave AnnualLeave = (EmployeeAnnualLeave)detailView.DataControl.CurrentItem;
                SelectedEmployeeAnnualLeave = AnnualLeave;
                if (AnnualLeave != null)
                {
                    AddAuthorizedLeaveView addAuthorizedLeaveView = new AddAuthorizedLeaveView();
                    AddAuthorizedLeaveViewModel addAuthorizedLeaveViewModel = new AddAuthorizedLeaveViewModel();
                    EventHandler handle = delegate { addAuthorizedLeaveView.Close(); };
                    addAuthorizedLeaveViewModel.RequestClose += handle;
                    addAuthorizedLeaveView.DataContext = addAuthorizedLeaveViewModel;
                    addAuthorizedLeaveViewModel.IsNew = false;
                    addAuthorizedLeaveViewModel.IsEdit = true;
                    //addAuthorizedLeaveViewModel.SelectedYear = GeosApplication.Instance.ServerDateTime.Date.Year;

                    //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                    addAuthorizedLeaveViewModel.SelectedYear = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                    if (HrmCommon.Instance.IsPermissionReadOnly)
                    {
                        addAuthorizedLeaveViewModel.InitReadOnly(SelectedEmployeeAnnualLeave, EmployeeAnnualLeaveList);
                    }
                    else
                    {
                        addAuthorizedLeaveViewModel.EditInit(SelectedEmployeeAnnualLeave, EmployeeAnnualLeaveList, EmployeeDetail);
                    }

                    var ownerInfo = (detailView as FrameworkElement);
                    addAuthorizedLeaveView.Owner = Window.GetWindow(ownerInfo);
                    addAuthorizedLeaveView.ShowDialog();

                    if (addAuthorizedLeaveViewModel.IsSave)
                    {
                        AnnualLeave.LeaveInUse = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.LeaveInUse;
                        AnnualLeave.Employee = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.Employee;
                        AnnualLeave.IdLeave = (long)addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.CompanyLeave.IdCompanyLeave;
                        AnnualLeave.CompanyLeave = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.CompanyLeave;
                        AnnualLeave.RegularHoursCount = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.RegularHoursCount;
                        //AnnualLeave.AdditionalHoursCount = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.AdditionalHoursCount;
                        AnnualLeave.BacklogHoursCount = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.BacklogHoursCount;
                        AnnualLeave.EnjoyedTillYesterday = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.EnjoyedTillYesterday;
                        AnnualLeave.EnjoyingFromToday = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.EnjoyingFromToday;
                        AnnualLeave.Remaining = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.Remaining;
                        // AnnualLeave.Remaining = addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.Remaining;
                        AnnualLeave.SelectedAdditionalLeaves = addAuthorizedLeaveViewModel.AdditionalLeaveListForGrid.ToList();
                        AnnualLeave.TransactionOperation =
                            addAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.TransactionOperation;
                        // ModelBase.TransactionOperations.Update;
                        SelectedEmployeeAnnualLeave = AnnualLeave;

                        bool LeaveReplacedInList = false;
                        for (int i = 0; i < UpdatedEmployeeAnnualLeaveList.Count; i++)
                        {
                            if (UpdatedEmployeeAnnualLeaveList[i].IdLeave ==
                                SelectedEmployeeAnnualLeave.IdLeave)
                            {
                                UpdatedEmployeeAnnualLeaveList[i] = SelectedEmployeeAnnualLeave;
                                LeaveReplacedInList = true;
                                break;
                            }
                        }
                        if (!LeaveReplacedInList)
                        {
                            UpdatedEmployeeAnnualLeaveList.Add(SelectedEmployeeAnnualLeave);
                        }
                    }

                }

                GeosApplication.Instance.Logger.Log("Method EditAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// <para>[001][spawar][03-03-2020][GEOS2-1710] Mandatory exit event when the user changes the Employee Profile status to Inactive</para>
        /// </summary>
        /// <param name="obj"></param>
        private void EmployeeStatusSelectedIndexChanged(object obj)
        {
            try
            {
                //001
                //[002]
                EmployeeExitEvent employeeExitEventObj = SelectedExitEventItem;

                // set current exit event
                if (SelectedEmpolyeeStatusIndex == 1)
                {
                    if (EmployeeDetail.EmployeeExitEvents.Count == 0)
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 1).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                    else if (EmployeeDetail.EmployeeExitEvents.Count == 1)
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 1).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                    else if (EmployeeDetail.EmployeeExitEvents.Count == 4)
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 4).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }
                    else
                    {
                        EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IsExist != true).FirstOrDefault();
                        SelectedExitEventItem = employeeExitEventSelectedItems;
                    }

                }
                var CurrentItemList = ExitEventList.Where(item1 => !employeeDetail.EmployeeExitEvents.Any(item2 => item1.IdExitEvent == item2.IdExitEvent)).ToList();
                EmployeeExitEvent employeeExitEventObjlist = CurrentItemList.Where(x => x.IsExist != true).FirstOrDefault();

                if (SelectedEmpolyeeStatusIndex != 2)
                {
                    if (employeeExitEventObjlist.SelectedExitEventReasonIndex == 0)
                    {
                        employeeExitEventObjlist.SelectedEmpolyeeStatusIndex = SelectedEmpolyeeStatusIndex;
                        employeeExitEventObjlist.SelectedExitEventReasonIndex = 0;

                    }
                    if (employeeExitEventObjlist.ExitDate == null)
                    {
                        employeeExitEventObjlist.ExitDate = null;
                    }
                }

                //
                GeosApplication.Instance.Logger.Log("Method EmployeeStatusSelectedIndexChanged()...", category: Category.Info, priority: Priority.Low);
                if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                {
                    if (EmployeeJobDescriptionList.Count > 0)
                    {
                        EmployeeJobDescription EmpJobDescription = EmployeeJobDescriptionList.FirstOrDefault(x => x.JobDescriptionStartDate > DateTime.Now);
                        ObservableCollection<EmployeeJobDescription> EmpJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(x => x.JobDescriptionStartDate > DateTime.Now).ToList());
                        if (EmpJobDescription != null)
                        {
                            JobDescriptionStartDate = EmpJobDescription.JobDescriptionStartDate;
                            foreach (EmployeeJobDescription item in EmpJobDescriptionList)
                            {
                                JobDescriptionName = JobDescriptionName + " " + item.JobDescription.JobDescriptionTitle;
                            }
                            PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));
                        }
                    }
                }
                else
                    PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));

                GeosApplication.Instance.Logger.Log("Method EmployeeStatusSelectedIndexChanged()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeStatusSelectedIndexChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [SP66][000][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// [001][cpatil][26-03-2020][GEOS2-1974] Add improvements in PrintIdCard [#POC21] (linked #POC20)
        /// [002][7-21-2020][smazhar][GESO2-2262][It isn’t possible to print an IdCard/Id Badge from a distinct section than "Employees"]
        /// [003][07-04-2022][cpatil][GEOS2-3624][[ID_BADGE] Change the “Location” Field for the “Organization” one]
        /// </summary>
        /// <param name="obj"></param>
        private void PrintEmployeeIdBadgeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeIdBadgeAction()...", category: Category.Info, priority: Priority.Low);
                // [001] Changed service method
                //[002] Changed service method
                //[003] Changed service method
                List<JobDescription> tempJobDescriptionList = HrmService.GetEmployeeLatestJobDescriptionsByIdEmployee_V2260(employeeDetail.IdEmployee, HrmCommon.Instance.SelectedPeriod);

                if (EmployeeDetail.ExitDate != null && DateTime.Compare(EmployeeDetail.ExitDate.Value.Date, DateTime.Now.Date) == -1 && tempJobDescriptionList.Count == 0)
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["IDBadgeExitErrorMessage"].ToString(), EmployeeDetail.FullName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else if (tempJobDescriptionList.Count == 0)
                {
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["IDBadgeNoValidJDErrorMessage"].ToString(), EmployeeDetail.FullName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else if (tempJobDescriptionList.Count > 1)
                {
                    EmployeeIdBadgeSelectionView employeeIdBadgeSelectionView = new EmployeeIdBadgeSelectionView();
                    EmployeeIdBadgeSelectionViewModel employeeIcardBadgeSelectionViewModel = new EmployeeIdBadgeSelectionViewModel();
                    EventHandler handle = delegate { employeeIdBadgeSelectionView.Close(); };
                    employeeIcardBadgeSelectionViewModel.RequestClose += handle;
                    employeeIdBadgeSelectionView.DataContext = employeeIcardBadgeSelectionViewModel;
                    employeeIcardBadgeSelectionViewModel.Init(EmployeeDetail, HrmCommon.Instance.SelectedPeriod);
                    employeeIdBadgeSelectionView.ShowDialog();
                    EmployeeDetail.EmployeeDocuments = EmployeeDocumentList.ToList(); //[rdixit][15.11.2024][GEOS2-6013]
                    if (employeeIcardBadgeSelectionViewModel.IsPrint)
                    {
                        ShowEmployeeIdBadge(EmployeeDetail, employeeIcardBadgeSelectionViewModel.EmployeeJobDescription);
                    }
                }
                else
                {
                    ShowEmployeeIdBadge(EmployeeDetail, tempJobDescriptionList[0]);
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeIdBadgeAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeIdBadgeAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for add employee new shift
        /// [000][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        /// <param name="obj"></param>
        private void AddEmployeeShift(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEmployeeShift()...", category: Category.Info, priority: Priority.Low);

                if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Count > 0)
                {
                    string empJDCompanyIds = string.Join(",", EmployeeJobDescriptionList.Where(x => x.JobDescriptionEndDate == null || x.JobDescriptionEndDate >= DateTime.Now.Date).ToList().GroupBy(x => x.Company.IdCompany).ToList().Select(i => i.Key.ToString()));

                    if (string.IsNullOrEmpty(empJDCompanyIds))
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeShiftAddError"].ToString(), EmployeeDetail.FullName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }

                AddEmployeeShiftView addNewEmployeeShiftView = new AddEmployeeShiftView();
                AddEmployeeShiftViewModel addNewEmployeeShiftViewModel = new AddEmployeeShiftViewModel();
                EventHandler handle = delegate { addNewEmployeeShiftView.Close(); };
                addNewEmployeeShiftViewModel.RequestClose += handle;
                addNewEmployeeShiftView.DataContext = addNewEmployeeShiftViewModel;
                addNewEmployeeShiftViewModel.Init(EmployeeShiftList, EmployeeDetail, EmployeeJobDescriptionList);
                addNewEmployeeShiftViewModel.IsNew = true;
                var ownerInfo = (obj as FrameworkElement);
                addNewEmployeeShiftView.Owner = Window.GetWindow(ownerInfo);
                addNewEmployeeShiftView.ShowDialog();

                if (addNewEmployeeShiftViewModel.IsSave == true)
                {
                    EmployeeShiftList.AddRange(new ObservableCollection<EmployeeShift>(addNewEmployeeShiftViewModel.EmployeeNewShiftList));
                    //  TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());
                    //[Sudhir.Jangra][GEOS2-4477][01/06/2023]
                    TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.Where(use => use.CompanyShift.IsInUse == 1).OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());

                    #region [rdixit][25.07.2023][GEOS4037]
                    if (EmployeeShiftList != null)
                    {
                        if (EmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                        {
                            if (!TopFourEmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                            {
                                if (TopFourEmployeeShiftList.Count == 4)
                                    TopFourEmployeeShiftList.RemoveAt(3);
                                TopFourEmployeeShiftList.Insert(0, EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible));
                            }
                            else
                            {
                                EmployeeShift Fev = EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible);
                                TopFourEmployeeShiftList.Remove(Fev);
                                TopFourEmployeeShiftList.Insert(0, Fev);
                            }
                        }
                    }

                    #endregion

                    if (EmployeeShiftList.Count > 4)
                        IsShiftAsteriskVisible = true;
                    else
                        IsShiftAsteriskVisible = false;

                    //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
                    DateTime maxDate = EmployeeShiftList.Max(x => x.AccountingDate);
                    DateTime minDate = EmployeeShiftList.Min(y => y.AccountingDate);
                    bool ismorethanone = false;
                    if (EmployeeShiftList.Count > 1)
                    {
                        ismorethanone = true;
                    }

                    foreach (var temp in EmployeeShiftList)
                    {
                        if (ismorethanone == true)
                        {
                            if (temp.AccountingDate == maxDate && temp.AccountingDate != minDate)
                            {
                                temp.IsMaxVisible = Visibility.Visible;
                            }
                            else
                            {
                                temp.IsMaxVisible = Visibility.Hidden;
                            }
                        }
                        else
                        {
                            temp.IsMaxVisible = Visibility.Hidden;
                        }

                        // obj.IsMaxVisible = obj.AccountingDate == maxDate;
                    }
                }

                if (EmployeeShiftList.Count < 1)
                    EmployeeShiftError = "";
                else
                    EmployeeShiftError = null;

                GeosApplication.Instance.Logger.Log("Method AddEmployeeShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEmployeeShift()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for delete employee shift
        /// [000][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteEmployeeShift(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteEmployeeShift()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteShiftMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeShift employeeShifts = (EmployeeShift)obj;

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    #region Commented by [rdixit][GEOS2-4477][22.06.2023]
                    //bool InUse = employeeShifts.CompanyShift.IsInUse == 0;
                    //if (InUse)
                    //{
                    //    EmployeeShiftList.Remove(employeeShifts);
                    //}

                    // TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.OrderByDescending(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());
                    #endregion
                    EmployeeShiftList.Remove(employeeShifts);
                    TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.Where(use => use.CompanyShift.IsInUse == 1).OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());

                    #region [rdixit][25.07.2023][GEOS4037]
                    if (EmployeeShiftList != null)
                    {
                        if (EmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                        {
                            if (!TopFourEmployeeShiftList.Any(i => i.IsMaxVisible == Visibility.Visible))
                            {
                                if (TopFourEmployeeShiftList.Count == 4)
                                    TopFourEmployeeShiftList.RemoveAt(3);
                                TopFourEmployeeShiftList.Insert(0, EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible));
                            }
                            else
                            {
                                EmployeeShift Fev = EmployeeShiftList.FirstOrDefault(i => i.IsMaxVisible == Visibility.Visible);
                                TopFourEmployeeShiftList.Remove(Fev);
                                TopFourEmployeeShiftList.Insert(0, Fev);
                            }
                        }
                    }
                    #endregion

                    if (EmployeeShiftList.Count > 4)
                        IsShiftAsteriskVisible = true;
                    else
                        IsShiftAsteriskVisible = false;

                }
                if (EmployeeShiftList.Count < 1)
                    EmployeeShiftError = "";
                else
                    EmployeeShiftError = null;

                GeosApplication.Instance.Logger.Log("Method DeleteEmployeeShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteEmployeeShift()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [srowlo][20201002][HRM][GEOS2-2496] “Shortcuts” menu - Access directly (link and keyboard combination) to the same features. [#ERF64]
        /// </summary>
        /// <param name="obj"></param>
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

        private void LoadedAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method LoadedAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                // HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method LoadedAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadedAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintEmployeeERFForm(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeERFForm()...", category: Category.Info, priority: Priority.Low);

                EmployeeRequisitionFormView ERFform = new EmployeeRequisitionFormView();
                EmployeeRequisitionFormViewModel erfViewModel = new EmployeeRequisitionFormViewModel(); //new EmployeeProfileDetailViewModel();
                EventHandler handle = delegate { ERFform.Close(); };
                erfViewModel.RequestClose += handle;
                ERFform.DataContext = erfViewModel;
                string departmentName = string.Empty;

                //EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date).ToList()[0].JobDescription.Department.DepartmentName;

                if (EmployeeJobDescriptionList.Count == 1)
                {
                    departmentName = EmployeeJobDescriptionList[0].JobDescription.Department.DepartmentName;
                }
                else if (EmployeeJobDescriptionList.Count > 1 && EmployeeJobDescriptionList.Any(x => x.IsMainJobDescription == 1))
                {
                    departmentName = EmployeeJobDescriptionList.FirstOrDefault(x => x.IsMainJobDescription == 1).JobDescription.Department.DepartmentName;
                }
                else if (EmployeeJobDescriptionList.Any(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date))
                {
                    departmentName = EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date).FirstOrDefault(x => x.JobDescriptionUsage ==
                    EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null && a.JobDescriptionStartDate <= DateTime.Now) || a.JobDescriptionEndDate >= DateTime.Now.Date).Max(y => y.JobDescriptionUsage)).JobDescription.Department.DepartmentName;
                }
                else
                {
                    departmentName = EmployeeJobDescriptionList.FirstOrDefault(x => x.JobDescriptionUsage ==
                    EmployeeJobDescriptionList.Max(y => y.JobDescriptionUsage)).JobDescription.Department.DepartmentName;
                }

                erfViewModel.Init(EmployeeDetail, departmentName);
                var ownerInfo = (obj as FrameworkElement);
                ERFform.Owner = Window.GetWindow(ownerInfo);
                ERFform.ShowDialog();
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeERFForm()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeERFFormAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
        private byte[] GetEmployeesImage(string EmployeeCode)
        {
            try
            {
                string ProfileImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + EmployeeCode + ".png";
                byte[] ImageBytes = null;
                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage()...", category: Category.Info, priority: Priority.Low);
                try
                {

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
                }
                catch (Exception ex)
                {
                    if (GeosApplication.IsImageURLException == false)
                        GeosApplication.IsImageURLException = true;

                    if (!string.IsNullOrEmpty(ProfileImagePath))
                    {
                        ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                        GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                    }

                }

                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage()....executed successfully", category: Category.Info, priority: Priority.Low);
                return ImageBytes;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetEmployeesImage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                string ProfileImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + EmployeeCode + ".png";
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

        #region //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
        private string GenerateDisplayName()
        {
            string newDisplayName = $"{FirstName} {LastName}".Trim();

            if (string.IsNullOrEmpty(DisplayName))
            {
                DisplayName = newDisplayName;
            }

            return newDisplayName;
        }

        private void UpdateDisplayName()
        {
            string previousDisplayName = DisplayName;
            string newDisplayName = GenerateDisplayName();
            if (AnyNameChanged())
            {
                bool userClickedYes = ShowMessageBox(previousDisplayName, newDisplayName);

                if (userClickedYes)
                {
                    DisplayName = newDisplayName;
                }
            }
        }

        private bool AnyNameChanged()
        {
            bool firstNameChanged = FirstName != EmployeeDetail.FirstName;
            bool lastNameChanged = LastName != EmployeeDetail.LastName;

            if (firstNameChanged || lastNameChanged)
            {
                GenerateDisplayName();
            }

            return firstNameChanged || lastNameChanged;
        }
        private bool ShowMessageBox(string previousDisplayName, string newDisplayName)
        {
            MessageBoxResult defaultResult = MessageBoxResult.No;

            MessageBoxResult result = CustomMessageBox.Show(
                string.Format(Application.Current.Resources["NotifyDisplayNameMessage"].ToString(),
                    previousDisplayName,
                    newDisplayName),
                Application.Current.Resources["PopUpNotifyColor"].ToString(),
                CustomMessageBox.MessageImagePath.Question,
                MessageBoxButton.YesNo, defaultResult);

            return result == MessageBoxResult.Yes;
        }
        #endregion


        private void FillJobDescriptionList(List<EmployeeJobDescription> temp)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()...", category: Category.Info, priority: Priority.Low);
                var tempList = new List<EmployeeJobDescription>(temp.OrderByDescending(x => x.JobDescriptionStartDate).ToList());
                List<JobDescription> data = new List<JobDescription>();


                foreach (var item in tempList)
                {
                    List<JobDescription> tempCountryList = HrmService.GetAllJobDescriptionForApprovalResponsible_V2480((Int32)item.JobDescription.IdJobDescription).OrderBy(a => a.JobDescriptionTitleAndCode).ToList();
                    foreach (var tempItem in tempCountryList)
                    {
                        if (!data.Any(x => x.JobDescriptionTitleAndCode == tempItem.JobDescriptionTitleAndCode))
                        {
                            data.Add(tempItem);
                        }
                    }
                }

                var uniqueList = data.Take(3).ToList();

                uniqueList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---", JobDescriptionInUse = 1 });


                JobDescriptionList = new ObservableCollection<JobDescription>(uniqueList);

                //  SelectedJobDescription = JobDescriptionList.IndexOf(JobDescriptionList.FirstOrDefault(x => x.JobDescriptionTitleAndCode == "---"));

                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillJobDescriptionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5273]
        private void FillExtraHoursTime()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(144);
                ExtraHoursTimeList = new List<LookupValue>();
                ExtraHoursTimeList.Insert(0, new LookupValue() { Value = "---" });
                ExtraHoursTimeList.AddRange(tempTypeList);

                // SelectedExtraHoursTime = ExtraHoursTimeList.FirstOrDefault(x => x.Value == "Do not change");


                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-3418]
        private void FillEmployeeListForProfessionalContactEmail()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeListForProfessionalContactEmail()...", category: Category.Info, priority: Priority.Low);
                //Service GetAllEmployeeListForProfessionalContact_V2490 updated with GetAllEmployeeListForProfessionalContact_V2500 [GEOS2-5545][27.03.2024][rdixit]
                ExistProfessionalEmailEmployeeList = new ObservableCollection<Employee>(HrmService.GetAllEmployeeListForProfessionalContact_V2500());

                GeosApplication.Instance.Logger.Log("Method FillEmployeeListForProfessionalContactEmail()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeListForProfessionalContactEmail() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeListForProfessionalContactEmail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillEmployeeListForProfessionalContactEmail() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region chitra.girigosavi[21/08/2024] GEOS2-5579 HRM - New section in employee profile (1 of 3)

        private void AddEquipmentAndToolsFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddEmployeeEquipmentView addEquimentAndToolsView = new AddEmployeeEquipmentView();
                AddEmployeeEquipmentViewModel addEquimentAndToolsViewModel = new AddEmployeeEquipmentViewModel();
                EventHandler handle = delegate { addEquimentAndToolsView.Close(); };
                addEquimentAndToolsViewModel.RequestClose += handle;
                addEquimentAndToolsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddEquipmentAndToolsDocumentHeader").ToString();
                addEquimentAndToolsViewModel.IsNew = true;
                addEquimentAndToolsViewModel.Init();
                addEquimentAndToolsView.DataContext = addEquimentAndToolsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEquimentAndToolsView.Owner = Window.GetWindow(ownerInfo);
                addEquimentAndToolsView.ShowDialog();

                if (addEquimentAndToolsViewModel.IsSave)
                {
                    if (EmployeeEquipmentList == null)
                    {
                        EmployeeEquipmentList = new ObservableCollection<EmployeeEquipmentAndTools>();
                    }
                    EmployeeEquipmentList.Add(addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile);
                }
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEquipmentAndToolsFileCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenEquipmentPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenEmployeeEquipmentAndToolsPdf((EmployeeEquipmentAndTools)SelectedEmployeeEquipment, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditEquipmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeEquipmentAndTools equipmentAndToolsAttachedDoc = (EmployeeEquipmentAndTools)detailView.DataControl.CurrentItem;
                AddEmployeeEquipmentView addEquimentAndToolsView = new AddEmployeeEquipmentView();
                AddEmployeeEquipmentViewModel addEquimentAndToolsViewModel = new AddEmployeeEquipmentViewModel();
                EventHandler handle = delegate { addEquimentAndToolsView.Close(); };
                addEquimentAndToolsViewModel.RequestClose += handle;
                addEquimentAndToolsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditEquipmentAndToolsHeader").ToString();
                addEquimentAndToolsViewModel.IsNew = false;
                addEquimentAndToolsViewModel.EditInit(equipmentAndToolsAttachedDoc);
                addEquimentAndToolsView.DataContext = addEquimentAndToolsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEquimentAndToolsView.Owner = Window.GetWindow(ownerInfo);
                addEquimentAndToolsView.ShowDialog();

                if (addEquimentAndToolsViewModel.IsSave)
                {
                    equipmentAndToolsAttachedDoc.IdJobDescriptionEquipment = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.IdJobDescriptionEquipment;
                    equipmentAndToolsAttachedDoc.IdJobDescription = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.IdJobDescription;
                    equipmentAndToolsAttachedDoc.IdCategory = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.IdCategory;
                    equipmentAndToolsAttachedDoc.Category = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.Category;
                    equipmentAndToolsAttachedDoc.IdEquipment = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.IdEquipment;
                    equipmentAndToolsAttachedDoc.EquipmentType = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.EquipmentType;
                    equipmentAndToolsAttachedDoc.IsMandatory = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.IsMandatory;
                    equipmentAndToolsAttachedDoc.Remarks = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.Remarks;
                    equipmentAndToolsAttachedDoc.ExpectedDuration = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.ExpectedDuration;
                    equipmentAndToolsAttachedDoc.StartDate = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.StartDate;
                    equipmentAndToolsAttachedDoc.EndDate = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.EndDate;
                    equipmentAndToolsAttachedDoc.OriginalFileName = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.OriginalFileName;
                    equipmentAndToolsAttachedDoc.SavedFileName = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.SavedFileName;
                    equipmentAndToolsAttachedDoc.EquipmentAndToolsAttachedDocInBytes = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.EquipmentAndToolsAttachedDocInBytes;
                    equipmentAndToolsAttachedDoc.CreatedBy = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.CreatedBy;
                    equipmentAndToolsAttachedDoc.CreatedIn = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.CreatedIn;
                    EmployeeEquipmentList = new ObservableCollection<EmployeeEquipmentAndTools>(EmployeeEquipmentList);
                }

                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteEquipmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteEquipmentFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["JobDescriptionDeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeEquipmentList.Remove(SelectedEmployeeEquipment);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteEquipmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteEquipmentFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        //[Sudhir.Jangra][GEOS2-5579]
        private void AddEquipmentAndToolsFileFromRowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddEmployeeEquipmentView addEquimentAndToolsView = new AddEmployeeEquipmentView();
                AddEmployeeEquipmentViewModel addEquimentAndToolsViewModel = new AddEmployeeEquipmentViewModel();

                EmployeeEquipmentAndTools equipment = (EmployeeEquipmentAndTools)obj;

                EventHandler handle = delegate { addEquimentAndToolsView.Close(); };
                addEquimentAndToolsViewModel.RequestClose += handle;
                addEquimentAndToolsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddEquipmentAndToolsDocumentHeader").ToString();
                // addEquimentAndToolsViewModel.IsNew = true;
                addEquimentAndToolsViewModel.EditInitRequiredEquipment(equipment);
                addEquimentAndToolsView.DataContext = addEquimentAndToolsViewModel;
                // var ownerInfo = (obj as FrameworkElement);
                // addEquimentAndToolsView.Owner = Window.GetWindow(ownerInfo);
                addEquimentAndToolsView.ShowDialog();

                if (addEquimentAndToolsViewModel.IsSave)
                {
                    if (EmployeeEquipmentList == null)
                    {
                        EmployeeEquipmentList = new ObservableCollection<EmployeeEquipmentAndTools>();
                    }
                    EmployeeEquipmentList.Add(addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile);

                }
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEquipmentAndToolsFileCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Change Log

        /// <summary>
        ///[HRM-M049-30] 
        ///[GEOS2-280][0001]
        ///[001][skale][2019-09-05][GEOS2-270] JD, contract and location companies in employee profile
        ///[002][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        ///[003][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        ///[004][smazhar][17-08-2020][GEOS2-2540]Change Log not updated regarding Display Name
        /// </summary>
        public void AddChangedEmployeeLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedDetailsInLog()...", category: Category.Info, priority: Priority.Low);
                //[0001]
                //IsLongTermAbsent Changelog
                if (EmployeeDetail.IsLongTermAbsent == 0)
                    if (EmployeeUpdatedDetail.IsLongTermAbsent == 1)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = System.Windows.Application.Current.FindResource("EmployeeChangeLogIsLongTermAbsentEnabled").ToString() });
                    }
                if (EmployeeDetail.IsLongTermAbsent == 1)
                    if (EmployeeUpdatedDetail.IsLongTermAbsent == 0)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = System.Windows.Application.Current.FindResource("EmployeeChangeLogIsLongTermAbsentDisabled").ToString() });
                    }

                //IsTrainer
                if (EmployeeDetail.IsTrainer == 0)
                    if (EmployeeUpdatedDetail.IsTrainer == 1)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = System.Windows.Application.Current.FindResource("EmployeeChangeLogIsTrainerON").ToString() });
                    }
                if (EmployeeDetail.IsTrainer == 1)
                    if (EmployeeUpdatedDetail.IsTrainer == 0)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = System.Windows.Application.Current.FindResource("EmployeeChangeLogIsTrainerOFF").ToString() });
                    }
                //[00001]
                //First Name
                if (EmployeeDetail.FirstName != null && !EmployeeDetail.FirstName.Equals(FirstName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogFirstName").ToString(), EmployeeDetail.FirstName, FirstName.Trim()) });
                }
                //Last Name
                if (EmployeeDetail.LastName != null && !EmployeeDetail.LastName.Equals(LastName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLastName").ToString(), EmployeeDetail.LastName, LastName.Trim()) });
                }
                //Display Name
                if (!string.IsNullOrEmpty(EmployeeDetail.DisplayName) && !string.IsNullOrEmpty(DisplayName) && !EmployeeDetail.DisplayName.Equals(DisplayName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog()
                    {
                        IdEmployee = EmployeeDetail.IdEmployee,
                        ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                        ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLastDisplayName").ToString(),
                        EmployeeDetail.DisplayName, DisplayName.Trim())
                    });
                }
                //Display Name Null
                if (string.IsNullOrEmpty(EmployeeDetail.DisplayName) && !string.IsNullOrEmpty(DisplayName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog()
                    {
                        IdEmployee = EmployeeDetail.IdEmployee,
                        ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                        ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLastDisplayName").ToString(),
                        "None", DisplayName.Trim())
                    });
                }
                //
                if (!string.IsNullOrEmpty(EmployeeDetail.DisplayName) && string.IsNullOrEmpty(DisplayName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog()
                    {
                        IdEmployee = EmployeeDetail.IdEmployee,
                        ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                        ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLastDisplayName").ToString(),
                        EmployeeDetail.DisplayName, "None")
                    });
                }
                //Native Name
                if (!string.IsNullOrEmpty(EmployeeDetail.NativeName) && !string.IsNullOrEmpty(NativeName) && !EmployeeDetail.NativeName.Equals(NativeName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogNativeName").ToString(), EmployeeDetail.NativeName, NativeName.Trim()) });
                }
                //Native Name Null
                if (string.IsNullOrEmpty(EmployeeDetail.NativeName) && !string.IsNullOrEmpty(NativeName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogNativeName").ToString(), "None", NativeName.Trim()) });
                }
                //New Native Name Null
                if (!string.IsNullOrEmpty(EmployeeDetail.NativeName) && string.IsNullOrEmpty(NativeName))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogNativeName").ToString(), EmployeeDetail.NativeName, "None") });
                }
                //Natinality
                if (!EmployeeDetail.IdNationality.Equals(NationalityList[SelectedNationalityIndex].IdCountry))
                {
                    Country Old_Nationality = NationalityList.FirstOrDefault(x => x.IdCountry == EmployeeDetail.IdNationality);
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogNationality").ToString(), Old_Nationality.Name, NationalityList[SelectedNationalityIndex].Name) });
                }
                //Address
                if (EmployeeDetail.AddressStreet != null && !string.IsNullOrEmpty(Address) && !EmployeeDetail.AddressStreet.Equals(Address))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogAddress").ToString(), EmployeeDetail.AddressStreet, Address.Trim()) });
                }

                if (string.IsNullOrEmpty(EmployeeDetail.AddressStreet) && !string.IsNullOrEmpty(Address))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogAddress").ToString(), "None", Address.Trim()) });
                }
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressStreet) && string.IsNullOrEmpty(Address))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogAddress").ToString(), EmployeeDetail.AddressStreet, "None") });
                }
                //City
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressCity) && !string.IsNullOrEmpty(SelectedCity) && !EmployeeDetail.AddressCity.Equals(SelectedCity))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCity").ToString(), EmployeeDetail.AddressCity.Trim(), SelectedCity.Trim()) });
                }
                //City Null
                if (string.IsNullOrEmpty(EmployeeDetail.AddressCity) && !string.IsNullOrEmpty(SelectedCity))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCity").ToString(), "None", SelectedCity.Trim()) });
                }
                //New City Null
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressCity) && string.IsNullOrEmpty(SelectedCity))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCity").ToString(), EmployeeDetail.AddressCity.Trim(), "None") });
                }
                //ZipCode
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressZipCode) && !string.IsNullOrEmpty(ZipCode) && !EmployeeDetail.AddressZipCode.Equals(ZipCode))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogZipcode").ToString(), EmployeeDetail.AddressZipCode, ZipCode.Trim()) });
                }
                //Zip Code Null
                if (string.IsNullOrEmpty(EmployeeDetail.AddressZipCode) && ZipCode != null)
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogZipcode").ToString(), "None", ZipCode.Trim()) });
                }
                //New Zip Code Null
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressZipCode) && string.IsNullOrEmpty(ZipCode))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogZipcode").ToString(), EmployeeDetail.AddressZipCode, "None") });
                }
                //if (SelectedCountryRegion != null)
                //{
                //Region
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressRegion) && !string.IsNullOrEmpty(SelectedCountryRegion) && !EmployeeDetail.AddressRegion.Equals(SelectedCountryRegion))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRegion").ToString(), EmployeeDetail.AddressRegion, SelectedCountryRegion) });
                }
                //Region Null
                if (string.IsNullOrEmpty(EmployeeDetail.AddressRegion) && !string.IsNullOrEmpty(SelectedCountryRegion))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRegion").ToString(), "None", SelectedCountryRegion) });
                }
                //New Region Null
                if (!string.IsNullOrEmpty(EmployeeDetail.AddressRegion) && string.IsNullOrEmpty(SelectedCountryRegion))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRegion").ToString(), EmployeeDetail.AddressRegion, "None") });
                }
                // }
                //Country
                if (!EmployeeDetail.AddressIdCountry.Equals(CountryList[SelectedCountryIndex].IdCountry))
                {
                    Country Old_Country = CountryList.FirstOrDefault(x => x.IdCountry == EmployeeDetail.AddressIdCountry);
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCountry").ToString(), Old_Country.Name, CountryList[SelectedCountryIndex].Name) });
                }
                //Gender
                if (EmployeeDetail.IdGender != GenderList[SelectedIndexGender].IdLookupValue)
                {
                    LookupValue Old_Gender = GenderList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.IdGender);
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogGender").ToString(), Old_Gender.Value, GenderList[SelectedIndexGender].Value) });
                }
                //BirthDate
                if (EmployeeDetail.DateOfBirth != null && BirthDate != null && !EmployeeDetail.DateOfBirth.Equals(BirthDate))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBirthDate").ToString(), EmployeeDetail.DateOfBirth.Value.ToShortDateString(), BirthDate.Value.ToShortDateString()) });
                }
                if (EmployeeDetail.DateOfBirth == null && BirthDate != null)
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBirthDate").ToString(), "None", BirthDate.Value.ToShortDateString()) });
                }
                if (EmployeeDetail.DateOfBirth != null && BirthDate == null)
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBirthDate").ToString(), EmployeeDetail.DateOfBirth.Value.ToShortDateString(), "None") });
                }
                //MaritalStatus
                if (EmployeeDetail.IdMaritalStatus != MaritalStatusList[SelectedMaritalStatusIndex].IdLookupValue && SelectedMaritalStatusIndex > 0 && EmployeeDetail.IdMaritalStatus != 0)
                {
                    LookupValue Old_MaritalStatus = MaritalStatusList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.IdMaritalStatus);
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogMaritalStatus").ToString(), Old_MaritalStatus.Value, MaritalStatusList[SelectedMaritalStatusIndex].Value) });
                }
                if (EmployeeDetail.IdMaritalStatus == 0 && SelectedMaritalStatusIndex > 0)
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogMaritalStatus").ToString(), "None", MaritalStatusList[SelectedMaritalStatusIndex].Value) });
                }
                if (EmployeeDetail.IdMaritalStatus != MaritalStatusList[SelectedMaritalStatusIndex].IdLookupValue && SelectedMaritalStatusIndex == 0)
                {
                    LookupValue Old_MaritalStatus = MaritalStatusList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.IdMaritalStatus);
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogMaritalStatus").ToString(), Old_MaritalStatus.Value, "None") });
                }
                //EmployeeStatus
                if (SelectedEmpolyeeStatusIndex != -1 && EmployeeDetail.IdEmployeeStatus != null)
                {
                    if (EmployeeDetail.IdEmployeeStatus != EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue)
                    {
                        LookupValue Old_EmployeeStatus = EmployeeStatusList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.IdEmployeeStatus);
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogEmployeeStatus").ToString(), Old_EmployeeStatus.Value, EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value) });
                    }
                }
                //Sprint 42-for existing employee if user select ant status log not inserted---sdesai
                if (EmployeeDetail.IdEmployeeStatus == null)
                {
                    //Status
                    if (EmployeeDetail.IdEmployeeStatus > 0)
                    {
                        if (EmployeeDetail.IdEmployeeStatus != EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue)
                        {

                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogEmployeeStatus").ToString(), "None", EmployeeStatusList[SelectedEmpolyeeStatusIndex].Value) });
                        }
                    }
                }

                //Disability
                if (EmployeeDetail.Disability != SelectedDisability)
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogDisability").ToString(), EmployeeDetail.Disability, SelectedDisability) });
                }

                //Remarks
                if (!string.IsNullOrEmpty(EmployeeDetail.Remarks) && !string.IsNullOrEmpty(Remark) && !EmployeeDetail.Remarks.Equals(Remark))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRemarks").ToString(), EmployeeDetail.Remarks, Remark.Trim()) });
                }
                //Remarks Null
                if (string.IsNullOrEmpty(EmployeeDetail.Remarks) && !string.IsNullOrEmpty(Remark))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRemarks").ToString(), "None", Remark.Trim()) });
                }
                //New Remarks Null
                if (!string.IsNullOrEmpty(EmployeeDetail.Remarks) && string.IsNullOrEmpty(Remark))
                {
                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogRemarks").ToString(), EmployeeDetail.Remarks, "None") });
                }
                // Location  [001] added
                if (EmployeeDetail.IdCompanyLocation != LocationList[SelectedLocationIndex].IdCompany && SelectedLocationIndex > 0)
                {
                    Company Old_Company = new Company();
                    Old_Company = LocationList.FirstOrDefault(x => x.IdCompany == EmployeeDetail.IdCompanyLocation);
                    if (Old_Company != null)
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLocation").ToString(), Old_Company.Alias, LocationList[SelectedLocationIndex].Alias) });
                    else
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogLocation").ToString(), "None", LocationList[SelectedLocationIndex].Alias) });
                }
                // Backup  [001] added
                if (EmployeeDetail.IdEmployeeBackup != LstBackupEmployee[SelectedIndexBackupEmployee].IdEmployee && SelectedIndexBackupEmployee > 0)
                {

                    if (EmployeeDetail.EmployeeBackup != null)
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBackup").ToString(), EmployeeDetail.EmployeeBackup.FullName, LstBackupEmployee[SelectedIndexBackupEmployee].FullName) });
                    else
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBackup").ToString(), "None", LstBackupEmployee[SelectedIndexBackupEmployee].FullName) });
                }
                else
                {
                    if (SelectedIndexBackupEmployee == 0 && EmployeeDetail.EmployeeBackup != null)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogBackup").ToString(), EmployeeDetail.EmployeeBackup.FullName, "None") });
                    }
                }
                //End
                #region Old functionality

                //if (ExitDate1 != null && EmployeeDetail.ExitDate != null)
                //{
                //    //ExitDate
                //    if (!EmployeeDetail.ExitDate.Equals(ExitDate1))
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitDate").ToString(), EmployeeDetail.ExitDate.Value.ToShortDateString(), ExitDate1.Value.ToShortDateString()) });
                //    }
                //    //Employee Exit Reason
                //    if (EmployeeDetail.ExitIdReason != ExitReasonList[SelectedReasonIndex].IdLookupValue)
                //    {
                //        LookupValue Old_EmployeeExitReason = ExitReasonList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.ExitIdReason);
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitReason").ToString(), Old_EmployeeExitReason.Value, ExitReasonList[SelectedReasonIndex].Value) });
                //    }
                //    //Employee Exit Remark
                //    if (EmployeeDetail.ExitRemarks != null)
                //    {
                //        if (!EmployeeDetail.ExitRemarks.Equals(ExitRemark))
                //        {
                //            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), EmployeeDetail.ExitRemarks, ExitRemark.Trim()) });
                //        }
                //    }
                //    if (EmployeeDetail.ExitFileName == null && FileName != null)
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), "None", FileName) });
                //    }
                //    else if (EmployeeDetail.ExitFileName != null && FileName != null && string.IsNullOrEmpty(FileName))
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), EmployeeDetail.ExitFileName, "None") });
                //    }
                //    else if (!string.IsNullOrEmpty(EmployeeDetail.ExitFileName) && !string.IsNullOrEmpty(FileName))
                //    {
                //        if (!EmployeeDetail.ExitFileName.Equals(FileName))
                //        {
                //            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), EmployeeDetail.ExitFileName, FileName) });
                //        }
                //    }
                //}

                //if (ExitDate1 != null && EmployeeDetail.ExitDate == null)
                //{
                //    //ExitDate
                //    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitDate").ToString(), "None", ExitDate1.Value.ToShortDateString()) });

                //    //Employee Exit Reason
                //    LookupValue Old_EmployeeExitReason = ExitReasonList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.ExitIdReason);
                //    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitReason").ToString(), "None", ExitReasonList[SelectedReasonIndex].Value) });

                //    //Employee Exit Remark
                //    if (ExitRemark != null && EmployeeDetail.ExitRemarks == null)
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), "None", ExitRemark.Trim()) });

                //    if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(EmployeeDetail.ExitFileName))
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), "None", FileName) });
                //    }
                //}

                //if (ExitDate1 == null && EmployeeDetail.ExitDate != null)
                //{
                //    //ExitDate
                //    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitDate").ToString(), EmployeeDetail.ExitDate.Value.ToShortDateString(), "None") });

                //    //Employee Exit Reason
                //    LookupValue Old_EmployeeExitReason = ExitReasonList.FirstOrDefault(x => x.IdLookupValue == EmployeeDetail.ExitIdReason);
                //    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitReason").ToString(), Old_EmployeeExitReason.Value, "None") });

                //    //Employee Exit Remark
                //    if (EmployeeDetail.ExitRemarks != null)
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), EmployeeDetail.ExitRemarks, "None") });

                //    if (!string.IsNullOrEmpty(EmployeeDetail.ExitFileName))
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), EmployeeDetail.ExitFileName, "None") });
                //    }
                //}
                #endregion



                //Employee Exit Event
                if (UpdateEmployeeExitEventsList.Count > 0 || UpdateEmployeeExitEventsList != null)
                {
                    foreach (EmployeeExitEvent employeeExitEvent in UpdateEmployeeExitEventsList)
                    {
                        if (employeeExitEvent.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {

                            //ExitDate
                            EmployeeExitEvent oldExitEvent = employeeDetail.EmployeeExitEvents.Where(x => x.IdExitEvent == employeeExitEvent.IdExitEvent).FirstOrDefault();
                            if (!employeeExitEvent.ExitDate.Equals(oldExitEvent.ExitDate))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitDate").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), oldExitEvent.ExitDate.Value.ToShortDateString(), employeeExitEvent.ExitDate.Value.ToShortDateString()) });
                            }
                            //Employee Exit Reason
                            if (oldExitEvent.IdReason != ExitReasonList[employeeExitEvent.SelectedExitEventReasonIndex].IdLookupValue)
                            {
                                LookupValue Old_EmployeeExitReason = ExitReasonList.FirstOrDefault(x => x.IdLookupValue == oldExitEvent.IdReason);
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitReason").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), Old_EmployeeExitReason.Value, ExitReasonList[employeeExitEvent.SelectedExitEventReasonIndex].Value) });
                            }
                            //Employee Exit Remark
                            if (oldExitEvent.Remarks != null && employeeExitEvent.Remarks != null)
                            {
                                if (!oldExitEvent.Remarks.Equals(employeeExitEvent.Remarks))
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), oldExitEvent.Remarks.Trim(), employeeExitEvent.Remarks.Trim()) });
                                }
                            }

                            else if (oldExitEvent.Remarks == null && employeeExitEvent.Remarks != null)
                            {
                                if (!oldExitEvent.Remarks.Equals(employeeExitEvent.Remarks))
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), "None", employeeExitEvent.Remarks.Trim()) });
                                }
                            }

                            else if (oldExitEvent.Remarks != null && employeeExitEvent.Remarks == null)
                            {
                                if (!oldExitEvent.Remarks.Equals(employeeExitEvent.Remarks))
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitRemarks").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), employeeExitEvent.Remarks.Trim(), "None") });
                                }
                            }
                            //Employee Exit File
                            if (oldExitEvent.FileName == null && employeeExitEvent.FileName != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), "None", employeeExitEvent.FileName) });
                            }

                            else if (oldExitEvent.FileName != null && employeeExitEvent.FileName == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), oldExitEvent.FileName, "None") });
                            }
                            else if (!string.IsNullOrEmpty(oldExitEvent.FileName) && !string.IsNullOrEmpty(employeeExitEvent.FileName))
                            {
                                if (!oldExitEvent.FileName.Equals(employeeExitEvent.FileName))
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeExitDocumentFileName").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), oldExitEvent.FileName, employeeExitEvent.FileName) });
                                }
                            }
                        }
                        if (employeeExitEvent.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            if (employeeExitEvent.ExitDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitEvent").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent)), employeeExitEvent.ExitDate.Value.Date.ToShortDateString(), employeeExitEvent.ExitEventReasonList[employeeExitEvent.SelectedExitEventReasonIndex].Value) });
                            }

                        }
                        if (employeeExitEvent.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {

                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogExitEventDeleted").ToString(), SetTabName(Convert.ToInt32(employeeExitEvent.IdExitEvent))) });
                        }
                    }
                }

                //Company Schedule  
                //[HRM-M049-30] 
                // string _idCompany = null;
                //  if (EmployeeExistDetail.EmployeeJobDescriptions != null && EmployeeExistDetail.EmployeeJobDescriptions.Count > 0)
                //  {
                //     _idCompany = string.Join(",", EmployeeExistDetail.EmployeeJobDescriptions.Select(i => i.IdCompany));
                //  }

                // List<CompanySchedule> _tempCompanySchedule = HrmService.GetCompanyScheduleAndCompanyShifts(_idCompany);

                //CompanySchedule Old_CompanySchedule = _tempCompanySchedule.Where(c => c.CompanyShifts.Any(m => m.IdCompanyShift == EmployeeDetail.IdCompanyShift)).FirstOrDefault();
                //CompanyShift Old_CompanyShift = Old_CompanySchedule.CompanyShifts.FirstOrDefault(x => x.IdCompanyShift == EmployeeDetail.IdCompanyShift);
                //if (Old_CompanySchedule != null)
                //{
                //    if (EmployeeDetail.IdCompanyShift != CompanyShifts[SelectedCompanyShiftIndex].IdCompanyShift && SelectedCompanyShiftIndex > 0 && EmployeeDetail.IdCompanyShift != 0)
                //    {
                //        if (Old_CompanySchedule.Name != CompanySchedules[SelectedCompanyScheduleIndex].Name)
                //            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanySchedule").ToString(), Old_CompanySchedule.Name, CompanySchedules[SelectedCompanyScheduleIndex].Name) });
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanyShift").ToString(), Old_CompanyShift.ShiftTime, CompanyShifts[SelectedCompanyShiftIndex].ShiftTime) });
                //    }

                //    if (EmployeeDetail.IdCompanyShift != CompanyShifts[SelectedCompanyShiftIndex].IdCompanyShift && SelectedCompanyShiftIndex == 0)
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanySchedule").ToString(), Old_CompanySchedule.Name, "None") });
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanyShift").ToString(), Old_CompanyShift.ShiftTime, "None") });
                //    }

                //    if (EmployeeDetail.IdCompanyShift == 0 && SelectedCompanyShiftIndex > 0)
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanySchedule").ToString(), "None", CompanySchedules[SelectedCompanyScheduleIndex].Name) });
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanyShift").ToString(), "None", CompanyShifts[SelectedCompanyShiftIndex].ShiftTime) });
                //    }
                //}
                //else
                //{
                //    if (EmployeeDetail.IdCompanyShift == 0 && SelectedCompanyShiftIndex > 0)
                //    {
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanySchedule").ToString(), "None", CompanySchedules[SelectedCompanyScheduleIndex].Name) });
                //        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogCompanyShift").ToString(), "None", CompanySchedules[SelectedCompanyScheduleIndex].Name) });
                //    }
                //}



                //Personal Contact
                //ObservableCollection<EmployeeContact> ExistEmployeeContactList = new ObservableCollection<EmployeeContact>(EmployeeContactDetail.EmployeePersonalContacts.ToList());
                if (EmployeeExistDetail.EmployeePersonalContacts.ToList() != EmployeeDetail.EmployeePersonalContacts.ToList())
                {
                    foreach (EmployeeContact contact in UpdatedEmployeeContactList)
                    {
                        if (contact.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeContact oldContact = EmployeeExistDetail.EmployeePersonalContacts.FirstOrDefault(x => x.IdEmployeeContact == contact.IdEmployeeContact);
                            //ContactType
                            if (oldContact.EmployeeContactType.Value != contact.EmployeeContactType.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactType").ToString(), oldContact.EmployeeContactType.Value, contact.EmployeeContactType.Value) });
                            }
                            //ContactValue
                            if (oldContact.EmployeeContactValue != contact.EmployeeContactValue)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactValue").ToString(), contact.EmployeeContactType.Value, oldContact.EmployeeContactValue, contact.EmployeeContactValue) });
                            }
                            //ContactRemarks
                            if (!string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && !string.IsNullOrEmpty(contact.EmployeeContactRemarks) && oldContact.EmployeeContactRemarks != contact.EmployeeContactRemarks)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactRemark").ToString(), contact.EmployeeContactType.Value, oldContact.EmployeeContactRemarks, contact.EmployeeContactRemarks) });
                            }
                            //ContactRemarks Null
                            if (string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && !string.IsNullOrEmpty(contact.EmployeeContactRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactRemark").ToString(), contact.EmployeeContactType.Value, "None", contact.EmployeeContactRemarks) });
                            }
                            //New ContactRemarks Null
                            if (!string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && string.IsNullOrEmpty(contact.EmployeeContactRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactRemark").ToString(), contact.EmployeeContactType.Value, oldContact.EmployeeContactRemarks, "None") });
                            }
                        }
                        //Contact Deleted
                        if (contact.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactDelete").ToString(), contact.EmployeeContactType.Value, contact.EmployeeContactValue) });
                        }
                        //Contact Created
                        if (contact.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePersonalContactAdd").ToString(), contact.EmployeeContactType.Value, contact.EmployeeContactValue) });
                        }
                    }
                }

                //Professional Contact
                //ObservableCollection<EmployeeContact> ExistEmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>(EmployeeContactDetail.EmployeeProfessionalContacts);
                if (EmployeeExistDetail.EmployeeProfessionalContacts != EmployeeDetail.EmployeeProfessionalContacts)
                {
                    foreach (EmployeeContact prof_contact in UpdatedEmployeeProfessionalContactList)
                    {
                        if (prof_contact.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeContact oldContact = EmployeeExistDetail.EmployeeProfessionalContacts.FirstOrDefault(x => x.IdEmployeeContact == prof_contact.IdEmployeeContact);
                            //ContactType
                            if (oldContact.EmployeeContactType.Value != prof_contact.EmployeeContactType.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactType").ToString(), oldContact.EmployeeContactType.Value, prof_contact.EmployeeContactType.Value) });
                            }
                            //ContactValue
                            if (oldContact.EmployeeContactValue != prof_contact.EmployeeContactValue)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactValue").ToString(), prof_contact.EmployeeContactType.Value, oldContact.EmployeeContactValue, prof_contact.EmployeeContactValue) });
                            }
                            //ContactRemarks
                            if (!string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && !string.IsNullOrEmpty(prof_contact.EmployeeContactRemarks) && oldContact.EmployeeContactRemarks != prof_contact.EmployeeContactRemarks)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactRemark").ToString(), prof_contact.EmployeeContactType.Value, oldContact.EmployeeContactRemarks, prof_contact.EmployeeContactRemarks) });
                            }
                            //ContactRemarks Null
                            if (string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && !string.IsNullOrEmpty(prof_contact.EmployeeContactRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactRemark").ToString(), prof_contact.EmployeeContactType.Value, "None", prof_contact.EmployeeContactRemarks) });
                            }
                            //New ContactRemarks Null
                            if (!string.IsNullOrEmpty(oldContact.EmployeeContactRemarks) && string.IsNullOrEmpty(prof_contact.EmployeeContactRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactRemark").ToString(), prof_contact.EmployeeContactType.Value, oldContact.EmployeeContactRemarks, "None") });
                            }
                        }
                        //Contact Deleted
                        if (prof_contact.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactDelete").ToString(), prof_contact.EmployeeContactType.Value, prof_contact.EmployeeContactValue) });
                        }
                        //Contact Created
                        if (prof_contact.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalContactAdd").ToString(), prof_contact.EmployeeContactType.Value, prof_contact.EmployeeContactValue) });
                        }
                    }
                }

                //Identification Document
                //ObservableCollection<EmployeeDocument> ExistEmployeeIdentificationDocumentList = new ObservableCollection<EmployeeDocument>(EmployeeContactDetail.EmployeeDocuments);
                if (EmployeeExistDetail.EmployeeDocuments != EmployeeDetail.EmployeeDocuments)
                {
                    foreach (EmployeeDocument document in UpdatedEmployeeDocumentList)
                    {
                        if (document.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeDocument oldDocument = EmployeeExistDetail.EmployeeDocuments.FirstOrDefault(x => x.IdEmployeeDocument == document.IdEmployeeDocument);
                            //DocumentType
                            if (oldDocument.EmployeeDocumentType.Value != document.EmployeeDocumentType.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentType").ToString(), oldDocument.EmployeeDocumentType.Value, document.EmployeeDocumentType.Value) });
                            }
                            //DocumentName
                            if (oldDocument.EmployeeDocumentName != document.EmployeeDocumentName)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentName").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentName, document.EmployeeDocumentName) });
                            }
                            //DocumentNumber
                            if (oldDocument.EmployeeDocumentNumber != document.EmployeeDocumentNumber)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentNumber").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentNumber, document.EmployeeDocumentNumber) });
                            }
                            //IssueDate
                            if (oldDocument.EmployeeDocumentIssueDate != document.EmployeeDocumentIssueDate)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentIssueDate").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentIssueDate.Value.ToShortDateString(), document.EmployeeDocumentIssueDate.Value.ToShortDateString()) });
                            }
                            //ExpiryDate
                            if (oldDocument.EmployeeDocumentExpiryDate != null && oldDocument.EmployeeDocumentExpiryDate != document.EmployeeDocumentExpiryDate && document.EmployeeDocumentExpiryDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentExpiryDate").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentExpiryDate.Value.ToShortDateString(), document.EmployeeDocumentExpiryDate.Value.ToShortDateString()) });
                            }
                            //ExpiryDate Null
                            if (oldDocument.EmployeeDocumentExpiryDate == null && document.EmployeeDocumentExpiryDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentExpiryDate").ToString(), document.EmployeeDocumentType.Value, "None", document.EmployeeDocumentExpiryDate.Value.ToShortDateString()) });
                            }
                            //New ExpiryDate Null
                            if (oldDocument.EmployeeDocumentExpiryDate != null && document.EmployeeDocumentExpiryDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentExpiryDate").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentExpiryDate.Value.ToShortDateString(), "None") });
                            }
                            //File Name
                            if (!string.IsNullOrEmpty(oldDocument.EmployeeDocumentFileName) && !string.IsNullOrEmpty(document.EmployeeDocumentFileName) && !oldDocument.EmployeeDocumentFileName.Equals(document.EmployeeDocumentFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentFileName").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentFileName, document.EmployeeDocumentFileName) });
                            }
                            //File Name Null
                            if (string.IsNullOrEmpty(oldDocument.EmployeeDocumentFileName) && !string.IsNullOrEmpty(document.EmployeeDocumentFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentFileName").ToString(), document.EmployeeDocumentType.Value, "None", document.EmployeeDocumentFileName) });
                            }
                            //New File Name Null
                            if (!string.IsNullOrEmpty(oldDocument.EmployeeDocumentFileName) && string.IsNullOrEmpty(document.EmployeeDocumentFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentFileName").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentFileName, "None") });
                            }
                            //Remarks
                            if (!string.IsNullOrEmpty(oldDocument.EmployeeDocumentRemarks) && !string.IsNullOrEmpty(document.EmployeeDocumentRemarks) && !oldDocument.EmployeeDocumentRemarks.Equals(document.EmployeeDocumentRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentRemark").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentRemarks, document.EmployeeDocumentRemarks) });
                            }
                            //Remarks Null
                            if (string.IsNullOrEmpty(oldDocument.EmployeeDocumentRemarks) && !string.IsNullOrEmpty(document.EmployeeDocumentRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentRemark").ToString(), document.EmployeeDocumentType.Value, "None", document.EmployeeDocumentRemarks) });
                            }
                            //New Remarks Null
                            if (!string.IsNullOrEmpty(oldDocument.EmployeeDocumentRemarks) && string.IsNullOrEmpty(document.EmployeeDocumentRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentRemark").ToString(), document.EmployeeDocumentType.Value, oldDocument.EmployeeDocumentRemarks, "None") });
                            }
                        }
                        //Document Deleted
                        if (document.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentDelete").ToString(), document.EmployeeDocumentType.Value, document.EmployeeDocumentName) });
                        }
                        //Document Created
                        if (document.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeDocumentAdd").ToString(), document.EmployeeDocumentType.Value, document.EmployeeDocumentName) });
                        }
                    }
                }

                //Employee Language
                //ObservableCollection<EmployeeLanguage> ExistEmployeeLanguageList = new ObservableCollection<EmployeeLanguage>(EmployeeContactDetail.EmployeeLanguages);
                if (EmployeeExistDetail.EmployeeLanguages != EmployeeDetail.EmployeeLanguages)
                {
                    foreach (EmployeeLanguage language in UpdatedEmployeeLanguageList)
                    {
                        if (language.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeLanguage oldLanguage = EmployeeExistDetail.EmployeeLanguages.FirstOrDefault(x => x.IdEmployeeLanguage == language.IdEmployeeLanguage);
                            //Language
                            if (oldLanguage.Language.Value != language.Language.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguage").ToString(), oldLanguage.Language.Value, language.Language.Value) });
                            }
                            //Understanding
                            if (oldLanguage.UnderstandingIdLanguageLevel != language.UnderstandingIdLanguageLevel)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageUnderstand").ToString(), language.Language.Value, oldLanguage.UnderstandingLevel.Value, language.UnderstandingLevel.Value) });
                            }
                            //Speaking
                            if (oldLanguage.SpeakingIdLanguageLevel != language.SpeakingIdLanguageLevel)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageSpeaking").ToString(), language.Language.Value, oldLanguage.SpeakingLevel.Value, language.SpeakingLevel.Value) });
                            }
                            //Writing
                            if (oldLanguage.WritingIdLanguageLevel != language.WritingIdLanguageLevel)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageWriting").ToString(), language.Language.Value, oldLanguage.WritingLevel.Value, language.WritingLevel.Value) });
                            }
                            //Remarks
                            if (!string.IsNullOrEmpty(oldLanguage.LanguageRemarks) && !string.IsNullOrEmpty(language.LanguageRemarks) && !oldLanguage.LanguageRemarks.Equals(language.LanguageRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageRemark").ToString(), language.Language.Value, oldLanguage.LanguageRemarks, language.LanguageRemarks) });
                            }
                            //Remarks Null
                            if (string.IsNullOrEmpty(oldLanguage.LanguageRemarks) && !string.IsNullOrEmpty(language.LanguageRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageRemark").ToString(), language.Language.Value, "None", language.LanguageRemarks) });
                            }
                            //New Remarks Null
                            if (!string.IsNullOrEmpty(oldLanguage.LanguageRemarks) && string.IsNullOrEmpty(language.LanguageRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageRemark").ToString(), language.Language.Value, oldLanguage.LanguageRemarks, "None") });
                            }
                        }
                        //Language Deleted
                        if (language.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageDelete").ToString(), language.Language.Value) });
                        }
                        //Language Created
                        if (language.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeLanguageAdd").ToString(), language.Language.Value) });
                        }
                    }
                }

                //Employee Education Qualification

                if (EmployeeExistDetail.EmployeeEducationQualifications != EmployeeDetail.EmployeeEducationQualifications)
                {
                    foreach (EmployeeEducationQualification education in UpdatedEmployeeEducationQualificationList)
                    {
                        if (education.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeEducationQualification oldEducation = EmployeeExistDetail.EmployeeEducationQualifications.FirstOrDefault(x => x.IdEmployeeEducationQualification == education.IdEmployeeEducationQualification);

                            //Qualification Type
                            if (oldEducation.EducationQualification.Name != education.EducationQualification.Name)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationQualification").ToString(), oldEducation.EducationQualification.Name, education.EducationQualification.Name) });
                            }
                            //QualificationName
                            if (oldEducation.QualificationName != education.QualificationName)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationName").ToString(), education.EducationQualification.Name, oldEducation.QualificationName, education.QualificationName) });
                            }
                            //QualificationEntity
                            if (oldEducation.QualificationEntity != education.QualificationEntity)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationEntity").ToString(), education.EducationQualification.Name, oldEducation.QualificationEntity, education.QualificationEntity) });
                            }
                            //Classification
                            if (!string.IsNullOrEmpty(oldEducation.QualificationClassification) && oldEducation.QualificationClassification != education.QualificationClassification && !string.IsNullOrEmpty(education.QualificationClassification))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationClassification").ToString(), education.EducationQualification.Name, oldEducation.QualificationClassification, education.QualificationClassification) });
                            }
                            //Classification Null
                            if (string.IsNullOrEmpty(oldEducation.QualificationClassification) && !string.IsNullOrEmpty(education.QualificationClassification))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationClassification").ToString(), education.EducationQualification.Name, "None", education.QualificationClassification) });
                            }
                            //New Classification Null
                            if (!string.IsNullOrEmpty(oldEducation.QualificationClassification) && string.IsNullOrEmpty(education.QualificationClassification))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationClassification").ToString(), education.EducationQualification.Name, oldEducation.QualificationClassification, "None") });
                            }
                            //QualificationFile
                            if (!string.IsNullOrEmpty(oldEducation.QualificationFileName) && !string.IsNullOrEmpty(education.QualificationFileName) && oldEducation.QualificationFileName != education.QualificationFileName)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationFile").ToString(), education.EducationQualification.Name, oldEducation.QualificationFileName, education.QualificationFileName) });
                            }
                            //QualificationFile Null
                            if (string.IsNullOrEmpty(oldEducation.QualificationFileName) && !string.IsNullOrEmpty(education.QualificationFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationFile").ToString(), education.EducationQualification.Name, "None", education.QualificationFileName) });
                            }
                            //New QualificationFile Null
                            if (!string.IsNullOrEmpty(oldEducation.QualificationFileName) && string.IsNullOrEmpty(education.QualificationFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationFile").ToString(), education.EducationQualification.Name, oldEducation.QualificationFileName, "None") });
                            }
                            //QualificationStartDate
                            if (oldEducation.QualificationStartDate != null && oldEducation.QualificationStartDate != education.QualificationStartDate && education.QualificationStartDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationStartDate").ToString(), education.EducationQualification.Name, oldEducation.QualificationStartDate.Value.ToShortDateString(), education.QualificationStartDate.Value.ToShortDateString()) });
                            }
                            //QualificationStartDate Null
                            if (oldEducation.QualificationStartDate == null && education.QualificationStartDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationStartDate").ToString(), education.EducationQualification.Name, "None", education.QualificationStartDate.Value.ToShortDateString()) });
                            }
                            //New QualificationStartDate Null
                            if (oldEducation.QualificationStartDate != null && education.QualificationStartDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationStartDate").ToString(), education.EducationQualification.Name, oldEducation.QualificationStartDate.Value.ToShortDateString(), "None") });
                            }
                            //QualificationEndDate
                            if (oldEducation.QualificationEndDate != null && oldEducation.QualificationEndDate != education.QualificationEndDate && education.QualificationEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationEndDate").ToString(), education.EducationQualification.Name, oldEducation.QualificationEndDate.Value.ToShortDateString(), education.QualificationEndDate.Value.ToShortDateString()) });
                            }
                            //QualificationEndDate Null
                            if (oldEducation.QualificationEndDate == null && education.QualificationEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationEndDate").ToString(), education.EducationQualification.Name, "None", education.QualificationEndDate.Value.ToShortDateString()) });
                            }
                            //New QualificationEndDate Null
                            if (oldEducation.QualificationEndDate != null && education.QualificationEndDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationEndDate").ToString(), education.EducationQualification.Name, oldEducation.QualificationEndDate.Value.ToShortDateString(), "None") });
                            }
                            //QualificationRemarks
                            if (!string.IsNullOrEmpty(oldEducation.QualificationRemarks) && oldEducation.QualificationRemarks != education.QualificationRemarks && !string.IsNullOrEmpty(education.QualificationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationRemark").ToString(), education.EducationQualification.Name, oldEducation.QualificationRemarks, education.QualificationRemarks) });
                            }
                            //QualificationRemarks Null
                            if (string.IsNullOrEmpty(oldEducation.QualificationRemarks) && !string.IsNullOrEmpty(education.QualificationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationRemark").ToString(), education.EducationQualification.Name, "None", education.QualificationRemarks) });
                            }
                            //New QualificationRemarks Null
                            if (!string.IsNullOrEmpty(oldEducation.QualificationRemarks) && string.IsNullOrEmpty(education.QualificationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationRemark").ToString(), education.EducationQualification.Name, oldEducation.QualificationRemarks, "None") });
                            }
                        }
                        //Education Qualification Deleted
                        if (education.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationDelete").ToString(), education.EducationQualification.Name) });
                        }
                        //Education Qualification Created
                        if (education.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeEducationAdd").ToString(), education.EducationQualification.Name) });
                        }
                    }
                }

                //Employee Contract Situation
                if (EmployeeExistDetail.EmployeeContractSituations != EmployeeDetail.EmployeeContractSituations)
                {
                    foreach (EmployeeContractSituation contractSituation in UpdatedEmployeeContractSituationList)
                    {
                        if (contractSituation.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeContractSituation oldcontractSituation = EmployeeExistDetail.EmployeeContractSituations.FirstOrDefault(x => x.IdEmployeeContractSituation == contractSituation.IdEmployeeContractSituation);
                            //Contract Situation Company //01 Added
                            if (oldcontractSituation.Company != null)
                            {
                                if (oldcontractSituation.Company.IdCompany != contractSituation.IdCompany)
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationCompany").ToString(), oldcontractSituation.Company.Alias, contractSituation.Company.Alias) });
                                }
                            }
                            else
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationCompany").ToString(), "None", contractSituation.Company.Alias) });

                            //Contract Situation
                            if (oldcontractSituation.ContractSituation.Name != contractSituation.ContractSituation.Name)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituation").ToString(), oldcontractSituation.ContractSituation.Name, contractSituation.ContractSituation.Name) });
                            }
                            //ProfessionalCategory
                            if (oldcontractSituation.IdProfessionalCategory != contractSituation.IdProfessionalCategory)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationCategory").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ProfessionalCategory.Name, contractSituation.ProfessionalCategory.Name) });
                            }
                            //ContractSituationStartDate
                            if (oldcontractSituation.ContractSituationStartDate != contractSituation.ContractSituationStartDate)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationStartDate").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationStartDate.Value.ToShortDateString(), contractSituation.ContractSituationStartDate.Value.ToShortDateString()) });
                            }
                            //ContractSituationEndDate
                            if (oldcontractSituation.ContractSituationEndDate != null && oldcontractSituation.ContractSituationEndDate != contractSituation.ContractSituationEndDate && contractSituation.ContractSituationEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationEndDate").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationEndDate.Value.ToShortDateString(), contractSituation.ContractSituationEndDate.Value.ToShortDateString()) });
                            }
                            //ContractSituationEndDate Null
                            if (oldcontractSituation.ContractSituationEndDate == null && contractSituation.ContractSituationEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationEndDate").ToString(), contractSituation.ContractSituation.Name, "None", contractSituation.ContractSituationEndDate.Value.ToShortDateString()) });
                            }
                            //ContractSituationEndDate Not Null
                            if (oldcontractSituation.ContractSituationEndDate != null && contractSituation.ContractSituationEndDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationEndDate").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationEndDate.Value.ToShortDateString(), "None") });
                            }
                            //File Name
                            if (!string.IsNullOrEmpty(oldcontractSituation.ContractSituationFileName) && (oldcontractSituation.ContractSituationFileName != contractSituation.ContractSituationFileName) && (!string.IsNullOrEmpty(contractSituation.ContractSituationFileName)))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationFileName").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationFileName, contractSituation.ContractSituationFileName) });
                            }
                            //FileName Null
                            if (string.IsNullOrEmpty(oldcontractSituation.ContractSituationFileName) && !string.IsNullOrEmpty(contractSituation.ContractSituationFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationFileName").ToString(), contractSituation.ContractSituation.Name, "None", contractSituation.ContractSituationFileName) });
                            }
                            //FileName Null
                            if (!string.IsNullOrEmpty(oldcontractSituation.ContractSituationFileName) && string.IsNullOrEmpty(contractSituation.ContractSituationFileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationFileName").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationFileName, "None") });
                            }
                            //ContractSituationRemarks
                            if (!string.IsNullOrEmpty(oldcontractSituation.ContractSituationRemarks) && oldcontractSituation.ContractSituationRemarks != contractSituation.ContractSituationRemarks && !string.IsNullOrEmpty(contractSituation.ContractSituationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationRemarks").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationRemarks, contractSituation.ContractSituationRemarks) });
                            }
                            //ContractSituationRemarks Null
                            if (string.IsNullOrEmpty(oldcontractSituation.ContractSituationRemarks) && !string.IsNullOrEmpty(contractSituation.ContractSituationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationRemarks").ToString(), contractSituation.ContractSituation.Name, "None", contractSituation.ContractSituationRemarks) });
                            }
                            //New ContractSituationRemarks Null
                            if (!string.IsNullOrEmpty(oldcontractSituation.ContractSituationRemarks) && string.IsNullOrEmpty(contractSituation.ContractSituationRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationRemarks").ToString(), contractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationRemarks, "None") });
                            }
                            // File Bytes
                            if (contractSituation.ContractSituationFileInBytes != null && oldcontractSituation.ContractSituationFileInBytes == null && contractSituation.ContractSituationFileName == oldcontractSituation.ContractSituationFileName)
                            {
                                foreach (var eachContractSituation in ExistEmployeeContarctSituationFromMergeWindow)
                                {
                                    if ((eachContractSituation.ContractSituationFileName.Equals(oldcontractSituation.ContractSituationFileName) && eachContractSituation.IdEmployeeContractSituation.Equals(oldcontractSituation.IdEmployeeContractSituation)) || ExistEmployeeContarctSituationFromMergeWindow.Count == 1)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(eachContractSituation.ContractSituationFileName) && eachContractSituation.ContractSituationFileInBytes != null)
                                        {
                                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationFileMerged").ToString(), oldcontractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationFileName, eachContractSituation.ContractSituationFileName) });
                                        }
                                    }
                                }
                                //EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationFileMerged").ToString(), oldcontractSituation.ContractSituation.Name, oldcontractSituation.ContractSituationFileName, contractSituation.ContractSituationFileName) });
                            }
                        }
                        //Contract Situation Deleted
                        if (contractSituation.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationDelete").ToString(), contractSituation.ContractSituation.Name) });
                        }
                        //Contract Situation Created
                        if (contractSituation.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeContractSituationAdd").ToString(), contractSituation.ContractSituation.Name) });
                        }
                    }
                }
                //[002] added
                //Employee polyvalence
                if (EmployeeExistDetail.EmployeePolyvalences != EmployeeDetail.EmployeePolyvalences)
                {
                    foreach (EmployeePolyvalence employeePolyvalence in UpdatedEmployeePolyvalenceList)
                    {
                        if (employeePolyvalence.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeePolyvalence oldEmployeePolyvalence = EmployeeExistDetail.EmployeePolyvalences.FirstOrDefault(x => x.IdEmployeePolyvalence == employeePolyvalence.IdEmployeePolyvalence);

                            //Employee polyvalence Organization
                            if (oldEmployeePolyvalence.Company.IdCompany != employeePolyvalence.Company.IdCompany)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceOrganization").ToString(), oldEmployeePolyvalence.Company.Alias, employeePolyvalence.Company.Alias) });
                            }
                            //JobDescription
                            if (oldEmployeePolyvalence.IdJobDescription != employeePolyvalence.IdJobDescription)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceJobDescriptionCode").ToString(), employeePolyvalence.Company.Alias, oldEmployeePolyvalence.JobDescription.JobDescriptionCode, employeePolyvalence.JobDescription.JobDescriptionCode) });
                            }
                            // Polyvalence Percentage
                            if (oldEmployeePolyvalence.PolyvalenceUsage != employeePolyvalence.PolyvalenceUsage)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalencePercentage").ToString(), employeePolyvalence.Company.Alias, oldEmployeePolyvalence.PolyvalenceUsage, employeePolyvalence.PolyvalenceUsage) });
                            }
                            //Polyvalence Remarks
                            if (!string.IsNullOrEmpty(oldEmployeePolyvalence.PolyvalenceRemarks) && oldEmployeePolyvalence.PolyvalenceRemarks != employeePolyvalence.PolyvalenceRemarks && !string.IsNullOrEmpty(employeePolyvalence.PolyvalenceRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceRemarks").ToString(), employeePolyvalence.Company.Alias, oldEmployeePolyvalence.PolyvalenceRemarks, employeePolyvalence.PolyvalenceRemarks) });
                            }
                            //Polyvalence Remarks Null
                            if (string.IsNullOrEmpty(oldEmployeePolyvalence.PolyvalenceRemarks) && !string.IsNullOrEmpty(employeePolyvalence.PolyvalenceRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceRemarks").ToString(), employeePolyvalence.Company.Alias, "None", employeePolyvalence.PolyvalenceRemarks) });
                            }
                            //New Polyvalence Remarks Null
                            if (!string.IsNullOrEmpty(oldEmployeePolyvalence.PolyvalenceRemarks) && string.IsNullOrEmpty(employeePolyvalence.PolyvalenceRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceRemarks").ToString(), employeePolyvalence.Company.Alias, oldEmployeePolyvalence.PolyvalenceRemarks, "None") });
                            }
                        }
                        //Polyvalence Deleted
                        if (employeePolyvalence.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceDelete").ToString(), employeePolyvalence.Company.Alias, employeePolyvalence.JobDescription.JobDescriptionTitle + " " + employeePolyvalence.JobDescription.Abbreviation) });
                        }
                        // Polyvalence Created
                        if (employeePolyvalence.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeePolyvalenceAdd").ToString(), employeePolyvalence.Company.Alias, employeePolyvalence.JobDescription.JobDescriptionTitle + " " + employeePolyvalence.JobDescription.Abbreviation) });
                        }
                    }
                }
                //END
                //Employee Family Members
                //ObservableCollection<EmployeeFamilyMember> ExistEmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>(EmployeeContactDetail.EmployeeFamilyMembers);
                if (EmployeeExistDetail.EmployeeFamilyMembers != EmployeeDetail.EmployeeFamilyMembers)
                {
                    foreach (EmployeeFamilyMember familyMember in UpdatedEmployeeFamilyMemberList)
                    {
                        if (familyMember.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeFamilyMember oldfamilyMember = EmployeeExistDetail.EmployeeFamilyMembers.FirstOrDefault(x => x.IdEmployeeFamilyMember == familyMember.IdEmployeeFamilyMember);
                            //FamilyMemberFirstName
                            if (oldfamilyMember.FamilyMemberFirstName != familyMember.FamilyMemberFirstName)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstName").ToString(), oldfamilyMember.FamilyMemberFirstName, familyMember.FamilyMemberFirstName) });
                            }
                            //FamilyMemberLastName
                            if (oldfamilyMember.FamilyMemberLastName != familyMember.FamilyMemberLastName)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberLastName").ToString(), oldfamilyMember.FamilyMemberLastName, familyMember.FamilyMemberLastName) });
                            }
                            //FamilyMemberNativeName
                            if (!string.IsNullOrEmpty(oldfamilyMember.FamilyMemberNativeName) && oldfamilyMember.FamilyMemberNativeName != familyMember.FamilyMemberNativeName && !string.IsNullOrEmpty(familyMember.FamilyMemberNativeName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberNativeName").ToString(), oldfamilyMember.FamilyMemberNativeName, familyMember.FamilyMemberNativeName) });
                            }
                            //FamilyMemberNativeName Null
                            if (string.IsNullOrEmpty(oldfamilyMember.FamilyMemberNativeName) && !string.IsNullOrEmpty(familyMember.FamilyMemberNativeName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberNativeName").ToString(), "None", familyMember.FamilyMemberNativeName) });
                            }
                            //New FamilyMemberNativeName null
                            if (!string.IsNullOrEmpty(oldfamilyMember.FamilyMemberNativeName) && string.IsNullOrEmpty(familyMember.FamilyMemberNativeName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberNativeName").ToString(), oldfamilyMember.FamilyMemberNativeName, "None") });
                            }
                            //FamilyMemberNationality
                            if (oldfamilyMember.FamilyMemberIdNationality != familyMember.FamilyMemberIdNationality)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberNationality").ToString(), oldfamilyMember.FamilyMemberNationality.Name, familyMember.FamilyMemberNationality.Name) });
                            }
                            //FamilyMemberRelationshipType
                            if (oldfamilyMember.FamilyMemberIdRelationshipType != familyMember.FamilyMemberIdRelationshipType)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberRelationType").ToString(), oldfamilyMember.FamilyMemberRelationshipType.Value, familyMember.FamilyMemberRelationshipType.Value) });
                            }
                            //Gender
                            if (oldfamilyMember.FamilyMemberIdGender != familyMember.FamilyMemberIdGender)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberGender").ToString(), oldfamilyMember.FamilyMemberGender.Value, familyMember.FamilyMemberGender.Value) });
                            }
                            //Disability
                            if (oldfamilyMember.FamilyMemberDisability != familyMember.FamilyMemberDisability)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberDisability").ToString(), oldfamilyMember.FamilyMemberDisability, familyMember.FamilyMemberDisability) });
                            }
                            //Date Of Birth
                            if (oldfamilyMember.FamilyMemberBirthDate != null && oldfamilyMember.FamilyMemberBirthDate != familyMember.FamilyMemberBirthDate && familyMember.FamilyMemberBirthDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberBirthDate").ToString(), oldfamilyMember.FamilyMemberBirthDate.Value.ToShortDateString(), familyMember.FamilyMemberBirthDate.Value.ToShortDateString()) });
                            }
                            //Date Of Birth null
                            if (oldfamilyMember.FamilyMemberBirthDate == null && familyMember.FamilyMemberBirthDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberBirthDate").ToString(), "None", familyMember.FamilyMemberBirthDate.Value.ToShortDateString()) });
                            }
                            //New Date Of Birth null
                            if (oldfamilyMember.FamilyMemberBirthDate != null && familyMember.FamilyMemberBirthDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberBirthDate").ToString(), oldfamilyMember.FamilyMemberBirthDate.Value.ToShortDateString(), "None") });
                            }
                            //FamilyMemberIsDependent
                            if (oldfamilyMember.FamilyMemberIsDependent != familyMember.FamilyMemberIsDependent)
                            {
                                string OldIsDependent = "", NewIsDependent = "";
                                if (oldfamilyMember.FamilyMemberIsDependent == 0)
                                {
                                    OldIsDependent = "No";
                                }
                                if (oldfamilyMember.FamilyMemberIsDependent == 1)
                                {
                                    OldIsDependent = "Yes";
                                }
                                if (familyMember.FamilyMemberIsDependent == 0)
                                {
                                    NewIsDependent = "No";
                                }
                                if (familyMember.FamilyMemberIsDependent == 1)
                                {
                                    NewIsDependent = "Yes";
                                }
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberIsDependent").ToString(), OldIsDependent, NewIsDependent) });
                            }
                            //FamilyMemberRemarks
                            if (!string.IsNullOrEmpty(oldfamilyMember.FamilyMemberRemarks) && oldfamilyMember.FamilyMemberRemarks != familyMember.FamilyMemberRemarks && !string.IsNullOrEmpty(familyMember.FamilyMemberRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberRemarks").ToString(), oldfamilyMember.FamilyMemberRemarks, familyMember.FamilyMemberRemarks) });
                            }
                            //FamilyMemberRemarks Null
                            if (string.IsNullOrEmpty(oldfamilyMember.FamilyMemberRemarks) && !string.IsNullOrEmpty(familyMember.FamilyMemberRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberRemarks").ToString(), "None", familyMember.FamilyMemberRemarks) });
                            }
                            //New FamilyMemberRemarks Null
                            if (!string.IsNullOrEmpty(oldfamilyMember.FamilyMemberRemarks) && string.IsNullOrEmpty(familyMember.FamilyMemberRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberRemarks").ToString(), oldfamilyMember.FamilyMemberRemarks, "None") });
                            }
                        }
                        //Family Member Deleted
                        if (familyMember.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberDelete").ToString(), familyMember.FamilyMemberFirstName, familyMember.FamilyMemberLastName) });
                        }
                        //Family Member Created
                        if (familyMember.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeFamilyMemberAdd").ToString(), familyMember.FamilyMemberFirstName, familyMember.FamilyMemberLastName) });
                        }
                    }
                }

                //Job Description               
                if (EmployeeExistDetail.EmployeeJobDescriptions != EmployeeDetail.EmployeeJobDescriptions)
                {
                    foreach (EmployeeJobDescription job_description in UpdatedEmployeeJobDescriptionList)
                    {
                        if (job_description.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeJobDescription oldJobDescription = EmployeeExistDetail.EmployeeJobDescriptions.FirstOrDefault(x => x.IdEmployeeJobDescription == job_description.IdEmployeeJobDescription);
                            //JobDescriptionOrganization
                            if (oldJobDescription.Company.IdCompany != job_description.Company.IdCompany)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionOrganization").ToString(), oldJobDescription.Company.Alias, job_description.Company.Alias) });
                            }
                            //JobDescription
                            if (oldJobDescription.IdJobDescription != job_description.IdJobDescription)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionCode").ToString(), job_description.Company.Alias, oldJobDescription.JobDescription.JobDescriptionCode, job_description.JobDescription.JobDescriptionCode) });
                            }
                            //JobDescriptionPercentage
                            if (oldJobDescription.JobDescriptionUsage != job_description.JobDescriptionUsage)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionPercentage").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionUsage, job_description.JobDescriptionUsage) });
                            }
                            //JobDescriptionStartDate
                            if (oldJobDescription.JobDescriptionStartDate != job_description.JobDescriptionStartDate)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionStartDate").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionStartDate.Value.ToShortDateString(), job_description.JobDescriptionStartDate.Value.ToShortDateString()) });
                            }
                            //JobDescriptionEndDate
                            if (oldJobDescription.JobDescriptionEndDate != null && oldJobDescription.JobDescriptionEndDate != job_description.JobDescriptionEndDate && job_description.JobDescriptionEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionEndDate").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionEndDate.Value.ToShortDateString(), job_description.JobDescriptionEndDate.Value.ToShortDateString()) });
                            }
                            //JobDescriptionEndDate Null
                            if (oldJobDescription.JobDescriptionEndDate == null && job_description.JobDescriptionEndDate != null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionEndDate").ToString(), job_description.Company.Alias, "None", job_description.JobDescriptionEndDate.Value.ToShortDateString()) });
                            }
                            //New JobDescriptionEndDate Null
                            if (oldJobDescription.JobDescriptionEndDate != null && job_description.JobDescriptionEndDate == null)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionEndDate").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionEndDate.Value.ToShortDateString(), "None") });
                            }
                            //JobDescriptionRemarks
                            if (!string.IsNullOrEmpty(oldJobDescription.JobDescriptionRemarks) && oldJobDescription.JobDescriptionRemarks != job_description.JobDescriptionRemarks && !string.IsNullOrEmpty(job_description.JobDescriptionRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionRemarks").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionRemarks, job_description.JobDescriptionRemarks) });
                            }
                            //JobDescriptionRemarks Null
                            if (string.IsNullOrEmpty(oldJobDescription.JobDescriptionRemarks) && !string.IsNullOrEmpty(job_description.JobDescriptionRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionRemarks").ToString(), job_description.Company.Alias, "None", job_description.JobDescriptionRemarks) });
                            }
                            //New JobDescriptionRemarks Null
                            if (!string.IsNullOrEmpty(oldJobDescription.JobDescriptionRemarks) && string.IsNullOrEmpty(job_description.JobDescriptionRemarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionRemarks").ToString(), job_description.Company.Alias, oldJobDescription.JobDescriptionRemarks, "None") });
                            }
                        }
                        //Job Description Deleted
                        if (job_description.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionDelete").ToString(), job_description.Company.Alias, job_description.JobDescription.JobDescriptionTitle) });
                        }
                        //Job Description Created
                        if (job_description.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionAdd").ToString(), job_description.Company.Alias, job_description.JobDescription.JobDescriptionTitle) });
                        }
                    }
                }

                //Employee Professional Education
                if (EmployeeExistDetail.EmployeeProfessionalEducations != EmployeeDetail.EmployeeProfessionalEducations)
                {
                    foreach (EmployeeProfessionalEducation prof_education in UpdatedemployeeProfessionalEducationList)
                    {
                        if (prof_education.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            EmployeeProfessionalEducation old_prof_Education = EmployeeExistDetail.EmployeeProfessionalEducations.FirstOrDefault(x => x.IdEmployeeProfessionalEducation == prof_education.IdEmployeeProfessionalEducation);

                            //Education Type
                            if (old_prof_Education.Type.Value != prof_education.Type.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationType").ToString(), old_prof_Education.Type.Value, prof_education.Type.Value) });
                            }
                            //Education Description
                            if (old_prof_Education.Name != prof_education.Name)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationName").ToString(), prof_education.Type.Value, old_prof_Education.Name, prof_education.Name) });
                            }
                            //Education Entity
                            if (old_prof_Education.Entity != prof_education.Entity)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationEntity").ToString(), prof_education.Type.Value, old_prof_Education.Entity, prof_education.Entity) });
                            }
                            //Education Duration Unit
                            if (old_prof_Education.DurationUnit.Value != prof_education.DurationUnit.Value)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationDurationUnit").ToString(), prof_education.Type.Value, old_prof_Education.DurationUnit.Value, prof_education.DurationUnit.Value) });
                            }
                            //Education Duration Value
                            if (old_prof_Education.DurationValue != prof_education.DurationValue)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationDurationValue").ToString(), prof_education.Type.Value, old_prof_Education.DurationValue, prof_education.DurationValue) });
                            }
                            //QualificationStartDate
                            if (old_prof_Education.StartDate != prof_education.StartDate)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationStartDate").ToString(), prof_education.Type.Value, old_prof_Education.StartDate.ToShortDateString(), prof_education.StartDate.ToShortDateString()) });
                            }
                            //QualificationEndDate
                            if (old_prof_Education.EndDate != prof_education.EndDate)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationEndDate").ToString(), prof_education.Type.Value, old_prof_Education.EndDate.ToShortDateString(), prof_education.EndDate.ToShortDateString()) });
                            }
                            //QualificationFile
                            if (!string.IsNullOrEmpty(old_prof_Education.FileName) && old_prof_Education.FileName != prof_education.FileName && !string.IsNullOrEmpty(prof_education.FileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationFile").ToString(), prof_education.Type.Value, old_prof_Education.FileName, prof_education.FileName) });
                            }
                            //QualificationFile Null
                            if (string.IsNullOrEmpty(old_prof_Education.FileName) && !string.IsNullOrEmpty(prof_education.FileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationFile").ToString(), prof_education.Type.Value, "None", prof_education.FileName) });
                            }
                            //New QualificationFile Null
                            if (!string.IsNullOrEmpty(old_prof_Education.FileName) && string.IsNullOrEmpty(prof_education.FileName))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationFile").ToString(), prof_education.Type.Value, old_prof_Education.FileName, "None") });
                            }
                            //QualificationRemarks
                            if (!string.IsNullOrEmpty(old_prof_Education.Remarks) && old_prof_Education.Remarks != prof_education.Remarks && !string.IsNullOrEmpty(prof_education.Remarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationRemark").ToString(), prof_education.Type.Value, old_prof_Education.Remarks, prof_education.Remarks) });
                            }
                            //QualificationRemarks Null
                            if (string.IsNullOrEmpty(old_prof_Education.Remarks) && !string.IsNullOrEmpty(prof_education.Remarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationRemark").ToString(), prof_education.Type.Value, "None", prof_education.Remarks) });
                            }
                            //New QualificationRemarks Null
                            if (!string.IsNullOrEmpty(old_prof_Education.Remarks) && string.IsNullOrEmpty(prof_education.Remarks))
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationRemark").ToString(), prof_education.Type.Value, old_prof_Education.Remarks, "None") });
                            }
                        }
                        //Education Qualification Deleted
                        if (prof_education.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationDelete").ToString(), prof_education.Type.Value) });
                        }
                        //Education Qualification Created
                        if (prof_education.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalEducationAdd").ToString(), prof_education.Type.Value) });
                        }
                    }
                }

                //Employee Authorized Leave
                if (EmployeeExistDetail.EmployeeAnnualLeaves != EmployeeDetail.EmployeeAnnualLeaves)
                {
                    foreach (EmployeeAnnualLeave annual_leave in UpdatedEmployeeAnnualLeaveList)
                    {
                        EmployeeAnnualLeave oldAnnualLeave = EmployeeExistDetail.EmployeeAnnualLeaves.FirstOrDefault(x => x.IdEmployeeAnnualLeave == annual_leave.IdEmployeeAnnualLeave);

                        //Employee Authorized Leave created
                        if (annual_leave.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = EmployeeDetail.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAuthorizedLeaveAdd").ToString(), annual_leave.CompanyLeave.Name)
                            });

                        }

                        //Employee Authorized Leave deleted
                        if (annual_leave.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = EmployeeDetail.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAuthorizedLeaveDelete").ToString(), annual_leave.CompanyLeave.Name)
                            });


                            //Parent record is deleted. It means child record deleted.
                            var deletedLeaveNamesList = new List<string>();
                            for (int i = 0; i < oldAnnualLeave.SelectedAdditionalLeaves.Count; i++)
                            {
                                string leaveName = $"{oldAnnualLeave.SelectedAdditionalLeaves[i].AdditionalLeaveReasonName}";
                                deletedLeaveNamesList.Add(leaveName);
                            }
                            if (deletedLeaveNamesList.Count > 0)
                            {
                                string allAdditionalLeaves = $"'{annual_leave.CompanyLeave.Name}' {String.Join(", ", deletedLeaveNamesList)}";

                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAuthorizedLeaveDelete").ToString(),
                                    allAdditionalLeaves)
                                });
                            }
                            //}

                        }

                        //Employee Authorized Leave updated
                        if (annual_leave.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            UpdateEmployeeAnnualLeaveInEmployeeChangeLogList(
                                oldAnnualLeave.CompanyLeave.Name,
                                "Backlog",
                                oldAnnualLeave.BacklogHoursCount,
                                oldAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                annual_leave.BacklogHoursCount,
                                annual_leave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                Application.Current.FindResource("EmployeeAuthorizedLeaveChange").ToString());

                            UpdateEmployeeAnnualLeaveInEmployeeChangeLogList(
                               oldAnnualLeave.CompanyLeave.Name,
                               "Regular",
                               oldAnnualLeave.RegularHoursCount,
                               oldAnnualLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                               annual_leave.RegularHoursCount,
                               annual_leave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                               Application.Current.FindResource("EmployeeAuthorizedLeaveChange").ToString());

                            if (oldAnnualLeave.CompanyLeave.Name != annual_leave.CompanyLeave.Name)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog()
                                {
                                    IdEmployee = EmployeeDetail.IdEmployee,
                                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAuthorizedLeaveType").ToString(), oldAnnualLeave.CompanyLeave.Name, annual_leave.CompanyLeave.Name)
                                });
                            }
                        }

                        var leaveNamesList = new List<string>();

                        for (int i = 0; i < annual_leave.SelectedAdditionalLeaves.Count; i++)
                        {
                            string leaveName = $"{annual_leave.SelectedAdditionalLeaves[i].AdditionalLeaveReasonName}";

                            if (annual_leave.SelectedAdditionalLeaves[i].TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                leaveNamesList.Add(leaveName);
                            }
                        }

                        if (leaveNamesList.Count > 0)
                        {
                            string allAdditionalLeaves = $"'{annual_leave.CompanyLeave.Name}' {String.Join(", ", leaveNamesList)}";
                            EmployeeChangeLogList.Add(new EmployeeChangelog()
                            {
                                IdEmployee = EmployeeDetail.IdEmployee,
                                ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                                ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeAuthorizedLeaveAdd").ToString(),
                                       allAdditionalLeaves)
                            });
                        }

                        for (int i = 0; i < annual_leave.SelectedAdditionalLeaves.Count; i++)
                        {
                            if (annual_leave.SelectedAdditionalLeaves[i].TransactionOperation == ModelBase.TransactionOperations.Update)
                            {

                                var oldAnnualAdditionalLeave =
                                oldAnnualLeave.SelectedAdditionalLeaves.FirstOrDefault(x =>
                                x.IdAdditionalLeaveReason ==
                                annual_leave.SelectedAdditionalLeaves[i].IdAdditionalLeaveReason);

                                //UpdateEmployeeAnnualLeaveInEmployeeChangeLogList(
                                //   oldAnnualLeave.CompanyLeave.Name,
                                //   oldAnnualLeave.SelectedAdditionalLeaves[i].AdditionalLeaveReasonName,
                                //   oldAnnualAdditionalLeave.AdditionalLeaveTotalHours,
                                //   oldAnnualAdditionalLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                //   annual_leave.SelectedAdditionalLeaves[i].AdditionalLeaveTotalHours,
                                //   annual_leave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                //   Application.Current.FindResource("EmployeeAuthorizedLeaveChange").ToString());
                                // shubham[skadam] GEOS2-3655 HRM - Add holiday type  12 Sep 2022
                                UpdateEmployeeAnnualLeaveInEmployeeChangeLogList_V2301(
                                  oldAnnualLeave.CompanyLeave.Name,
                                  oldAnnualLeave.SelectedAdditionalLeaves[i].AdditionalLeaveReasonName,
                                  oldAnnualAdditionalLeave.AdditionalLeaveTotalHours,
                                  oldAnnualAdditionalLeave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                  annual_leave.SelectedAdditionalLeaves[i].AdditionalLeaveTotalHours,
                                  annual_leave.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount,
                                  Application.Current.FindResource("EmployeeAuthorizedLeaveChange").ToString(),
                                 Application.Current.FindResource("AddAuthorizedLeaveViewCommentsLog").ToString() + " " + annual_leave.SelectedAdditionalLeaves[i].Comments
                                  );

                            }

                        }

                    }
                }
                //Employee Shift
                // [003] Added
                if (EmployeeExistDetail.EmployeeShifts != EmployeeDetail.EmployeeShifts)
                {

                    if (EmployeeUpdatedDetail.EmployeeShifts != null)
                    {
                        foreach (EmployeeShift empshift in EmployeeUpdatedDetail.EmployeeShifts)
                        {

                            if (empshift.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeShiftDeletelogmsg").ToString(), empshift.CompanyShift.CompanySchedule.Name + " - " + empshift.CompanyShift.Name) });
                            }
                            if (empshift.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = EmployeeDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeShiftAddlogmsg").ToString(), empshift.CompanyShift.CompanySchedule.Name + " - " + empshift.CompanyShift.Name) });
                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddChangedDetailsInLog()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedDetailsInLog()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateEmployeeAnnualLeaveInEmployeeChangeLogList(
            string leaveName, string leaveAdditionalName,
            decimal oldTotalHoursCount, decimal oldDailyHoursCount,
            decimal newTotalHoursCount, decimal newdailyHoursCount,
            string changeLogFormat)
        {
            if (oldTotalHoursCount != newTotalHoursCount)
            {
                Int32 oldAnnualLeaveDays = (Int32)(oldTotalHoursCount / oldDailyHoursCount);
                Int32 oldAnnualLeaveHours = (Int32)(oldTotalHoursCount % oldDailyHoursCount);

                string OldDaysAndHours = oldAnnualLeaveDays + "d" + " " + oldAnnualLeaveHours + "H";

                Int32 NewAnnualLeaveDays = (Int32)(newTotalHoursCount / newdailyHoursCount);
                Int32 NewAnnualLeaveHours = (Int32)(newTotalHoursCount % newdailyHoursCount);
                string NewDaysAndHours = NewAnnualLeaveDays + "d" + " " + NewAnnualLeaveHours + "H";

                using (var employeeChangelog = new EmployeeChangelog
                {
                    IdEmployee = EmployeeDetail.IdEmployee,
                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    ChangeLogChange = string.Format(
                        changeLogFormat, leaveName, leaveAdditionalName, OldDaysAndHours, NewDaysAndHours)
                })
                {
                    EmployeeChangeLogList.Add(employeeChangelog);
                }
            }
        }

        // shubham[skadam] GEOS2-3655 HRM - Add holiday type  12 Sep 2022
        private void UpdateEmployeeAnnualLeaveInEmployeeChangeLogList_V2301(
           string leaveName, string leaveAdditionalName,
           decimal oldTotalHoursCount, decimal oldDailyHoursCount,
           decimal newTotalHoursCount, decimal newdailyHoursCount,
           string changeLogFormat, string Comments)
        {
            if (oldTotalHoursCount != newTotalHoursCount)
            {
                Int32 oldAnnualLeaveDays = (Int32)(oldTotalHoursCount / oldDailyHoursCount);
                Int32 oldAnnualLeaveHours = (Int32)(oldTotalHoursCount % oldDailyHoursCount);

                string OldDaysAndHours = oldAnnualLeaveDays + "d" + " " + oldAnnualLeaveHours + "H";

                Int32 NewAnnualLeaveDays = (Int32)(newTotalHoursCount / newdailyHoursCount);
                Int32 NewAnnualLeaveHours = (Int32)(newTotalHoursCount % newdailyHoursCount);
                string NewDaysAndHours = NewAnnualLeaveDays + "d" + " " + NewAnnualLeaveHours + "H";

                using (var employeeChangelog = new EmployeeChangelog
                {
                    IdEmployee = EmployeeDetail.IdEmployee,
                    ChangeLogDatetime = GeosApplication.Instance.ServerDateTime,
                    ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    ChangeLogChange = string.Format(
                        changeLogFormat, leaveName, leaveAdditionalName, OldDaysAndHours, NewDaysAndHours + " " + Comments)
                })
                {
                    EmployeeChangeLogList.Add(employeeChangelog);
                }
            }
        }
        private void SearchButtonClickCommandAction(object obj)
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
                        Address = employeeSetLocationMapViewModel.LocationAddress;
                    }
                }
                else
                {
                    Latitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                    Longitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                    MapLatitudeAndLongitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude;
                    Address = employeeSetLocationMapViewModel.LocationAddress;
                }
            }
        }

        private void FillCityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCityList()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.CityList == null)
                {
                    GeosApplication.Instance.CityList = HrmService.GetAllCitiesByIdCountry(0).ToList();
                }

                GeosApplication.Instance.Logger.Log("Method FillCityList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillCityList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            IsInformationError = false;
            InformationError = string.Empty;
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
                    me[BindableBase.GetPropertyName(() => FirstName)] +
                    me[BindableBase.GetPropertyName(() => LastName)] +
                    me[BindableBase.GetPropertyName(() => NativeName)] +
                    me[BindableBase.GetPropertyName(() => Address)] +
                    me[BindableBase.GetPropertyName(() => City)] +
                    me[BindableBase.GetPropertyName(() => Region)] +
                    me[BindableBase.GetPropertyName(() => SelectedNationalityIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedCountryIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedMaritalStatusIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexGender)] +
                    me[BindableBase.GetPropertyName(() => BirthDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedJobDescriptionRow)] +
                    //me[BindableBase.GetPropertyName(() => SelectedCompanyScheduleIndex)] + 
                    //me[BindableBase.GetPropertyName(() => SelectedCompanyShiftIndex)] +
                    me[BindableBase.GetPropertyName(() => JobDescriptionError)] +
                    me[BindableBase.GetPropertyName(() => ZipCode)] +
                    me[BindableBase.GetPropertyName(() => SelectedCity)] +
                    me[BindableBase.GetPropertyName(() => SelectedLocationIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedCountryRegion)] +
                    me[BindableBase.GetPropertyName(() => EmployeeShiftError)];
                //+
                //     me[BindableBase.GetPropertyName(() => SelectedJobDescription)
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        /// [001][cpatil][17-03-2022][GEOS2-3565]HRM - Allow add future Job descriptions [#ERF97] - 1
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string empFirstName = BindableBase.GetPropertyName(() => FirstName);
                string empLastName = BindableBase.GetPropertyName(() => LastName);
                string empNativeName = BindableBase.GetPropertyName(() => NativeName);
                string empAddress = BindableBase.GetPropertyName(() => Address);
                string empCity = BindableBase.GetPropertyName(() => City);
                string empRegion = BindableBase.GetPropertyName(() => Region);

                string empSelectedNationality = BindableBase.GetPropertyName(() => SelectedNationalityIndex);
                string empSelectedCountry = BindableBase.GetPropertyName(() => SelectedCountryIndex);
                string empSelectedMaritalStatus = BindableBase.GetPropertyName(() => SelectedMaritalStatusIndex);
                string empselectedIndexGender = BindableBase.GetPropertyName(() => SelectedIndexGender);
                string empbirthDate = BindableBase.GetPropertyName(() => BirthDate);
                string empJobDescription = BindableBase.GetPropertyName(() => JobDescriptionError);
                string empContractSituation = BindableBase.GetPropertyName(() => ContractSituationError);
                //string empCompanySchedule = BindableBase.GetPropertyName(() => SelectedCompanyScheduleIndex);
                //string empCompanyShift = BindableBase.GetPropertyName(() => SelectedCompanyShiftIndex);
                string empZipCode = BindableBase.GetPropertyName(() => ZipCode);
                string empSelectedCountryRegion = BindableBase.GetPropertyName(() => SelectedCountryRegion);
                string empSelectedCity = BindableBase.GetPropertyName(() => SelectedCity);
                string empSelectedlocation = BindableBase.GetPropertyName(() => SelectedLocationIndex);

                string empShiftError = BindableBase.GetPropertyName(() => EmployeeShiftError);

                // string empselectedJobDescription = BindableBase.GetPropertyName(() => SelectedJobDescription);

                //if (columnName == empExitReason)
                //{
                //    if (!string.IsNullOrEmpty(exitReasonErrorMessage))
                //    {
                //        return CheckInformationError(exitReasonErrorMessage);
                //    }
                //}

                //if (columnName == empselectedJobDescription)
                //{
                //    return EmployeeProfileValidation.GetErrorMessage(empselectedJobDescription, SelectedJobDescription);
                //}

                if (columnName == empFirstName)
                {
                    if (!string.IsNullOrEmpty(firstNameErrorMsg))
                    {
                        return CheckInformationError(firstNameErrorMsg);
                    }
                    else
                    {

                        firstNameErrorMsg = EmployeeProfileValidation.GetErrorMessage(empFirstName, FirstName);
                        return CheckInformationError(firstNameErrorMsg);
                    }
                }
                if (columnName == empLastName)
                {
                    if (!string.IsNullOrEmpty(lastNameErrorMsg))
                    {
                        return CheckInformationError(lastNameErrorMsg);
                    }
                    else
                    {
                        lastNameErrorMsg = EmployeeProfileValidation.GetErrorMessage(empLastName, LastName);
                        return CheckInformationError(lastNameErrorMsg);
                    }
                }
                if (columnName == empNativeName)
                {
                    if (!string.IsNullOrEmpty(nativeNameErrorMsg))
                    {
                        return CheckInformationError(nativeNameErrorMsg);
                    }
                }
                if (columnName == empCity)
                {
                    if (!string.IsNullOrEmpty(cityErrorMsg))
                    {
                        return CheckInformationError(cityErrorMsg);
                    }
                }
                if (columnName == empRegion)
                {
                    if (!string.IsNullOrEmpty(regionErrorMsg))
                    {
                        return CheckInformationError(regionErrorMsg);
                    }
                }

                if (columnName == empSelectedNationality)
                {
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedNationality, SelectedNationalityIndex));
                }
                if (columnName == empSelectedCountry)
                {
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedCountry, SelectedCountryIndex));
                }

                if (columnName == empselectedIndexGender)
                {
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empselectedIndexGender, SelectedIndexGender));
                }

                if (columnName == empbirthDate)
                {
                    if (BirthDate >= DateTime.Now)
                    {
                        return CheckInformationError(birthDateErrorMessage);
                    }
                    else if (SelectedEmpolyeeStatusIndex != -1)
                    {
                        if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                        {
                            birthDateErrorMessage = EmployeeProfileValidation.GetErrorMessage(empbirthDate, BirthDate);
                            return CheckInformationError(birthDateErrorMessage);
                        }
                    }
                }
                if (SelectedEmpolyeeStatusIndex != -1)
                {
                    if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                    {
                        if (columnName == empAddress)
                        {
                            return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empAddress, Address));
                        }


                        if (columnName == empSelectedMaritalStatus)
                        {
                            return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedMaritalStatus, SelectedMaritalStatusIndex));
                        }
                    }
                }
                //if (columnName == empCompanyShift)
                //{
                //    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empCompanyShift, SelectedCompanyShiftIndex));
                //}
                //if (columnName == empCompanySchedule)
                //{
                //    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empCompanySchedule, SelectedCompanyScheduleIndex));
                //}

                if (columnName == empZipCode)
                {
                    if (!string.IsNullOrEmpty(ZipCodeErrorMsg))
                        return CheckInformationError(ZipCodeErrorMsg);
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)

                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empZipCode, ZipCode));
                }

                if (columnName == empSelectedCountryRegion)
                {
                    if (!string.IsNullOrEmpty(SelectedRegionErrorMsg))
                        return CheckInformationError(SelectedRegionErrorMsg);
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedCountryRegion, SelectedCountryRegion));
                }
                if (columnName == empSelectedCity)
                {
                    if (!string.IsNullOrEmpty(SelectedCityErrorMsg))
                        return CheckInformationError(SelectedCityErrorMsg);
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedCity, SelectedCity));
                }
                if (columnName == empSelectedlocation)
                {
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedlocation, SelectedLocationIndex));
                }

                #region ContractSituation
                if (columnName == empContractSituation)
                    return EmployeeProfileValidation.GetErrorMessage(empContractSituation, ContractSituationError);
                #endregion

                #region JobDescription

                if (columnName == empJobDescription)
                {
                    if (Convert.ToInt16(TotalJobDescriptionUsage) < 100 && Convert.ToInt16(TotalJobDescriptionUsage) > 0)
                    {
                        // [001]
                        if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                        {
                            Int32 MaxLimitOfPercentage = 100 - EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                            if (Convert.ToInt16(MaxLimitOfPercentage) < 100 && Convert.ToInt16(MaxLimitOfPercentage) > 0)
                            {
                                return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                            }
                        }
                        else
                            return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                    }
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                    {
                        //if (JobDescriptionStartDate != null && JobDescriptionStartDate > DateTime.Now)
                        //{
                        //   return (string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionStartDateError").ToString(), JobDescriptionName));
                        //}
                    }

                    else
                        return EmployeeProfileValidation.GetErrorMessage(empJobDescription, JobDescriptionError);

                    if (EmployeeJobDescriptionList.Count < 1)
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empJobDescription, JobDescriptionError);
                    }


                    if (Convert.ToInt16(TotalJobDescriptionUsage) < 1)
                    {
                        if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 136)
                        {
                            //[001]
                            if (EmployeeJobDescriptionList.Any(a => a.JobDescriptionStartDate > DateTime.Now.Date))
                            {
                                Int32 MaxLimitOfPercentage = 100 - EmployeeJobDescriptionList.Where(a => a.JobDescriptionStartDate > DateTime.Now.Date).Sum(x => x.JobDescriptionUsage);
                                if (Convert.ToInt16(MaxLimitOfPercentage) < 100 && Convert.ToInt16(MaxLimitOfPercentage) > 0)
                                {
                                    return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                                }
                            }
                            else
                                return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                        }


                    }
                }
                #endregion

                if (columnName == empShiftError)
                    return EmployeeProfileValidation.GetErrorMessage(empShiftError, EmployeeShiftError);

                return null;
            }
        }

        public string CheckInformationError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                IsInformationError = true;

                InformationError = EmployeeProfileValidation.GetErrorMessage("InformationError", "Error");
            }

            return error;


        }

        /// <summary>
        /// this method use for display Employee IdBadge
        /// [SP66][000][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// [001][cpatil][07-04-2022][GEOS2-3624][ID_BADGE] Change the “Location” Field for the “Organization” one.
        /// [002][cpatil][07-04-2022][GEOS2-3623][ID_BADGE] Remove the “Diamonds” section in Employee ID Badge card.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="EmployeeJobDescription"></param>
        public void ShowEmployeeIdBadge(Employee EmployeeObj, JobDescription EmployeeJobDescription)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ShowEmployeeIdBadge()...", category: Category.Info, priority: Priority.Low);

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


                EmployeeIdBadge employeeIdBadgeReport = new EmployeeIdBadge();

                Employee EmployeeIdBadgeObj = new Employee();
                EmployeeIdBadgeObj = EmployeeObj;

                employeeIdBadgeReport.xrLblEmployeeName.Text = string.Format("{0} {1}", EmployeeIdBadgeObj.FirstName, EmployeeIdBadgeObj.LastName);
                employeeIdBadgeReport.xrLblEmployeeJdTitle.Text = EmployeeJobDescription.JobDescriptionTitle;
                employeeIdBadgeReport.xrLblEmployeeLocations.Text = EmployeeJobDescription.EmployeeJobDescriptions[0].Company.Alias; //[001]
                employeeIdBadgeReport.xrLblEmployeeDepartment.Text = EmployeeJobDescription.Department.DepartmentName;
                employeeIdBadgeReport.xrPBEmdepLogo.Image = ResizeImage(new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo));
                employeeIdBadgeReport.xrBrEmployeeCode.Text = EmployeeIdBadgeObj.EmployeeCode;
                CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
                #region [rdixit][15.11.2024][GEOS2-6013]
                var minDate = EmployeeJobDescriptionList.Where(k => k.IdEmployeeExitEvent == null || k.IdEmployeeExitEvent == 0)
                    .Select(i => i.JobDescriptionStartDate?.Date).Where(date => date.HasValue).Min(date => date.Value);
                employeeIdBadgeReport.lblHireDate.Text = minDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                employeeIdBadgeReport.RFCTextlbl.Visible = false;
                employeeIdBadgeReport.RFClbl.Visible = false;
                employeeIdBadgeReport.IMSSTextlbl.Visible = false;
                employeeIdBadgeReport.IMSSlbl.Visible = false;
                IdCompany = EmployeeObj.EmployeeJobDescriptions.FirstOrDefault(i => i.JobDescription?.JobDescriptionTitle == EmployeeJobDescription?.JobDescriptionTitle)?.IdCompany ?? default;
                if (HrmService.IsCompanyInIdCardExtraInfoCountries(IdCompany))
                {
                    if (EmployeeDetail.EmployeeDocuments != null)
                    {
                        if (EmployeeDetail.EmployeeDocuments.Any(k => k.EmployeeDocumentIdType == 118
                         && DateTime.Now.Date <= (k.EmployeeDocumentExpiryDate == null ? DateTime.Now : k.EmployeeDocumentExpiryDate).Value.Date
                         && DateTime.Now.Date >= (k.EmployeeDocumentIssueDate == null ? DateTime.Now : k.EmployeeDocumentIssueDate).Value.Date))
                        {
                            employeeIdBadgeReport.IMSSTextlbl.Visible = true;
                            employeeIdBadgeReport.IMSSlbl.Visible = true;
                            employeeIdBadgeReport.IMSSlbl.Text = EmployeeDetail.EmployeeDocuments.FirstOrDefault(k => k.EmployeeDocumentIdType == 118).EmployeeDocumentNumber;
                        }
                        if (EmployeeDetail.EmployeeDocuments.Any(k => k.EmployeeDocumentIdType == 2170
                         && DateTime.Now.Date <= (k.EmployeeDocumentExpiryDate == null ? DateTime.Now : k.EmployeeDocumentExpiryDate).Value.Date
                         && DateTime.Now.Date >= (k.EmployeeDocumentIssueDate == null ? DateTime.Now : k.EmployeeDocumentIssueDate).Value.Date))
                        {
                            employeeIdBadgeReport.RFCTextlbl.Visible = true;
                            employeeIdBadgeReport.RFClbl.Visible = true;
                            employeeIdBadgeReport.RFClbl.Text = EmployeeDetail.EmployeeDocuments.FirstOrDefault(k => k.EmployeeDocumentIdType == 2170).EmployeeDocumentNumber;
                        }
                    }
                }
                #endregion

                //[rdixit][GEOS2-2626][29.07.2022]
                employeeIdBadgeReport.xrLblEmpNationalID.Text = "";
                EmployeeID_Companies_GeosAppSetting = CrmStartUp.GetGeosAppSettings(92).DefaultValue;
                EmployeeID_CompaniestGeosAppSettingList = EmployeeID_Companies_GeosAppSetting.Split(';').ToList();
                EmployeeContractSituation contractSituation = new EmployeeContractSituation();
                foreach (EmployeeContractSituation EmployeeContract in EmployeeIdBadgeObj.EmployeeContractSituations)
                {
                    if (EmployeeContract.ContractSituationEndDate != null)
                    {
                        if (EmployeeContract.ContractSituationEndDate.Value.Date >= DateTime.Now.Date && EmployeeContract.ContractSituationStartDate.Value.Date <= DateTime.Now.Date)
                        {
                            contractSituation = EmployeeContract;
                            break;
                        }
                    }
                    else if (EmployeeContract.ContractSituationEndDate == null && EmployeeContract.ContractSituationStartDate.Value.Date <= DateTime.Now.Date)
                    {
                        contractSituation = EmployeeContract;
                        break;
                    }
                    else if (EmployeeContract.ContractSituationEndDate == null)    //chitra.girigosavi[24/09/2024]GEOS2-6101 Prind IdCard for Employee in Draft Status
                    {
                        contractSituation = EmployeeContract;
                        break;
                    }
                }
                if (EmployeeID_CompaniestGeosAppSettingList.Any(i => i == contractSituation.Company.IdCompany.ToString()) == true)
                {
                    if (EmployeeIdBadgeObj.EmployeeDocuments.Where(i => i.EmployeeDocumentType.Value == "NationalID").FirstOrDefault() != null)
                    {
                        EmployeeDocument EmployeeDocument = EmployeeIdBadgeObj.EmployeeDocuments.Where(i => i.EmployeeDocumentType.Value == "NationalID").FirstOrDefault();

                        if (EmployeeDocument.EmployeeDocumentExpiryDate == null)
                        {
                            employeeIdBadgeReport.xrLblEmpNationalID.Text = EmployeeDocument.EmployeeDocumentNumber;
                        }
                        else
                        {
                            if (EmployeeDocument.EmployeeDocumentExpiryDate.Value.Date < DateTime.Now.Date)
                            {
                                employeeIdBadgeReport.xrLblEmpNationalID.Text = "";
                            }
                            else
                            {
                                employeeIdBadgeReport.xrLblEmpNationalID.Text = EmployeeDocument.EmployeeDocumentNumber;
                            }
                        }
                    }
                }
                employeeIdBadgeReport.xrBrEmployeeCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrBrEmployeeCode_BeforePrint);

                if (EmployeeJobDescription.JDLevel != null)
                {

                    float xCoordinate;
                    if (EmployeeJobDescription.JDLevel.IdImage == 1)
                        xCoordinate = 38F;

                    else if (EmployeeJobDescription.JDLevel.IdImage == 2)
                        xCoordinate = 28F;

                    else if (EmployeeJobDescription.JDLevel.IdImage == 3)
                        xCoordinate = 20F;

                    else if (EmployeeJobDescription.JDLevel.IdImage == 4)
                        xCoordinate = 11F;
                    else
                        xCoordinate = 4F;
                    //[002] commented code Diamond Removed from report
                    //for (int index = 0; EmployeeJobDescription.JDLevel.IdImage > index; index++)
                    //{
                    //if (EmployeeJobDescription.JDLevel.IdLookupValue != 212 && EmployeeJobDescription.JDLevel.IdLookupValue != 213 && EmployeeJobDescription.JDLevel.IdLookupValue != 214)
                    //{
                    //    XRShape JdlevelDiamondControl = new XRShape();
                    //    ShapePolygon shapePolygon = new ShapePolygon();
                    //    JdlevelDiamondControl.BorderColor = System.Drawing.SystemColors.HotTrack;
                    //    JdlevelDiamondControl.FillColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl.BackColor = System.Drawing.Color.Transparent;
                    //    JdlevelDiamondControl.BorderColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl.ForeColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl.BorderWidth = 0F;
                    //    shapePolygon.NumberOfSides = 4;
                    //    JdlevelDiamondControl.Shape = shapePolygon;
                    //    JdlevelDiamondControl.SizeF = new System.Drawing.SizeF(10.96609F, 20.16075F);
                    //    JdlevelDiamondControl.StylePriority.UseBorderColor = false;
                    //    JdlevelDiamondControl.LocationFloat = new DevExpress.Utils.PointFloat(xCoordinate, 7F);
                    //    xCoordinate += 17;
                    //    //employeeIdBadgeReport.xrPanelJdLevel.Controls.Add(JdlevelDiamondControl);

                    //}
                    //else
                    //{
                    //    XRShape JdlevelDiamondControl1 = new XRShape();
                    //    JdlevelDiamondControl1.BorderColor = System.Drawing.SystemColors.HotTrack;
                    //    JdlevelDiamondControl1.LocationFloat = new DevExpress.Utils.PointFloat(xCoordinate, 10F);
                    //    JdlevelDiamondControl1.SizeF = new System.Drawing.SizeF(15.03369F, 15.03369F);
                    //    JdlevelDiamondControl1.BorderWidth = 0F;
                    //    JdlevelDiamondControl1.ForeColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl1.BorderColor = ColorTranslator.FromHtml("#b8860b");
                    //    XRShape JdlevelDiamondControl2 = new XRShape();
                    //    ShapePolygon shapePolygon1 = new ShapePolygon();
                    //    JdlevelDiamondControl2.BorderColor = System.Drawing.SystemColors.HotTrack;
                    //    JdlevelDiamondControl2.FillColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl2.BackColor = System.Drawing.Color.Transparent;
                    //    JdlevelDiamondControl2.BorderColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl2.ForeColor = ColorTranslator.FromHtml("#b8860b");
                    //    JdlevelDiamondControl2.BorderWidth = 0F;
                    //    shapePolygon1.NumberOfSides = 4;
                    //    JdlevelDiamondControl2.Shape = shapePolygon1;
                    //    JdlevelDiamondControl2.SizeF = new System.Drawing.SizeF(10.96609F, 20.16075F);
                    //    JdlevelDiamondControl2.StylePriority.UseBorderColor = false;
                    //    JdlevelDiamondControl2.LocationFloat = new DevExpress.Utils.PointFloat(xCoordinate + 2, 7.5f);
                    //    JdlevelDiamondControl2.BorderWidth = 0F;
                    //    xCoordinate += 17;
                    //    employeeIdBadgeReport.xrPanelJdLevel.Controls.Add(JdlevelDiamondControl1);
                    //    employeeIdBadgeReport.xrPanelJdLevel.Controls.Add(JdlevelDiamondControl2);
                    //}
                    //}
                }
                if (EmployeeIdBadgeObj.EmployeePolyvalences.Count > 0)
                {

                    EmployeeIdBadgeObj.EmployeePolyvalences = EmployeeDetail.EmployeePolyvalences.OrderByDescending(x => x.PolyvalenceUsage).ToList();

                    for (int i = 0; EmployeeIdBadgeObj.EmployeePolyvalences.Count > i; i++)
                    {
                        if (i == 0)
                        {
                            System.Drawing.Color backgroundcolor = ColorTranslator.FromHtml(SetColorEmployeeIdBadgeReportCard(EmployeeIdBadgeObj.EmployeePolyvalences[i].PolyvalenceUsage));
                            employeeIdBadgeReport.xrPolyvalences1.FillColor = backgroundcolor;
                            employeeIdBadgeReport.xrlblPolyvalences1.Text = EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation;
                            if (EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation.Length > 4)
                            {
                                employeeIdBadgeReport.xrlblPolyvalences1.Font = new Font(employeeIdBadgeReport.xrlblPolyvalences1.Font.FontFamily,
                                    employeeIdBadgeReport.xrlblPolyvalences1.Font.Size - 2, System.Drawing.FontStyle.Bold);
                            }
                        }
                        if (i == 1)
                        {
                            System.Drawing.Color backgroundcolor = ColorTranslator.FromHtml(SetColorEmployeeIdBadgeReportCard(EmployeeIdBadgeObj.EmployeePolyvalences[i].PolyvalenceUsage));
                            employeeIdBadgeReport.xrPolyvalences2.FillColor = backgroundcolor;
                            employeeIdBadgeReport.xrlblPolyvalences2.Text = EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation;
                            if (EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation.Length > 4)
                            {
                                employeeIdBadgeReport.xrlblPolyvalences2.Font = new Font(employeeIdBadgeReport.xrlblPolyvalences2.Font.FontFamily,
                                    employeeIdBadgeReport.xrlblPolyvalences2.Font.Size - 2, System.Drawing.FontStyle.Bold);
                            }
                        }
                        if (i == 2)
                        {
                            System.Drawing.Color backgroundcolor = ColorTranslator.FromHtml(SetColorEmployeeIdBadgeReportCard(EmployeeIdBadgeObj.EmployeePolyvalences[i].PolyvalenceUsage));
                            employeeIdBadgeReport.xrPolyvalences3.FillColor = backgroundcolor;
                            employeeIdBadgeReport.xrlblPolyvalences3.Text = EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation;
                            if (EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation.Length > 4)
                            {
                                employeeIdBadgeReport.xrlblPolyvalences3.Font = new Font(employeeIdBadgeReport.xrlblPolyvalences3.Font.FontFamily,
                                    employeeIdBadgeReport.xrlblPolyvalences3.Font.Size - 2, System.Drawing.FontStyle.Bold);
                            }
                        }
                        if (i == 3)
                        {
                            System.Drawing.Color backgroundcolor = ColorTranslator.FromHtml(SetColorEmployeeIdBadgeReportCard(EmployeeIdBadgeObj.EmployeePolyvalences[i].PolyvalenceUsage));
                            employeeIdBadgeReport.xrPolyvalences4.FillColor = backgroundcolor;
                            employeeIdBadgeReport.xrlblPolyvalences4.Text = EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation;
                            if (EmployeeIdBadgeObj.EmployeePolyvalences[i].JobDescription.Abbreviation.Length > 4)
                            {
                                employeeIdBadgeReport.xrlblPolyvalences4.Font = new Font(employeeIdBadgeReport.xrlblPolyvalences4.Font.FontFamily,
                                    employeeIdBadgeReport.xrlblPolyvalences4.Font.Size - 2, System.Drawing.FontStyle.Bold);
                            }
                        }
                    }
                }

                //EmployeeIdBadgeObj.ProfileImageInBytes = GeosRepositoryServiceController.GetEmployeesImage(EmployeeIdBadgeObj.EmployeeCode);
                //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
                //EmployeeIdBadgeObj.ProfileImageInBytes = GetEmployeesImage(EmployeeIdBadgeObj.EmployeeCode);
                //Shubham[skadam] GEOS2-5548 HRM - Employee photo 29 05 2024
                EmployeeIdBadgeObj.ProfileImageInBytes = GetEmployeesImage_V2520(EmployeeIdBadgeObj.EmployeeCode);
                if (EmployeeIdBadgeObj.ProfileImageInBytes != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(EmployeeIdBadgeObj.ProfileImageInBytes))
                    {
                        BitmapImage image = new BitmapImage();

                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = new Bitmap(image.StreamSource);
                        employeeIdBadgeReport.xrPBEmployeeImage.Image = img;
                    }
                }
                else
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/No_Photo.png", UriKind.RelativeOrAbsolute))));
                        enc.Save(outStream);
                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                        Bitmap image = new Bitmap(bitmap);
                        employeeIdBadgeReport.xrPBEmployeeImage.Image = image;
                    }
                }
                //employeeIdBadgeReport.xrPBEmployeeImage.WidthF = employeeIdBadgeReport.xrPBEmployeeImage.Image.Width;
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = employeeIdBadgeReport;
                employeeIdBadgeReport.CreateDocument();
                window.Show();
                GeosApplication.Instance.Logger.Log("Method ShowEmployeeIdBadge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method ShowEmployeeIdBadge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// set color base on Polyvalence usage
        /// [SP66][000][skale][05-07-2019-][HRM]Print Employee Badge option.
        /// </summary>
        /// <param name="PolyvalenceUsage"></param>
        /// <returns></returns>
        public string SetColorEmployeeIdBadgeReportCard(int PolyvalenceUsage)
        {
            string color = string.Empty;
            if (PolyvalenceUsage < 50)
                return "#FFFFFF";
            else if (PolyvalenceUsage >= 50 && PolyvalenceUsage < 74)
                return "#ffffcd";
            else if (PolyvalenceUsage > 74 && PolyvalenceUsage <= 90)
                return "#ffff79";
            else if (PolyvalenceUsage > 90)
                return "#fffb07";
            else
                return "#FFFFFF";
        }
        /// <summary>
        /// [SP66][000][skale][05-07-2019-][GEOS2-1474]GHRM - Print ID Badge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrBrEmployeeCode_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // Obtain the current label. 
            XRBarCode label = (XRBarCode)sender;
            label.AutoModule = false;

            BarCodeError berror = label.Validate();
            if (berror == BarCodeError.ControlBoundsTooSmall)
            {
                label.AutoModule = true;
            }
        }

        /// <summary>
        /// [000][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// [001][skale][09-10-2019][GEOS2-1804] I need same additional validations related with Exit Events [#ERF42]
        /// [002][smazhar][30-07-2020][GEOS2-2204] Employee profile has the status “Inactive” must show the last exit event.
        /// [003][cpatil][14-09-2021][GEOS2-3360] Exit event exit date should not be the same for 4 exit events
        /// [004][cpatil][20-04-2022][GEOS2-3713] HRM - Attached document to Exit Event
        /// [005][cpatil][07-10-2022][GEOS2-3497] HRM - Remove Exit Event [#ERF95]
        /// </summary>
        public void FillExitEventList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExitEventList()...", category: Category.Info, priority: Priority.Low);

                ExitEventList = new ObservableCollection<EmployeeExitEvent>();

                string ExitEventHeader = string.Empty;

                for (int i = 1; i < 5; i++)
                {
                    int tempSelectedExitEventReasonIndex = 0;
                    DateTime? tempExitDate = null;
                    DateTime? tempMinExitEventDate = null;
                    int tempIdEmployee = EmployeeDetail.IdEmployee;
                    string tempfileName = null;
                    ObservableCollection<Data.Common.Attachment> tempExitEventattachmentList = new ObservableCollection<Data.Common.Attachment>();
                    string tempRemark = string.Empty;
                    Data.Common.Attachment tempAttachedFile = null;
                    Visibility tempisVisible = Visibility.Collapsed;
                    bool tempIsExitDateEnabled = true;
                    int idExitEvent = i;
                    bool tempExist = false;
                    bool tempIsReadonly = false;
                    bool tempIsEnabled = false;
                    byte[] tempExitEventBytes = null;

                    int tempIdreason = 0;
                    long idEmployeeExitEvent = 0;

                    tempMinExitEventDate = MinExitDate;


                    ExitEventHeader = String.Format(SetTabName(i) + " {0}", Application.Current.FindResource("ExitEvent").ToString());

                    if (EmployeeDetail.EmployeeExitEvents != null && EmployeeDetail.EmployeeExitEvents.Count > 0)
                    {

                        if (EmployeeDetail.EmployeeExitEvents.Any(x => x.IdExitEvent == i))  // if Employee have Exit event 
                        {
                            EmployeeExitEvent employeeExitEventObj = EmployeeDetail.EmployeeExitEvents.Where(x => x.IdExitEvent == i).FirstOrDefault();
                            tempSelectedExitEventReasonIndex = ExitReasonList.FindIndex(erl => erl.IdLookupValue == employeeExitEventObj.IdReason);
                            tempIdEmployee = employeeExitEventObj.IdEmployee;
                            tempRemark = employeeExitEventObj.Remarks;
                            tempExist = true;
                            tempExitEventBytes = employeeExitEventObj.ExitEventBytes;
                            tempIdreason = employeeExitEventObj.IdReason;
                            idEmployeeExitEvent = employeeExitEventObj.IdEmployeeExitEvent;

                            if (!string.IsNullOrEmpty(employeeExitEventObj.FileName)) // for file
                            {
                                Data.Common.Attachment attachment = new Data.Common.Attachment();
                                attachment.FilePath = null;
                                attachment.OriginalFileName = employeeExitEventObj.FileName;
                                tempfileName = employeeExitEventObj.FileName;
                                attachment.IsDeleted = false;
                                tempExitEventattachmentList.Add(attachment);

                                if (tempExitEventattachmentList != null)
                                {
                                    tempAttachedFile = tempExitEventattachmentList[0];
                                    tempisVisible = Visibility.Visible;
                                }
                                else
                                    tempAttachedFile = null;
                            }
                            else
                            {

                                tempisVisible = Visibility.Collapsed;
                            }



                            if (employeeExitEventObj.ExitDate != null)
                            {
                                tempExitDate = employeeExitEventObj.ExitDate;

                                if (!HrmCommon.Instance.IsPermissionReadOnly) //validation for permission (user have permission so he can modify event ) // true
                                {
                                    //if Emplyee status  is  inactive and draft so he can modify event (exit event)
                                    if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138 || EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 137)
                                    {

                                        // employee have only one exit event  so he can modify 1st event 
                                        //[001] added
                                        if (EmployeeDetail.EmployeeExitEvents.Count == 1)
                                        {
                                            tempIsExitDateEnabled = false;
                                            tempIsReadonly = false;
                                            tempIsEnabled = true;

                                        }
                                        else // if employee have more than one event so he can modify only last one (ex he have 3 event.so 1st,2nd can not modify)
                                        {
                                            if (i != 4)
                                            {
                                                // modify last one 
                                                if (EmployeeDetail.EmployeeExitEvents.Any(x => x.IdExitEvent == i + 1 && x.IsExist == true))
                                                {
                                                    tempIsExitDateEnabled = true;
                                                    if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                                        tempIsReadonly = false;
                                                    else
                                                        tempIsReadonly = true;
                                                    tempIsEnabled = true;
                                                }
                                                else
                                                {
                                                    tempIsExitDateEnabled = false;
                                                    tempIsReadonly = false;
                                                    tempIsEnabled = true;
                                                }

                                            }
                                            else // employee have 4 exits event so that case he can only modify 4th event 
                                            {

                                                tempIsExitDateEnabled = false;
                                                tempIsReadonly = false;
                                                tempIsEnabled = true;
                                            }
                                        }

                                    }// if Employee status is active so user can not modify event
                                    else
                                    {
                                        tempIsExitDateEnabled = true;
                                        if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                            tempIsReadonly = false;
                                        else
                                            tempIsReadonly = true;
                                        tempIsEnabled = true;
                                    }
                                } //if user dont have permission so he can not modify
                                else
                                {
                                    tempIsExitDateEnabled = true;
                                    tempIsReadonly = true;
                                    tempIsEnabled = true;
                                }
                            }
                            else
                            {

                                tempisVisible = Visibility.Collapsed;
                            }


                        } // if Employee do not have any Exit event 
                        else
                        {
                            tempExist = false;

                            if (!HrmCommon.Instance.IsPermissionReadOnly)  // check permission and set Field disable
                            {
                                if (ExitEventList.Any(x => x.IsExist == false))
                                {
                                    tempIsExitDateEnabled = true;
                                    if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                        tempIsReadonly = false;
                                    else
                                        tempIsReadonly = true;
                                    tempIsEnabled = true;
                                }
                                else
                                {
                                    tempIsExitDateEnabled = false;
                                    if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                        tempIsReadonly = false;
                                    else
                                        tempIsReadonly = true;
                                    tempIsEnabled = true;
                                }

                                if (ExitEventList.Count > 0)
                                {
                                    EmployeeExitEvent employeeExitEventObj = ExitEventList.Where(x => x.IdExitEvent == i - 1).FirstOrDefault();
                                    //[003]
                                    if (employeeExitEventObj != null)
                                    {
                                        if (employeeExitEventObj.ExitDate != null)
                                            tempMinExitEventDate = employeeExitEventObj.ExitDate.Value.AddDays(1);
                                        else
                                        {
                                            if (ExitEventList.Any(x => x.ExitDate != null))
                                                tempMinExitEventDate = ExitEventList.Where(x => x.ExitDate != null).OrderBy(x => x.IdExitEvent).ToList().LastOrDefault().ExitDate.Value.AddDays(1);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                tempIsExitDateEnabled = true;
                                tempIsReadonly = true;
                                tempIsEnabled = true;

                            }

                        }
                    }
                    else
                    {
                        if (!HrmCommon.Instance.IsPermissionReadOnly) // check permission and set Field disable
                        {
                            if (idExitEvent == 1)
                            {
                                tempIsExitDateEnabled = false;
                                if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                    tempIsReadonly = false;
                                else
                                    tempIsReadonly = true;
                                tempIsEnabled = true;
                            }
                            else
                            {
                                tempIsExitDateEnabled = true;
                                if (tempExitDate != null && tempExitDate.Value.Date >= DateTime.Now.Date)//[004]
                                    tempIsReadonly = false;
                                else
                                    tempIsReadonly = true;
                                tempIsEnabled = false;
                            }

                        }
                        else
                        {
                            tempIsExitDateEnabled = true;
                            tempIsReadonly = true;
                            tempIsEnabled = false;
                        }
                    }
                    // Rahul.Gadhave[GEOS2-3593][26/12/2023]
                    if (ExitEventList.Where(j => j.ExitDate == null).ToList().Count < 1)
                    {
                        // fill list
                        ExitEventList.Add(new EmployeeExitEvent()
                        {
                            Header = ExitEventHeader,
                            ExitEventReasonList = ExitReasonList,
                            SelectedExitEventReasonIndex = tempSelectedExitEventReasonIndex,
                            MinExitEventDate = tempMinExitEventDate,
                            ExitDate = tempExitDate,
                            IdEmployee = tempIdEmployee,
                            FileName = tempfileName,
                            ExitEventattachmentList = tempExitEventattachmentList,
                            Remarks = tempRemark,
                            AttachedFile = tempAttachedFile,
                            IsVisible = tempisVisible,
                            IdExitEvent = idExitEvent,
                            IsExist = tempExist,
                            IsExitDateEnable = tempIsExitDateEnabled,
                            IsReadOnly = tempIsReadonly,
                            IsEnable = tempIsEnabled,
                            IdReason = tempIdreason,
                            IdEmployeeExitEvent = idEmployeeExitEvent,
                            ExitEventBytes = tempExitEventBytes
                        });
                    }
                }
                // set current exit event
                //[001]
                if (EmployeeDetail.EmployeeExitEvents.Count == 0)
                {
                    EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 1).FirstOrDefault();
                    SelectedExitEventItem = employeeExitEventSelectedItems;
                }

                else if (EmployeeDetail.EmployeeExitEvents.Count == 4)
                {
                    EmployeeExitEvent employeeExitEventSelectedItems = ExitEventList.Where(x => x.IdExitEvent == 4).FirstOrDefault();
                    SelectedExitEventItem = employeeExitEventSelectedItems;
                }
                else
                {
                    EmployeeExitEvent employeeExitEventSelectedItem = ExitEventList.Where(x => x.IsExist == true).LastOrDefault();
                    SelectedExitEventItem = employeeExitEventSelectedItem;

                }
                //[005] Added
                if (ExitEventList.Any(i => i.IdEmployeeExitEvent != 0))
                {
                    EmployeeExitEvent employeeExitEvent = ExitEventList.Where(i => i.IdEmployeeExitEvent != 0).LastOrDefault();
                    if (!(EmployeeJobDescriptionList.Any(ejdl => ejdl.JobDescriptionStartDate.Value.Date >= employeeExitEvent.ExitDate.Value.Date) || EmployeeContractSituationList.Any(ejdl => ejdl.ContractSituationStartDate.Value.Date >= employeeExitEvent.ExitDate.Value.Date)))
                    {
                        ExitEventList.Where(i => i.IdEmployeeExitEvent != 0).LastOrDefault().IsDeleteExitEventEnabled = true;
                    }
                }


                GeosApplication.Instance.Logger.Log("Method FillExitEventList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in Method FillExitEventList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        /// <summary>
        ///  [000][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// </summary>
        /// <param name="idExitEvent"></param>
        /// <returns></returns>
        public string SetTabName(int idExitEvent)
        {

            if (idExitEvent == 1)
            {
                return "1st";
            }

            else if (idExitEvent == 2)
            {
                return "2nd";
            }
            else if (idExitEvent == 3)
            {
                return "3rd";
            }
            else if (idExitEvent == 4)
            {
                return "4th";
            }
            return null;
        }


        #endregion
        //----------mazhar-----------//
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
            //[GEOS2-6499][rdixit][07.11.2024]
            if (GeosApplication.Instance.IsHRMManageEmployeeContactsPermission)
            {
                IsReadOnlyField = true;
                IsAcceptEnabled = true;
            }
        }
        //[nsatpute][26-03-2025][GEOS2-7011]
        private void ExportEmployeeProfileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEmployeeProfileCommandAction()...", category: Category.Info, priority: Priority.Low);

                string ResultFileName;
                SaveFileDialogService saveFileDialogService = new SaveFileDialogService();
                saveFileDialogService.DefaultExt = "txt";
                saveFileDialogService.DefaultFileName = EmployeeDetail.EmployeeCode;
                saveFileDialogService.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialogService.FilterIndex = 1;
                bool DialogResult = saveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                    return;
                }
                else
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            System.Windows.Window win = new System.Windows.Window()
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
                    ResultFileName = saveFileDialogService.GetFullFileName();
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (GenerateEmployeeProfileFile(ResultFileName))
                {
                    EmployeeChangelog changelog = new EmployeeChangelog();
                    changelog.IdEmployee = EmployeeDetail.IdEmployee;
                    changelog.ChangeLogDatetime = GeosApplication.Instance.ServerDateTime;
                    changelog.ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    changelog.ChangeLogUser = GeosApplication.Instance.ActiveUser;
                    changelog.ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("Employeeprofiledetailview_Employeeexported").ToString());
                    HrmService.AddEmployeeChangelogs_V2500(changelog);
                    EmployeeAllChangeLogList.Insert(0, changelog);
                }

                GeosApplication.Instance.Logger.Log("Method ExportEmployeeProfileCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportEmployeeProfileCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        //[nsatpute][26-03-2025][GEOS2-7011]
        public bool GenerateEmployeeProfileFile(string resultFileName)
        {
            try
            {
                string numero = EmployeeDetail.EmployeeCode;
                string nombres = EmployeeDetail.FirstName;
                string apellidoPaterno = EmployeeDetail.LastName.Split(' ')[0];
                string apellidoMaterno = EmployeeDetail.LastName.Split(' ').Length > 1 ? EmployeeDetail.LastName.Split(' ')[1] : string.Empty;

                string tipo = "SUELDOS Y SALARIOS";
                string clavePuesto = string.Empty;
                string claveDepto = string.Empty;
                string claveFrecuenciaPago = "SEMANAL";
                string numRegPatronal = dictPetronalNumbers[Convert.ToInt32(EmployeeDetail.IdCompanyLocation)];
                string claveTurno = string.Empty;
                string formaPago = "T";
                string contrato = "P";
                string turno = string.Empty;
                string jornada = string.Empty;
                string fechaIngreso = string.Empty;
                string estatus = string.Empty;
                string tipoSalario = "0";
                string salarioDiario = "100";
                string zonaSalario = "A";
                string nombreCalle = EmployeeDetail.AddressStreet ?? string.Empty;
                string poblacion = EmployeeDetail.AddressCity ?? string.Empty;
                string codigoPostal = EmployeeDetail.AddressZipCode ?? string.Empty;
                string telefono1 = string.Empty;
                string email = string.Empty;
                string sexo = string.Empty;
                string estadoCivil = string.Empty;
                string fechaNacimiento = EmployeeDetail.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty;
                string rfc = string.Empty;
                string curp = string.Empty;
                string registroImss = string.Empty;

                if (EmployeeDetail.EmployeeDocuments != null && EmployeeDetail.EmployeeDocuments.Any(i => i.EmployeeDocumentType.IdLookupValue == 2231))
                    rfc = EmployeeDetail.EmployeeDocuments.FirstOrDefault(i => i.EmployeeDocumentType.IdLookupValue == 2231).EmployeeDocumentNumber;

                if (EmployeeDetail.EmployeeDocuments != null && EmployeeDetail.EmployeeDocuments.Any(i => i.EmployeeDocumentType.IdLookupValue == 2232))
                    curp = EmployeeDetail.EmployeeDocuments.FirstOrDefault(i => i.EmployeeDocumentType.IdLookupValue == 2232).EmployeeDocumentNumber;

                if (EmployeeDetail.EmployeeDocuments != null && EmployeeDetail.EmployeeDocuments.Any(i => i.EmployeeDocumentType.IdLookupValue == 2233))
                    registroImss = EmployeeDetail.EmployeeDocuments.FirstOrDefault(i => i.EmployeeDocumentType.IdLookupValue == 2233).EmployeeDocumentNumber;


                JobDescription jd = EmployeeJobDescriptionList.Where(a => (a.JobDescriptionEndDate == null || a.JobDescriptionEndDate.Value.Date >= DateTime.Now.Date) && a.JobDescriptionStartDate.Value.Date <= DateTime.Now.Date).ToList()?[0].JobDescription;

                if (jd != null)
                {
                    clavePuesto = jd.JobDescriptionTitle;
                    claveDepto = jd.Department.DepartmentName;
                }
                if (EmployeeDetail.EmployeeShifts.Count() > 1)
                {
                    turno = claveTurno = "X";
                }
                else if (EmployeeDetail.EmployeeShifts.Any(x => x.CompanyShift.IdType == 1711))
                {
                    turno = claveTurno = "M";
                }
                else if (EmployeeDetail.EmployeeShifts.Any(x => x.CompanyShift.IdType == 1712))
                {
                    turno = claveTurno = "V";
                }
                else if (EmployeeDetail.EmployeeShifts.Any(x => x.CompanyShift.IdType == 1713))
                {
                    turno = claveTurno = "N";
                }

                if (EmployeeDetail.IdGender == 1)
                    sexo = "F";
                else
                    sexo = "M";

                switch (EmployeeDetail.IdMaritalStatus)
                {
                    case 53:
                        estadoCivil = "S"; break;
                    case 54:
                        estadoCivil = "C"; break;
                    case 55:
                        estadoCivil = "D"; break;
                    case 56:
                        estadoCivil = "V"; break;
                    default:
                        estadoCivil = "U"; break;
                }
                DateTime? contractStartdate = null;

                if (EmployeeContractSituationList.Any(x => x.ContractSituationEndDate == null))
                    contractStartdate = EmployeeContractSituationList.OrderBy(x => x.ContractSituationStartDate).FirstOrDefault().ContractSituationStartDate;
                else
                    contractStartdate = EmployeeContractSituationList.OrderBy(x => x.ContractSituationEndDate).LastOrDefault().ContractSituationStartDate;

                fechaIngreso = contractStartdate?.ToString("yyyy-MM-dd") ?? string.Empty;

                jornada = EmployeeDetail.DailyWorkingHours.ToString();

                if (EmployeeDetail.EmployeeStatus.IdLookupValue == 136)
                    estatus = "A";
                else if (EmployeeDetail.EmployeeStatus.IdLookupValue == 137)
                    estatus = "I";
                else
                    estatus = "B";


                if (EmployeeDetail.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 88).ToList().Count > 0)
                {
                    email = EmployeeDetail.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88).EmployeeContactValue;
                }
                if (EmployeeDetail.EmployeePersonalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 89).ToList().Count > 0)
                {
                    telefono1 = EmployeeDetail.EmployeePersonalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 89).EmployeeContactValue;
                }


                string header = "NUMERO;NOMBRES;APELLIDO_PATERNO;APELLIDO_MATERNO;TIPO;CLAVE_PUESTO;CLAVE_DEPTO;CLAVE_FRECUENCIA_PAGO;NUM_REG_PATRONAL;CLAVE_TURNO;FORMA_PAGO;CONTRATO;TURNO;JORNADA;FECHA_INGRESO;ESTATUS;TIPO_SALARIO;SALARIO_DIARIO;ZONA_SALARIO;NOMBRE_CALLE;POBLACION;CODIGO_POSTAL;TELEFONO1;EMAIL;SEXO;ESTADO_CIVIL;FECHA_NACIMIENTO;RFC;CURP;REGISTRO_IMSS";
                string data = $"{numero};{nombres};{apellidoPaterno};{apellidoMaterno};{tipo};{clavePuesto};{claveDepto};{claveFrecuenciaPago};{numRegPatronal};{claveTurno};{formaPago};{contrato};{turno};{jornada};{fechaIngreso};{estatus};{tipoSalario};{salarioDiario};{zonaSalario};{nombreCalle};{poblacion};{codigoPostal};{telefono1};{email};{sexo};{estadoCivil};{fechaNacimiento};{rfc};{curp};{registroImss}";

                using (StreamWriter writer = new StreamWriter(resultFileName))
                {
                    writer.WriteLine(header);
                    writer.WriteLine(data);
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(string.Format(string.Format(System.Windows.Application.Current.FindResource("Employeeprofiledetailview_Employeeexportedsuccessfully").ToString())), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //System.Diagnostics.Process.Start(resultFileName);
                return true;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to export employee profile - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return false;
            }
        }
    }
}
