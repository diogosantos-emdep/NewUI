using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using NodaTime;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewEmployeeViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region TaskLog

        /// <summary>
        /// [HRM-M049-35] Display length of service months in employee profile [adadibathina][23102018]
        /// [CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>

        #endregion

        #region Service       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public INavigationService _Service;

        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController("localhost:6699");

        #endregion

        #region Declaration
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private int selectedNationalityIndex;
        private int selectedCountryIndex;
        IWorkbenchStartUp objWorkbenchStartUp;
        List<GeosProvider> geosProviderList = new List<GeosProvider>();
        private List<CountryRegion> countryRegionList = new List<CountryRegion>();
        private List<string> countryWiseRegions;
        private int selectedEmpolyeeStatusIndex;
        private ObservableCollection<EmployeeDocument> employeeDocumentList;
        private EmployeeDocument selectedDocumentRow;
        private ObservableCollection<EmployeeContractSituation> employeeContractSituationList;
        private EmployeeContractSituation selectedContractSituationRow;
        private ObservableCollection<EmployeeEducationQualification> employeeEducationQualificationList;
        private EmployeeEducationQualification selectedEducationQualificationRow;
        private ObservableCollection<EmployeeFamilyMember> employeeFamilyMemberList;
        private EmployeeFamilyMember selectedFamilyMemberRow;
        private ObservableCollection<EmployeeProfessionalEducation> employeeProfessionalEducationList;
        private EmployeeProfessionalEducation selectedProfessionalEducationRow;
        private string employeeCode;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionList;
        private ObservableCollection<EmployeeJobDescription> employeeTopFourJobDescriptionList;
        private ObservableCollection<EmployeeContact> employeeContactList;
        private ObservableCollection<EmployeeContact> employeeProfessionalContactList;
        private ObservableCollection<EmployeeLanguage> employeeLanguageList;
        private EmployeeLanguage selectedLanguageRow;
        private bool isSaved;
        private string jobDescriptionError;
        private string firstNameErrorMsg = string.Empty;
        private string lastNameErrorMsg = string.Empty;
        private string DisplayNameErrorMsg = string.Empty;
        private string nativeNameErrorMsg = string.Empty;
        private string cityErrorMsg = string.Empty;
        private string regionErrorMsg = string.Empty;
        private string informationError;
        private string selectedCountryRegion;
        private string error = string.Empty;
        private Employee employeeDetail;
        private int selectedDisability;
        private int selectedTabIndex;
        private int selectedMaritalStatusIndex;
        private int selectedCountryRegionIndex;
        private string firstName;
        private string lastName;
        private string nativeName;
        private string city;
        private string zipCode;
        private string region;
        private DateTime? birthDate;
        private string remark;
        private string address;
        private int selectedIndexGender = -1;
        private LookupValue selectedGender;
        private EmployeeJobDescription selectedJobDescriptionRow;
        private string birthDateErrorMessage = string.Empty;
        private bool isBusy;
        private byte[] profilePhotoBytes = null;
        private ImageSource profilePhotoSource = null;
        private EmployeeContact selectedContactRow;
        private EmployeeContact selectedProfessionalContactRow;
        private Regex regex;
        EmployeeContact ProfContact;
        EmployeeContact PersonalContact;
        private string professionalEmail;
        private string professionalPhone;
        private int contactCount;
        private string lengthOfService;
        private string age;
        private string contractSituationError;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private List<City> cityList;
        private List<string> countryWiseCities;
        private string selectedCity;
        // Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        //private List<CompanySchedule> companySchedules; 
        //private int selectedCompanyScheduleIndex = -1;
        //private List<CompanyShift> companyShifts;
        //private int selectedCompanyShiftIndex;

        private int totalJobDescriptionUsage;
        private string ZipCodeErrorMsg = string.Empty;
        private string SelectedRegionErrorMsg = string.Empty;
        private string SelectedCityErrorMsg = string.Empty;
        private ObservableCollection<EmployeeJobDescription> employeeJobDescriptionsList;
        private bool isPhoto;
        private string JobDescriptionName = string.Empty;
        private bool isInformationError = false;
        private int selectedLocationIndex;
        // [001] added
        private ObservableCollection<EmployeePolyvalence> employeePolyvalenceList;
        private EmployeePolyvalence selectedPolyvalenceRow;
        private bool IsJobDescriptionOrganizationChanged = true;
        private List<int> SelectedOrganizationIdList;
        private double dialogWidth;
        private double dialogHeight;

        //[002] Added
        private ObservableCollection<EmployeeShift> employeeShiftList;
        private string employeeShiftError;
        private ObservableCollection<EmployeeShift> topFourEmployeeShiftList;
        private bool isShiftAsteriskVisible;
        private ObservableCollection<EmployeeAnnualLeave> employeeAnnualLeaveList;
        private EmployeeAnnualLeave selectedEmployeeAnnualLeave;
        private ObservableCollection<EmployeeChangelog> employeeChangeLogList;
        private ObservableCollection<EmployeeAnnualLeave> tempEmployeeAnnualLeaveList;
        private string displayName;
        private bool isRemote;
        private int Remote;

        private bool dynamicColumnsAdded;
        private ObservableCollection<GridColumn> additionalBandGridColumns;
        private ObservableCollection<JobDescription> jobDescriptionList;//[Sudhir.Jangra][GEOS2-4846]
        public int selectedJobDescription;//[Sudhir.Jangra][GEOS2-4846]
        private List<LookupValue> extraHoursTimeList;//[Sudhir.Jangra][GEOS2-5273]
        private LookupValue selectedExtraHoursTime;//[Sudhir.Jangra][GEOS2-5273]
        private ObservableCollection<Employee> existProfessionalEmailEmployeeList;//[Sudhir.Jangra][GEOS2-3418]
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

        #region Properties
        public bool IsReadOnlyField//[GEOS2-5112][05.02.2024][rdixit]
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }
        public bool IsAcceptEnabled//[GEOS2-5112][05.02.2024][rdixit]
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
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

        //[001] added
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
        //End
        public EmployeeContractSituation CurrentContractSituation { get; set; }
        public DateTime? JobDescriptionStartDate { get; set; }
        public bool IsFromFirstName { get; set; }
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
        public bool IsFromLastName { get; set; }
        public bool IsFromNativeName { get; set; }
        public bool IsFromCity { get; set; }
        public bool IsFromRegion { get; set; }
        public bool IsFromDisplayName { get; set; }

        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionsList
        {
            get { return employeeJobDescriptionsList; }
            set
            {
                employeeJobDescriptionsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionsList"));
            }
        }
        public string LengthOfService
        {
            get
            {
                return lengthOfService;
            }

            set
            {
                lengthOfService = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LengthOfService"));
            }
        }
        public string ProfessionalEmail
        {
            get
            {
                return professionalEmail;
            }

            set
            {
                professionalEmail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalEmail"));
            }
        }

        public string ProfessionalPhone
        {
            get
            {
                return professionalPhone;
            }

            set
            {
                professionalPhone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalPhone"));
            }
        }
        public int ContactCount
        {
            get
            {
                return contactCount;
            }

            set
            {
                contactCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactCount"));
            }
        }
        public int SelectedNationalityIndex
        {
            get
            {
                return selectedNationalityIndex;
            }

            set
            {
                selectedNationalityIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNationalityIndex"));
            }
        }

        public int SelectedCountryIndex
        {
            get
            {
                return selectedCountryIndex;
            }

            set
            {
                selectedCountryIndex = value;
                if (selectedCountryIndex != -1)
                {
                    CountryRegionList = AllCountryRegionList.FindAll(x => x.IdCountry == CountryList[selectedCountryIndex].IdCountry);
                    CountryWiseRegions = new List<string>();
                    CountryWiseRegions = CountryRegionList.Select(x => x.Name).ToList();

                    CityList = GeosApplication.Instance.CityList.FindAll(x => x.IdCountry == CountryList[selectedCountryIndex].IdCountry).ToList();
                    CountryWiseCities = new List<string>();
                    CountryWiseCities = CityList.Select(x => x.Name).ToList();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryIndex"));
            }
        }
        public List<CountryRegion> CountryRegionList
        {
            get
            {
                return countryRegionList;
            }

            set
            {
                countryRegionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryRegionList"));
            }
        }
        public List<string> CountryWiseRegions
        {
            get
            {
                return countryWiseRegions;
            }

            set
            {
                countryWiseRegions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryWiseRegions"));
            }
        }

        public int SelectedEmpolyeeStatusIndex
        {
            get
            {
                return selectedEmpolyeeStatusIndex;
            }

            set
            {
                selectedEmpolyeeStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmpolyeeStatusIndex"));
            }
        }

        public ObservableCollection<EmployeeDocument> EmployeeDocumentList
        {
            get
            {
                return employeeDocumentList;
            }

            set
            {
                employeeDocumentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDocumentList"));
            }
        }
        public EmployeeDocument SelectedDocumentRow
        {
            get
            {
                return selectedDocumentRow;
            }

            set
            {
                selectedDocumentRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDocumentRow"));
            }
        }
        public ObservableCollection<EmployeeContractSituation> EmployeeContractSituationList
        {
            get
            {
                return employeeContractSituationList;
            }

            set
            {
                employeeContractSituationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContractSituationList"));
            }
        }
        public EmployeeContractSituation SelectedContractSituationRow
        {
            get
            {
                return selectedContractSituationRow;
            }

            set
            {
                selectedContractSituationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContractSituationRow"));
            }
        }
        public ObservableCollection<EmployeeEducationQualification> EmployeeEducationQualificationList
        {
            get
            {
                return employeeEducationQualificationList;
            }

            set
            {
                employeeEducationQualificationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEducationQualificationList"));
            }
        }
        public EmployeeEducationQualification SelectedEducationQualificationRow
        {
            get
            {
                return selectedEducationQualificationRow;
            }

            set
            {
                selectedEducationQualificationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEducationQualificationRow"));
            }
        }
        public ObservableCollection<EmployeeFamilyMember> EmployeeFamilyMemberList
        {
            get
            {
                return employeeFamilyMemberList;
            }

            set
            {
                employeeFamilyMemberList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeFamilyMemberList"));
            }
        }

        public EmployeeFamilyMember SelectedFamilyMemberRow
        {
            get
            {
                return selectedFamilyMemberRow;
            }

            set
            {
                selectedFamilyMemberRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamilyMemberRow"));
            }
        }
        public ObservableCollection<EmployeeProfessionalEducation> EmployeeProfessionalEducationList
        {
            get
            {
                return employeeProfessionalEducationList;
            }

            set
            {
                employeeProfessionalEducationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfessionalEducationList"));
            }
        }
        public EmployeeProfessionalEducation SelectedProfessionalEducationRow
        {
            get
            {
                return selectedProfessionalEducationRow;
            }

            set
            {
                selectedProfessionalEducationRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfessionalEducationRow"));
            }
        }
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }

            set
            {
                employeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNationalityIndex"));
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

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
            get
            {
                return lastName;
            }

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
            get
            {
                return nativeName;
            }

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
            get
            {
                return city;
            }

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
            get
            {
                return zipCode;
            }

            set
            {
                zipCode = value;
                IsFromLastName = false;
                IsFromFirstName = false;
                IsFromNativeName = false;
                IsFromCity = true;
                IsFromRegion = false;
                OnPropertyChanged(new PropertyChangedEventArgs("ZipCode"));
            }
        }

        public string Region
        {
            get
            {
                return region;
            }

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
            get
            {
                return birthDate;
            }

            set
            {
                birthDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BirthDate"));
            }
        }

        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }

        public Employee EmployeeDetail
        {
            get
            {
                return employeeDetail;
            }

            set
            {
                employeeDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDetail"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList
        {
            get
            {
                return employeeJobDescriptionList;
            }

            set
            {
                employeeJobDescriptionList = value;
                if (employeeJobDescriptionList != null)
                {
                    // EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList().Take(4).ToList());
                    if (employeeJobDescriptionList.Count > 0)
                    {
                        var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now).ToList();
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
                }
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeeJobDescription> EmployeeTopFourJobDescriptionList
        {
            get
            {
                return employeeTopFourJobDescriptionList;
            }

            set
            {
                employeeTopFourJobDescriptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTopFourJobDescriptionList"));
            }
        }

        public ObservableCollection<EmployeeContact> EmployeeContactList
        {
            get
            {
                return employeeContactList;
            }

            set
            {
                employeeContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContactList"));
            }
        }
        public EmployeeContact SelectedContactRow
        {
            get
            {
                return selectedContactRow;
            }

            set
            {
                selectedContactRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContactRow"));
            }
        }
        public ObservableCollection<EmployeeContact> EmployeeProfessionalContactList
        {
            get
            {
                return employeeProfessionalContactList;
            }

            set
            {
                employeeProfessionalContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfessionalContactList"));
            }
        }

        public ObservableCollection<EmployeeLanguage> EmployeeLanguageList
        {
            get
            {
                return employeeLanguageList;
            }

            set
            {
                employeeLanguageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeLanguageList"));
            }
        }
        public EmployeeContact SelectedProfessionalContactRow
        {
            get
            {
                return selectedProfessionalContactRow;
            }

            set
            {
                selectedProfessionalContactRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfessionalContactRow"));
            }
        }
        public EmployeeLanguage SelectedLanguageRow
        {
            get
            {
                return selectedLanguageRow;
            }

            set
            {
                selectedLanguageRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLanguageRow"));
            }
        }
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }

            set
            {
                isSaved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaved"));
            }
        }
        public string JobDescriptionError
        {
            get
            {
                return jobDescriptionError;
            }

            set
            {
                jobDescriptionError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionError"));
            }
        }

        public string ContractSituationError
        {
            get
            {
                return contractSituationError;
            }

            set
            {
                contractSituationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationError"));
            }
        }
        public int SelectedMaritalStatusIndex
        {
            get
            {
                return selectedMaritalStatusIndex;
            }

            set
            {
                selectedMaritalStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMaritalStatusIndex"));
            }
        }
        public string SelectedCountryRegion
        {
            get
            {
                return selectedCountryRegion;
            }

            set
            {
                selectedCountryRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryRegion"));
            }
        }
        public int SelectedIndexGender
        {
            get
            {
                return selectedIndexGender;
            }

            set
            {
                selectedIndexGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGender"));
            }
        }
        public LookupValue SelectedGender
        {
            get
            {
                return selectedGender;
            }

            set
            {
                selectedGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGender"));
            }
        }
        public int SelectedDisability
        {
            get
            {
                return selectedDisability;
            }

            set
            {
                selectedDisability = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDisability"));
            }
        }
        public EmployeeJobDescription SelectedJobDescriptionRow
        {
            get
            {
                return selectedJobDescriptionRow;
            }

            set
            {
                selectedJobDescriptionRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescriptionRow"));
            }
        }

        public int SelectedTabIndex
        {
            get
            {
                return selectedTabIndex;
            }

            set
            {
                selectedTabIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTabIndex"));
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
        public byte[] ProfilePhotoBytes
        {
            get
            {
                return profilePhotoBytes;
            }

            set
            {
                profilePhotoBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfilePhotoBytes"));
            }
        }
        public ImageSource ProfilePhotoSource
        {
            get
            {
                return profilePhotoSource;
            }
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
            get
            {
                return isPhoto;
            }

            set
            {
                isPhoto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPhoto"));
            }
        }

        public GeoPoint MapLatitudeAndLongitude
        {
            get
            {
                return mapLatitudeAndLongitude;
            }

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
            get
            {
                return isCoordinatesNull;
            }

            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
        }

        public double? Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Latitude"));
            }
        }

        public double? Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Longitude"));
            }
        }

        public List<City> CityList
        {
            get
            {
                return cityList;
            }

            set
            {
                cityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CityList"));
            }
        }

        public string SelectedCity
        {
            get
            {
                return selectedCity;
            }

            set
            {
                selectedCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCity"));
            }
        }

        public List<string> CountryWiseCities
        {
            get
            {
                return countryWiseCities;
            }

            set
            {
                countryWiseCities = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryWiseCities"));
            }
        }
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

        public GeosProvider CurrentGeosProvider { get; set; }
        public List<GeosProvider> GeosProviderList { get; set; }

        public IList<LookupValue> GenderList { get; set; }
        public IList<LookupValue> MaritalStatusList { get; set; }
        public IList<Country> NatinalityList { get; set; }
        public List<Country> CountryList { get; set; }
        public List<CountryRegion> AllCountryRegionList { get; set; }
        public IList<LookupValue> EmployeeStatusList { get; set; }

        public List<Company> LocationList { get; set; }
        public string Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
        }

        #region  Old Code

        //Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)

        //public List<CompanySchedule> CompanySchedules
        //{
        //    get
        //    {
        //        return companySchedules;
        //    }

        //    set
        //    {
        //        companySchedules = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanySchedules"));
        //    }
        //}

        //public int SelectedCompanyScheduleIndex
        //{
        //    get
        //    {
        //        return selectedCompanyScheduleIndex;
        //    }

        //    set
        //    {
        //        selectedCompanyScheduleIndex = value;
        //        if (SelectedCompanyScheduleIndex > -1)
        //        {
        //           // CompanyShifts = CompanySchedules[SelectedCompanyScheduleIndex].CompanyShifts;
        //            SelectedCompanyShiftIndex = 0;
        //        }
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyScheduleIndex"));
        //    }
        //}
        //public List<CompanyShift> CompanyShifts
        //{
        //    get
        //    {
        //        return companyShifts;
        //    }

        //    set
        //    {
        //        companyShifts = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompanyShifts"));
        //    }
        //}
        //public int SelectedCompanyShiftIndex
        //{
        //    get
        //    {
        //        return selectedCompanyShiftIndex;
        //    }

        //    set
        //    {
        //        selectedCompanyShiftIndex = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyShiftIndex"));
        //    }
        //}

        #endregion

        public int TotalJobDescriptionUsage
        {
            get
            {
                return totalJobDescriptionUsage;
            }

            set
            {
                totalJobDescriptionUsage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalJobDescriptionUsage"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
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

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
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

        //[002] Added
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
        public ObservableCollection<EmployeeAnnualLeave> EmployeeAnnualLeaveList
        {
            get { return employeeAnnualLeaveList; }
            set
            {
                employeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeAnnualLeaveList"));
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
        public ObservableCollection<EmployeeChangelog> EmployeeChangeLogList
        {
            get { return employeeChangeLogList; }
            set
            {
                employeeChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeChangeLogList"));
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

        public decimal DailyHoursCount { get; set; }
        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;

                OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
            }
        }

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
        #endregion

        #region Public ICommands
        public ICommand AddEmployeeViewCancelButtonCommand { get; set; }
        public ICommand AddNewIdentificationDocumentCommand { get; set; }
        public ICommand AddNewContractSituationInformationCommand { get; set; }
        public ICommand AddNewEducationQualificationInformationCommand { get; set; }
        public ICommand AddNewFamilyMemberInformationCommand { get; set; }
        public ICommand AddNewProfessionalEducationCommand { get; set; }
        public ICommand AddNewLanguageInformationCommand { get; set; }
        public ICommand AddNewProfessionalContactInformationCommand { get; set; }
        public ICommand AddNewJobDescriptionCommand { get; set; }
        public ICommand DeleteLanguageInformationCommand { get; set; }
        public ICommand DeleteContractSituationInformationCommand { get; set; }
        public ICommand EmployeeDocumentViewCommand { get; set; }
        public ICommand AddEmployeeViewAcceptButtonCommand { get; set; }
        public ICommand DeleteDocumentInformationCommand { get; set; }
        public ICommand AddNewContactInformationCommand { get; set; }
        public ICommand DeleteContactInformationCommand { get; set; }
        public ICommand DeleteProfessionalContactCommand { get; set; }
        public ICommand DeleteJobDescriptionCommand { get; set; }
        public ICommand EditJobDescriptionDoubleClickCommand { get; set; }
        public ICommand EditFamilyMemberInformationDoubleClickCommand { get; set; }
        public ICommand EditEducationInformationDoubleClickCommand { get; set; }
        public ICommand EditContractSituationInformationDoubleClickCommand { get; set; }
        public ICommand EditLanguageInformationDoubleClickCommand { get; set; }
        public ICommand EditProfessionalContactInformationDoubleClickCommand { get; set; }
        public ICommand EditPersonalContactInformationDoubleClickCommand { get; set; }
        public ICommand EditDocumentInformationDoubleClickCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand DeleteProfessionalEducationCommand { get; set; }
        public ICommand EditProfessionalEducationDoubleClickCommand { get; set; }
        public ICommand DeleteEducationInformationCommand { get; set; }
        public ICommand DeleteFamilyMemberInformationCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand HyperlinkForEmailInGroupBox { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }

        public ICommand EmployeeStatusSelectedIndexChangedCommand { get; set; }
        public ICommand CommonValueChanged { get; set; }
        //[001] added
        public ICommand AddNewPolyvalenceCommand { get; set; }
        public ICommand EditPolyvalenceDoubleClickCommand { get; set; }
        public ICommand DeletePolyvalenceCommand { get; set; }
        //end

        //[002]added
        public ICommand AddNewEmployeeShiftCommand { get; set; }
        public ICommand DeleteEmployeeShiftCommand { get; set; }
        public ICommand AddAuthorizedLeaveCommand { get; set; }
        public ICommand DeleteAuthorizedLeave { get; set; }
        public ICommand CustomRowFilterCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand EditAuthorizedLeaveDoubleClickCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region  Constructor
        /// <summary>
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>
        public AddNewEmployeeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeViewModel()...", category: Category.Info, priority: Priority.Low);
                objWorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosProviderList = objWorkbenchStartUp.GetGeosProviderList();
                CurrentGeosProvider = GeosProviderList.Where(x => x.Company.Alias == GeosApplication.Instance.SiteName).FirstOrDefault();

                if (ProfilePhotoSource == null)
                {
                    ProfilePhotoSource = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
                    IsPhoto = false;
                }

                regex = new Regex(@"[~`!@#$%^&*_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");
                EmployeeCode = HrmService.GetLatestEmployeeCode();
                EmployeeContactList = new ObservableCollection<EmployeeContact>();
                EmployeeProfessionalContactList = new ObservableCollection<EmployeeContact>();
                EmployeeDocumentList = new ObservableCollection<EmployeeDocument>();
                EmployeeLanguageList = new ObservableCollection<EmployeeLanguage>();
                EmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>();
                EmployeeContractSituationList = new ObservableCollection<EmployeeContractSituation>();
                EmployeeFamilyMemberList = new ObservableCollection<EmployeeFamilyMember>();
                EmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>();
                EmployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>();
                //[001] added
                EmployeePolyvalenceList = new ObservableCollection<EmployeePolyvalence>();
                //end
                CommonValueChanged = new RelayCommand(new Action<object>(CommonValidation));
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                AddEmployeeViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddNewIdentificationDocumentCommand = new DelegateCommand<object>(AddNewIdentificationDocument);
                AddNewContractSituationInformationCommand = new DelegateCommand<object>(AddNewEmployeeContractSituation);
                AddNewEducationQualificationInformationCommand = new DelegateCommand<object>(AddNewEmployeeEducationQualification);
                AddNewFamilyMemberInformationCommand = new DelegateCommand<object>(AddNewEmployeeFamilyMember);
                AddNewProfessionalEducationCommand = new DelegateCommand<object>(AddNewProfessionalEducation);
                AddNewLanguageInformationCommand = new DelegateCommand<object>(AddNewLanguage);
                AddNewContactInformationCommand = new DelegateCommand<object>(AddNewContactInformation);
                AddNewJobDescriptionCommand = new DelegateCommand<object>(AddNewEmployeeJobDescription);
                EmployeeDocumentViewCommand = new RelayCommand(new Action<object>(OpenEmployeeEducationDocument));
                AddEmployeeViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveNewEmployeeProfile));
                AddNewProfessionalContactInformationCommand = new DelegateCommand<object>(AddNewProfessionalContactInformation);
                DeleteLanguageInformationCommand = new RelayCommand(new Action<object>(DeleteLanguageInformationRecord));
                DeleteContractSituationInformationCommand = new RelayCommand(new Action<object>(DeleteContractSituationInformationRecord));
                DeleteDocumentInformationCommand = new RelayCommand(new Action<object>(DeleteDocumentInformationRecord));
                DeleteContactInformationCommand = new RelayCommand(new Action<object>(DeleteContactInformationRecord));
                DeleteProfessionalContactCommand = new RelayCommand(new Action<object>(DeleteProfessionalContactInformationRecord));
                DeleteJobDescriptionCommand = new RelayCommand(new Action<object>(DeleteJobDescriptionRecord));
                EditJobDescriptionDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeJobDescription));
                EditFamilyMemberInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeFamilyMember));
                EditEducationInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeEducationQualification));
                EditContractSituationInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeContractSituation));
                EditLanguageInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditLanguageInformation));
                EditProfessionalContactInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditProfessionalContactInformation));
                EditPersonalContactInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditPersonalContactInformation));
                EditDocumentInformationDoubleClickCommand = new RelayCommand(new Action<object>(EditDocumentInformation));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                DeleteProfessionalEducationCommand = new RelayCommand(new Action<object>(DeleteProfessionalEducationRecord));
                EditProfessionalEducationDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeeProfessionalEducation));
                DeleteEducationInformationCommand = new RelayCommand(new Action<object>(DeleteEducationQualificationInformationRecord));
                DeleteFamilyMemberInformationCommand = new RelayCommand(new Action<object>(DeleteFamilyMemberInformationRecord));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                HyperlinkForEmailInGroupBox = new DelegateCommand<object>(new Action<object>((obj) => { SendMail(obj); }));
                SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
                EmployeeStatusSelectedIndexChangedCommand = new DelegateCommand<object>(new Action<object>((obj) => { EmployeeStatusSelectedIndexChanged(obj); }));
                //[001] added
                AddNewPolyvalenceCommand = new DelegateCommand<object>(AddNewEmployeePolyvalence);
                EditPolyvalenceDoubleClickCommand = new RelayCommand(new Action<object>(EditEmployeePolyvalence));
                DeletePolyvalenceCommand = new RelayCommand(new Action<object>(DeletePolyvalenceRecord));
                //end

                //[002] added 
                AddNewEmployeeShiftCommand = new DelegateCommand<object>(AddEmployeeShift);
                DeleteEmployeeShiftCommand = new RelayCommand(new Action<object>(DeleteEmployeeShift));
                AddAuthorizedLeaveCommand = new DelegateCommand<object>(AddAuthorizedLeave);
                DeleteAuthorizedLeave = new RelayCommand(new Action<object>(DeleteAuthorizedLeaveRecord));
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                EditAuthorizedLeaveDoubleClickCommand = new RelayCommand(new Action<object>(EditAuthorizedLeave));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //end
                SetUserPermission();
                DisplayEmployeeProfEmailAndPhone();
                CalculateLengthOfService();
                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewEmployeeViewModel()" + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewEmployeeViewModel() - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewEmployeeViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// [001][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                //System.Windows.Forms.Screen screen = GeosApplication.Instance.GetWorkingScreenFrom();
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;

                List<object> Days = GeosApplication.Instance.GetWeekNames();
                FillGenderList();
                FillMaritalStatusList();
                FillNationalityList();
                FillCountryRegionList();
                FillCityList();
                FillCountryList();
                FillLocationList();

                if (SelectedLocationIndex == -1)
                    SelectedLocationIndex = 0;
                IsRemote = false;
                FillEmployeeStatusList();
                FillEmployeeStatusList();
                JobDescriptionList = new ObservableCollection<JobDescription>(); //[Sudhir.Jangra][GEOS2-4846]
                JobDescriptionList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---", JobDescriptionInUse = 1 });//[Sudhir.Jangra][GEOS2-4846]
                FillExtraHoursTime();//[Sudhir.Jangra][GEOS2-5273]
                //FillCompanySchedules(string.Empty);//Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                SelectedOrganizationIdList = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().Select(x => x.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();

                //[001]added
                EmployeeShiftList = new ObservableCollection<EmployeeShift>();
                EmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>();
                EmployeeChangeLogList = new ObservableCollection<EmployeeChangelog>();
                TempEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>();

                IndexSun = Days.FindIndex(x => x.ToString() == "Sunday") + 3;
                IndexMon = Days.FindIndex(x => x.ToString() == "Monday") + 3;
                IndexTue = Days.FindIndex(x => x.ToString() == "Tuesday") + 3;
                IndexWed = Days.FindIndex(x => x.ToString() == "Wednesday") + 3;
                IndexThu = Days.FindIndex(x => x.ToString() == "Thursday") + 3;
                IndexFri = Days.FindIndex(x => x.ToString() == "Friday") + 3;
                IndexSat = Days.FindIndex(x => x.ToString() == "Saturday") + 3;
                //End
                AddAdditionalBandGridColumns();
                FillEmployeeListForProfessionalContactEmail();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        /// <summary>
        /// When ever the changes in view values the command will execute
        /// </summary>
        private void CommonValidation(object obj)
        {
            if (allowValidation)
                EnableValidationAndGetError();
        }

        /// <summary>
        /// Method for Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// Method to Fill Gender List
        /// </summary>
        private void FillGenderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGenderList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(1);
                GenderList = new List<LookupValue>();
                GenderList.AddRange(temptypeList);

                GeosApplication.Instance.Logger.Log("Method FillGenderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGenderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGenderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGenderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Fill Marital Status List
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
        /// Method to Fill Nationality List
        /// </summary>
        private void FillNationalityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillNationalityList()...", category: Category.Info, priority: Priority.Low);

                IList<Country> temptypeList = HrmService.GetAllCountries();
                NatinalityList = new List<Country>();
                NatinalityList.Insert(0, new Country() { Name = "---" });
                NatinalityList.AddRange(temptypeList);
                SelectedNationalityIndex = NatinalityList.IndexOf(cl => cl.IdCountry == CurrentGeosProvider.Company.IdCountry);

                GeosApplication.Instance.Logger.Log("Method FillNationalityList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillNationalityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Fill Country List
        ///  </summary>
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

                SelectedCountryIndex = CountryList.FindIndex(cl => cl.IdCountry == CurrentGeosProvider.Company.IdCountry);

                GeosApplication.Instance.Logger.Log("Method FillCountryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Fill Country Region List
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryRegionList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Fill Employee Status List
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
        /// this method use for fill location
        /// </summary>
        private void FillLocationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationList()...", category: Category.Info, priority: Priority.Low);

                IList<Company> tempList = HrmCommon.Instance.IsLocationList;
                LocationList = new List<Company>();
                LocationList.Insert(0, new Company() { Alias = "---" });
                if (tempList != null)
                {
                    LocationList.AddRange(tempList.Where(i => i.IsStillActive == 1).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method FillLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for Add New Identification Document
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewIdentificationDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewIdentificationDocument()...", category: Category.Info, priority: Priority.Low);

                AddIdentificationDocumentView addIdentificationDocumentView = new AddIdentificationDocumentView();
                AddIdentificationDocumentViewModel addIdentificationDocumentViewModel = new AddIdentificationDocumentViewModel();
                EventHandler handle = delegate { addIdentificationDocumentView.Close(); };
                addIdentificationDocumentViewModel.RequestClose += handle;
                addIdentificationDocumentView.DataContext = addIdentificationDocumentViewModel;

                EmployeeContractSituation empActiveContract = (EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationEndDate == null || (x.ContractSituationEndDate.HasValue && x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)));

                addIdentificationDocumentViewModel.Init(EmployeeDocumentList, 0, empActiveContract);
                addIdentificationDocumentViewModel.IsNew = true;
                addIdentificationDocumentView.ShowDialog();

                if (addIdentificationDocumentViewModel.IsSave == true)
                {
                    EmployeeDocumentList.Add(addIdentificationDocumentViewModel.NewDocument);
                    SelectedDocumentRow = addIdentificationDocumentViewModel.NewDocument;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewIdentificationDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewIdentificationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add New Employee Contract Situation        /// </summary>
        /// <param name="obj"></param>
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
                    EmployeeContractSituationList.Add(addContractSituationViewModel.NewEmployeeContractSituation);
                    SelectedContractSituationRow = addContractSituationViewModel.NewEmployeeContractSituation;
                    employeeContractSituation.IdCompany = addContractSituationViewModel.NewEmployeeContractSituation.IdCompany;
                    employeeContractSituation.Company = addContractSituationViewModel.NewEmployeeContractSituation.Company;
                    CalculateLengthOfService();
                    CurrentContractSituation = EmployeeContractSituationList.LastOrDefault();
                }


                if (EmployeeContractSituationList.Count < 1)
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
        /// Method for Add New Employee Education Qualification
        /// </summary>
        /// <param name="obj"></param>
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
                addEducationView.ShowDialog();
                if (addEducationViewModel.IsSave == true)
                {
                    EmployeeEducationQualificationList.Add(addEducationViewModel.NewEducationQualification);
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
        /// Method for Add New Employee Family Member
        /// </summary>
        /// <param name="obj"></param>
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
                addFamilyMembersView.ShowDialog();
                if (addFamilyMembersViewModel.IsSave == true)
                {
                    EmployeeFamilyMemberList.Add(addFamilyMembersViewModel.NewFamilyMember);
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
        /// Method for Add New Professional Education
        /// </summary>
        /// <param name="obj"></param>
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
                addProfessionalEducationView.ShowDialog();
                if (addProfessionalEducationViewModel.IsSave == true)
                {
                    EmployeeProfessionalEducationList.Add(addProfessionalEducationViewModel.NewProfessionalEducation);
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
        /// Method for Add New Language
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewLanguage(object obj)
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
                addLanguagesView.ShowDialog();

                if (addLanguagesViewModel.IsSave == true)
                {
                    EmployeeLanguageList.Add(addLanguagesViewModel.NewLanguage);
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
        /// Method to Delete Language Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteLanguageInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteLanguageInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLanguageInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeLanguage empEmployeeLanguage = (EmployeeLanguage)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeLanguageList.Remove(empEmployeeLanguage);

                }
                GeosApplication.Instance.Logger.Log("Method DeleteLanguageInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteLanguageInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Delete Contract Situation Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteContractSituationInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteContractSituationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeContractSituation empContractSituation = (EmployeeContractSituation)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeContractSituationList.Remove(empContractSituation);
                }
                CalculateLengthOfService();
                CurrentContractSituation = EmployeeContractSituationList.LastOrDefault();

                if (EmployeeContractSituationList.Count < 1)
                {
                    ContractSituationError = "";
                }
                else
                {
                    ContractSituationError = null;
                }

                GeosApplication.Instance.Logger.Log("Method DeleteContractSituationInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteContractSituationInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Open Employee Education Document
        /// </summary>
        /// <param name="obj"></param>
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

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Save New Employee Profile
        /// [001][skale][2019-17-04][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// [002][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [003][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// [004][smazhar][11-08-2020][GEOS2-2498] Employee display name [#ERF67]
        /// [005][cpatil][01-10-2020][GEOS2-2495] Validate the Start Date of the employee Job Description [#ERF66]
        /// [006][13-09-2021][cpatil][GEOS2-3358] Status should be draft to active if current date is equal to hire date
        /// [007][06-04-2022][cpatil][GEOS2-3562] Allow negative value in Backlog Leave - 1
        /// </summary>
        /// <param name="obj"></param>
        private void SaveNewEmployeeProfile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveNewEmployeeProfile()...", category: Category.Info, priority: Priority.Low);
                InformationError = null;
                allowValidation = true;
                if (IsSaved == false)
                {
                    if (EmployeeJobDescriptionList.Count < 1)
                    {

                        JobDescriptionError = "";
                    }
                    else
                    {
                        JobDescriptionError = null;
                    }
                    if (EmployeeContractSituationList.Count < 1)
                    {
                        ContractSituationError = "";
                    }
                    else
                    {
                        ContractSituationError = null;
                    }


                    //[003] added
                    if (EmployeeShiftList != null && EmployeeShiftList.Count < 1)
                        EmployeeShiftError = "";
                    else
                        EmployeeShiftError = null;


                    EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now));
                    if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                    {
                        TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                    }
                    else
                        TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

                    if (EmployeeJobDescriptionList.Count > 0 && IsJobDescriptionOrganizationChanged)
                    {
                        List<int> JobDescriptionCompanyIdList = EmployeeJobDescriptionList.Select(x => x.Company.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();

                        if (!JobDescriptionCompanyIdList.SequenceEqual(SelectedOrganizationIdList))
                        {
                            string _idCompany = null;
                            if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Count > 0)
                            {
                                _idCompany = string.Join(",", EmployeeJobDescriptionList.Select(i => i.IdCompany));
                            }
                            // FillCompanySchedules(_idCompany);//Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                            IsJobDescriptionOrganizationChanged = false;
                            SelectedOrganizationIdList = EmployeeJobDescriptionList.Select(x => x.Company.IdCompany).ToList().OrderBy(i => i).Distinct().ToList();
                        }
                    }

                    //[006]
                    if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue == 138)
                    {

                        if (EmployeeContractSituationList.Any((ecs => ecs.ContractSituationStartDate.Value.Date <= GeosApplication.Instance.ServerDateTime.Date && (ecs.ContractSituationEndDate == null ? GeosApplication.Instance.ServerDateTime.Date : ecs.ContractSituationEndDate.Value.Date) >= GeosApplication.Instance.ServerDateTime.Date)))
                        {
                            SelectedEmpolyeeStatusIndex = EmployeeStatusList.IndexOf(x => x.IdLookupValue == 136);

                        }
                    }

                    error = EnableValidationAndGetError();
                    if (isInformationError)
                        InformationError = "";

                    else
                        InformationError = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedMaritalStatusIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedNationalityIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCountryIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGender"));
                    PropertyChanged(this, new PropertyChangedEventArgs("BirthDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ContractSituationError"));
                    PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionError"));
                    //Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                    //PropertyChanged(this, new PropertyChangedEventArgs("SelectedCompanyScheduleIndex")); 
                    //PropertyChanged(this, new PropertyChangedEventArgs("SelectedCompanyShiftIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCountryRegion"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCity"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedLocationIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("EmployeeShiftError"));
                    // PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));
                    if (error != null)
                    {
                        SelectedTabIndex = 0;
                        return;
                    }


                    if (JobDescriptionError != null || ContractSituationError != null)
                    {
                        return;
                    }

                    int PendingCount = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Count();
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
                                if (EmployeeProfessionalContactList != null)
                                {
                                    foreach (var item in EmployeeProfessionalContactList.Where(c => c.EmployeeContactType?.IdLookupValue == 88).ToList())
                                    {
                                        if (item != null)
                                        {
                                            if (ExistProfessionalEmailEmployeeList.Any(i => i.EmployeeContactProfessionalEmail?.ToLower() == item.EmployeeContactValue?.ToLower()))
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


                    EmployeeDetail = new Employee();
                    if (!string.IsNullOrEmpty(FirstName))
                        EmployeeDetail.FirstName = FirstName.Trim();
                    if (!string.IsNullOrEmpty(LastName))
                        EmployeeDetail.LastName = LastName.Trim();
                    if (!string.IsNullOrEmpty(DisplayName))
                        EmployeeDetail.DisplayName = DisplayName.Trim();
                    if (!string.IsNullOrEmpty(NativeName))
                        EmployeeDetail.NativeName = NativeName.Trim();
                    if (!string.IsNullOrEmpty(Address))
                        EmployeeDetail.AddressStreet = Address.Trim();
                    if (!string.IsNullOrEmpty(SelectedCity))
                        EmployeeDetail.AddressCity = SelectedCity.Trim();
                    if (!string.IsNullOrEmpty(ZipCode))
                        EmployeeDetail.AddressZipCode = ZipCode.Trim();
                    if (!string.IsNullOrEmpty(Remark))
                        EmployeeDetail.Remarks = Remark.Trim();
                    if (!string.IsNullOrEmpty(SelectedCountryRegion))
                        EmployeeDetail.AddressRegion = SelectedCountryRegion;

                    SelectedGender = GenderList.FirstOrDefault(x => x.IdLookupValue == GenderList[SelectedIndexGender].IdLookupValue);
                    EmployeeDetail.IdGender = (uint)SelectedGender.IdLookupValue;
                    EmployeeDetail.DateOfBirth = BirthDate;

                    EmployeeDetail.Latitude = Latitude;
                    EmployeeDetail.Longitude = Longitude;

                    EmployeeDetail.Disability = (ushort)SelectedDisability;
                    EmployeeDetail.IdMaritalStatus = (uint)MaritalStatusList[SelectedMaritalStatusIndex].IdLookupValue;
                    EmployeeDetail.IdNationality = NatinalityList[SelectedNationalityIndex].IdCountry;
                    EmployeeDetail.AddressIdCountry = CountryList[SelectedCountryIndex].IdCountry;
                    EmployeeDetail.IdEmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                    EmployeeDetail.Gender = SelectedGender;
                    EmployeeDetail.IdCompanyLocation = LocationList[SelectedLocationIndex].IdCompany;

                    if (SelectedEmpolyeeStatusIndex != -1)
                    {
                        EmployeeDetail.EmployeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex];
                    }

                    if (SelectedMaritalStatusIndex > 0)
                    {
                        EmployeeDetail.MaritalStatus = MaritalStatusList[SelectedMaritalStatusIndex];
                    }
                    else
                    {
                        EmployeeDetail.MaritalStatus = null;
                    }
                    EmployeeDetail.AddressCountry = CountryList[SelectedCountryIndex];
                    EmployeeDetail.IsEnabled = 1;
                    EmployeeDetail.EmployeeCode = EmployeeCode;
                    if (IsRemote)
                    {
                        Remote = 1;
                    }
                    else
                    {
                        Remote = 0;
                    }
                    EmployeeDetail.IsTrainer = (byte)Remote;
                    //Add Profile Image
                    if (IsPhoto)
                    {
                        ProfilePhotoBytes = PhotoSourceToByteArray(ProfilePhotoSource);
                    }

                    EmployeeDetail.ProfileImageInBytes = ProfilePhotoBytes;
                    //Add Tab Information

                    EmployeeDetail.EmployeePersonalContacts = new List<EmployeeContact>(EmployeeContactList);
                    EmployeeDetail.EmployeeProfessionalContacts = new List<EmployeeContact>(EmployeeProfessionalContactList);
                    EmployeeDetail.EmployeeDocuments = new List<EmployeeDocument>(EmployeeDocumentList);
                    EmployeeDetail.EmployeeLanguages = new List<EmployeeLanguage>(EmployeeLanguageList);
                    EmployeeDetail.EmployeeEducationQualifications = new List<EmployeeEducationQualification>(EmployeeEducationQualificationList);
                    EmployeeDetail.EmployeeContractSituations = new List<EmployeeContractSituation>(EmployeeContractSituationList);
                    //[001] added 
                    EmployeeDetail.EmployeePolyvalences = new List<EmployeePolyvalence>(employeePolyvalenceList);
                    //end
                    EmployeeDetail.EmployeeFamilyMembers = new List<EmployeeFamilyMember>(EmployeeFamilyMemberList);
                    EmployeeDetail.EmployeeJobDescriptions = new List<EmployeeJobDescription>(EmployeeJobDescriptionList);
                    EmployeeDetail.EmployeeProfessionalEducations = new List<EmployeeProfessionalEducation>(EmployeeProfessionalEducationList);
                    EmployeeDetail.EmployeeJobCodes = EmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionCode).ToList();
                    EmployeeDetail.Languages = String.Join("\n", EmployeeLanguageList.Select(x => x.Language.Value).ToArray());
                    if (EmployeeDetail.EmployeeDocuments.Count > 0)
                    {
                        EmployeeDetail.EmployeeDocument = new EmployeeDocument();
                        EmployeeDetail.EmployeeDocument = EmployeeDetail.EmployeeDocuments[0];

                    }
                    //Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                    //EmployeeDetail.IdCompanyShift = CompanyShifts[SelectedCompanyShiftIndex].IdCompanyShift;
                    //EmployeeDetail.CompanyShift = CompanyShifts[SelectedCompanyShiftIndex];
                    //EmployeeDetail.CompanyShift.CompanySchedule = CompanySchedules[SelectedCompanyScheduleIndex];

                    //[003] Added
                    if (EmployeeShiftList.Count > 0)
                    {
                        EmployeeDetail.IdCompanyShift = EmployeeShiftList[0].IdCompanyShift;
                    }

                    EmployeeDetail.EmployeeShifts = new List<EmployeeShift>(EmployeeShiftList).ToList();
                    EmployeeDetail.EmployeeAnnualLeaves = new List<EmployeeAnnualLeave>(EmployeeAnnualLeaveList).ToList();
                    EmployeeDetail.EmployeeAnnualLeavesAdditional = new List<EmployeeAnnualAdditionalLeave>();
                    for (int i = 0; i < EmployeeDetail.EmployeeAnnualLeaves.Count; i++)
                    {
                        EmployeeDetail.EmployeeAnnualLeavesAdditional.AddRange(EmployeeDetail.EmployeeAnnualLeaves[i].SelectedAdditionalLeaves.Select(x => (EmployeeAnnualAdditionalLeave)x.Clone()).ToList());
                    }

                    EmployeeDetail.IsLongTermAbsent = 0;
                    //(cpatil)Sprint52 - HRM-052-02	Avoid duplicated employees
                    //Show warning if employee fullname exist
                    List<Employee> employees = new List<Employee>();
                    employees.AddRange(HrmService.IsEmployeeExists(EmployeeDetail.FirstName, EmployeeDetail.LastName, 0));

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

                    EmployeeChangeLogList.Add(new EmployeeChangelog() { ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("NewEmployeeAddChangeLog").ToString(), string.Format("{0} {1}", EmployeeDetail.FirstName, EmployeeDetail.LastName)) });
                    EmployeeDetail.EmployeeChangelogs = new List<EmployeeChangelog>(EmployeeChangeLogList).ToList();
                    //[005]
                    List<EmployeeJobDescription> currentJobDescription = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).ToList();
                    EmployeeContractSituation itemECS = EmployeeContractSituationList.Where(a => a.ContractSituationEndDate >= DateTime.Now.Date || a.ContractSituationEndDate == null).FirstOrDefault();
                    if (currentJobDescription != null && itemECS != null)
                        if (currentJobDescription.Any(i => i.JobDescriptionStartDate.Value.Date == itemECS.ContractSituationStartDate.Value.Date))
                        {
                            if (currentJobDescription.Any(i => i.JobDescriptionStartDate.Value.Date < itemECS.ContractSituationStartDate.Value.Date))
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                IsBusy = false;
                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                return;
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionStartDateAndContractStartDateWarning").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            IsBusy = false;
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            return;
                        }

                    string newDisplayName = DisplayName;
                    //CheckAndShowDisplayNameMessageBox(newDisplayName);  //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
                    if (!string.IsNullOrEmpty(DisplayName))     //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
                        EmployeeDetail.DisplayName = DisplayName.Trim();

                    EmployeeDetail.IdApprovalResponsible = JobDescriptionList[SelectedJobDescription].IdJobDescription;
                    if (SelectedExtraHoursTime != null)
                    {
                        EmployeeDetail.IdExtraHoursTime = (UInt32)SelectedExtraHoursTime.IdLookupValue;
                    }


                    // [002] Changed Service method
                    // Employee newEmployee = HrmService.AddEmployee_V2034(EmployeeDetail);

                    //Employee newEmployee = HrmService.AddEmployee_V2036(EmployeeDetail);  //06-05-2020
                    //[004] service method change

                    // Employee newEmployee = HrmService.AddEmployee_V2046(EmployeeDetail); 
                    //[2844] service method changed 
                    //[007][GEOS2-3562] service method changed 
                    // Employee newEmployee = HrmService.AddEmployee_V2260(EmployeeDetail);
                    //[Sudhir.jangra][GEOS2-4846]
                    Employee newEmployee = HrmService.AddEmployee_V2480(EmployeeDetail);

                    EmployeeDetail.IdEmployee = newEmployee.IdEmployee;

                    IsSaved = true;

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmployeeSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);

                }
                GeosApplication.Instance.Logger.Log("Method SaveNewEmployeeProfile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveNewEmployeeProfile() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                IsBusy = false;
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveNewEmployeeProfile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                IsBusy = false;
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log(string.Format("Error in SaveNewEmployeeProfile() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Scale Image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
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
        /// <summary>
        /// method to Delete Document Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteDocumentInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDocumentInformationRecord()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                EmployeeDocument empDoc = (EmployeeDocument)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeDocumentList.Remove(empDoc);

                }

                GeosApplication.Instance.Logger.Log("Method DeleteDocumentInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDocumentInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Add New Employee JobDescription
        /// </summary>
        /// <param name="obj"></param>
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
                addJobDescriptionView.ShowDialog();
                if (addJobDescriptionViewModel.IsSave == true)
                {
                    if (addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription == 1 && EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                    {
                        EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                    }
                    else
                    {
                        if (addJobDescriptionViewModel.NewEmployeeJobDescription.IsMainJobDescription != 1)
                        {
                            List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Select(a => a.JobDescriptionUsage).ToList();
                            if (Percentages.Count > 0)
                            {
                                List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList();
                                if (JDList_Top.Count == 1 && addJobDescriptionViewModel.NewEmployeeJobDescription.JobDescriptionUsage < JDList_Top.FirstOrDefault().JobDescriptionUsage)
                                {
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                }
                                else
                                {
                                    if (JDList_Top.Count == 1 && Percentages.Max() != 100)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Hidden && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                    }
                                }
                            }
                        }
                    }
                    EmployeeJobDescriptionList.Add(addJobDescriptionViewModel.NewEmployeeJobDescription);
                    SelectedJobDescriptionRow = addJobDescriptionViewModel.NewEmployeeJobDescription;

                    EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now));
                    if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                    {
                        TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                    }
                    else
                        TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

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
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                        }
                        else
                        {
                            IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                            if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
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
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
                            }
                            else
                            {
                                EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                var temp = EmployeeJobDescriptionList;
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
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
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
                        var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null ||
                        a.JobDescriptionEndDate >= DateTime.Now).ToList();
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

                        FillJobDescriptionList(tempEmployeeTopFourJobDescriptionList);
                    }
                    if (EmployeeJobDescriptionList.Count < 1)
                    {
                        JobDescriptionError = "";
                    }
                    else
                    {
                        JobDescriptionError = null;
                    }


                }

                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Add New Contact Information        /// </summary>
        /// <param name="obj"></param>
        private void AddNewContactInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewContactInformation()...", category: Category.Info, priority: Priority.Low);

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
                addContactInformationView.ShowDialog();
                if (addContactInformationViewModel.IsSave == true)
                {
                    EmployeeContactList.Add(addContactInformationViewModel.NewContact);
                    SelectedContactRow = addContactInformationViewModel.NewContact;
                }
                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method AddNewContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewContactInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Delete Contact Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteContactInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteContactInformationRecord()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePersonalContactMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                EmployeeContact empContact = (EmployeeContact)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
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
        /// <summary>
        /// Method to Delete Professional ContactInformation Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteProfessionalContactInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalContactInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfessionalContactMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeContact empContact = (EmployeeContact)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeProfessionalContactList.Remove(empContact);
                }

                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalContactInformationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteProfessionalContactInformationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Add New Professional Contact Information
        /// </summary>
        /// <param name="obj"></param>
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

                addContactInformationView.ShowDialog();

                if (addContactInformationViewModel.IsSave == true)
                {
                    //EmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                    //SelectedProfessionalContactRow = addContactInformationViewModel.NewContact;

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
                        EmployeeProfessionalContactList.Add(addContactInformationViewModel.NewContact);
                        SelectedProfessionalContactRow = addContactInformationViewModel.NewContact;
                    }
                    #endregion
                }
                DisplayEmployeeProfEmailAndPhone();
                GeosApplication.Instance.Logger.Log("Method AddNewProfessionalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewProfessionalContactInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Delete Job Description Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteJobDescriptionRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteJobDescriptionRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteJobDescriptionMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeJobDescription empJobDescription = (EmployeeJobDescription)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeJobDescriptionList.Remove(empJobDescription);


                    EnableValidationAndGetError();
                    if (EmployeeJobDescriptionList != null)
                    {
                        //  EmployeeTopFourJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.ToList().Take(4).ToList());
                        var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now).ToList();
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

                EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now));
                if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                {
                    TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                }
                else
                    TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);
                if (EmployeeJobDescriptionList.Count < 1)
                {
                    JobDescriptionError = "";
                }
                else
                {
                    JobDescriptionError = null;
                }
                GeosApplication.Instance.Logger.Log("Method DeleteJobDescriptionRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteJobDescriptionRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Edit Employee Job Description
        /// </summary>
        /// <param name="obj"></param>
        private void EditEmployeeJobDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeJobDescription()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeJobDescription empJobDescription = (EmployeeJobDescription)detailView.DataControl.CurrentItem;
                SelectedJobDescriptionRow = empJobDescription;
                if (empJobDescription != null)
                {
                    AddJobDescriptionView addJobDescriptionView = new AddJobDescriptionView();
                    AddJobDescriptionViewModel addJobDescriptionViewModel = new AddJobDescriptionViewModel();
                    EventHandler handle = delegate { addJobDescriptionView.Close(); };
                    addJobDescriptionViewModel.RequestClose += handle;
                    addJobDescriptionView.DataContext = addJobDescriptionViewModel;
                    addJobDescriptionViewModel.SelectedEmpolyeeStatus = EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue;
                    addJobDescriptionViewModel.EditInit(EmployeeJobDescriptionList, empJobDescription);
                    addJobDescriptionViewModel.IsNew = false;
                    addJobDescriptionView.ShowDialog();
                    if (addJobDescriptionViewModel.IsSave == true)
                    {

                        if (addJobDescriptionViewModel.EditEmployeeJobDescription.IsMainJobDescription == 1 && EmployeeJobDescriptionList.Any(a => a.IsMainJobDescription == 1))
                        {
                            EmployeeJobDescriptionList.ToList().ForEach(a => { a.IsMainJobDescription = 0; a.IsMainVisible = Visibility.Hidden; });
                        }
                        if (empJobDescription.IdEmployeeJobDescription == 0)
                        {
                            addJobDescriptionViewModel.EditEmployeeJobDescription.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empJobDescription.Company = addJobDescriptionViewModel.EditEmployeeJobDescription.Company;
                        empJobDescription.IdCompany = addJobDescriptionViewModel.EditEmployeeJobDescription.IdCompany;
                        empJobDescription.JobDescription = addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescription;
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
                            List<ushort> Percentages = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null).Select(a => a.JobDescriptionUsage).ToList();
                            if (Percentages.Count > 0 && (addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionEndDate >= DateTime.Now.Date || addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionEndDate == null))
                            {
                                List<EmployeeJobDescription> JDList_Top = EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList();
                                if (JDList_Top.Count == 1 && addJobDescriptionViewModel.EditEmployeeJobDescription.JobDescriptionUsage < JDList_Top.FirstOrDefault().JobDescriptionUsage)
                                {
                                    EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
                                }
                                else
                                {
                                    if (JDList_Top.Count == 1 && Percentages.Max() != 100)
                                    {
                                        EmployeeJobDescriptionList.Where(a => a.JobDescriptionUsage == Percentages.Max() && a.IsMainJobDescription == 1 && a.IsMainVisible == Visibility.Hidden && (a.JobDescriptionEndDate >= DateTime.Now.Date || a.JobDescriptionEndDate == null)).ToList().ForEach(a => { a.IsMainJobDescription = 1; a.IsMainVisible = Visibility.Visible; });
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
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }
                            }
                            else
                            {
                                IsMainImageStyleList = finalEmployeeJobDescriptionList.Where(t => t.JobDescriptionEndDate == null).ToList();
                                if (IsMainImageStyleList.Any(a => a.IsMainJobDescription == 1))
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage) && x.IsMainJobDescription == 1).FirstOrDefault();
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
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
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
                                }
                                else
                                {
                                    EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                    EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                    EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                    EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                    var temp = EmployeeJobDescriptionList;
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
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
                                    }
                                    else
                                    {
                                        EmployeeJobDescription EmployeeJobDescription = IsMainImageStyleList.Where(x => x.JobDescriptionUsage == IsMainImageStyleList.Max(i => i.JobDescriptionUsage)).FirstOrDefault();
                                        EmployeeJobDescription.IsMainVisible = Visibility.Visible;
                                        EmployeeJobDescription.IsMainVisibleGreyImageVisible = Visibility.Hidden;
                                        EmployeeJobDescriptionList.Where(w => w.Equals(EmployeeJobDescription)).ToList().ForEach(f => { f.IsMainVisible = Visibility.Visible; f.IsMainVisibleGreyImageVisible = Visibility.Hidden; });
                                        var temp = EmployeeJobDescriptionList;
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
                            var tempEmployeeTopFourJobDescriptionList = EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now).ToList();
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
                        }

                        EmployeeJobDescriptionsList = new ObservableCollection<EmployeeJobDescription>(EmployeeJobDescriptionList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now));
                        if (EmployeeJobDescriptionList.Count > 0 && EmployeeJobDescriptionsList.Count == 0)
                        {
                            TotalJobDescriptionUsage = EmployeeJobDescriptionList.Sum(x => x.JobDescriptionUsage);
                        }
                        else
                            TotalJobDescriptionUsage = EmployeeJobDescriptionsList.Sum(x => x.JobDescriptionUsage);

                        if (EmployeeJobDescriptionList.Count < 1)
                        {
                            JobDescriptionError = "";
                        }
                        else
                        {
                            JobDescriptionError = null;
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditEmployeeJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeJobDescription()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Edit Employee Family Member
        /// </summary>
        /// <param name="obj"></param>
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

                    addFamilyMembersViewModel.EditInit(EmployeeFamilyMemberList, empFamilyMember);
                    addFamilyMembersViewModel.IsNew = false;
                    addFamilyMembersView.ShowDialog();
                    if (addFamilyMembersViewModel.IsSave == true)
                    {

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
        /// Method to Edit Employee Education Qualification
        /// </summary>
        /// <param name="obj"></param>
        private void EditEmployeeEducationQualification(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeEducationQualification()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeEducationQualification empEducationQualification = (EmployeeEducationQualification)detailView.DataControl.CurrentItem;
                SelectedEducationQualificationRow = empEducationQualification;
                if (empEducationQualification != null)
                {
                    AddEducationView addEducationView = new AddEducationView();
                    AddEducationViewModel addEducationViewModel = new AddEducationViewModel();
                    EventHandler handle = delegate { addEducationView.Close(); };
                    addEducationViewModel.RequestClose += handle;
                    addEducationView.DataContext = addEducationViewModel;

                    addEducationViewModel.EditInit(EmployeeEducationQualificationList, empEducationQualification);
                    addEducationViewModel.IsNew = false;
                    addEducationView.ShowDialog();
                    if (addEducationViewModel.IsSave == true)
                    {
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
                        empEducationQualification.TransactionOperation = addEducationViewModel.EditEducationQualification.TransactionOperation;
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
        /// Method to Edit Employee ContractSituation
        /// </summary>
        /// <param name="obj"></param>
        private void EditEmployeeContractSituation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeContractSituation()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeContractSituation empContractSituation = (EmployeeContractSituation)detailView.DataControl.CurrentItem;
                SelectedContractSituationRow = empContractSituation;
                if (empContractSituation != null)
                {

                    AddContractSituationView addContractSituationView = new AddContractSituationView();
                    AddContractSituationViewModel addContractSituationViewModel = new AddContractSituationViewModel();
                    EventHandler handle = delegate { addContractSituationView.Close(); };
                    addContractSituationViewModel.RequestClose += handle;
                    addContractSituationView.DataContext = addContractSituationViewModel;

                    int index = EmployeeContractSituationList.IndexOf(x => x.IdContractSituation == empContractSituation.IdContractSituation);

                    addContractSituationViewModel.EditInit(EmployeeContractSituationList, empContractSituation);
                    addContractSituationViewModel.IsNew = false;
                    addContractSituationView.ShowDialog();
                    if (addContractSituationViewModel.IsSave == true)
                    {

                        if (empContractSituation.IdEmployeeContractSituation == 0)
                        {
                            addContractSituationViewModel.EditEmployeeContractSituation.TransactionOperation = ModelBase.TransactionOperations.Add;
                        }

                        empContractSituation.ContractSituation = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituation;
                        empContractSituation.IdContractSituation = addContractSituationViewModel.EditEmployeeContractSituation.IdContractSituation;
                        empContractSituation.ProfessionalCategory = addContractSituationViewModel.EditEmployeeContractSituation.ProfessionalCategory;
                        empContractSituation.IdProfessionalCategory = addContractSituationViewModel.EditEmployeeContractSituation.IdProfessionalCategory;
                        empContractSituation.ContractSituationStartDate = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationStartDate;
                        empContractSituation.ContractSituationEndDate = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationEndDate;
                        empContractSituation.ContractSituationFileName = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationFileName;
                        empContractSituation.ContractSituationFileInBytes = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationFileInBytes;
                        empContractSituation.ContractSituationRemarks = addContractSituationViewModel.EditEmployeeContractSituation.ContractSituationRemarks;
                        empContractSituation.TransactionOperation = addContractSituationViewModel.EditEmployeeContractSituation.TransactionOperation;
                        SelectedContractSituationRow = empContractSituation;


                    }
                }
                CalculateLengthOfService();
                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeContractSituation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeContractSituation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to Edit Language Information
        /// </summary>
        /// <param name="obj"></param>
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

                    addLanguagesViewModel.EditInit(EmployeeLanguageList, empLanguage);
                    addLanguagesViewModel.IsNew = false;
                    addLanguagesView.ShowDialog();
                    if (addLanguagesViewModel.IsSave == true)
                    {

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
                        SelectedLanguageRow = empLanguage;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditLanguageInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditLanguageInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Edit Professional Contact Information
        /// </summary>
        /// <param name="obj"></param>
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

                    addContactInformationViewModel.EditInit(empContact);
                    addContactInformationViewModel.WindowHeader = Application.Current.FindResource("EditProfessionalContactInformation").ToString();
                    addContactInformationViewModel.IsNew = false;
                    addContactInformationViewModel.IsCompanyUse = true;
                    addContactInformationView.ShowDialog();
                    if (addContactInformationViewModel.IsSave == true)
                    {

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
                        SelectedProfessionalContactRow = empContact;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditProfessionalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditProfessionalContactInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Edit Personal Contact Information
        /// </summary>
        /// <param name="obj"></param>
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
                    addContactInformationViewModel.EditInit(empContact);
                    addContactInformationViewModel.WindowHeader = Application.Current.FindResource("EditPersonalContactInformation").ToString();
                    addContactInformationViewModel.IsNew = false;
                    addContactInformationViewModel.IsCompanyUse = false;
                    addContactInformationView.ShowDialog();
                    if (addContactInformationViewModel.IsSave == true)
                    {

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
                        SelectedContactRow = empContact;

                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditPersonalContactInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditPersonalContactInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Edit Document Information
        /// </summary>
        /// <param name="obj"></param>
        private void EditDocumentInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDocumentInformation()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                EmployeeDocument empDocument = (EmployeeDocument)detailView.DataControl.CurrentItem;
                SelectedDocumentRow = empDocument;

                if (empDocument != null)
                {
                    AddIdentificationDocumentView addIdentificationDocumentView = new AddIdentificationDocumentView();
                    AddIdentificationDocumentViewModel addIdentificationDocumentViewModel = new AddIdentificationDocumentViewModel();
                    EventHandler handle = delegate { addIdentificationDocumentView.Close(); };
                    addIdentificationDocumentViewModel.RequestClose += handle;
                    addIdentificationDocumentView.DataContext = addIdentificationDocumentViewModel;

                    EmployeeContractSituation empActiveContract = (EmployeeContractSituationList.FirstOrDefault(x => x.ContractSituationEndDate == null || (x.ContractSituationEndDate.HasValue && x.ContractSituationEndDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)));
                    addIdentificationDocumentViewModel.EditInit(EmployeeDocumentList, empDocument, 0, empActiveContract);
                    addIdentificationDocumentViewModel.IsNew = false;
                    addIdentificationDocumentView.ShowDialog();
                    if (addIdentificationDocumentViewModel.IsSave == true)
                    {
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
                        SelectedDocumentRow = empDocument;


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
        /// Method for Text EditValue Changing
        /// </summary>
        /// <param name="e"></param>
        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);

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
                        firstNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    if (IsFromLastName)
                        lastNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    if (IsFromNativeName)
                        nativeNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    if (IsFromCity)
                        cityErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    if (IsFromRegion)
                        regionErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                }
                error = EnableValidationAndGetError();

                if (IsFromFirstName)
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                if (IsFromLastName)
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                if (IsFromDisplayName)
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
                if (IsFromNativeName)
                    PropertyChanged(this, new PropertyChangedEventArgs("NativeName"));
                if (IsFromCity)
                    PropertyChanged(this, new PropertyChangedEventArgs("City"));
                if (IsFromRegion)
                    PropertyChanged(this, new PropertyChangedEventArgs("Region"));
                IsFromFirstName = false;
                IsFromLastName = false;
                IsFromNativeName = false;
                IsFromCity = false;
                IsFromRegion = false;
            }


            GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method to Send Mail to Person        
        /// /// </summary>
        /// <param name="obj"></param>
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
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Delete Professional Education Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteProfessionalEducationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalEducationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfessionalEducationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeProfessionalEducation empProfessionalEducation = (EmployeeProfessionalEducation)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeProfessionalEducationList.Remove(empProfessionalEducation);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteProfessionalEducationRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteProfessionalEducationRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Edit Employee Professional Education
        /// </summary>
        /// <param name="obj"></param>
        private void EditEmployeeProfessionalEducation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditEmployeeProfessionalEducation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeProfessionalEducation empProfEducation = (EmployeeProfessionalEducation)detailView.DataControl.CurrentItem;
                SelectedProfessionalEducationRow = empProfEducation;

                if (empProfEducation != null)
                {


                    AddProfessionalEducationView addProfessionalEducationView = new AddProfessionalEducationView();
                    AddProfessionalEducationViewModel addProfessionalEducationViewModel = new AddProfessionalEducationViewModel();

                    EventHandler handle = delegate { addProfessionalEducationView.Close(); };
                    addProfessionalEducationViewModel.RequestClose += handle;
                    addProfessionalEducationView.DataContext = addProfessionalEducationViewModel;

                    addProfessionalEducationViewModel.EditInit(EmployeeProfessionalEducationList, empProfEducation);
                    addProfessionalEducationViewModel.IsNew = false;
                    addProfessionalEducationView.ShowDialog();
                    if (addProfessionalEducationViewModel.IsSave == true)
                    {
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
                        SelectedProfessionalEducationRow = empProfEducation;
                    }
                }
                DisplayEmployeeProfEmailAndPhone();
                GeosApplication.Instance.Logger.Log("Method EditEmployeeProfessionalEducation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeeProfessionalEducation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Display Employee Professional Email And Phone
        /// </summary>

        private void DisplayEmployeeProfEmailAndPhone()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeProfEmailAndPhone()...", category: Category.Info, priority: Priority.Low);

                ProfContact = EmployeeProfessionalContactList.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88);
                if (ProfContact != null)
                    ProfessionalEmail = ProfContact.EmployeeContactValue;
                else
                    ProfessionalEmail = string.Empty;
                ProfContact = EmployeeProfessionalContactList.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 90);
                if (ProfContact != null)
                    ProfessionalPhone = ProfContact.EmployeeContactValue;
                else
                    ProfessionalPhone = string.Empty;

                ContactCount = EmployeeProfessionalContactList.Count + EmployeeContactList.Count;
                GeosApplication.Instance.Logger.Log("Method DisplayEmployeeProfEmailAndPhone()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DisplayEmployeeProfEmailAndPhone()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Calculate Length Of Service
        ///[HRM-M049-35] Display length of service months in employee profile [adadibathina][23102018]
        ///Added lengthOfServiceYears and lengthOfServiceMonths
        /// </summary>
        //[HRM-M052-03]----Length of service wrong[sdesai][10122018]
        private void CalculateLengthOfService()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()...", category: Category.Info, priority: Priority.Low);
                if (EmployeeContractSituationList.Count > 0)
                {
                    int month = 0;
                    int year = 0;
                    foreach (EmployeeContractSituation employeeContractSituation in EmployeeContractSituationList)
                    {
                        if (employeeContractSituation.ContractSituationStartDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)
                        {
                            lengthOfService += 0;
                        }
                        else if (employeeContractSituation.ContractSituationEndDate == null)
                        {

                            LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
                            LocalDate end = new LocalDate(GeosApplication.Instance.ServerDateTime.Year, GeosApplication.Instance.ServerDateTime.Month, GeosApplication.Instance.ServerDateTime.Day).PlusDays(1);
                            Period period = Period.Between(start, end, PeriodUnits.Months);
                            month = month + period.Months;
                        }
                        else
                        {
                            if (employeeContractSituation.ContractSituationEndDate > GeosApplication.Instance.ServerDateTime.Date)
                            {
                                LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
                                LocalDate end = new LocalDate(GeosApplication.Instance.ServerDateTime.Year, GeosApplication.Instance.ServerDateTime.Month, GeosApplication.Instance.ServerDateTime.Day).PlusDays(1);
                                Period period = Period.Between(start, end, PeriodUnits.Months);
                                month = month + period.Months;
                            }
                            else
                            {
                                LocalDate start = new LocalDate(employeeContractSituation.ContractSituationStartDate.Value.Year, employeeContractSituation.ContractSituationStartDate.Value.Month, employeeContractSituation.ContractSituationStartDate.Value.Day);
                                LocalDate end = new LocalDate(employeeContractSituation.ContractSituationEndDate.Value.Year, employeeContractSituation.ContractSituationEndDate.Value.Month, employeeContractSituation.ContractSituationEndDate.Value.Day).PlusDays(1);
                                Period period = Period.Between(start, end, PeriodUnits.Months);
                                month = month + period.Months;
                            }
                        }

                    }

                    if (month >= 12)
                    {
                        year += month / 12;
                        month = month % 12;
                        year = year + month / 12;
                    }

                    LengthOfService = Convert.ToString(year) + "y  " + Convert.ToString(month) + "M";
                }

                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateLengthOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }





        /// <summary>
        /// Method to Delete Education Qualification Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteEducationQualificationInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteEducationQualificationInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteEducationQualificationInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeEducationQualification empEducationQualification = (EmployeeEducationQualification)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
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
        /// Method to Delete Family Member Information Record
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteFamilyMemberInformationRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFamilyMemberInformationRecord()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteFamilyMemberInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeFamilyMember empFamilyMember = (EmployeeFamilyMember)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
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
        /// Method to calculate age of employee
        /// <para>[001][skale][2019-04-03][GEOS2-46] Wrong Age in employees</para>
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
                GeosApplication.Instance.Logger.Log("Method CalculateAge()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateAge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("BirthDate"));
                CalculateAge();
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDateEditValueChanging()", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in SendMail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    addPolyvalenceView.ShowDialog();

                    if (addPolyvalenceViewModel.IsSave == true)
                    {
                        EmployeePolyvalenceList.Add(addPolyvalenceViewModel.NewEmployeePolyvalence);
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
        /// [000][avpawar][02/05/2019][GEOS2-1468][Add polyvalence section in employee profile]
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

                    int index = employeePolyvalenceList.IndexOf(x => x.IdEmployeePolyvalence == empPolyvalence.IdEmployeePolyvalence);

                    addPolyvalenceViewModel.EditInit(employeePolyvalenceList, empPolyvalence);
                    addPolyvalenceViewModel.IsNew = false;
                    addPolyvalenceView.ShowDialog();

                    if (addPolyvalenceViewModel.IsSave == true)
                    {
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
                        /// UpdatedEmployeePolyvalenceList.Add(addPolyvalenceViewModel.EditEmployeePolyvalence);
                        SelectedPolyvalenceRow = empPolyvalence;
                    }
                    GeosApplication.Instance.Logger.Log("Method EditEmployeePolyvalence()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditEmployeePolyvalence()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // <summary>
        // [000][avpawar][02/05/2019][GEOS2-1468][Add polyvalence section in employee profile]
        /// Method for Delete Polyvalence Record. 
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
                    EmployeePolyvalenceList.Remove(empPolyvalence);
                }

                GeosApplication.Instance.Logger.Log("Method DeletePolyvalenceRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeletePolyvalenceRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        //Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
        //private void FillCompanySchedules(string idCompany)
        //{

        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillCompanySchedules()...", category: Category.Info, priority: Priority.Low);
        //        if (!string.IsNullOrEmpty(idCompany))
        //        {
        //            CompanySchedules = HrmService.GetCompanyScheduleAndCompanyShifts(idCompany);
        //            SelectedCompanyScheduleIndex = 0;
        //        }
        //        else
        //        {
        //            string idCompanies = null;
        //            if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null && HrmCommon.Instance.SelectedAuthorizedPlantsList.Count > 0)
        //            {
        //                idCompanies = string.Join(",", HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList().Select(i => i.IdCompany));
        //            }
        //            else
        //            {
        //                idCompanies = Convert.ToString(CurrentGeosProvider.IdCompany);
        //            }
        //            CompanySchedules = HrmService.GetCompanyScheduleAndCompanyShifts(idCompanies);
        //            SelectedCompanyScheduleIndex = 0;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method FillCompanySchedules()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method FillCompanySchedules()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void EmployeeStatusSelectedIndexChanged(object obj)
        {
            try
            {
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
        /// this method use for add employee new shift
        /// [000][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>
        /// <param name="obj"></param>
        private void AddEmployeeShift(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEmployeeShift()...", category: Category.Info, priority: Priority.Low);

                AddEmployeeShiftView addNewEmployeeShiftView = new AddEmployeeShiftView();
                AddEmployeeShiftViewModel addNewEmployeeShiftViewModel = new AddEmployeeShiftViewModel();
                EventHandler handle = delegate { addNewEmployeeShiftView.Close(); };
                addNewEmployeeShiftViewModel.RequestClose += handle;
                addNewEmployeeShiftView.DataContext = addNewEmployeeShiftViewModel;
                addNewEmployeeShiftViewModel.AddNewEmployeeShiftInit(EmployeeShiftList, EmployeeJobDescriptionList);
                addNewEmployeeShiftViewModel.IsNew = true;
                addNewEmployeeShiftView.ShowDialog();

                if (addNewEmployeeShiftViewModel.IsSave == true)
                {
                    EmployeeShiftList.AddRange(new ObservableCollection<EmployeeShift>(addNewEmployeeShiftViewModel.EmployeeNewShiftList));

                    TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());

                    if (EmployeeShiftList.Count > 4)
                        IsShiftAsteriskVisible = true;
                    else
                        IsShiftAsteriskVisible = false;
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
        ///  this method use for delete employee shift
        ///  [000][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
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
                    EmployeeShiftList.Remove(employeeShifts);
                    TopFourEmployeeShiftList = new ObservableCollection<EmployeeShift>(EmployeeShiftList.OrderBy(x => x.CompanyShift.IdCompanySchedule).Take(4).ToList());
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
        /// Method to add authorized leave
        /// [000][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// [001][cpatil][2020-01-06][GEOS2-1997]Authorized Leaves not added in selected period when new year comes
        /// </summary>
        /// <param name="obj"></param>
        private void AddAuthorizedLeave(object obj)
        {
            try
            {
                if (EmployeeShiftList.Count > 0)
                {
                    GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);

                    AddAuthorizedLeaveView addNewEmployeeAuthorizedLeaveView = new AddAuthorizedLeaveView();
                    AddAuthorizedLeaveViewModel addNewEmployeeAuthorizedLeaveViewModel = new AddAuthorizedLeaveViewModel();

                    EventHandler handle = delegate { addNewEmployeeAuthorizedLeaveView.Close(); };
                    addNewEmployeeAuthorizedLeaveViewModel.RequestClose += handle;
                    addNewEmployeeAuthorizedLeaveView.DataContext = addNewEmployeeAuthorizedLeaveViewModel;
                    //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                    addNewEmployeeAuthorizedLeaveViewModel.SelectedYear = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                    addNewEmployeeAuthorizedLeaveViewModel.IsAdd = true;
                    addNewEmployeeAuthorizedLeaveViewModel.IdCompanyShift = EmployeeShiftList[0].IdCompanyShift;
                    addNewEmployeeAuthorizedLeaveViewModel.IsNew = true;

                    //CompanyShift TempCompanyShift = HrmService.GetAnnualScheduleByIdCompanyShift(EmployeeShiftList[0].IdCompanyShift, GeosApplication.Instance.ServerDateTime.Date.Year);
                    //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                    CompanyShift TempCompanyShift = HrmService.GetAnnualScheduleByIdCompanyShift(EmployeeShiftList[0].IdCompanyShift, Convert.ToInt32(HrmCommon.Instance.SelectedPeriod));

                    if (TempCompanyShift != null)
                    {
                        DailyHoursCount = TempCompanyShift.CompanySchedule.CompanyAnnualSchedule.DailyHoursCount;
                        addNewEmployeeAuthorizedLeaveViewModel.DailyWorkHoursCount = TempCompanyShift.CompanySchedule.CompanyAnnualSchedule.DailyHoursCount;
                    }
                    else
                    {
                        DailyHoursCount = 0;
                        addNewEmployeeAuthorizedLeaveViewModel.DailyWorkHoursCount = 0;
                    }

                    addNewEmployeeAuthorizedLeaveViewModel.Init(EmployeeAnnualLeaveList, EmployeeDetail);
                    addNewEmployeeAuthorizedLeaveView.ShowDialog();

                    if (addNewEmployeeAuthorizedLeaveViewModel.IsSave)
                    {
                        EmployeeAnnualLeaveList.Add(addNewEmployeeAuthorizedLeaveViewModel.NewEmployeeAnnualLeave);
                        SelectedEmployeeAnnualLeave = addNewEmployeeAuthorizedLeaveViewModel.NewEmployeeAnnualLeave;
                        if (EmployeeAnnualLeaveList.Count > 0)
                        {
                            TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotifyEmployeeLeaveMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// [001][cpatil][2020-01-06][GEOS2-1997]Authorized Leaves not added in selected period when new year comes
        /// </summary>
        /// <param name="obj"></param>
        private void EditAuthorizedLeave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);

                if (SelectedEmployeeAnnualLeave != null)
                {
                    AddAuthorizedLeaveView addNewEmployeeAuthorizedLeaveView = new AddAuthorizedLeaveView();
                    AddAuthorizedLeaveViewModel addNewEmployeeAuthorizedLeaveViewModel = new AddAuthorizedLeaveViewModel();

                    EventHandler handle = delegate { addNewEmployeeAuthorizedLeaveView.Close(); };
                    addNewEmployeeAuthorizedLeaveViewModel.RequestClose += handle;
                    addNewEmployeeAuthorizedLeaveView.DataContext = addNewEmployeeAuthorizedLeaveViewModel;
                    //addNewEmployeeAuthorizedLeaveViewModel.SelectedYear = GeosApplication.Instance.ServerDateTime.Date.Year;
                    //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                    addNewEmployeeAuthorizedLeaveViewModel.SelectedYear = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                    addNewEmployeeAuthorizedLeaveViewModel.IsAdd = false;
                    addNewEmployeeAuthorizedLeaveViewModel.IsEdit = true;
                    addNewEmployeeAuthorizedLeaveViewModel.IsNew = false;
                    addNewEmployeeAuthorizedLeaveViewModel.IdCompanyShift = EmployeeShiftList[0].IdCompanyShift;
                    addNewEmployeeAuthorizedLeaveViewModel.DailyWorkHoursCount = DailyHoursCount;

                    if (DailyHoursCount < 0)
                    {
                        // CompanyShift TempCompanyShift = HrmService.GetAnnualScheduleByIdCompanyShift(EmployeeShiftList[0].IdCompanyShift, GeosApplication.Instance.ServerDateTime.Date.Year);
                        //[001] Changes selected year GeosApplication.Instance.ServerDateTime.Date.Year to Convert.ToInt32(HrmCommon.Instance.SelectedPeriod)
                        CompanyShift TempCompanyShift = HrmService.GetAnnualScheduleByIdCompanyShift(EmployeeShiftList[0].IdCompanyShift, Convert.ToInt32(HrmCommon.Instance.SelectedPeriod));

                        if (TempCompanyShift != null)
                        {
                            DailyHoursCount = TempCompanyShift.CompanySchedule.CompanyAnnualSchedule.DailyHoursCount;
                            addNewEmployeeAuthorizedLeaveViewModel.DailyWorkHoursCount = TempCompanyShift.CompanySchedule.CompanyAnnualSchedule.DailyHoursCount;
                        }
                        else
                        {
                            DailyHoursCount = 0;
                            addNewEmployeeAuthorizedLeaveViewModel.DailyWorkHoursCount = 0;
                        }

                    }
                    addNewEmployeeAuthorizedLeaveViewModel.EditInit(SelectedEmployeeAnnualLeave, EmployeeAnnualLeaveList, employeeDetail);
                    addNewEmployeeAuthorizedLeaveView.ShowDialog();

                    if (addNewEmployeeAuthorizedLeaveViewModel.IsSave)
                    {
                        // SelectedEmployeeAnnualLeave =(EmployeeAnnualLeave) addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.Clone();




                        bool LeaveReplacedInList = false;
                        for (int i = 0; i < EmployeeAnnualLeaveList.Count; i++)
                        {
                            if (EmployeeAnnualLeaveList[i].IdLeave ==
                                SelectedEmployeeAnnualLeave.IdLeave)
                            {
                                EmployeeAnnualLeaveList[i] = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave;
                                LeaveReplacedInList = true;
                                break;
                            }
                        }
                        if (!LeaveReplacedInList)
                        {
                            EmployeeAnnualLeaveList.Add(addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave);
                        }

                        //TempEmployeeAnnualLeaveList
                        LeaveReplacedInList = false;
                        for (int i = 0; i < TempEmployeeAnnualLeaveList.Count; i++)
                        {
                            if (TempEmployeeAnnualLeaveList[i].IdLeave ==
                                SelectedEmployeeAnnualLeave.IdLeave)
                            {
                                TempEmployeeAnnualLeaveList[i] = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave;
                                LeaveReplacedInList = true;
                                break;
                            }
                        }
                        if (!LeaveReplacedInList)
                        {
                            TempEmployeeAnnualLeaveList.Add(addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave);
                        }

                        //TableView detailView = (TableView)obj;
                        //EmployeeAnnualLeave AnnualLeave = (EmployeeAnnualLeave)detailView.DataControl.CurrentItem;
                        //  AnnualLeave = SelectedEmployeeAnnualLeave;

                        //SelectedEmployeeAnnualLeave.IdLeave = (long)addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.CompanyLeave.IdCompanyLeave;
                        //SelectedEmployeeAnnualLeave.CompanyLeave = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.CompanyLeave;
                        //SelectedEmployeeAnnualLeave.BacklogHoursCount = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.BacklogHoursCount;
                        //SelectedEmployeeAnnualLeave.RegularHoursCount = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.RegularHoursCount;
                        //SelectedEmployeeAnnualLeave.AdditionalHoursCount = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.AdditionalHoursCount;
                        //SelectedEmployeeAnnualLeave.EnjoyedTillYesterday = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.EnjoyedTillYesterday;
                        //SelectedEmployeeAnnualLeave.EnjoyingFromToday = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.EnjoyingFromToday;
                        //SelectedEmployeeAnnualLeave.Remaining = addNewEmployeeAuthorizedLeaveViewModel.UpdateEmployeeAnnualLeave.Remaining;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAuthorizedLeave() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAuthorizedLeave() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete employee leaves
        /// [000][skale][15-10-2019][GEOS2-1737] Available the “leaves”, “Shift” and “change Log” menu when create e new employee profile [ #ERF40]
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteAuthorizedLeaveRecord(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAuthorizedLeaveRecord()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLeaveInformationMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                EmployeeAnnualLeave employeeAnnualLeave = (EmployeeAnnualLeave)obj;

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EmployeeAnnualLeaveList.Remove(employeeAnnualLeave);

                    if (EmployeeAnnualLeaveList.Count > 0)
                    {
                        if (EmployeeAnnualLeaveList[0] != null)
                        {
                            TempEmployeeAnnualLeaveList.Add(EmployeeAnnualLeaveList[0]);
                        }
                    }
                    else
                    {
                        TempEmployeeAnnualLeaveList.Clear();
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteAuthorizedLeaveRecord()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAuthorizedLeaveRecord()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
        }

        //private void CustomShowFilterPopup(FilterPopupEventArgs e)
        //{
        //    GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);
        //    if (e.Column.FieldName != "RegularHoursCount" && e.Column.FieldName != "AdditionalHoursCount" && e.Column.FieldName != "Enjoyed" && e.Column.FieldName != "Remaining")
        //    {
        //        return;
        //    }

        //    List<object> filterItems = new List<object>();
        //    if (e.Column.FieldName == "RegularHoursCount")
        //    {
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Blanks)",
        //            EditValue = CriteriaOperator.Parse("RegularHoursCount = ''")
        //        });
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Non blanks)",
        //            EditValue = CriteriaOperator.Parse("RegularHoursCount <> ''")
        //        });
        //        foreach (var dataObject in EmployeeAnnualLeaveList)
        //        {
        //            if (dataObject.RegularHoursCount == 0)
        //            {
        //                string DisplayValues = "0d";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.RegularHoursCount;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //            else
        //            {
        //                Int32 Days = (Int32)dataObject.RegularHoursCount / (Int32)DailyHoursCount;
        //                Int32 Hours = (Int32)dataObject.RegularHoursCount % (Int32)DailyHoursCount;

        //                string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.RegularHoursCount;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //        }
        //    }
        //    else if (e.Column.FieldName == "AdditionalHoursCount")
        //    {
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Blanks)",
        //            EditValue = CriteriaOperator.Parse("AdditionalHoursCount = ''")
        //        });

        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Non blanks)",
        //            EditValue = CriteriaOperator.Parse("AdditionalHoursCount <> ''")
        //        });

        //        foreach (var dataObject in EmployeeAnnualLeaveList)
        //        {
        //            if (dataObject.AdditionalHoursCount == 0)
        //            {
        //                string DisplayValues = "0d";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.AdditionalHoursCount;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //            else
        //            {
        //                Int32 Days = (Int32)dataObject.AdditionalHoursCount / (Int32)DailyHoursCount;
        //                Int32 Hours = (Int32)dataObject.AdditionalHoursCount % (Int32)DailyHoursCount;

        //                string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.AdditionalHoursCount;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //        }
        //    }
        //    else if (e.Column.FieldName == "Enjoyed")
        //    {
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Blanks)",
        //            EditValue = CriteriaOperator.Parse("Enjoyed = ''")
        //        });
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Non blanks)",
        //            EditValue = CriteriaOperator.Parse("Enjoyed <> ''")
        //        });
        //        foreach (var dataObject in EmployeeAnnualLeaveList)
        //        {
        //            if (dataObject.Enjoyed == 0)
        //            {
        //                string DisplayValues = "0d";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.Enjoyed;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //            else
        //            {
        //                Int32 Days = (Int32)dataObject.Enjoyed / (Int32)DailyHoursCount;
        //                Int32 Hours = (Int32)dataObject.Enjoyed % (Int32)DailyHoursCount;

        //                string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.Enjoyed;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //        }
        //    }
        //    else if (e.Column.FieldName == "Remaining")
        //    {
        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Blanks)",
        //            EditValue = CriteriaOperator.Parse("Remaining = ''")
        //        });

        //        filterItems.Add(new CustomComboBoxItem()
        //        {
        //            DisplayValue = "(Non blanks)",
        //            EditValue = CriteriaOperator.Parse("Remaining <> ''")
        //        });

        //        foreach (var dataObject in EmployeeAnnualLeaveList)
        //        {
        //            if (dataObject.Remaining == 0)
        //            {
        //                string DisplayValues = "0d";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.Remaining;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //            else
        //            {
        //                Int32 Days = (Int32)dataObject.Remaining / (Int32)DailyHoursCount;
        //                Int32 Hours = (Int32)dataObject.Remaining % (Int32)DailyHoursCount;

        //                string DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";
        //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
        //                {
        //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
        //                    customComboBoxItem.DisplayValue = DisplayValues;
        //                    customComboBoxItem.EditValue = dataObject.Remaining;
        //                    filterItems.Add(customComboBoxItem);
        //                }
        //            }
        //        }
        //    }
        //    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue));
        //    GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
        //}


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

        #region //chitra.girigosavi[10/01/2024][GEOS2-3401] HRM - Display name [#ERF89]
        private void GenerateDisplayName()
        {
            DisplayName = $"{FirstName} {LastName}".Trim();
        }

        //private void CheckAndShowDisplayNameMessageBox(string newDisplayName)
        //{
        //    // Concatenate FirstName and LastName to form the expected DisplayName
        //    string expectedDisplayName = $"{FirstName} {LastName}".Trim();

        //    // Check if the entered DisplayName is different from the expected one
        //    if (string.Compare(DisplayName, expectedDisplayName, StringComparison.OrdinalIgnoreCase) != 0)
        //    {
        //        MessageBoxResult defaultResult = MessageBoxResult.No;

        //        MessageBoxResult result = CustomMessageBox.Show(
        //            string.Format(Application.Current.Resources["NotifyDisplayNameMessage"].ToString(),
        //                DisplayName,
        //                expectedDisplayName),
        //            Application.Current.Resources["PopUpNotifyColor"].ToString(),
        //            CustomMessageBox.MessageImagePath.Question,
        //            MessageBoxButton.YesNo,
        //            defaultResult);

        //        // Handle the result as needed (e.g., update DisplayName if the user confirms)
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            DisplayName = newDisplayName;
        //        }
        //    }
        //}
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

                SelectedJobDescription = JobDescriptionList.IndexOf(JobDescriptionList.FirstOrDefault(x => x.JobDescriptionTitleAndCode == "---"));

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

                SelectedExtraHoursTime = ExtraHoursTimeList.FirstOrDefault(x => x.Value == "Do not change");


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

        private void SetUserPermission()//[GEOS2-5112][05.02.2024][rdixit]
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

        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {

            IsInformationError = false;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
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
                                me[BindableBase.GetPropertyName(() => DisplayName)] +
                                me[BindableBase.GetPropertyName(() => NativeName)] +
                                me[BindableBase.GetPropertyName(() => Address)] +
                                me[BindableBase.GetPropertyName(() => City)] +
                                me[BindableBase.GetPropertyName(() => Region)] +
                                me[BindableBase.GetPropertyName(() => SelectedNationalityIndex)] +
                                me[BindableBase.GetPropertyName(() => SelectedCountryIndex)] +
                                me[BindableBase.GetPropertyName(() => SelectedMaritalStatusIndex)] +
                                me[BindableBase.GetPropertyName(() => SelectedIndexGender)] +
                                me[BindableBase.GetPropertyName(() => BirthDate)] +
                                // Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                                //me[BindableBase.GetPropertyName(() => SelectedCompanyScheduleIndex)] + 
                                //me[BindableBase.GetPropertyName(() => SelectedCompanyShiftIndex)] +
                                me[BindableBase.GetPropertyName(() => JobDescriptionError)] +
                                me[BindableBase.GetPropertyName(() => ZipCode)] +
                                me[BindableBase.GetPropertyName(() => SelectedCity)] +
                                me[BindableBase.GetPropertyName(() => InformationError)] +
                                me[BindableBase.GetPropertyName(() => SelectedLocationIndex)] +
                                me[BindableBase.GetPropertyName(() => SelectedCountryRegion)] +
                                me[BindableBase.GetPropertyName(() => EmployeeShiftError)];
                //+
                //                 me[BindableBase.GetPropertyName(() => SelectedJobDescription)]
                //me[BindableBase.GetPropertyName(() => SelectedJobDescriptionRow)];

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

                string empFirstName = BindableBase.GetPropertyName(() => FirstName);
                string empLastName = BindableBase.GetPropertyName(() => LastName);
                string empDisplayName = BindableBase.GetPropertyName(() => DisplayName);
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
                // Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                //string empCompanySchedule = BindableBase.GetPropertyName(() => SelectedCompanyScheduleIndex); 
                //string empCompanyShift = BindableBase.GetPropertyName(() => SelectedCompanyShiftIndex);
                string empZipCode = BindableBase.GetPropertyName(() => ZipCode);
                string empSelectedCountryRegion = BindableBase.GetPropertyName(() => SelectedCountryRegion);
                string empSelectedCity = BindableBase.GetPropertyName(() => SelectedCity);
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string empSelectedlocation = BindableBase.GetPropertyName(() => SelectedLocationIndex);
                string empShiftError = BindableBase.GetPropertyName(() => EmployeeShiftError);
                //  string empselectedJobDescription = BindableBase.GetPropertyName(() => SelectedJobDescription);
                //if (columnName == empselectedJobDescription)
                //{
                //    return EmployeeProfileValidation.GetErrorMessage(empselectedJobDescription, SelectedJobDescription);
                //}

                if (columnName == empFirstName)
                {
                    if (!string.IsNullOrEmpty(firstNameErrorMsg))
                        return CheckInformationError(firstNameErrorMsg);
                    else
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empFirstName, FirstName));
                }
                if (columnName == empLastName)
                {
                    if (!string.IsNullOrEmpty(lastNameErrorMsg))
                        return CheckInformationError(lastNameErrorMsg);
                    else
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empLastName, LastName));

                }
                if (columnName == empDisplayName)
                {
                    if (!string.IsNullOrEmpty(DisplayNameErrorMsg))
                        return CheckInformationError(DisplayNameErrorMsg);
                    else
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empDisplayName, DisplayName));

                }




                if (columnName == empSelectedNationality)
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedNationality, SelectedNationalityIndex));
                if (columnName == empSelectedCountry)
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedCountry, SelectedCountryIndex));

                if (columnName == empselectedIndexGender)
                    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empselectedIndexGender, SelectedIndexGender));


                if (columnName == empbirthDate)
                {
                    if (BirthDate >= DateTime.Now)
                        return CheckInformationError(birthDateErrorMessage);
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                    {
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empbirthDate, BirthDate));
                    }
                }
                if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                {
                    if (columnName == empAddress)
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empAddress, Address));

                    if (columnName == empSelectedMaritalStatus)
                        return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empSelectedMaritalStatus, SelectedMaritalStatusIndex));
                }

                //if (columnName == empJobDescription)
                //{

                //    if (Convert.ToInt16(TotalJobDescriptionUsage) < 100 && Convert.ToInt16(TotalJobDescriptionUsage) > 0)
                //    {

                //        return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                //    }
                //    else
                //        return  EmployeeProfileValidation.GetErrorMessage(empJobDescription, JobDescriptionError);
                //}

                // Comment by [skale][03-10-2019][GEOS2-1714] Adjust shift developments (improvements), (#ERF38)
                //if (columnName == empCompanyShift)
                //{
                //    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empCompanyShift, SelectedCompanyShiftIndex));
                //}
                //if (columnName == empCompanySchedule)
                //{
                //    return CheckInformationError(EmployeeProfileValidation.GetErrorMessage(empCompanySchedule, SelectedCompanyScheduleIndex));
                //}

                if (columnName == empJobDescription)
                {

                    if (Convert.ToInt16(TotalJobDescriptionUsage) < 100 && Convert.ToInt16(TotalJobDescriptionUsage) > 0)
                    {

                        return (System.Windows.Application.Current.FindResource("EmployeeJobDescriptionUsageError").ToString());
                    }
                    else if (EmployeeStatusList[SelectedEmpolyeeStatusIndex].IdLookupValue != 138)
                    {
                        if (JobDescriptionStartDate != null && JobDescriptionStartDate > DateTime.Now)
                        {
                            return (string.Format(System.Windows.Application.Current.FindResource("EmployeeJobDescriptionStartDateError").ToString(), JobDescriptionName));
                        }
                    }
                    else
                        return EmployeeProfileValidation.GetErrorMessage(empJobDescription, JobDescriptionError);

                    if (EmployeeJobDescriptionList.Count < 1)
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empJobDescription, JobDescriptionError);
                    }

                }
                if (columnName == empContractSituation)
                    return EmployeeProfileValidation.GetErrorMessage(empContractSituation, ContractSituationError);

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

                if (columnName == empShiftError)
                    return EmployeeProfileValidation.GetErrorMessage(empShiftError, EmployeeShiftError);

                return null;
            }
        }
        /// <summary>
        /// If any feild is of Information has error set isInformationError = true;
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public string CheckInformationError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                IsInformationError = true;

                InformationError = EmployeeProfileValidation.GetErrorMessage("InformationError", "Error");
            }

            return error;
        }
        #endregion
    }

}