using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Modules.Hrm.Views;
using DevExpress.Xpf.Map;
using System.Windows;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Hrm;
using System.IO;
using Emdep.Geos.UI.Validations;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Hrm.Reports;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Epc;
using System.ServiceModel;
using DevExpress.XtraReports.UI;
using System.Globalization;
using System.Threading;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EditCompanyViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region TaskLog
        //[001][CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        //[002][skhade][19-12-2019][GEOS2-1888]Strange behaviour in option "Print" in Company details
        //[003][skhade][20-12-2019][GEOS2-1886]Add new fields in company report
        #endregion

        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private List<string> allComapanyTypes;
        private List<object> selectedComapanyTypes;
        private bool isInformationError;
        private int sateSelectedIndex;
        private string strSate;

        private string windowHeader;
        private bool isSave;
        private Company existCompany;
        private List<City> cityList;
        private int selectedCountryIndex;
        private List<string> countryWiseCities;
        private string selectedCity;
        private string selectedGroup;
        private string fiscalNumber;
        private string abbreviation;
        private string registeredName;
        private string email;
        private string website;
        private string address;
        private string plantName;
        private double? size;
        private string telephoneNumber;
        private string fax;
        private string coordinates;
        private string strState;
        private string state;
        private string zipCode;
        private ImageSource siteImage;
        private bool isPhoto;
        private DateTime? dateOfEstablishment;
        private string age;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private List<string> countryWiseRegions;
        private List<CountryRegion> countryRegionList;
        private List<Employee> employeeContactsList;
        private ImageSource employeeProfilePhotoSource;
        private byte[] siteImageBytes = null;
        private byte[] oldImageBytes = null;
        private ObservableCollection<CompanyChangelog> companyChangeLogList;
        public Int32 idCompany;
        private ObservableCollection<CompanyChangelog> companyChangeLogDetailsList;
        private string plantNameErrorMsg = string.Empty;
        private string error = string.Empty;
        private string registeredNameErrorMsg = string.Empty;
        private string abbreviationErrorMsg = string.Empty;
        private string fiscalNumberErrorMsg = string.Empty;
        private string sizeErrorMsg = string.Empty;
        private string establistmentDateErrorMsg = string.Empty;
        private string telephoneNumberErrorMsg = string.Empty;
        private string emailErrorMsg = string.Empty;
        private string addressErrorMsg = string.Empty;
        private string cityErrorMsg = string.Empty;
        private string stateErrorMsg = string.Empty;
        private string zipCodeErrorMsg = string.Empty;
        private string countryErrorMsg = string.Empty;
        private Regex regex;
        private string informationError;

        private ImageSource defaultPhotoSource;
        private bool isImageDeleted;
        private string extension;
        private bool isSiteImageChanged;
        private ObservableCollection<LookupValue> departmentAreaAverageList;
        private ObservableCollection<Company> totalEmployeeList;

        GeosAppSetting geosAppSettingMap;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties
        //public Bitmap ReportHeaderImage { get; set; }
        public Company UpdateComapny { get; set; }
        public List<Country> CountryList { get; set; }
        public List<CountryRegion> AllCountryRegionList { get; set; }
        public Company CompanyDetails { get; set; }

        public List<string> AllComapanyTypes
        {
            get { return allComapanyTypes; }
            set { allComapanyTypes = value; OnPropertyChanged(new PropertyChangedEventArgs("AllComapanyTypes")); }
        }

        public List<object> SelectedComapanyTypes
        {
            get { return selectedComapanyTypes; }
            set
            {
                if (value == null)
                    selectedComapanyTypes = new List<object>();
                else
                    selectedComapanyTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComapanyTypes"));
            }
        }



        public string StrSate
        {
            get { return strSate; }
            set { strSate = value; OnPropertyChanged(new PropertyChangedEventArgs("StrSate")); }
        }
        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
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

        public int SateSelectedIndex
        {
            get
            {
                return sateSelectedIndex;
            }

            set
            {
                sateSelectedIndex = value;
                if (value.Equals(-1))
                {
                    State = string.Empty;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("SateSelectedIndex"));
            }
        }


        public string Abbreviation
        {
            get
            {
                return abbreviation;
            }

            set
            {
                abbreviation = value;
                IsFromAbbreviation = true;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("Abbreviation"));
            }
        }
        public string FiscalNumber
        {
            get
            {
                return fiscalNumber;
            }

            set
            {
                fiscalNumber = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = true;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("FiscalNumber"));
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

        public Company ExistCompany
        {
            get
            {
                return existCompany;
            }

            set
            {
                existCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistCompany"));
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
                    SateSelectedIndex = CountryRegionList.FindIndex(x => x.Name == State);

                    CityList = GeosApplication.Instance.CityList.FindAll(x => x.IdCountry == CountryList[selectedCountryIndex].IdCountry).ToList();
                    CountryWiseCities = new List<string>();
                    CountryWiseCities = CityList.Select(x => x.Name).ToList();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountryIndex"));
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

        public string SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
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

        public string RegisteredName
        {
            get
            {
                return registeredName;
            }

            set
            {
                registeredName = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = true;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisteredName"));
            }
        }


        public string Website
        {
            get
            {
                return website;
            }

            set
            {
                website = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Website"));
            }
        }
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = true;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
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
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = true;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }

        public double? Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = true;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("Size"));
            }
        }

        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = true;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantName"));
            }
        }

        public string TelephoneNumber
        {
            get
            {
                return telephoneNumber;
            }

            set
            {
                telephoneNumber = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = true;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("TelephoneNumber"));
            }
        }

        public string Fax
        {
            get
            {
                return fax;
            }

            set
            {
                fax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fax"));
            }
        }

        public string Coordinates
        {
            get
            {
                return coordinates;
            }

            set
            {
                coordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Coordinates"));
            }
        }

        public string StrState
        {
            get
            {
                return strState;
            }

            set
            {
                strState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StrState"));
            }
        }


        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = true;
                IsFromTelephoneNumber = false;
                IsFromZipCode = false;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
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
                IsFromAbbreviation = false;
                IsFromCity = false;
                IsFromEmail = false;
                IsFromFiscalNumber = false;
                IsFromPlantName = false;
                IsFromRegisteredNumber = false;
                IsFromSize = false;
                IsFromState = false;
                IsFromTelephoneNumber = false;
                IsFromZipCode = true;
                IsFromAddress = false;
                OnPropertyChanged(new PropertyChangedEventArgs("ZipCode"));
            }
        }
        public ImageSource SiteImage
        {
            get
            {
                return siteImage;
            }
            set
            {
                siteImage = value;
                if (siteImage != null)
                {
                    IsPhoto = true;
                }
                else
                {
                    SiteImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));

                    IsPhoto = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SiteImage"));
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


        public DateTime? DateOfEstablishment
        {
            get
            {
                return dateOfEstablishment;
            }

            set
            {
                dateOfEstablishment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateOfEstablishment"));
            }
        }

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

        public List<Employee> EmployeeContactsList
        {
            get { return employeeContactsList; }
            set
            {
                employeeContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeContactsList"));
            }
        }

        public ImageSource EmployeeProfilePhotoSource
        {
            get
            {
                return employeeProfilePhotoSource;
            }
            set
            {
                employeeProfilePhotoSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeProfilePhotoSource"));
            }
        }

        public byte[] SiteImageBytes
        {
            get
            {
                return siteImageBytes;
            }

            set
            {
                siteImageBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SiteImageBytes"));
            }
        }

        public byte[] OldImageBytes
        {
            get
            {
                return oldImageBytes;
            }

            set
            {
                oldImageBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldImageBytes"));
            }
        }

        public ObservableCollection<CompanyChangelog> CompanyChangeLogList
        {
            get
            {
                return companyChangeLogList;
            }
            set
            {
                companyChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyChangeLogList"));
            }

        }


        public Int32 IdCompany
        {
            get
            {
                return idCompany;
            }
            set
            {
                idCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompany"));
            }
        }

        public ObservableCollection<CompanyChangelog> CompanyChangeLogDetailsList
        {
            get
            {
                return companyChangeLogDetailsList;
            }
            set
            {
                companyChangeLogDetailsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyChangeLogDetailsList"));
            }

        }

        public bool IsFromPlantName { get; set; }
        public bool IsFromRegisteredNumber { get; set; }
        public bool IsFromAbbreviation { get; set; }
        public bool IsFromCity { get; set; }
        public bool IsFromState { get; set; }
        public bool IsFromSize { get; set; }
        public bool IsFromTelephoneNumber { get; set; }
        public bool IsFromEmail { get; set; }
        public bool IsFromFiscalNumber { get; set; }
        public bool IsFromZipCode { get; set; }
        public bool IsFromAddress { get; set; }
        public bool IsImageDeleted
        {
            get
            {
                return isImageDeleted;
            }
            set
            {
                isImageDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageDeleted"));
            }
        }

        public string Extension
        {
            get
            {
                return extension;
            }
            set
            {
                extension = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Extension"));
            }
        }

        public bool IsSiteImageChanged
        {
            get
            {
                return isSiteImageChanged;
            }

            set
            {
                isSiteImageChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSiteImageChanged"));
            }
        }

        public ObservableCollection<LookupValue> DepartmentAreaAverageList
        {
            get
            {
                return departmentAreaAverageList;
            }

            set
            {
                departmentAreaAverageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentAreaAverageList"));
            }
        }

        public ObservableCollection<Company> TotalEmployeeList
        {
            get
            {
                return totalEmployeeList;
            }

            set
            {
                totalEmployeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalEmployeeList"));
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

        #region Public Commands
        public ICommand EditCompanyViewCancelButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand EditCompanyViewAcceptButtonCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand PrintCompanyDetailsCommand { get; set; }
        public ICommand CommonValueChanged { get; set; }

        public ICommand CommandTextInput { get; set; }
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

        #region Constructor

        public EditCompanyViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor EditCompanyViewModel()...", category: Category.Info, priority: Priority.Low);
                EditCompanyViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
                EditCompanyViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveCompanyInformation));
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                PrintCompanyDetailsCommand = new DelegateCommand<object>(PrintCompanyReport);

                CommonValueChanged = new RelayCommand(new Action<object>(CommonValidation));
                //ReportHeaderImage = new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo_mini);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                AllComapanyTypes = new List<string> { "Company", "Organization", "Location" };
                SelectedComapanyTypes = new List<object>();

                GeosApplication.Instance.Logger.Log("Constructor EditCompanyViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EditCompanyViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        private void CommonValidation(object obj)
        {
            if (allowValidation)
                EnableValidationAndGetError();
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        public void Init(Company company)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillCountryList();
                FillCountryRegionList();
                FillCityList();

                WindowHeader = System.Windows.Application.Current.FindResource("EditCompanyWindowHeader").ToString();
                ExistCompany = company;
                IdCompany = ExistCompany.IdCompany;
                GetEmdepSiteImage();
                SelectedGroup = ExistCompany.EnterpriseGroup.Name;
                PlantName = ExistCompany.Name;
                RegisteredName = ExistCompany.RegisteredName;
                FiscalNumber = ExistCompany.CIF;
                Abbreviation = ExistCompany.Alias;
                Size = ExistCompany.Size;
                DateOfEstablishment = ExistCompany.EstablishmentDate;
                TelephoneNumber = ExistCompany.Telephone;
                Email = ExistCompany.Email;
                Fax = ExistCompany.Fax;
                Website = ExistCompany.Website;
                Address = ExistCompany.Address;
                SelectedCity = ExistCompany.City;
                State = ExistCompany.Region;
                ZipCode = ExistCompany.ZipCode;
                SelectedCountryIndex = CountryList.FindIndex(x => x.IdCountry == ExistCompany.IdCountry);
                Latitude = ExistCompany.Latitude;

                Longitude = ExistCompany.Longitude;
                Coordinates = ExistCompany.Coordinates;
                FillSelectedComapanyTypes();
                EmployeeContactsList = new List<Employee>(ExistCompany.Employees).ToList();
                CompanyChangeLogDetailsList = new ObservableCollection<CompanyChangelog>(ExistCompany.CompanyChangelogs.OrderByDescending(x => x.IdCompanyChangeLog).ToList());

                if (Latitude != null && Longitude != null)
                    MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);

                if (SiteImage == null)
                {
                    SiteImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
                    IsPhoto = false;
                }
                else
                {
                    defaultPhotoSource = SiteImage;
                }

                CalculateAge();

                if (!HrmCommon.Instance.IsPermissionReadOnly)
                    error = EnableValidationAndGetError();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillSelectedComapanyTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSelectedComapanyTypes()...", category: Category.Info, priority: Priority.Low);

                if (ExistCompany.IsCompany == 1)
                    SelectedComapanyTypes.Add("Company");

                if (ExistCompany.IsOrganization == 1)
                    SelectedComapanyTypes.Add("Organization");

                if (ExistCompany.IsLocation == 1)
                    SelectedComapanyTypes.Add("Location");

                GeosApplication.Instance.Logger.Log("Method FillSelectedComapanyTypes()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillSelectedComapanyTypes() - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
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
                CountryList = new List<Country>();
                CountryList.Insert(0, new Country() { Name = "---" });
                CountryList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillCountryList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        public void SendMailtoPerson(object obj)
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
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);
                string website = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(website) && website != "-" && !website.Contains("@"))
                {
                    string[] websiteArray = website.Split(' ');
                    System.Diagnostics.Process.Start(websiteArray[0]);
                }

                GeosApplication.Instance.Logger.Log("Method OpenWebsite() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenWebsite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CalculateAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateAge()...", category: Category.Info, priority: Priority.Low);
                if (DateOfEstablishment != null)
                {
                    DateTime now = DateTime.Today;
                    DateTime Date = (DateTime)DateOfEstablishment;
                    int _Age; //= now.Year - Date.Year;

                    if (Date > now)
                    {
                        _Age = 0;
                    }
                    else
                    {
                        _Age = now.Year - Date.Year;
                        if(now.DayOfYear < Date.DayOfYear)
                        {
                            _Age = _Age - 1;
                        }
                    }
                   
                    Age = _Age.ToString();
                }
                GeosApplication.Instance.Logger.Log("Method CalculateAge()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateAge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                    error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("DateOfEstablishment"));
                CalculateAge();
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDateEditValueChanging() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                        Coordinates = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude.ToString() + " " + employeeSetLocationMapViewModel.MapLatitudeAndLongitude.ToString();
                        string[] fullAddress = employeeSetLocationMapViewModel.LocationAddress.Split(',');
                        var withoutCountryAddess = fullAddress.Take(fullAddress.Length - 1);
                        string addressWithoutCountry = string.Join(",", withoutCountryAddess.Where(x => x.Trim().Length != 0));
                        Address = addressWithoutCountry;
                        SelectedCountryIndex = CountryList.FindIndex(i => i.Name == fullAddress[fullAddress.Length - 1].ToUpper().Trim());

                        if (SelectedCountryIndex < 0)
                        {
                            SelectedCountryIndex = 0;
                            Address = employeeSetLocationMapViewModel.LocationAddress;
                        }

                        State = string.Empty;
                        ZipCode = string.Empty;
                        SelectedCity = string.Empty;
                    }
                }
                else
                {
                    Latitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                    Longitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                    MapLatitudeAndLongitude = employeeSetLocationMapViewModel.MapLatitudeAndLongitude;
                    Coordinates = employeeSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude.ToString() + " " + employeeSetLocationMapViewModel.MapLatitudeAndLongitude.ToString();
                    string[] fullAddress = employeeSetLocationMapViewModel.LocationAddress.Split(',');

                    //  address without country name.
                    var withoutCountryAddess = fullAddress.Take(fullAddress.Length - 1);
                    string addressWithoutCountry = string.Join(",", withoutCountryAddess.Where(x => x.Trim().Length != 0));
                    Address = addressWithoutCountry;

                    SelectedCountryIndex = CountryList.FindIndex(i => i.Name == fullAddress[fullAddress.Length - 1].ToUpper().Trim());

                    if (SelectedCountryIndex < 0)
                    {
                        SelectedCountryIndex = 0;
                        Address = employeeSetLocationMapViewModel.LocationAddress;
                    }

                    State = string.Empty;
                    ZipCode = string.Empty;
                    SelectedCity = string.Empty;
                }
            }
        }

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
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCountryRegionList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][2019-21-5][GEOS2-273][SP63]Add new fields Company, Organization,Location in employees grid
        /// </summary>
        private void SaveCompanyInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveCompanyInformation()...", category: Category.Info, priority: Priority.Low);

                InformationError = null;
                if (!HrmCommon.Instance.IsPermissionReadOnly)
                    error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("PlantName"));
                PropertyChanged(this, new PropertyChangedEventArgs("RegisteredName"));
                PropertyChanged(this, new PropertyChangedEventArgs("Abbreviation"));
                PropertyChanged(this, new PropertyChangedEventArgs("FiscalNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("Size"));
                PropertyChanged(this, new PropertyChangedEventArgs("DateOfEstablishment"));
                PropertyChanged(this, new PropertyChangedEventArgs("TelephoneNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCity"));
                PropertyChanged(this, new PropertyChangedEventArgs("State"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCountryIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedComapanyTypes"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                if (error != null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(PlantName))
                    PlantName = PlantName.Trim();
                if (!string.IsNullOrEmpty(RegisteredName))
                    RegisteredName = RegisteredName.Trim();
                if (!string.IsNullOrEmpty(FiscalNumber))
                    FiscalNumber = FiscalNumber.Trim();
                if (!string.IsNullOrEmpty(ZipCode))
                    ZipCode = ZipCode.Trim();
                if (!string.IsNullOrEmpty(SelectedCity))
                    SelectedCity = SelectedCity.Trim();
                if (!string.IsNullOrEmpty(State))
                    State = State.Trim();
                if (!string.IsNullOrEmpty(Abbreviation))
                    Abbreviation = Abbreviation.Trim();
                if (!string.IsNullOrEmpty(TelephoneNumber))
                    TelephoneNumber = TelephoneNumber.Trim();
                if (!string.IsNullOrEmpty(Email))
                    Email = Email.Trim();
                if (!string.IsNullOrEmpty(Fax))
                    Fax = Fax.Trim();
                if (!string.IsNullOrEmpty(Website))
                    Website = Website.Trim();
                if (!string.IsNullOrEmpty(Address))
                    Address = Address.Trim();

                if (IsPhoto)
                {
                    SiteImageBytes = ImageSourceToBytes(SiteImage);
                }

                if (!IsPhoto)
                {
                    IsImageDeleted = true;
                    SiteImageBytes = null;
                    Extension = ".png";
                    IsSiteImageChanged = true;
                }
                else if (defaultPhotoSource != SiteImage && IsPhoto)
                {
                    SiteImageBytes = SiteImageBytes;
                    IsImageDeleted = false;
                    Extension = ".png";
                    IsSiteImageChanged = true;
                }
                else
                {
                    SiteImageBytes = null;
                    IsSiteImageChanged = false;
                }

                UpdateComapny = new Company()
                {
                    IdCompany = IdCompany,
                    Name = PlantName,
                    RegisteredName = RegisteredName,
                    Alias = Abbreviation,
                    CIF = FiscalNumber,
                    Size = Size,
                    EstablishmentDate = (DateTime)DateOfEstablishment,
                    Telephone = TelephoneNumber,
                    Email = Email,
                    Fax = Fax,
                    Website = Website,
                    Address = Address,
                    City = SelectedCity,
                    Region = State,
                    ZipCode = ZipCode,
                    IdCountry = CountryList[SelectedCountryIndex].IdCountry,
                    Country = CountryList[SelectedCountryIndex],
                    Latitude = Latitude,
                    Longitude = Longitude,
                    ImageInBytes = SiteImageBytes,
                    FileExtension = Extension,
                    IsImageDeleted = IsImageDeleted,
                };

                UpdateComapny.Country.Name = CountryList[SelectedCountryIndex].Name;
                UpdateComapny.IsCompany = SelectedComapanyTypes.Any(x => x.Equals("Company")) ? (byte)1 : (byte)0;
                UpdateComapny.IsOrganization = SelectedComapanyTypes.Any(x => x.Equals("Organization")) ? (byte)1 : (byte)0;
                UpdateComapny.IsLocation = SelectedComapanyTypes.Any(x => x.Equals("Location")) ? (byte)1 : (byte)0;
                UpdateComapny.ShortName = UpdateComapny.Alias;
                AddCompanyChangedLogDetails();
                UpdateComapny.CompanyChangelogs = new List<CompanyChangelog>(CompanyChangeLogList);
                UpdateComapny = HrmService.UpdateCompany_V2031(UpdateComapny);

                if (HrmCommon.Instance.UserAuthorizedPlantsList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                {
                    if (UpdateComapny.IsCompany == 1)
                        if (!HrmCommon.Instance.IsCompanyList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                        {
                            HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany).IsCompany = 1;
                            HrmCommon.Instance.IsCompanyList.Add(HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany));
                        }

                    if (UpdateComapny.IsOrganization == 1)
                        if (!HrmCommon.Instance.IsOrganizationList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                        {
                            HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany).IsOrganization = 1;
                            HrmCommon.Instance.IsOrganizationList.Add(HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany));
                        }

                    if (UpdateComapny.IsLocation == 1)
                        if (!HrmCommon.Instance.IsLocationList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                        {
                            HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany).IsLocation = 1;
                            HrmCommon.Instance.IsLocationList.Add(HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.IdCompany == UpdateComapny.IdCompany));
                        }

                    if (UpdateComapny.IsCompany == 0)
                        if (HrmCommon.Instance.IsCompanyList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                            HrmCommon.Instance.IsCompanyList.Remove(HrmCommon.Instance.IsCompanyList.FirstOrDefault(x => x.IdCompany == ExistCompany.IdCompany));

                    if (UpdateComapny.IsOrganization == 0)
                        if (HrmCommon.Instance.IsOrganizationList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                            HrmCommon.Instance.IsOrganizationList.Remove(HrmCommon.Instance.IsOrganizationList.First(x => x.IdCompany == ExistCompany.IdCompany));

                    if (UpdateComapny.IsLocation == 0)
                        if (HrmCommon.Instance.IsLocationList.Any(x => x.IdCompany == UpdateComapny.IdCompany))
                            HrmCommon.Instance.IsLocationList.Remove(HrmCommon.Instance.IsLocationList.First(x => x.IdCompany == ExistCompany.IdCompany));

                    //[001] added
                    ObservableCollection<Company> tempCommonList = new ObservableCollection<Company>();

                    tempCommonList.AddRange(HrmCommon.Instance.IsCompanyList);
                    tempCommonList.AddRange(HrmCommon.Instance.IsOrganizationList);
                    tempCommonList.AddRange(HrmCommon.Instance.IsLocationList);

                    tempCommonList = new ObservableCollection<Company>(tempCommonList.OrderBy(Company => Company.IdCountry).ToList());
                    ObservableCollection<Company> tempCollection = new ObservableCollection<Company>(tempCommonList.GroupBy(x => x.IdCompany).Select(group => group.First()));
                    HrmCommon.Instance.CombineIslocationIsorganizationIscompanyList = tempCollection;

                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList.Count == 0)
                    {
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                        Company selectedPlant = HrmCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.ShortName == serviceurl);
                        if (selectedPlant != null)
                        {
                            HrmCommon.Instance.SelectedAuthorizedPlantsList.Add(selectedPlant);
                        }
                        else
                        {
                            HrmCommon.Instance.SelectedAuthorizedPlantsList.AddRange(GeosApplication.Instance.PlantOwnerUsersList);
                        }
                    }

                    //End
                }

                IsSave = true;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CompanyInformationUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method SaveCompanyInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EmployeeLeavesViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SaveCompanyInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void GetEmdepSiteImage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetEmdepSiteImage()...", category: Category.Info, priority: Priority.Low);
                OldImageBytes = GeosRepositoryServiceController.GetCompanyImage(IdCompany);
                SiteImage = ByteToImage(OldImageBytes);
                GeosApplication.Instance.Logger.Log("Method GetEmdepSiteImage()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                SiteImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/ImageEditLogo.png"));
                IsPhoto = false;
            }
        }

        public ImageSource ByteToImage(byte[] imageData)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (imageData == null || imageData.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(imageData))
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
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }


        /// <summary>
        /// This method is for to convert ImageSource to ByteArray
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public byte[] ImageSourceToBytes(ImageSource imageSource)
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

        //Change Log
        public void AddCompanyChangedLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCompanyChangedLogDetails()...", category: Category.Info, priority: Priority.Low);

                CompanyChangeLogList = new ObservableCollection<CompanyChangelog>();

                //Types

                if (UpdateComapny.IsCompany != ExistCompany.IsCompany)
                {
                    if (UpdateComapny.IsCompany == 1)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsCompanyEnable").ToString(), ExistCompany.Name) });
                    }
                    if (UpdateComapny.IsCompany == 0)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsCompanyDisable").ToString(), ExistCompany.Name) });
                    }
                }

                if (UpdateComapny.IsLocation != ExistCompany.IsLocation)
                {
                    if (UpdateComapny.IsLocation == 1)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsLocationEnable").ToString(), ExistCompany.Name) });
                    }
                    if (UpdateComapny.IsLocation == 0)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsLocationDisable").ToString(), ExistCompany.Name) });
                    }
                }

                if (UpdateComapny.IsOrganization != ExistCompany.IsOrganization)
                {
                    if (UpdateComapny.IsOrganization == 1)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsOrganizationEnable").ToString(), ExistCompany.Name) });

                    }
                    if (UpdateComapny.IsOrganization == 0)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogIsOrganizationDisable").ToString(), ExistCompany.Name) });
                    }
                }

                //Plant Name
                if (ExistCompany.Name != null && !ExistCompany.Name.Equals(PlantName))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogPlantName").ToString(), ExistCompany.Name) });
                }

                //RegisteredName
                if (!string.IsNullOrEmpty(ExistCompany.RegisteredName) && !string.IsNullOrEmpty(RegisteredName) && !ExistCompany.RegisteredName.Equals(RegisteredName))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogRegisteredName").ToString(), ExistCompany.RegisteredName, RegisteredName.Trim()) });
                }
                ////RegisteredName  Null
                if (string.IsNullOrEmpty(ExistCompany.RegisteredName) && !string.IsNullOrEmpty(RegisteredName))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogRegisteredName").ToString(), "None", RegisteredName.Trim()) });
                }

                //Abbreviation
                if (!string.IsNullOrEmpty(ExistCompany.Alias) && !string.IsNullOrEmpty(Abbreviation) && !ExistCompany.Alias.Equals(Abbreviation))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogAbbreviation").ToString(), ExistCompany.Alias, Abbreviation.Trim()) });
                }
                ////Abbreviation  Null
                if (string.IsNullOrEmpty(ExistCompany.Alias) && !string.IsNullOrEmpty(Abbreviation))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogAbbreviation").ToString(), "None", Abbreviation.Trim()) });
                }


                //FiscalNumber
                if (!string.IsNullOrEmpty(ExistCompany.CIF) && !string.IsNullOrEmpty(FiscalNumber) && !ExistCompany.CIF.Equals(FiscalNumber))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogFiscalNumber").ToString(), ExistCompany.CIF, FiscalNumber.Trim()) });
                }
                ////FiscalNumber  Null
                if (string.IsNullOrEmpty(ExistCompany.CIF) && !string.IsNullOrEmpty(FiscalNumber))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogFiscalNumber").ToString(), "None", FiscalNumber.Trim()) });
                }

                //Size
                if (ExistCompany.Size != null && Size != null && ExistCompany.Size != Size)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogSize").ToString(), ExistCompany.Size, Size) });
                }
                //Size  Null
                if (ExistCompany.Size == null && Size != null)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogSize").ToString(), "None", Size) });
                }

                //DateOfEstablishment
                if (ExistCompany.EstablishmentDate != null && DateOfEstablishment != null && ExistCompany.EstablishmentDate != DateOfEstablishment)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogDateOfEstablishment").ToString(), ExistCompany.EstablishmentDate.ToString("dd/MM/yyyy"), DateOfEstablishment.Value.Date.ToString("dd/MM/yyyy")) });
                }
                ////DateOfEstablishment  Null
                if (ExistCompany.EstablishmentDate == null && DateOfEstablishment != null)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogDateOfEstablishment").ToString(), "None", DateOfEstablishment.Value.Date.ToString("dd/MM/yyyy")) });
                }

                //Telephone Number
                if (!string.IsNullOrEmpty(ExistCompany.Telephone) && !string.IsNullOrEmpty(TelephoneNumber) && !ExistCompany.Telephone.Equals(TelephoneNumber))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogTelephoneNumber").ToString(), ExistCompany.Telephone, TelephoneNumber.Trim()) });
                }
                //Telephone Number  Null
                if (string.IsNullOrEmpty(ExistCompany.Telephone) && !string.IsNullOrEmpty(TelephoneNumber))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogTelephoneNumber").ToString(), "None", TelephoneNumber.Trim()) });
                }

                //Email 
                if (!string.IsNullOrEmpty(ExistCompany.Email) && !string.IsNullOrEmpty(Email) && !ExistCompany.Email.Equals(Email))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogEmail").ToString(), ExistCompany.Email, Email.Trim()) });
                }
                //Email  Null
                if (string.IsNullOrEmpty(ExistCompany.Email) && !string.IsNullOrEmpty(Email))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogEmail").ToString(), "None", Email.Trim()) });
                }

                //Address 
                if (!string.IsNullOrEmpty(ExistCompany.Address) && !string.IsNullOrEmpty(Address) && !ExistCompany.Address.Equals(Address))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogAddress").ToString(), ExistCompany.Address, Address.Trim()) });
                }
                //Address  Null
                if (string.IsNullOrEmpty(ExistCompany.Address) && !string.IsNullOrEmpty(Address))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogAddress").ToString(), "None", Address.Trim()) });
                }

                //City 
                if (!string.IsNullOrEmpty(ExistCompany.City) && !string.IsNullOrEmpty(SelectedCity) && !ExistCompany.City.Equals(SelectedCity))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogCity").ToString(), ExistCompany.City, SelectedCity.Trim()) });
                }
                //City  Null
                if (string.IsNullOrEmpty(ExistCompany.City) && !string.IsNullOrEmpty(SelectedCity))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogCity").ToString(), "None", SelectedCity.Trim()) });
                }

                //State 
                if (!string.IsNullOrEmpty(ExistCompany.Region) && !string.IsNullOrEmpty(State) && !ExistCompany.Region.Equals(State))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogState").ToString(), ExistCompany.Region, State.Trim()) });
                }
                //State  Null
                if (string.IsNullOrEmpty(ExistCompany.Region) && !string.IsNullOrEmpty(State))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogState").ToString(), "None", State.Trim()) });
                }

                //Country
                if (!ExistCompany.IdCountry.Equals(CountryList[SelectedCountryIndex].IdCountry))
                {
                    Country Old_Country = CountryList.FirstOrDefault(x => x.IdCountry == ExistCompany.IdCountry);
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogCountry").ToString(), Old_Country.Name, CountryList[SelectedCountryIndex].Name) });
                }
                if (ExistCompany.IdCountry == null && !ExistCompany.IdCountry.Equals(CountryList[SelectedCountryIndex].IdCountry))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogCountry").ToString(), "None", CountryList[SelectedCountryIndex].Name) });
                }

                //Zip code 
                if (!string.IsNullOrEmpty(ExistCompany.ZipCode) && !string.IsNullOrEmpty(ZipCode) && !ExistCompany.ZipCode.Equals(ZipCode))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogZipCode").ToString(), ExistCompany.ZipCode, ZipCode.Trim()) });
                }
                //Zip code  Null
                if (string.IsNullOrEmpty(ExistCompany.ZipCode) && !string.IsNullOrEmpty(ZipCode))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogZipCode").ToString(), "None", ZipCode.Trim()) });
                }

                //Fax 
                if (!string.IsNullOrEmpty(ExistCompany.Fax) && !string.IsNullOrEmpty(Fax) && !ExistCompany.Fax.Equals(Fax))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogFax").ToString(), ExistCompany.Fax, Fax.Trim()) });
                }
                //Fax  Null
                if (string.IsNullOrEmpty(ExistCompany.Fax) && !string.IsNullOrEmpty(Fax))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogFax").ToString(), "None", Fax.Trim()) });
                }
                if (!string.IsNullOrEmpty(ExistCompany.Fax) && string.IsNullOrEmpty(Fax))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogFax").ToString(), ExistCompany.Fax, "None") });
                }

                //Website 
                if (!string.IsNullOrEmpty(ExistCompany.Website) && !string.IsNullOrEmpty(Website) && !ExistCompany.Website.Equals(Website))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebsite").ToString(), ExistCompany.Website, Website.Trim()) });
                }

                if (string.IsNullOrEmpty(ExistCompany.Website) && !string.IsNullOrEmpty(Website))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebsite").ToString(), "None", Website.Trim()) });
                }
                if (!string.IsNullOrEmpty(ExistCompany.Website) && string.IsNullOrEmpty(Website))
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebsite").ToString(), ExistCompany.Website, "None") });
                }

                //Coordinates 
                if (ExistCompany.Longitude != null && ExistCompany.Latitude != null && Latitude != ExistCompany.Latitude && Longitude != ExistCompany.Longitude)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebCoordinates").ToString(), ExistCompany.Coordinates, Latitude.ToString() + " " + Longitude.ToString()) });
                }

                if (Longitude != null && Latitude != null && ExistCompany.Latitude == null && ExistCompany.Longitude == null)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebCoordinates").ToString(), "None", Latitude.ToString() + " " + Longitude.ToString()) });
                }

                if (ExistCompany.Longitude != null && ExistCompany.Latitude != null && Latitude == null && Longitude == null)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyChangeLogWebCoordinates").ToString(), ExistCompany.Coordinates, "None") });
                }

                if (OldImageBytes == null && SiteImageBytes != null)
                {
                    CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyImageAddedChangeLog").ToString()) });

                }
                else
                {
                    if (OldImageBytes != null && SiteImageBytes == null && IsSiteImageChanged)
                    {
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyImageRemovedChangeLog").ToString()) });
                    }
                    else if (OldImageBytes != null && SiteImageBytes != null)
                        CompanyChangeLogList.Add(new CompanyChangelog() { IdCompany = ExistCompany.IdCompany, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("CompanyImageChangedChangeLog").ToString()) });
                }

                GeosApplication.Instance.Logger.Log("Method AddCompanyChangedLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCompanyChangedLogDetails()....executed successfully", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                EnableValidationAndGetError();
                return;

                //pre code
                if (IsFromAbbreviation)
                    abbreviationErrorMsg = string.Empty;
                if (IsFromCity)
                    cityErrorMsg = string.Empty;
                if (IsFromEmail)
                    emailErrorMsg = string.Empty;
                if (IsFromFiscalNumber)
                    fiscalNumberErrorMsg = string.Empty;
                if (IsFromPlantName)
                    plantNameErrorMsg = string.Empty;
                if (IsFromRegisteredNumber)
                    registeredNameErrorMsg = string.Empty;
                if (IsFromSize)
                    sizeErrorMsg = string.Empty;
                if (IsFromState)
                    stateErrorMsg = string.Empty;
                if (IsFromTelephoneNumber)
                    telephoneNumberErrorMsg = string.Empty;
                if (IsFromZipCode)
                    zipCodeErrorMsg = string.Empty;
                if (IsFromAddress)
                    addressErrorMsg = string.Empty;

                var newInput = (string)e.NewValue;

                if (!string.IsNullOrEmpty(newInput))
                {
                    MatchCollection matches = regex.Matches(newInput.ToLower().ToString());
                    if (newInput.Count(char.IsDigit) > 0 || matches.Count > 0)
                    {
                        if (IsFromCity)
                        {
                            cityErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }

                        if (IsFromState)
                        {
                            stateErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                        }
                    }

                    if (!HrmCommon.Instance.IsPermissionReadOnly)
                        error = EnableValidationAndGetError();

                    if (IsFromAbbreviation)
                        PropertyChanged(this, new PropertyChangedEventArgs("Abbreviation"));
                    if (IsFromCity)
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedCity"));
                    if (IsFromEmail)
                        PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                    if (IsFromFiscalNumber)
                        PropertyChanged(this, new PropertyChangedEventArgs("FiscalNumber"));
                    if (IsFromPlantName)
                        PropertyChanged(this, new PropertyChangedEventArgs("PlantName"));
                    if (IsFromRegisteredNumber)
                        PropertyChanged(this, new PropertyChangedEventArgs("RegisteredName"));
                    if (IsFromSize)
                        PropertyChanged(this, new PropertyChangedEventArgs("Size"));
                    if (IsFromState)
                        PropertyChanged(this, new PropertyChangedEventArgs("State"));
                    if (IsFromTelephoneNumber)
                        PropertyChanged(this, new PropertyChangedEventArgs("TelephoneNumber"));
                    if (IsFromZipCode)
                        PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                    if (IsFromAddress)
                        PropertyChanged(this, new PropertyChangedEventArgs("Address"));

                    IsFromAbbreviation = false;
                    IsFromCity = false;
                    IsFromEmail = false;
                    IsFromFiscalNumber = false;
                    IsFromPlantName = false;
                    IsFromRegisteredNumber = false;
                    IsFromSize = false;
                    IsFromState = false;
                    IsFromTelephoneNumber = false;
                    IsFromZipCode = false;
                    IsFromAddress = false;
                }

                GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCompanyChangedLogDetails() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintCompanyReport(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCompanyReport()...", category: Category.Info, priority: Priority.Low);
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
                uint plantTotal = 0;
                DateTime date = DateTime.Now;

                ComapnyReport comapnyReport = new ComapnyReport();
                comapnyReport.xrCompanyAverageChart.BeginInit();
                comapnyReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                comapnyReport.xrLabel1.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrLabel1.Font.Size);
                comapnyReport.xrLabel2.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrLabel2.Font.Size);
                comapnyReport.xrlblPlantTotal.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrlblPlantTotal.Font.Size);
                comapnyReport.xrCompanyAverageChart.Legend.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrCompanyAverageChart.Legend.Font.Size);
                comapnyReport.xrLblPlantData.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrLblPlantData.Font.Size);
                comapnyReport.xrLblPlantLegalName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantAddress.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantTelephone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantManagerTelephone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantManager.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblPlantManagerEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                comapnyReport.xrLblPlantManagerEmail.PreviewClick += OnPreviewClick;
                comapnyReport.xrLabel5.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.xrLblPlantData.Font.Size);

                comapnyReport.xrLblPlantEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                comapnyReport.xrLblPlantEmailValue.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                comapnyReport.xrLblPlantEmailValue.PreviewClick += OnPreviewClick;

                comapnyReport.xrLblDOEValue.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, 9, System.Drawing.FontStyle.Regular);
                comapnyReport.xrLblWebsite.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                comapnyReport.xrLblWebsite.PreviewClick += xrLblWebsite_PreviewClick;

                comapnyReport.xrPictureBoxGMap.PreviewClick += xrPictureBoxGMap_PreviewClick;

                CompanyDepartmentReport companyDepartmentReport = new CompanyDepartmentReport();
                companyDepartmentReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                companyDepartmentReport.xrTableRow2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                CompanyJobDescriptionReport companyJobDescriptionReport = new CompanyJobDescriptionReport();
                companyJobDescriptionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                companyJobDescriptionReport.xrTableRow1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                //CompanyDetails = HrmService.GetCompanyDetailsByCompanyIdSelectedPeriod(IdCompany, HrmCommon.Instance.SelectedPeriod);
                CompanyDetails = ExistCompany;

                DepartmentAreaAverageList = new ObservableCollection<LookupValue>(HrmService.GetOrganizationalChartDepartmentArea(IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod));
                TotalEmployeeList = new ObservableCollection<Company>(HrmService.GetEmployeesCountByIdCompany(IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod));
                CompanyDetails.ImageInBytes = OldImageBytes;

                Employee PlantManager = CompanyDetails.Employees.FirstOrDefault(x => x.EmployeeJobDescription.JobDescription.IdJobDescription == 21);
                if (PlantManager != null)
                {
                    comapnyReport.xrLblPlantManager.Text = PlantManager.FirstName + " " + PlantManager.LastName;
                    comapnyReport.xrLblPlantManagerTelephone.Text = PlantManager.EmployeeContactCompanyMobiles;
                    comapnyReport.xrLblPlantManagerEmail.Text = PlantManager.EmployeeContactEmail;

                    if (PlantManager.ProfileImageInBytes != null)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(PlantManager.ProfileImageInBytes))
                        {
                            BitmapImage image = new BitmapImage();

                            image.BeginInit();
                            image.StreamSource = ms;
                            image.EndInit();
                            Bitmap img = new Bitmap(image.StreamSource);
                            comapnyReport.xrPbPlantManagetPgofile.Image = img;
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
                            comapnyReport.xrPbPlantManagetPgofile.Image = image;
                        }
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
                        comapnyReport.xrPbPlantManagetPgofile.Image = image;
                    }
                }

                CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
                comapnyReport.xrLabel4.Text = DateTime.Now.ToString(CultureInfo);
                XRLabel label = new XRLabel() { Text = CompanyDetails.Alias };
                label.WidthF = 60;
                label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, comapnyReport.Font.Size);
                comapnyReport.Bands[BandKind.TopMargin].Controls.Add(label);
                label.LocationF = new System.Drawing.PointF(754, 30);
                label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                label.ForeColor = System.Drawing.Color.White;
                label.BackColor = System.Drawing.Color.Black;

                //comapnyReport.imgLogo.Image = ReportHeaderImage;

                if (CompanyDetails.ImageInBytes != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(CompanyDetails.ImageInBytes))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();
                        Bitmap img = new Bitmap(image.StreamSource);
                        comapnyReport.xrPbSiteImage.Image = img;

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
                        comapnyReport.xrPbSiteImage.Image = image;
                    }

                }

                //using (MemoryStream outStream = new MemoryStream())
                //{
                //    BitmapEncoder enc = new BmpBitmapEncoder();
                //    enc.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/Call.png", UriKind.RelativeOrAbsolute))));
                //    enc.Save(outStream);
                //    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                //    Bitmap image = new Bitmap(bitmap);
                //    comapnyReport.xrPictureBox1.Image = image;
                //}
                //using (MemoryStream outStream = new MemoryStream())
                //{
                //    BitmapEncoder enc = new BmpBitmapEncoder();
                //    enc.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/Mail.png", UriKind.RelativeOrAbsolute))));
                //    enc.Save(outStream);
                //    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                //    Bitmap image = new Bitmap(bitmap);
                //    comapnyReport.xrPictureBox2.Image = image;
                //}

                if (TotalEmployeeList.Count > 0)
                {
                    for (int i = 0; i < TotalEmployeeList.Count; i++)
                    {
                        plantName = plantName + " " + TotalEmployeeList[i].Alias;
                        plantTotal = plantTotal + TotalEmployeeList[i].EmployeesCount;
                    }

                    comapnyReport.xrlblPlantTotal.Text = plantTotal.ToString();
                }

                comapnyReport.xrCompanyAverageChart.DataSource = DepartmentAreaAverageList;
                comapnyReport.xrCompanyAverageChart.Series[1].Name = CompanyDetails.Alias;
                comapnyReport.xrCompanyAverageChart.EndInit();

                List<Company> company = new List<Company>();
                company.Add(CompanyDetails);
                List<CompanyDepartment> CompanyDepartments = HrmService.GetCompanyDepartment(CompanyDetails.IdCompany, HrmCommon.Instance.SelectedPeriod);

                if (CompanyDepartments.Count == 0)
                {
                    companyDepartmentReport.Bands[BandKind.Detail].Controls.Remove(companyDepartmentReport.xrPanel1);
                }

                comapnyReport.DataSource = company;
                companyDepartmentReport.DataSource = CompanyDepartments;
                comapnyReport.xrCompanyDepartmentSubRpt.ReportSource = companyDepartmentReport;

                //001
                DocumentPreviewWindow window = new DocumentPreviewWindow() { Owner = (EditCompanyView)obj, WindowState = WindowState.Maximized, Top = 1 };
                window.PreviewControl.DocumentSource = comapnyReport;
                comapnyReport.CreateDocument();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //window.Activate();
                //window.ShowActivated = true;
                //window.ShowDialog();
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintEmployeeProfileReport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintCompanyReport() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintCompanyReport() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCompanyReport()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
        /// 002 - Added GMap click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrPictureBoxGMap_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method xrPictureBoxGMap_PreviewClick ...", category: Category.Info, priority: Priority.Low);

                if (geosAppSettingMap == null)
                    geosAppSettingMap = WorkbenchService.GetGeosAppSettings(31);

                System.Diagnostics.Process.Start(string.Format(geosAppSettingMap.DefaultValue, CompanyDetails.Latitude, CompanyDetails.Longitude));
                GeosApplication.Instance.Logger.Log("Method xrPictureBoxGMap_PreviewClick() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in xrPictureBoxGMap_PreviewClick() Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 002 - Added website click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrLblWebsite_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method xrLblWebsite_PreviewClick ...", category: Category.Info, priority: Priority.Low);
                System.Diagnostics.Process.Start(((DevExpress.XtraPrinting.TextBrickBase)e.Brick).Text);
                GeosApplication.Instance.Logger.Log("Method xrLblWebsite_PreviewClick() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in xrLblWebsite_PreviewClick() Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                IsInformationError = false;
                InformationError = string.Empty;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => PlantName)] +
                    me[BindableBase.GetPropertyName(() => RegisteredName)] +
                    me[BindableBase.GetPropertyName(() => Abbreviation)] +
                    me[BindableBase.GetPropertyName(() => FiscalNumber)] +
                    me[BindableBase.GetPropertyName(() => Size)] +
                    me[BindableBase.GetPropertyName(() => DateOfEstablishment)] +
                    me[BindableBase.GetPropertyName(() => TelephoneNumber)] +
                    me[BindableBase.GetPropertyName(() => Email)] +
                    me[BindableBase.GetPropertyName(() => Address)] +
                    me[BindableBase.GetPropertyName(() => SelectedCity)] +
                    me[BindableBase.GetPropertyName(() => State)] +
                    me[BindableBase.GetPropertyName(() => ZipCode)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => SelectedComapanyTypes)] +
                    me[BindableBase.GetPropertyName(() => SelectedCountryIndex)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string comapnyPlantName = BindableBase.GetPropertyName(() => PlantName);
                string comapnyRegisteredName = BindableBase.GetPropertyName(() => RegisteredName);
                string comapnyAbbreviation = BindableBase.GetPropertyName(() => Abbreviation);
                string comapnyFiscalNumber = BindableBase.GetPropertyName(() => FiscalNumber);
                string comapnySize = BindableBase.GetPropertyName(() => Size);
                string comapnyEstablishmentDate = BindableBase.GetPropertyName(() => DateOfEstablishment);
                string comapnyTelephoneNumber = BindableBase.GetPropertyName(() => TelephoneNumber);
                string comapnyEmail = BindableBase.GetPropertyName(() => Email);
                string comapnyAddress = BindableBase.GetPropertyName(() => Address);
                string comapnyCity = BindableBase.GetPropertyName(() => SelectedCity);
                string comapnyState = BindableBase.GetPropertyName(() => State);
                string comapnyZipCode = BindableBase.GetPropertyName(() => ZipCode);
                string comapnyCountry = BindableBase.GetPropertyName(() => SelectedCountryIndex);
                string selectedComapanyTypes = BindableBase.GetPropertyName(() => SelectedComapanyTypes);

                if (columnName == selectedComapanyTypes)
                {
                    if (SelectedComapanyTypes.Count == 0 || SelectedComapanyTypes == null)
                        return CompanyValidation.GetErrorMessage(selectedComapanyTypes, SelectedComapanyTypes);
                }

                if (columnName == comapnyPlantName)
                {
                    if (!string.IsNullOrEmpty(plantNameErrorMsg))
                    {
                        return CheckInformationError(plantNameErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyPlantName, PlantName));
                    }
                }

                if (columnName == comapnyRegisteredName)
                {
                    if (!string.IsNullOrEmpty(registeredNameErrorMsg))
                    {
                        return CheckInformationError(registeredNameErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyRegisteredName, RegisteredName));
                    }
                }

                if (columnName == comapnyAbbreviation)
                {
                    if (!string.IsNullOrEmpty(abbreviationErrorMsg))
                    {
                        return CheckInformationError(abbreviationErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyAbbreviation, Abbreviation));
                    }
                }

                if (columnName == comapnyFiscalNumber)
                {
                    if (!string.IsNullOrEmpty(fiscalNumberErrorMsg))
                    {
                        return CheckInformationError(fiscalNumberErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyFiscalNumber, FiscalNumber));
                    }
                }

                if (columnName == comapnySize)
                {
                    if (!string.IsNullOrEmpty(fiscalNumberErrorMsg))
                    {
                        return CheckInformationError(sizeErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnySize, Size));
                    }
                }

                if (columnName == comapnyEstablishmentDate)
                {
                    if (!string.IsNullOrEmpty(establistmentDateErrorMsg))
                    {
                        return CheckInformationError(establistmentDateErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyEstablishmentDate, DateOfEstablishment));
                    }
                }

                if (columnName == comapnyTelephoneNumber)
                {
                    if (!string.IsNullOrEmpty(telephoneNumberErrorMsg))
                    {
                        return CheckInformationError(telephoneNumberErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyTelephoneNumber, TelephoneNumber));
                    }
                }

                if (columnName == comapnyEmail)
                {
                    if (!string.IsNullOrEmpty(emailErrorMsg))
                    {
                        return CheckInformationError(emailErrorMsg);
                    }
                    else
                    {
                        return CompanyValidation.GetErrorMessage(comapnyEmail, Email);
                    }
                }

                if (columnName == comapnyAddress)
                {
                    if (!string.IsNullOrEmpty(addressErrorMsg))
                    {
                        return CheckInformationError(addressErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyAddress, Address));
                    }
                }

                if (columnName == comapnyCity)
                {
                    if (!string.IsNullOrEmpty(cityErrorMsg))
                    {
                        return CheckInformationError(cityErrorMsg);
                    }
                    else
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyCity, SelectedCity));

                }

                if (columnName == comapnyState)
                {
                    if (!string.IsNullOrEmpty(stateErrorMsg))
                    {
                        return CheckInformationError(stateErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyState, State));
                    }
                }

                if (columnName == comapnyZipCode)
                {
                    if (!string.IsNullOrEmpty(zipCodeErrorMsg))
                    {
                        return CheckInformationError(zipCodeErrorMsg);
                    }
                    else
                    {
                        return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyZipCode, ZipCode));
                    }
                }

                if (columnName == comapnyCountry)
                {
                    return CheckInformationError(CompanyValidation.GetErrorMessage(comapnyCountry, SelectedCountryIndex));
                }

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

                InformationError = CompanyValidation.GetErrorMessage("InformationError", "Error");
            }

            return error;
        }

        #endregion
        //-----------------------------mazhar---------------------------------//
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
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
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
