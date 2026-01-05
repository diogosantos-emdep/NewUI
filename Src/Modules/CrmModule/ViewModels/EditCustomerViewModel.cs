using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class EditCustomerViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        //GEOS2-194 Sprint60 (#70171) Not possible to create customer [adadibathina]
        #endregion

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        List<People> cloneSalesOwnerList;
        private int selectedIndexCompanyGroup;
        private int selectedIndexCompanyPlant = 0;
        private int selectedIndexBusinessField = 0;
        private int selectedIndexBusinessCenter = 0;
        private int selectedIndexBusinessProduct = 0;
        private int selectedIndexCountry = 0;

        private int? customerNumberofEmplyee;
        private int? customerLines;
        private int? cuttingMachines;
        private double? size;
        private string informationError;


        private ObservableCollection<Customer> companyGroupList;
        private ObservableCollection<Company> companyPlantList;
        private IList<Company> selectedCompanyList;
        public List<Country> CountryList { get; set; }
        public List<LookupValue> BusinessCenter { get; set; }
        public List<LookupValue> BusinessField { get; set; }
        public int CustomerGenerateDays { get; set; }
        public DateTime CustomerCreatedIn { get; set; }
        public DateTime? CustomerModifiedIn { get; set; }
        public IList<LookupValue> BusinessProduct { get; set; }
        private List<Object> selectedBusinessProductItems;
        private ObservableCollection<People> salesOwnerList;
        private ObservableCollection<People> siteContactsList;

        private string customerPlantName;
        private string customerWebsite;
        private string customerAddress;
        private string registeredName;
        private string customerCity;
        private string customerState;
        private string customerZip;
        private string customerTelephone;
        private string customerEmail;
        private string alias;
        private string customerFax;
        private string salesOwnersIds = "";
        private bool isShowErrorInAddress;
        private ObservableCollection<LogEntryBySite> listChangeLog;
        private List<LogEntryBySite> tempListChangeLog = new List<LogEntryBySite>();

        //  private ImageSource customerImageSource;
        private bool isBusy;
        private bool isCoordinatesNull;
        private Company customerData;
        private int citySelectedIndex;
        private double? latitude;    // map  latitude
        private double? longitude;   // map  longitude

        private GeoPoint mapLatitudeAndLongitude;

        public string AccountsCreatedDays { get; set; }
        public string AccountsCreatedDaysStr { get; set; }

        private ObservableCollection<People> listSalesUsers;
        byte[] UserProfileImageByte = null;
        private People salesResponsible;

        private bool inIt = false;
        public List<Company> AllPlantsLst = new List<Company>();
        private List<Company> plantNameLst;
        private List<string> plantNameStrLst;
        private Visibility alertVisibility;
        private List<Activity> customerActivityList;
        private Activity selectedActivity;
        private ObservableCollection<Offer> listPlantOpportunity;
        // private ObservableCollection<People> linkeditemsPeopleList;
        private bool isCustomerDetailsModified = false;
        private ObservableCollection<Company> entireCompanyPlantList;
        private List<City> cityList;
        private List<string> countryWiseCities;
        private List<string> countryWiseRegions;
        private List<CountryRegion> countryRegionList;
        private Visibility showAddValidationVisibility;
        string registerationNumber;
        private string assignedSalesOwnerError;
        private ObservableCollection<LookupValue> sourceList;//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        private LookupValue selectedSource;//[Sudhir.jangra][GEOS2-4663][28/08/2023]
        private bool isSourceButtonEnabled;//[Sudhir.Jangra][GEOS2-5170]
        private ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList;
        private Emdep.Geos.Data.Common.Crm.ShippingAddress selectedLinkedAddress;
        private string contentcountryname;
        string contentcountryimgurl = null;
        //[RGadhave][12.11.2024][GEOS2-6461]
        private ObservableCollection<CustomerDetail> incotermsList;
        private int selectedIndexIncoterms;
        private ObservableCollection<CustomerDetail> paymentTermsList;
        private int selectedIndexPaymentTerms;
        private int idincoterm;//[pramod.misal][GEOS2-6462][18-11-2024]
        private int idPaymentType;//[pramod.misal][GEOS2-6462][18-11-2024]
        private List<LookupValue> statusList;//chitra.girigosavi GEOS2-7242 31/03/2025
        private int selectedStatusIndex;//chitra.girigosavi GEOS2-7242 31/03/2025
        private LookupValue selectedStatus;//chitra.girigosavi GEOS2-7242 31/03/2025
        private string status;//chitra.girigosavi GEOS2-7242 31/03/2025
        private bool isPermissionManageCustomerContacts; //[pallavi.kale][GEOS2-8953][18.09.2025]
        private bool isPermissionCustomerReadOnly=false; //[pallavi.kale][GEOS2-8953][18.09.2025]
        private bool isPermissionToManageCustomer = true; //[pallavi.kale][GEOS2-8953][18.09.2025]
        private bool isContactEditAllowed=true; //[pallavi.kale][GEOS2-8953][18.09.2025]
        private bool isPermissionEnableToAddActivity; //[pallavi.kale][GEOS2-8953][18.09.2025]
        private bool isPermissionCustomerEdit;//[pallavi.kale][GEOS2-8961][06.11.2025]
        #endregion // Declaration

        #region Properties


        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Alias"));
            }
        }

        public int Idincoterm
        {
            get { return idincoterm; }
            set
            {
                idincoterm = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idincoterm"));

            }
        }

        public int IdPaymentType
        {
            get { return idPaymentType; }
            set
            {
                idPaymentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPaymentType"));

            }
        }

        public Emdep.Geos.Data.Common.Crm.ShippingAddress SelectedLinkedAddress
        {
            get
            {
                return selectedLinkedAddress;
            }

            set
            {
                selectedLinkedAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLinkedAddress"));
            }
        }
        public string AssignedSalesOwnerError
        {
            get { return assignedSalesOwnerError; }
            set { assignedSalesOwnerError = value; OnPropertyChanged(new PropertyChangedEventArgs("AssignedSalesOwnerError")); }
        }
        public Visibility ShowAddValidationVisibility
        {
            get
            {
                return showAddValidationVisibility;
            }

            set
            {
                showAddValidationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowAddValidationVisibility"));
            }
        }
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public bool IsCustomerDetailsModified
        {
            get { return isCustomerDetailsModified; }
            set
            {
                isCustomerDetailsModified = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomerDetailsModified"));
            }
        }
        //public ObservableCollection<People> LinkeditemsPeopleList
        //{
        //    get { return linkeditemsPeopleList; }
        //    set
        //    {
        //        linkeditemsPeopleList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsPeopleList"));
        //    }
        //}
        public ObservableCollection<Offer> ListPlantOpportunity
        {
            get
            {
                return listPlantOpportunity;
            }

            set
            {
                listPlantOpportunity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlantOpportunity"));
            }
        }
        public Activity SelectedActivity
        {
            get
            {
                return selectedActivity;
            }

            set
            {
                selectedActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivity"));
            }
        }

        public Company CustomerLog { get; set; }
        public List<LogEntryBySite> TempListChangeLog
        {
            get { return tempListChangeLog; }
            set
            {
                tempListChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListChangeLog"));
            }
        }

        public Visibility AlertVisibility
        {
            get
            {
                return alertVisibility;
            }

            set
            {
                alertVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibility"));
            }
        }

        public List<Company> PlantNameLst
        {
            get
            {
                return plantNameLst;
            }

            set
            {
                plantNameLst = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantNameLst"));
            }
        }

        public List<string> PlantNameStrLst
        {
            get
            {
                return plantNameStrLst;
            }

            set
            {
                plantNameStrLst = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantNameStrLst"));
            }
        }

        public People SalesResponsible
        {
            get { return salesResponsible; }
            set
            {
                salesResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesResponsible"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }
        public ObservableCollection<People> ListSalesUsers
        {
            get { return listSalesUsers; }
            set { listSalesUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("ListSalesUsers")); }
        }
        public ObservableCollection<LogEntryBySite> ListChangeLog
        {
            get { return listChangeLog; }
            set
            {
                SetProperty(ref listChangeLog, value, () => ListChangeLog);
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
        public Company CustomerData
        {
            get { return customerData; }
            set
            {
                customerData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerData"));
            }
        }

        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                selectedIndexCompanyGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
            }
        }

        public int SelectedIndexBusinessField
        {
            get { return selectedIndexBusinessField; }
            set
            {
                selectedIndexBusinessField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBusinessField"));
            }
        }

        public int SelectedIndexBusinessCenter
        {
            get { return selectedIndexBusinessCenter; }
            set
            {
                selectedIndexBusinessCenter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBusinessCenter"));
            }
        }

        public int SelectedIndexBusinessProduct
        {
            get { return selectedIndexBusinessProduct; }
            set
            {
                selectedIndexBusinessProduct = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexBusinessProduct"));
            }
        }

        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public ObservableCollection<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyPlantList"));
            }
        }

        public ObservableCollection<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
            }
        }

        public ObservableCollection<People> SiteContactsList
        {
            get { return siteContactsList; }
            set
            {
                siteContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SiteContactsList"));
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

        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));


            }
        }


        public string CustomerPlantName
        {
            get { return customerPlantName; }
            set
            {
                customerPlantName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlantName"));
                if (!inIt)
                    ShowPopupAsPerPlantName(customerPlantName);
            }
        }

        public string CustomerWebsite
        {
            get { return customerWebsite; }
            set
            {
                if (value != null)
                {
                    customerWebsite = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerWebsite"));
                }
            }
        }

        public string RegisteredName
        {
            get { return registeredName; }
            set
            {

                registeredName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisteredName"));
            }
        }
        public string CustomerAddress
        {
            get { return customerAddress; }
            set
            {

                customerAddress = value;
                ShowAddValidationVisibility = Visibility.Hidden;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerAddress"));

            }
        }
        //[GEOS2-3994][rdixit][17.11.2022]
        public string RegisterationNumber
        {
            get { return registerationNumber; }
            set
            {

                registerationNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegisterationNumber"));
            }
        }

        public string CustomerCity
        {
            get { return customerCity; }
            set
            {


                customerCity = value;
                if (!string.IsNullOrEmpty(value))
                {
                    customerCity = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerCity"));

                    GeosApplication.Instance.IsCityNameExist = true;
                    if (!string.IsNullOrEmpty(customerCity) && !inIt)
                        GeosApplication.Instance.IsCityNameExist = CustomerPlantName.Contains(CustomerCity);
                }
            }
        }
        public bool IsShowErrorInAddress
        {
            get { return isShowErrorInAddress; }

            set
            {
                isShowErrorInAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowErrorInAddress"));
            }

        }
        public string CustomerState
        {
            get { return customerState; }
            set
            {
                if (value != null)
                {
                    customerState = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerState"));
                }
            }
        }

        public string CustomerZip
        {
            get { return customerZip; }
            set
            {
                customerZip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerZip"));
            }
        }

        public string CustomerTelephone
        {
            get { return customerTelephone; }
            set
            {
                customerTelephone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerTelephone"));
            }
        }

        public string CustomerEmail
        {
            get { return customerEmail; }
            set
            {
                customerEmail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerEmail"));
            }
        }

        public string CustomerFax
        {
            get { return customerFax; }
            set
            {
                if (value != null)
                {
                    customerFax = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerFax"));
                }
            }
        }

        public double? Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Size"));
            }
        }

        public int? CustomerLines
        {
            get { return customerLines; }
            set
            {
                customerLines = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerLines"));
            }
        }

        public int? CustomerNumberofEmplyee
        {
            get { return customerNumberofEmplyee; }
            set
            {
                customerNumberofEmplyee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerNumberofEmplyee"));
            }
        }

        public int? CuttingMachines
        {
            get { return cuttingMachines; }
            set
            {
                cuttingMachines = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CuttingMachines"));
            }
        }


        public int SelectedIndexCountry
        {
            get { return selectedIndexCountry; }
            set
            {
                selectedIndexCountry = value;
                if (selectedIndexCountry != -1)
                {
                    if (selectedIndexCountry == 0)
                    {
                        CountryWiseRegions = new List<string>();
                        CountryWiseRegions = AllCountryRegionList.Select(x => x.Name).ToList();

                        CountryWiseCities = new List<string>();
                        CountryWiseCities = GeosApplication.Instance.CityList.Select(x => x.Name).ToList();
                    }
                    else
                    {
                        CountryRegionList = AllCountryRegionList.FindAll(x => x.IdCountry == CountryList[selectedIndexCountry].IdCountry);
                        CountryWiseRegions = new List<string>();
                        CountryWiseRegions = CountryRegionList.Select(x => x.Name).ToList();

                        CityList = GeosApplication.Instance.CityList.FindAll(x => x.IdCountry == CountryList[selectedIndexCountry].IdCountry).ToList();
                        CountryWiseCities = new List<string>();
                        CountryWiseCities = CityList.Select(x => x.Name).ToList();
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCountry"));
            }
        }

        //public ImageSource CustomerImageSource
        //{
        //    get { return customerImageSource; }
        //    set
        //    {
        //        customerImageSource = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CustomerImageSource"));
        //    }
        //}

        public IList<Company> SelectedCompanyList
        {
            get { return selectedCompanyList; }
            set { selectedCompanyList = value; }
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

        public List<object> SelectedBusinessProductItems
        {
            get
            {
                return selectedBusinessProductItems;
            }

            set
            {
                selectedBusinessProductItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBusinessProductItems"));
            }
        }

        private string businessProductStringLog;

        public string BusinessProductStringLog
        {
            get { return businessProductStringLog; }
            set { businessProductStringLog = value; OnPropertyChanged(new PropertyChangedEventArgs("BusinessProductStringLog")); }
        }


        public MapControl MapControlCopy { get; set; }

        public List<Activity> CustomerActivityList
        {
            get
            {
                return customerActivityList;
            }

            set
            {
                customerActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerActivityList"));
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

        public List<CountryRegion> AllCountryRegionList { get; set; }

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

        public List<People> CloneSalesOwnerList
        {
            get { return cloneSalesOwnerList; }
            set
            {
                cloneSalesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CloneSalesOwnerList"));
            }
        }

        public ObservableCollection<LookupValue> SourceList
        {
            get { return sourceList; }
            set
            {
                sourceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SourceList"));
            }
        }

        public LookupValue SelectedSource
        {
            get { return selectedSource; }
            set
            {
                selectedSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSource"));
            }
        }


        //[Sudhir.Jangra][GEOS2-5170]
        public bool IsSourceButtonEnabled
        {
            get { return isSourceButtonEnabled; }
            set
            {
                isSourceButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSourceButtonEnabled"));
            }
        }

        //[pramod.misal][GEOS2-][15-11-2024]
        public ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> ShippingAddressList
        {
            get { return shippingAddressList; }
            set
            {
                shippingAddressList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShippingAddressList"));
            }
        }
        List<Data.Common.Crm.ShippingAddress> originalShippingAddressList;
        public List<Data.Common.Crm.ShippingAddress> OriginalShippingAddressList
        {
            get { return originalShippingAddressList; }
            set
            {
                originalShippingAddressList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalShippingAddressList"));
            }
        }

        public string Contentcountryname
        {
            get { return contentcountryname; }
            set
            {
                contentcountryname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contentcountryname"));
            }
        }

        public string Contentcountryimgurl
        {
            get { return contentcountryimgurl; }
            set
            {
                contentcountryimgurl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contentcountryimgurl"));
            }
        }

        private int selectedIncotermId;
        public int SelectedIncotermId
        {
            get { return selectedIncotermId; }
            set
            {
                selectedIncotermId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncotermId"));
            }
        }

        private string selectedIncotermsName;
        public string SelectedIncotermsName
        {
            get { return selectedIncotermsName; }
            set { selectedIncotermsName = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncotermsName")); }
        }

        public int SelectedIndexPaymentTerms
        {
            get { return selectedIndexPaymentTerms; }
            set
            {
                selectedIndexPaymentTerms = value;
                if (PaymentTermsList != null && selectedIndexPaymentTerms >= 0 && selectedIndexPaymentTerms < PaymentTermsList.Count)
                {
                    SelectedPaymentTermsName = PaymentTermsList[selectedIndexPaymentTerms].PaymentTypeName;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPaymentTerms"));
            }
        }

        private string selectedPaymentTermsName;
        public string SelectedPaymentTermsName
        {
            get { return selectedPaymentTermsName; }
            set { selectedPaymentTermsName = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedPaymentTermsName")); }
        }

        public ObservableCollection<CustomerDetail> IncotermsList
        {
            get { return incotermsList; }
            set
            {
                incotermsList = value; OnPropertyChanged(new PropertyChangedEventArgs("IncotermsList"));
            }
        }

        public ObservableCollection<CustomerDetail> PaymentTermsList
        {
            get { return paymentTermsList; }
            set
            {
                paymentTermsList = value; OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsList"));
            }
        }

        public int SelectedIndexIncoterms
        {
            get { return selectedIndexIncoterms; }
            set
            {
                selectedIndexIncoterms = value;
                if (IncotermsList != null && selectedIndexIncoterms >= 0 && selectedIndexIncoterms < IncotermsList.Count)
                {
                    SelectedIncotermsName = IncotermsList[selectedIndexIncoterms].IncotermName;
                    SelectedIncotermId = IncotermsList[selectedIndexIncoterms].IdIncoterm;

                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexIncoterms"));
            }
        }


        //[pramod.misal][GEOS2-6462][22-11-2024]
        public bool isEnabledCancelButton = false;
        public bool IsEnabledCancelButton
        {
            get { return isEnabledCancelButton; }
            set
            {

                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
            }
        }

        //chitra.girigosavi GEOS2-7242 31/03/2025
        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
            }
        }

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
                if (SelectedStatus != null)
                {
                    SelectedStatusIndex = StatusList.FindIndex(i => i.IdLookupValue == SelectedStatus.IdLookupValue);
                }
            }
        }
        public string Status
        {
            get { return status; }
            set
            {
                if (value != null)
                {
                    status = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("Status"));
                }
            }
        }
        //[pallavi.kale][GEOS2-8953][18.09.2025]
        public bool IsPermissionManageCustomerContacts
        {
            get { return isPermissionManageCustomerContacts; }
            set
            {
                isPermissionManageCustomerContacts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionManageCustomerContacts "));
            }
        }
        //[pallavi.kale][GEOS2-8953][18.09.2025]
        public bool IsPermissionCustomerReadOnly
        {
            get { return isPermissionCustomerReadOnly; }
            set
            {
                isPermissionCustomerReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionCustomerReadOnly"));
            }
        }
        //[pallavi.kale][GEOS2-8953][18.09.2025]
        public bool IsPermissionToManageCustomer
        {
            get { return isPermissionToManageCustomer; }
            set
            {
                isPermissionToManageCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionToManageCustomer "));
            }
        }
        //[pallavi.kale][GEOS2-8953][18.09.2025]
        public bool IsContactEditAllowed
        {
            get { return isContactEditAllowed; }
            set
            {
                isContactEditAllowed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContactEditAllowed  "));
            }
        }
        //[pallavi.kale][GEOS2-8953][18.09.2025]
        public bool IsPermissionEnableToAddActivity
        {
            get { return isPermissionEnableToAddActivity; }
            set
            {
                isPermissionEnableToAddActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionEnableToAddActivity "));
            }
        }
        //[pallavi.kale][GEOS2-8961][06.11.2025]
        public bool IsPermissionCustomerEdit
        {
            get { return isPermissionCustomerEdit; }
            set
            {
                isPermissionCustomerEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionCustomerEdit"));
            }
        }
        #endregion // Properties

        #region public ICommand

        public ICommand EditCustomerViewCancelButtonCommand { get; set; }
        public ICommand EditCustomerViewAcceptButtonCommand { get; set; }
        public ICommand CustomerImageRemoveButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }
        public ICommand OnSearchCompletedCommand { get; set; }
        public ICommand OnLayerItemsGeneratingCommand { get; set; }
        public ICommand MapControlLoadedCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }

        public ICommand GetSalesUserContactCommand { get; set; }
        public ICommand SetSalesResponsibleCommand { get; set; }

        public ICommand AssignedSalesCancelCommand { get; set; }

        public ICommand AddNewActivityCommand { get; set; }
        public ICommand ActivitiesGridDoubleClickCommand { get; set; }
        public ICommand AddNewContactCommand { get; set; }
        public ICommand LinkedContactDoubleClickCommand { get; set; }

        public ICommand CommandTextInput { get; set; }

        public ICommand AddSourceButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-5170]

        public ICommand AddShippingAddressCommand { get; set; }// [pramod.misal][GEOS2-6462][13.11.2024]

        public ICommand LinkedAddressDoubleClickCommand { get; set; }// [pramod.misal][GEOS2-6462][13.11.2024]

        public ICommand SetDefaultShippingAddressCommand { get; set; }


        #endregion // public ICommand

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

        #endregion // Events

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
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +
                    me[BindableBase.GetPropertyName(() => CustomerPlantName)] +
                    me[BindableBase.GetPropertyName(() => SelectedBusinessProductItems)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexBusinessField)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexBusinessCenter)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCountry)] +
                    me[BindableBase.GetPropertyName(() => CustomerAddress)] +
                    me[BindableBase.GetPropertyName(() => CustomerCity)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => SelectedStatusIndex)];//chitra.girigosavi[GEOS2-7242][14/04/2025]
                string saleerror = me[BindableBase.GetPropertyName(() => AssignedSalesOwnerError)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                if (!string.IsNullOrEmpty(saleerror))
                    return "1";

                return null;
            }
        }


        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string SelectedIndexCompanyGroupProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);
                string CustomerPlantNameProp = BindableBase.GetPropertyName(() => CustomerPlantName);   // Lead Source
                string SelectedBusinessProductItemsProp = BindableBase.GetPropertyName(() => SelectedBusinessProductItems);
                string SelectedIndexBusinessFieldProp = BindableBase.GetPropertyName(() => SelectedIndexBusinessField);
                string SelectedIndexBusinessCenterProp = BindableBase.GetPropertyName(() => SelectedIndexBusinessCenter);
                string SelectedIndexCountryProp = BindableBase.GetPropertyName(() => SelectedIndexCountry);
                string CustomerAddressProp = BindableBase.GetPropertyName(() => CustomerAddress);
                string CustomerCityProp = BindableBase.GetPropertyName(() => CustomerCity);
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string assignedSalesOwnerError = BindableBase.GetPropertyName(() => AssignedSalesOwnerError);
                string SelectedStatusIndexProp = BindableBase.GetPropertyName(() => SelectedStatusIndex);//chitra.girigosavi[GEOS2-7242][14/04/2025]
                if (columnName == SelectedIndexCompanyGroupProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup, null);

                if (columnName == CustomerPlantNameProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(CustomerPlantNameProp, CustomerPlantName, null);

                if (columnName == SelectedBusinessProductItemsProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedBusinessProductItemsProp, SelectedBusinessProductItems, null);

                if (columnName == SelectedIndexBusinessFieldProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedIndexBusinessFieldProp, SelectedIndexBusinessField, null);

                if (columnName == SelectedIndexBusinessCenterProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedIndexBusinessCenterProp, SelectedIndexBusinessCenter, null);

                if (columnName == SelectedIndexCountryProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedIndexCountryProp, SelectedIndexCountry, null);

                if (columnName == CustomerAddressProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(CustomerAddressProp, IsShowErrorInAddress, null);

                if (columnName == CustomerCityProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(CustomerCityProp, CustomerCity, null);
                if (columnName == informationError)
                {
                    //[rdixit][07.04.2023][GEOS2-4295]
                    if (IsShowErrorInAddress)
                    {
                        return CustomerRequiredFieldValidation.GetErrorMessage(informationError, "AddressError", null);
                    }
                    else
                    {
                        return CustomerRequiredFieldValidation.GetErrorMessage(informationError, InformationError, null);
                    }
                }
                if (columnName == assignedSalesOwnerError)
                    return CustomerRequiredFieldValidation.GetErrorMessage(assignedSalesOwnerError, AssignedSalesOwnerError, SalesOwnerList.ToList());
                //chitra.girigosavi[GEOS2-7242][14/04/2025]
                if (columnName == SelectedStatusIndexProp)
                    return CustomerRequiredFieldValidation.GetErrorMessage(SelectedStatusIndexProp, SelectedStatusIndex, SalesOwnerList.ToList());
                return null;
            }
        }

        #endregion

        #region Constructor

        public EditCustomerViewModel()
        {
            EditCustomerViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            EditCustomerViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveCutomerDetails));
            CustomerImageRemoveButtonCommand = new RelayCommand(new Action<object>(RemoveCustomerImage));
            HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
            HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
            MapControlLoadedCommand = new DelegateCommand<object>(new Action<object>((obj) => { MapControlLoadedAction(obj); }));
            OnSearchCompletedCommand = new DelegateCommand<object>(new Action<object>((obj) => { OnSearchCompletedAction(obj); }));
            OnLayerItemsGeneratingCommand = new DelegateCommand<object>(new Action<object>((obj) => { OnLayerItemsGeneratingAction(obj); }));
            SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
            GetSalesUserContactCommand = new DelegateCommand<object>(SalesUserContactCheckedAction);
            SetSalesResponsibleCommand = new DelegateCommand<object>(SetSalesResponsibleCommandAction);
            AssignedSalesCancelCommand = new DelegateCommand<object>(AssignedSalesCancelCommandAction);
            AddNewContactCommand = new DelegateCommand<object>(AddContactViewWindowShow);
            AddNewActivityCommand = new DelegateCommand<object>(AddActivityViewWindowShow);
            ActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditActivityViewWindowShow);
            LinkedContactDoubleClickCommand = new DelegateCommand<object>(LinkedContactDoubleClickCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            AddShippingAddressCommand = new DelegateCommand<object>(AddShippingAddressCommandAction);// [pramod.misal][GEOS2-4450][27.06.2023]
            LinkedAddressDoubleClickCommand = new DelegateCommand<object>(LinkedAddressDoubleClickCommandAction);
            SetDefaultShippingAddressCommand = new DelegateCommand<object>(SetDefaultShippingAddressCommandAction);

            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 101))
            {
                IsSourceButtonEnabled = true;
            }
            else
            {
                IsSourceButtonEnabled = false;
            }

            AddSourceButtonCommand = new RelayCommand(new Action<object>(AddSourceButtonCommandAction));//[Sudhir.Jangra][GEOS2-5170]
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
            }
            ShowAddValidationVisibility = Visibility.Hidden;
            AllPlantsLst = CrmStartUp.GetAllCustomerCompanies();
            AlertVisibility = Visibility.Hidden;
        }
        #endregion // Constructor

        #region Methods
        /// <summary>
        /// Method to open contact on double click
        /// </summary>
        /// <param name="obj"></param>
        public void LinkedContactDoubleClickCommandAction(object obj)
        {
           //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsContactEditAllowed)
                return;
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditContactAction...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                People ppl = obj as People;
                People peopleData = CrmStartUp.GetContactsByIdPerson(ppl.IdPerson);

                EditContactViewModel editContactViewModel = new EditContactViewModel();
                EditContactView editContactView = new EditContactView();
                editContactViewModel.InIt(peopleData);
                editContactViewModel.InItPermisssion(IsPermissionCustomerEdit);  //[pallavi.kale][GEOS2-9626][07.11.2025]
                EventHandler handle = delegate { editContactView.Close(); };
                editContactViewModel.RequestClose += handle;
                editContactView.DataContext = editContactViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                editContactView.ShowDialogWindow();
                if (editContactViewModel.IsSave && editContactViewModel.SelectedContact[0] != null)
                {

                    ppl.Name = editContactViewModel.SelectedContact[0].Name;
                    ppl.Surname = editContactViewModel.SelectedContact[0].Surname;
                    ppl.FullName = editContactViewModel.SelectedContact[0].FullName;
                    ppl.JobTitle = editContactViewModel.SelectedContact[0].JobTitle;
                    ppl.Phone = editContactViewModel.SelectedContact[0].Phone;
                    ppl.Email = editContactViewModel.SelectedContact[0].Email;
                    ppl.OwnerImage = GetContactImage(editContactViewModel.SelectedContact[0]);
                    FillSiteContacts();
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditContactAction...executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditContactAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for getting ContactImage
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ImageSource GetContactImage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetContactImage ...", category: Category.Info, priority: Priority.Low);

                People people = obj as People;
                if (!string.IsNullOrEmpty(people.ImageText))
                {
                    byte[] imageBytes = Convert.FromBase64String(people.ImageText);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    people.OwnerImage = byteArrayToImage(imageBytes);
                }
                else
                {
                    if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (people != null && people.IdPersonGender == 1)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else if (people != null && people.IdPersonGender == 2)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        //else if (people != null && people.IdPersonGender == null)
                        //    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (people != null && people.IdPersonGender == 1)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (people != null && people.IdPersonGender == 2)
                            people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        //else if (people != null && people.IdPersonGender == null)
                        //    people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }

                GeosApplication.Instance.Logger.Log("Method GetContactImage() executed successfully", category: Category.Info, priority: Priority.Low);
                return people.OwnerImage;

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return null;
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetContactImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                return null;
            }

        }
        /// <summary>
        /// Method to fill List of opportunity of selected plant from Plant Combo
        /// </summary>
        private void FillPlantOpportunityList()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList ...", category: Category.Info, priority: Priority.Low);

                ListPlantOpportunity = new ObservableCollection<Offer>();

                foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                {
                    ListPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
                }

                GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                AddActivityView addActivityView = new AddActivityView();

                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                addActivityViewModel.IsAddedFromOutSide = true;
                List<Activity> _ActivityList = new List<Activity>();

                //**[Start] code for add Account Detail.

                Activity _Activity = new Activity();
                _Activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                ActivityLinkedItem _ActivityLinkedItem = new ActivityLinkedItem();
                _ActivityLinkedItem.Company = new Company();
                _ActivityLinkedItem.IdLinkedItemType = 42;
                _ActivityLinkedItem.Company = SelectedCompanyList[0];
                _ActivityLinkedItem.IdSite = SelectedCompanyList[0].IdCompany;
                _ActivityLinkedItem.IdCustomer = SelectedCompanyList[0].IdCustomer;
                _ActivityLinkedItem.Name = SelectedCompanyList[0].Customers[0].CustomerName + " - " + SelectedCompanyList[0].Name;


                _ActivityLinkedItem.LinkedItemType = new LookupValue();
                _ActivityLinkedItem.LinkedItemType.IdLookupValue = 42;
                _ActivityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                _ActivityLinkedItem.IsVisible = false;
                //FillGroupList();
                addActivityViewModel.SelectedIndexCompanyGroup = SelectedIndexCompanyGroup;
                addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == SelectedCompanyList[0].IdCompany));

                //addActivityViewModel.SelectedIndexCompanyGroup = addActivityViewModel.CompanyGroupList.FindIndex(x => x.IdCompany == SelectedCompanyList[0].IdCompany);;
                // addActivityViewModel.SelectedIndexCompanyPlant = CompanyPlantList.FindIndex(x => x.IdCompany == SelectedCompanyList[0].IdCompany);
                //addActivityViewModel.LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount(Convert.ToString(_ActivityLinkedItem.IdSite)).ToList());
                //addActivityViewModel.ListPlantOpportunity = ListPlantOpportunity;


                CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();

                if (!string.IsNullOrEmpty(_ActivityLinkedItem.Company.Address))
                {
                    _ActivityLinkedItem.Company.Address = _ActivityLinkedItem.Company.Address;
                    if (_ActivityLinkedItem.Company.Latitude != null && _ActivityLinkedItem.Company.Longitude != null)
                    {
                        customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(_ActivityLinkedItem.Company.Latitude);
                        customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(_ActivityLinkedItem.Company.Longitude);

                        customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                        MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                        Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                        Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                    }
                    else
                    {
                        MapLatitudeAndLongitude = null;
                        Latitude = null;
                        Longitude = null;
                    }
                }

                _Activity.ActivityLinkedItem.Add(_ActivityLinkedItem);
                _ActivityList.Add(_Activity);
                addActivityViewModel.IsInternalEnable = false;



                addActivityViewModel.Init(_ActivityList);

                //**[End] code for add Account Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                if (addActivityViewModel.IsActivitySave)
                {
                    foreach (Activity newActivity in addActivityViewModel.NewCreatedActivityList)
                    {
                        if (newActivity.IsCompleted == 1)
                        {
                            newActivity.ActivityGridStatus = "Completed";
                            newActivity.CloseDate = GeosApplication.Instance.ServerDateTime;
                        }
                        else
                        {
                            newActivity.ActivityGridStatus = newActivity.ActivityStatus != null ? newActivity.ActivityStatus.Value : "";
                            newActivity.CloseDate = null;
                        }

                        CustomerActivityList.Add(newActivity);
                    }

                    CustomerActivityList = new List<Activity>(CustomerActivityList);
                    SelectedActivity = CustomerActivityList.Last();

                    TableView detailView = ((TableView)obj);
                    detailView.Focus();
                }

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add Contact
        /// </summary>
        /// <param name="obj"></param>
        private void AddContactViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow...", category: Category.Info, priority: Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            AddContactView addContactView = new AddContactView();
            AddContactViewModel addContactViewModel = new AddContactViewModel();

            People people = new People();
            people.Company = new Company();
            people.Company.Customers = new List<Customer>();
            int idcust = SelectedCompanyList[0].Customers[0].IdCustomer;

            people.Company.IdCompany = SelectedCompanyList[0].IdCompany;
            people.Company.Customers = SelectedCompanyList[0].Customers;
            addContactViewModel.Init(people);

            EventHandler handle = delegate { addContactView.Close(); };
            addContactViewModel.RequestClose += handle;
            addContactView.DataContext = addContactViewModel;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            addContactView.ShowDialog();

            if (addContactViewModel.IsSave)
            {
                if (addContactViewModel.ContactData.OwnerImage == null)
                {
                    //User user = WorkbenchStartUp.GetUserById(addContactViewModel.ContactData.IdPerson);
                    //if (user != null)
                    //{
                    //    try
                    //    {
                    //        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                    //        if (UserProfileImageByte != null)
                    //        {
                    //            addContactViewModel.ContactData.OwnerImage = byteArrayToImage(UserProfileImageByte);
                    //        }
                    //        else    // If User not found in the emdep_geos db then display default image by gender.
                    //        {
                    //            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                    //            {
                    //                if (addContactViewModel.ContactData.IdPersonGender == 1)
                    //                    addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                    //                else if (addContactViewModel.ContactData.IdPersonGender == 2)
                    //                    addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                    //            }
                    //            else
                    //            {
                    //                if (addContactViewModel.ContactData.IdPersonGender == 1)
                    //                    addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                    //                else if (addContactViewModel.ContactData.IdPersonGender == 2)
                    //                    addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                    //            }
                    //        }
                    //    }
                    //    catch (FaultException<ServiceException> ex)
                    //    {
                    //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Get an error in AddContactViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    }
                    //    catch (ServiceUnexceptedException ex)
                    //    {
                    //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Get an error in AddContactViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Get an error in AddContactViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(addContactViewModel.ContactData.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(addContactViewModel.ContactData.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        addContactViewModel.ContactData.OwnerImage = byteArrayToImage(imageBytes);
                    }
                    else    // If User is Null then Show temporary image by gender.
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (addContactViewModel.ContactData.IdPersonGender == 1)
                                addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (addContactViewModel.ContactData.IdPersonGender == 2)
                                addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (addContactViewModel.ContactData.IdPersonGender == 1)
                                addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (addContactViewModel.ContactData.IdPersonGender == 2)
                                addContactViewModel.ContactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }
                SiteContactsList.Add(addContactViewModel.ContactData);
            }
            GeosApplication.Instance.Logger.Log("Method AddContactViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// method for displaying activities
        /// </summary>
        private void FillActivity()
        {
            try
            {
                var Temp = CrmStartUp.GetActivitiesByIdSite(SelectedCompanyList[0].IdCompany);
                CustomerActivityList = Temp.OrderByDescending(x => x.CloseDate).ToList();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivity() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete linkeditem from linked items.
        /// </summary>
        /// <param name="obj"></param>
        private void AssignedSalesCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction ...", category: Category.Info, priority: Priority.Low);
             //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;

            try
            {
                if (obj is People)
                {

                    People people = obj as People;

                    if (SalesResponsible != null && SalesResponsible.IdPerson == people.IdPerson)
                    {
                        SalesResponsible = null;
                    }

                    people.IsSalesResponsible = false;
                    SalesOwnerList.Remove(people);
                    CloneSalesOwnerList.FirstOrDefault(j => j.IdPerson == people.IdPerson).TransactionOperation = ModelBase.TransactionOperations.Delete;
                    // if there is only one saleOwner then assign SalesResponsible also.
                    if (SalesOwnerList.Count != 0 && (!SalesOwnerList.Any(j => j.IsSalesResponsible == true)))
                    {
                        SalesOwnerList[0].IsSalesResponsible = true;
                        SalesResponsible = SalesOwnerList[0];
                        SalesOwnerList[0].IsSelected = true;
                    }

                    ListSalesUsers.Add(people);
                    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesDeleted").ToString(), people.FullName) });
                }

                GeosApplication.Instance.Logger.Log("Method AssignedSalesCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedSalesCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }

            return 0;
        }

        /// <summary>
        /// Method for similar plant name search.
        /// </summary>
        /// <param name="Name"></param>
        private void ShowPopupAsPerPlantName(string Name)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerPlantName ...", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.IsCityNameExist = true;

                // PlantNameLst = AllPlantsLst.ToList();
                int SelectedIdCustomer = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                PlantNameLst = AllPlantsLst.Where(cust => cust.IdCustomer == SelectedIdCustomer).ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList();
                if (PlantNameLst != null && !string.IsNullOrEmpty(Name))
                {
                    if (Name.Length > 1)
                    {
                        PlantNameLst = PlantNameLst.Where(h => h.Name.ToUpper().Contains(Name.ToUpper()) || h.Name.ToUpper().StartsWith(Name.Substring(0, 2).ToUpper())
                                                                || h.Name.ToUpper().EndsWith(Name.Substring(Name.ToUpper().Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, CustomerPlantName)).ToList();
                        PlantNameStrLst = PlantNameLst.Select(pn => pn.Name).ToList();
                    }
                    else
                    {
                        PlantNameLst = PlantNameLst.Where(h => h.Name.ToUpper().Contains(Name.ToUpper()) || h.Name.ToUpper().StartsWith(Name.Substring(0, 1).ToUpper())
                                                                || h.Name.ToUpper().EndsWith(Name.Substring(Name.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, CustomerPlantName)).ToList();
                        PlantNameStrLst = PlantNameLst.Select(pn => pn.Name).ToList();
                    }

                    if (!string.IsNullOrEmpty(CustomerCity))
                        GeosApplication.Instance.IsCityNameExist = Name.ToUpper().Contains(CustomerCity.ToUpper());

                    //  GeosApplication.Instance.IsGroupNameExist = CompanyGroupList.Any(cl => Name.ToUpper().Contains(cl.CustomerName.ToUpper()));
                    GeosApplication.Instance.IsGroupNameExist = CustomerPlantName.ToUpper().TrimStart().TrimEnd().Contains(CompanyGroupList[SelectedIndexCompanyGroup].CustomerName.TrimEnd().TrimStart().ToUpper());
                }

                else
                {
                    PlantNameLst = new List<Company>();
                    PlantNameStrLst = new List<string>();
                }

                if (PlantNameLst.Count > 0)
                {
                    AlertVisibility = Visibility.Visible;

                    if (!GeosApplication.Instance.IsCityNameExist)
                        AlertVisibility = Visibility.Hidden;
                    else if (GeosApplication.Instance.IsGroupNameExist)
                        AlertVisibility = Visibility.Hidden;
                }
                else
                {
                    AlertVisibility = Visibility.Hidden;
                }

                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerPlantName() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPopupAsPerPlantName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InIt(IList<Company> CompanyList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt ...", category: Category.Info, priority: Priority.Low);
                inIt = true;
                SelectedCompanyList = CompanyList;
                //[pramod.misal][GEOS2-6462][18-11-2024]
                Idincoterm = SelectedCompanyList[0].Idincoterm;
                IdPaymentType = SelectedCompanyList[0].IdPaymentType;
                Alias = SelectedCompanyList[0].ShortName;
                FillCountryRegionList();
                FillCityList();
                FillCompanyPlantList();
                FillGroupList();
                FillBusinessField();
                FillBusinessCenter();
                FillBusinessProduct();
                FillCountry();
                FillUsers();
                FillSalesOwnerList();
                FillSiteContacts();
                FillListChangeLog();
                FillActivity();
                FillSourceList();//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                FillShippingAddress(SelectedCompanyList); //[pramod.misal][GEOS2-6462][18-11-2024]
                FillIncotermsList(Idincoterm); //[pramod.misal][GEOS2-6462][18-11-2024]
                FillPaymentTermsList(IdPaymentType); //[pramod.misal][GEOS2-6462][18-11-2024]
                FillStatusList();   //chitra.girigosavi[GEOS2-7242][10/04/2025]
                if (SelectedCompanyList != null && SelectedCompanyList.Count > 0)
                {
                    CustomerPlantName = SelectedCompanyList[0].Name;
                    //  Size = SelectedCompanyList[0].Size;
                    RegisterationNumber = SelectedCompanyList[0].CIF;//[GEOS2-3994][rdixit][17.11.2022]
                    RegisteredName = SelectedCompanyList[0].RegisteredName;
                    CustomerNumberofEmplyee = SelectedCompanyList[0].NumberOfEmployees;

                    SelectedSource = SourceList.FirstOrDefault(x => x.IdLookupValue == SelectedCompanyList[0].IdSource);//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                    CustomerLines = SelectedCompanyList[0].Line;
                    CuttingMachines = SelectedCompanyList[0].CuttingMachines;
                    CustomerAddress = SelectedCompanyList[0].Address;
                    CustomerCity = SelectedCompanyList[0].City;
                    CustomerZip = SelectedCompanyList[0].ZipCode;
                    CustomerFax = SelectedCompanyList[0].Fax;
                    CustomerState = SelectedCompanyList[0].Region;

                    CustomerEmail = SelectedCompanyList[0].Email;
                    CustomerWebsite = SelectedCompanyList[0].Website;
                    CustomerTelephone = SelectedCompanyList[0].Telephone;

                    Latitude = SelectedCompanyList[0].Latitude;
                    Longitude = SelectedCompanyList[0].Longitude;

                    if (Latitude != null && Longitude != null)
                        MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value);

                    //**[Start][For Show Contact Age.]

                    if (SelectedCompanyList[0].CreatedIn != null)
                    {
                        CustomerCreatedIn = SelectedCompanyList[0].CreatedIn.Value;

                        DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, SelectedCompanyList[0].CreatedIn.Value.Date);

                        if (dateCalculateInYearAndMonth.Years > 0)
                        {
                            AccountsCreatedDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                            AccountsCreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("EditContactViewYears").ToString());
                        }
                        else
                        {
                            CustomerGenerateDays = (GeosApplication.Instance.ServerDateTime.Date - SelectedCompanyList[0].CreatedIn.Value.Date).Days;

                            if (CustomerGenerateDays > 99)
                                AccountsCreatedDays = "99+";
                            else
                                AccountsCreatedDays = CustomerGenerateDays.ToString();

                            AccountsCreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("EditContactViewDays").ToString());
                        }

                    }
                    if (SelectedCompanyList[0].ModifiedIn != null)
                        CustomerModifiedIn = SelectedCompanyList[0].ModifiedIn.Value;

                    if (SelectedCompanyList[0].ModifiedIn != null)
                    {
                        if (SelectedCompanyList[0].ModifiedIn == DateTime.MinValue)
                            CustomerModifiedIn = null;
                        else
                            CustomerModifiedIn = SelectedCompanyList[0].ModifiedIn;
                    }
                    else
                    {
                        CustomerModifiedIn = null;
                    }

                    //**[End][For Show Contact Age.]
                    //chitra.girigosavi[GEOS2-7242][1/04/2025]
                    if (StatusList.Any()) // Ensure the list is not empty
                    {
                        if (SelectedCompanyList[0].IdStatus > 0)
                        {
                            SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == SelectedCompanyList[0].IdStatus);
                        }
                        if (SelectedStatus != null)
                        {
                            Status = SelectedStatus.Value;
                        }
                        else
                        {
                            Status = string.Empty;
                        }
                    }

                }
                //CustomerImageSource = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Anupam.jpg");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor LeadsViewModel() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            inIt = false;
        }

        /// <summary>
        /// Method for fill emdep ListChangeLog. 
        /// </summary>
        private void FillListChangeLog()
        {
            GeosApplication.Instance.Logger.Log("Method FillListChangeLog ...", category: Category.Info, priority: Priority.Low);

            try
            {
                ListChangeLog = new ObservableCollection<LogEntryBySite>(CrmStartUp.GetAllLogEntriesByIdSite(Convert.ToInt64(SelectedCompanyList[0].IdCompany)));
                GeosApplication.Instance.Logger.Log("Method FillListChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  FillListChangeLog() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill emdep BusinessCenter. 
        /// </summary>
        private void FillBusinessCenter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupValues(6);
                BusinessCenter = new List<LookupValue>();
                BusinessCenter.Insert(0, new LookupValue() { Value = "---", InUse = true });
                BusinessCenter.AddRange(tempBusinessUnitList);

                SelectedIndexBusinessCenter = BusinessCenter.FindIndex(i => i.IdLookupValue == SelectedCompanyList[0].IdBusinessCenter);
                if (SelectedIndexBusinessCenter == -1)
                {
                    SelectedIndexBusinessCenter = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessCenter() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessCenter() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessCenter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill emdep BusinessField.
        /// </summary>
        private void FillBusinessField()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessField ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessUnitList = CrmStartUp.GetLookupValues(5);
                BusinessField = new List<LookupValue>();
                BusinessField.Insert(0, new LookupValue() { Value = "---", InUse = true });
                BusinessField.AddRange(tempBusinessUnitList);

                SelectedIndexBusinessField = BusinessField.FindIndex(i => i.IdLookupValue == SelectedCompanyList[0].IdBusinessField);
                if (SelectedIndexBusinessField == -1)
                {
                    SelectedIndexBusinessField = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillBusinessField() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessField() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessField() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessField() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill emdep BusinessCenter.
        /// </summary>
        private void FillBusinessProduct()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessProduct ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempBusinessProductList = CrmStartUp.GetLookupValues(7);
                BusinessProduct = new List<LookupValue>();
                BusinessProduct.AddRange(tempBusinessProductList);
                SelectedBusinessProductItems = new List<Object>();
                List<string> ConcatString = new List<string>();
                BusinessProductStringLog = "";
                if (SelectedCompanyList[0].BusinessProductList != null && SelectedCompanyList[0].BusinessProductList.Count > 0)
                {
                    // this code remove duplicate Business Product in list.
                    SelectedCompanyList[0].BusinessProductList = SelectedCompanyList[0].BusinessProductList.GroupBy(b => b.IdBusinessProduct).Select(s => s.First()).ToList();

                    foreach (var item in SelectedCompanyList[0].BusinessProductList)
                    {
                        LookupValue selectedValue = BusinessProduct.FirstOrDefault(bsp => bsp.IdLookupValue == item.IdBusinessProduct);

                        if (selectedValue != null)
                            SelectedBusinessProductItems.Add(selectedValue);

                        ConcatString.Add(((LookupValue)selectedValue).Value.ToString());
                    }
                    BusinessProductStringLog = string.Join(" , ", ConcatString.ToList());
                }

                GeosApplication.Instance.Logger.Log("Method FillBusinessProduct() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessProduct() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessProduct() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessProduct() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Fill Country.
        /// </summary>
        //chitra.girigosavi GEOS2-6498 When add/Edit customer taking to much time to load form
        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry ...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = CrmStartUp.GetCountries_V2570();
                CountryList = new List<Country>(tempCountryList);
                // Insert "---" at the 0 index after populating the list
                CountryList.Insert(0, new Country() { Name = "---" });

                SelectedIndexCountry = CountryList.FindIndex(i => i.IdCountry == SelectedCompanyList[0].IdCountry);
                if (SelectedIndexCountry == -1)
                {
                    SelectedIndexCountry = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillCountry() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
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
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                if (CompanyGroupList == null)
                    CompanyGroupList = new ObservableCollection<Customer>();

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    //TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                    else
                    {
                        //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                        CompanyGroupList.AddRange(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                    }
                }
                else
                {
                    // TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {
                        //CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                        CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                        //  SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedCompanyList[0].Customers[0].IdCustomer);
                    }

                }

                //CompanyGroupList = new List<Customer>();
                //CompanyGroupList.Insert(0, new Customer() { CustomerName = "---" });
                //CompanyGroupList.AddRange(TempCompanyGroupList);

                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedCompanyList[0].Customers[0].IdCustomer));
                if (SelectedIndexCompanyGroup == -1)
                {
                    SelectedIndexCompanyGroup = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Company list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                    else
                    {
                        // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];

                    else
                    {
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Sales Owner list.
        /// </summary>
        private void FillSalesOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList ...", category: Category.Info, priority: Priority.Low);
                #region Service Comments
                //[rdixit][GEOS2-4278][04.05.2023]
                //SalesOwnerList = new ObservableCollection<People>(CrmStartUp.GetSalesOwnerBySiteId(SelectedCompanyList[0].IdCompany));
                //Service GetSalesOwnerBySiteId_V2390 updated with GetSalesOwnerBySiteId_V2450 [rdixit][GEOS2-4903][26.10.2023]
                #endregion
                SalesOwnerList = new ObservableCollection<People>(CrmStartUp.GetSalesOwnerBySiteId_V2450(SelectedCompanyList[0].IdCompany));

                #region Old Code
                /*
                ObservableCollection<People> tempList = new ObservableCollection<People>();
                CloneSalesOwnerList = SalesOwnerList.Select(item => (People)item.Clone()).ToList();
                CloneSalesOwnerList.ForEach(t => t.TransactionOperation = ModelBase.TransactionOperations.Modify);

                if (SalesOwnerList.Count > 0)
                {
                    foreach (People item in SalesOwnerList)
                    {
                        People ppl = ListSalesUsers.FirstOrDefault(x => x.IdPerson == item.IdPerson);

                        if (ppl != null)
                        {
                            if (item.IsSalesResponsible)
                                ppl.IsSalesResponsible = true;

                            ppl.IsSelected = true;
                            tempList.Add(ppl);
                            ListSalesUsers.Remove(ppl);
                        }
                    }

                    SalesOwnerList = tempList;

                }
                for (int i = 0; i < SalesOwnerList.Count; i++)
                {
                    User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(SalesOwnerList[i].IdPerson));

                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);

                        if (UserProfileImageByte != null)
                            SalesOwnerList[i].OwnerImage = byteArrayToImage(UserProfileImageByte);
                        else
                        {
                            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (user.IdUserGender == 1)
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                else
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            }
                            else
                            {
                                if (user.IdUserGender == 1)
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                else
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    // if there is only one saleOwner then assign SalesResponsible also.
                    if (SalesOwnerList != null && SalesOwnerList.Count == 1)
                    {
                        SalesOwnerList[0].IsSalesResponsible = true;
                        SalesResponsible = SalesOwnerList[0];
                        SalesOwnerList[0].IsSelected = true;
                    }

                    SalesResponsible = SalesOwnerList.FirstOrDefault(x => x.IsSalesResponsible == true);

                }
                */
                #endregion

                ObservableCollection<People> tempList = new ObservableCollection<People>();
                CloneSalesOwnerList = SalesOwnerList.Select(item => (People)item.Clone()).ToList();
                CloneSalesOwnerList.ForEach(t => t.TransactionOperation = ModelBase.TransactionOperations.Modify);

                if (SalesOwnerList.Count > 0)
                {
                    foreach (People item in SalesOwnerList)
                    {
                        People ppl = ListSalesUsers.FirstOrDefault(x => x.IdPerson == item.IdPerson);
                        if (ppl != null)
                            ListSalesUsers.Remove(ppl);
                    }
                }
                #region Images code
                for (int i = 0; i < SalesOwnerList.Count; i++)
                {
                    User user = WorkbenchStartUp.GetUserById(Convert.ToInt32(SalesOwnerList[i].IdPerson));
                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                        if (UserProfileImageByte != null)
                            SalesOwnerList[i].OwnerImage = byteArrayToImage(UserProfileImageByte);
                        else
                        {
                            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (user.IdUserGender == 1)
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                else
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            }
                            else
                            {
                                if (user.IdUserGender == 1)
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                else
                                    SalesOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                #endregion
                // if there is only one saleOwner then assign SalesResponsible also.
                if (SalesOwnerList != null && SalesOwnerList.Count == 1 && SalesOwnerList?[0].IsEnabled == true)
                {
                    SalesOwnerList[0].IsSalesResponsible = true;
                    SalesResponsible = SalesOwnerList[0];
                    SalesOwnerList[0].IsSelected = true;
                }
                SalesResponsible = SalesOwnerList.FirstOrDefault(x => x.IsSalesResponsible == true);
                GeosApplication.Instance.Logger.Log("Method FillSalesOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSalesOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToImage(byte[] imageData)
        {
            GeosApplication.Instance.Logger.Log("Method byteArrayToImage...", category: Category.Info, priority: Priority.Low);

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

            GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed Successfully.", category: Category.Info, priority: Priority.Low);

            return image;
        }

        // This method is used to fill Contacts grid.
        private void FillSiteContacts()
        {
            GeosApplication.Instance.Logger.Log("Method FillSiteContacts...", category: Category.Info, priority: Priority.Low);
            SiteContactsList = new ObservableCollection<People>(CrmStartUp.GetSiteContacts(SelectedCompanyList[0].IdCompany));      // CompanyPlantList[SelectedIndexCompanyPlant].IdCompany));

            foreach (People person in SiteContactsList)
            {
                if (person.OwnerImage == null)
                {
                    //User user = WorkbenchStartUp.GetUserById(person.IdPerson);
                    //if (user != null)
                    //{
                    //    try
                    //    {
                    //        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                    //        if (UserProfileImageByte != null)
                    //        {
                    //            person.OwnerImage = byteArrayToImage(UserProfileImageByte);
                    //        }
                    //        else    // If User not found in the emdep_geos db then display default image by gender.
                    //        {
                    //            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                    //            {
                    //                if (person.IdPersonGender == 1)
                    //                    person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                    //                else if (person.IdPersonGender == 2)
                    //                    person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                    //            }
                    //            else
                    //            {
                    //                if (person.IdPersonGender == 1)
                    //                    person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                    //                else if (person.IdPersonGender == 2)
                    //                    person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        GeosApplication.Instance.Logger.Log("Get an error in Method FillSiteContacts.", category: Category.Info, priority: Priority.Low);
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(person.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(person.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        person.OwnerImage = byteArrayToImage(imageBytes);
                    }
                    else    // If User is Null then Show temporary image by gender.
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (person.IdPersonGender == 1)
                                person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (person.IdPersonGender == 2)
                                person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (person.IdPersonGender == 1)
                                person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (person.IdPersonGender == 2)
                                person.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("Method FillSiteContacts executed Successfully.", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Remove user profile picture 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveCustomerImage(object obj)
        {
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// Method For Save Cutomer Details
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCutomerDetails(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveCustomerDetails ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                InformationError = null;
                IsShowErrorInAddress = false;
                ShowAddValidationVisibility = Visibility.Hidden;
                IsShowErrorInAddress = false;
                if (CustomerAddress != null && CustomerAddress != "")
                {
                    if (RegisteredName != null && RegisteredName != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(RegisteredName.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }
                    //[GEOS2-3994][rdixit][17.11.2022]
                    if (RegisterationNumber != null && RegisterationNumber != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(RegisterationNumber))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }
                    if (CustomerCity != null && CustomerCity != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(CustomerCity.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }
                    if (CustomerZip != null && CustomerZip != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(CustomerZip))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }

                    if (CustomerState != null && CustomerState != "")
                    {
                        if (CustomerAddress.ToUpper().Contains(CustomerState.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }

                    if (SelectedIndexCountry > 0 && CountryList != null)
                    {
                        if (CustomerAddress.ToUpper().Contains(CountryList[SelectedIndexCountry].Name.ToUpper()))
                        {
                            IsShowErrorInAddress = true;
                        }
                    }

                }
                InformationError = null;  //[rdixit][07.04.2023][GEOS2-4295]
                AssignedSalesOwnerError = null; //[GEOS2-4559][rdixit][18.06.2023]
                string error = EnableValidationAndGetError();

                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                {
                    if (error == "Please check inputted data.")
                        InformationError = "";
                    else
                        InformationError = null;
                }
                if ((SalesOwnerList?.Count != 0 || (ListSalesUsers.Any(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))) && (!SalesOwnerList.Any(i => i.IsEnabled == false)))
                {
                    AssignedSalesOwnerError = null;
                }
                else//[GEOS2-4559][rdixit][16.06.2023]
                {
                    AssignedSalesOwnerError = "";
                }
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerPlantName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedBusinessProductItems"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessField"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexBusinessCenter"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerCity"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                PropertyChanged(this, new PropertyChangedEventArgs("AssignedSalesOwnerError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatusIndex"));

                if (error != null || IsShowErrorInAddress == true || AssignedSalesOwnerError != null)
                {
                    if (IsShowErrorInAddress == true)
                    {
                        ShowAddValidationVisibility = Visibility.Visible;
                    }
                    else
                    {
                        ShowAddValidationVisibility = Visibility.Hidden;
                    }
                    IsBusy = false;
                    return;
                }
                else
                {
                    ShowAddValidationVisibility = Visibility.Hidden;
                    CustomerChangedLogsDetails();                // Chnage log for Customer
                    CustomerData = new Company();
                    CustomerData.IdCompany = SelectedCompanyList[0].IdCompany;
                    CustomerData.IdSource = SelectedSource.IdLookupValue;//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                    //rajashri[GEOS2-4964]
                    if (SelectedSource.Value == "---")
                    {
                        CustomerData.SourceName = "";//[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                    }
                    else
                    {
                        CustomerData.SourceName = SelectedSource.Value;
                    }

                    CustomerData.IdCustomer = Convert.ToInt32(CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer);
                    if (!string.IsNullOrEmpty(CustomerPlantName))
                        CustomerData.Name = CustomerPlantName.Trim(); //  CompanyPlantList[SelectedIndexCompanyPlant].Name;
                    CustomerData.IdBusinessCenter = Convert.ToByte(BusinessCenter[SelectedIndexBusinessCenter].IdLookupValue);
                    CustomerData.IdBusinessField = Convert.ToByte(BusinessField[SelectedIndexBusinessField].IdLookupValue);
                    #region //chitra.girigosavi[GEOS2-7242][10/04/2025]
                    CustomerData.IdStatus = SelectedStatus.IdLookupValue;
                    if (CustomerData != null)
                    {
                        if (CustomerData.Status == null)
                        {
                            CustomerData.Status = new LookupValue();
                        }
                        CustomerData.Status.Value = SelectedStatus?.Value ?? string.Empty;
                        CustomerData.Status.HtmlColor = SelectedStatus?.HtmlColor ?? string.Empty;
                    }
                    #endregion
                    CustomerData.BusinessProductList = new List<SitesByBusinessProduct>();

                    List<LookupValue> FinalSelectedBusinessProductItems = SelectedBusinessProductItems.Select(s => (LookupValue)s).ToList();

                    //[** Start Save selected Business Product]
                    foreach (var item in BusinessProduct)
                    {

                        bool isExistInBusinessProductList = false;
                        bool isExistInFinalSelectedBusinessProductList = false;

                        isExistInFinalSelectedBusinessProductList = FinalSelectedBusinessProductItems.Exists(bs => bs.IdLookupValue == item.IdLookupValue);

                        if (SelectedCompanyList[0].BusinessProductList == null || SelectedCompanyList[0].BusinessProductList.Count == 0 && isExistInFinalSelectedBusinessProductList)
                        {
                            CustomerData.BusinessProductList.Add(new SitesByBusinessProduct() { IdBusinessProduct = ((LookupValue)item).IdLookupValue, IdSite = SelectedCompanyList[0].IdCompany, IsAdded = true, IsDeleted = false });
                        }

                        else
                        {
                            if (SelectedCompanyList[0].BusinessProductList != null && SelectedCompanyList[0].BusinessProductList.Count > 0)
                                isExistInBusinessProductList = SelectedCompanyList[0].BusinessProductList.Exists(bs => bs.IdBusinessProduct == item.IdLookupValue);

                            //if exist in not exist in main list and exist in selected list then Add.
                            if (!isExistInBusinessProductList && isExistInFinalSelectedBusinessProductList)
                            {
                                CustomerData.BusinessProductList.Add(new SitesByBusinessProduct() { IdBusinessProduct = ((LookupValue)item).IdLookupValue, IdSite = SelectedCompanyList[0].IdCompany, IsAdded = true, IsDeleted = false });
                            }

                            //if exist in main list and not exist in selected list then Delete.
                            if (isExistInBusinessProductList && !isExistInFinalSelectedBusinessProductList)
                            {
                                CustomerData.BusinessProductList.Add(new SitesByBusinessProduct() { IdBusinessProduct = ((LookupValue)item).IdLookupValue, IdSite = SelectedCompanyList[0].IdCompany, IsAdded = false, IsDeleted = true });
                            }

                        }

                    }

                    //[** End Save selected Business Product]

                    CustomerData.IdCountry = Convert.ToByte(CountryList[SelectedIndexCountry].IdCountry);
                    CustomerData.Country = CountryList[SelectedIndexCountry];
                    if (!string.IsNullOrEmpty(CustomerState))
                        CustomerData.Region = CustomerState.Trim();
                    if (!string.IsNullOrEmpty(CustomerWebsite))
                        CustomerData.Website = CustomerWebsite.Trim();
                    CustomerData.Line = CustomerLines;
                    CustomerData.CuttingMachines = CuttingMachines;
                    if (!string.IsNullOrEmpty(RegisteredName))
                        CustomerData.RegisteredName = RegisteredName.Trim();
                    if (!string.IsNullOrEmpty(CustomerAddress))
                        CustomerData.Address = CustomerAddress.Trim();
                    if (!string.IsNullOrEmpty(CustomerCity))
                        CustomerData.City = CustomerCity.Trim();
                    if (!string.IsNullOrEmpty(CustomerState))
                        CustomerData.Region = CustomerState.Trim();
                    if (!string.IsNullOrEmpty(CustomerZip))
                        CustomerData.ZipCode = CustomerZip.Trim();
                    if (!string.IsNullOrEmpty(CustomerTelephone))
                        CustomerData.Telephone = CustomerTelephone.Trim();
                    if (!string.IsNullOrEmpty(CustomerEmail))
                        CustomerData.Email = CustomerEmail.Trim();
                    if (!string.IsNullOrEmpty(CustomerFax))
                        CustomerData.Fax = CustomerFax.Trim();
                    //[GEOS2-3994][rdixit][17.11.2022]
                    if (!string.IsNullOrEmpty(RegisterationNumber))
                        CustomerData.CIF = RegisterationNumber;
                    CustomerData.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    CustomerData.Latitude = Latitude;
                    CustomerData.Longitude = Longitude;
                    #region
                    //if (SelectedCompanyList[0].People != null)
                    //{
                    //    CustomerData.People = new People();
                    //    CustomerData.People = SelectedCompanyList[0].People;
                    //}

                    //if (SelectedCompanyList[0].PeopleSalesResponsibleAssemblyBU != null)
                    //{
                    //    CustomerData.PeopleSalesResponsibleAssemblyBU = new People();
                    //    CustomerData.PeopleSalesResponsibleAssemblyBU = SelectedCompanyList[0].PeopleSalesResponsibleAssemblyBU;
                    //}

                    //CustomerData.IdSalesResponsible = SelectedCompanyList[0].IdSalesResponsible;
                    //CustomerData.IdSalesResponsibleAssemblyBU = SelectedCompanyList[0].IdSalesResponsibleAssemblyBU;
                    /* [GEOS2-4278][rdixit][04.05.2023] Commented Code
                    if (SalesOwnerList != null && SalesOwnerList.Count > 0)
                    {
                        if (SalesOwnerList.Count == 1)
                        {
                            if (SalesOwnerList[0].IsSelected && SalesOwnerList[0].IsSalesResponsible)
                            {
                                //SalesOwnerList[0].IsSalesResponsible = true;
                                CustomerData.IdSalesResponsible = SalesOwnerList[0].IdPerson;
                                CustomerData.IdSalesResponsibleAssemblyBU = null;
                            }
                            else
                            {
                                CustomerData.IdSalesResponsible = null;
                                CustomerData.IdSalesResponsibleAssemblyBU = SalesOwnerList[0].IdPerson;
                            }
                            //ListChangeLog.Add(new LogEntryBySite() { IdSite = CustomerData.IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesAdded").ToString(), SalesOwnerList[0].FullName) });
                        }
                        else if (SalesOwnerList.Count == 2)
                        {
                            if (SalesResponsible != null)
                            {
                                foreach (People item in SalesOwnerList)
                                {
                                    if (item.IsSalesResponsible)
                                    {
                                        CustomerData.IdSalesResponsible = item.IdPerson;
                                    }
                                    else
                                    {
                                        CustomerData.IdSalesResponsibleAssemblyBU = item.IdPerson;
                                    }
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewWarningSalesResponsible").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                IsBusy = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        CustomerData.IdSalesResponsible = null;
                        CustomerData.People = null;
                        CustomerData.IdSalesResponsibleAssemblyBU = null;
                        CustomerData.SalesResponsibleAssemblyBU = null;
                    }*/
                    #endregion
                    if (ListChangeLog != null)
                    {
                        CustomerData.LogEntryBySites = new List<LogEntryBySite>();
                        CustomerData.LogEntryBySites.AddRange(TempListChangeLog.OrderByDescending(x => x.DateTime));
                    }

                    CustomerData.ShortName = SelectedCompanyList[0].ShortName;
                    CustomerData.Idincoterm = IncotermsList[SelectedIndexIncoterms].IdIncoterm;
                    CustomerData.IdPaymentType = PaymentTermsList[SelectedIndexPaymentTerms].IdPaymentType;
                    CustomerData.PaymentTerm = PaymentTermsList[SelectedIndexPaymentTerms].PaymentTypeName;
                    CustomerData.Alias = Alias;
                    //[GEOS2-6462][rdixit][23.11.2024]
                    //if(SelectedLinkedAddress!=null)
                    //{
                    //    if(CustomerData.CompanyShippingAddress==null)
                    //    {
                    //        CustomerData.CompanyShippingAddress = new List<Data.Common.Crm.ShippingAddress>();
                    //    }                       
                    //    CustomerData.CompanyShippingAddress.Add(new Data.Common.Crm.ShippingAddress() { IdShippingAddress= SelectedLinkedAddress.IdShippingAddress, Name=SelectedLinkedAddress.Name, Address = SelectedLinkedAddress.Address,
                    //                                                                                   ZipCode= SelectedLinkedAddress.ZipCode,City= SelectedLinkedAddress.City,Region= SelectedLinkedAddress.Region,IdCountry= SelectedLinkedAddress.IdCountry,
                    //                                                                                    Comments= SelectedLinkedAddress.Remark,IdSite= SelectedLinkedAddress.IdSite,IsDefault= SelectedLinkedAddress.IsDefault,IsDisabled= SelectedLinkedAddress.IsDisabled });
                    //}

                    List<string> bpstring = new List<string>();
                    foreach (var item in SelectedBusinessProductItems)
                    {
                        if (CustomerData.BusinessProductList != null
                            && CustomerData.BusinessProductList.Count > 0
                            && !CustomerData.BusinessProductList.Any(bp => bp.IdBusinessProduct == ((LookupValue)item).IdLookupValue))
                        {
                            CustomerData.BusinessProductList.Add(new SitesByBusinessProduct() { IdBusinessProduct = ((LookupValue)item).IdLookupValue, IdSite = 0, IsAdded = true });
                        }
                        bpstring.Add(((LookupValue)item).Value.ToString());
                    }

                    #region [GEOS2-4278][rdixit][04.05.2023]
                    CustomerData.SalesOwnerList = new List<People>();
                    if (SalesOwnerList.Count != 0)
                    {
                        foreach (People item in SalesOwnerList)
                        {
                            if (CloneSalesOwnerList != null)
                            {
                                People ppl = CloneSalesOwnerList.FirstOrDefault(i => i.IdPerson == item.IdPerson);
                                if (ppl != null)
                                {
                                    if (item.IsSalesResponsible != ppl.IsSalesResponsible)
                                    {
                                        CloneSalesOwnerList.FirstOrDefault(i => i.IdPerson == item.IdPerson).IsSalesResponsible = item.IsSalesResponsible;
                                        CloneSalesOwnerList.FirstOrDefault(i => i.IdPerson == item.IdPerson).TransactionOperation = ModelBase.TransactionOperations.Update;
                                    }
                                }
                            }
                        }
                    }
                    else if (ListSalesUsers.Any(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))//[GEOS2-4278][rdixit][04.05.2023]
                    {
                        People saleowner = new People();
                        saleowner.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        saleowner.Name = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Name;
                        saleowner.Surname = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Surname;
                        saleowner.FullName = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FullName;
                        saleowner.Email = GeosApplication.Instance.PeopleList.FirstOrDefault(pl => pl.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).Email;
                        saleowner.IsSalesResponsible = true;
                        CloneSalesOwnerList.Add(saleowner);
                    }
                    if (CloneSalesOwnerList != null)
                        CloneSalesOwnerList.ForEach(it => it.OwnerImage = null);
                    CustomerData.SalesOwnerList = new List<People>(CloneSalesOwnerList);
                    #endregion
                    CustomerData.Country.CountryIconImage = null;
                    Company updatedcompanydetails = null;
                    try
                    {
                        // Update code here
                        #region Service Comments
                        //updatedcompanydetails = CrmStartUp.UpdateCompany_V2043(CustomerData);
                        //[GEOS2-3994][rdixit][17.11.2022]
                        //Service UpdateCompany_V2340 updated with UpdateCompany_V2390 by [GEOS2-4278][rdixit][04.05.2023]
                        // updatedcompanydetails = CrmStartUp.UpdateCompany_V2390(CustomerData);
                        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                        //updatedcompanydetails = CrmStartUp.UpdateCompany_V2430(CustomerData);
                        //[pooja.jadhav][22-11-2024][GEOS2-6462]
                        #endregion
                        //[GEOS2-6462][rdixit][23.11.2024]
                        if (ShippingAddressList != null)
                        {
                            ShippingAddressList.ForEach(i => i.IdSite = SelectedCompanyList[0].IdCompany);
                            CustomerData.CompanyShippingAddress = ShippingAddressList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add || i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList();
                        }
                        CustomerData.Latitude = MapLatitudeAndLongitude?.Latitude;
                        CustomerData.Longitude = MapLatitudeAndLongitude?.Longitude;
                        //CrmStartUp = new CrmServiceController("localhost:6699");
                        //updatedcompanydetails = CrmStartUp.UpdateCompany_V2580(CustomerData);
                        updatedcompanydetails = CrmStartUp.UpdateCompany_V2630(CustomerData);//chitra.girigosavi GEOS2-7242 31/03/2025
                        if (updatedcompanydetails.IsUpdate)
                        {
                            IsCustomerDetailsModified = true;
                            CustomerData.Customers = new List<Customer>();
                            CustomerData.Customers.Add(CompanyGroupList[SelectedIndexCompanyGroup]);
                            CustomerData.Country = CountryList[SelectedIndexCountry];
                            CustomerData.BusinessField = BusinessField[SelectedIndexBusinessField];
                            CustomerData.BusinessCenter = BusinessCenter[SelectedIndexBusinessCenter];
                            CustomerData.BusinessProductString = string.Join(" ,", bpstring.ToList());
                            CustomerData.BusinessProductString = CustomerData.BusinessProductString.Replace(",", "\n");
                            CustomerData.CreatedIn = CustomerCreatedIn;
                            CustomerData.ModifiedIn = CustomerModifiedIn;
                            CustomerData.SalesOwnerList = SalesOwnerList.ToList();
                            #region GEOS2-4278 Commented by rdixit
                            //if (SalesOwnerList != null && SalesOwnerList.Count > 0)
                            //{
                            //    foreach (People person in SalesOwnerList)
                            //    {
                            //        if (person.IsSalesResponsible)
                            //        {
                            //            CustomerData.IdSalesResponsible = person.IdPerson;
                            //            CustomerData.People = person;
                            //        }
                            //        else
                            //        {
                            //            CustomerData.IdSalesResponsibleAssemblyBU = person.IdPerson;
                            //            CustomerData.PeopleSalesResponsibleAssemblyBU = person;
                            //        }
                            //    }
                            //}
                            #endregion
                            try
                            {
                                IList<Customer> TempCompanyGroupList = null;
                                if (GeosApplication.Instance.IdUserPermission == 21)
                                {
                                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                                    {
                                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                                        //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
                                        GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                                        // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                        //[pramod.misal][GEOS2-4682][08-08-2023]
                                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
                                        GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                                    }
                                }
                                else
                                {

                                    //CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                    //[pramod.misal][GEOS2-4682][08-08-2023]
                                    //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                                    CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                                    // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                    //[pramod.misal][GEOS2-4682][08-08-2023]
                                    EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            RequestClose(null, null);
                        }
                        else if (updatedcompanydetails.IsExist)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewCompanyExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewNotCreated").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        IsBusy = false;

                        GeosApplication.Instance.Logger.Log("Method SaveCustomerDetails() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        IsBusy = false;
                        CustomerData = null;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SaveCustomerDetails() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        IsBusy = false;
                        CustomerData = null;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in SaveCustomerDetails() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }

                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                CustomerData = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SaveCustomerDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// method for add change logs.
        /// </summary>
        private void CustomerChangedLogsDetails()
        {
            GeosApplication.Instance.Logger.Log("Method CustomerChangedLogsDetails ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (TempListChangeLog == null)
                    TempListChangeLog = new List<LogEntryBySite>();
                if (!SelectedCompanyList[0].IdSource.Equals(SelectedSource.IdLookupValue))
                {
                    if (SelectedCompanyList[0].IdSource == 0)
                    {

                        TempListChangeLog.Add(
                        new LogEntryBySite()
                        {
                            IdSite = SelectedCompanyList[0].IdCompany,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            DateTime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewSourceAddChange").ToString(),
                            "None", SelectedSource.Value),
                            IdLogEntryBySite = 2
                        });


                    }
                    else
                    {
                        LookupValue test = SourceList.FirstOrDefault(i => i.IdLookupValue == SelectedCompanyList[0].IdSource);
                        if (SelectedSource.Value == "---")
                        {
                            TempListChangeLog.Add(
                            new LogEntryBySite()
                            {
                                IdSite = SelectedCompanyList[0].IdCompany,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                DateTime = GeosApplication.Instance.ServerDateTime,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewSourceChnage").ToString(),
                                test.Value, "None"),
                                IdLogEntryBySite = 2
                            });
                        }
                        else
                        {
                            TempListChangeLog.Add(
                                new LogEntryBySite()
                                {
                                    IdSite = SelectedCompanyList[0].IdCompany,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewSourceChnage").ToString(),
                                    test.Value, SelectedSource.Value),
                                    IdLogEntryBySite = 2
                                });
                        }
                    }
                }
                // #Lines
                if (!SelectedCompanyList[0].Line.Equals(CustomerLines))
                {
                    if (SelectedCompanyList[0].Line == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewLineAdd").ToString(), CustomerLines), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Line.Equals(CustomerLines))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewLineChnage").ToString(), SelectedCompanyList[0].Line, CustomerLines), IdLogEntryBySite = 2 });
                    }
                }

                // Size
                if (!SelectedCompanyList[0].Size.Equals(Size))
                {
                    if (SelectedCompanyList[0].Size == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewLineAdd").ToString(), size), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Size.Equals(Size))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewSizeChnage").ToString(), SelectedCompanyList[0].Size, size), IdLogEntryBySite = 2 });
                    }
                }

                // NumberOfEmployees
                if (!SelectedCompanyList[0].NumberOfEmployees.Equals(CustomerNumberofEmplyee))
                {
                    if (SelectedCompanyList[0].NumberOfEmployees == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewNumberOfEmployeeAdd").ToString(), CustomerNumberofEmplyee), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].NumberOfEmployees.Equals(CustomerNumberofEmplyee))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewNumberOfEmployeeChnage").ToString(), SelectedCompanyList[0].NumberOfEmployees, CustomerNumberofEmplyee), IdLogEntryBySite = 2 });
                    }
                }

                // plant
                if (!SelectedCompanyList[0].Name.Equals(CustomerPlantName))
                {
                    if (SelectedCompanyList[0].Name == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewPlantAdd").ToString(), CustomerPlantName), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Name.Equals(CustomerPlantName))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewPlantChnage").ToString(), SelectedCompanyList[0].Name, CustomerPlantName), IdLogEntryBySite = 2 });
                    }
                }

                // CuttingMachines
                if (!SelectedCompanyList[0].CuttingMachines.Equals(CuttingMachines))
                {
                    if (SelectedCompanyList[0].CuttingMachines == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCuttingMachinesAdd").ToString(), CuttingMachines), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].CuttingMachines.Equals(CuttingMachines))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCuttingMachinesChnage").ToString(), SelectedCompanyList[0].CuttingMachines, CuttingMachines), IdLogEntryBySite = 2 });
                    }
                }

                Latitude = SelectedCompanyList[0].Latitude;
                Longitude = SelectedCompanyList[0].Longitude;
                double? newLatitude = MapLatitudeAndLongitude?.Latitude == 0 ? null : MapLatitudeAndLongitude?.Latitude;
                double? newLongitude = MapLatitudeAndLongitude?.Longitude == 0 ? null : MapLatitudeAndLongitude?.Longitude;
                // Latitude
                if (!Latitude.Equals(newLatitude))
                {
                    TempListChangeLog.Add(new LogEntryBySite()
                    {
                        IdSite = SelectedCompanyList[0].IdCompany,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerLatitudeChangeLog").ToString(),
                        Latitude == null ? "None" : Latitude.ToString(), newLatitude == null ? "None" : newLatitude.ToString()),
                        IdLogEntryBySite = 2
                    });
                }
                // Longitude
                if (!Longitude.Equals(newLongitude))
                {
                    TempListChangeLog.Add(new LogEntryBySite()
                    {
                        IdSite = SelectedCompanyList[0].IdCompany,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        DateTime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerLongitudeChangeLog").ToString(),
                        Longitude == null ? "None" : Longitude.ToString(), newLongitude == null ? "None" : newLongitude.ToString()),
                        IdLogEntryBySite = 2
                    });
                }
                // Phone
                if (SelectedCompanyList[0].Telephone != CustomerTelephone)
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Telephone))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewPhoneAdd").ToString(), CustomerTelephone), IdLogEntryBySite = 2 });
                    }
                    else if (SelectedCompanyList[0].Telephone != CustomerTelephone)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewPhoneChnage").ToString(), SelectedCompanyList[0].Telephone, CustomerTelephone), IdLogEntryBySite = 2 });
                    }
                }

                // Fax
                if (!SelectedCompanyList[0].Fax.Equals(CustomerFax))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Fax))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewFaxAdd").ToString(), CustomerFax), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Fax.Equals(CustomerFax))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewFaxChnage").ToString(), SelectedCompanyList[0].Fax, CustomerFax), IdLogEntryBySite = 2 });
                    }
                }

                // ZipCode
                if (!SelectedCompanyList[0].ZipCode.Equals(CustomerZip))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].ZipCode))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewZipCodeAdd").ToString(), CustomerZip), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].ZipCode.Equals(CustomerZip))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewZipCodeChnage").ToString(), SelectedCompanyList[0].ZipCode, CustomerZip), IdLogEntryBySite = 2 });
                    }
                }

                // City
                if (!SelectedCompanyList[0].City.Equals(CustomerCity))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].City))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCityAdd").ToString(), CustomerCity), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].City.Equals(CustomerCity))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCityChnage").ToString(), SelectedCompanyList[0].City, CustomerCity), IdLogEntryBySite = 2 });
                    }
                }

                //RegisteredName
                if (!SelectedCompanyList[0].RegisteredName.Equals(RegisteredName))
                {
                    if (!SelectedCompanyList[0].RegisteredName.Equals(RegisteredName))
                    {
                        string tempRegisteredName = SelectedCompanyList[0].RegisteredName;
                        string tempNewRegisteredName = RegisteredName;
                        if (string.IsNullOrEmpty(tempRegisteredName))
                        {
                            tempRegisteredName = "None";
                        }
                        if (string.IsNullOrEmpty(RegisteredName))
                        {
                            tempNewRegisteredName = "None";
                        }
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewRegisteredNameChange").ToString(), tempRegisteredName, tempNewRegisteredName), IdLogEntryBySite = 2 });
                    }
                }
                //Registration Number [GEOS2-3994][rdixit][17.11.2022]
                if (!SelectedCompanyList[0].CIF.Equals(RegisterationNumber))
                {
                    string tempRegisterationNumber = SelectedCompanyList[0].CIF;
                    string tempNewRegisterationNumber = RegisterationNumber;
                    if (string.IsNullOrEmpty(tempRegisterationNumber))
                    {
                        tempRegisterationNumber = "None";
                    }
                    if (string.IsNullOrEmpty(RegisterationNumber))
                    {
                        tempNewRegisterationNumber = "None";
                    }
                    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewRegistrationNumberChange").ToString(), tempRegisterationNumber, tempNewRegisterationNumber), IdLogEntryBySite = 2 });
                }
                // Address
                if (!SelectedCompanyList[0].Address.Equals(CustomerAddress))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Address))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewAddressAdd").ToString(), CustomerAddress), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Address.Equals(CustomerAddress))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewAddressChnage").ToString(), SelectedCompanyList[0].Address, CustomerAddress), IdLogEntryBySite = 2 });
                    }
                }

                // State
                if (!SelectedCompanyList[0].Region.Equals(CustomerState))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Region))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewStateAdd").ToString(), CustomerState), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Region.Equals(CustomerState))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewStateChnage").ToString(), SelectedCompanyList[0].Region, CustomerState), IdLogEntryBySite = 2 });
                    }
                }

                // Email
                if (!SelectedCompanyList[0].Email.Equals(CustomerEmail))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Email))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewEmailAdd").ToString(), CustomerEmail), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Email.Equals(CustomerEmail))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewEmailChnage").ToString(), SelectedCompanyList[0].Email, CustomerEmail), IdLogEntryBySite = 2 });
                    }
                }

                // Website
                if (!SelectedCompanyList[0].Website.Equals(CustomerWebsite))
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Website))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewWebsiteAdd").ToString(), CustomerWebsite), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Website.Equals(CustomerWebsite))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewWebsiteChnage").ToString(), SelectedCompanyList[0].Website, CustomerWebsite), IdLogEntryBySite = 2 });
                    }
                }

                // Country
                if (SelectedCompanyList[0].IdCountry != CountryList[SelectedIndexCountry].IdCountry)
                {
                    if (string.IsNullOrEmpty(SelectedCompanyList[0].Country.Name))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCountryAdd").ToString(), CountryList[SelectedIndexCountry].Name), IdLogEntryBySite = 2 });
                    }
                    else if (SelectedCompanyList[0].IdCountry != CountryList[SelectedIndexCountry].IdCountry)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewCountryChnage").ToString(), SelectedCompanyList[0].Country.Name, CountryList[SelectedIndexCountry].Name), IdLogEntryBySite = 2 });
                    }
                }

                // Group
                if (!SelectedCompanyList[0].Customers[0].IdCustomer.Equals(CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer))
                {
                    if (SelectedCompanyList[0].Customers[0] == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewGroupAdd").ToString(), CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryBySite = 2 });
                    }
                    else if (!SelectedCompanyList[0].Customers[0].IdCustomer.Equals(CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewGroupChange").ToString(), SelectedCompanyList[0].Customers[0].CustomerName, CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryBySite = 2 });
                    }
                }

                // Business Product
                if (SelectedBusinessProductItems != null)
                {
                    List<string> bpstring1 = new List<string>();
                    foreach (var item in SelectedBusinessProductItems)
                    {
                        bpstring1.Add(((LookupValue)item).Value.ToString());
                    }
                    string BPString = string.Join(" , ", bpstring1.ToList());
                    if (string.IsNullOrEmpty(BusinessProductStringLog))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessProductAdd").ToString(), BPString), IdLogEntryBySite = 2 });
                    }
                    else if (BusinessProductStringLog != null && !BusinessProductStringLog.Equals(BPString))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessProductChnage").ToString(), BusinessProductStringLog, BPString), IdLogEntryBySite = 2 });
                    }
                }

                // Business Field
                if (SelectedCompanyList[0].IdBusinessField != BusinessField[SelectedIndexBusinessField].IdLookupValue)
                {
                    if (SelectedCompanyList[0].BusinessField == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessFieldAdd").ToString(), BusinessField[SelectedIndexBusinessField].Value), IdLogEntryBySite = 2 });
                    }
                    else if (SelectedCompanyList[0].IdBusinessField != BusinessField[SelectedIndexBusinessField].IdLookupValue)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessFieldChnage").ToString(), SelectedCompanyList[0].BusinessField.Value, BusinessField[SelectedIndexBusinessField].Value), IdLogEntryBySite = 2 });
                    }
                }

                // Business Type
                if (SelectedCompanyList[0].IdBusinessCenter != BusinessCenter[SelectedIndexBusinessCenter].IdLookupValue)
                {
                    if (SelectedCompanyList[0].BusinessCenter == null)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessTypeAdd").ToString(), BusinessCenter[SelectedIndexBusinessCenter].Value), IdLogEntryBySite = 2 });
                    }
                    else if (SelectedCompanyList[0].IdBusinessCenter != BusinessCenter[SelectedIndexBusinessCenter].IdLookupValue)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewBusinessTypeChnage").ToString(), SelectedCompanyList[0].BusinessCenter.Value, BusinessCenter[SelectedIndexBusinessCenter].Value), IdLogEntryBySite = 2 });
                    }
                    GeosApplication.Instance.Logger.Log("Method CustomerChangedLogsDetails() executed successfully", category: Category.Info, priority: Priority.Low);
                }

                // [nsatpute][22-11-2024][GEOS2-6462]
                // Incoterms                
                if (!SelectedIncotermId.Equals(SelectedCompanyList[0].Idincoterm))
                {
                    if (SelectedCompanyList[0].Idincoterm == 0)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_IncotermAdded").ToString(), IncotermsList.FirstOrDefault(x => x.IdIncoterm == SelectedIncotermId).IncotermName) });
                    }
                    else
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_IncotermChanged").ToString(), IncotermsList.FirstOrDefault(x => x.IdIncoterm == SelectedCompanyList[0].Idincoterm).IncotermName, IncotermsList.FirstOrDefault(x => x.IdIncoterm == SelectedIncotermId).IncotermName), IdLogEntryBySite = 2 });
                    }
                }
                // Payment Status
                var selectedPayTerms = PaymentTermsList.FirstOrDefault(x => x.PaymentTypeName == SelectedPaymentTermsName);
                var selectedCompanyPayType = PaymentTermsList.FirstOrDefault(x => x.IdPaymentType == SelectedCompanyList[0].IdPaymentType);

                if (selectedPayTerms != null && selectedCompanyPayType != null && !selectedPayTerms.PaymentTypeName.Equals(selectedCompanyPayType.PaymentTypeName))
                {
                    if (SelectedCompanyList[0].IdPaymentType == 0)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_PaymentTermAdded").ToString(), PaymentTermsList.FirstOrDefault(x => x.PaymentTypeName == SelectedPaymentTermsName).PaymentTypeName) });
                    }
                    else
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_PaymentTermChanged").ToString(), PaymentTermsList.FirstOrDefault(x => x.IdPaymentType == SelectedCompanyList[0].IdPaymentType).PaymentTypeName, PaymentTermsList.FirstOrDefault(x => x.PaymentTypeName == SelectedPaymentTermsName).PaymentTypeName), IdLogEntryBySite = 2 });
                    }
                }

                // Alias                
                if (!Alias.Equals(SelectedCompanyList[0].ShortName))
                {
                    if (SelectedCompanyList[0].ShortName.Trim() == string.Empty)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_AliasAdded").ToString(), Alias) });
                    }
                    else
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("Editcustomerview_AliasChanged").ToString(), SelectedCompanyList[0].ShortName.Trim(), Alias), IdLogEntryBySite = 2 });
                    }
                }

                #region ShippingAddressList Change log [GEOS2-6462][rdixit][23.11.2024]
                if (ShippingAddressList != null)
                {
                    foreach (Data.Common.Crm.ShippingAddress item in ShippingAddressList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                    {
                        TempListChangeLog.Add(new LogEntryBySite()
                        {
                            IdSite = SelectedCompanyList[0].IdCompany,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            DateTime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerViewShippingAddressAdd").ToString(),
                            item.Name),
                            IdLogEntryBySite = 2
                        });
                    }

                    foreach (var item in ShippingAddressList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update))
                    {
                        var old = OriginalShippingAddressList.FirstOrDefault(i => i.IdShippingAddress == item.IdShippingAddress);
                        if (old == null) continue;

                        // Check each property and log changes
                        if (old.Name != item.Name)
                            AddLogEntryForChange("EditCustomerViewShippAddNameEdit", item.Name, old.Name, item.Name);

                        if (old.Address != item.Address)
                            AddLogEntryForChange("EditCustomerViewShippAddAddressEdit", item.Name, old.Address, item.Address);

                        if (old.ZipCode != item.ZipCode)
                            AddLogEntryForChange("EditCustomerViewShippAddZipEdit", item.Name, old.ZipCode, item.ZipCode);

                        if (old.City != item.City)
                            AddLogEntryForChange("EditCustomerViewShippAddCityEdit", item.Name, old.City, item.City);

                        if (old.Region != item.Region)
                            AddLogEntryForChange("EditCustomerViewShippAddRegionEdit", item.Name, old.Region, item.Region);

                        if (old.CountryName != item.CountryName)
                            AddLogEntryForChange("EditCustomerViewShippAddCountryNameEdit", item.Name, old.CountryName, item.CountryName);

                        if (old.Remark != item.Remark)
                            AddLogEntryForChange("EditCustomerViewShippAddRemarkEdit", item.Name, old.Remark, item.Remark);

                        if (old.IsDefault != item.IsDefault)
                        {
                            var oldDefault = old.IsDefault ? "Yes" : "No";
                            var newDefault = item.IsDefault ? "Yes" : "No";
                            AddLogEntryForChange("EditCustomerViewShippAddPrimaryAddressEdit", item.Name, oldDefault, newDefault);
                        }
                    }
                }
                #endregion

                // status chitra.girigosavi[GEOS2-7242][10/04/2025]
                if (!string.IsNullOrEmpty(SelectedStatus.Value))
                {
                    if (string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(SelectedStatus.Value))
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerStatusChangeLog").ToString(), "None", SelectedStatus.Value) });
                    }
                    else if (!string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(SelectedStatus.Value) && Status != SelectedStatus.Value)
                    {
                        TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("EditCustomerStatusChangeLog").ToString(), Status, SelectedStatus.Value) });
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomerChangedLogsDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[GEOS2-6462][rdixit][23.11.2024]
        private void AddLogEntryForChange(string resourceKey, string address, string oldValue, string newValue)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLogEntryForChange ...", category: Category.Info, priority: Priority.Low);
                TempListChangeLog.Add(new LogEntryBySite()
                {
                    IdSite = SelectedCompanyList[0].IdCompany,
                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    DateTime = GeosApplication.Instance.ServerDateTime,
                    Comments = string.Format(System.Windows.Application.Current.FindResource(resourceKey).ToString(),
                        address,
                        string.IsNullOrEmpty(oldValue) ? "None" : oldValue,
                        string.IsNullOrEmpty(newValue) ? "None" : newValue),
                    IdLogEntryBySite = 2
                });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLogEntryForChange() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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
            IsBusy = false;
        }

        /// <summary>
        /// Method for open website on browser . 
        /// </summary>
        /// <param name="obj"></param>
        public void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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
            IsBusy = false;
        }

        public void MapControlLoadedAction(object obj)
        {
            MapControlCopy = (MapControl)((System.Windows.RoutedEventArgs)(obj)).Source;
        }

        public void OnSearchCompletedAction(object obj)
        {
            BingSearchCompletedEventArgs e = (BingSearchCompletedEventArgs)obj;


            if (e.Cancelled) return;


            if (e.RequestResult.ResultCode != RequestResultCode.Success)
            {
                return;
            }
            foreach (LocationInformation resultInfo in e.RequestResult.SearchResults)
            {
                Latitude = resultInfo.Location.Latitude;
                Longitude = resultInfo.Location.Longitude;
            }
        }

        public void OnLayerItemsGeneratingAction(object obj)
        {
            LayerItemsGeneratingEventArgs args = (LayerItemsGeneratingEventArgs)obj;
            MapControlCopy.ZoomToFit(args.Items, 0.4);
            MapControlCopy.ZoomLevel = 17;
        }

        public void SearchButtonClickCommandAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method SearchButtonClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
            CustomerSetLocationMapView customerSetLocationMapView = new CustomerSetLocationMapView();
            EventHandler handle = delegate { customerSetLocationMapView.Close(); };
            customerSetLocationMapViewModel.RequestClose += handle;

            if (MapLatitudeAndLongitude != null)
            {
                customerSetLocationMapViewModel.MapLatitudeAndLongitude = MapLatitudeAndLongitude;
                customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = MapLatitudeAndLongitude;
            }


            customerSetLocationMapView.DataContext = customerSetLocationMapViewModel;
            var ownerInfo = (obj as FrameworkElement);
            customerSetLocationMapView.Owner = Window.GetWindow(ownerInfo);
            customerSetLocationMapView.ShowDialog();

            if (customerSetLocationMapViewModel.MapLatitudeAndLongitude != null &&
                !string.IsNullOrEmpty(customerSetLocationMapViewModel.LocationAddress))
            {

                if (!string.IsNullOrEmpty(CustomerAddress))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {

                        Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                        Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                        MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                        string[] fullAddress = customerSetLocationMapViewModel.LocationAddress.Split(',');

                        // For customer address without country name.
                        var withoutCountryAddess = fullAddress.Take(fullAddress.Length - 1);
                        string addressWithoutCountry = string.Join(",", withoutCountryAddess.Where(x => x.Trim().Length != 0));
                        CustomerAddress = addressWithoutCountry;

                        SelectedIndexCountry = CountryList.FindIndex(i => i.Name == fullAddress[fullAddress.Length - 1].ToUpper().Trim());

                        if (SelectedIndexCountry < 0)
                        {
                            SelectedIndexCountry = 0;
                            CustomerAddress = customerSetLocationMapViewModel.LocationAddress;
                        }

                        CustomerState = string.Empty;
                        CustomerZip = string.Empty;
                        CustomerCity = string.Empty;
                    }

                }
                else
                {
                    Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                    Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                    MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                    string[] fullAddress = customerSetLocationMapViewModel.LocationAddress.Split(',');

                    // For customer address without country name.
                    var withoutCountryAddess = fullAddress.Take(fullAddress.Length - 1);
                    string addressWithoutCountry = string.Join(",", withoutCountryAddess.Where(x => x.Trim().Length != 0));
                    CustomerAddress = addressWithoutCountry;

                    SelectedIndexCountry = CountryList.FindIndex(i => i.Name == fullAddress[fullAddress.Length - 1].ToUpper().Trim());

                    if (SelectedIndexCountry < 0)
                    {
                        SelectedIndexCountry = 0;
                        CustomerAddress = customerSetLocationMapViewModel.LocationAddress;
                    }

                    CustomerState = string.Empty;
                    CustomerZip = string.Empty;
                    CustomerCity = string.Empty;
                }
            }

            GeosApplication.Instance.Logger.Log("Method SearchButtonClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            //MapControlCopy = null;
            IsCustomerDetailsModified = false;
            RequestClose(null, null);
        }


        /// <summary>
        /// Method for Fill all contact list depend on Sales Owner. 
        /// </summary>
        private void FillUsers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUsers ...", category: Category.Info, priority: Priority.Low);
                //Updated Service Method GetAllActivePeoples with GetAllActivePeoples_V2390 TO get only IsStillActive and IsEnabled Users
                ListSalesUsers = new ObservableCollection<People>(CrmStartUp.GetAllActivePeoples_V2390());
                if (ListSalesUsers != null)
                    ListSalesUsers.ToList().ForEach(i => i.IsEnabled = true);
                GeosApplication.Instance.Logger.Log("Method FillUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        /// <summary>
        /// This Method for Add or Remove Sales User in Customer through check uncheck
        /// </summary>
        /// <param name="e"></param>
        public void SalesUserContactCheckedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesUserContactCheckedAction ...", category: Category.Info, priority: Priority.Low);
           //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;

            try
            {
                if (obj is People)
                {
                    People people = obj as People;  // (People)obj;

                    //if (SalesOwnerList.Count < 2)
                    //{

                    User user = WorkbenchStartUp.GetUserById(people.IdPerson);
                    //[rdixit][GEOS2-4278][04.05.2023]
                    CloneSalesOwnerList.Add(people);
                    CloneSalesOwnerList.FirstOrDefault(j => j.IdPerson == people.IdPerson).TransactionOperation = ModelBase.TransactionOperations.Add;
                    try
                    {
                        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }

                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }

                    if (UserProfileImageByte != null)
                    {
                        people.OwnerImage = byteArrayToImage(UserProfileImageByte);
                    }
                    else
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (user.IdUserGender == 1)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (user.IdUserGender == 2)
                                people.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                    SalesOwnerList.Add(people);

                    // if there is only one saleOwner then assign SalesResponsible also.
                    if (SalesOwnerList.Count == 1)
                    {
                        SalesOwnerList[0].IsSalesResponsible = true;
                        SalesResponsible = SalesOwnerList[0];
                        SalesOwnerList[0].IsSelected = true;
                    }

                    ListSalesUsers.Remove(people);
                    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesAdded").ToString(), people.FullName) });
                    #region Commented [rdixit][GEOS2-4278][04.05.2023]
                    //}
                    //else
                    //{
                    //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewWarningExceedAddSalesResponsible").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    //if (cellValueEventArgs.Column.FieldType == typeof(bool))
                    //    //{
                    //    //    cellValueEventArgs.Source.CloseEditor();
                    //    //    people.IsSelected = false;
                    //    //}
                    //}

                    //}
                    //else
                    //{
                    //    SalesOwnerList.Remove(people);
                    //    SalesResponsible = SalesOwnerList.FirstOrDefault(x => x.IsSalesResponsible == true);
                    //    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesDeleted").ToString(), people.FullName) });
                    //}
                    #endregion
                    SalesOwnerList = new ObservableCollection<People>(SalesOwnerList);

                    GeosApplication.Instance.Logger.Log("Method SalesUserContactCheckedAction() executed successfully", category: Category.Info, priority: Priority.Low);

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }
        /// <summary>
        /// This Method is for set a sales responsible
        /// </summary>
        /// <param name="obj"></param>
        public void SetSalesResponsibleCommandAction(object obj)
        {
          //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;

            GeosApplication.Instance.Logger.Log("Method SetSalesResponsibleCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                List<People> tempSalesOwnerList = new List<People>(SalesOwnerList);

                People data = (People)(obj);
                if (SalesOwnerList != null && SalesOwnerList.Any(x => x.IdPerson == data.IdPerson))
                {


                    foreach (People item in tempSalesOwnerList)
                    {
                        if (item.IdPerson == data.IdPerson)
                        {
                            if (SalesResponsible == null)
                            {
                                SalesResponsible = new People();
                                item.IsSelected = true;
                                item.IsSalesResponsible = true;
                                SalesResponsible = item;
                                TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleAdded").ToString(), item.FullName) });
                            }
                            else
                            {
                                if (SalesResponsible.IdPerson == data.IdPerson)
                                {
                                    return;
                                }
                                else
                                {
                                    //SalesResponsible = null;
                                    string OldSalesResponsible = "";
                                    OldSalesResponsible = SalesResponsible.FullName;
                                    SalesResponsible = new People();
                                    item.IsSelected = true;
                                    item.IsSalesResponsible = true;
                                    SalesResponsible = item;
                                    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleChanged").ToString(), OldSalesResponsible, SalesResponsible.FullName) });

                                }

                            }
                        }
                        else
                        {
                            if (item != null)
                            {
                                if (item.IsSalesResponsible)
                                    item.IsSalesResponsible = false;
                            }

                            if (item.IsSelected)
                            {
                                if (item.IsSalesResponsible == true)
                                {
                                    item.IsSalesResponsible = false;
                                    TempListChangeLog.Add(new LogEntryBySite() { IdSite = SelectedCompanyList[0].IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("CustomerEditViewUserChangeLogEntryAssignedSalesResponsibleRemoved").ToString(), item.FullName) });
                                }
                            }
                        }
                    }
                }

                SalesOwnerList = new ObservableCollection<People>(tempSalesOwnerList);

                GeosApplication.Instance.Logger.Log("Method SetSalesResponsibleCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSalesResponsibleCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Activity View
        /// </summary>
        /// <param name="obj"></param>
        private void EditActivityViewWindowShow(object obj)
        {
          //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;

            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                
                if (obj == null) return;

                Activity activity = ((Activity)obj);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                Activity tempActivity = new Activity();

                tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(activity.IdActivity);
                EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                EditActivityView editActivityView = new EditActivityView();
                editActivityViewModel.IsEditedFromOutSide = true;

                foreach (var item in tempActivity.ActivityLinkedItem)
                {
                    if (item.IdLinkedItemType == 42)
                    {
                        item.IsVisible = false;
                    }
                }

                editActivityViewModel.IsInternalEnable = false;
                editActivityViewModel.Init(tempActivity);

                EventHandler handle = delegate { editActivityView.Close(); };
                editActivityViewModel.RequestClose += handle;
                editActivityView.DataContext = editActivityViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                editActivityView.ShowDialogWindow();

                if (editActivityViewModel.objActivity != null)
                {
                    activity.Subject = editActivityViewModel.objActivity.Subject;
                    activity.Description = editActivityViewModel.objActivity.Description;
                    activity.LookupValue = editActivityViewModel.objActivity.LookupValue;
                    activity.ToDate = editActivityViewModel.objActivity.ToDate;
                    activity.FromDate = editActivityViewModel.objActivity.FromDate;
                    activity.IsCompleted = editActivityViewModel.objActivity.IsCompleted;
                    //activity.ActivityLinkedItem[0] = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer;
                    activity.People = editActivityViewModel.objActivity.People;
                    activity.ActivityLinkedItem = editActivityViewModel.objActivity.ActivityLinkedItem;

                    if (activity.IsCompleted == 1)
                    {
                        activity.ActivityGridStatus = "Completed";
                        activity.CloseDate = GeosApplication.Instance.ServerDateTime;
                    }
                    else
                    {
                        activity.ActivityGridStatus = editActivityViewModel.objActivity.ActivityStatus != null ? editActivityViewModel.objActivity.ActivityStatus.Value : "";
                        activity.CloseDate = null;
                    }
                    TableView detailView = ((TableView)obj);
                    detailView.Focus();
                }

                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to fill city list
        /// </summary>
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
                GeosApplication.Instance.Logger.Log("Method FillCountryRegionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-4663][26/08/2023]
        private void FillSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter ...", category: Category.Info, priority: Priority.Low);

                // IList<LookupValue> tempSourceListList = CrmStartUp.GetLookupValues(126);
                SourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(126));
                SourceList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                // SourceList.AddRange(tempSourceListList);



                GeosApplication.Instance.Logger.Log("Method FillBusinessCenter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5170]
        private void AddSourceButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSourceButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
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

                AddSourceView addSourceView = new AddSourceView();
                AddSourceViewModel addSourceViewModel = new AddSourceViewModel();

                EventHandler handle = delegate { addSourceView.Close(); };
                addSourceViewModel.RequestClose += handle;
                addSourceView.DataContext = addSourceViewModel;
                addSourceViewModel.IsCustomerView = true;
                addSourceViewModel.Init();

                addSourceView.ShowDialogWindow();

                if (addSourceViewModel.IsSave)
                {
                    FillSourceList();
                    // LookupValue lookupValue = new LookupValue();
                    // lookupValue.IdLookupValue = addSourceViewModel.NewSourceList.IdLookupValue;
                    //  lookupValue.Value = addSourceViewModel.NewSourceList.Value;
                    //  lookupValue.HtmlColor = addSourceViewModel.NewSourceList.HtmlColor;
                    //  lookupValue.IdLookupKey = addSourceViewModel.NewSourceList.IdLookupKey;
                    //  lookupValue.Position = addSourceViewModel.NewSourceList.Position;
                    //  lookupValue.InUse = addSourceViewModel.NewSourceList.InUse;

                    //   SourceList.Add(lookupValue);
                    SelectedSource = SourceList.FirstOrDefault(x => x.Value == addSourceViewModel.NewSourceList.Value);



                }

                GeosApplication.Instance.Logger.Log("Method AddSourceButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSourceButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-6462][14-11-2024]
        // [nsatpute][22-11-2024][GEOS2-6462]
        private void AddShippingAddressCommandAction(object obj)
        {
           //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddShippingAddressCommandAction()...", category: Category.Info, priority: Priority.Low);

                AddEditShippingAddressView addEditShippingAddressView = new AddEditShippingAddressView();
                AddEditShippingAddressViewModel addEditShippingAddressViewModel = new AddEditShippingAddressViewModel();
                EventHandler handle = delegate { addEditShippingAddressView.Close(); };
                addEditShippingAddressViewModel.RequestClose += handle;
                addEditShippingAddressViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddShippingAddress").ToString();
                addEditShippingAddressViewModel.IsNew = true;
                addEditShippingAddressView.DataContext = addEditShippingAddressViewModel;
                addEditShippingAddressView.ShowDialog();
                if (addEditShippingAddressViewModel.IsSave && addEditShippingAddressViewModel.NewAddress != null)
                {
                    if (ShippingAddressList == null)
                        ShippingAddressList = new ObservableCollection<Data.Common.Crm.ShippingAddress>();
                    //[GEOS2-6462][rdixit][23.11.2024]
                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                    {
                        var oldprimary = ShippingAddressList.FirstOrDefault(x => x.IsDefault == true);
                        if (oldprimary != null)
                        {
                            oldprimary.IsDefault = false;
                            if (oldprimary.TransactionOperation != ModelBase.TransactionOperations.Add)
                                oldprimary.TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                    }

                    ShippingAddressList.Add(addEditShippingAddressViewModel.NewAddress);

                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                        ReorderShippingAddresses();

                    ShippingAddressList = new ObservableCollection<Data.Common.Crm.ShippingAddress>(ShippingAddressList);
                }
                GeosApplication.Instance.Logger.Log("Method AddShippingAddressCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddShippingAddressCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        // [nsatpute][22-11-2024][GEOS2-6462]
        private void FillShippingAddress(IList<Company> CompanyList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillShippingAddress ...", category: Category.Info, priority: Priority.Low);
                ShippingAddressList = new ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress>(CrmStartUp.GetShippingAddressByIdPlant(CompanyList));
                if (ShippingAddressList.Count != 0)
                {
                    ShippingAddressList.ForEach(i => { i.Address = i.Address.Replace("\n", " ").Replace("\r", " "); i.Zipcityregion = i.ZipCode + " - " + i.City + " - " + i.Region; i.TransactionOperation = ModelBase.TransactionOperations.Modify; });
                    var Address = ShippingAddressList.FirstOrDefault();
                    if (Address != null)
                    {
                        Contentcountryname = Address.CountryName;
                        Contentcountryimgurl = Address.Country.CountryIconUrl;
                    }
                    ReorderShippingAddresses();
                    OriginalShippingAddressList = ShippingAddressList.Select(i => (Data.Common.Crm.ShippingAddress)i.Clone()).ToList();//[GEOS2-6462][rdixit][23.11.2024]
                }

                GeosApplication.Instance.Logger.Log("Method FillShippingAddress() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddress() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddress() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillShippingAddress() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void LinkedAddressDoubleClickCommandAction(object obj)
        {
           //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkedAddressDoubleClickCommandAction...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                if (obj != null)
                    SelectedLinkedAddress = obj as Emdep.Geos.Data.Common.Crm.ShippingAddress;
                AddEditShippingAddressView addEditShippingAddressView = new AddEditShippingAddressView();
                AddEditShippingAddressViewModel addEditShippingAddressViewModel = new AddEditShippingAddressViewModel();
                EventHandler handle = delegate { addEditShippingAddressView.Close(); };
                addEditShippingAddressViewModel.RequestClose += handle;
                addEditShippingAddressViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditShippingAddress").ToString();
                addEditShippingAddressViewModel.IsNew = false;
                addEditShippingAddressViewModel.EditInit(SelectedLinkedAddress);
                addEditShippingAddressView.DataContext = addEditShippingAddressViewModel;
                addEditShippingAddressView.ShowDialog();

                if (addEditShippingAddressViewModel.IsSave == true)
                {
                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                    {
                        var oldprimary = ShippingAddressList.FirstOrDefault(x => x.IsDefault == true);
                        if (oldprimary != null)
                        {
                            oldprimary.IsDefault = false;
                            if (oldprimary.TransactionOperation != ModelBase.TransactionOperations.Add)
                                oldprimary.TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                    }
                    SelectedLinkedAddress.IsDefault = addEditShippingAddressViewModel.IsPrimaryAddress;
                    SelectedLinkedAddress.IsDisabled = addEditShippingAddressViewModel.IsInUse;
                    SelectedLinkedAddress.Name = addEditShippingAddressViewModel.Name;
                    SelectedLinkedAddress.Address = addEditShippingAddressViewModel.Address;
                    SelectedLinkedAddress.ZipCode = addEditShippingAddressViewModel.Zipcode;
                    SelectedLinkedAddress.City = addEditShippingAddressViewModel.City;
                    SelectedLinkedAddress.Region = addEditShippingAddressViewModel.Region;
                    SelectedLinkedAddress.Zipcityregion = addEditShippingAddressViewModel.Zipcode + " - " + addEditShippingAddressViewModel.City + " - " + addEditShippingAddressViewModel.Region; ;
                    SelectedLinkedAddress.Remark = addEditShippingAddressViewModel.Remark;
                    SelectedLinkedAddress.IsInUse = addEditShippingAddressViewModel.IsInUse;
                    SelectedLinkedAddress.IdCountry = addEditShippingAddressViewModel.SelectedCountry.IdCountry;
                    SelectedLinkedAddress.CountryName = addEditShippingAddressViewModel.SelectedCountry.Name;
                    SelectedLinkedAddress.Country = addEditShippingAddressViewModel.SelectedCountry;
                    if (SelectedLinkedAddress.TransactionOperation != ModelBase.TransactionOperations.Add)
                        SelectedLinkedAddress.TransactionOperation = ModelBase.TransactionOperations.Update;

                    IsEnabledCancelButton = true;
                    if (addEditShippingAddressViewModel.IsPrimaryAddress)
                    {
                        ReorderShippingAddresses();
                    }
                    ShippingAddressList = new ObservableCollection<Data.Common.Crm.ShippingAddress>(ShippingAddressList);
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method LinkedAddressDoubleClickCommandAction...executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LinkedAddressDoubleClickCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][22-11-2024][GEOS2-6462]
        public void SetDefaultShippingAddressCommandAction(object obj)
        {
            //[pallavi.kale][GEOS2-8953][18.09.2025]
            if (!IsPermissionToManageCustomer)
                return;
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetDefaultShippingAddressCommandAction...", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    SelectedLinkedAddress = obj as Emdep.Geos.Data.Common.Crm.ShippingAddress;
                    if (ShippingAddressList != null)
                    {
                        var oldPrimary = ShippingAddressList.FirstOrDefault(x => x.IsDefault == true);
                        if (oldPrimary != null)
                        {
                            oldPrimary.IsDefault = false;
                            if (oldPrimary.TransactionOperation != ModelBase.TransactionOperations.Add)
                                oldPrimary.TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                        SelectedLinkedAddress.IsDefault = true;
                        if (SelectedLinkedAddress.TransactionOperation != ModelBase.TransactionOperations.Add)
                            SelectedLinkedAddress.TransactionOperation = ModelBase.TransactionOperations.Update;
                        ReorderShippingAddresses();
                    }
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SetDefaultShippingAddressCommandAction...executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetDefaultShippingAddressCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillIncotermsList(int Idincoterm)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);


                if (IncotermsList == null)
                    IncotermsList = new ObservableCollection<CustomerDetail>();
                //CrmStartUp = new CrmServiceController("localhost:6699");

                IncotermsList.AddRange(CrmStartUp.GetIncotermsList_V2580());

                //IncotermsList.Insert(0, new Customer() { CustomerName = "---"});
                ////IncotermsList.AddRange(tempSourceListList);
                //SelectedIndexIncoterms = IncotermsList.FirstOrDefault();
                IncotermsList.Insert(0, new CustomerDetail() { IncotermName = "---" });

                SelectedIndexIncoterms = IncotermsList.IndexOf(IncotermsList.FirstOrDefault(x => x.IdIncoterm == Idincoterm));  // Get the index of the first item



                //SelectedIndexIncoterms = 0;
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillIncotermsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillIncotermsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillIncotermsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillPaymentTermsList(int IdPaymentType)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPaymentTermsList ...", category: Category.Info, priority: Priority.Low);


                if (PaymentTermsList == null)
                    PaymentTermsList = new ObservableCollection<CustomerDetail>();
                //CrmStartUp = new CrmServiceController("localhost:6699");

                PaymentTermsList.AddRange(CrmStartUp.GetPaymentTermsList_V2580());
                PaymentTermsList.Insert(0, new CustomerDetail() { PaymentTypeName = "---" });

                //SelectedIndexPaymentTerms = PaymentTermsList.IndexOf(PaymentTermsList.FirstOrDefault());
                SelectedIndexPaymentTerms = PaymentTermsList.IndexOf(PaymentTermsList.FirstOrDefault(x => x.IdPaymentType == IdPaymentType));
                GeosApplication.Instance.Logger.Log("Method FillPaymentTermsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPaymentTermsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPaymentTermsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillIncotermsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [nsatpute][22-11-2024][GEOS2-6462]
        public void ReorderShippingAddresses()
        {
            var defaultAddress = ShippingAddressList.FirstOrDefault(addr => addr.IsDefault);
            if (defaultAddress != null)
            {
                ShippingAddressList.Remove(defaultAddress);
                ShippingAddressList.Insert(0, defaultAddress);
            }
        }

        //chitra.girigosavi GEOS2-7242 31/03/2025
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(177);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);
                // Default value set to -1
                SelectedStatusIndex = StatusList.FindIndex(i => i.IdLookupValue == SelectedCompanyList[0].IdStatus);
                if (SelectedStatusIndex == -1)
                {
                    SelectedStatusIndex = -1; // Keep it as -1 instead of setting it to 0
                }

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Methods
    }
}
